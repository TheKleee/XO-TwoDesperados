using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NodeDetector : MonoBehaviour, IWinCondition, IStrike
{
    [Header("Node:"), SerializeField] GameObject nodePrefab;
    [Header("Effects:"), SerializeField] GameObject[] placementEffectPrefab;
    [SerializeField] GameObject basePlacementEffectPrefab;
    public Dictionary<byte, bool> playerList { get; set; } = new();
    public int strike { get; set; }
    CameraController cc;
    bool gameOver;

    static readonly Vector2Int[] CheckDirections =
    {
        new(1,  0),
        new(0,  1),
        new(1,  1),
        new(1, -1),
    };

    Vector2Int endpoint;
    Map map;

    private void Start()
    {
        map = FindFirstObjectByType<Map>();
        cc = FindFirstObjectByType<CameraController>();
        strike = MapBuilder.instance.strike;
    }


    private void Update()
    {
        if (!IsPressed() || gameOver) return;
        Ray ray = Camera.main.ScreenPointToRay(GetScreenPosition());
        if (!Physics.Raycast(ray, out RaycastHit hit)) return;

        var hp = new Vector3((int)hit.point.x, 0, (int)hit.point.z);
        Vector2Int gridPos = map.WorldToGrid(hp);

        byte currentPlayer = GameManager.instance.CurrentPlayer;

        if (!map.IsOnMap(gridPos) || map.virtualMap[gridPos] != null) return;

        map.UpdateMap(gridPos, currentPlayer);
        HUD.instance.RegisterMove(currentPlayer);
        GameManager.instance.RegisterPlacement(gridPos);

        var gridPosMap = new Vector2(gridPos.x + 0.5f, gridPos.y + 0.5f); //Visual offset on the map :)
        var node = Instantiate(nodePrefab, map.GridToWorld(gridPosMap), Quaternion.identity, map.transform);
        node.GetComponent<Node>().Init(currentPlayer);
        AudioManager.instance.PlayPlacement();

        //To show alternating particle effects based on which player placed the node :D
        Instantiate(basePlacementEffectPrefab, node.transform.position, Quaternion.identity);
        Instantiate(currentPlayer % 2 == 0 ?
            placementEffectPrefab[0] :
            placementEffectPrefab[1],
            node.transform.position,
            Quaternion.identity);

        cc.FocusOn(GetAdjacentOpenField(gridPos)); //Following the closest open field to make sure we can always see one :D

        if (WinCondition(currentPlayer))
        {
            gameOver = true;
            playerList[currentPlayer] = true;
        }
        else if (IsBoardFull())
        {
            gameOver = true;
            GameManager.instance.OnDraw();
        }
        else
            GameManager.instance.NextTurn();
    }

    bool IsPressed()
    {
        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
            return true;
        return false;
    }

    Vector2 GetScreenPosition()
    {
        if (Pointer.current != null)
            return Pointer.current.position.ReadValue();
        return Vector2.zero;
    }

    public bool WinCondition(byte playerId)
    {
        Vector2Int placed = GameManager.instance.LastPlaced;
        foreach (var dir in CheckDirections)
        {
            var (strikes, firstNode) = WalkDirection(placed, dir, playerId);
            if (strikes == strike)
            {
                GameManager.instance.OnWin(playerId, firstNode, endpoint);
                return true;
            }
        }
        return false;
    }

    (int, Vector2Int) WalkDirection(Vector2Int current, Vector2Int dir, byte playerId, int curLength = 1, bool lastSearch = false, Vector2Int? firstPlaced = null)
    {
        if (firstPlaced == null)
            firstPlaced = current;

        if (curLength == strike)
        {
            endpoint = current;
            return (curLength, (Vector2Int)firstPlaced);
        }
        Vector2Int next = current + dir;
        if (!map.IsOnMap(next) || map.virtualMap[next] != playerId)
        {
            //Here we need to go backwards... from the starting point
            if (!lastSearch)
            {
                lastSearch = true;
                return WalkDirection(current, -dir, playerId, 1, lastSearch);
            }

            endpoint = current;
            return (curLength, (Vector2Int)firstPlaced);
        }
        return WalkDirection(next, dir, playerId, ++curLength, lastSearch, firstPlaced);
    }

    bool IsBoardFull()
    {
        foreach (var cell in map.virtualMap.Values)
            if (cell == null) return false;
        return true;
    }

    Vector3 GetAdjacentOpenField(Vector2Int placed)
    {
        int maxRadius = Mathf.Max(map.mapSize.x, map.mapSize.y);

        for (int radius = 1; radius < maxRadius; radius++)
            for (int x = -radius; x <= radius; x++)
                for (int y = -radius; y <= radius; y++)
                {
                    // Only check the outer ring of current radius
                    if (Mathf.Abs(x) != radius && Mathf.Abs(y) != radius) continue;

                    Vector2Int candidate = placed + new Vector2Int(x, y);
                    if (map.IsOnMap(candidate) && map.virtualMap[candidate] == null)
                        return map.GridToWorld(new Vector2(candidate.x + 0.5f, candidate.y + 0.5f));
                }

        // Board is full, stay on placed node
        return map.GridToWorld(new Vector2(placed.x + 0.5f, placed.y + 0.5f));
    }
}
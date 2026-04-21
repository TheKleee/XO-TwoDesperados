using System.Collections.Generic;
using UnityEngine;

public class NodeDetector : MonoBehaviour, IWinCondition, IStrike
{
    [Header("Node:"), SerializeField] GameObject nodePrefab;
    public Dictionary<byte, bool> playerList { get; set; } = new();
    public int strike { get; set; }

    static readonly Vector2Int[] CheckDirections =
    {
        new(1,  0),  // Horizontal
        new(0,  1),  // Vertical
        new(1,  1),  // Diagonal
        new(1, -1),  // Anti-diagonal
    };

    Vector2Int endpoint;
    Map map;

    private void Start()
    {
        map = FindFirstObjectByType<Map>();
        strike = MapBuilder.instance.strike;
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit)) return;

        Vector2Int gridPos = map.WorldToGrid(hit.point);
        byte currentPlayer = GameManager.instance.CurrentPlayer;

        if (!map.IsOnMap(gridPos) || map.virtualMap[gridPos] != null) return;

        map.UpdateMap(gridPos, currentPlayer);
        GameManager.instance.RegisterPlacement(gridPos);

        var node = Instantiate(nodePrefab, map.GridToWorld(gridPos), Quaternion.identity);
        node.GetComponent<Node>().Init(currentPlayer);
        AudioManager.instance.PlayPlacement();

        if (WinCondition(currentPlayer))
            playerList[currentPlayer] = true;
        else if (IsBoardFull())
            GameManager.instance.OnDraw();
        else
            GameManager.instance.NextTurn();
    }

    public bool WinCondition(byte playerId)
    {
        Vector2Int placed = GameManager.instance.LastPlaced;

        foreach (var dir in CheckDirections)
        {
            if (WalkDirection(placed, dir, playerId) == strike)
            {
                GameManager.instance.OnWin(playerId, placed, endpoint);
                return true;
            }

            if (WalkDirection(placed, -dir, playerId) == strike)
            {
                GameManager.instance.OnWin(playerId, endpoint, placed);
                return true;
            }
        }
        return false;
    }

    int WalkDirection(Vector2Int current, Vector2Int dir, byte playerId, int curLength = 0)
    {
        if (curLength == strike)
        {
            endpoint = current;
            return curLength;
        }

        Vector2Int next = current + dir;
        if (!map.IsOnMap(next) || map.virtualMap[next] != playerId)
        {
            endpoint = current;
            return curLength;
        }

        return WalkDirection(next, dir, playerId, ++curLength);
    }

    bool IsBoardFull()
    {
        foreach (var cell in map.virtualMap.Values)
            if (cell == null) return false;
        return true;
    }
}
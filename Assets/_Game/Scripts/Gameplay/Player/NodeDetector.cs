using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NodeDetector : MonoBehaviour, IWinCondition, IStrike
{
    [Header("Node:"), SerializeField] GameObject nodePrefab;
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

        cc.FocusOn(node.transform.position); //Following the node for large maps :D

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
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) return true;
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame) return true;
        return false;
    }

    Vector2 GetScreenPosition()
    {
        if (Mouse.current != null) return Mouse.current.position.ReadValue();
        if (Touchscreen.current != null) return Touchscreen.current.primaryTouch.position.ReadValue();
        return Vector2.zero;
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

    int WalkDirection(Vector2Int current, Vector2Int dir, byte playerId, int curLength = 1)
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
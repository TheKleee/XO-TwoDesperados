using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NodeDetector : MonoBehaviour, IWinCondition, IStrike
{
    [Header("Node:"), SerializeField] GameObject nodePrefab;
    public Dictionary<byte, bool> playerList { get; set; } = new();
    public int strike { get; set; }

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
        strike = MapBuilder.instance.strike;
    }

    private void Update()
    {
        if (!IsPressed()) return;
        Ray ray = Camera.main.ScreenPointToRay(GetScreenPosition());
        if (!Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log("Raycast missed");
            return;
        }

        Vector2Int gridPos = map.WorldToGrid(hit.point);
        Debug.Log($"Hit: {hit.point} | Grid: {gridPos} | OnMap: {map.IsOnMap(gridPos)}");

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
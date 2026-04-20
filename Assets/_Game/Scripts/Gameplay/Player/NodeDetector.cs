using System.Collections.Generic;
using UnityEngine;

public class NodeDetector : MonoBehaviour, IWinCondition, IStrike
{
    public Dictionary<byte, bool> playerList { get; set; } = new();
    public int strike { get; set; }

    // 4 axes — each pair of opposite directions covers one line type
    static readonly Vector2Int[] CheckDirections =
    {
        new(1,  0),  // Right  → also covers Left
        new(0,  1),  // Up     → also covers Down
        new(1,  1),  // UpRight → also covers DownLeft
        new(1, -1),  // UpLeft  → also covers DownRight
    };

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

        // Ignore clicks outside the map or on occupied cells
        if (!map.IsOnMap(gridPos) || map.virtualMap[gridPos] != null) return;

        map.UpdateMap(gridPos, currentPlayer);

        if (WinCondition(currentPlayer))
        {
            playerList[currentPlayer] = true;
        }
        else if (IsBoardFull())
        {
            GameManager.instance.OnDraw();
        }
        else
        {
            GameManager.instance.NextTurn();
        }
    }

    // ── IWinCondition ────────────────────────────────────────────────────────

    public bool WinCondition(byte playerId)
    {
        Vector2Int placed = GameManager.instance.LastPlaced;

        foreach (var dir in CheckDirections)
        {
            Vector2Int start = WalkDirection(placed, -dir, playerId);
            Vector2Int end = WalkDirection(placed, dir, playerId);

            // Works for all 4 axes
            int count = Mathf.Max(
                Mathf.Abs(end.x - start.x),
                Mathf.Abs(end.y - start.y)
            ) + 1;

            if (count >= strike)
            {
                GameManager.instance.OnWin(playerId, start, end);
                return true;
            }
        }
        return false;
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    // Recursively walks in dir until it hits a different player or edge
    // Returns the furthest same-player node found
    Vector2Int WalkDirection(Vector2Int current, Vector2Int dir, byte playerId)
    {
        Vector2Int next = current + dir;
        if (!map.IsOnMap(next) || map.virtualMap[next] != playerId)
            return current; // base case
        return WalkDirection(next, dir, playerId);
    }

    /// <summary>
    /// Walks from <paramref name="origin"/> in <paramref name="dir"/> 
    /// and counts consecutive cells owned by <paramref name="playerId"/>.
    /// </summary>
    int CountInDirection(Vector2Int origin, Vector2Int dir, byte playerId)
    {
        int count = 0;
        Vector2Int current = origin + dir;

        while (map.IsOnMap(current) && map.virtualMap[current] == playerId)
        {
            count++;
            current += dir;
        }
        return count;
    }

    bool IsBoardFull()
    {
        foreach (var cell in map.virtualMap.Values)
            if (cell == null) return false;
        return true;
    }
}
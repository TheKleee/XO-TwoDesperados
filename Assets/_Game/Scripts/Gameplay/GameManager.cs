using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Players")]
    [SerializeField] byte playerCount = 2;

    [Header("Strike")]
    [SerializeField] GameObject strikeLinePrefab;

    public byte CurrentPlayer { get; private set; } = 1;
    public Vector2Int LastPlaced { get; private set; }

    Map map;

    private void Awake()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
    }

    private void Start()
    {
        map = FindFirstObjectByType<Map>();
    }

    public void RegisterPlacement(Vector2Int pos) =>
        LastPlaced = pos;

    public void NextTurn() =>
        CurrentPlayer = (byte)(CurrentPlayer % playerCount + 1);

    public void OnWin(byte playerId, Vector2Int start, Vector2Int end)
    {
        Vector3 startWorld = map.GridToWorld(start);
        Vector3 endWorld = map.GridToWorld(end);

        GameObject line = Instantiate(strikeLinePrefab, startWorld, Quaternion.identity);
        Strike strike = line.GetComponent<Strike>();
        strike.Init(startWorld, endWorld);

        Debug.Log($"Player {playerId} wins!");
        // TODO: show Game Over popup, record stats
    }

    public void OnDraw()
    {
        Debug.Log("Draw!");
        // TODO: show Game Over popup, record stats
    }

    public void RestartMatch()
    {
        CurrentPlayer = 1;
        // TODO: reset map, clear virtualMap, rebuild board
    }
}
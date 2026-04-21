using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Players")]
    [SerializeField] byte playerCount = 2;

    byte[] activePlayers;
    ScoreCalculator scoreCalculator;

    [Header("Strike")]
    [SerializeField] GameObject strikeLinePrefab;

    [Header("Popups:")]
    [SerializeField] GameResultPopup gameResultPopup;

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
        scoreCalculator = FindFirstObjectByType<ScoreCalculator>();
        activePlayers = new byte[playerCount];
        for (byte i = 0; i < playerCount; i++)
            activePlayers[i] = (byte)(i + 1);
    }

    public void RegisterPlacement(Vector2Int pos) =>
        LastPlaced = pos;

    public void NextTurn() =>
        CurrentPlayer = (byte)(CurrentPlayer % playerCount + 1);

    public void OnWin(byte playerId, Vector2Int start, Vector2Int end)
    {
        HUD.instance.StopMatch();

        scoreCalculator.RecordWin(playerId, HUD.instance.MatchDuration);
        Vector3 startWorld = map.GridToWorld(start);
        Vector3 endWorld = map.GridToWorld(end);

        GameObject line = Instantiate(strikeLinePrefab, startWorld, Quaternion.identity);
        Strike strike = line.GetComponent<Strike>();
        strike.Init(startWorld, endWorld);

        gameResultPopup.OpenWin(playerId, HUD.instance.MatchDuration);
    }

    public void OnDraw()
    {
        HUD.instance.StopMatch();
        scoreCalculator.RecordDraw(activePlayers, HUD.instance.MatchDuration);
        gameResultPopup.OpenDraw(HUD.instance.MatchDuration);
    }

    public void RestartMatch() =>
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    
}
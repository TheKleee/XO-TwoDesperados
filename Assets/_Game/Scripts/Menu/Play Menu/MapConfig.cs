using UnityEngine;

public class MapConfig : MonoBehaviour
{
    [SerializeField] StepCounter mapSizeCounter;
    [SerializeField] StepCounter playerCounter;
    [SerializeField] StepCounter strikeCounter;

    private void Start()
    {
        mapSizeCounter.Init(defaultVal: 3, minVal: 3, maxVal: 7);
        playerCounter.Init(defaultVal: 2, minVal: 2, maxVal: 3);
        strikeCounter.Init(defaultVal: 3, minVal: 3, maxVal: 3);
    }

    // Called by − and + buttons on mapSize in inspector
    public void OnMapSizeIncrement()
    {
        mapSizeCounter.Increment();
        playerCounter.SetMax(Mathf.Min(mapSizeCounter.Value, 6));
        strikeCounter.SetMax(mapSizeCounter.Value);
    }

    public void OnMapSizeDecrement()
    {
        mapSizeCounter.Decrement();
        playerCounter.SetMax(Mathf.Min(mapSizeCounter.Value, 6));
        strikeCounter.SetMax(mapSizeCounter.Value);
    }

    public void Apply()
    {
        MapBuilder.instance.SetMapSize(mapSizeCounter.Value, mapSizeCounter.Value);
        MapBuilder.instance.SetStrike(strikeCounter.Value);
    }

    public byte Players => (byte)playerCounter.Value;
}
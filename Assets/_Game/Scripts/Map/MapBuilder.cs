using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class MapBuilder : MonoBehaviour, IVirtualMapSqare, IStrike, ISkinData
{
    //This one will create virtual nodes and visual map lines.
    //Selecting a space between the lines will approximate to the closest node

    #region Singleton
    public static MapBuilder instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        Destroy(gameObject);
    }
    #endregion singleton />
    Transform map;
    [Header("Node"), SerializeField] GameObject borderLine;
    Vector3Int lineData = Vector3Int.zero, lineRot = Vector3Int.zero;

    #region InterfaceData
    public byte playerCount { get; set; } = 2;
    public Vector2Int mapSize { get; set; } = new(3, 3);    
    public int strike { get; set; } = 3;
    public Dictionary<byte, byte> playerSkinMap { get; set; } = new();
    #endregion interace data />

    #region Methods
    public void SetMapSize(int x = 3, int y = 3) => mapSize = new(x, y);
    public void SetStrike(int strike) => this.strike = strike;

    public void BuildMap()
    {
        map = FindFirstObjectByType<Map>().transform;
        if (map == null) return;

        OffsetMap(new((float)mapSize.x / 2, 0, (float)mapSize.y / 2));

        //Without the first and the last element -> just the inner border
        for (int i = 1; i < mapSize.x; i++)
        {
            CreateBorderLine(i, 0, false);
            for (int j = 1; j < mapSize.y; j++)
            {
                CreateBorderLine(i, j);
            }
        }
    }

    void CreateBorderLine(float x, float y, bool rotate = true)
    {
        lineData = Vector3Int.zero;
        lineData.x = rotate ? 0 : (int)x;
        lineData.z = rotate ? (int)y : 0;
        var line = Instantiate(borderLine, map);
        line.transform.localPosition = lineData;

        if (rotate)
        {
            lineRot.y = 90;
            line.transform.localEulerAngles = lineRot;
            lineRot.y = 0;
        }

        lineData.x = lineData.y = 1;
        lineData.z = mapSize.y;
        line.transform.localScale = lineData;
    }

    void OffsetMap(Vector3 offset)
    {
        //Calculate offsetposition...
        map.position -= offset;
    }
    #endregion methods />
}

using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class MapBuilder : MonoBehaviour, IVirtualMapSqare, IStrike
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
    Vector3 linePos = Vector3.zero, lineRot = Vector3.zero;

    #region InterfaceData
    public Vector2Int mapSize { get; set; } = new(3, 3);
    public int strike { get; set; } = 3;
    #endregion interace data />

    #region Methods
    public void SetMapSize(int x = 3, int y = 3) => mapSize = new(x, y);
    public void SetStrike(int strike) => this.strike = strike;

    public void BuildMap()
    {
        map = FindFirstObjectByType<Map>().transform;
        if (map == null) return;

        OffsetMap(new(mapSize.x, mapSize.y, 0));

        //Without the first and the last element -> just the inner border
        for (int i = 1; i < mapSize.x - 1; i++)
        {
            CreateBorderLine(i, 0, false);
            for (int j = 1; j < mapSize.y - 1; j++)
            {
                CreateBorderLine(i, j);
            }
        }
    }

    void CreateBorderLine(int x, int y, bool rotate = true)
    {
        linePos.x = rotate ? 0 : x;
        linePos.z = rotate ? y : 0;
        var line = Instantiate(borderLine, map);
        line.transform.localPosition = linePos;

        if (rotate)
        {
            lineRot.y = 90;
            line.transform.localEulerAngles = lineRot;
            lineRot.y = 0;
        }

        linePos.x = linePos.y = 1;
        linePos.z = mapSize.y;
        line.transform.localScale = linePos;
    }

    void OffsetMap(Vector3 offset)
    {
        //Calculate offsetposition...
        map.position -= offset;
    }

    public void UpdateMap(Vector2Int pos, byte? player = null)
    {
        throw new System.NotImplementedException();
    }
    #endregion methods />
}

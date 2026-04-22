using System;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour, IVirtualMapSqare, IVirtualMapXO, IMapHelper
{
    //#region Singleton
    //public static Map instance;
    //private void Awake()
    //{
    //    if (instance != null)        
    //        Destroy(gameObject);
    //    instance = this;
    //}
    //#endregion singleton />

    [Header("Background"), SerializeField] Transform background;

    #region VirtualMapData
    public Vector2Int mapSize { get; set; }
    public Dictionary<Vector2Int, byte?> virtualMap { get; set; } = new(); //Up to 6 players (for all of the colors)
    #endregion virtual map data />

    #region Methods
    private void Start() =>
        CreateMap();

    /// <summary>
    /// Call this only when you need to create a map
    /// </summary>
    private void CreateMap()
    {
        ReadMap();
        Vector2Int vec = Vector2Int.zero;
        for (int i = 0; i < mapSize.x; i++)
            for (int j = 0; j < mapSize.y; j++)
            {
                vec.x = i;
                vec.y = j;
                UpdateMap(vec);
                //I could be using:
                //UpdateMap(new(i,j)); //but this approach would run a bit slower if you'd really wanna make millions of nodes on the
                //map for some reason xD
            }
    }

    void ReadMap()
    {
        MapBuilder.instance.BuildMap();
        mapSize = MapBuilder.instance.mapSize;
        background.localScale = new Vector3(mapSize.x, mapSize.y, 1);
    }

    /// <summary>
    /// Call this whenever you select a node on the map
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="player"></param>
    public void UpdateMap(Vector2Int pos, byte? player = null) =>
        virtualMap[pos] = player;

    public bool IsOnMap(Vector2Int pos) =>
        virtualMap.ContainsKey(pos);

    /// <summary>
    /// Converts a world position to the nearest grid coordinate.
    /// Uses local space so map offset/scale is handled automatically.
    /// </summary>
    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        Vector3 local = transform.InverseTransformPoint(worldPos);
        return new Vector2Int(Mathf.RoundToInt(local.x), Mathf.RoundToInt(local.z));
    }

    public Vector3 GridToWorld(Vector2Int gridPos)
    {
        Vector3 local = new Vector3(gridPos.x, 0, gridPos.y);
        return transform.TransformPoint(local);
    }
    #endregion methods />
}

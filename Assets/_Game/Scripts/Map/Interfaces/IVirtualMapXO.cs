
using UnityEngine;
using System.Collections.Generic;

public interface IVirtualMapXO
{
    public Dictionary<Vector2Int, byte?> virtualMap { get; set; } //Let's make the map dynamic! :D
    public void UpdateMap(Vector2Int pos, byte? player = null); //player -> 1 = X, 2 = O, else => no one...
}

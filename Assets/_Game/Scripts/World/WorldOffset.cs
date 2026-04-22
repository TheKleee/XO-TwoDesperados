using UnityEngine;

public class WorldOffset : MonoBehaviour
{
    private void Start()
    {
        float pos = (float)MapBuilder.instance.mapSize.x / 2;
        transform.position += new Vector3(pos, 0, pos);
    }
}

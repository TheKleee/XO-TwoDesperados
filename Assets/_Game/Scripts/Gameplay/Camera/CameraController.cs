using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float smoothSpeed = 5f;
    Camera cam;
    bool shouldFollow;
    Vector3 targetPos;

    private void Start()
    {
        cam = Camera.main;
        float pos = (float)MapBuilder.instance.mapSize.x / 2;
        targetPos = new Vector3(pos, cam.transform.position.y, pos);

        // Only follow if map is larger than what the camera can see
        float camHeight = 2f * cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;
        shouldFollow = MapBuilder.instance.mapSize.x > camWidth ||
                       MapBuilder.instance.mapSize.y > camHeight;
    }

    public void FocusOn(Vector3 worldPos)
    {
        if (!shouldFollow) return;
        targetPos = new Vector3(worldPos.x, transform.position.y, worldPos.z);
    }

    private void LateUpdate()
    {
        if (!shouldFollow) return;
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);
    }
}
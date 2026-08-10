using UnityEngine;

public class GameCamera : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float smoothSpeed = 5f;

    private Bounds bounds;

    private void Start() {
        BoundsInt mapBounds = FindFirstObjectByType<TilemapManager>().GetMapBounds();
        bounds.SetMinMax(
            new Vector3(
                mapBounds.xMin + cam.orthographicSize * cam.aspect, 
                mapBounds.yMin + cam.orthographicSize), 
            new Vector3(
                mapBounds.xMax - cam.orthographicSize * cam.aspect, 
                mapBounds.yMax - cam.orthographicSize)
            );
        
    }

    private void LateUpdate() {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Mathf.Abs(transform.position.z);

        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(mouseScreenPos);
        Vector3 targetPos = new Vector3(mouseWorldPos.x, mouseWorldPos.y, transform.position.z);
        
        Vector3 currentPos = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime); ;
        currentPos.x = Mathf.Clamp(currentPos.x, bounds.min.x, bounds.max.x);
        currentPos.y = Mathf.Clamp(currentPos.y, bounds.min.y, bounds.max.y);
        
        transform.position = currentPos;
    }
}

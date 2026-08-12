using UnityEngine;

public class GameCamera : MonoBehaviour
{
    [Header("Scroll")]
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private float padding;
    public bool canMove;

    [Header("Zoom")]
    [SerializeField] private float zoomStep = 2f;
    [SerializeField] private float minZoom = 3f;
    [SerializeField] private float maxZoom = 20f;
    [SerializeField] private float zoomSpeed = 5f;
    private float targetZoom;
    private float targetPad;

    [Header("Other")]
    [SerializeField] private Camera cam;
    private BoundsInt mapBounds;

    private void Start() {
        mapBounds = FindFirstObjectByType<TilemapManager>().GetMapBounds();

        targetZoom = cam.orthographicSize;
        targetPad = padding;
    }

    private void Update() {
        float scrollData = Input.GetAxis("Mouse ScrollWheel");
        if (scrollData != 0f) {
            targetZoom -= zoomStep*Mathf.Sign(scrollData);
            targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);

            targetPad = padding * (targetZoom / 5);
        }

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * zoomSpeed);

        Vector3 mouseScreenPos = Input.mousePosition;
        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(mouseScreenPos);
        if (CanMove(mouseWorldPos)) {
            Vector3 targetPos = new Vector3(mouseWorldPos.x, mouseWorldPos.y, transform.position.z);

            Vector3 currentPos = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime); ;
            currentPos.x = Mathf.Clamp(currentPos.x, mapBounds.xMin + cam.orthographicSize * cam.aspect, mapBounds.xMax - cam.orthographicSize * cam.aspect);
            currentPos.y = Mathf.Clamp(currentPos.y, mapBounds.yMin + cam.orthographicSize, mapBounds.yMax - cam.orthographicSize);

            transform.position = currentPos;
        }
    }

    private bool CanMove(Vector3 mousePos) {
        if (!canMove)
            return false;

        Vector3 cursorPos = mousePos - transform.position;
        float padX = cam.orthographicSize * cam.aspect - targetPad;
        float padY = cam.orthographicSize - targetPad;

        if (cursorPos.x < padX && cursorPos.x > -padX && cursorPos.y < padY && cursorPos.y > -padY)
            return false;
        return true;
    }
}

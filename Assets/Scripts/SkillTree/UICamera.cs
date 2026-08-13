using UnityEngine;
using UnityEngine.EventSystems;

public class UICamera : MonoBehaviour, IScrollHandler, IDragHandler {
    private RectTransform rectTransform;

    [SerializeField] private float zoomSpeed = 0.1f;
    [SerializeField] private float minZoom = 0.5f;
    [SerializeField] private float maxZoom = 3.0f;
    [SerializeField] private RectTransform bounds;

    void Awake() {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnScroll(PointerEventData eventData) {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localMousePos);

        float zoomDelta = eventData.scrollDelta.y * zoomSpeed;
        Vector3 previousScale = rectTransform.localScale;
        Vector3 newScale = previousScale + Vector3.one * zoomDelta;
        
        newScale.x = Mathf.Clamp(newScale.x, minZoom, maxZoom);
        newScale.y = Mathf.Clamp(newScale.y, minZoom, maxZoom);
        newScale.z = 1f;

        rectTransform.localScale = newScale;

        Vector3 deltaScale = newScale - previousScale;
        rectTransform.localPosition -= Vector3.Scale(localMousePos, deltaScale);

        KeepInBounds();
    }

    public void OnDrag(PointerEventData eventData) {
        rectTransform.anchoredPosition += eventData.delta;

        KeepInBounds();
    }

    private void KeepInBounds() {
        RectTransform parentRect = rectTransform.parent as RectTransform;
        Vector2 viewSize = parentRect.rect.size;
        Vector2 panelSize = Vector2.Scale(bounds.rect.size, rectTransform.localScale);

        float minX = (viewSize.x - panelSize.x) * 0.5f;
        float maxX = (panelSize.x - viewSize.x) * 0.5f;
        float minY = (viewSize.y - panelSize.y) * 0.5f;
        float maxY = (panelSize.y - viewSize.y) * 0.5f;

        Vector3 currentPos = rectTransform.localPosition;
        currentPos.x = Mathf.Clamp(currentPos.x, minX, maxX);
        currentPos.y = Mathf.Clamp(currentPos.y, minY, maxY);

        rectTransform.localPosition = currentPos;
    }
}
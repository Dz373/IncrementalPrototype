using UnityEngine;
using UnityEngine.EventSystems;

public class UICamera : MonoBehaviour, IScrollHandler, IDragHandler {
    private RectTransform rectTransform;

    [SerializeField] private float zoomSpeed = 0.1f;
    [SerializeField] private float minZoom = 0.5f;
    [SerializeField] private float maxZoom = 3.0f;

    void Awake() {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnScroll(PointerEventData eventData) {
        Vector3 newScale = rectTransform.localScale + Vector3.one * (eventData.scrollDelta.y * zoomSpeed);

        newScale.x = Mathf.Clamp(newScale.x, minZoom, maxZoom);
        newScale.y = Mathf.Clamp(newScale.y, minZoom, maxZoom);
        newScale.z = 1f;

        rectTransform.localScale = newScale;
    }

    public void OnDrag(PointerEventData eventData) {
        rectTransform.anchoredPosition += eventData.delta;
    }
}
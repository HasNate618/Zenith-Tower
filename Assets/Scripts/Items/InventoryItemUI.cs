using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItemUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public static GameObject selectedItem;
    public InventoryItemData referenceData;
    public int inventoryType;

    [SerializeField] CanvasGroup group;
    [SerializeField] RectTransform rectTransform;
    private float canvasScale;

    private Text itemDesc;

    private void Awake()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
        canvasScale = canvas.GetComponent<Canvas>().scaleFactor;
        itemDesc = canvas.GetComponent<InventoryUI>().itemDesc;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (selectedItem == null) selectedItem = gameObject;
        group.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (selectedItem == gameObject) rectTransform.anchoredPosition += eventData.delta / canvasScale;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition = Vector2.zero;
        if (selectedItem == gameObject) selectedItem = null;
        group.blocksRaycasts = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (selectedItem == null) itemDesc.text = referenceData.description;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (itemDesc.text == referenceData.description) itemDesc.text = null;
    }
}

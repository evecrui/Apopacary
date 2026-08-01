using UnityEngine;
using UnityEngine.EventSystems;

public class DragUI : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    PlayerInventory inventory;
    Vector2 offset;

    void Start() {
        inventory = PlayerInventory.PI;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        offset = new Vector2(transform.position.x, transform.position.y) - eventData.position;
    }

    public void OnDrag(PointerEventData eventData) {
        transform.position = eventData.position + offset;
    }

    public void OnEndDrag(PointerEventData eventData) {

    }

    public void OnPointerEnter(PointerEventData eventData) {
        inventory.hoveringShelf = gameObject;
    }

    public void OnPointerExit(PointerEventData eventData) {
        inventory.hoveringShelf = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

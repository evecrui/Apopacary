using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UIElements;


public class DragUI : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler, IMouseCaptureEvent
{
    PlayerInventory inventory;
    Vector2 offset;
    public Vector3 ogPos;

    void Start() {
        inventory = PlayerInventory.PI;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        ogPos = transform.position;
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

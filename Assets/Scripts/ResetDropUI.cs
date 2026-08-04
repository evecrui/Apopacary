using UnityEngine;
using UnityEngine.EventSystems;

public class ResetDropUI : MonoBehaviour, IDropHandler {
    public void OnDrop(PointerEventData eventData) {
        eventData.pointerDrag.transform.position = eventData.pointerDrag.GetComponent<DragUI>().ogPos;
    }
}

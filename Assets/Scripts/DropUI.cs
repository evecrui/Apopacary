using UnityEngine;
using UnityEngine.EventSystems;

public class DropUI : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData) {
        Vector3 ogPos = eventData.pointerDrag.GetComponent<DragUI>().ogPos;
        eventData.pointerDrag.transform.position = transform.position;
        transform.position = ogPos;
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragObj : MonoBehaviour
{
    [SerializeField] private InputAction press, screenPos;
    private Vector3 mousePos;
    Camera cam;
    Rigidbody rb;
    private bool isDragging;
    private Vector3 mouseWorldPos {
        get {
            float z = cam.WorldToScreenPoint(transform.position).z;
            return cam.ScreenToWorldPoint(mousePos + new Vector3(0, 0, z));
        }
    }
    private bool isClicked { 
        get {
            if (mousePos.x < 0) return false;
            Ray ray = cam.ScreenPointToRay(mousePos);
            RaycastHit hit;
            Debug.DrawRay(ray.origin, ray.direction);
            if (Physics.Raycast(ray, out hit)) {
                Debug.Log("Hit: " + hit.transform.name);
                return hit.transform == transform;
            }
            return false;
        } }

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        screenPos.Enable();
        press.Enable();
        screenPos.performed += context => { 
            mousePos = context.ReadValue<Vector2>() - new Vector2(520, 0);
            mousePos.x = Mathf.Clamp(mousePos.x, -1, 1400); };
        press.performed += _ => { Debug.Log("MouseClick"); if(isClicked) StartCoroutine(Drag()); };
        press.canceled += _ => { isDragging = false; };
    }

    void Update() {
        Ray ray = cam.ScreenPointToRay(mousePos);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction);
    }

    private IEnumerator Drag() {
        Debug.Log("Dragging");
        isDragging = true;
        Vector3 offset = transform.position - mouseWorldPos;
        rb.useGravity = false;
        rb.constraints = (RigidbodyConstraints)126; // no rotation
        //Grab
        while (isDragging) {
            //Dragging
            transform.position = mouseWorldPos + offset;
            yield return null;
        }
        //Drop
        rb.useGravity = true;
        rb.constraints = (RigidbodyConstraints)0;
    }
}

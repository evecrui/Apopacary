using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragObj : MonoBehaviour
{
    [SerializeField] private InputAction press, screenPos;
    private Vector3 mousePos;
    private Transform playerTrans;
    Camera cam;
    Rigidbody rb;
    public bool isDragging;
    public Interactible hoveredInteractable;
    public Interactible holdingInteractable;

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
            if (Physics.Raycast(ray, out hit)) {
                return hit.transform == transform;
            }
            return false;
        } }

    private bool inPlayerRange {
        get {
            return Vector3.Magnitude(playerTrans.position - transform.position) < maxRangeThreshold;
        }
    }

    float maxRangeThreshold = 3;

    private void CheckHoveredObjs()
    {
        if (mousePos.x < 0)
        {
            if (hoveredInteractable != null)
            {
                hoveredInteractable.UnHover();
                hoveredInteractable = null;
            }
            return;
        }
        Ray ray = cam.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Interactible i = hit.transform.GetComponent<Interactible>();
            if (hoveredInteractable != i && hoveredInteractable != null)
                hoveredInteractable.UnHover();
            if (i != null)
                i.Hover();
            hoveredInteractable = i; // do this if hoveredI = null, and if hoveredI = or != i
        }
    }

    private void Awake() {
        playerTrans = GameObject.FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None)[0].transform;
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        screenPos.Enable();
        press.Enable();
        screenPos.performed += context => { 
            mousePos = context.ReadValue<Vector2>() - new Vector2(520, 0);
            mousePos.x = Mathf.Clamp(mousePos.x, -1, 1400); };
        press.performed += _ => { if(isClicked && inPlayerRange) StartCoroutine(Drag()); };
        press.canceled += _ => { isDragging = false; };
    }

    public IEnumerator Drag() {
        isDragging = true;
        PlayerInventory.PI.draggedIngredient = gameObject;
        Vector3 offset = transform.position - mouseWorldPos;
        float playerZOffset = transform.position.z - playerTrans.position.z;
        rb.useGravity = false;
        rb.constraints = (RigidbodyConstraints)126; // no rotation
        if (GetComponent<Collider>() != null)
            GetComponent<Collider>().enabled = false;
        if (holdingInteractable != null)
            holdingInteractable.Release(gameObject);
        //Grab
        while (isDragging && inPlayerRange) {
            //Dragging
            transform.position = mouseWorldPos + offset;
            transform.position = new Vector3(transform.position.x, transform.position.y, playerTrans.position.z + playerZOffset);

            // check for objects that are being hovered over that can be interacted with
            CheckHoveredObjs();
            yield return null;
        }
        //Drop
        rb.useGravity = true;
        rb.constraints = (RigidbodyConstraints)0;
        if (GetComponent<Collider>() != null)
            GetComponent<Collider>().enabled = true;
        PlayerInventory.PI.draggedIngredient = null;
        if (hoveredInteractable != null) { hoveredInteractable.Interact(gameObject); hoveredInteractable.UnHover(); }
        
    }
}

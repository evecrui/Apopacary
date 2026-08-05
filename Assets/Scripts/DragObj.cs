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
    public MeshRenderer mr;
    public bool isDragging;
    public Interactable hoveredInteractable;
    public Interactable holdingInteractable;

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
            Interactable i = hit.transform.GetComponent<Interactable>();
            if (i != null && !i.InteractableWithOtherIng()) return;
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
        if (holdingInteractable != null)
            holdingInteractable.Release(gameObject);
        rb.useGravity = false;
        rb.constraints = (RigidbodyConstraints)126; // no rotation
        if (GetComponent<Collider>() != null)
            GetComponent<Collider>().enabled = false;
        GetComponent<ParticleSystem>().Play();
        //Grab
        while (isDragging) {
            //Dragging
            transform.position = mouseWorldPos + offset;
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, 0, transform.position.y), playerTrans.position.z + playerZOffset);
            if (mousePos.x < 0)
                mr.enabled = false;
            else mr.enabled = true;

            // check for objects that are being hovered over that can be interacted with
            CheckHoveredObjs();
            yield return null;
        }
        //Drop
        GetComponent<ParticleSystem>().Stop();
        if (mousePos.x < 0)
        {
            PlayerInventory.PI.draggedIngredient = null;
            PlayerInventory.PI.AddIngredient(gameObject.name);
            gameObject.SetActive(false);
        } else {
            rb.useGravity = true;
            rb.constraints = (RigidbodyConstraints)0;
            if (GetComponent<Collider>() != null)
                GetComponent<Collider>().enabled = true;
            PlayerInventory.PI.draggedIngredient = null;
            if (hoveredInteractable != null)
                { hoveredInteractable.Interact(gameObject); hoveredInteractable.UnHover(); }
        }
    }
}

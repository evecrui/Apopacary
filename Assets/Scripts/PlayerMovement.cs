using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Transform cameraTransform;
    public Transform playerGraphics;
    public PlayerCamera playerCam;
    public Rigidbody rb;

    public Vector2 _moveDelta;
    public float speed;
    public float runSpeed;
    public bool running = false;

    public List<GameObject> inRangeOptions;
    private Vector3 mousePos;
    private Camera cam;
    [SerializeField] private InputAction screenPos;

    private Transform lastHoveredFruit;
    private int ogHoveredlayermask;

    private void Awake()
    {
        cam = Camera.main;
        screenPos.Enable();
        screenPos.performed += context =>
        {
            mousePos = context.ReadValue<Vector2>() - new Vector2(520, 0);
            mousePos.x = Mathf.Clamp(mousePos.x, -1, 1400);
            Interactable i = CheckHovered();
            Transform ing = CheckHoveredIngredients();
            SetHovered(i, ing);

        };
    }

    // Update is called once per frame
    void Update()
    {
        if (_moveDelta.magnitude > 0)
        {
            Interactable i = CheckHovered();
            Transform ing = CheckHoveredIngredients();
            SetHovered(i, ing);

            Vector2 moveDelta = _moveDelta * (running ? runSpeed : speed) * Time.deltaTime;
            transform.Translate(new Vector3(moveDelta.x, 0, moveDelta.y));
            playerGraphics.rotation = Quaternion.LookRotation(new Vector3(moveDelta.x, 0, moveDelta.y));
        }
    }

    private Interactable CheckHovered()
    {
        if (mousePos.x < 0) return null;
        Ray ray = cam.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Interactable i = hit.transform.GetComponent<Interactable>();
            if (i == null || !i.emptyHandInteractable || (i.needHoldIngToInteractEmptyHanded && i.heldIngredient != null)
                || (i.heldIngredient != null && !PlayerInventory.PI.NameToIngredient[i.heldIngredient.name].Alterations.ContainsKey(i.name))
                || Vector3.Distance(hit.transform.position, transform.position) >= 3)
                return null;
            return i;
        }
        return null;
    }

    private Transform CheckHoveredIngredients()
    {
        if (mousePos.x < 0) return null;
        Ray ray = cam.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
            if (hit.transform.tag == "Ingredient" && Vector3.Distance(hit.transform.position, transform.position) < 3)
                return hit.transform;
        return null;
    }

    private void SetHovered(Interactable i, Transform ing)
    {
        if (ing != null)
        {
            if (ing != lastHoveredFruit)
            {
                if (lastHoveredFruit != null)
                    Unhover(lastHoveredFruit);
                lastHoveredFruit = ing;
                Hover(lastHoveredFruit);
            }
        } else if (lastHoveredFruit != null)
        {
            Unhover(lastHoveredFruit);
            lastHoveredFruit = null;
        }
        Interactable prevI = PlayerInventory.PI.hoveringInteractable;
        if (i != null && !i.highlighted)
        {
            lastHoveredFruit = null;
            if (prevI != i)
            {
                if (prevI != null)
                    prevI.UnHover();
                PlayerInventory.PI.hoveringInteractable = i;
            }
            i.Hover();
        }
        else if (i == null && prevI != null)
        {
            prevI.UnHover();
            PlayerInventory.PI.hoveringInteractable = null;
        }
    }

    private void Hover(Transform t)
    {
        if (t.gameObject.layer != 3)
            ogHoveredlayermask = t.gameObject.layer;
        t.gameObject.layer = 3;
        foreach (Transform _t in t.GetComponentsInChildren<Transform>())
        {
            _t.gameObject.layer = 3;
        }
    }

    private void Unhover(Transform t)
    {
        t.gameObject.layer = ogHoveredlayermask;
        foreach (Transform _t in t.GetComponentsInChildren<Transform>())
        {
            _t.gameObject.layer = ogHoveredlayermask;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveDelta = context.ReadValue<Vector2>();
        _moveDelta.Normalize();
    }

    public void OnShift(InputAction.CallbackContext context) {
        if (context.started) {
            running = true;
        } else if (context.canceled) {
            running = false;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Ingredient") {
            if (!inRangeOptions.Contains(other.gameObject))
                inRangeOptions.Add(other.gameObject);
            other.gameObject.layer = 3;
        } else if (other.gameObject.name == "Floor") {
            playerCam.EnterRoom();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Ingredient") {
            if (inRangeOptions.Contains(other.gameObject))
                inRangeOptions.Remove(other.gameObject);
            other.gameObject.layer = LayerMask.GetMask("Default");
        } else if (other.gameObject.name == "Floor") {
            playerCam.ExitRoom();
        }
    }
}

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

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

    private Transform lastHoveredngredient;
    private Transform tooltippedObject;
    private int ogHoveredlayermask;
    public RectTransform tooltipTrans;
    public TextMeshProUGUI tooltipText;

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

    public void UpdateHovered() {
        Interactable i = CheckHovered();
        Transform ing = CheckHoveredIngredients();
        SetHovered(i, ing);
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
        Debug.DrawRay(ray.origin, ray.direction);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            if (hit.transform.tag == "Ingredient" && Vector3.Distance(hit.transform.position, transform.position) < 3)
                return hit.transform;
        }
        return null;
    }

    private void SetHovered(Interactable i, Transform ing)
    {
        if (ing != null)
        {
            if (ing != lastHoveredngredient)
            {
                if (lastHoveredngredient != null)
                    Unhover(lastHoveredngredient);
                lastHoveredngredient = ing;
                Hover(lastHoveredngredient);
                tooltippedObject = lastHoveredngredient;
            }
        } else if (lastHoveredngredient != null)
        {
            Unhover(lastHoveredngredient);
            lastHoveredngredient = null;
            tooltippedObject = null;
        }
        Interactable prevI = PlayerInventory.PI.hoveringInteractable;
        if (i != null && !i.highlighted)
        {
            lastHoveredngredient = null;
            if (prevI != i)
            {
                if (prevI != null)
                    prevI.UnHover();
                PlayerInventory.PI.hoveringInteractable = i;
            }
            i.Hover();
            tooltippedObject = i.transform;
        }
        else if (i == null && prevI != null)
        {
            prevI.UnHover();
            PlayerInventory.PI.hoveringInteractable = null;
            if (lastHoveredngredient == null)
                tooltippedObject = null;
        }

        if (tooltippedObject != null) {
            Regex r = new Regex(@"(?!^)(?=[A-Z])");
            string extraLines = "\n";
            if (lastHoveredngredient == null)
                extraLines = PlayerInventory.PI.hoveringInteractable.heldIngredient == null ?
                    "" : "\nHolding: " + r.Replace(PlayerInventory.PI.hoveringInteractable.heldIngredient.name, " ");
            else if (lastHoveredngredient.name == "Drink") {
                Drink drink = lastHoveredngredient.GetComponent<Drink>();
                extraLines += drink.GetDrinkComponents();
                if (drink.flavours.Count > 0) {
                    extraLines += "\nFlavours: ";
                    foreach ((Drink.Flavour, Drink.Strength) e in drink.flavours)
                        extraLines += e.Item2 + " " + e.Item1 + ", ";
                    if (extraLines.Length > 2)
                        extraLines = extraLines.Substring(0, extraLines.Length - 2);
                }
            } else { 
                Ingredient hovering = PlayerInventory.PI.NameToIngredient[lastHoveredngredient.name];
                foreach (Enum e in hovering.preDrinkVars)
                    extraLines += e.ToSafeString() + ", ";
                if (extraLines.Length > 2)
                    extraLines = extraLines.Substring(0, extraLines.Length - 2);
                if (hovering.CheckInfusion(lastHoveredngredient.gameObject) != Ingredient.Infusion.None && hovering.infusable)
                    extraLines += "\nInfusion: " + hovering.CheckInfusion(lastHoveredngredient.gameObject);
            }
            tooltipText.text = r.Replace(tooltippedObject.name, " ") + extraLines;
            tooltipTrans.gameObject.SetActive(true);
            tooltipTrans.position = mousePos + new Vector3(520, 0, 0);
        } else {
            tooltipTrans.gameObject.SetActive(false);
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
            playerCam.EnterRoom(other.gameObject);
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

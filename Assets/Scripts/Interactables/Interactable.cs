using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;

public class Interactable : MonoBehaviour
{
    int layermask;
    public GameObject interactableHeldIngredient;
    public GameObject heldLiquidIngredient;
    public GameObject heldLiquidIngredientShape;
    public bool onlyLiquid;
    public bool onlySolid;
    public bool canHoldLiquidAndSolid;
    public bool emptyHandInteractable;
    public bool onlyEmptyWithHeld;
    public bool onlyEmptyNoHeld;
    public bool highlighted;
    public AudioSource source;
    public List<AudioClip> clips;
    public bool coroutineRunning;
    public PlayerInventory inventory;
    public Rigidbody rb;
    public Vector3 relativeHeldPos;



    public virtual void Interact(GameObject ingredient) {
        if (inventory == null)
            inventory = PlayerInventory.PI;
        // if ing = cup and you're taking liquid out
        // if ing = liquid
        // if ing = solid
            // if solid already in

        if (ingredient.name.StartsWith("Bucket")) {
            Bucket bucket = ingredient.GetComponent<Bucket>();
            if (heldLiquidIngredient != null && bucket.empty) {
                bucket.Fill(heldLiquidIngredient);
                heldLiquidIngredientShape.SetActive(false);
                heldLiquidIngredient = null;
            }
            else if (heldLiquidIngredient == null && !bucket.empty) {
                heldLiquidIngredient = bucket.Empty();
                heldLiquidIngredientShape.SetActive(true);
            }
            return;
        }
        Ingredient ing = inventory.NameToIngredient[ingredient.name];
        if (interactableHeldIngredient != null) {
            Release(interactableHeldIngredient);
        }

        // if solid and now nothing else solid inside
        ingredient.GetComponent<DragObj>().holdingInteractable = this;

        interactableHeldIngredient = ingredient;

        rb = interactableHeldIngredient.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = (RigidbodyConstraints)126; // no rotation

        interactableHeldIngredient.transform.position = transform.TransformPoint(relativeHeldPos);
    }

    public virtual void Release(GameObject ingredient) {
        interactableHeldIngredient = null;
    }

    public void Hover() {
        highlighted = true;
        if (gameObject.layer != 3)
            layermask = gameObject.layer;
        foreach (Transform t in transform.GetComponentsInChildren<Transform>())
        {
            if (t.name.StartsWith("Transparent") || t.name == "Infuser")
                continue;
            t.gameObject.layer = 3;
        }
    }

    public void UnHover()
    {
        highlighted = false;
        gameObject.layer = layermask;
        foreach (Transform t in transform.GetComponentsInChildren<Transform>())
        {
            t.gameObject.layer = layermask;
        }
    }

    public virtual bool InteractableWithOtherIng() {
        if (inventory == null)
            inventory = PlayerInventory.PI;

        if (inventory.draggedIngredient != null)
        {
            Ingredient ing = inventory.NameToIngredient[inventory.draggedIngredient.name];
            if (inventory.draggedIngredient.name == "Bucket")
            {
                Bucket bucket = inventory.draggedIngredient.GetComponent<Bucket>();
                if (heldLiquidIngredient != null && bucket.empty)
                {
                    return true; // if holding liquid and interacting with something to take it out
                }
                if (!bucket.empty && heldLiquidIngredient == null && (onlyLiquid || canHoldLiquidAndSolid))
                    return true; // if ok to put liquid in
            }
            else if (inventory.draggedIngredient.name == "Drink") return false;
            else if (!ing.isLiquid && (onlySolid || canHoldLiquidAndSolid) && ing.AlterationKeys.Contains(name))
                return true; // if ok to put solid in
        }

        return false;
    }

    public virtual bool InteractableWithHand()
    {
        if (inventory == null)
            inventory = PlayerInventory.PI;

        if (interactableHeldIngredient == null && onlyEmptyWithHeld)
            return false;
        else if (interactableHeldIngredient != null && onlyEmptyNoHeld)
            return false;

        else if (heldLiquidIngredient != null && interactableHeldIngredient != null)
        {
            Ingredient liquid = inventory.NameToIngredient[heldLiquidIngredient.name];
            Ingredient solid = inventory.NameToIngredient[interactableHeldIngredient.name];
            if (heldLiquidIngredient.name.EndsWith("Water") && CheckFlags(liquid, Ingredient.FlavourVariable.TeaType)
                && interactableHeldIngredient.name.StartsWith("Dried") && (name == "Stove" || name == "Infuser")
                || (name == "Infuser" && liquid.infusable && solid.infusion != Ingredient.Infusion.None))
                return true;
        }
        else if (interactableHeldIngredient != null && !onlyEmptyNoHeld)
        {
            if (inventory.NameToIngredient[interactableHeldIngredient.name].Alterations.ContainsKey(name))
                return true;
        }
        return false;
    }


    private bool CheckFlags(Ingredient ingredient, Ingredient.FlavourVariable flag) {
        return ingredient.relevantFlavourFlag == flag ||
            ingredient.relevantFlavourFlag2 == flag ||
            ingredient.relevantFlavourFlag3 == flag;
    }

    public virtual void InteractEmptyHand()
    {
        if (!InteractableWithHand()) return;
        AudioClip clip = null;
        if (source != null) {
            clip = clips[Random.Range(0, clips.Count)];
            source.PlayOneShot(clip);
        }
        Debug.Log("Interact with empty hand on " + name + "!!");
        if (interactableHeldIngredient)
        {
            if (coroutineRunning) {
                coroutineRunning = false;
                StopCoroutine(nameof(WaitTillSFXFinished));
                SwitchObjs();
            }
            coroutineRunning = true;
            if (clip == null)
                StartCoroutine(WaitTillSFXFinished(0));
            else
                StartCoroutine(WaitTillSFXFinished(clip.length));
        }
        PlayerInventory.PI.GetComponent<PlayerMovement>().UpdateHovered();
    }

    public IEnumerator WaitTillSFXFinished(float delay) {
        interactableHeldIngredient.GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(delay);
        interactableHeldIngredient.GetComponent<Collider>().enabled = true;
        SwitchObjs();
    }

    public virtual void SwitchObjs() {
        PlayerInventory PI = PlayerInventory.PI;
        if (interactableHeldIngredient == null) return;
        Ingredient oldIng = PI.NameToIngredient[interactableHeldIngredient.name];
        if (!oldIng.Alterations.ContainsKey(name)) return;
        GameObject prefab = oldIng.Alterations[name].Prefab;
        GameObject newVersion = Instantiate(prefab);
        newVersion.transform.position = interactableHeldIngredient.transform.position;
        newVersion.transform.rotation = interactableHeldIngredient.transform.rotation;
        newVersion.transform.parent = interactableHeldIngredient.transform.parent;
        newVersion.name = prefab.name;
        interactableHeldIngredient.SetActive(false);
        interactableHeldIngredient = newVersion;

        if (!PI.NameToIngredient[prefab.name].Alterations.ContainsKey(name))
            UnHover();
        PlayerInventory.PI.GetComponent<PlayerMovement>().UpdateHovered();
        coroutineRunning = false;
    }
}

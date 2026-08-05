using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;

public class Interactable : MonoBehaviour
{
    int layermask;
    public GameObject heldIngredient;
    public GameObject heldLiquidIngredient;
    public GameObject heldLiquidIngredientShape;
    public bool onlyLiquid;
    public bool onlySolid;
    public bool canHoldLiquidAndSolid;
    public bool emptyHandInteractable;
    public bool needHoldIngToInteractEmptyHanded;
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
        if (heldIngredient != null) {
            Release(heldIngredient);
        }

        // if solid and now nothing else solid inside
        ingredient.GetComponent<DragObj>().holdingInteractable = this;

        heldIngredient = ingredient;

        rb = heldIngredient.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = (RigidbodyConstraints)126; // no rotation

        heldIngredient.transform.position = transform.TransformPoint(relativeHeldPos);
    }

    public virtual void Release(GameObject ingredient) {
        heldIngredient = null;
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
            else if (!inventory.NameToIngredient[inventory.draggedIngredient.name].isLiquid && (onlySolid || canHoldLiquidAndSolid))
                return true; // if ok to put solid in
        }

        return false;
    }

    public virtual bool InteractableWithHand() {
        if (heldLiquidIngredient != null && heldIngredient != null) {
            Ingredient liquid = inventory.NameToIngredient[heldLiquidIngredient.name];
            Ingredient solid = inventory.NameToIngredient[heldIngredient.name];
            if (heldLiquidIngredient.name.EndsWith("Water") && CheckFlags(liquid, Ingredient.FlavourVariable.TeaType)
                && heldIngredient.name.StartsWith("Dried") && (name == "Stove" || name == "Infuser")
                || (name == "Infuser" && liquid.infusable && solid.infusion != Ingredient.Infusion.None))
                return true;
        } else if (heldIngredient != null) 
            if (inventory.NameToIngredient[heldIngredient.name].Alterations.ContainsKey(name))
                return true;
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
        if (heldIngredient)
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
        yield return new WaitForSeconds(delay);
        SwitchObjs();
    }

    public virtual void SwitchObjs() {
        PlayerInventory PI = PlayerInventory.PI;
        if (heldIngredient == null) return;
        Ingredient oldIng = PI.NameToIngredient[heldIngredient.name];
        if (!oldIng.Alterations.ContainsKey(name)) return;
        GameObject prefab = oldIng.Alterations[name].Prefab;
        GameObject newVersion = Instantiate(prefab);
        newVersion.transform.position = heldIngredient.transform.position;
        newVersion.transform.rotation = heldIngredient.transform.rotation;
        newVersion.transform.parent = heldIngredient.transform.parent;
        newVersion.name = prefab.name;
        heldIngredient.SetActive(false);
        heldIngredient = newVersion;

        if (!PI.NameToIngredient[prefab.name].Alterations.ContainsKey(name))
            UnHover();
        PlayerInventory.PI.GetComponent<PlayerMovement>().UpdateHovered();
        coroutineRunning = false;
    }
}

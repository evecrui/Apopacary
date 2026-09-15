using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

public class InfuserInteractable : Interactable
{
    public override void InteractEmptyHand() {
        AudioClip clip = null;
        if (source != null) {
            clip = clips[Random.Range(0, clips.Count)];
            source.PlayOneShot(clip);
        }
        Debug.Log("Interact with empty hand on " + name + "!!");
        if (interactableHeldIngredient != null && heldLiquidIngredient != null) {
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

    public override void SwitchObjs() {
        if (inventory == null)
            inventory = PlayerInventory.PI;
        Ingredient liquid = inventory.NameToIngredient[heldLiquidIngredient.name];
        Ingredient solid = inventory.NameToIngredient[interactableHeldIngredient.name];
        if (liquid != null && liquid.infusable && solid.infusion != Ingredient.Infusion.None) {

            liquid.AddInfusion(solid.infusion, heldLiquidIngredient);
            interactableHeldIngredient.SetActive(false);
            interactableHeldIngredient = null;
            return;
        } else if (liquid != null && liquid.Name.EndsWith("Water") && 
                CheckFlags(solid, Ingredient.FlavourVariable.TeaType) && solid.name.StartsWith("Dried")) {
            liquid.AddTea(solid.teaType, heldLiquidIngredient);
            interactableHeldIngredient.SetActive(false);
            interactableHeldIngredient = null;
        }


        if (interactableHeldIngredient == null) return;
        Ingredient oldIng = inventory.NameToIngredient[interactableHeldIngredient.name];
        if (!oldIng.Alterations.ContainsKey(name)) return;
        GameObject prefab = oldIng.Alterations[name].Prefab;
        GameObject newVersion = Instantiate(prefab);
        newVersion.transform.position = interactableHeldIngredient.transform.position;
        newVersion.transform.rotation = interactableHeldIngredient.transform.rotation;
        newVersion.transform.parent = interactableHeldIngredient.transform.parent;
        newVersion.name = prefab.name;
        interactableHeldIngredient.SetActive(false);
        interactableHeldIngredient = newVersion;

        if (!inventory.NameToIngredient[prefab.name].Alterations.ContainsKey(name))
            UnHover();
        inventory.GetComponent<PlayerMovement>().UpdateHovered();
        coroutineRunning = false;
    }

    private bool CheckFlags(Ingredient ingredient, Ingredient.FlavourVariable flag) {
        return ingredient.relevantFlavourFlag == flag ||
            ingredient.relevantFlavourFlag2 == flag ||
            ingredient.relevantFlavourFlag3 == flag;
    }

    public override void Release(GameObject ingredient)
    {
        ingredient.GetComponent<DragObj>().holdingInteractable = null;
        rb.useGravity = true;
        rb.constraints = (RigidbodyConstraints)0;

        interactableHeldIngredient = null;
    }
}

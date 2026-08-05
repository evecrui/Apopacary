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
        if (heldIngredient != null && heldLiquidIngredient != null) {
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
        Ingredient solid = inventory.NameToIngredient[heldIngredient.name];
        if (liquid != null && liquid.infusable && solid.infusion != Ingredient.Infusion.None) {

            liquid.AddInfusion(solid.infusion, heldLiquidIngredient);
            heldIngredient.SetActive(false);
            heldIngredient = null;
            return;
        } else if (liquid != null && liquid.Name.EndsWith("Water") && 
                CheckFlags(solid, Ingredient.FlavourVariable.TeaType) && solid.name.StartsWith("Dried")) {
            liquid.AddTea(solid.teaType, heldLiquidIngredient);
            heldIngredient.SetActive(false);
            heldIngredient = null;
        }


        if (heldIngredient == null) return;
        Ingredient oldIng = inventory.NameToIngredient[heldIngredient.name];
        if (!oldIng.Alterations.ContainsKey(name)) return;
        GameObject prefab = oldIng.Alterations[name].Prefab;
        GameObject newVersion = Instantiate(prefab);
        newVersion.transform.position = heldIngredient.transform.position;
        newVersion.transform.rotation = heldIngredient.transform.rotation;
        newVersion.transform.parent = heldIngredient.transform.parent;
        newVersion.name = prefab.name;
        heldIngredient.SetActive(false);
        heldIngredient = newVersion;

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

        heldIngredient = null;
    }
}

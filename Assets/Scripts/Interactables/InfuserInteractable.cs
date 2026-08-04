using Unity.VisualScripting;
using UnityEngine;

public class InfuserInteractable : Interactable
{
    Rigidbody rb;
    public Vector3 relativeHeldPos;

    public override void Interact(GameObject ingredient)
    {
        Ingredient ing = PlayerInventory.PI.NameToIngredient[ingredient.name];
        Ingredient ogIng = null;
        if (heldIngredient != null)
            ogIng = PlayerInventory.PI.NameToIngredient[heldIngredient.name];

        if (ogIng != null && ogIng.infusable) {
            if (ing.infusable || ing.infusion == Ingredient.Infusion.None)
                Release(heldIngredient);
            else {
                ogIng.AddInfusion(ing.infusion, heldIngredient);
                ingredient.SetActive(false);
            }
            return;
        }
        else if (ogIng != null)
            Release(heldIngredient);
            
        ingredient.GetComponent<DragObj>().holdingInteractable = this;
        heldIngredient = ingredient;


        rb = heldIngredient.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = (RigidbodyConstraints)126; // no rotation

        heldIngredient.transform.position = transform.TransformPoint(relativeHeldPos);
    }

    public override void Release(GameObject ingredient)
    {
        ingredient.GetComponent<DragObj>().holdingInteractable = null;
        rb.useGravity = true;
        rb.constraints = (RigidbodyConstraints)0;

        heldIngredient = null;
    }
}

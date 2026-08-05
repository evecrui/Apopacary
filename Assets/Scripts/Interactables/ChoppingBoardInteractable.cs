using UnityEngine;

public class ChoppingBoardInteractable : Interactable
{

    public override void Interact(GameObject ingredient)
    {
        Ingredient newIng = PlayerInventory.PI.NameToIngredient[ingredient.name];
        if (heldIngredient != null) {
            Ingredient oldIng = PlayerInventory.PI.NameToIngredient[heldIngredient.name];
            if (!oldIng.isLiquid)
                Release(heldIngredient);
            else if (newIng.isLiquid)
                return;
            else if (oldIng.name.EndsWith("Water") && canHoldLiquidAndSolid) {
                heldLiquidIngredient = heldIngredient;
                heldIngredient = ingredient;
            }

        }

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

using UnityEngine;

public class PlayerInteractHold : Interactable
{
    Transform ogParent;

    public override void Interact(GameObject ingredient)
    {
        if (heldIngredient != null)
            Release(heldIngredient);

        PlayerInventory.PI.draggedIngredient = ingredient;
        ingredient.GetComponent<DragObj>().holdingInteractable = this;
        ogParent = ingredient.transform.parent;

        heldIngredient = ingredient;

        rb = heldIngredient.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = (RigidbodyConstraints)126; // no rotation
        heldIngredient.transform.parent = transform.GetChild(0);
        heldIngredient.transform.localPosition = new Vector3(0, 2.75f, 2);
    }

    public override void Release(GameObject ingredient)
    {
        ingredient.GetComponent<DragObj>().holdingInteractable = null;
        rb.useGravity = true;
        rb.constraints = (RigidbodyConstraints)0;
        heldIngredient.transform.parent = ogParent;

        heldIngredient = null;
    }

    public override bool InteractableWithOtherIng()
    {
        return true;
    }
}

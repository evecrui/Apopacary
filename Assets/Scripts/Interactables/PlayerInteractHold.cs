using UnityEngine;

public class PlayerInteractHold : Interactable
{
    Transform ogParent;

    public override void Interact(GameObject ingredient)
    {
        if (interactableHeldIngredient != null)
            Release(interactableHeldIngredient);

        PlayerInventory.PI.draggedIngredient = ingredient;
        ingredient.GetComponent<DragObj>().holdingInteractable = this;
        ogParent = ingredient.transform.parent;

        interactableHeldIngredient = ingredient;

        rb = interactableHeldIngredient.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = (RigidbodyConstraints)126; // no rotation
        interactableHeldIngredient.transform.parent = transform.GetChild(0);
        interactableHeldIngredient.transform.localPosition = new Vector3(0, 2.75f, 2);
    }

    public override void Release(GameObject ingredient)
    {
        ingredient.GetComponent<DragObj>().holdingInteractable = null;
        rb.useGravity = true;
        rb.constraints = (RigidbodyConstraints)0;
        interactableHeldIngredient.transform.parent = ogParent;

        interactableHeldIngredient = null;
    }

    public override bool InteractableWithOtherIng()
    {
        return true;
    }
}

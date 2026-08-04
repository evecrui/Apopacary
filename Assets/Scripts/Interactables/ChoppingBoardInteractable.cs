using UnityEngine;

public class ChoppingBoardInteractable : Interactable
{
    Rigidbody rb;
    Transform ogParent;
    public Vector3 relativeHeldPos;

    public override void Interact(GameObject ingredient)
    {
        if (heldIngredient != null)
            Release(heldIngredient);

        ingredient.GetComponent<DragObj>().holdingInteractable = this;
        ogParent = ingredient.transform.parent;

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

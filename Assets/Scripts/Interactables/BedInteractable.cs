using UnityEngine;

public class BedInteractable : Interactable {
    public Animator anim;
    public override bool InteractableWithOtherIng() {
        return false;
    }

    public override bool InteractableWithHand() {
        if (PlayerInventory.PI.draggedIngredient != null)
            return false;
        return true;
    }

    public override void InteractEmptyHand() {
        if ((anim.GetCurrentAnimatorStateInfo(0).normalizedTime * 300) > 160)
            anim.Rebind();
    }
}

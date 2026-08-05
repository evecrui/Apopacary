using UnityEngine;

public class CowInteractable : Interactable {
    public override bool InteractableWithOtherIng() {
        if (PlayerInventory.PI.draggedIngredient.name == "Bucket") {
            Bucket bucket = PlayerInventory.PI.draggedIngredient.GetComponent<Bucket>();
            if (bucket.empty)
                return true;
        }
        return false;
    }

    public override bool InteractableWithHand() {
        return false;
    }

    public override void Interact(GameObject ingredient) {
        if (ingredient.name == "Bucket") {
            Bucket bucket = ingredient.GetComponent<Bucket>();
            GameObject milk = Instantiate(PlayerInventory.PI.NameToIngredient["CowMilk"].Prefab);
            milk.SetActive(false);
            milk.name = "CowMilk";
            bucket.Fill(milk);
        }
    }
}

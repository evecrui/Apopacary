using UnityEngine;

public class BeesInteractable : Interactable {
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
            GameObject honey = Instantiate(PlayerInventory.PI.NameToIngredient["Honey"].Prefab);
            honey.SetActive(false);
            honey.name = "Honey";
            bucket.Fill(honey);
        }
    }
}

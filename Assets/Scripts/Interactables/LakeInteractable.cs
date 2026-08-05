using UnityEngine;

public class LakeInteractable : Interactable {
    public enum waterType
    {
        Clear, Rain, Moon
    }
    public waterType water;

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
            GameObject waterGO = null;
            if (water == waterType.Clear)
            {
                waterGO = Instantiate(PlayerInventory.PI.NameToIngredient["ClearWater"].Prefab);
                waterGO.name = "ClearWater";
            } else if (water == waterType.Rain) {
                waterGO = Instantiate(PlayerInventory.PI.NameToIngredient["RainWater"].Prefab);
                waterGO.name = "RainWater";
            } else
            {
                waterGO = Instantiate(PlayerInventory.PI.NameToIngredient["MoonWater"].Prefab);
                waterGO.name = "MoonWater";
            }

            waterGO.SetActive(false);
            bucket.Fill(waterGO);
        }
    }
}

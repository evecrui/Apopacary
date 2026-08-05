using UnityEngine;

public class TrashInteractable : Interactable {
    public override void Interact(GameObject ingredient) {
        if (ingredient.name == "Bucket") {
            ingredient.GetComponent<Bucket>().Empty(); 
        } else if (ingredient.name == "Drink") {
            ingredient.GetComponent<Drink>().Empty();
        } else 
            ingredient.SetActive(false);
    }
    public override bool InteractableWithOtherIng() {
        return true;
    }
}

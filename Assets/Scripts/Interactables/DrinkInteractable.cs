using UnityEngine;

public class DrinkInteractable : Interactable
{
    public Drink drink;

    public override void Interact(GameObject ingredient)
    {
        if (ingredient.name == "Bucket")
            ingredient = ingredient.GetComponent<Bucket>().liquidIngredient;
        Ingredient ing = PlayerInventory.PI.NameToIngredient[ingredient.name];
        ingredient.SetActive(false);

        drink.AddIngredient(ing, ingredient);
    }

    public override bool InteractableWithOtherIng() {
        return true;
    }
}

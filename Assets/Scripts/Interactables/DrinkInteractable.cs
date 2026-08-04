using UnityEngine;

public class DrinkInteractable : Interactable
{
    public Vector3 relativeHeldPos;
    public Drink drink;

    public override void Interact(GameObject ingredient)
    {
        Ingredient ing = PlayerInventory.PI.NameToIngredient[ingredient.name];
        ingredient.SetActive(false);

        drink.AddIngredient(ing);
    }
}

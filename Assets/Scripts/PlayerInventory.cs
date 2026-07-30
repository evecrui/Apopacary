using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<Ingredient> Inventory = new List<Ingredient>();
    public Dictionary<Ingredient, int> InventoryIndexes = new Dictionary<Ingredient, int>();
    public Dictionary<Vector2Int, Ingredient> ShelfToIngredient = new Dictionary<Vector2Int, Ingredient>();
    public int shelvesHigh;
    public int shelvesWide;

    private void Start()
    {
        int i = 0;
        foreach (Ingredient ing in Inventory)
        {
            InventoryIndexes.Add(ing, i);
            i++;
        }
    }

    public void AddIngredient(Ingredient ingredient, int amount = 1)
    {
        if (Inventory[InventoryIndexes[ingredient]].AmountHeld == 0)
        {
            Vector2Int emptyShelf = GetNextEmptyShelf();
            // NEED TO ADD PICTURE AND SHELF OBJ PROBLY
            ShelfToIngredient.Add(emptyShelf, ingredient);
            ingredient.SetShelfPos(emptyShelf);
        }
        ingredient.AddAmount(amount);
    }

    private Vector2Int GetNextEmptyShelf()
    {
        for (int i = 0; i < shelvesHigh; i++)
        {
            for(int j = 0; j < shelvesWide; j++)
            {
                if (ShelfToIngredient[new Vector2Int(j, i)] == null)
                    return new Vector2Int(j, i);
            }
        }
        return Vector2Int.zero;
    }
}

[System.Serializable]
public class Ingredient
{
    public string Name;
    public string Description;
    public int AmountHeld;
    private Vector2Int ShelfPos;
    public GameObject Prefab;

    public Ingredient(string name, string description, int amountHeld, Vector2Int shelfPos, GameObject prefab)
    {
        Name = name;
        Description = description;
        AmountHeld = amountHeld;
        ShelfPos = shelfPos;
        Prefab = prefab;
    }

    public void AddAmount(int amount)
    {
        AmountHeld += amount;
    }

    public void SetShelfPos(Vector2Int shelfPos)
        { ShelfPos = shelfPos; }
}

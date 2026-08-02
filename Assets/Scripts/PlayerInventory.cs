using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory PI;
    public List<Ingredient> Inventory = new List<Ingredient>();
    public Dictionary<Vector2Int, Ingredient> ShelfPosToIngredient = new Dictionary<Vector2Int, Ingredient>();
    public Dictionary<string, Ingredient> NameToIngredient = new Dictionary<string, Ingredient>();
    public List<float> xShelfPositions = new List<float>();
    public List<float> yShelfPositions = new List<float>();
    public int shelvesHigh;
    public int shelvesWide;
    public GameObject draggedIngredient;
    public GameObject hoveringShelf;

    private void Start()
    {
        if (PI == null)
            PI = this;
        int i = 0;
        foreach (Ingredient ing in Inventory)
        {
            NameToIngredient.Add(ing.Name, ing);
            i++;
        }
    }

    public void OnInteract(InputAction.CallbackContext context) {
        if (context.started && draggedIngredient != null) {
            if (draggedIngredient.tag == "Ingredient") {
                AddIngredient(draggedIngredient.name);
                StopCoroutine(draggedIngredient.GetComponent<DragObj>().Drag());
                draggedIngredient.SetActive(false);
                draggedIngredient=null;
            }
        } else if (!context.canceled && hoveringShelf != null) {
            Debug.Log(hoveringShelf.name.Replace("Drawer", ""));
            Ingredient hoveringIng = NameToIngredient[hoveringShelf.name.Replace("Drawer", "")];
            GameObject ingObj = Instantiate(hoveringIng.Prefab);
            ingObj.name = hoveringIng.Name;
            hoveringIng.AddAmount(-hoveringIng.BundleSize);
            hoveringIng.Shelf.GetComponentInChildren<TextMeshProUGUI>().text = hoveringIng.AmountHeld.ToString();
            GetComponent<Interactible>().Interact(ingObj);
        }
    }

    public void AddIngredient(Ingredient ingredient, int amount = 1)
    {
        if (ingredient.AmountHeld == 0)
        {
            int i = 0;
            while (i < Inventory.Count && Inventory[i].FoundAny == true) { i++; }
            int j = Inventory.IndexOf(ingredient);
            Ingredient temp = Inventory[i];
            Inventory[i] = ingredient;
            Inventory[j] = temp;
            ingredient.Shelf.SetActive(true);
            ingredient.Shelf.GetComponent<RectTransform>().anchoredPosition = new Vector2(xShelfPositions[i % 3], yShelfPositions[i / 7]);
        }
        ingredient.AddAmount(amount);
        ingredient.Shelf.GetComponentInChildren<TextMeshProUGUI>().text = ingredient.AmountHeld.ToString();
    }

    public void AddIngredient(string name) {
        AddIngredient(NameToIngredient[name], NameToIngredient[name].BundleSize);
    }
}

[System.Serializable]
public class Ingredient
{
    public string Name;
    public string Description;
    public int AmountHeld;
    public int BundleSize;
    private Vector2Int ShelfPos;
    public GameObject Shelf;
    public GameObject Prefab;
    public bool FoundAny;
    public List<GameObject> ObjectsThatCanUseThis;

    public Ingredient(string name, string description, int amountHeld, int bundleSize, Vector2Int shelfPos, GameObject prefab)
    {
        Name = name;
        Description = description;
        AmountHeld = amountHeld;
        BundleSize = bundleSize;
        ShelfPos = shelfPos;
        Prefab = prefab;
        FoundAny = false;
    }

    public void AddAmount(int amount)
    {
        AmountHeld += amount;
    }

    public void SetShelfPos(Vector2Int shelfPos)
        { ShelfPos = shelfPos; }
}

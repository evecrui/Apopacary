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
    public Interactable hoveringInteractable;
    public GameObject DrawersParent;
    private int numIngFound = 0;

    private void Start()
    {
        if (PI == null)
            PI = this;
        int i = 0;
        foreach (Ingredient ing in Inventory)
        {
            ing.PrepIngredient();
            GameObject emptyDrawer = null;
            foreach (Transform drawer in DrawersParent.transform.GetComponentsInChildren<Transform>(true))
            {
                if (drawer.name == "Drawer") {
                    emptyDrawer = drawer.gameObject;
                    break;
                }
            }
            ing.Shelf = emptyDrawer;
            ing.Shelf.name = ing.Name;
            NameToIngredient.Add(ing.Name, ing);
            i++;
        }
    }

    public void OnInteract(InputAction.CallbackContext context) {
        if (context.started && hoveringInteractable != null)
        {
            hoveringInteractable.InteractEmptyHand();
        }
        else if (context.started && draggedIngredient != null) {
            if (draggedIngredient.tag == "Ingredient" && NameToIngredient[draggedIngredient.name].StoredInShelves) {
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
            hoveringIng.AddAmount(-1);
            hoveringIng.Shelf.GetComponentInChildren<TextMeshProUGUI>().text = hoveringIng.AmountHeld.ToString();
            GetComponent<Interactable>().Interact(ingObj);
        }
    }

    public void AddIngredient(Ingredient ingredient, int amount = 1)
    {
        if (!ingredient.FoundAny)
        {
            ingredient.Shelf.SetActive(true);
            ingredient.Shelf.GetComponent<RectTransform>().anchoredPosition = new Vector2(xShelfPositions[numIngFound % 3], yShelfPositions[numIngFound / 7]);
            numIngFound++;
        }
        ingredient.AddAmount(amount);
        ingredient.Shelf.GetComponentInChildren<TextMeshProUGUI>().text = ingredient.AmountHeld.ToString();
    }

    public void AddIngredient(string name) {
        AddIngredient(NameToIngredient[name], 1);
    }
}
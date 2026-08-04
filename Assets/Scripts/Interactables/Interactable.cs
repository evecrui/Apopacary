using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactable : MonoBehaviour
{
    int layermask;
    public GameObject heldIngredient;
    public bool emptyHandInteractable;
    public bool needHoldIngToInteractEmptyHanded;
    public bool highlighted;


    public virtual void Interact(GameObject ingredient) {
        heldIngredient = ingredient;
    }

    public virtual void Release(GameObject ingredient) {
        heldIngredient = null;
    }

    public void Hover() {
        highlighted = true;
        if (gameObject.layer != 3)
            layermask = gameObject.layer;
        gameObject.layer = 3;
        foreach (Transform t in transform.GetComponentsInChildren<Transform>())
        {
            t.gameObject.layer = 3;
        }
    }

    public void UnHover()
    {
        Debug.Log("Unhover " + name);
        highlighted = false;
        gameObject.layer = layermask;
        foreach (Transform t in transform.GetComponentsInChildren<Transform>())
        {
            t.gameObject.layer = layermask;
        }
    }

    public virtual void InteractEmptyHand()
    {
        Debug.Log("Interact with empty hand on " + name + "!!");
        if (heldIngredient)
        {
            PlayerInventory PI = PlayerInventory.PI;
            GameObject prefab = PI.NameToIngredient[heldIngredient.name].Alterations[name].Prefab;
            GameObject newVersion = Instantiate(prefab);
            newVersion.transform.position = heldIngredient.transform.position;
            newVersion.transform.rotation = heldIngredient.transform.rotation;
            newVersion.transform.parent = heldIngredient.transform.parent;
            newVersion.name = prefab.name;
            heldIngredient.SetActive(false);
            heldIngredient = newVersion;

            if (!PI.NameToIngredient[prefab.name].Alterations.ContainsKey(name))
                UnHover();
        }
    }
}

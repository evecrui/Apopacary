using UnityEngine;

public class Interactible : MonoBehaviour
{
    int layermask;
    public GameObject heldIngredient;

    public virtual void Interact(GameObject ingredient) {
        heldIngredient = ingredient;
    }

    public virtual void Release(GameObject ingredient) {
        heldIngredient = null;
    }

    public void Hover()
    {
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
        gameObject.layer = layermask;
        foreach (Transform t in transform.GetComponentsInChildren<Transform>())
        {
            t.gameObject.layer = layermask;
        }
    }
}

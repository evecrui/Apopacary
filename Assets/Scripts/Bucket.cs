using UnityEngine;

public class Bucket : MonoBehaviour
{
    public bool empty;
    public GameObject liquidIngredient;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void Fill(GameObject go) {
        empty = false;
        liquidIngredient = go;
    }

    public GameObject Empty() {
        empty = true;
        GameObject go = liquidIngredient;
        liquidIngredient = null;
        return go;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class GardeningGuide : MonoBehaviour
{
    int currentPage = 0;
    bool bookOpen = false;

    public GameObject book;
    public List<GameObject> pages;

    public void OnBookOpen(InputAction.CallbackContext context) {
        bookOpen = !bookOpen;
        book.SetActive(bookOpen);
    }

    public void NextPage() {
        currentPage++;
        foreach (GameObject page in pages)
            page.SetActive(false);
        pages[currentPage].SetActive(true);
    }

    public void PrevPage() {
        currentPage--;
        foreach (GameObject page in pages)
            page.SetActive(false);
        pages[currentPage].SetActive(true);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

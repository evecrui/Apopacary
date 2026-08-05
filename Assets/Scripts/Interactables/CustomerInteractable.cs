using UnityEngine;

public class CustomerInteractable : Interactable
{
    public bool waiting;
    public bool ordered;
    public GameObject request;
    public override bool InteractableWithOtherIng()
    {
        return false;
    }
    public override bool InteractableWithHand()
    {
        return waiting;
    }

    public override void InteractEmptyHand()
    {
        request.SetActive(true);
    }

    public void DisableRequest()
    {
        request.SetActive(false);
    }
}

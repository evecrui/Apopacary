using UnityEngine;
using UnityEngine.InputSystem;

public class BookInteractor : Interactable
{
    public GardeningGuide gg;
    public override bool InteractableWithHand()
    {
        return true;
    }

    public override bool InteractableWithOtherIng()
    {
        return false;
    }

    public override void InteractEmptyHand()
    {
        gg.OnBookOpen(new InputAction.CallbackContext());
    }
}

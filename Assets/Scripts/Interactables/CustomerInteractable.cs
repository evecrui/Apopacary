using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class CustomerInteractable : Interactable
{
    public bool waiting;
    public bool ordered;
    public GameObject request;

    public override void Interact(GameObject ingredient)
    {
        Debug.Log("Given drink yay :D");
    }

    public override bool InteractableWithOtherIng()
    {
        return ordered && waiting; // if ordered and waiting you can give em
    }
    public override bool InteractableWithHand()
    {
        return waiting && !ordered && !request.activeSelf;
    }

    public override void InteractEmptyHand()
    {
        Regex r = new Regex(@"(?!^)(?=[A-Z])");
        int one = Random.Range(1, 5);
        int two = one - Random.Range(1, 4);
        int three = 9 - one - two;
        string s = "Customer " + _Mult("customer ", one-1);
        Request request = GetComponent<Request>();
        if (request.temperature != Ingredient.Temperature.None)
            s += r.Replace(request.temperature.ToString(), " ") + " ";
        if (request.waterFlav != (Ingredient.Strength.None, Ingredient.TeaType.None))
            s += r.Replace(request.waterFlav.Item1.ToString(), " ") + " " + request.waterFlav.Item2 + " ";
        s += _Mult("customer ", two);
        if (request.ball != Ingredient.Balls.None)
            s += r.Replace(request.ball.ToString(), " ") + " ";
        if (request.water != Ingredient.Waters.None)
            s += r.Replace(request.water.ToString(), " ") + " ";
        if (request.sweetness != Ingredient.Sweetness.None)
            s += r.Replace(request.sweetness.ToString(), " ") + " ";
        if (request.milk != Ingredient.Milks.None)
            s += r.Replace(request.milk.ToString(), " ") + " ";
        s += _Mult("customer ", three);
        if (request.syrup != Ingredient.Syruped.None)
            s += r.Replace(request.syrup.ToString(), " ") + " ";
        this.request.GetComponentsInChildren<TextMeshProUGUI>()[1].text = s;
        this.request.SetActive(true);
    }

    public string _Mult(string line, int times)
    {
        string sb = "";
        for (int i = 0; i < times; i++)
        {
            sb += line;
        }
        return sb;
    }

    public void DisableRequest()
    {
        request.SetActive(false);
    }
}

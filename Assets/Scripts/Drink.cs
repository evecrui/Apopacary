using System.Collections.Generic;
using UnityEngine;

public class Drink : MonoBehaviour {
    Ingredient.Temperature temperature;
    (Ingredient.Strength, Ingredient.TeaType) waterFlav;
    Ingredient.Balls ball;
    Ingredient.Waters water;
    Ingredient.Sweetness sweetness;
    Ingredient.Infusion sweetnessInfusion;
    Ingredient.Milks milk;
    Ingredient.Infusion milkInfusion;
    Ingredient.Syruped syrup;
    public List<(Flavour, Strength)> flavours;
    public bool noLiquid;

    private void Start() {
        flavours = new List<(Flavour, Strength)> ();
        temperature = Ingredient.Temperature.None;
        waterFlav = (Ingredient.Strength.None, Ingredient.TeaType.None);
        ball = Ingredient.Balls.None;
        water = Ingredient.Waters.None;
        sweetness = Ingredient.Sweetness.None;
        sweetnessInfusion = Ingredient.Infusion.None;
        milk = Ingredient.Milks.None;
        milkInfusion = Ingredient.Infusion.None;
        syrup = Ingredient.Syruped.None;
    }

    public enum Flavour {
        Leafy,
        Floral,
        Perfumed,
        Nutty,
        Woody,
        Nettles,
        Celestial,
        Rained,
        Clear,
        Fruity,
        Ginger,
        Milk,
        Honey,
        Nectar,
        Thick,
        Juice,
        Alcohol,
        Caramel
    }

    public enum Strength {
        Standard,
        Weak, Strong, ExtraStrong
    }

    public void AddIngredient(Ingredient ingredient, GameObject ingredientObj) {
        Debug.Log("Adding ing");
        if (CheckFlags(ingredient, Ingredient.FlavourVariable.Perfumed) && !ContainsOfAnyStrength(Flavour.Perfumed))
            flavours.Add((Flavour.Perfumed, Strength.Standard));
        if (CheckFlags(ingredient, Ingredient.FlavourVariable.Alcohol) && !ContainsOfAnyStrength(Flavour.Alcohol))
            flavours.Add((Flavour.Alcohol, Strength.Standard));
        if (ingredient.CheckInfusion(ingredientObj) != Ingredient.Infusion.None) {
            if (!ContainsOfGreaterStrength((Flavour)ingredient.CheckInfusion(ingredientObj), Strength.Weak))
                flavours.Add(((Flavour)ingredient.CheckInfusion(ingredientObj), Strength.Weak));
        }
        if (CheckFlags(ingredient, Ingredient.FlavourVariable.Strength)) {
            bool nuttiness = CheckFlags(ingredient, Ingredient.FlavourVariable.Nuttiness);
            Flavour f = nuttiness ? (Flavour)ingredient.nuttiness : (Flavour)ingredient.teaType;
            if (!ContainsOfGreaterStrength(f, (Strength)ingredient.strength))
                flavours.Add((f, (Strength)ingredient.strength));
        }


        if (CheckFlags(ingredient, Ingredient.FlavourVariable.TeaType)) {
            waterFlav = (ingredient.strength, ingredient.teaType);
        }
        if (CheckFlags(ingredient, Ingredient.FlavourVariable.Balls)) {
            ball = ingredient.balls;
        }
        if (CheckFlags(ingredient, Ingredient.FlavourVariable.Waters)) {
            water = ingredient.waters;
        }
        if (CheckFlags(ingredient, Ingredient.FlavourVariable.Sweetness)) {
            sweetness = ingredient.sweetness;
            if (ingredient.CheckInfusion(ingredientObj) != Ingredient.Infusion.None) {
                sweetnessInfusion = ingredient.CheckInfusion(ingredientObj);
            }
        }
        if (CheckFlags(ingredient, Ingredient.FlavourVariable.Milks)) {
            milk = ingredient.milks;
            Debug.Log("Adding milk");
            if (ingredient.CheckInfusion(ingredientObj) != Ingredient.Infusion.None) {
                milkInfusion = ingredient.CheckInfusion(ingredientObj);
            }
        }
        if (CheckFlags(ingredient, Ingredient.FlavourVariable.Syruped)) {
            syrup = ingredient.syruped;
        }
    }

    private bool CheckFlags(Ingredient ingredient, Ingredient.FlavourVariable flag) {
        return ingredient.relevantFlavourFlag == flag ||
            ingredient.relevantFlavourFlag2 == flag ||
            ingredient.relevantFlavourFlag3 == flag;
    }

    private bool ContainsOfGreaterStrength(Flavour flav, Strength s) {
        foreach (var item in flavours) {
            if (item.Item1 == flav) {
                if (item.Item2 == s || (int)item.Item2 > (int)s)
                    return true;
                else {
                    flavours.Remove(item);
                    return false;
                }
            }
        }
        return false;
    }

    private bool ContainsOfAnyStrength(Flavour flav) {
        foreach (var item in flavours) {
            if (item.Item1 == flav) return true;
        }
        return false;
    }

    public string GetDrinkComponents() {
        string s = "";
        s += "Temperature: " + temperature.ToString();
        s += "\nTea type: " + waterFlav.Item2 + ", " + waterFlav.Item1;
        s += "\nBalls: " + ball;
        s += "\nWaters: " + water;
        s += "\nSweetness: " + sweetness + (sweetnessInfusion == Ingredient.Infusion.None ? "" : " infused with " + sweetnessInfusion);
        s += "\nMilk: " + milk + (milkInfusion == Ingredient.Infusion.None ? "" : " infused with " + milkInfusion);
        s += "\nSyrup: " + syrup;
        return s;
    }

    public void Empty() {
        temperature = Ingredient.Temperature.None;
        waterFlav = (Ingredient.Strength.None, Ingredient.TeaType.None);
        ball = Ingredient.Balls.None;
        water = Ingredient.Waters.None;
        sweetness = Ingredient.Sweetness.None;
        sweetnessInfusion = Ingredient.Infusion.None;
        milk = Ingredient.Milks.None;
        milkInfusion = Ingredient.Infusion.None;
        syrup = Ingredient.Syruped.None;
        flavours = new List<(Flavour, Strength)>();
    }
}
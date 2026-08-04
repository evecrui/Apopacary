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
        Celestial,
        Rained,
        Clear,
        Fruity,
        Ginger,
        Milky,
        Honey,
        Nectar,
        Thick,
        Juicey,
        Alcohol,
        Caramel
    }

    public enum Strength {
        Standard,
        Weak, Strong, ExtraStrong
    }

    public void AddIngredient(Ingredient ingredient) {

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
}
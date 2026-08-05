using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewIngredientData", menuName = "ScriptableObjects/IngredientData", order = 1)]
public class Ingredient : ScriptableObject {
    public string Name;
    public string Description;
    public int AmountHeld;
    private Vector2Int ShelfPos;
    public GameObject Shelf;
    public GameObject Prefab;
    public bool FoundAny;
    public List<string> AlterationKeys;
    public List<Ingredient> AlterationValues;
    public Dictionary<string, Ingredient> Alterations;
    public Infusion infusion;
    public FlavourVariable relevantFlavourFlag;
    public FlavourVariable relevantFlavourFlag2;
    public FlavourVariable relevantFlavourFlag3;
    public Temperature temperature;
    public Strength strength;
    public Sweetness sweetness;
    public TeaType teaType;
    public Nuttiness nuttiness;
    public Syruped syruped;
    public Waters waters;
    public Milks milks;
    public Balls balls;
    public bool alcoholic;
    public bool perfumed;
    public bool infusable;
    private List<Enum> allEnums;
    public List<Enum> preDrinkVars;
    private (Drink.Flavour, Drink.Strength) vars;
    public Dictionary<GameObject, Infusion> infusedCopies;
    public Dictionary<GameObject, TeaType> teadCopies;
    public bool isLiquid = false;

    #region Enums
    public enum FlavourVariable {
        None,
        Temperature,
        Strength,
        Sweetness,
        TeaType,
        Nuttiness,
        Syruped,
        Waters,
        Milks,
        Balls,
        Alcohol,
        Perfumed
    }

    public enum Infusion {
        None,
        Nutty,
        Woody,
        Floral,
        Perfumed,
        Celestial,
        Rain,
        Fruity,
        Ginger,
    }

    public enum Temperature {
        Hot, Cold, Iced, None
    }
    public enum Strength { Weak, Strong, ExtraStrong, None }
    public enum Sweetness { None, HoneySweet, NectarSweet }
    public enum TeaType { Leafy, Floral, Fruity, Ginger, Nettles, Juice, Milk, Milkshake, None }
    public enum Nuttiness { Nutty, Woody }
    public enum Syruped { CaramelSyrup, BerrySyrup, None }
    public enum Waters { Moon, Rain, Clear, None }
    public enum Milks { Cow, Nut, None }
    public enum Balls { None, HokeyPokey, ChoppedLeaves, ChoppedFlowers, ChoppedNuts, ChoppedBerries, ChoppedGinger, ChoppedNettles }
    public enum Alcohol { None, Alcohol };
    public enum Perfumed { None, Perfumed };
    #endregion

    public void PrepIngredient() {
        allEnums = new List<Enum>() { temperature, strength, sweetness, teaType, nuttiness, syruped, waters, milks, balls };
        preDrinkVars = new List<Enum>();
        if (relevantFlavourFlag != FlavourVariable.None) {
            int var1 = (int)relevantFlavourFlag-1;
            preDrinkVars.Add(var1 < 9 ? allEnums[var1] : var1 < 10 ? Alcohol.Alcohol : Perfumed.Perfumed);
        }
        if (relevantFlavourFlag2 != FlavourVariable.None) {
            int var2 = (int)relevantFlavourFlag2-1;
            preDrinkVars.Add(var2 < 9 ? allEnums[var2] : var2 < 10 ? Alcohol.Alcohol : Perfumed.Perfumed);
        }
        if (relevantFlavourFlag3 != FlavourVariable.None) {
            int var3 = (int)relevantFlavourFlag3 - 1;
            preDrinkVars.Add(var3 < 9 ? allEnums[var3] : var3 < 10 ? Alcohol.Alcohol : Perfumed.Perfumed);
        }

        if (infusable) infusion = Infusion.None;
        if (infusedCopies != null)
            infusedCopies.Clear();
        infusedCopies = new Dictionary<GameObject, Infusion>();
        if (teadCopies != null)
            teadCopies.Clear();
        teadCopies = new Dictionary<GameObject, TeaType>();


        AmountHeld = 0;
        FoundAny = false;
        string aKey;
        Ingredient aValue;
        Alterations = new Dictionary<string, Ingredient>();

        for (int index = 0; index < AlterationKeys.Count; index++) {
            aKey = AlterationKeys[index];
            aValue = AlterationValues[index];

            Alterations[aKey] = aValue;
        }        
    }

    public void AddInfusion(Infusion infusion, GameObject ingredient) {
        infusedCopies.Add(ingredient, infusion);
    }

    public Infusion CheckInfusion(GameObject ingredient) {
        if (!infusedCopies.ContainsKey(ingredient))
            return Infusion.None;
        return infusedCopies[ingredient];
    }

    public void AddTea(TeaType type, GameObject ingredient) {
        teadCopies.Add(ingredient, type);
    }

    public TeaType CheckTea(GameObject ingredient) {
        if (!teadCopies.ContainsKey(ingredient))
            return TeaType.None;
        return teadCopies[ingredient];
    }

    public void AddAmount(int amount) {
        if (!FoundAny) FoundAny = true;
        AmountHeld += amount;
    }

    public void SetShelfPos(Vector2Int shelfPos) { ShelfPos = shelfPos; }
}
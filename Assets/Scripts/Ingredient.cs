using System.Collections.Generic;
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
        Hot, Cold, Iced
    }
    public enum Strength { Weak, Strong, ExtraStrong }
    public enum Sweetness { NotSweet, HoneySweet, NectarSweet }
    public enum TeaType { Leafy, Floral, Fruity, Ginger, Nettles, Juice, Milk, Milkshake }
    public enum Nuttiness { Nutty, Woody }
    public enum Syruped { CaramelSyrup, BerrySyrup }
    public enum Waters { Moon, Rain, Clear }
    public enum Milks { Cow, Nut }
    public enum Balls { HokeyPokey, ChoppedLeaves, ChoppedFlowers, ChoppedNuts, ChoppedBerries, ChoppedGinger, ChoppedNettles }
    #endregion

    public void PrepIngredient() {
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

    public void AddAmount(int amount) {
        if (!FoundAny) FoundAny = true;
        AmountHeld += amount;
    }

    public void SetShelfPos(Vector2Int shelfPos) { ShelfPos = shelfPos; }
}

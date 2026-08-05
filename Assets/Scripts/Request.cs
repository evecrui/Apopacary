using System.Collections.Generic;
using UnityEditor.Analytics;
using UnityEngine;
using UnityEngine.AI;
using static Drink;

public class Request : MonoBehaviour
{
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
    public Animator anim;
    public NavMeshAgent nma;
    public Transform shoptopathfindto;
    bool ordered = false;

    public void Start()
    {
        nma.SetDestination(shoptopathfindto.position);
        waterFlav = Random.Range(0, 100) > 70 ? ((Ingredient.Strength)Random.Range(0, 4), Ingredient.TeaType.Leafy) : Random.Range(0, 100) > 70 ? ((Ingredient.Strength)Random.Range(0, 4), Ingredient.TeaType.Floral) : (Ingredient.Strength.None, (Ingredient.TeaType)Random.Range(2, 9));
        if ((int)waterFlav.Item2 <= 5 || (int)waterFlav.Item2 == 8)
        {
            temperature = Random.Range(0, 100) > 50 ? Ingredient.Temperature.None : Random.Range(0, 100) > 50 ? Ingredient.Temperature.Hot : Ingredient.Temperature.Cold;
            int var = Random.Range(0, 5);
            switch (var)
            {
                case 0:
                    ball = (Ingredient.Balls)Random.Range(1, 8);
                    break;
                case 1:
                    water = (Ingredient.Waters)Random.Range(0, 4);
                    break;
                case 2:
                    sweetness = (Ingredient.Sweetness)Random.Range(1, 3);
                    break;
                case 3:
                    milk = (Ingredient.Milks)Random.Range(0, 2);
                    break;
                case 4:
                    syrup = (Ingredient.Syruped)Random.Range(0, 2);
                    break;
                default:
                    break;

            }
        }
    }

    private void Update()
    {
        anim.SetFloat("Speed", nma.speed);
        if (nma.speed < 0.01f && !ordered)
        {
            ordered = true;
        }
    }
}

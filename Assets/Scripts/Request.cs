using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Analytics;
using UnityEngine;
using UnityEngine.AI;
using static Drink;
using static Unity.Burst.Intrinsics.X86;
using static Unity.VisualScripting.Member;

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
    public Transform exittopathfindto;
    public Transform waitingPos;
    bool moving = false;
    bool waiting = false;
    bool ordered = false;
    public AudioSource source;
    public AudioClip bell;

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
        if (nma.velocity.magnitude > 0.05f)
            moving = true;
        anim.SetFloat("Speed", nma.velocity.magnitude);
        if (moving && nma.velocity.magnitude < 0.05f && !waiting)
        {
            Debug.Log("waiting");
            GetComponent<CustomerInteractable>().waiting = true;
            source.PlayOneShot(bell);
            waiting = true;
        }
    }

    public void Accept()
    {
        Debug.Log("Accept");
        waiting = false;
        ordered = true;
        GetComponent<CustomerInteractable>().ordered = true;
        GetComponent<CustomerInteractable>().waiting = false;
        Vector2 randomness = Random.insideUnitCircle * 5f;
        nma.SetDestination(waitingPos.position + new Vector3(randomness.x, 0, randomness.y));
        GetComponent<CustomerInteractable>().DisableRequest();
    }

    public void Deny()
    {
        Debug.Log("Deny");
        waiting = false;
        nma.SetDestination(exittopathfindto.position);
        GetComponent<CustomerInteractable>().DisableRequest();
    }
}

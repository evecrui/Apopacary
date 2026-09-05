using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject witch;
    public int maxWitches = 6;
    public List<NavMeshAgent> waitingAtTillWitches;
    public Transform shoptopathfindto;
    public Transform exittopathfindto;
    public Transform waitingPos;
    public GameObject request;
    public List<NavMeshAgent> witchObjs;
    public float baseStop = 1f;
    public GameObject NPCForDialogue;

    private void Start()
    {
        witchObjs = new List<NavMeshAgent>();
        waitingAtTillWitches = new List<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (NavMeshAgent obj in witchObjs)
        {
            Request r = obj.GetComponent<Request>();
            if (!(r.ordered || r.left) && r.moving && obj.velocity.magnitude < 0.05f)
                if (!waitingAtTillWitches.Contains(obj))
                    waitingAtTillWitches.Add(obj);
        }
        foreach (NavMeshAgent obj in witchObjs)
        {
            Request r = obj.GetComponent<Request>();
            if (!(r.ordered || r.left))
            {
                obj.stoppingDistance = baseStop * (1 + waitingAtTillWitches.Count);
            }
        }

        if (Random.Range(0, 1000f) < 5 && witchObjs.Count < maxWitches)
        {
            GameObject w = Instantiate(witch);
            witchObjs.Add(w.GetComponent<NavMeshAgent>());
            Request r = w.GetComponent<Request>();
            r.cs = this;
            r.shoptopathfindto = shoptopathfindto;
            r.exittopathfindto = exittopathfindto;
            r.waitingPos = waitingPos;
            w.GetComponent<CustomerInteractable>().request = request;
            w.GetComponent<CustomerInteractable>().cs = this;
        }
    }

    public void Accept()
    {
        NPCForDialogue.GetComponent<Request>().Accept();
    }

    public void Deny()
    {
        NPCForDialogue.GetComponent<Request>().Deny();
    }
}

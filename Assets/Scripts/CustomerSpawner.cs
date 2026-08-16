using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject witch;
    public int witches = 0;
    public int maxWitches = 6;
    public Transform shoptopathfindto;
    public Transform exittopathfindto;
    public Transform waitingPos;
    public GameObject request;

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0, 1000f) > 1 && witches < maxWitches)
        {
            witches++;
            GameObject w = Instantiate(witch);
            Request r = w.GetComponent<Request>();
            r.shoptopathfindto = shoptopathfindto;
            r.exittopathfindto = exittopathfindto;
            r.waitingPos = waitingPos;
            w.GetComponent<CustomerInteractable>().request = request;
        }
    }
}

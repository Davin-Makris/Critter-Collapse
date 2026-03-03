using UnityEngine;
using Unity.Netcode;

public class GrowPlant : NetworkBehaviour
{
    // access the objects that we're going to be updating
    [SerializeField] GameObject dirtPlot;
    [SerializeField] GameObject seed;
    [SerializeField] GameObject plant;
    [SerializeField] GameObject rot;

    private WateringCan wateringCan; // the watering can for checks

    // flags
    private bool timerIsRunning = false; // if the timer is running
    private bool plantHarvested = false; // if the plant has been harvested yet

    // timer
    [SerializeField] public float timeUntilRot = 20f; // the time until the plant turns to rot
    [SerializeField] private float timer; // the current timer


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        wateringCan = GameObject.Find("WateringCan").GetComponent<WateringCan>();
        timer = timeUntilRot;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }

    // runs and updates the timer- place this in Update()
    private void runTimer()
    {
        if (!plantHarvested) // if we haven't harvested the plant yet
        {
            bool check = TimePlant(); // update the timer 
            if (check == false) // if the player ran out of time
            {
                PlantRotServerRPC(); // make the plant rot
            }
        }
        else
        {
            timerIsRunning = false; // stop the timer
            timer = timeUntilRot; // reset the timer
        }
    }

    // sent from the server to instruct all clients to update the dirtPlot prefab
    [ServerRpc (InvokePermission = RpcInvokePermission.Everyone)]
    public void PlantSeedServerRPC()
    {
        PlantSeedClientRPC();
    }

    // runs on all clients to update prefabs
    [ClientRpc(InvokePermission = RpcInvokePermission.Everyone)]
    public void PlantSeedClientRPC()
    {
        dirtPlot.SetActive(false);
        seed.SetActive(true);
        plantHarvested = false;
        Debug.Log("Seed planted");
    }

    [ServerRpc (InvokePermission = RpcInvokePermission.Everyone)]
    public void WaterSeedServerRPC()
    {
        WaterSeedClientRPC();
    }

    [ClientRpc(InvokePermission = RpcInvokePermission.Everyone)]
    public void WaterSeedClientRPC()
    {
        // if the player is holding the watering can
        if (wateringCan.holdingCan)
        {
            seed.SetActive(false);
            plant.SetActive(true);
            timerIsRunning = true; // start the timer when a plant is watered
            timer = timeUntilRot; // reset the timer in case it has been changed
            Debug.Log("Seed watered");
        }
        else
        {
            Debug.Log("Cannot water seeds without the watering can");
        }
    }

    [ServerRpc(InvokePermission = RpcInvokePermission.Everyone)]
    public void HarvestPlantServerRPC()
    {
        HarvestPlantClientRPC();
    }


    [ClientRpc(InvokePermission = RpcInvokePermission.Everyone)]
    public void HarvestPlantClientRPC()
    {
        dirtPlot.SetActive(true);
        plant.SetActive(false);
        plantHarvested = true;
        Debug.Log("Plant Harvested");
    }

    [ServerRpc(InvokePermission = RpcInvokePermission.Everyone)]
    public void PlantRotServerRPC()
    {
        PlantRotClientRPC();
    }

    [ClientRpc(InvokePermission = RpcInvokePermission.Everyone)]
    public void PlantRotClientRPC()
    {
        plant.SetActive(false);
        rot.SetActive(true);
        plantHarvested = false;
        Debug.Log("Plant Rotted");
    }

    [ServerRpc(InvokePermission = RpcInvokePermission.Everyone)]
    public void CleanRotServerRPC()
    {
        CleanRotClientRPC();
    }

    [ClientRpc(InvokePermission = RpcInvokePermission.Everyone)]
    public void CleanRotClientRPC()
    {
        rot.SetActive(false);
        dirtPlot.SetActive(true);
        Debug.Log("Rot cleaned up");
    }

    // runs a timer for how long the player has to harvest the plant before it turns into rot
    // returns true if the plant was harvested and false if the player ran out of time
    private bool TimePlant()
    {
        if (timerIsRunning)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime; // decrease timer
            }
            else // otherwise, stop the timer
            {
                Debug.Log("Time is up");
                timer = 0f;
                timerIsRunning = false;
                return false;
            }
        }
        return true;
    }
}

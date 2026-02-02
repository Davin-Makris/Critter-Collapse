using UnityEngine;

public class GrowPlant : MonoBehaviour
{
    // access the objects that we're going to be updating
    [SerializeField] GameObject dirtPlot;
    [SerializeField] GameObject seed;
    [SerializeField] GameObject plant;
    [SerializeField] GameObject rot;

    // flags
    private bool seedWatered = false; // if the seed has been watered
    private bool timerIsRunning = false;
    private bool plantHarvested = false; // if the plant has been harvested yet

    // timer
    [SerializeField] public float timeUntilRot = 20f; // the time until the plant turns to rot
    [SerializeField] private float timer; // the current timer


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = timeUntilRot;
    }

    // Update is called once per frame
    void Update()
    {
        if (!plantHarvested) // if we haven't harvested the plant yet
        {
            bool check = TimePlant(); // update the timer
            if (check == false) // if the player ran out of time
            {
                PlantRot(); // make the plant rot
            }
        }
        else
        {
            timerIsRunning = false; // stop the timer
            timer = timeUntilRot; // reset the timer
        }
    }

    public void PlantSeed()
    {
        dirtPlot.SetActive(false);
        seed.SetActive(true);
        plantHarvested = false;
        Debug.Log("Seed planted");
    }

    public void WaterSeed()
    {
        seed.SetActive(false);
        plant.SetActive(true);
        seedWatered = true;
        timerIsRunning = true; // start the timer when a plant is watered
        timer = timeUntilRot; // reset the timer in case it has been changed
        Debug.Log("Seed watered");
    }

    public void HarvestPlant()
    {
        dirtPlot.SetActive(true);
        plant.SetActive(false);
        plantHarvested = true;
        Debug.Log("Plant Harvested");
    }

    public void PlantRot()
    {
        plant.SetActive(false);
        rot.SetActive(true);
        plantHarvested = false;
        Debug.Log("Plant Rotted");
    }

    public void CleanRot()
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

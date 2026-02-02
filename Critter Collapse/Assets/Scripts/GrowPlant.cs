using UnityEngine;

public class GrowPlant : MonoBehaviour
{
    // access the objects that we're going to be updating
    [SerializeField] GameObject dirtPlot;
    [SerializeField] GameObject seed;
    [SerializeField] GameObject plant;
    [SerializeField] GameObject rot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PlantSeed()
    {
        seed.SetActive(true);
        Debug.Log("Seed planted");
    }

    private void WaterSeed()
    {
        seed.SetActive(false);
        plant.SetActive(true);
        Debug.Log("Seed watered");
    }

    private void HarvestPlant()
    {
        plant.SetActive(false);
        Debug.Log("Plant Harvested");
    }

    private void PlantRot()
    {
        plant.SetActive(false);
        rot.SetActive(true);
        Debug.Log("Plant Rotted");
    }
}

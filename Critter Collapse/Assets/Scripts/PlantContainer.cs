using UnityEngine;

public class PlantContainer : MonoBehaviour
{
    [SerializeField] public int containerWidth = 8;
    [SerializeField] public int containerHeight = 8;
    [SerializeField] public short[,] plantContainerMatrix;

    private void Awake()
    {
        plantContainerMatrix = new short[containerWidth, containerHeight];
        for (int i = 0; i < containerWidth; ++i)
        {
            for (int j = 0; j < containerHeight; ++j)
            {
                plantContainerMatrix[i, j] = 0;
            }
        }
        

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // prints the container when P is pressed- for testing
    // REMOVE ON BUILD
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            printContainer();

        }
    }


    void removePlant(short plantID)
    {
        //set all matrix positions where plantContainerMatrix[i,j] == plantID to 0.
    }

    public void printContainer()
    {
        string toPrint = "\n";
        for (int i = 0; i < containerWidth; i++)
        {
            for (int j = 0; j < containerHeight; j++)
            {
                toPrint += plantContainerMatrix[i, j] + " ";
            }
            toPrint += "END\n";
        }
        Debug.Log(toPrint + " DONE");
    }

    public void goodPrintContainer()
    {
        string toPrint = "\n";
        foreach (short r in plantContainerMatrix)
        {
            toPrint += r + " ";
        }
        Debug.Log(toPrint + " DONE");
    }
}

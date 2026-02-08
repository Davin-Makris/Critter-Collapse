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
        //Debug.Log("" + plantContainerMatrix[0,0] + " " + plantContainerMatrix[1, 0] + " " + plantContainerMatrix[2, 0] + " " + plantContainerMatrix[3, 0] + "\n" +
        //   "" + plantContainerMatrix[0, 1] + " " + plantContainerMatrix[1, 1] + " " + plantContainerMatrix[2, 1] + " " + plantContainerMatrix[3, 1] + "\n" +
        //  "" + plantContainerMatrix[0, 2] + " " + plantContainerMatrix[1, 2] + " " + plantContainerMatrix[2, 2] + " " + plantContainerMatrix[3, 2] + "\n" +
        // "" + plantContainerMatrix[0, 3] + " " + plantContainerMatrix[1, 3] + " " + plantContainerMatrix[2, 3] + " " + plantContainerMatrix[3, 3]);
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

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
            for (int j = 0; j < containerWidth; ++j)
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
        //Debug.Log("" + plantContainerMatrix[0,0] + " " + plantContainerMatrix[1, 0] + " " + plantContainerMatrix[2, 0] + " " + plantContainerMatrix[3, 0] + "\n" +
        //    "" + plantContainerMatrix[0, 1] + " " + plantContainerMatrix[1, 1] + " " + plantContainerMatrix[2, 1] + " " + plantContainerMatrix[3, 1] + "\n" +
        //    "" + plantContainerMatrix[0, 2] + " " + plantContainerMatrix[1, 2] + " " + plantContainerMatrix[2, 2] + " " + plantContainerMatrix[3, 2] + "\n" +
        //    "" + plantContainerMatrix[0, 3] + " " + plantContainerMatrix[1, 3] + " " + plantContainerMatrix[2, 3] + " " + plantContainerMatrix[3, 3]);
    }
    
    void removePlant(short plantID)
    {
        //set all matrix positions where plantContainerMatrix[i,j] == plantID to 0.
    }
}

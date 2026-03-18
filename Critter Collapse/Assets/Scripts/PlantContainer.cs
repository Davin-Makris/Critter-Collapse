using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

public class PlantContainer : NetworkBehaviour
{
    [SerializeField] public int containerWidth = 8;
    [SerializeField] public int containerHeight = 8;
    [SerializeField] public NetworkVariable<short>[,] plantContainerMatrix;
    public NetworkList<ulong> plantsOnGridContainer;


    private void Awake()
    {
        plantContainerMatrix = new NetworkVariable<short>[containerWidth, containerHeight];
        for (int i = 0; i < containerWidth; ++i)
        {
            for (int j = 0; j < containerHeight; ++j)
            {
                plantContainerMatrix[i, j] = new NetworkVariable<short>();
                plantContainerMatrix[i, j].Value = 0;
            }
        }

        plantsOnGridContainer = new NetworkList<ulong>(null, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server); ;
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

    [ServerRpc] //server rpc because only the server can despawn.
    public void clearBoardServerRPC()
    {
        // for every id in plantsOnGridContainer.id
        for (int i = 0; i < plantsOnGridContainer.Count; ++i)
        {
            // use the id to grab the object and despawn
            NetworkManager.Singleton.SpawnManager.SpawnedObjects[plantsOnGridContainer[i]].Despawn();
        }
        plantsOnGridContainer.Clear(); //empty the list
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
        foreach (NetworkVariable<short> r in plantContainerMatrix)
        {
            toPrint += r + " ";
        }
        Debug.Log(toPrint + " DONE");
    }
}

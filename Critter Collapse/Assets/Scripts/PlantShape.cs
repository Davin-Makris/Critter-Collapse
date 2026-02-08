using UnityEngine;
using Unity.Netcode;
public class PlantShape : NetworkBehaviour
{
    public NetworkVariable<short> _GLOBALPLANTID = new NetworkVariable<short>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner); //Plant ID unique identifies plants, and the ID will be how we differentiate the plants in different positions of the container
    [SerializeField] public short plantWidth;
    [SerializeField] public short plantHeight;
    [SerializeField] PlantContainer pc; //Reference to the shipment container
    public short[,] plantMatrix; //our shape 'fingerprint,' where plantMatrix[i,j] == myPlantID denotes a occupied space
    public short myPlantID;

    private NetworkVariable<int> rotations = new NetworkVariable<int>(0);


    // How to incorporate into seeds/interactables:
    // 1. Add a tag to the plantGrow object that will determine what plant it grows into, like 'Cactus' or 'Flower'
    // 2. When the plant goes from seed to full plant, AddComponent<PlantShape>() to the object, or set the component to active. 
    //          --Issue: We need a reference to the plant container to put into the container, may need to findComponent<PlantContainer>() or else we'll null reference
    // 3. Add interactable elements, when we interact with a plant in our hand and we're near the plant shipment box we need to open a menu to determine where to place.
    //          --

    private void Awake()
    {
        myPlantID = _GLOBALPLANTID.Value; //Assign plantID, then increment the class-shared variable to keep it unique
        _GLOBALPLANTID.Value+=1;
    }
    void Start()
    {
        //setFlower(); //test code teehee
        //RotateMatrix();
        //placeInBin(0, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void testPlace()
    {
        placeInBinRPC(0, 0);
    }

    [Rpc(SendTo.Everyone)]
    public void placeInBinRPC(int x, int y) //X and Y should correspond to the top left square we're placing the plant into.
    {
        if (!IsOwner)
        {
            updateRotations();
        }
        if (x >= 0 && y >= 0 && (x + plantWidth) <= pc.containerWidth && (y + plantHeight) <= pc.containerHeight) //if we fit in the container
        {
            if (!doesPlantOverlap(x, y)) //if no part of the space we're inserting into is already taken
            {
                insertPlant(x, y); //add the plant to the container!
            }
        }
    }

    private bool doesPlantOverlap(int x, int y)
    {
        for (int i = 0; i < plantWidth; ++i)
        {
            for (int j = 0; j < plantHeight; ++j)
            {
                //if we take up a space in our matrix, make sure that the container is empty in that spot
                if (plantMatrix[i, j] != 0 && pc.plantContainerMatrix[x + i, y + j] != 0) // CHECK [j, i]?
                    return true;
            }
        }
        return false;
    }

    //assigns the values in the container to the plant ID
    private void insertPlant(int x, int y)
    {
        for (int i = 0; i < plantWidth; ++i)
        {
            for (int j = 0; j < plantHeight; ++j)
            {
                if (plantMatrix[i, j] != 0) //CHECK
                {
                    pc.plantContainerMatrix[x + i, y + j] = myPlantID; //Fill the space
                }

            }
        }
    }

    //mathy tricks i learned in linear algebra, "transposing a matrix then reversing the rows rotates it clockwise"

    public void RotateMatrix(bool clockwise = true) 
    {
        int oldHeight = plantHeight;
        int oldWidth = plantWidth;

        short[,] rotated = new short[oldWidth, oldHeight]; //create a new matrix to rotate

        for (int y = 0; y < oldHeight; y++)
        {
            for (int x = 0; x < oldWidth; x++)
            {
                if (clockwise)
                {
                    rotated[x, oldHeight - 1 - y] = plantMatrix[y, x]; //y,x is the transpose operation, x, oldHeight-1-y is the reverse
                }
                else
                {
                    rotated[oldWidth - 1 - x, y] = plantMatrix[y, x];
                }
            }
        }

        if (clockwise){
            rotations.Value += 1;
        }
        else
        {
            rotations.Value += 3; //3 clockwise rotations = 1 counter clockwise rotation
        }

        rotations.Value %= 4; //0: No rotations, 1: Clockwise once, 2: 180 Degrees, 3: Counter Clockwise

        plantMatrix = rotated; //replace the old matrix 
        plantHeight = (short)plantMatrix.GetLength(0);
        plantWidth = (short)plantMatrix.GetLength(1);
    }

    private void updateRotations()
    {
        for (int i = 0; i < rotations.Value; ++i)
        {
            RotateMatrix(clockwise: true);
        }
        rotations.Value = 0;
    }

    [Rpc(SendTo.Everyone)]
    public void setCactusRPC()
    {
        short[,] cactusMatrix = {
        { myPlantID, 0,         myPlantID, 0,         myPlantID },
        { myPlantID, 0,         myPlantID, 0,         myPlantID },
        { myPlantID, myPlantID, myPlantID, myPlantID, myPlantID },
        { 0,         0,         myPlantID, 0,         0         },
        { 0,         0,         myPlantID, 0,         0         } };

        plantMatrix = cactusMatrix;
        plantHeight = (short)plantMatrix.GetLength(0);
        plantWidth = (short)plantMatrix.GetLength(1);
    }

    [Rpc(SendTo.Everyone)]
    public void setFlowerRPC()
    {
        short[,] flowerMatrix = {
        { 0,         myPlantID, 0         },
        { myPlantID, myPlantID, myPlantID },
        { 0,         myPlantID, 0         },
        { 0,         myPlantID, 0         } };

        plantMatrix = flowerMatrix;
        plantHeight = (short)plantMatrix.GetLength(0);
        plantWidth = (short)plantMatrix.GetLength(1);
    }
}

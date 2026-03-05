using UnityEngine;
using UnityEngine.InputSystem;

public class ShipCrateNavigator : MonoBehaviour
{
    [SerializeField] int containerWidth = 8;
    [SerializeField] int containerHeight = 8;
    private Tile selectedTile;

    bool needInit = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }



    //Ship Crate Movement logic
    public PlantShape heldPlantObject;
    Vector2 plantMatrixLocation = new Vector2(0, 0);
    [SerializeField] GameObject ghostPrefab;
    private GameObject currentGhostedObject;
    PlantContainer plantContainer;

    void self_init() //player is spawned before scene so we need to initialize when needed instead of in start
    {
        GameObject gc = GameObject.Find("GridContainer");

        plantContainer = gc.GetComponent<PlantContainer>(); //reference to the matrix that the plants are managed in

        selectedTile = gc.GetComponent<Transform>().Find("Tile [0, 0]").GetComponent<Tile>(); //reference to the first tile we select
        selectedTile.activateHighlight();

        needInit = false;
    }

    void OnOnMoveSelector(InputValue value)
    {
        Debug.Log("Move Selector!!!!!");
        if (needInit)
        {
            self_init();
        }

        if (!currentGhostedObject)
        {
            currentGhostedObject = Instantiate(ghostPrefab, selectedTile.transform);
            currentGhostedObject.GetComponent<SpriteRenderer>().sprite = heldPlantObject.gameObject.GetComponent<SpriteRenderer>().sprite;
        }

        selectedTile.deactivateHighlight();
        Vector2 direction = value.Get<Vector2>();
        int xChange = 0;
        int yChange = 0;

        //Turn a normalized vector (i.e .7,.7) into a direction we can use to navigate the matrix (like 1,1)
        if (direction.x != 0)
        {
            xChange = direction.x > 0 ? 1 : -1;
        }
        if (direction.y != 0)
        {
            yChange = direction.y > 0 ? 1 : -1;
        }

        if (!(0 <= plantMatrixLocation.x + xChange && plantMatrixLocation.x + xChange < containerWidth))
        {
            xChange = 0; //if the updated location would move us out of bounds, cancel the xChange
        }

        if (!(0 <= plantMatrixLocation.y + yChange && plantMatrixLocation.y + yChange < containerHeight))
        {
            yChange = 0; //same for y
        }

        plantMatrixLocation = new Vector2(plantMatrixLocation.x + xChange, plantMatrixLocation.y + yChange);
        
        if (xChange != 0)
        {selectedTile = xChange > 0 ? selectedTile.eastNeighbor : selectedTile.westNeighbor;}

        if (yChange != 0)
        {selectedTile = yChange > 0 ? selectedTile.northNeighbor : selectedTile.southNeighbor;}

        selectedTile.activateHighlight();
        currentGhostedObject.transform.position = selectedTile.transform.position + new Vector3(-0.5f, -0.5f, -0.1f);
    }

    

    void OnOnExitSelector(InputValue input)
    {
        gameObject.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
    }

    void OnOnRotate(InputValue input)
    {
        heldPlantObject.RotateMatrix();
    }

    void OnOnSelectorInteract(InputValue input)
    {
        Debug.Log("OnOnSelectorInteract");
        heldPlantObject.placeInBinRPC((int)plantMatrixLocation.x, (int)plantMatrixLocation.y);
    }


}

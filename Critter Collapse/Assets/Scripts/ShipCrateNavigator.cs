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


    void OnOnMoveSelector(InputValue value)
    {
        Debug.Log("Move Selector!!!!!");
        if (needInit)
        {

            selectedTile = GameObject.Find("GridContainer").GetComponent<Transform>().Find("Tile [0, 0]").GetComponent<Tile>();
            selectedTile.activateHighlight();
            needInit = false;
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
        {
            //if (!(plantMatrixLocation.x == 0 && xChange == -1))
            //{
            selectedTile = xChange > 0 ? selectedTile.eastNeighbor : selectedTile.westNeighbor;
            //}
        }

        if (yChange != 0)
        {
            //if (!(plantMatrixLocation.y == 0 && yChange == 1))
            //{
                selectedTile = yChange > 0 ? selectedTile.northNeighbor : selectedTile.southNeighbor;
            //}

        }

        selectedTile.activateHighlight();
    }

    void OnOnExitSelector(InputValue input)
    {
        gameObject.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
    }


}

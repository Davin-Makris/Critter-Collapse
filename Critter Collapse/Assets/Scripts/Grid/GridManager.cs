using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int width, height; // width and height (how many tiles to spawn)
    [SerializeField] private Tile tilePrefab; // a ref to the tile prefab
    [SerializeField] private GameObject gridContainer; // the container to put all the tiles in
    [SerializeField] private float targetXPos; // the X postion for the gridContainer (usually -8.4)
    [SerializeField] private float targetYPos; // the Y postion for the gridContainer (usually -4.7)
    private Vector3 targetPos; // the pos for the gridContainer

    private void Start()
    {
        GenerateGrid();
        targetPos = new Vector3(targetXPos, targetYPos, 0f); // set the position
    }

    // generates a grid of tiles by width and height
    void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // spawn the tiles based on width and height
                var spawnedTile = Instantiate(tilePrefab, new Vector3(x + targetXPos, y + targetYPos), Quaternion.identity);
                spawnedTile.transform.SetParent(gridContainer.transform); // put the tiles in the gridContainer to make the editor neat
                spawnedTile.name = $"Tile [{x}, {y}]"; // name the tiles in the editor
               

                // a var to check if the tile is offset (to create a grid pattern)
                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset); // init the tile with the offset check
            }
        }
    }

    
}

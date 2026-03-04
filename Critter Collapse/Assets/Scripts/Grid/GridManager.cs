using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int width, height; // width and height (how many tiles to spawn)
    [SerializeField] private Tile tilePrefab; // a ref to the tile prefab
    [SerializeField] private GameObject gridContainer; // the container to put all the tiles in
    [SerializeField] private float targetXPos; // the X postion for the gridContainer (usually -8.4)
    [SerializeField] private float targetYPos; // the Y postion for the gridContainer (usually -4.7)
    private Vector3 targetPos; // the pos for the gridContainer

    private void Awake()
    {
        GenerateGrid();
        targetPos = new Vector3(targetXPos, targetYPos, 0f); // set the position
    }

    // generates a grid of tiles by width and height
    void GenerateGrid()
    {
        Tile[,] tiles = new Tile[width,height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // spawn the tiles based on width and height
                var spawnedTile = Instantiate(tilePrefab, new Vector3(x + targetXPos, y + targetYPos), Quaternion.identity);
                spawnedTile.transform.SetParent(gridContainer.transform); // put the tiles in the gridContainer to make the editor neat
                spawnedTile.name = $"Tile [{x}, {y}]"; // name the tiles in the editor
                spawnedTile.xPos = x; // set the x pos
                spawnedTile.yPos = y; // set the y pos
               

                // a var to check if the tile is offset (to create a grid pattern)
                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset); // init the tile with the offset check
                tiles[x, y] = spawnedTile;
            }
        }

        //link tiles with all the other tiles
        for (int x = 0; x < width; x++)
        {
            for (int y =0; y < height; y++)
            {
                if (x - 1 >= 0)
                {
                    tiles[x, y].westNeighbor = tiles[x - 1, y];
                }

                if (x + 1 < width)
                {
                    tiles[x, y].eastNeighbor = tiles[x + 1, y];
                }

                if (y - 1 >= 0)
                {
                    tiles[x, y].southNeighbor = tiles[x, y - 1];
                }

                if (y + 1 < height)
                {
                    tiles[x, y].northNeighbor = tiles[x, y + 1];
                }
            }
        }
   
    }

    
}

using UnityEngine;
using System.Collections;

public class RoomGen : MonoBehaviour
{
    // The type of tile that will be laid in a specific position.
    public enum TileType
    {
        Wall, Floor
    }

    // The type of Event that will be laid in a specific position.
    public enum EventType
    {
        None, Enemy, Exit
    }

    //All Fields are Set up in Hierarchy Through RoomGenEditor.cs

    public int columns = 10, rows = 10; //The number of rows and columns for the tiles (How many Tiles)

    public int w_min, w_max; //width
    public int h_min, h_max; //height
    public int e_min, e_max; //enemies

    public RangeAttribute roomWidth, roomHeight, enemyCount;

    public Sprite[] floorTiles;                           // An array of floor tile prefabs.
    public Sprite[] wallTiles;                            // An array of wall tile prefabs.
    public Sprite[] outerWallTiles;                       // An array of outer wall tile prefabs.
    public GameObject[] enemies;                              // An array of the random enemies that can appear
    public GameObject exitSign;
    public GameObject player;
    public CameraScript cam;
    public HexGrid room;

    private TileType[][] tiles;                               // A jagged array of tile types representing the board, like a grid.
    private EventType[][] events;                               // A jagged array of tile types representing the board, like a grid.
    private GameObject boardHolder;                           // GameObject that acts as a container for all other tiles.
    private GameObject eventHolder;
    private GameObject enemyHolder;
    private HexGridBoard gridMap;

    private int width, height; //The saved width and heights on runtime

    private void Start()
    {
        roomWidth = new RangeAttribute(w_min, w_max);
        roomHeight = new RangeAttribute(h_min, h_max);
        enemyCount = new RangeAttribute(e_min, e_max);

        boardHolder = new GameObject("Board Holder");
        gridMap = boardHolder.AddComponent<HexGridBoard>();

        enemyHolder = new GameObject("Enemy Holder");
        eventHolder = new GameObject("Event Holder");

        SetupTilesAndEventsArray();

        CreateRoom();

        SetEventValues();

        SetTileValues();

        InstantiateTiles();

        InstantiateEvents();

        InstantiateOuterWalls();
    }

    /// <summary>
    /// Creates an empty Tile array
    /// </summary>
    void SetupTilesAndEventsArray()
    {
        // Set the tiles jagged array to the correct width.
        tiles = new TileType[columns][];
        events = new EventType[columns][];

        // Go through all the tile arrays...
        for (int i = 0; i < tiles.Length; i++)
        {
            // ... and set each tile array is the correct height.
            tiles[i] = new TileType[rows];
            events[i] = new EventType[rows];
        }
    }

    /// <summary>
    /// Creates and Sets up the Room
    /// </summary>
    void CreateRoom()
    {
        room = ScriptableObject.CreateInstance<HexGrid>();
        room.cells = new System.Collections.Generic.List<Cell>();
        width = (int)Random.Range(roomWidth.min, roomWidth.max);
        height = (int)Random.Range(roomWidth.min, roomHeight.max);
        room.createCellGroup(0, 0, columns, rows);
        gridMap.GenerateMap(room);

        Vector3 playerPos = new Vector3(0, 0, 0);
        GameObject playerInstance = Instantiate(player, playerPos, Quaternion.identity) as GameObject;
        playerInstance.GetComponent<Player>().board = gridMap;
        cam.target = playerInstance.transform;
    }

    void SetTileValues()
    {
        // ... and for each room go through it's width.
        for (int i = 0; i < width; i++)
        {
            int xCoord = i;

            // For each horizontal tile, go up vertically through the room's height.
            for (int j = 0; j < height; j++)
            {
                int yCoord = j;

                // The coordinates in the jagged array are based on the room's position and it's width and height.
                tiles[xCoord][yCoord] = TileType.Floor;
            }
        }
    }

    void SetEventValues()
    {
        int enemiesPlaced = 0;

        int enemies = (int)Random.Range(enemyCount.min, enemyCount.max);

        while (enemiesPlaced < enemies)
        {
            int posx_min = 1;
            int posx_max = posx_min + width - 1;

            int posy_min = 1;
            int posy_max = posy_min + height - 1;

            int posx = Random.Range(posx_min, posx_max);
            int posy = Random.Range(posy_min, posy_max);

            if (events[posx][posy] == EventType.None)
            {
                events[posx][posy] = EventType.Enemy;
                enemiesPlaced++;
            }
        }

        bool exitPlaced = false;
        while (exitPlaced)
        {
            int posx_min = 1 + (int)Mathf.Round(width * (1 - (1 / 6)));
            int posx_max = posx_min + width - 1;

            int posy_min = 1;
            int posy_max = posy_min + height - 1;

            int posx = Random.Range(posx_min, posx_max);
            int posy = Random.Range(posy_min, posy_max);

            if (events[posx][posy] == EventType.None)
            {
                events[posx][posy] = EventType.Exit;
                exitPlaced = true;
            }
        }
    }

    void InstantiateTiles()
    {
        // Go through all the tiles in the jagged array...
        for (int i = 0; i < tiles.Length; i++)
        {
            for (int j = 0; j < tiles[i].Length; j++)
            {
                // ... and instantiate a floor tile for it.
                InstantiateMapCell(floorTiles, i, j);

                // If the tile type is Wall...
                if (tiles[i][j] == TileType.Wall)
                {
                    // ... instantiate a wall over the top.
                    InstantiateMapCell(wallTiles, i, j);
                }
            }
        }
    }

    void InstantiateEvents()
    {
        // Go through all the tiles in the jagged array...
        for (int i = 0; i < events.Length; i++)
        {
            for (int j = 0; j < events[i].Length; j++)
            {
                // If the tile type is Wall...
                if (events[i][j] == EventType.Enemy)
                {
                    InstantiateFromArray(enemies, i, j, enemyHolder);
                }

                // If the tile type is Wall...
                if (events[i][j] == EventType.Exit)
                {
                    GameObject instance = Instantiate(exitSign, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
                    instance.transform.parent = eventHolder.transform;
                }
            }
        }
    }

    void InstantiateOuterWalls()
    {
        // The outer walls are one unit left, right, up and down from the board.
        int leftEdgeX = -1;
        int rightEdgeX = columns;
        int bottomEdgeY = -1;
        int topEdgeY = rows;

        // Instantiate both vertical walls (one on each side).
        InstantiateVerticalOuterWall(leftEdgeX, bottomEdgeY, topEdgeY);
        InstantiateVerticalOuterWall(rightEdgeX, bottomEdgeY, topEdgeY);

        // Instantiate both horizontal walls, these are one in left and right from the outer walls.
        InstantiateHorizontalOuterWall(leftEdgeX + 1, rightEdgeX - 1, bottomEdgeY);
        InstantiateHorizontalOuterWall(leftEdgeX + 1, rightEdgeX - 1, topEdgeY);
    }

    void InstantiateVerticalOuterWall(int xCoord, int startingY, int endingY)
    {
        // Start the loop at the starting value for Y.
        int currentY = startingY;

        // While the value for Y is less than the end value...
        while (currentY <= endingY)
        {
            // ... instantiate an outer wall tile at the x coordinate and the current y coordinate.
            InstantiateMapCell(outerWallTiles, xCoord, currentY);

            currentY++;
        }
    }

    void InstantiateHorizontalOuterWall(int startingX, int endingX, int yCoord)
    {
        // Start the loop at the starting value for X.
        int currentX = startingX;

        // While the value for X is less than the end value...
        while (currentX <= endingX)
        {
            // ... instantiate an outer wall tile at the y coordinate and the current x coordinate.
            InstantiateMapCell(outerWallTiles, currentX, yCoord);

            currentX++;
        }
    }

    /// <summary>
    /// Instantiates a random Prefab from a prefab array
    /// </summary>
    /// <param name="prefabs">The prefab array</param>
    /// <param name="xCoord"></param>
    /// <param name="yCoord"></param>
    /// <param name="parent"></param>
    void InstantiateFromArray(GameObject[] prefabs, float xCoord, float yCoord, GameObject parent)
    {
        // Create a random index for the array.
        int randomIndex = Random.Range(0, prefabs.Length);

        // The position to be instantiated at is based on the coordinates.
        Vector3 position = new Vector3(xCoord, yCoord, 0f);

        // Create an instance of the prefab from the random index of the array.
        GameObject instance = Instantiate(prefabs[randomIndex], position, Quaternion.identity) as GameObject;

        // Set the tile's parent to the board holder.
        instance.transform.parent = parent.transform;
    }

    void InstantiateMapCell(Sprite[] sprites, int xCoord, int yCoord)
    {
        // Create a random index for the array.
        int randomIndex = Random.Range(0, sprites.Length);

        MapCell cell = gridMap.getCellAtPos(xCoord, yCoord);
        cell.GetComponent<SpriteRenderer>().sprite = sprites[randomIndex];
    }
}

using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

    Cell.Direction direction;
    public HexGrid grid;
    int x = 0;
    int y = 0;

    float lastMoved;
    float moveDelay = 0.5f;

    // Use this for initialization
    void Start()
    {
        lastMoved = Mathf.NegativeInfinity;
    }

    // Update is called once per frame
    void Update()
    {
        inputHandling();
        highlightCellNeighbor();
    }

    void inputHandling()
    {
        if (Time.time - lastMoved > moveDelay)
        {
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
            {
                direction = Cell.Direction.NorthEast;
                moveForward();
                lastMoved = Time.time;

            }
            else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
            {
                direction = Cell.Direction.NorthWest;
                moveForward();
                lastMoved = Time.time;
            }
            else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
            {
                direction = Cell.Direction.SouthEast;
                moveForward();
                lastMoved = Time.time;
            }
            else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
            {
                direction = Cell.Direction.SouthWest;
                moveForward();
                lastMoved = Time.time;
            }
            else if (Input.GetKey(KeyCode.W))
            {
                direction = Cell.Direction.North;
                moveForward();
                lastMoved = Time.time;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                direction = Cell.Direction.South;
                moveForward();
                lastMoved = Time.time;
            }
        }
    }

    void highlightCellLine()
    {
        Cell cell = grid.getCellAtPos(x, y);
        //cell.GetComponent<MeshRenderer>().material.color = Color.blue;
        cell.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;

        for (int i = 0; i < 2; i++)
        {
            if (cell.getNeighbor(direction))
                cell = cell.getNeighbor(direction);
            else
                break;

            if (cell != null)
            {
                cell.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                //cell.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
            }
        }
    }

    void highlightCellNeighbor()
    {
        Cell cell = grid.getCellAtPos(x, y);
        //cell.GetComponent<MeshRenderer>().material.color = Color.blue;
        cell.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;

        foreach (Cell surroundingcell in cell.getNeightbors())
        {
            surroundingcell.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
    }

    void highlightCellNuke() {

        Cell cell = grid.getCellAtPos(x, y);
        cell.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;

        for (int i = 0; i < 5; i++)
        {
            if (cell.getNeighbor(direction))
                cell = cell.getNeighbor(direction);
            else
                break;

            if (cell)
            {
                cell.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
            }
        }

        cell.gameObject.GetComponent<SpriteRenderer>().color = Color.red;

        foreach (Cell surroundingcell in cell.getNeightbors())
        {
            surroundingcell.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
    }

    void moveForward()
    {
        Cell cell = grid.getCellAtPos(x, y);
        cell = cell.getNeighbor(direction);
        if (cell != null)
        {
            x = cell.x;
            y = cell.y;
            transform.position = cell.transform.position;
        }
    }
}
using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

    Cell.Direction direction;
    public HexGridBoard board;
    private HexGrid grid;
    int x = 0;
    int y = 0;

    float lastMoved;
    float moveDelay = 0.5f;

    // Use this for initialization
    void Start()
    {
        grid = board.grid;
        lastMoved = Mathf.NegativeInfinity;
    }

    // Update is called once per frame
    void Update()
    {
        inputHandling();
        //highlightCellLine(3);
        highlightCellNeighbor();
        highlightCellRemoteRadial(6);
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

    void highlightCellLine(int dist)
    {
        MapCell cell = board.getCellAtPos(x, y);
        //cell.GetComponent<MeshRenderer>().material.color = Color.blue;
        cell.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;

        for (int i = 0; i < dist; i++)
        {
            if (cell.cellData.getNeighbor(direction) != null)
                cell = board.getCellAtPos(cell.cellData.getNeighbor(direction).x, cell.cellData.getNeighbor(direction).y);
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
        MapCell cell = board.getCellAtPos(x, y);
        //cell.GetComponent<MeshRenderer>().material.color = Color.blue;
        if (cell != null)
        {
            cell.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;

            foreach (Cell surroundingcell in cell.cellData.getNeightbors())
            {
                MapCell obj = board.getCellAtPos(surroundingcell.x, surroundingcell.y);

                if (obj != null)
                {
                    obj.gameObject.GetComponent<SpriteRenderer>().color = Color.magenta;
                }
            }
        }
    }

    void highlightCellRemoteRadial(int dist)
    {

        MapCell cell = board.getCellAtPos(x, y);
        if (cell != null)
        {
            cell.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;

            for (int i = 0; i < dist; i++)
            {
                if (cell.cellData.getNeighbor(direction) != null)
                {
                    Cell neighbor = cell.cellData.getNeighbor(direction);
                    cell = board.getCellAtPos(neighbor.x, neighbor.y);
                }
                else
                    break;

                if (cell != null)
                {
                    cell.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                }
            }

            cell.gameObject.GetComponent<SpriteRenderer>().color = Color.red;

            foreach (Cell surroundingcell in cell.cellData.getNeightbors())
            {
                MapCell obj = board.getCellAtPos(surroundingcell.x, surroundingcell.y);

                if (obj != null)
                {
                    obj.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
                }
            }
        }
    }

    void moveForward()
    {
        MapCell m_cell = board.getCellAtPos(x, y);
        Cell neighbor = m_cell.cellData.getNeighbor(direction);
        if (neighbor != null)
            m_cell = board.getCellAtPos(neighbor.x, neighbor.y);
        else
            return;

        x = m_cell.cellData.x;
        y = m_cell.cellData.y;
        transform.position = m_cell.transform.position;

    }

}
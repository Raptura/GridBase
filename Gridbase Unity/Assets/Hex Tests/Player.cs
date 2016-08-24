using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

    Cell.Direction direction;
    public HexGrid grid;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        inputHandling();
        highlightCell();
    }

    void inputHandling()
    {
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
        {
            direction = Cell.Direction.NorthEast;
        }
        else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
        {
            direction = Cell.Direction.NorthWest;
        }
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            direction = Cell.Direction.SouthEast;
        }
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
        {
            direction = Cell.Direction.SouthWest;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            direction = Cell.Direction.North;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            direction = Cell.Direction.South;
        }
    }

    void highlightCell()
    {
        Cell cell = grid.getCellAtPos(0, 0);
        cell.GetComponent<LineRenderer>().SetColors(Color.blue, Color.blue);

        for (int i = 0; i < 2; i++)
        {
            cell = cell.getNeighbor(direction);
            if (cell != null)
                cell.gameObject.GetComponent<LineRenderer>().SetColors(Color.red, Color.red);

        }
    }
}
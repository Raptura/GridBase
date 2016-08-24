using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Cell : MonoBehaviour
{
    public HexGrid grid;

    public enum Direction
    {
        North,
        NorthEast,
        NorthWest,
        South,
        SouthEast,
        SouthWest,
        East,
        West
    }

    /// <summary>
    /// Gets the x value of the cell
    /// This value is used for the Grid, NOT for absolute position
    /// </summary>
    /// <value>
    /// The x.
    /// </value>
    public int x;

    /// <summary>
    /// Gets the y value of the cell
    /// This value is used for the Grid, NOT for absolute position
    /// </summary>
    /// <value>
    /// The y.
    /// </value>
    public int y;

    /// <summary>
    /// Gets the height of the specified cell.
    /// </summary>
    /// <value>
    /// The height.
    /// </value>
    public float height
    {
        get
        {
            if (grid.hasFlatTop)
            {
                return ((float)Mathf.Sqrt(3)) / 2 * width;
            }
            else
            {
                return grid.cellSize * 2;
            }
        }
    }

    /// <summary>
    /// Gets the width of the specified cell.
    /// </summary>
    /// <value>
    /// The width.
    /// </value>
    public float width
    {
        get
        {
            if (grid.hasFlatTop)
            {
                return grid.cellSize * 2;
            }
            else
            {
                return ((float)Mathf.Sqrt(3)) / 2 * height;
            }
        }
    }

    /// <summary>
    /// Gets the horizontal distance of the specified cell.
    /// </summary>
    /// <value>
    /// The horiz dist.
    /// </value>
    public float horizDist
    {
        get
        {
            if (grid.hasFlatTop)
            {
                return width * 3 / 4;
            }
            else
            {
                return width;
            }
        }

    }

    /// <summary>
    /// Gets the vertical distance of the spefied cell.
    /// </summary>
    /// <value>
    /// The vert dist.
    /// </value>
    public float vertDist
    {
        get
        {
            if (grid.hasFlatTop)
            {
                return height;
            }
            else
            {
                return height * 3 / 4;
            }
        }
    }

    /// <summary>
    /// Gets all Cells in a specified radius.
    /// </summary>
    /// <param name="radius">The radius.</param>
    /// <returns></returns>
    public List<Cell> getAllInRadius(int radius)
    {
        List<Cell> neighbors = new List<Cell>();

        int xBound = radius + Mathf.Abs(this.x);
        int yBound = radius + Mathf.Abs(this.y);


        for (int x = -xBound; x <= xBound; x++)
        {
            for (int y = -yBound; y <= yBound; y++)
            {
                if (x == 0 && y == 0) //that is this current cell
                    continue;

                Cell currentCell = grid.getCellAtPos(x, y);
                if (getDist(this, currentCell) <= radius)
                    neighbors.Add(currentCell);
            }
        }


        return neighbors;
    }

    /// <summary>
    /// Gets all neighbors
    /// </summary>
    /// <returns></returns>
    public List<Cell> getNeightbors()
    {
        return getAllInRadius(1);
    }
    
    /// <summary>
    /// Gets a neighbor cell in a specified direction
    /// </summary>
    /// <param name="dirX">The dir x.</param>
    /// <param name="dirY">The dir y.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">The directions must be within -1 and 1</exception>
    public Cell getNeighbor(int dirX, int dirY)
    {
        if (dirX < -1 || dirX > 1)
            throw new ArgumentException("The directions must be within -1 and 1");

        foreach (Cell cell in getNeightbors())
        {
            if (cell.x == this.x + dirX && cell.y == this.y + dirY)
                return cell;
        }

        return null; //There is no neighbor
    }

    /// <summary>
    /// Gets the neighbor cell in a specified direction.
    /// </summary>
    /// <param name="direction">The direction.</param>
    /// <returns></returns>
    public Cell getNeighbor(Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                if (grid.hasFlatTop)
                    return getNeighbor(0, -1);
                else
                    return null;

            case Direction.NorthEast:
                if (grid.hasFlatTop)
                    return getNeighbor(1, -1);
                else
                    return getNeighbor(1, -1);
            case Direction.NorthWest:
                if (grid.hasFlatTop)
                    return getNeighbor(-1, 0);
                else
                    return getNeighbor(0, -1);

            case Direction.South:
                if (grid.hasFlatTop)
                    return getNeighbor(0, 1);
                else
                    return null;
            case Direction.SouthEast:
                if (grid.hasFlatTop)
                    return getNeighbor(1, 0);
                else
                    return getNeighbor(0, 1);
            case Direction.SouthWest:
                if (grid.hasFlatTop)
                    return getNeighbor(-1, 1);
                else
                    return getNeighbor(-1, 1);

            case Direction.West:
                if (grid.hasFlatTop)
                    return null;
                else
                    return getNeighbor(-1, 0);
            case Direction.East:
                if (grid.hasFlatTop)
                    return null;
                else
                    return getNeighbor(1, 0);

            default:
                return null;

        }
    }

    /// <summary>
    /// Creates the cell.
    /// If cell already exists at position specified, returns the existing cell
    /// </summary>
    /// <param name="grid">The grid.</param>
    /// <param name="x">The x.</param>
    /// <param name="y">The y.</param>
    /// <param name="size">The size.</param>
    /// <returns></returns>
    public static Cell createCell(HexGrid grid, int x, int y, float size)
    {
        Cell cellAtPos = grid.getCellAtPos(x, y);

        if (cellAtPos != null)
        {
            return cellAtPos;
        }
        else
        {
            GameObject cellObj = new GameObject("Cell");
            Cell newCell = cellObj.AddComponent<Cell>();
            cellObj.GetComponent<Cell>().grid = grid;
            cellObj.GetComponent<Cell>().x = x;
            cellObj.GetComponent<Cell>().y = y;

            //cellObj.AddComponent<MeshRenderer>();
            //cellObj.AddComponent<MeshFilter>();

            cellObj.AddComponent<LineRenderer>();
            cellObj.GetComponent<LineRenderer>().material.shader = Shader.Find("Sprites/Default");

            cellObj.transform.SetParent(grid.transform);

            grid.cells.Add(cellObj.GetComponent<Cell>());
            cellObj.GetComponent<Cell>().maintainPos();

            return cellObj.GetComponent<Cell>();
        }
    }

    /// <summary>
    /// Gets the distance between two Cells.
    /// </summary>
    /// <param name="a">Cell a.</param>
    /// <param name="b">Cell b.</param>
    /// <returns></returns>
    public static int getDist(Cell a, Cell b)
    {
        return Mathf.Max(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y));
    }

    /// <summary>
    /// Cell Update 
    /// </summary>
    void CellUpdate()
    {
        maintainPos();
        //drawCellMesh();
        drawCellLine();
    }

    /// <summary>
    /// Maintains position for the cell
    /// </summary>
    public void maintainPos()
    {
        float posX = (float)x * (3f / 2f) * grid.cellSize;
        float posY = (((float)x / 2f) - (y + x)) * Mathf.Sqrt(3) * grid.cellSize;

        transform.localPosition = new Vector2(posX, posY);
    }

    /// <summary>
    /// Draws the Cell using Mesh Filters
    /// (Broken)
    /// </summary>
    /// 
    [Obsolete("This is ONLY for development testing: DO NOT USE", true)]
    public void drawCellMesh()
    {
        Vector3[] verticies = new Vector3[6];
        Vector2[] newUV = new Vector2[6];
        int[] triangles = { 1, 0, 5, 2, 4, 3, 2, 1, 4, 1, 5, 4 };

        for (int i = 0; i < 6; i++)
        {
            float angle = Mathf.Deg2Rad * (60 * i + (grid.hasFlatTop ? 0 : 30));
            float vectX = transform.position.x + grid.cellSize * Mathf.Cos(angle);
            float vectY = transform.position.y + grid.cellSize * Mathf.Sin(angle);

            verticies[i] = new Vector2(vectX, vectY);
        }

        for (int i = 0; i < 6; i++)
        {
            int nextIndex = i + 1;
            if (nextIndex >= 6)
                nextIndex = 0;

            //Debug.DrawLine(verticies[i], verticies[nextIndex]); //For Internal Use only
        }

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.vertices = verticies;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.MarkDynamic();
        mesh.Optimize();
    }

    /// <summary>
    /// Draws the Cell using Line Rendererss
    /// </summary>
    public void drawCellLine()
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.SetVertexCount(7);
        lineRenderer.SetColors(Color.white, Color.white);
        lineRenderer.SetWidth(0.05f, 0.05f);

        for (int i = 0; i < 7; i++)
        {
            float angle = Mathf.Deg2Rad * (60 * i + (grid.hasFlatTop ? 0 : 30));

            float vectX = transform.position.x + grid.cellSize * Mathf.Cos(angle);
            float vectY = transform.position.y + grid.cellSize * Mathf.Sin(angle);
            lineRenderer.SetPosition(i, new Vector2(vectX, vectY));
        }
    }

    public void drawCellDebug()
    {
        Vector3[] verticies = new Vector3[6];

        for (int i = 0; i < 6; i++)
        {
            float angle = Mathf.Deg2Rad * (60 * i + (grid.hasFlatTop ? 0 : 30));
            float vectX = transform.position.x + grid.cellSize * Mathf.Cos(angle);
            float vectY = transform.position.y + grid.cellSize * Mathf.Sin(angle);

            verticies[i] = new Vector2(vectX, vectY);
        }

        for (int i = 0; i < 6; i++)
        {
            int nextIndex = i + 1;
            if (nextIndex >= 6)
                nextIndex = 0;

            Debug.DrawLine(verticies[i], verticies[nextIndex]); //For Internal Use only
        }
    }

    void Update()
    {
        CellUpdate();
    }

}

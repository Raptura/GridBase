using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// A Standard Hexagon Grid
/// </summary>
public class HexGrid : MonoBehaviour
{
    /// <summary>
    /// Gets or sets the size of the cell.
    /// Size is relative to your drawing handler
    /// </summary>
    /// <value>
    /// The size of the cell.
    /// </value>
    public float cellSize = 1.0f;

    /// <summary>
    /// Gets or sets a value indicating whether the cells in this Hex Grid has flat tops.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance has flat top; otherwise, <c>false</c>.
    /// </value>
    public bool hasFlatTop = true;

    public List<Cell> cells = new List<Cell>();

    /// <summary>
    /// Gets the cell at a specific position
    /// </summary>
    /// <param name="x">The x.</param>
    /// <param name="y">The y.</param>
    /// <returns></returns>
    public Cell getCellAtPos(int x, int y)
    {
        foreach (Cell cell in cells)
        {
            if (cell.x == x && cell.y == y)
                return cell;
        }

        return null;
    }

    /// <summary>
    /// Creates a group of cells centered at a specific point.
    /// </summary>
    /// <param name="x">The x.</param>
    /// <param name="y">The y.</param>
    /// <param name="radius">The radius.</param>
    public void createCellGroup(int x, int y, int radius)
    {
        for (int mx = -radius; mx <= radius; mx++)
        {
            for (int my = -radius; my <= radius; my++)
            {
                if (Mathf.Abs(mx + my) <= radius)
                    Cell.createCell(this, mx + x, my + y, cellSize);
            }
        }
    }


}

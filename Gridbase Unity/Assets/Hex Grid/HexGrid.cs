﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// A Standard Hexagon Grid
/// </summary>
[System.Serializable, CreateAssetMenu]
public class HexGrid : ScriptableObject
{

    /// <summary>
    /// Gets or sets the size of the cell.
    /// Size is relative to your drawing handler
    /// </summary>
    /// <value>
    /// The size of the cell.
    /// </value>
    [SerializeField]
    public float cellSize = 1.0f;

    /// <summary>
    /// Gets or sets a value indicating whether the cells in this Hex Grid has flat tops.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance has flat top; otherwise, <c>false</c>.
    /// </value>
    [SerializeField]
    public bool hasFlatTop = true;
    /// <summary>
    /// The list of cells associated with the grid
    /// </summary>
    [SerializeField]
    public List<Cell> cells;

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
    public List<Cell> createCellGroup(int x, int y, int radius)
    {
        List<Cell> created = new List<Cell>();
        for (int mx = -radius; mx <= radius; mx++)
        {
            for (int my = -radius; my <= radius; my++)
            {
                if (Mathf.Abs(mx + my) <= radius)
                {
                    Cell newCell = Cell.createCell(mx + x, my + y);
                    this.cells.Add(newCell);
                    created.Add(newCell);
                }
            }
        }
        return created;

    }

    /// <summary>
    /// Creates a group of cells centered at a specific point.
    /// </summary>
    /// <param name="x">The x.</param>
    /// <param name="y">The y.</param>
    /// <param name="width">The width</param>
    /// <param name="height">The height</param>
    public List<Cell> createCellGroup(int x, int y, int width, int height)
    {
        List<Cell> created = new List<Cell>();
        for (int mx = -width; mx <= width; mx++)
        {
            for (int my = -height; my <= height; my++)
            {
                Cell newCell = Cell.createCell(mx + x, my + y);
                this.cells.Add(newCell);
                created.Add(newCell);
            }
        }
        return created;

    }


    /// <summary>
    /// Links the cells in the cell list to this grid
    /// </summary>
    public void linkCells()
    {
        foreach (Cell cell in cells)
        {
            cell.grid = this;
        }
    }
}

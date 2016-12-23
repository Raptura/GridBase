using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGridBoard : MonoBehaviour
{
    public HexGrid grid;

    /// <summary>
    /// Generates the Map using the Map Cell class
    /// </summary>
    /// <param name="grid">The hex grid that is being generated</param>
    /// <returns>The list of newly created Map Cells</returns>
    public List<MapCell> GenerateMap(HexGrid grid)
    {
        this.grid = grid;
        List<MapCell> cellList = new List<MapCell>();
        foreach (Cell cell in grid.cells)
        {
            MapCell newCell = MapCell.createCell(grid, cell);
            newCell.transform.SetParent(this.transform);
            cellList.Add(newCell);

        }

        return cellList;
    }

    public MapCell getCellAtPos(int x, int y)
    {
        Cell cell = grid.getCellAtPos(x, y);
        if (cell != null)
        {
            foreach (MapCell m_cell in GetComponentsInChildren<MapCell>())
            {
                if (m_cell.cellData.x == cell.x && m_cell.cellData.y == cell.y)
                    return m_cell;
            }
        }
        return null;
    }

}

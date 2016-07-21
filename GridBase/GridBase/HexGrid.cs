using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridBase
{
    public class HexGrid
    {
        int m_cellSides;
        public int cellSides { get; set; }
        public float cellSize { get; set; }
        public bool flatTop = true;

        public List<Cell> cells;

        public static HexGrid createGrid(bool flatTop = true)
        {
            HexGrid newGrid = new HexGrid();
            newGrid.flatTop = flatTop;
            newGrid.cells = new List<Cell>();
            return newGrid;
        }

        public Cell getCellAtPos(int x, int y)
        {
            foreach (Cell cell in cells)
            {
                if (cell.x == x && cell.y == y)
                    return cell;
            }

            return null;
        }


    }
    public class Cell
    {
        public HexGrid grid;

        public int x { get; private set; }
        public int y { get; private set; }

        public float height
        {

            get
            {
                if (grid.flatTop)
                {
                    return ((float)Math.Sqrt(3)) / 2 * width;
                }
                else
                {
                    return grid.cellSize * 2;
                }
            }
        }
        public float width
        {
            get
            {
                if (grid.flatTop)
                {
                    return grid.cellSize * 2;
                }
                else
                {
                    return ((float)Math.Sqrt(3)) / 2 * height;
                }
            }
        }

        public float horizDist
        {
            get
            {
                if (grid.flatTop)
                {
                    return width * 3 / 4;
                }
                else
                {
                    return width;
                }
            }

        }
        public float vertDist
        {
            get
            {
                if (grid.flatTop)
                {
                    return height;
                }
                else
                {
                    return height * 3 / 4;
                }
            }
        }

        public List<Cell> getAllInRadius(int dist)
        {
            List<Cell> neighbors = new List<Cell>();

            for (int i = 0; i < grid.cells.Count; i++)
            {
                for (int x = -dist; x <= dist; x++)
                {
                    for (int y = -dist; y <= dist; y++)
                    {
                        if (x == 0 && y == 0)
                            continue;

                        Cell currentCell = grid.getCellAtPos(x, y);
                        if (getDist(this, currentCell) <= dist)
                            neighbors.Add(currentCell);
                    }
                }
            }

            return neighbors;
        }

        public List<Cell> getNeightbors()
        {
            return getAllInRadius(1);
        }

        public Cell getNeighbor(int dirX, int dirY)
        {

            if (dirX < -1 || dirX > 1)
                throw new ArgumentException("The directions must be within -1 and 1");

            foreach (Cell cell in getNeightbors())
            {
                if (cell.x == this.x + dirX && cell.y == this.y + dirY)
                    return cell;
            }

            return this; //There is no neighbor
        }

        public static Cell createCell(HexGrid grid, int x, int y, float size)
        {
            Cell newCell = new Cell();
            newCell.grid = grid;
            newCell.x = x;
            newCell.y = y;

            return newCell;
        }

        public static int getDist(Cell a, Cell b)
        {
            return Math.Max(Math.Abs(a.x - b.x), Math.Abs(a.y - b.y));
        }
    }


}

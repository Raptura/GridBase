using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridBase
{
    /// <summary>
    /// A Standard Hexagon Grid
    /// </summary>
    public class HexGrid
    {
        int m_cellSides;

        /// <summary>
        /// Gets or sets the cell sides.
        /// </summary>
        /// <value>
        /// The cell sides.
        /// </value>
        public int cellSides { get; set; }

        /// <summary>
        /// Gets or sets the size of the cell.
        /// Size is relative to your drawing handler
        /// </summary>
        /// <value>
        /// The size of the cell.
        /// </value>
        public float cellSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the cells in this Hex Grid has flat tops.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has flat top; otherwise, <c>false</c>.
        /// </value>
        public bool hasFlatTop { get; set; }

        public List<Cell> cells;

        /// <summary>
        /// Creates the grid.
        /// </summary>
        /// <param name="flatTop">if set to <c>true</c> [flat top].</param>
        /// <returns></returns>
        public static HexGrid createGrid(bool flatTop = true)
        {
            HexGrid newGrid = new HexGrid();
            newGrid.hasFlatTop = flatTop;
            newGrid.cells = new List<Cell>();
            return newGrid;
        }

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
            for (int mx = -radius; mx < radius; mx++)
            {
                for (int my = -radius; my < radius; my++)
                {
                    Cell.createCell(this, mx, my, cellSize);
                }
            }
        }

        public class Cell
        {
            private HexGrid grid;

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
            public int x { get; private set; }

            /// <summary>
            /// Gets the y value of the cell
            /// This value is used for the Grid, NOT for absolute position
            /// </summary>
            /// <value>
            /// The y.
            /// </value>
            public int y { get; private set; }

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
                        return ((float)Math.Sqrt(3)) / 2 * width;
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
                        return ((float)Math.Sqrt(3)) / 2 * height;
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

                for (int i = 0; i < grid.cells.Count; i++)
                {
                    for (int x = -radius; x <= radius; x++)
                    {
                        for (int y = -radius; y <= radius; y++)
                        {
                            if (x == 0 && y == 0)
                                continue;

                            Cell currentCell = grid.getCellAtPos(x, y);
                            if (getDist(this, currentCell) <= radius)
                                neighbors.Add(currentCell);
                        }
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

                return this; //There is no neighbor
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
                            return getNeighbor(0, 1);
                        else
                            return getNeighbor(1, 0);
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

                if (cellAtPos == null)
                {
                    Cell newCell = new Cell();
                    newCell.grid = grid;
                    newCell.x = x;
                    newCell.y = y;

                    return newCell;
                }
                else
                {
                    return cellAtPos;
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
                return Math.Max(Math.Abs(a.x - b.x), Math.Abs(a.y - b.y));
            }
        }
    }
}

using System;
using System.Collections.Generic;

namespace MazeSolver
{
    public class AStarMazeSolver : IMazeSolver
    {
        #region Implementation of IMazeSolver

        public List<Cell> Solve(Cell[,] maze, Cell startingCell, Cell endingCell)
        {
            var open = new List<Cell>();
            var closed = new List<Cell>();

            startingCell["g"] = 0;
            startingCell["h"] = EstimateDistance(startingCell, endingCell);
            startingCell["f"] = (int)startingCell["g"] + (int)startingCell["h"];
            open.Add(startingCell);

            while(true)
            {
                open.Sort((c1, c2) => (int)c1["f"] - (int)c2["f"]);

                var current = open[0];
                open.RemoveAt(0);
                closed.Add(current);

                if (current == endingCell) break;

                var neighbour = new [] {current.North, current.East, current.South, current.West};
                foreach (var cell in neighbour)
                {
                    if (cell == null || closed.Contains(cell))
                        continue;
                    
                    if (open.Contains(cell))
                    {
                        if ((int)current["g"] + 1 < (int)cell["g"])
                        {
                            cell["g"] = (int) current["g"] + 1;
                            cell["f"] = (int) cell["g"] + (int)cell["h"];
                            cell["parent"] = current;
                        }
                    }
                    else
                    {
                        cell["g"] = (int)current["g"] + 1;
                        cell["h"] = EstimateDistance(cell, endingCell);
                        cell["f"] = (int)cell["g"] + (int)cell["h"];
                        cell["parent"] = current;

                        open.Add(cell);
                    } 
                }
            }

            var solution = RebuildPath(endingCell);
            solution.Reverse();

            foreach (var cell in maze)
                cell.ClearAdditionalValues();

            return solution;
        }

        private static List<Cell> RebuildPath(Cell endingCell)
        {
            List<Cell> ret = new List<Cell>();
            Cell current = endingCell;

            do
            {
                ret.Add(current);
                current = current["parent"] as Cell;
            } while (current != null);

            return ret;
        }

        #endregion

        private static int EstimateDistance(Cell cell, Cell endingCell)
        {
            return Math.Abs(cell.X - endingCell.X) + Math.Abs(cell.Y - endingCell.Y);
        }
    }
}

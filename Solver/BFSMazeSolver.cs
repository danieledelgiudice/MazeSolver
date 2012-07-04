using System.Collections.Generic;
using MazeSolver.Common;

namespace MazeSolver.Solver
{
    public class BFSMazeSolver : IMazeSolver
    {
        #region Implementation of IMazeSolver

        public List<Cell> Solve(Cell[,] maze, Cell startingCell, Cell endingCell)
        {
            var q = new Queue<Cell>();

            startingCell["parent"] = startingCell;
            q.Enqueue(startingCell);

            while (q.Peek() != endingCell)
            {
                Cell current = q.Dequeue();
                foreach (Cell cell in current.Neighbors)
                    if (cell["parent"] == null)
                    {
                        cell["parent"] = current;
                        q.Enqueue(cell);
                    }
            }

            var solution = RebuildPath(endingCell);

            foreach (var cell in maze)
                cell.ClearAdditionalValues();

            return solution;
        }

        #endregion

        private static List<Cell> RebuildPath(Cell endingCell)
        {
            List<Cell> ret = new List<Cell>();
            Cell current = endingCell;

            while(current != current["parent"])
            {
                ret.Add(current);
                current = current["parent"] as Cell;
            }

            ret.Add(current);

            ret.Reverse();

            return ret;
        }
    }
}

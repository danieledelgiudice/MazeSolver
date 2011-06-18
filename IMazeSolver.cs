using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeSolver
{
    public interface IMazeSolver
    {
        List<Cell> Solve(Cell[,] maze, Cell startingCell, Cell endingCell);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MazeSolver.Common;

namespace MazeSolver
{
    public interface IMazeBuilder
    {
        Cell[,] Build(int width, int height);
    }
}

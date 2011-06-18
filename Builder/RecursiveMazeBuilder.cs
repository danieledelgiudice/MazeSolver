using System;
using System.Collections.Generic;
using MazeSolver.Common;

namespace MazeSolver.Builder
{
    class RecursiveMazeBuilder : IMazeBuilder
    {
        #region Implementation of IMazeBuilder

        public Cell[,] Build(int width, int height)
        {
            var rnd = new Random();
            var maze = new Cell[width, height];
            int x, y;
            int startX = x = rnd.Next(width);
            int startY = y = rnd.Next(height);

            var directions = new List<int>();
            var history = new Stack<Cell>();

            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {
                    maze[i, j] = new Cell(i, j);
                    maze[i, j]["processed"] = false;
                }

            history.Push(maze[x,y]);
            do
            {
                maze[x, y]["processed"] = true;
                directions.Clear();

                if (y - 1 >= 0 && !(bool)maze[x, y - 1]["processed"])
                    directions.Add(0);

                if (x + 1 < width && !(bool)maze[x + 1, y]["processed"])
                    directions.Add(1);

                if (y + 1 < height && !(bool)maze[x, y + 1]["processed"])
                    directions.Add(2);

                if (x - 1 >= 0 && !(bool)maze[x - 1, y]["processed"])
                    directions.Add(3);

                if (directions.Count == 0)
                {
                    var lastCell = history.Pop();
                    x = lastCell.X;
                    y = lastCell.Y;
                    continue;
                }

                switch (directions[rnd.Next(directions.Count)])
                {
                    case 0:
                        maze[x, y].North = maze[x, y - 1];
                        maze[x, y - 1].South = maze[x, y];
                        y -= 1;
                        break;

                    case 1:
                        maze[x, y].East = maze[x + 1, y];
                        maze[x + 1, y].West = maze[x, y];
                        x += 1;
                        break;

                    case 2:
                        maze[x, y].South = maze[x, y + 1];
                        maze[x, y + 1].North = maze[x, y];
                        y += 1;
                        break;

                    case 3:
                        maze[x, y].West = maze[x - 1, y];
                        maze[x - 1, y].East = maze[x, y];
                        x -= 1;
                        break;
                }
                history.Push(maze[x, y]);
            } while (x != startX || y != startY);

            foreach (var cell in maze)
                cell.ClearAdditionalValues();


            return maze;
        }

        #endregion
    }
}

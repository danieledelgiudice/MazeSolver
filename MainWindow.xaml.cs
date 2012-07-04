using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MazeSolver.Builder;
using MazeSolver.Common;
using MazeSolver.Solver;
using System;
using System.ComponentModel;
using System.Windows.Threading;

namespace MazeSolver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private int _height;
        private Cell[,] _maze;
        private int _width;
        private IList<Cell> _solution;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void ExecuteBackground(Action task, Action taskDone)
        {
            solveButton.IsEnabled = false;
            generateButton.IsEnabled = false;
            progressBar.IsIndeterminate = true;

            var worker = new BackgroundWorker();
            worker.DoWork += (o, e) => task();
            worker.RunWorkerCompleted += ((o, e) => Dispatcher.BeginInvoke(new Action(() =>
                                                        {
                                                            taskDone();
                                                            solveButton.IsEnabled = true;
                                                            generateButton.IsEnabled = true;
                                                            progressBar.IsIndeterminate = false;
                                                        }), DispatcherPriority.Render));

            worker.RunWorkerAsync();
        }

        private void GenerateButtonClick(object sender, RoutedEventArgs e)
        {
            _width = _height = (int) slider.Value;
            image.Width = _width*10;
            image.Height = _height*10;

            IMazeBuilder builder = new DFSMazeBuilder();
            ExecuteBackground(() => _maze = builder.Build(_width, _height),
                              DrawMaze);
        }

        private void SolveButtonClick(object sender, RoutedEventArgs e)
        {
            IMazeSolver solver = new BFSMazeSolver();

            Cell startingPoint = _maze[0, 0];
            Cell endingPoint = _maze[_width - 1, _height - 1];
            
            ExecuteBackground(() => _solution = solver.Solve(_maze, startingPoint, endingPoint),
                              () => DrawSolution(_solution));
        }

        private void DrawMaze()
        {
            mazeDrawing.Geometry = null;
            solutionDrawing.Geometry = null;

            double xFactor = image.Width/_width;
            double yFactor = image.Height/_height;

            var g = new StreamGeometry();
            using (StreamGeometryContext context = g.Open())
            {
                for (int i = 0; i < _width; i++)
                    for (int j = 0; j < _height; j++)
                    {
                        if (_maze[i, j].North == null)
                        {
                            context.BeginFigure(new Point(i*xFactor, j*yFactor), false, false);
                            context.LineTo(new Point((i + 1)*xFactor, j*yFactor), true, false);
                        }

                        if (_maze[i, j].East == null)
                        {
                            context.BeginFigure(new Point((i + 1)*xFactor, j*yFactor), false, false);
                            context.LineTo(new Point((i + 1)*xFactor, (j + 1)*yFactor), true, false);
                        }
                    }

                context.BeginFigure(new Point(0, 0), false, false);
                context.LineTo(new Point(0, yFactor*_height), true, false);
                context.LineTo(new Point(xFactor*_width, yFactor*_height), true, false);

            }

            mazeDrawing.Geometry = g;

            if (saveImagesCheckbox.IsChecked.Value)
            {
                SaveImage("maze.png");
                MessageBox.Show("Maze saved in maze.png");
            }
            
        }

        private void DrawSolution(IList<Cell> solution)
        {
            solutionDrawing.Geometry = null;

            double xFactor = image.Width/_width;
            double yFactor = image.Height/_height;

            var g = new StreamGeometry();
            using (StreamGeometryContext context = g.Open())
            {
                context.BeginFigure(new Point((solution[0].X + 0.5)*xFactor, (solution[0].Y + 0.5)*yFactor), false,
                                    false);
                for (int i = 1; i < solution.Count; i++)
                {
                    context.LineTo(new Point((solution[i].X + 0.5)*xFactor, (solution[i].Y + 0.5)*yFactor), true, false);
                }
            }

            solutionDrawing.Geometry = g;

            if (saveImagesCheckbox.IsChecked.Value)
            {
                SaveImage("solution.png");
                MessageBox.Show("Solution saved in solution.png");
            }
            
        }

        private void SaveImage(string filename)
        {
            RenderTargetBitmap bmp = new RenderTargetBitmap(_width * 10, _height * 10, 96, 96, PixelFormats.Default);
            image.Measure(new Size(_width * 10, _height * 10));

            bmp.Render(image);
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                encoder.Save(fs);
            }
        }
    }
}
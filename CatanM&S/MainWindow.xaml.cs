using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using CatanM_S.Models;

namespace CatanM_S
{
    public partial class MainWindow : Window
    {
        private Game _game;
        private StrategySimulator _simulator;
        private const double HexSize = 40;  // Ajusta el tamaño del hexágono

        public MainWindow()
        {
            InitializeComponent();
            _game = new Game();
            _simulator = new StrategySimulator(_game);
            DrawBoard();
        }

        private void StartSimulation_Click(object sender, RoutedEventArgs e)
        {
            _simulator.RunSimulation();
            UpdateUI();
        }

<<<<<<< Updated upstream
        private void DrawBoard()
=======
        private void Exit_Click(object sender, RoutedEventArgs e)
{
    this.Close();
}


        private void InitializePlayers()
        {
            // Initialize players only once
            players.Clear();
            for (int i = 0; i < 4; i++)
            {
                players.Add(new Player());
            }
        }

        private void ResetPlayersResources()
        {
            // Reset players' resources between simulations, but not their wins
            foreach (var player in players)
            {
                player.Resources[ResourceType.Wood] = 0;
                player.Resources[ResourceType.Brick] = 0;
                player.Resources[ResourceType.Sheep] = 0;
                player.Resources[ResourceType.Wheat] = 0;
                player.Resources[ResourceType.Ore] = 0;
                player.Houses.Clear();
            }
        }

        private void ResetGame(out Game game, out StrategySimulator simulator)
        {
            ResetPlayersResources();
            game = new Game();
            simulator = new StrategySimulator(game);
            players = game.Players; // Update players list
            DrawBoard(game);
        }

        private void DrawBoard(Game game)
>>>>>>> Stashed changes
        {
            BoardCanvas.Children.Clear();
            foreach (var tile in _game.Board.Tiles)
            {
                var (x, y) = Hexagon.HexToPixel(tile.Q, tile.R, HexSize);
                DrawHexagon(x + BoardCanvas.Width / 2, y + BoardCanvas.Height / 2, HexSize, tile.Resource, tile.Number);
            }

            for (int i = 0; i < _game.Players.Count; i++)
            {
                var player = _game.Players[i];
                foreach (var house in player.Houses)
                {
                    var (x, y) = Hexagon.IntersectionToPixel(house.Q, house.R, HexSize);
                    DrawHouse(x + BoardCanvas.Width / 2, (y + BoardCanvas.Height / 2) + 20, GetPlayerColor(i));
                }
            }
        }

        private void DrawHexagon(double x, double y, double size, ResourceType resource, int number)
        {
            Polygon hex = new Polygon
            {
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                Fill = GetResourceBrush(resource),
                Points = new PointCollection
                {
                    new Point(x + size * Math.Cos(0), y + size * Math.Sin(0)),
                    new Point(x + size * Math.Cos(Math.PI / 3), y + size * Math.Sin(Math.PI / 3)),
                    new Point(x + size * Math.Cos(2 * Math.PI / 3), y + size * Math.Sin(2 * Math.PI / 3)),
                    new Point(x + size * Math.Cos(Math.PI), y + size * Math.Sin(Math.PI)),
                    new Point(x + size * Math.Cos(4 * Math.PI / 3), y + size * Math.Sin(4 * Math.PI / 3)),
                    new Point(x + size * Math.Cos(5 * Math.PI / 3), y + size * Math.Sin(5 * Math.PI / 3))
                }
            };
            BoardCanvas.Children.Add(hex);

            // Añadir el número en el centro del hexágono
            if (number != 0)
            {
                TextBlock numberText = new TextBlock
                {
                    Text = number.ToString(),
                    Foreground = Brushes.Black,
                    FontWeight = FontWeights.Bold,
                    FontSize = 14
                };
                Canvas.SetLeft(numberText, x - 10);
                Canvas.SetTop(numberText, y - 10);
                BoardCanvas.Children.Add(numberText);
            }
        }

        private void DrawHouse(double x, double y, Brush color)
        {
<<<<<<< Updated upstream
            Ellipse house = new Ellipse
=======
            // Set the size of the house
            double houseWidth = 10;
            double houseHeight = 7.5;
            double roofHeight = 10;

            // Create the body of the house
            Rectangle houseBody = new Rectangle
>>>>>>> Stashed changes
            {
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Fill = color,
                Width = houseWidth,
                Height = houseHeight
            };
<<<<<<< Updated upstream
            Canvas.SetLeft(house, x - 5);  // Centrar la casa
            Canvas.SetTop(house, y - 5);   // Centrar la casa
            BoardCanvas.Children.Add(house);
=======
            Canvas.SetLeft(houseBody, x - houseWidth / 2); // Center the house body
            Canvas.SetTop(houseBody, y - houseHeight / 2); // Center the house body vertically

            // Create the roof of the house
            Polygon roof = new Polygon
            {
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Fill = color, // Fill the roof with the same color as the house body
                Points = new PointCollection
        {
            new Point(x - houseWidth / 2, y - houseHeight / 2), // Left corner of the roof
            new Point(x + houseWidth / 2, y - houseHeight / 2), // Right corner of the roof
            new Point(x, y - houseHeight / 2 - roofHeight)      // Top of the roof
>>>>>>> Stashed changes
        }
            };

            // Add the house body and roof to the canvas
            BoardCanvas.Children.Add(houseBody);
            BoardCanvas.Children.Add(roof);
        }


        private Brush GetResourceBrush(ResourceType resource)
        {
            return resource switch
            {
                ResourceType.Wood => Brushes.ForestGreen,
                ResourceType.Brick => Brushes.IndianRed,
                ResourceType.Sheep => Brushes.LightGreen,
                ResourceType.Wheat => Brushes.Gold,
                ResourceType.Ore => Brushes.Gray,
                ResourceType.Desert => Brushes.SandyBrown,
                _ => Brushes.White,
            };
        }

        private Brush GetPlayerColor(int playerIndex)
        {
            return playerIndex switch
            {
                0 => Brushes.Red,
                1 => Brushes.Blue,
                2 => Brushes.Green,
                3 => Brushes.Yellow,
                _ => Brushes.Black,
            };
        }

        private string GetPlayerColorText(int playerIndex)
        {
            return playerIndex switch
            {
                0 => "Red",
                1 => "Blue",
                2 => "Green",
                3 => "Yellow",
                _ => "",
            };
        }

        private string GetPlayerStrategy(int playerIndex)
        {
            return playerIndex switch
            {
                0 => "Random",
                1 => "Best Number Probability",
                2 => "Best Resources",
                3 => "Best Resources and Number",
                _ => "Unknown",
            };
        }

        private void UpdateUI()
        {
            DrawBoard();
            UpdateResourceCounts();
            UpdateDiceResults();
        }

        private void UpdateResourceCounts()
        {
            ResultTextBlock.Text = "";
            for (int i = 0; i < _game.Players.Count; i++)
            {
                var player = _game.Players[i];
                var playerColor = GetPlayerColorText(i).ToString();
                var playerStrategy = GetPlayerStrategy(i);
                ResultTextBlock.Text += $"({playerStrategy}, {playerColor}): " +
                    $"Wood: {player.Resources[ResourceType.Wood]}, " +
                    $"Brick: {player.Resources[ResourceType.Brick]}, " +
                    $"Sheep: {player.Resources[ResourceType.Sheep]}, " +
                    $"Wheat: {player.Resources[ResourceType.Wheat]}, " +
                    $"Ore: {player.Resources[ResourceType.Ore]}\n";
            }
        }

        private void UpdateDiceResults()
        {
<<<<<<< Updated upstream
=======
            // Calculate the score based on the player's resources
            return player.Resources[ResourceType.Wood] * 10 +
                   player.Resources[ResourceType.Brick] * 10 +
                   player.Resources[ResourceType.Wheat] * 10 +
                   player.Resources[ResourceType.Ore] * 8 +
                   player.Resources[ResourceType.Sheep] * 6;
        }

        private void ShowTotalWins()
        {
            TotalTextBox.Text = "\nTotal Wins:\n";
            int maxWins = int.MinValue;
            string overallWinner = "";

            // Display the total wins for each player
            for (int i = 0; i < players.Count; i++)
            {
                var player = players[i];
                var playerColor = GetPlayerColorText(i).ToString();
                var playerStrategy = GetPlayerStrategy(i);
                TotalTextBox.Text += $"{playerColor} ({playerStrategy}) Wins: {player.Wins}\n";

                if (player.Wins > maxWins)
                {
                    maxWins = player.Wins;
                    overallWinner = $"{playerColor} ({playerStrategy})";
                }
            }

            TotalTextBox.Text += $"Overall Winner: {overallWinner} with {maxWins} wins.\n";
        }

        private void UpdateDiceResults(Game game)
        {
            // Display the dice roll results
>>>>>>> Stashed changes
            DiceResultsListBox.Items.Clear();
            foreach (var result in _game.DiceResults)
            {
                DiceResultsListBox.Items.Add($"Dice Roll: {result}");
            }
        }
    }

    public static class Hexagon
    {
        public static (double x, double y) HexToPixel(int q, int r, double size)
        {
            double x = size * (3.0 / 2 * q);
            double y = size * (Math.Sqrt(3) / 2 * q + Math.Sqrt(3) * r);
            return (x, y);
        }

        public static (double x, double y) IntersectionToPixel(int q, int r, double size)
        {
            // Calcular las posiciones de las intersecciones
            double x = size * (3.0 / 2 * q);
            double y = size * (Math.Sqrt(3) / 2 * q + Math.Sqrt(3) * r);
            return (x, y);
        }
    }
}

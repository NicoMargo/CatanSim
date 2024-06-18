using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Threading.Tasks;
using System.IO;
using CatanM_S.Models;
using System.Linq;

namespace CatanM_S
{
    public partial class MainWindow : Window
    {
        private const double HexSize = 40;  // Adjust the size of the hexagon
        private List<Player> players;       // List of players
        private string ResultsDirectoryPath;
        private string ResultsFilePath;

        public MainWindow()
        {
            InitializeComponent();
            players = new List<Player>();
            ResultsDirectoryPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "Results");
            if (!Directory.Exists(ResultsDirectoryPath))
            {
                Directory.CreateDirectory(ResultsDirectoryPath);
            }
            ResultsFilePath = System.IO.Path.Combine(ResultsDirectoryPath, "results.txt");
            InitializePlayers();
            LoadPlayerWins();
            ResetGame(out Game game, out StrategySimulator simulator);
            DrawBoard(game);
        }

        private async void StartSimulation_Click(object sender, RoutedEventArgs e)
        {
            // Execute 10 Monte Carlo simulations
            ResultTextBlock.Text = "";
            for (int i = 0; i < 2000; i++)
            {
                ResetGame(out Game game, out StrategySimulator simulator);
                simulator.RunSimulation();
                UpdateUI(game);
                SaveResults(); // Save results after each simulation
            }

            // Display total wins
            ShowTotalWins();
        }

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
        {
            BoardCanvas.Children.Clear();
            foreach (var tile in game.Board.Tiles)
            {
                var (x, y) = Hexagon.HexToPixel(tile.Q, tile.R, HexSize);
                DrawHexagon(x + BoardCanvas.Width / 2, y + BoardCanvas.Height / 2, HexSize, tile.Resource, tile.Number);
            }

            // Draw houses for each player
            for (int i = 0; i < game.Players.Count; i++)
            {
                var player = game.Players[i];
                foreach (var house in player.Houses)
                {
                    var (x, y) = Hexagon.IntersectionToPixel(house.Q, house.R, HexSize);
                    DrawHouse(x + BoardCanvas.Width / 2, (y + BoardCanvas.Height / 2) + 15, GetPlayerColor(i));
                }
            }
        }

        private void DrawHexagon(double x, double y, double size, ResourceType resource, int number)
        {
            // Draw a hexagon with the given resource type and number
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

            // Add the number in the center of the hexagon
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
            // Set the size of the house
            double houseWidth = 7.5;
            double houseHeight = 5;
            double roofHeight = 7.5;

            // Create the body of the house
            Rectangle houseBody = new Rectangle
            {
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Fill = color,
                Width = houseWidth,
                Height = houseHeight
            };
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
        }
            };

            // Add the house body and roof to the canvas
            BoardCanvas.Children.Add(houseBody);
            BoardCanvas.Children.Add(roof);
        }


        private Brush GetResourceBrush(ResourceType resource)
        {
            // Get the brush color corresponding to the resource type
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
            // Get the color corresponding to the player index
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
            // Get the text representation of the player's color
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
            // Get the text representation of the player's strategy
            return playerIndex switch
            {
                0 => "Random",
                1 => "Best Number Probability",
                2 => "Best Resources",
                3 => "Best Resources and Number",
                _ => "Unknown",
            };
        }

        private void UpdateUI(Game game)
        {
            // Update the UI with the latest game state
            DrawBoard(game);
            UpdateResourceCounts(game);
            UpdateDiceResults(game);
        }

        private void UpdateResourceCounts(Game game)
        {
            ResultTextBlock.Text += "\nSimulation Result:\n";
            int maxScore = int.MinValue;
            string winner = "";
            int winnerIndex = -1;

            // Calculate and display the score for each player
            for (int i = 0; i < game.Players.Count; i++)
            {
                var player = game.Players[i];
                var playerColor = GetPlayerColorText(i).ToString();
                var playerStrategy = GetPlayerStrategy(i);
                int score = CalculateScore(player);
                ResultTextBlock.Text += $"({playerStrategy}, {playerColor}): " +
                    $"Wood: {player.Resources[ResourceType.Wood]}, " +
                    $"Brick: {player.Resources[ResourceType.Brick]}, " +
                    $"Sheep: {player.Resources[ResourceType.Sheep]}, " +
                    $"Wheat: {player.Resources[ResourceType.Wheat]}, " +
                    $"Ore: {player.Resources[ResourceType.Ore]}, " +
                    $"Score: {score}\n";
                if (score > maxScore)
                {
                    maxScore = score;
                    winner = $"Player {i + 1} ({playerColor})";
                    winnerIndex = i;
                }
            }

            // Update the winner's win count
            if (winnerIndex != -1)
            {
                game.Players[winnerIndex].Wins++;
            }

            ResultTextBlock.Text += $"Winner: {winner} with {maxScore} points.\n";
        }

        private int CalculateScore(Player player)
        {
            // Calculate the score based on the player's resources
            return player.Resources[ResourceType.Wood] * 10 +
                   player.Resources[ResourceType.Brick] * 10 +
                   player.Resources[ResourceType.Wheat] * 10 +
                   player.Resources[ResourceType.Ore] * 8 +
                   player.Resources[ResourceType.Sheep] * 6;
        }

        private void ShowTotalWins()
        {
            if (!File.Exists(ResultsFilePath))
            {
                TotalTextBox.Text = "Results file not found.";
                return;
            }

            string[] lines = File.ReadAllLines(ResultsFilePath);
            TotalTextBox.Text = "\nTotal Wins:\n";
            int maxWins = int.MinValue;
            string overallWinner = "";
            Dictionary<string, int> playerWins = new Dictionary<string, int>();

            foreach (var line in lines)
            {
                var parts = line.Split(':');
                if (parts.Length == 2)
                {
                    string playerInfo = parts[0].Trim();
                    int wins = int.Parse(parts[1].Trim());

                    playerWins[playerInfo] = wins;
                    TotalTextBox.Text += $"{playerInfo} Wins: {wins}\n";

                    if (wins > maxWins)
                    {
                        maxWins = wins;
                        overallWinner = playerInfo;
                    }
                }
            }

            TotalTextBox.Text += $"Overall Winner: {overallWinner} with {maxWins} wins.\n";
        }

        private void UpdateDiceResults(Game game)
        {
            // Display the dice roll results
            DiceResultsListBox.Items.Clear();
            foreach (var result in game.DiceResults)
            {
                DiceResultsListBox.Items.Add($"Dice Roll: {result}");
            }
        }

        private void SaveResults()
        {
            // Read previous results from the file
            var previousResults = new Dictionary<string, int>();
            if (File.Exists(ResultsFilePath))
            {
                using (StreamReader reader = new StreamReader(ResultsFilePath))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var parts = line.Split(':');
                        if (parts.Length == 2 && int.TryParse(parts[1], out int wins))
                        {
                            previousResults[parts[0]] = wins;
                        }
                    }
                }
            }

            // Update the results with the new wins
            foreach (var player in players)
            {
                var key = $"{GetPlayerColorText(players.IndexOf(player))} ({GetPlayerStrategy(players.IndexOf(player))})";
                if (previousResults.ContainsKey(key))
                {
                    previousResults[key] += player.Wins;
                }
                else
                {
                    previousResults[key] = player.Wins;
                }
            }

            // Write the updated results back to the file
            using (StreamWriter writer = new StreamWriter(ResultsFilePath))
            {
                foreach (var entry in previousResults)
                {
                    writer.WriteLine($"{entry.Key}: {entry.Value}");
                }
            }
        }

        private void LoadPlayerWins()
        {
            // Load the win counts from the file
            if (File.Exists(ResultsFilePath))
            {
                using (StreamReader reader = new StreamReader(ResultsFilePath))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var parts = line.Split(':');
                        if (parts.Length == 2 && int.TryParse(parts[1], out int wins))
                        {
                            for (int i = 0; i < players.Count; i++)
                            {
                                var key = $"{GetPlayerColorText(i)} ({GetPlayerStrategy(i)})";
                                if (key == parts[0])
                                {
                                    players[i].Wins = wins;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    public static class Hexagon
    {
        // Convert hexagon coordinates to pixel coordinates
        public static (double x, double y) HexToPixel(int q, int r, double size)
        {
            double x = size * (3.0 / 2 * q);
            double y = size * (Math.Sqrt(3) / 2 * q + Math.Sqrt(3) * r);
            return (x, y);
        }

        // Convert intersection coordinates to pixel coordinates
        public static (double x, double y) IntersectionToPixel(int q, int r, double size)
        {
            double x = size * (3.0 / 2 * q);
            double y = size * (Math.Sqrt(3) / 2 * q + Math.Sqrt(3) * r);
            return (x, y);
        }
    }
}

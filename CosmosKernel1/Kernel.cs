using System;
using Sys = Cosmos.System;
using System.Collections.Generic;
using Cosmos.System.Graphics;
using System.Drawing;
using System.Linq;

namespace TLSPOS
{
    public class Kernel : Sys.Kernel
    {
        private Dictionary<string, string> virtualFileSystem = new Dictionary<string, string>();
        private string currentDirectory = "root"; // Initial root folder

        protected override void BeforeRun()
        {


            Console.Clear();
            AnimateBoot();
            Console.Clear();
            Console.WriteLine("######## ##         ####     ######  ########   #######   ######");
            Console.WriteLine("   ##    ##        ##  ##   ##    ## ##     ## ##     ## ##    ##");
            Console.WriteLine("   ##    ##         ####    ##       ##     ## ##     ## ##");
            Console.WriteLine("   ##    ##        ####      ######  ########  ##     ##  ######");
            Console.WriteLine("   ##    ##       ##  ## ##       ## ##        ##     ##       ##");
            Console.WriteLine("   ##    ##       ##   ##   ##    ## ##        ##     ## ##    ##");
            Console.WriteLine("   ##    ########  ####  ##  ######  ##         #######   ######");

            Console.WriteLine("Welcome to TL&SPOS!");
            Console.WriteLine("Type 'help' for a list of commands.");
        }

        protected override void Run()
        {
            var originalColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"{currentDirectory}@TLSPOS> ");
            Console.ForegroundColor = originalColor;

            string input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input))
            {
                ProcessCommand(input);
            }
        }

        private void ProcessCommand(string input)
        {
            string command;
            string arguments = null;

            int spaceIndex = input.IndexOf(' ');
            if (spaceIndex > -1)
            {
                command = input.Substring(0, spaceIndex).ToLower();
                arguments = input.Substring(spaceIndex + 1).Trim();
            }
            else
            {
                command = input.ToLower();
            }

            switch (command)
            {
                case "help":
                    ShowHelp();
                    break;
                case "echo":
                    EchoCommand();
                    break;
                case "calc":
                    Calculator();
                    break;
                case "createf":
                    if (!string.IsNullOrEmpty(arguments))
                        CreateFolder(arguments);
                    else
                        Console.WriteLine("Please specify the folder name.");
                    break;
                case "addtxt":
                    if (!string.IsNullOrEmpty(arguments))
                        CreateTextFile(arguments);
                    else
                        Console.WriteLine("Please specify the file name.");
                    break;
                case "edit":
                    if (!string.IsNullOrEmpty(arguments))
                        EditTextFile(arguments);
                    else
                        Console.WriteLine("Please specify the file name.");
                    break;
                case "read":
                    if (!string.IsNullOrEmpty(arguments))
                        ReadTextFile(arguments);
                    else
                        Console.WriteLine("Please specify the file name.");
                    break;
                case "dir":
                    ListFiles();
                    break;
                case "clear":
                    Console.Clear();
                    break;
                case "dirf":
                    if (!string.IsNullOrEmpty(arguments))
                        ChangeDirectory(arguments);
                    else
                        Console.WriteLine("Please specify the folder name.");
                    break;
                case "bttree":
                    BackToRoot();
                    break;
                case "shutdown":
                    Shutdown();
                    break;
                case "reboot":
                    Reboot();
                    break;
                case "sleepmode":
                    SleepMode();
                    break;
                case "rmtxt":
                    if (!string.IsNullOrEmpty(arguments))
                        RemoveTextFile(arguments);
                    else
                        Console.WriteLine("Please specify the file name.");
                    break;
                case "rmf":
                    if (!string.IsNullOrEmpty(arguments))
                        RemoveFolder(arguments);
                    else
                        Console.WriteLine("Please specify the folder name.");
                    break;
                case "rnf":
                    if (!string.IsNullOrEmpty(arguments))
                        RenameFolder(arguments);
                    else
                        Console.WriteLine("Please specify the folder names (old and new).");
                    break;
                case "rntxt":
                    if (!string.IsNullOrEmpty(arguments))
                        RenameTextFile(arguments);
                    else
                        Console.WriteLine("Please specify the file names (old and new).");
                    break;
                case "ttt":
                    PlayTicTacToe();
                    break;
                case "snake":
                    PlaySnake();
                    break;
                default:
                    var originalColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Unknown command. Type 'help' for a list of commands.");
                    Console.ForegroundColor = originalColor;
                    break;
            }
        }


        private void ShowHelp()
        {
            Console.WriteLine("Available commands:");
            Console.WriteLine(" help - Show this help message");
            Console.WriteLine(" echo - Repeat your input");
            Console.WriteLine(" calc - Simple calculator");
            Console.WriteLine(" createf - Create a folder in memory");
            Console.WriteLine(" addtxt - Create a text file in memory");
            Console.WriteLine(" edit - Edit a text file in memory");
            Console.WriteLine(" read - Read a text file from memory");
            Console.WriteLine(" dir - List files in memory");
            Console.WriteLine(" dirf - Change current directory");
            Console.WriteLine(" bttree - Return to the root directory");
            Console.WriteLine(" shutdown - Shutdown the system");
            Console.WriteLine(" reboot - Reboot the system");
            Console.WriteLine(" sleepmode - Enter sleep mode");
            Console.WriteLine(" clear - Clear the screen");
            Console.WriteLine(" rmtxt - Remove a text file");
            Console.WriteLine(" rmf - Remove a folder and its contents");
            Console.WriteLine(" rnf - Rename a folder (Usage: rnf oldFolder newFolder)");
            Console.WriteLine(" rntxt - Rename a text file (Usage: rntxt oldFile newFile)");
            Console.WriteLine(" ttt - Play Tic-Tac-Toe (2 Players)");
            Console.WriteLine(" snake - Play snake game)");
        }

        private void EchoCommand()
        {
            Console.Write("Enter text to echo: ");
            string text = Console.ReadLine();
            Console.WriteLine($"You typed: {text}");
        }

        private void Calculator()
        {
            try
            {
                Console.Write("Enter first number: ");
                double num1 = double.Parse(Console.ReadLine());
                Console.Write("Enter operator (+, -, *, /): ");
                string op = Console.ReadLine();
                Console.Write("Enter second number: ");
                double num2 = double.Parse(Console.ReadLine());

                double result = op switch
                {
                    "+" => num1 + num2,
                    "-" => num1 - num2,
                    "*" => num1 * num2,
                    "/" => num2 != 0 ? num1 / num2 : throw new DivideByZeroException(),
                    _ => throw new InvalidOperationException("Invalid operator")
                };

                Console.WriteLine($"Result: {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void CreateFolder(string folderName)
        {
            string path = GetFullPath(folderName);
            if (!virtualFileSystem.ContainsKey(path))
            {
                virtualFileSystem[path] = null;
                Console.WriteLine($"Folder '{folderName}' created successfully in memory.");
            }
            else
            {
                Console.WriteLine($"Folder '{folderName}' already exists in memory.");
            }
        }

        private void CreateTextFile(string fileName)
        {
            string path = GetFullPath(fileName);
            if (!virtualFileSystem.ContainsKey(path))
            {
                virtualFileSystem[path] = "This is a new text file.";
                Console.WriteLine($"Text file '{fileName}' created successfully in memory.");
            }
            else
            {
                Console.WriteLine($"Text file '{fileName}' already exists in memory.");
            }
        }

        private void EditTextFile(string fileName)
        {
            string path = GetFullPath(fileName);
            if (virtualFileSystem.ContainsKey(path) && virtualFileSystem[path] != null)
            {
                Console.Clear();
                List<string> lines = new List<string>();

                // Initialize with existing content
                if (!string.IsNullOrEmpty(virtualFileSystem[path]))
                {
                    lines.AddRange(virtualFileSystem[path].Split('\n'));
                }
                else
                {
                    lines.Add("");
                }

                int currentLine = 0;
                int currentColumn = lines[0].Length;
                bool editing = true;

                while (editing)
                {
                    Console.Clear();
                    // Draw header
                    Console.WriteLine($"                                                            TL&SPOS EDITOR");

                    // Display content with line numbers
                    for (int i = 0; i < 20; i++) // Show max 20 lines
                    {
                        if (i < lines.Count)
                        {
                            Console.Write($"|{lines[i]}");
                        }
                        else
                        {
                            Console.Write("|");
                        }
                        Console.WriteLine();
                    }

                    // Draw footer
                    Console.WriteLine("|-----------------------------------------------------------------------|");
                    Console.WriteLine("Note: To save press F2, To exit press ESC, To make new line press ENTER");

                    // Handle key input
                    var key = Console.ReadKey(true);

                    switch (key.Key)
                    {
                        case ConsoleKey.Enter:
                            // Insert new line
                            string currentLineText = lines[currentLine];
                            string restOfLine = currentLineText.Substring(currentColumn);
                            lines[currentLine] = currentLineText.Substring(0, currentColumn);
                            lines.Insert(currentLine + 1, restOfLine);
                            currentLine++;
                            currentColumn = 0;
                            break;

                        case ConsoleKey.Backspace:
                            if (currentColumn > 0)
                            {
                                lines[currentLine] = lines[currentLine].Remove(currentColumn - 1, 1);
                                currentColumn--;
                            }
                            else if (currentLine > 0)
                            {
                                // Merge with previous line
                                currentColumn = lines[currentLine - 1].Length;
                                lines[currentLine - 1] += lines[currentLine];
                                lines.RemoveAt(currentLine);
                                currentLine--;
                            }
                            break;

                        case ConsoleKey.LeftArrow:
                            if (currentColumn > 0)
                            {
                                currentColumn--;
                            }
                            else if (currentLine > 0)
                            {
                                currentLine--;
                                currentColumn = lines[currentLine].Length;
                            }
                            break;

                        case ConsoleKey.RightArrow:
                            if (currentColumn < lines[currentLine].Length)
                            {
                                currentColumn++;
                            }
                            else if (currentLine < lines.Count - 1)
                            {
                                currentLine++;
                                currentColumn = 0;
                            }
                            break;

                        case ConsoleKey.UpArrow:
                            if (currentLine > 0)
                            {
                                currentLine--;
                                currentColumn = Math.Min(currentColumn, lines[currentLine].Length);
                            }
                            break;

                        case ConsoleKey.DownArrow:
                            if (currentLine < lines.Count - 1)
                            {
                                currentLine++;
                                currentColumn = Math.Min(currentColumn, lines[currentLine].Length);
                            }
                            break;

                        case ConsoleKey.Escape:
                            editing = false;
                            break;

                        case ConsoleKey.F2:
                            // Save the file
                            virtualFileSystem[path] = string.Join("\n", lines.ToArray());
                            Console.WriteLine($"\nFile '{fileName}' saved successfully!");
                            System.Threading.Thread.Sleep(1000);
                            break;

                        default:
                            if (!char.IsControl(key.KeyChar))
                            {
                                lines[currentLine] = lines[currentLine].Insert(currentColumn, key.KeyChar.ToString());
                                currentColumn++;
                            }
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine($"File '{fileName}' does not exist in memory or is not a file.");
            }
        }

        private void ReadTextFile(string fileName)
        {
            string path = GetFullPath(fileName);
            if (virtualFileSystem.ContainsKey(path) && virtualFileSystem[path] != null)
            {
                Console.Clear();
                Console.WriteLine($"                                                            TL&SPOS READER");

                string[] lines = virtualFileSystem[path].Split('\n');
                foreach (string line in lines)
                {
                    Console.WriteLine($"|{line}");
                }

                Console.WriteLine("|-----------------------------------------------------------------------|");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey(true);
            }
            else
            {
                Console.WriteLine($"File '{fileName}' does not exist in memory or is not a file.");
            }
        }

        private void ListFiles()
        {
            Console.WriteLine($"Current directory: {currentDirectory}");
            bool foundFiles = false;
            foreach (var item in virtualFileSystem)
            {
                if (item.Key.StartsWith(currentDirectory) && item.Key != currentDirectory)
                {
                    string relativePath = item.Key.Substring(currentDirectory.Length).TrimStart('/');
                    if (!relativePath.Contains('/'))
                    {
                        foundFiles = true;
                        Console.WriteLine($"  {relativePath} {(item.Value == null ? "(Folder)" : "(File)")}");
                    }
                }
            }
            if (!foundFiles)
            {
                Console.WriteLine("No files or folders in this directory.");
            }
        }

        private void ChangeDirectory(string folderName)
        {
            if (folderName == "..")
            {
                if (currentDirectory != "root")
                {
                    int lastSlashIndex = currentDirectory.LastIndexOf('/');
                    currentDirectory = lastSlashIndex > 0
                        ? currentDirectory.Substring(0, lastSlashIndex)
                        : "root";
                    Console.WriteLine($"Moved up one directory: {currentDirectory}");
                }
                else
                {
                    Console.WriteLine("Already at the root directory.");
                }
                return;
            }

            string path = GetFullPath(folderName);
            if (virtualFileSystem.ContainsKey(path) && virtualFileSystem[path] == null)
            {
                currentDirectory = path;
                Console.WriteLine($"Changed current directory to: {currentDirectory}");
            }
            else
            {
                Console.WriteLine($"Folder '{folderName}' does not exist or is not a directory.");
            }
        }

        private void BackToRoot()
        {
            currentDirectory = "root";
            Console.WriteLine("Returned to the root directory.");
        }

        private string GetFullPath(string name)
        {
            return name.StartsWith("root") ? name : $"{currentDirectory.TrimEnd('/')}/{name}";
        }

        private void Shutdown()
        {
            Console.WriteLine("Shutting down...");
            Sys.Power.Shutdown();
        }

        private void Reboot()
        {
            Console.WriteLine("Rebooting...");
            Sys.Power.Reboot();
        }

        private void SleepMode()
        {
            Console.Clear();
            Console.WriteLine("Sleeping... (Press ESC to exit Sleep mode.)");
            while (true)
            {
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.Escape)
                {
                    Console.Clear();
                    Console.WriteLine("Exited sleep mode.");
                    break;
                }
            }
        }

        private void RemoveTextFile(string fileName)
        {
            string path = GetFullPath(fileName);
            if (virtualFileSystem.ContainsKey(path) && virtualFileSystem[path] != null)
            {
                virtualFileSystem.Remove(path);
                Console.WriteLine($"Text file '{fileName}' removed successfully from memory.");
            }
            else
            {
                Console.WriteLine($"Text file '{fileName}' does not exist in memory or is not a file.");
            }
        }

        private void RemoveFolder(string folderName)
        {
            string path = GetFullPath(folderName);
            if (virtualFileSystem.ContainsKey(path) && virtualFileSystem[path] == null)
            {
                var keysToRemove = new List<string>();
                foreach (var key in virtualFileSystem.Keys)
                {
                    if (key.StartsWith(path))
                    {
                        keysToRemove.Add(key);
                    }
                }
                foreach (var key in keysToRemove)
                {
                    virtualFileSystem.Remove(key);
                }
                Console.WriteLine($"Folder '{folderName}' and its contents removed successfully from memory.");
            }
            else
            {
                Console.WriteLine($"Folder '{folderName}' does not exist in memory or is not a folder.");
            }
        }

        private void RenameFolder(string folderNames)
        {
            string[] parts = folderNames.Split(new[] { ' ' }, 2);
            if (parts.Length == 2)
            {
                string oldName = GetFullPath(parts[0]);
                string newName = GetFullPath(parts[1]);

                if (virtualFileSystem.ContainsKey(oldName) && virtualFileSystem[oldName] == null)
                {
                    if (!virtualFileSystem.ContainsKey(newName))
                    {
                        var keysToRename = new List<string>();
                        foreach (var key in virtualFileSystem.Keys)
                        {
                            if (key.StartsWith(oldName))
                            {
                                keysToRename.Add(key);
                            }
                        }

                        foreach (var key in keysToRename)
                        {
                            string renamedKey = key.Replace(oldName, newName);
                            virtualFileSystem[renamedKey] = virtualFileSystem[key];
                            virtualFileSystem.Remove(key);
                        }

                        Console.WriteLine($"Folder '{parts[0]}' renamed to '{parts[1]}' successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Folder '{parts[1]}' already exists.");
                    }
                }
                else
                {
                    Console.WriteLine($"Folder '{parts[0]}' does not exist.");
                }
            }
            else
            {
                Console.WriteLine("Invalid folder rename command format.");
            }
        }

        private void PlayTicTacToe()
        {
            char[] board = { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            int player = 1; // 1 -> Player 1, 2 -> Player 2
            int choice;
            int flag = 0;

            do
            {
                Console.Clear();
                Console.WriteLine("Player 1: X and Player 2: O");
                Console.WriteLine("\n");
                if (player % 2 == 0)
                {
                    Console.WriteLine("Turn Player 2");
                }
                else
                {
                    Console.WriteLine("Turn Player 1");
                }
                Console.WriteLine("\n");
                Board(board);
                choice = int.Parse(Console.ReadLine());

                if (board[choice - 1] != 'X' && board[choice - 1] != 'O')
                {
                    if (player % 2 == 0)
                    {
                        board[choice - 1] = 'O';
                        player++;
                    }
                    else
                    {
                        board[choice - 1] = 'X';
                        player++;
                    }
                }
                else
                {
                    Console.WriteLine("The row {0} is already marked with an X or O", choice);
                    Console.WriteLine("\n");
                    Console.WriteLine("Please wait 2 seconds board is loading again.....");
                    System.Threading.Thread.Sleep(2000);
                }
                flag = CheckWin(board);
            }
            while (flag != 1 && flag != -1);

            Console.Clear();
            Board(board);

            if (flag == 1)
            {
                Console.WriteLine("Player {0} has won!", (player % 2) + 1);
            }
            else
            {
                Console.WriteLine("Draw");
            }
        }

        private void Board(char[] board)
        {
            Console.WriteLine("     |     |      ");
            Console.WriteLine("  {0}  |  {1}  |  {2}", board[0], board[1], board[2]);
            Console.WriteLine("_____|_____|_____ ");
            Console.WriteLine("     |     |      ");
            Console.WriteLine("  {0}  |  {1}  |  {2}", board[3], board[4], board[5]);
            Console.WriteLine("_____|_____|_____ ");
            Console.WriteLine("     |     |      ");
            Console.WriteLine("  {0}  |  {1}  |  {2}", board[6], board[7], board[8]);
            Console.WriteLine("     |     |      ");
        }

        private int CheckWin(char[] board)
        {
            #region Horizontal Winning Condition
            // Winning Condition For First Row
            if (board[0] == board[1] && board[1] == board[2])
            {
                return 1;
            }
            // Winning Condition For Second Row
            else if (board[3] == board[4] && board[4] == board[5])
            {
                return 1;
            }
            // Winning Condition For Third Row
            else if (board[6] == board[7] && board[7] == board[8])
            {
                return 1;
            }
            #endregion

            #region Vertical Winning Condition
            // Winning Condition For First Column
            else if (board[0] == board[3] && board[3] == board[6])
            {
                return 1;
            }
            // Winning Condition For Second Column
            else if (board[1] == board[4] && board[4] == board[7])
            {
                return 1;
            }
            // Winning Condition For Third Column
            else if (board[2] == board[5] && board[5] == board[8])
            {
                return 1;
            }
            #endregion

            #region Diagonal Winning Condition
            else if (board[0] == board[4] && board[4] == board[8])
            {
                return 1;
            }
            else if (board[2] == board[4] && board[4] == board[6])
            {
                return 1;
            }
            #endregion

            // Checking For Draw
            // If all cells are filled with X or O then no player has won
            else if (board[0] != '1' && board[1] != '2' && board[2] != '3' && board[3] != '4' && board[4] != '5' && board[5] != '6' && board[6] != '7' && board[7] != '8' && board[8] != '9')
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }


        private void PlaySnake()
        {
            int width = 20;
            int height = 20;
            int[] snakeX = new int[50];
            int[] snakeY = new int[50];
            int foodX = new Random().Next(1, width - 1);
            int foodY = new Random().Next(1, height - 1);
            int snakeLength = 3;
            int score = 0;
            string direction = "right";

            // Initialize snake position
            for (int i = 0; i < snakeLength; i++)
            {
                snakeX[i] = 10 - i;
                snakeY[i] = 10;
            }

            ConsoleKeyInfo keyInfo;
            do
            {
                Console.Clear();
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                        {
                            Console.Write("#");
                        }
                        else if (x == foodX && y == foodY)
                        {
                            Console.Write("O");
                        }
                        else
                        {
                            bool isSnake = false;
                            for (int i = 0; i < snakeLength; i++)
                            {
                                if (snakeX[i] == x && snakeY[i] == y)
                                {
                                    Console.Write("S");
                                    isSnake = true;
                                    break;
                                }
                            }
                            if (!isSnake)
                            {
                                Console.Write(" ");
                            }
                        }
                    }
                    Console.WriteLine();
                }

                if (Console.KeyAvailable)
                {
                    keyInfo = Console.ReadKey(true);
                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.UpArrow:
                            if (direction != "down") direction = "up";
                            break;
                        case ConsoleKey.DownArrow:
                            if (direction != "up") direction = "down";
                            break;
                        case ConsoleKey.LeftArrow:
                            if (direction != "right") direction = "left";
                            break;
                        case ConsoleKey.RightArrow:
                            if (direction != "left") direction = "right";
                            break;
                    }
                }

                for (int i = snakeLength - 1; i > 0; i--)
                {
                    snakeX[i] = snakeX[i - 1];
                    snakeY[i] = snakeY[i - 1];
                }

                switch (direction)
                {
                    case "up":
                        snakeY[0]--;
                        break;
                    case "down":
                        snakeY[0]++;
                        break;
                    case "left":
                        snakeX[0]--;
                        break;
                    case "right":
                        snakeX[0]++;
                        break;
                }

                if (snakeX[0] == foodX && snakeY[0] == foodY)
                {
                    foodX = new Random().Next(1, width - 1);
                    foodY = new Random().Next(1, height - 1);
                    snakeLength++;
                    score++;
                }

                if (snakeX[0] == 0 || snakeX[0] == width - 1 || snakeY[0] == 0 || snakeY[0] == height - 1)
                {
                    Console.Clear();
                    Console.WriteLine("Game Over! Your score: " + score);
                    break;
                }

                System.Threading.Thread.Sleep(100);
            } while (true);
        }

        private void AnimateBoot()
        {
            string[] logo = {
        "######## ##         ####     ######  ########   #######   ######",
        "   ##    ##        ##  ##   ##    ## ##     ## ##     ## ##    ##",
        "   ##    ##         ####    ##       ##     ## ##     ## ##",
        "   ##    ##        ####      ######  ########  ##     ##  ######",
        "   ##    ##       ##  ## ##       ## ##        ##     ##       ##",
        "   ##    ##       ##   ##   ##    ## ##        ##     ## ##    ##",
        "   ##    ########  ####  ##  ######  ##         #######   ######"
    };

            int maxLength = logo.Max(line => line.Length);
            string[] buffer = new string[logo.Length];
            for (int i = 0; i < maxLength; i++)
            {
                Console.Clear();
                for (int j = 0; j < logo.Length; j++)
                {
                    if (i < logo[j].Length)
                        buffer[j] += logo[j][i];
                    Console.WriteLine(buffer[j]);
                }
                System.Threading.Thread.Sleep(50);
            }

            Console.WriteLine("\nBoot completed! Welcome to TL&SPOS!");
            Console.WriteLine("Press any key to start working!");
            Console.ReadKey();
        }




        private void RenameTextFile(string fileNames)
        {
            string[] parts = fileNames.Split(new[] { ' ' }, 2);
            if (parts.Length == 2)
            {
                string oldName = GetFullPath(parts[0]);
                string newName = GetFullPath(parts[1]);

                if (virtualFileSystem.ContainsKey(oldName) && virtualFileSystem[oldName] != null)
                {
                    if (!virtualFileSystem.ContainsKey(newName))
                    {
                        virtualFileSystem[newName] = virtualFileSystem[oldName];
                        virtualFileSystem.Remove(oldName);
                        Console.WriteLine($"Text file '{parts[0]}' renamed to '{parts[1]}' successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Text file '{parts[1]}' already exists.");
                    }
                }
                else
                {
                    Console.WriteLine($"Text file '{parts[0]}' does not exist.");
                }
            }
            else
            {
                Console.WriteLine("Invalid file rename command format.");
            }
        }
    }
}

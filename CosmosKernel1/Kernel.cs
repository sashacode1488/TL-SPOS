using System;
using Sys = Cosmos.System;
using System.Collections.Generic;

namespace TLSPOS
{
    public class Kernel : Sys.Kernel
    {
        private Dictionary<string, string> virtualFileSystem = new Dictionary<string, string>();
        private string currentDirectory = "root"; // Initial root folder

        protected override void BeforeRun()
        {
            Console.Clear();
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
                Console.WriteLine($"Editing file: {fileName}");
                Console.WriteLine($"Current content: {virtualFileSystem[path]}");
                Console.Write("Enter new content: ");
                string newContent = Console.ReadLine();
                virtualFileSystem[path] = newContent;
                Console.WriteLine($"File '{fileName}' updated successfully in memory.");
            }
            else
            {
                Console.WriteLine($"File '{fileName}' does not exist in memory or is a folder.");
            }
        }

        private void ReadTextFile(string fileName)
        {
            string path = GetFullPath(fileName);
            if (virtualFileSystem.ContainsKey(path) && virtualFileSystem[path] != null)
            {
                Console.WriteLine($"Reading file: {fileName}");
                Console.WriteLine($"Content: {virtualFileSystem[path]}");
            }
            else
            {
                Console.WriteLine($"File '{fileName}' does not exist in memory or is a folder.");
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
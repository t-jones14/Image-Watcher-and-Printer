/*
    Image Watcher and Printer Application
    -------------------------------------
    This application watches a folder for new JPEG, BMP, or TIFF image files, prints them 
    to a specified printer, and moves them to a "Finished" folder. All transactions and events 
    are logged to a file, and a configuration file defines various parameters.

    Licensed under the GNU General Public License, version 3 (GPL-3.0).
    For more information, visit the official repository: https://github.com/t-jones14/Image-Watcher-and-Printer

    You should have received a copy of the GNU General Public License along with this program.
    If not, see: https://www.gnu.org/licenses/

    © 2024 Taylor Jones - NexGen Digital Solutions
*/

using System; // Provides basic system functions like console output.
using System.IO; // Provides file handling functions.
using System.Configuration; // Allows reading configuration settings from AppConfig.
using System.Drawing.Printing; // Allows interacting with printers.
using System.Drawing; // Provides tools for image handling.
using System.Threading.Tasks; // Allows for asynchronous tasks.

class Program
{
    static void Main(string[] args)
    {
        // Load configuration settings from AppConfig
        string inputFolder = ConfigurationManager.AppSettings["InputFolder"];
        string outputFolder = ConfigurationManager.AppSettings["OutputFolder"];
        string logFile = ConfigurationManager.AppSettings["LogFile"];
        string printerName = ConfigurationManager.AppSettings["PrinterName"];

        // Ensure that the output folder exists, create it if it doesn't
        Directory.CreateDirectory(outputFolder);

        // Create a FileSystemWatcher to monitor the input folder for changes
        FileSystemWatcher watcher = new FileSystemWatcher
        {
            Path = inputFolder, // Set the path to watch
            Filter = "*.*", // Watch all file types initially
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite // Only trigger on file creation or modification
        };

        // When a file is created, trigger the event
        watcher.Created += (source, e) =>
        {
            // If the new file is an image file, process it
            if (IsImageFile(e.FullPath))
            {
                Task.Run(() => ProcessFile(e.FullPath, outputFolder, logFile, printerName));
            }
        };

        // Start watching for events
        watcher.EnableRaisingEvents = true;

        // Inform the user the application is running and watching the folder
        Console.WriteLine("Watching folder: " + inputFolder);
        Console.WriteLine("Press 'q' to quit the application.");

        // Keep the application alive until the user presses 'q'
        while (Console.Read() != 'q') ;
    }

    // Helper method to determine if a file is an image based on its extension
    private static bool IsImageFile(string path)
    {
        // Get the file extension and convert it to lowercase for comparison
        string extension = Path.GetExtension(path).ToLower();
        // Return true if the file is a JPG, JPEG, BMP, or TIFF
        return extension == ".jpg" || extension == ".jpeg" || extension == ".bmp" || extension == ".tiff";
    }

    // Method to process an image file: print it, move it, and log the event
    private static void ProcessFile(string filePath, string outputFolder, string logFile, string printerName)
    {
        string fileName = Path.GetFileName(filePath); // Get the file name
        string outputFilePath = Path.Combine(outputFolder, fileName); // Set the destination file path

        try
        {
            // Print the image file
            PrintImage(filePath, printerName);

            // Move the file to the output folder after printing
            File.Move(filePath, outputFilePath);

            // Log the success event
            LogEvent(logFile, $"Successfully processed and moved file: {fileName}");
        }
        catch (Exception ex)
        {
            // Log any error that occurs during the process
            LogEvent(logFile, $"Error processing file {fileName}: {ex.Message}");
        }
    }

    // Method to print an image file using the specified printer
    private static void PrintImage(string filePath, string printerName)
    {
        // Create a new PrintDocument object
        PrintDocument pd = new PrintDocument();
        // Set the printer to the one specified in the config
        pd.PrinterSettings.PrinterName = printerName;

        // Define what happens when the print job starts
        pd.PrintPage += (sender, args) =>
        {
            // Load the image from the file
            Image img = Image.FromFile(filePath);
            // Set the print location
            Point loc = new Point(100, 100);
            // Draw the image on the printed page
            args.Graphics.DrawImage(img, loc);
        };

        // Send the print job
        pd.Print();
    }

    // Method to log events to the specified log file
    private static void LogEvent(string logFile, string message)
    {
        // Format the log message with a timestamp
        string logMessage = $"{DateTime.Now}: {message}";
        // Append the log message to the log file
        File.AppendAllText(logFile, logMessage + Environment.NewLine);
    }
}

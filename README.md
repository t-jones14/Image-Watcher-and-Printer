![image-watcher-and-printer](https://github.com/user-attachments/assets/ab9a693c-d500-4199-b55c-9d7a40788d66)

# Image Watcher and Printer

This is a simple C# command-line application that watches a folder for newly added JPEG, BMP, or TIFF image files, prints them to a specified printer, and then moves the printed files to a "Finished" folder. All actions, including success and errors, are logged to a log file. The app uses a configuration file to set parameters such as input/output folders, printer name, and log file location. The idea behind this application is to watch for FTP event snapshot image uploads from an NVR/Surveillance System and to subsequently print the snapshot(s).

## Features

- Monitors a specified folder for image files (JPEG, BMP, TIFF).
- Prints the files to a specified printer.
- Moves the processed files to a "Finished" folder.
- Logs all actions (success and errors) to a log file.
- Configurable through an `AppConfig.config` file.

## Requirements

- .NET Framework (or .NET Core)
- A valid printer installed on your system.

## Installation

1. Clone the repository to your local machine:
   1. `git clone https://github.com/t-jones14/Image-Watcher-and-Printer.git `
2. Open the project in your preferred IDE (e.g., Visual Studio).
3. Modify the `AppConfig.config` file to match your folder paths and printer settings.
4. Build and run the project.

## Configuration

You can configure the application using the `AppConfig.config` file. Here's an example:

```
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <appSettings>
    <!-- Input folder where the app will watch for new images -->
    <add key="InputFolder" value="C:\\Images\\Input"/>

    <!-- Output folder where processed (printed) images will be moved -->
    <add key="OutputFolder" value="C:\\Images\\Finished"/>

    <!-- Log file path where event logs will be stored -->
    <add key="LogFile" value="C:\\Logs\\fileProcessing.log"/>

    <!-- Name of the printer where the images will be printed -->
    <add key="PrinterName" value="YourPrinterName"/>
  </appSettings>
</configuration>
```

## Usage

1. Place any `.jpg`, `.jpeg`, `.bmp`, or `.tiff` image files into the folder specified in the `InputFolder` config setting.
2. The application will automatically detect new files, print them, move them to the `OutputFolder`, and log the event.
3. To stop the application, press 'q' in the console.

## Logging

The application writes logs to a file specified in the `LogFile` setting. It logs each file processed, including success and error messages.

## Customization

* **Input/Output Folders** : Modify the `InputFolder` and `OutputFolder` paths in the `AppConfig.config` file.
* **Printer Name** : Set the printer name in the `PrinterName` setting.
* **Logging Path** : Customize the path of the log file by changing the `LogFile` setting.

## Known Issues

* Ensure that the printer name in the `AppConfig.config` matches the name of an installed printer on your system.
* The application only supports `.jpg`, `.jpeg`, `.bmp`, and `.tiff` image formats.

## Contributing

Feel free to open issues and submit pull requests if you have suggestions or improvements for this project.

## License

This project is licensed under the GNU GPL v3 License - see the LICENSE file for details.

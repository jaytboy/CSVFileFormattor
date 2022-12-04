# CSV formattor for Uploading to GoodBudget™
Converts a csv file downloaded from a specific bank to the format required by GoodBudget™.

__**What's new in v1.0.0?**__
- Use a better csv parser.
    - Faster than the stardard parser for C#.
    - Column feilds have a type. This allows a quick data check to make sure the feilds are being parsed correctly.
    - Provides helpful error messages if the file cannot be parsed. This includes line numbers so you can quickly got to the line that is causing the error in your favorite text editor and fix the issue.
    - Addition of a "Hint" button. The hints do not help you with the function of the program but you might enjoy them.

### Compatible OS
- Windows OS.

## Install
Download setup file from release tab and install in the typical way for .msi files.

## Edit source code for your bank
Download sources code and update the "Transaction" class and ClassMap to the headings of your csv file.

## Releases:
 - [v1.0.0](https://github.com/jaytboy/CSVFileFormattor/releases/tag/v1.0.0)
 - [v0.1.2](https://github.com/jaytboy/CSVFileFormattor/releases/tag/v0.1.2)

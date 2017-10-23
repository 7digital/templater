templater
=========

Dotnet based exe for doing config templates.

The application looks for config files with "template" in the name in a given directory and then applies the correct settings based on the local environment variable.

A global settings file can also used in addition to the local files in the selected directory.

### Usage

There are 3 parameters which can be used:

-d "directory". Required. The working directory to recursively scan.

-g "global-settings". Not Required. Path to a global settings json file.

-e "environment". Not Required. Run for a specific environment only - defaults to all.

Example:
templater.exe -d src -g catalogue-availability.settings.json

# NiceSON - Make your JSON look nice

## Installing
NiceSON is a .NET CLI tool and can be installed via the dotnet command
```dotnet tool install --global NiceSon```

[View the package on nuget.org](https://www.nuget.org/packages/NiceSon/)

## Usage:
  NiceSon \<json\> [options]

## Arguments:
  \<json\>  The JSON to format. It is recommended to wrap the JSON is single quotes to avoid conflicts with double quotes escaping the JSON string early.

## Options:
**-nc, --no-clipboard**

Disable storing the formatted JSON in the clipboard

**-t, --console, --terminal**

Output the formatted JSON to the console


**-f, --file <file>**

Directory to output a text file containing the formatted JSON


**--version**

Show version information


**-?, -h, --help**

Show help and usage information

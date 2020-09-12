Yo Command Line
================


[![NuGet][nuget-badge]][nuget]

[nuget]: https://www.nuget.org/packages/Skikra.YoCli/1.8.0
[nuget-badge]: https://img.shields.io/badge/nuget-v1.8.0-blue?style=flat-square


This is a command line tool for writing and reading notes built with .Net Core. I will continue to make improvements, release updates, and accept contributions.

## Install

Install the [Tool][nuget] as a global or local tool.

```
$ dotnet tool install --global Skikra.YoCli --version 1.8.0
```

## Usage

The main command is **yo** that have three sub commands :

```
Usage: yo [command] [options]

Options:
  --version     Show version information
  -?|-h|--help  Show help information

Commands:
  find          Find notes
  read          Read notes
  write         Write a note

Run 'yo [command] -?|-h|--help' for more information about a command.
```


### Write

```
$ yo write --help
    Write a note

    Usage: yo write [options] <Note>

    Arguments:
    Note

    Options:
    -?|-h|--help  Show help information
```

### Read


```
$ yo read --help
Read notes

Usage: yo read [options]

Options:
  -t|--today      Read today notes
  -y|--yesterday  Read yesterday notes
  -w|--week       Read current week notes
  -?|-h|--help    Show help information
```

### Find

```
$ yo find --help
Find notes

Usage: yo find [options]

Options:
  -c|--content[:<CONTENT>]  Find notes that contains this value
  -d|--day[:<DAY>]          Find notes written this day on the current month
  -m|--month[:<MONTH>]      Find notes written this month on the current year
  -?|-h|--help              Show help information
```

## Contributing

I will be very happy if you help to improve this tool.

You have a new feature idea, fill an issue and let's discuss it.
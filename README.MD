Yo Command Line
================


[![NuGet][nuget-badge]][nuget]

[nuget]: https://www.nuget.org/packages/Skikra.YoCli/2.0.0
[nuget-badge]: https://img.shields.io/badge/nuget-v2.0.0-blue?style=flat-square


This is a command line tool for writing and reading notes built with .Net Core. I will continue to make improvements, release updates, and accept contributions.

## Install

Install the [Tool][nuget] as a global or local tool.

```
$ dotnet tool install --global Skikra.YoCli --version 2.2.0
```

## Usage

The main command is **yo** that have six sub commands :

```
2.2.0

Usage: yo [command] [options]

Options:
  --version     Show version information
  -?|-h|--help  Show help information

Commands:
  export        Export notes to Json file
  find          Find notes
  import        Import notes from Json file
  read          Read notes
  remove        Remove a note by a specified date
  write         Write a note

Run 'yo [command] -?|-h|--help' for more information about a command.
```
### Export

```
$ yo export --help
  Export notes to Json file

  Usage: yo export [options] <Path>

  Arguments:
    Path          The directory where to save the file

  Options:
    -?|-h|--help  Show help information
```

### Import

Check json file [example](https://github.com/elrhomariyounes/YoCli/blob/master/example/notes.json)

```
$ yo import --help
  Import notes from Json file

  Usage: yo import [options] <Path>

  Arguments:
    Path          Path to Json file to import from

  Options:
    -?|-h|--help  Show help information
```

### Write

```
$ yo write --help
    Write a note

    Usage: yo write [options] <Note>

    Arguments:
    Note    Content of the note

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

### Remove


```
$ yo remove --help
Remove a note by a specified date

Usage: yo remove [options] <Date>

Arguments:
  Date          Date criteria in local date format

Options:
  -?|-h|--help  Show help information
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
  -y|--year[:<YEAR>]        Find notes written this year
  -?|-h|--help              Show help information
```

## Contributing

I will be very happy if you help to improve this tool.

You have a new feature idea, fill an issue and let's discuss it.

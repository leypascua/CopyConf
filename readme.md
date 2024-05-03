# CopyConf

A command line tool to copy default config files from a specified location to an output directory. Use this in your post-build event to automatically add default configuration files that can be modified in your local development environment.

## Installation

Add via dotnet CLI
   dotnet tool install copyconf -g

## How it works

Typically, you would like to distribute default config files for your app but you don't want developers to push their local versions of the files to the remote repository. CopyConf takes files ending with .dist and renames them to the correct file name after copying to the destination.

Consider the directory structure for a solution below:

```tree

SolutionDir
   |-- conf
   |    |-- environment.config.dist
   |    |-- connections.Development.json.dist
   |    |-- appsettings.Development.json.dist
   |    |-- build.yaml.dist
   |-- src
   |    |-- MyAspNetProject
   |    |       |-- MyAspNetProject.csproj
   |    |-- MySolution.sln

```

Running CopyConf under the folder 'MyAspNetProject' where the csproj file is located will copy files ending in .dist to the current working directory.

## Usage

Add it as a pre/post-build command in your .csproj file:

```

<Target Name="PreBuild" AfterTargets="PreBuildEvent">
   <Exec Command="copyconf &quot;$(SolutionDir)conf&quot; &quot;$(ProjectDir)&quot;" Condition="'$(Configuration)' == 'Debug' />
</Target>

```

OR manually run on the command line:

```bash

   $> copyconf "C:/project/conf" "C:/project/src/MyWebProject"

```

## Arguments, Switches and Options

```bash

Usage: copyconf [options] <sourceDir> <destinationDir>

Arguments:
  sourceDir        "/path/to/source" The path to the directory where default
                   content will be copied from.
  destinationDir   "/path/to/dest" The path to the directory where files will be
                   copied to

Options:
  -?|-h|--help     Show help information.
  -e|--extensions  [OPTIONAL] Supported file extensions delimited by semi-colon.
                   Defaults to: ".conf;.config;.json;.yaml;.yml;.xml;.env"
  -r|--rootonly    [OPTIONAL] Prevent files in subdirectories from being copied
  -f|--force       [OPTIONAL] Force create destination directory when it doesn't
                   exist.
  -v|--verbose     [OPTIONAL] Verbose mode.

Syntax:  
  copyconf sourceDir destinationDir [--extensions=".conf;.config;.json;.yaml;.yml;.xml;.env"] [--rootonly] [--force]

```
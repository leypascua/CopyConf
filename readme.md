# CopyConf

A command line tool to copy default config files from a specified location to an output directory. Use this in your post-build event to automatically add default configuration files that can be modified in your local development environment.

## Installation

Add via NuGet Package Console
   Install-Package CopyConf

OR use as a dotnet CLI tool by editing the .csproj file:

```xml

  <DotNetCliToolReference Include="CopyConf" />

```

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
   <Exec Command="dotnet copyconf &quot;$(SolutionDir)conf&quot; &quot;$(ProjectDir)&quot;" Condition="'$(Configuration)' == 'Debug' />
</Target>

```

OR manually execute via the dotnet CLI:

```bash

   $> dotnet copyconf "C:/project/conf" "C:/project/src/MyWebProject"

```

OR manually execute using the executable when using in non-dotnetcore projects:

```bash

   $> dotnet-copyconf "C:/project/conf" "C:/project/src/MyWebProject"

```

## Arguments, Switches and Options

```bash

Usage: dotnet-copyconf sourceDir destinationDir [options]

Arguments:  
  sourceDir       "/path/to/source" [REQUIRED] The path to the directory where default content will be copied from.  
  destinationDir  "/path/to/dest" [REQUIRED] The path to the directory where files will be copied to  

Options:  
  -?|-h|--help     Show help information  
  -e|--extensions  [OPTIONAL] Supported file extensions delimited by semi-colon. Defaults to: ".conf;.config;.json;.yaml;.yml;.xml"  
  -r|--rootonly    [OPTIONAL] Prevent files in subdirectories from being copied  
  -f|--force       [OPTIONAL] Force create destination directory when it doesn't exist.  
  -v|--verbose     [OPTIONAL] Verbose mode.  

Syntax:  
  dotnet-copyconf sourceDir destinationDir [--extensions=".conf;.config;.json;.yaml;.yml;.xml"] [--rootonly] [--force]  

```
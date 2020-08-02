## Introduction 
The application is a simulation of a toy robot moving on a square tabletop, of dimensions 5 units x 5 units. There are no other obstructions on the table surface.

The robot is free to roam around the surface of the table, but must be prevented from falling to destruction. Any movement that would result in the robot falling from the table must be prevented, however further valid movement commands must still be allowed.

This application read commands of the following form:

- PLACE X,Y, FACING
- MOVE
- LEFT
- RIGHT
- REPORT

**PLACE** will put the toy robot on the table in position X,Y and facing NORTH, SOUTH, EAST or WEST.

The origin (0,0) can be considered to be the SOUTH WEST most corner.

The first valid command to the robot is a PLACE command, after that, any sequence of commands may be issued, in any order, including another PLACE command. The application will discard all commands in the sequence until a valid PLACE command has been executed.

**MOVE** will move the toy robot one unit forward in the direction it is currently facing. 
**LEFT** and **RIGHT** will rotate the robot 90 degrees in the specified direction without changing the position of the robot.

**REPORT** will announce the X,Y and FACING of the robot. The report is displayed as a standard output.

A robot that is not on the table will ignore the MOVE, LEFT, RIGHT and REPORT commands.

## Getting Started

### Pre-Requisites
1. [Git](https://git-scm.com/download/win) installed.
1. Require [.Net Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)

### Installation Process
```shell
# Cloning the repository
> git clone https://github.com/rakeshraman89/toy-robot.git

# Build and run the console application
> cd toy-robot-puzzle
> dotnet publish -o "build"
> cd build
> dotnet Toy.Robot.ConsoleApp.dll
```
#### Input file
The application only accepts .txt files.
There is a test data file located at `$(Root directory)\TestData`. This file will be executed when the application starts. You can add more files with commands to automatically execute when starting application.
**The name of the newly added text file should be added to the `TestFiles` property in `appsettings.json` file located at `$(Root directory)\src\Toy.Robot.ConsoleApp`**
Name of the text file can also be entered as console input once the application is started.

### Running Unit tests
The unit tests are written using NUnit framework and have Moq for mocking the properties and calls.

```shell
# Cloning the repository
> git clone https://github.com/rakeshraman89/toy-robot.git
```
Cloning is not required if it has already been cloned during the installation process.
The folowing steps can be carried out from the directory where it was cloned previously.

```shell
# Running unit tests
> cd toy-robot-puzzle\test\Toy.Robot.UnitTest
> dotnet test Toy.Robot.UnitTest.csproj
```
#### Customizing the board size
The size of the board can be modified by changing the values of length and breadth of Board settings in the `appsettings.json` file located at,
```
$(Root directory)\src\Toy.Robot.ConsoleApp
```

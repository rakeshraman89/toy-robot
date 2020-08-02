## Introduction 
The application is a simulation of a toy robot moving on a square tabletop, of dimensions 5 units x 5 units. There are no other obstructions on the table surface.

The robot is free to roam around the surface of the table, but must be prevented from falling to destruction. Any movement that would result in the robot falling from the table must be prevented, however further valid movement commands must still be allowed.

Create an application that can read in commands of the following form:

PLACE X,Y, FACING
MOVE
LEFT
RIGHT
REPORT
PLACE will put the toy robot on the table in position X,Y and facing NORTH, SOUTH, EAST or WEST.

The origin (0,0) can be considered to be the SOUTH WEST most corner.

The first valid command to the robot is a PLACE command, after that, any sequence of commands may be issued, in any order, including another PLACE command. The application should discard all commands in the sequence until a valid PLACE command has been executed.

MOVE will move the toy robot one unit forward in the direction it is currently facing. LEFT and RIGHT will rotate the robot 90 degrees in the specified direction without changing the position of the robot.

REPORT will announce the X,Y and FACING of the robot. This can be in any form, but standard output is sufficient.

A robot that is not on the table can choose the ignore the MOVE, LEFT, RIGHT and REPORT commands.

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
> cd pwd-strength\test\Password.Strength.UnitTest
> dotnet test Password.Strength.UnitTests.csproj
```

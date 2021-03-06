# Parking Lot

## Requirements
- .NET Core SDK 3.1 or later (https://dotnet.microsoft.com/download).
- A text editor of choice (this project was developed using [Visual Studio Code](https://code.visualstudio.com/)).
- (Optional) Latest version of [Postman](https://www.getpostman.com/) for running the included tests.
- Docker Desktop for Windows/Mac or Linux (`docker`, `docker-compose`)

## Setting Up
- `git clone` the repository into a directory on your computer.
- Use the `dotnet restore` command to install project dependencies.
- Navigate to the `./ParkingLot.Api` directort and run the project using the `dotnet run` command.
- Execute requests against `http://localhost:5000/` or `https://localhost:5001/`.

## Generating Migrations
To generate a new migration after the data model has changed, run `dotnet ef migrations add <migration_name> -p ParkingLot.Data -s ParkingLot.Api` from the project root.

## Useful Tools
Some development tools you might find useful for this project.
- [Visual Studio 2019](https://visualstudio.microsoft.com/) IDE (Windows only).
- [JetBrains Rider](https://www.jetbrains.com/rider/)
- [C# debugging extension](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp) for Visual Studio Code.

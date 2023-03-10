# My Travel Journal API

Server-side application for storing information about your adventures abroad or at home.
Provides REST API for communication between client applications and database.

## Installation

### Prerequisites
- [.NET](https://dotnet.microsoft.com/en-us/download) - at least version .NET 6
- [Docker](https://docs.docker.com/engine/install/)
  - Needed for running database image
- (for development) Some IDE - for example one of listed below
  - [Rider](https://www.jetbrains.com/rider/)
  - [Visual Studio](https://visualstudio.microsoft.com/cs/)
  - [MonoDevelop](https://www.monodevelop.com/download/)


### Step-by-step guide
1. Clone the repo
```shell
git clone git@github.com:filipsyn/my-travel-journal-api.git
```

2. Change into project directory
```shell
cd MyTravelJournal.Api/
```

3. Install Entity Framework tool
```shell
dotnet tool install --global dotnet-ef
```

4. Change into API project directory
```shell
cd MyTravelJournal.Api
```

5. Apply migrations
```shell
dotnet ef database update
```

### Run
Run the application with
```shell
dotnet run
```

## Used technologies
- .NET 6
- Entity Framework
- Docker
- PostgreSQL

## Additional resources
- [Endpoints documentation](docs/endpoints.md)



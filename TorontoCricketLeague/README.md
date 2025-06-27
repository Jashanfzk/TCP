# Toronto Cricket League - Content Management System

## Project Overview

The Toronto Cricket League CMS is a comprehensive web application built with ASP.NET Core that manages cricket franchises, teams, players, and sponsors. This project demonstrates mastery of Content Management System concepts including Entity Framework, LINQ operations, API development, and MVC architecture.

## Features

### Core Entities
- **Franchises**: Cricket league franchises with team and sponsor management
- **Teams**: Teams belonging to franchises with player rosters
- **Players**: Individual players with team assignments and roles
- **Sponsors**: Sponsors associated with franchises

### Relationships
- **1-M Relationships**: 
  - Franchise → Teams (one franchise can have multiple teams)
  - Franchise → Sponsors (one franchise can have multiple sponsors)
  - Team → Players (one team can have multiple players)
- **M-M Relationships**:
  - Teams ↔ Sponsors (through TeamSponsor junction table)

## Technology Stack

- **Framework**: ASP.NET Core 8.0
- **Database**: SQL Server with Entity Framework Core
- **Architecture**: MVC with API endpoints
- **Data Access**: LINQ with Entity Framework
- **Documentation**: XML documentation with detailed examples

## Project Structure

```
TorontoCricketLeague/
├── Controllers/          # MVC and API controllers
│   ├── FranchisesController.cs
│   ├── TeamsController.cs
│   ├── PlayersController.cs
│   └── SponsorsController.cs
├── Models/              # Entity models
│   ├── Franchise.cs
│   ├── Team.cs
│   ├── Player.cs
│   ├── Sponsor.cs
│   └── TeamSponsor.cs
├── DTOs/                # Data Transfer Objects
│   ├── FranchiseDto.cs
│   ├── TeamDto.cs
│   └── PlayerDto.cs
├── Services/            # Business logic layer
│   ├── IFranchiseService.cs
│   └── FranchiseService.cs
├── Data/                # Database context
│   └── ApplicationDbContext.cs
└── Views/               # MVC views
    ├── Franchises/
    ├── Teams/
    ├── Players/
    └── Sponsors/
```

## API Endpoints

### Franchises API

#### Get All Franchises
```http
GET /Franchises/ListFranchises
```
**Response Example:**
```json
[
  {
    "FranchiseId": 1,
    "Name": "Super Kings",
    "HomeCity": "Toronto",
    "LogoUrl": "",
    "TeamCount": 3,
    "SponsorCount": 2
  }
]
```

#### Get Franchise by ID
```http
GET /Franchises/FindFranchise/{id}
```
**Response Example:**
```json
{
  "FranchiseId": 1,
  "Name": "Super Kings",
  "HomeCity": "Toronto",
  "LogoUrl": "",
  "TeamCount": 3,
  "SponsorCount": 2
}
```

#### Create Franchise
```http
POST /Franchises/CreateFranchise
Content-Type: application/json

{
  "Name": "Super Kings",
  "HomeCity": "Toronto",
  "LogoUrl": "https://example.com/logo.png"
}
```

#### Update Franchise
```http
PUT /Franchises/UpdateFranchise/{id}
Content-Type: application/json

{
  "FranchiseId": 1,
  "Name": "Updated Super Kings",
  "HomeCity": "Toronto",
  "LogoUrl": "https://example.com/newlogo.png"
}
```

#### Delete Franchise
```http
DELETE /Franchises/DeleteFranchise/{id}
```

### Teams API

#### Get All Teams
```http
GET /Teams/ListTeams
```
**Response Example:**
```json
[
  {
    "TeamId": 1,
    "Name": "Super Kings A",
    "HomeGround": "Toronto",
    "FranchiseId": 1,
    "FranchiseName": "Super Kings"
  }
]
```

#### Get Team by ID
```http
GET /Teams/FindTeam/{id}
```

#### Create Team
```http
POST /Teams/CreateTeam
Content-Type: application/json

{
  "Name": "Super Kings A",
  "HomeGround": "Toronto",
  "FranchiseId": 1
}
```

#### Update Team
```http
PUT /Teams/UpdateTeam/{id}
Content-Type: application/json

{
  "TeamId": 1,
  "Name": "Updated Super Kings A",
  "HomeGround": "New Toronto",
  "FranchiseId": 1
}
```

#### Delete Team
```http
DELETE /Teams/DeleteTeam/{id}
```

### Players API

#### Get All Players
```http
GET /Players/ListPlayers
```
**Response Example:**
```json
[
  {
    "PlayerId": 1,
    "FirstName": "John Doe",
    "LastName": "",
    "DateOfBirth": "1998-01-01T00:00:00",
    "Position": "Batsman",
    "TeamId": 1,
    "TeamName": "Super Kings A"
  }
]
```

#### Get Player by ID
```http
GET /Players/FindPlayer/{id}
```

#### Create Player
```http
POST /Players/CreatePlayer
Content-Type: application/json

{
  "FirstName": "John",
  "LastName": "Doe",
  "DateOfBirth": "1990-01-01",
  "Position": "Batsman",
  "TeamId": 1
}
```

#### Update Player
```http
PUT /Players/UpdatePlayer/{id}
Content-Type: application/json

{
  "PlayerId": 1,
  "FirstName": "John",
  "LastName": "Smith",
  "DateOfBirth": "1990-01-01",
  "Position": "Bowler",
  "TeamId": 1
}
```

#### Delete Player
```http
DELETE /Players/DeletePlayer/{id}
```

## Web Interface

### Franchises Management
- **Index**: View all franchises with team and sponsor counts
- **Details**: View franchise details with related teams and sponsors
- **Create**: Add new franchises
- **Edit**: Modify existing franchises
- **Delete**: Remove franchises with confirmation

### Teams Management
- **Index**: View all teams with franchise information
- **Details**: View team details with franchise info
- **Create**: Add new teams with franchise selection
- **Edit**: Modify existing teams
- **Delete**: Remove teams with confirmation

### Players Management
- **Index**: View all players with team information
- **Details**: View player details with team info
- **Create**: Add new players with team selection
- **Edit**: Modify existing players
- **Delete**: Remove players with confirmation

### Sponsors Management
- **Index**: View all sponsors
- **Create**: Add new sponsors with franchise selection
- **Edit**: Modify existing sponsors
- **Delete**: Remove sponsors with confirmation

## Database Schema

### Franchise Entity
```csharp
public class Franchise
{
    public int FranchiseId { get; set; }
    public string Name { get; set; }
    public string? HomeCity { get; set; }
    public string? LogoUrl { get; set; }
    
    // Navigation Properties
    public ICollection<Team> Teams { get; set; }
    public ICollection<Sponsor> Sponsors { get; set; }
}
```

### Team Entity
```csharp
public class Team
{
    public int TeamId { get; set; }
    public string Name { get; set; }
    public string? City { get; set; }
    public string? LogoUrl { get; set; }
    public int FranchiseId { get; set; }
    
    // Navigation Properties
    public Franchise? Franchise { get; set; }
    public ICollection<Player> Players { get; set; }
    public ICollection<TeamSponsor> TeamSponsors { get; set; }
}
```

### Player Entity
```csharp
public class Player
{
    public int PlayerId { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Role { get; set; }
    public int TeamId { get; set; }
    
    // Navigation Properties
    public Team? Team { get; set; }
}
```

### Sponsor Entity
```csharp
public class Sponsor
{
    public int SponsorId { get; set; }
    public string Name { get; set; }
    public string? LogoUrl { get; set; }
    public int FranchiseId { get; set; }
    
    // Navigation Properties
    public Franchise? Franchise { get; set; }
    public ICollection<TeamSponsor> TeamSponsors { get; set; }
}
```

## Installation and Setup

### Prerequisites
- .NET 8.0 SDK
- SQL Server (LocalDB or full instance)
- Visual Studio 2022 or VS Code

### Setup Instructions

1. **Clone the repository**
   ```bash
   git clone [repository-url]
   cd TorontoCricketLeague
   ```

2. **Update connection string**
   - Open `appsettings.json`
   - Update the `DefaultConnection` string to point to your SQL Server instance

3. **Run database migrations**
   ```bash
   dotnet ef database update
   ```

4. **Build and run the application**
   ```bash
   dotnet build
   dotnet run
   ```

### API Testing
All API endpoints have been tested and documented with:
- Request/response examples
- Proper HTTP status codes
- Error handling
- Data validation

### Web Interface Testing
The MVC interface provides:
- Complete CRUD operations
- Form validation
- User-friendly error messages
- Responsive design

## Contributing

This project was developed as a CMS course assignment demonstrating:
- Entity Framework Core mastery
- LINQ query proficiency
- API development skills
- MVC architecture implementation
- Professional documentation standards

## License

This project is developed for educational purposes as part of a Content Management Systems course.

---

**Developer**: Jashanpreet Singh Gill
**Course**: Web development  
**Instructor**: Christine Bittle  

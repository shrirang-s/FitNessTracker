# Workout Tracker

A full-stack workout tracking application built with ASP.NET Core 5, Entity Framework Core, and SQLite. Log gym sessions, track sets, and view exercise history through a clean web UI or REST API.

---

## Tech Stack

| Layer | Technology |
|---|---|
| API | ASP.NET Core 5 Web API |
| UI | Razor Pages + Bootstrap 5 |
| ORM | Entity Framework Core 5 |
| Database | SQLite (file-based, no setup required) |
| Language | C# |

---

## Architecture

```
WorkoutTracker.Api/
├── Controllers/        # HTTP layer — routes requests, returns responses
├── Services/           # Business logic — validation, orchestration
├── Repositories/       # Data access — EF Core queries only
├── Models/             # EF Core entities (map to DB tables)
├── DTOs/               # API contracts — decoupled from DB schema
├── Data/               # AppDbContext — EF config and relationships
├── Infrastructure/     # ExerciseInfoProvider — static exercise/muscle data
├── Pages/              # Razor Pages UI
│   ├── Exercises/      # List, Create, Edit, Details, History
│   ├── Sessions/       # List, Create, Edit, Details
│   └── Sets/           # Create, Edit
└── wwwroot/            # Static files
```

**Request flow (example — Add Set):**
```
POST /api/workoutsessions/{id}/sets
  → WorkoutSetsController
    → WorkoutSetService   (validates session + exercise exist, checks reps/weight)
      → WorkoutSetRepository
        → AppDbContext → SQLite
```

---

## Features

### Exercises
- Create exercises with name and muscle group (dropdown)
- View exercise details with muscle anatomy diagram (public domain Gray's Anatomy images)
- Edit and delete exercises
- View full history of sets logged for any exercise

### Workout Sessions
- Create sessions with date and notes
- View all sessions ordered by date
- Edit session details
- View a session with all its sets

### Sets
- Log sets against a session (exercise, weight, reps, set number)
- Edit any logged set
- Delete sets from a session

---

## Getting Started

### Prerequisites
- [.NET 5 SDK](https://dotnet.microsoft.com/download/dotnet/5.0)

### Run locally

```bash
git clone <your-repo-url>
cd cards/WorkoutTracker.Api
dotnet run
```

Open **http://localhost:5050** in your browser.

On first run, EF Core automatically creates `workout.db` and applies the migration. No manual database setup needed.

### Swagger (API docs)

```
http://localhost:5050/swagger
```

### Reset the database

```bash
rm WorkoutTracker.Api/workout.db
dotnet run
```

---

## API Endpoints

```
GET    /api/exercises                          List all exercises
POST   /api/exercises                          Create exercise
                                               Body: { "name": "...", "muscleGroup": "..." }

GET    /api/workoutsessions                    List all sessions
GET    /api/workoutsessions/{id}               Get session with sets
POST   /api/workoutsessions                    Create session
                                               Body: { "date": "2026-03-20T09:00:00", "notes": "..." }

POST   /api/workoutsessions/{id}/sets          Add set to session
                                               Body: { "exerciseId": 1, "weight": 100, "reps": 8, "setNumber": 1 }

GET    /api/exercises/{id}/history             All sets logged for an exercise
```

---

## Design Decisions

**Why separate Repositories from Services?**
Services contain *why* (business rules). Repositories contain *how* (SQL/EF). Swapping SQLite for Postgres only touches repositories.

**Why DTOs instead of returning entities?**
Entities are coupled to the DB schema. DTOs are the API contract. Adding a DB column doesn't accidentally expose it to clients.

**Why interfaces on everything?**
Enables dependency injection and unit testing — you can mock `IExerciseRepository` without hitting the DB.

**Why SQLite?**
Zero setup. Anyone can clone and run immediately. For production, swap the connection string and EF provider for Postgres or SQL Server — no other code changes needed.

**Delete behaviour:**
- Deleting a session cascades to its sets (they're meaningless without the session)
- Deleting an exercise is restricted if it has logged sets (protects workout history)

---

## Muscle Group Images

Exercise detail pages display public domain anatomy diagrams sourced from:
- [NCBI StatPearls](https://www.ncbi.nlm.nih.gov/books/NBK539897/) — Gray's Anatomy illustrations (public domain)
- [Wikimedia Commons](https://commons.wikimedia.org/) — OpenStax and CC-licensed diagrams

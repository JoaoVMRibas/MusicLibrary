# MusicLibrary

This project was created for **study and practice purposes** using **.NET**, focusing on concepts such as:

- Domain-Driven Design (DDD)
- Test-Driven Development (TDD)
- Clean Architecture
- Entity Framework Core

The goal is to incrementally build a well-structured application while applying best practices step by step.

---

## Domain Layer

The **Domain Layer** represents the core of the system and contains all business rules. It is completely independent from infrastructure, frameworks, and external concerns.

### Implemented Entities

- Artist (Aggregate Root)
- Album
- Music

### Domain Rules

- An **artist must have a valid name**
- An **album must have a valid name**
- A **music must have a valid name**
- A **music must have a valid duration (greater than zero)**

- An **artist cannot have duplicated album names**
- An **artist cannot have duplicated music names**

- A **music can only be added to an album through the Artist aggregate**
- A **music cannot be added to a non-existent album**
- A **non-existent music cannot be added to an album**

- A **music cannot be deleted if it belongs to at least one album**
- A **music must be explicitly removed from an album before it can be deleted**

- Albums calculate their **duration dynamically** based on the sum of their musics' durations

All invalid operations are handled through custom exceptions for each case mentioned above.

---

## Application Layer

The **Application Layer** contains the use cases of the system and orchestrates interactions between the domain and external layers.

### Implemented Services

- **ArtistService**
  - Create artist
  - Get artist by id
  - Get all artists
  - Update artist
  - Delete artist

- **AlbumService**
  - Create album
  - Get album by id
  - Get albums by artist
  - Delete album

- **MusicService**
  - Create music
  - Get music by id
  - Get musics by artist
  - Add music to album
  - Remove music from album
  - Delete music (only if not associated with any album)

### Requests & DTOs

- Each use case has its own **Request object**
- Requests are grouped by entity (Artist, Album, Music)
- Output is returned via **DTOs**, keeping domain entities isolated

---

## Infrastructure Layer

The Infrastructure Layer is responsible for all technical concerns related to data persistence and external frameworks.

### Entity Framework Core

#### DbContext

- MusicLibraryDbContext : DbContext
  - Represents the database session
  - Exposes DbSets for:
    - Artists
    - Albums
    - Musics
  - Applies all entity configurations via OnModelCreating

#### Entity Configurations

Entity mappings are explicitly defined using Fluent API, keeping persistence concerns out of the domain.

- ArtistConfiguration : IEntityTypeConfiguration<Artist>
- AlbumConfiguration : IEntityTypeConfiguration<Album>
- MusicConfiguration : IEntityTypeConfiguration<Music>

#### Migrations

The Migrations folder contains all database versioning history.

#### Repositories

Repositories are implemented using Entity Framework Core and follow contracts defined in the Application layer.

- ArtistRepository : IArtistRepository

---

## Web API Layer

The Web API Layer exposes the application use cases via HTTP and acts as the system’s entry point.

### Controllers

- Use RESTful conventions
- Delegate all logic to Application Services

#### Controllers
- ArtistController
  - Artist creation, retrieval, update, and deletion
  - Entry point for artist-related operations
  - Manage album–music relationships

- AlbumController
  - Album creation and deletion
  - Retrieve albums by artist

- MusicController
  - Music creation and deletion
  - Retrieve musics by artist

---

## Tests

Unit tests were implemented to validate:

### Domain Tests
- Entity creation rules
- Business invariants
- Aggregate behavior
- Error scenarios (duplicates, invalid input, invalid relationships)

### Application Tests
- Service behavior
- Repository interaction
- Exception propagation

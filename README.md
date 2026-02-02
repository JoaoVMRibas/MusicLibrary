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
- A **music must have a valid name**
- A **music must have a valid duration (greater than zero)**
- An **artist cannot have duplicated music names**
- An **album must have a valid name**
- An **artist cannot have duplicated album names**
- A **music cannot be added to a non-existent album**
- A **non-existent music cannot be added to an album**
- Albums calculate their duration based on the sum of their musics' durations

---

## Application Layer

The **Application Layer** contains the use cases of the system and orchestrates interactions between the domain and external layers.

### Implemented Services

- **ArtistService**
  - Create
  - Get by id
  - Get all
  - Update
  - Delete

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
  - Delete music

### Requests & DTOs

- Each use case has its own **Request object**
- Requests are grouped by entity (Artist, Album, Music)
- Output is returned via **DTOs**

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

---

## Next Steps

- Implement the **Infrastructure Layer**
  - Entity Framework Core
  - Repository implementations
- Create database migrations
- Expose functionality through a **Web API**

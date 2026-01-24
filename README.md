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

## Tests

Unit tests were implemented to validate:

- Entity creation rules
- Business rules
- Aggregate behavior
- Error scenarios (invalid input, duplicates, invalid relationships)

---

## Next Steps

- Implement the **Application Layer**
- Define use cases and application services
- Add repository abstractions
- Implement the **Infrastructure Layer** using Entity Framework Core
- Expose functionality through a **Web API**

# Student Course Registration System

A console-based application for managing student course registrations, built as a layered architecture demonstration.

## Project Structure

```
src/
├── StudentRegistrationSystem.Domain/     # Core business entities and interfaces
├── StudentRegistrationSystem.Application/ # Business logic and services
├── StudentRegistrationSystem.Infrastructure/ # Data access implementations
└── StudentRegistrationSystem.Console/    # User interface

tests/
└── StudentRegistrationSystem.Tests/      # Unit tests

docs/
├── vision.md
├── backlog.md
├── class-diagram.md
├── sequence-diagram.md
└── iteration-1.md
```

## How to Run

1. Navigate to the solution directory
2. Restore dependencies: `dotnet restore`
3. Build the solution: `dotnet build`
4. Run the console application: `dotnet run --project src/StudentRegistrationSystem.Console`

## Available Commands

1. Register student for course
2. View student's courses
3. View available courses
4. Exit

## Running Tests

`dotnet test`

## Architecture

- **Domain**: Contains entities (Student, Course) and repository interfaces
- **Application**: Business services and logic
- **Infrastructure**: Repository implementations (currently in-memory)
- **Console**: User interface layer

## Current Features

- Student and course management
- Course registration with capacity checks
- Basic validation and error handling
- Unit tests for core functionality
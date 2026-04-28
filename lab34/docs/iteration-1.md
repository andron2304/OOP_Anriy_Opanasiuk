# Iteration 1 - Baseline

## What is working
- Domain model with Student and Course entities, including validation and business rules
- Repository interfaces and in-memory implementations
- CourseService with registration logic and error handling using Result pattern
- Console application with menu for course registration, viewing student courses, and available courses
- Basic unit tests covering entity creation, validation, and service operations

## Artifacts in the repository
- docs/vision.md - Project vision and requirements
- docs/backlog.md - Iteration plan
- docs/class-diagram.md - UML class diagram
- docs/sequence-diagram.md - Sequence diagram for registration scenario
- Solution with Domain, Application, Infrastructure, Console, and Tests projects
- Vertical slice: Complete course registration scenario through console interface

## Scenarios to expand in Lab 35
- Add course creation and management for administrators
- Implement student profile management
- Add course search and filtering capabilities

## Risks and uncertainties
- Scalability of in-memory repositories for larger datasets
- Lack of data persistence across application restarts
- No user authentication or authorization

## Classes/interfaces prepared for extension
- ICourseRepository and IStudentRepository interfaces allow for different storage implementations
- CourseService can be extended with additional business logic
- Domain entities have validation that can be enhanced
- Result class provides a foundation for more sophisticated error handling
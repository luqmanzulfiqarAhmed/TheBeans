**Covering Note: Technology Choices**

The TheBeans project follows Clean Architecture with CQRS to ensure modularity, scalability, and maintainability.

- .NET 8 & C#: Chosen for performance, reliability, and rich ecosystem support.

- PostgreSQL: Selected for its robustness, ACID compliance, and scalability in handling relational data.

- MediatR (CQRS Pattern): Decouples request handling, making the application more testable and maintainable.

- Entity Framework Core: Used as the ORM for seamless database interactions.

- AutoMapper: Simplifies object-to-object mapping, reducing boilerplate code.

- Quartz.NET: Handles scheduled tasks, such as daily selection of "Bean of the Day."

- xUnit & Moq: Ensures unit testing coverage and reliable mocking of dependencies.

These choices enhance maintainability, scalability, and testability, aligning with industry best practices.

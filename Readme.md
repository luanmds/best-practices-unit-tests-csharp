# Best Practices Unit Tests in C#

This project aims to exemplify good practices in Unit Tests

To achieve this goal, it demonstrates through scenarios based on the domain context of this repository 
and several examples of unit tests to help the Developer 
to understand what is a good test.

## Repository Structure

In this repo there are three projects:

- `src/BestPractices.Domain`: have scenarios and busines rules to grant loan for a customer context.
- `tests/BestPractices.GoodTests`: good tests examples using the Domain context.

## Libraries and Frameworks used in this repo

- [Moq](https://github.com/moq/moq)
- [xUnit](https://xunit.net/)
- [FluentValidation](https://fluentassertions.com/)

## References
- [Unit testing best practices with .NET Core and .NET Standard](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices)
- Unit Testing Principles, Practices, and Patterns - by Vladimir Khorikov
- [Unit testing C# in .NET Core using dotnet test and xUnit](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-dotnet-test)
- [https://www.arhohuttunen.com/test-data-builders/](https://www.arhohuttunen.com/test-data-builders/)
- [https://martinfowler.com/articles/mocksArentStubs.html](https://martinfowler.com/articles/mocksArentStubs.html#TestsWithMockObjects)

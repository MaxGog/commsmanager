# Contributing Guide

Thank you for deciding to help develop CommsManager! We welcome all contributions – from fixing typos to adding new features.

## 🧭 Getting Started

1. **Fork the repository** and clone your fork.
2. Create a branch for your changes: `git checkout -b feature/my-awesome-feature`.
3. Make your changes following the guidelines below.
4. Ensure all tests pass: `dotnet test`.
5. Push your branch and open a Pull Request against the `main` branch.

## 📝 Code Style

- Use **C# 12** and **.NET 10**.
- Follow the [Microsoft .NET Coding Conventions](https://docs.microsoft.com/dotnet/csharp/fundamentals/coding-style/coding-conventions).
- Class, method, and property names should be in **PascalCase**, local variables in **camelCase**.
- Document public APIs with XML comments (`///`).

## 📦 Commit Message Guidelines

We follow **Conventional Commits**:

```
<type>(<scope>): <short description>

[optional body]
```

Allowed types:
- `feat` – new feature
- `fix` – bug fix
- `docs` – documentation changes
- `style` – formatting, whitespace, etc.
- `refactor` – code refactoring without behavior change
- `test` – adding or updating tests
- `chore` – maintenance tasks (dependency updates, CI configuration)

Example:  
`feat(api): add endpoint for order status update`

## 🔄 Pull Request Process

- In the PR description, briefly explain the essence of your changes.
- Mention which issues your PR resolves (if any, link to them).
- Wait for review from maintainers.

## ❓ Questions?

Open an [Issue](https://github.com/MaxGog/CommsManager/issues) or start a discussion in [Discussions](https://github.com/MaxGog/CommsManager/discussions).
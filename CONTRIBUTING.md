# Contributing

Thanks for your interest in contributing to this project.

## Code of Conduct

By participating, you agree to follow the guidelines in [CODE_OF_CONDUCT.md](CODE_OF_CONDUCT.md).

## Prerequisites

- .NET SDK 10.x
- Bun (for WebUi build assets)

## Local Setup

1. Fork and clone your fork.
2. Create a feature branch from `main`.
3. Restore and build:

```bash
dotnet restore --solution Template.slnx
dotnet build --solution Template.slnx --configuration Release
```

4. Run tests:

```bash
dotnet test --solution Template.slnx --configuration Release
```

5. Build docs (optional but recommended when docs change):

```bash
dotnet docfx metadata
dotnet docfx build
```

## Development Guidelines

- Keep changes focused and small.
- Follow existing architecture boundaries (`Domain`, `Features`, `Infrastructure`, `Configuration`).
- Add or update tests for behavior changes.
- Update documentation when introducing new features or folder conventions.

## Commit and PR Guidelines

- Use clear commit messages describing intent.
- Open pull requests against `main`.
- Include in PR description:
  - What changed
  - Why it changed
  - How it was tested

## Pull Request Checklist

- [ ] Build passes locally
- [ ] Tests pass locally
- [ ] Documentation updated if needed
- [ ] No unrelated files changed

## Reporting Bugs and Requesting Features

Please open an issue with:

- Steps to reproduce (for bugs)
- Expected vs actual behavior
- Environment details (.NET SDK, OS)

# nd100-markdown

Generates **Markdown instruction-set documentation** from the [nd100-definitions](https://github.com/HackerCorpLabs/nd100-definitions) YAML specs.

---

## Requirements

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- Git (for submodule initialization)

---

## Getting started

```bash
git clone --recurse-submodules https://github.com/HackerCorpLabs/nd100-markdown.git
cd nd100-markdown
```

If you already cloned without submodules:

```bash
git submodule update --init --recursive
```

---

## Build & generate

```bat
build.bat
```

Or manually:

```bash
dotnet build ND100.Markdown.sln -c Release
dotnet run --project src/ND100.Markdown.Tool -c Release --no-build
```

Output is written to `docs/`.

> **Workflow:** generated docs are committed to the repository. After running the build, stage and commit any changes in `docs/` so the rendered documentation stays in sync with the specs.

### Updating the specs submodule

When `nd100-definitions` has upstream changes you want to pull in:

```bash
git submodule update --remote nd100-definitions
```

Then rebuild and commit everything together:

```bash
build.bat
git add nd100-definitions docs/
git commit -m "Update nd100-definitions and regenerate docs"
git push
```

This records the new submodule pin and the regenerated docs in a single commit.

---

## Project structure

```
nd100-markdown/
├── src/
│   ├── ND100.Markdown.Tool/          # CLI entry point
│   └── NDGen.Generators.Markdown/    # Markdown generation library
├── nd100-definitions/                # Git submodule — pinned YAML specs
├── docs/                             # Generated Markdown docs (committed)
├── build.bat                         # One-step build + generate
└── ND100.Markdown.sln
```

> **Note:** `nd100-definitions/` is a git submodule. Do not replace it with a symlink or directory junction — the submodule pin ensures reproducible builds.

---

## Configuration

| Environment variable      | Description                                  |
|---------------------------|----------------------------------------------|
| `ND100_SPECS_CPU_YAML`    | Override path to `cpu.yaml` spec file        |

---

## Generated documentation

| Document | Description |
|----------|-------------|
| [docs/index.md](docs/index.md) | ND-100 instruction set reference — full instruction listing |
| [docs/cpu_documentation.md](docs/cpu_documentation.md) | CPU documentation — registers, flags, and instruction details |
| [docs/addressing_modes.md](docs/addressing_modes.md) | Addressing modes — bit structure and mode descriptions |

---

## License

[MIT](LICENSE) — Copyright (c) 2026 Ronny Hansen

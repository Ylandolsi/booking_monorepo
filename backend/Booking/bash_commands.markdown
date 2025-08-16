# Bash Commands

## .NET CLI Commands

Install the Entity Framework Core CLI tool globally:

```bash
dotnet tool install --global dotnet-ef
```

Build commands for debugging:

```bash
# Build with detailed output
dotnet build --verbosity detailed

# Build without restoring packages
dotnet build --no-restore

# Filter build output to show only errors (first 10)
dotnet build 2>&1 | grep -E "(error|Error)" | head -10
```

## PostgreSQL Client Installation

Update package lists and install the PostgreSQL client:

```bash
sudo apt update
sudo apt install postgresql-client
```
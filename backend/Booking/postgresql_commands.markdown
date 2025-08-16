# PostgreSQL Commands

## Connecting to PostgreSQL

Connect to a PostgreSQL database:

```bash
psql -h localhost -p 5432 -U postgres -d clean-architecture
```

## psql Commands

```sql
# List all databases
\l

# Connect to a different database
\c database_name

# List tables in the current database
\dt

# Describe table structure
\d table_name

# Run an SQL query (example)
SELECT * FROM table_name;

# Query AspNetUsers table (correct case sensitivity)
SELECT * FROM "AspNetUsers";

# Example query with condition
SELECT * FROM "users"."AspNetUsers" WHERE "slug" = 'yassine-landolsi';

# Exit the psql shell
\q
```

## Notes on Case Sensitivity

PostgreSQL is case-sensitive for identifiers. For example:

```sql
SELECT * FROM AspNetUsers;
```

is interpreted as:

```sql
SELECT * FROM aspnetusers;
```

This will fail if the table is named `"AspNetUsers"` (with quotes preserving case). Always use quotes for case-sensitive identifiers.
# Docker Commands

## Container Management

```bash
# Check running containers
docker ps

# Check all containers (including stopped)
docker ps -a

# Start a container by name or ID
docker start <container_name_or_id>

# Stop a container by name or ID
docker stop <container_name_or_id>

# Remove a container by name or ID
docker rm <container_name_or_id>

# View logs of a container
docker logs <container_name_or_id>
```

## Running a PostgreSQL Container

Run a PostgreSQL container with port mapping and password set:

```bash
docker run -d --name postgres -e POSTGRES_PASSWORD=postgres -p 5432:5432 postgres:latest
```

## Interactive Shell

Access an interactive shell inside a running container:

```bash
docker exec -it <container_name_or_id> bash
```

## Docker Compose

Build and start services defined in `docker-compose.yml`:

```bash
docker compose up --build
```

## Inspect Containers

Inspect container names and their published ports:

```bash
docker inspect -f '{{ .Name }} - {{ range .NetworkSettings.Ports }}{{ println . }}{{ end }}' $(docker ps -q)
```

## Port Management

Check and free a specific port (e.g., 8081):

```bash
# List processes using port 8081
sudo lsof -i :8081

# Manually kill processes blocking port 8081
sudo kill 50559 50568

# Force kill if necessary
sudo kill -9 50559 50568

# Confirm port is free (no output means free)
sudo lsof -i :8081
```
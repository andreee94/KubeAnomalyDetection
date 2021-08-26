# Useful commands

## EFCore Migrations

To create the initial migration:

```bash
dotnet ef migrations add InitialCreate -p AnomalyDetection.Data/ -s AnomalyDetection.Manager --context ManagerContext --output-dir ./Migrations/Sqlite
```

To apply the migration:

```bash
dotnet ef database update -p AnomalyDetection.Data/ -s AnomalyDetection.Manager
```
# Exagochi aka ЕОГЭ

Test hackathon task

# How to run?

- clone the repo
- `dotnet restore`
- `dotnet run`

If you want, you can configure settings in Properties/launchSettings.json and appsettings.json files.

# Features

- JWT auth
- Hangfire recurring job (every day all exagochis will starve)
- SQLite db for users, exagochis and challenges
- Swagger UI

# What I did not do

- Parsing tasks from exam sites
- Verbose documentation for all endpoints
- Real-time communication with the client
# ToDo
Just another ToDo app

# Local setup

```
docker-compose up -d
```

Then navigate to http://localhost:8080 with your favourite browser.

```
docker-compose down -d
```

When you are working with Visual Studio or any other IDE outside of Docker you just need to ensure that the the PostgreSQL database server is running:

```
docker-compose up -d db
```

Make it comfortable with `dotnet watch`:

```
cd .\src\Api\Todo.Api
dotnet watch run --project .\Todo.Api.csproj
```
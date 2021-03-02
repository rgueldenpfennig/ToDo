# ToDo
Just another ToDo app

## Local setup
Use Docker to get everything running locally. Currently there's only the API server available.

```console
docker-compose up -d
```

Then navigate to `http://localhost:8080` with your favourite browser. This should open the OpenAPI Swagger documentation.

To shutdown everything you can use

```console
docker-compose down
```

When you are working with Visual Studio or any other IDE outside of Docker you just need to ensure that the the PostgreSQL database server is running:

```console
docker-compose up -d db
```

To make the development process more comfortable you can use `dotnet watch`:

```console
cd .\src\Api\Todo.Api
dotnet watch run --project .\Todo.Api.csproj
```
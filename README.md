# AdForm-API

## Descryption
A .NET API project with PostgreSQL and Docker for [Order Management API Exercise](https://github.com/erinev/order-management-api-exercise) by AdForm

# Technology used
1. PostgreSQL 17.
2. DBeaver: a database management system.
3. Docker.
4. Visual Studio 2022.
5. .NET 8.0
6. xUnit test.
7. Moq .NET: a data mocking tool.
8. Swagger API teting tool.
9. HotChocolate.aspnetcore: GraphQL endpoint creation.
10. Serilog.
11. Prometheus monitoring.

## Tasks and time duration
Database and project set up: 30 min.</br>
Linq query creation and testing: 6 hours.</br>
GraphQL endpoint set up and testing: 30 min.</br>
Serilog setup : 30 min.</br>
Prometheus setup : 30 min.</br>

### DB Scafold
dotnet ef dbcontext scaffold "Server=localhost;Port=5432;UserId=postgres;Password=admin;Database=AdFormSQL;" Npgsql.EntityFrameworkCore.PostgreSQL -o AdFormDB
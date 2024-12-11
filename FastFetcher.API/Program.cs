using Dapper;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.MapGet("/getProcedures", (IConfiguration configuration) =>
{
    using var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
    string query = @"SELECT name 
                             FROM sys.objects 
                             WHERE type = 'P' AND is_ms_shipped = 0";

    connection.Open();
    return connection.Query<string>(query);
})
.WithName("GetProcedures");

app.Run();

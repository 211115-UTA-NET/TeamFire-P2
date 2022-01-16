using pokeApi.Data;
var builder = WebApplication.CreateBuilder(args);

/** 
 *      if you want to test your db locally, 
 *      just uncomment line 8 and comment out line 14:
 */
//var connectionString = "Server=tcp:daleyserver.database.windows.net,1433;Initial Catalog=PokeApp;Persist Security Info=False;User ID=dbadmin;Password=adminpw1!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

/** 
 *      Azuer App Service URL: https://211115pokemonapp.azurewebsites.net/
 *      Get connection string from Azure App Service:
 */
string connectionString = builder.Configuration.GetConnectionString("Poke-DB-Connection");

IRepository repository = new SqlRepository(connectionString);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IRepository>(repository);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


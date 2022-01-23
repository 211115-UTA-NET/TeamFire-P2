using pokeApi.Data;
var builder = WebApplication.CreateBuilder(args);

/** 
 *      Azuer App Service URL: https://211115pokemonapp.azurewebsites.net/
 *      Uses Teamfire db connection string from Azure App Service when push to github.
 *      Uses kareem's db coonection string when run locally (can change the connection string in secrets)
 */
string connectionString = builder.Configuration.GetConnectionString("Poke-DB-Connection");

IRepository repository = new SqlRepository(connectionString);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IRepository>(repository);

builder.Services.AddCors(options =>
{
    // here you put all the origins that websites making requests to this API via JS are hosted at
    options.AddDefaultPolicy(builder =>
        builder
            .WithOrigins("http://127.0.0.1:4200",
                         "https://211115pokemonapp.azurewebsites.net/")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();


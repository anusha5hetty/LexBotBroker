using Amazon.Lex;
using Amazon.LexModelBuildingService;
using Amazon.LexModelsV2;
using Amazon.LexRuntimeV2;


var builder = WebApplication.CreateBuilder(args);
var awsOptions = builder.Configuration.GetAWSOptions("default");
awsOptions.Region = Amazon.RegionEndpoint.USWest2;
builder.Services.AddDefaultAWSOptions(awsOptions);
// Add services to the container.

builder.Services.AddControllers();

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy =>
        {
            policy.WithOrigins("*")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAWSService<IAmazonLex>();
builder.Services.AddAWSService<IAmazonLexModelBuildingService>();
builder.Services.AddAWSService<IAmazonLexModelsV2>();
builder.Services.AddAWSService<IAmazonLexRuntimeV2>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

// Use CORS
app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => "Hello World!");

app.Run();


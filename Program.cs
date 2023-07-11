using OpenAI.Extensions;

var builder = WebApplication.CreateBuilder(args);

// register OpenAI service
var apiKey = builder.Configuration.GetSection("OpenAI:ApiKey").Value!;
builder.Services.AddOpenAIService(s => s.ApiKey = apiKey);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();

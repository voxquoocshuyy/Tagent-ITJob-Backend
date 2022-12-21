using System.Text;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Json;
using ITJob.API.Configurations;
using ITJob.API.Cron;
using ITJob.Entity;
using ITJob.Services;
using ITJob.Services.Middleware;
using ITJob.Services.Utility.ErrorHandling;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var apiCorsPolicy = "ApiCorsPolicy";

builder.Services.AddCors(options =>
{
  options.AddPolicy(name: apiCorsPolicy,
      builder =>
      {
        builder.WithOrigins("http://localhost:3000", "http://127.0.0.1:5500", "https://spacenet.vn:8881",
                  "https://spacenet.vn:8882", "https://capstone-web-company.vercel.app", "https://capstone-web-admin-coral.vercel.app")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
      });
});
builder.Services.AddControllers().AddNewtonsoftJson(o =>
{
  o.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
  o.SerializerSettings.ContractResolver = new NewtonsoftJsonContractResolver()
  {
    NamingStrategy = new SnakeCaseNamingStrategy()
  };
  o.SerializerSettings.Converters.Add(new StringEnumConverter()
  {
    AllowIntegerValues = true
  });

});

builder.Services.RegisterSecurityModule(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.RegisterErrorHandling();
builder.Services.RegisterData();
builder.Services.RegisterBusiness();
builder.Services.RegisterSwaggerModule();
builder.Services.RegisterQuartz();
FirebaseApp.Create(new AppOptions()
{
  Credential = GoogleCredential.FromFile("Configurations/capstone-firebase-adminsdk.json")
});
var app = builder.Build();
app.UseExceptionHandler(err => err.UseExceptionMiddleware());

app.UseApplicationSwagger();
app.UseApplicationSecurity();
app.UseHttpsRedirection();
app.UseCors(apiCorsPolicy);
app.UseAuthorization();

app.MapControllers();

app.Run();
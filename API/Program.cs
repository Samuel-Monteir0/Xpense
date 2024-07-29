using System.Text;
using API.BusinessLayer.Interfaces;
using API.BusinessLayer.Services;
using API.DataAccess;
using API.Repositories.Implementation;
using API.Repositories.Interface;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<FirebaseContext>();
builder.Services.AddScoped<ISignUp, SignUp>();
builder.Services.AddScoped<ISignUpRepository, SignUpRepository>();
builder.Services.AddScoped<ILogin, Login>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<IAddTransaction, AddTransaction>();
builder.Services.AddScoped<IShowTransaction, ShowTransaction>();
builder.Services.AddScoped<IDailyTrendService, DailyTrendService>();
builder.Services.AddScoped<IDayToDay, DayToDay>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
// Initialize Firebase Admin SDK
try
{
    FirebaseApp.Create(new AppOptions()
    {
        Credential = GoogleCredential.GetApplicationDefault(),
        ProjectId = "xpense-android", // Replace with your Firebase Project ID
    });
}
catch (Exception ex)
{
    Console.WriteLine($"Error initializing Firebase: {ex.Message}");
    throw; // Optionally rethrow to fail fast if Firebase initialization fails
}
builder.Services.AddCors();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer( options => 
{
    var tokenKey = builder.Configuration["TokenKey"] ?? throw new Exception("Token key not found");
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200", "https://localhost:4200"));
// Configure the HTTP request pipeline.

app.UseAuthentication();
app.UseAuthorization();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers(); //Maps controller to endpoint

app.MapFallbackToController("Index", "Fallback");

app.Run();

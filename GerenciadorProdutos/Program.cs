using System.Text;
using GerenciadorProdutos.Repository;
using GerenciadorProdutos.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Servi�o de controllers
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
});

builder.Services.AddEndpointsApiExplorer();
// Configura��o do Swagger/OpenAPI
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Projeto GerenciadorProdutos",
        Description = "API de gerenciamento de produtos",
    });

    string applicationBasePath = AppContext.BaseDirectory;
    string applicationName = AppDomain.CurrentDomain.FriendlyName;
    string xmlDocumentPath = Path.Combine(applicationBasePath, $"{applicationName}.xml");

    if (File.Exists(xmlDocumentPath))
    {
        options.IncludeXmlComments(xmlDocumentPath);
    }

    // Configura��o do JWT para o Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Insira o token JWT desta maneira: Bearer SEU_TOKEN",
        Name = "Authorization",
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Todas as rotas de Servi�o
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<CategoryService>();

// Configura��o do banco de dados
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\bruno\\source\\repos\\GerenciadorProdutos\\GerenciadorProdutos\\DB_ProdutosGostaria.mdf;Integrated Security=True;Connect Timeout=30;Integrated Security=True;Connect Timeout=30");
});

// Configura��o de autentica��o JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "api-segura",
            ValidAudience = "api-clientes",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("minha-chave-segura-maior-que-256-bits-123456789"))
        };
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                context.Response.StatusCode = 401; // Unauthorized
                return context.Response.WriteAsJsonAsync(new { message = "Falha na autentica��o. Token inv�lido ou expirado." });
            },
            OnChallenge = context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = 401; // Unauthorized
                return context.Response.WriteAsJsonAsync(new { message = "Voc� precisa estar autenticado para acessar este recurso." });
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("Token validado com sucesso.");
                return Task.CompletedTask;
            }
        };
    });

// Pol�tica de autoriza��o (para Roles "Gerente" ou "Funcion�rio")
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("GerenteFuncionario", policy =>
        policy.RequireRole("Gerente", "Funcion�rio"));
});

// Habilitar o CORS para permitir requisi��es de qualquer origem
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

var app = builder.Build();

// Middleware para capturar erros de autoriza��o
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        if (ex is UnauthorizedAccessException)
        {
            context.Response.StatusCode = 403; // Forbidden
            await context.Response.WriteAsJsonAsync(new { message = "Acesso negado. Voc� n�o tem permiss�o para acessar este recurso." });
        }
        else
        {
            context.Response.StatusCode = 500; // Internal Server Error
            await context.Response.WriteAsJsonAsync(new { message = "Erro inesperado. Por favor, tente novamente mais tarde." });
        }
    }
});

// Configura��o do pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll"); // Aplicando a pol�tica de CORS
app.UseAuthentication(); // Adicionando o middleware de autentica��o
app.UseAuthorization();  // Adicionando o middleware de autoriza��o
app.MapControllers();

app.Run();

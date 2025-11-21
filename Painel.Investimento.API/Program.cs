using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Painel.investimento.Infra.Data;
using Painel.investimento.Infra.Repositorie;
using Painel.Investimento.API.Mapper;
using Painel.Investimento.API.Middlewares;
using Painel.Investimento.Aplication.UseCaseInvestimentos;
using Painel.Investimento.Aplication.UseCasesCadastros;
using Painel.Investimento.Aplication.useCaseSimulacoes;
using Painel.Investimento.Aplication.UseCasesLogin;
using Painel.Investimento.Aplication.UseCasesProdutos;
using Painel.Investimento.Application.Services;
using Painel.Investimento.Domain.Models;
using Painel.Investimento.Domain.Repository.Abstract;
using Painel.Investimento.Domain.Services;
using Painel.Investimento.Infra.Auth;
using Painel.Investimento.Infra.Data;
using Painel.Investimento.Infra.Repositorie;
using Painel.Investimento.Infra.Repositories;
using Painel.Investimento.Infra.Repository;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var jwt = builder.Configuration.GetSection("Jwt");
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));


// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// UseCases
builder.Services.AddScoped<ProdutoInvestimentoUseCase>();
builder.Services.AddScoped<ClienteUseCase>();
builder.Services.AddScoped<PerfilProdutoUseCase>();
builder.Services.AddScoped<InvestimentosUseCase>();
builder.Services.AddScoped<CalcularPerfilDeRiscoUseCase>();
builder.Services.AddScoped<SimularInvestimentoUseCase>();
builder.Services.AddScoped<ConsultarHistoricoSimulacoesUseCase>();
builder.Services.AddScoped<ConsultarSimulacoesAgrupadasUseCase>();
builder.Services.AddScoped<RecomendarProdutosUseCase>();


//Repository

builder.Services.AddScoped<IInvestimentosRepository, InvestimentosRepository>();
builder.Services.AddScoped<IPerfilProdutoRepository, PerfilProdutoRepository>();
builder.Services.AddScoped<IProdutoInvestimentoRepository, ProdutoInvestimentoRepository>();
builder.Services.AddScoped<IPerfilDeRiscoRepository, PerfilDeRiscoRepository>();
builder.Services.AddScoped<IRiskProfileService, RiskProfileService>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<ISimulacaoRepository, SimulacaoRepository>();

builder.Services.AddSingleton<ITelemetriaService, TelemetriaService>();
builder.Services.AddScoped<IAuthService, JwtAuthService>();
builder.Services.AddScoped<LoginUseCase>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//AutoMapper
builder.Services.AddAutoMapper(typeof(ProdutoInvestimentoProfile));
builder.Services.AddAutoMapper(typeof(PerfilProdutoProfile));
builder.Services.AddAutoMapper(typeof(InvestimentoProfile));
builder.Services.AddAutoMapper(typeof(UsuarioProfile));
builder.Services.AddAutoMapper(typeof(PerfilDeRisco));
builder.Services.AddAutoMapper(typeof(SimulacaoProfile));


// Registro do FluentValidation moderno
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = key,
            ClockSkew = TimeSpan.FromMinutes(2)
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Painel Investimento API", Version = "v1" });

    // ✅ Configuração para JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT no campo abaixo usando o formato: Bearer {seu token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
});

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<TelemetriaMiddleware>();

//Seed inicial
//using (var scope = app.Services.CreateScope())
//{
//    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
//    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

//    // 1. Garante que o banco existe
//    await DatabaseSeeder.EnsureDatabaseExistsAsync(configuration, db);

//    // 2. Aplica migrations → cria a tabela Cliente
//    await db.Database.MigrateAsync();

    // 3. Popula a tabela Cliente
    //await ClienteSeeder.SeedClientesAsync(db);
    //await PerfilDeRiscoSeeder.SeedPerfisDeRiscoAsync(db);
    //await ProdutoInvestimentoSeeder.SeedProdutosAsync(db);
    //await PerfilProdutoSeeder.SeedPerfilProdutosAsync(db);
    //await EnderecoSeeder.SeedEnderecosAsync(db);
    //await InvestimentosSeeder.SeedInvestimentosAsync(db);
//}


//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//    await UsuarioSeeder.SeedAdminAsync(db);
//}

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

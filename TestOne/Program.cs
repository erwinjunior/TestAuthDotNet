using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TestOne.Contexts;
using TestOne.Models;
using TestOne.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Note Api", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
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
            new string[]{}
        }
    });
});

// cors
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

// pattern
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IDataRepository<Note>, NoteRepository>();
builder.Services.AddScoped<IDataRepository<Author>, AuthorRepository>();

// database
builder.Services.AddDbContext<NoteDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NoteDb")));

// Add Identity and JWT Authentication
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<NoteDbContext>()
    .AddDefaultTokenProviders()
    .AddRoles<IdentityRole>()
    .AddSignInManager();

// JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    /*Defines whether the bearer token
    should be stored in the AuthenticationProperties after a successful authorization.*/
    options.SaveToken = true;
    /*
     Gets or sets if HTTPS is required for the metadata address or authority. 
    The default is true. This should be disabled only in development environments.
     */
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        // Gets or sets a boolean to control if the issuer will be validated during token validation.
        ValidateIssuer = true,
        //Gets or sets a boolean to control if the audience will be validated during token validation.
        ValidateAudience = true,
        /* Gets or sets a String that represents a valid issuer
        that will be used to check against the token's issuer. The default is null.*/
        ValidIssuer = builder.Configuration["JWT:ValidIsuser"],
        /* Gets or sets a string that represents a valid audience
         that will be used to check against the token's audience. The default is null.*/
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        // Gets or sets the SecurityKey that is to be used for signature validation.
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secrect"]))
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseCors();

app.Run();

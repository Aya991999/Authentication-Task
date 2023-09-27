using Data_Access.Data;
using Data_Access.IRepository;
using Data_Access.Mapper;
using Data_Access.Repository;
using Data_Access.UnitOfWork.AcountUnitOfWork;
using Data_Access.UnitOfWork.ProductUnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models.DbModels;
using Models.Helpers;
using Models.SendEmail;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
// Add services to the container.
builder.Services.Configure<JWT>(configuration.GetSection("JWT"));
builder.Services.AddIdentity<ApplicationUser,IdentityRole>().AddDefaultTokenProviders()
                .AddDefaultUI().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddDbContext<ApplicationDbContext>(options=>
options.UseSqlServer(("Server=.;Database=McShippers;Trusted_Connection=true;TrustServerCertificate=True")));
builder.Services.AddDbContext<DataDbContext>(options =>
options.UseSqlServer("Server=.;Database=McShippers;Trusted_Connection=true;TrustServerCertificate=True"));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IUnitOfWorkAccount,UnitOfWorkAccount>();
builder.Services.AddScoped<IUnitOfWorkProduct, UnitOfWorkProduct>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.RequireHttpsMetadata = false;
    o.SaveToken = false;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = configuration["JWT:ValidIssuer"],
        ValidAudience = configuration["JWT:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:key"]))
    };
});
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;
using HouseRentalSystem.Middlewares;
using HouseRentalSystem.Options;
using HouseRentalSystem.Services;
using HouseRentalSystem.Services.MongoDB;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);


{
    builder.Services.Configure<MongoDbOptions>(
        builder.Configuration.GetSection(MongoDbOptions.SectionName)
    );
    builder.Services.Configure<JwtOptions>(
        builder.Configuration.GetSection(JwtOptions.SectionName)
    );

    builder.Services.AddTransient(typeof(IPasswordHasher<>), typeof(PasswordHasher<>));
    builder.Services.AddScoped<IUserContext, UserContext>();
    builder.Services.AddScoped<IListingContext, ListingContext>();

    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IListingService, ListingService>();

    builder.Services.AddControllers();
    builder.Services.AddSwaggerGen(c =>
    {
        c.IncludeXmlComments(
            Path.Combine(
                AppContext.BaseDirectory,
                $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"
            )
        );
    });

    var jwtOptions = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()!;

    builder
        .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        jwtOptions.SecretKey
                            ?? throw new InvalidOperationException("Jwt Secret key is null!")
                    )
                ),
            };
        });
}

var app = builder.Build();


{
    // register middlewares
    app.UseMiddleware<ExceptionHandlingMiddleware>();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}

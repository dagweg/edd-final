using HouseRentalSystem.Middlewares;
using HouseRentalSystem.Options;
using HouseRentalSystem.Services;
using HouseRentalSystem.Services.MongoDB;
using Microsoft.AspNetCore.Identity;

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
    builder.Services.AddSwaggerGen();
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

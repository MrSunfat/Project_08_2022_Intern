using MISA.Web06.APIS.Core.Interfaces.Infrastructure;
using MISA.Web06.APIS.Core.Interfaces.Services;
using MISA.Web06.APIS.Core.Services;
using MISA.Web06.APIS.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

var myAllowSpecificOrigins = "_myAllowSpecificOrigins";


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IBaseRespository<object>, BaseRepository<object>>();
builder.Services.AddSingleton<IUsersRepository, UsersRepository>();
builder.Services.AddSingleton<IUserGroupsRepository, UserGroupsRepository>();
builder.Services.AddSingleton<IJobTitlesRepository, JobTitlesRepository>();
builder.Services.AddSingleton<IUserOptionsRepository, UserOptionsRepository>();
builder.Services.AddSingleton<IBaseService<object>, BaseService<object>>();
builder.Services.AddSingleton<IUserService, UserService>();

// trả về JSON ParcalCase
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

// enable cors
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins, policy =>
    {
        policy.WithOrigins("http://localhost:8080")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(myAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

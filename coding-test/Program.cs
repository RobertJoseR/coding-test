using coding_test.Data;
using coding_test.Data.Entities;
using coding_test.Repositories;
using coding_test.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the containe


builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<BankDBContext>(options => options.UseInMemoryDatabase(databaseName: "MyBank"));
builder.Services.AddTransient<AccountService>();
builder.Services.AddTransient<TransactionService>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Initialize DB with My Own and UniqueUser  (Robert Rodriguez)
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;

    using (var context = new BankDBContext(
            serviceProvider.GetRequiredService<DbContextOptions<BankDBContext>>()))
    {
        if (!context.Customers.Any())
        {
            context.Customers.Add(
           new Customer
           {
               ClientId = 1,
               FirstName = "Robert",
               LastName = "Rodriguez"
           });

            context.SaveChanges();
        }
    }
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();



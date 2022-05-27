var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.WebHost.UseSentry(options => {
    options.TracesSampler = 
        (data) => data.TransactionContext.Name.Contains("Wait") || 
                  data.TransactionContext.Name.Contains("GetDog") ? 0 : 1;
});
builder.Services.AddControllers();
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSentryTracing();

app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();

app.Run();

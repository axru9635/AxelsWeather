using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;



var builder = WebApplication.CreateBuilder(args);
var myAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin() 
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                      });
});


builder.Services.AddRateLimiter(_ => _
    .GlobalLimiter = PartitionedRateLimiter.CreateChained(
        PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        {
            var userAgent = httpContext.Request.Headers.UserAgent.ToString();

            return RateLimitPartition.GetSlidingWindowLimiter
            (userAgent, _ =>
                new SlidingWindowRateLimiterOptions
                {
                    //AutoReplenishment = true,  ?? what is this?
                    PermitLimit = 60,
                    SegmentsPerWindow = 100,
                    Window = TimeSpan.FromSeconds(60),
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    QueueLimit = 10
                });
        }),
        PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        {
            var userAgent = httpContext.Request.Headers.UserAgent.ToString();

            return RateLimitPartition.GetFixedWindowLimiter
            (userAgent, _ =>
                new FixedWindowRateLimiterOptions
                {
                    //AutoReplenishment = true,  ?? what is this?
                    PermitLimit = 1000,
                    Window = TimeSpan.FromDays(1),
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    QueueLimit = 10
                });
        })

    ));


// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.UseStaticFiles();

app.UseCors(myAllowSpecificOrigins);

app.UseRateLimiter();



app.Run();

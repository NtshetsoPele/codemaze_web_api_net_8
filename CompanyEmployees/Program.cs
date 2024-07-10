
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.ConfigureIisIntegration();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();
//app.UseStaticFiles(); No static content available
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
}); 
app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();

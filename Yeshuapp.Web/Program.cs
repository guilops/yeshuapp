var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddRazorPages();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<IrmaosServices>(client =>
{
    client.BaseAddress = new Uri("https://localhost:44337/");
});
builder.Services.AddHttpClient<ProdutosServices>(client =>
{
    client.BaseAddress = new Uri("https://localhost:44337/"); 
});
builder.Services.AddHttpClient<PedidosServices>(client =>
{
    client.BaseAddress = new Uri("https://localhost:44337/"); 
});
builder.Services.AddHttpClient<EventosServices>(client =>
{
    client.BaseAddress = new Uri("https://localhost:44337/");
});
builder.Services.AddHttpClient<FrasesServices>(client =>
{
    client.BaseAddress = new Uri("https://localhost:44337/");
});
builder.Services.AddHttpClient<AutenticacaoServices>(client =>
{
    client.BaseAddress = new Uri("https://localhost:44337/");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    // Redireciona a raiz para a página de Login
    endpoints.MapGet("/", async context =>
    {
        context.Response.Redirect("/Login");
        await Task.CompletedTask; // Garantir que o método seja assíncrono
    });
    endpoints.MapRazorPages();
});

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();

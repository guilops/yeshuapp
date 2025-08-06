public static class ServiceCollectionExtensions
{
    public static void AddCustomHttpClients(this IServiceCollection services, string baseUrl)
    {
        var baseAddress = new Uri(baseUrl);

        services.AddHttpClient<ProdutosServices>(client =>
        {
            client.BaseAddress = baseAddress;
        });

        services.AddHttpClient<PedidosServices>(client =>
        {
            client.BaseAddress = baseAddress;
        });

        services.AddHttpClient<EventosServices>(client =>
        {
            client.BaseAddress = baseAddress;
        });

        services.AddHttpClient<FrasesServices>(client =>
        {
            client.BaseAddress = baseAddress;
        });

        services.AddHttpClient<AutenticacaoServices>(client =>
        {
            client.BaseAddress = baseAddress;
        });

        services.AddHttpClient<IrmaosServices>(client =>
        {
            client.BaseAddress = baseAddress;
        });

        services.AddHttpClient<FluxoCaixaServices>(client =>
        {
            client.BaseAddress = baseAddress;
        });

        services.AddHttpClient<DespesasServices>(client =>
        {
            client.BaseAddress = baseAddress;
        });
    }
}
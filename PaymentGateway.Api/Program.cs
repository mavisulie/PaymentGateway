using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Api.Client;
using PaymentGateway.Api.Client.Interfaces;
using PaymentGateway.Api.Interfaces;
using PaymentGateway.Api.Services;
using PaymentGateway.Api.Validators;
using PaymentGateway.Domain.Entities;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(worker => worker.UseNewtonsoftJson())
    .ConfigureOpenApi()
    .ConfigureServices((services) =>
    {
        services.AddDbContext<PaymentContext>();
        services.AddScoped<IPaymentsRepository, PaymentRepository>();
        services.AddHttpClient();
        services.AddTransient<IPaymentService, PaymentService>();
        services.AddTransient<ICKOBankClient, CKOBankClientMock>();
        services.AddScoped<PaymentRequestValidator>();
    })
    .Build();

host.Run();
// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using PublicacaoMetricas.Abstractions.Application;
using PublicacaoMetricas.Common;
using PublicacaoMetricas.Configuration;

var services = new ServiceCollection();
var startup = new Startup();
startup.ConfigureServices(services);

var serviceProvider = services.BuildServiceProvider();
var checklist = new CriarChecklist("Checklist 1", "Descrição da checklist 1");
var useCase = serviceProvider.GetRequiredService<IUseCase<CriarChecklist, ServiceResult>>();
var result = await useCase.ExecuteAsync(checklist);

Console.WriteLine(result);
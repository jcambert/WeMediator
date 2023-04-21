// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using We.Results;
using WeMediator;

Console.WriteLine("Hello, World!");
var services = new ServiceCollection();
services.AddMediator();

var sp = services.BuildServiceProvider();

var mediator = sp.GetRequiredService<IMediator>();
try
{

    var result = await mediator.Send(new Ping(default));
    Console.WriteLine(result);
    if (result)
        Console.WriteLine(result.Value.ToString());
    if((result.IsValidFailure && result.HasError) || !result)
    {
        Console.WriteLine(result.Errors.AsString(!result?"[ERR]:":""));
    }
}
catch (Exception ex)
{
    Console.Error.WriteLine(ex);

}
/*
var result1 = await mediator.Send(new Ping1(Guid.NewGuid()));
Console.WriteLine(result1);
Console.WriteLine(result1.Value.ToString());
*/
Console.ReadLine();
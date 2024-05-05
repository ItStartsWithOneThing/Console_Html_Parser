
using Console_Html_Parser.StaticTools;

public class Program
{
    static async Task Main(string[] args)
    {
        var articulMock = "30UN0500";

        await Task.Factory.StartNew(() => ServiceLauncher.Launch());

        Console.ReadLine();
    }
}
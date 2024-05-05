
using Console_Html_Parser.Models.romstal.ua;
using Console_Html_Parser.Services.romstal.ua;

namespace Console_Html_Parser.StaticTools
{
    public static class ServiceLauncher
    {
        public static async Task Launch()
        {
            var articuls = Tools.GetArticulsFromConsoleInput();

            var bimetalRadiatorService = new BimetallicRadiatorService();

            var targetItems = await bimetalRadiatorService.GetTargetItems(articuls);

            await Console.Out.WriteLineAsync("ShowItem<T>");
            Tools.ShowItem(targetItems[0]);

            //var targetvalues = bimetalRadiatorService.GetTargetItemsValues();

            //Console.WriteLine("ShowItems");
            //Tools.ShowItems(BimetallicRadiator.PropertiesNames.ToList(), targetvalues);
        }
    }
}

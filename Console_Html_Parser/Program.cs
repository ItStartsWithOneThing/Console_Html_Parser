
using Console_Html_Parser.Services.romstal.ua;
using Console_Html_Parser.StaticTools;

var articul = "30UN0500";

var bimetalRadiatorService = new BimetallicRadiatorService(articul);

var result = await bimetalRadiatorService.GetTargetItemValuesList();

Tools.ShowItem(result);

Console.ReadLine();
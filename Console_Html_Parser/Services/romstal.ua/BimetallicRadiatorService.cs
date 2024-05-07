
using Console_Html_Parser.Models.romstal.ua;
using Console_Html_Parser.Requests;
using Console_Html_Parser.StaticTools;
using System.Net;

namespace Console_Html_Parser.Services.romstal.ua
{
    public class BimetallicRadiatorService
    {
        private List<BimetallicRadiator> _targetItems;

        private List<List<string>> _targetItemsValues;

        private GetRequestForHtmlParsing Request { get; set; }

        private string Host { get; } = "romstal.ua";
        private string Address { get; set; }
        private string Referrer { get; set; }
        private string AcceptHeader { get; } = "*/*";
        private Dictionary<string, string> Headers { get; set; }
        private string Proxy { get; } = "127.0.0.1:8888";
        private CookieContainer Cookies { get; set; } = new CookieContainer();

        public BimetallicRadiatorService()
        {
            _targetItems = new List<BimetallicRadiator>();

            _targetItemsValues = new List<List<string>>();

            Headers = new Dictionary<string, string>
            {
                { "sec-ch-ua", "\"Chromium\";v=\"124\", \"Microsoft Edge\";v=\"124\", \"Not-A.Brand\";v=\"99\"" },
                { "sec-ch-ua-mobile", "?0" },
                { "sec-ch-ua-platform", "\"Windows\"" },
                { "Sec-Fetch-Dest", "document" },
                { "Sec-Fetch-Mode", "navigate" },
                { "Sec-Fetch-Site", "same-origin" },
                { "Sec-Fetch-User", "?1" },
                { "Upgrade-Insecure-Requests", "1" }
            };
        }

        /// <summary>
        /// Fetching needed items from web-site
        /// </summary>
        /// <returns>Traget item instance</returns>
        public async Task<List<BimetallicRadiator>> GetTargetItems(List<string> articuls)
        {
            try 
            {
                foreach (var articul in articuls)
                {
                    Address = $"https://{Host}/uk/catalog/search?search={articul}";
                    Referrer = $"https://{Host}";

                    Request = new GetRequestForHtmlParsing(
                    AcceptHeader,
                    Host,
                    Headers,
                    Proxy);

                    var searchResult = await Request.Run(Address, Referrer, Cookies);

                    Console.WriteLine(" 65 var searchResult = await Request.Run(Address, Referrer, Cookies);"); // ИНДИКАТОР ВЫПОЛНЕНИЯ ДЛЯ КОНСОЛИ

                    var targetPath = Tools.FindValue(searchResult, "<div class=\"product__card__img\">", "<a\thref=\"", "\"");

                    Referrer = Address;
                    Address = $"https://{Host}{targetPath}";

                    var resultingHtml = await Request.Run(Address, Referrer, Cookies);

                    Console.WriteLine("Launched 74 var resultingHtml = await Request.Run(Address, Referrer, Cookies);"); // ИНДИКАТОР ВЫПОЛНЕНИЯ ДЛЯ КОНСОЛИ

                    Create(resultingHtml);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error in method \"GetTargetItems\": {ex.Message}.");
            }
            

            return _targetItems;
        }

        /// <summary>
        /// Getting list of collections with values of target items. Call this method only after "GetTargetItems"
        /// </summary>
        /// <returns>Item values</returns>
        public List<List<string>> GetTargetItemsValues()
        {
            if(_targetItemsValues == null || _targetItemsValues.Count == 0)
            {
                Console.WriteLine($"Error in method \"GetTargetItemsValues\". GetTargetItemsValues is null. You must call \"GetTargetItems\" before.");
            }
            
            return _targetItemsValues;
        }

        private void Create(string html)
        {
            var targetItem = new BimetallicRadiator()
            {
                Brand = Tools.FindValue(html, "class=\"item__right product-data-ga\"", "data-brand=\"", "\""),
                Code = Tools.FindValue(html, "class=\"item__right product-data-ga\"", "data-article=\"", "\""),
                ManufacturerCode = Tools.FindValue(html, "<p class=\"item__info__side-title\">Код виробника:</p>", "<p class=\"item__info__side-sub\">", "<"),
                Name = Tools.FindValue(html, "class=\"item__right product-data-ga\"", "data-name=\"", "\""),
                Price = decimal.Parse(Tools.FindValue(html, "class=\"item__right product-data-ga\"", "data-price=\"", "\"").Replace('.', ',')),
                Weight = double.Parse(Tools.FindValue(html, "class=\"spec__title\">Вага", "<span class=\"spec__sub\">", "<").Replace('.', ',')),
                TestPressureInBar = Int32.Parse(Tools.FindValue(html, "<span class=\"spec__title\">Випробний тиск, бар</span>", "<span class=\"spec__sub\">", "<")),
                SectionHeightInMM = Int32.Parse(Tools.FindValue(html, "<span class=\"spec__title\">Висота секції, мм</span>", "<span class=\"spec__sub\">", "<")),
                HeightInMM = Int32.Parse(Tools.FindValue(html, "<span class=\"spec__title\">Висота, мм</span>", "class=\"link\">", "<")),
                GuaranteeInYears = Int32.Parse(Tools.FindValue(html, "<span class=\"spec__title\">Гарантія, років</span>", "<span class=\"spec__sub\">", "<")),
                SectionDepthInMM = Int32.Parse(Tools.FindValue(html, "<span class=\"spec__title\">Глибина секції, мм</span>", "<span class=\"spec__sub\">", "<")),
                Color = Tools.FindValue(html, "<span class=\"spec__title\">Колір</span>", "<span class=\"spec__sub\">", "<"),
                CenterDistanceInMM = Int32.Parse(Tools.FindValue(html, "<span class=\"spec__title\">Міжосьова відстань, мм</span>", "<span class=\"spec__sub\">", "<")),
                MaxCoolantTemperatureInCelsium = Int32.Parse(Tools.FindValue(html, "<span class=\"spec__title\">Макс. температура теплоносія,", "<span class=\"spec__sub\">", "<")),
                WorkingPressureInBar = Int32.Parse(Tools.FindValue(html, "<span class=\"spec__title\">Робочий тиск, бар</span>", "<span class=\"spec__sub\">", "<")),
                HeatOutputWithDefaultTemperatureInW = Int32.Parse(Tools.FindValue(html, "<span class=\"spec__title\">Тепловіддача", "<span class=\"spec__sub\">", "<")),
                Description = Tools.FindValue(html, "<h4>Опис</h4>", "<p>", "<")
            };

            _targetItems.Add(targetItem);

            var itemValues = new List<string>()
            {
                targetItem.Brand,
                targetItem.Code,
                targetItem.ManufacturerCode,
                targetItem.Name,
                targetItem.Price.ToString(),
                targetItem.Weight.ToString(),
                targetItem.TestPressureInBar.ToString(),
                targetItem.SectionHeightInMM.ToString(),
                targetItem.HeightInMM.ToString(),
                targetItem.GuaranteeInYears.ToString(),
                targetItem.SectionDepthInMM.ToString(),
                targetItem.Color,
                targetItem.CenterDistanceInMM.ToString(),
                targetItem.MaxCoolantTemperatureInCelsium.ToString(),
                targetItem.WorkingPressureInBar.ToString(),
                targetItem.HeatOutputWithDefaultTemperatureInW.ToString(),
                targetItem.Description
            };

            _targetItemsValues.Add(itemValues);
        }
    }
}
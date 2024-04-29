
using Console_Html_Parser.Models.romstal.ua;
using Console_Html_Parser.Requests;
using Console_Html_Parser.StaticTools;
using System.Net;

namespace Console_Html_Parser.Services.romstal.ua
{
    public class BimetallicRadiatorService
    {
        public BimetallicRadiator TargetItem { get; private set; }

        public string Host { get; set; } = "romstal.ua";
        public string Articul { get; set; }
        public string Address { get; set; }
        public string Referrer { get; set; }
        public string AcceptHeader { get; set; } = "*/*";
        public Dictionary<string, string> Headers { get; set; }
        public string Proxy { get; set; } = "127.0.0.1:8888";
        public CookieContainer Cookies { get; set; } = new CookieContainer();

        public BimetallicRadiatorService(string articul)
        {
            Address = articul;
            Address = $"https://{Host}/uk/catalog/search?search={articul}";
            Referrer = $"https://{Host}";

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

        public async Task<BimetallicRadiator> GetTargetItemInfo()
        {
            var searchResult = await GetHtml();

            var targetPath = Tools.FindValue(searchResult, "<div class=\"product__card__img\">", "<a\thref=\"", "\"");

            Referrer = Address;
            Address = $"https://{Host}{targetPath}";

            var resultingHtml = await GetHtml();

            TargetItem = new BimetallicRadiator(resultingHtml);

            return await Task.Run(() => TargetItem);
        }

        private async Task<string> GetHtml()
        {
            var request = new GetRequestForHtmlParsing(
                Address,
                AcceptHeader,
                Host,
                Referrer,
                Headers,
                Proxy);

            await request.Run(Cookies);

            return await Task.Run(() => request.Response);
        }
    }
}

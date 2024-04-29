
using System.Net;
using System.Net.Http.Headers;

namespace Console_Html_Parser.Requests
{
    public class GetRequestForHtmlParsing
    {
        private readonly string _address;

        public string Response { get; set; }
        private MediaTypeWithQualityHeaderValue AcceptHeader { get; set; }
        private string HostHeader { get; set; }
        private ProductInfoHeaderValue[] UserAgentHeaderParts { get; set; }
        private Uri ReferrerHeader { get; set; }
        private Dictionary<string, string> CustomHeaders { get; set; } = new Dictionary<string, string>();
        private WebProxy Proxy { get; set; }

        public GetRequestForHtmlParsing(
            string address,
            string acceptHeader,
            string hostHeader,
            string referrerHeader,
            Dictionary<string, string> customHeaders,
            string proxy)
        {
            _address = address;

            UserAgentHeaderParts = new ProductInfoHeaderValue[]
                {
                    new ProductInfoHeaderValue("Mozilla", "5.0"),
                    new ProductInfoHeaderValue("(Windows NT 10.0; Win64; x64)"),
                    new ProductInfoHeaderValue("AppleWebKit", "537.36"),
                    new ProductInfoHeaderValue("(KHTML, like Gecko)"),
                    new ProductInfoHeaderValue("Chrome", "124.0.0.0"),
                    new ProductInfoHeaderValue("Safari", "537.36"),
                    new ProductInfoHeaderValue("Edg", "124.0.0.0")
                };

            AcceptHeader = new MediaTypeWithQualityHeaderValue(acceptHeader);
            HostHeader = hostHeader;
            ReferrerHeader = new Uri(referrerHeader);
            CustomHeaders = customHeaders;
            Proxy = new WebProxy(proxy);
        }

        public async Task Run(CookieContainer cookieContainer)
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.CookieContainer = cookieContainer;
            httpHandler.Proxy = Proxy;

            using (var client = new HttpClient(httpHandler))
            {
                #region InitHeaders
                client.DefaultRequestHeaders.Accept.Add(AcceptHeader);
                client.DefaultRequestHeaders.Host = HostHeader;
                client.DefaultRequestHeaders.Referrer = ReferrerHeader;

                client.DefaultRequestHeaders.UserAgent.Clear();
                foreach (var item in UserAgentHeaderParts)
                {
                    client.DefaultRequestHeaders.UserAgent.Add(item);
                }

                foreach (var header in CustomHeaders)
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
                #endregion InitHeaders


                try
                {
                    HttpResponseMessage response = await client.GetAsync(_address);

                    if (response.IsSuccessStatusCode)
                    {
                        Response = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        Console.WriteLine($"Ошибка: status coe {(int)response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка выполнения запроса: {ex.Message}");
                }
            }
        }
    }
}

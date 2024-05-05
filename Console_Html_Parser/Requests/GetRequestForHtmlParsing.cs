
using System.Net;
using System.Net.Http.Headers;

namespace Console_Html_Parser.Requests
{
    public class GetRequestForHtmlParsing
    {
        private Uri ReferrerHeader { get; set; }

        public string Response { get; private set; }

        private MediaTypeWithQualityHeaderValue AcceptHeader { get; set; }
        private string HostHeader { get; set; }
        private ProductInfoHeaderValue[] UserAgentHeaderParts { get; set; }
        private Dictionary<string, string> CustomHeaders { get; set; } = new Dictionary<string, string>();
        private WebProxy Proxy { get; set; }

        public GetRequestForHtmlParsing(
            string acceptHeader,
            string hostHeader,
            Dictionary<string, string> customHeaders,
            string proxy)
        {
            UserAgentHeaderParts =
                [
                    new ProductInfoHeaderValue("Mozilla", "5.0"),
                    new ProductInfoHeaderValue("(Windows NT 10.0; Win64; x64)"),
                    new ProductInfoHeaderValue("AppleWebKit", "537.36"),
                    new ProductInfoHeaderValue("(KHTML, like Gecko)"),
                    new ProductInfoHeaderValue("Chrome", "124.0.0.0"),
                    new ProductInfoHeaderValue("Safari", "537.36"),
                    new ProductInfoHeaderValue("Edg", "124.0.0.0")
                ];

            AcceptHeader = new MediaTypeWithQualityHeaderValue(acceptHeader);
            HostHeader = hostHeader;
            
            CustomHeaders = customHeaders;
            Proxy = new WebProxy(proxy);
        }

        public async Task<string> Run(string address, string referrerHeader, CookieContainer cookieContainer)
        {
            ReferrerHeader = new Uri(referrerHeader);

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
                    HttpResponseMessage response = await client.GetAsync(address);

                    if (response.IsSuccessStatusCode)
                    {
                        Response = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        await Console.Out.WriteLineAsync($"Ошибка: status code {(int)response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    await Console.Out.WriteLineAsync($"Ошибка выполнения запроса: {ex.Message}. URI: {address}");
                }
            }

            return Response;
        }
    }
}

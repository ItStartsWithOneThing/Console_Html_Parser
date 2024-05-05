
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace Console_Html_Parser.Requests
{
    public class PostRequestForHtmlParsing
    {
        private readonly string _address;

        public string Response { get; set; }
        public string RequestBody { get; set; }
        public MediaTypeWithQualityHeaderValue AcceptHeader { get; set; }
        public string HostHeader { get; set; }
        public ProductInfoHeaderValue[] UserAgentHeaderParts { get; set; }
        public string ReferrerHeader { get; set; }
        public Dictionary<string, string> CustomHeaders { get; set; }
        public WebProxy Proxy { get; set; }
        public HttpContent Content { get; set; }

        public PostRequestForHtmlParsing(string address)
        {
            _address = address;
            CustomHeaders = new Dictionary<string, string>();
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
                client.DefaultRequestHeaders.Referrer = new Uri(ReferrerHeader);
                client.DefaultRequestHeaders.UserAgent.Clear();
                foreach (var item in UserAgentHeaderParts)
                {
                    client.DefaultRequestHeaders.UserAgent.Add(item);
                }

                foreach (var header in CustomHeaders)
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }

                Content = new StringContent(RequestBody, Encoding.UTF8, new MediaTypeHeaderValue("application/json"));
                Content.Headers.ContentLength = RequestBody.Length;
                #endregion InitHeaders

                try
                {
                    HttpResponseMessage response = await client.PostAsync(_address, Content);

                    if (response.IsSuccessStatusCode)
                    {
                        Response = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        await Console.Out.WriteLineAsync($"Ошибка: status coe {(int)response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    await Console.Out.WriteLineAsync($"Ошибка выполнения запроса: {ex.Message}");
                }
            }
        }
    }
}

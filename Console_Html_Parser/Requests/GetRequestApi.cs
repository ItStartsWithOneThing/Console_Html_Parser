
using Console_Html_Parser.Models.MockApi;
using System.Net.Http.Json;

namespace Console_Html_Parser.Requests
{
    public class GetRequestApi
    {
        private readonly string _address;
        public ICollection<string> Response { get; set; }
        public bool IsError { get; set; }
        public string ErrorText { get; set; }

        public GetRequestApi(string address)
        {
            _address = address;
        }

        public async Task Run()
        {
            // Use HttpWebRequest for .net core 2.1 and older
            using (var client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(_address);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadFromJsonAsync<ICollection<ShopItem>>();

                        Response = responseBody.Select(x => x.Title).ToList();
                    }
                    else
                    {
                        IsError = true;
                        ErrorText = $"Ошибка: status coe {(int)response.StatusCode}";
                        Console.WriteLine(ErrorText);
                    }
                }
                catch (Exception ex)
                {
                    IsError = true;
                    ErrorText = $"Ошибка выполнения запроса: {ex.Message}";
                    Console.WriteLine(ErrorText);
                }
            }
        }
    }
}

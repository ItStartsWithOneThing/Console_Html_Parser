
using Console_Html_Parser.Requests;
using Console_Html_Parser.Secrets;

namespace Console_Html_Parser.Services.MockApi
{
    public class ShopItemService
    {
        private readonly string Secret = ApiKeys.MockApiSecret;

        public async Task GetByApi()
        {
            var request = new GetRequestApi($"https://{Secret}.mockapi.io/shop-items");

            await request.Run();

            if (!request.IsError)
            {
                foreach (var item in request.Response)
                {
                    Console.WriteLine(item);
                }
            }
            else
            {
                Console.WriteLine(request.ErrorText);
            }
        }
    }
}

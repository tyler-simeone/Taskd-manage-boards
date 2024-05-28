using System.Net.Http.Headers;
using manage_boards.src.models;

namespace manage_boards.src.clients
{
    public class ColumnsClient : IColumnsClient
    {
        private IConfiguration _configuration;
        private readonly HttpClient _client;
        private readonly IAuthClient _authClient;

        public ColumnsClient(IConfiguration configuration, IAuthClient authClient)
        {
            _configuration = configuration;
            _client = new HttpClient
            {
                BaseAddress = new Uri(_configuration.GetConnectionString("ManageColumnsLocalConnection"))
            };
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _authClient = authClient;
        }

        public async Task<List<Column>> GetColumns(int boardId, int userId)
        {
            var bearerToken = await _authClient.GetBearerToken();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken.access_token);
            HttpResponseMessage response = await _client.GetAsync($"/api/Columns?boardId={boardId}&userId={userId}");

            if (response.IsSuccessStatusCode)
            {
                var columnList = await response.Content.ReadAsAsync<ColumnList>();
                return columnList.Columns;
            }
            else 
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }
}
using manage_boards.src.models;

namespace manage_boards.src.clients
{
    public class ColumnsClient : IColumnsClient
    {
        private IConfiguration _configuration;
        private readonly HttpClient _client;

        public ColumnsClient(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = new HttpClient
            {
                BaseAddress = new Uri(_configuration.GetConnectionString("ManageColumnsLocalConnection"))
            };
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<ColumnList> GetColumns(int boardId, int userId)
        {
            HttpResponseMessage response = await _client.GetAsync($"/api/Columns/{boardId}?userId={userId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<ColumnList>();
            }
            else 
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }
}
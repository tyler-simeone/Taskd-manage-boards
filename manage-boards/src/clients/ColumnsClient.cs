using System.Net.Http.Headers;
using manage_boards.src.models;

namespace manage_boards.src.clients
{
    public class ColumnsClient : IColumnsClient
    {
        private IConfiguration _configuration;
        private readonly HttpClient _client;
        private readonly IAuthClient _authClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ColumnsClient(IConfiguration configuration, 
                             IAuthClient authClient, 
                             IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _client = new HttpClient
            {
                BaseAddress = new Uri(_configuration.GetConnectionString("ManageColumnsLocalConnection"))
            };
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _authClient = authClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<Column>> GetColumns(int boardId, int userId)
        {
            // check for existing auth token in request and reuse that instead of generating new...
            // if we're at this point then client has auth token
            var context = _httpContextAccessor.HttpContext;
            var accessToken = context.Request.Headers["Authorization"].ToString().Split(' ')[1]; // remove "Bearer " prefix from header
            var bearerToken = new TokenResponse();
            if (accessToken == String.Empty)
                bearerToken = await _authClient.GetBearerToken();
            else 
                bearerToken.access_token = accessToken;
                
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
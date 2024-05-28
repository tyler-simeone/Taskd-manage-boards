using manage_boards.src.models;

namespace manage_boards.src.clients
{
    public interface IColumnsClient
    { 
        public Task<ColumnList> GetColumns(int boardId, int userId);
    }
}
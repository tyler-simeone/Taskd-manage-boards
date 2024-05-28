using manage_boards.src.clients;
using manage_boards.src.dataservice;
using manage_boards.src.models;
using manage_boards.src.models.requests;
using Mysqlx.Cursor;

namespace manage_boards.src.repository
{
    public class BoardsRepository : IBoardsRepository
    {
        IBoardsDataservice _boardsDataservice;
        IColumnsClient _columnsClient;

        public BoardsRepository(IBoardsDataservice boardsDataservice, IColumnsClient columnsClient)
        {
            _boardsDataservice = boardsDataservice;
            _columnsClient = columnsClient;
        }

        public async Task<Board> GetBoard(int boardId, int userId)
        {
            try
            {
                var board = await _boardsDataservice.GetBoard(boardId, userId); 
                FetchAndAppendColumns(boardId, userId, board);
                return board;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

        public async Task<BoardList> GetBoards(int userId)
        {
            try
            {
                BoardList boardList = await _boardsDataservice.GetBoards(userId);
                boardList.Boards.ForEach(board => FetchAndAppendColumns(board.BoardId, userId, board));
                return boardList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

        public void CreateBoard(CreateBoard createBoardRequest)
        {
            try
            {
                _boardsDataservice.CreateBoard(createBoardRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

        public void UpdateBoard(UpdateBoard updateBoardRequest)
        {
            try
            {
                _boardsDataservice.UpdateBoard(updateBoardRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

        public void DeleteBoard(int boardId, int userId)
        {
            try
            {
                _boardsDataservice.DeleteBoard(boardId, userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

        private async void FetchAndAppendColumns(int boardId, int userId, Board board)
        {
            var columns = await _columnsClient.GetColumns(boardId, userId);
            board.Columns.AddRange(columns);
        }
    }
}
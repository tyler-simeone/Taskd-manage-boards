using System.Data;
using manage_boards.src.models;
using manage_boards.src.models.requests;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;

namespace manage_boards.src.dataservice
{
    public class BoardsDataservice : IBoardsDataservice
    { 
        private IConfiguration _configuration;
        private string _conx;

        public BoardsDataservice(IConfiguration configuration)
        {
            _configuration = configuration;
            _conx = _configuration["LocalDBConnection"];

            if (_conx.IsNullOrEmpty())
                _conx = _configuration.GetConnectionString("LocalDBConnection");
        }
        
        public async Task<BoardDetails> GetBoard(int boardId, int userId)
        {
            using (MySqlConnection connection = new(_conx))
            {
                using (MySqlCommand command = new("taskd_db_dev.BoardGetByUserIdAndBoardId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    
                    command.Parameters.AddWithValue("@paramUserId", userId);
                    command.Parameters.AddWithValue("@paramBoardId", boardId);

                    try
                    {
                        await connection.OpenAsync();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                BoardDetails board = ExtractBoardDetailsFromReader(reader);
                                return board;
                            }

                            return new BoardDetails();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        throw;
                    }
                }
            }
        }

        public async Task<BoardList> GetBoards(int userId)
        {
            using (MySqlConnection connection = new(_conx))
            {
                using (MySqlCommand command = new("taskd_db_dev.BoardGetListByUserId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@paramUserId", userId);

                    try
                    {
                        await connection.OpenAsync();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            var boardList = new BoardList();

                            while (reader.Read())
                            {
                                Board board = ExtractBoardFromReader(reader);
                                boardList.Boards.Add(board);
                            }

                            return boardList;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        throw;
                    }
                }
            }
        }

        public async void CreateBoard(CreateBoard createBoardRequest)
        {
            using (MySqlConnection connection = new(_conx))
            {
                using (MySqlCommand command = new("taskd_db_dev.BoardPersist", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@paramUserId", createBoardRequest.UserId);
                    command.Parameters.AddWithValue("@paramBoardName", createBoardRequest.BoardName);
                    command.Parameters.AddWithValue("@paramBoardDescription", createBoardRequest.BoardDescription);
                    command.Parameters.AddWithValue("@paramCreateUserId", createBoardRequest.UserId);

                    try
                    {
                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        throw;
                    }
                }
            }
        }

        public async void UpdateBoard(UpdateBoard updateBoardRequest)
        {

            using (MySqlConnection connection = new(_conx))
            {
                using (MySqlCommand command = new("taskd_db_dev.BoardUpdate", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@paramBoardId", updateBoardRequest.BoardId);
                    command.Parameters.AddWithValue("@paramBoardName", updateBoardRequest.BoardName);
                    command.Parameters.AddWithValue("@paramBoardDescription", updateBoardRequest.BoardDescription);
                    command.Parameters.AddWithValue("@paramUpdateUserId", updateBoardRequest.UserId);

                    try
                    {
                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        throw;
                    }
                }
            }
        }

        public async void DeleteBoard(int boardId, int userId)
        {
            using (MySqlConnection connection = new(_conx))
            {
                using (MySqlCommand command = new("taskd_db_dev.BoardDelete", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@paramBoardId", boardId);
                    command.Parameters.AddWithValue("@paramUpdateUserId", userId);

                    try
                    {
                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        throw;
                    }
                }
            }
        }

        #region HELPERS
        
        private static Board ExtractBoardFromReader(MySqlDataReader reader)
        {
            int boardId = reader.GetInt32("BoardId");
            int userId = reader.GetInt32("UserId");
            string name = reader.GetString("BoardName");

            return new Board()
            {
                BoardId = boardId,
                UserId = userId,
                BoardName = name
            };
        }

        private static BoardDetails ExtractBoardDetailsFromReader(MySqlDataReader reader)
        {
            int boardId = reader.GetInt32("BoardId");
            int userId = reader.GetInt32("UserId");
            string name = reader.GetString("BoardName");
            string description = reader.GetString("BoardDescription");
            DateTime createDatetime = reader.GetDateTime("CreateDatetime");
            int createUserId = reader.GetInt32("CreateUserId");
            DateTime? updateDatetime = reader.IsDBNull(reader.GetOrdinal("UpdateDatetime")) ? null : reader.GetDateTime("UpdateDatetime");
            int? updateUserId = reader.IsDBNull(reader.GetOrdinal("UpdateUserId")) ? null : reader.GetInt32("UpdateUserId");

            return new BoardDetails()
            {
                BoardId = boardId,
                UserId = userId,
                BoardName = name,
                BoardDescription = description,
                CreateDatetime = createDatetime,
                CreateUserId = createUserId,
                UpdateDatetime = updateDatetime,
                UpdateUserId = updateUserId
            };
        }

        #endregion
    }
}
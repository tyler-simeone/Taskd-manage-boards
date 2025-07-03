using manage_boards.src.models.requests;

namespace manage_boards.src.util
{
    public interface IRequestValidator
    {
        bool ValidateGetBoard(int boardId, int userId);

        bool ValidateGetBoards(int userId);

        bool ValidateCreateBoard(CreateBoard createBoardRequest);

        bool ValidateUpdateBoardName(UpdateBoardName updateBoardRequest);

        bool ValidateUpdateBoard(UpdateBoard updateBoardRequest);

        bool ValidateDeleteBoard(int boardId, int userId);
    }

    public class RequestValidator : IRequestValidator
    {
        public RequestValidator()
        {

        }

        public bool ValidateGetBoard(int boardId, int userId)
        {
            return true;
        }

        public bool ValidateGetBoards(int userId)
        {
            return true;
        }

        public bool ValidateCreateBoard(CreateBoard createBoardRequest)
        {
            return true;
        }

        public bool ValidateUpdateBoardName(UpdateBoardName updateBoardRequest)
        {
            return true;
        }
        
        public bool ValidateUpdateBoard(UpdateBoard updateBoardRequest)
        {
            return true;
        }

        public bool ValidateDeleteBoard(int boardId, int userId)
        {
            return true;
        }
    }
}
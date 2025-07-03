namespace manage_boards.src.models.requests
{
    public class UpdateBoardName
    {
        public UpdateBoardName()
        {

        }

        public int UserId { get; set; }

        public int BoardId { get; set; }

        public string BoardName { get; set; }
    }
}
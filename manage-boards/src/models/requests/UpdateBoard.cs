namespace manage_boards.src.models.requests
{
    public class UpdateBoard
    {
        public UpdateBoard()
        {

        }

        public int UserId { get; set; }

        public int BoardId { get; set; }

        public string BoardName { get; set; }

        public string BoardDescription { get; set; }
    }
}
namespace manage_boards.src.models.requests
{
    public class CreateBoard
    {
        public CreateBoard()
        {

        }

        public int UserId { get; set; }

        public string BoardName { get; set; }

        public string BoardDescription { get; set; }
    }
}
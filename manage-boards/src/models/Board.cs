namespace manage_boards.src.models
{
    public class Board : ResponseBase
    {
        public Board()
        {
        }

        public int BoardId { get; set; }

        public int UserId { get; set; }

        public string BoardName { get; set; }
    }
}
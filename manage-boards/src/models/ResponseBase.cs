namespace manage_boards.src.models
{
    public class ResponseBase
    {
        public ResponseBase(int statusCode)
        {
            Status = statusCode;
        }

        public ResponseBase()
        {
            Status = 200;
        }

        public int Status { get; set; }
    }
}
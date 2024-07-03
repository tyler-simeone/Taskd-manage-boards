namespace manage_boards.src.models
{
    public class ErrorResponseBase
    {
        public ErrorResponseBase(int statusCode)
        {
            Status = statusCode;
        }

        public int Status;
    }
}
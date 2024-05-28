namespace manage_boards.src.models
{
    public class ColumnList : ResponseBase
    {
        public ColumnList()
        {
            Columns = [];
        }

        public List<Column> Columns { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }
}

namespace OnlineChess.Data
{
    public class FieldCell
    {
        public string Value { get; set; }
        public FieldCell(){}
        public string DefaultCellStyle { get; set; }
        public string CellStyle { get; set; }
        public FieldCell(string val, string style)
        {
            Value = val;
            DefaultCellStyle = style;
            CellStyle = style;
        }
    }
}
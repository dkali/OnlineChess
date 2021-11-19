
namespace OnlineChess.Data
{
    public class FieldCell
    {
        public string Value { get; set; }
        public FieldCell(){}
        public string DefaultCellStyle { get; set; }
        public FieldCell(string style)
        {
            Value = string.Empty;
            DefaultCellStyle = style;
        }

        public bool IsWhite()
        {
            return ChessFigures.WhiteFigures.Contains(Value);
        }

        public bool IsBlack()
        {
            return ChessFigures.BlackFigures.Contains(Value);
        }
    }
}
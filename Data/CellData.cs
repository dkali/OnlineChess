
namespace OnlineChess.Data
{
    public class CellData
    {
        public string Value { get; set; }
        public CellData(){}
        public string DefaultCellStyle { get; set; }
        public CellData(string style)
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
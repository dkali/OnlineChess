
namespace OnlineChess.Data
{
    public class FieldCell
    {
        public string Value { get; set; }
        public FieldCell(){}
        public FieldCell(string val)
        {
            Value = val;
        }
    }
}
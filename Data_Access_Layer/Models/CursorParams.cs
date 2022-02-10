namespace WebAPI.BLL.Models
{
    public class CursorParams
    {
        private int _count;

        public int Count
        {
            get { return _count; }
            set { _count = (value > 50) ? 50 : value; }
        }

        public int Cursor { get; set; } = 0;
    }
}

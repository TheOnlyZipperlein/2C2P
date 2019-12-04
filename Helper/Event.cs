namespace _2C2P.Helper
{
    class Event
    {
        public int x, y, type, data;
        public Event(int type, int data)
        {
            this.type = type;
            this.data = data;

        }
        public Event(int type, int x, int y)
        {
            this.type = type;
            this.x = x;
            this.y = y;
        }
    }
}

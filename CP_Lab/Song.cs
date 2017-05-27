namespace CP_Lab
{
    public class Song : LiteraryWork
    {
        public string Music { get; set; } = "";

        public Song()
        {
        }

        public Song(string name = "", int price = 0, string content = "") : base(name, price, content)
        {
        }

        public override string ToString()
        {
            return $"{base.ToString()}\t music = {Music}";
        }
    }
}
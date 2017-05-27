namespace CP_Lab
{
    public class Drama : LiteraryWork
    {
        public int SceneCount { get; set; }
        
        public Drama()
        {
        }

        public Drama(string name = "", int price = 0, string content = "") : base(name, price, content)
        {
        }

        public override string ToString()
        {
            return base.ToString() + $"\tsceneCount = {SceneCount}";
        }
    }

}
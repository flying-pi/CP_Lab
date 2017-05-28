namespace CP_Lab
{
    public class LiteraryWork : IProduct
    {
        public long SymbolCount { get; set; }
        public string Author { get; set; } = "";
        public string Content { get; set; } = "";
        public string Name { get; set; } = "";
        public int Price { get; set; }

        public LiteraryWork()
        {
            
        }

        public LiteraryWork(string name = "", int price = 0, string content = "") 
        {
            Name = name;
            Content = content;
            Price = price;
        }

        public override string ToString()
        {
            return
                $"name = {Name}\tauthor = {Author}\t content = {(Content.Length > 25 ? Content.Substring(0, 25) + "...." : Content)}";
        }
    }
}
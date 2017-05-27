namespace CP_Lab
{
    public abstract class LiteraryWork
    {
        public LiteraryWork(string content = "", string name = "", string author = "")
        {
            Author = author;
            Content = content;
            Name = name;
        }

        public long SymbolCount { get; set; }

        public string Author { get; set; }

        public string Name { get; set; }

        public string Content { get; set; };

        public override string ToString()
        {
            return
                $"name :: {Name}\t author :: {Author}\t content :: {(Content.Length > 25 ? Content.Substring(0, 25) + "...." : Content)}";
        }
    }
}
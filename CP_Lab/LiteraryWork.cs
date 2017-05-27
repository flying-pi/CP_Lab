namespace CP_Lab
{
    public abstract class LiteraryWork
    {

        private long _symbolsCount;
        private string _content = "";


        public LiteraryWork(string content = "", string name = "", string author = "")
        {
            Author = author;
            Content = content;
            Name = name;
        }

        public long SymbolCount{ get; set; }

        public string Author { get; set; }

        public string Name { get; set; }

        public string Content
        {
            get => _content;

            set
            {
                _symbolsCount = value.Length;
                _content = value;
            }
        }

        public override string ToString()
        {
            return $"name :: {Name}\t author :: {Author}\t content :: {(_content.Length > 25 ? _content.Substring(0, 25) + "...." : _content)}";
        }
    }
}
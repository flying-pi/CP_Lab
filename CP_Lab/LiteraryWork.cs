using System;

namespace CP_Lab
{
    public class LiteraryWork : IProduct
    {
        public long SymbolCount { get; set; }
        public string Author { get; set; } = "";
        public string Content { get; set; } = "";
        public string Name { get; set; } = "";
        private int _price;

        public int Price
        {
            get { return _price; }
            set
            {
                ProductEventArgs arg = new ProductEventArgs(value,_price);
                _price = value;
                OnPriceChange?.Invoke(this, arg);
            }
        }

        public event ChangedEventHandler OnPriceChange;

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

        public int CompareTo(object obj)
        {
            return obj == null ? 1 : String.CompareOrdinal(Name, ((LiteraryWork) obj).Name);
        }
    }
}
using System;

namespace CP_Lab
{
    
    public delegate void ChangedEventHandler(object sender, ProductEventArgs e);

    public class ProductEventArgs : EventArgs
    {
        public ProductEventArgs(int newPrice, int oldPrice)
        {
            NewPrice = newPrice;
            OldPrice = oldPrice;
        }

        public int NewPrice { get; }
        public int OldPrice { get; }
    }
    
    public interface IProduct: IComparable
    {
        string Name { get; set; }

        int Price { get; set; }
        
        event ChangedEventHandler OnPriceChange;
    }
}
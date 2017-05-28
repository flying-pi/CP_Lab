using System;

namespace CP_Lab
{
    public interface IProduct: IComparable
    {
        string Name { get; set; }

        int Price { get; set; }
    }
}
using BoxesProject.API;
using DataStructures;
using System;

namespace BoxesProject
{
    internal class Stock : IComparable<Stock>, IUIStock
    {
        internal QueueListNode<Stock> node;

        public DateTime LastPurchaseDate { get; set; }
        public int Amount { get; set; }
        public Box BoxType { get; set; }

        IUIBox IUIStock.BoxType => BoxType;

        public int CompareTo(Stock other) => BoxType.CompareTo(other.BoxType);
    }
}

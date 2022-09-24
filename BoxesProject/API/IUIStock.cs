using System;

namespace BoxesProject.API
{
    public interface IUIStock
    {
        int Amount { get; }
        IUIBox BoxType { get; }
        DateTime LastPurchaseDate { get; }
    }
}

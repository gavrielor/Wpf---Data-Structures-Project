using BoxesProject;
using BoxesProject.API;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace WpfBoxesProjectUI
{
    public class Notifier : INotifier
    {
        public void ShowMessage(string message)
        {
            MessageBox.Show(message, "Message");
        }

        public bool ShowUserShoppingCartAndReturnAnswer(List<IUIStock> stocksToBuy)
        {
            StringBuilder shoppingCart = new StringBuilder("We found the following boxes:\n\n");
            foreach (var item in stocksToBuy)
            {
                shoppingCart.Append($"Width: {item.BoxType.Width}, Height: {item.BoxType.Height}, Amount: {item.Amount}\n");
            }
            shoppingCart.Append("\nWould you like to buy them?");
            if (MessageBox.Show(shoppingCart.ToString(), "Purchase Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) return true;
            return false;
        }
    }
}

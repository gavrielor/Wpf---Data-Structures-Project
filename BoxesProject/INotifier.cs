using BoxesProject.API;
using System.Collections.Generic;

namespace BoxesProject
{
    public interface INotifier
    {
        void ShowMessage(string message);

        bool ShowUserShoppingCartAndReturnAnswer(List<IUIStock> stocksToBuy);
    }
}

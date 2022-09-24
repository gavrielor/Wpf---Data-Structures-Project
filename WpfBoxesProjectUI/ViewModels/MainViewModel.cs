using CommonServiceLocator;
using GalaSoft.MvvmLight;
using System.Collections.Generic;

namespace WpfBoxesProjectUI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string menuSelectedItem;
        private ViewModelBase selectedViewModel;
        private Dictionary<string, ViewModelBase> menuDictionery;

        public string MenuSelectedItem
        {
            get => menuSelectedItem;
            set
            {
                menuSelectedItem = value;
                SelectedViewModel = menuDictionery[menuSelectedItem];
            }
        }
        public ViewModelBase SelectedViewModel
        {
            get => selectedViewModel;
            set => Set(ref selectedViewModel, value);
        }
        public Dictionary<string, ViewModelBase> MenuDictionery { get => menuDictionery; set => menuDictionery = value; }

        public MainViewModel()
        {
            menuDictionery = new Dictionary<string, ViewModelBase>
            {
                ["Supply"] = ServiceLocator.Current.GetInstance<SupplyViewModel>(),
                ["Show Stock Data"] = ServiceLocator.Current.GetInstance<ShowStockDataViewModel>(),
                ["Buy"] = ServiceLocator.Current.GetInstance<BuyViewModel>(),
                ["Show Boxes By Date"] = ServiceLocator.Current.GetInstance<ShowBoxesByDateViewModel>()
            };
        }
    }
}
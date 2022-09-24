using GalaSoft.MvvmLight;
using System.Linq;

namespace WpfBoxesProjectUI.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        private MainViewModel mainVM;
        private string selectedItem;

        public string[] MenuList => mainVM.MenuDictionery.Keys.ToArray();
        public string SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value;
                mainVM.MenuSelectedItem = selectedItem;
            }
        }

        public MenuViewModel(MainViewModel mainViewModel)
        {
            mainVM = mainViewModel;
            SelectedItem = MenuList.First();
        }
    }
}

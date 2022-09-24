using BoxesProject;
using BoxesProject.API;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Command;
using WpfBoxesProjectUI.DataValidation;

namespace WpfBoxesProjectUI.ViewModels
{
    public class ShowStockDataViewModel : ValidatableViewModelBase
    {
        private Manager manager;
        private IUIStock searchedStock;

        public RelayCommand ShowStockDataCommand { get; set; }

        [DataType(DataType.PositiveDecimalNumber)]
        public string ButtomSize { get; set; } = "1.0";
        [DataType(DataType.PositiveDecimalNumber)]
        public string HeightSize { get; set; } = "1.0";

        public IUIStock SearchedStock
        {
            get => searchedStock;
            set => Set(ref searchedStock, value);
        }

        public ShowStockDataViewModel()
        {
            manager = ServiceLocator.Current.GetInstance<Manager>();
            ShowStockDataCommand = new RelayCommand(() => SearchedStock = manager.ShowStockData(double.Parse(ButtomSize), double.Parse(HeightSize)),
                () => !HasError);
        }
    }
}

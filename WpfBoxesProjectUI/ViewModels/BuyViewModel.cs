using BoxesProject;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Command;
using WpfBoxesProjectUI.DataValidation;

namespace WpfBoxesProjectUI.ViewModels
{
    public class BuyViewModel : ValidatableViewModelBase
    {
        private Manager manager;

        public RelayCommand SupplyCommand { get; set; }

        [DataType(DataType.PositiveDecimalNumber)]
        public string ButtomSize { get; set; } = "1.0";
        [DataType(DataType.PositiveDecimalNumber)]
        public string HeightSize { get; set; } = "1.0";
        [DataType(DataType.PositiveNumber)]
        public string Amount { get; set; } = "1";

        public BuyViewModel()
        {
            manager = ServiceLocator.Current.GetInstance<Manager>();
            SupplyCommand = new RelayCommand(() => manager.Buy(double.Parse(ButtomSize), double.Parse(HeightSize), int.Parse(Amount)), () => !HasError);
        }
    }
}
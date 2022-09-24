using BoxesProject;
using BoxesProject.API;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using WpfBoxesProjectUI.DataValidation;

namespace WpfBoxesProjectUI.ViewModels
{
    public class ShowBoxesByDateViewModel : ValidatableViewModelBase
    {
        private Manager manager;
        private List<IUIBox> boxesList;

        public RelayCommand ShowCommand { get; set; }

        [DataType(DataType.PositiveNumberOrZero)]
        public string Days { get; set; } = "000";
        [DataType(DataType.PositiveNumberOrZero)]
        public string Hours { get; set; } = "00";
        [DataType(DataType.PositiveNumberOrZero)]
        public string Minutes { get; set; } = "00";
        [DataType(DataType.PositiveNumberOrZero)]
        public string Seconds { get; set; } = "00";

        private TimeSpan Time() => new TimeSpan(int.Parse(Days), int.Parse(Hours), int.Parse(Minutes), int.Parse(Seconds));

        public List<IUIBox> BoxesList
        {
            get => boxesList;
            set => Set(ref boxesList, value);
        }

        public ShowBoxesByDateViewModel()
        {
            manager = ServiceLocator.Current.GetInstance<Manager>();
            ShowCommand = new RelayCommand(() => BoxesList = manager.ShowBoxesByDate(Time()),
                () => !HasError && (Time() != TimeSpan.Zero));
        }
    }
}

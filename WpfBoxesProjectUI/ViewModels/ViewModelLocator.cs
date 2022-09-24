using BoxesProject;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Configuration;

namespace WpfBoxesProjectUI.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<MenuViewModel>();
            SimpleIoc.Default.Register<SupplyViewModel>();
            SimpleIoc.Default.Register<ShowStockDataViewModel>();
            SimpleIoc.Default.Register<BuyViewModel>();
            SimpleIoc.Default.Register<ShowBoxesByDateViewModel>();
            SimpleIoc.Default.Register<INotifier, Notifier>();
            SimpleIoc.Default.Register<ManagerConfiguration>(() => CreateConfiguration());
            SimpleIoc.Default.Register<Manager>();
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
        public MenuViewModel Menu => ServiceLocator.Current.GetInstance<MenuViewModel>();
        public SupplyViewModel Supply => ServiceLocator.Current.GetInstance<SupplyViewModel>();
        public ShowStockDataViewModel ShowStockData => ServiceLocator.Current.GetInstance<ShowStockDataViewModel>();
        public BuyViewModel Buy => ServiceLocator.Current.GetInstance<BuyViewModel>();
        public ShowBoxesByDateViewModel ShowBoxesByDate => ServiceLocator.Current.GetInstance<ShowBoxesByDateViewModel>();

        private ManagerConfiguration CreateConfiguration()
        {
            var appSettings = ConfigurationManager.AppSettings;

            var maxCorrect = int.TryParse(appSettings["MaximumAmountAllowed"], out int maximumAmountAllowed);
            var minCorrect = int.TryParse(appSettings["MinimumAmountAllowed"], out int minimumAmountAllowed);
            var maxExCorrect = double.TryParse(appSettings["MaximumExceedanceAllowed"], out double maximumExceedanceAllowed);
            var valCorrect = int.TryParse(appSettings["ValidityInSeconds"], out int validityInSeconds);
            var timerIntCorrect = int.TryParse(appSettings["TimerIntervalInSeconds"], out int timerIntervalInSeconds);

            TimeSpan validity = new TimeSpan(0, 0, 0, validityInSeconds);
            TimeSpan timerInterval = new TimeSpan(0, 0, 0, timerIntervalInSeconds);

            if (maxCorrect && minCorrect && maxExCorrect && valCorrect && timerIntCorrect)
            {
                try
                {
                    return new ManagerConfiguration(maximumAmountAllowed, minimumAmountAllowed, maximumExceedanceAllowed, validity, timerInterval);
                }
                catch { }
            }
            
            return ManagerConfiguration.Default;
        }
    }
}
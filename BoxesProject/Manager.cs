using BoxesProject.API;
using DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;

namespace BoxesProject
{
    public class Manager
    {
        SortedSet<Stock> mainTree;
        QueueList<Stock> queueList;

        DispatcherTimer timer;

        int maximumAmountAllowed;
        int minimumAmountAllowed;
        double maximumExceedanceAllowed;
        TimeSpan validity;
        TimeSpan timerInterval;

        INotifier notifier;

        public Manager(INotifier notifier, ManagerConfiguration configuration)
        {
            maximumAmountAllowed = configuration.MaximumAmountAllowed;
            minimumAmountAllowed = configuration.MinimumAmountAllowed;
            maximumExceedanceAllowed = configuration.MaximumExceedanceAllowed;
            validity = configuration.Validity;
            timerInterval = configuration.TimerInterval;

            this.notifier = notifier;
            mainTree = new SortedSet<Stock>();
            queueList = new QueueList<Stock>();
            timer = new DispatcherTimer();
            timer.Interval = validity;
            timer.Tick += TimerTick;
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (timer.Interval != timerInterval) timer.Interval = timerInterval;

            List<Stock> stocks = GetStocksByDate(validity);
            foreach (Stock stock in stocks)
            {
                Remove(stock, false);
            }
            if (stocks.Count > 0) notifier.ShowMessage($"The expiration of {stocks.Count} types of boxes has expired");
        }

        private void Validate(double buttomSize, double heightSize, int amount = 1)
        {
            if (buttomSize <= 0) throw new ArgumentException("Buttom size must be positive");
            if (heightSize <= 0) throw new ArgumentException("Height size must be positive");
            if (amount <= 0) throw new ArgumentException("Amount must be positive");
        }

        public void Supply(double buttomSize, double heightSize, int amount)
        {
            Validate(buttomSize, heightSize, amount);

            Stock newStock = new Stock() { BoxType = new Box() { Width = buttomSize, Height = heightSize } };
            if (mainTree.TryGetValue(newStock, out Stock stock))
            {
                stock.Amount += amount;
            }
            else
            {
                stock = newStock;
                stock.Amount = amount;
                stock.LastPurchaseDate = DateTime.Now;
                mainTree.Add(stock);

                QueueListNode<Stock> node = new QueueListNode<Stock>(stock);
                queueList.AddLast(node);
                stock.node = node;
            }

            if (stock.Amount > maximumAmountAllowed)
            {
                amount -= stock.Amount - maximumAmountAllowed;
                stock.Amount = maximumAmountAllowed;
                notifier.ShowMessage("The current Box type has reached the maximum amount allowed");
            }

            notifier.ShowMessage($"{amount} boxes supplied");
        }

        public IUIStock ShowStockData(double buttomSize, double heightSize)
        {
            Validate(buttomSize, heightSize);

            Stock targetStock = new Stock() { BoxType = new Box() { Width = buttomSize, Height = heightSize } };
            mainTree.TryGetValue(targetStock, out Stock resultStock);
            return resultStock;
        }

        public void Buy(double buttomSize, double heightSize, int amount)
        {
            Validate(buttomSize, heightSize, amount);

            List<IUIStock> list = new List<IUIStock>();
            int purchasedBoxes = 0;

            Box targetBox = new Box() { Width = buttomSize, Height = heightSize };
            Box maxValidBox = targetBox.IncreaseBy(maximumExceedanceAllowed);
            Stock targetStock = new Stock() { BoxType = targetBox };
            Stock maxValidStock = new Stock() { BoxType = maxValidBox };

            foreach (var item in mainTree.GetViewBetween(targetStock, maxValidStock))
            {
                if (item.BoxType.Height <= maxValidBox.Height)
                {
                    list.Add(new Stock(){ BoxType=item.BoxType, Amount=item.Amount, node=item.node });
                    purchasedBoxes += item.Amount;

                    if (purchasedBoxes >= amount)
                    {
                        int difference = purchasedBoxes - amount;
                        if (difference != 0)
                        {
                            Stock lastItem = list[list.Count - 1] as Stock;
                            lastItem.Amount -= difference;
                            purchasedBoxes -= difference;
                        }
                        break;
                    }
                }
            }

            if (list.Count == 0) notifier.ShowMessage("No Match Found");
            else
            {
                bool DidUserAgree = notifier.ShowUserShoppingCartAndReturnAnswer(list);
                if (!DidUserAgree) notifier.ShowMessage("Purchase has been canceled");
                else
                {
                    foreach (var item in list)
                    {
                        Remove(item as Stock, true);
                    }
                    notifier.ShowMessage($"{purchasedBoxes} boxes were purchased successfully");
                }
            }
        }

        private void Remove(Stock stockToRemove, bool isPurchase)
        {
            mainTree.TryGetValue(stockToRemove, out Stock targetStock);

            int boxesLeft = targetStock.Amount - stockToRemove.Amount;
            if (isPurchase && boxesLeft <= minimumAmountAllowed && targetStock.Amount > minimumAmountAllowed)
                notifier.ShowMessage($"Minimum amount of: \nWidth: {targetStock.BoxType.Width}, Height: {targetStock.BoxType.Height} ({boxesLeft})");

            if (targetStock.Amount == stockToRemove.Amount)
            {
                queueList.Remove(targetStock.node);
                mainTree.Remove(targetStock);
            }
            else
            {
                targetStock.Amount -= stockToRemove.Amount;
                if (isPurchase)
                {
                    targetStock.LastPurchaseDate = DateTime.Now;
                    queueList.Remove(targetStock.node);
                    queueList.AddLast(targetStock.node);
                }
            }
        }

        public List<IUIBox> ShowBoxesByDate(TimeSpan elapsedTime)
        {
            List<IUIBox> boxes = GetStocksByDate(elapsedTime).Select(stock => stock.BoxType as IUIBox).ToList();

            return boxes;
        }

        private List<Stock> GetStocksByDate(TimeSpan elapsedTime)
        {
            List<Stock> boxes = new List<Stock>();

            foreach (var stock in queueList)
            {
                var currentElapsedTime = DateTime.Now - stock.LastPurchaseDate;
                if (currentElapsedTime < elapsedTime) break;

                boxes.Add(stock);
            }

            return boxes;
        }
    }
}

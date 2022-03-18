using LottoWPF.Core;
using System.Collections.Generic;
using System.Windows;

namespace LottoWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int _drawCounter = 0;

        public int accountBalance = 30;

        private string _couponXD;

        private int _couponCounter;

        private LottoEngine _lottoEngine = new LottoEngine();

        public List<List<int>> listOfCoupons { get; set; } = new List<List<int>>();

        public MainWindow()
        {
            InitializeComponent();
            InitializedData();
        }


        ///Metody
        private void InitializedData()
        {
            UpdateWelcomeTextBlock();
            UpdateDayCounterTextBlock();
            UpdateAccountBalanceTextBlock();
            UpdateInformationTextBlock();
        }
        private void UpdateWelcomeTextBlock()
        {
            WelcomeTextBlock.Text = "Witaj w grze Lotto, dziś do wygrania jest " + _lottoEngine.CalculateCumulation() + " zł!";
        }

        private void UpdateDayCounterTextBlock()
        {
            DayCounterTextBlock.Text = "Liczba losowań: " + _drawCounter;
        }

        public void UpdateAccountBalanceTextBlock()
        {
            AccountBalanceTextBlock.Text = "Stan konta: " + accountBalance + " zł";
        }

        private void DrawButtonClick(object sender, RoutedEventArgs e)
        {
            _drawCounter++;
            UpdateDayCounterTextBlock();
        }

        private void BetButtonClick(object sender, RoutedEventArgs e)
        {
            if (accountBalance >= 3)
            {
                CreateNewCouponWindow createNewCouponWindow = new CreateNewCouponWindow();
                this.Visibility = Visibility.Hidden;
                createNewCouponWindow.Closed += CreateNewCouponWindow_Closed;
                createNewCouponWindow.Show();
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Masz za mało środków na koncie!\n Czy chcesz doładować konto za 30 zł?", "Gra Lotto", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        accountBalance += 30;
                        UpdateAccountBalanceTextBlock();
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
        }

        private void CreateNewCouponWindow_Closed(object sender, System.EventArgs e)
        {
            var senderInfo = (CreateNewCouponWindow)sender;

            if (senderInfo.acceptCouponButtonClickInfo == true)
            {
                int i = 0;
                listOfCoupons.Add(senderInfo.listOfNumbersForCoupon);
                foreach (var item in senderInfo.listOfNumbersForCoupon)
                {
                    _couponXD += item;
                    if (i < senderInfo.listOfNumbersForCoupon.Count - 1)
                    {
                        i++;
                        _couponXD += ", ";
                    }
                }
                UpdateCouponsTextBlock();
                _couponCounter++;
                UpdateInformationTextBlock();
                accountBalance -= 3;
                UpdateAccountBalanceTextBlock();
            }
        }

        private void UpdateCouponsTextBlock()
        {
            CouponListTextBlock.Text = _couponXD;
        }

        private void UpdateInformationTextBlock()
        {
            InformationTextBlock.Text = $"Twoje kupony [{_couponCounter}/8]:";
        }

        private void CloseAppButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}

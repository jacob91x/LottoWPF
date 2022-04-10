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
        private int _accountBalance = 30;
        private int _couponCounter;
        private int _currentCumulationValue;
        private LottoEngine _lottoEngine = new LottoEngine();
        private List<Coupon> _listOfCoupons = new List<Coupon>();

        public MainWindow()
        {
            InitializeComponent();
            InitializedData();
        }

        ///Metody
        private void InitializedData()
        {
            CurrentCumulation();
            UpdateWelcomeTextBlock();
            UpdateDayCounterTextBlock();
            UpdateAccountBalanceTextBlock();
            UpdateInformationTextBlock();
        }

        private void CurrentCumulation()
        {
            _currentCumulationValue = _lottoEngine.CalculateCumulation();
        }

        private void UpdateWelcomeTextBlock()
        {
            WelcomeTextBlock.Text = "Witaj w grze Lotto, dziś do wygrania jest " + _currentCumulationValue + " zł!";
        }

        private void UpdateDayCounterTextBlock()
        {
            DayCounterTextBlock.Text = "Liczba losowań: " + _drawCounter;
        }

        public void UpdateAccountBalanceTextBlock()
        {
            AccountBalanceTextBlock.Text = "Stan konta: " + _accountBalance + " zł";
        }

        private void DrawButtonClick(object sender, RoutedEventArgs e)
        {
            _drawCounter++;
            _couponCounter = 0;
            UpdateDayCounterTextBlock();
            UpdateInformationTextBlock();
            LuckyNumbersPresentationWindow luckyNumbersPresentationWindow = new LuckyNumbersPresentationWindow(_currentCumulationValue, _listOfCoupons, _lottoEngine.DrawLuckyNumbers());
            this.Visibility = Visibility.Hidden;
            luckyNumbersPresentationWindow.Closed += LuckyNumbersPresentationWindow_Closed;
            luckyNumbersPresentationWindow.Show();
        }

        private void BetButtonClick(object sender, RoutedEventArgs e)
        {
            if (_couponCounter >= 8)
            {
                MessageBox.Show("Nie możesz obstawić więcej niż 8 kuponów", "Gra Lotto", MessageBoxButton.OK);
                return;
            }
            if (_accountBalance >= 3)
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
                        _accountBalance += 30;
                        UpdateAccountBalanceTextBlock();
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
        }

        private void LuckyNumbersPresentationWindow_Closed(object sender, System.EventArgs e)
        {
            var senderInfo = (LuckyNumbersPresentationWindow)sender;
            _accountBalance += senderInfo.totalWinValue;
            UpdateAccountBalanceTextBlock();
            _listOfCoupons.Clear();
            UpdateCouponsTextBlock();
        }

        private void CreateNewCouponWindow_Closed(object sender, System.EventArgs e)
        {
            var senderInfo = (CreateNewCouponWindow)sender;

            if (senderInfo.AcceptCouponButtonClickInfo == true)
            {
                _listOfCoupons.Add(senderInfo.Coupon);
                UpdateCouponsTextBlock();
                _couponCounter++;
                UpdateInformationTextBlock();
                _accountBalance -= 3;
                UpdateAccountBalanceTextBlock();
            }
        }

        private void UpdateCouponsTextBlock()
        {
            var listOfStringCoupons = new List<string>();

            foreach (var coupon in _listOfCoupons)
            {
                listOfStringCoupons.Add(string.Join(", ", coupon));
            }
            CouponListTextBlock.Text = string.Join("\n ", listOfStringCoupons);
        }


        private void UpdateInformationTextBlock()
        {
            InformationTextBlock.Text = $" Twoje kupony [{_couponCounter}/8]:";
        }

        private void CloseAppButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}

using System;
using System.Timers;
using System.Windows;
using System.Windows.Media;

namespace LottoWPF
{
    /// <summary>
    /// Interaction logic for CreateNewCouponWindow.xaml
    /// </summary>
    public partial class CreateNewCouponWindow : Window
    {
        public Coupon Coupon { get; set; } = new Coupon();

        public bool AcceptCouponButtonClickInfo { get; set; } = false;

        private static Timer _informationTimer;

        private int _currentTime;

        public CreateNewCouponWindow()
        {
            InitializeComponent();
            UpdateInsertedDataInformationTextBlock(false);
            EnableAcceptCouponButton(false);
            FocusNewNumberForCouponTextBox();
        }

        private void AddNumberButtonClick(object sender, RoutedEventArgs e)
        {
            AddNumberToCoupon();
        }

        private void AddNumberToCoupon()
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(NewNumberForCouponTextBox.Text, "[^0-9]") || string.IsNullOrEmpty(NewNumberForCouponTextBox.Text))
            {
                MessageBox.Show("Wprowadź liczbę!");
                ClearNewNumberForCouponTextBox();
                return;
            }

            switch (Coupon.Add(int.Parse(NewNumberForCouponTextBox.Text)))
            {
                case CouponError.NoError:
                    UpdateCurrentCouponTextBlock();
                    UpdateInsertedDataInformationTextBlock(false);
                    break;
                case CouponError.NumberAlreadyExist:
                    MessageBox.Show("Nie możesz skreślić ponownie tej samej liczby!");
                    break;
                case CouponError.NumberOutOfRange:
                    StartMessageHideTimer();
                    UpdateInsertedDataInformationTextBlock(true);
                    break;
            }
            if (Coupon.Count >= 6)
            {
                EnableInsertingOfNumbers(false);
                EnableAcceptCouponButton(true);
            }
            ClearNewNumberForCouponTextBox();
            FocusNewNumberForCouponTextBox();
        }

        private void FocusNewNumberForCouponTextBox()
        {
            NewNumberForCouponTextBox.Focus();
        }

        private void EnableInsertingOfNumbers(bool enable)
        {
            NewNumberForCouponTextBox.IsEnabled = enable;
            AddNumberButton.IsEnabled = enable;
        }

        private void EnableAcceptCouponButton(bool enable)
        {
            AcceptCouponButton.IsEnabled = enable;
        }

        private void ClearNewNumberForCouponTextBox()
        {
            NewNumberForCouponTextBox.Text = string.Empty;
        }

        private void OnTimeEvent(object sender, ElapsedEventArgs e)
        {
            bool timerStatus;

            if (_currentTime == 0)
            {
                _informationTimer.Stop();
                timerStatus = true;
            }
            else
            {
                _currentTime--;
                timerStatus = false;
            }
            Dispatcher.Invoke(() => UpdateInsertedDataInformationTextBlock(timerStatus));
        }

        private void StartMessageHideTimer()
        {
            _currentTime = 3;
            _informationTimer = new Timer();
            _informationTimer.Interval = 1000;
            _informationTimer.Elapsed += OnTimeEvent;
            _informationTimer.AutoReset = true;
            _informationTimer.Start();
        }

        private void UpdateCurrentCouponTextBlock()
        {
            CurrentCouponTextBlock.Text = Coupon.ToString();
        }

        public void UpdateInsertedDataInformationTextBlock(bool isErrorMessage)
        {
            if (!isErrorMessage)
            {
                AddedNumberInformationTextBlock.Foreground = new SolidColorBrush(Colors.Black);
                AddedNumberInformationTextBlock.Text = "Podaj liczbę z zakresu od 1 do 20";
            }
            else
            {
                AddedNumberInformationTextBlock.Foreground = new SolidColorBrush(Colors.Red);
                AddedNumberInformationTextBlock.Text = "Liczba spoza zakresu! " + _currentTime;
            }
        }

        public void AcceptCouponButtonClick(object sender, RoutedEventArgs e)
        {
            AcceptCouponButtonClickInfo = true;
            CloseCurrentWindow();
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            CloseCurrentWindow();
        }

        private void CloseCurrentWindow()
        {
            this.Close();
            App.Current.MainWindow.Show();
        }

        private void BackspaceButtonClick(object sender, RoutedEventArgs e)
        {
            Coupon.RemoveLast();
            EnableInsertingOfNumbers(true);
            EnableAcceptCouponButton(false);
            UpdateCurrentCouponTextBlock();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            CloseCurrentWindow();
        }

        private void OnEnterKeyDownHandler(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                AddNumberToCoupon();
            }
        }
    }
}

using System;
using System.Collections.Generic;
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
        public List<int> listOfNumbersForCoupon = new List<int>();

        public bool acceptCouponButtonClickInfo = false;

        private static Timer _informationTimer;

        private int _currentTime;

        public CreateNewCouponWindow()
        {
            InitializeComponent();
            UpdateInsertedDataInformationTextBlock(true);
        }

        private void AddNumberButtonClick(object sender, RoutedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(NewNumberForCouponTextBox.Text, "[^0-9]"))
            {
                MessageBox.Show("Wprowadź liczbę!");
                ClearNewNumberForCouponTextBox();
            }
            else if (listOfNumbersForCoupon.Contains(int.Parse(NewNumberForCouponTextBox.Text)))
            {
                MessageBox.Show("Nie możesz skreślić ponownie tej samej liczby!");
                ClearNewNumberForCouponTextBox();
            }
            else if (AddedNumberValidation())
            {
                listOfNumbersForCoupon.Add(int.Parse(NewNumberForCouponTextBox.Text));
                UpdateCurrentCouponTextBlock();
                UpdateInsertedDataInformationTextBlock(true);
            }
            else
            {
                myTimer();
                UpdateInsertedDataInformationTextBlock(false);
            }

            if (listOfNumbersForCoupon.Count >= 6)
            {
                EnableInsertingOfNumbers(false);
                EnableAcceptCouponButton(true);
            }
            NewNumberForCouponTextBox.Text = string.Empty;
        }

        private void EnableInsertingOfNumbers(bool _enable)
        {
            NewNumberForCouponTextBox.IsEnabled = _enable;
            AddNumberButton.IsEnabled = _enable;
        }

        private void EnableAcceptCouponButton(bool _enable)
        {
            AcceptCouponButton.IsEnabled = _enable;
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
            //dispatcher - property of window, wywołanie, funkcja lambda - definiowana lokalnie
            Dispatcher.Invoke(() => UpdateInsertedDataInformationTextBlock(timerStatus));
        }

        private void myTimer()
        {
            _currentTime = 4;
            _informationTimer = new Timer();
            _informationTimer.Interval = 1000;
            _informationTimer.Elapsed += OnTimeEvent;
            _informationTimer.AutoReset = true;
            _informationTimer.Start();
        }

        private bool AddedNumberValidation()
        {
            int validatedNumber = int.Parse(NewNumberForCouponTextBox.Text);

            if (validatedNumber > 0 && validatedNumber <= 20)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void UpdateCurrentCouponTextBlock()
        {
            var currentCopuonString = string.Join(", ", listOfNumbersForCoupon);
            CurrentCouponTextBlock.Text = currentCopuonString;
        }

        public void UpdateInsertedDataInformationTextBlock(bool informationType)
        {
            if (informationType)
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
            acceptCouponButtonClickInfo = true;
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
            listOfNumbersForCoupon.RemoveAt(listOfNumbersForCoupon.Count - 1);
            EnableInsertingOfNumbers(true);
            EnableAcceptCouponButton(false);
            UpdateCurrentCouponTextBlock();
        }
    }
}

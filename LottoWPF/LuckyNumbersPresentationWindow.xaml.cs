using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LottoWPF
{
    /// <summary>
    /// Interaction logic for LuckyNumbersPresentationWindow.xaml
    /// </summary>
    public partial class LuckyNumbersPresentationWindow : Window
    {
        private int[] _winningCouponsCounter = new int[4];
        public int totalWinValue;

        public LuckyNumbersPresentationWindow(int _currentCumulationValue, List<Coupon> listOfCoupons, int[] luckyNumbers)
        {
            InitializeComponent();
            UpdateLuckyNumbersTextBlock(luckyNumbers);
            GenerateNumbersView(listOfCoupons, luckyNumbers);
            TotalWinValueCalculation(_currentCumulationValue);
            UpdateWinnerInformationTextBlock();
        }

        private void GenerateNumbersView(List<Coupon> listOfCoupons, int[] luckyNumbers)
        {
            int couponIndex = 1;

            foreach (var coupon in listOfCoupons)
            {
                int _hitsNumber = 0;

                StackPanel horizontalCouponStackPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(4) };

                _ = horizontalCouponStackPanel.Children.Add(new Label { Content = "kupon " + couponIndex + ":", Margin = new Thickness(0, 0, 7, 0) });

                foreach (var number in coupon.Numbers)
                {
                    Label numberLabel = new Label
                    {
                        Content = number,
                        Width = 25,
                        Height = 25,
                        FontWeight = FontWeights.Bold,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        Margin = new Thickness(1)
                    };

                    if (luckyNumbers.Contains(number))
                    {
                        numberLabel.Background = Brushes.Green;
                        _hitsNumber++;
                    }
                    else
                    {
                        numberLabel.Background = Brushes.White;
                    }
                    _ = horizontalCouponStackPanel.Children.Add(numberLabel);
                }
                couponIndex++;
                string hitsLabel = "Ilość trafień: " + _hitsNumber;
                horizontalCouponStackPanel.Children.Add(new Label { Content = hitsLabel, Margin = new Thickness(7, 0, 0, 0) });
                _ = VerticalCouponStackPanel.Children.Add(horizontalCouponStackPanel);

                switch (_hitsNumber)
                {
                    case 3:
                        _winningCouponsCounter[0]++;
                        break;
                    case 4:
                        _winningCouponsCounter[1]++;
                        break;
                    case 5:
                        _winningCouponsCounter[2]++;
                        break;
                    case 6:
                        _winningCouponsCounter[3]++;
                        break;
                }
            }
        }

        private void TotalWinValueCalculation(int _currentCumulationValue)
        {
            var _drawRandomNumber = new Random();

            if (_winningCouponsCounter[0] > 0)
            {
                totalWinValue += _winningCouponsCounter[0] * 24;
            }
            if (_winningCouponsCounter[1] > 0)
            {
                totalWinValue += _winningCouponsCounter[1] * _drawRandomNumber.Next(100, 301);
            }
            if (_winningCouponsCounter[2] > 0)
            {
                totalWinValue += _winningCouponsCounter[2] * _drawRandomNumber.Next(4000, 8001);
            }
            if (_winningCouponsCounter[3] > 0)
            {
                totalWinValue += _winningCouponsCounter[3] * _currentCumulationValue / (_winningCouponsCounter[3] + _drawRandomNumber.Next(0, 5));
            }
        }

        private void UpdateWinnerInformationTextBlock()
        {
            WinnerInformationTextBlock.Text = "Twoja wygrana to: " + totalWinValue.ToString() + " zł";
        }

        private void UpdateLuckyNumbersTextBlock(int[] luckyNumbers)
        {
            LuckyNumbersTextBlock.Text = "Szczęśliwe liczby to: " + string.Join(", ", luckyNumbers);
        }

        private void CloseCurrentWindow()
        {
            this.Close();
            App.Current.MainWindow.Show();
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            CloseCurrentWindow();
        }

        private void CloseWindowButtonClick(object sender, RoutedEventArgs e)
        {
            CloseCurrentWindow();
        }
    }
}

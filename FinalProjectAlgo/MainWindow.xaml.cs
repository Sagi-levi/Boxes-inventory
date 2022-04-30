using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Models;
using BLService;
using System.Threading;
using System.Text.RegularExpressions;

namespace FinalProjectAlgo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ICommunicator
    {
        Manager _manager;
        public MainWindow()
        {
            InitializeComponent();
            _manager = new Manager(50, 15, new TimeSpan(00, 0, 10), new TimeSpan(00, 0, 20), this);

        }

        public void OnMessage(string message)
        {
            MessageBox.Show(message);
        }

        public bool OnQuestion(string message)
        {
            MessageBoxResult result = MessageBox.Show(message,
                                          "Confirmation",
                                          MessageBoxButton.YesNo,
                                          MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                return true;
            }
            else return false;
        }


        private void ADDBTN_Click(object sender, RoutedEventArgs e)
        {
            double x, y;
            int amount;
            if (!double.TryParse(X_TXT.Text, out x) || x < 0)
            {
                OnMessage("somthong wrong with the widht and lenght");
                X_TXT.Text = "";
                return;
            }
            if (!double.TryParse(Y_TXT.Text, out y) || y < 0)
            {
                OnMessage("somthong wrong with height");
                Y_TXT.Text = "";
                return;
            }
            if (!int.TryParse(Amount_TXT.Text, out amount) || amount < 0)
            {
                OnMessage("somthong wrong with the amount");
                Amount_TXT.Text = "";
                return;
            }
            _manager.Add(x, y, amount);
            X_TXT.Text = "";
            Y_TXT.Text = "";
            Amount_TXT.Text = "";
        }

        private void BuyBTN_Click(object sender, RoutedEventArgs e)
        {
            double x, y;
            int amount;
            if (!double.TryParse(XBuy_TXT.Text, out x) || x < 0)
            {
                OnMessage("somthong wrong with the widht and lenght");
                XBuy_TXT.Text = "";
                return;
            }
            if (!double.TryParse(YBuy_TXT.Text, out y) || y < 0)
            {
                OnMessage("somthong wrong with height");
                YBuy_TXT.Text = "";
                return;
            }
            if (!int.TryParse(AmountBuy_TXT.Text, out amount) || amount < 0)
            {
                OnMessage("somthong wrong with the amount");
                AmountBuy_TXT.Text = "";
                return;
            }
            _manager.Buy(x, y, amount);
            XBuy_TXT.Text = "";
            YBuy_TXT.Text = "";
            AmountBuy_TXT.Text = "";
        }

        private void InfoBTN_Click(object sender, RoutedEventArgs e)
        {
            double x, y;
            if (!double.TryParse(XInfo_TXT.Text, out x) || x < 0)
            {
                OnMessage("somthong wrong with the widht and lenght");
                XInfo_TXT.Text = "";
                return;
            }
            if (!double.TryParse(YInfo_TXT.Text, out y) || y < 0)
            {
                OnMessage("somthong wrong with height");
                YInfo_TXT.Text = "";
                return;
            }
            _manager.Info(x, y);
            XInfo_TXT.Text = "";
            YInfo_TXT.Text = "";
        }
    }
}

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

namespace SerialPortCommunicationTool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitView();
        }
        private void InitView()
        {
            SendPortListComboBox.ItemsSource = pchSend.PortNameArr;
            ReceivePortListComboBox.ItemsSource = pchReceive.PortNameArr;
            PotterBitListComboBox.ItemsSource = BaudRateArr;
            PotterBitListComboBox.SelectedItem = BaudRateArr[BaudRateArr.Length - 1].ToString();
            DataBitsListComboBox.ItemsSource = DataBitArr;
            DataBitsListComboBox.Text = DataBitArr[DataBitArr.Length - 1].ToString();
        }

        #region 字段/属性

        int[] BaudRateArr = new int[] { 110, 300, 1200, 2400, 4800, 115200 };
        int[] DataBitArr = new int[] { 6, 7, 8 };
        int[] StopBitArr = new int[] { 1, 2, 3 };
        int[] TimeoutArr = new int[] { 500, 1000, 2000, 5000, 10000 };
        object[] CheckBitArr = new object[] { "None" };
        private bool ReceiveState = false;
        private PortCommunicationHelper pchSend;
        private PortCommunicationHelper pchReceive;

        #endregion
    }
}

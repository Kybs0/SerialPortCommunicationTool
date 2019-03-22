using System;
using System.Linq;
using System.Windows;

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
            pchSend = new PortCommunicationHelper();
            pchReceive = new PortCommunicationHelper();
            InitView();
        }
        private void InitView()
        {
            SendPortListComboBox.ItemsSource = pchSend.PortNameArr;
            SendPortListComboBox.SelectedItem = pchSend.PortNameArr?.FirstOrDefault();
            ReceivePortListComboBox.ItemsSource = pchReceive.PortNameArr;
            ReceivePortListComboBox.SelectedItem = pchReceive.PortNameArr?.FirstOrDefault();
            PotterBitListComboBox.ItemsSource = BaudRateArr;
            PotterBitListComboBox.SelectedItem = BaudRateArr[BaudRateArr.Length - 1];
            DataBitsListComboBox.ItemsSource = DataBitArr;
            DataBitsListComboBox.Text = DataBitArr[DataBitArr.Length - 1].ToString();
        }

        #region 字段/属性

        int[] BaudRateArr = new int[] { 110, 300, 1200, 2400, 4800, 115200 };
        int[] DataBitArr = new int[] { 6, 7, 8 };
        int[] StopBitArr = new int[] { 1, 2, 3 };
        int[] TimeoutArr = new int[] { 500, 1000, 2000, 5000, 10000 };
        object[] CheckBitArr = new object[] { "None" };
        private bool IsConnecting = false;

        private bool _isReceiving = false;
        private PortCommunicationHelper pchSend;
        private PortCommunicationHelper pchReceive;

        #endregion

        private void PortConnectionButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (pchSend.PortState)
            {
                pchSend.ClosePort();

                pchReceive.ClosePort();
                pchReceive.ComDataReceived -= OnComReceiveData;
                ReceiveDataButton.Content = "开始接收";
                _isReceiving = false;
            }
            else
            {
                pchSend.OpenPort(SendPortListComboBox.Text, int.Parse(PotterBitListComboBox.Text), int.Parse(DataBitsListComboBox.Text));
                pchReceive.OpenPort(ReceivePortListComboBox.Text, int.Parse(PotterBitListComboBox.Text), int.Parse(DataBitsListComboBox.Text));
            }
            FreshBtnState(pchSend.PortState);
        }
        /// <summary>
        /// 刷新按钮状态
        /// </summary>
        /// <param name="state"></param>
        private void FreshBtnState(bool state)
        {
            if (state)
            {
                PortConnectionButton.Content = "关闭发送接收串口";
            }
            else
            {
                PortConnectionButton.Content = "打开发送接收串口";
            }
        }

        #region 发送

        private void SendDataButton_OnClick(object sender, RoutedEventArgs e)
        {
            //var work16 = textWork16(SendTextBox.Text);
            pchSend.SendData(SendTextBox.Text);
        }
        private byte[] textWork16(string strText)
        {
            strText = strText.Replace(" ", "");
            byte[] bText = new byte[strText.Length / 2];
            for (int i = 0; i < strText.Length / 2; i++)
            {
                bText[i] = Convert.ToByte(Convert.ToInt32(strText.Substring(i * 2, 2), 16));
            }
            return bText;
        }
        #endregion

        #region 接收

        private void ReceiveDataButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_isReceiving)
            {
                pchReceive.ComDataReceived -= OnComReceiveData;
                ReceiveDataButton.Content = "开始接收";
                _isReceiving = false;
            }
            else
            {
                pchReceive.ComDataReceived += OnComReceiveData;
                ReceiveDataButton.Content = "停止接收";
                _isReceiving = true;
            }
        }

        /// <summary>
        /// 接收到的数据，写入文本框内
        /// </summary>
        /// <param name="data"></param>
        private void OnComReceiveData(object sender, string data)
        {
            Application.Current.Dispatcher.Invoke(new EventHandler(delegate
            {
                ReceiveTextBox.AppendText($"接收：{data}\n");
            }));
        }

        #endregion


    }
}

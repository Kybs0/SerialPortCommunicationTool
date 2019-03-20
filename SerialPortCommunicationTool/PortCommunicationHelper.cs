using System;
using System.IO.Ports;
using System.Text;

namespace SerialPortCommunicationTool
{
    /// <summary>
    /// 串口通信辅助类
    /// </summary>
    public class PortCommunicationHelper
    {

        #region 字段/属性/委托

        /// <summary>
        /// 串行端口对象
        /// </summary>
        private SerialPort _serialPort;

        /// <summary>
        /// 串口接收数据委托
        /// </summary>
        public delegate void ComReceiveDataHandler(string data);

        public ComReceiveDataHandler OnComReceiveDataHandler = null;

        /// <summary>
        /// 端口名称数组
        /// </summary>
        public string[] PortNameArr { get; set; }

        /// <summary>
        /// 串口通信开启状态
        /// </summary>
        public bool PortState { get; set; } = false;

        /// <summary>
        /// 编码类型
        /// </summary>
        public Encoding EncodingType { get; set; } = Encoding.Unicode;

        #endregion

        #region 方法

        public PortCommunicationHelper()
        {
            PortNameArr = SerialPort.GetPortNames();
            _serialPort = new SerialPort();
            _serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceived);
        }

        /// <summary>
        /// 打开端口
        /// </summary>
        /// <param name="portName">端口名称</param>
        /// <param name="boudRate">波特率</param>
        /// <param name="dataBit">数据位</param>
        /// <param name="stopBit">停止位</param>
        /// <param name="timeout">超时时间</param>
        public void OpenPort(string portName, int boudRate = 115200, int dataBit = 8, int stopBit = 1, int timeout = 5000)
        {
            try
            {
                _serialPort.PortName = portName;
                _serialPort.BaudRate = boudRate;
                _serialPort.DataBits = dataBit;
                _serialPort.StopBits = (StopBits)stopBit;
                _serialPort.ReadTimeout = timeout;
                _serialPort.Close();
                _serialPort.Open();
                PortState = true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 关闭端口
        /// </summary>
        public void ClosePort()
        {
            try
            {
                _serialPort.Close();
                PortState = false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="sendData"></param>
        public void SendData(string sendData)
        {
            try
            {
                _serialPort.Encoding = EncodingType;
                _serialPort.Write(sendData);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="charData"></param>
        public void SendData(byte[] charData)
        {
            try
            {
                _serialPort.Encoding = EncodingType;
                _serialPort.Write(charData, 0, charData.Length);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 接收数据回调用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] buffer = new byte[_serialPort.BytesToRead];
            _serialPort.Read(buffer, 0, buffer.Length);
            string str = EncodingType.GetString(buffer);
            if (OnComReceiveDataHandler != null)
            {
                OnComReceiveDataHandler(str);
            }
        }

        #endregion

    }
}

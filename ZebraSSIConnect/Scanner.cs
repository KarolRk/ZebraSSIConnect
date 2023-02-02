
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using InTheHand.Net;
using System.Text;
using System.Net.Sockets;
using System.Reflection;
using System.Diagnostics;

namespace ZebraSSIConnect
{
    public class BarcodeScanner
    {

        public string readerID;
        public string serviceDescription = "SSI Scanner Serial";
        private BluetoothAddress? bluetoothAddress;
        private BluetoothClient? bluetoothClient;
        private NetworkStream? networkStream;
        private Thread? streamReader;
        private string errorBluetoothAddressNullOrEmpty = "Bluetooth Address is null or empty";
        private string errorCannotFindPortNumber = "Cannot find the port number";
        private string errorMessageNullOrEmpty = "Message is null or empty";
        private string errorNetworkStreamNotWritable = "Network stream is not writable";
        private string errorNetworkStreamNotReadable = "Network stream is not readable";
        private string errorDeviceNotConnected = "Device is not connected";
        private bool IsSocketAccessible => this.bluetoothClient != null && this.bluetoothClient.Connected;
        public bool IsConnected => this.IsSocketAccessible;


        public event EventHandler<AllEventArgs> AllMessagesEvent;
        public event EventHandler<NegativeAckEventArgs> NegativeAckEvent;
        public event EventHandler<BarcodeEventArgs> BarcodeEvent;
        public event EventHandler<CapabilitiesReplyEventArgs> CapabilitiesReplyEvent;
        public event EventHandler<EventEventArgs> EventEvent;

        public class AllEventArgs : EventArgs
        {
            public byte[]? packet { get; set; }
        }
        public class NegativeAckEventArgs : EventArgs
        {
            public byte reason { get; set; }
            public byte[]? packet { get; set; }
        }
        public class BarcodeEventArgs : EventArgs
        {
            public string? barcode { get; set; }
            public string? type { get; set; }
            public byte[]? packet { get; set; }
        }
        public class CapabilitiesReplyEventArgs : EventArgs
        {
            public byte[]? baudRates { get; set; }
            public byte miscSerial { get; set; }
            public byte multiPacket { get; set; }
            public byte[]? commandCodes { get; set; }
            public string[]? commands { get; set; }
            public byte[]? packet { get; set; }
        }
        public class EventEventArgs : EventArgs
        {
            public byte code { get; set; }
            public byte[]? packet { get; set; }
        }



        public BarcodeScanner(string readerID)
        {
            this.readerID = readerID;
        }
        public BarcodeScanner(string readerID, string serviceDescription)
        {
            this.readerID = readerID;
            this.serviceDescription = serviceDescription;
        }

        public void Connect()
        {
            try
            {
                this.bluetoothAddress = !string.IsNullOrEmpty(this.readerID) ? BluetoothAddress.Parse(this.readerID) : throw new ArgumentException(this.errorBluetoothAddressNullOrEmpty, this.readerID);
                int portNumber = this.GetPortNumber(this.bluetoothAddress);
                this.bluetoothClient = new BluetoothClient();
                this.bluetoothClient.Connect(new BluetoothEndPoint(this.bluetoothAddress, BluetoothService.RFCommProtocol, portNumber));

                this.networkStream = this.bluetoothClient.GetStream();
                if (!this.networkStream.CanWrite)
                {
                    this.bluetoothClient.Close();
                    throw new ApplicationException(this.errorNetworkStreamNotWritable);
                }
                if (!this.networkStream.CanRead)
                {
                    this.bluetoothClient.Close();
                    throw new ApplicationException(this.errorNetworkStreamNotReadable);
                }
                this.streamReader = new Thread(new ThreadStart(this.ReadStream));
                this.streamReader.IsBackground = true;
                this.streamReader.Start();
                Debug.WriteLine($"Stream opened: {this.bluetoothAddress}:{portNumber}");

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Exception");
            }
        }

        public void Disconnect()
        {
            if (this.streamReader != null)
            {
                try
                {
                    this.streamReader.Abort();
                    this.streamReader = (Thread)null;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message, "Exception");
                }
            }
            if (this.networkStream != null)
            {
                try
                {
                    this.networkStream.Flush();
                    this.networkStream.Close();
                    this.networkStream = (NetworkStream)null;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message, "Exception");
                }
            }
            if (this.bluetoothClient != null)
            {
                try
                {
                    this.bluetoothClient.Close();
                    this.bluetoothClient = (BluetoothClient)null;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message, "Exception");
                }
            }
            Debug.WriteLine("Stream closed");
        }

        private int GetPortNumber(BluetoothAddress bluetoothAddress)
        {
            try
            {
                string empty = string.Empty;
                int num = -1;
                foreach (ServiceRecord serviceRecord in new BluetoothDeviceInfo(bluetoothAddress).GetServiceRecords(BluetoothService.SerialPort))
                {
                    if (ServiceRecordUtilities.Dump(serviceRecord).Contains(this.serviceDescription))
                    {
                        num = ServiceRecordHelper.GetRfcommChannelNumber(serviceRecord);
                        break;
                    }
                }
                return num != -1 ? num : throw new ApplicationException(this.errorCannotFindPortNumber);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ReadStream()
        {
            while (this.IsSocketAccessible)
            {
                try
                {
                    byte[] bytes = new byte[1024];
                    if (this.networkStream.Read(bytes, 0, bytes.Length) > 0)
                    {
                        ProcessDecoderMessage(bytes);
                    }
                }
                catch (ObjectDisposedException ex)
                {
                    Debug.WriteLine(ex.Message, "Exception");
                }
                catch (IOException ex)
                {
                    Debug.WriteLine(ex.Message, "Exception");
                }
            }
        }


        private void ProcessDecoderMessage(byte[] bytes)
        {

            Debug.WriteLine("Message received: " + ByteMessageToString(bytes));

            int length = (int)bytes[0];
            byte opcode = bytes[1];


            if (this.AllMessagesEvent != null)
            {
                var args = new AllEventArgs();
                args.packet = bytes.Take(length).ToArray();
                this.AllMessagesEvent.Invoke(this, args);
            }

            switch (opcode)
            {
                case Command.CMD_ACK:
                    break;

                case Command.CMD_NAK:
                    if (this.NegativeAckEvent != null)
                    {
                        var args = new NegativeAckEventArgs();
                        args.reason = bytes[4];
                        args.packet = bytes.Take(length).ToArray();
                        this.NegativeAckEvent.Invoke(this, args);
                    }
                    break;

                case Command.DECODE_DATA:
                    if (CheckMessage(bytes))
                    {
                        if (this.BarcodeEvent != null)
                        {
                            var args = new BarcodeEventArgs();
                            args.barcode = Encoding.ASCII.GetString(bytes, 5, length - 5);
                            args.type = BarcodeTypes.getBarcodeType(bytes[4]);
                            args.packet = bytes.Take(length).ToArray();
                            this.BarcodeEvent.Invoke(this, args);
                        }
                    }
                    break;

                case Command.CAPABILITIES_REPLY:
                    if (CheckMessage(bytes))
                    {
                        if (this.CapabilitiesReplyEvent != null)
                        {
                            var args = new CapabilitiesReplyEventArgs();
                            args.baudRates = bytes.Skip(4).Take(2).ToArray();
                            args.miscSerial = bytes[6];
                            args.multiPacket = bytes[7];
                            args.commandCodes = bytes.Skip(8).Take(length - 8).ToArray();
                            args.commands = Array.Empty<string>();
                            foreach (FieldInfo field in typeof(Command).GetFields(BindingFlags.Public | BindingFlags.Static))
                            {
                                foreach (byte commandCode in args.commandCodes)
                                {
                                    if (commandCode == (byte)field.GetValue(null))
                                    {

                                        args.commands = args.commands.Append(field.Name).ToArray();
                                        break;
                                    }
                                }

                            }
                            args.packet = bytes.Take(length).ToArray();
                            this.CapabilitiesReplyEvent.Invoke(this, args);
                        }
                    }
                    break;

                case Command.EVENT:
                    if (CheckMessage(bytes))
                    {
                        if (this.EventEvent != null)
                        {
                            var args = new EventEventArgs();
                            args.code = bytes[4];
                            args.packet = bytes.Take(length).ToArray();
                            this.EventEvent.Invoke(this, args);
                        }
                    }
                    break;

            }

        }

        private bool CheckMessage(byte[] bytes)
        {

            byte[] messageChecksum = bytes.Skip((int)bytes[0]).ToArray();
            byte[] calculatedChecksum = CalculateChecksum(bytes);

            if (messageChecksum[0] == calculatedChecksum[0] && messageChecksum[1] == calculatedChecksum[1])
            {
                SendMessage(new byte[] { 0x04, Command.CMD_ACK, 0x04, 0x80 });
                return true;
            }
            else
            {
                SendMessage(new byte[] { 0x05, Command.CMD_NAK, 0x04, 0x80, 0x01 });
                return false;
            }

        }


        private byte[] CalculateChecksum(byte[] bytes)
        {
            ushort checksum = (ushort)(~bytes.Take((int)bytes[0]).Select(x => (int)x).Sum() + 1);
            byte[] checksumBytes = BitConverter.GetBytes(checksum);
            return new byte[] { checksumBytes[1], checksumBytes[0] };
        }

        private byte[] MessageAddChecksum(byte[] bytes)
        {
            bytes = bytes.Take((int)bytes[0]).ToArray();
            byte[] checksumBytes = CalculateChecksum(bytes);
            return bytes.Concat(checksumBytes).ToArray();
        }

        public void SendMessage(byte[] bytes)
        {
            try
            {
                if (bytes.Length <= 0)
                    throw new ArgumentException(this.errorMessageNullOrEmpty);

                if (!this.IsConnected)
                    throw new ArgumentException(this.errorDeviceNotConnected);

                bytes = MessageAddChecksum(bytes);
                this.networkStream.Write(bytes, 0, bytes.Length);
                Debug.WriteLine("Message send: " + ByteMessageToString(bytes));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Exception");
            }

        }

        public void CapabilitiesRequest()
        {
            SendMessage(new byte[] { 0x04, Command.CAPABILITIES_REQUEST, 0x04, 0x00 });
        }
        public void AimOn()
        {
            SendMessage(new byte[] { 0x04, Command.AIM_ON, 0x04, 0x00 });
        }
        public void AimOff()
        {
            SendMessage(new byte[] { 0x04, Command.AIM_OFF, 0x04, 0x00 });
        }
        public void Beep()
        {
            SendMessage(new byte[] { 0x05, Command.BEEP, 0x04, 0x00, 0x00 });
        }
        public void Beep(byte type)
        {
            SendMessage(new byte[] { 0x05, Command.BEEP, 0x04, 0x00, type });
        }
        public void LedOn()
        {
            SendMessage(new byte[] { 0x05, Command.LED_ON, 0x04, 0x00, 0x01 });
        }
        public void LedOn(byte type)
        {
            SendMessage(new byte[] { 0x05, Command.LED_ON, 0x04, 0x00, type });
        }
        public void LedOff()
        {
            SendMessage(new byte[] { 0x05, Command.LED_OFF, 0x04, 0x00, 0x01 });
        }
        public void LedOff(byte type)
        {
            SendMessage(new byte[] { 0x05, Command.LED_OFF, 0x04, 0x00, type });
        }
        public void ScanEnable()
        {
            SendMessage(new byte[] { 0x04, Command.SCAN_ENABLE, 0x04, 0x00 });
        }
        public void ScanDisable()
        {
            SendMessage(new byte[] { 0x04, Command.SCAN_DISABLE, 0x04, 0x00 });
        }
        public void Sleep()
        {
            SendMessage(new byte[] { 0x04, Command.SLEEP, 0x04, 0x00 });
        }
        public void StartSession()
        {
            SendMessage(new byte[] { 0x04, Command.START_SESSION, 0x04, 0x00 });
        }
        public void StoptSession()
        {
            SendMessage(new byte[] { 0x04, Command.STOP_SESSION, 0x04, 0x00 });
        }

        private string ByteMessageToString(byte[] bytes)
        {
            return string.Join(" ", bytes.Take((int)bytes[0] + 2).Select(x => $"{x:X}"));

        }

    }

}


using InTheHand.Net.Sockets;
using ZebraSSIConnect;


class Program
{

    static void Main(string[] args)
    {
        Sample exmpl = new Sample();
        exmpl.Run();
    }
}

class Sample
{

    BarcodeScanner? scanner;

    public void Run()
    {

        string deviceName = "RFD40+"; // Device name to connect
        string deviceAddress = ""; // Alternatively you can put a bluetooth address of your scanner

        if (string.IsNullOrEmpty(deviceAddress))
        {
            BluetoothClient bluetoothClient = new BluetoothClient();
            BluetoothDeviceInfo[] devices = bluetoothClient.DiscoverDevices();
            foreach (BluetoothDeviceInfo device in devices)
            {
                if (device.DeviceName.Contains(deviceName))
                {
                    Console.WriteLine($"Device: {device.DeviceName} [{device.DeviceAddress}]");
                    deviceAddress = device.DeviceAddress.ToString();
                    break;
                }
            }
        }

        if (!string.IsNullOrEmpty(deviceAddress))
        {
            this.scanner = new BarcodeScanner(deviceAddress);
            this.scanner.Connect();
            RegisterEvents();
            Console.WriteLine("You can scan!");

            Console.ReadLine();
            this.scanner.Beep();

            Console.ReadLine();
            this.scanner.LedOn();

            Console.ReadLine();
            this.scanner.LedOff();

            Console.ReadLine();
            this.scanner.CapabilitiesRequest();

            Console.ReadLine();
            this.scanner.Disconnect();


        }

    }

    private void RegisterEvents()
    {
        this.scanner.BarcodeEvent += Barcode; //Barcode scan event
        this.scanner.NegativeAckEvent += NegativeAck; // Negative acknowledgment of received packet 
        this.scanner.CapabilitiesReplyEvent += CapabilitiesReply; // List of available commands (return from CapabilitiesRequest)

    }

    private void NegativeAck(object sender, BarcodeScanner.NegativeAckEventArgs e)
    {
        WriteData(e.reason);
    }
    private void Barcode(object sender, BarcodeScanner.BarcodeEventArgs e)
    {
        Console.WriteLine($"{e.barcode} [{e.type}]"); // Write a barcode and type
        
    }
    private void CapabilitiesReply(object sender, BarcodeScanner.CapabilitiesReplyEventArgs e)
    {
        Console.WriteLine("Command codes:");
        WriteData(e.commandCodes);
        Console.WriteLine("Commands:");
        WriteData(e.commands);

    }

    private void WriteData(byte data)
    {
        Console.WriteLine("{0:X} ", data);
    }

    private void WriteData(byte[] data)
    {
        Console.WriteLine(string.Join(" ", data.Select(x => $"{x:X}")));
    }
    private void WriteData(string[] data)
    {
        Console.WriteLine(string.Join(" ", data));
    }

}
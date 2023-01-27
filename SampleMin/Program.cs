using BluetoothSSIConnect;


BarcodeScanner scanner = new BarcodeScanner("48A493BA143A");
scanner.Connect();
scanner.BarcodeEvent += (object sender, BarcodeScanner.BarcodeEventArgs e) => {
    Console.WriteLine($"{e.barcode} [{e.type}]");
};
Console.ReadLine();
scanner.Disconnect();
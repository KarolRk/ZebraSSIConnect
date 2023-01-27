# Bluetooth SSI Connection Library for Zebra Barcode Scanners (C#)

This library allows developers to easily establish a Bluetooth SSI (Serial Port Profile) connection with a Zebra barcode scanner and use it to scan barcodes using C#.

## Getting Started

These instructions will help you set up the library and start using it in your project.

### Prerequisites

* A Zebra barcode scanner that supports Bluetooth SSI Scanner Serial (Serial Port Profile), e.g. RFD40 Premium Plus, RFD90
* A device (e.g. PC, laptop) with Bluetooth capability and support for SPP

### Installation

1. Add the library to your project dependencies.
   * Reference the dll file in your project
2. Initialize the library in your code by creating a new instance of the `BarcodeScanner` class.

### Usage

1. Make sure that Zebra scanner is ready to connect.
2. Find Bluetooth address of the device.
   * Use e.g. InTheHand.Net.Personal library
   * Check in the device properties: Bluetooth Tab -> Unique identifier
3. Connect to the Zebra scanner using the `Connect()` method.
   ```
   BarcodeScanner scanner = new BarcodeScanner(deviceAddress);
   scanner.Connect();
   ```
4. Add barcode event.
   ```
   scanner.BarcodeEvent += (object sender, BarcodeScanner.BarcodeEventArgs e) => {
       Console.WriteLine(e.barcode);
   };
   ```
5. The scanned barcode will be displayed in the app.
6. Disconnect from the Zebra scanner.
   ```
   scanner.Disconnect();
   ```

### Example
```
using BluetoothSSIConnect;

BarcodeScanner scanner = new BarcodeScanner("A1B2C3D4E5F5");
scanner.Connect();
scanner.BarcodeEvent += (object sender, BarcodeScanner.BarcodeEventArgs e) => {
    Console.WriteLine($"{e.barcode} [{e.type}]");
};
Console.ReadLine();
scanner.Disconnect();
```

## Functionality

This library allows developers to easily establish a Bluetooth SSI connection with a Zebra barcode scanner and use it to scan barcodes using C#. The library provides methods to connect, disconnect and get scanned barcode from the scanner.

## Built With

* Visual Studio
* C#

## Author

* Karol Rybak

# Bluetooth SSI Connection with Zebra Barcode Scanner (C#)

This project demonstrates how to establish a Bluetooth SSI connection with a Zebra barcode scanner and use it to scan barcodes using C#.

## Getting Started

These instructions will help you set up the project and establish a connection with the Zebra scanner.

### Prerequisites

* A Zebra barcode scanner that supports Bluetooth SSI Scanner Serial (Serial Port Profile), e.g. RFD40 Premium Plus, RFD90
* A device (e.g. PC, laptop) with Bluetooth capability and support for SPP
* Visual Studio

### Installing

1. Clone or download the project onto your device.
2. Open the project in Visual Studio.
3. Connect the Zebra scanner to your device via Bluetooth.
4. Run the project on your device.

### Usage

1. Press the "Connect" button to establish a connection with the Zebra scanner.
2. Once connected, the scanner will be in "Trigger Mode", meaning it will automatically scan any barcode that is presented to it.
3. The scanned barcode will be displayed in the app.

### Example

```
using InTheHand.Net.Sockets;
using BluetoothSSIConnect;

BarcodeScanner scanner = new BarcodeScanner("48A493BA143A");
scanner.Connect();
scanner.BarcodeEvent += (object sender, BarcodeScanner.BarcodeEventArgs e) => {
    Console.WriteLine($"{e.barcode} [{e.type}]");
};
Console.ReadLine();
scanner.Disconnect();

```

## Functionality

This project demonstrates how to establish a Bluetooth SSI connection with a Zebra barcode scanner and use it to scan barcodes using C#. The app allows the user to connect to the scanner and display the scanned barcode.

## Built With

* Visual Studio
* C#

## Author

* Karol Rybak

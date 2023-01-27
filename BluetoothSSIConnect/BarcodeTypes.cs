namespace BluetoothSSIConnect
{
    internal class BarcodeTypes
    {
        public static String getBarcodeType(int code)
        {
            switch (code)
            {
                case 0x0: return "Unknown";
                case 0x1: return "Code 39";
                case 0x2: return "Codabar";
                case 0x3: return "Code 128";
                case 0x4: return "Discrete 2 of 5";
                case 0x5: return "IATA";
                case 0x6: return "Interleaved 2 of 5";
                case 0x7: return "Code 93";
                case 0x8: return "UPCA";
                case 0x9: return "UPCE 0";
                case 0xA: return "EAN 8";
                case 0xB: return "EAN 13";
                case 0xC: return "Code 11";
                case 0xD: return "Code 49";
                case 0xE: return "MSI";
                case 0xF: return "EAN 128";
                case 0x10: return "UPCE 1";
                case 0x11: return "PDF 417";
                case 0x12: return "Code 16K";
                case 0x13: return "Code 39 Full ASCII";
                case 0x14: return "UPCD";
                case 0x15: return "Trioptic";
                case 0x16: return "Bookland";
                case 0x17: return "Coupon Code";
                case 0x18: return "NW7";
                case 0x19: return "ISBT-128";
                case 0x1A: return "Micro PDF";
                case 0x1B: return "Data Matrix";
                case 0x1C: return "QR Code";
                case 0x1D: return "Micro PDF CCA";
                case 0x1E: return "Postnet US";
                case 0x1F: return "Planet Code";
                case 0x20: return "Code 32";
                case 0x21: return "ISBT-128 Concat";
                case 0x22: return "Japan Postal";
                case 0x23: return "Aus Postal";
                case 0x24: return "Dutch Postal";
                case 0x25: return "Maxicode";
                case 0x26: return "Canada Postal";
                case 0x27: return "UK Postal";
                case 0x28: return "Macro PDF-417";
                case 0x29: return "Macro QR Code";
                case 0x2C: return "Micro QR Code";
                case 0x2D: return "Aztec Code";
                case 0x2E: return "Aztec Rune Code";
                case 0x2F: return "French Lottery";
                case 0x30: return "GS1 Databar";
                case 0x31: return "GS1 Databar Limited";
                case 0x32: return "GS1 Databar Expanded";
                case 0x33: return "Parameter (FNC3)";
                case 0x34: return "4 State US";
                case 0x35: return "4 State US4";
                case 0x36: return "ISSN";
                case 0x37: return "Scanlet Webcode";
                case 0x38: return "Cue CAT Code";
                case 0x39: return "Matrix 2 Of 5";
                case 0x48: return "UPCA + 2";
                case 0x49: return "UPCE0 + 2";
                case 0x4A: return "EAN8 + 2";
                case 0x4B: return "EAN13 + 2";
                case 0x50: return "UPCE1 + 2";
                case 0x51: return "CC-A + EAN-128";
                case 0x52: return "CC-A + EAN-13";
                case 0x53: return "CC-A + EAN-8";
                case 0x54: return "CC-A + GS1 Databar Expanded";
                case 0x55: return "CC-A + GS1 Databar Limited";
                case 0x56: return "CC-A + GS1 Databar";
                case 0x57: return "CC-A + UPCA";
                case 0x58: return "CC-A + UPC-E";
                case 0x59: return "CC-C + EAN-128";
                case 0x5A: return "TLC-39";
                case 0x61: return "CC-B + EAN-128";
                case 0x62: return "CC-B + EAN-13";
                case 0x63: return "CC-B + EAN-8";
                case 0x64: return "CC-B + GS1 Databar Expanded";
                case 0x65: return "CC-B + GS1 Databar Limited";
                case 0x66: return "CC-B + GS1 Databar";
                case 0x67: return "CC-B + UPC-A";
                case 0x68: return "CC-B + UPC-E";
                case 0x69: return "Signature";
                case 0x71: return "Matrix 2 Of 5";
                case 0x72: return "Chinese 2 Of 5";
                case 0x73: return "Korean 3 Of 5";
                case 0x88: return "UPCA 5";
                case 0x89: return "UPCE0 5";
                case 0x8A: return "EAN8 5";
                case 0x8B: return "EAN13 5";
                case 0x90: return "UPCE1 5";
                case 0x9A: return "Macro Micro PDF";
                case 0xA0: return "OCRB";
                case 0xB4: return "GS1 Databar Expanded Coupon";
                case 0xB7: return "Han Xin";
                case 0xC1: return "GS1 Datamatrix";
                case 0xE0: return "RFID Raw";
                case 0xE1: return "RFID URI";
                default: return "";
            }
        }

    }
}

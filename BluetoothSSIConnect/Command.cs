namespace BluetoothSSIConnect
{
    internal class Command
    {
        public const byte ABORT_MACRO_PDF = 0x11;           //Terminates MacroPDF sequence and discards segments.
        public const byte AIM_OFF = 0xC4;                   //Deactivates aim pattern.
        public const byte AIM_ON = 0xC5;                    //Activates aim pattern.
        public const byte BATCH_DATA = 0xD6;                //Transmits stored decode data.
        public const byte BATCH_REQUEST = 0xD5;             //Requests stored decode data.
        public const byte BEEP = 0xE6;                      //Sounds the beeper.
        public const byte CAPABILITIES_REQUEST = 0xD3;      //Requests commands which decoder will perform.
        public const byte CAPABILITIES_REPLY = 0xD4;        //Lists commands which decoder will perform.
        public const byte CHANGE_ALL_CODE_TYPES = 0xC9;     //Enables / Disables all code types.
        public const byte CMD_ACK = 0xD0;                   //Positive acknowledgment of received packet.
        public const byte CMD_ACK_ACTION = 0xD8;            //This is a positive acknowledgment of a received packet and can be used in place of the CMD_ACK command to allow users to control the beeper, pager motor (i.e., vibration feedback) and LEDs after receiving decoded data or any other SSI command. Note: This command in not supported by all scanners.
        public const byte CMD_NAK = 0xD1;                   //Negative acknowledgment of received packet.
        public const byte CUSTOM_DEFAULTS = 0x12;           //Host command to update Custom Defaults Buffer.
        public const byte DECODE_DATA = 0xF3;               //Decode data in SSI packet format.
        public const byte EVENT = 0xF6;                     //Event indicated by associated event code.
        public const byte FLUSH_MACRO_PDF = 0x10;           //Terminates MacroPDF sequence and transmits captured segments.
        public const byte FLUSH_QUEUE = 0xD2;               //Tells the decoder to eliminate all packets in its transmission queue.
        public const byte ILLUMINATION_OFF = 0xC0;          //Deactivates Illumination
        public const byte ILLUMINATION_ON = 0xC1;           //Activates Illumination.
        public const byte IMAGE_DATA = 0xB1;                //Data comprising the image.
        public const byte IMAGER_MODE = 0xF7;               //Commands imager into operational modes.
        public const byte LED_OFF = 0xE8;                   //Extinguishes LEDs.
        public const byte LED_ON = 0xE7;                    //Activates LED output.
        public const byte PAGER_MOTOR_ACTIVATION = 0xCA;    //Actuates the vibration feedback.
        public const byte PARAM_DEFAULTS = 0xC8;            //Sets parameter default values.
        public const byte PARAM_REQUEST = 0xC7;             //Requests values of certain parameters.
        public const byte PARAM_SEND = 0xC6;                //Sends parameter values.
        public const byte REPLY_REVISION = 0xA4;            //Replies to REQUEST_REVISION with decoder's software/hardware configuration.
        public const byte REQUEST_REVISION = 0xA3;          //Requests the decoder's configuration.
        public const byte SCAN_DISABLE = 0xEA;              //Prevents the operator from scanning bar codes.
        public const byte SCAN_ENABLE = 0xE9;               //Permits bar code scanning.
        public const byte SLEEP = 0xEB;                     //Requests to place the decoder into low power.
        public const byte SSI_MGMT_COMMAND = 0x80;          //RSM command to read/set some scanner attributes.
        public const byte START_SESSION = 0xE4;             //Tells decoder to attempt to decode a bar code.
        public const byte STOP_SESSION = 0xE5;              //Tells decoder to abort a decode attempt.
        public const byte VIDEO_DATA = 0xB4;                //Data comprising the video.

    }
}

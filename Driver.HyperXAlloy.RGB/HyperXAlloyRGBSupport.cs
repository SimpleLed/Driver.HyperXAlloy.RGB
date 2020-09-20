using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HidSharp;
using SimpleLed;

namespace Driver.HyperXAlloy.RGB
{
    public class HyperXKeyboardSupport
    {
        public static string[] KeyNames = {
    "ESC",
    "OEM8",
    "TAB",
    "CAPITAL",
    "LSHIFT",
    "LCONTROL",
    "F12",
    "OEMPLUS",
    "F9",
    "D9",
    "O",
    "L",
    "OEMCOMMA",
    "APPS",
    "ENTER",
    "LEFT",
    "F1",
    "D1",
    "Q",
    "A",
    "OEMPIPE",
    "LWIN",
    "PRINTSCREEN",
    "F10",
    "D0",
    "P",
    "OEMSEMICOLON",
    "OEMPERIOD",
    "ENTER",
    "DOWN",
    "F2",
    "D2",
    "W",
    "S",
    "Z",
    "LMENU",
    "SCROLL",
    "BACK",
    "F11",
    "OEMMINUS",
    "OEMOPENBRACKETS",
    "OEMTILDE",
    "OEMQUESTION",
    "RIGHT",
    "F3",
    "D3",
    "E",
    "D",
    "X",
    "PAUSE",
    "DELETE",
    "NUMPAD7",
    "NUMLOCK",
    "NUMPAD6",
    "F4",
    "D4",
    "R",
    "F",
    "C",
    "SPACE",
    "INSERT",
    "END",
    "NUMPAD8",
    "DIVIDE",
    "NUMPAD1",
    "F5",
    "D5",
    "T",
    "G",
    "V",
    "HOME",
    "NEXT",
    "NUMPAD9",
    "MULTIPLY",
    "NUMPAD2",
    "F6",
    "D6",
    "Y",
    "H",
    "B",
    "PRIOR",
    "RSHIFT",
    "SUBTRACT",
    "NUMPAD3",
    "F7",
    "D7",
    "U",
    "J",
    "N",
    "RMENU",
    "OEMCLOSEBRACKETS",
    "RCONTROL",
    "NUMPAD4",
    "ADD",
    "NUMPAD0",
    "F8",
    "D8",
    "I",
    "K",
    "M",
    "RWIN",
    "OEMPIPE",
    "UP",
    "NUMPAD5",
    "NUMPADEnter",
    "DECIMAL",
    "trip 1",
    "trip 2",
    "trip 3",
    "trip 4",
    "trip 5",
    "trip 6",
    "trip 7",
    "trip 8",
    "trip 9",
    "trip 10",
    "trip 11",
    "trip 12",
    "trip 13",
    "trip 14",
    "trip 15",
    "trip 16",
    "trip 17",
    "trip 18",
    "Media Previous",
    "Media Play/Pause",
    "Media Next",
    "Media Mute"
};

        public static uint NA = 0xFFFFFFFF;
        public static uint[][] matrixMap = new uint[][] {
    new uint[]{   0, NA, 16, 30, 44, 54, NA, 65, 75, 84, 95, NA, 8, 23, 38, 6, 22, 36, 49, NA, NA, NA, NA},
    new uint[]{   1,  17,  31,  45,  55,  66,  76,  85,  96,   9,  24, NA,  39,   7 ,  37, NA,  60,  70,  80,  52,  63,  73,  82 },
    new uint[]{ 2,  NA,  18,  32,  46,  56,  NA,  67,  77,  86,  97,  10,  25,  40 ,  90,  101,  50,  61,  71,  51,  62,  72,  93 },
    new uint[]{ 3,  NA,  19,  33,  47,  57,  NA,  68,  78,  87,  98,  11,  26,  41 ,  28,  14 ,  NA,  NA,  NA,  92, 103,  53,  NA },
    new uint[]{ 4,  20,  34,  48,  58,  69,  NA,  79,  NA,  88,  99,  12,  27,  42 ,  81,  NA ,  NA, 102,  NA,  64,  74,  83, 104 },
    new uint[]{ 5,  21,  35,  NA,  NA,  NA,  NA,  59,  NA,  NA,  NA,  NA,  89,  100,  13,  91 ,  15,  29,  43,  94,  NA, 105,  NA } };



        byte[] keys = new byte[]{0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14,
                0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x20, 0x21, 0x22,
                0x23, 0x24, 0x26, 0x27, 0x28, 0x29, 0x2A, 0x2B, 0x2C, 0x2D, 0x2E, 0x2F, 0x30,
                0x31, 0x32, 0x33, 0x34, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3E, 0x3F, 0x41,
                0x44, 0x45, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0x51, 0x54, 0x55,
                0x58, 0x59, 0x5A, 0x5B, 0x5C, 0x5E, 0x5F, 0x61, 0x64, 0x65, 0x68, 0x69, 0x6A,
                0x6B, 0x6C, 0x6E, 0x6F, 0x74, 0x75, 0x78, 0x79, 0x7A, 0x7B, 0x7C, 0x7D, 0x7E,
                0x7F, 0x81, 0x84, 0x85, 0x88, 0x89, 0x8A, 0x8B, 0x8C, 0x8D, 0x8E, 0x8F, 0x91,
                0x94, 0x95 };

        HidDevice device = null;
        HidStream stream = null;
        static byte[] skip_idx = { 6, 23, 29, 41, 47, 59, 70, 71, 75, 76, 87, 88, 93, 99, 100, 102, 108, 113, 114, 120, 123, 124 };

        public int VENDOR_ID = 9494;

        public int PRODUCT_ID = 81;
        public int USB_INTERFACE = 0;

        public uint[] OrderOfKeys;
        public XY[] humm;
        public List<XY> xy;
        public List<uint> order = new List<uint>();
        public class XY
        {
            public int X { get; set; }
            public int Y { get; set; }
            public uint Key { get; set; }
            public int Order { get; set; }

            public LEDColor Color { get; set; }
            public string DebugName { get; set; }
            public string DebugName2 { get; set; }

            public override string ToString() => $"{X},{Y} - {Key} / {DebugName}";
        }
        public HyperXKeyboardSupport(int vid, int pid, int usb)
        {
            List<uint> order = new List<uint>();
            xy = new List<XY>();
            int pp = 0;

            humm = new XY[106];
            for (int x = 0; x < 23; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    var tt = matrixMap[y][x];
                    if (tt == 4294967295)
                    {

                    }
                    else
                    {
                        humm[tt] = new XY
                        {
                            X = x,
                            Y = y,
                            Order = (int) tt,
                            DebugName = KeyNames[tt]
                        };
                    }

                }
            }

            xy = humm.ToList();
            //for (int x = 0; x < 23; x++)
            //{
            //    for (int y = 0; y < 6; y++)
            //    {
            //        var tt = matrixMap[y][x];
            //        if (tt != NA)
            //        {

            //            int ypos = (int)(tt % 6);
            //            int xpos = (int) ((tt - ypos) / 6);

            //            var poopoo = new XY
            //            {
            //                X = x,
            //                Y = y,
            //                Key = tt,
            //                Order = pp,
            //                DebugName = KeyNames[pp],
            //                DebugName2 = KeyNames[tt]
            //            };

            //            xy.Add(poopoo);
            //             Debug.WriteLine(tt);
            //            order.Add(tt);
            //            pp++;
            //        }
                    
            //    }
            //}

            OrderOfKeys = order.ToArray();

            OrderOfKeys = xy.OrderBy(p => p.X + p.Y).Select(p => p.Key).ToArray();

            VENDOR_ID = vid;

            PRODUCT_ID = pid;

            USB_INTERFACE = usb;

            var terp = new OpenConfiguration();
            terp.SetOption(OpenOption.Interruptible, true);

            var loader = new HidDeviceLoader();

            var devices = loader.GetDevices(VENDOR_ID, PRODUCT_ID).ToArray();

            int ct = 0;
            foreach (var tdevice in devices)
            {
                ct++;
                HidStream tmpstream;// = tdevice.Open(terp);

                try
                {
                    tmpstream = tdevice.Open(terp);
                    if (tmpstream != null)
                    {
                        Debug.WriteLine("");
                        Debug.WriteLine("device opened " + ct + " " + tdevice.DevicePath);
                        SwitchProfile(0, tmpstream);
                        Debug.WriteLine("Well, that seemed to work");
                        stream = tmpstream;

                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }

            if (stream == null)
            {
                throw new Exception("Stream null");
            }
        }

        public LEDColor[] From2D(ControlDevice.LedUnit[,] input)
        {
            var rrr = new LEDColor[106];
            int ct = 0;
            for (int y = 0; y < input.GetLength(1); y++)
            {
                for (int x = 0; x < input.GetLength(0); x++)
                {
                    var mm = matrixMap[y][x];
                    if (mm != NA)
                    {
                        if (input[x, y] != null)
                        {
                            rrr[mm] = (input[x, y].Color);
                            ct++;
                        }
                    }
                }
            }

            return rrr.ToArray();
        }


        public void SwitchProfile(byte profile, HidStream str = null)
        {
            byte[] buf = new byte[264];
            buf[0] = 0x07;
            buf[1] = 0x81;
            buf[2] = profile;

            if (str == null) str = stream;

            str.SendData(buf);
        }

        public void SendColors(LEDColor[] colors)
        {
            var red = BuildPacket(colors, PacketColor.Red);
            var green = BuildPacket(colors, PacketColor.Green);
            var blue = BuildPacket(colors, PacketColor.Blue);
            stream.SendData(red);
            Thread.Sleep(5);
            stream.SendData(green);
            Thread.Sleep(5);
            stream.SendData(blue);
            Thread.Sleep(5);
        }

        public enum PacketColor
        {
            Red = 1,
            Green = 2,
            Blue = 3
        }


        public byte[] BuildPacket(LEDColor[] colors, PacketColor packetColor)
        {
            byte[] buf = new byte[264];
            buf[0] = 0x07;
            buf[1] = 0x16;

            buf[2] = (byte)packetColor;
            buf[3] = 0xA0;

            int ptr = 0;
            for (int ct = 0; ct < colors.Length; ct++)
            {
                ptr = ct;
                int rindex = (int)OrderOfKeys[ptr];

                var ledColor = colors[ct];
                buf[keys[ct]] = packetColor == PacketColor.Red ? (byte)ledColor.Red : packetColor == PacketColor.Green ? (byte)ledColor.Green : (byte)ledColor.Blue;

            }

            return buf;
        }

        public void SendDirectColorPacket(LEDColor[] colours)
        {
            byte[] buf = new byte[64];
            int ps = 0;
            for (byte color_idx = 0; color_idx < colours.Length; color_idx++)
            {

                buf[ps] = 0x81;
                buf[ps + 1] = (byte)colours[color_idx].Red;
                buf[ps + 2] = (byte)colours[color_idx].Green;
                buf[ps + 3] = (byte)colours[color_idx].Blue;
                ps = ps + 4;

            }

            stream.SendData(buf);
        }

    }

    public static class HXExtensions
    {
        static bool useStreamWrite = false;
        public static void SendData(this HidStream stream, byte[] buffer)
        {


            if (useStreamWrite)
            {
                stream.Write(buffer);
            }
            else
            {
                stream.SetFeature(buffer);
            }
        }
    }

}

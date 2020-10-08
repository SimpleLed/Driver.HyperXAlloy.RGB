using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HidSharp;
using Newtonsoft.Json;
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
            public int ScanCode { get; set; }

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
                            DebugName = KeyNames[tt],
                            
                        };
                    }

                }
            }

            string shitmap = @"[
  {
    ""X"": 0,
    ""Y"": 0,
    ""Key"": 0,
    ""Order"": 0,
    ""Color"": null,
    ""DebugName"": ""ESC"",
    ""DebugName2"": null,
    ""ScanCode"": 1
  },
  {
    ""X"": 0,
    ""Y"": 1,
    ""Key"": 0,
    ""Order"": 1,
    ""Color"": null,
    ""DebugName"": ""OEM8"",
    ""DebugName2"": null,
    ""ScanCode"": 41
    
  },
  {
    ""X"": 0,
    ""Y"": 2,
    ""Key"": 0,
    ""Order"": 2,
    ""Color"": null,
    ""DebugName"": ""TAB"",
    ""DebugName2"": null,
    ""ScanCode"": 15
  },
  {
    ""X"": 0,
    ""Y"": 3,
    ""Key"": 0,
    ""Order"": 3,
    ""Color"": null,
    ""DebugName"": ""CAPITAL"",
    ""DebugName2"": null,
    ""ScanCode"": 58
  },
  {
    ""X"": 0,
    ""Y"": 4,
    ""Key"": 0,
    ""Order"": 4,
    ""Color"": null,
    ""DebugName"": ""LSHIFT"",
    ""DebugName2"": null,
    ""ScanCode"": 42
  },
  {
    ""X"": 0,
    ""Y"": 5,
    ""Key"": 0,
    ""Order"": 5,
    ""Color"": null,
    ""DebugName"": ""LCONTROL"",
    ""DebugName2"": null,
    ""ScanCode"": 29
  },
  {
    ""X"": 15,
    ""Y"": 0,
    ""Key"": 0,
    ""Order"": 6,
    ""Color"": null,
    ""DebugName"": ""F12"",
    ""DebugName2"": null,
    ""ScanCode"": 88
  },
  {
    ""X"": 13,
    ""Y"": 1,
    ""Key"": 0,
    ""Order"": 7,
    ""Color"": null,
    ""DebugName"": ""OEMPLUS"",
    ""DebugName2"": null,
    ""ScanCode"": 13
  },
  {
    ""X"": 12,
    ""Y"": 0,
    ""Key"": 0,
    ""Order"": 8,
    ""Color"": null,
    ""DebugName"": ""F9"",
    ""DebugName2"": null,
    ""ScanCode"": 67
  },
  {
    ""X"": 9,
    ""Y"": 1,
    ""Key"": 0,
    ""Order"": 9,
    ""Color"": null,
    ""DebugName"": ""D9"",
    ""DebugName2"": null,
    ""ScanCode"": 10
  },
  {
    ""X"": 11,
    ""Y"": 2,
    ""Key"": 0,
    ""Order"": 10,
    ""Color"": null,
    ""DebugName"": ""O"",
    ""DebugName2"": null,
    ""ScanCode"": 24
  },
  {
    ""X"": 11,
    ""Y"": 3,
    ""Key"": 0,
    ""Order"": 11,
    ""Color"": null,
    ""DebugName"": ""L"",
    ""DebugName2"": null,
    ""ScanCode"": 38
  },
  {
    ""X"": 11,
    ""Y"": 4,
    ""Key"": 0,
    ""Order"": 12,
    ""Color"": null,
    ""DebugName"": ""OEMCOMMA"",
    ""DebugName2"": null,
    ""ScanCode"": 51
  },
  {
    ""X"": 14,
    ""Y"": 5,
    ""Key"": 0,
    ""Order"": 13,
    ""Color"": null,
    ""DebugName"": ""APPS"",
    ""DebugName2"": null,
    ""ScanCode"": 56
  },
  {
    ""X"": 15,
    ""Y"": 3,
    ""Key"": 0,
    ""Order"": 14,
    ""Color"": null,
    ""DebugName"": ""ENTER"",
    ""DebugName2"": null,
    ""ScanCode"": 28
    
  },
  {
    ""X"": 16,
    ""Y"": 5,
    ""Key"": 0,
    ""Order"": 15,
    ""Color"": null,
    ""DebugName"": ""LEFT"",
    ""DebugName2"": null,
    ""ScanCode"": 107
  },
  {
    ""X"": 2,
    ""Y"": 0,
    ""Key"": 0,
    ""Order"": 16,
    ""Color"": null,
    ""DebugName"": ""F1"",
    ""DebugName2"": null,
    ""ScanCode"": 59
  },
  {
    ""X"": 1,
    ""Y"": 1,
    ""Key"": 0,
    ""Order"": 17,
    ""Color"": null,
    ""DebugName"": ""D1"",
    ""DebugName2"": null,
    ""ScanCode"": 2
  },
  {
    ""X"": 2,
    ""Y"": 2,
    ""Key"": 0,
    ""Order"": 18,
    ""Color"": null,
    ""DebugName"": ""Q"",
    ""DebugName2"": null,
    ""ScanCode"": 16
  },
  {
    ""X"": 2,
    ""Y"": 3,
    ""Key"": 0,
    ""Order"": 19,
    ""Color"": null,
    ""DebugName"": ""A"",
    ""DebugName2"": null,
    ""ScanCode"": 30
  },
  {
    ""X"": 1,
    ""Y"": 4,
    ""Key"": 0,
    ""Order"": 20,
    ""Color"": null,
    ""DebugName"": ""OEMPIPE"",
    ""DebugName2"": null,
    ""ScanCode"": 86
  },
  {
    ""X"": 1,
    ""Y"": 5,
    ""Key"": 0,
    ""Order"": 21,
    ""Color"": null,
    ""DebugName"": ""LWIN"",
    ""DebugName2"": null,
    ""ScanCode"": 91
  },
  {
    ""X"": 16,
    ""Y"": 0,
    ""Key"": 0,
    ""Order"": 22,
    ""Color"": null,
    ""DebugName"": ""PRINTSCREEN"",
    ""DebugName2"": null,
    ""ScanCode"": 55
  },
  {
    ""X"": 13,
    ""Y"": 0,
    ""Key"": 0,
    ""Order"": 23,
    ""Color"": null,
    ""DebugName"": ""F10"",
    ""DebugName2"": null,
    ""ScanCode"": 68
  },
  {
    ""X"": 10,
    ""Y"": 1,
    ""Key"": 0,
    ""Order"": 24,
    ""Color"": null,
    ""DebugName"": ""D0"",
    ""DebugName2"": null,
    ""ScanCode"": 11
  },
  {
    ""X"": 12,
    ""Y"": 2,
    ""Key"": 0,
    ""Order"": 25,
    ""Color"": null,
    ""DebugName"": ""P"",
    ""DebugName2"": null,
    ""ScanCode"": 25
  },
  {
    ""X"": 12,
    ""Y"": 3,
    ""Key"": 0,
    ""Order"": 26,
    ""Color"": null,
    ""DebugName"": ""OEMSEMICOLON"",
    ""DebugName2"": null,
    ""ScanCode"": 39
  },
  {
    ""X"": 12,
    ""Y"": 4,
    ""Key"": 0,
    ""Order"": 27,
    ""Color"": null,
    ""DebugName"": ""OEMPERIOD"",
    ""DebugName2"": null,
    ""ScanCode"": 52
  },
  {
    ""X"": 14,
    ""Y"": 3,
    ""Key"": 0,
    ""Order"": 28,
    ""Color"": null,
    ""DebugName"": ""ENTER"",
    ""DebugName2"": null,
    ""ScanCode"": 28
  },
  {
    ""X"": 17,
    ""Y"": 5,
    ""Key"": 0,
    ""Order"": 29,
    ""Color"": null,
    ""DebugName"": ""DOWN"",
    ""DebugName2"": null,
    ""ScanCode"": 72
  },
  {
    ""X"": 3,
    ""Y"": 0,
    ""Key"": 0,
    ""Order"": 30,
    ""Color"": null,
    ""DebugName"": ""F2"",
    ""DebugName2"": null,
    ""ScanCode"": 60
  },
  {
    ""X"": 2,
    ""Y"": 1,
    ""Key"": 0,
    ""Order"": 31,
    ""Color"": null,
    ""DebugName"": ""D2"",
    ""DebugName2"": null,
    ""ScanCode"": 3
  },
  {
    ""X"": 3,
    ""Y"": 2,
    ""Key"": 0,
    ""Order"": 32,
    ""Color"": null,
    ""DebugName"": ""W"",
    ""DebugName2"": null,
    ""ScanCode"": 17
  },
  {
    ""X"": 3,
    ""Y"": 3,
    ""Key"": 0,
    ""Order"": 33,
    ""Color"": null,
    ""DebugName"": ""S"",
    ""DebugName2"": null,
    ""ScanCode"": 31
  },
  {
    ""X"": 2,
    ""Y"": 4,
    ""Key"": 0,
    ""Order"": 34,
    ""Color"": null,
    ""DebugName"": ""Z"",
    ""DebugName2"": null,
    ""ScanCode"": 44
  },
  {
    ""X"": 2,
    ""Y"": 5,
    ""Key"": 0,
    ""Order"": 35,
    ""Color"": null,
    ""DebugName"": ""LMENU"",
    ""DebugName2"": null,
    ""ScanCode"": 56
  },
  {
    ""X"": 17,
    ""Y"": 0,
    ""Key"": 0,
    ""Order"": 36,
    ""Color"": null,
    ""DebugName"": ""SCROLL"",
    ""DebugName2"": null,
    ""ScanCode"": 70
  },
  {
    ""X"": 14,
    ""Y"": 1,
    ""Key"": 0,
    ""Order"": 37,
    ""Color"": null,
    ""DebugName"": ""BACK"",
    ""DebugName2"": null,
    ""ScanCode"": 14
  },
  {
    ""X"": 14,
    ""Y"": 0,
    ""Key"": 0,
    ""Order"": 38,
    ""Color"": null,
    ""DebugName"": ""F11"",
    ""DebugName2"": null    ,
    ""ScanCode"": 87
  },
  {
    ""X"": 12,
    ""Y"": 1,
    ""Key"": 0,
    ""Order"": 39,
    ""Color"": null,
    ""DebugName"": ""OEMMINUS"",
    ""DebugName2"": null,
    ""ScanCode"": 12
  },
  {
    ""X"": 13,
    ""Y"": 2,
    ""Key"": 0,
    ""Order"": 40,
    ""Color"": null,
    ""DebugName"": ""OEMOPENBRACKETS"",
    ""DebugName2"": null,
    ""ScanCode"": 26
  },
  {
    ""X"": 13,
    ""Y"": 3,
    ""Key"": 0,
    ""Order"": 41,
    ""Color"": null,
    ""DebugName"": ""OEMTILDE"",
    ""DebugName2"": null,
    ""ScanCode"": 41
  },
  {
    ""X"": 13,
    ""Y"": 4,
    ""Key"": 0,
    ""Order"": 42,
    ""Color"": null,
    ""DebugName"": ""OEMQUESTION"",
    ""DebugName2"": null,
    ""ScanCode"": 53
  },
  {
    ""X"": 18,
    ""Y"": 5,
    ""Key"": 0,
    ""Order"": 43,
    ""Color"": null,
    ""DebugName"": ""RIGHT"",
    ""DebugName2"": null,
    ""ScanCode"": 77
  },
  {
    ""X"": 4,
    ""Y"": 0,
    ""Key"": 0,
    ""Order"": 44,
    ""Color"": null,
    ""DebugName"": ""F3"",
    ""DebugName2"": null,
    ""ScanCode"": 61
  },
  {
    ""X"": 3,
    ""Y"": 1,
    ""Key"": 0,
    ""Order"": 45,
    ""Color"": null,
    ""DebugName"": ""D3"",
    ""DebugName2"": null,
    ""ScanCode"": 4
  },
  {
    ""X"": 4,
    ""Y"": 2,
    ""Key"": 0,
    ""Order"": 46,
    ""Color"": null,
    ""DebugName"": ""E"",
    ""DebugName2"": null,
    ""ScanCode"": 18
  },
  {
    ""X"": 4,
    ""Y"": 3,
    ""Key"": 0,
    ""Order"": 47,
    ""Color"": null,
    ""DebugName"": ""D"",
    ""DebugName2"": null,
    ""ScanCode"": 32
  },
  {
    ""X"": 3,
    ""Y"": 4,
    ""Key"": 0,
    ""Order"": 48,
    ""Color"": null,
    ""DebugName"": ""X"",
    ""DebugName2"": null,
    ""ScanCode"": 45
  },
  {
    ""X"": 18,
    ""Y"": 0,
    ""Key"": 0,
    ""Order"": 49,
    ""Color"": null,
    ""DebugName"": ""PAUSE"",
    ""DebugName2"": null
    ,
    ""ScanCode"": 29
  },
  {
    ""X"": 16,
    ""Y"": 2,
    ""Key"": 0,
    ""Order"": 50,
    ""Color"": null,
    ""DebugName"": ""DELETE"",
    ""DebugName2"": null,
    ""ScanCode"": 83
  },
  {
    ""X"": 19,
    ""Y"": 2,
    ""Key"": 0,
    ""Order"": 51,
    ""Color"": null,
    ""DebugName"": ""NUMPAD7"",
    ""DebugName2"": null,
    ""ScanCode"": 71
  },
  {
    ""X"": 19,
    ""Y"": 1,
    ""Key"": 0,
    ""Order"": 52,
    ""Color"": null,
    ""DebugName"": ""NUMLOCK"",
    ""DebugName2"": null,
    ""ScanCode"": 69
  },
  {
    ""X"": 21,
    ""Y"": 3,
    ""Key"": 0,
    ""Order"": 53,
    ""Color"": null,
    ""DebugName"": ""NUMPAD6"",
    ""DebugName2"": null,
    ""ScanCode"": 77
  },
  {
    ""X"": 5,
    ""Y"": 0,
    ""Key"": 0,
    ""Order"": 54,
    ""Color"": null,
    ""DebugName"": ""F4"",
    ""DebugName2"": null,
    ""ScanCode"": 62
  },
  {
    ""X"": 4,
    ""Y"": 1,
    ""Key"": 0,
    ""Order"": 55,
    ""Color"": null,
    ""DebugName"": ""D4"",
    ""DebugName2"": null,
    ""ScanCode"": 5
  },
  {
    ""X"": 5,
    ""Y"": 2,
    ""Key"": 0,
    ""Order"": 56,
    ""Color"": null,
    ""DebugName"": ""R"",
    ""DebugName2"": null,
    ""ScanCode"": 19
  },
  {
    ""X"": 5,
    ""Y"": 3,
    ""Key"": 0,
    ""Order"": 57,
    ""Color"": null,
    ""DebugName"": ""F"",
    ""DebugName2"": null,
    ""ScanCode"": 33
  },
  {
    ""X"": 4,
    ""Y"": 4,
    ""Key"": 0,
    ""Order"": 58,
    ""Color"": null,
    ""DebugName"": ""C"",
    ""DebugName2"": null,
    ""ScanCode"": 46
  },
  {
    ""X"": 7,
    ""Y"": 5,
    ""Key"": 0,
    ""Order"": 59,
    ""Color"": null,
    ""DebugName"": ""SPACE"",
    ""DebugName2"": null,
    ""ScanCode"": 57
  },
  {
    ""X"": 16,
    ""Y"": 1,
    ""Key"": 0,
    ""Order"": 60,
    ""Color"": null,
    ""DebugName"": ""INSERT"",
    ""DebugName2"": null,
    ""ScanCode"": 82
  },
  {
    ""X"": 17,
    ""Y"": 2,
    ""Key"": 0,
    ""Order"": 61,
    ""Color"": null,
    ""DebugName"": ""END"",
    ""DebugName2"": null,
    ""ScanCode"": 79
  },
  {
    ""X"": 20,
    ""Y"": 2,
    ""Key"": 0,
    ""Order"": 62,
    ""Color"": null,
    ""DebugName"": ""NUMPAD8"",
    ""DebugName2"": null,
    ""ScanCode"": 72
  },
  {
    ""X"": 20,
    ""Y"": 1,
    ""Key"": 0,
    ""Order"": 63,
    ""Color"": null,
    ""DebugName"": ""DIVIDE"",
    ""DebugName2"": null,
    ""ScanCode"": 53
  },
  {
    ""X"": 19,
    ""Y"": 4,
    ""Key"": 0,
    ""Order"": 64,
    ""Color"": null,
    ""DebugName"": ""NUMPAD1"",
    ""DebugName2"": null,
    ""ScanCode"": 79
  },
  {
    ""X"": 7,
    ""Y"": 0,
    ""Key"": 0,
    ""Order"": 65,
    ""Color"": null,
    ""DebugName"": ""F5"",
    ""DebugName2"": null,
    ""ScanCode"": 63
  },
  {
    ""X"": 5,
    ""Y"": 1,
    ""Key"": 0,
    ""Order"": 66,
    ""Color"": null,
    ""DebugName"": ""D5"",
    ""DebugName2"": null,
    ""ScanCode"": 6
  },
  {
    ""X"": 7,
    ""Y"": 2,
    ""Key"": 0,
    ""Order"": 67,
    ""Color"": null,
    ""DebugName"": ""T"",
    ""DebugName2"": null,
    ""ScanCode"": 20
  },
  {
    ""X"": 7,
    ""Y"": 3,
    ""Key"": 0,
    ""Order"": 68,
    ""Color"": null,
    ""DebugName"": ""G"",
    ""DebugName2"": null,
    ""ScanCode"": 34
  },
  {
    ""X"": 5,
    ""Y"": 4,
    ""Key"": 0,
    ""Order"": 69,
    ""Color"": null,
    ""DebugName"": ""V"",
    ""DebugName2"": null,
    ""ScanCode"": 47
  },
  {
    ""X"": 17,
    ""Y"": 1,
    ""Key"": 0,
    ""Order"": 70,
    ""Color"": null,
    ""DebugName"": ""HOME"",
    ""DebugName2"": null,
    ""ScanCode"": 71
  },
  {
    ""X"": 18,
    ""Y"": 2,
    ""Key"": 0,
    ""Order"": 71,
    ""Color"": null,
    ""DebugName"": ""NEXT"",
    ""DebugName2"": null,
    ""ScanCode"": 81
  },
  {
    ""X"": 21,
    ""Y"": 2,
    ""Key"": 0,
    ""Order"": 72,
    ""Color"": null,
    ""DebugName"": ""NUMPAD9"",
    ""DebugName2"": null,
    ""ScanCode"": 73
  },
  {
    ""X"": 21,
    ""Y"": 1,
    ""Key"": 0,
    ""Order"": 73,
    ""Color"": null,
    ""DebugName"": ""MULTIPLY"",
    ""DebugName2"": null,
    ""ScanCode"": 55
  },
  {
    ""X"": 20,
    ""Y"": 4,
    ""Key"": 0,
    ""Order"": 74,
    ""Color"": null,
    ""DebugName"": ""NUMPAD2"",
    ""DebugName2"": null,
    ""ScanCode"": 80
  },
  {
    ""X"": 8,
    ""Y"": 0,
    ""Key"": 0,
    ""Order"": 75,
    ""Color"": null,
    ""DebugName"": ""F6"",
    ""DebugName2"": null,
    ""ScanCode"": 64
  },
  {
    ""X"": 6,
    ""Y"": 1,
    ""Key"": 0,
    ""Order"": 76,
    ""Color"": null,
    ""DebugName"": ""D6"",
    ""DebugName2"": null,
    ""ScanCode"": 7
  },
  {
    ""X"": 8,
    ""Y"": 2,
    ""Key"": 0,
    ""Order"": 77,
    ""Color"": null,
    ""DebugName"": ""Y"",
    ""DebugName2"": null,
    ""ScanCode"": 21
  },
  {
    ""X"": 8,
    ""Y"": 3,
    ""Key"": 0,
    ""Order"": 78,
    ""Color"": null,
    ""DebugName"": ""H"",
    ""DebugName2"": null,
    ""ScanCode"": 35
  },
  {
    ""X"": 7,
    ""Y"": 4,
    ""Key"": 0,
    ""Order"": 79,
    ""Color"": null,
    ""DebugName"": ""B"",
    ""DebugName2"": null,
    ""ScanCode"": 48
  },
  {
    ""X"": 18,
    ""Y"": 1,
    ""Key"": 0,
    ""Order"": 80,
    ""Color"": null,
    ""DebugName"": ""PRIOR"",
    ""DebugName2"": null,
    ""ScanCode"": 73
  },
  {
    ""X"": 14,
    ""Y"": 4,
    ""Key"": 0,
    ""Order"": 81,
    ""Color"": null,
    ""DebugName"": ""RSHIFT"",
    ""DebugName2"": null,
    ""ScanCode"": 54
  },
  {
    ""X"": 22,
    ""Y"": 1,
    ""Key"": 0,
    ""Order"": 82,
    ""Color"": null,
    ""DebugName"": ""SUBTRACT"",
    ""DebugName2"": null,
    ""ScanCode"": 74
  },
  {
    ""X"": 21,
    ""Y"": 4,
    ""Key"": 0,
    ""Order"": 83,
    ""Color"": null,
    ""DebugName"": ""NUMPAD3"",
    ""DebugName2"": null,
    ""ScanCode"": 81
  },
  {
    ""X"": 9,
    ""Y"": 0,
    ""Key"": 0,
    ""Order"": 84,
    ""Color"": null,
    ""DebugName"": ""F7"",
    ""DebugName2"": null,
    ""ScanCode"": 65
  },
  {
    ""X"": 7,
    ""Y"": 1,
    ""Key"": 0,
    ""Order"": 85,
    ""Color"": null,
    ""DebugName"": ""D7"",
    ""DebugName2"": null,
    ""ScanCode"": 8
  },
  {
    ""X"": 9,
    ""Y"": 2,
    ""Key"": 0,
    ""Order"": 86,
    ""Color"": null,
    ""DebugName"": ""U"",
    ""DebugName2"": null,
    ""ScanCode"": 22
  },
  {
    ""X"": 9,
    ""Y"": 3,
    ""Key"": 0,
    ""Order"": 87,
    ""Color"": null,
    ""DebugName"": ""J"",
    ""DebugName2"": null,
    ""ScanCode"": 36
  },
  {
    ""X"": 9,
    ""Y"": 4,
    ""Key"": 0,
    ""Order"": 88,
    ""Color"": null,
    ""DebugName"": ""N"",
    ""DebugName2"": null,
    ""ScanCode"": 49
  },
  {
    ""X"": 12,
    ""Y"": 5,
    ""Key"": 0,
    ""Order"": 89,
    ""Color"": null,
    ""DebugName"": ""RMENU"",
    ""DebugName2"": null,
    ""ScanCode"": 56
  },
  {
    ""X"": 14,
    ""Y"": 2,
    ""Key"": 0,
    ""Order"": 90,
    ""Color"": null,
    ""DebugName"": ""OEMCLOSEBRACKETS"",
    ""DebugName2"": null,
    ""ScanCode"": 27
  },
  {
    ""X"": 15,
    ""Y"": 5,
    ""Key"": 0,
    ""Order"": 91,
    ""Color"": null,
    ""DebugName"": ""RCONTROL"",
    ""DebugName2"": null,
    ""ScanCode"": 29
  },
  {
    ""X"": 19,
    ""Y"": 3,
    ""Key"": 0,
    ""Order"": 92,
    ""Color"": null,
    ""DebugName"": ""NUMPAD4"",
    ""DebugName2"": null,
    ""ScanCode"": 75
  },
  {
    ""X"": 22,
    ""Y"": 2,
    ""Key"": 0,
    ""Order"": 93,
    ""Color"": null,
    ""DebugName"": ""ADD"",
    ""DebugName2"": null,
    ""ScanCode"": 78
  },
  {
    ""X"": 19,
    ""Y"": 5,
    ""Key"": 0,
    ""Order"": 94,
    ""Color"": null,
    ""DebugName"": ""NUMPAD0"",
    ""DebugName2"": null,
    ""ScanCode"": 82
  },
  {
    ""X"": 10,
    ""Y"": 0,
    ""Key"": 0,
    ""Order"": 95,
    ""Color"": null,
    ""DebugName"": ""F8"",
    ""DebugName2"": null,
    ""ScanCode"": 88
  },
  {
    ""X"": 8,
    ""Y"": 1,
    ""Key"": 0,
    ""Order"": 96,
    ""Color"": null,
    ""DebugName"": ""D8"",
    ""DebugName2"": null,
    ""ScanCode"": 9
  },
  {
    ""X"": 10,
    ""Y"": 2,
    ""Key"": 0,
    ""Order"": 97,
    ""Color"": null,
    ""DebugName"": ""I"",
    ""DebugName2"": null,
    ""ScanCode"": 23
  },
  {
    ""X"": 10,
    ""Y"": 3,
    ""Key"": 0,
    ""Order"": 98,
    ""Color"": null,
    ""DebugName"": ""K"",
    ""DebugName2"": null,
    ""ScanCode"": 37
  },
  {
    ""X"": 10,
    ""Y"": 4,
    ""Key"": 0,
    ""Order"": 99,
    ""Color"": null,
    ""DebugName"": ""M"",
    ""DebugName2"": null,
    ""ScanCode"": 50
  },
  {
    ""X"": 13,
    ""Y"": 5,
    ""Key"": 0,
    ""Order"": 100,
    ""Color"": null,
    ""DebugName"": ""RWIN"",
    ""DebugName2"": null,
    ""ScanCode"": 92
  },
  {
    ""X"": 15,
    ""Y"": 2,
    ""Key"": 0,
    ""Order"": 101,
    ""Color"": null,
    ""DebugName"": ""OEMPIPE"",
    ""DebugName2"": null,
    ""ScanCode"": 43
  },
  {
    ""X"": 17,
    ""Y"": 4,
    ""Key"": 0,
    ""Order"": 102,
    ""Color"": null,
    ""DebugName"": ""UP"",
    ""DebugName2"": null,
    ""ScanCode"": 72
  },
  {
    ""X"": 20,
    ""Y"": 3,
    ""Key"": 0,
    ""Order"": 103,
    ""Color"": null,
    ""DebugName"": ""NUMPAD5"",
    ""DebugName2"": null,
    ""ScanCode"": 76
  },
  {
    ""X"": 21,
    ""Y"": 5,
    ""Key"": 0,
    ""Order"": 105,
    ""Color"": null,
    ""DebugName"": ""DECIMAL"",
    ""DebugName2"": null,
    ""ScanCode"": 83
  }
]";

            //xy = JsonConvert.DeserializeObject<List<XY>>(shitmap);
            //Debug.WriteLine("---------------------");
            //foreach (XY xy1 in xy)
            //{
            //    Debug.Write(xy1.X+","+xy1.Y+","+xy1.ScanCode+",");
            //}

            //Debug.WriteLine("");
            //Debug.WriteLine("---------------------");
            //string jshumm = JsonConvert.SerializeObject(xy);
            //Debug.WriteLine(jshumm);


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


            xy = humm.ToList();

            OrderOfKeys = order.ToArray();

            OrderOfKeys = xy.OrderBy(p => (p.X*6) + p.Y).Select(p => p.Key).ToArray();

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

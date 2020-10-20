using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SimpleLed.RawInput;
using SimpleLed;

namespace Driver.HyperXAlloy.RGB
{
    public class DeviceInfo
    {
        public int Vid { get; set; }
        public int Pid { get; set; }
        public int Usb { get; set; }
        public string Name { get; set; }
    }

    public class DriverHyperXAlloyRGB : ISimpleLed
    {
        public event EventHandler DeviceRescanRequired;

        private const int HYPERX_KEYBOARD_VID = 0x0951;
        const int HYPERX_ALLOY_ELITE_PID = 0x16BE;
        const int HYPERX_ALLOY_FPS_RGB_PID = 0x16DC;

        public DeviceInfo[] supportedDevices = new DeviceInfo[]
        {
            new DeviceInfo {Vid=HYPERX_KEYBOARD_VID, Pid=HYPERX_ALLOY_ELITE_PID,Usb= 2, Name="Elite RGB"},
            new DeviceInfo {Vid=HYPERX_KEYBOARD_VID, Pid=HYPERX_ALLOY_FPS_RGB_PID, Usb=2,Name= "FPS RGB"}
        };


        public void Dispose()
        {

        }

        public DriverHyperXAlloyRGB()
        {

        }

        public event Events.DeviceChangeEventHandler DeviceAdded;
        public event Events.DeviceChangeEventHandler DeviceRemoved;

        List<ControlDevice> foundDevices =new List<ControlDevice>();
        public void Configure(DriverDetails driverDetails)
        {
            SLSManager.GetSupportedDevices(supportedDevices.Select(x=>new USBDevice { HID = x.Pid, VID = x.Vid} ).ToList()).ForEach(x=>InterestedUSBChange(x.VID,x.HID.Value, true));
        }

        private HyperXAlloyRgbControlDevice dv;

        List<ControlDevice> devices = new List<ControlDevice>();
     



        private bool isWriting = false;
        public void Push(ControlDevice controlDevice)
        {
            if (isWriting)
            {
                return;

            }

            isWriting = true;

            Task.Run(() =>
            {
                if (controlDevice.In2DMode)
                {
                    var xxx = controlDevice.LEDs.OrderBy(p => p.Data.LEDNumber).ToList();
                    ((HyperXAlloyRgbControlDevice) controlDevice).HyperXSupport.SendColors(xxx.Select(x => x.Color)
                        .ToArray());
                }
                else
                {

                    ((HyperXAlloyRgbControlDevice) controlDevice).HyperXSupport.SendColors(controlDevice.LEDs
                        .Select(x => x.Color).ToArray());
                }

                isWriting = false;
            });

        }

        public void Pull(ControlDevice controlDevice)
        {
            //throw new NotImplementedException();
        }

        public DriverProperties GetProperties()
        {
            return new DriverProperties
            {
                SupportsPull = false,
                SupportsPush = true,
                IsSource = false,
                Id = Guid.Parse("a9440d02-bba3-4e35-a9a3-88b024cc0e2d"),
                Author = "mad ninja",
                Blurb = "Support for HyperX Alloy Elite RGB and HyperX Alloy FPS RGB",
                CurrentVersion = new ReleaseNumber(1, 0, 0, 7),
                GitHubLink = "https://github.com/SimpleLed/Driver.HyperXAlloy.RGB",
                IsPublicRelease = true,
                SupportsCustomConfig = false,
                SupportedDevices = new List<USBDevice>
                {
                    new USBDevice(){ VID=HYPERX_KEYBOARD_VID, HID = HYPERX_ALLOY_FPS_RGB_PID},
                    new USBDevice(){ VID=HYPERX_KEYBOARD_VID, HID = HYPERX_ALLOY_ELITE_PID}
                }
                
            };
        }

        public T GetConfig<T>() where T : SLSConfigData
        {
            return null;
        }

        public void PutConfig<T>(T config) where T : SLSConfigData
        {

        }

        public string Name()
        {
            return "HyperX Alloy RGB";
        }

        public void InterestedUSBChange(int VID, int PID, bool connected)
        {
            if (!connected)
            {
                var dev = devices.First(x => x is HyperXAlloyRgbControlDevice hx && hx.HID == PID);

                devices.Remove(dev);
                DeviceRemoved?.Invoke(this, new Events.DeviceChangeEventArgs(dev));
            }
            else
            {
                var sdevice = supportedDevices.First(x => x.Pid == PID);

                HyperXKeyboardSupport hyperX = new HyperXKeyboardSupport(sdevice.Vid, sdevice.Pid, sdevice.Usb);

                dv = new HyperXAlloyRgbControlDevice
                {
                    Name = sdevice.Name,
                    DeviceType = DeviceTypes.Keyboard,
                    Driver = this,
                    ProductImage = Assembly.GetExecutingAssembly().GetEmbeddedImage("Driver.HyperXAlloy.RGB." + sdevice.Name + ".png"),
                    HyperXSupport = hyperX,
                    GridHeight = 6,
                    GridWidth = 23,
                    Has2DSupport = true,
                    HID = sdevice.Pid
                };

                KeyboardHelper.AddKeyboardWatcher(sdevice.Vid, sdevice.Pid, dv.HandleInput);

                List<ControlDevice.LedUnit> leds = new List<ControlDevice.LedUnit>();
                int ctt = 0;
                var tled = new ControlDevice.LedUnit[106];
                int ct = 0;

                foreach (var tp in hyperX.humm)
                {
                    var ld = new ControlDevice.LedUnit
                    {
                        LEDName = HyperXKeyboardSupport.KeyNames[tp.Order],
                        Data = new ControlDevice.PositionalLEDData
                        {
                            LEDNumber = Array.IndexOf(hyperX.humm, tp),
                            X = tp.X,
                            Y = tp.Y
                        },

                    };

                    leds.Add(ld);

                }

                dv.LEDs = leds.OrderBy(p => ((ControlDevice.PositionalLEDData)p.Data).X + ((ControlDevice.PositionalLEDData)p.Data).Y).ToArray();

                devices.Add(dv);
                DeviceAdded?.Invoke(this, new Events.DeviceChangeEventArgs(dv));
            }
        }
    }

    public class HyperXAlloyRgbControlDevice : InteractiveControlDevice
    {
        public int HID { get; set; }
        public HyperXKeyboardSupport HyperXSupport { get; set; }
     
    }
}

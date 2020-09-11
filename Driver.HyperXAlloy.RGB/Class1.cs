using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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

        public void Configure(DriverDetails driverDetails)
        {

        }

        public List<ControlDevice> GetDevices()
        {
            List<ControlDevice> devices = new List<ControlDevice>();

            foreach (var sdevice in supportedDevices)
            {
                try
                {
                    HyperXKeyboardSupport hyperX = new HyperXKeyboardSupport(sdevice.Vid, sdevice.Pid, sdevice.Usb);

                    HyperXAlloyRgbControlDevice dv =new HyperXAlloyRgbControlDevice
                    {
                        Name = sdevice.Name,
                        DeviceType = DeviceTypes.Keyboard,
                        Driver = this,
                        ProductImage = Assembly.GetExecutingAssembly().GetEmbeddedImage("Driver.HyperXAlloy.RGB."+sdevice.Name+".png"),
                        HyperXSupport = hyperX
                    };

                    List<ControlDevice.LedUnit> leds=new List<ControlDevice.LedUnit>();
                    int ct = 0;
                    foreach (var ky in hyperX.OrderOfKeys)
                    {
                        leds.Add(new ControlDevice.LedUnit
                        {
                            LEDName = HyperXKeyboardSupport.KeyNames[ky],
                            Data = new ControlDevice.LEDData{LEDNumber = ct}
                        });

                        ct++;
                    }

                    dv.LEDs = leds.ToArray();

                    devices.Add(dv);
                }
                catch
                {

                }

            }

            return devices;
        }

        public void Push(ControlDevice controlDevice)
        {
            ((HyperXAlloyRgbControlDevice)controlDevice).HyperXSupport.SendColors(controlDevice.LEDs.Select(x=>x.Color).ToArray());
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
                CurrentVersion = new ReleaseNumber(1, 0, 0, 0),
                GitHubLink = "https://github.com/SimpleLed/Driver.HyperXAlloy.RGB",
                IsPublicRelease = true,
                SupportsCustomConfig = false
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
    }

    public class HyperXAlloyRgbControlDevice : ControlDevice
    {
        public HyperXKeyboardSupport HyperXSupport { get; set; }
    }
}

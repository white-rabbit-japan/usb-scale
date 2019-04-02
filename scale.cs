//Scale.cs
//Original code source: http://stackoverflow.com/questions/11961412/read-weight-from-a-fairbanks-scb-9000-usb-scale
using HidLibrary;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Scale
{
    class USBScale
    {
        public bool IsConnected
        {
            get
            {
                return scale == null ? false : scale.IsConnected;
            }
        }

        public decimal ScaleStatus
        {
            get
            {
                return inData.Data[1];
            }
        }

        public decimal ScaleWeightUnits
        {
            get
            {
                return inData.Data[2];
            }
        }

        private HidDevice scale;
        private HidDeviceData inData;

        public HidDevice[] GetDevices()
        {

            HidDevice[] hidDevice;


            // product/vendor ID number Dymo 25lb Postal Scale
            hidDevice = HidDevices.Enumerate(0x0922, 0x8003).ToArray();
            Console.WriteLine("hidDevice len {0}", hidDevice.Length);

            if (hidDevice.Length > 0)
                return hidDevice;

            // product/vendor ID number for Elane USB 30
            hidDevice = HidDevices.Enumerate(0x7B7C, 0x0204).ToArray();
            Console.WriteLine("hidDevice len {0}", hidDevice.Length);

            if (hidDevice.Length > 0)
                return hidDevice;

            return null;

        }

        public bool Connect()
        {
            // Find a Scale
            HidDevice[] deviceList = GetDevices();

            if (deviceList.Length > 0)

                return Connect(deviceList[0]);

            else

                return false;
        }

        public bool Connect(HidDevice device)
        {
            scale = device;
            int waitTries = 0;
            scale.OpenDevice();

            // sometimes the scale is not ready immedietly after
            // Open() so wait till its ready
            while (!scale.IsConnected && waitTries < 10)
            {
                Thread.Sleep(50);
                waitTries++;
            }
            return scale.IsConnected;
        }

        public void Disconnect()
        {
            if (scale.IsConnected)
            {
                scale.CloseDevice();
                scale.Dispose();
            }
        }

        public void DebugScaleData()
        {
            for (int i = 0; i < inData.Data.Length; ++i)
            {
                Console.WriteLine("Byte {0}: {1}", i, inData.Data[i]);
            }
        }

        public void GetWeight(out decimal? weightInLb, out decimal? weightInG, out decimal? weightInOz, out bool? isStable)
        {
            decimal? weight;
            weight = null;
            weightInG = null;
            weightInLb = null;
            weightInOz = null;
            isStable = false;

            if (scale.IsConnected)
            {
                inData = scale.Read(250);
                // Byte 0 == Report ID?
                // Byte 1 == Scale Status (1 == Fault, 2 == Stable @ 0, 3 == In Motion, 4 == Stable, 5 == Under 0, 6 == Over Weight, 7 == Requires Calibration, 8 == Requires Re-Zeroing)
                // Byte 2 == Weight Unit
                // Byte 3 == Data Scaling (decimal placement)
                // Byte 4 == Weight LSB
                // Byte 5 == Weight MSB

                // FIXME: dividing by 100 probably wont work with
                // every scale, need to figure out what to do with
                // Byte 3
                //weight = (Convert.ToDecimal(inData.Data[4]) + Convert.ToDecimal(inData.Data[5]) * 256) / 10;

                Console.WriteLine("scale status {0}", inData.Data[1]);


                switch (Convert.ToInt16(inData.Data[2]))
                {
                    case 2:  // Scale reading in g
                        weight = (Convert.ToDecimal(inData.Data[4]) + Convert.ToDecimal(inData.Data[5]) * 256);
                        weightInG = weight;
                        weightInLb = weight * (decimal?)0.00220462;
                        weightInOz = weight * (decimal?)0.035274;
                        //weightInG = weightInG * (decimal?)0.625;
                        break;
                    case 11: // Ounces
                        weight = (Convert.ToDecimal(inData.Data[4]) + Convert.ToDecimal(inData.Data[5]) * 256) / 10;
                        weightInLb = weight * (decimal?)0.0625;
                        weightInOz = weight;
                        weightInG = weight * (decimal?)28.3495;
                        break;
                    case 12: // Pounds
                        // already in pounds, do nothing
                        break;
                }
                isStable = inData.Data[1] == 0x4;
            }
        }
    }
}
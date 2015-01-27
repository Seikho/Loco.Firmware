using System;
using System.Collections;
using System.Threading;
using GHIElectronics.NETMF.FEZ;
using Microsoft.SPOT;

namespace Loco.Firmware
{
    public class Program
    {
        private const int SampleFrequency = 500; // Sample size in miliseconds or length of 1 tick.
        private const int BufferLimit = 20; // The number of messages to store in the buffer before printing and emptying. This will occur at a rate of 5 messages per tick.
        private static readonly Enclosure[] Enclosures = new Enclosure[5];
        private static readonly ArrayList Buffer = new ArrayList();
        private static Timer MessageTimer { get; set; }

        public static void Main()
        {
            for (var enclosure = 1; enclosure <= 5; enclosure++)
            {
                Enclosures[enclosure] = new Enclosure(enclosure, GetEnclosurePins(enclosure));
            }
            Debug.Print("Starting application...\n");
            MessageTimer = new Timer(ReadEnclosures, null, SampleFrequency, Timeout.Infinite);
        }

        /// <summary>
        /// 
        /// This method will run every 500 milliseconds (SampleFrequency).
        /// Software on the receiving computer will decide what to record
        /// </summary>
        public static void ReadEnclosures(object state)
        {
            foreach (var enclosure in Enclosures)
            {
                Buffer.Add("Enclosure0" + enclosure.Id + " " + enclosure.PrintLocation());
                if (Buffer.Count == BufferLimit) RecycleBuffer();
            }
            MessageTimer.Change(SampleFrequency, Timeout.Infinite);
        }

        /// <summary>
        /// Print the contents of the buffer and empty it
        /// </summary>
        private static void RecycleBuffer()
        {
            foreach (string line in Buffer)
            {
                Debug.Print(line);
            }
            Buffer.Clear();
        }

        /// <summary>
        /// This configuration determines what infrared sensors comprise each enclosure.
        /// The order of FEZ_Pins will depend heavily on the physical configuration of the sensors.
        /// </summary>
        /// <param name="encNumber"></param>
        /// <returns></returns>
        private static FEZ_Pin.Digital[] GetEnclosurePins(int encNumber)
        {
            switch (encNumber)
            {
                case 1:
                    return new[]
                    {
                        FEZ_Pin.Digital.An0,
                        FEZ_Pin.Digital.An1,
                        FEZ_Pin.Digital.An2,
                        FEZ_Pin.Digital.An3,
                        FEZ_Pin.Digital.An4,
                        FEZ_Pin.Digital.An5,
                        FEZ_Pin.Digital.Di51,
                        FEZ_Pin.Digital.Di52
                    };
                case 2:
                    return new[]
                    {
                        FEZ_Pin.Digital.Di20,
                        FEZ_Pin.Digital.Di21,
                        FEZ_Pin.Digital.Di22,
                        FEZ_Pin.Digital.Di23,
                        FEZ_Pin.Digital.Di24,
                        FEZ_Pin.Digital.Di25,
                        FEZ_Pin.Digital.Di26,
                        FEZ_Pin.Digital.Di27
                    };
                case 3:
                    return new[]
                    {
                        FEZ_Pin.Digital.Di28,
                        FEZ_Pin.Digital.Di29,
                        FEZ_Pin.Digital.Di30,
                        FEZ_Pin.Digital.Di31,
                        FEZ_Pin.Digital.Di32,
                        FEZ_Pin.Digital.Di33,
                        FEZ_Pin.Digital.Di34,
                        FEZ_Pin.Digital.Di35
                    };
                case 4:
                    return new[]
                    {
                        FEZ_Pin.Digital.Di36,
                        FEZ_Pin.Digital.Di37,
                        FEZ_Pin.Digital.Di38,
                        FEZ_Pin.Digital.Di39,
                        FEZ_Pin.Digital.Di40,
                        FEZ_Pin.Digital.Di41,
                        FEZ_Pin.Digital.Di42,
                        FEZ_Pin.Digital.Di43
                    };
                case 5:
                    return new[]
                    {
                        FEZ_Pin.Digital.Di6,
                        FEZ_Pin.Digital.Di7,
                        FEZ_Pin.Digital.Di8,
                        FEZ_Pin.Digital.Di9,
                        FEZ_Pin.Digital.Di10,
                        FEZ_Pin.Digital.Di11,
                        FEZ_Pin.Digital.Di12,
                        FEZ_Pin.Digital.Di13,
                    };
                default:
                    throw new Exception("Invalid enclosure number provided to EnclosurePinProvider (1 - 5)");
            }
        }
    }
}

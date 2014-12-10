using System;
using GHIElectronics.NETMF.FEZ;
using Microsoft.SPOT.Hardware;

namespace Loco.Firmware
{
    class Enclosure
    {
        readonly InputPort[] _inputs = new InputPort[8];
        public int Id { get; private set; }

        public Enclosure(int id, FEZ_Pin.Digital[] pins)
        {
            Id = id;
           if (pins.Length != 8) throw new Exception("Insuffient pins provided. Must be 8 pins");
            for (var pin = 0; pin < pins.Length; pin++)
            {
                _inputs[pin] = new InputPort((Cpu.Pin)pins[pin], false, Port.ResistorMode.Disabled);
            }
        }

        public string PrintLocation()
        {
            var output = "";
            for (var count = 0; count < _inputs.Length; count++)
            {
                output = output + (_inputs[count].Read() ? '1' : '0');
            }
            return output;
        }
    }
}

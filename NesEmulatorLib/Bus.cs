using System;
using System.Collections.Generic;
using System.Text;

namespace NesEmulator
{
    public class Bus
    {
        public byte[] ram = new byte[64 * 1024];

        public olc6502 cpu = new olc6502();

        public Bus()
        {
            for(int x = 0;x < ram.Length; x++) {
                ram[x] = 0;
            }
            cpu.ConnectBus(this);
        }

        public void write(UInt16 addr, byte data)
        {
            if (addr >= 0x0000 && addr <= 0xFFFF)
                ram[addr] = data;
        }

        public byte read(UInt16 addr, bool bReadOnly = false)
        {
            if (addr >= 0x0000 && addr <= 0xFFFF)
                return ram[addr];
            return 0;
        }
    }
}

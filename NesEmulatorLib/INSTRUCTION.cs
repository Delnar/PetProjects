using System;
using System.Collections.Generic;
using System.Text;

namespace NesEmulator
{
    public struct INSTRUCTION
    {
        public INSTRUCTION(string name, Func<byte> operate, Func<byte> addrMode, byte cycles)
        {
            Name = name;
            this.operate = operate;
            this.addrMode = addrMode;
            this.cycles = cycles;
        }

        public string Name { get; set; }
        public Func<byte> operate { get; set; }
        public Func<byte> addrMode { get; set; }
        public byte cycles { get; set; }
    }
}

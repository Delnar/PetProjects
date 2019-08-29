using System;
using System.Collections.Generic;
using System.Text;

namespace NesEmulator
{
    public class Registers
    {
        #region Enumerations
        public enum FLAGS6502
        {
            /// <summary>
            /// Carry Bit
            /// </summary>
            C = (1 << 0), 
            /// <summary>
            /// Zero
            /// </summary>
            Z = (1 << 1), 
            /// <summary>
            /// Disable Interrupts
            /// </summary>
            I = (1 << 2), 
            /// <summary>
            /// Decimal Mode (unused in this implementation)
            /// </summary>
            D = (1 << 3), 
            /// <summary>
            /// Break
            /// </summary>
            B = (1 << 4), // Break
            /// <summary>
            /// Unused
            /// </summary>
            U = (1 << 5), 
            /// <summary>
            /// Overflow
            /// </summary>
            V = (1 << 6),
            /// <summary>
            /// Negative
            /// </summary>
            N = (1 << 7)  // Negative
        }
        #endregion

        public byte a { get; set; } = 0x00;             // Accumulator
        public byte x { get; set; } = 0x00;             // X
        public byte y { get; set; } = 0x00;             // Y
        public byte stkp { get; set; } = 0x00;          // Stack Pointer (points to location on bus)
        public UInt16 pc { get; set; } = 0x0000;        // Program Counter
        public byte status { get; set; } = 0x00;        // Status Register


        public byte GetFlagByte(FLAGS6502 f)
        {
            return GetFlag(f) ? (byte)1 : (byte)0;
        }
        public bool GetFlag(FLAGS6502 f)
        {
            return (status & (byte)f) > 0;
        }

        public void SetFlag(FLAGS6502 f, bool v)
        {
            if (v) {
                status |= (byte)f;
            } else {
                status &= (byte)(~f);
            }
        }
        public void SetFlag(FLAGS6502 f, int v)
        {
            if (v > 0) {
                status |= (byte)f;
            } else {
                status &= (byte)(~f);
            }
        }

    }
}

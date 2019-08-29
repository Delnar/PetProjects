using System;
using System.Collections.Generic;
using System.Text;

namespace NesEmulator
{
    public class olc6502
    {
        #region Constants
        public const UInt16 HighByteMask = 0xFF00;
        public const UInt16 LowByteMask = 0x00FF;
        #endregion
        #region Public Properties
        public Bus bus { get; set; } = null;
        public Registers reg { get; set; } = new Registers();
        public byte fetched { get; set; } = 0x00;
        public UInt16 addr_abs { get; set; } = 0x0000;
        public UInt16 addr_rel { get; set; } = 0x00;
        public byte opcode { get; set; } = 0x00;
        public byte cycles { get; set; } = 0x00;

        public List<INSTRUCTION> lookup { get; set; } = null;
        #endregion

        public olc6502()
        {
            lookup = new List<INSTRUCTION>() {
                new INSTRUCTION("BRK", BRK, IMM, 7),new INSTRUCTION( "ORA", ORA, IZX, 6 ),new INSTRUCTION("???", XXX, IMP, 2),new INSTRUCTION("???", XXX, IMP, 8),new INSTRUCTION("???", NOP, IMP, 3),new INSTRUCTION("ORA", ORA, ZP0, 3),new INSTRUCTION("ASL", ASL, ZP0, 5),new INSTRUCTION("???", XXX, IMP, 5),new INSTRUCTION("PHP", PHP, IMP, 3),new INSTRUCTION("ORA", ORA, IMM, 2),new INSTRUCTION("ASL", ASL, IMP, 2),new INSTRUCTION("???", XXX, IMP, 2),new INSTRUCTION("???", NOP, IMP, 4),new INSTRUCTION("ORA", ORA, ABS, 4),new INSTRUCTION("ASL", ASL, ABS, 6),new INSTRUCTION("???", XXX, IMP, 6),
                new INSTRUCTION("BPL", BPL, REL, 2),new INSTRUCTION( "ORA", ORA, IZY, 5 ),new INSTRUCTION("???", XXX, IMP, 2),new INSTRUCTION("???", XXX, IMP, 8),new INSTRUCTION("???", NOP, IMP, 4),new INSTRUCTION("ORA", ORA, ZPX, 4),new INSTRUCTION("ASL", ASL, ZPX, 6),new INSTRUCTION("???", XXX, IMP, 6),new INSTRUCTION("CLC", CLC, IMP, 2),new INSTRUCTION("ORA", ORA, ABY, 4),new INSTRUCTION("???", NOP, IMP, 2),new INSTRUCTION("???", XXX, IMP, 7),new INSTRUCTION("???", NOP, IMP, 4),new INSTRUCTION("ORA", ORA, ABX, 4),new INSTRUCTION("ASL", ASL, ABX, 7),new INSTRUCTION("???", XXX, IMP, 7),
                new INSTRUCTION("JSR", JSR, ABS, 6),new INSTRUCTION( "AND", AND, IZX, 6 ),new INSTRUCTION("???", XXX, IMP, 2),new INSTRUCTION("???", XXX, IMP, 8),new INSTRUCTION("BIT", BIT, ZP0, 3),new INSTRUCTION("AND", AND, ZP0, 3),new INSTRUCTION("ROL", ROL, ZP0, 5),new INSTRUCTION("???", XXX, IMP, 5),new INSTRUCTION("PLP", PLP, IMP, 4),new INSTRUCTION("AND", AND, IMM, 2),new INSTRUCTION("ROL", ROL, IMP, 2),new INSTRUCTION("???", XXX, IMP, 2),new INSTRUCTION("BIT", BIT, ABS, 4),new INSTRUCTION("AND", AND, ABS, 4),new INSTRUCTION("ROL", ROL, ABS, 6),new INSTRUCTION("???", XXX, IMP, 6),
                new INSTRUCTION("BMI", BMI, REL, 2),new INSTRUCTION( "AND", AND, IZY, 5 ),new INSTRUCTION("???", XXX, IMP, 2),new INSTRUCTION("???", XXX, IMP, 8),new INSTRUCTION("???", NOP, IMP, 4),new INSTRUCTION("AND", AND, ZPX, 4),new INSTRUCTION("ROL", ROL, ZPX, 6),new INSTRUCTION("???", XXX, IMP, 6),new INSTRUCTION("SEC", SEC, IMP, 2),new INSTRUCTION("AND", AND, ABY, 4),new INSTRUCTION("???", NOP, IMP, 2),new INSTRUCTION("???", XXX, IMP, 7),new INSTRUCTION("???", NOP, IMP, 4),new INSTRUCTION("AND", AND, ABX, 4),new INSTRUCTION("ROL", ROL, ABX, 7),new INSTRUCTION("???", XXX, IMP, 7),
                new INSTRUCTION("RTI", RTI, IMP, 6),new INSTRUCTION( "EOR", EOR, IZX, 6 ),new INSTRUCTION("???", XXX, IMP, 2),new INSTRUCTION("???", XXX, IMP, 8),new INSTRUCTION("???", NOP, IMP, 3),new INSTRUCTION("EOR", EOR, ZP0, 3),new INSTRUCTION("LSR", LSR, ZP0, 5),new INSTRUCTION("???", XXX, IMP, 5),new INSTRUCTION("PHA", PHA, IMP, 3),new INSTRUCTION("EOR", EOR, IMM, 2),new INSTRUCTION("LSR", LSR, IMP, 2),new INSTRUCTION("???", XXX, IMP, 2),new INSTRUCTION("JMP", JMP, ABS, 3),new INSTRUCTION("EOR", EOR, ABS, 4),new INSTRUCTION("LSR", LSR, ABS, 6),new INSTRUCTION("???", XXX, IMP, 6),
                new INSTRUCTION("BVC", BVC, REL, 2),new INSTRUCTION( "EOR", EOR, IZY, 5 ),new INSTRUCTION("???", XXX, IMP, 2),new INSTRUCTION("???", XXX, IMP, 8),new INSTRUCTION("???", NOP, IMP, 4),new INSTRUCTION("EOR", EOR, ZPX, 4),new INSTRUCTION("LSR", LSR, ZPX, 6),new INSTRUCTION("???", XXX, IMP, 6),new INSTRUCTION("CLI", CLI, IMP, 2),new INSTRUCTION("EOR", EOR, ABY, 4),new INSTRUCTION("???", NOP, IMP, 2),new INSTRUCTION("???", XXX, IMP, 7),new INSTRUCTION("???", NOP, IMP, 4),new INSTRUCTION("EOR", EOR, ABX, 4),new INSTRUCTION("LSR", LSR, ABX, 7),new INSTRUCTION("???", XXX, IMP, 7),
                new INSTRUCTION("RTS", RTS, IMP, 6),new INSTRUCTION( "ADC", ADC, IZX, 6 ),new INSTRUCTION("???", XXX, IMP, 2),new INSTRUCTION("???", XXX, IMP, 8),new INSTRUCTION("???", NOP, IMP, 3),new INSTRUCTION("ADC", ADC, ZP0, 3),new INSTRUCTION("ROR", ROR, ZP0, 5),new INSTRUCTION("???", XXX, IMP, 5),new INSTRUCTION("PLA", PLA, IMP, 4),new INSTRUCTION("ADC", ADC, IMM, 2),new INSTRUCTION("ROR", ROR, IMP, 2),new INSTRUCTION("???", XXX, IMP, 2),new INSTRUCTION("JMP", JMP, IND, 5),new INSTRUCTION("ADC", ADC, ABS, 4),new INSTRUCTION("ROR", ROR, ABS, 6),new INSTRUCTION("???", XXX, IMP, 6),
                new INSTRUCTION("BVS", BVS, REL, 2),new INSTRUCTION( "ADC", ADC, IZY, 5 ),new INSTRUCTION("???", XXX, IMP, 2),new INSTRUCTION("???", XXX, IMP, 8),new INSTRUCTION("???", NOP, IMP, 4),new INSTRUCTION("ADC", ADC, ZPX, 4),new INSTRUCTION("ROR", ROR, ZPX, 6),new INSTRUCTION("???", XXX, IMP, 6),new INSTRUCTION("SEI", SEI, IMP, 2),new INSTRUCTION("ADC", ADC, ABY, 4),new INSTRUCTION("???", NOP, IMP, 2),new INSTRUCTION("???", XXX, IMP, 7),new INSTRUCTION("???", NOP, IMP, 4),new INSTRUCTION("ADC", ADC, ABX, 4),new INSTRUCTION("ROR", ROR, ABX, 7),new INSTRUCTION("???", XXX, IMP, 7),
                new INSTRUCTION("???", NOP, IMP, 2),new INSTRUCTION( "STA", STA, IZX, 6 ),new INSTRUCTION("???", NOP, IMP, 2),new INSTRUCTION("???", XXX, IMP, 6),new INSTRUCTION("STY", STY, ZP0, 3),new INSTRUCTION("STA", STA, ZP0, 3),new INSTRUCTION("STX", STX, ZP0, 3),new INSTRUCTION("???", XXX, IMP, 3),new INSTRUCTION("DEY", DEY, IMP, 2),new INSTRUCTION("???", NOP, IMP, 2),new INSTRUCTION("TXA", TXA, IMP, 2),new INSTRUCTION("???", XXX, IMP, 2),new INSTRUCTION("STY", STY, ABS, 4),new INSTRUCTION("STA", STA, ABS, 4),new INSTRUCTION("STX", STX, ABS, 4),new INSTRUCTION("???", XXX, IMP, 4),
                new INSTRUCTION("BCC", BCC, REL, 2),new INSTRUCTION( "STA", STA, IZY, 6 ),new INSTRUCTION("???", XXX, IMP, 2),new INSTRUCTION("???", XXX, IMP, 6),new INSTRUCTION("STY", STY, ZPX, 4),new INSTRUCTION("STA", STA, ZPX, 4),new INSTRUCTION("STX", STX, ZPY, 4),new INSTRUCTION("???", XXX, IMP, 4),new INSTRUCTION("TYA", TYA, IMP, 2),new INSTRUCTION("STA", STA, ABY, 5),new INSTRUCTION("TXS", TXS, IMP, 2),new INSTRUCTION("???", XXX, IMP, 5),new INSTRUCTION("???", NOP, IMP, 5),new INSTRUCTION("STA", STA, ABX, 5),new INSTRUCTION("???", XXX, IMP, 5),new INSTRUCTION("???", XXX, IMP, 5),
                new INSTRUCTION("LDY", LDY, IMM, 2),new INSTRUCTION( "LDA", LDA, IZX, 6 ),new INSTRUCTION("LDX", LDX, IMM, 2),new INSTRUCTION("???", XXX, IMP, 6),new INSTRUCTION("LDY", LDY, ZP0, 3),new INSTRUCTION("LDA", LDA, ZP0, 3),new INSTRUCTION("LDX", LDX, ZP0, 3),new INSTRUCTION("???", XXX, IMP, 3),new INSTRUCTION("TAY", TAY, IMP, 2),new INSTRUCTION("LDA", LDA, IMM, 2),new INSTRUCTION("TAX", TAX, IMP, 2),new INSTRUCTION("???", XXX, IMP, 2),new INSTRUCTION("LDY", LDY, ABS, 4),new INSTRUCTION("LDA", LDA, ABS, 4),new INSTRUCTION("LDX", LDX, ABS, 4),new INSTRUCTION("???", XXX, IMP, 4),
                new INSTRUCTION("BCS", BCS, REL, 2),new INSTRUCTION( "LDA", LDA, IZY, 5 ),new INSTRUCTION("???", XXX, IMP, 2),new INSTRUCTION("???", XXX, IMP, 5),new INSTRUCTION("LDY", LDY, ZPX, 4),new INSTRUCTION("LDA", LDA, ZPX, 4),new INSTRUCTION("LDX", LDX, ZPY, 4),new INSTRUCTION("???", XXX, IMP, 4),new INSTRUCTION("CLV", CLV, IMP, 2),new INSTRUCTION("LDA", LDA, ABY, 4),new INSTRUCTION("TSX", TSX, IMP, 2),new INSTRUCTION("???", XXX, IMP, 4),new INSTRUCTION("LDY", LDY, ABX, 4),new INSTRUCTION("LDA", LDA, ABX, 4),new INSTRUCTION("LDX", LDX, ABY, 4),new INSTRUCTION("???", XXX, IMP, 4),
                new INSTRUCTION("CPY", CPY, IMM, 2),new INSTRUCTION( "CMP", CMP, IZX, 6 ),new INSTRUCTION("???", NOP, IMP, 2),new INSTRUCTION("???", XXX, IMP, 8),new INSTRUCTION("CPY", CPY, ZP0, 3),new INSTRUCTION("CMP", CMP, ZP0, 3),new INSTRUCTION("DEC", DEC, ZP0, 5),new INSTRUCTION("???", XXX, IMP, 5),new INSTRUCTION("INY", INY, IMP, 2),new INSTRUCTION("CMP", CMP, IMM, 2),new INSTRUCTION("DEX", DEX, IMP, 2),new INSTRUCTION("???", XXX, IMP, 2),new INSTRUCTION("CPY", CPY, ABS, 4),new INSTRUCTION("CMP", CMP, ABS, 4),new INSTRUCTION("DEC", DEC, ABS, 6),new INSTRUCTION("???", XXX, IMP, 6),
                new INSTRUCTION("BNE", BNE, REL, 2),new INSTRUCTION( "CMP", CMP, IZY, 5 ),new INSTRUCTION("???", XXX, IMP, 2),new INSTRUCTION("???", XXX, IMP, 8),new INSTRUCTION("???", NOP, IMP, 4),new INSTRUCTION("CMP", CMP, ZPX, 4),new INSTRUCTION("DEC", DEC, ZPX, 6),new INSTRUCTION("???", XXX, IMP, 6),new INSTRUCTION("CLD", CLD, IMP, 2),new INSTRUCTION("CMP", CMP, ABY, 4),new INSTRUCTION("NOP", NOP, IMP, 2),new INSTRUCTION("???", XXX, IMP, 7),new INSTRUCTION("???", NOP, IMP, 4),new INSTRUCTION("CMP", CMP, ABX, 4),new INSTRUCTION("DEC", DEC, ABX, 7),new INSTRUCTION("???", XXX, IMP, 7),
                new INSTRUCTION("CPX", CPX, IMM, 2),new INSTRUCTION( "SBC", SBC, IZX, 6 ),new INSTRUCTION("???", NOP, IMP, 2),new INSTRUCTION("???", XXX, IMP, 8),new INSTRUCTION("CPX", CPX, ZP0, 3),new INSTRUCTION("SBC", SBC, ZP0, 3),new INSTRUCTION("INC", INC, ZP0, 5),new INSTRUCTION("???", XXX, IMP, 5),new INSTRUCTION("INX", INX, IMP, 2),new INSTRUCTION("SBC", SBC, IMM, 2),new INSTRUCTION("NOP", NOP, IMP, 2),new INSTRUCTION("???", SBC, IMP, 2),new INSTRUCTION("CPX", CPX, ABS, 4),new INSTRUCTION("SBC", SBC, ABS, 4),new INSTRUCTION("INC", INC, ABS, 6),new INSTRUCTION("???", XXX, IMP, 6),
                new INSTRUCTION("BEQ", BEQ, REL, 2),new INSTRUCTION( "SBC", SBC, IZY, 5 ),new INSTRUCTION("???", XXX, IMP, 2),new INSTRUCTION("???", XXX, IMP, 8),new INSTRUCTION("???", NOP, IMP, 4),new INSTRUCTION("SBC", SBC, ZPX, 4),new INSTRUCTION("INC", INC, ZPX, 6),new INSTRUCTION("???", XXX, IMP, 6),new INSTRUCTION("SED", SED, IMP, 2),new INSTRUCTION("SBC", SBC, ABY, 4),new INSTRUCTION("NOP", NOP, IMP, 2),new INSTRUCTION("???", XXX, IMP, 7),new INSTRUCTION("???", NOP, IMP, 4),new INSTRUCTION("SBC", SBC, ABX, 4),new INSTRUCTION("INC", INC, ABX, 7),new INSTRUCTION("???", XXX, IMP, 7)
            };
        }

        public void ConnectBus(Bus n) { bus = n; }

        public byte fetch()
        {
            if (!(lookup[opcode].addrMode == IMP)) {
                fetched = read(addr_abs);
            }

            return fetched;
        }
        public void write(UInt16 addr, byte data)
        {
            bus.write(addr, data);
        }
        public byte read(UInt16 addr, bool bReadOnly = false)
        {
            return bus.read(addr);
        }
        public byte read(Int32 addr, bool bReadOnly = false)
        {
            return bus.read((UInt16)addr);
        }
        #region Addresing Modes
        public byte IMP() {
            fetched = reg.a;
            return 0;
        }
        public byte IMM() {
            addr_abs = reg.pc++;
            return 0;
        }
        public byte ZP0() {
            addr_abs = read(reg.pc);
            reg.pc++;
            addr_abs &= 0x00FF;
            return 0;
        }
        public byte ZPX() {

            addr_abs = read(reg.pc);
            addr_abs += reg.x;
            reg.pc++;
            addr_abs &= 0x00FF;
            return 0;
        }
        public byte ZPY() {
            addr_abs = read(reg.pc);
            addr_abs += reg.x;
            reg.pc++;
            addr_abs &= 0x00FF;
            return 0;
        }
        public byte REL() {
            addr_rel = read(reg.pc);
            reg.pc++;
            if ((addr_rel & 0x80) != 0) {
                addr_rel |= 0xFF00;
            }
            return 0;
        }
        public byte ABS() {
            UInt16 lo = read(reg.pc);
            reg.pc++;
            UInt16 hi = read(reg.pc);
            reg.pc++;

            addr_abs = (UInt16)(((UInt16)(hi << 8)) | lo);
            return 0;
        }
        public byte ABX() {

            UInt16 lo = (UInt16)read(reg.pc);
            reg.pc++;
            UInt16 hi = (UInt16)read(reg.pc);
            reg.pc++;

            addr_abs = (UInt16)(((UInt16)(hi << 8)) | lo);

            addr_abs += reg.x;
            if ((addr_abs & 0xFF00) != (hi << 8)) {
                return 1;
            }
            return 0;
        }
        public byte ABY() {
            UInt16 lo = read(reg.pc);
            reg.pc++;
            UInt16 hi = read(reg.pc);
            reg.pc++;

            addr_abs = (UInt16)(((UInt16)(hi << 8)) | lo);

            addr_abs += reg.y;
            if ((addr_abs & 0xFF00) != (hi << 8)) {
                return 1;
            }
            return 0;
        }
        public byte IND() {
            UInt16 lo_addr = read(reg.pc);
            reg.pc++;
            UInt16 hi_addr = read(reg.pc);
            reg.pc++;
            UInt16 ptr = (UInt16)(hi_addr << 8 | lo_addr); ;

            if (lo_addr == 0x00FF) {  // Simulate page boundary hardware bug
                addr_abs = (UInt16)((read(ptr & 0xFF00) << 8) | read(ptr + 0));
            } else {
                addr_abs = (UInt16)((read(ptr + 1) << 8) | read(ptr + 0));
            }
            return 0;
        }
        public byte IZX() {
            UInt16 t = read(reg.pc);
            reg.pc++;
            UInt16 lo_addr = (UInt16)(read(t + reg.x) & LowByteMask);
            UInt16 hi_addr = (UInt16)(read(t + reg.x + 1) & LowByteMask);
            addr_abs = (UInt16)(hi_addr << 8 | lo_addr);
            return 0;
        }
        public byte IZY() {

            UInt16 t = read(reg.pc);
            reg.pc++;

            UInt16 lo_addr = (UInt16)(read(t) & LowByteMask);
            UInt16 hi_addr = (UInt16)(read(t + 1) & LowByteMask);

            addr_abs = (UInt16)(hi_addr << 8 | lo_addr);
            addr_abs += reg.y;

            if ((addr_abs & HighByteMask) != hi_addr << 8) {
                return 1;
            } else {
                return 0;
            }
        }
        #endregion
        #region Opcodes


        ///////////////////////////////////////////////////////////////////////////////
        // INSTRUCTION IMPLEMENTATIONS

        // Note: Ive started with the two most complicated instructions to emulate, which
        // ironically is addition and subtraction! Ive tried to include a detailed 
        // explanation as to why they are so complex, yet so fundamental. Im also NOT
        // going to do this through the explanation of 1 and 2's complement.

        // Instruction: Add with Carry In
        // Function:    A = A + M + C
        // Flags Out:   C, V, N, Z
        //
        // Explanation:
        // The purpose of this function is to add a value to the accumulator and a carry bit. If
        // the result is > 255 there is an overflow setting the carry bit. Ths allows you to
        // chain together ADC instructions to add numbers larger than 8-bits. This in itself is
        // simple, however the 6502 supports the concepts of Negativity/Positivity and Signed Overflow.
        //
        // 10000100 = 128 + 4 = 132 in normal circumstances, we know this as unsigned and it allows
        // us to represent numbers between 0 and 255 (given 8 bits). The 6502 can also interpret 
        // this word as something else if we assume those 8 bits represent the range -128 to +127,
        // i.e. it has become signed.
        //
        // Since 132 > 127, it effectively wraps around, through -128, to -124. This wraparound is
        // called overflow, and this is a useful to know as it indicates that the calculation has
        // gone outside the permissable range, and therefore no longer makes numeric sense.
        //
        // Note the implementation of ADD is the same in binary, this is just about how the numbers
        // are represented, so the word 10000100 can be both -124 and 132 depending upon the 
        // context the programming is using it in. We can prove this!
        //
        //  10000100 =  132  or  -124
        // +00010001 = + 17      + 17
        //  ========    ===       ===     See, both are valid additions, but our interpretation of
        //  10010101 =  149  or  -107     the context changes the value, not the hardware!
        //
        // In principle under the -128 to 127 range:
        // 10000000 = -128, 11111111 = -1, 00000000 = 0, 00000000 = +1, 01111111 = +127
        // therefore negative numbers have the most significant set, positive numbers do not
        //
        // To assist us, the 6502 can set the overflow flag, if the result of the addition has
        // wrapped around. V <- ~(A^M) & A^(A+M+C) :D lol, let's work out why!
        //
        // Let's suppose we have A = 30, M = 10 and C = 0
        //          A = 30 = 00011110
        //          M = 10 = 00001010+
        //     RESULT = 40 = 00101000
        //
        // Here we have not gone out of range. The resulting significant bit has not changed.
        // So let's make a truth table to understand when overflow has occurred. Here I take
        // the MSB of each component, where R is RESULT.
        //
        // A  M  R | V | A^R | A^M |~(A^M) | 
        // 0  0  0 | 0 |  0  |  0  |   1   |
        // 0  0  1 | 1 |  1  |  0  |   1   |
        // 0  1  0 | 0 |  0  |  1  |   0   |
        // 0  1  1 | 0 |  1  |  1  |   0   |  so V = ~(A^M) & (A^R)
        // 1  0  0 | 0 |  1  |  1  |   0   |
        // 1  0  1 | 0 |  0  |  1  |   0   |
        // 1  1  0 | 1 |  1  |  0  |   1   |
        // 1  1  1 | 0 |  0  |  0  |   1   |
        //
        // We can see how the above equation calculates V, based on A, M and R. V was chosen
        // based on the following hypothesis:
        //       Positive Number + Positive Number = Negative Result -> Overflow
        //       Negative Number + Negative Number = Positive Result -> Overflow
        //       Positive Number + Negative Number = Either Result -> Cannot Overflow
        //       Positive Number + Positive Number = Positive Result -> OK! No Overflow
        //       Negative Number + Negative Number = Negative Result -> OK! NO Overflow
        /// <summary>
        /// Instruction: Add with Carry In
        /// Function:    A = A + M + C
        /// Flags Out:   C, V, N, Z
        /// </summary>
        /// <returns></returns>
        public byte ADC() {
            // Grab the data that we are adding to the accumulator
            fetch();

            // Add is performed in 16-bit domain for emulation to capture any
            // carry bit, which will exist in bit 8 of the 16-bit word
            var temp = (UInt16)((UInt16)reg.a + (UInt16)fetched + (UInt16)(reg.GetFlag(Registers.FLAGS6502.C) ? 1 : 0));

            // The carry flag out exists in the high byte bit 0
            reg.SetFlag(Registers.FLAGS6502.C, temp > 255);

            // The Zero flag is set if the result is 0
            reg.SetFlag(Registers.FLAGS6502.Z, (temp & 0x00FF) == 0);

            // The signed Overflow flag is set based on all that up there! :D
            reg.SetFlag(Registers.FLAGS6502.V, (~((UInt16)reg.a ^ (UInt16)fetched) & ((UInt16)reg.a ^ (UInt16)temp)) & 0x0080);

            // The negative flag is set to the most significant bit of the result
            reg.SetFlag(Registers.FLAGS6502.N, temp & 0x80);

            // Load the result into the accumulator (it's 8-bit dont forget!)
            reg.a = (byte)(temp & 0x00FF);

            // This instruction has the potential to require an additional clock cycle
            return 1;
        }

        /// <summary>
        /// Instruction: Bitwise Logic AND
        /// Function:    A = A & M
        /// Flags Out:   N, Z
        /// </summary>
        /// <returns></returns>
        public byte AND() {
            fetch();
            reg.a = (byte)(reg.a & fetched);
            reg.SetFlag(Registers.FLAGS6502.Z, addr_abs == 0x0);
            reg.SetFlag(Registers.FLAGS6502.N, addr_abs & 0x80);
            return 1;
        }

        /// <summary>
        /// Instruction: Arithmetic Shift Left
        /// Function:    A = C <- (A << 1) <- 0
        /// Flags Out:   N, Z, C
        /// </summary>
        /// <returns></returns>
        public byte ASL() {
            fetch();
            var temp = (UInt16)(fetched << 1);
            reg.SetFlag(Registers.FLAGS6502.C, (temp & 0xFF00));
            reg.SetFlag(Registers.FLAGS6502.Z, (temp & 0x00FF) == 0x00);
            reg.SetFlag(Registers.FLAGS6502.N, temp & 0x80);
            if (lookup[opcode].addrMode == IMP)
                reg.a = (byte)(temp & 0x00FF);
            else
                write(addr_abs, (byte)(temp & 0x00FF));
            return 0;
        }
        /// <summary>
        /// Instruction: Branch if Carry Clear
        /// Function:    if(C == 0) pc = address 
        /// </summary>
        /// <returns></returns>
        public byte BCC() {
            if (reg.GetFlag(Registers.FLAGS6502.C) == false) {
                cycles++;
                addr_abs = (UInt16)(reg.pc + addr_rel);
                if ((addr_abs & 0xFF00) != (reg.pc & 0xFF00)) {
                    cycles++;
                }

                reg.pc = addr_abs;
            }
            return 0;
        }

        /// <summary>
        /// Instruction: Branch if Carry Set
        /// Function:    if(C == 1) pc = address
        /// </summary>
        /// <returns></returns>
        public byte BCS() {
            if (reg.GetFlag(Registers.FLAGS6502.C)) {
                cycles++;
                addr_abs = (UInt16)(reg.pc + addr_rel);
                if ((addr_abs & 0xFF00) != (reg.pc & 0xFF00)) {
                    cycles++;
                }

                reg.pc = addr_abs;
            }
            return 0;
        }

        /// <summary>
        /// Instruction: Branch if Equal
        /// Function:    if(Z == 1) pc = address
        /// </summary>
        /// <returns></returns>
        public byte BEQ() {
            if (reg.GetFlag(Registers.FLAGS6502.Z)) {
                cycles++;
                addr_abs = (UInt16)(reg.pc + addr_rel);
                if ((addr_abs & 0xFF00) != (reg.pc & 0xFF00)) {
                    cycles++;
                }

                reg.pc = addr_abs;
            }
            return 0;
        }
        public byte BIT() {
            fetch();
            var temp = (UInt16)(reg.a & fetched);
            reg.SetFlag(Registers.FLAGS6502.Z, (temp & 0x00FF) == 0x00);
            reg.SetFlag(Registers.FLAGS6502.N, fetched & (1 << 7));
            reg.SetFlag(Registers.FLAGS6502.V, fetched & (1 << 6));
            return 0;
        }
        /// <summary>
        /// Instruction: Branch if Negative
        /// Function:    if(N == 1) pc = address
        /// </summary>
        /// <returns></returns>
        public byte BMI()
        {
            if (reg.GetFlag(Registers.FLAGS6502.N)) {
                cycles++;
                addr_abs = (UInt16)(reg.pc + addr_rel);
                if ((addr_abs & 0xFF00) != (reg.pc & 0xFF00)) {
                    cycles++;
                }

                reg.pc = addr_abs;
            }
            return 0;
        }
        /// <summary>
        /// Instruction: Branch if Not Equal
        /// Function:    if(Z == 0) pc = address
        /// </summary>
        /// <returns></returns>
        public byte BNE() {
            if (reg.GetFlag(Registers.FLAGS6502.Z) == false) {
                cycles++;
                addr_abs = (UInt16)(reg.pc + addr_rel);
                if ((addr_abs & 0xFF00) != (reg.pc & 0xFF00)) {
                    cycles++;
                }

                reg.pc = addr_abs;
            }
            return 0;
        }
        /// <summary>
        /// Instruction: Branch if Positive
        /// Function:    if(N == 0) pc = address
        /// </summary>
        /// <returns></returns>
        public byte BPL() {
            if (reg.GetFlag(Registers.FLAGS6502.N) == false) {
                cycles++;
                addr_abs = (UInt16)(reg.pc + addr_rel);
                if ((addr_abs & 0xFF00) != (reg.pc & 0xFF00)) {
                    cycles++;
                }

                reg.pc = addr_abs;
            }
            return 0;
        }

        /// <summary>
        /// Instruction: Break
        /// Function:    Program Sourced Interrupt
        /// </summary>
        /// <returns></returns>

        public byte BRK() {
            reg.pc++;

            reg.SetFlag(Registers.FLAGS6502.I, true);
            write((UInt16)(0x0100 + reg.stkp), (byte)((reg.pc >> 8) & 0x00FF));
            reg.stkp--;
            write((UInt16)(0x0100 + reg.stkp), (byte)(reg.pc & 0x00FF));
            reg.stkp--;

            reg.SetFlag(Registers.FLAGS6502.B, true);
            write((UInt16)(0x0100 + reg.stkp), reg.status);
            reg.stkp--;
            reg.SetFlag(Registers.FLAGS6502.B, false);

            reg.pc = (UInt16)((UInt16)read(0xFFFE) | ((UInt16)(read(0xFFFF)) << 8));
            return 0;
        }

        /// <summary>
        /// Instruction: Branch if Overflow Clear
        /// Function:    if(V == 0) pc = address
        /// </summary>
        /// <returns></returns>
        public byte BVC()
        {
            if (reg.GetFlag(Registers.FLAGS6502.V) == false) {
                cycles++;
                addr_abs = (UInt16)(reg.pc + addr_rel);
                if ((addr_abs & 0xFF00) != (reg.pc & 0xFF00)) {
                    cycles++;
                }

                reg.pc = addr_abs;
            }
            return 0;
        }

        /// <summary>
        /// Instruction: Branch if Overflow Set
        /// Function:    if(V == 1) pc = address
        /// </summary>
        /// <returns></returns>
        public byte BVS() {
            if (reg.GetFlag(Registers.FLAGS6502.V)) {
                cycles++;
                addr_abs = (UInt16)(reg.pc + addr_rel);
                if ((addr_abs & 0xFF00) != (reg.pc & 0xFF00)) {
                    cycles++;
                }

                reg.pc = addr_abs;
            }
            return 0;
        }
        /// <summary>
        /// Instruction: Clear Carry Flag
        /// Function:    C = 0
        /// </summary>
        /// <returns></returns>
        public byte CLC() {
            reg.SetFlag(Registers.FLAGS6502.C, false);
            return 0;
        }
        /// <summary>
        /// Instruction: Clear Decimal Flag
        /// Function:    D = 0>
        /// <returns></returns>
        public byte CLD() {
            reg.SetFlag(Registers.FLAGS6502.D, false);
            return 0;
        }

        /// <summary>
        /// Instruction: Disable Interrupts / Clear Interrupt Flag
        /// Function:    I = 0
        /// </summary>
        /// <returns></returns>
        public byte CLI() {
            reg.SetFlag(Registers.FLAGS6502.I, false);
            return 0;
        }

        // Instruction: Clear Overflow Flag
        // Function:    V = 0
        public byte CLV() {
            reg.SetFlag(Registers.FLAGS6502.V, false);
            return 0;
        }
        /// <summary>
        /// // Instruction: Compare Accumulator
        /// Function:    C <- A >= M      Z <- (A - M) == 0
        /// Flags Out:   N, C, Z
        /// </summary>
        /// <returns></returns>
        public byte CMP() {
            fetch();
            var temp = (UInt16)((UInt16)reg.a - (UInt16)fetched);
            reg.SetFlag(Registers.FLAGS6502.C, reg.a >= fetched);
            reg.SetFlag(Registers.FLAGS6502.Z, (temp & 0x00FF) == 0x0000);
            reg.SetFlag(Registers.FLAGS6502.N, temp & 0x0080);
            return 1;
        }

        /// <summary>
        /// // Instruction: Compare X Register
        /// Function:    C <- X >= M      Z <- (X - M) == 0
        /// Flags Out:   N, C, Z
        /// </summary>
        /// <returns></returns>

        public byte CPX() {
            fetch();
            var temp = (UInt16)((UInt16)reg.x - (UInt16)fetched);
            reg.SetFlag(Registers.FLAGS6502.C, reg.x >= fetched);
            reg.SetFlag(Registers.FLAGS6502.Z, (temp & 0x00FF) == 0x0000);
            reg.SetFlag(Registers.FLAGS6502.N, temp & 0x0080);
            return 0;
        }

        /// <summary>
        /// Instruction: Compare Y Register
        /// Function:    C <- Y >= M      Z <- (Y - M) == 0
        /// Flags Out:   N, C, Z
        /// </summary>
        /// <returns></returns>
        public byte CPY() {
            fetch();
            var temp = (UInt16)((UInt16)reg.y - (UInt16)fetched);
            reg.SetFlag(Registers.FLAGS6502.C, reg.y >= fetched);
            reg.SetFlag(Registers.FLAGS6502.Z, (temp & 0x00FF) == 0x0000);
            reg.SetFlag(Registers.FLAGS6502.N, temp & 0x0080);
            return 0;
        }

        /// <summary>
        /// Instruction: Decrement Value at Memory Location
        /// Function:    M = M - 1
        /// Flags Out:   N, Z
        /// </summary>
        /// <returns></returns>
        public byte DEC() {
            fetch();
            var temp = fetched - 1;
            write(addr_abs, (byte)(temp & 0x00FF));
            reg.SetFlag(Registers.FLAGS6502.Z, (temp & 0x00FF) == 0x0000);
            reg.SetFlag(Registers.FLAGS6502.N, temp & 0x0080);
            return 0;
        }
        /// <summary>
        /// Instruction: Decrement X Register
        /// Function:    X = X - 1
        /// Flags Out:   N, Z
        /// </summary>
        /// <returns></returns>
        public byte DEX() {
            reg.x--;
            reg.SetFlag(Registers.FLAGS6502.Z, (reg.x & 0x00FF) == 0x0000);
            reg.SetFlag(Registers.FLAGS6502.N, reg.x & 0x0080);
            return 0;
        }
        /// <summary>
        /// Instruction: Decrement Y Register
        /// Function:    Y = Y - 1
        /// Flags Out:   N, Z
        /// </summary>
        /// <returns></returns>
        public byte DEY() {
            reg.y--;
            reg.SetFlag(Registers.FLAGS6502.Z, (reg.y & 0x00FF) == 0x0000);
            reg.SetFlag(Registers.FLAGS6502.N, reg.y & 0x0080);
            return 0;
        }

        /// <summary>
        /// Instruction: Bitwise Logic XOR
        /// Function:    A = A xor M
        /// Flags Out:   N, Z
        /// </summary>
        /// <returns></returns>
        public byte EOR() {
            fetch();
            reg.a = (byte)(reg.a ^ fetched);
            reg.SetFlag(Registers.FLAGS6502.Z, reg.a == 0x00);
            reg.SetFlag(Registers.FLAGS6502.N, (reg.a & 0x80) != 1);
            return 1;
        }
        /// <summary>
        /// Instruction: Increment Value at Memory Location
        /// Function:    M = M + 1
        /// Flags Out:   N, Z
        /// </summary>
        /// <returns></returns>
        public byte INC() {
            fetch();
            var temp = fetched + 1;
            write(addr_abs, (byte)(temp & 0x00FF));
            reg.SetFlag(Registers.FLAGS6502.Z, (temp & 0x00FF) == 0x0000);
            reg.SetFlag(Registers.FLAGS6502.N, temp & 0x0080);
            return 0;
        }
        /// <summary>
        /// Instruction: Increment X Register
        /// Function:    X = X + 1
        /// Flags Out:   N, Z
        /// </summary>
        /// <returns></returns>
        public byte INX() {
            reg.x++;
            reg.SetFlag(Registers.FLAGS6502.Z, (reg.x & 0x00FF) == 0x0000);
            reg.SetFlag(Registers.FLAGS6502.N, reg.x & 0x0080);
            return 0;
        }
        /// <summary>
        /// Instruction: Increment Y Register
        /// Function:    Y = Y + 1
        /// Flags Out:   N, Z
        /// </summary>
        /// <returns></returns>
        public byte INY() {
            reg.y++;
            reg.SetFlag(Registers.FLAGS6502.Z, (reg.y & 0x00FF) == 0x0000);
            reg.SetFlag(Registers.FLAGS6502.N, reg.y & 0x0080);
            return 0;
        }

        /// <summary>
        /// Instruction: Jump To Location
        /// Function:    pc = address
        /// </summary>
        /// <returns></returns>
        public byte JMP() {
            reg.pc = addr_abs;
            return 0;
        }

        /// <summary>
        /// Instruction: Jump To Sub-Routine
        /// Function:    Push current pc to stack, pc = address
        /// </summary>
        /// <returns></returns>
        public byte JSR() {
            reg.pc--;

            write((UInt16)(0x0100 + reg.stkp), (byte)((reg.pc >> 8) & 0x00FF));
            reg.stkp--;
            write((UInt16)(0x0100 + reg.stkp), (byte)(reg.pc & 0x00FF));
            reg.stkp--;

            reg.pc = addr_abs;
            return 0;
        }

        /// <summary>
        /// Instruction: Load The Accumulator
        /// Function:    A = M
        /// Flags Out:   N, Z
        /// </summary>
        /// <returns></returns>
        public byte LDA() {
            fetch();
            reg.a = fetched;
            reg.SetFlag(Registers.FLAGS6502.Z, reg.a == 0x00);
            reg.SetFlag(Registers.FLAGS6502.N, reg.a & 0x80);
            return 1;
        }

        /// <summary>
        /// Instruction: Load The X Register
        /// Function:    X = M
        /// Flags Out:   N, Z
        /// </summary>
        /// <returns></returns>
        public byte LDX() {
            fetch();
            reg.x = fetched;
            reg.SetFlag(Registers.FLAGS6502.Z, reg.x == 0x00);
            reg.SetFlag(Registers.FLAGS6502.N, reg.x & 0x80);
            return 1;

        }
        /// <summary>
        /// Instruction: Load The Y Register
        /// Function:    Y = M
        /// Flags Out:   N, Z
        /// </summary>
        /// <returns></returns>
        public byte LDY() {
            fetch();
            reg.y = fetched;
            reg.SetFlag(Registers.FLAGS6502.Z, reg.y == 0x00);
            reg.SetFlag(Registers.FLAGS6502.N, reg.y & 0x80);
            return 1;
        }
        /// <summary>
        /// Instruction: Logial Shift Right
        /// Function: M >> 1
        /// Flags Out: C, Z, N
        /// </summary>
        /// <returns></returns>
        public byte LSR() {
            fetch();
            reg.SetFlag(Registers.FLAGS6502.C, (fetched & 0x0001));
            var temp = fetched >> 1;
            reg.SetFlag(Registers.FLAGS6502.Z, (temp & 0x00FF) == 0x0000);
            reg.SetFlag(Registers.FLAGS6502.N, (temp & 0x0080));
            if (lookup[opcode].addrMode == IMP)
                reg.a = (byte)(temp & 0x00FF);
            else
                write(addr_abs, (byte)(temp & 0x00FF));
            return 0;
        }
        /// <summary>
        /// No Operations
        /// </summary>
        /// <returns></returns>
        public byte NOP() {
            // Sadly not all NOPs are equal, Ive added a few here
            // based on https://wiki.nesdev.com/w/index.php/CPU_unofficial_opcodes
            // and will add more based on game compatibility, and ultimately
            // I'd like to cover all illegal opcodes too
            switch (opcode) {
                case 0x1C:
                case 0x3C:
                case 0x5C:
                case 0x7C:
                case 0xDC:
                case 0xFC:
                    return 1;
            }
            return 0;
        }
        /// <summary>
        /// Instruction: Bitwise Logic OR
        /// Function:    A = A | M
        /// Flags Out:   N, Z
        /// </summary>
        /// <returns></returns>
        public byte ORA() {
            fetch();
            reg.a = (byte)(reg.a | fetched);
            reg.SetFlag(Registers.FLAGS6502.Z, reg.a == 0x00);
            reg.SetFlag(Registers.FLAGS6502.N, reg.a & 0x80);
            return 1;
        }
        /// <summary>
        /// Instruction: Push Accumulator to Stack
        /// Function:    A -> stack
        /// </summary>
        /// <returns></returns>
        public byte PHA() {
            write((UInt16)(0x0100 + reg.stkp), reg.a);
            reg.stkp--;
            return 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte PHP() {
            write((UInt16)(0x0100 + reg.stkp), (byte)(reg.status | (byte)Registers.FLAGS6502.B | (byte)Registers.FLAGS6502.U));
            reg.SetFlag(Registers.FLAGS6502.B, false);
            reg.SetFlag(Registers.FLAGS6502.U, false);
            reg.stkp--;
            return 0;

        }
        /// <summary>
        /// Instruction: Push Status Register to Stack
        /// Function:    status -> stack
        /// Note:        Break flag is set to 1 before push
        /// </summary>
        /// <returns></returns>
        public byte PLA() {

            reg.stkp++;
            reg.a = read(0x0100 + reg.stkp);
            reg.SetFlag(Registers.FLAGS6502.Z, reg.a == 0x00);
            reg.SetFlag(Registers.FLAGS6502.N, reg.a & 0x80);
            return 0;
        }
        /// <summary>
        /// Instruction: Pop Status Register off Stack
        /// Function:    Status <- stack
        /// </summary>
        /// <returns></returns>
        public byte PLP() {
            reg.stkp++;
            reg.status = read(0x0100 + reg.stkp);
            reg.SetFlag(Registers.FLAGS6502.U, 1);
            return 0;
        }
        /// <summary>
        /// ROtate Left
        /// </summary>
        /// <returns></returns>
        public byte ROL() {
            fetch();

            var temp = (UInt16)((fetched << 1) | reg.GetFlagByte(Registers.FLAGS6502.C));
            reg.SetFlag(Registers.FLAGS6502.C, temp & 0xFF00);
            reg.SetFlag(Registers.FLAGS6502.Z, (temp & 0x00FF) == 0x0000);
            reg.SetFlag(Registers.FLAGS6502.N, temp & 0x0080);
            if (lookup[opcode].addrMode == IMP)
                reg.a = (byte)(temp & 0x00FF);
            else
                write(addr_abs, (byte)(temp & 0x00FF));
            return 0;
        }
        /// <summary>
        /// ROtate Right
        /// </summary>
        /// <returns></returns>
        public byte ROR() {
            fetch();
            var temp = (UInt16)((reg.GetFlag(Registers.FLAGS6502.C) ? 1 : 0) << 7) | (fetched >> 1);
            reg.SetFlag(Registers.FLAGS6502.C, fetched & 0x01);
            reg.SetFlag(Registers.FLAGS6502.Z, (temp & 0x00FF) == 0x00);
            reg.SetFlag(Registers.FLAGS6502.N, temp & 0x0080);
            if (lookup[opcode].addrMode == IMP)
                reg.a = (byte)(temp & 0x00FF);
            else
                write(addr_abs, (byte)(temp & 0x00FF));
            return 0;
        }
        /// <summary>
        /// Return from Interrupt
        /// </summary>
        /// <returns></returns>
        public byte RTI() {
            reg.stkp++;
            reg.status = read(0x0100 + reg.stkp);
            reg.status = (byte)(reg.status & ~(byte)Registers.FLAGS6502.B);
            reg.status = (byte)(reg.status & ~(byte)Registers.FLAGS6502.U);

            reg.stkp++;
            reg.pc = (UInt16)read(0x0100 + reg.stkp);
            reg.stkp++;
            reg.pc |= (UInt16)(read(0x0100 + reg.stkp) << 8);
            return 0;
        }
        /// <summary>
        /// Return from Subroutine
        /// </summary>
        /// <returns></returns>
        public byte RTS() {
            reg.stkp++;
            reg.pc = (UInt16)read(0x0100 + reg.stkp);
            reg.stkp++;
            reg.pc |= (UInt16)(read(0x0100 + reg.stkp) << 8);

            reg.pc++;
            return 0;
        }

        // Instruction: Subtraction with Borrow In
        // Function:    A = A - M - (1 - C)
        // Flags Out:   C, V, N, Z
        //
        // Explanation:
        // Given the explanation for ADC above, we can reorganise our data
        // to use the same computation for addition, for subtraction by multiplying
        // the data by -1, i.e. make it negative
        //
        // A = A - M - (1 - C)  ->  A = A + -1 * (M - (1 - C))  ->  A = A + (-M + 1 + C)
        //
        // To make a signed positive number negative, we can invert the bits and add 1
        // (OK, I lied, a little bit of 1 and 2s complement :P)
        //
        //  5 = 00000101
        // -5 = 11111010 + 00000001 = 11111011 (or 251 in our 0 to 255 range)
        //
        // The range is actually unimportant, because if I take the value 15, and add 251
        // to it, given we wrap around at 256, the result is 10, so it has effectively 
        // subtracted 5, which was the original intention. (15 + 251) % 256 = 10
        //
        // Note that the equation above used (1-C), but this got converted to + 1 + C.
        // This means we already have the +1, so all we need to do is invert the bits
        // of M, the data(!) therfore we can simply add, exactly the same way we did 
        // before.

        /// <summary>
        /// Instruction: Subtraction with Borrow In
        /// Function:    A = A - M - (1 - C)
        /// Flags Out:   C, V, N, Z        
        /// </summary>
        /// <returns></returns>
        public byte SBC() {
            fetch();

            // Operating in 16-bit domain to capture carry out

            // We can invert the bottom 8 bits with bitwise xor
            var value = ((UInt16)(fetched) ^ 0x00FF);

            // Notice this is exactly the same as addition from here!
            var temp = (UInt16)reg.a + value + (reg.GetFlag(Registers.FLAGS6502.C) ? 1 : 0);
            reg.SetFlag(Registers.FLAGS6502.C, temp & 0xFF00);
            reg.SetFlag(Registers.FLAGS6502.Z, ((temp & 0x00FF) == 0));
            reg.SetFlag(Registers.FLAGS6502.V, (temp ^ (UInt16)reg.a) & (temp ^ value) & 0x0080);
            reg.SetFlag(Registers.FLAGS6502.N, temp & 0x0080);
            reg.a = (byte)(temp & 0x00FF);
            return 1;
        }
        /// <summary>
        /// Instruction: Set Carry Flag
        /// Function:    C = 1
        /// </summary>
        /// <returns></returns>
        public byte SEC() {
            reg.SetFlag(Registers.FLAGS6502.C, true);
            return 0; 
        }
        /// <summary>
        /// Instruction: Set Decimal Flag
        /// Function:    D = 1
        /// </summary>
        /// <returns></returns>
        public byte SED() {
            reg.SetFlag(Registers.FLAGS6502.D, true);
            return 0;
        }
        /// <summary>
        /// Instruction: Set Interrupt Flag / Enable Interrupts
        /// Function:    I = 1
        /// </summary>
        /// <returns></returns>
        public byte SEI() {
            reg.SetFlag(Registers.FLAGS6502.I, true);
            return 0;
        }
        /// <summary>
        /// Instruction: Store Accumulator at Address
        /// Function:    M = A
        /// </summary>
        /// <returns></returns>
        public byte STA() {
            write(addr_abs, reg.a);
            return 0;
        }
        /// <summary>
        /// Instruction: Store X Register at Address
        /// Function:    M = X
        /// </summary>
        /// <returns></returns>
        public byte STX() {
            write(addr_abs, reg.x);
            return 0;
        }
        /// <summary>
        /// Instruction: Store Y Register at Address
        /// Function:    M = Y
        /// </summary>
        /// <returns></returns>
        public byte STY() {
            write(addr_abs, reg.y);
            return 0;
        }
        /// <summary>
        /// Instruction: Transfer Accumulator to X Register
        /// Function:    X = A
        /// Flags Out:   N, Z
        /// </summary>
        /// <returns></returns>
        public byte TAX() {
            reg.x = reg.a;
            reg.SetFlag(Registers.FLAGS6502.Z, reg.x == 0x00);
            reg.SetFlag(Registers.FLAGS6502.N, reg.x & 0x80);
            return 0;
        }
        /// <summary>
        /// Instruction: Transfer Accumulator to Y Register
        /// Function:    Y = A
        /// Flags Out:   N, Z
        /// </summary>
        /// <returns></returns>
        public byte TAY() {
            reg.y = reg.a;
            reg.SetFlag(Registers.FLAGS6502.Z, reg.y == 0x00);
            reg.SetFlag(Registers.FLAGS6502.N, reg.y & 0x80);
            return 0;
        }
        /// <summary>
        /// Instruction: Transfer Stack Pointer to X Register
        /// Function:    X = stack pointer
        /// Flags Out:   N, Z
        /// </summary>
        /// <returns></returns>
        public byte TSX() {
            reg.x = reg.stkp;
            reg.SetFlag(Registers.FLAGS6502.Z, reg.x == 0x00);
            reg.SetFlag(Registers.FLAGS6502.N, reg.x & 0x80);
            return 0;
        }
        /// <summary>
        /// Instruction: Transfer X Register to Accumulator
        /// Function:    A = X
        /// Flags Out:   N, Z
        /// </summary>
        /// <returns></returns>
        public byte TXA() {
            reg.a = reg.x;
            reg.SetFlag(Registers.FLAGS6502.Z, reg.a == 0x00);
            reg.SetFlag(Registers.FLAGS6502.N, reg.a & 0x80);
            return 0;
        }
        /// <summary>
        // Instruction: Transfer X Register to Stack Pointer
        // Function:    stack pointer = X
        /// </summary>
        /// <returns></returns>
        public byte TXS() {
            reg.stkp = reg.x;
            return 0;
        }
        /// <summary>
        // Instruction: Transfer Y Register to Accumulator
        // Function:    A = Y
        // Flags Out:   N, Z
        /// </summary>
        /// <returns></returns>
        public byte TYA() {
            reg.a = reg.y;
            reg.SetFlag(Registers.FLAGS6502.Z, reg.a == 0x00);
            reg.SetFlag(Registers.FLAGS6502.N, reg.a & 0x80);
            return 0;
        }
        /// <summary>
        /// This function captures illegal opcodes
        /// </summary>
        /// <returns></returns>
        public byte XXX() {
            return 0;
        }
        #endregion
        #region Other
        public void clock()
        {
            if (cycles == 0) {
                opcode = read(reg.pc);
                reg.pc++;

                cycles = lookup[opcode].cycles;
                byte additional_cycle1 = lookup[opcode].addrMode();
                byte additional_cycle2 = lookup[opcode].operate();
                cycles += (byte)(additional_cycle1 & additional_cycle2);
            }
            cycles--;
        }
        public void reset()
        {
            reg.a = 0;
            reg.x = 0;
            reg.y = 0;
            reg.stkp = 0xFD;
            reg.status = 0x00 | (byte)Registers.FLAGS6502.U;

            addr_abs = 0xFFFC;
            var lo = (UInt16)read(addr_abs + 0);
            var hi = (UInt16)read(addr_abs + 1);

            reg.pc = (UInt16)((hi << 8) | lo);
            addr_rel = 0x0000;
            addr_abs = 0x0000;
            fetched = 0x00;

            cycles = 8;
        }

        public void irq()
        {
            if (reg.GetFlag(Registers.FLAGS6502.I) == false) {
                write((UInt16)(0x0100 + reg.stkp), (byte)((reg.pc << 8) & 0x00FF));
                reg.stkp--;
                write((UInt16)(0x0100 + reg.stkp), (byte)(reg.pc & 0x00FF));
                reg.stkp--;

                reg.SetFlag(Registers.FLAGS6502.B, false);
                reg.SetFlag(Registers.FLAGS6502.U, true);
                reg.SetFlag(Registers.FLAGS6502.I, true);

                write((UInt16)(0x011 + reg.stkp), reg.status);
                reg.stkp--;

                addr_abs = 0xFFFE;
                var lo = read(addr_abs + 0);
                var hi = read(addr_abs + 1);
                reg.pc = (UInt16)((hi << 8) | lo);

                cycles = 7;
            }
        }

        public void nmi()
        {
            write((UInt16)(0x0100 + reg.stkp), (byte)((reg.pc << 8) & 0x00FF));
            reg.stkp--;
            write((UInt16)(0x0100 + reg.stkp), (byte)(reg.pc & 0x00FF));
            reg.stkp--;

            reg.SetFlag(Registers.FLAGS6502.B, false);
            reg.SetFlag(Registers.FLAGS6502.U, true);
            reg.SetFlag(Registers.FLAGS6502.I, true);

            write((UInt16)(0x011 + reg.stkp), reg.status);
            reg.stkp--;

            addr_abs = 0xFFFE;
            var lo = read(addr_abs + 0);
            var hi = read(addr_abs + 1);
            reg.pc = (UInt16)((hi << 8) | lo);

            cycles = 7;
        }

        #region Helper Functions
        public bool complete()
        {
            return cycles == 0;
        }

        // This is the disassembly function. Its workings are not required for emulation.
        // It is merely a convenience function to turn the binary instruction code into
        // human readable form. Its included as part of the emulator because it can take
        // advantage of many of the CPUs internal operations to do this.
        public List<string> disassemble(UInt16 nStart, UInt16 nStop)
        {
            UInt32 addr = (UInt32)nStart;
            byte value = 0x00, lo = 0x00, hi = 0x00;
            List<string> mapLines = new List<string>();
            UInt16 line_addr = 0;

    //        // A convenient utility to convert variables into
    //        // hex strings because "modern C++"'s method with 
    //        // streams is atrocious
    //        auto hex = [](uint32_t n, uint8_t d)
        
    //{
    //            std::string s(d, '0');
    //            for (int i = d - 1; i >= 0; i--, n >>= 4)
    //                s[i] = "0123456789ABCDEF"[n & 0xF];
    //            return s;
    //        };

            // Starting at the specified address we read an instruction
            // byte, which in turn yields information from the lookup table
            // as to how many additional bytes we need to read and what the
            // addressing mode is. I need this info to assemble human readable
            // syntax, which is different depending upon the addressing mode

            // As the instruction is decoded, a std::string is assembled
            // with the readable output
            while (addr <= (UInt32)nStop) {
                line_addr = (UInt16)addr;

                // Prefix line with instruction address
                var sInst = "$" + Convert.ToString(line_addr, 16) + ": ";

                // Read instruction, and get its readable name
                var opcode = bus.read(line_addr, true); addr++; line_addr++;
                sInst += lookup[opcode].Name + " ";

                // Get oprands from desired locations, and form the
                // instruction based upon its addressing mode. These
                // routines mimmick the actual fetch routine of the
                // 6502 in order to get accurate data as part of the
                // instruction
                if (lookup[opcode].addrMode == IMP) {
                    sInst += " {IMP}";
                } else if (lookup[opcode].addrMode == IMM) {
                    value = bus.read(line_addr, true); addr++;
                    sInst += "#$" + Convert.ToString(value, 16) + " {IMM}";
                } else if (lookup[opcode].addrMode == ZP0) {
                    lo = bus.read(line_addr, true); addr++;                    
                    hi = 0x00;
                    sInst += "$" + Convert.ToString(lo, 16) + " {ZP0}";
                } else if (lookup[opcode].addrMode == ZPX) {
                    lo = bus.read(line_addr, true); addr++;
                    hi = 0x00;
                    sInst += "$" + Convert.ToString(lo, 16) + ", X {ZPX}";
                } else if (lookup[opcode].addrMode == ZPY) {
                    lo = bus.read(line_addr, true); addr++;
                    hi = 0x00;
                    sInst += "$" + Convert.ToString(lo, 16) + ", Y {ZPY}";
                } else if (lookup[opcode].addrMode == IZX) {
                    lo = bus.read(line_addr, true); addr++;
                    hi = 0x00;
                    sInst += "($" + Convert.ToString(lo, 16) + ", X) {IZX}";
                } else if (lookup[opcode].addrMode == IZY) {
                    lo = bus.read(line_addr, true); addr++;
                    hi = 0x00;
                    sInst += "($" + Convert.ToString(lo, 16) + "), Y {IZY}";
                } else if (lookup[opcode].addrMode == ABS) {
                    lo = bus.read(line_addr, true); addr++; line_addr++;
                    hi = bus.read(line_addr, true); addr++; line_addr++;
                    sInst += "$" + Convert.ToString((hi << 8) | lo, 16) + " {ABS}";
                } else if (lookup[opcode].addrMode == ABX) {
                    lo = bus.read(line_addr, true); addr++; line_addr++;
                    hi = bus.read(line_addr, true); addr++; line_addr++;
                    sInst += "$" + Convert.ToString((hi << 8) | lo, 16) + ", X {ABX}";
                } else if (lookup[opcode].addrMode == ABY) {
                    lo = bus.read(line_addr, true); addr++; line_addr++;
                    hi = bus.read(line_addr, true); addr++; line_addr++;
                    sInst += "$" + Convert.ToString((hi << 8) | lo, 16) + ", Y {ABY}";
                } else if (lookup[opcode].addrMode == IND) {
                    lo = bus.read(line_addr, true); addr++; line_addr++;
                    hi = bus.read(line_addr, true); addr++; line_addr++;
                    sInst += "($" + Convert.ToString((hi << 8) | lo, 16) + ") {IND}";
                } else if (lookup[opcode].addrMode == REL) {
                    value = bus.read(line_addr, true); addr++;
                    sInst += "#$" + Convert.ToString(value, 16) + " {IMM}";

                    sInst += "$" + Convert.ToString(value, 16) + " [$" + Convert.ToString(addr + value, 16) + "] {REL}";
                }

                // Add the formed string to a std::map, using the instruction's
                // address as the key. This makes it convenient to look for later
                // as the instructions are variable in length, so a straight up
                // incremental index is not sufficient.
                mapLines[line_addr] = sInst;
            }

            return mapLines;
        }
        #endregion
        #endregion
    }
}

//    Nuked-OPL3-Port
//    Copyright (C) 2026  Guy Newman
//    https://github.com/guy-newman/Nuked-OPL3-Port
//    A C# port of the Nuked-OPL3 code, available here:
//    https://github.com/nukeykt/Nuked-OPL3

//    This library is free software; you can redistribute it and/or
//    modify it under the terms of the GNU Lesser General Public
//    License as published by the Free Software Foundation; either
//    version 2.1 of the License, or (at your option) any later version.

//    This library is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//    Lesser General Public License for more details.

//    You should have received a copy of the GNU Lesser General Public
//    License along with this library; if not, write to the Free Software
//    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110 - 1301
//    USA

using System;

namespace Nuked_OPL3_Port
{
    internal class Opl3_Slot
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        internal Opl3_Slot(Opl3_Chip chip)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        {
            this.chip = chip;

            mod = chip.ZeroModShortFunc;
            eg_rout = 0x1ff;
            eg_out = 0x1ff;
            eg_gen = EnvelopeGenNum.Release;
            trem = chip.ZeroModByteFunc;
        }

        public Opl3_Channel channel;
        public Opl3_Chip chip;
        public short out_renamed;
        public short fbmod;
        public Func<short> mod;
        public short prout;
        public ushort eg_rout;
        public ushort eg_out;
        public EnvelopeGenNum eg_gen;
        public byte eg_ksl;
        public Func<byte> trem;
        public byte reg_vib;
        public byte reg_type;
        public byte reg_ksr;
        public byte reg_mult;
        public byte reg_ksl;
        public byte reg_tl;
        public byte reg_ar;
        public byte reg_dr;
        public byte reg_sl;
        public byte reg_rr;
        public byte reg_wf;
        public byte key;
        public bool pg_reset;
        public uint pg_phase;
        public ushort pg_phase_out;
        public byte slot_num;

        public short FbModFunc() => fbmod;
        public short OutFunc() => out_renamed;
    }
}

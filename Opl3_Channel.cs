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
    internal class Opl3_Channel
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public Opl3_Channel(Opl3_Chip chip)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        {
            Reset(chip);
        }

        public void Reset(Opl3_Chip chip)
        {
            this.chip = chip;

            f_num = default;
            block = default;
            fb = default;
            con = default;
            alg = default;
            ksv = default;
            chc = default;
            chd = default;
            ch_num = default;

            out_renamed[0] = chip.ZeroModShortFunc;
            out_renamed[1] = chip.ZeroModShortFunc;
            out_renamed[2] = chip.ZeroModShortFunc;
            out_renamed[3] = chip.ZeroModShortFunc;
            chtype = OplChannelType.TwoOp;
            cha = 0xffff;
            chb = 0xffff;
#if OPL_ENABLE_STEREOEXT
            leftpan = 0x10000;
            rightpan = 0x10000;
#endif
        }

        public Opl3_Slot [] slotz = new Opl3_Slot[2];/*Don't use "slots" keyword to avoid conflict with Qt applications*/
        public Opl3_Channel pair;
        public Opl3_Chip chip;
        public Func<short> [] out_renamed = new Func<short>[4];

#if OPL_ENABLE_STEREOEXT
        public int leftpan;
        public int rightpan;
#endif

        public OplChannelType chtype;
        public ushort f_num;
        public byte block;
        public byte fb;
        public byte con;
        public byte alg;
        public byte ksv;
        public ushort cha, chb;
        public ushort chc, chd;
        public byte ch_num;
    }
}

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
using System.Threading;

namespace Nuked_OPL3_Port
{
    public class Opl3
    {
        internal delegate short EnvelopeSinFunc(ushort phase, ushort envelope);

        const int RSM_FRAC = 10;

        static readonly ushort [] logsinrom = {
            0x859, 0x6c3, 0x607, 0x58b, 0x52e, 0x4e4, 0x4a6, 0x471,
            0x443, 0x41a, 0x3f5, 0x3d3, 0x3b5, 0x398, 0x37e, 0x365,
            0x34e, 0x339, 0x324, 0x311, 0x2ff, 0x2ed, 0x2dc, 0x2cd,
            0x2bd, 0x2af, 0x2a0, 0x293, 0x286, 0x279, 0x26d, 0x261,
            0x256, 0x24b, 0x240, 0x236, 0x22c, 0x222, 0x218, 0x20f,
            0x206, 0x1fd, 0x1f5, 0x1ec, 0x1e4, 0x1dc, 0x1d4, 0x1cd,
            0x1c5, 0x1be, 0x1b7, 0x1b0, 0x1a9, 0x1a2, 0x19b, 0x195,
            0x18f, 0x188, 0x182, 0x17c, 0x177, 0x171, 0x16b, 0x166,
            0x160, 0x15b, 0x155, 0x150, 0x14b, 0x146, 0x141, 0x13c,
            0x137, 0x133, 0x12e, 0x129, 0x125, 0x121, 0x11c, 0x118,
            0x114, 0x10f, 0x10b, 0x107, 0x103, 0x0ff, 0x0fb, 0x0f8,
            0x0f4, 0x0f0, 0x0ec, 0x0e9, 0x0e5, 0x0e2, 0x0de, 0x0db,
            0x0d7, 0x0d4, 0x0d1, 0x0cd, 0x0ca, 0x0c7, 0x0c4, 0x0c1,
            0x0be, 0x0bb, 0x0b8, 0x0b5, 0x0b2, 0x0af, 0x0ac, 0x0a9,
            0x0a7, 0x0a4, 0x0a1, 0x09f, 0x09c, 0x099, 0x097, 0x094,
            0x092, 0x08f, 0x08d, 0x08a, 0x088, 0x086, 0x083, 0x081,
            0x07f, 0x07d, 0x07a, 0x078, 0x076, 0x074, 0x072, 0x070,
            0x06e, 0x06c, 0x06a, 0x068, 0x066, 0x064, 0x062, 0x060,
            0x05e, 0x05c, 0x05b, 0x059, 0x057, 0x055, 0x053, 0x052,
            0x050, 0x04e, 0x04d, 0x04b, 0x04a, 0x048, 0x046, 0x045,
            0x043, 0x042, 0x040, 0x03f, 0x03e, 0x03c, 0x03b, 0x039,
            0x038, 0x037, 0x035, 0x034, 0x033, 0x031, 0x030, 0x02f,
            0x02e, 0x02d, 0x02b, 0x02a, 0x029, 0x028, 0x027, 0x026,
            0x025, 0x024, 0x023, 0x022, 0x021, 0x020, 0x01f, 0x01e,
            0x01d, 0x01c, 0x01b, 0x01a, 0x019, 0x018, 0x017, 0x017,
            0x016, 0x015, 0x014, 0x014, 0x013, 0x012, 0x011, 0x011,
            0x010, 0x00f, 0x00f, 0x00e, 0x00d, 0x00d, 0x00c, 0x00c,
            0x00b, 0x00a, 0x00a, 0x009, 0x009, 0x008, 0x008, 0x007,
            0x007, 0x007, 0x006, 0x006, 0x005, 0x005, 0x005, 0x004,
            0x004, 0x004, 0x003, 0x003, 0x003, 0x002, 0x002, 0x002,
            0x002, 0x001, 0x001, 0x001, 0x001, 0x001, 0x001, 0x001,
            0x000, 0x000, 0x000, 0x000, 0x000, 0x000, 0x000, 0x000
        };

        static readonly ushort [] exprom = {
            0x7fa, 0x7f5, 0x7ef, 0x7ea, 0x7e4, 0x7df, 0x7da, 0x7d4,
            0x7cf, 0x7c9, 0x7c4, 0x7bf, 0x7b9, 0x7b4, 0x7ae, 0x7a9,
            0x7a4, 0x79f, 0x799, 0x794, 0x78f, 0x78a, 0x784, 0x77f,
            0x77a, 0x775, 0x770, 0x76a, 0x765, 0x760, 0x75b, 0x756,
            0x751, 0x74c, 0x747, 0x742, 0x73d, 0x738, 0x733, 0x72e,
            0x729, 0x724, 0x71f, 0x71a, 0x715, 0x710, 0x70b, 0x706,
            0x702, 0x6fd, 0x6f8, 0x6f3, 0x6ee, 0x6e9, 0x6e5, 0x6e0,
            0x6db, 0x6d6, 0x6d2, 0x6cd, 0x6c8, 0x6c4, 0x6bf, 0x6ba,
            0x6b5, 0x6b1, 0x6ac, 0x6a8, 0x6a3, 0x69e, 0x69a, 0x695,
            0x691, 0x68c, 0x688, 0x683, 0x67f, 0x67a, 0x676, 0x671,
            0x66d, 0x668, 0x664, 0x65f, 0x65b, 0x657, 0x652, 0x64e,
            0x649, 0x645, 0x641, 0x63c, 0x638, 0x634, 0x630, 0x62b,
            0x627, 0x623, 0x61e, 0x61a, 0x616, 0x612, 0x60e, 0x609,
            0x605, 0x601, 0x5fd, 0x5f9, 0x5f5, 0x5f0, 0x5ec, 0x5e8,
            0x5e4, 0x5e0, 0x5dc, 0x5d8, 0x5d4, 0x5d0, 0x5cc, 0x5c8,
            0x5c4, 0x5c0, 0x5bc, 0x5b8, 0x5b4, 0x5b0, 0x5ac, 0x5a8,
            0x5a4, 0x5a0, 0x59c, 0x599, 0x595, 0x591, 0x58d, 0x589,
            0x585, 0x581, 0x57e, 0x57a, 0x576, 0x572, 0x56f, 0x56b,
            0x567, 0x563, 0x560, 0x55c, 0x558, 0x554, 0x551, 0x54d,
            0x549, 0x546, 0x542, 0x53e, 0x53b, 0x537, 0x534, 0x530,
            0x52c, 0x529, 0x525, 0x522, 0x51e, 0x51b, 0x517, 0x514,
            0x510, 0x50c, 0x509, 0x506, 0x502, 0x4ff, 0x4fb, 0x4f8,
            0x4f4, 0x4f1, 0x4ed, 0x4ea, 0x4e7, 0x4e3, 0x4e0, 0x4dc,
            0x4d9, 0x4d6, 0x4d2, 0x4cf, 0x4cc, 0x4c8, 0x4c5, 0x4c2,
            0x4be, 0x4bb, 0x4b8, 0x4b5, 0x4b1, 0x4ae, 0x4ab, 0x4a8,
            0x4a4, 0x4a1, 0x49e, 0x49b, 0x498, 0x494, 0x491, 0x48e,
            0x48b, 0x488, 0x485, 0x482, 0x47e, 0x47b, 0x478, 0x475,
            0x472, 0x46f, 0x46c, 0x469, 0x466, 0x463, 0x460, 0x45d,
            0x45a, 0x457, 0x454, 0x451, 0x44e, 0x44b, 0x448, 0x445,
            0x442, 0x43f, 0x43c, 0x439, 0x436, 0x433, 0x430, 0x42d,
            0x42a, 0x428, 0x425, 0x422, 0x41f, 0x41c, 0x419, 0x416,
            0x414, 0x411, 0x40e, 0x40b, 0x408, 0x406, 0x403, 0x400
        };

        static readonly byte [] mt = {
            1, 2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 20, 24, 24, 30, 30
        };

        static readonly byte [] kslrom = {
            0, 32, 40, 45, 48, 51, 53, 55, 56, 58, 59, 60, 61, 62, 63, 64
        };

        static readonly byte [] kslshift = {
            8, 1, 2, 0
        };

        static readonly byte [][] eg_incstep = {
            new byte [] { 0, 0, 0, 0 },
            new byte [] { 1, 0, 0, 0 },
            new byte [] { 1, 0, 1, 0 },
            new byte [] { 1, 1, 1, 0 }
        };

        static readonly sbyte [] ad_slot = {
            0, 1, 2, 3, 4, 5, -1, -1, 6, 7, 8, 9, 10, 11, -1, -1,
            12, 13, 14, 15, 16, 17, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1
        };

        static readonly byte [] ch_slot = {
            0, 1, 2, 6, 7, 8, 12, 13, 14, 18, 19, 20, 24, 25, 26, 30, 31, 32
        };

#if OPL_ENABLE_STEREOEXT
        /*
            stereo extension panning table
        */

        static int [] panpot_lut = new int[256];
        static bool panpot_lut_build = false;
#endif

        static int[] generate4ChMix = new int[2];
        static short[] generateSamples = new short[4];

        static short OPL3_EnvelopeCalcExp(uint level)
        {
            if (level > 0x1fff)
            {
                level = 0x1fff;
            }
            return (short)(((exprom[level & 0xffu] << 1)) >> (int)(level >> 8));
        }

        static short OPL3_EnvelopeCalcSin0(ushort phase, ushort envelope)
        {
            ushort neg = 0;
            phase &= 0x3ff;
            if (Convert.ToBoolean(phase & 0x200))
            {
                neg = 0xffff;
            }
            ushort out1;
            if (Convert.ToBoolean(phase & 0x100))
            {
                out1 = logsinrom[(phase & 0xffu) ^ 0xffu];
            }
            else
            {
                out1 = logsinrom[phase & 0xffu];
            }
            return (short)(OPL3_EnvelopeCalcExp((uint)(out1 + (envelope << 3))) ^ neg);
        }

        static short OPL3_EnvelopeCalcSin1(ushort phase, ushort envelope)
        {
            phase &= 0x3ff;
            ushort out1;
            if (Convert.ToBoolean(phase & 0x200))
            {
                out1 = 0x1000;
            }
            else if (Convert.ToBoolean(phase & 0x100))
            {
                out1 = logsinrom[(phase & 0xffu) ^ 0xffu];
            }
            else
            {
                out1 = logsinrom[phase & 0xffu];
            }
            return OPL3_EnvelopeCalcExp((uint)(out1 + (envelope << 3)));
        }

        static short OPL3_EnvelopeCalcSin2(ushort phase, ushort envelope)
        {
            phase &= 0x3ff;
            ushort out1;
            if (Convert.ToBoolean(phase & 0x100))
            {
                out1 = logsinrom[(phase & 0xffu) ^ 0xffu];
            }
            else
            {
                out1 = logsinrom[phase & 0xffu];
            }
            return OPL3_EnvelopeCalcExp((uint)(out1 + (envelope << 3)));
        }

        static short OPL3_EnvelopeCalcSin3(ushort phase, ushort envelope)
        {
            phase &= 0x3ff;
            ushort out1;
            if (Convert.ToBoolean(phase & 0x100))
            {
                out1 = 0x1000;
            }
            else
            {
                out1 = logsinrom[phase & 0xffu];
            }
            return OPL3_EnvelopeCalcExp((uint)(out1 + (envelope << 3)));
        }

        static short OPL3_EnvelopeCalcSin4(ushort phase, ushort envelope)
        {
            ushort neg = 0;
            phase &= 0x3ff;
            if ((phase & 0x300) == 0x100)
            {
                neg = 0xffff;
            }
            ushort out1;
            if (Convert.ToBoolean(phase & 0x200))
            {
                out1 = 0x1000;
            }
            else if (Convert.ToBoolean(phase & 0x80))
            {
                out1 = logsinrom[((phase ^ 0xff) << 1) & 0xffu];
            }
            else
            {
                out1 = logsinrom[(phase << 1) & 0xffu];
            }
            return (short)(OPL3_EnvelopeCalcExp((uint)(out1 + (envelope << 3))) ^ neg);
        }

        static short OPL3_EnvelopeCalcSin5(ushort phase, ushort envelope)
        {
            phase &= 0x3ff;
            ushort out1;
            if (Convert.ToBoolean(phase & 0x200))
            {
                out1 = 0x1000;
            }
            else if (Convert.ToBoolean(phase & 0x80))
            {
                out1 = logsinrom[((phase ^ 0xff) << 1) & 0xffu];
            }
            else
            {
                out1 = logsinrom[(phase << 1) & 0xffu];
            }
            return OPL3_EnvelopeCalcExp((uint)(out1 +(envelope << 3)));
        }

        static short OPL3_EnvelopeCalcSin6(ushort phase, ushort envelope)
        {
            ushort neg = 0;
            phase &= 0x3ff;
            if (Convert.ToBoolean(phase & 0x200))
            {
                neg = 0xffff;
            }
            return (short)(OPL3_EnvelopeCalcExp((uint)(envelope << 3)) ^ neg);
        }

        static short OPL3_EnvelopeCalcSin7(ushort phase, ushort envelope)
        {
            ushort neg = 0;
            phase &= 0x3ff;
            if (Convert.ToBoolean(phase & 0x200))
            {
                neg = 0xffff;
                phase = (ushort)((phase & 0x1ff) ^ 0x1ff);
            }
            var out1 = (ushort)(phase << 3);
            return (short)(OPL3_EnvelopeCalcExp((uint)(out1 + (envelope << 3))) ^ neg);
        }

        static readonly EnvelopeSinFunc[] envelope_sin = {
            OPL3_EnvelopeCalcSin0,
            OPL3_EnvelopeCalcSin1,
            OPL3_EnvelopeCalcSin2,
            OPL3_EnvelopeCalcSin3,
            OPL3_EnvelopeCalcSin4,
            OPL3_EnvelopeCalcSin5,
            OPL3_EnvelopeCalcSin6,
            OPL3_EnvelopeCalcSin7
        };

        static void OPL3_EnvelopeUpdateKSL(Opl3_Slot slot)
        {
            var ksl = (short)((kslrom[slot.channel.f_num >> 6] << 2) - ((0x08 - slot.channel.block) << 5));
            if (ksl < 0)
            {
                ksl = 0;
            }
            slot.eg_ksl = (byte)ksl;
        }

        static void OPL3_EnvelopeCalc(Opl3_Slot slot)
        {
            byte reg_rate = 0;
            var reset = false;

            slot.eg_out = (ushort)(slot.eg_rout + (slot.reg_tl << 2)
                         + (slot.eg_ksl >> kslshift[slot.reg_ksl]) + slot.trem());
            if (slot.key != 0 && slot.eg_gen == EnvelopeGenNum.Release)
            {
                reset = true;
                reg_rate = slot.reg_ar;
            }
            else
            {
                switch (slot.eg_gen)
                {
                    case EnvelopeGenNum.Attack:
                        reg_rate = slot.reg_ar;
                        break;
                    case EnvelopeGenNum.Decay:
                        reg_rate = slot.reg_dr;
                        break;
                    case EnvelopeGenNum.Sustain:
                        if (slot.reg_type == 0)
                        {
                            reg_rate = slot.reg_rr;
                        }
                        break;
                    case EnvelopeGenNum.Release:
                        reg_rate = slot.reg_rr;
                        break;
                }
            }
            slot.pg_reset = reset;
            var ks = (byte)(slot.channel.ksv >> ((slot.reg_ksr ^ 1) << 1));
            var nonzero = (reg_rate != 0);
            var rate = (byte)(ks + (reg_rate << 2));
            var rate_hi = (byte)(rate >> 2);
            var rate_lo = (byte)(rate & 0x03);
            if (Convert.ToBoolean(rate_hi & 0x10))
            {
                rate_hi = 0x0f;
            }
            var eg_shift = (byte)(rate_hi + slot.chip.eg_add);
            var shift = (byte)0;
            if (nonzero)
            {
                if (rate_hi < 12)
                {
                    if (slot.chip.eg_state)
                    {
                        switch (eg_shift)
                        {
                            case 12:
                                shift = 1;
                                break;
                            case 13:
                                shift = (byte)((rate_lo >> 1) & 0x01);
                                break;
                            case 14:
                                shift = (byte)(rate_lo & 0x01);
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    shift = (byte)((rate_hi & 0x03) + eg_incstep[rate_lo][slot.chip.eg_timer_lo]);
                    if (Convert.ToBoolean(shift & 0x04))
                    {
                        shift = 0x03;
                    }
                    if (shift == 0)
                    {
                        shift = slot.chip.eg_state ? (byte)1 : (byte)0;
                    }
                }
            }
            var eg_rout = slot.eg_rout;
            short eg_inc = 0;
            byte eg_off = 0;
            /* Instant attack */
            if (reset && rate_hi == 0x0f)
            {
                eg_rout = 0x00;
            }
            /* Envelope off */
            if ((slot.eg_rout & 0x1f8) == 0x1f8)
            {
                eg_off = 1;
            }
            if (slot.eg_gen != EnvelopeGenNum.Attack && !reset && eg_off != 0)
            {
                eg_rout = 0x1ff;
            }
            switch (slot.eg_gen)
            {
                case EnvelopeGenNum.Attack:
                    if (slot.eg_rout == 0)
                    {
                        slot.eg_gen = EnvelopeGenNum.Decay;
                    }
                    else if (slot.key != 0 && shift > 0 && rate_hi != 0x0f)
                    {
                        eg_inc = (short)(~slot.eg_rout >> (4 - shift));
                    }
                    break;
                case EnvelopeGenNum.Decay:
                    if ((slot.eg_rout >> 4) == slot.reg_sl)
                    {
                        slot.eg_gen = EnvelopeGenNum.Sustain;
                    }
                    else if (eg_off == 0 && !reset && shift > 0)
                    {
                        eg_inc = (short)(1 << (shift - 1));
                    }
                    break;
                case EnvelopeGenNum.Sustain:
                case EnvelopeGenNum.Release:
                    if (eg_off == 0 && !reset && shift > 0)
                    {
                        eg_inc = (short)(1 << (shift - 1));
                    }
                    break;
            }
            slot.eg_rout = (ushort)((eg_rout + eg_inc) & 0x1ff);
            /* Key off */
            if (reset)
            {
                slot.eg_gen = EnvelopeGenNum.Attack;
            }
            if (slot.key == 0)
            {
                slot.eg_gen = EnvelopeGenNum.Release;
            }
        }

        static void OPL3_EnvelopeKeyOn(Opl3_Slot slot, EnvelopeType type)
        {
            slot.key |= (byte)type;
        }

        static void OPL3_EnvelopeKeyOff(Opl3_Slot slot, EnvelopeType type)
        {
            slot.key &= (byte)~type;
        }

        /*
            Phase Generator
        */

        static void OPL3_PhaseGenerate(Opl3_Slot slot)
        {
            var chip = slot.chip;
            var f_num = slot.channel.f_num;
            if (slot.reg_vib != 0)
            {
                var range = (sbyte)((f_num >> 7) & 7);
                var vibpos = slot.chip.vibpos;

                if (!Convert.ToBoolean(vibpos & 3))
                {
                    range = 0;
                }
                else if (Convert.ToBoolean(vibpos & 1))
                {
                    range >>= 1;
                }
                range >>= slot.chip.vibshift;

                if (Convert.ToBoolean(vibpos & 4))
                {
                    range = (sbyte)-range;
                }
                f_num += (ushort)range;
            }
            var basefreq = (uint)((f_num << slot.channel.block) >> 1);
            var phase = (ushort)(slot.pg_phase >> 9);
            if (slot.pg_reset)
            {
                slot.pg_phase = 0;
            }
            slot.pg_phase += (basefreq * mt[slot.reg_mult]) >> 1;
            /* Rhythm mode */
            var noise = chip.noise;
            slot.pg_phase_out = phase;
            if (slot.slot_num == 13) /* hh */
            {
                chip.rm_hh_bit2 = (byte)((phase >> 2) & 1);
                chip.rm_hh_bit3 = (byte)((phase >> 3) & 1);
                chip.rm_hh_bit7 = (byte)((phase >> 7) & 1);
                chip.rm_hh_bit8 = (byte)((phase >> 8) & 1);
            }
            if (slot.slot_num == 17 && Convert.ToBoolean(chip.rhy & 0x20)) /* tc */
            {
                chip.rm_tc_bit3 = (byte)((phase >> 3) & 1);
                chip.rm_tc_bit5 = (byte)((phase >> 5) & 1);
            }
            if (Convert.ToBoolean(chip.rhy & 0x20))
            {
                var rm_xor = (byte)((chip.rm_hh_bit2 ^ chip.rm_hh_bit7)
                       | (chip.rm_hh_bit3 ^ chip.rm_tc_bit5)
                       | (chip.rm_tc_bit3 ^ chip.rm_tc_bit5));
                switch (slot.slot_num)
                {
                    case 13: /* hh */
                        slot.pg_phase_out = (ushort)(rm_xor << 9);
                        if ((rm_xor ^ (noise & 1)) != 0)
                        {
                            slot.pg_phase_out |= 0xd0;
                        }
                        else
                        {
                            slot.pg_phase_out |= 0x34;
                        }
                        break;
                    case 16: /* sd */
                        slot.pg_phase_out = (ushort)((chip.rm_hh_bit8 << 9)
                                           | ((chip.rm_hh_bit8 ^ ((ushort)noise & 1)) << 8));
                        break;
                    case 17: /* tc */
                        slot.pg_phase_out = (ushort)((rm_xor << 9) | 0x80);
                        break;
                    default:
                        break;
                }
            }
            var n_bit = (byte)(((noise >> 14) ^ noise) & 0x01);
            chip.noise = (noise >> 1) | ((uint)n_bit << 22);
        }

        /*
            Slot
        */

        static void OPL3_SlotWrite20(Opl3_Slot slot, byte data)
        {
            if (Convert.ToBoolean((data >> 7) & 0x01))
            {
                slot.trem = slot.chip.TremeloFunc;
            }
            else
            {
                slot.trem = slot.chip.ZeroModByteFunc;
            }
            slot.reg_vib = (byte)((data >> 6) & 0x01);
            slot.reg_type = (byte)((data >> 5) & 0x01);
            slot.reg_ksr = (byte)((data >> 4) & 0x01);
            slot.reg_mult = (byte)(data & 0x0f);
        }

        static void OPL3_SlotWrite40(Opl3_Slot slot, byte data)
        {
            slot.reg_ksl = (byte)((data >> 6) & 0x03);
            slot.reg_tl = (byte)(data & 0x3f);
            OPL3_EnvelopeUpdateKSL(slot);
        }

        static void OPL3_SlotWrite60(Opl3_Slot slot, byte data)
        {
            slot.reg_ar = (byte)((data >> 4) & 0x0f);
            slot.reg_dr = (byte)(data & 0x0f);
        }

        static void OPL3_SlotWrite80(Opl3_Slot slot, byte data)
        {
            slot.reg_sl = (byte)((data >> 4) & 0x0f);
            if (slot.reg_sl == 0x0f)
            {
                slot.reg_sl = 0x1f;
            }
            slot.reg_rr = (byte)(data & 0x0f);
        }

        static void OPL3_SlotWriteE0(Opl3_Slot slot, byte data)
        {
            slot.reg_wf = (byte)(data & 0x07);
            if (slot.chip.newm == 0x00)
            {
                slot.reg_wf &= 0x03;
            }
        }

        static void OPL3_SlotGenerate(Opl3_Slot slot)
        {
            slot.out_renamed = envelope_sin[slot.reg_wf]((ushort)(slot.pg_phase_out + slot.mod()), slot.eg_out);
        }

        static void OPL3_SlotCalcFB(Opl3_Slot slot)
        {
            if (slot.channel.fb != 0x00)
            {
                slot.fbmod = (short)((slot.prout + slot.out_renamed) >> (0x09 - slot.channel.fb));
            }
            else
            {
                slot.fbmod = 0;
            }
            slot.prout = slot.out_renamed;
        }

        /*
            Channel
        */

        static void OPL3_ChannelUpdateRhythm(Opl3_Chip chip, byte data)
        {
            chip.rhy = (byte)(data & 0x3f);
            if (Convert.ToBoolean(chip.rhy & 0x20))
            {
                var channel6 = chip.channel[6];
                var channel7 = chip.channel[7];
                var channel8 = chip.channel[8];

                channel6.out_renamed[0] = channel6.slotz[1].OutFunc;
                channel6.out_renamed[1] = channel6.slotz[1].OutFunc;
                channel6.out_renamed[2] = chip.ZeroModShortFunc;
                channel6.out_renamed[3] = chip.ZeroModShortFunc;

                channel7.out_renamed[0] = channel7.slotz[0].OutFunc;
                channel7.out_renamed[1] = channel7.slotz[0].OutFunc;
                channel7.out_renamed[2] = channel7.slotz[1].OutFunc;
                channel7.out_renamed[3] = channel7.slotz[1].OutFunc;

                channel8.out_renamed[0] = channel8.slotz[0].OutFunc;
                channel8.out_renamed[1] = channel8.slotz[0].OutFunc;
                channel8.out_renamed[2] = channel8.slotz[1].OutFunc;
                channel8.out_renamed[3] = channel8.slotz[1].OutFunc;

                for (var chnum = 6; chnum < 9; chnum++)
                {
                    chip.channel[chnum].chtype = OplChannelType.Drum;
                }
                OPL3_ChannelSetupAlg(channel6);
                OPL3_ChannelSetupAlg(channel7);
                OPL3_ChannelSetupAlg(channel8);
                /* hh */
                if (Convert.ToBoolean(chip.rhy & 0x01))
                {
                    OPL3_EnvelopeKeyOn(channel7.slotz[0], EnvelopeType.Drum);
                }
                else
                {
                    OPL3_EnvelopeKeyOff(channel7.slotz[0], EnvelopeType.Drum);
                }
                /* tc */
                if (Convert.ToBoolean(chip.rhy & 0x02))
                {
                    OPL3_EnvelopeKeyOn(channel8.slotz[1], EnvelopeType.Drum);
                }
                else
                {
                    OPL3_EnvelopeKeyOff(channel8.slotz[1], EnvelopeType.Drum);
                }
                /* tom */
                if (Convert.ToBoolean(chip.rhy & 0x04))
                {
                    OPL3_EnvelopeKeyOn(channel8.slotz[0], EnvelopeType.Drum);
                }
                else
                {
                    OPL3_EnvelopeKeyOff(channel8.slotz[0], EnvelopeType.Drum);
                }
                /* sd */
                if (Convert.ToBoolean(chip.rhy & 0x08))
                {
                    OPL3_EnvelopeKeyOn(channel7.slotz[1], EnvelopeType.Drum);
                }
                else
                {
                    OPL3_EnvelopeKeyOff(channel7.slotz[1], EnvelopeType.Drum);
                }
                /* bd */
                if (Convert.ToBoolean(chip.rhy & 0x10))
                {
                    OPL3_EnvelopeKeyOn(channel6.slotz[0], EnvelopeType.Drum);
                    OPL3_EnvelopeKeyOn(channel6.slotz[1], EnvelopeType.Drum);
                }
                else
                {
                    OPL3_EnvelopeKeyOff(channel6.slotz[0], EnvelopeType.Drum);
                    OPL3_EnvelopeKeyOff(channel6.slotz[1], EnvelopeType.Drum);
                }
            }
            else
            {
                for (var chnum = 6; chnum < 9; chnum++)
                {
                    chip.channel[chnum].chtype = OplChannelType.TwoOp;
                    OPL3_ChannelSetupAlg(chip.channel[chnum]);
                    OPL3_EnvelopeKeyOff(chip.channel[chnum].slotz[0], EnvelopeType.Drum);
                    OPL3_EnvelopeKeyOff(chip.channel[chnum].slotz[1], EnvelopeType.Drum);
                }
            }
        }

        static void OPL3_ChannelWriteA0(Opl3_Channel channel, byte data)
        {
            if (channel.chip.newm != 0 && channel.chtype == OplChannelType.FourOp2)
            {
                return;
            }
            channel.f_num = (ushort)((channel.f_num & 0x300) | data);
            channel.ksv = (byte)((channel.block << 1)
                         | ((channel.f_num >> (0x09 - channel.chip.nts)) & 0x01));
            OPL3_EnvelopeUpdateKSL(channel.slotz[0]);
            OPL3_EnvelopeUpdateKSL(channel.slotz[1]);
            if (channel.chip.newm != 0 && channel.chtype == OplChannelType.FourOp)
            {
                channel.pair.f_num = channel.f_num;
                channel.pair.ksv = channel.ksv;
                OPL3_EnvelopeUpdateKSL(channel.pair.slotz[0]);
                OPL3_EnvelopeUpdateKSL(channel.pair.slotz[1]);
            }
        }

        static void OPL3_ChannelWriteB0(Opl3_Channel channel, byte data)
        {
            if (channel.chip.newm != 0 && channel.chtype == OplChannelType.FourOp2)
            {
                return;
            }
            channel.f_num = (ushort)((channel.f_num & 0xff) | ((data & 0x03) << 8));
            channel.block = (byte)((data >> 2) & 0x07);
            channel.ksv = (byte)((channel.block << 1)
                         | ((channel.f_num >> (0x09 - channel.chip.nts)) & 0x01));
            OPL3_EnvelopeUpdateKSL(channel.slotz[0]);
            OPL3_EnvelopeUpdateKSL(channel.slotz[1]);
            if (channel.chip.newm != 0 && channel.chtype == OplChannelType.FourOp)
            {
                channel.pair.f_num = channel.f_num;
                channel.pair.block = channel.block;
                channel.pair.ksv = channel.ksv;
                OPL3_EnvelopeUpdateKSL(channel.pair.slotz[0]);
                OPL3_EnvelopeUpdateKSL(channel.pair.slotz[1]);
            }
        }

        static void OPL3_ChannelSetupAlg(Opl3_Channel channel)
        {
            if (channel.chtype == OplChannelType.Drum)
            {
                if (channel.ch_num == 7 || channel.ch_num == 8)
                {
                    channel.slotz[0].mod = channel.chip.ZeroModShortFunc;
                    channel.slotz[1].mod = channel.chip.ZeroModShortFunc;
                    return;
                }
                switch (channel.alg & 0x01)
                {
                    case 0x00:
                        channel.slotz[0].mod = channel.slotz[0].FbModFunc;
                        channel.slotz[1].mod = channel.slotz[0].OutFunc;
                        break;
                    case 0x01:
                        channel.slotz[0].mod = channel.slotz[0].FbModFunc;
                        channel.slotz[1].mod = channel.chip.ZeroModShortFunc;
                        break;
                }
                return;
            }
            if (Convert.ToBoolean(channel.alg & 0x08))
            {
                return;
            }
            if (Convert.ToBoolean(channel.alg & 0x04))
            {
                channel.pair.out_renamed[0] = channel.chip.ZeroModShortFunc;
                channel.pair.out_renamed[1] = channel.chip.ZeroModShortFunc;
                channel.pair.out_renamed[2] = channel.chip.ZeroModShortFunc;
                channel.pair.out_renamed[3] = channel.chip.ZeroModShortFunc;
                switch (channel.alg & 0x03)
                {
                    case 0x00:
                        channel.pair.slotz[0].mod = channel.pair.slotz[0].FbModFunc;
                        channel.pair.slotz[1].mod = channel.pair.slotz[0].OutFunc;
                        channel.slotz[0].mod = channel.pair.slotz[1].OutFunc;
                        channel.slotz[1].mod = channel.slotz[0].OutFunc;
                        channel.out_renamed[0] = channel.slotz[1].OutFunc;
                        channel.out_renamed[1] = channel.chip.ZeroModShortFunc;
                        channel.out_renamed[2] = channel.chip.ZeroModShortFunc;
                        channel.out_renamed[3] = channel.chip.ZeroModShortFunc;
                        break;
                    case 0x01:
                        channel.pair.slotz[0].mod = channel.pair.slotz[0].FbModFunc;
                        channel.pair.slotz[1].mod = channel.pair.slotz[0].OutFunc;
                        channel.slotz[0].mod = channel.chip.ZeroModShortFunc;
                        channel.slotz[1].mod = channel.slotz[0].OutFunc;
                        channel.out_renamed[0] = channel.pair.slotz[1].OutFunc;
                        channel.out_renamed[1] = channel.slotz[1].OutFunc;
                        channel.out_renamed[2] = channel.chip.ZeroModShortFunc;
                        channel.out_renamed[3] = channel.chip.ZeroModShortFunc;
                        break;
                    case 0x02:
                        channel.pair.slotz[0].mod = channel.pair.slotz[0].FbModFunc;
                        channel.pair.slotz[1].mod = channel.chip.ZeroModShortFunc;
                        channel.slotz[0].mod = channel.pair.slotz[1].OutFunc;
                        channel.slotz[1].mod = channel.slotz[0].OutFunc;
                        channel.out_renamed[0] = channel.pair.slotz[0].OutFunc;
                        channel.out_renamed[1] = channel.slotz[1].OutFunc;
                        channel.out_renamed[2] = channel.chip.ZeroModShortFunc;
                        channel.out_renamed[3] = channel.chip.ZeroModShortFunc;
                        break;
                    case 0x03:
                        channel.pair.slotz[0].mod = channel.pair.slotz[0].FbModFunc;
                        channel.pair.slotz[1].mod = channel.chip.ZeroModShortFunc;
                        channel.slotz[0].mod = channel.pair.slotz[1].OutFunc;
                        channel.slotz[1].mod = channel.chip.ZeroModShortFunc;
                        channel.out_renamed[0] = channel.pair.slotz[0].OutFunc;
                        channel.out_renamed[1] = channel.slotz[0].OutFunc;
                        channel.out_renamed[2] = channel.slotz[1].OutFunc;
                        channel.out_renamed[3] = channel.chip.ZeroModShortFunc;
                        break;
                }
            }
            else
            {
                switch (channel.alg & 0x01)
                {
                    case 0x00:
                        channel.slotz[0].mod = channel.slotz[0].FbModFunc;
                        channel.slotz[1].mod = channel.slotz[0].OutFunc;
                        channel.out_renamed[0] = channel.slotz[1].OutFunc;
                        channel.out_renamed[1] = channel.chip.ZeroModShortFunc;
                        channel.out_renamed[2] = channel.chip.ZeroModShortFunc;
                        channel.out_renamed[3] = channel.chip.ZeroModShortFunc;
                        break;
                    case 0x01:
                        channel.slotz[0].mod = channel.slotz[0].FbModFunc;
                        channel.slotz[1].mod = channel.chip.ZeroModShortFunc;
                        channel.out_renamed[0] = channel.slotz[0].OutFunc;
                        channel.out_renamed[1] = channel.slotz[1].OutFunc;
                        channel.out_renamed[2] = channel.chip.ZeroModShortFunc;
                        channel.out_renamed[3] = channel.chip.ZeroModShortFunc;
                        break;
                }
            }
        }

        static void OPL3_ChannelUpdateAlg(Opl3_Channel channel)
        {
            channel.alg = channel.con;
            if (channel.chip.newm != 0)
            {
                if (channel.chtype == OplChannelType.FourOp)
                {
                    channel.pair.alg = (byte)(0x04 | (channel.con << 1) | channel.pair.con);
                    channel.alg = 0x08;
                    OPL3_ChannelSetupAlg(channel.pair);
                }
                else if (channel.chtype == OplChannelType.FourOp2)
                {
                    channel.alg = (byte)(0x04 | (channel.pair.con << 1) | channel.con);
                    channel.pair.alg = 0x08;
                    OPL3_ChannelSetupAlg(channel);
                }
                else
                {
                    OPL3_ChannelSetupAlg(channel);
                }
            }
            else
            {
                OPL3_ChannelSetupAlg(channel);
            }
        }

        static void OPL3_ChannelWriteC0(Opl3_Channel channel, byte data)
        {
            channel.fb = (byte)((data & 0x0e) >> 1);
            channel.con = (byte)(data & 0x01);
            OPL3_ChannelUpdateAlg(channel);
            if (channel.chip.newm != 0)
            {
                channel.cha = Convert.ToBoolean((data >> 4) & 0x01) ? (ushort)0xFFFF : (ushort)0;
                channel.chb = Convert.ToBoolean((data >> 5) & 0x01) ? (ushort)0xFFFF : (ushort)0;
                channel.chc = Convert.ToBoolean((data >> 6) & 0x01) ? (ushort)0xFFFF : (ushort)0;
                channel.chd = Convert.ToBoolean((data >> 7) & 0x01) ? (ushort)0xFFFF : (ushort)0;
            }
            else
            {
                channel.cha = channel.chb = (ushort)0xFFFF;
                // TODO: Verify on real chip if DAC2 output is disabled in compat mode
                channel.chc = channel.chd = 0;
            }
#if OPL_ENABLE_STEREOEXT
            if (channel.chip.stereoext == 0)
            {
                channel.leftpan = channel.cha << 16;
                channel.rightpan = channel.chb << 16;
            }
#endif
        }

#if OPL_ENABLE_STEREOEXT
        static void OPL3_ChannelWriteD0(Opl3_Channel channel, byte data)
        {
            if (channel.chip.stereoext != 0)
            {
                channel.leftpan = panpot_lut[data ^ 0xffu];
                channel.rightpan = panpot_lut[data];
            }
        }
#endif

        static void OPL3_ChannelKeyOn(Opl3_Channel channel)
        {
            if (channel.chip.newm != 0)
            {
                if (channel.chtype == OplChannelType.FourOp)
                {
                    OPL3_EnvelopeKeyOn(channel.slotz[0], EnvelopeType.Normal);
                    OPL3_EnvelopeKeyOn(channel.slotz[1], EnvelopeType.Normal);
                    OPL3_EnvelopeKeyOn(channel.pair.slotz[0], EnvelopeType.Normal);
                    OPL3_EnvelopeKeyOn(channel.pair.slotz[1], EnvelopeType.Normal);
                }
                else if (channel.chtype == OplChannelType.TwoOp || channel.chtype == OplChannelType.Drum)
                {
                    OPL3_EnvelopeKeyOn(channel.slotz[0], EnvelopeType.Normal);
                    OPL3_EnvelopeKeyOn(channel.slotz[1], EnvelopeType.Normal);
                }
            }
            else
            {
                OPL3_EnvelopeKeyOn(channel.slotz[0], EnvelopeType.Normal);
                OPL3_EnvelopeKeyOn(channel.slotz[1], EnvelopeType.Normal);
            }
        }

        static void OPL3_ChannelKeyOff(Opl3_Channel channel)
        {
            if (channel.chip.newm != 0)
            {
                if (channel.chtype == OplChannelType.FourOp)
                {
                    OPL3_EnvelopeKeyOff(channel.slotz[0], EnvelopeType.Normal);
                    OPL3_EnvelopeKeyOff(channel.slotz[1], EnvelopeType.Normal);
                    OPL3_EnvelopeKeyOff(channel.pair.slotz[0], EnvelopeType.Normal);
                    OPL3_EnvelopeKeyOff(channel.pair.slotz[1], EnvelopeType.Normal);
                }
                else if (channel.chtype == OplChannelType.TwoOp || channel.chtype == OplChannelType.Drum)
                {
                    OPL3_EnvelopeKeyOff(channel.slotz[0], EnvelopeType.Normal);
                    OPL3_EnvelopeKeyOff(channel.slotz[1], EnvelopeType.Normal);
                }
            }
            else
            {
                OPL3_EnvelopeKeyOff(channel.slotz[0], EnvelopeType.Normal);
                OPL3_EnvelopeKeyOff(channel.slotz[1], EnvelopeType.Normal);
            }
        }

        static void OPL3_ChannelSet4Op(Opl3_Chip chip, byte data)
        {
            for (var bit = 0; bit < 6; bit++)
            {
                var chnum = bit;
                if (bit >= 3)
                {
                    chnum += 9 - 3;
                }
                if (Convert.ToBoolean((data >> bit) & 0x01))
                {
                    chip.channel[chnum].chtype = OplChannelType.FourOp;
                    chip.channel[chnum + 3u].chtype = OplChannelType.FourOp2;
                    OPL3_ChannelUpdateAlg(chip.channel[chnum]);
                }
                else
                {
                    chip.channel[chnum].chtype = OplChannelType.TwoOp;
                    chip.channel[chnum + 3u].chtype = OplChannelType.TwoOp;
                    OPL3_ChannelUpdateAlg(chip.channel[chnum]);
                    OPL3_ChannelUpdateAlg(chip.channel[chnum + 3u]);
                }
            }
        }

        static short OPL3_ClipSample(int sample)
        {
            if (sample > short.MaxValue)
            {
                sample = short.MaxValue;
            }
            else if (sample < short.MinValue)
            {
                sample = short.MinValue;
            }
            return (short)sample;
        }

        static void OPL3_ProcessSlot(Opl3_Slot slot)
        {
            OPL3_SlotCalcFB(slot);
            OPL3_EnvelopeCalc(slot);
            OPL3_PhaseGenerate(slot);
            OPL3_SlotGenerate(slot);
        }

        public static void OPL3_Generate4Ch(Opl3_Chip chip, Span<short> buf4)
        {
            buf4[1] = OPL3_ClipSample(chip.mixbuff[1]);
            buf4[3] = OPL3_ClipSample(chip.mixbuff[3]);

#if OPL_QUIRK_CHANNELSAMPLEDELAY
            for (var ii = 0; ii < 15; ii++)
#else
            for (var ii = 0; ii < 36; ii++)
#endif
            {
                OPL3_ProcessSlot(chip.slot[ii]);
            }

            generate4ChMix[0] = 0;
            generate4ChMix[1] = 0;

            for (var ii = 0; ii < 18; ii++)
            {
                var channel = chip.channel[ii];
                var out_renamed = channel.out_renamed;
                var accm = (short)(out_renamed[0]() + out_renamed[1]() + out_renamed[2]() + out_renamed[3]());
#if OPL_ENABLE_STEREOEXT
                generate4ChMix[0] += (short)((accm * channel.leftpan) >> 16);
#else
                generate4ChMix[0] += (short)(accm & channel.cha);
#endif
                generate4ChMix[1] += (short)(accm & channel.chc);
            }
            chip.mixbuff[0] = generate4ChMix[0];
            chip.mixbuff[2] = generate4ChMix[1];

#if OPL_QUIRK_CHANNELSAMPLEDELAY
            for (var ii = 15; ii < 18; ii++)
            {
                OPL3_ProcessSlot(chip.slot[ii]);
            }
#endif

            buf4[0] = OPL3_ClipSample(chip.mixbuff[0]);
            buf4[2] = OPL3_ClipSample(chip.mixbuff[2]);

#if OPL_QUIRK_CHANNELSAMPLEDELAY
            for (var ii = 18; ii < 33; ii++)
            {
                OPL3_ProcessSlot(chip.slot[ii]);
            }
#endif

            generate4ChMix[0] = generate4ChMix[1] = 0;
            for (var ii = 0; ii < 18; ii++)
            {
                var channel = chip.channel[ii];
                var out_renamed = channel.out_renamed;
                var accm = (short)(out_renamed[0]() + out_renamed[1]() + out_renamed[2]() + out_renamed[3]());
#if OPL_ENABLE_STEREOEXT
                generate4ChMix[0] += (short)((accm * channel.rightpan) >> 16);
#else
                generate4ChMix[0] += (short)(accm & channel.chb);
#endif
                generate4ChMix[1] += (short)(accm & channel.chd);
            }
            chip.mixbuff[1] = generate4ChMix[0];
            chip.mixbuff[3] = generate4ChMix[1];

#if OPL_QUIRK_CHANNELSAMPLEDELAY
            for (var ii = 33; ii < 36; ii++)
            {
                OPL3_ProcessSlot(chip.slot[ii]);
            }
#endif

            if ((chip.timer & 0x3f) == 0x3f)
            {
                chip.tremolopos = (byte)((chip.tremolopos + 1) % 210);
            }
            if (chip.tremolopos < 105)
            {
                chip.tremolo = (byte)(chip.tremolopos >> chip.tremoloshift);
            }
            else
            {
                chip.tremolo = (byte)((210 - chip.tremolopos) >> chip.tremoloshift);
            }

            if ((chip.timer & 0x3ff) == 0x3ff)
            {
                chip.vibpos = (byte)((chip.vibpos + 1) & 7);
            }

            chip.timer++;

            if (chip.eg_state)
            {
                var shift = 0;
                while (shift < 13 && ((chip.eg_timer >> shift) & 1) == 0)
                {
                    shift++;
                }
                if (shift > 12)
                {
                    chip.eg_add = 0;
                }
                else
                {
                    chip.eg_add = (byte)(shift + 1);
                }
                chip.eg_timer_lo = (byte)(chip.eg_timer & 0x3u);
            }

            if (chip.eg_timerrem || chip.eg_state)
            {
                if (chip.eg_timer == 0xfffffffff)
                {
                    chip.eg_timer = 0;
                    chip.eg_timerrem = true;
                }
                else
                {
                    chip.eg_timer++;
                    chip.eg_timerrem = false;
                }
            }

            chip.eg_state = !chip.eg_state;

            // Enclose our ref variable inside this scope so it doesn't leak anywhere
            {
                ref Opl3_Writebuf writebuf = ref chip.writebuf[chip.writebuf_cur];

                while (writebuf.time <= chip.writebuf_samplecnt)
                {
                    if ((writebuf.reg & 0x200) == 0)
                    {
                        break;
                    }
                    writebuf.reg &= 0x1ff;
                    OPL3_WriteReg(chip, writebuf.reg, writebuf.data);
                    chip.writebuf_cur = (chip.writebuf_cur + 1) % Opl3_Writebuf.OplWriteBufSize;
                    writebuf = ref chip.writebuf[chip.writebuf_cur];
                }
                chip.writebuf_samplecnt++;
            }
        }

        public static void OPL3_Generate(Opl3_Chip chip, Span<short> buf)
        {
            OPL3_Generate4Ch(chip, generateSamples);
            buf[0] = generateSamples[0];
            buf[1] = generateSamples[1];
        }

        public static void OPL3_Generate4ChResampled(Opl3_Chip chip, Span<short> buf4)
        {
            while (chip.samplecnt >= chip.rateratio)
            {
                chip.oldsamples[0] = chip.samples[0];
                chip.oldsamples[1] = chip.samples[1];
                chip.oldsamples[2] = chip.samples[2];
                chip.oldsamples[3] = chip.samples[3];
                OPL3_Generate4Ch(chip, chip.samples);
                chip.samplecnt -= chip.rateratio;
            }
            buf4[0] = (short)((chip.oldsamples[0] * (chip.rateratio - chip.samplecnt)
                                + chip.samples[0] * chip.samplecnt) / chip.rateratio);
            buf4[1] = (short)((chip.oldsamples[1] * (chip.rateratio - chip.samplecnt)
                                + chip.samples[1] * chip.samplecnt) / chip.rateratio);
            buf4[2] = (short)((chip.oldsamples[2] * (chip.rateratio - chip.samplecnt)
                                + chip.samples[2] * chip.samplecnt) / chip.rateratio);
            buf4[3] = (short)((chip.oldsamples[3] * (chip.rateratio - chip.samplecnt)
                                + chip.samples[3] * chip.samplecnt) / chip.rateratio);
            chip.samplecnt += 1 << RSM_FRAC;
        }

        public static void OPL3_GenerateResampled(Opl3_Chip chip, Span<short> buf)
        {
            OPL3_Generate4ChResampled(chip, generateSamples);
            buf[0] = generateSamples[0];
            buf[1] = generateSamples[1];
        }

        public static void OPL3_Reset(Opl3_Chip chip, uint samplerate)
        {
            chip.Reset();

            for (var slotnum = 0; slotnum < 36; slotnum++)
            {
                var slot = chip.slot[slotnum];
                slot.slot_num = (byte)slotnum;
            }
            for (var channum = 0; channum < 18; channum++)
            {
                var channel = chip.channel[channum];
                var local_ch_slot = ch_slot[channum];
                channel.slotz[0] = chip.slot[local_ch_slot];
                channel.slotz[1] = chip.slot[local_ch_slot + 3u];
                chip.slot[local_ch_slot].channel = channel;
                chip.slot[local_ch_slot + 3u].channel = channel;
                if ((channum % 9) < 3)
                {
                    channel.pair = chip.channel[channum + 3u];
                }
                else if ((channum % 9) < 6)
                {
                    channel.pair = chip.channel[channum - 3u];
                }
                channel.ch_num = (byte)channum;
                OPL3_ChannelSetupAlg(channel);
            }
            chip.noise = 1;
            chip.rateratio = (int)((samplerate << RSM_FRAC) / 49716);
            chip.tremoloshift = 4;
            chip.vibshift = 1;

#if OPL_ENABLE_STEREOEXT
            if (!panpot_lut_build)
            {
                for (var i = 0; i < 256; i++)
                {
                    panpot_lut[i] = (int)(Math.Sin(i * Math.PI / 512) * 65536.0);
                }
                panpot_lut_build = true;
            }
#endif
        }

        public static void OPL3_WriteReg(Opl3_Chip chip, ushort reg, byte v)
        {
            var high = (byte)((reg >> 8) & 0x01);
            var regm = (byte)(reg & 0xff);
            switch (regm & 0xf0)
            {
                case 0x00:
                    if (high != 0)
                    {
                        switch (regm & 0x0f)
                        {
                            case 0x04:
                                OPL3_ChannelSet4Op(chip, v);
                                break;
                            case 0x05:
                                chip.newm = (byte)(v & 0x01);
#if OPL_ENABLE_STEREOEXT
                                chip.stereoext = (byte)((v >> 1) & 0x01);
#endif
                                break;
                        }
                    }
                    else
                    {
                        switch (regm & 0x0f)
                        {
                            case 0x08:
                                chip.nts = (byte)((v >> 6) & 0x01);
                                break;
                        }
                    }
                    break;
                case 0x20:
                case 0x30:
                    if (ad_slot[regm & 0x1fu] >= 0)
                    {
                        OPL3_SlotWrite20(chip.slot[18u * high + ad_slot[regm & 0x1fu]], v);
                    }
                    break;
                case 0x40:
                case 0x50:
                    if (ad_slot[regm & 0x1fu] >= 0)
                    {
                        OPL3_SlotWrite40(chip.slot[18u * high + ad_slot[regm & 0x1fu]], v);
                    }
                    break;
                case 0x60:
                case 0x70:
                    if (ad_slot[regm & 0x1fu] >= 0)
                    {
                        OPL3_SlotWrite60(chip.slot[18u * high + ad_slot[regm & 0x1fu]], v);
                    }
                    break;
                case 0x80:
                case 0x90:
                    if (ad_slot[regm & 0x1fu] >= 0)
                    {
                        OPL3_SlotWrite80(chip.slot[18u * high + ad_slot[regm & 0x1fu]], v);
                    }
                    break;
                case 0xe0:
                case 0xf0:
                    if (ad_slot[regm & 0x1fu] >= 0)
                    {
                        OPL3_SlotWriteE0(chip.slot[18u * high + ad_slot[regm & 0x1fu]], v);
                    }
                    break;
                case 0xa0:
                    if ((regm & 0x0f) < 9)
                    {
                        OPL3_ChannelWriteA0(chip.channel[9u * high + (regm & 0x0fu)], v);
                    }
                    break;
                case 0xb0:
                    if (regm == 0xbd && high == 0)
                    {
                        chip.tremoloshift = (byte)((((v >> 7) ^ 1) << 1) + 2);
                        chip.vibshift = (byte)(((v >> 6) & 0x01) ^ 1);
                        OPL3_ChannelUpdateRhythm(chip, v);
                    }
                    else if ((regm & 0x0f) < 9)
                    {
                        OPL3_ChannelWriteB0(chip.channel[9u * high + (regm & 0x0fu)], v);
                        if (Convert.ToBoolean(v & 0x20))
                        {
                            OPL3_ChannelKeyOn(chip.channel[9u * high + (regm & 0x0fu)]);
                        }
                        else
                        {
                            OPL3_ChannelKeyOff(chip.channel[9u * high + (regm & 0x0fu)]);
                        }
                    }
                    break;
                case 0xc0:
                    if ((regm & 0x0f) < 9)
                    {
                        OPL3_ChannelWriteC0(chip.channel[9u * high + (regm & 0x0fu)], v);
                    }
                    break;
#if OPL_ENABLE_STEREOEXT
                case 0xd0:
                    if ((regm & 0x0f) < 9)
                    {
                        OPL3_ChannelWriteD0(chip.channel[9u * high + (regm & 0x0fu)], v);
                    }
                    break;
#endif
            }
        }

        public static void OPL3_WriteRegBuffered(Opl3_Chip chip, ushort reg, byte v)
        {
            var writebuf_last = chip.writebuf_last;
            ref Opl3_Writebuf writebuf = ref chip.writebuf[writebuf_last];

            if (Convert.ToBoolean(writebuf.reg & 0x200))
            {
                OPL3_WriteReg(chip, (ushort)(writebuf.reg & 0x1ff), writebuf.data);

                chip.writebuf_cur = (writebuf_last + 1) % Opl3_Writebuf.OplWriteBufSize;
                chip.writebuf_samplecnt = writebuf.time;
            }

            writebuf.reg = (ushort)(reg | 0x200);
            writebuf.data = v;
            var time1 = chip.writebuf_lasttime + Opl3_Writebuf.OplWriteBufDelay;
            var time2 = chip.writebuf_samplecnt;

            if (time1 < time2)
            {
                time1 = time2;
            }

            writebuf.time = time1;
            chip.writebuf_lasttime = time1;
            chip.writebuf_last = (writebuf_last + 1) % Opl3_Writebuf.OplWriteBufSize;
        }

        public static void OPL3_Generate4ChStream(Opl3_Chip chip, Span<short> sndptr1, Span<short> sndptr2, uint numsamples)
        {
            for (var i = 0; i < numsamples; i++)
            {
                var slice1 = sndptr1.Slice(2 * i, 2);
                var slice2 = sndptr2.Slice(2 * i, 2);
                OPL3_Generate4ChResampled(chip, generateSamples);

                slice1[0] = generateSamples[0];
                slice1[1] = generateSamples[1];
                slice2[0] = generateSamples[2];
                slice2[1] = generateSamples[3];
            }
        }

        public static void OPL3_GenerateStream(Opl3_Chip chip, Span<short> sndptr, uint numsamples)
        {
            for (var i = 0; i < numsamples; i++)
            {
                OPL3_GenerateResampled(chip, sndptr.Slice(i * 2, 2));
            }
        }
    }
}

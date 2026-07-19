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

namespace Nuked_OPL3_Port
{
    public class Opl3_Chip
    {
        public Opl3_Chip()
        {
            Reset();
        }

        internal void Reset()
        {
            for (var i = 0; i < channel.Length; i++)
            {
                channel[i] = new Opl3_Channel(this);
            }

            for (var i = 0; i < slot.Length; i++)
            {
                slot[i] = new Opl3_Slot(this);
            }

            timer = default;
            eg_timer = default;
            eg_timerrem = default;
            eg_state = default;
            eg_add = default;
            eg_timer_lo = default;
            newm = default;
            nts = default;
            rhy = default;
            vibpos = default;
            vibshift = default;
            tremolo = default;
            tremolopos = default;
            tremoloshift = default;
            noise = default;
            zeromod = default;
            for (var i = 0; i < mixbuff.Length; i++)
            {
                mixbuff[i] = default;
            }
        
            rm_hh_bit2 = default;
            rm_hh_bit3 = default;
            rm_hh_bit7 = default;
            rm_hh_bit8 = default;
            rm_tc_bit3 = default;
            rm_tc_bit5 = default;

#if OPL_ENABLE_STEREOEXT
            stereoext = default;
#endif

            /* OPL3L */
            rateratio = default;
            samplecnt = default;
            for (var i = 0; i < oldsamples.Length; i++)
            {
                oldsamples[i] = default;
            }
            for (var i = 0; i < samples.Length; i++)
            {
                samples[i] = default;
            }

            writebuf_samplecnt = default;
            writebuf_cur = default;
            writebuf_last = default;
            writebuf_lasttime = default;
        }

        internal Opl3_Channel [] channel = new Opl3_Channel[18];
        internal Opl3_Slot [] slot = new Opl3_Slot[36];
        internal ushort timer;
        internal ulong eg_timer;
        internal bool eg_timerrem;
        internal bool eg_state;
        internal byte eg_add;
        internal byte eg_timer_lo;
        internal byte newm;
        internal byte nts;
        internal byte rhy;
        internal byte vibpos;
        internal byte vibshift;
        internal byte tremolo;
        internal byte tremolopos;
        internal byte tremoloshift;
        internal uint noise;
        internal short zeromod;
        internal int [] mixbuff = new int[4];
        internal byte rm_hh_bit2;
        internal byte rm_hh_bit3;
        internal byte rm_hh_bit7;
        internal byte rm_hh_bit8;
        internal byte rm_tc_bit3;
        internal byte rm_tc_bit5;

#if OPL_ENABLE_STEREOEXT
        internal byte stereoext;
#endif

        /* OPL3L */
        internal int rateratio;
        internal int samplecnt;
        internal short [] oldsamples = new short[4];
        internal short [] samples = new short[4];

        internal ulong writebuf_samplecnt;
        internal uint writebuf_cur;
        internal uint writebuf_last;
        internal ulong writebuf_lasttime;
        internal Opl3_Writebuf [] writebuf = new Opl3_Writebuf[Opl3_Writebuf.OplWriteBufSize];

        internal byte TremeloFunc() => tremolo;
        internal byte ZeroModByteFunc() => (byte)zeromod;
        internal short ZeroModShortFunc() => zeromod;
    }
}

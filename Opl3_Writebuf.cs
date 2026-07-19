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
    internal struct Opl3_Writebuf
    {
        public ulong time;
        public ushort reg;
        public byte data;

        public const int OplWriteBufSize = 1024;
        public const int OplWriteBufDelay = 2;
    }
}

/* Iker Ruiz Arnauda 2012
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections;
using IKLogonServer.Helpers;

namespace IKLogonServer.Extensions
{
    public static class BinaryWriterExtensions
    {
        public static int WritePackedUInt64(this BinaryWriter binWriter, ulong number)
        {
            var buffer = BitConverter.GetBytes(number);

            byte mask = 0;
            var startPos = binWriter.BaseStream.Position;

            binWriter.Write(mask);

            for (var i = 0; i < 8; i++)
            {
                if (buffer[i] != 0)
                {
                    mask |= (byte)(1 << i);
                    binWriter.Write(buffer[i]);
                }
            }

            var endPos = binWriter.BaseStream.Position;

            binWriter.BaseStream.Position = startPos;
            binWriter.Write(mask);
            binWriter.BaseStream.Position = endPos;

            return (int)(endPos - startPos);
        }

        public static void WriteCString(this BinaryWriter writer, string input)
        {
            byte[] data = Encoding.UTF8.GetBytes(input + '\0');
            writer.Write(data);
        }

        public static void WriteBytes(this BinaryWriter writer, byte[] data, int count = 0)
        {
            if (count == 0)
                writer.Write(data);
            else
                writer.Write(data, 0, count);
        }

        public static void WriteBitArray(this BinaryWriter writer, BitArray buffer, int Len)
        {
            byte[] bufferarray = new byte[Convert.ToByte((buffer.Length + 8) / 8) + 1];
            buffer.CopyTo(bufferarray, 0);

            WriteBytes(writer, bufferarray.ToArray(), Len);
        }

        public static void WriteNullByte(this BinaryWriter writer, uint count)
        {
            for (uint i = 0; i < count; i++)
            {
                writer.Write((byte)0);
            }
        }

        public static void WriteNullUInt(this BinaryWriter writer, uint count)
        {
            for (uint i = 0; i < count; i++)
            {
                writer.Write((uint)0);
            }
        }

        public static void WriteHexPacket(this BinaryWriter writer, string data)
        {
            WriteBytes(writer, Helper.HexToByteArray(data));
        }
    }
}

namespace BlockParser;
public static class Extensions {
    public static T[] SubArray<T>(this T[] array, int offset, int length) {
        T[] result = new T[length];
        Array.Copy(array, offset, result, 0, length);
        return result;
    }
    public static long ReadVarIntOut(this BinaryReader reader, out List<byte> tmp) {
        tmp = new List<byte>();
        var t = reader.ReadByte();
        tmp.Add(t);
        if(t < 0xfd) {
            return t;
        }
        if(t == 0xfd) {
            var tmp2 = new List<byte>(reader.ReadBytes(2));
            tmp.AddRange(tmp2);
            return BitConverter.ToInt16(tmp2.ToArray());
        }
        if(t == 0xfe) {
            var tmp2 = new List<byte>(reader.ReadBytes(4));
            tmp.AddRange(tmp2);
            return BitConverter.ToInt32(tmp2.ToArray());

        }
        if(t == 0xff) {
            var tmp2 = new List<byte>(reader.ReadBytes(8));
            tmp.AddRange(tmp2);
            return BitConverter.ToInt64(tmp2.ToArray());
        }
        throw new InvalidDataException("Reading Var Int");
    }

    public static long ReadVarInt(this BinaryReader reader) {
        var t = reader.ReadByte();
        if(t < 0xfd) return t;
        if(t == 0xfd) return reader.ReadInt16();
        if(t == 0xfe) return reader.ReadInt32();
        if(t == 0xff) return reader.ReadInt64();
        throw new InvalidDataException("Reading Var Int");
    }
}



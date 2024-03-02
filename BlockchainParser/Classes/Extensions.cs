namespace BlockParser;
public static class Extensions {
    public static T[] SubArray<T>(this T[] array, int offset, int length) {
        T[] result = new T[length];
        Array.Copy(array, offset, result, 0, length);
        return result;
    }
    public static long ReadVarIntOut(this BinaryReader reader, List<byte> tmp) {
        tmp.Clear();
        var t = reader.ReadByte();
        if (t < 0xfd) {
            tmp.Add(t);
            return t;
        }
        if (t == 0xfd) {
            tmp = new List<byte>(reader.ReadBytes(2));
            return BitConverter.ToInt16(tmp.ToArray());
        }
        if (t == 0xfe) {
            tmp = new List<byte>(reader.ReadBytes(4));
            return BitConverter.ToInt32(tmp.ToArray());

        }
        if (t == 0xff) {
            tmp = new List<byte>(reader.ReadBytes(8));
            return BitConverter.ToInt64(tmp.ToArray());
        }
        throw new InvalidDataException("Reading Var Int");
    }

    public static long ReadVarInt(this BinaryReader reader) {
        var t = reader.ReadByte();
        if (t < 0xfd) return t;
        if (t == 0xfd) return reader.ReadInt16();
        if (t == 0xfe) return reader.ReadInt32();
        if (t == 0xff) return reader.ReadInt64();
        throw new InvalidDataException("Reading Var Int");
    }
}



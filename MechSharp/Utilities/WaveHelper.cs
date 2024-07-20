using System;
using NAudio.Wave;

namespace MechSharp.Utilities;

public static class WaveHelper
{
    public static byte[] Cut(byte[] buffer, WaveFormat waveFormat, int start, int length)
    {
        var offset = waveFormat.ConvertLatencyToByteSize(start);
        var count = waveFormat.ConvertLatencyToByteSize(length);

        if (offset + count > buffer.Length)
        {
            return [];
        }

        return buffer.AsSpan(offset, count).ToArray();
    }

    public static void Split(byte[] buffer, WaveFormat waveFormat, out byte[] first, out byte[] second)
    {
        var count = buffer.Length;
        var mid = count / 2;

        var align = mid % waveFormat.BlockAlign;
        if (align != 0)
        {
            mid += waveFormat.BlockAlign - align;
        }

        first = buffer.AsSpan(0, mid).ToArray();
        second = buffer.AsSpan(mid).ToArray();
    }
}

using System;
using System.IO;
using NAudio.Wave;

namespace MechSharp.Utilities;

public static class WaveHelper
{
    public static WaveStream Cut(WaveStream stream, int start, int length)
    {
        var offset = stream.WaveFormat.ConvertLatencyToByteSize(start);
        var desiredCount = stream.WaveFormat.ConvertLatencyToByteSize(length);

        var bytesAvailableToRead = stream.Length - stream.Position;
        var bytesToRead = Math.Min(desiredCount, bytesAvailableToRead);

        stream.Seek(offset, SeekOrigin.Begin);
        var buffer = new byte[bytesToRead];
        var count = stream.Read(buffer);

        return new RawSourceWaveStream(buffer, 0, count, stream.WaveFormat);
    }

    public static void Split(WaveStream stream, out WaveStream first, out WaveStream second)
    {
        var allocation = stream.Length - stream.Position;

        var buffer = new byte[allocation];
        var count = stream.Read(buffer);

        var mid = count / 2;

        var blockAlign = stream.WaveFormat.BlockAlign;
        var currentAlignment = mid % blockAlign;

        if (currentAlignment != 0)
        {
            mid += blockAlign - currentAlignment;
        }

        first = new RawSourceWaveStream(buffer, 0, mid, stream.WaveFormat);
        second = new RawSourceWaveStream(buffer, mid, count - mid, stream.WaveFormat);
    }
}

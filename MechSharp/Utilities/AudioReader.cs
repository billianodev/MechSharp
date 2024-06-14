using System.IO;
using NAudio.Vorbis;
using NAudio.Wave;

namespace MechSharp.Utilities;

public class AudioReader : WaveStream
{
    public override WaveFormat WaveFormat { get => baseStream.WaveFormat; }
    public override long Length { get => baseStream.Length; }
    public override long Position { get => baseStream.Position; set => baseStream.Position = value; }

    private WaveStream baseStream;

    public AudioReader(string file)
    {
        if (file.EndsWith(".ogg"))
        {
            baseStream = new VorbisWaveReader(file);
        }
        else
        {
            baseStream = new AudioFileReader(file);
        }
    }

    public override int Read(byte[] buffer, int offset, int count) => baseStream.Read(buffer, offset, count);
}
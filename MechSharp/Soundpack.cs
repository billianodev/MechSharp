using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Avalonia.Media;
using Billiano.NAudio;
using MechSharp.Utilities;
using NAudio.Wave;
using PortAudioSharp;
using SkiaSharp;

namespace MechSharp;

public class Soundpack : Dictionary<int, SampleCache>
{
	public SoundpackInfo Info { get; }

	private Soundpack(SoundpackInfo info)
	{
		Info = info;
	}

	public static Soundpack LoadKeypack(SoundpackInfo info, WaveFormat waveFormat, bool isKeyUp)
	{
		Soundpack soundpack = new(info);
		if (info.KeyDefineType == "multi")
		{
			Dictionary<int, string>? defines = info.Defines.Deserialize(SoundpackInfoContext.Default.DictionaryInt32String);
			if (defines == null)
			{
				return soundpack;
			}
			Dictionary<string, (SampleCache, SampleCache?)> caches = [];
			foreach (string file in defines.Values.Distinct())
			{
				if (file == null)
				{
					continue;
				}
				string path = Path.Combine(info.Dir!, file);
				if (!File.Exists(path))
				{
					continue;
				}
				using (FileStream stream = File.OpenRead(path))
				using (WaveStream reader = AudioStreamReader.LoadByHeader(stream))
				{
					caches.Add(file, Split(reader, waveFormat, isKeyUp));
				}
			}
			foreach ((int id, string file) in defines)
			{
				if (file == null)
				{
					continue;
				}
				if (caches.TryGetValue(file, out (SampleCache, SampleCache?) cache))
				{
					soundpack[id] = cache.Item1;
					if (isKeyUp)
					{
						soundpack[-id] = cache.Item2!;
					}
				}
			}
			return soundpack;
		}
		if (info.KeyDefineType == "single")
		{
			Dictionary<int, (int, int)?>? defines = info.Defines.Deserialize(SoundpackInfoContext.Default.DictionaryInt32NullableValueTupleInt32Int32);
			if (defines == null)
			{
				return soundpack;
			}
			string path = Path.Combine(info.Dir!, info.Sound!);
			if (!File.Exists(path))
			{
				return soundpack;
			}
			Dictionary<(int, int), (SampleCache, SampleCache?)> caches = [];
			using (FileStream stream = File.OpenRead(path))
			using (WaveStream reader = AudioStreamReader.LoadByHeader(stream))
			{
				byte[] buffer = new byte[reader.Length];
				reader.Read(buffer, 0, buffer.Length);

				foreach ((int, int)? range in defines.Values.Distinct())
				{
					if (!range.HasValue)
					{
						continue;
					}
					int start = reader.WaveFormat.ConvertLatencyToByteSize(range.Value.Item1);
					int length = reader.WaveFormat.ConvertLatencyToByteSize(range.Value.Item2);
					if (length > buffer.Length)
					{
						length = buffer.Length;
					}
					using (RawSourceWaveStream sStream = new(buffer, start, length, reader.WaveFormat))
					{
						caches.Add(range.Value, Split(sStream, waveFormat, isKeyUp));
					}
				}
			}
			foreach ((int id, (int, int)? range) in defines)
			{
				if (range.HasValue)
				{
					if (caches.TryGetValue(range.Value, out (SampleCache, SampleCache?) cache))
					{
						soundpack[id] = cache.Item1;
						if (isKeyUp)
						{
							soundpack[-id] = cache.Item2!;
						}
					}
				}
			}
			return soundpack;
		}
		throw new NotSupportedException();
	}

	public static Soundpack LoadMousepack(SoundpackInfo info, WaveFormat waveFormat)
	{
		Soundpack soundpack = new(info);
		Dictionary<string, string>? defines = info.Defines.Deserialize(SoundpackInfoContext.Default.DictionaryStringString);
		if (defines == null)
		{
			return soundpack;
		}
		foreach ((string id, string file) in defines)
		{
			if (file == null)
			{
				continue;
			}
			string path = Path.Combine(info.Dir!, file);
			if (!File.Exists(path))
			{
				continue;
			}
			using (FileStream stream = File.OpenRead(path))
			using (WaveStream reader = AudioStreamReader.LoadByHeader(stream))
			{
				soundpack[MousevibesButton.Parse(id)] = SampleCacheFactory.CreateFromWaveWithResampler(reader, waveFormat);
			}
		}
		return soundpack;
	}

	private static (SampleCache, SampleCache?) Split(WaveStream stream, WaveFormat waveFormat, bool keyUp)
	{
		if (!keyUp)
		{
			return (SampleCacheFactory.CreateFromWaveWithResampler(stream, waveFormat), null);
		}

		byte[] buffer = new byte[stream.Length];
		stream.Read(buffer, 0, buffer.Length);

		int length = (int)stream.Length;
		int mid = length / 2;
		mid += mid % stream.BlockAlign;

		using (RawSourceWaveStream fStream = new(buffer, 0, mid, stream.WaveFormat))
		using (RawSourceWaveStream sStream = new(buffer, mid, length - mid, stream.WaveFormat))
		{
			return (SampleCacheFactory.CreateFromWaveWithResampler(fStream, waveFormat),
				SampleCacheFactory.CreateFromWaveWithResampler(sStream, waveFormat));
		}
	}
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Billiano.NAudio;
using MechSharp.Utilities;
using NAudio.Wave;

namespace MechSharp;

public class Soundpack : Dictionary<string, WaveCache>
{
	public SoundpackInfo Info { get; }

	private Soundpack(SoundpackInfo info)
	{
		Info = info;
	}

	public static Soundpack LoadKeypack(SoundpackInfo info, WaveFormat waveFormat, bool split)
	{
		var soundpack = new Soundpack(info);
		if (info.KeyDefineType == "multi")
		{
			var defines = info.Defines.Deserialize(SoundpackInfoContext.Default.DictionaryStringString);
			if (defines == null)
			{
				return soundpack;
			}
			var caches = new Dictionary<string, (WaveCache, WaveCache?)>();
			foreach (var file in defines.Values.Distinct())
			{
				if (file == null)
				{
					continue;
				}
				var path = Path.Combine(info.Dir!, file);
				if (!File.Exists(path))
				{
					continue;
				}
				using (var stream = File.OpenRead(path))
				using (var wave = new AudioStreamReader(stream))
				{
					caches.Add(file, Split(wave, waveFormat, split));
				}
			}
			foreach ((var id, var file) in defines)
			{
				if (file == null)
				{
					continue;
				}
				if (caches.TryGetValue(file, out var cache))
				{
					soundpack[id] = cache.Item1;
					if (split)
					{
						soundpack[MechvibesMapper.MapUp(id)] = cache.Item2!;
					}
				}
			}
			return soundpack;
		}
		if (info.KeyDefineType == "single")
		{
			var defines = info.Defines.Deserialize(SoundpackInfoContext.Default.DictionaryStringNullableValueTupleInt32Int32);
			if (defines == null)
			{
				return soundpack;
			}
			string path = Path.Combine(info.Dir!, info.Sound!);
			if (!File.Exists(path))
			{
				return soundpack;
			}
			var caches = new Dictionary<(int, int), (WaveCache, WaveCache?)>();
			using (var stream = File.OpenRead(path))
			using (var wave = new AudioStreamReader(stream))
			{
				var buffer = new byte[wave.Length];
				var c = wave.Read(buffer, 0, buffer.Length);

				foreach (var range in defines.Values.Distinct())
				{
					if (!range.HasValue)
					{
						continue;
					}
					var start = wave.WaveFormat.ConvertLatencyToByteSize(range.Value.Item1);
					var length = wave.WaveFormat.ConvertLatencyToByteSize(range.Value.Item2);
					if (length > buffer.Length)
					{
						length = buffer.Length;
					}
					using (var sStream = new RawSourceWaveStream(buffer, start, length, wave.WaveFormat))
					{
						caches.Add(range.Value, Split(sStream, waveFormat, split));
					}
				}
			}
			foreach ((var id, var range) in defines)
			{
				if (range.HasValue)
				{
					if (caches.TryGetValue(range.Value, out var cache))
					{
						soundpack[id] = cache.Item1;
						if (split)
						{
							soundpack[MechvibesMapper.MapUp(id)] = cache.Item2!;
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
		var soundpack = new Soundpack(info);
		var defines = info.Defines.Deserialize(SoundpackInfoContext.Default.DictionaryStringString);
		if (defines == null)
		{
			return soundpack;
		}
		foreach ((var id, var file) in defines)
		{
			if (file == null)
			{
				continue;
			}
			var path = Path.Combine(info.Dir!, file);
			if (!File.Exists(path))
			{
				continue;
			}
			using (var stream = File.OpenRead(path))
			using (var reader = new AudioStreamReader(stream))
			{
				soundpack[id] = new WaveCache(reader, waveFormat);
			}
		}
		return soundpack;
	}

	private static (WaveCache, WaveCache?) Split(WaveStream stream, WaveFormat waveFormat, bool keyUp)
	{
		if (!keyUp)
		{
			return (new WaveCache(stream, waveFormat), null);
		}

		var buffer = new byte[stream.Length];
		stream.Read(buffer, 0, buffer.Length);

		var length = (int)stream.Length;
		var mid = length / 2;
		mid += mid % stream.BlockAlign;

		using (var fStream = new RawSourceWaveStream(buffer, 0, mid, stream.WaveFormat))
		using (var sStream = new RawSourceWaveStream(buffer, mid, length - mid, stream.WaveFormat))
		{
			return (new WaveCache(fStream, waveFormat), new WaveCache(sStream, waveFormat));
		}
	}
}
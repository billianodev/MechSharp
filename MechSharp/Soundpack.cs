﻿using System;
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
			var defines = info.Defines.Deserialize(SoundpackInfoContext.Default.DictionaryInt32String);
			if (defines == null)
			{
				return soundpack;
			}

			var caches = new Dictionary<string, (SampleCache, SampleCache?)>();
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
				using (var reader = new AudioReader(path))
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
				if (caches.TryGetValue(file, out var cache))
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
			var defines = info.Defines.Deserialize(SoundpackInfoContext.Default.DictionaryInt32NullableValueTupleInt32Int32);
			if (defines == null)
			{
				return soundpack;
			}
			var path = Path.Combine(info.Dir!, info.Sound!);
			if (!File.Exists(path))
			{
				return soundpack;
			}
			var caches = new Dictionary<(int, int), (SampleCache, SampleCache?)>();
			using (var reader = new AudioReader(path))
			{
				var buffer = new byte[reader.Length];
				reader.Read(buffer, 0, buffer.Length);

				foreach (var range in defines.Values.Distinct())
				{
					if (!range.HasValue)
					{
						continue;
					}
					var start = reader.WaveFormat.ConvertLatencyToByteSize(range.Value.Item1);
					var length = reader.WaveFormat.ConvertLatencyToByteSize(range.Value.Item2);
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
			foreach ((var id, var range) in defines)
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
		var soundpack = new Soundpack(info);
		var defines = info.Defines.Deserialize(SoundpackInfoContext.Default.DictionaryStringString);
		if (defines == null)
		{
			return soundpack;
		}
		foreach ((var id, string file) in defines)
		{
			var path = Path.Combine(info.Dir!, file);
			if (!File.Exists(path))
			{
				continue;
			}
			using (var reader = new AudioReader(path))
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

		var buffer = new byte[stream.Length];
		stream.Read(buffer, 0, buffer.Length);

		var length = (int)stream.Length;
		var mid = length / 2;
		mid += mid % stream.BlockAlign;

		using (var fStream = new RawSourceWaveStream(buffer, 0, mid, stream.WaveFormat))
		using (var sStream = new RawSourceWaveStream(buffer, mid, length - mid, stream.WaveFormat))
		{
			return (SampleCacheFactory.CreateFromWaveWithResampler(fStream, waveFormat),
				SampleCacheFactory.CreateFromWaveWithResampler(sStream, waveFormat));
		}
	}
}
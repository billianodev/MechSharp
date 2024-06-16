using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Billiano.Audio;
using Billiano.Audio.FireForget;
using CSCore.Codecs;
using MechSharp.Utilities;
using NAudio.Wave;

namespace MechSharp;

public class Soundpack : Dictionary<int, IFireForgetSource>
{
	public SoundpackInfo Info { get; }

	private Soundpack(SoundpackInfo info)
	{
		Info = info;
	}

	public static Soundpack LoadKeypack(SoundpackInfo info, bool isKeyUp)
	{
		var soundpack = new Soundpack(info);
		switch (info.KeyDefineType)
		{
			case "multi":
			{
				var defines = info.Defines.Deserialize(SoundpackInfoContext.Default.DictionaryInt32String);
				if (defines == null)
				{
					return soundpack;
				}

				var caches = new Dictionary<string, (IFireForgetSource, IFireForgetSource?)>();
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
					using (var reader = CodecFactory.Instance.GetCodec(path).ToWaveProvider())
					{
						caches.Add(file, Split(reader, isKeyUp));
					}
				}
				foreach (var (id, file) in defines)
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
			case "single":
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
				var caches = new Dictionary<(int, int), (IFireForgetSource, IFireForgetSource?)>();
				using (var reader = CodecFactory.Instance.GetCodec(path).ToWaveProvider())
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
							caches.Add(range.Value, Split(sStream, isKeyUp));
						}
					}
				}
				foreach (var (id, range) in defines)
				{
					if (range.HasValue)
					{
						if (caches.TryGetValue(range.Value, out var cache))
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
			default:
				throw new NotSupportedException();
		}
	}

	public static Soundpack LoadMousepack(SoundpackInfo info)
	{
		var soundpack = new Soundpack(info);
		var defines = info.Defines.Deserialize(SoundpackInfoContext.Default.DictionaryStringString);
		if (defines == null)
		{
			return soundpack;
		}
		foreach (var (id, file) in defines)
		{
			var path = Path.Combine(info.Dir!, file);
			if (!File.Exists(path))
			{
				continue;
			}
			using (var reader = CodecFactory.Instance.GetCodec(path).ToWaveProvider())
			{
				soundpack[MousevibesButton.Parse(id)] = reader.ToFireForgetSource();
			}
		}
		return soundpack;
	}

	private static (IFireForgetSource, IFireForgetSource?) Split(WaveStream stream, bool keyUp)
	{
		if (!keyUp)
		{
			return (stream.ToFireForgetSource(), null);
		}

		var buffer = new byte[stream.Length];
		stream.Read(buffer, 0, buffer.Length);

		var length = (int)stream.Length;
		var mid = length / 2;
		mid += mid % stream.BlockAlign;

		using (var fStream = new RawSourceWaveStream(buffer, 0, mid, stream.WaveFormat))
		using (var sStream = new RawSourceWaveStream(buffer, mid, length - mid, stream.WaveFormat))
		{
			return (fStream.ToFireForgetSource(),
				sStream.ToFireForgetSource());
		}
	}
}
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MechSharp.Utilities;

public class TupleConverter<T1, T2> : JsonConverter<(T1, T2)?>
{
	public override (T1, T2)? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (JsonElement.TryParseValue(ref reader, out var element))
		{
			if (element.Value.ValueKind != JsonValueKind.Array || element.Value.GetArrayLength() < 2)
				return null;

			if (options.TryGetTypeInfo(typeof(T1), out var typeInfo1))
			{
				if (options.TryGetTypeInfo(typeof(T2), out var typeInfo2))
				{
					var item1 = (T1?)JsonSerializer.Deserialize(element.Value[0], typeInfo1);
					var item2 = (T2?)JsonSerializer.Deserialize(element.Value[1], typeInfo2);

					if (item1 == null || item2 == null)
						return null;

					return (item1, item2);
				}
			}
		}
		return null;
	}

	public override void Write(Utf8JsonWriter writer, (T1, T2)? value, JsonSerializerOptions options)
	{
		if (!value.HasValue)
		{
			writer.WriteNullValue();
			return;
		}

		if (options.TryGetTypeInfo(typeof(T1), out var t1info))
		{
			if (options.TryGetTypeInfo(typeof(T2), out var t2info))
			{
				writer.WriteStartArray();
				writer.WriteRawValue(JsonSerializer.Serialize(value.Value.Item1, t1info));
				writer.WriteRawValue(JsonSerializer.Serialize(value.Value.Item2, t2info));
				writer.WriteEndArray();
			}
		}
	}
}


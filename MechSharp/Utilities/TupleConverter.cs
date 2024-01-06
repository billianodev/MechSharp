using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace MechSharp.Utilities;

public class TupleConverter<T1, T2> : JsonConverter<(T1, T2)?>
{
	public override (T1, T2)? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (JsonElement.TryParseValue(ref reader, out JsonElement? element))
		{
			if (element.Value.ValueKind != JsonValueKind.Array || element.Value.GetArrayLength() < 2)
				return null;

			if (options.TryGetTypeInfo(typeof(T1), out JsonTypeInfo? typeInfo1))
			{
				if (options.TryGetTypeInfo(typeof(T2), out JsonTypeInfo? typeInfo2))
				{
					T1? item1 = (T1?)element.Value[0].Deserialize(typeInfo1);
					T2? item2 = (T2?)element.Value[1].Deserialize(typeInfo2);
					
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

		if (options.TryGetTypeInfo(typeof(T1), out JsonTypeInfo? t1info))
		{
			if (options.TryGetTypeInfo(typeof(T2), out JsonTypeInfo? t2info))
			{
				writer.WriteStartArray();
				writer.WriteRawValue(JsonSerializer.Serialize(value.Value.Item1, t1info));
				writer.WriteRawValue(JsonSerializer.Serialize(value.Value.Item2, t2info));
				writer.WriteEndArray();
			}
		}
	}
}


using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using MechSharp.Core.Soundpacks.Defines;

namespace MechSharp.Json;

public class SoundpackDefinesRangeJsonConverter : JsonConverter<SoundpackDefinesRange?>
{
    public override SoundpackDefinesRange? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        try
        {
            var json = JsonElement.ParseValue(ref reader);

            if (json.ValueKind == JsonValueKind.Array)
            {
                var array = json.EnumerateArray().ToArray();

                if (array.Length == 2)
                {
                    if (array[0].TryGetInt32(out var start) && array[1].TryGetInt32(out var end))
                    {
                        return new SoundpackDefinesRange(start, end);
                    }
                }
            }
        }
        catch
        {
        }

        return null;
    }

    public override void Write(Utf8JsonWriter writer, SoundpackDefinesRange? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartArray();
        writer.WriteNumberValue(value.Start);
        writer.WriteNumberValue(value.Length);
        writer.WriteEndArray();
    }
}
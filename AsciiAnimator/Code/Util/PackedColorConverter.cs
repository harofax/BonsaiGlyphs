using Newtonsoft.Json;

namespace AsciiAnimator.Code.Util;

public class PackedColorConverter : JsonConverter<Color>
{
    public override void WriteJson(JsonWriter writer, Color value, JsonSerializer serializer) =>
        serializer.Serialize(writer, (PackedColorSerialized)value);

    public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue, bool hasExistingValue, JsonSerializer serializer) =>
        serializer.Deserialize<PackedColorSerialized>(reader);
}

public struct PackedColorSerialized
{
    public uint _packedValue;

    public static implicit operator PackedColorSerialized(Color color) =>
        new PackedColorSerialized()
        {
            _packedValue = color.PackedValue
        };

    public static implicit operator Color(PackedColorSerialized packedColor) =>
        new Color(packedColor._packedValue);
}
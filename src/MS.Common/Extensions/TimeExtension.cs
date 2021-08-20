using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MS.Common.Extensions
{
    public static class TimeExtension
    {
        /// <summary>
        /// 自定义DateTime解析类
        /// </summary>
        public class DateTimeConverter : JsonConverter<DateTime>
        {
            public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return DateTime.TryParse(reader.GetString(), out var dateTime) ? dateTime : default(DateTime);
            }

            public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToString("yyyy-MM-dd HH:mm:ss"));
            }
        }

        /// <summary>
        /// DateTime?类型解析类
        /// </summary>
        public class DateTimeNullableConvert : JsonConverter<DateTime?>
        {
            public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return DateTime.TryParse(reader.GetString(), out var dateTime) ? dateTime : default(DateTime?);
            }

            public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value?.ToString("yyyy-MM-dd HH:mm:ss"));
            }
        }

    }
}

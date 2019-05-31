namespace backend.jingle.net.Logic
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class MailChimpRequest
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("contact")]
        public Contact Contact { get; set; }

        [JsonProperty("permission_reminder")]
        public string PermissionReminder { get; set; }

        [JsonProperty("campaign_defaults")]
        public CampaignDefaults CampaignDefaults { get; set; }

        [JsonProperty("email_type_option")]
        public bool EmailTypeOption { get; set; }
    }

    public partial class CampaignDefaults
    {
        [JsonProperty("from_name")]
        public string FromName { get; set; }

        [JsonProperty("from_email")]
        public string FromEmail { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }
    }

    public partial class Contact
    {
        [JsonProperty("company")]
        public string Company { get; set; }

        [JsonProperty("address1")]
        public string Address1 { get; set; }

        [JsonProperty("address2")]
        public string Address2 { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("zip")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Zip { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }
    }

    public partial class MailChimpRequest
    {
        public static MailChimpRequest FromJson(string json) => JsonConvert.DeserializeObject<MailChimpRequest>(json, backend.jingle.net.Logic.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this MailChimpRequest self) => JsonConvert.SerializeObject(self, backend.jingle.net.Logic.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }
}
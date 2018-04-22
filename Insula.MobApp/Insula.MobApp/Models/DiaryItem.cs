using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insula.MobApp.Models
{
    public class DiaryItem
    {
        [JsonProperty(PropertyName = "Id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "Insulin")]
        public double Insulin { get; set; }

        [JsonProperty(PropertyName = "Time")]
        public string Time { get; set; }

        [JsonProperty(PropertyName = "Carbo")]
        public double Carbo { get; set; }

        [JsonProperty(PropertyName = "GlucoseBefore")]
        public double GlucoseBefore { get; set; }

        [JsonProperty(PropertyName = "GlucoseAfter")]
        public double GlucoseAfter { get; set; }

        [JsonProperty(PropertyName = "UserId")]
        public int UserId { get; set; }

        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }

        public string DisplayName { get { return $"{Time} Carbo: {Carbo} Dose: {Insulin}"; } }

    }
}

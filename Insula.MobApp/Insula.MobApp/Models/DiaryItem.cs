using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insula.MobApp.Models
{
    public class DiaryItem : ICloneable
    {
        public int Id { get; set; }
        public double Insulin { get; set; }
        public DateTime Time { get; set; }
        public double Carbo { get; set; }
        public double GlucoseBefore { get; set; }
        public double GlucoseAfter { get; set; }
        public int UserId { get; set; }
        public string uri { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}

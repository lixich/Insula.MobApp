using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insula.MobApp.Models
{
    public class DiaryItem
    {
        public int Id { get; set; }

        public double Insulin { get; set; }

        public string Time { get; set; }

        public double Carbo { get; set; }

        public double GlucoseBefore { get; set; }

        public double GlucoseAfter { get; set; }

        public int UserId { get; set; }

        public string uri { get; set; }

        public string DisplayName { get { return $"{Time} Carbo: {Carbo} Dose: {Insulin}"; } }

    }
}

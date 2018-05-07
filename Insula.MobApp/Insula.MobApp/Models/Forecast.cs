using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insula.MobApp.Models
{
    public class Forecast
    {
        public double Value { get; set; }
        public string Name { get; set; }
        public double Accuracy { get; set; }

        public string DisplayName
        {
            get
            {
                return $"{Value, -2} - {Name} ({Accuracy})";
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StajProje.Model
{
    public class Capacity
    {
        public int CapacityId { get; set; }
        public int ATMId { get; set; }
        [JsonIgnore]
        public ATM ATM{ get; set; }

        public int MevcutBanknot10 { get; set; }
        public int MevcutBanknot20 { get; set; }
        public int MevcutBanknot50 { get; set; }
        public int MevcutBanknot100 { get; set; }
        public int MevcutBanknot200 { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }

    }
}

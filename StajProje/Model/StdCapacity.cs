using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StajProje.Model
{
    public class StdCapacity
    {
        public int StdCapacityId { get; set; }
        public int ATMId { get; set; }
        [JsonIgnore]
        public ATM ATM { get; set; }

        public int Banknot10 { get; set; }
        public int Banknot20 { get; set; }
        public int Banknot50 { get; set; }
        public int Banknot100 { get; set; }
        public int Banknot200 { get; set; }
    }
}

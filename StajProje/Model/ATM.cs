using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StajProje.Model
{
    public class ATM
    {
        public int Id { get; set; }
        public int ATMId { get; set; }
        public float Latitude { get; set; }
        public float Longitude{ get; set; }
        public bool ParaYatırma{ get; set; }
        public bool ParaÇekme { get; set; }
        public bool TL { get; set; }
        public bool USD { get; set; }
        public bool Euro { get; set; }
        public bool Status { get; set; }
        public Capacity Capacity { get; set; }
       public StdCapacity  StdCapacity{ get; set; }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}

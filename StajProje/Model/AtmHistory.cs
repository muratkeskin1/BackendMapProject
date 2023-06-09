using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StajProje.Model
{
    public class AtmHistory
    {
        public int id { get; set; }
        public int ATMId { get; set; }
        public DateTime Date { get; set; }
        public int Banknot10 { get; set; }
        public int Banknot20 { get; set; }
        public int Banknot50 { get; set; }
        public int Banknot100 { get; set; }
        public int Banknot200 { get; set; }
        public int ProcessType { get; set; }
    }
}

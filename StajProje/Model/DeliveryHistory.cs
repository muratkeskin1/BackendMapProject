using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StajProje.Model
{
    public class DeliveryHistory
    {
        public int id { get; set; }
        public DateTime date { get; set; }
        //TODO:toplam yatırılan banknotlar
        //ve çekilen banknot sayılarıda ayrı bir tabloda tutulacak ve atmid ile dağıtım bilgisi tutulacak dbde
        public double TotalDistance { get; set; }
        public double TotalTimeMinute { get; set; }
        public int TotalRoute { get; set; }

    }
}

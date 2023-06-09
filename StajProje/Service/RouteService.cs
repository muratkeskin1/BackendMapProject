using StajProje.Helper;
using StajProje.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StajProje.Service
{
    public class RouteService
    {
        private readonly ApplicationDbContext _context;
        public RouteService(ApplicationDbContext context)
        {
            _context = context;
        }
        public void DeliveryDetailSave(double distance, double time, int route)
        {
            DeliveryHistory deliveryHistory = new DeliveryHistory
            {
                date = DateTime.Today,
                TotalDistance = distance,
                TotalTimeMinute = time,
                TotalRoute = route - 2
            };
            _context.DeliveryHistories.Add(deliveryHistory);
            _context.SaveChanges();
        }
    }
}

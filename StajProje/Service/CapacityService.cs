using StajProje.Helper;
using StajProje.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StajProje.Service
{
    public class CapacityService
    {
        private readonly ApplicationDbContext _context;
        public CapacityService(ApplicationDbContext context)
        {
            _context = context;
        }
        public void UpdateCapacity(Capacity capacity)
        {
            capacity.ATM = _context.ATMs.FirstOrDefault(o => o.Id == capacity.ATMId);
            ATM atm = capacity.ATM;
            atm.Status = capacityStatus(capacity);
            _context.Capacities.Update(capacity);
            _context.ATMs.Update(atm);
            _context.SaveChanges();
        }

        private bool capacityStatus(Capacity capacity)
        {
            throw new NotImplementedException();
        }
    }
}

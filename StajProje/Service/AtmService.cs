using Microsoft.EntityFrameworkCore;
using StajProje.Helper;
using StajProje.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace StajProje.Service
{
    public class AtmService
    {
        private readonly ApplicationDbContext _context;
        public AtmService(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<ATM> GetATMs()
        {
            List<ATM> atms = _context.ATMs.Include(atm => atm.Capacity).ToList();
            return atms;
        }
        public void AddAtm(ATM test)
        {
            Capacity capacity = new Capacity();
            capacity.ATMId = test.ATMId;
            test.Capacity = capacity;
            capacity.MevcutBanknot10 = 20;
            capacity.MevcutBanknot20 = 20;
            capacity.MevcutBanknot50 = 20;
            capacity.MevcutBanknot100 = 20;
            capacity.MevcutBanknot200 = 20;
            test.Status = true;
            _context.ATMs.Add(test);
            _context.Capacities.Add(capacity);
            _context.SaveChanges();
        }
        public List<ATM> GetByStatus()
        {
            return _context.ATMs.Where(o => o.Status == false).ToList();
        }
        public void SimulateAtm(int islem, ATM atm)
        {
            Random rnd = new Random();
            string json = JsonSerializer.Serialize<ATM>(atm);
            ATM temp = JsonSerializer.Deserialize<ATM>(json);
            AtmHistory atmHistory = new AtmHistory
            {
                Banknot10 = atm.Capacity.MevcutBanknot10,
                Banknot20 = atm.Capacity.MevcutBanknot20,
                Banknot50 = atm.Capacity.MevcutBanknot50,
                Banknot100 = atm.Capacity.MevcutBanknot100,
                Banknot200 = atm.Capacity.MevcutBanknot200,
                Date = DateTime.Today
            };
            //0 yatırma 1 çekme
            if (islem == 0)
            {
                atm.Capacity.MevcutBanknot10 -= rnd.Next(atm.Capacity.MevcutBanknot10 < 50 ? atm.Capacity.MevcutBanknot10 : 50);
                atm.Capacity.MevcutBanknot20 -= rnd.Next(atm.Capacity.MevcutBanknot20 < 50 ? atm.Capacity.MevcutBanknot20 : 50);
                atm.Capacity.MevcutBanknot50 -= rnd.Next(atm.Capacity.MevcutBanknot50 < 25 ? atm.Capacity.MevcutBanknot50 : 25);
                atm.Capacity.MevcutBanknot100 -= rnd.Next(atm.Capacity.MevcutBanknot100 < 20 ? atm.Capacity.MevcutBanknot100 : 20);
                atm.Capacity.MevcutBanknot200 -= rnd.Next(atm.Capacity.MevcutBanknot200 < 15 ? atm.Capacity.MevcutBanknot200 : 15);
            }
            else
            {
                atm.Capacity.MevcutBanknot10 += rnd.Next(25);
                atm.Capacity.MevcutBanknot20 += rnd.Next(25);
                atm.Capacity.MevcutBanknot50 += rnd.Next(15);
                atm.Capacity.MevcutBanknot100 += rnd.Next(10);
                atm.Capacity.MevcutBanknot200 += rnd.Next(9);
            }
            atmHistory.ATMId = temp.ATMId;
            atmHistory.Banknot10 = Math.Abs(temp.Capacity.MevcutBanknot10 - atm.Capacity.MevcutBanknot10);
            atmHistory.Banknot20 = Math.Abs(temp.Capacity.MevcutBanknot20 - atm.Capacity.MevcutBanknot20);
            atmHistory.Banknot50 = Math.Abs(temp.Capacity.MevcutBanknot50 - atm.Capacity.MevcutBanknot50);
            atmHistory.Banknot100 = Math.Abs(temp.Capacity.MevcutBanknot100 - atm.Capacity.MevcutBanknot100);
            atmHistory.Banknot200 = Math.Abs(temp.Capacity.MevcutBanknot200 - temp.Capacity.MevcutBanknot200);
            atmHistory.ProcessType = islem;
            _context.AtmHistories.Add(atmHistory);
            atm.Status = capacityStatus(atm.Capacity);
            _context.ATMs.Update(atm);
            _context.SaveChanges();

        }
        private bool capacityStatus(Capacity capacity)
        {
            throw new NotImplementedException();
        }
        public void SimulateDelivery()
        {
           /* List<ATM> atmListToDeliver = _context.ATMs.Include(o => o.Capacity).Where(o => o.Status == false).ToList();
            //status false olanları getirip bunlar ile bir rota çizdirilecek 
            List<StdCapacity> stdCapacities = _context.StdCapacities.Where(a
                => atmListToDeliver.Select(o => o.Id).Contains(a.ATMId)).ToList();
            //dbden std kapasite bilgilerini getirip bunlara göre ekleme veya çıkarma yapacak 
            for (int i = 0; i < stdCapacities.Count(); i++)
            {
                DeliverHistoryInformation(atmListToDeliver[i], stdCapacities.Find(o => o.ATMId == atmListToDeliver[i].Id));
                atmListToDeliver[i].Capacity.MevcutBanknot10 = stdCapacities.Find(o => o.ATMId == atmListToDeliver[i].Id).Banknot10;
                atmListToDeliver[i].Capacity.MevcutBanknot20 = stdCapacities.Find(o => o.ATMId == atmListToDeliver[i].Id).Banknot20;
                atmListToDeliver[i].Capacity.MevcutBanknot50 = stdCapacities.Find(o => o.ATMId == atmListToDeliver[i].Id).Banknot50;
                atmListToDeliver[i].Capacity.MevcutBanknot100 = stdCapacities.Find(o => o.ATMId == atmListToDeliver[i].Id).Banknot100;
                atmListToDeliver[i].Capacity.MevcutBanknot200 = stdCapacities.Find(o => o.ATMId == atmListToDeliver[i].Id).Banknot200;
                atmListToDeliver[i].Status = true;
                _context.ATMs.Update(atmListToDeliver[i]);

            }
            _context.SaveChanges();*/

            //en sonda dağıtım bilgilerini dbye kayıt edip çıkacak 
            //toplam ne kadar sürdüğünü  ne kadar yol gittiğini ne kadar noktaya uğradığını vs +
            //DeliveryDetailSave(); yazdıldı yarın düzenlemeler yapılacak githuba yükleme react ile beraber

        }
        public void DeliverHistoryInformation(ATM atm, StdCapacity stdCapacity)
        {
            AtmDeliveryHistory atmDeliveryHistory = new AtmDeliveryHistory
            {
                Banknot10 = stdCapacity.Banknot10 - atm.Capacity.MevcutBanknot10,
                Banknot20 = stdCapacity.Banknot20 - atm.Capacity.MevcutBanknot20,
                Banknot50 = stdCapacity.Banknot50 - atm.Capacity.MevcutBanknot50,
                Banknot100 = stdCapacity.Banknot100 - atm.Capacity.MevcutBanknot100,
                Banknot200 = stdCapacity.Banknot200 - atm.Capacity.MevcutBanknot200,
                Date = DateTime.Today,
                AtmId = atm.Id
            };
            _context.AtmDeliveryHistories.Add(atmDeliveryHistory);
            _context.SaveChanges();


        }
    }
}

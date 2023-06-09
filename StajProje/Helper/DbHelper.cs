using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using ServiceStack;
using StajProje.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace StajProje.Helper
{
    public class DbHelper
    {
        private readonly IConfiguration _configuration;
        private readonly TimeSpan _expirationTime;
        private readonly ApplicationDbContext _context;
        private readonly AzureRedisCacheService _cacheService;
        public static string cacheKey = "azureRedisAtmList";
        public DbHelper(ApplicationDbContext context,[Optional] IConfiguration configuration, [Optional] AzureRedisCacheService azureRedisCacheService)
        {
            _context = context;
            _configuration = configuration;
            _cacheService = new AzureRedisCacheService(_configuration); ; 
            _expirationTime= TimeSpan.FromSeconds(300);
        }
        
        public void RedisUpdate()
        {
            _cacheService.Set(cacheKey, _context.ATMs.Include(atm => atm.Capacity)
               .Include(atm => atm.StdCapacity).ToList(), _expirationTime);
        }
        
        public List<ATM> GetFromRedis()
        {
          
            var data = _cacheService.Get<List<ATM>>(cacheKey);
            if (data!=null)
                return data;

            data =  _context.ATMs.Include(atm => atm.Capacity).Include(atm=> atm.StdCapacity).ToList(); ;
            if (data != null)
            {
                _cacheService.Set(cacheKey, data, _expirationTime);
                return data;
            }
            return null;
        }
        public List<UserLogin> GetUsers()
        {
            return _context.User.ToList();
        }
        public UserLogin GetUserByEmail(string email)
        {
            return _context.User.FirstOrDefault(o => o.email == email);
        }
        public List<ATM> GetATMs()
        {
            List<ATM> atms = _context.ATMs.Include(atm => atm.Capacity).Include(atm => atm.StdCapacity).ToList();
            return atms;
        }
        public void AddAtm(ATM test)
        {
            Capacity capacity = new Capacity
            {
                ATMId = test.ATMId,
                MevcutBanknot10 = 1000,
                MevcutBanknot20 = 1000,
                MevcutBanknot50 = 750,
                MevcutBanknot100 = 500,
                MevcutBanknot200 = 250,
            };
            StdCapacity stdCapacity = new StdCapacity
            {
                ATMId = test.Id,
                Banknot10 = 1000,
                Banknot20 = 1000,
                Banknot50 = 750,
                Banknot100 = 500,
                Banknot200 = 250,
            };

            test.Capacity = capacity;
            test.StdCapacity = stdCapacity;
            test.Status = true;
            _context.ATMs.Add(test);
            _context.Capacities.Add(capacity);
            _context.StdCapacities.Add(stdCapacity);
            _context.SaveChanges();
            RedisUpdate();
        }
        public void RunAtmCapacitySimulation()
        {
            //atmlerde ki kapasitelerde değişim olacak sınır altındakiler kırmızı olacak
            //sınır 20 şuanlık
            //br günde mim 2k max 4k kişi atm kullanacak çekme ve yatırma 
            //0,1 random ile çekme yatırma belirlenecek
            //banknotlar için yatırma çekme miktarı belirlenecek
            List<ATM> atms = _context.ATMs.Where(o => o.Status == true).Include(atm => atm.Capacity).ToList();
            Random rnd = new Random();
            int rndatm = rnd.Next(atms.Count());
            for (int i = 0; i < atms.Count(); i++)
            {
                int islemSayısı = rnd.Next(50);
                if (atms[i].Status)
                {
                    for (int j = 0; j < islemSayısı; j++)
                    {
                        int islem = rnd.Next(0, 2);
                        if (atms[i].Status)
                        {
                            SimulateAtm(islem, atms[i]);
                        }

                    }
                }
            }
            _context.SaveChanges();
            RedisUpdate();
            if (arr.Count() > 0)
            {
                EmailTest();

            }
        }
        
        public void DeleteAtm(int id)
        {
            _context.ATMs.Remove(_context.ATMs.FirstOrDefault(o => o.Id == id));
            RedisUpdate();
            _context.SaveChanges();
        }

        public List<DeliveryHistory> GetDeliveryHistory()
        {
            return _context.DeliveryHistories.ToList();
        }

        public object GetChartData()
        {
            Dictionary<int, int> keyValues = new Dictionary<int, int>();
            List<AtmHistory> data = _context.AtmHistories.Where(o=>o.Date==DateTime.Today).ToList();
            foreach (var item in data)
            {
                if (keyValues.ContainsKey(item.ATMId))
                {
                    keyValues[item.ATMId] += 1;
                }
                else
                {
                    keyValues.Add(item.ATMId, 1);
                }
            }
            return keyValues;
        }

        //2 hafta yeni modüller 2 hafta admin page ve refactor 2 hafta publish 1 hafta android 1 hafta da refactor
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
        public void SimulateDelivery()
        {
            // status false olanların kapasiteleri güncellior değiştirilebilir.
            List<ATM> atmListToDeliver = _context.ATMs.Include(o => o.Capacity).Where(o => o.Status == false).ToList();
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
          
            _context.SaveChanges();
            RedisUpdate();

            //en sonda dağıtım bilgilerini dbye kayıt edip çıkacak 
            //toplam ne kadar sürdüğünü  ne kadar yol gittiğini ne kadar noktaya uğradığını vs +
            //DeliveryDetailSave(); yazdıldı yarın düzenlemeler yapılacak githuba yükleme react ile beraber

        }
        public void EmailTest([Optional] IFormFile file, [Optional] ExcelPackage excelPackage)
        {
            using MailMessage mm = new MailMessage("keskinmurat888@gmail.com", "legend918@hotmail.com");
            string body = "";
            body += "kapasite sıkıntısı olan atmlerin numaraları: " + DateTime.Now.ToString();
            for (int i = 0; i < arr.Count(); i++)
            {

                body += " " + arr[i];
            }
            arr.Clear();
            mm.Subject = "uyarı";
            mm.Body = body;
            mm.IsBodyHtml = false;
            using SmtpClient smtp = new SmtpClient();
            //"iyzavjzketybhoer"
            if (file != null)
            {

                mm.Attachments.Add(new Attachment(file.OpenReadStream(), file.FileName));
                // mm.Attachments.Add(new Attachment(sr,"excel.xlsx"));
            }
            if (excelPackage != null)
            {
                MemoryStream ms = new MemoryStream(excelPackage.GetAsByteArray());
                mm.Attachments.Add(new Attachment(ms, "sheet.xlsx"));
            }
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            NetworkCredential NetworkCred = new NetworkCredential("keskinmurat888@gmail.com", "iyzavjzketybhoer");
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = NetworkCred;
            smtp.Port = 587;
            smtp.Send(mm);

        }
        public void TestExcel()
        {
            using ExcelPackage excelPackage = new ExcelPackage();
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Atm Table");
            List<ATM> atmlist = _context.ATMs.ToList();
            List<Capacity> capacities = _context.Capacities.ToList();
           //worksheet.Cells["A2"].LoadFromCollection(atmlist);
           worksheet.Cells["A2"].LoadFromCollection(capacities);
            //kolonlar
            worksheet.DeleteColumn(3);
            worksheet.Cells["A1"].Value = "CapacityId";
            worksheet.Cells["B1"].Value = "atmId";
            worksheet.Cells["C1"].Value = "Banknot10";
            worksheet.Cells["D1"].Value = "Banknot20.";
            worksheet.Cells["E1"].Value = "Banknot50";
            worksheet.Cells["F1"].Value = "Banknot100";
            worksheet.Cells["G1"].Value = "Banknot200";
            /* worksheet.Cells["A1"].Value = "id";
             worksheet.Cells["B1"].Value = "atmId";
             worksheet.Cells["C1"].Value = "Lat.";
             worksheet.Cells["D1"].Value = "Long.";
             worksheet.Cells["E1"].Value = "Py";
             worksheet.Cells["F1"].Value = "Pç";
             worksheet.Cells["G1"].Value = "TL";
             worksheet.Cells["H1"].Value = "Usd";
             worksheet.Cells["I1"].Value = "euro";
             worksheet.Cells["J1"].Value = "status";*/
            EmailTest(excelPackage:excelPackage);

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
            atm.Status = CapacityStatus(atm.Capacity);
            _context.ATMs.Update(atm);
                 
        }
        public static List<int> arr = new List<int>();
        public bool CapacityStatus(Capacity capacity)
        {
            //daha sonra eklenecek
            if (capacity.MevcutBanknot10 < 20 || capacity.MevcutBanknot20 < 20
                         || capacity.MevcutBanknot50 < 20 || capacity.MevcutBanknot100 < 20 ||
                        capacity.MevcutBanknot200 < 20)
            {
                //email gönderimi yapılacak atm ve kapasite bilgileri ile beraber
                if (arr.Contains(capacity.ATM.ATMId) == false)
                {
                    arr.Add(capacity.ATM.ATMId);
                }
                return false;
            }
            return true;
        }
        public List<ATM> GetByStatus()
        {
            return _context.ATMs.Where(o => o.Status == false).ToList();
        }
        public void UpdateCapacity(Capacity capacity)
        {
            capacity.ATM = _context.ATMs.FirstOrDefault(o => o.Id == capacity.ATMId);
            ATM atm = capacity.ATM;
            atm.Status = CapacityStatus(capacity);
            _context.Capacities.Update(capacity);
            _context.ATMs.Update(atm);
            _context.SaveChanges();
            RedisUpdate();
        }
        public void SendEmail()
        {
            //TODO
            //kapasite beklenen altında olur ise mail gidecek ve status false olacak
            //free smtp server bul ve test eet
        }
        public void UpdateCapacityWithCar()
        {
            //db'den standart miktarları getirecek atmye özel onun üzerine ekleme yapacak ve o sayıya getirecek şu an 1000
            List<ATM> atms = _context.ATMs.Include(atm => atm.Capacity).ToList();
            foreach (var item in atms)
            {
                item.Capacity.MevcutBanknot10 = 1000;
                item.Capacity.MevcutBanknot20 = 1000;
                item.Capacity.MevcutBanknot50 = 1000;
                item.Capacity.MevcutBanknot100 = 1000;
                item.Capacity.MevcutBanknot200 = 1000;
                _context.Capacities.Update(item.Capacity);
                item.Status = true;
                _context.ATMs.Update(item);
            }
           
            _context.SaveChanges();
            RedisUpdate();
        }
        public void AddUser(UserLogin user)
        {
            _context.User.Add(user);
            _context.SaveChanges();
        }
        public void Update(UserLogin user)
        {
            _context.User.Update(user);
            _context.SaveChanges();
        }
        public UserLogin GetUser(string email,string pw)
        {

            return _context.User.FirstOrDefault(u => u.email == email && pw == u.password);
        }
        public void DeleteUser(int id)
        {
            UserLogin user = _context.User.FirstOrDefault(o => o.id == id);
            if (user != null)
            {
                _context.User.Remove(user);
                _context.SaveChanges();
            }
        }
        public string? ValidateToken(string token)
        {
            if (token == null) 
            return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("Secret phaseSecret phaseSecret phaseSecret phase");
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userEmail = jwtToken.Claims.First(x => x.Type == "email").Value;

            // return user id from JWT token if validation successful
            return userEmail;
        }
        catch
        {
            // return null if validation fails
            return null;
        }
        }
    }
}

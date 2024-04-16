using apigplx.ModelVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DemoGPLX.Models
{
    public static class QuestionUtil
    {
        private static List<Cau> lsCau = GetAllQuestion();
        private static List<Hang> lsHang = GetAllHang();
        private static List<Chuong> lsChuong = GetAllChuong();
        private static bool needToReload = true;
        private static DbGplxContext context = GetContext();
        public static void restoreDB()
        {
            needToReload = true;
        }
        public static Cau newQuestion()
        {
            Cau cau = new Cau();
            cau.IdChuong = 1;
            cau.IdCau = GetAllQuestion().Count + 1;
            cau.Stt = cau.IdCau;
            cau.HangCaus = new List<HangCau>();
            cau.Ttcaus = new List<Ttcau>() { new Ttcau() { Diemliet = false, Hinhcauhoi = new byte[0] } };
            cau.Dapans = new List<Dapan>();
            return cau;
        }

        public static List<Chuong> GetAllChuong()
        {
            if (needToReload || lsChuong==null)
            {
               lsChuong = new DbGplxContext().Chuongs.ToList();
            }
            return lsChuong;
        }

        public static List<Hang> GetAllHang()
        {
            if (needToReload || lsHang == null)
            {
                lsHang = new DbGplxContext().Hangs.ToList();
            }
            return lsHang;
        }

        public static List<Cau> GetAllQuestion()
        {
            if (needToReload)
            {
                DbGplxContext db = new DbGplxContext();
                lsCau = db.Caus
               .Include(c => c.Dapans)
               .Include(c => c.IdChuongNavigation)
               .Include(c => c.Ttcaus)
               .Include(c => c.HangCaus)
               .Select(c => new Cau
               {
                   IdChuong = c.IdChuong,
                   Stt = c.Stt,
                   IdCau = c.IdCau,
                   Ttcaus = c.Ttcaus.Select(t => new Ttcau
                   { 
                       Hinhcauhoi = t.Hinhcauhoi == new byte[0] ? null : new byte[] { t.Hinhcauhoi[0] },
                       Cauhoi = t.Cauhoi,
                       Diemliet = t.Diemliet,
                       Goiy = t.Goiy,
                       IdTtcau = t.IdTtcau,
                       IdCau = t.IdCau
                   }).ToList(),
                   Dapans = c.Dapans.Select(d => new Dapan
                   {
                       IdCau = d.IdCau,
                       IdDapan = d.IdDapan,
                       Dapan1 = d.Dapan1,
                       Dapandung = d.Dapandung,

                   }).ToList(),
                   IdChuongNavigation = new Chuong
                   {
                       IdChuong = c.IdChuongNavigation.IdChuong,
                       ThongTinChuong = c.IdChuongNavigation.ThongTinChuong,
                   },
                   HangCaus = c.HangCaus.Select(h => new HangCau
                   {
                       IdHang = h.IdHang,
                       IdCau = h.IdCau,
                   }).ToList(),
               }).ToList();
                needToReload = false;
            }
            return lsCau;
        }

        public static Cau GetQuestionWithID(int id)
        {
            using (var dbContext = new DbGplxContext())
            {
                return dbContext.Caus
                    .Where(e => e.IdCau == id)
                    .Include(c => c.Dapans)
                    .Include(c => c.IdChuongNavigation)
                    .Include(c => c.Ttcaus)
                    .Include(c => c.HangCaus)
                    .FirstOrDefault();
            }
        }

        public static Cau GetQuestionWithoutImage(int id)
        {
            using (var dbContext = new DbGplxContext())
            {
                return dbContext.Caus
                    .Where(e => e.IdCau == id)
                    .Include(c => c.Dapans)
                    .Include(c => c.IdChuongNavigation)
                    .Include(c => c.Ttcaus)
                    .Include(c => c.HangCaus)
                    .Select(e => new Cau
                    {
                        IdChuong = e.IdChuong,
                        Stt = e.Stt,
                        IdCau = e.IdCau,
                        Ttcaus = e.Ttcaus.Select(t => new Ttcau
                        {
                            Hinhcauhoi = new byte[0],
                            Cauhoi = t.Cauhoi,
                            Diemliet = t.Diemliet,
                            Goiy = t.Goiy,
                            IdTtcau = t.IdTtcau,
                            IdCau = t.IdCau
                        }).ToList(),
                        Dapans = e.Dapans.Select(d => new Dapan
                        {
                            IdCau = d.IdCau,
                            IdDapan = d.IdDapan,
                            Dapan1 = d.Dapan1,
                            Dapandung = d.Dapandung,
                        }).ToList(),
                        IdChuongNavigation = new Chuong
                        {
                            IdChuong = e.IdChuongNavigation.IdChuong,
                            ThongTinChuong = e.IdChuongNavigation.ThongTinChuong,
                        },
                        HangCaus = e.HangCaus.Select(h => new HangCau
                        {
                            IdHang = h.IdHang,
                            IdCau = h.IdCau,
                        }).ToList(),
                    })
                    .FirstOrDefault();
            }

        }

        public static List<Cau> GetDataWithID(int id)
        {
            return (List<Cau>)GetAllQuestion().
                            Where(question => question.HangCaus.
                            Any(hangCau => hangCau.IdHang == id)).
                            ToList();
        }
        public static List<Cau> CreateNewTest(int id)
        {
            Random rd = new Random();
            List<Cau> lsCauHoi = new List<Cau>();
            try
            {
                List<Hang> lsHangChuong = new DbGplxContext().Hangs.Include(e => e.HangChuongs).Where(e => e.HangChuongs.Any(e => e.IdHang == id)).ToList();
                List<Cau> tempCauHang = GetAllQuestion().Where(e => e.HangCaus.Any(e => e.IdHang == id)).ToList();
                List<Hang> lsHang = new DbGplxContext().Hangs.ToList();
                if (lsHang.Where(e => e.Thongtin == "B1").First().IdHang > id) // Các hạng A1 A2 A3 A4
                {
                    foreach (HangChuong hc in lsHangChuong.ElementAt(0).HangChuongs.ToList())
                    {
                        List<Cau> tempLs = tempCauHang.Where(e => e.IdChuong == hc.IdChuong).ToList();
                        if (hc.IdChuong == 6) {
                            tempLs = tempCauHang.Where(e => e.IdChuong == hc.IdChuong || e.IdChuong == hc.IdChuong + 1).ToList();
                        }
                        int loadCC = hc.Soluong;
                        while (loadCC != 0)
                        {
                            int tempIndex = rd.Next(tempLs.Count);
                            if (!tempLs.ElementAt(tempIndex).Ttcaus.ElementAt(0).Diemliet && !lsCauHoi.Contains(tempLs.ElementAt(tempIndex)))
                            {
                                lsCauHoi.Add(tempLs.ElementAt(tempIndex));
                                loadCC--;
                            }
                        }
                    }
                    List<Cau> tempLsParalysis = tempCauHang.Where(e => e.Ttcaus.ElementAt(0).Diemliet).ToList();
                    lsCauHoi.Add(tempLsParalysis.ElementAt(rd.Next(tempLsParalysis.Count)));
                }
                else //Hạng B trở lên
                {
                    foreach (HangChuong hc2 in lsHangChuong.ElementAt(0).HangChuongs.ToList())
                    {
                        List<Cau> tempLs = tempCauHang.Where(e => e.IdChuong == hc2.IdChuong).ToList();
                        int loadCC = hc2.Soluong;
                        while (loadCC != 0)
                        {
                            int tempIndex = rd.Next(tempLs.Count);
                            if (!tempLs.ElementAt(tempIndex).Ttcaus.ElementAt(0).Diemliet && !lsCauHoi.Contains(tempLs.ElementAt(tempIndex)))
                            {
                                lsCauHoi.Add(tempLs.ElementAt(tempIndex));
                                loadCC--;
                            }
                        }
                    }
                    List<Cau> tempLsParalysis = tempCauHang.Where(e => e.Ttcaus.ElementAt(0).Diemliet).ToList();
                    lsCauHoi.Add(tempLsParalysis.ElementAt(rd.Next(tempLsParalysis.Count)));
                }
            }
            catch(Exception ex) { Console.WriteLine(ex.ToString()); }

            return lsCauHoi;
         
        }

        static public byte[] GetImageQuestion(int id)
        {
            try
            {
                return new DbGplxContext().Ttcaus.FirstOrDefault(e => e.IdCau == id).Hinhcauhoi;
            }
            catch (Exception ex)
            {
                return new byte[0];
            }
        }

        static public DbGplxContext GetContext()
        {
            if (context == null)
            {
                context = new DbGplxContext();
            }
            return context;
        }



        //Mobile
        public static List<CCau> GetDataForMobile(int id)
        {
            return GetAllQuestion()
             .Where(question => question.HangCaus.Any(hangCau => hangCau.IdHang == id))
             .Select(c => new CCau
             {
                 IdChuong = c.IdChuong,
                 Ttcaus = new CTtcau
                 {
                     Cauhoi = c.Ttcaus.First().Cauhoi,
                     Diemliet = c.Ttcaus.First().Diemliet,
                     Goiy = c.Ttcaus.First().Goiy,
                     Hinhcauhoi = GetImageQuestion(c.Ttcaus.First().IdCau),
                     IdTtcau = c.Ttcaus.First().IdTtcau
                 },
                 Dapans = c.Dapans.Select(ch => new CDapan
                 {
                     IdDapan = ch.IdDapan,
                     Dapan1 = ch.Dapan1,
                     Dapandung = ch.Dapandung
                 }).ToList(),
                 HangCaus = c.HangCaus.Select(ch => new CHangCau
                 {
                     IdHang = ch.IdHang
                 }).ToList(),
                 Stt = c.Stt
             }).ToList();
        }

        public static List<CHang> GetDataTypeForMobile()
        {
            return GetAllHang().Select(h => new CHang
            {
                IdHang = h.IdHang,
                Thongtin = h.Thongtin,
                Thongtinchitiet = h.Thongtinchitiet,
                Diemtoida = h.Diemtoida,
                Diemtoitheu = h.Diemtoitheu,
                Thoigianthi = h.Thoigianthi
            }).ToList();
        }

        public static List<CChuong> GetDataChapterForMobile()
        {
            return GetAllChuong().Select(h => new CChuong
            {
                IdChuong = h.IdChuong,
                ThongTinChuong = h.ThongTinChuong
            }).ToList();
        }
    }
}

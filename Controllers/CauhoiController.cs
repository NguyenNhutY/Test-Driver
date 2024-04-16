using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Dynamic;
using DemoGPLX.Models;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace DemoGPLX.Controllers
{
    public class CauhoiController : Controller
    {

        JsonSerializerOptions opt = new JsonSerializerOptions()
        {
            ReferenceHandler = ReferenceHandler.Preserve
        };
        static private String PWD = "admin";
        public IActionResult Index()
        {
            try
            {
                UserGplx dtu = JsonConvert.DeserializeObject<UserGplx>(HttpContext.Request.Cookies["UserGplx"]);
                if (CheckLogin(dtu.Username, dtu.Password))
                {
                    return View(QuestionUtil.GetAllQuestion());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("UserGplx");
            return RedirectToAction("Hoc", "LyThuyet");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Login")]
        public IActionResult Login_Post(string username, string password)
        {
            HttpContext.Response.Cookies.Delete("UserGplx");
            UserGplx data = new UserGplx();
            data.Username = username;
            data.Password = password;
            if(CheckLogin(username,password))
                HttpContext.Response.Cookies.Append("UserGplx", JsonConvert.SerializeObject(data));
            return RedirectToAction("Index");
        }

        public bool CheckLogin(string username, string password)
        {
            if(username=="admin" && password == PWD)
            {
                return true;
            }
            return false;
        }

        public IActionResult Sua(int id)
        {
            try
            {
                UserGplx dtu = JsonConvert.DeserializeObject<UserGplx>(HttpContext.Request.Cookies["UserGplx"]);
                if (!CheckLogin(dtu.Username, dtu.Password))
                {
                    return RedirectToAction("Login");
                }
            }
            catch { return RedirectToAction("Login"); }
            ViewBag.hang = QuestionUtil.GetAllHang();
            ViewBag.chuong = QuestionUtil.GetAllChuong();
            ViewBag.edit = true;
            return View(QuestionUtil.GetQuestionWithID(id));
        }

        public IActionResult Xoa(int id)
        {
            try
            {
                UserGplx dtu = JsonConvert.DeserializeObject<UserGplx>(HttpContext.Request.Cookies["UserGplx"]);
                if (!CheckLogin(dtu.Username, dtu.Password))
                {
                    return RedirectToAction("Login");
                }
            }
            catch { return RedirectToAction("Login"); }
            QuestionUtil.GetContext().HangCaus.RemoveRange(QuestionUtil.GetContext().HangCaus.Where(e => e.IdCau == id));
            QuestionUtil.GetContext().SaveChanges();
            return RedirectToAction("Index");
        }

       

        public IActionResult Them()
        {
            try
            {
                UserGplx dtu = JsonConvert.DeserializeObject<UserGplx>(HttpContext.Request.Cookies["UserGplx"]);
                if (!CheckLogin(dtu.Username, dtu.Password))
                {
                    return RedirectToAction("Login");
                }
            }
            catch { return RedirectToAction("Login"); }
            ViewBag.hang = QuestionUtil.GetAllHang();

            ViewBag.chuong = QuestionUtil.GetAllChuong();
            ViewBag.edit = false;
            return View("Sua",QuestionUtil.newQuestion());
        }

        [HttpPost]
        [ActionName("Them")]
        public async Task<IActionResult> Them_Post(IFormFile hinh)
        {
            try
            {
                UserGplx dtu = JsonConvert.DeserializeObject<UserGplx>(HttpContext.Request.Cookies["UserGplx"]);
                if (!CheckLogin(dtu.Username, dtu.Password))
                {
                    return RedirectToAction("Login");
                }
            }
            catch { return RedirectToAction("Login"); }
            int idCau = int.Parse(Request.Form["IdCau"].ToString());
            bool diemLiet = !string.IsNullOrEmpty(Request.Form["required"]);
            int idChuong = int.Parse(Request.Form["IdChuongNavigation.IdChuong"].ToString());
            string cauHoi = Request.Form["Cauhoi"].ToString(); 
            string goiY = Request.Form["Goiy"].ToString();
            string[] tthangs = Request.Form["tthang[]"];
            string[] correctValues = Request.Form["Correct[]"];
            string[] textValues = Request.Form["Text[]"];
            Cau cauNew = new Cau() { IdCau = idCau,IdChuong = idChuong, Stt = idCau};
            Cau cau = QuestionUtil.GetContext().Caus.Find(cauNew.IdCau);

            if (cau != null)
            {
                cau.IdChuong = idChuong;
                QuestionUtil.GetContext().SaveChanges();
            }
                        

            List<Ttcau> ttcau = new List<Ttcau>(){ new Ttcau() {IdTtcau=idCau, IdCau = idCau,Cauhoi = cauHoi,Diemliet = diemLiet,Goiy = goiY } };

            byte[] fileBytes = new byte[0];

            if (hinh != null && hinh.Length > 0)
            {
                 using (var memoryStream = new MemoryStream())
                 {
                    hinh.CopyTo(memoryStream);
                    fileBytes = memoryStream.ToArray();
                 }
            }
            ttcau.ElementAt(0).Hinhcauhoi = fileBytes;

            QuestionUtil.GetContext().Ttcaus.RemoveRange(QuestionUtil.GetContext().Ttcaus.Where(e=>e.IdCau == idCau).ToList());
            QuestionUtil.GetContext().Ttcaus.AddRange(ttcau);
            /** Add Dapans **/
            QuestionUtil.GetContext().Dapans.RemoveRange(QuestionUtil.GetContext().Dapans.Where(d => d.IdCau == cauNew.IdCau));
            List<Dapan> dapans = new List<Dapan>();
            for (int j = 0; j < textValues.Length; j++)
            {
                dapans.Add(new Dapan()
                {
                    IdCau = idCau,
                    IdDapan = j + 1,
                    Dapan1 = textValues[j],
                    Dapandung = bool.Parse(correctValues[j])
                });
            }

            QuestionUtil.GetContext().HangCaus.RemoveRange(QuestionUtil.GetContext().HangCaus.Where(h => h.IdCau == cau.IdCau));
            int i = 0;
            foreach (Hang h in QuestionUtil.GetAllHang())
            {
                if (tthangs[i++] == "1")
                {
                    QuestionUtil.GetContext().HangCaus.Add(new HangCau() { IdCau = idCau, IdHang = h.IdHang, Index = 1 });
                }
            }

            QuestionUtil.GetContext().Dapans.AddRange(dapans);
           await QuestionUtil.GetContext().SaveChangesAsync();
            QuestionUtil.restoreDB();
            return RedirectToAction("Index");
        }

        public IActionResult HangLaiXe()
        {
            try
            {
                UserGplx dtu = JsonConvert.DeserializeObject<UserGplx>(HttpContext.Request.Cookies["UserGplx"]);
                if (!CheckLogin(dtu.Username, dtu.Password))
                {
                    return RedirectToAction("Login");
                }
            }
            catch { return RedirectToAction("Login"); }
            List<Hang> hangList = QuestionUtil.GetContext().Hangs.ToList();
            return View(hangList);
        }


        public IActionResult ThemHang()
        {
            try
            {
                UserGplx dtu = JsonConvert.DeserializeObject<UserGplx>(HttpContext.Request.Cookies["UserGplx"]);
                if (!CheckLogin(dtu.Username, dtu.Password))
                {
                    return RedirectToAction("Login");
                }
            }
            catch { return RedirectToAction("Login"); }

            ViewBag.Hang = QuestionUtil.GetAllHang();
            ViewBag.Chuong = QuestionUtil.GetAllChuong();
            ViewBag.HangChuong = QuestionUtil.GetContext().HangChuongs.ToList();
            return View(new List<HangChuong>() {
                new HangChuong() {
                    IdChuongNavigation = new Models.Chuong(), IdHangNavigation = new Hang()
                } 
            });
        }

        [HttpPost]
        public IActionResult ThemHang(HangChuong hc)
        {
            try
            {
                UserGplx dtu = JsonConvert.DeserializeObject<UserGplx>(HttpContext.Request.Cookies["UserGplx"]);
                if (!CheckLogin(dtu.Username, dtu.Password))
                {
                    return RedirectToAction("Login");
                }
            }
            catch { return RedirectToAction("Login"); }
            
            if (ModelState.IsValid)
            {
                ViewBag.Chuong = QuestionUtil.GetAllChuong();
                string[] ds_SoLuongChuong = Request.Form["Soluong[]"];
                if (ds_SoLuongChuong.Any(e => e.Trim() == ""))
                {
                    ModelState.AddModelError("", "Không để trống các mục số lượng câu");
                    return View(hc);
                }
                if (QuestionUtil.GetContext().Hangs.Find(hc.IdHangNavigation.IdHang)==null)
                {
                    QuestionUtil.GetContext().Hangs.Add(hc.IdHangNavigation);
                    QuestionUtil.GetContext().SaveChanges();

                    var hangChuongDelete = QuestionUtil.GetContext().HangChuongs.Where(e => e.IdHang == hc.IdHangNavigation.IdHang).ToList();
                    QuestionUtil.GetContext().HangChuongs.RemoveRange(hangChuongDelete);

                    for(int i = 0; i < ViewBag.Chuong.Count;i++)
                    {
                        QuestionUtil.GetContext().HangChuongs.Add(new HangChuong() { IdChuong = ViewBag.Chuong[i].IdChuong, IdHang = hc.IdHangNavigation.IdHang, Soluong = int.Parse(ds_SoLuongChuong[i]) });
                    }
                    QuestionUtil.GetContext().SaveChanges();
                    QuestionUtil.restoreDB();
                    return RedirectToAction("HangLaiXe");
                }
                else
                {
                    ModelState.AddModelError("","Mã hạng bị trùng");
                    return View(hc);
                }
            }  
            return View();
        }

        public IActionResult SuaHang(int id)
        {
            try
            {
                UserGplx dtu = JsonConvert.DeserializeObject<UserGplx>(HttpContext.Request.Cookies["UserGplx"]);
                if (!CheckLogin(dtu.Username, dtu.Password))
                {
                    return RedirectToAction("Login");
                }
            }
            catch { return RedirectToAction("Login"); }
            ViewBag.Chuong = QuestionUtil.GetAllChuong();
            ViewBag.HangChuong = QuestionUtil.GetContext().HangChuongs.ToList();
            ViewBag.edit = true;
            ViewBag.Hang = QuestionUtil.GetAllHang().FirstOrDefault(e=>e.IdHang == id);
            List<HangChuong> model = QuestionUtil.GetContext().HangChuongs.Include(e => e.IdChuongNavigation).Include(e => e.IdHangNavigation).Where(e => e.IdHang == id).OrderBy(e=>e.IdChuong).ToList();
            return View("ThemHang", model);
        }

        [HttpPost]
        [ActionName("SuaHang")]
        public IActionResult SuaHang_Post(HangChuong hc)
        {
            try
            {
                UserGplx dtu = JsonConvert.DeserializeObject<UserGplx>(HttpContext.Request.Cookies["UserGplx"]);
                if (!CheckLogin(dtu.Username, dtu.Password))
                {
                    return RedirectToAction("Login");
                }
            }
            catch { return RedirectToAction("Login"); }

            ViewBag.Chuong = QuestionUtil.GetAllChuong();
            string[] ds_SoLuongChuong = Request.Form["Soluong[]"];
            if (ds_SoLuongChuong.Any(e => e.Trim() == ""))
            {
                ModelState.AddModelError("", "Không để trống các mục số lượng câu");
                return View(hc);
            }
            if (ModelState.IsValid)
            {
                if (QuestionUtil.GetAllHang().FirstOrDefault(e=>e.IdHang == hc.IdHangNavigation.IdHang) != null)
                {
                    Hang oldHang = QuestionUtil.GetContext().Hangs.Find(hc.IdHangNavigation.IdHang);
                    oldHang.Thongtin = hc.IdHangNavigation.Thongtin;
                    oldHang.Thoigianthi = hc.IdHangNavigation.Thoigianthi;
                    oldHang.Diemtoida = hc.IdHangNavigation.Diemtoida;
                    oldHang.Diemtoitheu = hc.IdHangNavigation.Diemtoitheu;
                    oldHang.Thongtinchitiet = hc.IdHangNavigation.Thongtinchitiet;

                    var hangChuongDelete = QuestionUtil.GetContext().HangChuongs.Where(e => e.IdHang == hc.IdHangNavigation.IdHang).ToList();
                    QuestionUtil.GetContext().HangChuongs.RemoveRange(hangChuongDelete);

                    for (int i = 0; i < ViewBag.Chuong.Count; i++)
                    {
                        QuestionUtil.GetContext().HangChuongs.Add(new HangChuong() { IdChuong = ViewBag.Chuong[i].IdChuong, IdHang = hc.IdHangNavigation.IdHang, Soluong = int.Parse(ds_SoLuongChuong[i]) });
                    }
                    QuestionUtil.GetContext().SaveChanges();
                    QuestionUtil.restoreDB();
                    return RedirectToAction("HangLaiXe");
                }
            }
            return View(hc);
        }

        public IActionResult XoaHang(int id)
        {
            try
            {
                UserGplx dtu = JsonConvert.DeserializeObject<UserGplx>(HttpContext.Request.Cookies["UserGplx"]);
                if (!CheckLogin(dtu.Username, dtu.Password))
                {
                    return RedirectToAction("Login");
                }
            }
            catch { return RedirectToAction("Login"); }

            if (!QuestionUtil.GetContext().Hangs.Any(e => e.HangCaus.Any(e=> e.IdHang==id)))
            {
                QuestionUtil.GetContext().HangChuongs.RemoveRange(QuestionUtil.GetContext().HangChuongs.Where(e => e.IdHang == id));
                QuestionUtil.GetContext().HangCaus.RemoveRange(QuestionUtil.GetContext().HangCaus.Where(e => e.IdHang == id));
                QuestionUtil.GetContext().SaveChanges();
                QuestionUtil.GetContext().Hangs.Remove(QuestionUtil.GetContext().Hangs.Find(id));
                QuestionUtil.GetContext().SaveChanges();
                QuestionUtil.restoreDB();

            }
            else
            {
                TempData["ErrorMessage"] = "Không thể xóa hạng này bao gồm "+ QuestionUtil.GetAllQuestion().Where(e => e.HangCaus.Any(hc => hc.IdHang == id)).Count() + " câu";
            }
            return RedirectToAction("HangLaiXe");
        }
        public IActionResult Chuong()
        {
            try
            {
                UserGplx dtu = JsonConvert.DeserializeObject<UserGplx>(HttpContext.Request.Cookies["UserGplx"]);
                if (!CheckLogin(dtu.Username, dtu.Password))
                {
                    return RedirectToAction("Login");
                }
            }
            catch { return RedirectToAction("Login"); }
            List<Chuong> chuongList = QuestionUtil.GetAllChuong();
            return View(chuongList);
        }

        public IActionResult ThemChuong()
        {
            try
            {
                UserGplx dtu = JsonConvert.DeserializeObject<UserGplx>(HttpContext.Request.Cookies["UserGplx"]);
                if (!CheckLogin(dtu.Username, dtu.Password))
                {
                    return RedirectToAction("Login");
                }
            }
            catch { return RedirectToAction("Login"); }
            return View();
        }

        [HttpPost]
        public IActionResult ThemChuong(Chuong chuong)
        {
            try
            {
                UserGplx dtu = JsonConvert.DeserializeObject<UserGplx>(HttpContext.Request.Cookies["UserGplx"]);
                if (!CheckLogin(dtu.Username, dtu.Password))
                {
                    return RedirectToAction("Login");
                }
            }
            catch { return RedirectToAction("Login"); }
            if (ModelState.IsValid)
            {
                //
                if (QuestionUtil.GetAllChuong().FirstOrDefault(e=>e.IdChuong == chuong.IdChuong) == null)
                {
                    QuestionUtil.GetContext().Chuongs.Add(chuong);
                    QuestionUtil.GetContext().SaveChanges();
                    QuestionUtil.restoreDB();
                    return RedirectToAction("Chuong");
                }
                else
                {
                    ModelState.AddModelError("", "Mã chương bị trùng");
                    return View(chuong);
                }
            }
            return View();
        }

        public IActionResult XoaChuong(int id)
        {
            try
            {
                UserGplx dtu = JsonConvert.DeserializeObject<UserGplx>(HttpContext.Request.Cookies["UserGplx"]);
                if (!CheckLogin(dtu.Username, dtu.Password))
                {
                    return RedirectToAction("Login");
                }
            }
            catch { return RedirectToAction("Login"); }
            if (!QuestionUtil.GetContext().Chuongs.Any(e => e.HangChuongs.Any(e => e.IdChuong == id)) && !QuestionUtil.GetContext().Chuongs.Any(e => e.Caus.Any(e => e.IdChuong == id)))
            {
                QuestionUtil.GetContext().Chuongs.Remove(QuestionUtil.GetContext().Chuongs.FirstOrDefault(e=> e.IdChuong == id));
                QuestionUtil.GetContext().SaveChanges();
                QuestionUtil.restoreDB();
            }
            else
            {
                TempData["ErrorMessage"] = "Không thể xóa chương này vì có "+QuestionUtil.GetAllQuestion().Where(e=>e.IdChuong == id).Count()+" câu.";
            }
            return RedirectToAction("Chuong");
        }

        public IActionResult SuaChuong(int id)
        {
            try
            {
                UserGplx dtu = JsonConvert.DeserializeObject<UserGplx>(HttpContext.Request.Cookies["UserGplx"]);
                if (!CheckLogin(dtu.Username, dtu.Password))
                {
                    return RedirectToAction("Login");
                }
            }
            catch { return RedirectToAction("Login"); }

            return View(QuestionUtil.GetAllChuong().FirstOrDefault(e=>e.IdChuong == id));
        }
        [HttpPost]
        [ActionName("SuaChuong")]
        public IActionResult SuaChuong_Post(Chuong c)
        {
            try
            {
                UserGplx dtu = JsonConvert.DeserializeObject<UserGplx>(HttpContext.Request.Cookies["UserGplx"]);
                if (!CheckLogin(dtu.Username, dtu.Password))
                {
                    return RedirectToAction("Login");
                }
            }
            catch { return RedirectToAction("Login"); }

            if(QuestionUtil.GetContext().Chuongs.Find(c.IdChuong)!=null)
            {
                Chuong chuong = QuestionUtil.GetContext().Chuongs.Find(c.IdChuong);
                chuong.ThongTinChuong = c.ThongTinChuong;
                QuestionUtil.GetContext().SaveChanges();
                QuestionUtil.restoreDB();
            }

            return RedirectToAction("Chuong");
        }

        [HttpGet]
        public IActionResult GetDataForMobile(int type)
        {
            return Json(QuestionUtil.GetDataForMobile(type));
        }

        [HttpGet]
        public IActionResult GetTypeForMobile()
        {
            return Json(QuestionUtil.GetDataTypeForMobile());
        }

        [HttpGet]
        public IActionResult GetChapterForMobile()
        {
            return Json(QuestionUtil.GetDataChapterForMobile());
        }
    }
}

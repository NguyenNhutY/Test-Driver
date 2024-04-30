using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json.Serialization;
using System.Text.Json;
using DemoGPLX.Models;
using Newtonsoft.Json;
using apigplx.ModelVM;

namespace DemoGPLX.Controllers
{
    public class LyThuyetController : Controller
    {
        JsonSerializerOptions opt = new JsonSerializerOptions()
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.Preserve
        };

        CookieOptions options = new CookieOptions() { Expires = DateTime.Now.AddDays(1) };
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Hoc()
        {
            if (HttpContext.Request.Cookies["DataUserGPLX"] == null)
            {
                return RedirectToAction("ChonHang");
            }
            DataUser user = JsonConvert.DeserializeObject<DataUser>(HttpContext.Request.Cookies["DataUserGPLX"]);

            ViewBag.Cookies = user.Id;
            return View();
        }

        public IActionResult Thi()
        {
            if (HttpContext.Request.Cookies["DataUserGPLX"] == null)
            {
                return RedirectToAction("ChonHang");
            }
            DataUser user = JsonConvert.DeserializeObject<DataUser>(HttpContext.Request.Cookies["DataUserGPLX"]);
            ViewBag.Cookies = user.Id;
            ViewBag.setTest = true;
            ViewBag.time = new DbGplxContext().Hangs.Where(e => e.IdHang == int.Parse(user.Id)).FirstOrDefault().Thoigianthi;
            return View();
        }

        public IActionResult CauHaySai()
        {
            DataUser user = null;
            try
            {
                user = JsonConvert.DeserializeObject<DataUser>(HttpContext.Request.Cookies["DataUserGPLX"]);
            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }

            if (user == null)
                return RedirectToAction("ChonHang");
            ViewBag.data = user;
            return View(user);

        }

        public IActionResult XoaDuLieuCauSai()
        {
            DataUser data = JsonConvert.DeserializeObject<DataUser>(HttpContext.Request.Cookies["DataUserGPLX"]);
            data.CauSais = new List<int>();
            HttpContext.Response.Cookies.Append("DataUserGPLX", JsonConvert.SerializeObject(data), options);
            return RedirectToAction("CauHaySai");
        }

        public IActionResult ChonHang()
        {
            ViewBag.hang = QuestionUtil.GetAllHang();
            return View(ViewBag.hang);
        }

        public IActionResult ChonHangThi(string id)
        {
            HttpContext.Response.Cookies.Delete("DataUserGPLX");
            DataUser data = new DataUser();
            data.Id = id;
            HttpContext.Response.Cookies.Append("DataUserGPLX", JsonConvert.SerializeObject(data), options);
            return RedirectToAction("Hoc");
        }

       [HttpPost]
        public JsonResult LayCauHoi(int id)
        {
            List<Cau> data = new List<Cau>();
            data = QuestionUtil.GetDataWithID(id);
            return Json(data, opt);
        }

        [HttpPost]
        public IActionResult ThemCau(int id, int index)
        {
            try
            {
                DataUser data = JsonConvert.DeserializeObject<DataUser>(HttpContext.Request.Cookies["DataUserGPLX"]);

                if (data.Caus.IndexOf(id) == -1)
                {
                    data.Caus.Add(id);
                    data.LuaChons.Add(index);
                }

                // Update user data
                HttpContext.Response.Cookies.Append("DataUserGPLX", JsonConvert.SerializeObject(data), options);

                // Optionally, return the updated data as JSON
                return Json(data);
            }
            catch (Exception )
            {
                // Handle exceptions appropriately
                return BadRequest("An error occurred while updating user data.");
            }
        }

        [HttpPost]
        public IActionResult ThemCauSai(int id)
        {
            try
            {
                DataUser data = JsonConvert.DeserializeObject<DataUser>(HttpContext.Request.Cookies["DataUserGPLX"]);

                if (data.CauSais.IndexOf(id) == -1)
                    data.CauSais.Add(id);
                // Update user data
                HttpContext.Response.Cookies.Append("DataUserGPLX", JsonConvert.SerializeObject(data));

                // Optionally, return the updated data as JSON
                return Json(data);
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return BadRequest("An error occurred while updating user data.");
            }
        }

        public IActionResult LayCauSai()
        {
            if (HttpContext.Request.Cookies["DataUserGPLX"] == null)
            {
                return RedirectToAction("ChonHang");
            }
            DataUser user = JsonConvert.DeserializeObject<DataUser>(HttpContext.Request.Cookies["DataUserGPLX"]);
            List<Cau> lsCauHaySai = new List<Cau>();
            foreach (int i in user.CauSais)
            {
                lsCauHaySai.Add(QuestionUtil.GetQuestionWithoutImage(i));
            }
            return Json(lsCauHaySai, opt);
        }

        [HttpPost]
        public IActionResult LayDeThiMoi(int id)
        {
            var questions = QuestionUtil.CreateNewTest(id);

            var random = new Random();
            questions = questions.OrderBy(x => random.Next()).ToList();

            return Json(questions, opt);
        }

        [HttpPost]
        public IActionResult LayCauDaLam()
        {
            List<List<int>> ls = new List<List<int>>() {
                new List<int>(JsonConvert.DeserializeObject<DataUser>(HttpContext.Request.Cookies["DataUserGPLX"]).Caus),
                new List<int>(JsonConvert.DeserializeObject<DataUser>(HttpContext.Request.Cookies["DataUserGPLX"]).LuaChons)
            };


            return Json(ls);
        }



        [HttpPost]
        public IActionResult LayHinhCauHoi(int id)
        {
            return Json(QuestionUtil.GetImageQuestion(id));
        }


        public IActionResult SearchQuestion(string search, bool choose = false)
        {
            search = AppUtil.ToLowerCaseNonAccentVietnamese(search.ToLower());
            List<Cau> lsAllQues = QuestionUtil.GetAllQuestion();
            List<Cau> lsQues = new List<Cau>();
            if (!choose)
            {
                foreach (Cau c in lsAllQues)
                {
                    if (AppUtil.ToLowerCaseNonAccentVietnamese(c.Ttcaus.ElementAt(0).Cauhoi.ToLower()).Contains(search))
                    {
                        lsQues.Add(c);
                        continue;
                    }
                    else
                    {
                        foreach (Dapan dapan in c.Dapans)
                        {
                            if ((AppUtil.ToLowerCaseNonAccentVietnamese(dapan.Dapan1.ToLower()).Contains(search)))
                            {
                                lsQues.Add(c);
                                break;
                            }
                        }
                    }
                }
            }
            return Json(lsQues, opt);
        }
    }
}
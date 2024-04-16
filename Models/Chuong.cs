using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DemoGPLX.Models;

public partial class Chuong
{
    [Required(ErrorMessage = "Bắt buộc nhập mã chương")]
    [DisplayName("Mã")]
    public int IdChuong { get; set; }

    [DisplayName("THÔNG TIN")]
    public string ThongTinChuong { get; set; } = null!;
    public virtual ICollection<Cau> Caus { get; set; } = new List<Cau>();

    public virtual ICollection<HangChuong> HangChuongs { get; set; } = new List<HangChuong>();
}

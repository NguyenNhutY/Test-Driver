using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DemoGPLX.Models;
public partial class Hang
{
    [Display(Name = "Mã hạng")]
    [Required(ErrorMessage = "Thông tin không được để trống")]
    public int IdHang { get; set; }

    [Display(Name = "Thông tin")]
    [Required(ErrorMessage = "Thông tin không được để trống")]
    public string Thongtin { get; set; } = null!;

    [Display(Name = "Thông tin chi tiết")]
    [Required(ErrorMessage = "Thông tin chi tiết không được để trống")]
    public string Thongtinchitiet { get; set; } = null!;

    [Display(Name = "Điểm tối đa")]
    public int Diemtoida { get; set; }

    [Display(Name = "Điểm tối thiểu")]
    public int Diemtoitheu { get; set; }

    [Display(Name = "Thời gian thi")]
    [Required(ErrorMessage = "Thời gian không được trống")]
    public int Thoigianthi { get; set; }

    public virtual ICollection<De> Des { get; set; } = new List<De>();

    public virtual ICollection<HangCau> HangCaus { get; set; } = new List<HangCau>();

    [Display(Name = "Cấu trúc hạng")]
    public virtual ICollection<HangChuong> HangChuongs { get; set; } = new List<HangChuong>();
}

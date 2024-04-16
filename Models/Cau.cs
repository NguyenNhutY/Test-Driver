using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DemoGPLX.Models;
public partial class Cau
{
    [Required(ErrorMessage = ("Bắt buộc nhập mã câu"))]
    [Display(Name = "Mã câu")]
    public int IdCau { get; set; }

    [Display(Name = "STT")]
    public int Stt { get; set; }

    [Display(Name = "Mã chương")]
    [Required(ErrorMessage = ("Bắt buộc chọn chương"))]
    public int IdChuong { get; set; }

    [Required(ErrorMessage = ("Câu hỏi phải có đáp án!!!"))]

    [Display(Name = "Đáp án câu hỏi")]
    public virtual ICollection<Dapan> Dapans { get; set; } = new List<Dapan>();

    [Display(Name = "Các hạng được sử dụng cho câu")]
    public virtual ICollection<HangCau> HangCaus { get; set; } = new List<HangCau>();

    [Display(Name = "Chương")]
    public virtual Chuong IdChuongNavigation { get; set; } = null!;

    [Display(Name = "Thông tin câu hỏi")]
    public virtual ICollection<Ttcau> Ttcaus { get; set; } = new List<Ttcau>();

    public virtual ICollection<De> IdDes { get; set; } = new List<De>();
}

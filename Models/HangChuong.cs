using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DemoGPLX.Models;

public partial class HangChuong
{
    [DisplayName("Số lượng câu: ")]
    [Required(ErrorMessage = "Bắt buộc nhập số lượng")]
    public int Soluong { get; set; }

    public int IdHang { get; set; }

    public int IdChuong { get; set; }

    public virtual Chuong IdChuongNavigation { get; set; } = null!;

    public virtual Hang IdHangNavigation { get; set; } = null!;
}

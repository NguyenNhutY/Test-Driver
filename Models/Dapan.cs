using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DemoGPLX.Models;

public partial class Dapan
{
    [DisplayName("Câu trả lời")]
    [Required(ErrorMessage = ("Bắt buộc nhập đáp án"))]
    public string Dapan1 { get; set; } = null!;

    [Required(ErrorMessage = ("Bắt buộc quy định kết quả cho đáp án (Đúng/Sai)"))]
    [DisplayName("Đáp án đúng")]
    public bool Dapandung { get; set; }

    public int IdDapan { get; set; }

    public int IdCau { get; set; }

    public virtual Cau IdCauNavigation { get; set; } = null!;
}

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DemoGPLX.Models;

public partial class Ttcau
{
    [DisplayName("Gợi ý: ")]
    public string Goiy { get; set; } = null!;

    [DisplayName("Câu điểm liệt: ")]
    public bool Diemliet { get; set; }

    public int IdTtcau { get; set; }

    [DisplayName("Câu hỏi: ")]
    public string Cauhoi { get; set; } = null!;
    [DisplayName("Hình ảnh: ")]
    public byte[]? Hinhcauhoi { get; set; }

    public int IdCau { get; set; }

    public virtual Cau IdCauNavigation { get; set; } = null!;
}

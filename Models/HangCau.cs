using System;
using System.Collections.Generic;

namespace DemoGPLX.Models;

public partial class HangCau
{
    public int IdHang { get; set; }

    public int IdCau { get; set; }

    public int? Index { get; set; }

    public virtual Cau IdCauNavigation { get; set; } = null!;

    public virtual Hang IdHangNavigation { get; set; } = null!;
}

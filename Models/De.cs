using System;
using System.Collections.Generic;

namespace DemoGPLX.Models;

public partial class De
{
    public int IdDe { get; set; }

    public int? IdHang { get; set; }

    public virtual Hang? IdHangNavigation { get; set; }

    public virtual ICollection<Cau> IdCaus { get; set; } = new List<Cau>();
}

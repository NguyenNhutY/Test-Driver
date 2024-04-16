using System;
using System.Collections.Generic;

namespace apigplx.ModelVM;
public partial class CCau
{
    public int IdCau { get; set; }
    public int Stt { get; set; }
    public int IdChuong { get; set; }
    public List<CDapan> Dapans { get; set; } = new List<CDapan>();
    public  List<CHangCau> HangCaus { get; set; } = new List<CHangCau>();
    public CTtcau Ttcaus { get; set; } = new CTtcau();
}

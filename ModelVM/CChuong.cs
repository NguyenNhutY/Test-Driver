using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace apigplx.ModelVM;

public partial class CChuong
{
    public int IdChuong { get; set; }
    public string ThongTinChuong { get; set; } = null!;
}

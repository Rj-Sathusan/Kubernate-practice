using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevPro.Application.Common
{
    public class ApiResponse
    {
        public int Code { get; set; }
        public bool Status { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
    }
}

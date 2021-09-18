using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneDirectory.Common.Helper
{
    public class Response<T> where T : class, new()
    {
        public int Status { get; set; }
        public bool isSuccess { get; set; }
        public string Message { get; set; } = "";    
        public T Data { get; set; }
        public List<T> List { get; set; }

    }
}

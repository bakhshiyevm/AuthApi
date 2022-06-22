using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthApi.Models
{
    public class Response
    {
        public Guid Guid { get; set; }

        public DateTime DateTime { get; set; }

        public Response()
        {
            Guid = Guid.NewGuid();
            DateTime = DateTime.Now;
        }

    }

    public class Response<T> : Response
    {
        public T Data { get; set; }
        public Response(T data)
        {
            Data = data;
        }

    }
}

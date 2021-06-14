using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Services.StringProcService
{
    public interface IStringProcService
    {
        string this[string key] { get; set; }
    }
}

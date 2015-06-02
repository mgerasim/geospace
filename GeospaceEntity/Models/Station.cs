using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeospaceEntity.Models
{
    public class Station
    {
        int ID;
        int Code;
        DateTime created_at { get; set; }
        DateTime updated_at { get; set; }
    }
}

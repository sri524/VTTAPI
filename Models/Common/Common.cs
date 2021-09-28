using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VTTAPI.Models.Common
{
    public class Common
    {
        public class SearchString
        {
            public string strSearchString { get; set; }
        }
        public class TDetails
        {
            public string courseId { get; set; }
            public int tournamentid { get; set; }
            public int scorecardGroupId { get; set; }
        }
    }
}

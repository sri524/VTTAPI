using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using VTTAPI.Models;
using VTTAPI.Models.Tournaments;

namespace VTTAPI.Controllers.Tournaments
{
    [ApiController]
    public class TournamentController : ControllerBase
    {
        [Route("api/CUDTournament")]
        [HttpPost]
        public string[] CUDTournamentActions(Tournament TInfo)
        {
            string[] strErrMessage = new string[2];
            try
            {
                strErrMessage = Tournament.CUDTournamentActions(TInfo);
            }
            catch (Exception ex)
            {
                strErrMessage[0] = Convert.ToString(0);
                strErrMessage[1] = ex.ToString();
            }
            return strErrMessage;
        }
        [Route("api/getTournaments")]
        [HttpGet]
        public string getTournaments(VTTAPI.Models.Common.Common.SearchString searchExp)
        {
            string retString = string.Empty;
            try
            {
                retString = Tournament.getTournaments(searchExp.strSearchString);
            }
            catch (Exception ex)
            {
                retString = ex.ToString();
            }
            return retString;
        }
        [Route("api/GetGolfCourses/")]
        [HttpGet]
        public string getGolfCourse()
        {
            string retString = string.Empty;
            try
            {
                retString = Tournament.getGolfCourse();
            }
            catch (Exception ex)
            {
                retString = ex.ToString();
            }
            return retString;
        }
        [Route("api/getCourseHoles/")]
        [HttpGet]
        public string getCourseHolesInfo(VTTAPI.Models.Common.Common.SearchString searchExp)
        {
            string retString = string.Empty;
            try
            {
                retString = Tournament.getCourseHoles(searchExp.strSearchString);
            }
            catch (Exception ex)
            {
                retString = ex.ToString();
            }
            return retString;
        }
        [Route("api/getCourseTags/")]
        [HttpGet]
        public string getCourseTagsInfo(VTTAPI.Models.Common.Common.SearchString searchExp)
        {
            string retString = string.Empty;
            try
            {
                retString = Tournament.getCourseTags(searchExp.strSearchString);
            }
            catch (Exception ex)
            {
                retString = ex.ToString();
            }
            return retString;
        }
        [Route("api/SaveTournamentdetailschedule")]
        [HttpPost]
        public string[] SaveTournamentdetailschedule(List<tournamentdetailschedule> tournamentdetails)
        {
            string[] strErrMessage = new string[2];
            try
            {
                strErrMessage = tournamentdetailschedule.SaveTournamentdetailschedule(tournamentdetails);
            }
            catch (Exception ex)
            {
                strErrMessage[0] = Convert.ToString(0);
                strErrMessage[1] = ex.ToString();
            }
            return strErrMessage;
        }
        [Route("api/gettournamentscheduledetails")]
        [HttpGet]
        public string gettournamentscheduledetails(VTTAPI.Models.Common.Common.TDetails tdetails)
        {
            string retString = string.Empty;
            try
            {
                retString = tournamentdetailschedule.gettournamentscheduledetails(tdetails.tournamentid, tdetails.courseId, tdetails.scorecardGroupId);
            }
            catch (Exception ex)
            {
                retString = ex.ToString();
            }
            return retString;
        }
    }
}

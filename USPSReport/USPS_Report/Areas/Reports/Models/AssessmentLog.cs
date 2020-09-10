using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportsDatabase;
using System.ComponentModel.DataAnnotations;


namespace USPS_Report.Areas.Reports.Models
{
    public class AddAssessmentLogInfo
    {
        public static int AddAssessmentLog(AssessmentLog _vm)
        {
            int id = 0;
            var components = HttpContext.Current.User.Identity.Name.Split('\\');

            var userName = components.Last();
            try
            {
                using (IntranetEntities _db = new IntranetEntities())
                {
                    assessment_log _rec = new assessment_log();

                    _rec.Account_Number = _vm.Account;
                    _rec.Initial_Assessment = _vm.NewAssessment != true ? Convert.ToByte(0) : Convert.ToByte(1);
                    _rec.Update_Assessment = _vm.UpdateAssessment != true ? Convert.ToByte(0) : Convert.ToByte(1);
                    _rec.Update_PCU = _vm.UpdatePCU != true ? Convert.ToByte(0) : Convert.ToByte(1);
                    _rec.OneTime_PA = _vm.OneTimePA != true ? Convert.ToByte(0) : Convert.ToByte(1);
                    _rec.Initial_PA_Process = _vm.InitPAProcess != true ? Convert.ToByte(0) : Convert.ToByte(1);
                    _rec.POS_RWO_Created = _vm.POSorRWOcreated != true ? Convert.ToByte(0) : Convert.ToByte(1);
                    _rec.Faxes_to_State = _vm.FaxesToState != true ? Convert.ToByte(0) : Convert.ToByte(1);
                    _rec.LM_Letter = _vm.LMletter != true ? Convert.ToByte(0) : Convert.ToByte(1);
                    _rec.Date = DateTime.Now;
                    _rec.User_ID = userName;



                    _db.assessment_log.Add(_rec);
                    try { _db.SaveChanges(); }
                    catch (Exception ex) { var msg = ex.Message; }
                    id = _rec.ID;


                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }


            return id;

        }

        public static AssessmentLog getAssessmentLog(int? id)
        {
            AssessmentLog rec = new AssessmentLog();
            try
            {
                using (IntranetEntities _db = new IntranetEntities())
                {
                    rec = (from t in _db.assessment_log
                           where t.ID == id
                           select new AssessmentLog
                           {
                               Account = t.Account_Number,
                               NewAssessment = t.Initial_Assessment != 1 ? false : true,
                               UpdateAssessment = t.Update_Assessment != 1 ? false : true,
                               UpdatePCU = t.Update_PCU != 1 ? false : true,
                               OneTimePA = t.OneTime_PA != 1 ? false : true,
                               InitPAProcess = t.Initial_PA_Process != 1 ? false : true,
                               POSorRWOcreated = t.POS_RWO_Created != 1 ? false : true,
                               FaxesToState = t.Faxes_to_State != 1 ? false : true,
                               LMletter = t.LM_Letter != 1 ? false : true,

                           }).Take(1).SingleOrDefault();


                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }


            return rec;

        }
    }
    public class AssessmentLog
    {
        [Required]
        public int? Account { get; set; }
        public bool NewAssessment { get; set; }
        public bool UpdateAssessment { get; set; }
        public bool UpdatePCU { get; set; }
        public bool OneTimePA { get; set; }
        public bool InitPAProcess { get; set; }
        public bool POSorRWOcreated { get; set; }
        public bool FaxesToState { get; set; }
        public bool LMletter { get; set; }

    }
    public class AssessmentLogList
    {
        public AssessmentLog assessmentLog { get; set; }
        public AssessmentLog assessmentLogRec { get; set; }
    }
    public class AssessmentCount
    {
        public int Count { get; set; }

        public string User { get; set; }


    }
    public class AssessmentCountVM
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public IList<AssessmentCount> GetAssessmentCountReport { get; set; }

    }





    public class ReportAssessmentCount
    {
        public static IList<AssessmentCount> GetAssessmentCountByDate(DateTime? _startDt, DateTime? _endDt)
        {


            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {


                var _list = (from o in _db.tbl_Operator_Table
                             join c in _db.tbl_Clinical_Assessments
                             on o.ID equals c.ID_CreateBy
                             where c.CreateDate >= _startDt && c.CreateDate <= _endDt

                             select new { o.LegalName, o.ID }

                             ).ToList();


                var _list1 = (from a in _list
                              group a by new { a.LegalName, a.ID } into t
                              select new AssessmentCount
                              {

                                  User = t.Key.LegalName,
                                  Count = t.Count()
                              }).OrderBy(t=>t.User).ToList();



                return _list1;

            }



        }
    }

    public class AssessmentLogModel
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public int? Account { get; set; }
        public byte? InitAssessment { get; set; }
        public byte? UpdateAssessment { get; set; }
        public byte? UpdatePCU { get; set; }
        public byte? OneTimePA { get; set; }
        public byte? InitPAProcess { get; set; }
        public byte? POSorRWOcreated { get; set; }
        public byte? FaxesToState { get; set; }
        public byte? LMletter { get; set; }
      //  public Nullable<bool> LMletter { get; set; }

        public DateTime? Date { get; set; }

    }

    public class AssessmentLogVM
    {

     
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Account { get; set; }
        //public string UserID { get; set; }

        public string User_ID { get; set; }
        public List<AssessmentLogModel> GetAssessment { get; set; }

        public System.Web.Mvc.SelectList UserIDList { get; set; }

    }
    public class UserIdsVm
    {
        public string User_ID { get; set; }
        public string userName { get; set; }

    }

    public class GetAssessmentLogReport
    {
        public static IList<AssessmentLogModel> GetAssessmentLog(DateTime? _startDt, DateTime? _endDt, int? account, string user)
        {
            try
            {


                using (IntranetEntities _db = new IntranetEntities())
                {
                    IList<AssessmentLogModel> _vm = new List<AssessmentLogModel>();
                   // AssessmentLogModel tableRec = new AssessmentLogModel();

                    if (account != null)
                    {
                        if (user != null)
                        {
                            _vm = _db.assessment_log.Where(a => a.Account_Number == account && a.User_ID == user)
                                            .Select(a => new AssessmentLogModel
                                            {

                                                ID = a.ID,
                                                UserID = a.User_ID,
                                                Account = a.Account_Number,
                                                InitAssessment = a.Initial_Assessment,
                                                UpdateAssessment = a.Update_Assessment,
                                                UpdatePCU = a.Update_PCU,
                                                OneTimePA = a.OneTime_PA,
                                                InitPAProcess = a.Initial_PA_Process,
                                                POSorRWOcreated = a.POS_RWO_Created,
                                                FaxesToState = a.Faxes_to_State,
                                                LMletter = a.LM_Letter,
                                                Date = a.Date
                                            }).ToList();

                            return _vm.ToList();
                        }
                        else
                        {
                            _vm = _db.assessment_log.Where(a => a.Account_Number == account)
                                          .Select(a => new AssessmentLogModel
                                          {

                                              ID = a.ID,
                                              UserID = a.User_ID,
                                              Account = a.Account_Number,
                                              InitAssessment = a.Initial_Assessment,
                                              UpdateAssessment = a.Update_Assessment,
                                              UpdatePCU = a.Update_PCU,
                                              OneTimePA = a.OneTime_PA,
                                              InitPAProcess = a.Initial_PA_Process,
                                              POSorRWOcreated = a.POS_RWO_Created,
                                              FaxesToState = a.Faxes_to_State,
                                              LMletter = a.LM_Letter,
                                              Date = a.Date
                                          }).ToList();

                            return _vm.ToList();
                        }
                    }
                    else
                    {
                        if (user != null)
                        {
                            _vm = _db.assessment_log.Where(a => a.Date >= _startDt && a.Date <= _endDt && a.User_ID == user)
                                             .Select(a => new AssessmentLogModel
                                             {

                                                 ID = a.ID,
                                                 UserID = a.User_ID,
                                                 Account = a.Account_Number,
                                                 InitAssessment = a.Initial_Assessment,
                                                 UpdateAssessment = a.Update_Assessment,
                                                 UpdatePCU = a.Update_PCU,
                                                 OneTimePA = a.OneTime_PA,
                                                 InitPAProcess = a.Initial_PA_Process,
                                                 POSorRWOcreated = a.POS_RWO_Created,
                                                 FaxesToState = a.Faxes_to_State,
                                                 LMletter = a.LM_Letter,
                                                 Date = a.Date
                                             }).ToList();

                            return _vm.ToList();
                        }
                        else
                        {
                            _vm = _db.assessment_log.Where(a => a.Date >= _startDt && a.Date <= _endDt)
                                          .Select(a => new AssessmentLogModel
                                          {

                                              ID = a.ID,
                                              UserID = a.User_ID,
                                              Account = a.Account_Number,
                                              InitAssessment = a.Initial_Assessment,
                                              UpdateAssessment = a.Update_Assessment,
                                              UpdatePCU = a.Update_PCU,
                                              OneTimePA = a.OneTime_PA,
                                              InitPAProcess = a.Initial_PA_Process,
                                              POSorRWOcreated = a.POS_RWO_Created,
                                              FaxesToState = a.Faxes_to_State,
                                              LMletter = a.LM_Letter,
                                              Date = a.Date
                                          }).ToList();

                            return _vm.ToList();
                        }
                    }
                      
                    
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<AssessmentLogModel>();
            }



        }

        public static IList<UserIdsVm> GetUserIDs()
        {

            try
            {

                using (IntranetEntities _db = new IntranetEntities())
                {
                   IList<UserIdsVm> _list = new  List<UserIdsVm>();

                    _list = _db.Database.SqlQuery<UserIdsVm>("Select "+
                                                        " User_ID from "+
                                                         " assessment_log "+
                                                            " where year(date) > 2013 "+
                                                               " GROUP BY User_ID "+
                                                                " ORDER BY User_ID").ToList<UserIdsVm>();

                  //  _list.Insert(0, id);

                    return _list;

                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<UserIdsVm>();
            }
        }
    }

    //public class AssessmentLogVM
    //{

    //    public AssessmentLogVM()
    //    {
    //        GetAssessment = new List<AssessmentLogModel>();
    //    }
    //    public DateTime? StartDate { get; set; }
    //    public DateTime? EndDate { get; set; }
    //    public List<AssessmentLogModel> GetAssessment{ get; set; }

    //}


    //public class GetAssessmentLogReport
    //{
    //    public static IList<AssessmentLogModel> GetAssessmentLog(DateTime? _startDt, DateTime? _endDt)
    //    {
    //        try
    //        {
    //            using (IntranetEntities _db = new IntranetEntities())
    //            {

    //                var _woList = (from a in _db.assessment_log
    //                               where a.Date >= _startDt && a.Date <= _endDt

    //                               select new AssessmentLogModel
    //                               {
    //                                   ID = a.ID,
    //                                 UserID = a.User_ID,
    //                                 Account = a.Account_Number,
    //                                 InitAssessment = a.Initial_Assessment,
    //                                 UpdateAssessment = a.Update_Assessment,
    //                                 UpdatePCU = a.Update_PCU,
    //                                 OneTimePA = a.OneTime_PA,
    //                                 InitPAProcess = a.Initial_PA_Process,
    //                                 POSorRWOcreated = a.POS_RWO_Created,
    //                                 FaxesToState = a.Faxes_to_State,
    //                                 LMletter = a.LM_Letter,
    //                                 Date = a.Date


    //                               }).Take(10).ToList();

    //                return _woList.ToList();
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            string msg = ex.Message;
    //            return new List<AssessmentLogModel>();
    //        }



    //    }
    //}
}
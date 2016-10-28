using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Data;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Linq;
using System.Text;
using System.Threading;
using System.Web;
using LINQtoXSDLib;
using QTIUtility.QTIDataExtractTableAdapters;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

/*  course_id LIKE 'EACD!_%' ESCAPE '!'  */
namespace QTIUtility
{
    // DataSet connection string for QTIExtract
    // global::QTIUtility.Properties.Settings.Default.QTIDataExtractConnectionString;

    public class AcceptAllCertificatePolicy : ICertificatePolicy
    {
        public AcceptAllCertificatePolicy()
        {
        }

        public bool CheckValidationResult(ServicePoint sPoint, X509Certificate cert, WebRequest wRequest, int certProb)
        {
            // Always accept
            return true;
        }
    }

    public class BbQuery
    {

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool InternetSetCookie(string lpszUrlName, string lbszCookieName, string lpszCookieData);
        public static string BbLoginUrl = "auth_1.jsp";
        public static string BbCheckUrl = "check.jsp";
        public static string BbDeployUrl = "deployer_1.jsp";
        //putFile.jsp?debugging=1&shdCrs=157&filename=981234567.txt&emails=pheisen@verizon.net&fileContent=Hello_There
        public static string BbPutFile = "putFile.jsp";
        public static string BbUrl = null;
        public static string BbUrlStem = null;
        public static CookieContainer cc = null;
        public static CookieContainer regCc = null;
        public static bool BbloggedIn = false;
        public static bool BbSploggedIn = false;
        public static int BbInstructor_id = 0;
        public static string BbUsername = null;
        public static string BbLoggedInName = null;
        public static string BbServerTime = null;
        public static TimeSpan BbServerTimeSpan = new TimeSpan(0);
        public static string BbVersion = null;
        public static string BbPluginVersion = null;
        public static string BbInstructorEmail = null;
        public static string BbLoginString = null;
        public static string BbDeptMembers = null;
        public static string BbDomainString = null;
        public static string[] BbDomainPrefixes = null;
        public static string BbServiceLevel = "F"; // "F" is for regular course, "C" is for Organization
        public static string BbCourseSupervisorNameFragment = "EACS-";
        public static string BbCourseIdFragment = "EACDM!_";
        public static string BbCourseMaster = "EACSupervisor";
        public static string BbCourseEnterprise = "EACDMEnterprise";
        public static string BbCourseDeployer = "EACDeployer";
        public static string BbDepotCourseId = "EACDEPOT!_";
        public static string BbLibraryCourseName = "EACLibrary";
        public static string BbCategoryLibraryCourseName = "EACCategoryLibrary";
        public static string BbShadowCourseId = "EACD!_";
        public static string BbShadowPrefix = "EACD_";
        public static string BbDomainCourseNamePrefix = "EACAdministrator";
        public static string BbInstitutionalRole = "EACDMSupervisor";
        public static string BbEACSpCrsBatch_uid = "EACspc";
        public static string BbMultipleTA = "TA_EACH_";
        public static string BbMultipleInstr = "INSTR_EACH_";
        public static string BbUserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; EACDM; Windows NT 6.0)";
        public static string tokenBlock = "4d34f53389ed7f28ca91fc31ea360a66";
        public static EACMode EacMode = EACMode.All;
        public static EACLoggedInType EacInstructorType = EACLoggedInType.None;
        [Flags]
        public enum EACLoggedInType
        {
            None = 0, Instructor = 1, Admin = 2, Enterprise = 4, Deployer = 8, Retriever = 16, DomainManager = 32
        }
        public enum EACMode
        {
            All, Instructor, DepartmentHead, DepartmentHeadInstructor, DomainManager, Enterprise, Deployer, Retriever
        }
        public enum SurveyType
        {
            All, AnonymousOnly
        }
        public enum AssessmentType
        {
            Quiz = 1, Survey = 3
        }
        public enum QType
        {
            Unknown = 0, MultipleChoice = 1, TrueFalse = 2, MultipleAnswer = 3, Ordering = 4, Matching = 5, FillInBlank = 6, Essay = 7,
            Numeric = 8, Calculated = 9, FileUpload = 10, HotSpot = 11,
            FillInTheBlankPlus = 12, JumbledSentence = 13, QuizBowl = 14,
            OpinionScale = 15, ShortResponse = 16, EitherOr = 17
        }
        public static string[] ElementKindStrings =  {

"Unknown","Multiple Choice","True False","Multiple Answer","Ordering","Matching","Fill in the Blank","Essay","Numeric","Calculated",
"File Upload","Hot Spot", "Fill in the Blank Plus","Jumbled Sentence","Quiz Bowl","Opinion Scale","Short Response","Either Or"

                                };
        public const int MultipleChoice = 1;
        public const int TrueFalse = 2;
        public const int MultipleAnswer = 3;
        public const int Ordering = 4;
        public const int Matching = 5;
        public const int FillInBlank = 6;
        public const int Essay = 7;
        public const int Numeric = 8;
        public const int Calculated = 9;
        public const int FileUpload = 10;
        public const int HotSpot = 11;
        public const int FillInTheBlankPlus = 12;
        public const int JumbledSentence = 13;
        public const int QuizBowl = 14;
        public const int OpinionScale = 15;
        public const int ShortResponse = 16;
        public const int EitherOr = 17;

        public const string QuestionType = "./itemmetadata/bbmd_questiontype";
        public const string QuestionText = "./presentation/flow/flow/flow/material/mat_extension/mat_formattedtext";  // xPath probe
        // "./presentation/flow/flow/flow/material/mat_extension/mat_formattedtext"

        public static AssessmentType thisAssessmentType = AssessmentType.Survey; // default to survey
        public static bool SurveyAnonymous = true;  // on the safe side
        private static string SurveySql = null;
        private static string ResultsSql = null;

        public static int sqlCount = 0;
        public static string FirstEnrollDate = null;
        public static DataTable allStu;
        public static DataTable allCategory;
        public static DataTable getTable(string sql, string bbUrl, string token)
        {
            sql = HttpUtility.UrlEncode(sql);
            string qry = "?token=" + token + "&contents=" + sql;
            DataTable retValue = null;
            DataSet ds = new DataSet();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;
            try
            {
                XmlReader xr = XmlTextReader.Create(bbUrl + qry, settings);

                ds.ReadXml(xr, XmlReadMode.InferSchema);
                if (ds.Tables.Count > 0)
                {
                    retValue = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                retValue = new DataTable("error");
                retValue.Columns.Add("ErrorType", typeof(string));
                retValue.Columns.Add("ErrorMessage", typeof(string));
                DataRow dr = retValue.NewRow();
                dr["ErrorType"] = ex.GetType().ToString();
                dr["ErrorMessage"] = ex.Message;
                retValue.Rows.Add(dr);

            }
            /*
            if (ds != null)
            {
                QTIUtility.Utilities.logMe(ds.GetXmlSchema());
            }
             * */
            return retValue;
        }

        private static DataTable AsyncGetTable(string bbUrl, string qry)
        {
            //  QTIUtility.Utilities.logMe(bbUrl + qry);
            DataTable retValue = null;
            DataSet ds = new DataSet();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;
            try
            {
                XmlReader xr = XmlTextReader.Create(bbUrl + qry, settings);

                ds.ReadXml(xr, XmlReadMode.InferSchema);
                if (ds.Tables.Count > 0)
                {
                    retValue = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                retValue = new DataTable("error");
                retValue.Columns.Add("ErrorType", typeof(string));
                retValue.Columns.Add("ErrorMessage", typeof(string));
                DataRow dr = retValue.NewRow();
                dr["ErrorType"] = ex.GetType().ToString();
                dr["ErrorMessage"] = ex.Message;
                retValue.Rows.Add(dr);
                // QTIUtility.Utilities.logMe(ex.Message+Environment.NewLine+ex.GetType().ToString());
            }
            if (ds != null)
            {
                //QTIUtility.Utilities.logMe(ds.GetXmlSchema());
            }
            return retValue;
        }
        public static void StartConnection(string bbUrl, string token)
        {
            string simpleSql = "select count(pk1) as thecount from course_main";
            RequesterAsync rr = new RequesterAsync(simpleSql, bbUrl, token, true, cc);
            DataTable anything = rr.execute();
        }
        public static DataTable GetAllSurveys(string bbUrl, string token, BbQuery.AssessmentType assType)
        {
            return GetAllSurveys(bbUrl, null, null, null, token, assType);
        }
        public static DataTable GetAllSurveys(string bbUrl, string surveybox, string filter, string datefilter, string token, BbQuery.AssessmentType assType)
        {
            if (isMoodle(bbUrl))
            {
                return MdlQuery.GetAllSurveys(bbUrl, surveybox, filter, datefilter, token, assType);
            }
            string theIds = "0";
            ArrayList f_last = null;

            // Supervisor: first get supervisees including myself
            // user_pks placed in BbDeptMembers
            // <F>_<Lastname> placed in ArrayList f_last
            if ((BbloggedIn || BbSploggedIn) && (EacMode == EACMode.DepartmentHead || EacMode == EACMode.DepartmentHeadInstructor))
            {
                f_last = new ArrayList();
                if (EacMode == EACMode.DepartmentHeadInstructor)
                {
                    theIds += "," + BbInstructor_id.ToString();
                    string[] fl = BbLoggedInName.Split(' ');
                    f_last.Add(fl[0].ToUpper()[0] + "_" + fl[1]);
                }
                StringBuilder sf = new StringBuilder();
                sf.Append(" select cu.users_pk1 as id, u.firstname,u.lastname ");
                sf.Append(" from course_users cu ");
                sf.Append(" inner join users u on u.pk1 = cu.users_pk1 ");
                sf.Append(" where cu.role = 'S' and cu.AVAILABLE_IND = 'Y' and cu.crsmain_pk1 in  ");
                sf.Append(" ( select c.pk1 from course_main c inner join course_users cuu on c.pk1 = cuu.crsmain_pk1  ");
                sf.Append(" where  ((UPPER(c.course_name) like '" + BbCourseSupervisorNameFragment + "%')) and cuu.users_pk1 = " + BbInstructor_id.ToString());
                sf.Append(" and cuu.role='P' and c.service_level = '" + BbServiceLevel + "') ");
                sf.Append(" group by cu.users_pk1,u.lastname, u.firstname ");
                RequesterAsync rrr = new RequesterAsync(sf.ToString(), bbUrl, token, true, cc);
                DataTable dtp = rrr.execute();
                if (dtp != null && dtp.Rows.Count > 0)
                {
                    foreach (DataRow dx in dtp.Rows)
                    {
                        theIds += "," + dx["id"].ToString();
                        f_last.Add(dx["firstname"].ToString().ToUpper()[0] + "_" + dx["lastname"].ToString());
                    }
                }
                BbDeptMembers = theIds;
            }
            // EndEventHandler: get supervisees

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT  distinct qd.PK1, qd.CRSMAIN_PK1,qd.objectbank_pk1,");
            if (EacMode != EACMode.Deployer)
            {
                sb.Append(" (select min(cu.enrollment_date) from course_users cu where cu.crsmain_pk1 = c.pk1 and cu.role = 'S' and cu.AVAILABLE_IND = 'Y' and cu.row_status = 0)  as deployed,  ");
            }
            else
            {
                sb.Append(" cc.start_date,cc.end_date,");
                sb.Append(" cc.dtmodified  as deployed,  ");
            }
            sb.Append(" c.course_name AS name, ");
            sb.Append(" c.course_id as course_id, ");
            sb.Append(" cc.title as survey ");
            if (EacMode != EACMode.Deployer)
            {
                sb.Append(" ,(select min(qqr.bbmd_date_modified) from qti_result_data qqr where qqr.qti_asi_data_pk1 = qd.pk1) as mindate ");
                sb.Append(" ,(select max(qqr.bbmd_date_modified) from qti_result_data qqr where qqr.qti_asi_data_pk1 = qd.pk1) as maxdate ");
            }
            // Folder filter for Deployer and Retriever (these are EACLibrary folders)
            if (EacMode == EACMode.Deployer || EacMode == EACMode.Retriever)
            {
                sb.Append(" , (");
                sb.Append(" select title ");
                sb.Append(" from course_contents ");
                sb.Append(" where pk1 = cc.parent_pk1 ");
                if (!surveybox.Trim().Equals(""))
                {
                    sb.Append(SetFolderTitleFilter(surveybox));
                }
                sb.Append(" ) as folder ");
            }
            else
            {
                // a bit more involved to get folders if not in EACLibrary
                sb.Append(" , ( ");
                sb.Append(" select title ");
                sb.Append(" from course_contents  ");
                sb.Append(" where pk1 =  (select cc.parent_pk1 ");
                sb.Append(" from  course_contents cc ");
                sb.Append(" inner join course_main cm on cm.pk1 = cc.crsmain_pk1 ");
                sb.Append(" inner join gradebook_main ggm on ggm.crsmain_pk1 = cm.pk1 and ggm.course_contents_pk1 = cc.pk1 ");
                sb.Append(" where UPPER(cm.course_name) like 'EACLIBRAR%'  and ggm.qti_asi_data_pk1 = qd.objectbank_pk1 )");
                sb.Append(" ) as folder ");
            }
            sb.Append(" FROM course_assessment ca inner join course_main c on ca.crsmain_pk1 = c.pk1 inner join  qti_asi_data qd ON ca.qti_asi_data_pk1 = qd.pk1   ");
            sb.Append(" inner join course_users cu on cu.crsmain_pk1 = c.pk1  ");
            sb.Append(" inner join link ll on ll.link_source_pk1 = ca.pk1 ");
            sb.Append(" inner join course_contents cc on cc.pk1 = ll.course_contents_pk1 ");


            if (EacMode != EACMode.Deployer)
            {
                sb.Append(" inner join qti_result_data qr on qd.pk1 = qr.qti_asi_data_pk1 ");
            }
            sb.Append(" where ");
            sb.Append(" qd.BBMD_ASSESSMENTTYPE = " + ((int)assType).ToString() + " ");
            sb.Append(" and ll.LINK_SOURCE_TABLE='COURSE_ASSESSMENT'  ");

            // make sure students are present, available and active (may not be yer for deployer)
            if (EacMode != EACMode.Deployer)
            {
                sb.Append(" and cu.role = 'S' and cu.AVAILABLE_IND = 'Y' and cu.row_status = 0 ");
                sb.Append("  and (select count(pk1) from qti_result_data where qti_asi_data_pk1 = qd.pk1 and bbmd_date_added >= qd.bbmd_date_added ) > 0  ");
            }
            // instructor mode quite constrained.  No shadow courses and nobody but logged in users
            if ((BbloggedIn || BbSploggedIn) && EacMode == EACMode.Instructor)
            {
                sb.Append(" and (select count(pk1) from course_users where crsmain_pk1 = c.pk1 and users_pk1 = " + BbInstructor_id.ToString() + " and role = 'P') > 0 ");
                sb.Append(" and not (  (UPPER(c.course_id) like '%" + BbShadowCourseId + "%' escape '!') or (UPPER(c.course_name) like '" + BbLibraryCourseName + "%')  )");
            }

            // supervisors only see courses with their own supervisees AND after window close
            if ((BbloggedIn || BbSploggedIn) && (EacMode == EACMode.DepartmentHead || EacMode == EACMode.DepartmentHeadInstructor))
            {
                // sb.Append(" and (select count(pk1) from course_users where crsmain_pk1 = c.pk1 and users_pk1 in (" + theIds + ") and role = 'P') > 0 ");
                // sb.Append(" and " + WindowClosed());
                sb.Append(" and ( ");
                // regular courses     
                sb.Append(" (select count(pk1) from course_users where crsmain_pk1 = c.pk1 and not UPPER(c.course_id) like '%" + BbShadowCourseId + "%' escape '!' ");
                sb.Append(" and users_pk1 in (" + theIds + ") and role = 'P') > 0 ");
                sb.Append(" or ");
                sb.Append(" (select count(pk1) from course_users where crsmain_pk1 = Replace(UPPER(c.course_id),'" + BbShadowPrefix + "','') and UPPER(c.course_id) like '%" + BbShadowCourseId + "%' escape '!' ");
                sb.Append(" and users_pk1 in (" + theIds + ") and role = 'P') > 0 ");
                sb.Append(" ) ");
                sb.Append(" and " + WindowClosed());

            }
            if (filter != null)
            {
                sb.Append(filter);
            }

            // deployer only sees sha
            if (EacMode == EACMode.Deployer)
            {
                sb.Append(" and ( (UPPER(c.course_id) like '" + BbDepotCourseId + "%' escape '!') or (UPPER(c.course_name) like '" + BbLibraryCourseName.ToUpper() + "%" + "') )");
                sb.Append(" and (select count(pk1) from course_users where crsmain_pk1 = c.pk1 and users_pk1 = " + BbInstructor_id.ToString() + " and role = 'P') > 0 ");
            }
            if (datefilter != null && BbQuery.EacMode != EACMode.Deployer)
            {
                sb.Append(datefilter);
            }
            sb.Append(" order by deployed desc,survey, name asc,course_id ");
            RequesterAsync rr = new RequesterAsync(sb.ToString(), bbUrl, token, true, cc);
            //  Logger.__SpecialLogger("Mode: " + BbQuery.EacMode.ToString() + " Search sql: " + sb.ToString());
            //    Logger.__SpecialLogger("Bloom bbUrl: " + bbUrl+" token: "+token);
            DataTable dt = rr.execute();
            if (dt != null && dt.Rows.Count > 0 && !dt.TableName.ToUpper().Equals("ERROR"))
            {
                if (EacMode == EACMode.Deployer || EacMode == EACMode.Retriever)
                {
                    if (!surveybox.Trim().Equals(""))
                    {
                        DataRow[] rs = dt.Select("folder = ''");
                        foreach (DataRow dr in rs)
                        {
                            dt.Rows.Remove(dr);
                        }

                        dt.AcceptChanges();
                    }

                }

                dt.Columns.Add("qts", typeof(string));
                ArrayList pk11s = new ArrayList();
                int j = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if ((BbloggedIn || BbSploggedIn) && (EacMode == EACMode.DepartmentHead || EacMode == EACMode.DepartmentHeadInstructor))
                    {
                        // INSTR_B_Sikes_Instructor Evaluation
                        string[] survey = dt.Rows[i]["survey"].ToString().Split('_');
                        if (survey.Length >= 3 && survey[0].Equals("INSTR"))
                        {
                            if (!f_last.Contains(survey[1] + "_" + survey[2]))
                            {
                                pk11s.Add("0");// Convert.ToInt32(dt.Rows[i]["PK1"]).ToString();
                                dt.Rows[i].Delete();
                            }
                            else
                            {
                                pk11s.Add(Convert.ToInt32(dt.Rows[i]["PK1"]).ToString());
                            }
                        }
                        else
                        {
                            pk11s.Add(Convert.ToInt32(dt.Rows[i]["PK1"]).ToString());
                        }
                    }
                    else
                    {
                        pk11s.Add(Convert.ToInt32(dt.Rows[i]["PK1"]).ToString());
                    }
                    try
                    {
                        Convert.ToInt32(dt.Rows[i]["objectbank_pk1"]);
                    }

                    catch (Exception ex)
                    {
                        dt.Rows[i]["objectbank_pk1"] = 0;
                    }
                }
                if ((BbloggedIn || BbSploggedIn) && (EacMode == EACMode.DepartmentHead || EacMode == EACMode.DepartmentHeadInstructor))
                {
                    dt.AcceptChanges();
                }
                string[] thePks = new string[pk11s.Count];
                thePks = (string[])pk11s.ToArray(typeof(string));
                sb.Length = 0;
                sb.Append("SELECT q1.pk1 as qpk1 , q3.bbmd_questiontype as kind ");
                sb.Append(" from qti_asi_data q3 inner join qti_asi_data q2 on q3.parent_pk1 = q2.pk1 ");
                sb.Append(" inner join qti_asi_data q1 on q2.parent_pk1 = q1.pk1 ");
                sb.Append(" where q1.pk1 in (" + String.Join(",", thePks) + ")");
                sb.Append(" order by q3.position ");
                RequesterAsync rd = new RequesterAsync(sb.ToString(), bbUrl, token, true, cc);
                DataTable qts = rd.execute();
                if (qts != null && qts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string theQs = "";
                        DataView dv = new DataView(qts, "qpk1 = " + dr["PK1"].ToString(), "", DataViewRowState.CurrentRows);
                        foreach (DataRowView drv in dv)
                        {
                            theQs += drv["kind"].ToString() + "_";
                        }
                        if (theQs.Length > 0)
                        {
                            theQs = theQs.Substring(0, theQs.Length - 1);
                        }
                        dr["qts"] = theQs;
                    }
                }
            }
            return dt;
        }

        public static DataTable GetAllSurveysRetriever(string bbUrl, string surveybox, string filter, string datefilter, string token, BbQuery.AssessmentType assType)
        {
            if (isMoodle(bbUrl))
            {
                return MdlQuery.GetAllSurveysRetriever(bbUrl, surveybox, filter, datefilter, token, assType);
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT  qd.PK1, qd.CRSMAIN_PK1,qd.objectbank_pk1, ");
            sb.Append(" (select min(cu.enrollment_date) from course_users cu where cu.crsmain_pk1 = c.pk1 and cu.role = 'S' and cu.AVAILABLE_IND = 'Y' and cu.row_status = 0)  as deployed,   ");
            sb.Append(" c.course_name AS name,c.course_id as course_id,gm.title as survey ");
            sb.Append(" , (select title from course_contents where pk1 =  ");
            sb.Append(" ( select cc.parent_pk1 ");
            sb.Append(" from course_contents cc inner join course_main cs on cc.crsmain_pk1 = cs.pk1 ");
            sb.Append(" inner join course_assessment caa on caa.crsmain_pk1 = cs.pk1 ");
            sb.Append(" inner join link ll on ll.link_source_pk1 = caa.pk1   ");
            sb.Append(" where cc.pk1 = ll.course_contents_pk1 and cs.course_name = 'EACLibrary' ");
            sb.Append(" and caa.qti_asi_data_pk1 = qd.objectbank_pk1 and ll.LINK_SOURCE_TABLE='COURSE_ASSESSMENT') ");
            if (!surveybox.Trim().Equals(""))
            {

                sb.Append(SetFolderTitleFilter(surveybox));
            }

            sb.Append(") as folder  ");
            sb.Append(" ,(select min(bbmd_date_modified) from qti_result_data where qti_asi_data_pk1 = qd.pk1) as mindate ");
            sb.Append(" , (select max(bbmd_date_modified) from qti_result_data where qti_asi_data_pk1 = qd.pk1) as maxdate ");
            sb.Append("FROM course_assessment ca inner join course_main c on ca.crsmain_pk1 = c.pk1 INNER JOIN ");
            sb.Append("QTI_ASI_DATA qd ON ca.qti_asi_data_pk1 = qd.pk1 ");
            sb.Append(" inner join gradebook_main gm on qd.pk1 = gm.qti_asi_data_pk1 ");
            sb.Append(" where ");
            sb.Append(" qd.BBMD_ASSESSMENTTYPE = " + ((int)assType).ToString() + " ");
            sb.Append(" and (select count(pk1) from qti_result_data where qti_asi_data_pk1 = qd.pk1 and bbmd_date_added >= qd.bbmd_date_added ) > 0 ");
            sb.Append(" and (select count(pk1) from course_users where crsmain_pk1 = c.pk1 and users_pk1 = " + BbInstructor_id.ToString() + " and role = 'P') > 0 ");
            sb.Append(" and (select count(cu.enrollment_date) from course_users cu where cu.crsmain_pk1 = c.pk1 and cu.role = 'S' and cu.AVAILABLE_IND = 'Y' and cu.row_status = 0)  > 0  ");
            sb.Append(" and UPPER(c.course_id) like '%" + BbShadowCourseId + "%' escape '!' ");
            sb.Append(" and gm.linkrefid  like '%!_" + BbInstructor_id.ToString() + "%' escape '!' ");

            if (filter != null)
            {
                sb.Append(filter);
            }
            if (datefilter != null)
            {
                sb.Append(datefilter);
            }
            sb.Append(" order by deployed desc,survey, name asc,course_id ");
            RequesterAsync rr = new RequesterAsync(sb.ToString(), bbUrl, token, true, cc);
            //   Logger.__SpecialLogger("Retrieve sql: " + sb.ToString() + Environment.NewLine);
            //   Logger.__SpecialLogger("Bloom bbUrl: " + bbUrl+" token: "+token);
            DataTable dt = rr.execute();
            if (dt != null && dt.Rows.Count > 0 && !dt.TableName.ToUpper().Equals("ERROR"))
            {
                if (!surveybox.Trim().Equals(""))
                {
                    DataRow[] rs = dt.Select("folder = ''");
                    foreach (DataRow dr in rs)
                    {
                        dt.Rows.Remove(dr);
                    }

                    dt.AcceptChanges();
                }

                dt.Columns.Add("qts", typeof(string));
                string[] pk11s = new string[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    pk11s[i] = Convert.ToInt32(dt.Rows[i]["PK1"]).ToString();
                    // dt.Rows[i]["survey"] = dt.Rows[i]["survey"].ToString() + " (" + dt.Rows[i]["folder"].ToString() + ")";
                }
                sb.Length = 0;
                sb.Append("SELECT q1.pk1 as qpk1 , q3.bbmd_questiontype as kind ");
                sb.Append(" from qti_asi_data q3 inner join qti_asi_data q2 on q3.parent_pk1 = q2.pk1 ");
                sb.Append(" inner join qti_asi_data q1 on q2.parent_pk1 = q1.pk1 ");
                sb.Append(" where q1.pk1 in (" + String.Join(",", pk11s) + ")");
                sb.Append(" order by q3.position ");
                RequesterAsync rd = new RequesterAsync(sb.ToString(), bbUrl, token, true, cc);
                DataTable qts = rd.execute();
                if (qts != null && qts.Rows.Count > 0)
                {

                    foreach (DataRow dr in dt.Rows)
                    {
                        string theQs = "";
                        DataView dv = new DataView(qts, "qpk1 = " + dr["PK1"].ToString(), "", DataViewRowState.CurrentRows);
                        foreach (DataRowView drv in dv)
                        {
                            theQs += drv["kind"].ToString() + "_";
                        }
                        if (theQs.Length > 0)
                        {
                            theQs = theQs.Substring(0, theQs.Length - 1);
                        }
                        dr["qts"] = theQs;
                    }
                }
            }
            return dt;
        }

        public static DataTable GetAllTests(string bbUrl, string filter, string datefilter, string token, BbQuery.AssessmentType assType)
        {

            return GetAllSurveys(bbUrl, token, AssessmentType.Quiz);
        }

        public static DataTable GetAllBbCourses(string bbUrl, string datefilter, string token, int maxPk1, DateTime maxRefresh, bool BuildAll)
        {

            string maxRe = maxRefresh.ToString("yyyy-MM-dd HH:mm:ss.fff");
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT  c.PK1,c.available_ind, c.course_name AS name,c.course_id,");
            sb.Append("(select min(cu.enrollment_date) from course_users cu where cu.crsmain_pk1 = c.pk1 and cu.role = 'S' and cu.AVAILABLE_IND = 'Y' and cu.row_status = 0  ) as deployed,");
            sb.Append(" c.service_level,");
            sb.Append("(select cm.institution_name from course_main cm where cm.course_id = replace('" + BbShadowPrefix + "XXXX','XXXX',cast(c.pk1 as varchar(20)))) as instructor_survey, ");
            sb.Append("(select cm.dtmodified from course_main cm where cm.course_id = replace('" + BbShadowPrefix + "XXXX','XXXX',cast(c.pk1 as varchar(20)))) as refreshed ");
            sb.Append(" ,c.dtmodified as refreshed2 ");
            sb.Append("FROM course_main c ");
            sb.Append("where (( c.pk1 > " + maxPk1.ToString() + ") ");
            sb.Append(" or (    (select cm.dtmodified from course_main cm where cm.course_id = replace('" + BbShadowPrefix + "XXXX','XXXX',cast(c.pk1 as varchar(20))))     > {ts '" + maxRe + "'})  ");
            sb.Append(" or ");
            sb.Append(" ( ");
            if (BuildAll)
            {
                sb.Append("1=1 or ");
            }
            sb.Append(" c.dtmodified > {ts '" + maxRe + "'}");
            sb.Append(")");
            sb.Append(") and ");
            sb.Append("(select count(cu.pk1) from course_users cu where ");
            sb.Append("cu.role = 'S' and cu.AVAILABLE_IND = 'Y' and cu.row_status = 0 ");
            sb.Append(" and c.pk1 = cu.crsmain_pk1  ");
            if (datefilter != null)
            {
                sb.Append(datefilter);
            }
            sb.Append(" ) > 0 ");
            sb.Append("and not (  UPPER(c.course_name) like '" + BbCourseSupervisorNameFragment.ToUpper() + "%'  ");
            sb.Append(" or UPPER(c.course_name) like '%" + BbCourseMaster.ToUpper() + "%'  ");
            sb.Append(" or UPPER(c.course_name) like '%" + BbCourseEnterprise.ToUpper() + "%' ");
            sb.Append(" or UPPER(c.course_name) like '%" + BbCourseDeployer.ToUpper() + "%' ");
            sb.Append(" or UPPER(c.course_name) like '%" + BbDomainCourseNamePrefix.ToUpper() + "%' ");
            sb.Append(" or UPPER(c.course_id) like '%" + BbCourseIdFragment.ToUpper() + "%' escape '!' ");
            sb.Append(" or UPPER(c.course_id) like '%" + BbDepotCourseId + "%' escape '!' ");
            sb.Append(" or c.course_id like '%" + BbShadowCourseId + "%' escape '!'  ) ");

            sb.Append(" order by deployed desc,name asc,course_id ");

            RequesterAsync rr = new RequesterAsync(sb.ToString(), bbUrl, token, true, cc);
            // Logger.__SpecialLogger("Courses sql: " + sb.ToString());
            //Logger.__SpecialLogger("BbUrl: " + bbUrl);
            DataTable dt = rr.execute();
            return dt;
        }

        public static string SetRetrieveFilter(string surveys, string courses, string bDate, string eDate, bool orgs)
        {
            if (isMoodle(BbUrl))
            {
                return MdlQuery.SetRetrieveFilter(surveys, courses, bDate, eDate, orgs);
            }
            FirstEnrollDate = null;
            string retValue = null;
            StringBuilder uf = null;
            // handle surveys
            // new

            string expression = null;
            if (EacMode != EACMode.Retriever)
            {
                expression = "UPPER(cc.title)";

            }
            else
            {
                expression = "UPPER(gm.title)";
            }

            ArrayList tokens = Tokenizer(surveys);
            if (tokens != null && tokens.Count > 0)
            {
                if (uf == null)
                {
                    uf = new StringBuilder();
                }
                uf.Append(" and ");
                uf.Append(" (");
                for (int j = 0; j < tokens.Count; j++)
                {
                    string[] token = (string[])tokens[j];
                    if (j != 0)
                    {
                        uf.Append(" or ");
                    }
                    for (int i = 0; i < token.Length; i++)
                    {

                        if (i == 0)
                        {
                            uf.Append("(");
                            uf.Append("  ( " + expression + " like '%" + token[i].Trim().ToUpper() + "%' ) ");
                            if (token.Length == 1)
                            {
                                uf.Append(")");
                            }
                        }
                        else
                        {
                            if (i == (token.Length - 1))
                            {
                                uf.Append(" and ( " + expression + " like '%" + token[i].Trim().ToUpper() + "%' )");
                                uf.Append(")");
                            }
                            else
                            {
                                uf.Append(" and ( " + expression + " like '%" + token[i].Trim().ToUpper() + "%' )");
                            }
                        }
                    }
                }


                uf.Append(") ");
            }
            string course_name = "UPPER(c.course_name)";
            string course_id = "UPPER(c.course_id)";
            if (EacMode == EACMode.DomainManager)
            {
                FilterDomainCourse(courses, ref uf, course_name, course_id);
                if (uf == null)
                {
                    uf = new StringBuilder();
                }
                uf.Append(" and " + WindowClosed());
            }
            else
            {
                FilterCourse(courses, ref uf, course_name, course_id);
            }
            if (orgs)
            {
                if (uf == null)
                {
                    uf = new StringBuilder();
                }
                uf.Append(" and ");
                uf.Append(" c.service_level = 'C' ");

            }
            else
            {
                if (uf == null)
                {
                    uf = new StringBuilder();
                }
                uf.Append(" and ");
                uf.Append(" c.service_level = 'F' ");
            }

            if (uf != null)
            {
                retValue = uf.ToString();
            }
            return retValue;


        }

        public static string SetAssessmentFilter(string surveys, string courses, string bDate, string eDate, bool orgs)
        {
            if (BbUrl.Contains("moodle") || BbUrl.Contains("mdllinker"))
            {
                return MdlQuery.SetAssessmentFilter(surveys, courses, bDate, eDate, orgs);
            }
            FirstEnrollDate = null;
            string retValue = null;
            StringBuilder uf = null;
            string expression = null;
            if (EacMode != EACMode.Retriever)
            {
                expression = "UPPER(cc.title)";

            }
            else
            {
                expression = "UPPER(gm.title)";
            }

            // start new
            ArrayList tokens = Tokenizer(surveys);
            if (tokens != null && tokens.Count > 0)
            {
                if (uf == null)
                {
                    uf = new StringBuilder();
                }
                uf.Append(" and ");
                uf.Append(" (");
                for (int j = 0; j < tokens.Count; j++)
                {
                    string[] token = (string[])tokens[j];
                    if (j != 0)
                    {
                        uf.Append(" or ");
                    }
                    for (int i = 0; i < token.Length; i++)
                    {

                        if (i == 0)
                        {
                            uf.Append("(");
                            uf.Append("  ( " + expression + " like '%" + token[i].Trim().ToUpper() + "%' ) ");
                            if (token.Length == 1)
                            {
                                uf.Append(")");
                            }
                        }
                        else
                        {
                            if (i == (token.Length - 1))
                            {
                                uf.Append(" and ( " + expression + " like '%" + token[i].Trim().ToUpper() + "%' )");
                                uf.Append(")");
                            }
                            else
                            {
                                uf.Append(" and ( " + expression + " like '%" + token[i].Trim().ToUpper() + "%' )");
                            }
                        }
                    }
                }


                uf.Append(") ");
            }

            // end new

            // handle surveys
            //string[] survs = surveys.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //if (survs != null && survs.Length > 0)
            //{
            //    if (uf == null)
            //    {
            //        uf = new StringBuilder();
            //    }

            //    if (EacMode != EACMode.Retriever)
            //    {
            //        expression = "UPPER(cc.title)";

            //    }
            //    else
            //    {
            //        expression = "UPPER(gm.title)";
            //    }
            //    uf.Append(" and ");
            //    uf.Append(" (");
            //    for (int i = 0; i < survs.Length; i++)
            //    {
            //        if (i == 0)
            //        {
            //            uf.Append(" (" + expression + " like '%" + survs[i].Trim().ToUpper() + "%')");
            //        }
            //        else
            //        {
            //            uf.Append(" or (" + expression + " like '%" + survs[i].Trim().ToUpper() + "%')");
            //        }

            //    }
            //    uf.Append(") ");
            //}
            if (uf != null)
            {
                retValue = uf.ToString();
            }
            return retValue;


        }

        public static string SetCourseFilter(string surveys, string courses, string bDate, string eDate, bool orgs)
        {
            if (BbUrl.Contains("moodle") || BbUrl.Contains("mdllinker"))
            {
                return MdlQuery.SetCourseFilter(surveys, courses, bDate, eDate, orgs);
            }
            FirstEnrollDate = null;
            string retValue = null;
            StringBuilder uf = null;
            string course_name = "UPPER(c.course_name)";
            string course_id = "UPPER(c.course_id)";
            FilterCourse(courses, ref uf, course_name, course_id);
            if (orgs)
            {
                if (uf == null)
                {
                    uf = new StringBuilder();
                }
                uf.Append(" and ");
                uf.Append(" c.service_level = 'C' ");
            }
            else
            {
                if (uf == null)
                {
                    uf = new StringBuilder();
                }
                uf.Append(" and ");
                uf.Append(" c.service_level = 'F' ");
            }
            if (uf != null)
            {
                retValue = uf.ToString();
            }
            return retValue;
        }

        public static string SetBbCourseFilter(string courses, bool orgs)
        {
            if (isMoodle(BbUrl))
            {
                return MdlQuery.SetBbCourseFilter(courses, orgs);
            }
            string retValue = null;
            StringBuilder uf = null;
            string course_name = "name";
            string course_id = "course_id";
            FilterCourse(courses, ref uf, course_name, course_id);
            if (orgs)
            {
                if (uf == null)
                {
                    uf = new StringBuilder();
                }
                uf.Append(" and ");
                uf.Append(" service_level = 'C' ");
            }
            else
            {
                if (uf == null)
                {
                    uf = new StringBuilder();
                }
                uf.Append(" and ");
                uf.Append(" service_level = 'F' ");
            }
            if (uf != null)
            {
                retValue = uf.ToString();
            }
            return retValue;
        }

        public static string SetFolderTitleFilter(string surveys)
        {
            string folder = null;
            ArrayList tokens = Tokenizer(surveys);
            StringBuilder ft = null;
            if (tokens != null && tokens.Count > 0)
            {
                ft = new StringBuilder();
                ft.Append(" and ");
                ft.Append("(");
                for (int j = 0; j < tokens.Count; j++)
                {
                    string[] token = (string[])tokens[j];
                    if (j != 0)
                    {
                        ft.Append(" or ");
                    }
                    for (int i = 0; i < token.Length; i++)
                    {

                        if (i == 0)
                        {
                            ft.Append("(");
                            ft.Append("  ( UPPER(title) like '%" + token[i].Trim().ToUpper() + "%' escape '!') ");
                            if (token.Length == 1)
                            {
                                ft.Append(")");
                            }
                        }
                        else
                        {
                            if (i == (token.Length - 1))
                            {
                                ft.Append(" and ( UPPER(title) like '%" + token[i].Trim().ToUpper() + "%' escape '!')");
                                ft.Append(")");
                            }
                            else
                            {
                                ft.Append(" and ( UPPER(title) like '%" + token[i].Trim().ToUpper() + "%' escape '!')");
                            }
                        }
                    }


                }
                ft.Append(")");

            }
            if (ft != null)
            {
                folder = ft.ToString();
            }
            return folder;
        }
        private static void FilterCourse(string courses, ref StringBuilder uf, string course_name, string course_id)
        {

            ArrayList tokens = Tokenizer(courses);
            if (tokens != null && tokens.Count > 0)
            {
                if (uf == null)
                {
                    uf = new StringBuilder();
                }
                uf.Append(" and ");
                uf.Append(" (");
                for (int j = 0; j < tokens.Count; j++)
                {
                    string[] token = (string[])tokens[j];
                    if (j != 0)
                    {
                        uf.Append(" or ");
                    }
                    for (int i = 0; i < token.Length; i++)
                    {

                        if (i == 0)
                        {
                            uf.Append("(");
                            uf.Append("  ( " + course_name + " like '%" + token[i].Trim().ToUpper() + "%' ) ");
                            if (token.Length == 1)
                            {
                                uf.Append(")");
                            }
                        }
                        else
                        {
                            if (i == (token.Length - 1))
                            {
                                uf.Append(" and ( " + course_name + " like '%" + token[i].Trim().ToUpper() + "%' )");
                                uf.Append(")");
                            }
                            else
                            {
                                uf.Append(" and ( " + course_name + " like '%" + token[i].Trim().ToUpper() + "%' )");
                            }
                        }
                    }
                }
                uf.Append(" or ");
                for (int j = 0; j < tokens.Count; j++)
                {
                    string[] token = (string[])tokens[j];
                    if (j != 0)
                    {
                        uf.Append(" or ");
                    }
                    for (int i = 0; i < token.Length; i++)
                    {

                        if (i == 0)
                        {
                            uf.Append("(");
                            uf.Append("  ( " + course_id + " like '%" + token[i].Trim().ToUpper() + "%' ) ");
                            if (token.Length == 1)
                            {
                                uf.Append(")");
                            }
                        }
                        else
                        {
                            if (i == (token.Length - 1))
                            {
                                uf.Append(" and ( " + course_id + " like '%" + token[i].Trim().ToUpper() + "%' )");
                                uf.Append(")");
                            }
                            else
                            {
                                uf.Append(" and ( " + course_id + " like '%" + token[i].Trim().ToUpper() + "%' )");
                            }
                        }
                    }
                }
                uf.Append(") ");
            }
            return;
        }

        private static ArrayList Tokenizer(string courses)
        {
            ArrayList tokens = null;
            string[] tmps = courses.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (tmps != null && tmps.Length > 0)
            {
                tokens = new ArrayList();
                string[] token = null;
                for (int i = 0; i < tmps.Length; i++)
                {
                    if (tmps[i].Contains("^") /*&& tmps[i].IndexOf(@"\^") > -1 */)
                    {
                        tmps[i] = tmps[i].Replace(@"\^", "^");
                        token = tmps[i].Split(new char[] { '^' }, StringSplitOptions.RemoveEmptyEntries);
                    }
                    else
                    {
                        token = new string[1];
                        token[0] = tmps[i];
                    }
                    tokens.Add(token);
                }

            }

            return tokens;
        }

        private static string WindowClosed()
        {
            DateTime current = DateTime.Now.Add(BbServerTimeSpan);
            string endShow = current.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ts = "{ ts '" + endShow + "'}";
            return " (  cc.end_date is null or " + ts + " > cc.end_date   ) ";

        }

        private static void FilterDomainCourse(string courses, ref StringBuilder uf, string course_name, string course_id)
        {
            bool ok = false;
            if (courses == null || courses.Equals(""))
            {
                courses = BbDomainString;
            }
            string[] cours = courses.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if ((cours != null && cours.Length > 0) && (!courses.ToUpper().Trim().Equals(BbDomainString) ||
                courses.Contains("$") || !courses.Contains("*"))
                )
            {
                StringBuilder ts = new StringBuilder();
                foreach (string t in cours)
                {
                    if (CheckDomainCourse_id(t))
                    {
                        ts.Append(t + ",");
                    }
                }
                string courses1 = ts.ToString();
                if (courses1.Length > 0)
                {
                    courses1 = courses1.Substring(0, courses1.Length - 1);
                }
                ArrayList tokens = Tokenizer(courses1);
                if (tokens != null && tokens.Count > 0)
                {
                    if (uf == null)
                    {
                        uf = new StringBuilder();
                    }
                    uf.Append(" and ");
                    uf.Append(" (");
                    for (int j = 0; j < tokens.Count; j++)
                    {
                        string[] token = (string[])tokens[j];
                        if (j != 0)
                        {
                            uf.Append(" or ");
                        }
                        for (int i = 0; i < token.Length; i++)
                        {

                            if (i == 0)
                            {
                                uf.Append("(");

                                if (token[i].StartsWith("$"))
                                {
                                    string tk = token[i].Substring(1, token[i].Length - 1);
                                    string theToken = tk;
                                    int p = -1;
                                    if (tk.EndsWith("*"))
                                    {
                                        theToken = tk + "%";
                                    }
                                    else
                                    {
                                        if (tk.Contains("*"))
                                        {
                                            theToken = tk.Replace("*", "%");
                                        }
                                    }
                                    uf.Append("  ( " + course_name + " like '%(" + theToken.ToUpper() + "%' ) ");
                                }
                                else
                                {
                                    uf.Append("  ( " + course_name + " like '%(%" + token[i].Trim().ToUpper() + "%' ) ");
                                }



                                //uf.Append("  ( " + course_name + " like '%(%" + token[i].Trim().ToUpper() + "%' ) ");
                                ok = true;
                                if (token.Length == 1)
                                {
                                    uf.Append(")");
                                }
                            }
                            else
                            {
                                if (i == (token.Length - 1))
                                {
                                    uf.Append(" and ( " + course_name + " like '%(%" + token[i].Trim().ToUpper() + "%' )");
                                    uf.Append(")");
                                }
                                else
                                {
                                    uf.Append(" and ( " + course_name + " like '%(%" + token[i].Trim().ToUpper() + "%' )");
                                }
                            }
                        }
                    }
                    uf.Append(" or ");
                    for (int j = 0; j < tokens.Count; j++)
                    {
                        string[] token = (string[])tokens[j];
                        if (j != 0)
                        {
                            uf.Append(" or ");
                        }
                        for (int i = 0; i < token.Length; i++)
                        {

                            if (i == 0)
                            {
                                uf.Append("(");

                                if (token[i].StartsWith("$"))
                                {
                                    string tk = token[i].Substring(1, token[i].Length - 1);
                                    string theToken = tk;
                                    int p = -1;
                                    if (tk.EndsWith("*"))
                                    {
                                        theToken = tk + "%";
                                    }
                                    else
                                    {
                                        if (tk.Contains("*"))
                                        {
                                            theToken = tk.Replace("*", "%");
                                        }
                                    }
                                    uf.Append("  ( " + course_id + " like '" + theToken.ToUpper() + "%' ) ");
                                }
                                else
                                {
                                    uf.Append("  ( " + course_id + " like '%" + token[i].Trim().ToUpper() + "%' ) ");
                                }
                                ok = true;
                                if (token.Length == 1)
                                {
                                    uf.Append(")");
                                }
                            }
                            else
                            {
                                if (i == (token.Length - 1))
                                {
                                    uf.Append(" and ( " + course_id + " like '%" + token[i].Trim().ToUpper() + "%' )");
                                    uf.Append(")");
                                }
                                else
                                {
                                    uf.Append(" and ( " + course_id + " like '%" + token[i].Trim().ToUpper() + "%' )");
                                }
                            }
                        }
                    }
                    uf.Append(") ");
                }

            }
            if (!ok)
            {
                if (uf == null)
                {
                    uf = new StringBuilder();
                }
                uf.Append(" and ");
                uf.Append(" (");
                for (int i = 0; i < BbDomainPrefixes.Length; i++)
                {
                    string conjunction = (i == 0) ? "" : " or ";
                    uf.Append(conjunction + " (   (" + course_name + " like '%(%" + BbDomainPrefixes[i].ToUpper() + "%')  or ( (" + course_id + " like  '%" + BbDomainPrefixes[i].ToUpper() + "%') )  )");
                }
                uf.Append(") ");

            }
            return;
        }

        private static bool CheckDomainCourse_id(string token)
        {
            bool retValue = false;
            foreach (string d in BbDomainPrefixes)
            {
                if (token.ToUpper().Contains(d.ToUpper()))
                {
                    retValue = true;
                    break;
                }
            }
            return retValue;
        }

        public static string SetDateFilter(string surveys, string courses, string bDate, string eDate, bool orgs)
        {

            if (BbUrl.Contains("moodle") || BbUrl.Contains("mdllinker"))
            {
                return MdlQuery.SetDateFilter(surveys, courses, bDate, eDate, orgs);
            }

            FirstEnrollDate = null;
            string retValue = null;
            StringBuilder uf = null;
            // handle dates
            if (!bDate.Trim().Equals("") && !eDate.Trim().Equals(""))
            {
                bool bok = false;
                bool eok = false;
                DateTime fDate;
                DateTime lDate;
                bok = DateTime.TryParse(bDate.Trim(), out fDate);
                eok = DateTime.TryParse(eDate.Trim(), out lDate);
                if (bok && eok && (fDate <= lDate))
                {
                    if (uf == null)
                    {
                        uf = new StringBuilder();
                    }
                    uf.Append(" and ");
                    lDate = lDate.AddDays(1);
                    // tip: BIRTHDATE={d '1953-12-23'}
                    string fd = fDate.ToString("yyyy-MM-dd");
                    string ed = lDate.ToString("yyyy-MM-dd");
                    uf.Append(" (min(cu.enrollment_date) >= {d '" + fd + "'}  ");
                    uf.Append("  and min(cu.enrollment_date) <= {d '" + ed + "'} ) ");
                    FirstEnrollDate = fd;
                }
            }
            else
            {
                // wrong dates
            }
            if (uf != null)
            {
                retValue = uf.ToString();
            }
            return retValue;
        }

        public static string SetDateFilterRetriever(string surveys, string courses, string bDate, string eDate, bool orgs)
        {
            if (isMoodle(BbUrl))
            {
                return MdlQuery.SetDateFilterRetriever(surveys, courses, bDate, eDate, orgs);
            }
            FirstEnrollDate = null;
            string retValue = null;
            StringBuilder uf = null;
            // handle dates
            if (!bDate.Trim().Equals("") && !eDate.Trim().Equals(""))
            {
                bool bok = false;
                bool eok = false;
                DateTime fDate;
                DateTime lDate;
                bok = DateTime.TryParse(bDate.Trim(), out fDate);
                eok = DateTime.TryParse(eDate.Trim(), out lDate);
                if (bok && eok && (fDate <= lDate))
                {
                    if (uf == null)
                    {
                        uf = new StringBuilder();
                    }
                    uf.Append(" and ");
                    lDate = lDate.AddDays(1);
                    // tip: BIRTHDATE={d '1953-12-23'}
                    string fd = fDate.ToString("yyyy-MM-dd");
                    string ed = lDate.ToString("yyyy-MM-dd");
                    uf.Append(" ( (select count(cu.pk1) from course_users cu where cu.role = 'S' and cu.crsmain_pk1 = c.pk1 and cu.enrollment_date >= {d '" + fd + "'}) > 0  ");
                    uf.Append("  and (select count(cu.pk1) from course_users cu where cu.role = 'S' and cu.crsmain_pk1 = c.pk1 and cu.enrollment_date > {d '" + ed + "'}) < 1 ) ");
                    FirstEnrollDate = fd;
                }
            }
            else
            {
                // wrong dates
            }
            if (uf != null)
            {
                retValue = uf.ToString();
            }
            return retValue;
        }

        public static string SetDateFilterForCourses(string surveys, string courses, string bDate, string eDate, bool orgs)
        {
            FirstEnrollDate = null;
            string retValue = null;
            StringBuilder uf = null;
            // handle dates
            if (!bDate.Trim().Equals("") && !eDate.Trim().Equals(""))
            {
                bool bok = false;
                bool eok = false;
                DateTime fDate;
                DateTime lDate;
                bok = DateTime.TryParse(bDate.Trim(), out fDate);
                eok = DateTime.TryParse(eDate.Trim(), out lDate);
                if (bok && eok && (fDate <= lDate))
                {
                    if (uf == null)
                    {
                        uf = new StringBuilder();
                    }
                    uf.Append(" and ");
                    lDate = lDate.AddDays(1);
                    // tip: BIRTHDATE={d '1953-12-23'}
                    string fd = fDate.ToString("yyyy-MM-dd");
                    string ed = lDate.ToString("yyyy-MM-dd");
                    uf.Append(" cu.enrollment_date >= {d '" + fd + "'}  ");
                    uf.Append("  and cu.enrollment_date <= {d '" + ed + "'}  ");
                    FirstEnrollDate = fd;
                }
            }
            else
            {
                // wrong dates
            }
            if (uf != null)
            {
                retValue = uf.ToString();
            }
            return retValue;
        }
        public static ArrayList GetSurvey(int count, int qpk1, string bbUrl, string token)
        {
            ArrayList a = new ArrayList();
            a.Add(count);
            a.Add(GetSurvey(qpk1, bbUrl, token));
            return a;

        }

        public static DataTable GetSurvey(int qpk1, string bbUrl, string token)
        {
            if (isMoodle(bbUrl))
            {
                return MdlQuery.GetSurvey(qpk1, bbUrl, token);
            }
            DataTable sd = null;
            if (SurveySql == null)
            {
                initSql();
            }
            string sql = SurveySql.Replace("@pk1", qpk1.ToString());
            RequesterAsync rr = new RequesterAsync(sql, bbUrl, token, true, cc);
            sd = rr.execute();
            return sd;
        }
        public static int GetUserPk1(string username, string bbUrl, string token)
        {
            int pk1 = 0;
            if (bbUrl.Contains("moodle") || bbUrl.Contains("mdllinker"))
            {
                return MdlQuery.GetUserPk1(username, bbUrl, token);
            }
            string getUserPk = "select pk1 from users where user_id  = '" + username + "'";
            RequesterAsync rr = new RequesterAsync(getUserPk, bbUrl, token, true, cc);
            DataTable theUser = rr.execute();
            if (theUser != null && theUser.Rows.Count > 0)
            {
                pk1 = Convert.ToInt32(theUser.Rows[0]["pk1"]);
            }
            return pk1;
        }

        public static DataTable GetDeployers(string tmpbbUrl, string token)
        {
            StringBuilder sb = new StringBuilder("select u.firstname,u.lastname,u.email ");
            sb.Append(" from users u inner join course_users cu on u.pk1 = cu.users_pk1 inner join course_main c on cu.crsmain_pk1 = c.pk1 ");
            sb.Append(" where c.course_name = '" + BbLibraryCourseName + "' and cu.role = 'P' and cu.users_pk1 <> " + BbInstructor_id.ToString());
            sb.Append(" order by lastname, firstname ");
            RequesterAsync rr = new RequesterAsync(sb.ToString(), tmpbbUrl, token, true, cc);
            return rr.execute();

        }
        public static ArrayList GetResponses(int count, string surveys, string bbUrl, string token, ArrayList counter)
        {
            ArrayList a = new ArrayList();
            a.Add(count);
            a.Add(GetResponses(surveys, bbUrl, token));
            a.Add(counter);
            return a;

        }
        public static DataTable GetResponses(string surveys, string bbUrl, string token)
        {
            if (isMoodle(bbUrl))
            {
                return MdlQuery.GetResponses(surveys, bbUrl, token);
            }

            //  TimeSpan b = new TimeSpan(DateTime.Now.Ticks);
            DataTable sd = null;
            if (ResultsSql == null)
            {
                initSql();
            }
            string sql = ResultsSql.Replace("@qpk1s", surveys);
            RequesterAsync rr = new RequesterAsync(sql, bbUrl, token, true, cc);
            sd = rr.execute();
            //   TimeSpan e = new TimeSpan(DateTime.Now.Ticks - b.Ticks);
            //   QTIUtility.Logger.__SpecialLogger("Get Responses\t" + e.TotalMilliseconds.ToString("N1"));
            return sd;

            /*         
                      if (ResultsSql == null)
                      {
                          initSql();
                      }

                      // try different way
                      string[] survs = surveys.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                      // new asynch routines

                      string[] urls = new string[survs.Length];
                      for (int i = 0; i < survs.Length; i++)
                      {
                          string sql = ResultsSql.Replace("@qpk1s", survs[i]);
                          sql = HttpUtility.UrlEncode(sql);
                          string qry = "?token=" + token + "&contents=" + sql;
                          urls[i] = bbUrl + qry;

                      }
                    //  return DoReaders.Responses(urls);
                      //QTIUtility.Utilities.logMe("End Get Responses - qds " + Environment.NewLine);
                      return QTIUtility.DoBReaders.Responses(urls);
          */



            /*

          DataTable[] dts = new DataTable[survs.Length];
          GetTable[] gts = new GetTable[survs.Length];

          // spawn the requests
          for(int i = 0; i < survs.Length; i++)
          {
            string sql = ResultsSql.Replace("@qpk1s", survs[i]);




           // QTIUtility.Utilities.logIt("Spawning request "+i.ToString()+Environment.NewLine+sql);
            gts[i] = new GetTable(sql, bbUrl, token);
            dts[i] = gts[i].execute();
            System.Windows.Forms.Application.DoEvents();

          }
          bool finished = true;
          int tries = 0;
          do
          {
            Thread.Sleep(1000);
        
            finished = true;
         //   QTIUtility.Utilities.logIt("Checking tables (tries="+tries.ToString()+")");
            for (int i = 0; i < dts.Length; i++)
            {
              // check if each dts is non-null
              if (dts[i] == null || dts[i].Rows.Count == 0)
              {
                finished = false;
              }
            }
            tries++;
          } while (!finished && tries < 20);
          // if reaches this far
          sd = dts[0].Clone();
          int cols = sd.Columns.Count;
          foreach (DataTable dt in dts)
          {
            System.Windows.Forms.Application.DoEvents();
            if (dt.Rows != null && dt.Rows.Count > 0)
            {
              foreach (DataRow dr in dt.Rows)
              {
                DataRow sdr = sd.NewRow();
                for (int col = 0; col < cols; col++)
                {
                  sdr[col] = dr[col];
                }
                sd.Rows.Add(sdr);
              }
            }

          }

          return sd;
     */



            // Yet another async try
            string[] survs = surveys.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            DataTable[] dts = new DataTable[survs.Length];
            RequesterAsync[] gts = new RequesterAsync[survs.Length];
            // spawn the requests
            for (int i = 0; i < survs.Length; i++)
            {
                sql = ResultsSql.Replace("@qpk1s", survs[i]);
                // QTIUtility.Utilities.logIt("Spawning request "+i.ToString()+Environment.NewLine+sql);
                gts[i] = new RequesterAsync(sql, bbUrl, token, true, cc);
                dts[i] = gts[i].execute();
                //System.Windows.Forms.Application.DoEvents();

            }
            bool finished = true;
            int tries = 0;
            do
            {
                Thread.Sleep(10);

                finished = true;
                //   QTIUtility.Utilities.logIt("Checking tables (tries="+tries.ToString()+")");
                for (int i = 0; i < dts.Length; i++)
                {
                    // check if each dts is non-null
                    if (dts[i] == null || dts[i].Rows.Count == 0)
                    {
                        finished = false;
                    }
                }
                tries++;
            } while (!finished && tries < 20);
            // if reaches this far
            sd = dts[0].Clone();
            int cols = sd.Columns.Count;
            foreach (DataTable dt in dts)
            {
                // System.Windows.Forms.Application.DoEvents();
                if (dt.Rows != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DataRow sdr = sd.NewRow();
                        for (int col = 0; col < cols; col++)
                        {
                            sdr[col] = dr[col];
                        }
                        sd.Rows.Add(sdr);
                    }
                }

            }
            // QTIUtility.Utilities.logMe("End Get Responses - qds ");
            return sd;


        }
        public static short AdCount = 0;
        public static void AddAssignment(int ass_id, int crs_id, string title, Single maxScore, DataRow dr, QTIDataExtract eac, string identifier, int objectbank_pk1)
        {
            if (eac.assignments.Rows == null || (eac.assignments.Rows.Find(ass_id) == null))
            {
                QTIDataExtract.assignmentsRow ar = eac.assignments.NewassignmentsRow();
                ar.id = ass_id;
                string name = title;
                ar.name = (title.Trim().Length > 255) ? title.Trim().Substring(0, title.Trim().Length - 5) + "..." : title.Trim();
                //QTIDataExtract.classesRow[] crs = (QTIDataExtract.classesRow[])eac.classes.Select("id = " + crs_id.ToString());
                QTIDataExtract.classesRow cr = (QTIDataExtract.classesRow)eac.classes.FindByid(crs_id);
                ar.identifier = cr.name;
                ar.description = dr["keywords"].ToString();
                ar.theType = (dr["assessmenttype"].ToString().Equals("3")) ? 1 : 0;
                ar.instructor_id = crs_id;
                ar.QTypes = identifier;
                ar._public = false;
                ar.scale_id = 0; // default generic scale
                ar.created = Convert.ToDateTime(dr["created"]);
                ar.lastModified = Convert.ToDateTime(dr["lastmodified"]);
                ar.objectbank_pk1 = objectbank_pk1;
                ar.cmsmaxscore = 0.0F;
                if (thisAssessmentType == AssessmentType.Quiz)
                {
                    ar.origCmsmaxscore = Convert.ToSingle(maxScore);
                }
                eac.assignments.AddassignmentsRow(ar);
                AdCount = 0;
            }

            return;

        }

        public static void AddAssignment(int ass_id, int crs_id, string title, Single maxScore, DataRow dr, QTIDataExtract eac)
        {
            string identifier = "";
            int objectbank_pk1 = 0;
            AddAssignment(ass_id, crs_id, title, maxScore, dr, eac, identifier, objectbank_pk1);
            return;


        }
        public static int AddElement(DataRow dr, QTIDataExtract eac, DataSet ds)
        {
            StringBuilder nothing = new StringBuilder();
            return AddElement(dr, eac, ds, nothing);
        }
        public static int AddElement(DataRow dr, QTIDataExtract eac, DataSet ds, StringBuilder qd)
        {
            if (isMoodle(BbUrl))
            {
                return MdlQuery.AddElement(dr, eac, ds, qd);

            }

            int retValue = Convert.ToInt32(dr["pk1"]);
            Single cmsScore = 1.0F;
            Single cmsMaxScore = 0.0F;
            if (eac.elements.FindByid(retValue) == null)
            {
                QTIDataExtract.elementsRow er = eac.elements.NewelementsRow();
                er._public = false;
                er.id = retValue;
                string erName = ds.Tables["mat_formattedtext"].Rows[0]["mat_formattedtext_Text"].ToString().Trim();
                if (erName.Length > 255)
                {
                    erName = Utilities.StripTags(erName);
                    if (erName.Length > 255)
                    {
                        erName = erName.Substring(0, 250) + "...";
                    }
                }
                er.name = erName;
                int theqType = Convert.ToInt32(dr["kind_id"]);
                string qType = ElementKindStrings[theqType];
                er.identifier = qType;

                if (thisAssessmentType == AssessmentType.Quiz)
                {
                    er.cmsscore = 1.0F;//  Convert.ToSingle(ds.Tables["itemmetadata"].Rows[0]["qmd_absolutescore_max"]);
                    er.origCmsscore = Convert.ToSingle(ds.Tables["itemmetadata"].Rows[0]["qmd_absolutescore_max"]);
                    if (ds.Tables.Contains("varequal"))
                    {
                        DataTable dt = ds.Tables["varequal"];
                        string varequal_Text = "";
                        foreach (DataRow drr in dt.Rows)
                        {
                            if (drr["respident"].ToString().Equals("response"))
                            {

                                if (theqType == (int)QType.MultipleAnswer && Convert.ToBoolean(drr["correct"]))
                                {
                                    string theVar = drr["varequal_Text"].ToString();
                                    varequal_Text += theVar + ",";
                                }
                                else
                                {
                                    if (theqType != (int)QType.MultipleAnswer)
                                    {
                                        varequal_Text += drr["varequal_Text"].ToString() + ",";
                                    }
                                }
                            }
                        }
                        if (ds.Tables.Contains("response_label"))
                        {
                            if (theqType != (int)QType.Ordering)
                            {
                                dt = ds.Tables["response_label"];
                                string response_label_Id = "";
                                foreach (DataRow drr in dt.Rows)
                                {

                                    if (varequal_Text.Contains(drr["ident"].ToString()))
                                    {
                                        response_label_Id += (Convert.ToInt32(drr["response_label_Id"]) + 1).ToString() + ","; // zero-based so add one
                                    }
                                }
                                if (!response_label_Id.Equals(String.Empty))
                                {
                                    response_label_Id = response_label_Id.Substring(0, response_label_Id.Length - 1);
                                    er.answers = response_label_Id.ToString();
                                }
                            }
                            else
                            {
                                string[] vars = varequal_Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                                dt = ds.Tables["response_label"];
                                string response_label_Id = "";
                                foreach (string theVar in vars)
                                {
                                    foreach (DataRow drr in dt.Rows)
                                    {
                                        if (theVar.Equals(drr["ident"].ToString()))
                                        {
                                            response_label_Id += (Convert.ToInt32(drr["response_label_Id"]) + 1).ToString() + ",";
                                            break;
                                        }
                                    }
                                }
                                if (!response_label_Id.Equals(String.Empty))
                                {
                                    response_label_Id = response_label_Id.Substring(0, response_label_Id.Length - 1);
                                    er.answers = response_label_Id.ToString();
                                }
                            }
                        }
                    }
                }
                else
                {
                    er.cmsscore = cmsScore;
                    er.origCmsscore = cmsMaxScore;

                }
                er.description = dr["keywords"].ToString();
                er.type_id = -1;
                er.kind_id = Convert.ToInt32(dr["kind_id"]);
                er.compcoded = 2; // multiple competencies
                QTIDataExtract.competenciesRow[] comps = (QTIDataExtract.competenciesRow[])eac.competencies.Select("orig_id=" + er.id.ToString());
                if (comps != null && comps.Length > 0)
                {
                    qd.Append("     Category = ");
                    foreach (QTIDataExtract.competenciesRow c in comps)
                    {
                        eac.AssocComps.AddAssocCompsRow(er.id, c.id);
                        qd.Append(c.name + " ");
                    }
                    qd.Append(Environment.NewLine);
                }
                else
                {
                    er.type_id = 0;
                    er.compcoded = 0;
                }
                eac.elements.AddelementsRow(er);
                AddElementKind(eac, er.kind_id, er.identifier);
            }
            return retValue;

        }

        public static int AddElement(DataRow dr, QTIDataExtract eac, Hashtable hs, StringBuilder qd)
        {
            int retValue = Convert.ToInt32(dr["pk1"]);
            // test if already present
            if (eac.elements.FindByid(retValue) == null)
            {
                QTIDataExtract.elementsRow er = eac.elements.NewelementsRow();
                er._public = false;
                er.id = retValue;
                string erName = hs["question"].ToString().Trim();
                if (erName.Length > 255)
                {
                    erName = Utilities.StripTags(erName);
                    if (erName.Length > 255)
                    {
                        erName = erName.Substring(0, 250) + "...";
                    }
                }
                er.name = erName;
                int theqType = Convert.ToInt32(dr["kind_id"]);
                string qType = ElementKindStrings[theqType];
                er.identifier = qType;
                if (thisAssessmentType == AssessmentType.Quiz)
                {

                    er.cmsscore = (Single)hs["score"];
                    if (hs["answer"] != null)
                    {
                        string answer = (string)hs["answer"];
                        if (answer.Length > 255)
                        {
                            answer = answer.Substring(0, 250) + "...";
                        }
                        er.answers = answer;
                    }
                }
                er.description = dr["keywords"].ToString();
                er.type_id = -1;
                er.kind_id = Convert.ToInt32(dr["kind_id"]);
                er.compcoded = 2; // multiple competencies
                QTIDataExtract.competenciesRow[] comps = (QTIDataExtract.competenciesRow[])eac.competencies.Select("orig_id=" + er.id.ToString());
                if (comps != null && comps.Length > 0)
                {
                    qd.Append("     Category = ");
                    foreach (QTIDataExtract.competenciesRow c in comps)
                    {
                        eac.AssocComps.AddAssocCompsRow(er.id, c.id);
                        qd.Append(c.name + " ");
                    }
                    qd.Append(Environment.NewLine);
                }
                else
                {
                    er.type_id = 0;
                    er.compcoded = 0;
                }
                eac.elements.AddelementsRow(er);
                AddElementKind(eac, er.kind_id, er.identifier);
            }
            return retValue;

        }

        public static void AddElementKind(QTIDataExtract eac, int kind_id, string name)
        {
            QTIDataExtract.elementKindsRow ekrs = (QTIDataExtract.elementKindsRow)eac.elementKinds.Rows.Find(kind_id);
            if (ekrs == null)
            {
                QTIDataExtract.elementKindsRow ekr = eac.elementKinds.NewelementKindsRow();
                ekr.id = kind_id;
                ekr.name = name;
                eac.elementKinds.AddelementKindsRow(ekr);
            }

        }
        public static void AddAssignmentDetail(int ass_id, DataRow dr, QTIDataExtract eac)
        {
            if (isMoodle(BbUrl))
            {
                MdlQuery.AddAssignmentDetail(ass_id, dr, eac);
                return;

            }

            int element_id = Convert.ToInt32(dr["pk1"]);

            // DataRow[] theCount = eac.assignmentDetail.Select("assignment_id=" + ass_id.ToString());
            QTIDataExtract.assignmentDetailRow ad = eac.assignmentDetail.NewassignmentDetailRow();
            ad.assignment_id = ass_id;
            ad.element_id = element_id;
            //  ad.ordering = Convert.ToInt16(dr["position"]);
            ad.ordering = ++AdCount;//(short) ((theCount == null || theCount.Length == 0) ? 1 : theCount.Length + 1);
            ad.weight = 1.0F;
            eac.assignmentDetail.AddassignmentDetailRow(ad);
            if (thisAssessmentType == AssessmentType.Quiz)
            {
                QTIDataExtract.assignmentsRow assRow = eac.assignments.FindByid(ass_id);
                assRow.cmsmaxscore = assRow.cmsmaxscore + 1.0F;
            }
            return;
        }
        public static Single getFeedbackMasterScore(int qrpk1, string bbUrl, string token)
        {
            Single retValue = 0.0F;
            StringBuilder getScores = new StringBuilder();
            getScores.Append(" select manual_score AS mscore, average_score AS ascore ");
            getScores.Append(" FROM GRADEBOOK_GRADE");
            getScores.Append(" WHERE (PK1 = ");
            getScores.Append(" (SELECT MAX(GRADEBOOK_GRADE_PK1) AS Expr1 ");
            getScores.Append(" FROM  ATTEMPT ");
            getScores.Append(" WHERE (QTI_RESULT_DATA_PK1 = " + qrpk1.ToString() + "))) ");
            RequesterAsync rr = new RequesterAsync(getScores.ToString(), bbUrl, token, true, cc);
            DataTable theScores = rr.execute();
            if (theScores != null && theScores.Rows.Count > 0)
            {
                Single manual = 0.0F;
                Single average = 0.0F;
                if (theScores.Rows[0]["mscore"] != DBNull.Value)
                {
                    Single.TryParse(theScores.Rows[0]["mscore"].ToString(), out manual);
                    //manual = Convert.ToSingle(theScores.Rows[0]["mscore"]);

                }
                if (theScores.Rows[0]["ascore"] != DBNull.Value)
                {
                    Single.TryParse(theScores.Rows[0]["ascore"].ToString(), out average);
                    // average = Convert.ToSingle(theScores.Rows[0]["ascore"]);

                }
                if (manual != 0.0 && manual != average)
                {
                    retValue = manual;
                }
                else
                {
                    retValue = average;
                }

            }
            return retValue;
        }

        public static void AddFeedbackMaster(int fbid, int crs_id, int ass_id, DataRow sdr, QTIDataExtract eac, string bbUrl, string token)
        {
            // Utilities.logIt("Attempt fbMaster" + fbid.ToString());
            if (isMoodle(BbUrl))
            {
                MdlQuery.AddFeedbackMaster(fbid, crs_id, ass_id, sdr, eac, bbUrl, token);
                return;
            }

            if (eac.feedbackMaster.FindByid(fbid) == null)
            {
                // Utilities.logIt("New fbMaster " + fbid.ToString());
                QTIDataExtract.feedbackMasterRow fbr = eac.feedbackMaster.NewfeedbackMasterRow();
                fbr.id = fbid;
                fbr.instructor_id = crs_id;
                fbr.assignment_id = ass_id;
                if (!(SurveyAnonymous && (thisAssessmentType == AssessmentType.Survey)))
                {
                    fbr.reviewer_id = BbQuery.getStudent(Convert.ToInt32(sdr["grandparent_pk1"]), eac);
                }
                else
                {
                    fbr.reviewer_id = -1; // 0 has special meaning
                }
                if (thisAssessmentType == AssessmentType.Quiz)
                {
                    fbr.score = getFeedbackMasterScore(fbid, bbUrl, token);
                }
                else
                {
                    fbr.score = 0.0F;
                }
                fbr.student_id = crs_id;
                fbr.class_id = crs_id;

                fbr.created = Convert.ToDateTime(sdr["created"]);
                fbr.lastModified = Convert.ToDateTime(sdr["lastModified"]);
                eac.feedbackMaster.AddfeedbackMasterRow(fbr);
            }
            return;
        }
        public static void AddFeedbackElementsAndItems(int fbid, int e_id, DataRow sdr, DataSet dsr, Single[] cmsScores, QTIDataExtract eac)
        {
            StringBuilder nothing = new StringBuilder();
            AddFeedbackElementsAndItems(fbid, e_id, sdr, dsr, cmsScores, eac, nothing);
            return;


        }
        public static void AddFeedbackElementsAndItems(int fbid, int e_id, DataRow sdr, DataSet dsr, Single[] cmsScores, QTIDataExtract eac, StringBuilder qd)
        {
            if (isMoodle(BbUrl))
            {
                MdlQuery.AddFeedbackElementsAndItems(fbid, e_id, sdr, dsr, cmsScores, eac, qd);
                return;
            }
            QTIDataExtract.feedbackElementsRow fe = eac.feedbackElements.NewfeedbackElementsRow();
            fe.id = Convert.ToInt32(sdr["rpk"]);
            fe.feedback_id = fbid;
            fe.element_id = e_id;
            fe.scale = 0;// this is skip as default
            fe.cmsscore = cmsScores[0];
            Single theScore = cmsScores[0];
            if (thisAssessmentType == AssessmentType.Quiz)
            {
                if (dsr.Tables.Contains("score"))
                {
                    DataTable scores = dsr.Tables["score"];

                    foreach (DataRow scr in scores.Rows)
                    {
                        if (scr["varname"].ToString().Equals("AUTOGRADE"))
                        {
                            if ((scr["score_value"] != DBNull.Value) && !(scr["score_value"].ToString().Equals("")))
                            {
                                theScore = Convert.ToSingle(scr["score_value"]);//
                                break;
                            }
                        }
                        if (scr["varname"].ToString().Equals("MANUALGRADE"))
                        {
                            if ((scr["score_value"] != DBNull.Value) && !(scr["score_value"].ToString().Equals("")))
                            {
                                theScore = Convert.ToSingle(scr["score_value"]);//
                                break;
                            }
                        }
                    }
                }
                if (theScore == 0.0F)
                {
                    fe.cmsscore = theScore;
                }
                else
                {
                    if (cmsScores[1] > 0)
                    {
                        fe.cmsscore = theScore / cmsScores[1];
                    }
                }
            }
            QTIDataExtract.itemsRow[] irs = (QTIDataExtract.itemsRow[])eac.items.Select("element_id = " + e_id.ToString());
            // get question xml

            int myKind = eac.elements.FindByid(e_id).kind_id;
            // end get question xml
            if (myKind == (int)QType.Ordering || myKind == (int)QType.JumbledSentence || myKind == (int)QType.Matching || myKind == (int)QType.HotSpot)
            {
                switch (myKind)
                {
                    case (int)QType.Ordering:
                        {
                            string theObs = "";
                            if (dsr.Tables.Contains("response_value") && dsr.Tables["response_value"].Rows != null && dsr.Tables["response_value"].Rows.Count > 0)
                            {
                                foreach (DataRow fdr in dsr.Tables["response_value"].Rows)
                                {
                                    if (dsr.Tables["response_value"].Columns.Contains("response_value_Text"))
                                    {
                                        theObs += fdr["response_value_Text"].ToString() + ",";
                                    }
                                }
                            }
                            if (theObs.Length > 0)
                            {
                                theObs = theObs.Substring(0, theObs.Length - 1);
                            }
                            QTIDataExtract.fbitemsRow fi = eac.fbitems.NewfbitemsRow();
                            fi.id = Convert.ToInt32(sdr["rPk"]);
                            fi.item_id = 1;
                            fi.feedback_id = fbid;
                            fi.element_id = e_id;
                            fe.scale = 1;
                            fe.n = 1;
                            fi.obs_default = false;
                            if (theObs.Length > 0)
                            {
                                fi.obs = Utilities.StripTags(theObs);
                            }
                            else
                            {

                                fi.obs = "--";
                                fe.scale = 0;


                            }
                            fi.adv_default = true;
                            fi.ref_default = true;
                            break;
                        }
                    case (int)QType.QuizBowl:
                        {
                            string rValue = "Uncompleted";
                            string theObs = "";
                            int r = 1;
                            foreach (DataRow fdr in dsr.Tables["response_value"].Rows)
                            {
                                if (fdr["response_status"].ToString().Equals("Valid"))
                                {
                                    if (dsr.Tables["response_value"].Columns.Contains("response_value_Text"))
                                    {
                                        theObs += fdr["response_value_Text"].ToString() + ",";
                                    }
                                }
                            }
                            if (theObs.Length > 0)
                            {
                                theObs = theObs.Substring(0, theObs.Length - 1);
                            }
                            QTIDataExtract.fbitemsRow fi = eac.fbitems.NewfbitemsRow();
                            fi.id = Convert.ToInt32(sdr["rPk"]);
                            fi.item_id = 1;
                            fi.feedback_id = fbid;
                            fi.element_id = e_id;
                            fe.scale = 1;
                            fe.n = 1;
                            fi.obs_default = false;
                            if (theObs.Length > 0)
                            {
                                fi.obs = Utilities.StripTags(theObs);
                            }
                            else
                            {
                                fi.obs = "--";
                                fe.scale = 0;
                            }
                            fi.adv_default = true;
                            fi.ref_default = true;
                            eac.fbitems.AddfbitemsRow(fi);
                            break;
                        }
                    case (int)QType.JumbledSentence:
                        {
                            string rValue = "Uncompleted";
                            string theObs = "";
                            int r = 1;
                            DataTable dt = dsr.Tables["response"];
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                foreach (DataRow drr in dt.Rows)
                                {
                                    string variable = drr["ident_ref"].ToString();
                                    try
                                    {
                                        string itemId = drr.GetChildRows("response_response_value")[0][2].ToString();
                                        // lookup itemID in adv field of items and assign ordering

                                        foreach (QTIDataExtract.itemsRow ir in irs)
                                        {
                                            if (ir.adv.Equals(itemId))
                                            {
                                                theObs += variable + "=" + ir.ordering.ToString() + ",";
                                                break;
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        theObs += variable + "=-1,";
                                    }
                                }
                            }
                            if (theObs.Length > 0)
                            {
                                theObs = theObs.Substring(0, theObs.Length - 1);
                            }
                            QTIDataExtract.fbitemsRow fi = eac.fbitems.NewfbitemsRow();
                            fi.id = Convert.ToInt32(sdr["rPk"]);
                            fi.item_id = 1;
                            fi.feedback_id = fbid;
                            fi.element_id = e_id;
                            fe.scale = 1;
                            fe.n = 1;
                            fi.obs_default = false;
                            if (theObs.Length > 0)
                            {
                                fi.obs = Utilities.StripTags(theObs);
                            }
                            else
                            {
                                fi.obs = "--";
                                fe.scale = 0;
                            }
                            fi.adv_default = true;
                            fi.ref_default = true;
                            eac.fbitems.AddfbitemsRow(fi);
                            break;
                        }
                    case (int)QType.Matching:
                        {
                            string rValue = "Uncompleted";
                            string theObs = "";
                            int r = 1;
                            DataTable dt = dsr.Tables["response_value"];
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                int row = 0;
                                DataRow[] drrs = dt.Select("response_status='Valid'");
                                foreach (DataRow drr in drrs)
                                {
                                    if (drr["response_status"].ToString().Equals("Valid"))
                                    {
                                        try
                                        {
                                            string rMatch = drr["response_value_Text"].ToString();
                                            string[] adv = irs[row].adv.Split(',');
                                            for (int i = 0; i < adv.Length; i++)
                                            {
                                                if (rMatch.Equals(adv[i]))
                                                {
                                                    theObs += (row + 1).ToString() + "=" + (i + 1 + drrs.Length).ToString() + ",";
                                                }
                                            }
                                        }
                                        catch
                                        {
                                            theObs += (row + 1).ToString() + "=0,";
                                        }
                                    }
                                    row++;
                                }
                            }
                            if (theObs.Length > 0)
                            {
                                theObs = theObs.Substring(0, theObs.Length - 1);
                            }

                            QTIDataExtract.fbitemsRow fi = eac.fbitems.NewfbitemsRow();
                            fi.id = Convert.ToInt32(sdr["rPk"]);
                            fi.item_id = 1;
                            fi.feedback_id = fbid;
                            fi.element_id = e_id;
                            fe.scale = 1;
                            fe.n = 1;
                            fi.obs_default = false;
                            if (theObs.Length > 0)
                            {
                                fi.obs = Utilities.StripTags(theObs);
                            }
                            else
                            {
                                fi.obs = "--";
                                fe.scale = 0;
                            }
                            fi.adv_default = true;
                            fi.ref_default = true;
                            eac.fbitems.AddfbitemsRow(fi);
                            break;
                        }
                    case (int)QType.HotSpot: // True/False
                        {
                            fe.n = 2;
                            int thisItem = -1;
                            DataTable dt = dsr.Tables["response_value"];
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                DataRow dr = dt.Rows[0];
                                if (dr["response_status"].ToString().Equals("Valid"))
                                {
                                    try
                                    {
                                        string[] xy = dr["response_value_Text"].ToString().Split(',');
                                        int x = Convert.ToInt32(xy[0]);
                                        int y = Convert.ToInt32(xy[1]);
                                        string[] r = irs[0].adv.Split(',');
                                        if ((x >= Convert.ToInt32(r[0]) && x <= Convert.ToInt32(r[2]))
                                            && (y >= Convert.ToInt32(r[1]) && y <= Convert.ToInt32(r[3])))
                                        {
                                            thisItem = 1;
                                        }
                                        else
                                        {
                                            thisItem = 2;
                                        }
                                    }
                                    catch
                                    {
                                    }
                                }
                                QTIDataExtract.fbitemsRow fi = eac.fbitems.NewfbitemsRow();
                                fi.id = Convert.ToInt32(sdr["rPk"]);
                                fi.item_id = thisItem;
                                fi.feedback_id = fbid;
                                fi.element_id = e_id;
                                fe.scale = thisItem;
                                fi.obs_default = true;
                                if (thisItem < 0)
                                {
                                    fi.obs = "--";
                                    fe.scale = 0;
                                }
                                fi.adv_default = true;
                                fi.ref_default = true;
                                eac.fbitems.AddfbitemsRow(fi);
                            }
                            break;
                        }
                }
            }
            else
            {
                int r = 0;
                foreach (DataRow respType in dsr.Tables["response_form"].Rows)
                {

                    if (respType["response_type"].ToString().Equals("str") || respType["response_type"].ToString().Equals("num"))
                    {

                        // use formatted string table
                        if (!(myKind == (int)QType.FillInTheBlankPlus || myKind == (int)QType.FillInBlank))
                        {
                            if (dsr.Tables["formatted_text"] != null && dsr.Tables["formatted_text"].Rows != null && dsr.Tables["formatted_text"].Rows.Count > 0)
                            {
                                foreach (DataRow fdr in dsr.Tables["formatted_text"].Rows)
                                {
                                    r++;
                                    // if record doesn't already exit
                                    if (eac.fbitems.Select("feedback_id = " + fbid.ToString() + " and element_id = " + e_id.ToString() + " and item_id = " + r.ToString()).Length == 0)
                                    {
                                        QTIDataExtract.fbitemsRow fi = eac.fbitems.NewfbitemsRow();
                                        fi.id = Convert.ToInt32(sdr["rPk"]);
                                        fi.item_id = r;
                                        fi.feedback_id = fbid;
                                        fi.element_id = e_id;
                                        fe.scale = 1;
                                        fe.n = 1;
                                        fi.obs_default = false;
                                        if (dsr.Tables["formatted_text"].Columns.Contains("formatted_text_Text"))
                                        {
                                            fi.obs = Utilities.StripTags(fdr["formatted_text_Text"].ToString());
                                        }
                                        else
                                        {
                                            if (dsr.Tables["formatted_text"].Columns.Contains("SMART_TEXT"))
                                            {
                                                fi.obs = Utilities.StripTags(fdr["SMART_TEXT"].ToString());
                                            }
                                            else
                                            {
                                                fi.obs = "--";
                                                fe.scale = 0;
                                            }

                                        }
                                        fi.adv_default = true;
                                        fi.ref_default = true;
                                        eac.fbitems.AddfbitemsRow(fi);
                                    }
                                }
                            }
                            else
                            {
                                if (dsr.Tables["response_value"] != null && dsr.Tables["response_value"].Rows != null && dsr.Tables["response_value"].Rows.Count > 0)
                                {
                                    foreach (DataRow fdr in dsr.Tables["response_value"].Rows)
                                    {
                                        r++;
                                        // if record doesn't already exit
                                        if (eac.fbitems.Select("feedback_id = " + fbid.ToString() + " and element_id = " + e_id.ToString() + " and item_id = " + r.ToString()).Length == 0)
                                        {
                                            QTIDataExtract.fbitemsRow fi = eac.fbitems.NewfbitemsRow();
                                            fi.id = Convert.ToInt32(sdr["rPk"]);
                                            fi.item_id = r;
                                            fi.feedback_id = fbid;
                                            fi.element_id = e_id;
                                            fe.scale = 1;
                                            fe.n = 1;
                                            fi.obs_default = false;
                                            if (dsr.Tables["response_value"].Columns.Contains("response_value_Text"))
                                            {
                                                fi.obs = Utilities.StripTags(fdr["response_value_Text"].ToString());
                                            }
                                            else
                                            {
                                                if (dsr.Tables["response_value"].Columns.Contains("SMART_TEXT"))
                                                {
                                                    fi.obs = Utilities.StripTags(fdr["SMART_TEXT"].ToString());
                                                }
                                                else
                                                {
                                                    fi.obs = "--";
                                                    fe.scale = 0;
                                                }
                                            }
                                            fi.adv_default = true;
                                            fi.ref_default = true;
                                            eac.fbitems.AddfbitemsRow(fi);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (myKind == (int)QType.FillInTheBlankPlus)
                            {


                                if (dsr.Tables["response_value"] != null && dsr.Tables["response_value"].Rows != null && dsr.Tables["response_value"].Rows.Count > 0)
                                {
                                    if (Convert.ToInt32(sdr["grandparent_pk1"]) == fbid)
                                    {
                                        int position = Convert.ToInt32(sdr["position"]);
                                        r = 1;  // concatenated responses
                                        string filter = "feedback_id = " + fbid.ToString() + " and element_id = " +
                                              e_id.ToString() + " and id = " + sdr["rPk"].ToString() + " and item_id = " + r.ToString();

                                        if ((eac.fbitems.Select(filter)).Length == 0)
                                        {
                                            QTIDataExtract.fbitemsRow fi = eac.fbitems.NewfbitemsRow();
                                            fi.id = Convert.ToInt32(sdr["rPk"]);
                                            fi.item_id = r;
                                            fi.feedback_id = fbid;
                                            fi.element_id = e_id;
                                            fe.scale = 1;
                                            fe.n = 1;
                                            fi.obs_default = false;
                                            string theObs = "";
                                            foreach (DataRow fdr in dsr.Tables["response_value"].Rows)
                                            {
                                                if (dsr.Tables["response_value"].Columns.Contains("response_value_Text"))
                                                {
                                                    theObs += Utilities.StripTags(fdr["response_value_Text"].ToString()) + ","; // the delimiter may have to change
                                                }
                                                else
                                                {
                                                    if (dsr.Tables["response_value"].Columns.Contains("SMART_TEXT"))
                                                    {
                                                        theObs += Utilities.StripTags(fdr["SMART_TEXT"].ToString()) + ",";
                                                    }
                                                }
                                            }
                                            if (theObs.Length > 0)
                                            {
                                                theObs = theObs.Substring(0, theObs.Length - 1);
                                                fi.obs = theObs;
                                            }
                                            else
                                            {
                                                fi.obs = "--";
                                                fe.scale = 0;
                                            }
                                            fi.adv_default = true;
                                            fi.ref_default = true;
                                            eac.fbitems.AddfbitemsRow(fi);
                                        }
                                    }
                                }
                            }
                            if (myKind == (int)QType.FillInBlank)
                            {


                                if (dsr.Tables["response_value"] != null && dsr.Tables["response_value"].Rows != null && dsr.Tables["response_value"].Rows.Count > 0)
                                {
                                    if (Convert.ToInt32(sdr["grandparent_pk1"]) == fbid)
                                    {
                                        int position = Convert.ToInt32(sdr["position"]);
                                        r = 0;
                                        foreach (DataRow fdr in dsr.Tables["response_value"].Rows)
                                        {
                                            r++;
                                            string filter = "feedback_id = " + fbid.ToString() + " and element_id = " +
                                              e_id.ToString() + " and id = " + sdr["rPk"].ToString() + " and item_id = " + r.ToString();

                                            if ((eac.fbitems.Select(filter)).Length == 0)
                                            {
                                                QTIDataExtract.fbitemsRow fi = eac.fbitems.NewfbitemsRow();
                                                fi.id = Convert.ToInt32(sdr["rPk"]);
                                                fi.item_id = r;
                                                fi.feedback_id = fbid;
                                                fi.element_id = e_id;
                                                fe.scale = 1;
                                                fe.n = 1;
                                                fi.obs_default = false;
                                                if (dsr.Tables["response_value"].Columns.Contains("response_value_Text"))
                                                {
                                                    fi.obs = fdr["response_value_Text"].ToString();
                                                }
                                                else
                                                {
                                                    if (dsr.Tables["response_value"].Columns.Contains("SMART_TEXT"))
                                                    {
                                                        fi.obs = Utilities.StripTags(fdr["SMART_TEXT"].ToString());
                                                    }
                                                    else
                                                    {
                                                        fi.obs = "--";
                                                        fe.scale = 0;
                                                    }

                                                }
                                                fi.adv_default = true;
                                                fi.ref_default = true;
                                                eac.fbitems.AddfbitemsRow(fi);
                                            }
                                        }
                                    }
                                }
                            }


                        }
                    }
                    else
                    {

                        // use response_value table
                        fe.n = irs.Length;
                        fe.scale = 0;
                        if (dsr.Tables["response_value"] != null && dsr.Tables["response_value"].Rows != null && dsr.Tables["response_value"].Rows.Count > 0)
                        {
                            //fe.n = irs.Length;  // default is no answers listed in question
                            string rValue = "Uncompleted";
                            foreach (DataRow fdr in dsr.Tables["response_value"].Rows)
                            {
                                r++;
                                // look for a match on the item obs
                                bool ok = true;
                                int thisItem = 0;
                                QTIDataExtract.elementsRow eRow = eac.elements.FindByid(e_id);
                                switch (eRow.kind_id)
                                {


                                    case (int)QType.MultipleChoice: // Multiple Choice
                                        {
                                            if (dsr.Tables["response_value"].Columns.Contains("response_value_Text"))
                                            {
                                                if (int.TryParse(fdr["response_value_Text"].ToString(), out thisItem))
                                                {
                                                    thisItem++;
                                                    rValue = fdr["response_value_Text"].ToString();
                                                }
                                            }
                                            break;
                                        }
                                    case (int)QType.OpinionScale: // Opinion Scale
                                        {
                                            if (dsr.Tables["response_value"].Columns.Contains("response_value_Text"))
                                            {
                                                if (int.TryParse(fdr["response_value_Text"].ToString(), out thisItem))
                                                {
                                                    thisItem++;
                                                    rValue = fdr["response_value_Text"].ToString();
                                                }
                                            }
                                            break;
                                        }
                                    case (int)QType.MultipleAnswer: // Multiple Answer
                                        {
                                            if (dsr.Tables["response_value"].Columns.Contains("response_value_Text"))
                                            {
                                                if (!fdr["response_value_Text"].ToString().ToUpper().Equals("TRUE"))
                                                {
                                                    ok = false;
                                                    rValue = fdr["response_value_Text"].ToString();

                                                }

                                                else
                                                {
                                                    thisItem = r;
                                                    rValue = fdr["response_value_Text"].ToString();

                                                }
                                            }
                                            break;
                                        }
                                    case (int)QType.TrueFalse: // True/False
                                        {
                                            fe.n = 2;
                                            if (dsr.Tables["response_value"].Columns.Contains("response_value_Text"))
                                            {
                                                if (!fdr["response_value_Text"].ToString().ToUpper().Equals("TRUE"))
                                                {
                                                    thisItem = 2;
                                                    rValue = fdr["response_value_Text"].ToString();
                                                }
                                                else
                                                {
                                                    thisItem = 1;
                                                    rValue = fdr["response_value_Text"].ToString();
                                                }
                                            }
                                            break;
                                        }
                                    case (int)QType.EitherOr://TrueFalse: // True/False
                                        {
                                            fe.n = 2;
                                            QTIDataExtract.itemsRow[] items = (QTIDataExtract.itemsRow[])eac.items.Select("element_id = " + e_id.ToString());
                                            if (dsr.Tables["response_value"].Columns.Contains("response_value_Text"))
                                            {

                                                foreach (QTIDataExtract.itemsRow item in items)
                                                {
                                                    thisItem++;
                                                    if (fdr["response_value_Text"].ToString().Equals(item.obs))
                                                    {
                                                        break;
                                                    }
                                                }

                                            }
                                            break;
                                        }
                                }
                                if (ok)
                                {
                                    QTIDataExtract.fbitemsRow fi = eac.fbitems.NewfbitemsRow();
                                    fi.id = Convert.ToInt32(sdr["rPk"]);
                                    fi.item_id = thisItem;
                                    fi.feedback_id = fbid;
                                    fe.scale = thisItem;
                                    fi.element_id = e_id;
                                    fi.obs_default = true;
                                    //fi.obs = fdr["response_value_Text"].ToString();
                                    fi.adv_default = true;
                                    fi.ref_default = true;
                                    eac.fbitems.AddfbitemsRow(fi);
                                }

                            }
                        }
                    }
                }  // End of big huge for loop namely: foreach (DataRow respType in dsr.Tables["response_form"].Rows)
            }
            eac.feedbackElements.AddfeedbackElementsRow(fe);
            return;
        }
        public static void AddFeedbackElementsAndItems(int fbid, int e_id, DataRow sdr, Hashtable hs, DataSet dsr, QTIDataExtract eac)
        {
            QTIDataExtract.feedbackElementsRow fe = eac.feedbackElements.NewfeedbackElementsRow();
            fe.id = Convert.ToInt32(sdr["rpk"]);
            fe.feedback_id = fbid;
            fe.element_id = e_id;
            Single theScore = 0.0F;
            if (thisAssessmentType == AssessmentType.Quiz)
            {
                if (dsr.Tables.Contains("score"))
                {
                    DataTable scores = dsr.Tables["score"];

                    foreach (DataRow scr in scores.Rows)
                    {
                        if (scr["varname"].ToString().Equals("MANUALGRADE"))
                        {
                            if ((scr["score_value"] != DBNull.Value) && !(scr["score_value"].ToString().Equals("")))
                            {
                                theScore = Convert.ToSingle(scr["score_value"]);//
                                break;
                            }
                        }
                        if (scr["varname"].ToString().Equals("AUTOGRADE"))
                        {
                            if ((scr["score_value"] != DBNull.Value) && !(scr["score_value"].ToString().Equals("")))
                            {
                                theScore = Convert.ToSingle(scr["score_value"]);//
                                // break;
                            }
                        }
                    }
                }

                fe.cmsscore = theScore;
            }
            QTIDataExtract.itemsRow[] irs = (QTIDataExtract.itemsRow[])eac.items.Select("element_id = " + e_id.ToString());
            // get question xml
            int myKind = eac.elements.FindByid(e_id).kind_id;
            // end get question xml
            int r = 0;
            foreach (DataRow respType in dsr.Tables["response_form"].Rows)
            {

                if (respType["response_type"].ToString().Equals("str") || respType["response_type"].ToString().Equals("num"))
                {

                    // use formatted string table
                    if (!(myKind == (int)QType.FillInTheBlankPlus || myKind == (int)QType.FillInBlank))
                    {
                        if (dsr.Tables["formatted_text"] != null && dsr.Tables["formatted_text"].Rows != null && dsr.Tables["formatted_text"].Rows.Count > 0)
                        {
                            foreach (DataRow fdr in dsr.Tables["formatted_text"].Rows)
                            {
                                r++;
                                // if record doesn't already exit
                                if (eac.fbitems.Select("feedback_id = " + fbid.ToString() + " and element_id = " + e_id.ToString() + " and item_id = " + r.ToString()).Length == 0)
                                {
                                    QTIDataExtract.fbitemsRow fi = eac.fbitems.NewfbitemsRow();
                                    fi.id = Convert.ToInt32(sdr["rPk"]);
                                    fi.item_id = r;
                                    fi.feedback_id = fbid;
                                    fi.element_id = e_id;
                                    fe.scale = 1;
                                    fe.n = 1;
                                    fi.obs_default = false;
                                    if (dsr.Tables["formatted_text"].Columns.Contains("formatted_text_Text"))
                                    {
                                        fi.obs = fdr["formatted_text_Text"].ToString();
                                    }
                                    else
                                    {
                                        if (dsr.Tables["formatted_text"].Columns.Contains("SMART_TEXT"))
                                        {
                                            fi.obs = fdr["SMART_TEXT"].ToString();
                                        }
                                        else
                                        {
                                            fi.obs = "--";
                                            fe.scale = 0;
                                        }

                                    }
                                    fi.adv_default = true;
                                    fi.ref_default = true;
                                    eac.fbitems.AddfbitemsRow(fi);

                                    QTIDataExtract.feedbackMasterRow theFbr = eac.feedbackMaster.FindByid(fbid);

                                    string stuName = "Unknown";
                                    DataRow stu = eac.students.Rows.Find(theFbr.reviewer_id);
                                    if (stu != null)
                                    {
                                        if (stu["firstname"] != DBNull.Value)
                                        {
                                            stuName = stu["firstname"].ToString();
                                        }
                                        if (stu["lastname"] != DBNull.Value)
                                        {
                                            stuName += " " + stu["lastname"].ToString() + " ";
                                        }
                                    }
                                    string responseValue = "     " + stuName + " " + r.ToString() + " -> " + fi.obs + " scale=" + fe.scale.ToString() + " N=" + fe.n.ToString() + Environment.NewLine;
                                    //  qd.Append(responseValue);
                                }
                            }
                        }
                        else
                        {
                            if (dsr.Tables["response_value"] != null && dsr.Tables["response_value"].Rows != null && dsr.Tables["response_value"].Rows.Count > 0)
                            {
                                foreach (DataRow fdr in dsr.Tables["response_value"].Rows)
                                {
                                    r++;
                                    // if record doesn't already exit
                                    if (eac.fbitems.Select("feedback_id = " + fbid.ToString() + " and element_id = " + e_id.ToString() + " and item_id = " + r.ToString()).Length == 0)
                                    {
                                        QTIDataExtract.fbitemsRow fi = eac.fbitems.NewfbitemsRow();
                                        fi.id = Convert.ToInt32(sdr["rPk"]);
                                        fi.item_id = r;
                                        fi.feedback_id = fbid;
                                        fi.element_id = e_id;
                                        fe.scale = 1;
                                        fe.n = 1;
                                        fi.obs_default = false;
                                        if (dsr.Tables["response_value"].Columns.Contains("response_value_Text"))
                                        {
                                            fi.obs = fdr["response_value_Text"].ToString();
                                        }
                                        else
                                        {
                                            if (dsr.Tables["response_value"].Columns.Contains("SMART_TEXT"))
                                            {
                                                fi.obs = fdr["SMART_TEXT"].ToString();
                                            }
                                            else
                                            {
                                                fi.obs = "--";
                                                fe.scale = 0;
                                            }

                                        }
                                        fi.adv_default = true;
                                        fi.ref_default = true;
                                        eac.fbitems.AddfbitemsRow(fi);

                                        QTIDataExtract.feedbackMasterRow theFbr = eac.feedbackMaster.FindByid(fbid);
                                        DataRow stu = eac.students.Rows.Find(theFbr.reviewer_id);
                                        string stuName = stu["firstname"].ToString() + " " + stu["lastname"].ToString();
                                        string responseValue = "     " + stuName + " " + r.ToString() + " -> " + fi.obs + " scale=" + fe.scale.ToString() + " N=" + fe.n.ToString() + Environment.NewLine;
                                        //qd.Append(responseValue);
                                    }
                                }
                            }

                        }
                    }
                    else
                    {
                        if (myKind == (int)QType.FillInTheBlankPlus)
                        {


                            if (dsr.Tables["response_value"] != null && dsr.Tables["response_value"].Rows != null && dsr.Tables["response_value"].Rows.Count > 0)
                            {
                                if (Convert.ToInt32(sdr["grandparent_pk1"]) == fbid)
                                {
                                    int position = Convert.ToInt32(sdr["position"]);
                                    r = 0;
                                    foreach (DataRow fdr in dsr.Tables["response_value"].Rows)
                                    {
                                        r++;
                                        string filter = "feedback_id = " + fbid.ToString() + " and element_id = " +
                                          e_id.ToString() + " and id = " + sdr["rPk"].ToString() + " and item_id = " + r.ToString();

                                        if ((eac.fbitems.Select(filter)).Length == 0)
                                        {
                                            QTIDataExtract.fbitemsRow fi = eac.fbitems.NewfbitemsRow();
                                            fi.id = Convert.ToInt32(sdr["rPk"]);
                                            fi.item_id = r;
                                            fi.feedback_id = fbid;
                                            fi.element_id = e_id;
                                            fe.scale = 1;
                                            fe.n = 1;
                                            fi.obs_default = false;
                                            if (dsr.Tables["response_value"].Columns.Contains("response_value_Text"))
                                            {
                                                fi.obs = fdr["response_value_Text"].ToString();
                                            }
                                            else
                                            {
                                                if (dsr.Tables["response_value"].Columns.Contains("SMART_TEXT"))
                                                {
                                                    fi.obs = fdr["SMART_TEXT"].ToString();
                                                }
                                                else
                                                {
                                                    fi.obs = "--";
                                                    fe.scale = 0;
                                                }

                                            }
                                            fi.adv_default = true;
                                            fi.ref_default = true;
                                            eac.fbitems.AddfbitemsRow(fi);

                                            QTIDataExtract.feedbackMasterRow theFbr = eac.feedbackMaster.FindByid(fbid);
                                            DataRow stu = eac.students.Rows.Find(theFbr.reviewer_id);
                                            string stuName = stu["firstname"].ToString() + " " + stu["lastname"].ToString();
                                            string responseValue = "     " + stuName + " " + r.ToString() + " -> " + fi.obs + " scale=" + fe.scale.ToString() + " N=" + fe.n.ToString() + Environment.NewLine;
                                            //qd.Append(responseValue);

                                        }
                                    }
                                }
                            }
                        }
                        if (myKind == (int)QType.FillInBlank)
                        {


                            if (dsr.Tables["response_value"] != null && dsr.Tables["response_value"].Rows != null && dsr.Tables["response_value"].Rows.Count > 0)
                            {
                                if (Convert.ToInt32(sdr["grandparent_pk1"]) == fbid)
                                {
                                    int position = Convert.ToInt32(sdr["position"]);
                                    r = 0;
                                    foreach (DataRow fdr in dsr.Tables["response_value"].Rows)
                                    {
                                        r++;
                                        string filter = "feedback_id = " + fbid.ToString() + " and element_id = " +
                                          e_id.ToString() + " and id = " + sdr["rPk"].ToString() + " and item_id = " + r.ToString();

                                        if ((eac.fbitems.Select(filter)).Length == 0)
                                        {
                                            QTIDataExtract.fbitemsRow fi = eac.fbitems.NewfbitemsRow();
                                            fi.id = Convert.ToInt32(sdr["rPk"]);
                                            fi.item_id = r;
                                            fi.feedback_id = fbid;
                                            fi.element_id = e_id;
                                            fe.scale = 1;
                                            fe.n = 1;
                                            fi.obs_default = false;
                                            if (dsr.Tables["response_value"].Columns.Contains("response_value_Text"))
                                            {
                                                fi.obs = fdr["response_value_Text"].ToString();
                                            }
                                            else
                                            {
                                                if (dsr.Tables["response_value"].Columns.Contains("SMART_TEXT"))
                                                {
                                                    fi.obs = fdr["SMART_TEXT"].ToString();
                                                }
                                                else
                                                {
                                                    fi.obs = "--";
                                                    fe.scale = 0;
                                                }

                                            }
                                            fi.adv_default = true;
                                            fi.ref_default = true;
                                            eac.fbitems.AddfbitemsRow(fi);

                                            QTIDataExtract.feedbackMasterRow theFbr = eac.feedbackMaster.FindByid(fbid);
                                            DataRow stu = eac.students.Rows.Find(theFbr.reviewer_id);
                                            string stuName = stu["firstname"].ToString() + " " + stu["lastname"].ToString();
                                            string responseValue = "     " + stuName + " " + r.ToString() + " -> " + fi.obs + " scale=" + fe.scale.ToString() + " N=" + fe.n.ToString() + Environment.NewLine;
                                            //qd.Append(responseValue);

                                        }
                                    }
                                }
                            }
                        }

                    }
                }
                else
                {

                    // use response_value table
                    fe.n = irs.Length;
                    fe.scale = 0;
                    if (dsr.Tables["response_value"] != null && dsr.Tables["response_value"].Rows != null && dsr.Tables["response_value"].Rows.Count > 0)
                    {
                        //fe.n = irs.Length;  // default is no answers listed in question
                        string rValue = "Uncompleted";
                        foreach (DataRow fdr in dsr.Tables["response_value"].Rows)
                        {
                            r++;
                            // look for a match on the item obs
                            bool ok = true;
                            int thisItem = 0;
                            QTIDataExtract.elementsRow eRow = eac.elements.FindByid(e_id);
                            switch (eRow.kind_id)
                            {
                                case (int)QType.Ordering: // Multiple Choice
                                    {
                                        QTIDataExtract.itemsRow[] items = (QTIDataExtract.itemsRow[])eac.items.Select("element_id = " + e_id.ToString());
                                        fe.n = items.Length;
                                        if (dsr.Tables["response_value"].Columns.Contains("response_value_Text"))
                                        {
                                            if (int.TryParse(fdr["response_value_Text"].ToString(), out thisItem))
                                            {
                                                thisItem++;
                                                rValue = fdr["response_value_Text"].ToString();
                                            }
                                        }
                                        break;
                                    }

                                case (int)QType.MultipleChoice: // Multiple Choice
                                    {
                                        if (dsr.Tables["response_value"].Columns.Contains("response_value_Text"))
                                        {
                                            if (int.TryParse(fdr["response_value_Text"].ToString(), out thisItem))
                                            {
                                                thisItem++;
                                                rValue = fdr["response_value_Text"].ToString();
                                            }
                                        }
                                        break;
                                    }
                                case (int)QType.OpinionScale: // Opinion Scale
                                    {
                                        if (dsr.Tables["response_value"].Columns.Contains("response_value_Text"))
                                        {
                                            if (int.TryParse(fdr["response_value_Text"].ToString(), out thisItem))
                                            {
                                                thisItem++;
                                                rValue = fdr["response_value_Text"].ToString();
                                            }
                                        }
                                        break;
                                    }
                                case (int)QType.MultipleAnswer: // Multiple Answer
                                    {
                                        if (dsr.Tables["response_value"].Columns.Contains("response_value_Text"))
                                        {
                                            if (!fdr["response_value_Text"].ToString().ToUpper().Equals("TRUE"))
                                            {
                                                ok = false;
                                                rValue = fdr["response_value_Text"].ToString();

                                            }

                                            else
                                            {
                                                thisItem = r;
                                                rValue = fdr["response_value_Text"].ToString();

                                            }
                                        }
                                        break;
                                    }
                                case (int)QType.TrueFalse: // True/False
                                    {
                                        fe.n = 2;
                                        if (dsr.Tables["response_value"].Columns.Contains("response_value_Text"))
                                        {
                                            if (!fdr["response_value_Text"].ToString().ToUpper().Equals("TRUE"))
                                            {
                                                thisItem = 2;
                                                rValue = fdr["response_value_Text"].ToString();
                                            }
                                            else
                                            {
                                                thisItem = 1;
                                                rValue = fdr["response_value_Text"].ToString();
                                            }
                                        }
                                        break;
                                    }
                                case (int)QType.EitherOr://TrueFalse: // True/False
                                    {
                                        fe.n = 2;
                                        QTIDataExtract.itemsRow[] items = (QTIDataExtract.itemsRow[])eac.items.Select("element_id = " + e_id.ToString());
                                        if (dsr.Tables["response_value"].Columns.Contains("response_value_Text"))
                                        {

                                            foreach (QTIDataExtract.itemsRow item in items)
                                            {
                                                thisItem++;
                                                if (fdr["response_value_Text"].ToString().Equals(item.obs))
                                                {
                                                    break;
                                                }
                                            }

                                        }
                                        break;
                                    }



                            }
                            if (ok)
                            {
                                QTIDataExtract.fbitemsRow fi = eac.fbitems.NewfbitemsRow();
                                fi.id = Convert.ToInt32(sdr["rPk"]);
                                fi.item_id = thisItem;
                                fi.feedback_id = fbid;
                                fe.scale = thisItem;
                                fi.element_id = e_id;
                                fi.obs_default = true;
                                //fi.obs = fdr["response_value_Text"].ToString();
                                fi.adv_default = true;
                                fi.ref_default = true;
                                eac.fbitems.AddfbitemsRow(fi);

                                QTIDataExtract.feedbackMasterRow theFbr = eac.feedbackMaster.FindByid(fbid);
                                DataRow stu = eac.students.Rows.Find(theFbr.reviewer_id);
                                if (stu != null)
                                {
                                    string stuName = stu["firstname"].ToString() + " " + stu["lastname"].ToString();
                                    string responseValue = "     " + stuName + " " + r.ToString() + " -> " + rValue + " scale=" + fe.scale.ToString() + " N=" + fe.n.ToString() + Environment.NewLine;
                                    //  qd.Append(responseValue);
                                }
                            }

                        }
                    }
                }
            }  // End of big huge for loop namely: foreach (DataRow respType in dsr.Tables["response_form"].Rows)
            eac.feedbackElements.AddfeedbackElementsRow(fe);
            return;
        }
        public static void AddItem(int element_id, int ordering, DataSet ds, QTIDataExtract eac)
        {
            QTIDataExtract.itemsRow ir = eac.items.NewitemsRow();
            ir.item_id = ordering;
            ir.element_id = element_id;
            ir.obs = Utilities.StripTags(ds.Tables["mat_formattedtext"].Rows[ordering]["mat_formattedtext_Text"].ToString());
            ir.ordering = ordering;
            eac.items.AdditemsRow(ir);
            return;
        }
        public static void AddItem(int element_id, int ordering, string obs, QTIDataExtract eac)
        {
            QTIDataExtract.itemsRow ir = eac.items.NewitemsRow();
            ir.item_id = ordering;
            ir.element_id = element_id;
            ir.obs = Utilities.StripTags(obs);
            ir.ordering = ordering;
            eac.items.AdditemsRow(ir);
            return;
        }
        public static void AddItem(int element_id, int ordering, string obs, ArrayList advs, QTIDataExtract eac)
        {
            QTIDataExtract.itemsRow ir = eac.items.NewitemsRow();
            ir.item_id = ordering;
            ir.element_id = element_id;
            ir.obs = Utilities.StripTags(obs);
            ir.adv = String.Join(",", (string[])advs.ToArray(typeof(string)));
            ir.ordering = ordering;
            eac.items.AdditemsRow(ir);
            return;
        }
        public static void AddItem(int element_id, int ordering, string obs, string adv, QTIDataExtract eac)
        {
            QTIDataExtract.itemsRow ir = eac.items.NewitemsRow();
            ir.item_id = ordering;
            ir.element_id = element_id;
            ir.obs = Utilities.StripTags(obs);
            ir.adv = adv;
            ir.ordering = ordering;
            eac.items.AdditemsRow(ir);
            return;

        }
        public static void AddTheItems(int e_id, DataRow dr, DataSet ds, QTIDataExtract eac)
        {
            StringBuilder nothing = new StringBuilder();
            AddTheItems(e_id, dr, ds, eac, nothing);
            return;
        }


        public static void AddTheItems(int e_id, DataRow dr, DataSet ds, QTIDataExtract eac, StringBuilder qd)
        {
            //bool itemCreated = false;
            if (isMoodle(BbUrl))
            {
                MdlQuery.AddTheItems(e_id, dr, ds, eac, qd);
                return;
            }
            QTIDataExtract.elementsRow er = eac.elements.FindByid(e_id);
            int myKind = er.kind_id;
            switch (myKind)
            {
                case (int)QType.TrueFalse:
                    {
                        BbQuery.AddItem(e_id, 1, "True", eac);
                        BbQuery.AddItem(e_id, 2, "False", eac);
                        break;
                    }
                case (int)QType.HotSpot:
                    {
                        DataTable dt = ds.Tables["matapplication"];
                        string theAnswer = "";
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            theAnswer = dt.Rows[0]["label"].ToString();
                        }
                        er.answers = theAnswer;
                        dt = ds.Tables["varinside"];
                        string coor = "";
                        try
                        {
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                coor = dt.Rows[0]["varinside_Text"].ToString();
                            }
                            BbQuery.AddItem(e_id, 1, "Hit", coor, eac);
                            BbQuery.AddItem(e_id, 2, "Miss", eac);
                        }
                        catch (Exception ex)
                        {
                            //Utilities.logIt("At qpk1: "+e_id.ToString()+Environment.NewLine+ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine);
                            BbQuery.AddItem(e_id, 1, "Hit", "0,0,0,0", eac);
                            BbQuery.AddItem(e_id, 2, "Miss", eac);
                        }
                        break;
                    }
                case (int)QType.FillInTheBlankPlus:
                    {
                        // response_str is not in FillInTheBlankPlus
                        if (ds.Tables.Contains("response_str"))
                        {
                            for (int q = 0; q < ds.Tables["response_str"].Rows.Count; q++)
                            {
                                string response = ds.Tables["response_str"].Rows[q]["ident"].ToString();
                                if (!response.Trim().Equals(""))
                                {
                                    // response = response;
                                    qd.Append("     " + q.ToString() + " " + response + Environment.NewLine);
                                    BbQuery.AddItem(e_id, q + 1, response, eac);
                                    // itemCreated = true;
                                }
                            }
                        }
                        else
                        {
                            for (int q = 1; q < ds.Tables["mat_formattedtext"].Rows.Count; q++)
                            {
                                string response = ds.Tables["mat_formattedtext"].Rows[q]["mat_formattedtext_Text"].ToString();
                                if (!response.Trim().Equals(""))
                                {
                                    //response = response;
                                    qd.Append("     " + q.ToString() + " " + response + Environment.NewLine);
                                    BbQuery.AddItem(e_id, q, response, eac);
                                    // itemCreated = true;
                                }
                            }

                        }
                        break;
                    }
                case (int)QType.FillInBlank:
                    {
                        if (ds.Tables.Contains("response_str"))
                        {
                            for (int q = 0; q < ds.Tables["response_str"].Rows.Count; q++)
                            {
                                string response = ds.Tables["response_str"].Rows[q]["ident"].ToString();
                                if (!response.Trim().Equals(""))
                                {
                                    //response = response;
                                    qd.Append("     " + q.ToString() + " " + response + Environment.NewLine);
                                    BbQuery.AddItem(e_id, q + 1, response, eac);
                                    // itemCreated = true;
                                }
                            }
                        }
                        else
                        {
                            for (int q = 1; q < ds.Tables["mat_formattedtext"].Rows.Count; q++)
                            {
                                string response = ds.Tables["mat_formattedtext"].Rows[q]["mat_formattedtext_Text"].ToString();
                                if (!response.Trim().Equals(""))
                                {
                                    // response = "[" + response + "]";
                                    qd.Append("     " + q.ToString() + " " + response + Environment.NewLine);
                                    BbQuery.AddItem(e_id, q + 1, response, eac);
                                    // itemCreated = true;
                                }
                            }
                        }
                        break;
                    }
                case (int)QType.EitherOr:
                    {
                        if (ds.Tables.Contains("response_str"))
                        {
                            for (int q = 0; q < ds.Tables["response_str"].Rows.Count; q++)
                            {
                                string response = ds.Tables["response_str"].Rows[q]["ident"].ToString();
                                if (!response.Trim().Equals(""))
                                {
                                    //response = response;
                                    qd.Append("     " + q.ToString() + " " + response + Environment.NewLine);
                                    BbQuery.AddItem(e_id, q, response, eac);
                                    // itemCreated = true;
                                }
                            }
                        }
                        else
                        {
                            DataTable eo = ds.Tables["mat_formattedtext"];
                            for (int q = 1; q < eo.Rows.Count; q++)
                            {
                                string response = eo.Rows[q]["mat_formattedtext_Text"].ToString();
                                if (!response.Trim().Equals(""))
                                {
                                    // response = "[" + response + "]";
                                    qd.Append("     " + q.ToString() + " " + response + Environment.NewLine);
                                    BbQuery.AddItem(e_id, q, response, eac);
                                    // itemCreated = true;
                                }
                            }
                        }
                        break;
                    }
                case (int)QType.JumbledSentence:
                    {
                        ArrayList vars = new ArrayList();
                        ArrayList itemIds = new ArrayList();
                        ArrayList items = new ArrayList();
                        DataTable dt = ds.Tables["response_lid"];
                        foreach (DataRow drr in dt.Rows)
                        {
                            string ident = drr["ident"].ToString();
                            if (!vars.Contains(ident))
                            {
                                vars.Add(ident);
                            }
                            DataRow[] fs = drr.GetChildRows("response_lid_render_choice")[0].GetChildRows("render_choice_flow_label")[0].GetChildRows("flow_label_response_label");
                            foreach (DataRow dd in fs)
                            {
                                string itemId = dd["ident"].ToString();
                                if (!itemIds.Contains(itemId))
                                {
                                    itemIds.Add(itemId);
                                }
                                DataRow[] mts = dd.GetChildRows("response_label_flow_mat")[0].GetChildRows("flow_mat_material")[0].GetChildRows("material_mattext");
                                string item = mts[0]["mattext_Text"].ToString();
                                if (!items.Contains(item))
                                {
                                    items.Add(item);
                                }
                            }
                        }
                        string theAnswer = "";
                        dt = ds.Tables["respcondition"];
                        foreach (DataRow drr in dt.Rows)
                        {
                            if (drr["title"].ToString().Equals("correct"))
                            {
                                DataRow[] ans = drr.GetChildRows("respcondition_conditionvar")[0]
                                    .GetChildRows("conditionvar_and")[0]
                                    .GetChildRows("and_varequal");
                                foreach (DataRow a in ans)
                                {
                                    string aId = a["varequal_Text"].ToString();
                                    string variable = a["respident"].ToString();
                                    int p = itemIds.IndexOf(aId);
                                    theAnswer += variable + "=" + (p + 1).ToString() + ",";

                                }
                            }
                        }
                        if (theAnswer.Length > 0)
                        {
                            theAnswer = theAnswer.Substring(0, theAnswer.Length - 1);
                        }
                        er.answers = theAnswer;
                        for (int i = 0; i < vars.Count; i++)
                        {
                            BbQuery.AddItem(e_id, i + 1, (string)items[i], (string)itemIds[i], eac);
                        }
                        break;
                    }
                case (int)QType.Matching:
                    {
                        DataTable dt = ds.Tables["flow"];
                        if (dt != null)
                        {

                            DataRow[] rs = dt.Select("class='RIGHT_MATCH_BLOCK'");
                            if (rs.Length > 0)
                            {
                                ArrayList rChoices = new ArrayList();
                                foreach (DataRow g in rs)
                                {
                                    DataRow[] fs = g.GetChildRows("flow_flow");
                                    foreach (DataRow gg in fs)
                                    {
                                        DataRow[] ggg = gg.GetChildRows("flow_flow")[0]
                                            .GetChildRows("flow_material")[0]
                                            .GetChildRows("material_mat_extension")[0]
                                            .GetChildRows("mat_extension_mat_formattedtext");
                                        foreach (DataRow gh in ggg)
                                        {
                                            string choice = gh["mat_formattedtext_Text"].ToString();
                                            rChoices.Add(choice);
                                        }
                                    }
                                }
                                ArrayList lIdents = new ArrayList();
                                ArrayList[] rIdents = new ArrayList[rChoices.Count];
                                ArrayList lChoices = new ArrayList();
                                rs = dt.Select("class='RESPONSE_BLOCK'");
                                int r = 0;
                                foreach (DataRow g in rs)
                                {
                                    DataRow[] fs = g.GetChildRows("flow_flow");
                                    foreach (DataRow gg in fs)
                                    {
                                        DataRow[] ggg = gg.GetChildRows("flow_response_lid");
                                        foreach (DataRow d in ggg)
                                        {
                                            string lIdent = d["ident"].ToString();
                                            lIdents.Add(lIdent);
                                            DataRow[] lbs = d.GetChildRows("response_lid_render_choice")[0]
                                                .GetChildRows("render_choice_flow_label")[0]
                                                .GetChildRows("flow_label_response_label");
                                            ArrayList rIds = new ArrayList();
                                            foreach (DataRow lb in lbs)
                                            {
                                                string rIdent = lb["ident"].ToString();
                                                rIds.Add(rIdent);
                                            }
                                            try
                                            {
                                                rIdents[r] = rIds;
                                                r++;
                                            }
                                            catch
                                            {
                                                // odd  thatn there are not enough rChoices
                                            }
                                        }
                                        DataRow[] ggj = gg.GetChildRows("flow_flow")[0]
                                            .GetChildRows("flow_material")[0]
                                            .GetChildRows("material_mat_extension")[0]
                                            .GetChildRows("mat_extension_mat_formattedtext");
                                        foreach (DataRow gh in ggj)
                                        {
                                            string choice = gh["mat_formattedtext_Text"].ToString();
                                            lChoices.Add(choice);
                                        }
                                    }
                                }
                                dt = ds.Tables["varequal"];
                                int cc = 0;
                                string theAnswer = "";
                                foreach (DataRow c in dt.Rows)
                                {
                                    string leftIdent = c["respident"].ToString();
                                    int rr = 0;
                                    int l = 0;
                                    try
                                    {
                                        string rightIdent = c["varequal_Text"].ToString();
                                        l = lIdents.IndexOf(leftIdent);


                                        rr = rIdents[cc].IndexOf(rightIdent);
                                    }
                                    catch
                                    {
                                        rr = 0;
                                    }
                                    theAnswer += (l + 1).ToString() + "=" + (rr + 1 + lChoices.Count).ToString() + ",";
                                    cc++;
                                }
                                if (theAnswer.Length > 0)
                                {
                                    theAnswer = theAnswer.Substring(0, theAnswer.Length - 1);
                                }
                                er.answers = theAnswer;
                                // still problem here with index out of bounds because rchoices < lChoices apparently
                                for (int i = 0; i < lChoices.Count; i++)
                                {
                                    BbQuery.AddItem(e_id, i + 1, (string)lChoices[i], rIdents[i], eac);
                                }
                                for (int i = 0; i < rChoices.Count; i++)
                                {
                                    BbQuery.AddItem(e_id, i + 1 + lChoices.Count, (string)rChoices[i], eac);
                                }
                            }
                        }
                        break;
                    }
                case (int)QType.QuizBowl:
                    {
                        string theAnswer = "";
                        DataTable dt = ds.Tables["respcondition"];
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            DataRow[] ors = dt.Select("title='correct'")[0].GetChildRows("respcondition_conditionvar")[0]
                                .GetChildRows("conditionvar_and")[0]
                                .GetChildRows("and_or");
                            foreach (DataRow or in ors)
                            {
                                foreach (DataRow o in or.GetChildRows("or_varsubstring"))
                                {
                                    theAnswer += o["varsubstring_Text"].ToString() + ",";
                                }
                            }
                        }
                        if (theAnswer.Length > 0)
                        {
                            theAnswer = theAnswer.Substring(0, theAnswer.Length - 1);
                        }
                        er.answers = "1";// theAnswer;
                        BbQuery.AddItem(e_id, 1, theAnswer, eac);
                        break;
                    }
                default:
                    {
                        for (int q = 1; q < ds.Tables["mat_formattedtext"].Rows.Count; q++)
                        {
                            string response = ds.Tables["mat_formattedtext"].Rows[q]["mat_formattedtext_Text"].ToString();
                            if (!response.Trim().Equals(""))
                            {
                                qd.Append("     " + q.ToString() + " " + ds.Tables["mat_formattedtext"].Rows[q]["mat_formattedtext_Text"].ToString() + Environment.NewLine);
                                BbQuery.AddItem(e_id, q, ds, eac);
                            }
                        }
                        break;
                    }
            }
        }


        public static void AddTheItems(int e_id, DataRow dr, Hashtable hs, QTIDataExtract eac, StringBuilder qd)
        {
            //bool itemCreated = false;
            int myKind = eac.elements.FindByid(e_id).kind_id;
            BbQuery.QType qType = (BbQuery.QType)myKind;
            if (hs["responses"] != null)
            {
                ArrayList responses = (ArrayList)hs["responses"];
                StringBuilder sb = new StringBuilder("");
                int q = 1;
                switch (qType)
                {
                    case BbQuery.QType.JumbledSentence:
                        {
                            foreach (ItemResponseJS ir in responses)
                            {

                                string r = "topIdent: " + ir.topIdent + " ident = " + ir.ident + " text = " + ir.text;
                                BbQuery.AddItem(e_id, q++, r, eac);
                            }
                            break;
                        }
                    case BbQuery.QType.Matching:
                        {
                            foreach (MatchingResponse mr in responses)
                            {
                                string r = "rident = " + mr.lIdent + " text = " + mr.lText + " -> ";

                                for (int i = 0; i < mr.rAIdents.Length; i++)
                                {
                                    r += "*" + mr.rAIdents[i] + " is " + mr.rTexts[i];
                                }
                                BbQuery.AddItem(e_id, q++, r, eac);

                            }
                            break;
                        }
                    default:
                        {
                            foreach (ItemResponse ir in responses)
                            {
                                string r = "ident = " + ir.ident + " text = " + ir.text;
                                BbQuery.AddItem(e_id, q++, r, eac);
                            }

                            break;
                        }
                }

            }
        }

        public static string getStringFromData(string myData)
        {
            // QTIUtility.Utilities.logIt(myData);
            return Encoding.UTF8.GetString(Convert.FromBase64String(myData));
            // return myData.Replace("{${", "<").Replace("}$}", ">");
            string retValue = "";
            for (int i = 0; i < myData.Length; i += 2)
            {
                string hexString = myData.Substring(i, 2);
                retValue += (char)Int32.Parse(hexString, System.Globalization.NumberStyles.HexNumber);
            }


            return retValue;

        }
        public static DataSet RenderXml(string theData)
        {
            DataSet ds = new DataSet();
            StringReader stringReader = new StringReader(theData);
            XmlReaderSettings settings = new XmlReaderSettings();
            XmlReader xr = XmlTextReader.Create(stringReader, settings);
            ds.ReadXml(xr, XmlReadMode.InferSchema);
            return ds;
        }
        public static Dictionary<string, string> RenderXml(string theData, Dictionary<string, string> keys)
        {
            Dictionary<string, string> retValue =
                  new Dictionary<string, string>();

            StringReader stringReader = new StringReader(theData);
            XmlReaderSettings settings = new XmlReaderSettings();
            XmlReader xr = XmlTextReader.Create(stringReader, settings);


            XPathDocument document = new XPathDocument(xr);
            XPathNavigator navigator = document.CreateNavigator();

            foreach (KeyValuePair<string, string> kvp in keys)
            {

                XPathNodeIterator xp = navigator.Select(kvp.Key);
                string v = "";
                while (xp.MoveNext())
                {
                    v += xp.Current.Value;
                }
                retValue.Add(kvp.Key, v);
            }
            xr.Close();
            return retValue;
        }
        public static string RenderXml(string theData, string element_attribute)
        {
            string retValue = "";
            char[] delim = new char[] { '_' };
            string element = element_attribute.Split(delim)[0];
            string attribute = element_attribute.Split(delim)[1];
            StringReader stringReader = new StringReader(theData);
            XmlReaderSettings settings = new XmlReaderSettings();
            XmlReader xr = XmlTextReader.Create(stringReader, settings);
            while (xr.Read())
            {
                if (xr.NodeType == XmlNodeType.Element && xr.Name.Equals(element))
                {
                    retValue = xr.GetAttribute(attribute);
                    break;
                }
            }
            xr.Close();
            return retValue;

        }
        public static int[] getCategories(DataRow r, QTIDataExtract eac)
        {
            int[] retValue = null;
            string cpk1 = r["crsmain_pk1"].ToString();
            string qpk1 = r["pk1"].ToString();
            DataRow[] drs = allCategory.Select("cpk1=" + cpk1 + " and qpk1=" + qpk1, "", DataViewRowState.CurrentRows);
            // QTIUtility.Utilities.logIt("Cat Table Rows: "+allCategory.Rows.Count.ToString()+"This element's cats: "+drs.Length.ToString());

            if (drs != null && drs.Length > 0)
            {
                retValue = new int[drs.Length];
                int p = 0;
                foreach (DataRow dr in drs)
                {

                    retValue[p] = eac.competencies.FindByidorig_id(Convert.ToInt32(dr["id"]), Convert.ToInt32(dr["pk1"])).id;
                    p++;


                }
            }
            return retValue;
        }
        public static string getInstructors(int cpk1, QTIDataExtract eac)
        {
            string retValue = "";
            DataRow[] drs = eac.InstructorClass.Select("class_id = " + cpk1.ToString());
            foreach (DataRow dr in drs)
            {
                int id = Convert.ToInt32(dr["instructor_id"]);
                QTIDataExtract.InstructorsRow ins;
                if ((ins = (QTIDataExtract.InstructorsRow)eac.Instructors.Rows.Find(id)) != null)
                {
                    retValue += ins.FirstName + " " + ins.LastName + " (" + ins.Email + ")" + ",";
                }
            }
            if (!retValue.Equals(""))
            {
                retValue = retValue.Substring(0, retValue.Length - 1);
            }
            return retValue;
        }
        public static int getStudent(int qrpk1, QTIDataExtract eac)
        {
            int retValue = -1;
            if (allStu != null)
            {
                DataRow dr = allStu.Rows.Find(qrpk1);
                if (dr != null)
                {
                    retValue = ((QTIDataExtract.studentsRow)eac.students.FindByid(Convert.ToInt32(dr["id"]))).id;
                }
            }
            return retValue;
        }

        public static void MakeTables(string assessments, QTIDataExtract eac, string bbUrl, string token)
        {
            if (isMoodle(bbUrl))
            {
                MdlQuery.MakeTables(assessments, eac, bbUrl, token);
                return;
            }

            // categories
            string classTerm = "Course";
            QTIDataExtract.competenciesRow defaultComp = eac.competencies.NewcompetenciesRow(); // default
            defaultComp.id = 0;
            defaultComp.name = "Default";
            defaultComp.orig_id = 0;
            defaultComp.comp_set = 0;
            eac.competencies.AddcompetenciesRow(defaultComp);

            StringBuilder sb = new StringBuilder();
            sb.Append("select cat.pk1 as id, ic.qti_asi_data_pk1 as qpk1,cat.category,cat.crsmain_pk1 as cpk1 ");
            sb.Append(" from category cat ");
            sb.Append(" inner join item_category ic on cat.pk1 = ic.category_pk1 ");
            sb.Append(" inner join qti_asi_data qd on qd.pk1 = ic.qti_asi_data_pk1 ");
            sb.Append(" where ic.qti_asi_data_pk1 in (");
            sb.Append(" SELECT q3.pk1 as qpk1  ");
            sb.Append(" from qti_asi_data q3 inner join qti_asi_data q2 on q3.parent_pk1 = q2.pk1 ");
            sb.Append(" inner join qti_asi_data q1 on q2.parent_pk1 = q1.pk1 ");
            sb.Append(" where q1.pk1 in (" + assessments + ") ");
            sb.Append(" )");
            sb.Append(" and cat.category_type = 'C' ");
            RequesterAsync rr = new RequesterAsync(sb.ToString(), bbUrl, token, true, cc);
            DataTable dt = rr.execute();
            allCategory = dt;// getTable(sb.ToString(), bbUrl, token);
            if (allCategory != null && allCategory.Rows.Count > 0)
            {
                foreach (DataRow dr in allCategory.Rows)
                {
                    //QTIUtility.Utilities.logIt("AllCategories Table: catpk1=" + dr["id"].ToString() + " qti_asi_data_pk1=" + dr["qpk1"].ToString()+" " +dr["category"].ToString());
                    int id = Convert.ToInt32(dr["id"]);
                    int orig_id = Convert.ToInt32(dr["qpk1"]);
                    if ((eac.competencies.Rows == null) || (eac.competencies.FindByidorig_id(id, orig_id) == null))
                    {
                        QTIDataExtract.competenciesRow c = eac.competencies.NewcompetenciesRow();
                        c.id = id;
                        string catName = dr["category"].ToString();
                        if (catName.ToUpper().StartsWith("DEMOGRAPHIC"))
                        {
                            catName = "Demographic";
                            c.id = Int32.MaxValue;
                        }

                        c.name = catName;
                        c.comp_set = Convert.ToInt32(dr["cpk1"]);
                        c.orig_id = orig_id;
                        eac.competencies.AddcompetenciesRow(c);
                    }
                }
            }
            sb.Length = 0;
            sb.Append("select c.pk1 as cpk1,c.course_name as name, c.course_id, max(c.service_level) as classTerm, ");
            sb.Append(" (select count(pk1) from course_users where crsmain_pk1 = c.pk1 and role = 'S' and AVAILABLE_IND = 'Y' and row_status = 0) as enrollment  ");
            sb.Append(" ,(select min(enrollment_date) from course_users where crsmain_pk1 = c.pk1 and role = 'S' and AVAILABLE_IND = 'Y' and row_status = 0) as minenroll  ");
            sb.Append(" ,(select max(enrollment_date) from course_users where crsmain_pk1 = c.pk1 and role = 'S' and AVAILABLE_IND = 'Y' and row_status = 0) as maxenroll ");
            sb.Append(" from course_main c ");
            sb.Append(" inner join qti_asi_data qd on qd.crsmain_pk1 = c.pk1 ");
            sb.Append(" where qd.pk1 in (" + assessments + ") ");
            sb.Append(" group by c.pk1,c.course_name,c.course_id ");
            sb.Append(" order by name");
            rr = new RequesterAsync(sb.ToString(), bbUrl, token, true, cc);
            dt = rr.execute();
            DataTable allCourses = dt;// getTable(sb.ToString(), bbUrl, token);
            if (allCourses != null && allCourses.Rows.Count > 0)
            {
                if (allCourses.Rows[0]["classTerm"].Equals("C"))
                {
                    classTerm = "Org";
                }
            }
            foreach (DataRow dr in allCourses.Rows)
            {
                int id = Convert.ToInt32(dr["cpk1"]);
                if (eac.classes.FindByid(id) == null)
                {
                    QTIDataExtract.classesRow c = eac.classes.NewclassesRow();
                    c.id = id;
                    string theName = dr["name"].ToString().Trim();
                    string theCoursId = " (" + dr["course_id"].ToString().Trim() + ")";
                    if ((theName.Length + theCoursId).Length > 255)
                    {
                        theName = theName.Substring(0, 250 - theCoursId.Length) + "..." + theCoursId;
                    }
                    else
                    {
                        theName = theName + theCoursId;
                    }
                    c.name = Utilities.StripTags(theName);
                    c.enrollment = Convert.ToInt32(dr["enrollment"]);
                    c.cmsid = "_" + id + "_1";
                    c.orig_id = id;
                    c.minenroll = Convert.ToDateTime(dr["minenroll"]);
                    c.maxenroll = Convert.ToDateTime(dr["maxenroll"]);
                    eac.classes.AddclassesRow(c);
                }

            }

            string[] courses = new string[allCourses.Rows.Count];
            ArrayList sCs = new ArrayList();
            sCs.Add("0");
            ArrayList uCs = new ArrayList();
            uCs.Add("0");
            ArrayList suCs = new ArrayList();
            suCs.Add("0_0");
            ArrayList uuCs = new ArrayList();
            uuCs.Add("0_0");

            for (int i = 0; i < allCourses.Rows.Count; i++)
            {
                courses[i] = allCourses.Rows[i]["cpk1"].ToString();
                string[] eacds = allCourses.Rows[i]["course_id"].ToString().Split('_');
                if (eacds.Length > 0)
                {

                    if (eacds[0].ToUpper().Equals("EACD"))// course_id must have form EACD_XXXXX
                    {

                        sCs.Add(eacds[1]); // underlying pk1s of shadow course 
                        suCs.Add(eacds[1] + "_" + courses[i]); // form: [underlying_course_pk1]_[shdcrs_pk1]
                    }
                    else
                    {
                        uCs.Add(courses[i]);  // underlying course Pk1
                    }
                }
                else
                {
                    uCs.Add(courses[i]); // underlying course Pk1

                }
            }
            sb.Length = 0;
            sb.Append("select ");
            if ((BbloggedIn || BbSploggedIn) && (EacMode == EACMode.DepartmentHead || EacMode == EACMode.DepartmentHeadInstructor
                || EacMode == EACMode.DomainManager))
            {
                sb.Append(" distinct ");
            }

            sb.Append(" u.pk1,u.firstname,u.lastname,u.email,cu.crsmain_pk1 as cpk1,cu.role  ");
            sb.Append(" from users u inner join course_users cu on cu.users_pk1 = u.pk1 ");
            if ((BbloggedIn || BbSploggedIn) && EacMode == EACMode.Retriever)
            {

                sb.Append(" where cu.role in ('P','T') ");
                sb.Append(" and (");
                string[] uCourses = (string[])uCs.ToArray(typeof(string));
                sb.Append(" (u.pk1 = " + BbInstructor_id.ToString() + " and cu.crsmain_pk1 in (" + String.Join(",", uCourses) + ")) ");
                string[] sCourses = (string[])sCs.ToArray(typeof(string));
                sb.Append(" or ( cu.crsmain_pk1 in (" + String.Join(",", sCourses) + "))");
                sb.Append(" )");

            }
            else
            {
                if ((BbloggedIn || BbSploggedIn) && (EacMode == EACMode.DepartmentHead || EacMode == EACMode.DepartmentHeadInstructor
                    ))
                {
                    sb.Append(" inner join course_users ccu on ccu.crsmain_pk1 = cu.crsmain_pk1  ");
                    sb.Append(" inner join users uu on ccu.users_pk1 = uu.pk1  ");
                    sb.Append(" where cu.role in ('P','T') ");
                    sb.Append(" and uu.pk1 in (" + BbDeptMembers + ") and ccu.role='P' ");
                    //string[] uCourses = (string[])uCs.ToArray(typeof(string));
                    //sb.Append(" and (");
                    //sb.Append(" cu.crsmain_pk1 in (" + String.Join(",", uCourses) + ")");

                    sb.Append(" and (");
                    string[] uCourses = (string[])uCs.ToArray(typeof(string));
                    sb.Append(" (cu.crsmain_pk1 in (" + String.Join(",", uCourses) + ")) ");
                    string[] sCourses = (string[])sCs.ToArray(typeof(string));
                    sb.Append(" or ( cu.crsmain_pk1 in (" + String.Join(",", sCourses) + "))");
                    sb.Append(" )");

                }
                else
                {
                    if (EacMode == EACMode.Enterprise) // enterprise by login
                    {
                        sb.Append(" where cu.role in ('P','T') ");
                        sb.Append(" and (");
                        string[] uCourses = (string[])uCs.ToArray(typeof(string));
                        sb.Append(" (cu.crsmain_pk1 in (" + String.Join(",", uCourses) + ")) ");
                        string[] sCourses = (string[])sCs.ToArray(typeof(string));
                        sb.Append(" or ( cu.crsmain_pk1 in (" + String.Join(",", sCourses) + "))");
                        sb.Append(" )");

                    }
                    else
                    {
                        if (EacMode == EACMode.DomainManager)
                        {
                            StringBuilder sCoursids = new StringBuilder();
                            foreach (string s in sCs)
                            {
                                sCoursids.Append("'EACD_" + s + "',");

                            }
                            string shdcourseids = sCoursids.ToString().Substring(0, sCoursids.Length - 1);
                            sb.Append(" inner join course_main c on c.pk1 = cu.crsmain_pk1 ");
                            sb.Append(" where cu.role in ('P','T') ");
                            sb.Append(" and (");
                            string[] uCourses = (string[])uCs.ToArray(typeof(string));
                            sb.Append(" (cu.crsmain_pk1 in (" + String.Join(",", uCourses) + ")) ");
                            string[] sCourses = (string[])sCs.ToArray(typeof(string));
                            sb.Append(" or ( cu.crsmain_pk1 in (" + String.Join(",", sCourses) + "))");
                            sb.Append(" or (c.course_id in (" + shdcourseids + ")) ");
                            sb.Append(" )");

                        }
                        else // Enterprise but not logged in
                        {

                            sb.Append(" where cu.role in ('P','T') ");
                            sb.Append(" and (");
                            string[] uCourses = (string[])uCs.ToArray(typeof(string));
                            sb.Append(" (cu.crsmain_pk1 in (" + String.Join(",", uCourses) + ")) ");
                            string[] sCourses = (string[])sCs.ToArray(typeof(string));
                            sb.Append(" or ( cu.crsmain_pk1 in (" + String.Join(",", sCourses) + "))");

                            sb.Append(" )");
                        }
                    }
                }
            }

            sb.Append(" and cu.available_ind = 'Y' and cu.row_status = 0");

            rr = new RequesterAsync(sb.ToString(), bbUrl, token, true, cc);
            //     Logger.__SpecialLogger("Get instructors: " + sb.ToString() + Environment.NewLine);
            dt = rr.execute();


            DataTable allIns = dt;// getTable(sb.ToString(), bbUrl, token);
            eac.InstructorClass.Rows.Clear();

            foreach (DataRow dr in allIns.Rows)
            {
                int id = Convert.ToInt32(dr["pk1"]);
                if ((eac.Instructors.Rows == null) || (eac.Instructors.FindByid(id) == null))
                {
                    QTIDataExtract.InstructorsRow ir = eac.Instructors.NewInstructorsRow();
                    ir.id = id;
                    ir.Email = dr["email"].ToString();
                    ir.LastName = dr["lastname"].ToString().Replace("'", "");
                    ir.FirstName = dr["firstname"].ToString();
                    eac.Instructors.AddInstructorsRow(ir);
                }
                string cPk = dr["cpk1"].ToString();
                int course = Convert.ToInt32(cPk);
                if (sCs.Contains(cPk))
                {
                    for (int j = 0; j < suCs.Count; j++)
                    {
                        if (suCs[j].ToString().StartsWith(cPk + "_")) // is this course the underlying course?
                        {
                            course = Convert.ToInt32(suCs[j].ToString().Split('_')[1]); // get underlying course always
                            if ((eac.InstructorClass.Rows == null) || (eac.InstructorClass.Select("instructor_id = " + id.ToString() + " and class_id = " + course.ToString()).Length == 0))
                            {
                                QTIDataExtract.InstructorClassRow ic = eac.InstructorClass.NewInstructorClassRow();
                                ic.instructor_id = id;
                                ic.class_id = course;
                                ic.role = dr["role"].ToString();
                                eac.InstructorClass.AddInstructorClassRow(ic);
                            }
                        }
                    }
                }
                if (uCs.Contains(cPk))
                {
                    course = Convert.ToInt32(cPk);
                    if ((eac.InstructorClass.Rows == null) || (eac.InstructorClass.Select("instructor_id = " + id.ToString() + " and class_id = " + course.ToString()).Length == 0))
                    {
                        QTIDataExtract.InstructorClassRow ic = eac.InstructorClass.NewInstructorClassRow();
                        ic.instructor_id = id;
                        ic.class_id = course;
                        ic.role = dr["role"].ToString();
                        eac.InstructorClass.AddInstructorClassRow(ic);
                    }
                }
            }
            if (!(SurveyAnonymous && (thisAssessmentType == AssessmentType.Survey)))
            {
                sb.Length = 0;
                sb.Append("select qr.PK1 as qrpk1, u.PK1 AS id, u.FIRSTNAME, u.LASTNAME, u.EMAIL ");
                sb.Append(" FROM QTI_RESULT_DATA qr INNER JOIN ");
                sb.Append("QTI_ASI_DATA  qd ON qr.QTI_ASI_DATA_PK1 = qd.PK1 INNER JOIN ");
                sb.Append("ATTEMPT a ON qr.PK1 = a.QTI_RESULT_DATA_PK1 INNER JOIN ");
                sb.Append("GRADEBOOK_GRADE gb ON gb.PK1 = a.GRADEBOOK_GRADE_PK1 INNER JOIN ");
                sb.Append("COURSE_USERS cu ON gb.COURSE_USERS_PK1 = cu.PK1 INNER JOIN ");
                sb.Append("USERS u ON cu.USERS_PK1 = u.PK1 ");
                sb.Append("WHERE  (qd.PK1 IN (" + assessments + ")) ");
                RequesterAsync nr = new RequesterAsync(sb.ToString(), bbUrl, token, true, cc);
                dt = nr.execute();
                allStu = dt;// BbQuery.getTable(sb.ToString(), bbUrl, token);
                // make sure there are students
                if (allStu != null && allStu.Rows.Count > 0)
                {
                    DataColumn[] pks = new DataColumn[1];
                    pks[0] = allStu.Columns["qrpk1"];
                    allStu.PrimaryKey = pks;
                    foreach (DataRow dr in allStu.Rows)
                    {
                        int id = Convert.ToInt32(dr["id"]);
                        if ((eac.students.Rows == null) || (eac.students.Select("id = " + id.ToString()).Length == 0))
                        {
                            QTIDataExtract.studentsRow s = eac.students.NewstudentsRow();
                            s.id = id;
                            s.Firstname = dr["firstname"].ToString();
                            s.LastName = dr["lastname"].ToString();
                            s.email = dr["email"].ToString();
                            eac.students.AddstudentsRow(s);
                        }
                    }
                    // reviewers (respondents)
                    if (eac.students != null && eac.students.Rows.Count > 0)
                    {
                        foreach (QTIDataExtract.studentsRow sr in eac.students.Rows)
                        {
                            QTIDataExtract.ReviewersRow r = eac.Reviewers.NewReviewersRow();
                            r.id = sr.id;
                            r.lastname = sr.LastName;
                            r.firstname = sr.Firstname;
                            r.email = sr.email;
                            r.student_id = sr.id;
                            //  r.MI = sr.MI;
                            eac.Reviewers.AddReviewersRow(r);
                        }
                    }
                }
            }
            else
            {
                // anonymous respondents
                QTIDataExtract.studentsRow s = eac.students.NewstudentsRow();
                s.id = -1;
                s.Firstname = "Anonymous";
                s.LastName = "Respondents";
                // s.email = dr["email"].ToString();
                eac.students.AddstudentsRow(s);
                QTIDataExtract.ReviewersRow r = eac.Reviewers.NewReviewersRow();
                r.id = s.id;
                r.lastname = s.LastName;
                r.firstname = s.Firstname;
                // r.email = sr.email;
                r.student_id = s.id;
                //  r.MI = sr.MI;
                eac.Reviewers.AddReviewersRow(r);

            }
            // finally do clientParams table
            QTIDataExtract.clientParamsRow cp = eac.clientParams.NewclientParamsRow();
            switch (thisAssessmentType)
            {
                case AssessmentType.Survey: { cp.survey = true; break; }
                case AssessmentType.Quiz: { cp.survey = false; break; }
            }

            cp.classTerm = classTerm;
            cp.client_name = "";
            cp.client_id = "200000";
            cp.scaleList = "Top 5th,4th,3rd,2nd,Bottom 5th,N/A";
            cp.m = 1.0F;
            cp.b = 0.0F;
            cp.cmsurl = new Uri(BbUrl).Host;
            cp.email = BbInstructorEmail;
            cp.users_pk1 = BbInstructor_id;
            cp.client_name = BbLoggedInName;
            cp.username = BbUsername;
            eac.clientParams.AddclientParamsRow(cp);

            // set up relationship to get element items
            //  DataRelation eToi = new DataRelation("eToi", eac.elements.Columns["id"], eac.items.Columns["element_id"], false);
            //  eac.Relations.Add(eToi);

            return;
        }
        private static void initSql()
        {

            // surveySql starts here

            StringBuilder sb = new StringBuilder();
            sb.Append("select  qd.PK1, qd.PARENT_PK1,qd.crsmain_pk1, qd.POSITION, qd.BBMD_KEYWORDS as keywords, qd.data AS theData, ");
            sb.Append(" qd.BBMD_ASSESSMENTTYPE as assessmenttype, qd.qmd_absolutescore_max,qd.BBMD_QUESTIONTYPE as kind_id, qd.objectbank_pk1, ");
            sb.Append(" qd.bbmd_date_added as created,qd.bbmd_date_modified as lastmodified, ");
            sb.Append(" qd.crsmain_pk1 as cpk1, c.course_id, c.course_name as name,");
            sb.Append("(select count(pk1) from course_users where crsmain_pk1 = c.pk1 and role = 'S' and AVAILABLE_IND = 'Y' and row_status = 0) as enrollment ");
            sb.Append(" FROM QTI_ASI_DATA qd inner join course_main c on qd.crsmain_pk1 = c.pk1 ");
            sb.Append(" where qd.pk1 = @pk1 ");
            sb.Append(" or qd.parent_pk1= @pk1  ");
            sb.Append(" or qd.parent_pk1 = (select pk1 from qti_asi_data where parent_pk1 = @pk1)");
            sb.Append(" ORDER BY qd.crsmain_pk1,qd.POSITION,qd.pk1 ");
            // @pk1 and @pk2
            SurveySql = sb.ToString();
            sb.Length = 0;


            // ResultsSql starts here
            sb = new StringBuilder();
            sb.Append("select qr.pk1 as rPk, qr.POSITION, qr.parent_pk1,qr.QTI_ASI_DATA_PK1 as qPk, qr.data AS rdata, qd.objectbank_pk1, ");
            sb.Append(" qd.crsmain_pk1 as cpk1, ");
            sb.Append(" qr.bbmd_date_added as created,qr.bbmd_date_modified as lastModified,  ");
            sb.Append(" (select pk1 from QTI_RESULT_DATA where parent_pk1 is null and pk1 = (select parent_pk1 from QTI_RESULT_DATA ");
            sb.Append(" where pk1 =  qr.parent_pk1)) as grandparent_pk1 ");
            sb.Append("FROM  QTI_RESULT_DATA qr INNER JOIN ");
            sb.Append("QTI_ASI_DATA qd ON qr.QTI_ASI_DATA_PK1 = qd.PK1 ");
            sb.Append("WHERE  qr.QTI_ASI_DATA_PK1 in (@qpk1s)  ");
            //  sb.Append(" and (select count(cu.pk1) from course_users cu where cu.crsmain_pk1 = qd.crsmain_pk1 and cu.role ='S' and   ");
            //  sb.Append(" cu.row_status = 0 and cu.available_ind = 'Y') > 0 ");
            ResultsSql = sb.ToString();
            sb = null;


            /* StringBuilder getScores = new StringBuilder();
            getScores.Append(" select manual_score AS mscore, average_score AS ascore ");
            getScores.Append(" FROM GRADEBOOK_GRADE");
            getScores.Append(" WHERE (PK1 = ");
            getScores.Append(" (SELECT MAX(GRADEBOOK_GRADE_PK1) AS Expr1 ");
            getScores.Append(" FROM  ATTEMPT ");
            getScores.Append(" WHERE (QTI_RESULT_DATA_PK1 = " + qrpk1.ToString() + "))) ");
 */

        }

        private static void MakeScoreMaster(QTIDataExtract eac)
        {
            if (isMoodle(BbUrl))
            {
                MdlQuery.MakeScoreMaster(eac);
                return;
            }

            eac.Relations.Clear();
            DataRelation feToFbr = new DataRelation("feToFbr", eac.feedbackElements.Columns["feedback_id"],
              eac.feedbackMaster.Columns["id"], false);
            eac.Relations.Add(feToFbr);

            DataRelation feToElement = new DataRelation("feToElement", eac.feedbackElements.Columns["element_id"],
              eac.elements.Columns["id"], false);
            eac.Relations.Add(feToElement);

            DataRelation ElToItem = new DataRelation("ElToItem", eac.elements.Columns["id"],
              eac.items.Columns["element_id"], false);
            eac.Relations.Add(ElToItem);

            DataRelation feToFbItem = new DataRelation("feToFbItem", eac.feedbackElements.Columns["feedback_id"],
              eac.fbitems.Columns["feedback_id"], false);
            eac.Relations.Add(feToFbItem);


            foreach (QTIDataExtract.feedbackElementsRow fer in eac.feedbackElements.Rows)
            {

                DataRow[] fbs = fer.GetChildRows("feToFbItem");
                if (fbs == null || fbs.Length == 0)
                {
                    continue;
                }
                QTIDataExtract.ScoreMasterRow smr = eac.ScoreMaster.NewScoreMasterRow();
                smr.fbid = fer.feedback_id;
                smr.e_id = fer.element_id;

                smr.a_id = ((QTIDataExtract.feedbackMasterRow[])fer.GetChildRows(feToFbr))[0].assignment_id;
                smr.c_id = ((QTIDataExtract.feedbackMasterRow[])fer.GetChildRows(feToFbr))[0].class_id;
                smr.s_id = smr.c_id;
                smr.i_id = ((QTIDataExtract.feedbackMasterRow[])fer.GetChildRows(feToFbr))[0].instructor_id;
                smr.cdate = ((QTIDataExtract.feedbackMasterRow[])fer.GetChildRows(feToFbr))[0].created;
                smr.mdate = ((QTIDataExtract.feedbackMasterRow[])fer.GetChildRows(feToFbr))[0].lastModified;
                smr.r_id = ((QTIDataExtract.feedbackMasterRow[])fer.GetChildRows(feToFbr))[0].reviewer_id;
                smr.kind_id = ((QTIDataExtract.elementsRow[])fer.GetChildRows(feToElement))[0].kind_id;
                //if (fer.scale == DBNull.Value)
                //{

                //}
                smr.scale = fer.scale;
                if (fer.IsscaleNull())
                {
                    fer.scale = 0;
                }
                if (fer.IsnNull())
                {
                    fer.n = 1;
                }
                if (smr.kind_id == (int)QType.MultipleChoice || smr.kind_id == (int)QType.OpinionScale)
                {
                    ArrayList theFix = AreNA(smr, eac);

                    if (theFix.Count == 0)
                    {
                        smr.n = fer.n;
                        smr.scale = fer.scale;
                    }
                    else
                    {
                        QTIDataExtract.elementsRow er = eac.elements.FindByid(smr.e_id);
                        QTIDataExtract.itemsRow[] irs = (QTIDataExtract.itemsRow[])er.GetChildRows("ElToItem");
                        foreach (int item_id in theFix)
                        {
                            irs[item_id - 1].deductions = 1;
                        }
                        smr.n = fer.n - theFix.Count;
                        if (theFix.Contains(fer.scale))
                        {
                            smr.scale = -1;
                        }
                        else
                        {
                            foreach (int sc in theFix)
                            {
                                if (fer.scale > sc)
                                {
                                    smr.scale = (fer.scale - 1);
                                }
                            }
                        }
                    }
                }
                else
                {
                    smr.n = fer.n;
                    smr.scale = fer.scale;
                }

                smr.type_id = ((QTIDataExtract.elementsRow[])fer.GetChildRows(feToElement))[0].type_id;

                smr.ordering = ((QTIDataExtract.assignmentDetailRow[])eac.assignmentDetail.Select("assignment_id =" + smr.a_id.ToString() + " and element_id=" + smr.e_id.ToString()))[0].ordering;
                eac.ScoreMaster.AddScoreMasterRow(smr);

            }
        }
        public static void MakeAcccessExtract(string newConnection, QTIDataExtract eac)
        {

            MakeScoreMaster(eac);
            int rows = 0;
            using (OleDbConnection myConn = new OleDbConnection(newConnection))
            {
                OleDbDataAdapter da = new OleDbDataAdapter();
                OleDbCommand command = new OleDbCommand();
                DataSet ms = new DataSet();
                myConn.Open();
                DataTable s = myConn.GetSchema("tables");
                foreach (DataRow dr in s.Rows)
                {
                    if (dr["TABLE_TYPE"].ToString().Equals("TABLE"))
                    {
                        string TableName = dr["TABLE_NAME"].ToString();
                        command.CommandText = " delete * from " + TableName;
                        command.Connection = myConn;
                        int r = command.ExecuteNonQuery();
                    }
                }
                TableAdapterManager tam = new TableAdapterManager();
                tam.Connection = myConn;
                tam.assignmentDetailTableAdapter = new assignmentDetailTableAdapter();
                tam.assignmentDetailTableAdapter.ClearBeforeFill = true;
                // tam.assignmentDetailTableAdapter.Update(eac.assignmentDetail);

                tam.assignmentsTableAdapter = new assignmentsTableAdapter();
                tam.assignmentsTableAdapter.ClearBeforeFill = true;
                // tam.assignmentsTableAdapter.Update(eac.assignments);

                tam.AssocCompsTableAdapter = new AssocCompsTableAdapter();
                tam.AssocCompsTableAdapter.ClearBeforeFill = true;

                tam.classesTableAdapter = new classesTableAdapter();
                tam.classesTableAdapter.ClearBeforeFill = true;
                // tam.classesTableAdapter.Update(eac.classes);

                tam.clientParamsTableAdapter = new clientParamsTableAdapter();
                tam.clientParamsTableAdapter.ClearBeforeFill = true;
                // tam.clientParamsTableAdapter.Update(eac.clientParams);

                tam.competenciesTableAdapter = new competenciesTableAdapter();
                tam.competenciesTableAdapter.ClearBeforeFill = true;

                tam.elementsTableAdapter = new elementsTableAdapter();
                tam.elementsTableAdapter.ClearBeforeFill = true;
                //tam.elementsTableAdapter.Update(eac.elements);

                tam.fbitemsTableAdapter = new fbitemsTableAdapter();
                tam.fbitemsTableAdapter.ClearBeforeFill = true;
                //tam.fbitemsTableAdapter.Update(eac.fbitems);

                tam.feedbackElementsTableAdapter = new feedbackElementsTableAdapter();
                tam.feedbackElementsTableAdapter.ClearBeforeFill = true;
                //  tam.feedbackElementsTableAdapter.Update(eac.feedbackElements);

                tam.feedbackMasterTableAdapter = new feedbackMasterTableAdapter();
                tam.feedbackMasterTableAdapter.ClearBeforeFill = true;
                //  tam.feedbackMasterTableAdapter.Update(eac.feedbackMaster);

                tam.InstructorClassTableAdapter = new InstructorClassTableAdapter();
                tam.InstructorClassTableAdapter.ClearBeforeFill = true;
                //  tam.InstructorClassTableAdapter.Update(eac.InstructorClass);

                tam.InstructorsTableAdapter = new InstructorsTableAdapter();
                tam.InstructorsTableAdapter.ClearBeforeFill = true;
                //  tam.InstructorsTableAdapter.Update(eac.Instructors);

                tam.itemsTableAdapter = new itemsTableAdapter();
                tam.itemsTableAdapter.ClearBeforeFill = true;
                //  tam.itemsTableAdapter.Update(eac.items);

                tam.ReviewersTableAdapter = new ReviewersTableAdapter();
                tam.ReviewersTableAdapter.ClearBeforeFill = true;
                //  tam.ReviewersTableAdapter.Update(eac.Reviewers);

                tam.ScaleTypesTableAdapter = new ScaleTypesTableAdapter();
                tam.ScaleTypesTableAdapter.ClearBeforeFill = true;
                //  tam.ScaleTypesTableAdapter.Update(eac.ScaleTypes);

                tam.ScoreMasterTableAdapter = new ScoreMasterTableAdapter();
                tam.ScoreMasterTableAdapter.ClearBeforeFill = true;
                //  tam.ScoreMasterTableAdapter.Update(eac.ScoreMaster);

                tam.studentsTableAdapter = new studentsTableAdapter();
                tam.studentsTableAdapter.ClearBeforeFill = true;
                //  tam.studentsTableAdapter.Update(eac.students);

                tam.elementKindsTableAdapter = new elementKindsTableAdapter();
                tam.elementKindsTableAdapter.ClearBeforeFill = true;
                //  tam.elementKindsTableAdapter.Update(eac.elementKinds);
                rows = tam.UpdateAll(eac);
            }
        }


        private static int IsNA(int e_id, int item_id, QTIDataExtract eac)
        {
            int retValue = 0;
            QTIDataExtract.itemsRow[] items = ((QTIDataExtract.itemsRow[])eac.elements.FindByid(e_id).GetChildRows(eac.Relations["ElToItem"]));
            foreach (QTIDataExtract.itemsRow ir in items)
            {
                if (ir.obs.ToUpper().Equals("NA") || ir.obs.ToUpper().Equals("N/A") || ir.obs.ToUpper().Equals("NOT APPLICABLE")
                  || (ir.obs.ToUpper().Contains(" NOT APPL")) || (ir.obs.ToUpper().IndexOf("NOT APPLICABLE") > -1))
                {
                    retValue = (ir.item_id == item_id) ? -1 : 1;
                }

            }
            return retValue;
        }
        private static ArrayList AreNA(QTIDataExtract.ScoreMasterRow smr, QTIDataExtract eac)
        {
            ArrayList retValue = new ArrayList();
            int e_id = smr.e_id;
            QTIDataExtract.itemsRow[] items = ((QTIDataExtract.itemsRow[])eac.elements.FindByid(e_id).GetChildRows(eac.Relations["ElToItem"]));
            foreach (QTIDataExtract.itemsRow ir in items)
            {
                if (ir.obs.ToUpper().Equals("NA") || ir.obs.ToUpper().Equals("N/A") || ir.obs.ToUpper().Equals("NOT APPLICABLE")
                 || (ir.obs.ToUpper().Contains(" NOT APPL")) || (ir.obs.ToUpper().IndexOf("NOT APPLICABLE") > -1)
                    || ((smr.kind_id == (int)QType.OpinionScale || smr.kind_id == (int)QType.MultipleChoice)
                    && ir.obs.ToUpper().Equals("NO OPINION"))
                    )
                {
                    retValue.Add(ir.item_id);
                }

            }
            return retValue;
        }
        public class GetXml
        {

            public GetXml()
            {

            }

            public DataSet RenderXml(string theData, int kind_id)
            {
                // this is tricky with speical cases - to be rationalized someday
                DataSet ds = new DataSet();
                theData = getStringFromData(theData);
                BbQuery.QType theType = (QType)kind_id;
                bool typesExcluded = (theType == BbQuery.QType.JumbledSentence || theType == BbQuery.QType.Matching
                    || theType == BbQuery.QType.QuizBowl || theType == BbQuery.QType.HotSpot);
                if (!typesExcluded

                    &&
                    (

                    (BbQuery.thisAssessmentType == AssessmentType.Quiz)

                    || (theType == BbQuery.QType.MultipleChoice || theType == BbQuery.QType.OpinionScale || theType == QType.TrueFalse
                    || theType == QType.Essay || theType == QType.ShortResponse || theType == QType.MultipleAnswer
                    || theType == QType.FillInBlank || theType == QType.FillInTheBlankPlus
                    || theType == QType.EitherOr
                    )
                    )
                    )
                {

                    ds = LINQer._RenderUsingLINQ(theData, theType, BbQuery.thisAssessmentType);

                }
                else
                {
                    StringReader stringReader = new StringReader(theData);
                    XmlReaderSettings settings = new XmlReaderSettings();
                    XmlReader xr = XmlTextReader.Create(stringReader, settings);
                    ds.ReadXml(xr, XmlReadMode.InferSchema);
                }
                return ds;
            }
            public Hashtable RenderXmlHash(string theData, int kind_id)
            {
                theData = getStringFromData(theData);
                BbQuery.QType theType = (QType)kind_id;
                XElement el = XElement.Parse(theData);
                Hashtable ht = QTIUtility.LINQTesterNew._RenderLINQ(el, theType, BbQuery.thisAssessmentType);
                return ht;
            }
            public string getStringFromData(string myData)
            {
                return Encoding.UTF8.GetString(Convert.FromBase64String(myData));
                // return myData.Replace("{${", "<").Replace("}$}", ">");

            }
        }
        //private static void logIt(string message)
        //{
        //    string drive = @"c:\inetpub\wwwroot\wpBridge\";
        //    Stream myFile = File.Open(drive + "BbLinking.txt", FileMode.Append);
        //    System.Diagnostics.TextWriterTraceListener tw = new TextWriterTraceListener(myFile);
        //    System.Diagnostics.Trace.Listeners.Add(tw);
        //    DateTime logTime = DateTime.Now;
        //    tw.WriteLine(logTime.ToShortDateString() + " " + logTime.ToLongTimeString() + " " + message);
        //    tw.Flush();
        //    tw.Close();
        //}

        public static string getBbLoginUrl()
        {
            string path = BbUrl.Substring(0, BbUrl.LastIndexOf('/'));
            return path + "/" + BbQuery.BbLoginUrl;
        }
        public static string getBbLoginUrl(string BbPluginStem)
        {

            return BbPluginStem + BbQuery.BbLoginUrl;
        }
        public static string getBbDeployUrl()
        {
            //string tmpbbUrl = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Educational Assessments Corporation\Url", "Url", "");
            string path = BbUrl.Substring(0, BbUrl.LastIndexOf('/'));
            return path + "/" + BbQuery.BbDeployUrl;
        }

        public static string getBbCheckUrl()
        {
            // string tmpbbUrl = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Educational Assessments Corporation\Url", "Url", "");
            string path = BbUrl.Substring(0, BbUrl.LastIndexOf('/'));
            return path + "/" + BbQuery.BbCheckUrl;
        }
        private static bool wbDispose = false;
        //private static WebBrowser wb = new WebBrowser();

        public static void BbLogout()
        {
            String authUrl = getBbLoginUrl();// +"?logout=1";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(authUrl + "?logout=1");
            req.UserAgent = BbUserAgent;// "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0)";
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            req.AllowAutoRedirect = false;
            req.Timeout = 5000;
            // cc = new CookieContainer();
            req.CookieContainer = cc;
            //   Logger.__SpecialLogger("Logging out of " + authUrl + " with old cookies " + cc.GetCookieHeader(new Uri(authUrl)));
            try
            {
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                //  Logger.__SpecialLogger("Logging out of " + authUrl + " with response has " + resp.StatusCode);
                foreach (Cookie c in resp.Cookies)
                {
                    cc.Add(c);
                }
                //  Logger.__SpecialLogger("Logging out of " + authUrl + " with new cookies " + cc.GetCookieHeader(new Uri(authUrl)) + " and response has " + resp.StatusCode);
            }
            catch (Exception ex)
            {
                Logger.__SpecialLogger(ex.Message);
            }
        }

        public static string getToken()
        {
            string retvalue = null;
            string url = BbQuery.getBbCheckUrl();
            string tokenString = getResponse(url);
            if (tokenString.Split('=').Length > 1)
            {
                string[] tokens = tokenString.Split('=');
                retvalue = tokens[1].Trim();

            }
            return retvalue;

        }
        private static string getResponse(string url)
        {

            string retValue = null;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0)";
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            req.AllowAutoRedirect = false;
            //req.CookieContainer = cc;
            req.KeepAlive = true;
            req.Timeout = 60000;
            try
            {
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    if (resp.ContentLength != 0)
                    {
                        StreamReader sr = new StreamReader(resp.GetResponseStream());
                        retValue = sr.ReadToEnd();
                        sr.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                retValue = ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine;
            }
            return retValue;
        }

        public static void getDomainString(string bbUrl, string token, CookieContainer cct)
        {
            StringBuilder sb = new StringBuilder("select g.group_desc as descr ");
            sb.Append(" from groups g ");
            sb.Append(" inner join course_main c on c.pk1 = g.crsmain_pk1 ");
            sb.Append(" inner join course_users cu on cu.crsmain_pk1 = c.pk1 ");
            sb.Append(" inner join group_users gu on gu.groups_pk1 = g.pk1 and gu.course_users_pk1 = cu.pk1 ");
            sb.Append(" where UPPER(c.course_name) like '" + BbQuery.BbDomainCourseNamePrefix.ToUpper() + "%' and cu.users_pk1 = " + BbQuery.BbInstructor_id.ToString());
            RequesterAsync rr = new RequesterAsync(sb.ToString(), bbUrl, token, true, cct);
            //   Logger.__SpecialLogger("DomainUpDown String: " + sb.ToString());
            DataTable dt = rr.execute();
            ArrayList dns = new ArrayList();
            string[] delim = new string[] { "," };
            foreach (DataRow dr in dt.Rows)
            {
                string[] dms = Utilities.StripTags(dr["descr"].ToString()).ToUpper().Split(delim, StringSplitOptions.RemoveEmptyEntries);
                foreach (string dm in dms)
                {
                    if (!dns.Contains(dm))
                    {
                        dns.Add(dm);
                    }
                }
            }
            if (dns.Count > 0)
            {
                BbDomainPrefixes = (string[])dns.ToArray(typeof(string));
                BbDomainString = String.Join(",", BbDomainPrefixes);
            }
            else
            {
                BbDomainString = "";
            }
            return;
        }

        public static EACLoggedInType getBbLoggedInType(int id, string bbUrl, string token, CookieContainer cct)
        {
            /*public static string BbCourseMaster = "EACSupervisor";
        public static string BbCourseEnterprise = "EACDMEnterprise";
        public static string BbCourseDeployer = "EACDeployer";
        public static string BbDepotCourseId = "EACDEPOT!_";
        public static string BbLibraryCourseName = "EACLibrary";
        public static string BbShadowCourseId = "EACD!_";
        public static string BbShadowPrefix = "EACD_";
        public static string BbDomainCourseNamePrefix = "EACAdministrator";*/

            EACLoggedInType retValue = EACLoggedInType.None;
            DataTable sd = null;
            StringBuilder sb = new StringBuilder();
            sb.Append("select (select count(pk1) from course_users where role='S' and crsmain_pk1 = cm.pk1 ");
            sb.Append(" and (upper(cm.course_name) like '" + BbCourseMaster.ToUpper() + "%') and users_pk1 = " + BbInstructor_id.ToString() + ") as theCount ");
            sb.Append(" ,(select count(pk1) from course_users where role='S' and crsmain_pk1 = cm.pk1 ");
            sb.Append("  and (upper(cm.course_name) like '" + BbCourseEnterprise.ToUpper() + "%' ) and users_pk1 = " + BbInstructor_id.ToString() + ") as entCount ");
            sb.Append(" ,(select count(pk1) from course_users where (role='S' or role = 'P') and crsmain_pk1 = cm.pk1 ");
            sb.Append("  and (upper(cm.course_name) like '" + BbCourseDeployer.ToUpper() + "%' ) and users_pk1 = " + BbInstructor_id.ToString() + ") as depCount ");
            sb.Append(", (select count(g.pk1) from  course_main c inner join course_users cu ON c.pk1 = cu.crsmain_pk1 ");
            sb.Append(" inner join group_users gu ON cu.pk1 = gu.course_users_pk1  ");
            //   sb.Append(" inner join group_users ON cu.pk1 = gu.course_users_pk1 ");
            sb.Append(" inner join  groups g ON c.pk1 = g.crsmain_pk1 AND gu.groups_pk1 = g.pk1  ");
            sb.Append(" where UPPER(c.course_name) like '" + BbDomainCourseNamePrefix.ToUpper() + "%' and g.available_ind = 'Y' ");
            sb.Append(" and cu.available_ind = 'Y' and cu.users_pk1 = " + BbInstructor_id.ToString());
            sb.Append(" and c.pk1 = cm.pk1 ) as adminCount");
            sb.Append(" ,(select email from users where pk1 = " + BbInstructor_id.ToString() + ") as email ");
            sb.Append(" from users u inner join course_users cu on u.pk1 = cu.users_pk1 ");
            sb.Append(" inner join course_main cm on cu.crsmain_pk1 = cm.pk1");
            sb.Append(" where u.system_role in ( ");
            sb.Append(" select system_role from system_roles_entitlement ");
            sb.Append(" where entitlement_uid like 'system.admin%' ");
            sb.Append(" ) and cu.role = 'P' ");
            sb.Append("  and ( ");
            sb.Append(" upper(cm.course_name) like '" + BbCourseMaster.ToUpper() + "%' ");
            sb.Append(" or upper(cm.course_name) like '" + BbCourseEnterprise.ToUpper() + "%' ");
            sb.Append(" or upper(cm.course_name) like '" + BbCourseDeployer.ToUpper() + "%'  ");
            sb.Append(" or upper(cm.course_name) like '" + BbDomainCourseNamePrefix.ToUpper() + "%'  ");
            sb.Append(" )");
            string sql = sb.ToString();
            //   Logger.__SpecialLogger("LoginTypes: " + sql);
            if (token.Equals(""))
            {
                token = "EAC";
            }
            RequesterAsync rr = new RequesterAsync(sql, bbUrl, token, true, cct);
            //   Logger.__SpecialLogger("Login type SQL: " + sql);
            //Logger.__SpecialLogger("bbUrl: " + bbUrl);
            sd = rr.execute();
            BbInstructorEmail = null;
            if (sd != null && sd.Rows.Count > 0)
            {
                foreach (DataRow dr in sd.Rows)
                {
                    if (Convert.ToInt32(dr["theCount"]) == 0)
                    {
                        retValue |= EACLoggedInType.Instructor;
                    }
                    else
                    {
                        retValue |= EACLoggedInType.Admin;
                    }
                    if (Convert.ToInt32(dr["entCount"]) > 0)
                    {
                        // if token present and not = BLOCK (4d34f53389ed7f28ca91fc31ea360a66)
                        string theToken = BbQuery.getToken();
                        if (!(theToken == null || theToken.Equals(tokenBlock)))
                        {
                            retValue |= EACLoggedInType.Enterprise;
                        }
                    }
                    if (Convert.ToInt32(dr["depCount"]) > 0)
                    {
                        retValue |= EACLoggedInType.Deployer;
                        retValue |= EACLoggedInType.Retriever;
                    }
                    if (Convert.ToInt32(dr["adminCount"]) > 0)
                    {
                        retValue |= EACLoggedInType.DomainManager;

                    }
                    if (BbInstructorEmail == null)
                    {
                        BbInstructorEmail = dr["email"].ToString();
                    }
                }
            }
            return retValue;

        }

        private static DataTable[,] qds;
        private static int myCurrentCount = 0;
        private static BackgroundWorker[,] bws = null;
        private static string thebbUrl = null;
        private static string theToken = null;
        private static ArrayList theCounter = null;
        public static DataTable[,] GetBbData(ref ArrayList counter, string[] surveys, string bbUrl, string token)
        {
            myCurrentCount = 0;
            //done = false;
            qds = new DataTable[2, surveys.Length];
            bws = new BackgroundWorker[2, surveys.Length];
            thebbUrl = bbUrl;
            theToken = token;
            theCounter = counter;
            int myCount = 0;
            while (myCount < surveys.Length)
            {
                // get an assessment, criteria, and items
                bws[0, myCount] = new BackgroundWorker();
                bws[0, myCount].DoWork += new DoWorkEventHandler(BbQuery_DoWork);
                bws[0, myCount].RunWorkerCompleted += new RunWorkerCompletedEventHandler(BbQuery_RunWorkerCompleted);
                ArrayList a = new ArrayList();
                int q11 = Convert.ToInt32(surveys[myCount]);
                a.Add(myCount);
                a.Add(q11);
                a.Add(bbUrl);
                a.Add(token);
                bws[0, myCount].RunWorkerAsync(a);
                Logger.LogEvent("Launched: " + myCount.ToString());
                myCount++;

            }
            while (myCurrentCount < surveys.Length) ;
            return qds;
        }

        static void BbQuery_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ArrayList a = (ArrayList)e.Result;
            int myCount = (int)a[0];
            qds[0, myCount] = (DataTable)a[1];
            string qpk11s = "";
            foreach (DataRow dr in qds[0, myCount].Rows)
            {
                qpk11s += dr["pk1"].ToString() + ",";
            }
            qpk11s = qpk11s.Substring(0, qpk11s.Length - 1);
            bws[1, myCount] = new BackgroundWorker();
            bws[1, myCount].DoWork += new DoWorkEventHandler(BbQuery_DoWorkR);
            bws[1, myCount].RunWorkerCompleted += new RunWorkerCompletedEventHandler(BbQuery_RunWorkerCompletedR);
            a = new ArrayList();
            a.Add(myCount);
            a.Add(qpk11s);
            a.Add(thebbUrl);
            a.Add(theToken);
            a.Add(theCounter);
            bws[1, myCount].RunWorkerAsync(a);


        }
        static void BbQuery_RunWorkerCompletedR(object sender, RunWorkerCompletedEventArgs e)
        {
            ArrayList a = (ArrayList)e.Result;
            int myCount = (int)a[0];
            qds[1, myCount] = (DataTable)a[1];
            ((ArrayList)a[2]).Add(1);
            myCurrentCount++;
            Logger.LogEvent("Docked: " + myCount.ToString());

        }
        static void BbQuery_DoWork(object sender, DoWorkEventArgs e)
        {
            ArrayList a = (ArrayList)e.Argument;
            e.Result = BbQuery.GetSurvey((int)a[0], (int)a[1], (string)a[2], (string)a[3]);

        }
        static void BbQuery_DoWorkR(object sender, DoWorkEventArgs e)
        {
            ArrayList a = (ArrayList)e.Argument;
            e.Result = BbQuery.GetResponses((int)a[0], (string)a[1], (string)a[2], (string)a[3], (ArrayList)a[4]);

        }
        // users.row_status checked to be 0 
        public static DataTable GetStatus(string statusInfo, string bbUrl, string token, bool DoSurveys)
        {
            DateTime current = DateTime.Now.Add(BbServerTimeSpan);
            DateTime cuttoff = new DateTime(current.Ticks).AddDays(-14);
            string today = current.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string endShow = cuttoff.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ts = "{ ts '" + endShow + "'}";
            string shadowCourseIds = statusInfo.Split('*')[3];
            UpdateStudentStatus(shadowCourseIds, bbUrl, token);
            string qpk1 = statusInfo.Split('*')[0];
            StringBuilder sb = new StringBuilder("select u.firstname,u.lastname,u.email,c.course_name as cname,cc.start_date,cc.end_date,cc.title as title ");
            if (false && DoSurveys)
            {
                sb.Append(" ,(select max(status) from gradebook_grade g where g.gradebook_main_pk1 = gm.pk1 and g.course_users_pk1 = cu.pk1 ) as done  ");
            }
            else
            {
                sb.Append(" ,(select max(a.status) from attempt a inner join gradebook_grade g on a.gradebook_grade_pk1 = g.pk1 where g.gradebook_main_pk1 = gm.pk1 and g.course_users_pk1 = cu.pk1 ) as done  ");
                //sb.Append(" ,(select max(a.attempt_date) from attempt a inner join gradebook_grade g on a.gradebook_grade_pk1 = g.pk1 where not a.status is null and g.gradebook_main_pk1 = gm.pk1 and g.course_users_pk1 = cu.pk1 ) as lastDate  ");
                //sb.Append(" ,(select min(a.attemp_date) from attempt a inner join gradebook_grade g on a.gradebook_grade_pk1 = g.pk1 where not a.status is null and g.gradebook_main_pk1 = gm.pk1 and g.course_users_pk1 = cu.pk1 ) as minDate  ");

            }
            sb.Append(" ,q.bbmd_assessmenttype as theType ");
            sb.Append(" ,c.course_id as shdcourse_id ");
            sb.Append(" from users u inner join course_users cu on u.pk1 = cu.users_pk1 ");
            sb.Append(" inner join course_main c on c.pk1 = cu.crsmain_pk1 ");
            sb.Append(" inner join qti_asi_data q on q.crsmain_pk1 = c.pk1  ");
            sb.Append(" inner join gradebook_main gm on gm.crsmain_pk1 = c.pk1 and q.pk1 = gm.qti_asi_data_pk1 ");
            sb.Append("  inner join course_contents cc on cc.pk1 = gm.course_contents_pk1  ");
            sb.Append(" where c.course_id in (" + shadowCourseIds + ") and cu.role = 'S' and cu.row_status = 0 and cu.available_ind = 'Y' and c.institution_name like '%=" + qpk1 + "%' ");
            sb.Append(" and q.objectbank_pk1 = " + qpk1);
            sb.Append(" and cc.available_ind = 'Y' and u.row_status = 0 and u.available_ind = 'Y' ");
            sb.Append(" order by cname,lastname,firstname ");
            RequesterAsync rr = new RequesterAsync(sb.ToString(), bbUrl, token, true, cc);
            //Logger.__SpecialLogger("Bb Status by Student: " + sb.ToString());
            //   Logger.__SpecialLogger("Bb Server Time now: "+ts);
            return rr.execute();
        }

        private static void UpdateStudentStatus(string shadowCourseIds, string bbUrl, string token)
        {
            StringBuilder sb = new StringBuilder("select cu.pk1,cuu.row_status,cuu.available_ind ");
            sb.Append("from course_users cu   ");
            sb.Append(" inner join course_main c on c.pk1 = cu.crsmain_pk1  ");
            sb.Append(" inner join course_main cunder on cunder.pk1 = cast(replace(c.course_id,'EACD_','') as int) ");
            sb.Append(" inner join course_users cuu on cuu.crsmain_pk1 = cunder.pk1 ");
            sb.Append(" where c.course_id in (" + shadowCourseIds + ") and cu.role = 'S' ");
            sb.Append(" and cu.users_pk1 = cuu.users_pk1 ");
            sb.Append(" and (cu.row_status <> cuu.row_status or cu.available_ind <> cuu.available_ind) ");
            RequesterAsync rr = new RequesterAsync(sb.ToString(), bbUrl, token, true, cc);
            DataTable dt = rr.execute();
            if (dt != null)
            {

                foreach (DataRow dr in dt.Rows)
                {
                    sb.Length = 0;
                    sb.Append(" update course_users set ");
                    sb.Append("row_status =  " + dr["row_status"].ToString() + ", ");
                    sb.Append("available_ind = '" + dr["available_ind"].ToString() + "' ");
                    sb.Append(" where pk1 = " + dr["pk1"].ToString());
                    rr = new RequesterAsync(sb.ToString(), bbUrl, token, true, cc);
                    rr.execute();
                }
            }
        }
        // users.row_status checked to be 0 
        public static DataTable GetSummaryStatus(string statusInfo, string bbUrl, string token, bool DoSurveys)
        {
            DataSet sum = new DataSet();
            DateTime current = DateTime.Now.Add(BbServerTimeSpan);
            DateTime cuttoff = new DateTime(current.Ticks).AddDays(-30);
            string today = current.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string endShow = cuttoff.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string ts = "{ ts '" + endShow + "'}";
            string assName = statusInfo.Split('*')[1].Split('(')[0].Trim();
            string theSurvey = assName;
            int theType = 0; // regular
            if (assName.ToUpper().StartsWith(BbQuery.BbMultipleInstr) || assName.ToUpper().StartsWith(BbQuery.BbMultipleTA))
            {
                theType = (assName.ToUpper().Split('_')[0].Equals("TA")) ? 1 : 2;//  if(assName.ToUpper().Split("TA"))
                if (theType == 1)
                {
                    assName = assName.Replace(BbQuery.BbMultipleTA, "");
                }
                else
                {
                    assName = assName.Replace(BbQuery.BbMultipleInstr, "");
                }
            }
            //  string shadowCourseIds = statusInfo.Split('*')[2];
            string shadowCourseIds = statusInfo.Split('*')[3];
            string qpk1 = statusInfo.Split('*')[0];
            StringBuilder p = new StringBuilder("0");
            string[] course_ids = shadowCourseIds.Split(',');
            foreach (string pk in course_ids)
            {
                p.Append("," + pk.Split('_')[1].Substring(0, pk.Split('_')[1].Length - 1));
            }
            StringBuilder sb = new StringBuilder("select c.course_id,c.course_name as cname,cc.title,cc.pk1 as ccpk1,cc.available_ind,cc.start_date,cc.end_date ");
            //  StringBuilder sb = new StringBuilder("select c.course_id,c.course_name as cname,cc.start_date,cc.end_date,cc.title ");
            if (false && DoSurveys)
            {
                sb.Append(" ,(select count(g.status) from gradebook_grade g inner join course_users cu on g.course_users_pk1 = cu.pk1 inner join users u on u.pk1 = cu.users_pk1 where g.gradebook_main_pk1 = gm.pk1 and gm.course_contents_pk1 = cc.pk1 and not status is null) as done  ");
            }
            else
            {
                sb.Append(" ,(select count(a.status) from attempt a inner join gradebook_grade g on a.gradebook_grade_pk1 = g.pk1 inner join course_users cu on g.course_users_pk1 = cu.pk1 inner join users u on u.pk1 = cu.users_pk1 where g.gradebook_main_pk1 = gm.pk1 and gm.course_contents_pk1 = cc.pk1 and not a.status is null) as done  ");
            }
            sb.Append(" ,(select count(cu.pk1) from course_users cu inner join users u on u.pk1 = cu.users_pk1 where cu.role='S' and cu.crsmain_pk1 = c.pk1 and cu.available_ind = 'Y' and cu.row_status = 0 and u.row_status = 0 and u.available_ind = 'Y') as enrollment ");
            sb.Append(" ,q.bbmd_assessmenttype as theType ");
            sb.Append(" ,c.course_id as shdcourse_id ");
            sb.Append(" from course_main c inner join qti_asi_data q on q.crsmain_pk1 = c.pk1    ");
            sb.Append(" inner join gradebook_main gm on gm.crsmain_pk1 = c.pk1 and q.pk1 = gm.qti_asi_data_pk1 ");
            sb.Append("  inner join course_contents cc on cc.pk1 = gm.course_contents_pk1  ");
            sb.Append(" where c.course_id in (" + shadowCourseIds + ") and c.institution_name like '%=" + qpk1 + "%' ");
            sb.Append(" and q.objectbank_pk1 = " + qpk1);


            // sb.Append(" and cc.available_ind = 'Y' ");
            // sb.Append(" and ( (cc.start_date is null or cc.start_date <= " + ts + ") and ( cc.end_date is null or " + ts + " <= cc.end_date )  ) ");
            // Disable any cutoff
            //sb.Append(" and (  cc.end_date is null or " + ts + " <= cc.end_date   ) ");
            sb.Append(" order by c.course_id ");
            RequesterAsync rr = new RequesterAsync(sb.ToString(), bbUrl, token, true, cc);
            //   Logger.__SpecialLogger("Bb Status by Course: " + sb.ToString());
            // Logger.__SpecialLogger("Bb Summary SQL: " + sb.ToString());
            DataTable dtRaw = rr.execute();
            if (dtRaw == null || dtRaw.Rows.Count == 0)
            {
                return dtRaw;
            }
            dtRaw.TableName = "FirstTable";
            /*
             
             
             * 
             REMEMBER TO USE OFFLINE_NAME: YYY[,YYY]*   YYY = INSTRUCTOR PK1
             */
            sb = new StringBuilder(" select count(cc.pk1) as theFiles,u.pk1 as uPk1,replace(c.course_id,'" + BbShadowPrefix + "','') as cPk1 ");
            sb.Append(" from course_contents cc  ");
            sb.Append(" inner join course_main c on cc.crsmain_pk1 = c.pk1 ");
            sb.Append(" inner join course_users cu on cu.crsmain_pk1 = c.pk1 ");
            sb.Append(" inner join users u on u.pk1 = cu.users_pk1 ");
            sb.Append(" where c.course_id in (" + shadowCourseIds + " )  ");
            sb.Append(" and not cu.role in ('S') and cu.row_status = 0 and cu.available_ind = 'Y' ");
            sb.Append(" and cc.cnthndlr_handle = 'resource/x-bb-file' ");
            //  sb.Append(" and replace(cast(cc.main_data as varchar(1000)),u.email,'peter') like '%peter%' ");


            sb.Append(" and  ( ");

            sb.Append(" (replace(cast(cc.main_data as varchar(1000)),u.email,'peter') like '%peter%') ");
            sb.Append(" or ");


            sb.Append(" (replace(cc.offline_name,u.pk1,'joyce' ) like '%joyce%') ");

            sb.Append(" ) ");


            sb.Append(" and replace(cc.title,'" + assName.Replace('/', '-') + "','peter') like '%peter%' ");
            sb.Append(" group by c.course_id,u.pk1 ");
            rr = new RequesterAsync(sb.ToString(), bbUrl, token, true, cc);
            DataTable files = rr.execute();
            bool filesExist = false;
            if (files != null && files.Rows.Count > 0)
            {
                files.TableName = "files";
                filesExist = true;
            }
            DataTable dt = new DataView(dtRaw).ToTable("parentTable", true, "course_id");

            sb = new StringBuilder("select c.pk1,c.course_id,c.course_name as cname,cu.data_src_pk1 as ds,u.firstname,u.lastname,u.email,u.pk1 as uPk1 ");
            sb.Append(" from users u inner join course_users cu on cu.users_pk1 = u.pk1 ");
            sb.Append(" inner join course_main c on cu.crsmain_pk1 = c.pk1 ");
            sb.Append(" where c.pk1 in (" + p.ToString() + ") and cu.role = 'P' and u.row_status = 0 and cu.row_status = 0 and cu.available_ind='Y' ");
            sb.Append("order by c.pk1");
            rr = new RequesterAsync(sb.ToString(), bbUrl, token, true, cc);
            //    Logger.__SpecialLogger("Bb Status by Course Instructors: " + sb.ToString());
            DataTable ins = rr.execute();
            dt.Columns.Add("done", typeof(int));
            dt.Columns.Add("enrollment", typeof(int));
            dt.Columns.Add("firstname", typeof(string));
            dt.Columns.Add("lastname", typeof(string));
            dt.Columns.Add("files", typeof(int));
            dt.Columns.Add("email", typeof(string));
            dt.Columns.Add("cname", typeof(string));
            dt.Columns.Add("start_date", typeof(string));
            dt.Columns.Add("end_date", typeof(string));
            dt.Columns.Add("shdcourse_id", typeof(string));
            dt.Columns.Add("ds", typeof(int));

            if (dtRaw.DataSet != null)
            {
                dtRaw.DataSet.Tables.Remove(dtRaw);
            }
            sum.Tables.Add(dtRaw);
            sum.Tables.Add(dt);
            if (filesExist)
            {
                if (files.DataSet != null)
                {
                    files.DataSet.Tables.Remove(files);
                }
                sum.Tables.Add(files);
            }
            if (ins.DataSet != null)
            {
                ins.DataSet.Tables.Remove(ins);
            }
            sum.Tables.Add(ins);
            DataRelation reldtr = new DataRelation("dtTodtRaw", dt.Columns["course_id"], dtRaw.Columns["course_id"], false);
            sum.Relations.Add(reldtr);
            if (filesExist)
            {
                DataRelation reldtFiles = new DataRelation("reldtFiles", new DataColumn[] { ins.Columns["pk1"], ins.Columns["uPk1"] },
                    new DataColumn[] { files.Columns["cPk1"], files.Columns["uPk1"] }, false);
                sum.Relations.Add(reldtFiles);
            }
            if (theType == 0 || theType == 1 || theType == 2)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    dr["done"] = dr.GetChildRows(reldtr).Sum(t => Convert.ToInt32(t["done"]));
                    dr["enrollment"] = dr.GetChildRows(reldtr).Sum(t => Convert.ToInt32(t["enrollment"]));

                    dr["start_date"] = dr.GetChildRows(reldtr)[0]["start_date"];
                    dr["end_date"] = dr.GetChildRows(reldtr)[0]["end_date"];

                }
            }

            ins.Columns.Add("course_idEAC", typeof(string));
            foreach (DataRow dr in ins.Rows)
            {
                dr["course_idEAC"] = "EACD_" + dr["pk1"].ToString();
            }
            DataRelation rel = new DataRelation("dtToIns", dt.Columns["course_id"], ins.Columns["course_idEAC"], false);
            sum.Relations.Add(rel);
            DataTable rdt = dt.Clone();
            rdt.Columns.Add("ccpk1", typeof(string));
            rdt.Columns.Add("available_ind", typeof(string));
            foreach (DataRow dr in dt.Rows)
            {
                foreach (DataRow cr in dr.GetChildRows(rel))
                {
                    DataRow drt = rdt.NewRow();
                    drt.ItemArray = dr.ItemArray;
                    drt["shdcourse_id"] = dr.GetChildRows("dtTodtRaw")[0]["shdcourse_id"];
                    if (theType == 2)
                    {
                        foreach (DataRow drr in dr.GetChildRows("dtTodtRaw"))
                        {
                            if (drr["title"].ToString().ToUpper().Contains("_" + cr["firstname"].ToString().ToUpper().Substring(0, 1) + "_" + cr["lastname"].ToString().ToUpper().Replace("'", "") + "_"))
                            {
                                drt["ccpk1"] = drr["ccpk1"];
                                drt["available_ind"] = drr["available_ind"];
                                break;
                            }
                        }
                    }
                    else
                    {
                        drt["ccpk1"] = dr.GetChildRows("dtTodtRaw")[0]["ccpk1"];
                        drt["available_ind"] = dr.GetChildRows("dtTodtRaw")[0]["available_ind"];
                    }
                    drt["firstname"] = cr["firstname"];
                    drt["lastname"] = cr["lastname"];
                    drt["email"] = cr["email"];
                    drt["course_id"] = cr["course_id"];
                    drt["cname"] = cr["cname"];
                    drt["ds"] = cr["ds"];
                    drt["files"] = 0;
                    if (filesExist)
                    {
                        if (cr.GetChildRows("reldtFiles") != null && cr.GetChildRows("reldtFiles").Length > 0)
                        {
                            drt["files"] = cr.GetChildRows("reldtFiles")[0]["thefiles"];
                        }
                    }
                    if (theType == 2)
                    {
                        int done = 0;
                        int enrollment = 0;
                        foreach (DataRow t in dr.GetChildRows(reldtr))
                        {

                            done += (t["title"].ToString().ToUpper().Split('_')[1].Equals(cr["firstname"].ToString().ToUpper().Substring(0, 1)) && t["title"].ToString().ToUpper().Split('_')[2].Equals(cr["lastname"].ToString().ToUpper())) ? Convert.ToInt32(t["done"]) : 0;
                            enrollment = Convert.ToInt32(t["enrollment"]);
                        }
                        drt["done"] = done;
                        drt["enrollment"] = enrollment;
                    }
                    rdt.Rows.Add(drt);


                }
            }
            return rdt;
        }
        public static DataTable GetWindowDates(string shadowCourses, DataTable deployment, string bbUrl, string token)
        {

            DataTable dt = null;
            StringBuilder sql = new StringBuilder();
            sql.Append("select replace(c.course_id,'" + BbShadowPrefix + "','') as target, cc.start_date,cc.end_date  ");
            sql.Append(" from course_main c ");
            sql.Append(" inner join qti_asi_data q on q.crsmain_pk1 = c.pk1 ");
            sql.Append(" inner join gradebook_main gm on gm.crsmain_pk1 = c.pk1 and gm.qti_asi_data_pk1 = q.pk1 ");
            sql.Append(" inner join course_contents cc on gm.course_contents_pk1 = cc.pk1 and cc.crsmain_pk1 = c.pk1 ");
            sql.Append("  where c.course_id in (" + shadowCourses + ") and q.objectbank_pk1 = " + deployment.Rows[0]["qti_asi_dest_pk1"].ToString() + " and q.parent_pk1 is null  ");
            RequesterAsync rr = new RequesterAsync(sql.ToString(), bbUrl, token, true, cc);
            dt = rr.execute();
            return dt;
        }


        public static Single[] getCmsScore(int e_id, QTIDataExtract eac)
        {
            Single[] retValue = new Single[2];
            QTIDataExtract.elementsRow er = eac.elements.FindByid(e_id);
            if (er != null)
            {
                retValue[0] = er.cmsscore;
                retValue[1] = er.origCmsscore;
            }
            else
            {
                retValue[0] = 0.0F;
                retValue[1] = 0.0F;
            }
            return retValue;
        }

        public static void UpdateAdmins(string assignment, string bbUrl, string token)
        {
            if (isMoodle(bbUrl))
            {
                return;
            }
            StringBuilder sql = new StringBuilder();
            sql.Append(" select g.pk1 as gbPk1, g.manual_score  ");
            sql.Append(" from course_main c ");
            sql.Append(" inner join gradebook_main gm on gm.crsmain_pk1 = c.pk1  ");
            sql.Append(" inner join gradebook_grade g on g.gradebook_main_pk1 = gm.pk1  ");
            sql.Append(" inner join course_users cu on cu.crsmain_pk1 = c.pk1  ");
            sql.Append(" inner join users u on u.pk1 = cu.users_pk1  ");
            sql.Append(" where upper(gm.title) = '" + assignment.ToUpper() + "'");
            sql.Append(" and upper(c.course_name) = 'EACADMINISTRATOR' ");
            sql.Append(" and g.course_users_pk1 = cu.pk1 ");
            sql.Append(" and cu.role='S' ");
            sql.Append(" and u.pk1 = " + BbInstructor_id.ToString());
            RequesterAsync rr = new RequesterAsync(sql.ToString(), bbUrl, token, true, cc);
            DataTable dt = rr.execute();
            if (dt != null && dt.Rows.Count == 1)
            {
                string pk1 = dt.Rows[0]["gbPk1"].ToString();
                sql.Length = 0;
                sql.Append(" update gradebook_grade set ");
                sql.Append(" manual_score = manual_score + 1 ");
                sql.Append(" where pk1 = " + pk1);
                rr = new RequesterAsync(sql.ToString(), bbUrl, token, true, cc);
                rr.execute();
            }

        }
        public static bool isMoodle(string bbUrl)
        {
            return (BbUrl.Contains("moodle") || BbUrl.Contains("mdllinker"));
        }



        public static void DisableAssessments(string available_ind, ArrayList bb, string bbUrl, string token)
        {

            string[] pk1 = (string[])bb.ToArray(typeof(string));
            StringBuilder sql = new StringBuilder("update course_contents set ");
            sql.Append("available_ind = '" + available_ind + "' ");
            sql.Append(" where pk1 in (" + String.Join(",", pk1) + ")");
            RequesterAsync rr = new RequesterAsync(sql.ToString(), bbUrl, token, true, cc);
            rr.execute();

        }
        /****  Category/Outcome Stuff ******************************/

        public static DataTable getCMSCats(string bbUrl, string token)
        {
            DataTable retvalue = null;
            StringBuilder sql = new StringBuilder("select min(c.pk1) as CatPk1,c.category as Category ");
            sql.Append(" ,c.category_type as Category_Type ");
            sql.Append(" from category c ");
            sql.Append(" where category_type = 'C' ");
            sql.Append(" group by c.category,c.category_type ");
            RequesterAsync rr = new RequesterAsync(sql.ToString(), bbUrl, token, true, cc);
            DataTable dt = rr.execute();
            if (dt != null)
            {
                if (dt.DataSet != null)
                {
                    dt.DataSet.Tables.Remove(dt);
                }
                retvalue = dt;

            }
            return retvalue;
        }
        public static DataTable getCMSItemCats(string bbUrl, string token)
        {
            DataTable retvalue = null;
            StringBuilder sql = new StringBuilder("select ic.category_pk1,qti_asi_data_pk1 as QPk1 ");
            sql.Append(" ,case when q.bbmd_assessmenttype = 3 then 'S' else 'Q' end as Type");
            sql.Append(" from item_category ic ");
            sql.Append(" inner join qti_asi_data q on q.pk1 = ic.qti_asi_data_pk1 ");
            RequesterAsync rr = new RequesterAsync(sql.ToString(), bbUrl, token, true, cc);
            DataTable dt = rr.execute();
            if (dt != null)
            {
                if (dt.DataSet != null)
                {
                    dt.DataSet.Tables.Remove(dt);
                }
                retvalue = dt;

            }
            return retvalue;
        }
        public static XElement getQData(string qpk1, string bbUrl, string token)
        {
            StringBuilder sql = new StringBuilder("select data ");
            sql.Append(" from qti_asi_data ");
            sql.Append(" where pk1 = " + qpk1);
            RequesterAsync rr = new RequesterAsync(sql.ToString(), bbUrl, token, true, cc);
            DataTable dt = rr.execute();
            return XElement.Parse(getStringFromData(dt.Rows[0]["data"].ToString()));
        }
        public static DataTable getCMSCatsByDescription(string bbUrl, string token)
        {
            DataTable retvalue = null;
            StringBuilder sql = new StringBuilder("select c.category as Category ");
            sql.Append(" from category c ");
            sql.Append(" where category_type = 'C' ");
            sql.Append(" group by c.category ");
            sql.Append(" order by c.category ");
            RequesterAsync rr = new RequesterAsync(sql.ToString(), bbUrl, token, true, cc);
            DataTable dt = rr.execute();
            if (dt != null)
            {
                if (dt.DataSet != null)
                {
                    dt.DataSet.Tables.Remove(dt);
                }
                retvalue = dt;

            }
            return retvalue;
        }

        public static DataTable getCMSCatsByDescriptionDetails(string description, string bbUrl, string token)
        {
            DataTable retvalue = null;
            StringBuilder sql = new StringBuilder("select ic.category_pk1,qti_asi_data_pk1 as QPk1,q.data ");
            sql.Append(" ,crs.course_name as Course,crs.course_id as CourseId ");
            sql.Append(" ,case when q.bbmd_assessmenttype = 3 then 'S' else 'Q' end as Type");
            sql.Append(" from item_category ic ");
            sql.Append(" inner join qti_asi_data q on q.pk1 = ic.qti_asi_data_pk1 ");
            sql.Append(" inner join category c on c.pk1 = ic.category_pk1 ");
            sql.Append(" inner join course_main crs on crs.pk1 = c.crsmain_pk1 ");
            sql.Append(" where c.category = '" + description + "' ");
            sql.Append(" order by crs.course_name,crs.course_id,q.bbmd_assessmenttype,ic.category_pk1 ");
            RequesterAsync rr = new RequesterAsync(sql.ToString(), bbUrl, token, true, cc);
            DataTable dt = rr.execute();
            if (dt != null)
            {
                if (dt.DataSet != null)
                {
                    dt.DataSet.Tables.Remove(dt);
                }
                dt.Columns.Add("Kind", typeof(string));
                dt.Columns.Add("Question", typeof(string));

                foreach (DataRow dr in dt.Rows)
                {
                    XElement node = XElement.Parse(getStringFromData(dr["data"].ToString()));
                    XElement el1 = node.XPathSelectElement(BbQuery.QuestionText);
                    XElement el2 = node.XPathSelectElement(BbQuery.QuestionType);
                    dr["Question"] = Utilities.StripTags(el1.Value);
                    dr["Kind"] = el2.Value;
                    // dr["data"] = node.Value;


                }
                retvalue = dt;

            }
            return retvalue;
        }
        public static int updateCMSCatLibrary(DataTable cats, bool isOracle, string bbUrl, string token)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select c.pk1 from course_main c ");
            sql.Append(" where upper(c.course_name) = '" + BbCategoryLibraryCourseName.ToUpper() + "' ");
            RequesterAsync rr = new RequesterAsync(sql.ToString(), bbUrl, token, true, cc);
            DataTable dt = rr.execute();
            if (dt == null || dt.Rows.Count == 0)
            {
                return 0;
            }
            string eacCatCoursePk1 = dt.Rows[0]["pk1"].ToString();
            int additions = 0;
            foreach (DataRow ct in cats.Rows)
            {
                sql = new StringBuilder();
                sql.Append("select c.pk1 from category c ");
                sql.Append(" where crsmain_pk1 = " + eacCatCoursePk1);
                sql.Append(" and cat.category_type = 'C' ");
                sql.Append(" and upper(cat.category) <> '" + ct["category"].ToString().ToUpper() + "' ");
                rr = new RequesterAsync(sql.ToString(), bbUrl, token, true, cc);
                dt = rr.execute();
                if (dt == null || dt.Rows.Count == 0)
                {

                    sql = new StringBuilder("insert into category  ");
                    if (isOracle)
                    {
                        sql.Append(" (pk1,crsmain_pk1,category,category_type) ");
                        sql.Append("  select max(pk1) +1," + eacCatCoursePk1 + ",'" + ct["category"].ToString() + "','C' from category  ");
                    }
                    else
                    {
                        sql.Append(" (crsmain_pk1,category,category_type) ");
                        sql.Append(" values ");
                        sql.Append("  (" + eacCatCoursePk1 + ",'" + ct["category"].ToString() + "','C' )  ");
                    }
                    rr = new RequesterAsync(sql.ToString(), bbUrl, token, true, cc);
                    rr.execute();
                    Thread.Sleep(1000);
                    additions++;
                }
            }
            return additions;
        }

        /****  End Category/Outcome Stuff ******************************/

        /****  Gradebook Stuff ******************************/

        public static DataTable GetRecentCourses(string fd, string bbUrl, string token)
        {
            StringBuilder sql = new StringBuilder("select pk1 as PK1, course_name as Name, course_id as CourseId ");
            sql.Append(" from course_main ");
            sql.Append(" where dtcreated > " + fd);
            RequesterAsync rr = new RequesterAsync(sql.ToString(), bbUrl, token, true, cc);
            DataTable dt = rr.execute();
            return dt;
        }
        public static DataTable getGradebookDetail(string coursePk1, string bbUrl, string token)
        {
            StringBuilder sql = new StringBuilder("select gm.title,gm.position,gt.name as CatMain, gt.description as Cats,gt.user_defined_ind as UD, ");
            sql.Append(" g.average_score,g.manual_score,");
            sql.Append(" g.manual_grade,g.status,u.firstname,u.lastname ");
            sql.Append(" from gradebook_main gm ");
            sql.Append(" inner join course_users cu on cu.crsmain_pk1 = gm.crsmain_pk1 ");
            sql.Append(" inner join users u on u.pk1 = cu.users_pk1 ");
            sql.Append(" inner join gradebook_grade g on g.gradebook_main_pk1 = gm.pk1 and g.course_users_pk1 = cu.pk1 ");
            sql.Append(" inner join gradebook_type gt on gt.pk1 = gm.gradebook_type_pk1  ");
            sql.Append(" where gm.crsmain_pk1 = " + coursePk1);
            sql.Append(" order by gm.pk1, gm.position ");
            RequesterAsync rr = new RequesterAsync(sql.ToString(), bbUrl, token, true, cc);
            DataTable dt = rr.execute();
            return dt;
        }



        /****  End Gradebook Stuff ******************************/

        /****  Special course routines ******************************/

        public static DataTable GetEACSpecialCourses(string course_idFrag, string bbUrl, string token)
        {
            /*select c.pk1 as PK1, c.course_name as Name, c.course_id as CourseId  from course_main c  
inner join gateway_course_categories gcc on gcc.crsmain_pk1 = c.pk1  
inner join gateway_categories gc on gc.pk1 = gcc.gatewaycat_pk1
where UPPER(c.course_id) like '%NU-%'  

and gc.batch_uid = 'EACspc'*/

            StringBuilder sql = new StringBuilder("select c.pk1 as PK1, c.course_name as Name, c.course_id as CourseId ");
            sql.Append(" from course_main c ");
            sql.Append(" inner join gateway_course_categories gcc on gcc.crsmain_pk1 = c.pk1 ");
            sql.Append(" inner join gateway_categories gc on gc.pk1 = gcc.gatewaycat_pk1 ");
            sql.Append(" where UPPER(c.course_id) like '%" + course_idFrag.ToUpper() + "%' ");
            sql.Append(" and gc.batch_uid = '" + BbEACSpCrsBatch_uid + "' ");
            RequesterAsync rr = new RequesterAsync(sql.ToString(), bbUrl, token, true, cc);
            DataTable dt = rr.execute();
            return dt;
        }

        public static DataTable getPreceptors(string obdcConnection)
        {
            DataTable dt = new DataTable("Preceptors");
            using (OdbcDataAdapter da = new OdbcDataAdapter())
            {
                OdbcCommand users = new OdbcCommand("select * from users", new OdbcConnection(obdcConnection));
                da.SelectCommand = users;
                da.Fill(dt);
            }
            return dt;

        }

        /****  End Special course routines ******************************/




    }
    public class instrument
    {
        public string course_id;
        public string title;
        public instrument(string course_id, string title)
        {
            this.course_id = course_id;
            this.title = title;
        }
    }
}

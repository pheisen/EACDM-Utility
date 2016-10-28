using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using LINQtoXSDLib;
using QTIUtility;

namespace PeerReviewList
{
    public class PRUtility
    {
        public static DataTable students;
        public static void Modify(string BbUrl, string token, int qdpk1)
        {
            // get students - active
            StringBuilder sql = new StringBuilder();
            sql.Append("select u.firstname,u.lastname,u.user_id,cu.users_pk1 ");
            sql.Append("from qti_asi_data qd ");
            sql.Append(" inner join course_users cu on cu.crsmain_pk1 = qd.crsmain_pk1 ");
            sql.Append(" inner join users u on u.pk1 = cu.users_pk1 ");
            sql.Append(" where cu.role='S' and cu.available_ind = 'Y' and cu.row_status = 0 ");
            sql.Append(" and qd.pk1 = " + qdpk1.ToString());
            sql.Append(" order by u.lastname, u.firstname ");
            RequesterAsync rr = new RequesterAsync(sql.ToString(), BbUrl, token, true);
            students = rr.execute();
            multipleChoiceItem m =  fromExisting(BbUrl, token, qdpk1,students);
            return;
        }

        public static multipleChoiceItem fromExisting(string BbUrl, string token, int qdpk1,DataTable students)
        {
            bool doHex = false;
            DataTable theData;
            string theXml = null;
            int row = 0;
            multipleChoiceItem retValue = null;
            multipleChoiceItem mc = null;
            StringBuilder sql = new StringBuilder();
            sql.Append("select qd.data ");
            sql.Append("from qti_asi_data qd ");
            
            sql.Append(" where qd.pk1 = " + qdpk1.ToString());
            RequesterAsync rr = new RequesterAsync(sql.ToString(), BbUrl, token, true);
            theData = rr.execute();
            if (theData != null && !theData.TableName.Equals("error"))
            {
                string data = theData.Rows[0]["data"].ToString();
                if (!data.StartsWith("P"))
                {
                    doHex = true;
                }
                theXml = BbQuery.getStringFromData(data);
            }
            if (!String.IsNullOrEmpty(theXml))
            {
                theXml = theXml.Replace("<item ", "<multipleChoiceItem ").Replace("</item>","</multipleChoiceItem>");
                mc = multipleChoiceItem.Parse(theXml);
                
            }
            
            var names = mc.presentation.flow.flow.Where(t => t.@class.Equals("RESPONSE_BLOCK"));
            foreach(var n in names)
            {
                //n.response_lid.render_choice.flow_label.
                var labels = n.response_lid.render_choice.flow_label;
                foreach (var l in labels)
                {
                    string stuName = students.Rows[row]["lastname"].ToString() +", "+ students.Rows[row]["firstname"].ToString()+" ("+students.Rows[row]["user_id"].ToString()+")";
                    row++;
                    l.response_label.flow_mat.material.mat_extension.mat_formattedtext.TypedValue = stuName;
                    l.response_label.ident = QTIUtility.Utilities.Md5HashUtilityUTF8(stuName).ToLower();
                }
                multipleChoiceItem.presentationLocalType.flowLocalType.flowLocalType1.response_lidLocalType.render_choiceLocalType.flow_labelLocalType m = new multipleChoiceItem.presentationLocalType.flowLocalType.flowLocalType1.response_lidLocalType.render_choiceLocalType.flow_labelLocalType();
                if (row < students.Rows.Count)
                {
                   // multipleChoiceItem.presentationLocalType.flowLocalType.flowLocalType1.response_lidLocalType.render_choiceLocalType.flow_labelLocalType m = new multipleChoiceItem.presentationLocalType.flowLocalType.flowLocalType1.response_lidLocalType.render_choiceLocalType.flow_labelLocalType();
                    string stuName = students.Rows[row]["lastname"].ToString() + ", " + students.Rows[row]["firstname"].ToString() + " (" + students.Rows[row]["user_id"].ToString() + ")";
                    m.@class = "Block";
                    m.response_label = new multipleChoiceItem.presentationLocalType.flowLocalType.flowLocalType1.response_lidLocalType.render_choiceLocalType.flow_labelLocalType.response_labelLocalType();
                    m.response_label.ident = QTIUtility.Utilities.Md5HashUtilityUTF8(stuName).ToLower();
                    m.response_label.shuffle = "Yes";
                    m.response_label.rarea = "Ellipse";
                    m.response_label.rrange = "Exact";
                    m.response_label.flow_mat = new multipleChoiceItem.presentationLocalType.flowLocalType.flowLocalType1.response_lidLocalType.render_choiceLocalType.flow_labelLocalType.response_labelLocalType.flow_matLocalType();
                    m.response_label.flow_mat.@class = "FORMATTED_TEXT_BLOCK";
                    m.response_label.flow_mat.material = new multipleChoiceItem.presentationLocalType.flowLocalType.flowLocalType1.response_lidLocalType.render_choiceLocalType.flow_labelLocalType.response_labelLocalType.flow_matLocalType.materialLocalType();
                    m.response_label.flow_mat.material.mat_extension = new multipleChoiceItem.presentationLocalType.flowLocalType.flowLocalType1.response_lidLocalType.render_choiceLocalType.flow_labelLocalType.response_labelLocalType.flow_matLocalType.materialLocalType.mat_extensionLocalType();
                    m.response_label.flow_mat.material.mat_extension.mat_formattedtext = new multipleChoiceItem.presentationLocalType.flowLocalType.flowLocalType1.response_lidLocalType.render_choiceLocalType.flow_labelLocalType.response_labelLocalType.flow_matLocalType.materialLocalType.mat_extensionLocalType.mat_formattedtextLocalType();
                    m.response_label.flow_mat.material.mat_extension.mat_formattedtext.TypedValue = stuName;
                    //<mat_formattedtext type="HTML">Boxer, Barbara (barb)</mat_formattedtext>
                    m.response_label.flow_mat.material.mat_extension.mat_formattedtext.type = "HTML";
                    n.response_lid.render_choice.flow_label.Add(m);
                    row++;
                }
                while (row < students.Rows.Count)
                {
                    multipleChoiceItem.presentationLocalType.flowLocalType.flowLocalType1.response_lidLocalType.render_choiceLocalType.flow_labelLocalType mcc = (multipleChoiceItem.presentationLocalType.flowLocalType.flowLocalType1.response_lidLocalType.render_choiceLocalType.flow_labelLocalType) m.Clone();
                    string stuName = students.Rows[row]["lastname"].ToString() + ", " + students.Rows[row]["firstname"].ToString() + " (" + students.Rows[row]["user_id"].ToString() + ")";
                    mcc.response_label.ident = QTIUtility.Utilities.Md5HashUtilityUTF8(stuName).ToLower();
                    n.response_lid.render_choice.flow_label.Add(mcc);
                    row++;
                }
            }
            row = 0;
            var vcorrect = mc.resprocessing.respcondition.Where(t => t.title.Equals("correct")).FirstOrDefault().conditionvar.varequal;
            string thisName = students.Rows[row]["lastname"].ToString() + ", " + students.Rows[row]["firstname"].ToString() + " (" + students.Rows[row]["user_id"].ToString() + ")";
            vcorrect.TypedValue = QTIUtility.Utilities.Md5HashUtilityUTF8(thisName).ToLower();
            vcorrect.respident = "response";
            //<respcondition title="correct">
            var resps = mc.resprocessing.respcondition.Where(t => t.conditionvar != null && String.IsNullOrEmpty(t.title));

            /*<respcondition title="correct">
            <conditionvar>
              <varequal respident="response" case="No">0e1fffe32c27424094f9e85c12c55bfb</varequal>
            </conditionvar>
            <setvar variablename="SCORE" action="Set">SCORE.max</setvar>
            <displayfeedback linkrefid="correct" feedbacktype="Response"/>
          </respcondition>*/

            /*<respcondition>
      <conditionvar>
        <varequal respident="55394393588c4bb5927a67b4f0846048" case="No"/>
      </conditionvar>
      <setvar variablename="SCORE" action="Set">100</setvar>
      <displayfeedback linkrefid="55394393588c4bb5927a67b4f0846048" feedbacktype="Response"/>
    </respcondition>*/
            foreach (var r in resps)
            {
                if (r.conditionvar.varequal == null)
                {
                    continue;
                }
                string stuName = students.Rows[row]["lastname"].ToString() +", "+ students.Rows[row]["firstname"].ToString()+" ("+students.Rows[row]["user_id"].ToString()+")";
                r.conditionvar.varequal.respident = QTIUtility.Utilities.Md5HashUtilityUTF8(stuName).ToLower();
                r.displayfeedback.linkrefid = QTIUtility.Utilities.Md5HashUtilityUTF8(stuName).ToLower();
                row++;
                
            }
            while (row < students.Rows.Count)
            {
                string stuName = students.Rows[row]["lastname"].ToString() +", "+ students.Rows[row]["firstname"].ToString()+" ("+students.Rows[row]["user_id"].ToString()+")";
                multipleChoiceItem.resprocessingLocalType.respconditionLocalType res = new multipleChoiceItem.resprocessingLocalType.respconditionLocalType();
                res.conditionvar = new multipleChoiceItem.resprocessingLocalType.respconditionLocalType.conditionvarLocalType();
                res.conditionvar.varequal = new multipleChoiceItem.resprocessingLocalType.respconditionLocalType.conditionvarLocalType.varequalLocalType();
                res.conditionvar.varequal.respident = QTIUtility.Utilities.Md5HashUtilityUTF8(stuName).ToLower();
                res.conditionvar.varequal.@case = "No";
                res.setvar = new multipleChoiceItem.resprocessingLocalType.respconditionLocalType.setvarLocalType();
                res.setvar.variablename = "SCORE";
                res.setvar.action = "Set";
                res.setvar.TypedValue = "0.0";
                res.displayfeedback = new multipleChoiceItem.resprocessingLocalType.respconditionLocalType.displayfeedbackLocalType();
                res.displayfeedback.linkrefid = QTIUtility.Utilities.Md5HashUtilityUTF8(stuName).ToLower();
                res.displayfeedback.feedbacktype = "Response";
                mc.resprocessing.respcondition.Add(res);
                row++;
            }

            row = 0;

            /*should have 
              <itemfeedback ident="correct" view="All">
          <flow_mat class="Block">
            <flow_mat class="FORMATTED_TEXT_BLOCK">
              <material>
                <mat_extension>
                  <mat_formattedtext type="HTML"/>
                </mat_extension>
              </material>
            </flow_mat>
          </flow_mat>
        </itemfeedback>
        <itemfeedback ident="incorrect" view="All">
          <flow_mat class="Block">
            <flow_mat class="FORMATTED_TEXT_BLOCK">
              <material>
                <mat_extension>
                  <mat_formattedtext type="HTML"/>
                </mat_extension>
              </material>
            </flow_mat>
          </flow_mat>
        </itemfeedback>
             */
            var itf = mc.itemfeedback.Where(t => !t.ident.Contains("correct")); 
            foreach (var itp in itf)
            {
                string stuName = students.Rows[row]["lastname"].ToString() +", "+ students.Rows[row]["firstname"].ToString()+" ("+students.Rows[row]["user_id"].ToString()+")";
                itp.ident = QTIUtility.Utilities.Md5HashUtilityUTF8(stuName).ToLower();
                row++;
            }
            /*
             <itemfeedback ident="0e1fffe32c27424094f9e85c12c55bfb" view="All">
          <solution view="All" feedbackstyle="Complete">
            <solutionmaterial>
              <flow_mat class="Block">
                <flow_mat class="FORMATTED_TEXT_BLOCK">
                  <material>
                    <mat_extension>
                      <mat_formattedtext type="HTML"/>
                    </mat_extension>
                  </material>
                </flow_mat>
              </flow_mat>
            </solutionmaterial>
          </solution>
        </itemfeedback>
             * 
             * <itemfeedback ident="55394393588c4bb5927a67b4f0846048" view="All">
   <solution view="All" feedbackstyle="Complete">
     <solutionmaterial>
       <flow_mat class="Block">
         <flow_mat class="FORMATTED_TEXT_BLOCK">
           <material>
             <mat_extension>
               <mat_formattedtext type="HTML"/>
             </mat_extension>
           </material>
         </flow_mat>
       </flow_mat>
     </solutionmaterial>
   </solution>
 </itemfeedback>*/
            multipleChoiceItem.itemfeedbackLocalType it = null;
            if (row < students.Rows.Count)
            {
                it = new multipleChoiceItem.itemfeedbackLocalType();
                it.view = "All";
                string stuName = students.Rows[row]["lastname"].ToString() + ", " + students.Rows[row]["firstname"].ToString() + " (" + students.Rows[row]["user_id"].ToString() + ")";
                it.ident = QTIUtility.Utilities.Md5HashUtilityUTF8(stuName).ToLower();
                it.solution = new multipleChoiceItem.itemfeedbackLocalType.solutionLocalType();
                it.solution.view = "All";
                it.solution.feedbackstyle = "Complete";
                it.solution.solutionmaterial = new multipleChoiceItem.itemfeedbackLocalType.solutionLocalType.solutionmaterialLocalType();
                it.solution.solutionmaterial.flow_mat = new multipleChoiceItem.itemfeedbackLocalType.solutionLocalType.solutionmaterialLocalType.flow_matLocalType();
                it.solution.solutionmaterial.flow_mat.@class = "Block";
                it.solution.solutionmaterial.flow_mat.flow_mat = new multipleChoiceItem.itemfeedbackLocalType.solutionLocalType.solutionmaterialLocalType.flow_matLocalType.flow_matLocalType1();
                it.solution.solutionmaterial.flow_mat.flow_mat.@class = "FORMATTED_TEXT_BLOCK";
                it.solution.solutionmaterial.flow_mat.flow_mat.material = new multipleChoiceItem.itemfeedbackLocalType.solutionLocalType.solutionmaterialLocalType.flow_matLocalType.flow_matLocalType1.materialLocalType();
                it.solution.solutionmaterial.flow_mat.flow_mat.material.mat_extension = new multipleChoiceItem.itemfeedbackLocalType.solutionLocalType.solutionmaterialLocalType.flow_matLocalType.flow_matLocalType1.materialLocalType.mat_extensionLocalType();
                it.solution.solutionmaterial.flow_mat.flow_mat.material.mat_extension.mat_formattedtext = new multipleChoiceItem.itemfeedbackLocalType.solutionLocalType.solutionmaterialLocalType.flow_matLocalType.flow_matLocalType1.materialLocalType.mat_extensionLocalType.mat_formattedtextLocalType();
                it.solution.solutionmaterial.flow_mat.flow_mat.material.mat_extension.mat_formattedtext.type = "HTML";
                mc.itemfeedback.Add(it);
                row++;
            }
            while (row < students.Rows.Count)
            {
                multipleChoiceItem.itemfeedbackLocalType itc =(multipleChoiceItem.itemfeedbackLocalType) it.Clone();
                string stuName = students.Rows[row]["lastname"].ToString() + ", " + students.Rows[row]["firstname"].ToString() + " (" + students.Rows[row]["user_id"].ToString() + ")";
                itc.ident = QTIUtility.Utilities.Md5HashUtilityUTF8(stuName).ToLower();
                mc.itemfeedback.Add(itc);
                row++;
            }

            StringBuilder xm = new StringBuilder();
            XmlWriter xr = XmlWriter.Create(xm);
            mc.Save(xr);
            xr.Flush();
            xr.Close();
            string ix = xm.ToString();
            ix = ix.Replace("<multipleChoiceItem ", "<item ").Replace("</multipleChoiceItem>", "</item>");
            ix = ix.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "<?xml version=\"1.0\" encoding=\"UTF-8\"?>");

            //xm.Length = 0;
            //xm.Append(ix);
            string xmf = String.Empty;
            byte[] b = UTF8Encoding.UTF8.GetBytes(ix);
            if (doHex)
            {
                xmf = BitConverter.ToString(b).Replace("-", string.Empty);
            }
            else
            {
                xmf = Convert.ToBase64String(b);
            }

            sql.Length = 0;
            sql.Append("update qti_asi_data set ");
            sql.Append("data =cast('" + ix + "' AS varbinary(max))");

            sql.Append(" where pk1 = " + qdpk1.ToString());
            string theSql = sql.ToString();
            rr = new RequesterAsync(theSql, BbUrl, token, true);
            theData = rr.execute();
            retValue = mc;
            return retValue;
        }


    }
    
    
}

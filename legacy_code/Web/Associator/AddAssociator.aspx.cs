using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using Base.Page;
using Associator.Perform;
using Base.Biz;
using Base.DB;
using Base;
using BaseInfo.User;
using Associator.Associator;

public partial class Associator_AddAssociator : BasePage
{
    public string baseInfo;
    public string url;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BandCmd();
            GetInterest();
            GetActivity();
            GetFavor();
            if (Request.QueryString["MemberId"] != null)
            {
                BaseBO baseBO = new BaseBO();
                baseBO.WhereClause = "MembId = '" + Request.QueryString["MemberId"].ToString() + "'";
                Resultset rs = baseBO.Query(new LCust());
                GetInterestByID(Request.QueryString["MemberId"].ToString());
                GetActivityByID(Request.QueryString["MemberId"].ToString());
                GetFavorByID(Request.QueryString["MemberId"].ToString());
                SetContorlsValues(rs);
                ViewState["flag"] = 1; //修改
                ViewState["MemberId"] = Request.QueryString["MemberId"].ToString();

                this.labTitle.Text = "会员信息修改";
                baseInfo = "会员信息修改";
                url = "Associator/AddAssociator.aspx?MemberId=" + Request.QueryString["MemberId"].ToString() + "&modify=" + 1;
            }
            else
            {
                txtDateJoint.Text = DateTime.Now.ToShortDateString();
                this.labTitle.Text = "会员信息录入";
                baseInfo = "会员信息录入";
                //imgPeople.ImageUrl = "Default.aspx?Pic=Pic";
                ViewState["flag"] = 0; //添加
                url = "Associator/AddAssociator.aspx";
            }
            //txtLCustId.Text = BaseApp.GetMembID().ToString();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtMobilPhone.Text.Trim() != "" && txtLCustNm.Text.Trim() != "" && txtAddr1.Text.Trim() != "")
        {
            LCust lCust = new LCust();
            Activity activity = new Activity();
            Favor favor = new Favor();
            ConsumeInterest cInterest = new ConsumeInterest();
            int AcID;
            int FaID;
            int CoID;
            BaseBO bo = new BaseBO();
            DataSet ds1 = bo.QueryDataSet("select max(ActivityID) from Activity");
            if (ds1.Tables[0].Rows[0][0] != DBNull.Value)
            {
                AcID = int.Parse(ds1.Tables[0].Rows[0][0].ToString()) + 1;
            }
            else
            {
                AcID = 101;
            }
            DataSet ds2 = bo.QueryDataSet("select max(FavorID) from Favor");
            if (ds2.Tables[0].Rows[0][0] != DBNull.Value)
            {
                FaID = int.Parse(ds2.Tables[0].Rows[0][0].ToString()) + 1;
            }
            else
            {
                FaID = 101;
            }
            DataSet ds3 = bo.QueryDataSet("select max(ConsumeID) from ConsumeInterest");
            if (ds3.Tables[0].Rows[0][0] != DBNull.Value)
            {
                CoID = int.Parse(ds3.Tables[0].Rows[0][0].ToString()) + 1;
            }
            else
            {
                CoID = 101;
            }


            BaseTrans baseTrans = new BaseTrans();
            baseTrans.BeginTrans();
            if (ViewState["flag"].ToString() == "0")
            {
                lCust.MembId = BaseApp.GetMembID();
                lCust.MembCode = lCust.MembId.ToString();
                lCust = GetControlsVales(lCust);
                ViewState["MemberId"] = lCust.MembId;
                if (baseTrans.Insert(lCust) == 1)
                {
                    InsertInfo(baseTrans, activity, favor, cInterest, AcID, FaID, CoID);
                    baseTrans.Commit();
                    string msg = "会员信息添加成功,会员号: " + lCust.MembId.ToString();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + msg + "'", true);
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                    ClearPage();
                }
                else
                {
                    baseTrans.Rollback();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                }
            }
            else if (ViewState["flag"].ToString() == "1")
            {
                baseTrans.WhereClause = "MembId = '" + ViewState["MemberId"] + "'";
                lCust = GetControlsVales(lCust);
                if (baseTrans.Update(lCust) == 1)
                {
                    if (baseTrans.Delete(activity) < 0)
                    {
                        baseTrans.Rollback();
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                        return;
                    }
                    if (baseTrans.Delete(favor) < 0)
                    {
                        baseTrans.Rollback();
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                        return;
                    }
                    if (baseTrans.Delete(cInterest) < 0)
                    {
                        baseTrans.Rollback();
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                        return;
                    }
                    InsertInfo(baseTrans, activity, favor, cInterest, AcID, FaID, CoID);
                    baseTrans.Commit();
                    Response.Redirect("../Associator/QueryAssociator.aspx?flag=0");
                    //ClearPage();
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
                }
                else
                {
                    baseTrans.Rollback();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                }
            }
        }
        else
        {
            if (txtMobilPhone.Text.Trim() == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Associator_AssociatorMobileTel") + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") + "'", true);
                return;
            }
            if (txtLCustNm.Text.Trim() == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '称呼/姓名" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") + "'", true);
                return;
            }
            if (txtAddr1.Text.Trim() == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Associator_AssociatorAddress") + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") + "'", true);
                return;
            }
        }
    }

    private LCust GetControlsVales(LCust lCust)
    {
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        
        lCust.MemberName = txtLCustNm.Text.Trim();
        lCust.Salutation = cmbSalutation.SelectedValue;
        lCust.LOtherId = txtLOtherId.Text.Trim();
        lCust.DateJoint = txtDateJoint.Text == "" ? DateTime.Now : Convert.ToDateTime(txtDateJoint.Text);
        lCust.Addr1 = txtAddr1.Text.Trim();
        lCust.Addr2 = txtAddr2.Text.Trim();
        lCust.Addr3 = txtAddr3.Text.Trim();
        lCust.PostalCode1 = txtPostalCode1.Text.Trim();
        lCust.PostalCode2 = txtPostalCode2.Text.Trim();
        lCust.PostalCode3 = txtPostalCode3.Text.Trim();
        lCust.OffPhone = txtOffPhone.Text.Trim();
        lCust.HomePhone = txtHomePhone.Text.Trim();
        lCust.MobilPhone = txtMobilPhone.Text.Trim();
        lCust.Email = txtEmail.Text.Trim();
        //lCust.Dob = txtDob.Text == "" ? DateTime.Now : Convert.ToDateTime(txtDob.Text);
        lCust.Dob = Convert.ToDateTime("2000-01-01".Substring(0, "2000-01-01".IndexOf("-")) + "-" + cmbBM.SelectedValue.ToString() + "-" + cmbBD.SelectedValue.ToString());
        lCust.SexNm = cmbSexNm.SelectedValue.ToString();
        lCust.RaceNm = cmbRaceNm.SelectedValue.ToString();
        lCust.NatNm = cmbNatNm.SelectedValue.ToString();
        lCust.MStatusNm = cmbMStatusNm.SelectedValue.ToString();
        lCust.MAnnDate = cmbMAnnDateM.SelectedValue.ToString() + "-" + cmbMAnnDateD.SelectedValue.ToString();
        lCust.EduLevelNm = cmbEduLevelNm.SelectedValue.ToString();
        lCust.DistanceId = cmbDistanceId.SelectedValue.ToString();
        lCust.IncomeId = cmbIncomeId.SelectedValue.ToString();
        lCust.BizNm = cmbBizNm.SelectedValue.ToString();
        lCust.JobTitleNm = cmbJobTitleNm.SelectedValue.ToString();
        lCust.ComefromNM = drpOrigin.SelectedValue;
        //lCust.RecreationNm1 = cmbRecreationNm1.SelectedValue.ToString();
        //lCust.PreferMerNm1 = cmbPreferMerNm1.SelectedValue.ToString();
        //lCust.PreferGiftNm1 = cmbPreferGiftNm1.SelectedValue.ToString();
        lCust.Remarks = txtRemarks.Text.Trim();
        lCust.AgeID = Convert.ToInt32(cmbAge.SelectedValue);
        // 从输入文件中创建一个 byte[]
        //int len = FileUpload1.PostedFile.ContentLength;
        //byte[] pic = new byte[0];
        //FileUpload1.PostedFile.InputStream.Read(pic, 0, len);
        //lCust.MemberPic = pic;

        lCust.CreateUserID = sessionUser.UserID;
        lCust.ModifyUserID = sessionUser.UserID;
        lCust.OprDeptID = sessionUser.DeptID;
        lCust.OprRoleID = sessionUser.RoleID;
        return lCust;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {

    }
    public void OnUpload()
    {
        //// 从输入文件中创建一个 byte[]
        //int len = FileUpload1.PostedFile.ContentLength;
        //byte[] pic = new byte[len];
        //FileUpload1.PostedFile.InputStream.Read(pic, 0, len);
        //// 插入图片和说明到数据库中
        //SqlConnection connection = new
        //    SqlConnection(@"server=Flag;database=Ymi_net;uid=sa;pwd=sa");
        //try
        //{
        //    connection.Open();
        //    SqlCommand cmd = new SqlCommand("insert into Image "
        //      + "(Picture) values (@pic)", connection);
        //    cmd.Parameters.Add("@pic", pic);
        //    cmd.ExecuteNonQuery();
        //}
        //finally
        //{
        //    connection.Close();
        //}
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        //OnUpload();
    }


    private void BandCmd()
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();


        /*性别*/
        //cmbSexNm.Items.Add(new ListItem(""));
        cmbSexNm.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "TpUse_lblSexMan")));
        cmbSexNm.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "TpUse_lblSexWoman")));

        //称呼
        cmbSalutation.Items.Add("先生");
        cmbSalutation.Items.Add("女士");

        /*国籍*/
        baseBO.OrderBy = "EntryAt";
        rs = baseBO.Query(new Nationality());
        //cmbNatNm.Items.Add(new ListItem(""));
        foreach (Nationality nationality in rs)
        {
            cmbNatNm.Items.Add(new ListItem(nationality.NatNm.Trim()));
        }

        /*民族*/
        rs = baseBO.Query(new Race());
        //cmbRaceNm.Items.Add(new ListItem(""));
        foreach (Race race in rs)
        {
            cmbRaceNm.Items.Add(new ListItem(race.RaceNm.Trim()));
        }

        /*婚姻状况*/
        rs = baseBO.Query(new Mstatus());
        //cmbMStatusNm.Items.Add(new ListItem(""));
        foreach (Mstatus mstatus in rs)
        {
            cmbMStatusNm.Items.Add(new ListItem(mstatus.MstatusNm.Trim()));
        }

        /*结婚纪念日*/
        //cmbMAnnDateM.Items.Add(new ListItem(""));
        for (int i = 1; i <= 12; i++)
        {
            cmbMAnnDateM.Items.Add(new ListItem(i.ToString().Trim()));
            cmbBM.Items.Add(new ListItem(i.ToString().Trim()));
        }

        //cmbMAnnDateD.Items.Add(new ListItem(""));
        for (int i = 1; i <= 31; i++)
        {
            cmbMAnnDateD.Items.Add(new ListItem(i.ToString().Trim()));
            cmbBD.Items.Add(new ListItem(i.ToString().Trim()));
        }

        /*教育水平*/
        rs = baseBO.Query(new EduLevel());
        //cmbEduLevelNm.Items.Add(new ListItem(""));
        foreach (EduLevel eduLevel in rs)
        {
            cmbEduLevelNm.Items.Add(new ListItem(eduLevel.EduLevelNm.Trim().Trim()));
        }

        /*距离范围*/
        rs = baseBO.Query(new Distance());
        //cmbDistanceId.Items.Add(new ListItem(""));
        foreach (Distance distance in rs)
        {
            if (distance.DistanceTo == 0)
            {
                cmbDistanceId.Items.Add(new ListItem(distance.DistanceFr + "以上", distance.DistanceId.ToString()));
            }
            else if (distance.DistanceTo == 1)
            {
                cmbDistanceId.Items.Add(new ListItem("其它", distance.DistanceId.ToString()));
            }
            else
            {
                cmbDistanceId.Items.Add(new ListItem(distance.DistanceFr + "-" + distance.DistanceTo, distance.DistanceId.ToString()));
            }
        }

        /*收入水平*/
        rs = baseBO.Query(new Income());
        foreach (Income income in rs)
        {
            if (income.IncomeUpper == 0)
            {
                cmbIncomeId.Items.Add(new ListItem(income.IncomeLower + "以上", income.IncomeId.ToString()));
            }
            else if (income.IncomeUpper == 1)
            {
                cmbIncomeId.Items.Add(new ListItem("其它", income.IncomeId.ToString()));
            }
            else
            {
                cmbIncomeId.Items.Add(new ListItem(income.IncomeLower + "-" + income.IncomeUpper, income.IncomeId.ToString()));
            }
        }

        /*职业*/
        rs = baseBO.Query(new Biz());
        //cmbBizNm.Items.Add(new ListItem(""));
        foreach (Biz biz in rs)
        {
            cmbBizNm.Items.Add(new ListItem(biz.BizNm.Trim()));
        }

        /*职位*/
        rs = baseBO.Query(new JobTitle());
        //cmbJobTitleNm.Items.Add(new ListItem(""));
        foreach (JobTitle jobTitle in rs)
        {
            cmbJobTitleNm.Items.Add(new ListItem(jobTitle.JobTitleNm.Trim()));
        }

        /*兴趣爱好*/
        rs = baseBO.Query(new Recreation());
        //cmbRecreationNm1.Items.Add(new ListItem(""));
        foreach (Recreation recreation in rs)
        {
            cmbRecreationNm1.Items.Add(new ListItem(recreation.RecreationNm.Trim()));
        }

        /*喜好赠品*/
        rs = baseBO.Query(new PreferGift());
        //cmbPreferMerNm1.Items.Add(new ListItem(""));
        foreach (PreferGift preferGift in rs)
        {
            cmbPreferMerNm1.Items.Add(new ListItem(preferGift.PreferGiftNm.Trim()));
        }

        /*喜好商品*/
        rs = baseBO.Query(new PreferMer());
        //cmbPreferGiftNm1.Items.Add(new ListItem(""));
        foreach (PreferMer preferMer in rs)
        {
            cmbPreferGiftNm1.Items.Add(new ListItem(preferMer.PreferMerNm.Trim()));
        }

        //年龄段

        DataSet ds = AgePO.GetAge();
        int count = ds.Tables[0].Rows.Count;
        for (int i = 0; i < count; i++)
        {
            cmbAge.Items.Add(new ListItem(ds.Tables[0].Rows[i]["Agestr"].ToString(),ds.Tables[0].Rows[i]["AgeID"].ToString()));
        }

        //会员来源
        drpOrigin.Items.Add("普通入会");
        drpOrigin.Items.Add("公司嘉宾");
        drpOrigin.Items.Add("内部员工");
        drpOrigin.Items.Add("资源互换");
        drpOrigin.Items.Add("联名卡");
        drpOrigin.Items.Add("四海一家");
        drpOrigin.Items.Add("其它");
        

    }

    private void ClearPage()
    {
        txtLCustId.Text = "";
        txtLCustNm.Text = "";
        cmbSalutation.SelectedIndex = 0;
        txtLOtherId.Text = "";
        txtDateJoint.Text = "";
        txtAddr1.Text = "";
        txtAddr2.Text = "";
        txtAddr3.Text = "";
        txtPostalCode1.Text = "";
        txtPostalCode2.Text = "";
        txtPostalCode3.Text = "";
        txtOffPhone.Text = "";
        txtHomePhone.Text = "";
        txtMobilPhone.Text = "";
        txtEmail.Text = "";
        txtDob.Text = "";
        cmbSexNm.SelectedIndex = 0;
        cmbRaceNm.SelectedIndex = 0;
        cmbNatNm.SelectedIndex = 0;
        cmbMStatusNm.SelectedIndex = 0;
        cmbMAnnDateM.SelectedIndex = 0;
        cmbMAnnDateD.SelectedIndex = 0;
        cmbBD.SelectedIndex = 0;
        cmbBM.SelectedIndex = 0;
        cmbEduLevelNm.SelectedIndex = 0;
        cmbDistanceId.SelectedIndex = 0;
        cmbIncomeId.SelectedIndex = 0;
        cmbBizNm.SelectedIndex = 0;
        cmbJobTitleNm.SelectedIndex = 0;
        cmbAge.SelectedIndex = 0;
        drpOrigin.SelectedIndex = 0;
        //cmbRecreationNm1.SelectedIndex = 0; ;
        //cmbPreferMerNm1.SelectedIndex = 0;
        //cmbPreferGiftNm1.SelectedIndex = 0;
        txtRemarks.Text = "";
    }

    private void SetContorlsValues(Resultset rs)
    {
        if (rs.Count == 1)
        {
            LCust lCust = rs.Dequeue() as LCust;
            txtLCustId.Text = lCust.MembCode.Trim();
            txtLCustNm.Text = lCust.MemberName.Trim();
            cmbSalutation.SelectedValue = lCust.Salutation.Trim();
            txtLOtherId.Text = lCust.LOtherId.Trim();
            txtDateJoint.Text = lCust.DateJoint.ToShortDateString().Trim();
            txtAddr1.Text = lCust.Addr1.Trim();
            txtAddr2.Text = lCust.Addr2.Trim();
            txtAddr3.Text = lCust.Addr3.Trim();
            txtPostalCode1.Text = lCust.PostalCode1.Trim();
            txtPostalCode2.Text = lCust.PostalCode2.Trim();
            txtPostalCode3.Text = lCust.PostalCode3.Trim();
            txtOffPhone.Text = lCust.OffPhone.Trim();
            txtHomePhone.Text = lCust.HomePhone.Trim();
            txtMobilPhone.Text = lCust.MobilPhone.Trim();
            txtEmail.Text = lCust.Email.Trim();
            //txtDob.Text = lCust.Dob.ToShortDateString().Trim();
            cmbSexNm.SelectedValue = lCust.SexNm.Trim().Trim();
            cmbRaceNm.SelectedValue = lCust.RaceNm.Trim().Trim();
            cmbNatNm.SelectedValue = lCust.NatNm.Trim();
            cmbMStatusNm.SelectedValue = lCust.MStatusNm.Trim();
            cmbMAnnDateM.SelectedValue = lCust.MAnnDate.Substring(0,lCust.MAnnDate.IndexOf("-"));
            cmbMAnnDateD.SelectedValue = lCust.MAnnDate.Trim().Substring(lCust.MAnnDate.LastIndexOf("-") + 1);
            cmbBM.SelectedValue =lCust.Dob.ToShortDateString().Substring(lCust.Dob.ToShortDateString().IndexOf("-") + 1, lCust.Dob.ToShortDateString().LastIndexOf("-") - lCust.Dob.ToShortDateString().IndexOf("-") - 1);
            cmbBD.SelectedValue = lCust.Dob.ToShortDateString().Substring(lCust.Dob.ToShortDateString().LastIndexOf("-") + 1);
            cmbEduLevelNm.SelectedValue = lCust.EduLevelNm.Trim();
            cmbDistanceId.SelectedValue = lCust.DistanceId;
            cmbIncomeId.SelectedValue = lCust.IncomeId;
            cmbBizNm.SelectedValue = lCust.BizNm.Trim();
            cmbJobTitleNm.SelectedValue = lCust.JobTitleNm.Trim();
            drpOrigin.SelectedValue = lCust.ComefromNM.Trim();
            //cmbRecreationNm1.SelectedValue = lCust.RecreationNm1.Trim();
            //cmbPreferMerNm1.SelectedValue = lCust.PreferMerNm1.Trim();
            //cmbPreferGiftNm1.SelectedValue = lCust.PreferGiftNm1.Trim();
            txtRemarks.Text = lCust.Remarks.Trim();
            cmbAge.SelectedValue = lCust.AgeID.ToString();
        }
    }
    protected void btnQuit_Click(object sender, EventArgs e)
    {
        ClearPage();
    }

    private void GetInterest()
    {
        DataSet ds = ConsumeInterestPO.GetInterestItem();
        CBoxInterest.DataSource = ds;
        CBoxInterest.DataBind();
    }

    private void GetFavor()
    {
        DataSet ds = FacorPO.GetFavorItem();
        CBoxFavor.DataSource = ds;
        CBoxFavor.DataBind();
    }

    private void GetActivity()
    {
        DataSet ds = ActivityPO.GetActivityItem();
        CBoxActivity.DataSource = ds;
        CBoxActivity.DataBind();
    }

    private void GetInterestByID(string MembID)
    {
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "MembID = '" + MembID + "'";
        Resultset rs = baseBO.Query(new ConsumeInterest());
        int count = CBoxInterest.Items.Count;
        for(int i = 0; i < count; i++)
        {
            foreach (ConsumeInterest conInt in rs)
            {
                if (Convert.ToInt32(CBoxInterest.Items[i].Value) == conInt.IItemID)
                {
                    CBoxInterest.Items[i].Selected = true;
                }
            }
        }
    }

    private void GetFavorByID(string MembID)
    {
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "MembID = '" + MembID + "'";
        Resultset rs = baseBO.Query(new Favor());
        int count = CBoxFavor.Items.Count;
        for (int i = 0; i < count; i++)
        {
            foreach (Favor favor in rs)
            {
                if (Convert.ToInt32(CBoxFavor.Items[i].Value) == favor.FItemID)
                {
                    CBoxFavor.Items[i].Selected = true;
                }
            }
        }
    }

    private void GetActivityByID(string MembID)
    {
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "MembID = '" + MembID + "'";
        Resultset rs = baseBO.Query(new Activity());
        int count = CBoxActivity.Items.Count;
        for (int i = 0; i < count; i++)
        {
            foreach (Activity activity in rs)
            {
                if (Convert.ToInt32(CBoxActivity.Items[i].Value) == activity.AItemID)
                {
                    CBoxActivity.Items[i].Selected = true;
                }
            }
        }
    }

    private void InsertInfo(BaseTrans baseTrans, Activity activity, Favor favor, ConsumeInterest cInterest, int AcID, int FaID, int CoID)
    {
        for (int i = 0; i < CBoxActivity.Items.Count; i++)
        {
            if (CBoxActivity.Items[i].Selected)
            {
                activity.ActivityID =AcID+i;
                activity.MembID = Convert.ToInt32(ViewState["MemberId"]);
                activity.AItemID = Convert.ToInt32(CBoxActivity.Items[i].Value);

                if (baseTrans.Insert(activity) != 1)
                {
                    baseTrans.Rollback();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                    return;
                }
            }
        }
        for (int i = 0; i < CBoxFavor.Items.Count; i++)
        {
            if (CBoxFavor.Items[i].Selected)
            {
                favor.FavorID = FaID+i;
                favor.MembID = Convert.ToInt32(ViewState["MemberId"]);
                favor.FItemID = Convert.ToInt32(CBoxFavor.Items[i].Value);

                if (baseTrans.Insert(favor) != 1)
                {
                    baseTrans.Rollback();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                    return;
                }
            }
        }
        for (int i = 0; i < CBoxInterest.Items.Count; i++)
        {
            if (CBoxInterest.Items[i].Selected)
            {
                cInterest.ConsumeID = CoID+i;
                cInterest.MembID = Convert.ToInt32(ViewState["MemberId"]);
                cInterest.IItemID = Convert.ToInt32(CBoxInterest.Items[i].Value);

                if (baseTrans.Insert(cInterest) != 1)
                {
                    baseTrans.Rollback();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                    return;
                }
            }
        }
    }
}

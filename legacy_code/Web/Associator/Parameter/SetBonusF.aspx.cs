using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Base.Biz;
using Base.DB;
using Base;
using Associator;
using Associator.Perform;
using Associator.Associator;
using Base.Page;
using BaseInfo.User;


public partial class Associator_Parameter_SetBonusF : BasePage
{
    private BonusF bonusF;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            ViewState["Flag"] = 0;//插入
            BaseBO baseBO = new BaseBO();
            baseBO.WhereClause = "DeleteTime IS NULL";
            Resultset rs = baseBO.Query(new BonusF());
            if (rs.Count == 1)
            {
                ViewState["Flag"] = 1;//修改
                bonusF = rs.Dequeue() as BonusF;
                ViewState["bonusFID"] = bonusF.BonusFID;
                txtAmt.Text = bonusF.AdjFactor.ToString();
                if (bonusF.AdjShop == "Y")
                {
                    CBoxListType.Items[0].Selected = true;
                }
                else
                {
                    CBoxListType.Items[0].Selected = false;
                }
                if (bonusF.AdjCard == "Y")
                {
                    CBoxListType.Items[1].Selected = true;
                }
                else
                {
                    CBoxListType.Items[1].Selected = false;
                }
                if (bonusF.ProTime == "Y")
                {
                    CBoxListType.Items[2].Selected = true;
                }
                else
                {
                    CBoxListType.Items[2].Selected = false;
                }
            }
            if (CBoxListType.Items[2].Selected == true)
            {
                txtStartDate.Enabled = true;
                txtEndDate.Enabled = true;
                RbtnListDOrW.Enabled = true;
                txtStartTime.Enabled = true;
                txtEndTime.Enabled = true;
                txtProFactor.Enabled = true;
                if (bonusF.Frequency == "W")
                {
                    CBoxListWeek.Enabled = true;
                }
                CBoxListWeek.Items[0].Selected = bonusF.Mon == "Y" ? true : false;
                CBoxListWeek.Items[1].Selected = bonusF.Tue == "Y" ? true : false;
                CBoxListWeek.Items[2].Selected = bonusF.Wed == "Y" ? true : false;
                CBoxListWeek.Items[3].Selected = bonusF.Thu == "Y" ? true : false;
                CBoxListWeek.Items[4].Selected = bonusF.Fri == "Y" ? true : false;
                CBoxListWeek.Items[5].Selected = bonusF.Sat == "Y" ? true : false;
                CBoxListWeek.Items[6].Selected = bonusF.Sun == "Y" ? true : false;
                txtStartDate.Text = bonusF.StartDate.ToShortDateString();
                txtEndDate.Text = bonusF.EndDate.ToShortDateString();
                txtStartTime.Text = bonusF.StartTime.ToString().Substring(9);
                txtEndTime.Text = bonusF.EndTime.ToString().Substring(9);
                if (bonusF.Frequency == "D")
                {
                    RbtnListDOrW.Items[0].Selected = true;
                }
                else
                {
                    RbtnListDOrW.Items[1].Selected = true;
                }
                txtProFactor.Text = bonusF.ProFactor.ToString();
            }
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        if (ViewState["Flag"].ToString() == "0")
        {
            GetValues();
            if (txtStartDate.Text != "" && txtStartTime.Text != "")
            {
                bonusF.StartTime = Convert.ToDateTime(txtStartDate.Text + " " + txtStartTime.Text);
            }
            if (txtEndDate.Text != "" && txtEndTime.Text != "")
            {
                bonusF.EndTime = Convert.ToDateTime(txtEndDate.Text + " " + txtEndTime.Text);
            }
            if (txtProFactor.Text != "")
            {
                bonusF.ProFactor = Convert.ToDecimal(txtProFactor.Text);
            }
            bonusF.BonusFID = BaseApp.GetBonusFID();
            int i = baseBO.Insert(bonusF);
            if (i == 1)
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '保存成功！'", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '保存失败！'", true);
            }
        }
        else if (ViewState["Flag"].ToString() == "1")
        {
            GetValues();
            if (txtStartDate.Text != "" && txtStartTime.Text != "")
            {
                bonusF.StartTime = Convert.ToDateTime(txtStartDate.Text + " " + txtStartTime.Text);
            }
            if (txtEndDate.Text != "" && txtEndTime.Text != "")
            {
                bonusF.EndTime = Convert.ToDateTime(txtEndDate.Text + " " + txtEndTime.Text);
            }
            if (txtProFactor.Text != "")
            {
                bonusF.ProFactor = Convert.ToDecimal(txtProFactor.Text);
            }
            baseBO.WhereClause = "BonusFID = " + Convert.ToInt32(ViewState["bonusFID"]);
            int i = baseBO.Update(bonusF);
            if (i == 1)
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '修改成功！'", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '修改失败！'", true);
            }
        }
    }

    private void GetValues()
    {
        bonusF = new BonusF();
        
        bonusF.BonusFDesc = "VIP";
        bonusF.DIndex = 1;
        bonusF.AdjFactor = Convert.ToDecimal(txtAmt.Text);
        if (CBoxListType.Items[0].Selected == true)
        {
            bonusF.AdjShop = "Y";
        }
        else
        {
            bonusF.AdjShop = "N";
        }
        if (CBoxListType.Items[1].Selected == true)
        {
            bonusF.AdjCard = "Y";
        }
        else
        {
            bonusF.AdjCard = "N";
        }
        if (CBoxListType.Items[2].Selected == true)
        {
            bonusF.ProTime = "Y";
        }
        else
        {
            bonusF.ProTime = "N";
        }
        if (txtStartDate.Text != "")
        {
            bonusF.StartDate = Convert.ToDateTime(txtStartDate.Text);
        }
        if (txtEndDate.Text != "")
        {
            bonusF.EndDate = Convert.ToDateTime(txtEndDate.Text);
        }
        if (RbtnListDOrW.SelectedValue == "1")
        {
            bonusF.Frequency = "D";
        }
        else
        {
            bonusF.Frequency = "W";
        }
        bonusF.Mon = CBoxListWeek.Items[0].Selected == true ? "Y" : "N";
        bonusF.Tue = CBoxListWeek.Items[1].Selected == true ? "Y" : "N";
        bonusF.Wed = CBoxListWeek.Items[2].Selected == true ? "Y" : "N";
        bonusF.Thu = CBoxListWeek.Items[3].Selected == true ? "Y" : "N";
        bonusF.Fri = CBoxListWeek.Items[4].Selected == true ? "Y" : "N";
        bonusF.Sat = CBoxListWeek.Items[5].Selected == true ? "Y" : "N";
        bonusF.Sun = CBoxListWeek.Items[6].Selected == true ? "Y" : "N";
        
        bonusF.EntryAt = DateTime.Now;

        SessionUser sionUser = (SessionUser)Session["UserAccessInfo"];
        bonusF.EntryBy = sionUser.UserID.ToString();


    }

    protected void RbtnListDOrW_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RbtnListDOrW.SelectedValue == "2")
        {
            CBoxListWeek.Enabled = true;
        }
        else
        {
            CBoxListWeek.Enabled = false;
        }
    }
    protected void CBoxListType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (CBoxListType.Items[2].Selected == true)
        {
            txtStartDate.Enabled = true;
            txtEndDate.Enabled = true;
            RbtnListDOrW.Enabled = true;
            txtStartTime.Enabled = true;
            txtEndTime.Enabled = true;
            txtProFactor.Enabled = true;
            if (RbtnListDOrW.SelectedValue == "2")
            {
                CBoxListWeek.Enabled = true;
            }
        }
        else
        {
            //txtStartDate.Enabled = false;
            //txtEndDate.Enabled = false;
            RbtnListDOrW.Enabled = false;
            txtStartTime.Enabled = false;
            txtEndTime.Enabled = false;
            txtProFactor.Enabled = false;
            if (RbtnListDOrW.SelectedValue == "1")
            {
                CBoxListWeek.Enabled = false;
            }
        }
    }
}

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
using Base.Page;
using Base;
using RentableArea;

public partial class RentableArea_AreaSize_RentLevel : System.Web.UI.Page
{
    DataTable dt = new DataTable();
    PagedDataSource pds = new PagedDataSource();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            int[] status = RentLevel.GetRentLevelStatus();
            for (int i = 0; i < status.Length; i++)
            {
                this.cmbAreaSizeStatus.Items.Add(new ListItem(RentLevel.GetRentLevelStatusDesc(status[i]), status[i].ToString()));
            }

            ViewState["currentCount"] = 1;
            page();
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        RentLevel areaSize = new RentLevel();
        areaSize.RentLevelID = RentableAreaApp.GetID("RentLevel", "RentLevelID");
        areaSize.RentLevelCode = txtAreaSizeCode.Text.Trim();
        areaSize.RentLevelDesc = txtAreaSizeName.Text.Trim();
        areaSize.RentLevelStatus = Convert.ToInt32(cmbAreaSizeStatus.SelectedValue.ToString());
        areaSize.Note = this.txtNote.Text.Trim();

        //插入数据
        try
        {
            if (baseBO.Insert(areaSize) != -1)
            {
                //Response.Write("<script language=javascript>alert('添加成功!');</script>");
            }
            else
            {
               //Response.Write("<script language=javascript>alert('添加失败!');</script>");
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
        page();
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        RentLevel areaSize = new RentLevel();
        areaSize.RentLevelCode = this.txtAreaSizeCode.Text;
        areaSize.RentLevelDesc = this.txtAreaSizeName.Text;
        areaSize.RentLevelStatus = Convert.ToInt32(this.cmbAreaSizeStatus.SelectedValue);
        areaSize.Note = this.txtNote.Text;

        try
        {
            baseBO.WhereClause = "RentLevelID=" + Convert.ToInt32(ViewState["RentLevelID"]);
            if (baseBO.Update(areaSize) != -1)
            {
                //Response.Write("<script language=javascript>alert('修改成功!!');</script>");
            }
            else
            {
                //Response.Write("<script language=javascript>alert('修改失败!!');</script>");
            }

        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
        page();
    }
    protected void btnCel_Click(object sender, EventArgs e)
    {
        txtAreaSizeCode.Text = "";
        txtAreaSizeName.Text = "";
        txtNote.Text = "";
        page();
    }

    protected void page()
    {
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "";
        DataSet ds = baseBO.QueryDataSet(new RentLevel());
        dt = ds.Tables[0];
        int count = dt.Rows.Count;
        int ss = 0;
        //获取状态

        for (int j = 0; j < count; j++)
        {
            string areaLevelTypeStatusName = RentLevel.GetRentLevelStatusDesc(Convert.ToInt32(dt.Rows[j]["RentLevelStatus"].ToString()));
            dt.Rows[j]["RentLevelStatusName"] = areaLevelTypeStatusName;
        }

        pds.DataSource = dt.DefaultView;
        if (pds.Count < 1)
        {
            for (int i = 0; i < 10; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
        }
        else
        {

            GrdVewCustType.EmptyDataText = "";
            pds.AllowPaging = true;
            pds.PageSize = 9;
            pds.CurrentPageIndex = Convert.ToInt32(ViewState["currentCount"]) - 1;
            if (pds.IsFirstPage)
            {
                btnBack.Enabled = false;
                btnNext.Enabled = true;
            }

            if (pds.IsLastPage)
            {
                btnBack.Enabled = true;
                btnNext.Enabled = false;
            }

            if (pds.IsFirstPage && pds.IsLastPage)
            {
                btnBack.Enabled = false;
                btnNext.Enabled = false;
            }

            if (!pds.IsLastPage && !pds.IsFirstPage)
            {
                btnBack.Enabled = true;
                btnNext.Enabled = true;
            }
            GrdVewCustType.DataSource = pds;
            GrdVewCustType.DataBind();

            ss = GrdVewCustType.Rows.Count;
            for (int i = 0; i < pds.PageSize - ss; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;

        }
        GrdVewCustType.DataSource = pds;
        GrdVewCustType.DataBind();
        for (int j = 0; j < pds.PageSize - ss; j++)
            GrdVewCustType.Rows[(pds.PageSize - 1) - j].Cells[5].Text = "";

    }
    protected void GrdVewCustType_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void GrdVewCustType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        int rentLeveId = 0;
        rentLeveId = Convert.ToInt32(GrdVewCustType.SelectedRow.Cells[0].Text);
        ViewState["RentLevelID"] = rentLeveId;
        baseBO.WhereClause = "RentLevelID=" + rentLeveId;

        Resultset rs = baseBO.Query(new RentLevel());
        if (rs.Count == 1)
        {

            RentLevel areaLevel = rs.Dequeue() as RentLevel;
            txtAreaSizeCode.Text = areaLevel.RentLevelCode;
            txtAreaSizeName.Text = areaLevel.RentLevelDesc;
            cmbAreaSizeStatus.SelectedValue = areaLevel.RentLevelStatus.ToString();
            txtNote.Text = areaLevel.Note;
        }
        page();
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        ViewState["currentCount"] = Convert.ToInt32(Convert.ToInt32(ViewState["currentCount"]) - 1);
        page();
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        ViewState["currentCount"] = Convert.ToInt32(Convert.ToInt32(ViewState["currentCount"]) + 1);
        page();
    }
}

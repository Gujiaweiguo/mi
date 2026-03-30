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
using Lease.AdContract;
using Base.Biz;
using Base.DB;
using Base;
public partial class Lease_AdContract_AreaType : System.Web.UI.Page
{
    private int numCount = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        btnBack.Attributes.Add("OnMouseOver", "this.style.background='url(../App_Themes/CSS/BtnImage/Btnbacking.gif) no-repeat left top';this.style.fontWeight='bold';");
        btnBack.Attributes.Add("OnMouseOut", "this.style.background='url(../App_Themes/CSS/BtnImage/Btnback.gif) no-repeat left top';this.style.fontWeight='normal';");
        btnNext.Attributes.Add("OnMouseOver", "this.style.background='url(../App_Themes/CSS/BtnImage/Btnnexting.gif) no-repeat left top';this.style.fontWeight='bold';");
        btnNext.Attributes.Add("OnMouseOut", "this.style.background='url(../App_Themes/CSS/BtnImage/Btnnext.gif) no-repeat left top';this.style.fontWeight='normal';");
        btnSave.Attributes.Add("OnMouseOver", "this.style.background='url(../App_Themes/CSS/BtnImage/BtnSaveing.gif) no-repeat left top';this.style.fontWeight='bold';");
        btnSave.Attributes.Add("OnMouseOut", "this.style.background='url(../App_Themes/CSS/BtnImage/BtnSave.gif) no-repeat left top';this.style.fontWeight='normal';");
        btnEdit.Attributes.Add("OnMouseOver", "this.style.background='url(../App_Themes/CSS/BtnImage/btnEditing.gif) no-repeat left top';this.style.fontWeight='bold';");
        btnEdit.Attributes.Add("OnMouseOut", "this.style.background='url(../App_Themes/CSS/BtnImage/btnEdit.gif) no-repeat left top';this.style.fontWeight='normal';");
        btnSave.Attributes.Add("onclick", "return BizGrpValidator(form1)");
        btnEdit.Attributes.Add("onclick", "return BizGrpValidator(form1)");
        if (!IsPostBack)
        {
            int[] status = AreaType.GetAreaTypeStatus();
            for (int i = 0; i < status.Length; i++)
            {
                cmbAreaTypeStatus.Items.Add(new ListItem(AreaType.GetAreaTypeStatussDesc(status[i]), status[i].ToString()));
            }
            page();
        }
    }

    protected void page()
    {
        BaseBO basebo = new BaseBO();
        string gridhtml = "";

        PagedDataSource pds = new PagedDataSource();
        pds.DataSource = basebo.QueryDataSet(new AreaType()).Tables[0].DefaultView;

        if (pds.Count < 1)
        {
            gridhtml = "<tr style='background-color:#E1E0B2;font-size:50px;font-weight:normal;height:10px;'>" +
             "   <th scope='col' style='font-size:14px;font-weight:normal;'>" + hidAreaTypeCode.Value + "</th><th scope='col' style='font-size:14px;font-weight:normal;'>" + hidAreaTypeDesc.Value + "</th><th scope='col' style='font-size:14px;font-weight:normal;'>" + hidAreaTypeStatus.Value + "</th><th scope='col' style='font-size:14px;font-weight:normal;'>" + hidAreaTypeChang.Value + "</th>" +
             //"<th scope='col' style='font-size:14px;font-weight:normal;'>" + hidChang.Value + "</th>" +
          "  </tr><tr >" +
           " <td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>" +
            "</tr><tr >" +
                "<td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>" +
            "</tr><tr >" +
               "<td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>" +
            "</tr><tr >" +
                "<td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>" +
            "</tr><tr >" +
                "<td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>" +
            "</tr><tr >" +
                "<td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>" +
            "</tr><tr>" +
                "<td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>" +
            "</tr>";

            GridView1.EmptyDataText = gridhtml;
            GridView1.CellPadding = 0;
            GridView1.CellSpacing = 0;
            GridView1.BorderWidth = Unit.Pixel(0);
        }
        else
        {
            GridView1.EmptyDataText = "";
            pds.AllowPaging = true;
            pds.PageSize = 7;
            lblTotalNum.Text = "/" + pds.PageCount.ToString() + " page";
            pds.CurrentPageIndex = int.Parse(lblCurrent.Text) - 1;
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
        }
        this.GridView1.DataSource = pds;
        this.GridView1.DataBind();

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        AreaType areaType = new AreaType();
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "AreaTypeCode = '" + txtAreaTypeCode.Text.ToString() + "'";
        Resultset rs = baseBO.Query(areaType);
        if (rs.Count == 1)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "showlineError()", true);
            page();
            return;
        }
        else
        {
            areaType.AreaTypeID = BaseApp.GetAreaTypeID();
            areaType.AreaTypeCode = txtAreaTypeCode.Text.Trim();
            areaType.AreaTypeDesc = txtAreaTypeDesc.Text.Trim();
            areaType.AreaTypeStatus = Convert.ToInt32(cmbAreaTypeStatus.SelectedValue);

            if (baseBO.Insert(areaType) < 1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "showlineError()", true);
                page();
                return;
            }
            else
            {
                page();
                //textClear();
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "showlineIns()", true);
            }
        }
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {

    }
    protected void btnBack_Click(object sender, EventArgs e)
    {

    }
    protected void btnNext_Click(object sender, EventArgs e)
    {

    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // 计算自动填充的行数
            numCount++;
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            // 计算完毕，在此添加缺少的行
            int toLeft = GridView1.PageSize - numCount;
            int numCols = GridView1.Rows[0].Cells.Count - 1;

            for (int i = 0; i < toLeft; i++)
            {
                GridViewRow row = new GridViewRow(-1, -1, DataControlRowType.EmptyDataRow, DataControlRowState.Normal);
                for (int j = 0; j < numCols; j++)
                {
                    TableCell cell = new TableCell();
                    cell.Text = "&nbsp;";
                    row.Cells.Add(cell);
                }
                GridView1.Controls[0].Controls.AddAt(numCount + 1 + i, row);
            }
        }
        GridView1.BorderWidth = System.Web.UI.WebControls.Unit.Point(1);
        GridView1.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
    }
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        //BaseBO baseBO = new BaseBO();
        //Resultset rs = new Resultset();
        //QueryBizGrp bizGrp = new QueryBizGrp();
        //ViewState["BizGrpID"] = GridView1.SelectedRow.Cells[0].Text;
        //baseBO.WhereClause = "BizGrpID =" + GridView1.SelectedRow.Cells[0].Text;
        //rs = baseBO.Query(bizGrp);
        //if (rs.Count == 1)
        //{
        //    bizGrp = rs.Dequeue() as QueryBizGrp;
        //    txtBizGrpCode.Text = bizGrp.BizGrpCode;
        //    txtBizGrpName.Text = bizGrp.BizGrpName;
        //    txtNote.Text = bizGrp.Note;
        //    cmbBizGrpStatus.SelectedValue = bizGrp.BizGrpStatus.ToString();
        //}
        //page();
        //btnSave.Enabled = false;
        //btnEdit.Enabled = true;
        //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "showlineIns()", true);
    }
}

using System;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;

using Base.Biz;
using Base.DB;
using Base.Page;
using BaseInfo.Store;
using RentableArea;
using Lease.PotCust;

public partial class Report_PotCustomer:BasePage
{
    public string baseInfo;
    public string strFresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindBizProject();
            BindCustFrom();
            strFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
            baseInfo = (String)GetGlobalResourceObject("ReportInfo", "Menu_PotCustomer");
        }
        
    }


    //绑定商业项目
    private void BindBizProject()
    {
        BaseBO baseBo = new BaseBO();
        Resultset rs = baseBo.Query(new Store());
        txtBizproject.Items.Add(new ListItem("", ""));
        foreach (Store store in rs)
        {
            txtBizproject.Items.Add(new ListItem(store.StoreName, store.StoreId.ToString()));        
        }
    
    }

    //绑定大楼
    private void BindBuilding(string BindNumber)
    {
        BaseBO baseBo = new BaseBO();
        baseBo.WhereClause = " Building.StoreID=" + int.Parse(BindNumber) + "";
        txtBuilding.Items.Add(new ListItem("", ""));
        Resultset rs = baseBo.Query(new Building());
        foreach (Building building in rs)
        {
            txtBuilding.Items.Add(new ListItem(building.BuildingName, building.BuildingID.ToString()));
        }
    }

    //绑定楼层
    private void BindFloor(string BindNumber)
    {
        BaseBO baseBo = new BaseBO();
        baseBo.WhereClause = " Floors.StoreID=" + int.Parse(BindNumber) + "";
        txtFloor.Items.Add(new ListItem("", ""));
        Resultset rs = baseBo.Query(new Floors());
        foreach (Floors floors in rs)
        {
            txtFloor.Items.Add(new ListItem(floors.FloorName, floors.FloorID.ToString()));
        }
    }

    //选择绑定大楼和楼层
    protected void txtBizproject_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtBuilding.Items.Clear();
        txtFloor.Items.Clear();
        BindBuilding(txtBizproject.SelectedValue);
        BindFloor(txtBizproject.SelectedValue);
    }

    //绑定用户来源
    private void BindCustFrom()
    {
        BaseBO baseBo = new BaseBO();
        txtCustFrom.Items.Add(new ListItem("", ""));
        Resultset rs = baseBo.Query(new SourceType());
        foreach (SourceType st in rs)
        {
            txtCustFrom.Items.Add(new ListItem(st.SourceTypeName, st.SourceTypeId.ToString()));
        }
    }

    //绑定数据
    private void BindData()
    {
        ParameterFields Fields = new ParameterFields();
        ParameterField[] Field = new ParameterField[14];
        ParameterDiscreteValue[] DiscreteValue = new ParameterDiscreteValue[14];
        ParameterRangeValue RangeValue = new ParameterRangeValue();

        Field[0] = new ParameterField();
        Field[0].Name = "REXTitle";
        DiscreteValue[0] = new ParameterDiscreteValue();
        DiscreteValue[0].Value = (String)GetGlobalResourceObject("ReportInfo","Menu_PotCustomer");
        Field[0].CurrentValues.Add(DiscreteValue[0]);

        Field[1] = new ParameterField();
        Field[1].Name = "REXIndex";
        DiscreteValue[1] = new ParameterDiscreteValue();
        DiscreteValue[1].Value = (String)GetGlobalResourceObject("BaseInfo", "WrkFlw_Sequence");
        Field[1].CurrentValues.Add(DiscreteValue[1]);
        
        Field[2] = new ParameterField();
        Field[2].Name = "REXBizProject";
        DiscreteValue[2] = new ParameterDiscreteValue();
        DiscreteValue[2].Value = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_BusinessItem");
        Field[2].CurrentValues.Add(DiscreteValue[2]);

        Field[3] = new ParameterField();
        Field[3].Name = "REXCustCode";
        DiscreteValue[3] = new ParameterDiscreteValue();
        DiscreteValue[3].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_CustCode");// 商户号
        Field[3].CurrentValues.Add(DiscreteValue[3]);

        Field[4] = new ParameterField();
        Field[4].Name = "REXCustName";
        DiscreteValue[4] = new ParameterDiscreteValue();
        DiscreteValue[4].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_CustName");// 商户名称
        Field[4].CurrentValues.Add(DiscreteValue[4]);

        Field[5] = new ParameterField();
        Field[5].Name = "REXCustShortName";
        DiscreteValue[5] = new ParameterDiscreteValue();
        DiscreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_CustShortName"); //商户简称
        Field[5].CurrentValues.Add(DiscreteValue[5]);

        Field[6] = new ParameterField();
        Field[6].Name = "REXLawerName";
        DiscreteValue[6] = new ParameterDiscreteValue();
        DiscreteValue[6].Value = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_lblLegalRep");
        Field[6].CurrentValues.Add(DiscreteValue[6]);

        Field[7] = new ParameterField();
        Field[7].Name = "REXRegFinancing";
        DiscreteValue[7] = new ParameterDiscreteValue();
        DiscreteValue[7].Value = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_lblRegCap");
        Field[7].CurrentValues.Add(DiscreteValue[7]);

        Field[8] = new ParameterField();
        Field[8].Name = "REXRegAddress";
        DiscreteValue[8] = new ParameterDiscreteValue();
        DiscreteValue[8].Value = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_lblRegAddr");
        Field[8].CurrentValues.Add(DiscreteValue[8]);

        Field[9] = new ParameterField();
        Field[9].Name = "REXOfficeAddr";
        DiscreteValue[9] = new ParameterDiscreteValue();
        DiscreteValue[9].Value = (String)GetGlobalResourceObject("ReportInfo", "RptCustomerInfo_OfficeAddr");
        Field[9].CurrentValues.Add(DiscreteValue[9]);

        Field[10] = new ParameterField();
        Field[10].Name = "REXPostCode";
        DiscreteValue[10] = new ParameterDiscreteValue();
        DiscreteValue[10].Value = (String)GetGlobalResourceObject("ReportInfo", "RptCustomerInfo_PostCode");
        Field[10].CurrentValues.Add(DiscreteValue[10]);

        Field[11] = new ParameterField();
        Field[11].Name = "REXCustFrom";
        DiscreteValue[11] = new ParameterDiscreteValue();
        DiscreteValue[11].Value = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_CustSource");
        Field[11].CurrentValues.Add(DiscreteValue[11]);

        Field[12] = new ParameterField();
        Field[12].Name = "REXNoter";
        DiscreteValue[12] = new ParameterDiscreteValue();
        DiscreteValue[12].Value = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_InputPeople");
        Field[12].CurrentValues.Add(DiscreteValue[12]);

        Field[13] = new ParameterField();
        Field[13].Name = "REXMallTitle";
        DiscreteValue[13] = new ParameterDiscreteValue();
        DiscreteValue[13].Value = Session["MallTitle"].ToString();
        Field[13].CurrentValues.Add(DiscreteValue[13]);

        foreach (ParameterField pf in Field) {
            Fields.Add(pf);
        }



        string str_sql = "";

        str_sql = "select store.storeshortname,potcustomer.CustCode,potcustomer.custname,potcustomer.custshortname,"+
                          "potcustomer.LegalRep,potcustomer.RegCap,potcustomer.RegAddr,users.username,SourceType.SourceTypename " +
                  "from    potcustomer "+
                         "inner join users on (potcustomer.createuserid=users.userid ) " +
                         "inner join potshop on (potcustomer.custid=potshop.custid) " +
                         "inner join store on (store.storeid= potshop.storeid) " +
                         "inner join SourceType on (SourceType.SourceTypeid=potcustomer.SourceTypeid) " +
                 "where  1=1 " ;

        //条件查询
        if (txtBizproject.SelectedValue != "")
        {

            str_sql = str_sql + " AND potshop.StoreId=" + int.Parse(txtBizproject.SelectedValue) + "";

        }

        //if (txtBuilding.SelectedValue != "")
        //{
        //    if (str_sql.Contains("WHERE"))
        //    {
        //        str_sql = str_sql + " AND Building.BuildingID=" + int.Parse(txtBuilding.SelectedValue) + "";
        //    }
        //    else
        //    {
        //        str_sql = str_sql + " WHERE Building.BuildingID=" + int.Parse(txtBuilding.SelectedValue) + "";
        //    }
        //}

        //if (txtFloor.SelectedValue != "")
        //{
        //    if (str_sql.Contains("WHERE"))
        //    {
        //        str_sql = str_sql + " AND Floors.FloorID=" + int.Parse(txtFloor.SelectedValue) + "";
        //    }
        //    else
        //    {
        //        str_sql = str_sql + " WHERE Floors.FloorID=" + int.Parse(txtFloor.SelectedValue) + "";
        //    }
        //}

        if (txtBizPerson.Text != "")
        {
            str_sql = str_sql + " AND users.userName like '%" + txtBizPerson.Text.Trim() + "%'";
        }

        if (txtCustFrom.SelectedValue != "")
        {
            str_sql = str_sql + " AND SourceType.SourceTypeId=" + int.Parse(txtCustFrom.SelectedValue)+"";
        }

        if (txtStartDate.Text != "" )
        {
            str_sql = str_sql + " AND convert(varchar(10),potcustomer.createtime,120) >= '" + txtStartDate.Text.Trim() + "'";
        }

        if (txtEndDate.Text != "")
        {
            str_sql = str_sql + " AND convert(varchar(10),potcustomer.createtime,120) <= '" + this.txtEndDate.Text.Trim() + "'";
        }


        str_sql = str_sql + " Order By store.storeshortname,potcustomer.CustCode";
        Session["paraFil"] = Fields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Base\\RptPotCustomer.rpt";

    }

    //撤消操作
    private void ClearPage()
    {
        txtBizproject.SelectedIndex = 0;
        txtBuilding.SelectedIndex = 0;
        txtFloor.SelectedIndex = 0;
        txtBizPerson.Text = "";
        txtCustFrom.SelectedIndex = 0;
        txtStartDate.Text = "";
        txtEndDate.Text = "";
    }

    //查询按钮
    protected void BtnOK_Click(object sender, EventArgs e)
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        BindData();
        this.Response.Redirect("../ReportShow.aspx");
    }

    //撤消按钮
    protected void BtnCel_Click(object sender, EventArgs e)
    {
        ClearPage();
    }


}

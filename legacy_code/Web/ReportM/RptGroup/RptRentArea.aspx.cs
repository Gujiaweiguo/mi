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
using CrystalDecisions.Shared;

using Base.Biz;
using Base.DB;
using BaseInfo.Store;
using Base.Page;
using BaseInfo.Dept;

public partial class ReportM_RptGroup_RptRentArea : BasePage
{
    public string baseInfo;
    public string pageTitle;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitDDL();
            pageTitle = (String)GetGlobalResourceObject("BaseInfo", "Menu_RentAreaOverview");
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        }
    }

    private void InitDDL()
    {
        BaseBO baseBo = new BaseBO();
        DataSet ds = new DataSet();
        baseBo.WhereClause = "depttype='" + Dept.DEPT_TYPE_MALL + "'";
        ds = baseBo.QueryDataSet(new Dept());
        DDLStore.Items.Clear();
        DDLStore.Items.Add(new ListItem("", ""));
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            DDLStore.Items.Add(new ListItem(ds.Tables[0].Rows[i]["deptname"].ToString().Trim(), ds.Tables[0].Rows[i]["deptid"].ToString().Trim()));
        }


        int intMonth = 12;
        ddlMonth.Items.Clear();
        //ddlMonth.Items.Add(new ListItem("", ""));
        for (int iMonth = 1; iMonth <= intMonth; iMonth++)
        {
            ddlMonth.Items.Add(new ListItem(iMonth.ToString(), iMonth.ToString()));
        }
        ddlMonth.SelectedValue = DateTime.Now.Month.ToString();
        int year = Convert.ToInt16(DateTime.Now.Year);
        //  ddlYear.Items.Add(new ListItem("",""));
        ddlYear.Items.Clear();
        for (int time = year - 5; time <= year + 5; time++)
        {
            ddlYear.Items.Add(new ListItem(time.ToString(), time.ToString()));
        }
        ddlYear.SelectedValue = DateTime.Now.Year.ToString();
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        BindData();
        this.Response.Redirect("../ReportShow.aspx");
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        InitDDL();
    }

    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[3];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[3];
        ParameterRangeValue rangeValue = new ParameterRangeValue();

        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXTitleSum";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "Menu_RentAreaOverviewSum");
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].ParameterFieldName = "REXTitleDetail";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("BaseInfo", "Menu_RentAreaOverviewDetail");
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXMallTitle";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = Session["MallTitle"].ToString();
        paraField[2].CurrentValues.Add(discreteValue[2]);


        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }
        string str_sql = "";
        string wheresql = "";

        if (DDLStore.Text != "")
        {
            wheresql += " where pp.storeid='" + DDLStore.SelectedValue.Trim() + "' ";
        }

        if (RadioButton1.Checked == true)
        {
            #region sqlsum
            str_sql = @"select period,w,ch,floorarea,lmfloorarea,lyfloorarea,storeid,deptname
                    from (
	                    select aa.period,1 w,'建筑面积(sqm)' ch,aa.floorarea,bb.floorarea lmfloorarea,cc.floorarea lyfloorarea,aa.storeid,aa.deptname
	                    from (
		                    select a.period,sum(a.floorarea) floorarea,sum(a.usearea) canrentarea,cast(sum(a.usearea)/sum(a.floorarea)*100 as decimal(10,2)) houserate,b.rented rentedarea,cast(b.rented/sum(a.usearea)*100 as decimal(10,2)) rentrate,a.storeid,dept.deptname
		                    from unitrent a
		                    left join (
			                    select period,sum(usearea) rented,storeid 
			                    from unitrent 
			                    where shopid<>0 
			                    group by period,storeid) b on (b.period=a.period and b.storeid=a.storeid)
		                    left join dept on dept.deptid=a.storeid
		                    where a.period='" + ddlYear.Text.Trim() + "-" + ddlMonth.Text.Trim() + @"-1'
		                    group by a.period,b.rented,a.storeid,dept.deptname
		                    ) aa
	                    left join (
		                    select a.period,sum(a.floorarea) floorarea,sum(a.usearea) canrentarea,cast(sum(a.usearea)/sum(a.floorarea)*100 as decimal(10,2)) houserate,b.rented rentedarea,cast(b.rented/sum(a.usearea)*100 as decimal(10,2)) rentrate,a.storeid,dept.deptname
		                    from unitrent a
		                    left join (
			                    select period,sum(usearea) rented,storeid 
			                    from unitrent 
			                    where shopid<>0 
			                    group by period,storeid) b on (b.period=a.period and b.storeid=a.storeid)
		                    left join dept on dept.deptid=a.storeid
		                    where a.period='" + ddlYear.Text.Trim() + "-" + (int.Parse(ddlMonth.Text.Trim()) - 1) + @"-1'
		                    group by a.period,b.rented,a.storeid,dept.deptname
		                    ) bb on (aa.storeid=bb.storeid)
	                    left join (
		                    select a.period,sum(a.floorarea) floorarea,sum(a.usearea) canrentarea,cast(sum(a.usearea)/sum(a.floorarea)*100 as decimal(10,2)) houserate,b.rented rentedarea,cast(b.rented/sum(a.usearea)*100 as decimal(10,2)) rentrate,a.storeid,dept.deptname
		                    from unitrent a
		                    left join (
			                    select period,sum(usearea) rented,storeid 
			                    from unitrent 
			                    where shopid<>0 
			                    group by period,storeid) b on (b.period=a.period and b.storeid=a.storeid)
		                    left join dept on dept.deptid=a.storeid
		                    where a.period='" + (int.Parse(ddlYear.Text.Trim()) - 1) + "-" + ddlMonth.Text.Trim() + @"-1'
		                    group by a.period,b.rented,a.storeid,dept.deptname
		                    ) cc on (aa.storeid=cc.storeid)

	                    union 

	                    select aa.period,2 w,'可租赁面积(sqm)' ch,aa.canrentarea,bb.canrentarea,cc.canrentarea,aa.storeid,aa.deptname
	                    from (
		                    select a.period,sum(a.floorarea) floorarea,sum(a.usearea) canrentarea,cast(sum(a.usearea)/sum(a.floorarea)*100 as decimal(10,2)) houserate,b.rented rentedarea,cast(b.rented/sum(a.usearea)*100 as decimal(10,2)) rentrate,a.storeid,dept.deptname
		                    from unitrent a
		                    left join (
			                    select period,sum(usearea) rented,storeid 
			                    from unitrent 
			                    where shopid<>0 
			                    group by period,storeid) b on (b.period=a.period and b.storeid=a.storeid)
		                    left join dept on dept.deptid=a.storeid
		                    where a.period='" + ddlYear.Text.Trim() + "-" + ddlMonth.Text.Trim() + @"-1'
		                    group by a.period,b.rented,a.storeid,dept.deptname
		                    ) aa
	                    left join (
		                    select a.period,sum(a.floorarea) floorarea,sum(a.usearea) canrentarea,cast(sum(a.usearea)/sum(a.floorarea)*100 as decimal(10,2)) houserate,b.rented rentedarea,cast(b.rented/sum(a.usearea)*100 as decimal(10,2)) rentrate,a.storeid,dept.deptname
		                    from unitrent a
		                    left join (
			                    select period,sum(usearea) rented,storeid 
			                    from unitrent 
			                    where shopid<>0 
			                    group by period,storeid) b on (b.period=a.period and b.storeid=a.storeid)
		                    left join dept on dept.deptid=a.storeid
		                    where a.period='" + ddlYear.Text.Trim() + "-" + (int.Parse(ddlMonth.Text.Trim()) - 1) + @"-1'
		                    group by a.period,b.rented,a.storeid,dept.deptname
		                    ) bb on (aa.storeid=bb.storeid)
	                    left join (
		                    select a.period,sum(a.floorarea) floorarea,sum(a.usearea) canrentarea,cast(sum(a.usearea)/sum(a.floorarea)*100 as decimal(10,2)) houserate,b.rented rentedarea,cast(b.rented/sum(a.usearea)*100 as decimal(10,2)) rentrate,a.storeid,dept.deptname
		                    from unitrent a
		                    left join (
			                    select period,sum(usearea) rented,storeid 
			                    from unitrent 
			                    where shopid<>0 
			                    group by period,storeid) b on (b.period=a.period and b.storeid=a.storeid)
		                    left join dept on dept.deptid=a.storeid
		                    where a.period='" + (int.Parse(ddlYear.Text.Trim()) - 1) + "-" + ddlMonth.Text.Trim() + @"-1'
		                    group by a.period,b.rented,a.storeid,dept.deptname
		                    ) cc on (aa.storeid=cc.storeid)

	                    union 

	                    select aa.period,3 w,'得房率(%)' ch,aa.houserate,bb.houserate,cc.houserate,aa.storeid,aa.deptname
	                    from (
		                    select a.period,sum(a.floorarea) floorarea,sum(a.usearea) canrentarea,cast(sum(a.usearea)/sum(a.floorarea)*100 as decimal(10,2)) houserate,b.rented rentedarea,cast(b.rented/sum(a.usearea)*100 as decimal(10,2)) rentrate,a.storeid,dept.deptname
		                    from unitrent a
		                    left join (
			                    select period,sum(usearea) rented,storeid 
			                    from unitrent 
			                    where shopid<>0 
			                    group by period,storeid) b on (b.period=a.period and b.storeid=a.storeid)
		                    left join dept on dept.deptid=a.storeid
		                    where a.period='" + ddlYear.Text.Trim() + "-" + ddlMonth.Text.Trim() + @"-1'
		                    group by a.period,b.rented,a.storeid,dept.deptname
		                    ) aa
	                    left join (
		                    select a.period,sum(a.floorarea) floorarea,sum(a.usearea) canrentarea,cast(sum(a.usearea)/sum(a.floorarea)*100 as decimal(10,2)) houserate,b.rented rentedarea,cast(b.rented/sum(a.usearea)*100 as decimal(10,2)) rentrate,a.storeid,dept.deptname
		                    from unitrent a
		                    left join (
			                    select period,sum(usearea) rented,storeid 
			                    from unitrent 
			                    where shopid<>0 
			                    group by period,storeid) b on (b.period=a.period and b.storeid=a.storeid)
		                    left join dept on dept.deptid=a.storeid
		                    where a.period='" + ddlYear.Text.Trim() + "-" + (int.Parse(ddlMonth.Text.Trim()) - 1) + @"-1'
		                    group by a.period,b.rented,a.storeid,dept.deptname
		                    ) bb on (aa.storeid=bb.storeid)
	                    left join (
		                    select a.period,sum(a.floorarea) floorarea,sum(a.usearea) canrentarea,cast(sum(a.usearea)/sum(a.floorarea)*100 as decimal(10,2)) houserate,b.rented rentedarea,cast(b.rented/sum(a.usearea)*100 as decimal(10,2)) rentrate,a.storeid,dept.deptname
		                    from unitrent a
		                    left join (
			                    select period,sum(usearea) rented,storeid 
			                    from unitrent 
			                    where shopid<>0 
			                    group by period,storeid) b on (b.period=a.period and b.storeid=a.storeid)
		                    left join dept on dept.deptid=a.storeid
		                    where a.period='" + (int.Parse(ddlYear.Text.Trim()) - 1) + "-" + ddlMonth.Text.Trim() + @"-1'
		                    group by a.period,b.rented,a.storeid,dept.deptname
		                    ) cc on (aa.storeid=cc.storeid)

	                    union 

	                    select aa.period,4 w,'已租赁面积(sqm)' ch,aa.rentedarea,bb.rentedarea,cc.rentedarea,aa.storeid,aa.deptname
	                    from (
		                    select a.period,sum(a.floorarea) floorarea,sum(a.usearea) canrentarea,cast(sum(a.usearea)/sum(a.floorarea)*100 as decimal(10,2)) houserate,b.rented rentedarea,cast(b.rented/sum(a.usearea)*100 as decimal(10,2)) rentrate,a.storeid,dept.deptname
		                    from unitrent a
		                    left join (
			                    select period,sum(usearea) rented,storeid 
			                    from unitrent 
			                    where shopid<>0 
			                    group by period,storeid) b on (b.period=a.period and b.storeid=a.storeid)
		                    left join dept on dept.deptid=a.storeid
		                    where a.period='" + ddlYear.Text.Trim() + "-" + ddlMonth.Text.Trim() + @"-1'
		                    group by a.period,b.rented,a.storeid,dept.deptname
		                    ) aa
	                    left join (
		                    select a.period,sum(a.floorarea) floorarea,sum(a.usearea) canrentarea,cast(sum(a.usearea)/sum(a.floorarea)*100 as decimal(10,2)) houserate,b.rented rentedarea,cast(b.rented/sum(a.usearea)*100 as decimal(10,2)) rentrate,a.storeid,dept.deptname
		                    from unitrent a
		                    left join (
			                    select period,sum(usearea) rented,storeid 
			                    from unitrent 
			                    where shopid<>0 
			                    group by period,storeid) b on (b.period=a.period and b.storeid=a.storeid)
		                    left join dept on dept.deptid=a.storeid
		                    where a.period='" + ddlYear.Text.Trim() + "-" + (int.Parse(ddlMonth.Text.Trim()) - 1) + @"-1'
		                    group by a.period,b.rented,a.storeid,dept.deptname
		                    ) bb on (aa.storeid=bb.storeid)
	                    left join (
		                    select a.period,sum(a.floorarea) floorarea,sum(a.usearea) canrentarea,cast(sum(a.usearea)/sum(a.floorarea)*100 as decimal(10,2)) houserate,b.rented rentedarea,cast(b.rented/sum(a.usearea)*100 as decimal(10,2)) rentrate,a.storeid,dept.deptname
		                    from unitrent a
		                    left join (
			                    select period,sum(usearea) rented,storeid 
			                    from unitrent 
			                    where shopid<>0 
			                    group by period,storeid) b on (b.period=a.period and b.storeid=a.storeid)
		                    left join dept on dept.deptid=a.storeid
		                    where a.period='" + (int.Parse(ddlYear.Text.Trim()) - 1) + "-" + ddlMonth.Text.Trim() + @"-1'
		                    group by a.period,b.rented,a.storeid,dept.deptname
		                    ) cc on (aa.storeid=cc.storeid)

	                    union 

	                    select aa.period,5 w,'出租率(%)' ch,aa.rentrate,bb.rentrate,cc.rentrate,aa.storeid,aa.deptname
	                    from (
		                    select a.period,sum(a.floorarea) floorarea,sum(a.usearea) canrentarea,cast(sum(a.usearea)/sum(a.floorarea)*100 as decimal(10,2)) houserate,b.rented rentedarea,cast(b.rented/sum(a.usearea)*100 as decimal(10,2)) rentrate,a.storeid,dept.deptname
		                    from unitrent a
		                    left join (
			                    select period,sum(usearea) rented,storeid 
			                    from unitrent 
			                    where shopid<>0 
			                    group by period,storeid) b on (b.period=a.period and b.storeid=a.storeid)
		                    left join dept on dept.deptid=a.storeid
		                    where a.period='" + ddlYear.Text.Trim() + "-" + ddlMonth.Text.Trim() + @"-1'
		                    group by a.period,b.rented,a.storeid,dept.deptname
		                    ) aa
	                    left join (
		                    select a.period,sum(a.floorarea) floorarea,sum(a.usearea) canrentarea,cast(sum(a.usearea)/sum(a.floorarea)*100 as decimal(10,2)) houserate,b.rented rentedarea,cast(b.rented/sum(a.usearea)*100 as decimal(10,2)) rentrate,a.storeid,dept.deptname
		                    from unitrent a
		                    left join (
			                    select period,sum(usearea) rented,storeid 
			                    from unitrent 
			                    where shopid<>0 
			                    group by period,storeid) b on (b.period=a.period and b.storeid=a.storeid)
		                    left join dept on dept.deptid=a.storeid
		                    where a.period='" + ddlYear.Text.Trim() + "-" + (int.Parse(ddlMonth.Text.Trim()) - 1) + @"-1'
		                    group by a.period,b.rented,a.storeid,dept.deptname
		                    ) bb on (aa.storeid=bb.storeid)
	                    left join (
		                    select a.period,sum(a.floorarea) floorarea,sum(a.usearea) canrentarea,cast(sum(a.usearea)/sum(a.floorarea)*100 as decimal(10,2)) houserate,b.rented rentedarea,cast(b.rented/sum(a.usearea)*100 as decimal(10,2)) rentrate,a.storeid,dept.deptname
		                    from unitrent a
		                    left join (
			                    select period,sum(usearea) rented,storeid 
			                    from unitrent 
			                    where shopid<>0 
			                    group by period,storeid) b on (b.period=a.period and b.storeid=a.storeid)
		                    left join dept on dept.deptid=a.storeid
		                    where a.period='" + (int.Parse(ddlYear.Text.Trim()) - 1) + "-" + ddlMonth.Text.Trim() + @"-1'
		                    group by a.period,b.rented,a.storeid,dept.deptname
		                    ) cc on (aa.storeid=cc.storeid)) pp " + wheresql + " order by pp.storeid,pp.w ";

            #endregion


            Session["paraFil"] = paraFields;
            Session["sql"] = str_sql;
            Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Group\\RptRentArea.rpt";
        }
        else
        {
            #region sqldetail
            str_sql = @"select pp.period,pp.w,pp.ch,pp.floorarea,pp.lmfloorarea,pp.lyfloorarea,pp.storeid,pp.deptname,pp.floorid,pp.shoptypeid,pp.buildingid,floors.floorname,shoptype.shoptypename,building.buildingname
                        from (
	                        select aa.period,1 w,'建筑面积(sqm)' ch,aa.floorarea,bb.floorarea lmfloorarea,cc.floorarea lyfloorarea,aa.storeid,aa.deptname,aa.floorid,aa.shoptypeid,aa.buildingid
	                        from (
		                        select a.period,sum(a.floorarea) floorarea,sum(a.usearea) canrentarea,cast(sum(a.usearea)/sum(a.floorarea)*100 as decimal(10,2)) houserate,b.rented rentedarea,cast(b.rented/sum(a.usearea)*100 as decimal(10,2)) rentrate,a.storeid,dept.deptname,a.floorid,a.shoptypeid,a.buildingid
		                        from unitrent a
		                        left join (
			                        select period,sum(usearea) rented,storeid 
			                        from unitrent 
			                        where shopid<>0 
			                        group by period,storeid) b on (b.period=a.period and b.storeid=a.storeid)
		                        left join dept on dept.deptid=a.storeid
		                        where a.period='" + ddlYear.Text.Trim() + "-" + ddlMonth.Text.Trim() + @"-1'
		                        group by a.period,b.rented,a.storeid,dept.deptname,a.floorid,a.shoptypeid,a.buildingid
		                        ) aa
	                        left join (
		                        select a.period,sum(a.floorarea) floorarea,sum(a.usearea) canrentarea,cast(sum(a.usearea)/sum(a.floorarea)*100 as decimal(10,2)) houserate,b.rented rentedarea,cast(b.rented/sum(a.usearea)*100 as decimal(10,2)) rentrate,a.storeid,dept.deptname
		                        from unitrent a
		                        left join (
			                        select period,sum(usearea) rented,storeid 
			                        from unitrent 
			                        where shopid<>0 
			                        group by period,storeid) b on (b.period=a.period and b.storeid=a.storeid)
		                        left join dept on dept.deptid=a.storeid
		                        where a.period='" + ddlYear.Text.Trim() + "-" + (int.Parse(ddlMonth.Text.Trim()) - 1) + @"-1'
		                        group by a.period,b.rented,a.storeid,dept.deptname
		                        ) bb on (aa.storeid=bb.storeid)
	                        left join (
		                        select a.period,sum(a.floorarea) floorarea,sum(a.usearea) canrentarea,cast(sum(a.usearea)/sum(a.floorarea)*100 as decimal(10,2)) houserate,b.rented rentedarea,cast(b.rented/sum(a.usearea)*100 as decimal(10,2)) rentrate,a.storeid,dept.deptname
		                        from unitrent a
		                        left join (
			                        select period,sum(usearea) rented,storeid 
			                        from unitrent 
			                        where shopid<>0 
			                        group by period,storeid) b on (b.period=a.period and b.storeid=a.storeid)
		                        left join dept on dept.deptid=a.storeid
		                        where a.period='" + (int.Parse(ddlYear.Text.Trim()) - 1) + "-" + ddlMonth.Text.Trim() + @"-1'
		                        group by a.period,b.rented,a.storeid,dept.deptname
		                        ) cc on (aa.storeid=cc.storeid)

	                        union 

	                        select aa.period,2 w,'可租赁面积(sqm)' ch,aa.canrentarea,bb.canrentarea,cc.canrentarea,aa.storeid,aa.deptname,aa.floorid,aa.shoptypeid,aa.buildingid
	                        from (
		                        select a.period,sum(a.floorarea) floorarea,sum(a.usearea) canrentarea,cast(sum(a.usearea)/sum(a.floorarea)*100 as decimal(10,2)) houserate,b.rented rentedarea,cast(b.rented/sum(a.usearea)*100 as decimal(10,2)) rentrate,a.storeid,dept.deptname,a.floorid,a.shoptypeid,a.buildingid
		                        from unitrent a
		                        left join (
			                        select period,sum(usearea) rented,storeid 
			                        from unitrent 
			                        where shopid<>0 
			                        group by period,storeid) b on (b.period=a.period and b.storeid=a.storeid)
		                        left join dept on dept.deptid=a.storeid
		                        where a.period='" + ddlYear.Text.Trim() + "-" + ddlMonth.Text.Trim() + @"-1'
		                        group by a.period,b.rented,a.storeid,dept.deptname,a.floorid,a.shoptypeid,a.buildingid
		                        ) aa
	                        left join (
		                        select a.period,sum(a.floorarea) floorarea,sum(a.usearea) canrentarea,cast(sum(a.usearea)/sum(a.floorarea)*100 as decimal(10,2)) houserate,b.rented rentedarea,cast(b.rented/sum(a.usearea)*100 as decimal(10,2)) rentrate,a.storeid,dept.deptname
		                        from unitrent a
		                        left join (
			                        select period,sum(usearea) rented,storeid 
			                        from unitrent 
			                        where shopid<>0 
			                        group by period,storeid) b on (b.period=a.period and b.storeid=a.storeid)
		                        left join dept on dept.deptid=a.storeid
		                        where a.period='" + ddlYear.Text.Trim() + "-" + (int.Parse(ddlMonth.Text.Trim()) - 1) + @"-1'
		                        group by a.period,b.rented,a.storeid,dept.deptname
		                        ) bb on (aa.storeid=bb.storeid)
	                        left join (
		                        select a.period,sum(a.floorarea) floorarea,sum(a.usearea) canrentarea,cast(sum(a.usearea)/sum(a.floorarea)*100 as decimal(10,2)) houserate,b.rented rentedarea,cast(b.rented/sum(a.usearea)*100 as decimal(10,2)) rentrate,a.storeid,dept.deptname
		                        from unitrent a
		                        left join (
			                        select period,sum(usearea) rented,storeid 
			                        from unitrent 
			                        where shopid<>0 
			                        group by period,storeid) b on (b.period=a.period and b.storeid=a.storeid)
		                        left join dept on dept.deptid=a.storeid
		                        where a.period='" + (int.Parse(ddlYear.Text.Trim()) - 1) + "-" + ddlMonth.Text.Trim() + @"-1'
		                        group by a.period,b.rented,a.storeid,dept.deptname
		                        ) cc on (aa.storeid=cc.storeid)

	                        union 

	                        select aa.period,3 w,'得房率(%)' ch,aa.houserate,bb.houserate,cc.houserate,aa.storeid,aa.deptname,aa.floorid,aa.shoptypeid,aa.buildingid
	                        from (
		                        select a.period,sum(a.floorarea) floorarea,sum(a.usearea) canrentarea,cast(sum(a.usearea)/sum(a.floorarea)*100 as decimal(10,2)) houserate,b.rented rentedarea,cast(b.rented/sum(a.usearea)*100 as decimal(10,2)) rentrate,a.storeid,dept.deptname,a.floorid,a.shoptypeid,a.buildingid
		                        from unitrent a
		                        left join (
			                        select period,sum(usearea) rented,storeid 
			                        from unitrent 
			                        where shopid<>0 
			                        group by period,storeid) b on (b.period=a.period and b.storeid=a.storeid)
		                        left join dept on dept.deptid=a.storeid
		                        where a.period='" + ddlYear.Text.Trim() + "-" + ddlMonth.Text.Trim() + @"-1'
		                        group by a.period,b.rented,a.storeid,dept.deptname,a.floorid,a.shoptypeid,a.buildingid
		                        ) aa
	                        left join (
		                        select a.period,sum(a.floorarea) floorarea,sum(a.usearea) canrentarea,cast(sum(a.usearea)/sum(a.floorarea)*100 as decimal(10,2)) houserate,b.rented rentedarea,cast(b.rented/sum(a.usearea)*100 as decimal(10,2)) rentrate,a.storeid,dept.deptname
		                        from unitrent a
		                        left join (
			                        select period,sum(usearea) rented,storeid 
			                        from unitrent 
			                        where shopid<>0 
			                        group by period,storeid) b on (b.period=a.period and b.storeid=a.storeid)
		                        left join dept on dept.deptid=a.storeid
		                        where a.period='" + ddlYear.Text.Trim() + "-" + (int.Parse(ddlMonth.Text.Trim()) - 1) + @"-1'
		                        group by a.period,b.rented,a.storeid,dept.deptname
		                        ) bb on (aa.storeid=bb.storeid)
	                        left join (
		                        select a.period,sum(a.floorarea) floorarea,sum(a.usearea) canrentarea,cast(sum(a.usearea)/sum(a.floorarea)*100 as decimal(10,2)) houserate,b.rented rentedarea,cast(b.rented/sum(a.usearea)*100 as decimal(10,2)) rentrate,a.storeid,dept.deptname
		                        from unitrent a
		                        left join (
			                        select period,sum(usearea) rented,storeid 
			                        from unitrent 
			                        where shopid<>0 
			                        group by period,storeid) b on (b.period=a.period and b.storeid=a.storeid)
		                        left join dept on dept.deptid=a.storeid
		                        where a.period='" + (int.Parse(ddlYear.Text.Trim()) - 1) + "-" + ddlMonth.Text.Trim() + @"-1'
		                        group by a.period,b.rented,a.storeid,dept.deptname
		                        ) cc on (aa.storeid=cc.storeid)

	                        union 

	                        select aa.period,4 w,'已租赁面积(sqm)' ch,aa.rentedarea,bb.rentedarea,cc.rentedarea,aa.storeid,aa.deptname,aa.floorid,aa.shoptypeid,aa.buildingid
	                        from (
		                        select a.period,sum(a.floorarea) floorarea,sum(a.usearea) canrentarea,cast(sum(a.usearea)/sum(a.floorarea)*100 as decimal(10,2)) houserate,b.rented rentedarea,cast(b.rented/sum(a.usearea)*100 as decimal(10,2)) rentrate,a.storeid,dept.deptname,a.floorid,a.shoptypeid,a.buildingid
		                        from unitrent a
		                        left join (
			                        select period,sum(usearea) rented,storeid 
			                        from unitrent 
			                        where shopid<>0 
			                        group by period,storeid) b on (b.period=a.period and b.storeid=a.storeid)
		                        left join dept on dept.deptid=a.storeid
		                        where a.period='" + ddlYear.Text.Trim() + "-" + ddlMonth.Text.Trim() + @"-1'
		                        group by a.period,b.rented,a.storeid,dept.deptname,a.floorid,a.shoptypeid,a.buildingid
		                        ) aa
	                        left join (
		                        select a.period,sum(a.floorarea) floorarea,sum(a.usearea) canrentarea,cast(sum(a.usearea)/sum(a.floorarea)*100 as decimal(10,2)) houserate,b.rented rentedarea,cast(b.rented/sum(a.usearea)*100 as decimal(10,2)) rentrate,a.storeid,dept.deptname
		                        from unitrent a
		                        left join (
			                        select period,sum(usearea) rented,storeid 
			                        from unitrent 
			                        where shopid<>0 
			                        group by period,storeid) b on (b.period=a.period and b.storeid=a.storeid)
		                        left join dept on dept.deptid=a.storeid
		                        where a.period='" + ddlYear.Text.Trim() + "-" + (int.Parse(ddlMonth.Text.Trim()) - 1) + @"-1'
		                        group by a.period,b.rented,a.storeid,dept.deptname
		                        ) bb on (aa.storeid=bb.storeid)
	                        left join (
		                        select a.period,sum(a.floorarea) floorarea,sum(a.usearea) canrentarea,cast(sum(a.usearea)/sum(a.floorarea)*100 as decimal(10,2)) houserate,b.rented rentedarea,cast(b.rented/sum(a.usearea)*100 as decimal(10,2)) rentrate,a.storeid,dept.deptname
		                        from unitrent a
		                        left join (
			                        select period,sum(usearea) rented,storeid 
			                        from unitrent 
			                        where shopid<>0 
			                        group by period,storeid) b on (b.period=a.period and b.storeid=a.storeid)
		                        left join dept on dept.deptid=a.storeid
		                        where a.period='" + (int.Parse(ddlYear.Text.Trim()) - 1) + "-" + ddlMonth.Text.Trim() + @"-1'
		                        group by a.period,b.rented,a.storeid,dept.deptname
		                        ) cc on (aa.storeid=cc.storeid)

	                        union 

	                        select aa.period,5 w,'出租率(%)' ch,aa.rentrate,bb.rentrate,cc.rentrate,aa.storeid,aa.deptname,aa.floorid,aa.shoptypeid,aa.buildingid
	                        from (
		                        select a.period,sum(a.floorarea) floorarea,sum(a.usearea) canrentarea,cast(sum(a.usearea)/sum(a.floorarea)*100 as decimal(10,2)) houserate,b.rented rentedarea,cast(b.rented/sum(a.usearea)*100 as decimal(10,2)) rentrate,a.storeid,dept.deptname,a.floorid,a.shoptypeid,a.buildingid
		                        from unitrent a
		                        left join (
			                        select period,sum(usearea) rented,storeid 
			                        from unitrent 
			                        where shopid<>0 
			                        group by period,storeid) b on (b.period=a.period and b.storeid=a.storeid)
		                        left join dept on dept.deptid=a.storeid
		                        where a.period='" + ddlYear.Text.Trim() + "-" + ddlMonth.Text.Trim() + @"-1'
		                        group by a.period,b.rented,a.storeid,dept.deptname,a.floorid,a.shoptypeid,a.buildingid
		                        ) aa
	                        left join (
		                        select a.period,sum(a.floorarea) floorarea,sum(a.usearea) canrentarea,cast(sum(a.usearea)/sum(a.floorarea)*100 as decimal(10,2)) houserate,b.rented rentedarea,cast(b.rented/sum(a.usearea)*100 as decimal(10,2)) rentrate,a.storeid,dept.deptname
		                        from unitrent a
		                        left join (
			                        select period,sum(usearea) rented,storeid 
			                        from unitrent 
			                        where shopid<>0 
			                        group by period,storeid) b on (b.period=a.period and b.storeid=a.storeid)
		                        left join dept on dept.deptid=a.storeid
		                        where a.period='" + ddlYear.Text.Trim() + "-" + (int.Parse(ddlMonth.Text.Trim()) - 1) + @"-1'
		                        group by a.period,b.rented,a.storeid,dept.deptname
		                        ) bb on (aa.storeid=bb.storeid)
	                        left join (
		                        select a.period,sum(a.floorarea) floorarea,sum(a.usearea) canrentarea,cast(sum(a.usearea)/sum(a.floorarea)*100 as decimal(10,2)) houserate,b.rented rentedarea,cast(b.rented/sum(a.usearea)*100 as decimal(10,2)) rentrate,a.storeid,dept.deptname
		                        from unitrent a
		                        left join (
			                        select period,sum(usearea) rented,storeid 
			                        from unitrent 
			                        where shopid<>0 
			                        group by period,storeid) b on (b.period=a.period and b.storeid=a.storeid)
		                        left join dept on dept.deptid=a.storeid
		                        where a.period='" + (int.Parse(ddlYear.Text.Trim()) - 1) + "-" + ddlMonth.Text.Trim() + @"-1'
		                        group by a.period,b.rented,a.storeid,dept.deptname
		                        ) cc on (aa.storeid=cc.storeid)) pp
                        left join floors on pp.floorid=floors.floorid
                        left join shoptype on pp.shoptypeid=shoptype.shoptypeid
                        left join building on pp.buildingid=building.buildingid " + wheresql + " order by pp.storeid,pp.w ";
            #endregion
            Session["paraFil"] = paraFields;
            Session["sql"] = str_sql;
            Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Group\\RptRentAreaDetail.rpt";
        }

    }
}

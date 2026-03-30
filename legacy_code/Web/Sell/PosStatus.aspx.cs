using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using Base;
using Base.Biz;
using Base.DB;
using Base.Page;
using Sell;
using BaseInfo.User;
using BaseInfo.Dept;
using BaseInfo.Store;
using RentableArea;
using BaseInfo.authUser;

public partial class Sell_PosStatus : BasePage
{
    public string strBaseinfo = "";
    public string strFresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        strBaseinfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_PosStatusInfo");
        strFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        if (!this.IsPostBack)
        {
            tdNum.Value = "1";
            ShowTree();
            BindData("");
            BindDataPayType("");
            BindDataDetail("");
            BindSignOff("");
            BindSignOn("");
        }
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "ShowDept", "TableClick(" + tdNum.Value + ")", true);
    }

    private void ShowTree()
    {
        string jsdept = "";
        BaseBO objBaseBo = new BaseBO();
        objBaseBo.WhereClause = "storestatus = 1";
        objBaseBo.OrderBy = "OrderID";
        BaseBO baseBo = new BaseBO();
        DataSet ds = new DataSet();
        ds = baseBo.QueryDataSet("select DeptName from Dept where DeptType=1");
        if (ds.Tables[0].Rows.Count > 0)
        {
            jsdept = "100" + "|" + "10" + "|" + ds.Tables[0].Rows[0][0] + "^";
        }
        else
        {
            jsdept = "100" + "|" + "10" + "|" + "商业项目" + "^";
        }
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            objBaseBo.WhereClause += " and EXISTS (SELECT storeID FROM authUser WHERE  dept.deptID = authUser.storeID AND userID =" + sessionUser.UserID + ")";
        }
        Resultset rs = objBaseBo.Query(new Store());
        if (rs.Count > 0)
        {
            foreach (Store objStore in rs)
            {
                jsdept += objStore.StoreId + "|" + "100" + "|" + objStore.StoreName + "^";
            }
        }
        depttxt.Value = jsdept;
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }

    protected void treeClick_Click(object sender, EventArgs e)
    {
        ViewState["ID"] = deptid.Value;
        this.BindData(ViewState["ID"].ToString().Trim());
        this.BindDataPayType(ViewState["ID"].ToString().Trim());
        this.BindDataDetail(ViewState["ID"].ToString().Trim());
        this.BindSignOff(ViewState["ID"].ToString().Trim());
        this.BindSignOn(ViewState["ID"].ToString().Trim());
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }

    private void BindData(string DeptID)
    {
        string Sql = @"select ROW_NUMBER() OVER(order by conshop.shopcode) as 序号,posstatus.shopid ShopID,posstatus.POSid POS机编号,conshop.shopcode 商铺编码,
                        conshop.shopname 商铺名称,posstatus.ip POS机IP,posstatus.tpusrid 收银员编号,posstatus.poslasttime 更新时间,
                        case posstatus.posstatus when 0 then '关机' when 1 then '更新成功' when 2 then '开机' when 3 then '签到'
                        when 4 then '交易中' when 6 then '正常'  when 5 then '最终签退' when 9 then '断网' end 状态
                        from posstatus
                        left join conshop on (conshop.shopid=posstatus.shopid)
                        where conshop.storeid='" + DeptID + "'";
        BaseBO baseBo = new BaseBO();
        DataSet ds = new DataSet();
        ds = baseBo.QueryDataSet(Sql);
        DataTable dt = ds.Tables[0];
        PagedDataSource pds = new PagedDataSource();
        pds.DataSource = dt.DefaultView;
        int spareRow = 0;

        if (pds.Count < 1)
        {
            for (int i = 0; i < GVPosStatus.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GVPosStatus.DataSource = pds;
            GVPosStatus.DataBind();
        }
        else
        {
            this.GVPosStatus.DataSource = pds;
            this.GVPosStatus.DataBind();
            spareRow = GVPosStatus.Rows.Count;
            for (int i = 0; i < GVPosStatus.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GVPosStatus.DataSource = pds;
            GVPosStatus.DataBind();
        }

    }

    private void BindDataPayType(string DeptID)
    {
        string Sql = @"select ROW_NUMBER() OVER(order by conshop.shopcode) 序号,conshop.shopcode 商铺编码,conshop.shopname 商铺名称,
                        aa.mediacd 支付方式,sum(aa.localamt) 金额
                        from transfooter
                        inner join transheader on (transheader.transid=transfooter.transid)
                        inner join conshop on (conshop.shopid=transheader.tenantid)
                        right join (
	                        SELECT transid,mediacd,sum(localamt) localamt 
	                        from(
		                        select transid,mediacd,case mediatype when 'C' THEN sum(-localamt) when 'T' THEN SUM(localamt) END localamt
		                        from transmedia 
		                        group by transid,mediacd,mediatype) a
	                        group by transid,mediacd) aa on (aa.transid=transheader.transid)
                        where transfooter.totalamt <>0 and transheader.bizdate=convert(varchar(10),getdate(),120) and conshop.storeid='" + DeptID + @"'
                        group by conshop.shopcode,conshop.shopname,aa.mediacd";

        BaseBO baseBo = new BaseBO();
        DataSet ds = new DataSet();
        ds = baseBo.QueryDataSet(Sql);
        DataTable dt = ds.Tables[0];
        PagedDataSource pds = new PagedDataSource();
        pds.DataSource = dt.DefaultView;
        int spareRow = 0;

        if (pds.Count < 1)
        {
            for (int i = 0; i < GVPayType.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GVPayType.DataSource = pds;
            GVPayType.DataBind();
        }
        else
        {
            this.GVPayType.DataSource = pds;
            this.GVPayType.DataBind();
            spareRow = GVPayType.Rows.Count;
            for (int i = 0; i < GVPayType.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GVPayType.DataSource = pds;
            GVPayType.DataBind();
        }

    }

    private void BindDataDetail(string DeptID)
    {
        string Sql = @"SELECT ROW_NUMBER() over(ORDER BY conshop.Shopcode, convert(varchar(20),TransHeader.TransDate,120)) 序号,conshop.ShopCode AS 商铺编码, conshop.ShopName AS 商铺名称, 
                        TransHeader.PosId AS POS机号, convert(varchar(20),TransHeader.TransDate,120) AS 交易时间, 
                        CASE transheader.transtatus WHEN 'Sale' THEN '销售' WHEN 'Return' THEN '退货' END AS 交易类型, 
                        TransHeader.ReceiptId AS 小票号, aa.sumqty AS 商品数量, TransFooter.TotalAmt AS 总金额
                        FROM TransFooter 
                        INNER JOIN TransHeader ON TransHeader.TransId = TransFooter.TransId 
                        INNER JOIN conshop ON TransHeader.TenantId = conshop.ShopID 
                        INNER JOIN (
                        SELECT TransId, SUM(Qty) AS sumqty 
                        FROM TransDetail 
                        where transid in (select transid from transheader where bizdate=CONVERT (varchar(10), GETDATE(), 120))
                        GROUP BY TransId
                        ) AS aa ON aa.TransId = TransHeader.TransId 
                        WHERE (TransFooter.TotalAmt <> 0) AND (TransHeader.BizDate = CONVERT (varchar(10), GETDATE(), 120)) and conshop.storeid='" + DeptID + "'";
        BaseBO baseBo = new BaseBO();
        DataSet ds = new DataSet();
        ds = baseBo.QueryDataSet(Sql);
        DataTable dt = ds.Tables[0];
        PagedDataSource pds = new PagedDataSource();
        pds.DataSource = dt.DefaultView;
        int spareRow = 0;

        if (pds.Count < 1)
        {
            for (int i = 0; i < GVDetail.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GVDetail.DataSource = pds;
            GVDetail.DataBind();
        }
        else
        {
            this.GVDetail.DataSource = pds;
            this.GVDetail.DataBind();
            spareRow = GVDetail.Rows.Count;
            for (int i = 0; i < GVDetail.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GVDetail.DataSource = pds;
            GVDetail.DataBind();
        }

    }

    private void BindSignOff(string DeptID)
    {
        string Sql = @"select ROW_NUMBER() over(order by conshop.shopcode,szemplnmbr,lastsignofftime) '序号',conshop.shopcode '商铺编码',
                        conshop.shopname '商铺名称',posstatus.posid 'POS机编号',szemplnmbr as '收银员编码',convert(varchar(16),signontime,120) '签到时间',
                        convert(varchar(16),lastsignofftime,120) '最终签退时间',reciptcount as '交易笔数',
                        salescount as '销售数量',salesamt as '销售金额',returncount as '退货数量',returnamt as '退货金额'
                        from lastsignoff
                        inner join conshop on (conshop.shopid=lastsignoff.shopid)
                        inner join posstatus on posstatus.shopid=lastsignoff.shopid
                        where lastsignoff.storeid='" + DeptID + "' and lastsignoff.bizdate='" + DateTime.Now.AddDays(-1).ToShortDateString() + "'";
        BaseBO baseBo = new BaseBO();
        DataSet ds = new DataSet();
        ds = baseBo.QueryDataSet(Sql);
        DataTable dt = ds.Tables[0];
        PagedDataSource pds = new PagedDataSource();
        pds.DataSource = dt.DefaultView;
        int spareRow = 0;

        if (pds.Count < 1)
        {
            for (int i = 0; i < GVSignOff.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GVSignOff.DataSource = pds;
            GVSignOff.DataBind();
        }
        else
        {
            this.GVSignOff.DataSource = pds;
            this.GVSignOff.DataBind();
            spareRow = GVSignOff.Rows.Count;
            for (int i = 0; i < GVSignOff.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GVSignOff.DataSource = pds;
            GVSignOff.DataBind();
        }

    }

    private void BindSignOn(string DeptID)
    {
        string Sql = @"select row_number() over(order by conshop.shopcode,transdate) '序号',conshop.shopcode '商铺编码',conshop.shopname '商铺名称',POSID as 'POS机号',TpUsrID as '收银员号',
                        case transtatus when 'SignOn' then '签到' when 'SignOff' then '签退' end as '交易类型',transdate as '时间',Transid 
                        from possign 
                        inner join conshop on (conshop.shopid=possign.shopid)
                        where bizdate='" + DateTime.Now.ToShortDateString() + "' and possign.storeid='" + DeptID + "'";

        BaseBO baseBo = new BaseBO();
        DataSet ds = new DataSet();
        ds = baseBo.QueryDataSet(Sql);
        DataTable dt = ds.Tables[0];
        PagedDataSource pds = new PagedDataSource();
        pds.DataSource = dt.DefaultView;
        int spareRow = 0;

        if (pds.Count < 1)
        {
            for (int i = 0; i < GVSignOn.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GVSignOn.DataSource = pds;
            GVSignOn.DataBind();
        }
        else
        {
            this.GVSignOn.DataSource = pds;
            this.GVSignOn.DataBind();
            spareRow = GVSignOn.Rows.Count;
            for (int i = 0; i < GVSignOn.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GVSignOn.DataSource = pds;
            GVSignOn.DataBind();
        }

    }


}

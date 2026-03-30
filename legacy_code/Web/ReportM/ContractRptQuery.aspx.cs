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
using Lease;
using Lease.Customer;
using Lease.Contract;
using Lease.PotBargain;
using RentableArea;

public partial class ReportM_ContractRptQuery : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            InitDDL();
        }

    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        String sCon = "%26sPara=''";
        sCon += "%26ContractCode=" + GetStrNull(this.txtContractID.Text);
        sCon += "%26CustCode=" + GetStrNull(this.txtCustCode.Text);
        sCon += "%26CustName=" + GetStrNull(this.txtCustName.Text);
        sCon += "%26BizMode=" + GetStrNull(this.ddlContractType.Text);
        sCon += "%26ContractStatus=" + GetStrNull(this.ddlContractStatus.Text);

        this.Response.Redirect("ShowReport.aspx?ReportName=/Mi/Base/ContractInfo.rpt" + sCon);
    }
    protected void BtnCel_Click(object sender, EventArgs e)
    {

    }

    /* 判断数据空值,返回默认值
     * 
     * 
     */
    private String GetStrNull(String s)
    {
        return s.Trim() == "" ? "-32766" : s;
    }
    /* 初始化下拉列表
     * 
     * 
     */
    private void InitDDL()
    {
        //绑定经营方式
        int[] contractType = Contract.GetBizModes();
        int s = contractType.Length + 1;
        ddlContractType.Items.Add(new ListItem("", ""));
        for (int i = 1; i < s; i++)
        {
            ddlContractType.Items.Add(new ListItem(Contract.GetBizModeDesc(contractType[i - 1]), contractType[i - 1].ToString()));
        }

        //绑定合同状态

        int[] contractStutas = Contract.GetContractTypeStatus();
        int k = contractStutas.Length + 1;
        ddlContractStatus.Items.Add(new ListItem("", ""));
        for (int j = 1; j < s; j++)
        {
            ddlContractStatus.Items.Add(new ListItem(Contract.GetContractTypeStatusDesc(contractStutas[j - 1]), contractStutas[j - 1].ToString()));
        }

        //绑定二级经营类别
        BaseBO baseBo = new BaseBO();
        baseBo.WhereClause = "TradeLevel = '" + TradeRelation.TRADELEVEL_STATUS_TWO + "'";
        Resultset rs = baseBo.Query(new TradeRelation());
        foreach (TradeRelation tradeDef in rs)
            ddlTradeID.Items.Add(new ListItem(tradeDef.TradeName, tradeDef.TradeID.ToString()));
    }
}

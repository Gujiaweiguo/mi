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

public partial class Invoice_InvAdj_InvAdjRoll : BasePage
{
    public string baseInfo = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_InvAdjRoll");
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        BaseTrans baseTrans = new BaseTrans();
        BaseBO baseBO = new BaseBO();
        DataTable dt = new DataTable();
        string strSql = "select distinct invid from invadj where adjstatus=3";
        DataSet ds = baseBO.QueryDataSet(strSql);
        dt = ds.Tables[0];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataTable dt1 = new DataTable();
                strSql = "select invadjid,invid from invadj where adjstatus=3 and invid='" + dt.Rows[i][0].ToString() + "' order by invadjid desc";
                DataSet ds1 = baseBO.QueryDataSet(strSql);
                dt1 = ds1.Tables[0];
                if (dt1.Rows.Count > 0)
                {
                    for (int l = 0; l < dt1.Rows.Count; l++)
                    {
                        DataTable dt2 = new DataTable();
                        strSql = "select invadjdetid,invdetailid,invadjdet.invadjid,invadjdet.adjamt,invadjdet.adjamtl from invadjdet inner join invadj on (invadj.invadjid=invadjdet.invadjid) where adjstatus=3 and invadjdet.invadjid='" + dt1.Rows[l][0].ToString() + "' order by invadjdetid desc";
                        DataSet ds2 = baseBO.QueryDataSet(strSql);
                        dt2 = ds2.Tables[0];
                        if (dt2.Rows.Count > 0)
                        {
                            for (int j = 0; j < dt2.Rows.Count; j++)
                            {
                                DataTable dt3 = new DataTable();
                                strSql = "select invadjdetid,invadjdet.invdetailid,invadjdet.invadjid,invadjdet.adjamt,invadjdet.adjamtl," +
                                         "   invadjdet.InvPayAmt,invadjdet.InvPayAmtL,invadjdet.InvActPayAmt,invadjdet.InvActPayAmtL" +
                                         "    from invadjdet inner join invadj on " +
                                         "  (invadj.invadjid=invadjdet.invadjid) " +
                                         "   inner join invoicedetail on (invadjdet.invdetailid=invoicedetail.invdetailid) " +
                                         " where adjstatus=3 and invadjdet.invadjdetid>'" + dt2.Rows[j][0].ToString() + "' and invadjdet.invdetailid='" + dt2.Rows[j][1].ToString() + "' ";
                                DataSet ds3 = baseBO.QueryDataSet(strSql);
                                dt3 = ds3.Tables[0];
                                if (dt3.Rows.Count > 0)
                                {
                                    baseTrans.BeginTrans();
                                    //费用原金额（InvPayAmt）、费用本币原金额（InvPayAmtL）、费用调整后金额（InvActPayAmt）、费用调整后本币金额（InvActPayAmtL）
                                    decimal InvActPayAmt = Convert.ToDecimal(dt3.Rows[0][5]);
                                    decimal InvActPayAmtL = Convert.ToDecimal(dt3.Rows[0][6]);
                                    decimal InvPayAmt = InvActPayAmt - Convert.ToDecimal(dt2.Rows[j][3]);
                                    decimal InvPayAmtL = InvActPayAmtL - Convert.ToDecimal(dt2.Rows[j][4]);
                                    string updatesql = "update invadjdet set InvPayAmt='" + InvPayAmt + "',InvPayAmtL='" + InvPayAmtL + "',InvActPayAmt='" + InvActPayAmt + "',InvActPayAmtL='" + InvActPayAmtL + "' where invadjdetid='" + dt2.Rows[j][0].ToString() + "' ";
                                    if (baseTrans.ExecuteUpdate(updatesql) < 0)
                                    {
                                        baseTrans.Rollback();
                                    }
                                    baseTrans.Commit();
                                }
                                else
                                {
                                    DataTable dt4 = new DataTable();
                                    strSql = "select invdetailid,invid,invpayamt,invpayamtl,invactpayamt,invactpayamtl from invoicedetail where invdetailid='" + dt2.Rows[j][1].ToString() + "'";
                                    DataSet ds4 = baseBO.QueryDataSet(strSql);
                                    dt4 = ds4.Tables[0];
                                    if (dt4.Rows.Count > 0)
                                    {
                                        for (int k = 0; k < dt4.Rows.Count; k++)
                                        {
                                            baseTrans.BeginTrans();
                                            //费用原金额（InvPayAmt）、费用本币原金额（InvPayAmtL）、费用调整后金额（InvActPayAmt）、费用调整后本币金额（InvActPayAmtL）
                                            decimal InvActPayAmt = Convert.ToDecimal(dt4.Rows[0][4]);
                                            decimal InvActPayAmtL = Convert.ToDecimal(dt4.Rows[0][5]);
                                            decimal InvPayAmt = InvActPayAmt - Convert.ToDecimal(dt2.Rows[j][3]);
                                            decimal InvPayAmtL = InvActPayAmtL - Convert.ToDecimal(dt2.Rows[j][4]);
                                            string updatesql = "update invadjdet set InvPayAmt='" + InvPayAmt + "',InvPayAmtL='" + InvPayAmtL + "',InvActPayAmt='" + InvActPayAmt + "',InvActPayAmtL='" + InvActPayAmtL + "' where invadjdetid='" + dt2.Rows[j][0].ToString() + "' ";
                                            if (baseTrans.ExecuteUpdate(updatesql) < 0)
                                            {
                                                baseTrans.Rollback();
                                            }
                                            baseTrans.Commit();
                                        }
                                    }
                                }


                            }
                        }
                    }
                }
            }
        
        }

    }
}

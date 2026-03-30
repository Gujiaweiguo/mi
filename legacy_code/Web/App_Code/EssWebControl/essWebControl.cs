using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web;
using BaseInfo.Store;
using Base.Biz;
using Base.DB;
using System.Data;
using System;
using System.Security.Permissions;
namespace EssWebControls
{
    /// <summary>
    ///查询条件列表
    /// </summary>
    [
        AspNetHostingPermission(SecurityAction.Demand,   //控件的安全等级
            Level = AspNetHostingPermissionLevel.Minimal),
        AspNetHostingPermission(SecurityAction.InheritanceDemand,
            Level = AspNetHostingPermissionLevel.Minimal),
        DefaultProperty("Text"),                //指定控件的默认属性
        ToolboxData("<{0}:SeachList id=\"SeachListID\" runat=\"server\"> </{0}:SeachList>")   //控件默认格式字符串
    ]
    public class SeachList : WebControl 
    {
        XmlDataSource xmlDataSource = null;
        string id = "";

        [Browsable(true)]
        [Description("设置和返回XML数据源")]
        [Category("Appearance")]
        public XmlDataSource DataSourceID
        {
            set { this.xmlDataSource = value; }
            get { return this.xmlDataSource; }
        }

        [Browsable(true)]
        [Description("设置控件ID")]
        [Category("Appearance")]
        public virtual string ID
        {
            set { this.id = value; }
            get { return this.id; }
        }
        /// <summary>
        /// 输出html
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderControl(HtmlTextWriter writer)
        {
            writer.Write("");
            base.RenderControl(writer);
        }


    }

    /// <summary>
    /// POS机状态
    /// </summary>
    [DefaultProperty("ShopName"),
    ToolboxData("<{0}:ShopPOS POSID=\"POS机ID\" runat=\"server\"> </{0}:ShopPOS>")
    ]
    public class ConShopPOS : WebControl
    {
        //使用本控件的页面加入<link href="CSS/Pos.css" rel="stylesheet" type="text/css" />
        //控件尺寸大小：230*60
        private string shopid = "";//商铺ID
        private string posid = "";  //POS编号

        private string shopname = ""; //商铺名称        
        private string posstatus = "";//POS当前状态
        private string shoptel = "";  //商铺电话


        [Browsable(true)]
        [Description("设置和返回商铺POS编号")]
        [Category("Appearance")]
        public virtual string POSID
        {
            get { return this.posid; }
            set { this.posid = value; }
        }

        [Browsable(true)]
        [Description("设置和返回商铺ID")]
        [Category("Appearance")]
        public virtual string ShopID
        {
            get { return this.shopid; }
            set { this.shopid = value; }
        }



        protected override void Render(HtmlTextWriter writer)
        {
            string strStatus = "";
            string strSql = "select conshop.shopName,conshop.tel as shoptel,posstatus.posstatus" +
                            " from posstatus inner join conshop on posstatus.shopid=conshop.shopid" +
                            " where conshop.shopid=" + shopid + " and posstatus.posid='" + posid + "'";
            BaseBO baseBO = new BaseBO();
            DataSet posDt = baseBO.QueryDataSet(strSql);

            if (posDt.Tables[0].Rows.Count == 1)
            {
                shopname = posDt.Tables[0].Rows[0]["ShopName"].ToString();
                shoptel = posDt.Tables[0].Rows[0]["ShopTel"].ToString();
                posstatus = posDt.Tables[0].Rows[0]["PosStatus"].ToString();

                switch (posstatus)
                {
                    case "0":
                        strStatus = "断网中";
                        break;
                    case "1":
                        strStatus = "联机未签到";
                        break;
                    case "2":
                        strStatus = "联机签到";
                        break;
                    case "3":
                        strStatus = "联机休息中";
                        break;
                    default:
                        strStatus = "状态未知";
                        break;
                }
            }
            #region 新
            writer.Write("<table  width='150px' height='120px' background='../CSS/POSimages/Tbl_bg3.gif' class='text_b'>");
            writer.Write("<tr>");
            writer.Write("<td align='right' valign='middle' width='65px'>");
            writer.Write("<img src='../CSS/POSimages/PosStatus" + posstatus.ToString() + ".gif' width='60' height='55'" + " title='" + strStatus + "'/></td>");
            writer.Write("<td width='75px'>");
            writer.Write("</td>");
            writer.Write("</tr>");
            writer.Write("<tr>");
            writer.Write("<td width='65px' align='right'>商铺名称:</td>");
            writer.Write("<td width='75px'align='left'>" + shopname + "</td>");
            writer.Write("</tr>");
            writer.Write("<tr>");
            writer.Write("<td width='65px' align='right'>商铺电话:</td>");
            writer.Write("<td width='75px'align='left'>" + shoptel + "</td>");
            writer.Write("</tr>");
            writer.Write("<tr>");
            writer.Write("<td width='65px' align='right'>POS编号:</td>");
            writer.Write("<td width='75px'align='left'>" + posid.ToString() + "</td>");
            writer.Write("</tr>");
            writer.Write("</table>");
            #endregion
            base.Render(writer);

        }
    }
}

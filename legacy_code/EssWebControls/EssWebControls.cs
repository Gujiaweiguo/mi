using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Web;
using System.Security.Permissions;
using System.Web.UI;
using System;
using Base.Biz;
using System.Data;
namespace EssWebControls
{
    [
       AspNetHostingPermission(SecurityAction.Demand,
           Level = AspNetHostingPermissionLevel.Minimal),
       AspNetHostingPermission(SecurityAction.InheritanceDemand,
           Level = AspNetHostingPermissionLevel.Minimal),
       DefaultProperty("Text"),
       ToolboxData("<{0}:WelcomeLabel runat=\"server\"> </{0}:WelcomeLabel>")
       ]
    public class WelcomeLabel : WebControl
    {
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue(""),
        Description("The welcome message text."),
        Localizable(true)
        ]
        public virtual string Text
        {
            get
            {
                string s = (string)ViewState["Text"];
                return (s == null) ? String.Empty : s;
            }
            set
            {
                ViewState["Text"] = value;
            }
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            writer.WriteEncodedText(Text);
            if (Context != null)
            {
                string s = Context.User.Identity.Name;
                if (s != null && s != String.Empty)
                {
                    string[] split = s.Split('\\');
                    int n = split.Length - 1;
                    if (split[n] != String.Empty)
                    {
                        writer.Write(", ");
                        writer.Write(split[n]);
                    }
                }
            }
            writer.Write("!");
        }
    }

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
        public SeachList()
        {

        }
    }

    /// <summary>
    /// 
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


        [BrowsableAttribute(true)]
        [DescriptionAttribute("设置和返回商铺POS编号")]
        [DefaultValueAttribute("POS编号")]
        [CategoryAttribute("Appearance")]
        public virtual string POSID
        {
            get { return this.posid; }
            set { this.posid = value; }
        }

        [BrowsableAttribute(true)]
        [DescriptionAttribute("设置和返回商铺ID")]
        [DefaultValueAttribute("商铺ID")]
        [CategoryAttribute("Appearance")]
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

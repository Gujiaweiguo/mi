<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShopSellReceipt.aspx.cs" Inherits="Sell_ShopSellReceipt" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%=strBaseinfo%></title>
        <link href="../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Tabbar/css/longCss/webtab.css" rel="stylesheet" type="text/css"/>
    <link href="../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link rel="StyleSheet" href="../App_Themes/nlstree/nlstree-basic.css" type="text/css" />
    <link rel="StyleSheet" href="../App_Themes/nlstree/nlsctxmenu.css" type="text/css" />
    
    <script type="text/javascript"  src="../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript" src="../JavaScript/Common.js"> </script>
	<script language="javascript" type="text/javascript" src="../JavaScript/TabTools.js"></script>
    <script type="text/javascript" src="../JavaScript/Calendar.js" charset="gb2312"></script>
    
    <script type="text/javascript" src="../App_Themes/nlstree/nlstree.js"></script>
    <script type="text/javascript" src="../App_Themes/nlstree/nlsctxmenu.js"></script>
    
	<script type="text/javascript">
	function Load()
    {
        addTabTool("<%=strBaseinfo %>,Sell/ShopSellReceipt.aspx");
        loadTitle();
    }
    function BtnUp( p )
    {
	    var t = String(p)
	    var l = t.substring(3,15); 
	    document.getElementById( p ).style.backgroundImage = 'url(../App_Themes/CSS/BtnImage/btn_' + l + '.gif)';
    }
    function BtnOver( p )
    {
	    var t = String(p)
	    var l = t.substring(3,15); 
	    document.getElementById( p ).style.backgroundImage = 'url(../App_Themes/CSS/BtnImage/over_' + l + '.gif)';
    }
    function CheckCash()
    {
        if(isDouble(document.all.txtCash.value)==false)
        {
            alert("please input number.");
            document.all.txtCash.focus();
            return false;
        }
    }
    function CheckBankCard()
    {
        if(isDouble(document.all.txtBankcard.value)==false)
        {
            alert("please input number.");
            document.all.txtBankcard.focus();       
            return false;
        } 
    }
    var tabbar ;
    function treearray()
    {
        var t = new NlsTree('MyTree');
        var treestr =document.getElementById("depttxt").value;
        var treearr = new Array();
        var n=0;
        var id;
        var pid;
        var name;
        var num = treestr.split("^");
        for(var i=0;i<num.length-1;i++)
        {
            if(num[i]!="")
            {
               var treenode = num[i].split("|");
                for(var j=0;j<treenode.length;j++)
                {
                    id=treenode[0];
                    pid=treenode[1];
                    name=treenode[2];
                }
              
                t.add(id, pid, name, "", "", true);
            }
        }
        t.opt.sort='no';
        t.opt.enbScroll=true;
        t.opt.height="350px";
        t.opt.width="220px";
        t.opt.trg="mainFrame";
        t.opt.oneExp=true;
        t.opt.oneClick=true;
        
        t.render("treeview");
        
        t.treeOnClick = ev_click;   
        t.collapseAll();
        if(document.form1.selectdeptid.value!="")
        {
            t.expandNode(document.form1.selectdeptid.value);
            t.selectNodeById(document.form1.selectdeptid.value);
        }
        /*document.getElementById("lblTotalNum").style.display="none";
        document.getElementById("lblCurrent").style.display="none";
        */
    }
    function ev_click(e, id)
    {
        document.form1.deptid.value=id;
        document.form1.selectdeptid.value=id;
        document.form1.treeClick.click();
    } 
    </script>
</head>
<body style="margin:0px" onload ="Load();treearray();">
    <form id="form1" runat="server" >
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
            <asp:HiddenField id="depttxt" runat="server"></asp:HiddenField>
            <asp:HiddenField id="deptid" runat="server" ></asp:HiddenField>
            <asp:HiddenField ID="selectdeptid" runat="server" />
              <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 4px">
                <tr>
                    <td align="left" class="tdTopRightBackColor" style="vertical-align: top; width: 356px;
                        height: 22px; text-align: left">
                        <img class="imageLeftBack" src="" style="width: 7px; height: 22px" />
                        <asp:Label ID="Label31" runat="server" Text="<%$ Resources:BaseInfo,Menu_ShopSellReceipt %>"
                            Width="295px"></asp:Label></td>
                    <td align="left" class="tdTopRightBackColor" style="width: 562px; height: 22px">
                    </td>
                    <td class="tdTopRightBackColor" style="vertical-align: top; height: 22px;
                        text-align: right; width: 115px;" valign="top">
                        <img align="right" class="imageRightBack" src="" style="width: 7px; height: 22px" /></td>
                </tr>
                </table> 
                <table style="width: 100%" class="tdBackColor">
                    <tr>
                        <td style="height: 17px">
                            <table border="0" cellpadding="0" cellspacing="0" style="width:98%; height: 1px;
                                text-align: center">
                                <tr>
                                    <td style="left: 15px; width: 324px; position: relative; height: 1px; background-color: #738495">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 17px">
                            <table style="width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Panel ID="Panel1" runat="server" BackColor="White" BorderStyle="Inset" 
                                            BorderWidth="1px" Height="370px" ScrollBars="Auto" Width="230px">
                                            <table>
                                                <tr>
                                                    <td ID="treeview" style="height: 250px; width: 160px; text-align:left;" 
                                                        valign="top">
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                    <td align="center" colspan="3">
                                        <asp:GridView ID="ShopView" runat="server" AllowPaging="True" 
                                            AutoGenerateColumns="False" BackColor="White" BorderStyle="Inset" 
                                            BorderWidth="1px" Height="120px" 
                                            OnPageIndexChanging="ShopView_PageIndexChanging" 
                                            OnRowDataBound="ShopView_RowDataBound" PageSize="15" Width="98%">
                                            <Columns>
                                                <asp:BoundField DataField="ShopID">
                                                    <ItemStyle CssClass="hidden" />
                                                    <HeaderStyle CssClass="hidden" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ShopName" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="<%$ Resources:BaseInfo,PotShop_lblPotShopName %>">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="<%$ Resources:BaseInfo,ShopSell_InputDate %>">
                                                    <ItemTemplate>
                                                        &nbsp;<asp:TextBox ID="txtDate" runat="server" CssClass="ipt35px" Font-Size="9pt" 
                                                            onclick="calendar()" Text='<%# DataBinder.Eval(Container, "DataItem.Date")%>' 
                                                            Width="100px"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                    <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="<%$ Resources:BaseInfo,InvoicePay_INVPAYTYPE_CASH %>">
                                                    <ItemTemplate>
                                                        &nbsp;<asp:TextBox ID="txtCash" runat="server" CssClass="ipt35px" Font-Size="9pt" 
                                                            Text='<%# DataBinder.Eval(Container, "DataItem.Cash")%>' Width="100px"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                    <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,InvoicePay_INVPAYTYPE_BANK_CARD %>">
                                                    <ItemTemplate>
                                                        &nbsp;<asp:TextBox ID="txtBankCard" runat="server" CssClass="ipt35px" 
                                                            Font-Size="9pt" ReadOnly="true" 
                                                            Text='<%# DataBinder.Eval(Container, "DataItem.BankCard")%>' Width="100px"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle BorderColor="#E1E0B2" HorizontalAlign="Left" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" 
                                                        HorizontalAlign="Left" />
                                                    <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                </asp:TemplateField>
                                                
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        &nbsp;<asp:TextBox ID="txtShopID" runat="server" CssClass="ipt35px" 
                                                            Font-Size="9pt" ReadOnly="true" 
                                                            Text='<%# DataBinder.Eval(Container, "DataItem.ShopID")%>' Width="100px"></asp:TextBox>
                                                    </ItemTemplate>
                                                   <ItemStyle CssClass="hidden" />
                                                    <HeaderStyle CssClass="hidden" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <FooterStyle BackColor="Red" ForeColor="#000066" />
                                            <RowStyle Font-Overline="False" Font-Size="10pt" ForeColor="Black" 
                                                Height="20px" HorizontalAlign="Left" />
                                            <SelectedRowStyle BackColor="#FFFFCD" Font-Bold="False" ForeColor="Black" />
                                            <HeaderStyle BackColor="#E1E0B2" Font-Bold="False" Height="10px" />
                                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Right" />
                                            <PagerTemplate>
                                                <asp:LinkButton ID="LinkButtonFirstPage" runat="server" CommandArgument="First" 
                                                    CommandName="Page" Font-Size="Small" 
                                                    Visible="<%# ((GridView)Container.NamingContainer).PageIndex != 0 %>">首页</asp:LinkButton>
                                                <asp:LinkButton ID="LinkButtonPreviousPage" runat="server" 
                                                    CommandArgument="Prev" CommandName="Page" Font-Size="Small" 
                                                    Visible="<%# ((GridView)Container.NamingContainer).PageIndex != 0 %>">上一页</asp:LinkButton>
                                                <asp:LinkButton ID="LinkButtonNextPage" runat="server" CommandArgument="Next" 
                                                    CommandName="Page" Font-Size="Small" 
                                                    Visible="<%# ((GridView)Container.NamingContainer).PageIndex != ((GridView)Container.NamingContainer).PageCount - 1 %>">下一页</asp:LinkButton>
                                                <asp:LinkButton ID="LinkButtonLastPage" runat="server" CommandArgument="Last" 
                                                    CommandName="Page" Font-Size="Small" 
                                                    Visible="<%# ((GridView)Container.NamingContainer).PageIndex != ((GridView)Container.NamingContainer).PageCount - 1 %>">尾页</asp:LinkButton>
                                                <asp:TextBox ID="txtNewPageIndex" runat="server" 
                                                    text="<%# ((GridView)Container.Parent.Parent).PageIndex + 1 %>" width="20px" />
                                                /
                                                <asp:Label ID="LabelPageCount" runat="server" 
                                                    Text="<%# ((GridView)Container.NamingContainer).PageCount %>"></asp:Label>
                                                <asp:LinkButton ID="btnGo" runat="server" causesvalidation="False" 
                                                    commandargument="-1" commandname="Page" Font-Size="Small" text="GO" />
                                            </PagerTemplate>
                                            <PagerSettings Mode="NextPreviousFirstLast" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10%" align="right">
                                        <asp:Button ID="treeClick" runat="server" CssClass="buttonHidden" 
                                            OnClick="treeClick_Click" Width="1px" />
                                        <asp:Button ID="btnAdd" runat="server" CssClass="buttonAdd" Enabled="False" 
                                             onmouseout="BtnUp(this.id);" 
                                            onmouseover="BtnOver(this.id);" onmouseup="BtnUp(this.id);" 
                                            Text="<%$ Resources:BaseInfo,Dept_TitleAdd %>" onclick="btnAdd_Click" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td style="width: 10%">
                                    </td>
                                    <td style="width: 20%">
                                    </td>
                                    <td align="right" height="22">
                        <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" 
                                Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" OnClick="btnSave_Click" 
                                            onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" 
                                            onmouseup="BtnUp(this.id);" Enabled="False"/>&nbsp;&nbsp;
                                <asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel" 
                                Text="<%$ Resources:BaseInfo,User_btnCancel %>" OnClick="btnQuit_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                &nbsp;
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>

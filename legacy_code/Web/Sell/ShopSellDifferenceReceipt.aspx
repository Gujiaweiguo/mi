<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShopSellDifferenceReceipt.aspx.cs" Inherits="Sell_ShopSellDifferenceReceipt" %>

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
        addTabTool("<%=strFresh %>,Sell/ShopSellDifferenceReceipt.aspx");
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
    	     //验证数字类型
        function textleave(form1)
        {   
            var key=window.event.keyCode;
            if(key==8 || key==46 || key==48 || key==49 || key==50 || key==51 || key==52 || key==53 || key==54 || key==55 || key==56 ||
               key==57 || key==190 || key == 96 || key == 97 || key == 98 || key == 99 || key == 100 || key == 101 || key == 102 ||
               key == 103 || key == 104 || key == 105 || key == 110)
            {
		        window.event.returnValue =true;
	        }else
	        {
		        window.event.returnValue =false;
	        }
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
                        <asp:Label ID="Label31" runat="server" Text="<%$ Resources:BaseInfo,Menu_ShopSellDifferenceReceipt %>"
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
                                text-align: left">
                                <tr>
                                    <td valign="middle">
                                        <asp:Label ID="Label32" runat="server" Text="<%$ Resources:BaseInfo,Sell_SaleBeginDate %>"></asp:Label>
                                        &nbsp;<asp:TextBox ID="TextBox1" runat="server" CssClass="ipt160px" onclick="calendar()"></asp:TextBox>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Label ID="Label33" runat="server" 
                                            Text="<%$ Resources:BaseInfo,Sell_SaleEndDate %>"></asp:Label>
                                        <asp:TextBox ID="TextBox2" runat="server" CssClass="ipt160px" 
                                            onclick="calendar()"></asp:TextBox>
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
                                    <div style="overflow-y: scroll; height: 370px">
                                        <asp:GridView ID="ShopView" runat="server" 
                                            AutoGenerateColumns="False" BackColor="White" BorderStyle="Inset" 
                                            BorderWidth="1px" Height="120px" 
                                            OnRowDataBound="ShopView_RowDataBound" PageSize="15" Width="97%" 
                                            AllowSorting="True" onsorting="ShopView_Sorting">
                                            <Columns>
                                                <asp:BoundField DataField="ShopID">
                                                    <ItemStyle CssClass="hidden" />
                                                    <HeaderStyle CssClass="hidden" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="trade" SortExpression="trade" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="<%$ Resources:BaseInfo,LeaseholdContract_labTradeID %>">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ShopName" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="<%$ Resources:BaseInfo,PotShop_lblPotShopName %>">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Date" SortExpression="Date" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="<%$ Resources:BaseInfo,ShopSell_InputDate %>">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                
                                                <asp:BoundField DataField="paidamt" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="<%$ Resources:BaseInfo,ConLease_labSellCount %>">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Cash">
                                                    <ItemStyle CssClass="hidden" />
                                                    <HeaderStyle CssClass="hidden" />
                                                </asp:BoundField>
                                                <asp:TemplateField  HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="<%$ Resources:BaseInfo,InvoicePay_INVPAYTYPE_CASH %>">
                                                    <ItemTemplate>
                                                        &nbsp;<asp:TextBox ID="txtCash" onkeydown="textleave()" runat="server" CssClass="ipt35px" Font-Size="9pt" 
                                                            Text="" Width="100px"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
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
                                        </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10%" align="right">
                                        <asp:Button ID="ButAdd" runat="server" CssClass="buttonAdd" Enabled="False" 
                                            onclick="ButAdd_Click" onmouseout="BtnUp(this.id);" 
                                            onmouseover="BtnOver(this.id);" onmouseup="BtnUp(this.id);" Text="<%$ Resources:BaseInfo,Btn_ClearAdd %>" />
                                        <asp:Button ID="treeClick" runat="server" CssClass="buttonHidden" 
                                            OnClick="treeClick_Click" Width="1px" />
                                        <asp:Button ID="btnAdd" runat="server" CssClass="buttonAdd" Enabled="False" 
                                             onmouseout="BtnUp(this.id);" 
                                            onmouseover="BtnOver(this.id);" onmouseup="BtnUp(this.id);" 
                                            Text="<%$ Resources:BaseInfo,Dept_TitleAdd %>" onclick="btnAdd_Click" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td style="width: 10%">
                                        &nbsp;</td>
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

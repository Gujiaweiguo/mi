<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SellInfoQuery.aspx.cs" Inherits="Sell_SellInfoQuery" %>

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
        addTabTool("<%=fresh %>,Sell/SellInfoQuery.aspx");
        loadTitle();
        document.getElementById("PalStore").style.display = 'none';    
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
     var nodeid="";
     var nodeValue = "";//名称
    var treearr = new Array();
    var n=0;
    var id;
    var pid;
    var name;
    var chkstrtus;
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
                        chkstrtus=treenode[3];
                    }
                    if(chkstrtus==undefined)
                    {
                        t.add(id, pid, name, "", "", true,false);
                    }
                    if(chkstrtus==0)
                    {
                        t.add(id, pid, name, "", "", true,false);
                    }
                    else if(chkstrtus==1)
                    {
                        t.add(id, pid, name, "", "", true,true);
                        nodeid+=id+ ',';
                    }
                }
            }
            
            document.form1.deptid.value = nodeid;
           
            t.opt.sort='no';
            t.opt.enbScroll=true;
            t.opt.height="350px";
            t.opt.width="220px";
            t.opt.trg="mainFrame";
            t.opt.oneExp=true;
            t.opt.check = true;
            t.opt.checkIncSub = true; //default is true.
            t.opt.checkParents = false; //default is false.
            t.opt.checkOnLeaf = false; //default is false
            t.treeOnCheck = enableCheckbox;
            
            t.render("treeview");  
            t.collapseAll();
	    
}
function enableCheckbox(id,v)
{
    var str;
    str=document.form1.deptid.value;
    if(str.indexOf(id+',',0)!=-1)
    {
        document.form1.deptid.value =str.replace(id+',',"");
    }
    else
    {
        str+=id+',';
        document.form1.deptid.value =str;
    }
} 
function ReturnValue()
{
 var a = document.form1.deptid.value+"|"+document.form1.txtUnitCode.value+"|"+document.form1.txtStore.value;
    window.returnValue = a;
    window.close();
}

    
    function showItem()
    {
        document.getElementById("PalStore").style.display = 'block';
    }
    function HidItem()
    {
        document.getElementById("PalStore").style.display = 'none';    
    }
    function Ta1Hid()
    {
        document.getElementById("Panel1").style.display = 'none';      
        document.getElementById("btnAdd").style.display = 'none';      
        document.getElementById("Button2").style.display = '';      
        document.getElementById("Button1").style.display = 'none';      
    }
    function Ta1Show()
    {
        document.getElementById("Panel1").style.display = '';      
        document.getElementById("btnAdd").style.display = '';      
        document.getElementById("Button1").style.display = '';      
        document.getElementById("Button2").style.display = 'none';  
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
            <asp:HiddenField ID="deptValue" runat="server" />
            <asp:TextBox ID="txtUnitID" runat="server" Width="0px" CssClass="hidden"></asp:TextBox>
            <asp:TextBox ID="txtUnitCode" runat="server" Width="0px" CssClass="hidden"></asp:TextBox>
        <asp:TextBox ID="txtStore" runat="server" CssClass="hidden" Width="0px"></asp:TextBox>
              <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 4px">
                <tr>
                    <td align="left" class="tdTopRightBackColor" style="vertical-align: top; width: 356px;
                        height: 22px; text-align: left">
                        <img class="imageLeftBack" src="" style="width: 7px; height: 22px" />
                        <asp:Label ID="Label31" runat="server" Text="<%$ Resources:BaseInfo,Menu_SellInfoQuery %>"
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
                                        <asp:RadioButton ID="RbtDetail" runat="server" AutoPostBack="True" 
                                            Checked="True" GroupName="AA" oncheckedchanged="RbtDetail_CheckedChanged" 
                                            Text="明细" />
                                        <asp:RadioButton ID="RbtDay" runat="server" GroupName="AA" 
                                            Text="日报" AutoPostBack="True" oncheckedchanged="RbtDay_CheckedChanged" />
                                        <asp:RadioButton ID="RbtMtn" runat="server" GroupName="AA" Text="月报" 
                                            AutoPostBack="True" oncheckedchanged="RbtMtn_CheckedChanged" />
                                        <asp:RadioButton ID="RbtTotal" runat="server" AutoPostBack="True" GroupName="AA"
                                            Text="汇总" OnCheckedChanged="RbtTotal_CheckedChanged" />
                                    </td>
                                    <td valign="middle" align="right" style="position:relative; right:188px;">
                                        &nbsp;&nbsp;<asp:Label ID="Label32" runat="server" Text="<%$ Resources:BaseInfo,Sell_SaleBeginDate %>"></asp:Label>
                                        &nbsp;<asp:TextBox ID="TextBox1" runat="server" CssClass="ipt60px" 
                                            onclick="calendar()" Height="20px" Width="120px"></asp:TextBox>
                                        &nbsp;<asp:Label ID="Label33" runat="server" 
                                            Text="<%$ Resources:BaseInfo,Sell_SaleEndDate %>"></asp:Label>
                                        <asp:TextBox ID="TextBox2" runat="server" CssClass="ipt60px" 
                                            onclick="calendar()" Height="20px" Width="120px"></asp:TextBox>
                                        &nbsp;
                                            <table ID="Item" style="position:absolute; right:-175px; top:20px; text-align:left;">
                                            <tr>
                                            <td>
                                            <asp:Panel ID="PalStore" runat="server" onmouseover="showItem()" onmouseout="HidItem()">                                        
                                                <asp:CheckBoxList ID="CBList" runat="server" BackColor="White" Width="123px" 
                                                    AutoPostBack="True" onselectedindexchanged="CBList_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                        </td>
                                        </tr>
                                        </table>
                                        <table style="position:absolute; top:-3px;">
                                        <tr>
                                        <td>
                                            <asp:Label ID="Label34" runat="server" Text="<%$ Resources:BaseInfo,Sell_QueryItem %>"></asp:Label>                                            
                                            <asp:TextBox ID="TextBox3" runat="server" CssClass="ipt60px" onclick="showItem()" Height="20px" Width="120px"></asp:TextBox>
                                        </td>
                                        </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 17px;width:100%;">
                            <table style="width:100%">
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
                                    <td style="width:0px">
                                        <input id="Button1" type="button" value="<" style="width:10px; height:120px; font-size:xx-small" onclick="Ta1Hid()"/>
                                        <input id="Button2" type="button" value=">" style="width:10px; height:120px; font-size:xx-small; display:none" onclick="Ta1Show()"/>
                                    </td>
                                    <td align="center" colspan="3" style="width:100%">
                                    <div style="overflow-y: scroll; overflow-x: scroll; height: 370px; width: 100%">
                                        <asp:GridView ID="ShopView" runat="server" 
                                            AutoGenerateColumns="False" BackColor="White" BorderStyle="Inset" 
                                            BorderWidth="1px" Height="120px" 
                                            OnRowDataBound="ShopView_RowDataBound" PageSize="15" Width="120%" 
                                            AllowSorting="True">
                                            <Columns>
                                                <asp:BoundField DataField="" HeaderStyle-HorizontalAlign="Center" 
                                                    HeaderText="<%$ Resources:BaseInfo,WrkFlw_Sequence %>" ItemStyle-Width="30">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="storename" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="<%$ Resources:BaseInfo,PotCustomer_BusinessItem %>">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="contractcode" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="<%$ Resources:BaseInfo,AdBoard_lblContractID %>">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="custshortname" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="<%$ Resources:BaseInfo,PotCustomer_lblCustName %>">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="shopcode" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="<%$ Resources:BaseInfo,LeaseholdContract_labShopCode %>">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                </asp:BoundField>                                                
                                                <asp:BoundField DataField="shopname" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="<%$ Resources:BaseInfo,PotShop_lblPotShopName %>">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                </asp:BoundField>  
                                                <asp:BoundField DataField="shoptypename" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="<%$ Resources:BaseInfo,PotShop_lblShopType %>">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="tradename" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="<%$ Resources:BaseInfo,RentableArea_lblTradeRelation %>">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="brandname" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="<%$ Resources:BaseInfo,Rpt_Brand %>">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="bizdate" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="<%$ Resources:BaseInfo,Rpt_Date %>">
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
                                                <asp:BoundField DataField="skudesc" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="<%$ Resources:BaseInfo,Rpt_SkuDesc %>">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                </asp:BoundField>  
                                                <asp:BoundField DataField="datasource" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="<%$ Resources:BaseInfo,Lease_lblPayInDataSource %>">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                </asp:BoundField>   
                                                <asp:BoundField DataField="innum" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="<%$ Resources:BaseInfo,RptSalesTraffic_Traffic %>">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                </asp:BoundField>                                 
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
                                    <td align="right">
                                        <asp:Button ID="treeClick" runat="server" CssClass="buttonHidden" 
                                            OnClick="treeClick_Click" Width="1px" />
                                        <asp:Button ID="btnAdd" runat="server" CssClass="buttonAdd" Enabled="True" 
                                             onmouseout="BtnUp(this.id);" 
                                            onmouseover="BtnOver(this.id);" onmouseup="BtnUp(this.id);" 
                                            Text="<%$ Resources:BaseInfo,Tab_Query %>" onclick="btnAdd_Click" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td>
                                    </td>
                                    <td style="width: 20%">
                                        </td>
                                    <td align="right" height="22">
                                        <asp:Button ID="BtnSave" runat="server" CssClass="buttonSave" 
                                            Text="<%$ Resources:BaseInfo,Role_lblSubAuthExport %>" 
                                            onclick="BtnSave_Click" />
                                        &nbsp;&nbsp; &nbsp;
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
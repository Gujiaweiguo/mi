<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QueryLeaseDetail.aspx.cs" Inherits="Lease_QueryLeaseDetail" %>

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
        addTabTool("<%=fresh %>,Lease/QueryLeaseDetail.aspx");
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
            t.opt.width="250px";
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
                        <asp:Label ID="Label31" runat="server" Text="<%$ Resources:BaseInfo,Menu_QueryLeaseDetail %>"
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
                                        <asp:CheckBox ID="Chbzc" runat="server" Text="<%$ Resources:Parameter,CONTRACTSTATUS_TYPE_INGEAR %>" Checked="true"/>
                                        <asp:CheckBox ID="Chbcg" runat="server" Text="<%$ Resources:Parameter,CONTRACTSTATUS_TYPE_TEMP %>"/>
                                        <asp:CheckBox ID="Chbdq" runat="server" Text="<%$ Resources:Parameter,CONTRACTSTATUS_TYPE_ATTREM %>"/>
                                        <asp:CheckBox ID="Chbzz" runat="server" Text="<%$ Resources:Parameter,CONTRACTSTATUS_TYPE_END %>"/>
                                        <asp:Button ID="btnFresh" runat="server" OnClick="Button1_Click" Text="刷新树" /></td>
                                    <td align="right">
                                        &nbsp;
                                        <asp:CheckBox ID="CheckBox3" runat="server" Text="<%$ Resources:BaseInfo,Lease_lbBalance %>"/>
                                        &nbsp; &nbsp;&nbsp;
                                        <asp:Label ID="Label1" runat="server" Text="费用日期"></asp:Label>
                                        <asp:TextBox ID="txtChargeDate" runat="server" CssClass="ipt150px" onclick="calendar()"></asp:TextBox></td>
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
                                            BorderWidth="1px" Height="370px" ScrollBars="Auto" Width="260px">
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
                                        <div style="overflow-y: scroll; overflow-x: scroll; height: 370px; width: 100%">
                                        <asp:GridView ID="ShopView" runat="server" 
                                            AutoGenerateColumns="False" BackColor="White" BorderStyle="Inset" 
                                            BorderWidth="1px" Height="120px" 
                                            OnRowDataBound="ShopView_RowDataBound" PageSize="15" Width="250%" 
                                            AllowSorting="True">
                                            <Columns>
                                                <asp:BoundField DataField="storename" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="<%$ Resources:BaseInfo,PotCustomer_BusinessItem %>">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="floorname" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="<%$ Resources:BaseInfo,Floors %>">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="shopcode" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="<%$ Resources:BaseInfo,Lease_lblShopCode %>">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="tradename" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="<%$ Resources:BaseInfo,LeaseholdContract_labTradeID %>">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                </asp:BoundField>                                                
                                                <asp:BoundField DataField="custcode" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="<%$ Resources:BaseInfo,PotCustomer_lblCustCode %>">
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
                                                <asp:BoundField DataField="brandname" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="<%$ Resources:BaseInfo,Rpt_Brand %>">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="norentdays" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="<%$ Resources:BaseInfo,Lease_lblNorentDays %>">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="norentcondition" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="免租条件">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="shopstartdate" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="<%$ Resources:BaseInfo,LeaseholdContract_labChargeStartDate %>">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                </asp:BoundField>        
                                                <asp:BoundField DataField="shopenddate" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="<%$ Resources:BaseInfo,PotShop_lblShopEndDate %>">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                </asp:BoundField>                  
                                                <asp:BoundField DataField="单位租金" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="<%$ Resources:BaseInfo,ConLease_labUnitHire %>">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="月租金" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="<%$ Resources:BaseInfo,ConLease_rabMonthHire %>">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="年租金" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="年租金">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                </asp:BoundField>          
                                                <asp:BoundField DataField="推广费" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="推广费">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="物业费" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="物业费">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="履约保证金" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="履约保证金">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="POS机押金" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="POS机押金">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="POS租赁费" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="POS租赁费">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="营业员押金" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="营业员押金">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="电费押金" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="电费押金">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="人员培训费" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="人员培训费">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="钥匙押金" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="钥匙押金">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="胸卡押金" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="胸卡押金">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="信用卡承担" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="信用卡承担">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Vip卡承担" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="Vip卡承担">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="年节费" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="年节费">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="水费" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="水费">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="电费" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="电费">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                        CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="燃气费" HeaderStyle-HorizontalAlign="Left" 
                                                    HeaderText="燃气费">
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
                                    <td style="width: 10%" align="right">
                                        <asp:Button ID="treeClick" runat="server" CssClass="buttonHidden" 
                                            OnClick="treeClick_Click" Width="1px" />
                                        <asp:Button ID="btnAdd" runat="server" CssClass="buttonAdd" 
                                             onmouseout="BtnUp(this.id);" 
                                            onmouseover="BtnOver(this.id);" onmouseup="BtnUp(this.id);" 
                                            Text="<%$ Resources:BaseInfo,Tab_Query %>" onclick="btnAdd_Click" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td style="width: 10%">
                                    </td>
                                    <td style="width: 20%">
                                    </td>
                                    <td align="right" height="22">
                                        <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" OnClick="btnSave_Click"
                                            Text="导出" />
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
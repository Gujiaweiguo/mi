<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PosStatus.aspx.cs" Inherits="Sell_PosStatus" %>

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
        addTabTool("<%=strFresh %>,Sell/PosStatus.aspx");
        loadTitle();
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
        t.opt.enbScroll = true;
        t.opt.height = "420";
        t.opt.width = "220px";
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
    function TableClick(num) {
        var i = 0;
        for (i = 1; i < 6; i++) {
            document.getElementById("td" + i).style.display = "none";
            document.getElementById("tt" + i).style.background = "#E1E0B2";
        }
        document.getElementById("td" + num).style.display = "block";
        document.getElementById("tt" + num).style.background = "LightGoldenrodYellow";
        document.form1.tdNum.value = num;
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
            <asp:HiddenField id="tdNum" runat="server" ></asp:HiddenField>
              <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 4px">
                <tr>
                    <td align="left" class="tdTopRightBackColor" style="vertical-align: top; width: 356px;
                        height: 22px; text-align: left">
                        <img class="imageLeftBack" src="" style="width: 7px; height: 22px" />
                        <asp:Label ID="Label31" runat="server" Text="<%$ Resources:BaseInfo,Menu_PosStatusInfo %>"
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
                                    <table><tr>
                                    <td id="tt1" onclick="TableClick('1')" style="background-color:LightGoldenrodYellow; cursor:hand">POS实时状态</td>
                                    <td id="tt2" onclick="TableClick('2')" style="cursor:hand">商铺销售付款类型</td>
                                    <td id="tt3" onclick="TableClick('3')" style="cursor:hand">商铺销售明细</td>
                                    <td id="tt4" onclick="TableClick('4')" style="cursor:hand">POS签到查询</td>
                                    <td id="tt5" onclick="TableClick('5')" style="cursor:hand">POS最终签退查询</td>
                                    </tr></table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table style="width: 100%">
                                <tr>
                                    <td align="center" valign="top">
                                        <asp:Panel ID="Panel1" runat="server" BackColor="White" BorderStyle="Inset" 
                                            BorderWidth="1px" Height="100%" ScrollBars="Auto" Width="230px">
                                            <table>
                                                <tr>
                                                    <td ID="treeview" style="height: 100%; width: 160px; text-align:left;" 
                                                        valign="top">
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                    <td align="center" valign="top" colspan="3">
                                    <table width="100%" style="height:100%"><tr>
                                    <td id="td1" style="display:block">
                                        <asp:GridView ID="GVPosStatus" runat="server" BackColor="LightGoldenrodYellow" 
                                            BorderWidth="1px" PageSize="17" Width="97%" 
                                            BorderColor="#E1E0B2" CellPadding="2" ForeColor="Black" GridLines="None" 
                                            BorderStyle="Outset">                                       
                                            <RowStyle Height="20px" HorizontalAlign="Left" />
                                            <HeaderStyle BackColor="#E1E0B2" Font-Bold="True" Height="20px" />
                                            <AlternatingRowStyle BackColor="PaleGoldenrod" />
                                        </asp:GridView>
                                    </td>
                                    <td id="td2" style="display:none">                                    
                                        <asp:GridView ID="GVPayType" runat="server" BackColor="LightGoldenrodYellow" 
                                            BorderWidth="1px" PageSize="17" Width="97%" 
                                            BorderColor="#E1E0B2" CellPadding="2" ForeColor="Black" GridLines="None" 
                                            BorderStyle="Outset">                                       
                                            <RowStyle Height="20px" HorizontalAlign="Left" />
                                            <HeaderStyle BackColor="#E1E0B2" Font-Bold="True" Height="20px" />
                                            <AlternatingRowStyle BackColor="PaleGoldenrod" />
                                        </asp:GridView>                                    
                                    </td>
                                    <td id="td3" style="display:none">                                    
                                        <asp:GridView ID="GVDetail" runat="server" BackColor="LightGoldenrodYellow" 
                                            BorderWidth="1px" PageSize="17" Width="97%" 
                                            BorderColor="#E1E0B2" CellPadding="2" ForeColor="Black" GridLines="None" 
                                            BorderStyle="Outset">                                       
                                            <RowStyle Height="20px" HorizontalAlign="Left" />
                                            <HeaderStyle BackColor="#E1E0B2" Font-Bold="True" Height="20px" />
                                            <AlternatingRowStyle BackColor="PaleGoldenrod" />
                                        </asp:GridView>                                   
                                    </td>
                                    <td id="td4" style="display:none">
                                    <asp:GridView ID="GVSignOn" runat="server" BackColor="LightGoldenrodYellow" 
                                            BorderWidth="1px" PageSize="17" Width="97%" 
                                            BorderColor="#E1E0B2" CellPadding="2" ForeColor="Black" GridLines="None" 
                                            BorderStyle="Outset">                                       
                                            <RowStyle Height="20px" HorizontalAlign="Left" />
                                            <HeaderStyle BackColor="#E1E0B2" Font-Bold="True" Height="20px" />
                                            <AlternatingRowStyle BackColor="PaleGoldenrod" />
                                        </asp:GridView>    
                                    </td>
                                    <td id="td5" style="display:none">
                                    <asp:GridView ID="GVSignOff" runat="server" BackColor="LightGoldenrodYellow" 
                                            BorderWidth="1px" PageSize="17" Width="97%" 
                                            BorderColor="#E1E0B2" CellPadding="2" ForeColor="Black" GridLines="None" 
                                            BorderStyle="Outset">                                       
                                            <RowStyle Height="20px" HorizontalAlign="Left" />
                                            <HeaderStyle BackColor="#E1E0B2" Font-Bold="True" Height="20px" />
                                            <AlternatingRowStyle BackColor="PaleGoldenrod" />
                                        </asp:GridView>    
                                    </td>
                                    </tr></table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10%" align="right">
                                        <asp:Button ID="treeClick" runat="server" CssClass="buttonHidden" 
                                            OnClick="treeClick_Click" Width="1px" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td style="width: 10%">
                                        &nbsp;</td>
                                    <td style="width: 20%">
                                    </td>
                                    <td align="right" height="22">
                                        &nbsp;&nbsp; &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>

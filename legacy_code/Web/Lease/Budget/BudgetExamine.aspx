<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BudgetExamine.aspx.cs" Inherits="Lease_Budget_BudgetExamine" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%=baseinfo%></title>
        <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/longCss/webtab.css" rel="stylesheet" type="text/css"/>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link rel="StyleSheet" href="../../App_Themes/nlstree/nlstree-basic.css" type="text/css" />
    <link rel="StyleSheet" href="../../App_Themes/nlstree/nlsctxmenu.css" type="text/css" />
    
    <script type="text/javascript"  src="../../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"> </script>
	<script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
    <script type="text/javascript" src="../../JavaScript/Calendar.js" charset="gb2312"></script>
    
    <script type="text/javascript" src="../../App_Themes/nlstree/nlstree.js"></script>
    <script type="text/javascript" src="../../App_Themes/nlstree/nlsctxmenu.js"></script>
    
	<script type="text/javascript">
	function Load()
    {
        addTabTool("<%=strFresh %>,Lease/Budget/BudgetExamine.aspx");
        loadTitle();
    }
    function BtnUp( p )
    {
	    var t = String(p)
	    var l = t.substring(3,15); 
	    document.getElementById( p ).style.backgroundImage = 'url(../../App_Themes/CSS/BtnImage/btn_' + l + '.gif)';
    }
    function BtnOver( p )
    {
	    var t = String(p)
	    var l = t.substring(3,15); 
	    document.getElementById( p ).style.backgroundImage = 'url(../../App_Themes/CSS/BtnImage/over_' + l + '.gif)';
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
        t.opt.height="385px";
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
              <table border="0" cellpadding="0" cellspacing="0" style="width: 98%; height: 4px">
                <tr>
                    <td align="left" class="tdTopRightBackColor" style="vertical-align: top; width: 356px;
                        height: 22px; text-align: left">
                        <img class="imageLeftBack" src="" style="width: 7px; height: 22px" />
                        <asp:Label ID="Label31" runat="server" Text="<%$ Resources:BaseInfo,Menu_BudgetExamine %>"
                            Width="295px"></asp:Label></td>
                    <td class="tdTopRightBackColor" colspan="2" style="vertical-align: top; height: 22px;
                        text-align: right" valign="top">
                        <img align="right" class="imageRightBack" src="" style="width: 7px; height: 22px" /></td>
                </tr>
                </table> 
                <table style="width: 98%" class="tdBackColor">
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
                                    <td align="left">
                                        <asp:Panel ID="Panel1" runat="server" BackColor="White" BorderStyle="Inset" 
                                            BorderWidth="1px" Height="395px" ScrollBars="Auto" Width="230px">
                                            <table style="width: 150px; height: 200px">
                                                <tr>
                                                    <td ID="treeview" style="height: 260px; width: 150px; text-align:left;" 
                                                        valign="top">
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                    <td align="center" colspan="3">
                                    <div style="overflow-y: scroll; height: 370px;width:98%" id="dvBody"> 
                                        <asp:GridView ID="gvBudget" runat="server" AllowPaging="False" 
                                            AutoGenerateColumns="False" BackColor="White" BorderStyle="Inset" 
                                            BorderWidth="1px" Height="120px" 
                                            OnRowDataBound="gvBudget_RowDataBound" PageSize="15" Width="98%">
                                    <Columns>
                         <asp:TemplateField>
                                            <ItemTemplate>
                                                &nbsp;<asp:TextBox ID="txtBatchID" runat="server" CssClass="ipt35px" Text='<%# DataBinder.Eval(Container, "DataItem.BatchID")%>' Font-Size="9pt" Width="80px"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="hidden" />
                                            <HeaderStyle CssClass="hidden" />
                                        </asp:TemplateField>
                         <asp:TemplateField>
                                            <ItemTemplate>
                                                &nbsp;<asp:TextBox ID="txtUnitID" runat="server" CssClass="ipt35px" Text='<%# DataBinder.Eval(Container, "DataItem.UnitID")%>' Font-Size="9pt" Width="80px"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="hidden" />
                                            <HeaderStyle CssClass="hidden" />
                                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,RentableArea_lblUnitCode %>">
                                            <ItemTemplate>
                                                &nbsp;<asp:TextBox ID="txtUnitCode" runat="server" CssClass="ipt35px" Text='<%# DataBinder.Eval(Container, "DataItem.UnitCode")%>' Font-Size="9pt" Width="80px" ReadOnly="true"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                         <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                        </asp:TemplateField>
                       <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,RentableArea_lblFloorArea %>">
                                            <ItemTemplate>
                                                &nbsp;<asp:TextBox ID="txtFloorArea" runat="server" CssClass="ipt35px" Text='<%# DataBinder.Eval(Container, "DataItem.FloorArea")%>' Font-Size="9pt" Width="80px" ReadOnly="true"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                         <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                        </asp:TemplateField>
                       <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,Budget_lblUseArea %>">
                                            <ItemTemplate>
                                                &nbsp;<asp:TextBox ID="txtUseArea" runat="server" CssClass="ipt35px" Text='<%# DataBinder.Eval(Container, "DataItem.UseArea")%>' Font-Size="9pt" Width="80px" ReadOnly="true"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                         <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,Budget_UnitTypeName %>">
                                            <ItemTemplate>
                                                &nbsp;<asp:TextBox ID="txtUnitTypeName" runat="server" CssClass="ipt35px" Text='<%# DataBinder.Eval(Container, "DataItem.UnitTypeName")%>' Font-Size="9pt" Width="80px" ReadOnly="true"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                         <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                        </asp:TemplateField>                                                                                                                      
                        <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,Budget_UnitPrice %>">
                                            <ItemTemplate>
                                                &nbsp;<asp:TextBox ID="txtUnitPrice" runat="server" CssClass="ipt35px" Text='<%# DataBinder.Eval(Container, "DataItem.UnitPrice")%>' Font-Size="9pt" Width="80px" ReadOnly="true"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                         <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,Budget_MonthAmt %>">
                                            <ItemTemplate>
                                                &nbsp;<asp:TextBox ID="txtMonthAmt" runat="server" CssClass="ipt35px" Text='<%# DataBinder.Eval(Container, "DataItem.MonthAmt")%>' Font-Size="9pt" Width="80px" ReadOnly="true"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                         <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,Budget_YearAmt %>">
                                            <ItemTemplate>
                                                &nbsp;<asp:TextBox ID="txtYearAmt" runat="server" CssClass="ipt35px" Text='<%# DataBinder.Eval(Container, "DataItem.YearAmt")%>' Font-Size="9pt" Width="80px" ReadOnly="true"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                         <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                        </asp:TemplateField>
                           <asp:TemplateField>
                                            <ItemTemplate>
                                                &nbsp;<asp:TextBox ID="txtUnitTypeID" runat="server" CssClass="ipt35px" Text='<%# DataBinder.Eval(Container, "DataItem.UnitTypeID")%>' Font-Size="9pt"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="hidden" />
                                            <HeaderStyle CssClass="hidden" />
                                        </asp:TemplateField>
                                    </Columns>
                                     <FooterStyle BackColor="Red" ForeColor="#000066"/>
                                    <RowStyle ForeColor="Black" Height="20px" Font-Overline="False" Font-Size="10pt" HorizontalAlign="Left" />
                                    <SelectedRowStyle BackColor="#FFFFCD" Font-Bold="False" ForeColor="Black" />
                                    <HeaderStyle BackColor="#E1E0B2" Height="20px" Font-Bold="False"  HorizontalAlign="Center"  />
                                    <PagerStyle BackColor="White" ForeColor="#000066"   HorizontalAlign="Right"/>
                                </asp:GridView>
                                </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10%; height: 36px;" align="right">
                                        <asp:Button ID="treeClick" runat="server" CssClass="buttonHidden" 
                                            OnClick="treeClick_Click" Width="1px" />
                                        &nbsp;
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td style="width: 10%; height: 36px;">
                                    </td>
                                    <td style="width: 20%; height: 36px;">
                                    </td>
                                    <td align="right" style="height: 36px">
                                        &nbsp; &nbsp; &nbsp;
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

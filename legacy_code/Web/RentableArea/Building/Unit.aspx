<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Unit.aspx.cs" Inherits="RentableArea_Building_Unit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Menu_DefineRegularUnits")%></title>
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link rel="StyleSheet" href="../../App_Themes/nlstree/nlstree-basic.css" type="text/css" />
    <link rel="StyleSheet" href="../../App_Themes/nlstree/nlsctxmenu.css" type="text/css" />
    <script type="text/javascript" src="../../App_Themes/nlstree/nlstree.js"></script>
    <script type="text/javascript" src="../../App_Themes/nlstree/nlsctxmenu.js"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"></script>
    <script src="../../JavaScript/TreeShow.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
    <script type="text/javascript">
    var tabbar ;
    function treearray(TreeStr,SelectID,SelectedID,ClickName)
    {
        var t = new NlsTree('MyTree');
        var treestr =document.getElementById("depttxt").value;
         
        var treearr = new Array();
        var n=0;
        var id;
        var pid;
        var name;
        var imgurl;
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
                    imgurl=treenode[3];
                }
                
                t.add(id, pid, name, "", imgurl, true);
                
            }
        }
                t.opt.sort='no';
                t.opt.enbScroll=true;
                t.opt.height="300px";
                t.opt.width="235px";
                t.opt.trg="mainFrame";
                t.opt.oneExp=true;

                t.treeOnClick = ev_click;
                t.render("treeview");
                t.collapseAll();

                if(document.form1.selectdeptid.value!='')
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
    function Load()
    {
        parent.document.all.txtWroMessage.value = "";
        treearray();
        //addTabTool("<%=strFresh %>,RentableArea/Building/Unit.aspx?Type=Browse&funcid=65537");
        addTabTool("<%=strFresh %>,<%=strLink%>");
	    loadTitle(); 
    }
    //清空提示信息
    function ClearInfo()
    {
        parent.document.all.txtWroMessage.value = "";
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
    function ConfirmDel()//删除确认
    {
        if(confirm("是否删除勾选单元？")==true)
        {
            var objImgBtn1 = document.getElementById('<%= btnD.ClientID %>');
            objImgBtn1.click();
        }
    }
    </script>
</head>
<body onload='Load();' topmargin=0 leftmargin=0>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
    <div>
        <asp:HiddenField id="depttxt" runat="server"></asp:HiddenField>
            <asp:HiddenField id="deptid" runat="server" ></asp:HiddenField>
        <asp:HiddenField ID="selectdeptid" runat="server" />
        <table border="0" cellpadding="0" cellspacing="0" style="height: 430px; width:100%;">
            <tr>
                <td style="width:26%; height: 401px; text-align: right; vertical-align: top;">
                    <table border="0" cellpadding="0" cellspacing="0" style="vertical-align: top; height: 255px"
                        width="100%">
                        <tr>
                            <td class="tdTopBackColor" style="width: 266px; height: 27px;">
                                <img alt="" class="imageLeftBack" /><asp:Label ID="labUnitTitle" runat="server"
                                    CssClass="lblTitle" Text="<%$ Resources:BaseInfo,RentableArea_labUnitTitle %>" meta:resourcekey="labUnitTitleResource1"></asp:Label></td>
                            <td class="tdTopRightBackColor" valign="top" style="height: 27px">
                                &nbsp;<img class="imageRightBack" /></td>
                        </tr>
                        <tr height="1">
                            <td colspan="2" style="height: 1px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="2" rowspan="10" style="height: 341px; text-align: center"
                                valign="top">
                                <table style="height: 300px">
                                    <tr>
                                        <td style="height: 10px">
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 267px">
                                                <tr>
                                                    <td style="width: 160px; height: 1px; background-color: #738495; position: relative; top: 3px;">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 160px; height: 1px; background-color: #ffffff; position: relative; top: 3px;">
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                    <td style="height: 22px">
                                        <asp:RadioButton ID="rbtNoLeaseOut" runat="server" 
                                            Text="<%$ Resources:BaseInfo,RentableArea_NoLeaseOut %>" GroupName="area" 
                                            CssClass="rbtn" Font-Size="10.5pt" Checked="True" 
                                            meta:resourcekey="rbtnNoLeaseOutResource1" AutoPostBack="True" 
                                            oncheckedchanged="rbtNoLeaseOut_CheckedChanged" />
                                        <asp:RadioButton ID="rbtLeaseOut" runat="server" 
                                            Text="<%$ Resources:BaseInfo,RentableArea_LeaseOut %>" GroupName="area" 
                                            CssClass="rbtn" Font-Size="10.5pt" 
                                            meta:resourcekey="rbtnLeaseOutResource1" AutoPostBack="True" 
                                            oncheckedchanged="rbtLeaseOut_CheckedChanged" />&nbsp;
                                        <asp:RadioButton ID="rbtBlankOut" runat="server" 
                                            Text="<%$ Resources:BaseInfo,RentableArea_BlankOut %>" GroupName="area" 
                                            CssClass="rbtn" Font-Size="10.5pt" 
                                            meta:resourcekey="rbtnBlankOutResource1" AutoPostBack="True" 
                                            oncheckedchanged="rbtBlankOut_CheckedChanged" /></td>
                                    </tr>
                                    <tr>
                                        <td style="height: 275px">
                                            <asp:Panel ID="Panel1" runat="server" BackColor="White" BorderStyle="Inset" BorderWidth="1px"
                                                Font-Size="Medium" Height="315px" HorizontalAlign="Left" ScrollBars="Auto" Width="260px" meta:resourcekey="Panel1Resource1">
                                                <table>
                                                    <tr>
                                                        <td style="width: 207px; height: 270px" valign="top" id="treeview">
                                                            &nbsp;</td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px">
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 267px">
                                                <tr>
                                                    <td style="width: 160px; height: 1px; background-color: #738495">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 160px; height: 1px; background-color: #ffffff">
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top; height: 35px; text-align:right;">
                                            <asp:Button ID="treeClick"
                                                    runat="server" CssClass="buttonHidden" OnClick="treeClick_Click" Width="24px" meta:resourcekey="treeClickResource1" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 2%; height: 401px">
                </td>
                <td style="width: 63%; height: 401px; vertical-align: top;">
                    <table border="0" cellpadding="0" cellspacing="0" style="vertical-align: top; height: 288px; width:100%;">
                        <tr>
                        <td style="vertical-align:top; width:100%;" >
                         <table border="0" cellpadding="0" cellspacing="0" style="height: 22px; width: 100%;   ">
                            <tr>
                                <td class="tdTopRightBackColor"    valign="top" style="width: 8px; height:27px;  text-align:left" >
                                    <img alt="" class="imageLeftBack" style=" text-align:left"  />
                                    </td>
                                    <td class="tdTopRightBackColor" style="width: 251px; height: 27px; text-align:left; vertical-align:bottom;">
                                    <asp:Label
                                        ID="lblUnitit" runat="server" Text='<%$ Resources:BaseInfo,RentableArea_lblUnit %>' Height="12pt" Width="250px" meta:resourcekey="lblUnititResource1"></asp:Label></td>
                              
                                <td class="tdTopRightBackColor"   valign="top" style="height: 27px; text-align:right;">
                                    <img class="imageRightBack" style="width: 7px; height: 22px" /></td>
                            </tr>
                        </table>
                        </td>
                        </tr>
                        <tr height="1">
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" rowspan="10" style="height: 341px; text-align: center" 
                                valign="top">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 273px">
                                    <tr>
                                        <td style="height: 20px; text-align: center; vertical-align:middle;">
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 98%">
                                                <tr>
                                                    <td style="width: 160px; height: 1px; background-color: #738495">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 160px; height: 1px; background-color: #ffffff">
                                                    </td>
                                                </tr>
                                            </table>
                                            </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%; text-align:center; height: 25px;">
                                            <asp:GridView ID="GrdUnit" runat="server" AllowPaging="True" 
                                                AutoGenerateColumns="False" BackColor="White" BorderStyle="Inset" 
                                                BorderWidth="1px" CellPadding="3" Height="258px" Width="98%" 
                                                onrowdatabound="GrdUnit_RowDataBound" PageSize="14" 
                                                onpageindexchanging="GrdUnit_PageIndexChanging">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox id="Checkbox" runat="server" Value='<%# DataBinder.Eval(Container, "DataItem.UnitID")%>'></asp:CheckBox>
                                                        </ItemTemplate>
                                                     <ItemStyle BorderColor="#E1E0B2" />
                                                     <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="UnitID">
                                                        <ItemStyle CssClass="hidden" />
                                                        <HeaderStyle CssClass="hidden" />
                                                        <FooterStyle CssClass="hidden" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="UnitCode" 
                                                        HeaderText="<%$ Resources:BaseInfo,RentableArea_lblUnitCode %>">
                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" 
                                                            HorizontalAlign="Left" />
                                                        <ItemStyle BorderColor="#E1E0B2" HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="UnitTypeName" 
                                                        HeaderText="<%$ Resources:BaseInfo,RentableArea_lblUnitType %>">
                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" 
                                                            HorizontalAlign="Left" />
                                                        <ItemStyle BorderColor="#E1E0B2" HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="BuildingName" 
                                                        HeaderText="<%$ Resources:BaseInfo,RentableArea_lblSelBuildingID %>">
                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" 
                                                            HorizontalAlign="Left" />
                                                        <ItemStyle BorderColor="#E1E0B2" HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="FloorName" 
                                                        HeaderText="<%$ Resources:BaseInfo,RentableArea_lblSelFloorID %>">
                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" 
                                                            HorizontalAlign="Left" />
                                                        <ItemStyle BorderColor="#E1E0B2" HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="LocationName" 
                                                        HeaderText="<%$ Resources:BaseInfo,RentableArea_lblSelLocationID %>">
                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" 
                                                            HorizontalAlign="Left" />
                                                        <ItemStyle BorderColor="#E1E0B2" HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="AreaName" 
                                                        HeaderText="<%$ Resources:BaseInfo,RentableArea_lblArea %>">
                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" 
                                                            HorizontalAlign="Left" />
                                                        <ItemStyle BorderColor="#E1E0B2" HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="TradeName" 
                                                        HeaderText="<%$ Resources:BaseInfo,LeaseholdContract_labTradeID %>">
                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2"  CssClass="gridviewtitle" HorizontalAlign="Left"/>
                                                        <ItemStyle BorderColor="#E1E0B2" HorizontalAlign="Left"/>
                                                    </asp:BoundField>
                                                     <asp:BoundField DataField="ShopTypeName" 
                                                        HeaderText="<%$ Resources:BaseInfo,PotShop_lblShopType %>">
                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2"  CssClass="gridviewtitle" HorizontalAlign="Left"/>
                                                        <ItemStyle BorderColor="#E1E0B2" HorizontalAlign="Left"/>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="FloorArea" 
                                                        HeaderText="<%$ Resources:BaseInfo,RentableArea_lblFloorArea %>">
                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2"  CssClass="gridviewtitle" />
                                                        <ItemStyle BorderColor="#E1E0B2" HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="UseArea" 
                                                        HeaderText="<%$ Resources:BaseInfo,RentableArea_lblUseArea %>">
                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2"  CssClass="gridviewtitle" />
                                                        <ItemStyle BorderColor="#E1E0B2"　HorizontalAlign="Right"  />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="StoreID">
                                                        <ItemStyle CssClass="hidden" />
                                                        <HeaderStyle CssClass="hidden" />
                                                        <FooterStyle CssClass="hidden" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <FooterStyle BackColor="Red" ForeColor="#000066" />
                                                <RowStyle Font-Overline="False" Font-Size="10pt" ForeColor="Black" 
                                                    Height="10px" />
                                                <SelectedRowStyle BackColor="#FFFFCD" Font-Bold="False" ForeColor="Black" />
                                                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Right" />
                                                <HeaderStyle BackColor="#E1E0B2" Font-Bold="False" Height="10px" />
                                                <PagerTemplate>
                                                    <asp:LinkButton ID="LinkButtonFirstPage" runat="server" CommandArgument="First" 
                                                        CommandName="Page" 
                                                        Visible="<%# ((GridView)Container.NamingContainer).PageIndex != 0 %>" 
                                                        Font-Size="Small">首页</asp:LinkButton>
                                                    <asp:LinkButton ID="LinkButtonPreviousPage" runat="server" 
                                                        CommandArgument="Prev" CommandName="Page" 
                                                        Visible="<%# ((GridView)Container.NamingContainer).PageIndex != 0 %>" 
                                                        Font-Size="Small">上一页</asp:LinkButton>
                                                    <asp:LinkButton ID="LinkButtonNextPage" runat="server" CommandArgument="Next" 
                                                        CommandName="Page" 
                                                        
                                                        Visible="<%# ((GridView)Container.NamingContainer).PageIndex != ((GridView)Container.NamingContainer).PageCount - 1 %>" 
                                                        Font-Size="Small">下一页</asp:LinkButton>
                                                    <asp:LinkButton ID="LinkButtonLastPage" runat="server" CommandArgument="Last" 
                                                        CommandName="Page" 
                                                        
                                                        Visible="<%# ((GridView)Container.NamingContainer).PageIndex != ((GridView)Container.NamingContainer).PageCount - 1 %>" 
                                                        Font-Size="Small">尾页</asp:LinkButton>
                                                    <asp:TextBox ID="txtNewPageIndex" runat="server" 
                                                        text="<%# ((GridView)Container.Parent.Parent).PageIndex + 1 %>" width="20px" />
                                                    /
                                                    <asp:Label ID="LabelPageCount" runat="server" 
                                                        Text="<%# ((GridView)Container.NamingContainer).PageCount %>"></asp:Label>
                                                    <asp:LinkButton ID="btnGo" runat="server" causesvalidation="False" 
                                                        commandargument="-1" commandname="Page" text="GO" />
                                                </PagerTemplate>
                                                <PagerSettings Mode="NextPreviousFirstLast" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px; text-align: center; vertical-align: bottom;">
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 98%">
                                                <tr>
                                                    <td style="width: 160px; height: 1px; background-color: #738495">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 160px; height: 1px; background-color: #ffffff">
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px; text-align: right">
                                            </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 30px; text-align: right">
                                            <asp:LinkButton ID="btnReFresh" runat="server" onclick="btnReFresh_Click"></asp:LinkButton>
                                            <asp:LinkButton ID="btnD" runat="server" onclick="btnD_Click"></asp:LinkButton>
                                            <asp:Button ID="btnAdd" runat="server" CssClass="buttonAdd" 
                                                meta:resourcekey="btnAddResource1" 
                                                Text="<%$ Resources:BaseInfo,DeptTree_labDeptAdd %>" Enabled="False" 
                                                onclick="btnAdd_Click" />
                                            &nbsp;<asp:Button ID="btnExport" runat="server" CssClass="buttonEdit"
                                                Text="<%$ Resources:BaseInfo,BankCard_btnTransmit %>" 
                                                meta:resourcekey="btnEditResource1" Enabled="False" 
                                                onclick="btnExport_Click" />&nbsp;<asp:Button 
                                                ID="btnEdit" runat="server" CssClass="buttonEdit"
                                                Text="<%$ Resources:BaseInfo,User_btnChang %>" 
                                                meta:resourcekey="btnEditResource1" Enabled="False" 
                                                onclick="btnEdit_Click" />&nbsp;<input ID="Button1" runat="server" 
                                                class="buttonEdit" disabled="disabled" onclick="ConfirmDel()" type="button" 
                                                value="删除" /> 
                                            <asp:Button ID="btnSave"
                                                    runat="server" CssClass="buttonSave" 
                                                Text="<%$ Resources:BaseInfo,User_btnOk %>" Enabled="False" 
                                                meta:resourcekey="btnSaveResource1" onclick="btnSave_Click" />
                                            <asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel"
                                                Text="<%$ Resources:BaseInfo,User_btnCancel %>" 
                                                onclick="btnCancel_Click" />
                                            &nbsp; &nbsp;&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px; text-align: left">
                                            </td>
                                    </tr>
                                </table>
                                            <asp:DropDownList ID="cmbRentLevel" runat="server" Width="155px" CssClass="hidden" Enabled="False" meta:resourcekey="cmbRentLevelResource1">
                                            </asp:DropDownList></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    
    </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:HiddenField ID="hidUnitCode" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidUnitCode %>" />
        <asp:HiddenField ID="hidFloorArea" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidFloorArea %>" />
        <asp:HiddenField ID="hidUseArea" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidUseArea %>" />
        <asp:HiddenField ID="hidlblUnitit" runat="server" Value="<%$ Resources:BaseInfo,Menu_DefineRegularUnits %>" />
    </form>
</body>
</html>


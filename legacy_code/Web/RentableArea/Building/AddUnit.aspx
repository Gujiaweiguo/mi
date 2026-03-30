<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddUnit.aspx.cs" Inherits="RentableArea_Building_AddUnit" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
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
function treearray()
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
            t.opt.width="285px";
            t.opt.trg="mainFrame";
            t.opt.oneExp=true;

            t.treeOnClick = ev_click;
            t.render("treeview");      
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
		
    function Load()
    {
        parent.document.all.txtWroMessage.value = "";
        treearray();
        addTabTool("<%=strFresh %>,RentableArea/Building/AddUnit.aspx");
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
                <td style="width:49%; height: 401px; text-align: right; vertical-align: top;">
                    <table border="0" cellpadding="0" cellspacing="0" style="vertical-align: top; height: 255px"
                        width="100%">
                        <tr>
                            <td class="tdTopBackColor" colspan="2" style="width: 266px; height: 27px;">
                                <img alt="" class="imageLeftBack" /><asp:Label ID="labUnitTitle" runat="server"
                                    CssClass="lblTitle" Text="<%$ Resources:BaseInfo,RentableArea_labUnitTitle %>" meta:resourcekey="labUnitTitleResource1"></asp:Label></td>
                            <td class="tdTopRightBackColor" colspan="2" valign="top" style="height: 27px">
                                &nbsp;<img class="imageRightBack" /></td>
                        </tr>
                        <tr height="1">
                            <td colspan="5" style="height: 1px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" rowspan="10" style="height: 341px; text-align: center"
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
                                        <asp:RadioButton ID="rbtnNoLeaseOut" runat="server" Text="<%$ Resources:BaseInfo,RentableArea_NoLeaseOut %>" GroupName="area" CssClass="rbtn" Font-Size="10.5pt" AutoPostBack="True" OnCheckedChanged="rbtnNoLeaseOut_CheckedChanged" Checked="True" meta:resourcekey="rbtnNoLeaseOutResource1" />
                                        <asp:RadioButton ID="rbtnLeaseOut" runat="server" Text="<%$ Resources:BaseInfo,RentableArea_LeaseOut %>" GroupName="area" CssClass="rbtn" Font-Size="10.5pt" AutoPostBack="True" OnCheckedChanged="rbtnLeaseOut_CheckedChanged" meta:resourcekey="rbtnLeaseOutResource1" />&nbsp;
                                        <asp:RadioButton ID="rbtnBlankOut" runat="server" Text="<%$ Resources:BaseInfo,RentableArea_BlankOut %>" GroupName="area" CssClass="rbtn" Font-Size="10.5pt" AutoPostBack="True" OnCheckedChanged="rbtnBlankOut_CheckedChanged" meta:resourcekey="rbtnBlankOutResource1" /></td>
                                    </tr>
                                    <tr>
                                        <td style="height: 275px">
                                            <asp:Panel ID="Panel1" runat="server" BackColor="White" BorderStyle="Inset" BorderWidth="1px"
                                                Font-Size="Medium" Height="315px" HorizontalAlign="Left" ScrollBars="Auto" Width="300px" meta:resourcekey="Panel1Resource1">
                                                <table>
                                                    <tr>
                                                        <td style="width: 207px; height: 259px" valign="top" id="treeview">
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
                                        <td style="vertical-align: top; height: 41px; text-align:right;">
                                            <asp:Button ID="btnAdd" runat="server" CssClass="buttonAdd" OnClick="btnAdd_Click"
                                                Text="<%$ Resources:BaseInfo,DeptTree_labDeptAdd %>" meta:resourcekey="btnAddResource1" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/><asp:Button ID="treeClick"
                                                    runat="server" CssClass="buttonHidden" OnClick="treeClick_Click" Width="24px" meta:resourcekey="treeClickResource1" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 2%; height: 401px">
                </td>
                <td colspan="3" style="width: 49%; height: 401px; vertical-align: top;">
                    <table border="0" cellpadding="0" cellspacing="0" style="vertical-align: top; height: 288px; width:100%;">
                        <tr>
                        <td    style="vertical-align:top; width:100%;" >
                        
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
                            <td colspan="4">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" rowspan="10" style="height: 341px; text-align: center"
                                valign="top">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 292px; height: 273px">
                                    <tr>
                                        <td colspan="3" style="height: 20px; text-align: center; vertical-align:middle;">
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 265px">
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
                                        <td style="width: 131px; text-align: right; height: 25px;">
                                            <asp:Label ID="lblUnitCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblUnitCode %>" meta:resourcekey="lblUnitCodeResource1"></asp:Label>&nbsp;</td>
                                        <td style="height: 25px">
                                            &nbsp;</td>
                                        <td style="width: 195px; text-align: left; height: 25px;">
                                            <asp:TextBox ID="txtUnitCode" runat="server" CssClass="Enabledipt150px" ReadOnly="True" meta:resourcekey="txtUnitCodeResource1"></asp:TextBox>
                                            <img id="ImgUnitCode" src="../../App_Themes/Main/Images/must.gif" style="width: 16px;
                                                height: 16px" /></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 131px; height: 26px; text-align: right">
                                            <asp:Label ID="Label1" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblUnitType %>" meta:resourcekey="lblUnitCodeResource1"></asp:Label>&nbsp;</td>
                                        <td style="height: 26px">
                                            &nbsp;</td>
                                        <td style="width: 195px; height: 26px; text-align: left">
                                            <asp:DropDownList ID="ddlUnitType" runat="server" Width="155px" CssClass="cmb140px" OnSelectedIndexChanged="cmbBuildingID_SelectedIndexChanged" Enabled="False" meta:resourcekey="cmbBuildingIDResource1">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 131px; text-align: right; height: 22px;">
                                            <asp:Label ID="lblSelBuildingID" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblSelBuildingID %>" meta:resourcekey="lblSelBuildingIDResource1"></asp:Label>&nbsp;&nbsp;</td>
                                        <td style="height: 22px">
                                            &nbsp;</td>
                                        <td style="width: 195px; text-align: left; height: 25px;"><asp:DropDownList ID="cmbBuildingID" runat="server" Width="155px" CssClass="cmb140px" AutoPostBack="True" OnSelectedIndexChanged="cmbBuildingID_SelectedIndexChanged" Enabled="False" meta:resourcekey="cmbBuildingIDResource1">
                                        </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 131px; height: 24px; text-align: right">
                                            <asp:Label ID="lblSelFloorID" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblSelFloorID %>" meta:resourcekey="lblSelFloorIDResource1"></asp:Label>&nbsp;</td>
                                        <td style="height: 24px">
                                            &nbsp;</td>
                                        <td style="width: 195px; height: 25px; text-align: left">
                                            <asp:DropDownList ID="cmbFloorID" runat="server" Width="155px" CssClass="cmb140px" AutoPostBack="True" OnSelectedIndexChanged="cmbFloorID_SelectedIndexChanged" Enabled="False" meta:resourcekey="cmbFloorIDResource1">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 131px; height: 26px; text-align: right">
                                            <asp:Label ID="lblSelLocationID" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblSelLocationID %>" meta:resourcekey="lblSelLocationIDResource1"></asp:Label></td>
                                        <td style="height: 26px">
                                            &nbsp;</td>
                                        <td style="width: 195px; height: 25px; text-align: left">
                                            <asp:DropDownList ID="cmbLocationID" runat="server" Width="155px" CssClass="cmb140px" OnSelectedIndexChanged="cmbLocationID_SelectedIndexChanged" Enabled="False" meta:resourcekey="cmbLocationIDResource1">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 131px; height: 26px; text-align: right">
                                            <asp:Label ID="lblSelArea" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblSelArea %>" meta:resourcekey="lblSelAreaResource1"></asp:Label></td>
                                        <td style="height: 26px">
                                            &nbsp;</td>
                                        <td style="width: 195px; height: 25px; text-align: left">
                                            <asp:DropDownList ID="cmbTradeRelation" runat="server" CssClass="cmb150px" Width="155px" Enabled="False" meta:resourcekey="cmbTradeRelationResource1">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 131px; height: 22px; text-align: right">
                                            <asp:Label ID="lblTradeID" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labTradeID %>" meta:resourcekey="lblTradeIDResource1"></asp:Label></td>
                                        <td style="height: 22px">
                                            &nbsp;</td>
                                        <td style="width: 195px; height: 25px; text-align: left">
                                            <asp:DropDownList ID="cmbTradeID" runat="server" CssClass="cmb150px" Width="155px" Enabled="False" meta:resourcekey="cmbTradeIDResource1">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 131px; height: 22px; text-align: right">
                                            <asp:Label ID="Label2" runat="server" CssClass="labelStyle" meta:resourcekey="lblTradeIDResource1"
                                                Text="<%$ Resources:BaseInfo,PotShop_lblShopType %>"></asp:Label></td>
                                        <td style="height: 22px">
                                        </td>
                                        <td style="width: 195px; height: 25px; text-align: left">
                                            <asp:DropDownList ID="ddlShopType" runat="server" CssClass="cmb150px" Width="155px" meta:resourcekey="cmbTradeIDResource1" Enabled="False">
                                            </asp:DropDownList></td>
                                    </tr>
                                                                        <tr>
                                        <td style="width: 131px; height: 22px; text-align: right">
                                            <asp:Label ID="lblFloorArea" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblFloorArea %>" meta:resourcekey="lblFloorAreaResource1"></asp:Label></td>
                                        <td style="height: 22px">
                                            &nbsp;</td>
                                        <td style="width: 195px; height: 25px; text-align: left">
                                            <asp:TextBox ID="txtFloorArea" runat="server" CssClass="Enabledipt150px" ReadOnly="True" MaxLength="8" meta:resourcekey="txtFloorAreaResource1"></asp:TextBox>
                                            <img id="ImgFloorArea" src="../../App_Themes/Main/Images/must.gif" style="width: 16px;
                                                height: 16px" /></td>
                                    </tr>
                                                                        <tr>
                                        <td style="width: 131px; height: 22px; text-align: right">
                                            <asp:Label ID="lblUseArea" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblUseArea %>" meta:resourcekey="lblUseAreaResource1"></asp:Label></td>
                                        <td style="height: 22px">
                                            &nbsp;</td>
                                        <td style="width: 195px; height: 25px; text-align: left">
                                            <asp:TextBox ID="txtUseArea" runat="server" CssClass="Enabledipt150px" ReadOnly="True" MaxLength="8" meta:resourcekey="txtUseAreaResource1"></asp:TextBox>
                                            <img id="ImgtxtUseArea" src="../../App_Themes/Main/Images/must.gif" style="width: 16px;
                                                height: 16px" /></td>
                                    </tr>
                                                                        <tr>
                                        <td style="width: 131px; height: 25px; text-align: right">
                                            <asp:Label ID="lblShopName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblShopName %>" meta:resourcekey="lblShopNameResource1"></asp:Label></td>
                                        <td style="height: 25px">
                                            &nbsp;</td>
                                        <td style="width: 195px; height: 25px; text-align: left">
                                            <asp:TextBox ID="txtShopName" runat="server" CssClass="Enabledipt150px" ReadOnly="True" meta:resourcekey="txtShopNameResource1"></asp:TextBox></td>
                                    </tr>
                                                                        <tr>
                                        <td style="width: 131px; text-align: right; height: 25px;">
                                            <asp:Label ID="lblSelUnitStatus" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblSelUnitStatus %>" meta:resourcekey="lblSelUnitStatusResource1"></asp:Label></td>
                                        <td style="height: 25px">
                                            &nbsp;</td>
                                        <td style="width: 195px; text-align: left; height: 25px;">
                                            <asp:DropDownList ID="cmbUnitStatus" runat="server" CssClass="cmb140px" Width="155px" Enabled="False" meta:resourcekey="cmbUnitStatusResource1">
                                            </asp:DropDownList></td>
                                    </tr>
                                                                        <tr>
                                        <td style="width: 131px; height: 22px; text-align: right">
                                            <asp:Label ID="lblNode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,User_lblNote %>" meta:resourcekey="lblNodeResource1"></asp:Label></td>
                                        <td style="height: 22px">
                                            &nbsp;</td>
                                        <td style="width: 195px; height: 25px; text-align: left">
                                            <asp:TextBox ID="txtNode" runat="server" CssClass="Enabledipt150px" ReadOnly="True" MaxLength="128" meta:resourcekey="txtNodeResource1"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="height: 10px; text-align: center; vertical-align: bottom;">
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 268px">
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
                                        <td colspan="3" style="height: 5px; text-align: right">
                                            </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="height: 30px; text-align: right">
                                            <asp:Button ID="btnEdit" runat="server" CssClass="buttonEdit" OnClick="btnEdit_Click"
                                                Text="<%$ Resources:BaseInfo,User_btnChang %>" meta:resourcekey="btnEditResource1" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>&nbsp;<asp:Button ID="btnSave"
                                                    runat="server" CssClass="buttonSave" OnClick="btnSave_Click" Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" Enabled="False" meta:resourcekey="btnSaveResource1" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                                            <asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel" OnClick="btnCancel_Click"
                                                onmouseout="BtnUp(this.id);" onmouseover="BtnOver(this.id);" onmouseup="BtnUp(this.id);"
                                                Text="<%$ Resources:BaseInfo,User_btnCancel %>" />
                                            &nbsp; &nbsp;&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="height: 10px; text-align: left">
                                            </td>
                                    </tr>
                                </table>
                                            <asp:Label ID="lblSelAreaLevel" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblSelAreaLevel %>" Width="85px" meta:resourcekey="lblSelAreaLevelResource1" Visible="False"></asp:Label>
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

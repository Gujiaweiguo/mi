<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddBuilding.aspx.cs" Inherits="RentableArea_Building_AddBuilding" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Menu_DefineMallGeographicalInformation")%></title>
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link rel="StyleSheet" href="../../App_Themes/nlstree/nlstree-basic.css" type="text/css" />
    <link rel="StyleSheet" href="../../App_Themes/nlstree/nlsctxmenu.css" type="text/css" />
    <script type="text/javascript" src="../../App_Themes/nlstree/nlstree.js"></script>
    <script type="text/javascript" src="../../App_Themes/nlstree/nlsctxmenu.js"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"></script>
	<script type="text/javascript" src="../../JavaScript/TreeShow.js"></script>
	<script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
	<script language="javascript">
	function Load()
	{
	    treearray();
	    addTabTool("<%=strFresh %>,RentableArea/Building/AddBuilding.aspx");
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
	</script>
</head>
<body onload='Load();' topmargin=0 leftmargin=0>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:HiddenField id="depttxt" runat="server"></asp:HiddenField>
            <asp:HiddenField id="deptid" runat="server" ></asp:HiddenField>
                <asp:HiddenField ID="selectdeptid" runat="server" />
        <table border="0" cellpadding="0" cellspacing="0" style="height: 430px; width:100%;">
            <tr>
                <td style="width: 49%; height: 402px; vertical-align: top; text-align: right;">
                    <table border="0" cellpadding="0" cellspacing="0" style="height: 330px; width: 100%;">
                        <tr>
                            <td style="vertical-align:top; height: 22px;" >
                            
                         <table border="0" cellpadding="0" cellspacing="0" style="height: 22px; width:100%;">
                        
                        
                            <tr>
                                <td class="tdTopRightBackColor"    valign="top" style="height:27px;  text-align:left" >
                                    <img alt="" class="imageLeftBack" style=" text-align:left"  />
                                    </td>
                                    <td class="tdTopRightBackColor" style="height: 27px; text-align:left;">
                                <asp:Label ID="labBuildingTitle" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,RentableArea_lblBuildingtit %>"></asp:Label></td>
                              
                                <td class="tdTopRightBackColor"   valign="top" style="height: 27px;">
                                    <img class="imageRightBack" style="width: 7px; height: 22px" />
                                    </td>
                            </tr>
                        
                        </table>
                                </td>
                            
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" rowspan="10" style="height: 342px; text-align: center; width:100%;"
                                valign="top">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 380px">
                          <tr>
                                <td style="height:8px">
                                </td>
                                </tr>
                                <tr>
                                <td style="height:5px; text-align: left;">
                                </td>
                                </tr>
                                    <tr>
                                        <td style="height: 5px; text-align: left">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" rowspan="9" style="height: 207px; text-align: center; width: 285px;" valign="top">
                         <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 338px">
                        <tr style="width:100%">
                            <td colspan="2" style="vertical-align: top; height: 350px; background-color: #e1e0b2;
                                text-align: center; top: 20px; padding-left:40px;" width="100%">
                                <asp:Panel ID="Panel1" runat="server" BackColor="White" BorderStyle="Inset" BorderWidth="1px"
                                    Height="280px" ScrollBars="Auto" Width="100%">
                                    <table style="width: 100%; height: 263px">
                                        <tr>
                                            <td id="treeview" style="width: 100%; height: 174px" valign="top">
                                            </td>
                                        </tr>
                                    </table>
                                    </asp:Panel>
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; padding-left:40px" align="center">
                                        <tr>
                                            <td style="width: 160px; height: 1px; background-color: #738495; position: relative; top: 13px;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 160px; height: 1px; background-color: #ffffff; position: relative; top: 13px;">
                                            </td>
                                        </tr>
                                        
                                        <tr>
                                        <td style="text-align:right; position: relative; top: 25px; height: 31px;">
                                         <asp:Button ID="btnAdd" runat="server" CssClass="buttonAdd"
                                         Text="<%$ Resources:BaseInfo,DeptTree_labDeptAdd %>" OnClick="btnAdd_Click" Enabled="False" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>&nbsp;
                                        </td>
                                        </tr>
                                    </table>
                               </td>
                        </tr>
                    </table>
     
                            </td>
                                  </tr>
                             
                                </table>
                                         <asp:Button ID="treeClick" runat="server" CssClass="buttonHidden" 
                                    Width="1px" OnClick="treeClick_Click" Height="1px" /></td>
                        </tr>
                   
                    </table>
                </td>
                <td style="height: 402px; width: 2%;">
                </td>
                <td colspan="3" style="height: 402px; width: 49%; vertical-align:top;">
                    <table border="0" cellpadding="0" cellspacing="0" style="height: 367px; width: 100%; ">
                        <tr>
                            <td style="vertical-align:top; height: 22px;" >
                            
                         <table border="0" cellpadding="0" cellspacing="0" style="height: 22px; width: 100%;">
                        
                        
                            <tr>
                                <td class="tdTopRightBackColor"    valign="top" style="width: 8px; height:27px;  text-align:left" >
                                    <img alt="" class="imageLeftBack" style=" text-align:left"  />
                                    </td>
                                    <td class="tdTopRightBackColor" style="height: 27px; text-align:left;">
                                <asp:Label ID="Label4" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,RentableArea_lblBuildingtitrig %>"></asp:Label></td>
                              
                                <td class="tdTopRightBackColor"   valign="top" style="width: 20px; height: 27px;">
                                    <img class="imageRightBack" style="width: 7px; height: 22px" />
                                    </td>
                            </tr>
                        </table>
                                </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" rowspan="10" style="height: 384px; text-align: center; width: 391px;"
                                valign="top"><table border="0" cellpadding="0" cellspacing="0" style="height: 300px; width: 290px; ">
                                    <tr>
                                        <td class="tdBackColor" colspan="4" rowspan="10" style="height: 370px; text-align: center; width: 391px;"
                                valign="top">
                <table style="height: 370px">
                <tr>
                <td style="width: 173px; height: 335px; vertical-align: top;">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 216px; height: 360px;">
                                    <tr>
                                        <td colspan="3" style="height:16px;">
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 201px">
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
                                        <td style="width: 61px; height: 30px; text-align: right">
                                            <asp:Label ID="lblBuildingCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblBuildingCode %>"
                                                Width="60px"></asp:Label></td>
                                        <td style="height: 30px; width: 13px;">
                                           </td>
                                        <td style="height: 30px; width: 154px;">
                                            <asp:TextBox ID="txtBuildingCode" runat="server" CssClass="textstyle" ReadOnly="True" MaxLength="32"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 61px; height: 30px; text-align: right">
                                            <asp:Label ID="lblBuildingName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblBuildingName %>" Width="57px"></asp:Label></td>
                                        <td style="height: 30px; width: 13px;">
                                           </td>
                                        <td style="height: 30px; width: 154px;">
                        <asp:TextBox ID="txtBuildingName" runat="server" CssClass="textstyle" ReadOnly="True" MaxLength="128"></asp:TextBox></td>
                                    </tr>
                                    
                                 <tr>
                                        <td style="width: 61px; height: 30px; text-align: right">
                                            <asp:Label ID="lblBuildingSatus" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblLocationStatus %>"
                                                Width="57px"></asp:Label></td>
                                        <td style="height: 30px; width: 13px;">
                                           </td>
                                        <td style="height: 30px; width: 154px;">
                                            <asp:DropDownList ID="cmbBuildingStatus" runat="server" CssClass="textstyle" Enabled="False">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td style="height: 9px; text-align: center" colspan="3">
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 201px; text-align: center">
                                                <tr>
                                                    <td style="width: 240px; height: 1px; background-color: #738495">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 240px; height: 1px; background-color: #ffffff">
                                                    </td>
                                                </tr>
                                            </table>
                                            </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 61px; height: 30px; text-align: right">
                                            <asp:Label ID="lblFloorCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblFloorCode %>" Width="59px"></asp:Label></td>
                                        <td style="height: 30px; width: 13px;">
                                            </td>
                                        <td style="height: 30px; width: 154px;">
                                            <asp:TextBox ID="txtFloorCode" runat="server" CssClass="textstyle" ReadOnly="True" MaxLength="32"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 61px; height: 30px; text-align: right">
                                            <asp:Label ID="lblFloorName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblFloorName %>" Width="57px"></asp:Label></td>
                                        <td style="height: 30px; width: 13px;">
                                            </td>
                                        <td style="height: 30px; width: 154px;">
                                            <asp:TextBox ID="txtFloorName" runat="server" CssClass="textstyle" ReadOnly="True" MaxLength="64"></asp:TextBox></td>
                                    </tr>
                                                                        <tr>
                                        <td style="width: 61px; height: 30px; text-align: right">
                                            <asp:Label ID="lblFloorStatus" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblLocationStatus %>" Width="57px"></asp:Label></td>
                                        <td style="height: 30px; width: 13px;">
                                            </td>
                                        <td style="height: 30px; width: 154px;">
                                            <asp:DropDownList ID="cmbFloorStatus" runat="server" CssClass="textstyle" Enabled="False">
                                            </asp:DropDownList></td>
                                    </tr>
                                                                        <tr>
                                                                            <td colspan="3" style="height: 9px">
                                                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 201px; text-align: center">
                                                                                    <tr>
                                                                                        <td style="width: 240px; height: 1px; background-color: #738495">
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="width: 240px; height: 1px; background-color: #ffffff">
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                    </tr>
                                    
                                                                        <tr>
                                        <td style="width: 61px; height: 30px; text-align: right">
                                            <asp:Label ID="lblLocationCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblLocationCode %>" Width="57px"></asp:Label></td>
                                        <td style="height: 30px; width: 13px;">
                                            </td>
                                        <td style="height: 30px; width: 154px;">
                                            <asp:TextBox ID="txtLocationCode" runat="server" CssClass="textstyle" ReadOnly="True" MaxLength="32"></asp:TextBox></td>
                                    </tr>
                                                                        <tr>
                                        <td style="width: 61px; height: 30px; text-align: right">
                                            <asp:Label ID="lblLocationName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblLocationName %>"
                                                Width="59px"></asp:Label></td>
                                        <td style="height: 30px; width: 13px;">
                                            </td>
                                        <td style="height: 30px; width: 154px;">
                                            <asp:TextBox ID="txtLocationName" runat="server" CssClass="textstyle" ReadOnly="True" MaxLength="64"></asp:TextBox></td>
                                    </tr>
                                                                                                            <tr>
                                        <td style="width: 61px; height: 30px; text-align: right">
                                            <asp:Label ID="lblLocationStatus" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblLocationStatus %>"
                                                Width="59px"></asp:Label></td>
                                        <td style="height: 30px; width: 13px;">
                                            </td>
                                        <td style="height: 30px; width: 154px;">
                                            <asp:DropDownList ID="cmbLocationStatus" runat="server" CssClass="textstyle" Enabled="False">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" rowspan="2" style="height: 15px"><table border="0" cellpadding="0" cellspacing="0" style="width: 201px">
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
                                    </tr>
                                    <tr>
                                        <td colspan="3" rowspan="2" style="height: 17px; text-align:right;">
                                            <asp:Button ID="btnEdit" runat="server" CssClass="buttonEdit" OnClick="btnEdit_Click"
                                                Text="<%$ Resources:BaseInfo,User_btnChang %>" Enabled="False" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>&nbsp;<asp:Button ID="btnSave"
                                                        runat="server" CssClass="buttonSave" OnClick="btnSave_Click" Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" Enabled="False" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>&nbsp;<asp:Button
                                                            ID="btnCancel" runat="server" CssClass="buttonCancel" OnClick="btnCancel_Click"
                                                            onmouseout="BtnUp(this.id);" onmouseover="BtnOver(this.id);" onmouseup="BtnUp(this.id);"
                                                            Text="<%$ Resources:BaseInfo,User_btnCancel %>" />&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                    </tr>
                                    <tr>
                                    </tr>
                                </table>
                </td>
                </tr>
                </table>
                                </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
           
          
                </td>
            </tr>
        </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:HiddenField ID="hidNoBuilding" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidNoBuilding %>" />
        <asp:HiddenField ID="hidYesFloor" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidYesFloor %>" />
        <asp:HiddenField ID="hidNotNullFloor" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidNotNullFloor %>" />
        <asp:HiddenField ID="hidYesBulidingCode" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidYesBulidingCode %>" />
        <asp:HiddenField ID="hidYesLocationCode" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidYesLocationCode %>" />
        <asp:HiddenField ID="hidNotNullLocation" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidNotNullLocation %>" />
        <asp:HiddenField id="hidUpdateLost" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidUpdateLost %>"></asp:HiddenField>
        <asp:HiddenField id="hidAdd" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidAdd %>"></asp:HiddenField>
        <asp:HiddenField ID="hidMessage" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidMessage %>" />
        <asp:HiddenField ID="hidlblUnitit" runat="server" Value="<%$ Resources:BaseInfo,Menu_DefineMallGeographicalInformation %>" />
    </form>
</body>
</html>





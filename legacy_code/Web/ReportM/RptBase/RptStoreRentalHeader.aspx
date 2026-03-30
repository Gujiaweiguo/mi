<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RptStoreRentalHeader.aspx.cs" Inherits="ReportM_RptBase_RptStoreRentalHeader" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml"  >
<head id="Head1" runat="server">
    <title><%=strBaseInfo %></title>
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        <!--
            table.mainTbl {width:572px;height:401px;}
            
            tr{height:28px;}
            td.lable{padding-right:5px;text-align:right;}
            
        -->
    </style>
    <script type="text/javascript" src="../../JavaScript/Common.js"></script>
    <script type="text/javascript" src="../../JavaScript/setPeriod.js" charset="gb2312"></script>
        <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
        <script type="text/javascript">
        function Load()
	    {
	        addTabTool("<%=strBaseInfo %>,ReportM/RptBase/RptStoreRentalHeader.aspx");
	        loadTitle();
	    }
	</script>
</head>
<body style="margin:0px"  onload ="Load();">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="1200">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                                            <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="width:5px" class="tdTopRightBackColor">
                                                     <img class="imageLeftBack" />
                                                    </td>
                                                    <td class="tdTopRightBackColor" style="text-align:left;">
                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo,Rpt_StoreRentalInfo %>"></asp:Label>
                                                    </td>
                                                    <td style="width:5px" class="tdTopRightBackColor">
                                                        <img class="imageRightBack"/>
                                                    </td>
                                                </tr>
                                                <tr style="height:1px">
                                                    <td colspan="3" style="background-color:White; height:1px">
                                                    </td>
                                                </tr>
                                            </table>
                                            <table style="width:100%" class="tdBackColor">
                                                <tr style="height:10px">
                                                    <td style="width: 89px">
                                                    </td>
                                                    <td style="width: 218px">
                                                        
                                                    </td>
                                                    <td style="width: 14px">
                                                    </td>
                                                    <td style="width: 1055px">
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px" class="lable">
                                                        <asp:Label ID="Label14" runat="server" CssClass="labelStyle" 
                                                            Text="<%$ Resources:BaseInfo,PotCustomer_BusinessItem %>" Width="50px"></asp:Label>
                                                        </td>
                                                    <td style="width: 218px">
                                                        <asp:DropDownList ID="ddlStoreName" runat="server" AutoPostBack="True" 
                                                            OnSelectedIndexChanged="ddlStoreName_SelectedIndexChanged" Width="165px">
                                                        </asp:DropDownList>
                                                        </td>
                                                    <td style="width: 14px">
                                                        <asp:Label ID="Label13" runat="server" CssClass="labelStyle" 
                                                            Text="<%$ Resources:BaseInfo,Rpt_Sumtype %>" Width="73px"></asp:Label>
                                                        </td>
                                                    <td style="width: 1055px">
                                                        <asp:RadioButton ID="rdo1" runat="server" AutoPostBack="True" Checked="True" 
                                                            CssClass="labelStyle" GroupName="s" OnCheckedChanged="rdo1_CheckedChanged" 
                                                            Text="<%$ Resources:BaseInfo,Rpt_StoreRentInfoDetail %>" />
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px">
                                                        <asp:Label ID="Label12" runat="server" CssClass="labelStyle" 
                                                            Text="<%$ Resources:BaseInfo,InvAdj_KeepAccountsMth %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 218px">
                                                        <asp:TextBox ID="txtPeriod" runat="server" CssClass="ipt160px" 
                                                            onclick="calendar()"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 14px">
                                                        &nbsp;</td>
                                                    <td style="width: 1055px">
                                                        <asp:RadioButton ID="rdo2" runat="server" AutoPostBack="True" 
                                                            CssClass="labelStyle" GroupName="s" OnCheckedChanged="rdo2_CheckedChanged" 
                                                            Text="<%$ Resources:BaseInfo,Rpt_StoreRentInfo %>" />
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px">
                                                        <asp:Label ID="labBuildingName" runat="server" CssClass="labelStyle" 
                                                            Text="<%$ Resources:BaseInfo,RentableArea_lblBuildingName %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 218px">
                                                        <asp:DropDownList ID="ddlBuildingName" runat="server" AutoPostBack="True" 
                                                            OnSelectedIndexChanged="ddlBuildingName_SelectedIndexChanged1" Width="165px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="width: 14px">
                                                    </td>
                                                    <td style="width: 1055px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px">
                                                        <asp:Label ID="labFloorName" runat="server" CssClass="labelStyle" 
                                                            Text="<%$ Resources:BaseInfo,RentableArea_lblFloorName %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 218px">
                                                        <asp:DropDownList ID="ddlFloorName" runat="server" Width="165px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="width: 14px">
                                                    </td>
                                                    <td style="width: 1055px">
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 25px;">
                                                        <asp:Label ID="labShopType" runat="server" CssClass="labelStyle" 
                                                            Text="<%$ Resources:BaseInfo,PotShop_lblShopType %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 218px; height: 25px;">
                                                        <asp:DropDownList ID="ddlShopType" runat="server" BackColor="White" 
                                                            Width="165px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="width: 14px; height: 25px;">
                                                    </td>
                                                    <td style="height: 25px; width: 1055px;">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 21px;" class="lable">
                                                        &nbsp;</td>
                                                    <td style="width: 218px; height: 21px;">
                                                        <asp:Button ID="btnQuery" runat="server" CssClass="buttonQuery" 
                                                            OnClick="btnOK_Click" onmouseout="BtnUp(this.id);" 
                                                            onmouseover="BtnOver(this.id);" onmouseup="BtnUp(this.id);" 
                                                            Text="<%$ Resources:BaseInfo,User_lblQuery %> " />
                                                        <asp:Button ID="BtnCancel" runat="server" CssClass="buttonCancel" 
                                                            OnClick="BtnCel_Click" onmouseout="BtnUp(this.id);" 
                                                            onmouseover="BtnOver(this.id);" onmouseup="BtnUp(this.id);" 
                                                            Text="<%$ Resources:BaseInfo,User_btnCancel %> " />
                                                    </td>
                                                    <td style="width: 14px; height: 21px;">
                                                        </td>
                                                    <td style="height: 21px; width: 1055px;">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 21px;" class="lable">
                                                        </td>
                                                    <td style="width: 218px; height: 21px;">
                                                        </td>
                                                    <td style="width: 14px; height: 21px;" class="lable" align="right">
                                                        </td>
                                                    <td style="height: 21px; width: 1055px;">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 22px;">
                                                        </td>
                                                    <td style="width: 218px; height: 22px;">
                                                        &nbsp;</td>
                                                    <td align="right" class="lable" style="width: 14px; height: 22px;">
                                                    </td>
                                                    <td style="height: 22px; width: 1055px;">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px">
                                                        </td>
                                                    <td style="width: 218px">
                                                        &nbsp;</td>
                                                    <td align="right" class="lable" style="width: 14px">
                                                        &nbsp;</td>
                                                    <td style="width: 1055px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 26px;" class="lable">
                                                        </td>
                                                    <td style="width: 218px; height: 26px;">
                                                        </td>
                                                    <td style="width: 14px; height: 26px;">
                                                    </td>
                                                    <td style="height: 26px; width: 1055px;">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 15px;" class="lable">
                                                        </td>
                                                    <td style="width: 218px; height: 15px;">
                                                        </td>
                                                    <td style="width: 14px; height: 15px;">
                                                    </td>
                                                    <td style="height: 15px; width: 1055px;">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 28px;">
                                                        </td>
                                                    <td style="width: 218px; height: 28px;">
                                                        </td>
                                                    <td style="width: 14px; height: 28px;">
                                                    </td>
                                                    <td style="height: 28px; width: 1055px;">
                                                        </td>
                                                </tr>
                                            </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        &nbsp;
    </form>
</body>
</html>
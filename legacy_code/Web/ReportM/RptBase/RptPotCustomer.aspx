<!--
编写人：何思键  English Name: Bruce
编写时间:2009年3月24日
-->
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RptPotCustomer.aspx.cs" Inherits="Report_PotCustomer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%=baseInfo %></title>
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/longCss/webtab.css" rel="stylesheet" type="text/css"/>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    
    <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
    <script type="text/javascript"  src="../../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript" src="../../JavaScript/Calendar.js" charset="gb2312"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"> </script>
	<style type="text/css">
        <!--
            table.mainTbl {width:572px;height:401px;}
            
            tr{height:28px;}
            td.lable{padding-right:5px;text-align:right;}
            
        -->
    </style> 
    <script type="text/javascript">
        function Load()
        {
            addTabTool("<%=strFresh %>,ReportM/RptBase/RptPotCustomer.aspx");
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
<body style="margin:0px" onload="Load()">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                               <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
                                   <tr>
                                       <td style="width:5px" class="tdTopRightBackColor">
                                            <img class="imageLeftBack" />
                                       </td>
                                       <td class="tdTopRightBackColor" style="text-align:left;">
                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:ReportInfo, Menu_PotCustomer %>">
                                            </asp:Label>
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
                                        <td style="width:15%"></td>
                                        <td style="width:40%"></td>
                                        <td style="width:25%"></td>
                                        <td style="width:20%"></td>
                                    </tr>
<!--商业项目-->
                                    <tr class="bodyTbl">
                                        <td style="width:15%" class="lable">
                                            <asp:Label ID="label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_BusinessItem %>">
                                            </asp:Label>
                                        </td>
                                        <td style="width:40%">
                                            <asp:DropDownList ID="txtBizproject" runat="server" width="157px" AutoPostBack="true" OnSelectedIndexChanged="txtBizproject_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width:25%">
                                        </td>
                                        <td style="width:20%">
                                        </td>
                                    </tr>
<!--大楼-->
                                    <tr class="bodyTb1">
                                        <td style="width:15%" class="lable">
                                            <asp:Label ID="label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContractAuditing_labAttract %>">
                                            </asp:Label>&nbsp;</td>
                                        <td style="width:40%">
                                            <asp:TextBox ID="txtBizPerson" runat="server" CssClss="ipt160px" MaxLength="16"></asp:TextBox></td>
                                        <td style="width:25%">
                                        </td>
                                        <td style="width:20%">
                                        </td>
                                    </tr>
<!--楼层-->
                                    <tr class="bodyTb1">
                                        <td style="width:15%" class="lable">
                                            <asp:Label ID="label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_CustSource %>">
                                            </asp:Label>&nbsp;</td>
                                        <td style="width:40%">
                                            <asp:DropDownList ID="txtCustFrom" runat="server" width="157px">
                                            </asp:DropDownList></td>
                                        <td style="width:25%">
                                        </td>
                                        <td style="width:20%">
                                        </td>
                                    </tr>
<!--招商人员-->
                                     <tr class="bodyTb1">
                                        <td style="width:15%" class="lable">
                                            <asp:Label ID="label7" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblShopStartDate %>">
                                            </asp:Label>&nbsp;</td>
                                        <td style="width:40%">
                                            <asp:TextBox ID="txtStartDate" runat="server" CssClss="ipt160px" OnClick="calendar()">
                                            </asp:TextBox></td>
                                        <td style="width:25%">
                                        </td>
                                        <td style="width:20%">
                                        </td>
                                    </tr>
<!--商户来源-->
                                    <tr class="bodyTb1">
                                        <td style="width:15%" class="lable">
                                            <asp:Label ID="label8" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblShopEndDate %>">
                                            </asp:Label>&nbsp;</td>
                                        <td style="width:40%">
                                            <asp:TextBox ID="txtEndDate" runat="server" CssClss="ipt160px" OnClick="calendar()"></asp:TextBox></td>
                                        <td style="width:25%">
                                        </td>
                                        <td style="width:20%">
                                        </td>
                                    </tr>
<!--开始日期-->
                                   <tr class="bodyTb1">
                                       <td class="lable" style="width: 15%">
                                       </td>
                                       <td style="width: 40%">
                                       </td>
                                       <td style="width: 25%">
                                       </td>
                                       <td style="width: 20%">
                                       </td>
                                   </tr>
                                    <tr class="bodyTb1">
                                        <td style="width:15%" class="lable">
                                            &nbsp;</td>
                                        <td style="width:40%">
                                            &nbsp;<asp:Button ID="BtnQuery" runat="server" CssClass="buttonQuery" Text="<%$ Resources:BaseInfo,User_lblQuery %> " OnClick="BtnOK_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                                            <asp:Button ID="BtnCancel" runat="server" CssClass="buttonCancel" Text="<%$ Resources:BaseInfo,User_btnCancel %>" OnClick="BtnCel_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/></td>
                                        <td style="width:25%">
                                        </td>
                                        <td style="width:20%">
                                        </td>
                                    </tr>
<!--结束日期-->
                                    <tr class="bodyTb1">
                                        <td style="width:15%" class="lable">
                                            &nbsp;</td>
                                        <td style="width:40%">
                                            &nbsp;</td>
                                        <td style="width:25%">
                                        </td>
                                        <td style="width:20%">
                                        </td>
                                    </tr>

                                    <tr style="height:10px">
                                        <td style="width:15%"></td>
                                        <td style="width:40%"></td>
                                        <td style="width:25%"></td>
                                        <td style="width:20%"></td>
                                    </tr>
                                      <tr style="height:10px">
                                        <td style="width:15%"></td>
                                        <td style="width:40%"></td>
                                        <td style="width:25%"></td>
                                        <td style="width:20%"></td>
                                    </tr>
<!--按钮-->
                                     <tr style="height:10px">
                                        <td style="width:15%"></td>
                                        <td style="width:40%"></td>
                                        <td style="width:25%"></td>
                                        <td style="width:20%"></td>
                                    </tr>
                                     <tr style="height:10px">
                                        <td style="width:15%">
                                            <asp:Label ID="label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblBuildingName %>" Visible="False"></asp:Label><asp:DropDownList ID="txtBuilding" runat="server" width="157px" Visible="False">
                                            </asp:DropDownList><asp:Label ID="label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblFloorName %>" Visible="False"></asp:Label><asp:DropDownList ID="txtFloor" runat="server" width="157px" Visible="False">
                                            </asp:DropDownList></td>
                                        <td style="width:40%"></td>
                                        <td style="width:25%"></td>
                                        <td style="width:20%"></td>
                                    </tr>
                                     <tr style="height:10px">
                                        <td style="width:15%"></td>
                                        <td style="width:40%"></td>
                                        <td style="width:25%"></td>
                                        <td style="width:20%"></td>
                                    </tr>
                                     <tr style="height:10px">
                                        <td style="width:15%"></td>
                                        <td style="width:40%"></td>
                                        <td style="width:25%"></td>
                                        <td style="width:20%"></td>
                                    </tr>
                                   <tr style="height:10px">
                                        <td style="width:15%"></td>
                                        <td style="width:40%"></td>
                                        <td style="width:25%"></td>
                                        <td style="width:20%"></td>
                                    </tr>
                                      <tr style="height:10px">
                                        <td style="width:15%"></td>
                                        <td style="width:40%"></td>
                                        <td style="width:25%"></td>
                                        <td style="width:20%"></td>
                                    </tr>
                                      <tr style="height:10px">
                                        <td style="width:15%"></td>
                                        <td style="width:40%"></td>
                                        <td style="width:25%"></td>
                                        <td style="width:20%"></td>
                                    </tr>
                                      <tr style="height:10px">
                                        <td style="width:15%"></td>
                                        <td style="width:40%"></td>
                                        <td style="width:25%"></td>
                                        <td style="width:20%"></td>
                                    </tr>
                               </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>

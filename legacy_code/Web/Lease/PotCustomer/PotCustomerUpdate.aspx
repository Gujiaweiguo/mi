<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PotCustomerUpdate.aspx.cs" Inherits="Lease_PotCustomer_PotCustomerUpdate" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>无标题页</title>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
        <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/webtab.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript"  src="../../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	    <script type="text/javascript" src="../../JavaScript/Calendar.js" charset="gb2312"></script>
	<script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript" src="../../JavaScript/TabTools.js"></script>
	<script type="text/javascript">
 	    function chooseCard(id)
	    {
	    	document.getElementById("chooseDiv").style.position="absolute";
	        document.getElementById("chooseDiv").style.top ="408px";
	        document.getElementById("chooseDiv").style.left ="95px";
	        if(id==0){
	        	document.getElementById("PotCustomer").style.display ="none";
		        document.getElementById("PotCustLicense").style.display ="none";
	            document.getElementById("PotCustomerList").style.display ="block";

		        document.getElementById("PotCustomerList").style.position="absolute";
		        document.getElementById("PotCustomerList").style.top ="5px";
		        document.getElementById("PotCustLicense").style.position="absolute";
		        document.getElementById("PotCustLicense").style.top ="5px";
		        document.getElementById("PotCustomer").style.position="absolute";
		        document.getElementById("PotCustomer").style.top ="5px";
		        document.getElementById("showmain").style.height ="450px";
                
                document.getElementById("tab3").className="tab";
                document.getElementById("tab2").className="tab";
		        document.getElementById("tab1").className="selectedtab";
		        document.getElementById("lblTotalNum").style.display="none";
                document.getElementById("lblCurrent").style.display="none";
	        }
	        if(id==1){
	            document.getElementById("PotCustomerList").style.display ="none";
	            document.getElementById("PotCustomer").style.display ="block";
	        	document.getElementById("PotCustLicense").style.display ="none";
		        document.getElementById("PotCustomerList").style.position="absolute";
		        document.getElementById("PotCustomerList").style.top ="5px";
		        document.getElementById("PotCustLicense").style.position="absolute";
		        document.getElementById("PotCustLicense").style.top ="5px";
		        document.getElementById("PotCustomer").style.position="absolute";
		        document.getElementById("PotCustomer").style.top ="5px";
		        document.getElementById("tab3").className="tab";
                document.getElementById("tab1").className="tab";
		        document.getElementById("tab2").className="selectedtab";
	        }
	        if(id==2)
	        {
	            document.getElementById("PotCustomerList").style.display ="none";
	            document.getElementById("PotCustomer").style.display ="none";
	        	document.getElementById("PotCustLicense").style.display ="block";
		        document.getElementById("PotCustomerList").style.position="absolute";
		        document.getElementById("PotCustomerList").style.top ="5px";
		        document.getElementById("PotCustomer").style.position="absolute";
		        document.getElementById("PotCustomer").style.top ="5px";
		        document.getElementById("PotCustLicense").style.position="absolute";
		        document.getElementById("PotCustLicense").style.top ="5px";
                document.getElementById("tab2").className="tab";
                document.getElementById("tab1").className="tab";
		        document.getElementById("tab3").className="selectedtab";
		        document.getElementById("lblLTotalNum").style.display="none";
                document.getElementById("lblLCurrent").style.display="none";
		    }
		    addTabTool("null");
	    }
	    
	    function LicenseTextClear()
	    {
	        document.getElementById("txtLicenseID").value = "";
	        document.getElementById("txtLicenseName").value = "";
	        document.getElementById("txtLicenseBeginDate").value = "";
	        document.getElementById("txtLicenseEndDate").value = "";
	        return false;
	    }
    </script>
</head>
<body onload='chooseCard(0);' topmargin=0 leftmargin=0>
    <form id="form1" runat="server">
        <div style="left: 85px; overflow: auto; width: 272px; position: absolute; top: 840px;
            height: 55px" id="chooseDiv">
            <table>
                <tbody>
                    <tr>
                        <td style="height: 21px; width: 70px;" id="tab1" class="selectedtab" onclick="chooseCard(0);">
                            <span id="tabpage1">
                                <asp:Label ID="lblUserList" runat="server" Text="<%$ Resources:BaseInfo,Tool_UserList %>"
                                    Width="64px"></asp:Label></span></td>
                        <td style="height: 21px; width: 73px;" id="tab2" class="tab" onclick="chooseCard(1);">
                            <span id="tabpage2">
                                <asp:Label ID="hidBasic" runat="server" Text="<%$ Resources:BaseInfo,PotCustomer_Basic %>"
                                    Width="63px"></asp:Label></span></td>
                        <td id="tab3" class="tab" onclick="chooseCard(2);" style="height: 21px; width: 72px;">
                            <span id="tabpage3">
                                <asp:Label ID="hidClientCard" runat="server" Text="<%$ Resources:BaseInfo,PotCustomer_ClientCard %>"
                                    Width="65px"></asp:Label></span></td>
                    </tr>
                </tbody>
            </table>
        </div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <table id="showmain" border="0" cellpadding="0" cellspacing="0" class="tableBoderStyle"
            style="height: 445px">
            <tr height="15">
                <td colspan="8">
                </td>
            </tr>
            <tr>
                <td style="width: 95px; height: 401px; text-align: center" valign="top">
                    <img height="401" src="../../images/shuxian.jpg" />
                </td>
                <td colspan="5" style="vertical-align: top; width: 572px; height: 401px">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
            <div id="PotCustomerList" style="width: 100px; height: 100px">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 553px; height: 405px">
                    <tbody>
                        <tr>
                            <td class="tdTopBackColor" style="width: 892px; height: 25px; vertical-align: middle; text-align: left;" valign="top">
                                <img alt="" class="imageLeftBack" />
                                <asp:Label ID="labCustomer" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,PotCustomer_labCustomerUptate %>" Width="408px"></asp:Label></td>
                            <td class="tdTopRightBackColor" colspan="2" style="width: 538px; height: 25px; text-align: right"
                                valign="top">
                                <img alt="" class="imageRightBack" /></td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="3" style="width: 535px; height: 329px; text-align: center"
                                valign="top">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 551px; height: 380px">
                                    <tbody>
                                        <tr>
                                            <td colspan="8" style="width: 533px; height: 1px; background-color: white">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdBackColor" colspan="8" style="width: 495px; height: 5px">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                                <asp:Label ID="Label1" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustType %>"></asp:Label></td>
                                            <td class="tdBackColor" style="width: 8px; height: 22px">
                                            </td>
                                            <td class="tdBackColor" style="width: 137px; height: 22px; text-align: left">
                                                <asp:DropDownList ID="cmbCustTypeq" runat="server" Width="133px">
                                                </asp:DropDownList></td>
                                            <td class="tdBackColor" style="height: 22px">
                                            </td>
                                            <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                                <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustShortName %>"></asp:Label></td>
                                            <td class="tdBackColor" style="width: 5px; height: 22px">
                                            </td>
                                            <td class="tdBackColor" style="width: 120px; height: 22px; text-align: left">
                                                <asp:TextBox ID="TextBox2" runat="server" CssClass="textstyle"></asp:TextBox></td>
                                            <td class="tdBackColor" style="width: 100px; height: 22px">
                                                <asp:Button ID="btnQuery" runat="server" CssClass="buttonQuery" OnClick="btnQuery_Click"
                                                    Text="<%$ Resources:BaseInfo,User_lblQuery %>" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdBackColor" colspan="8" style="width: 495px; height: 12px; text-align: center">
                                                <table border="0" cellpadding="0" cellspacing="0" style="left: 12px; width: 526px;
                                                    position: relative">
                                                    <tbody>
                                                        <tr>
                                                            <td style="width: 160px; height: 1px; background-color: #738495">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 160px; height: 1px; background-color: #ffffff">
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdBackColor" colspan="8" style="vertical-align: top; width: 495px; height: 260px;
                                                text-align: center">
                                                <table style="width: 549px; height: 260px">
                                                    <tbody>
                                                        <tr>
                                                            <td style="left: 7px; vertical-align: top; width: 533px; position: relative; text-align: center">
                                                                <asp:GridView ID="GrdCust" runat="server" AutoGenerateColumns="False" BackColor="White" BorderStyle="Inset" BorderWidth="1px" CellPadding="3" Height="258px"
                                                                    OnRowDataBound="GrdCust_RowDataBound" OnSelectedIndexChanged="GrdCust_SelectedIndexChanged"
                                                                    Width="531px">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="CustID">
                                                                            <ItemStyle CssClass="hidden" />
                                                                            <HeaderStyle CssClass="hidden" />
                                                                            <FooterStyle CssClass="hidden" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="CustCode" HeaderText="<%$ Resources:BaseInfo,PotCustomer_lblCustCode %>">
                                                                            <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                            <ItemStyle BorderColor="#E1E0B2" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="CustName" HeaderText="<%$ Resources:BaseInfo,PotCustomer_lblCustName %>">
                                                                            <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                            <ItemStyle BorderColor="#E1E0B2" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="CustShortName" HeaderText="<%$ Resources:BaseInfo,PotCustomer_lblCustShortName %>">
                                                                            <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                            <ItemStyle BorderColor="#E1E0B2" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="CustTypeName" HeaderText="<%$ Resources:BaseInfo,PotCustomer_lblCustType %>">
                                                                            <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                            <ItemStyle BorderColor="#E1E0B2" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:BaseInfo,PotCustomer_ADDArchives %>">
                                                                            <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                            <ItemStyle BorderColor="#E1E0B2" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="ContactorName" HeaderText="<%$ Resources:BaseInfo,PotCustomer_Contact %>">
                                                                            <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                            <ItemStyle BorderColor="#E1E0B2" />
                                                                        </asp:BoundField>
                                                                        <asp:CommandField HeaderText="<%$ Resources:BaseInfo,User_btnChang %>" ShowSelectButton="True">
                                                                            <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                            <ItemStyle BorderColor="#E1E0B2" />
                                                                        </asp:CommandField>
                                                                    </Columns>
                                                    <FooterStyle BackColor="Red" ForeColor="#000066"/>
                                                <RowStyle ForeColor="Black" Height="10px" Font-Overline="False" Font-Size="10pt" />
                                                <SelectedRowStyle BackColor="#FFFFCD" Font-Bold="False" ForeColor="Black" />
                                                <PagerStyle BackColor="White" ForeColor="#000066"   HorizontalAlign="Left" />
                                                <HeaderStyle BackColor="#E1E0B2" Height="10px" Font-Bold="False"  />
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdBackColor" style="width: 55px; height: 12px">
                                            </td>
                                            <td class="tdBackColor" style="width: 8px; height: 12px">
                                            </td>
                                            <td class="tdBackColor" style="width: 137px; height: 12px">
                                                <asp:Label ID="lblTotalNum" runat="server"></asp:Label>
                                                <asp:Label ID="lblCurrent" runat="server" ForeColor="Red">1</asp:Label></td>
                                            <td class="tdBackColor" style="height: 22px">
                                            </td>
                                            <td class="tdBackColor" colspan="4" style="left: 30px; vertical-align: top; width: 270px;
                                                height: 44px; text-align: right">
                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 283px">
                                                    <tbody>
                                                        <tr>
                                                            <td style="left: 25px; width: 160px; position: relative; height: 1px; background-color: #738495">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="left: 25px; width: 160px; position: relative; height: 1px; background-color: #ffffff">
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                                <table>
                                                    <tr>
                                                        <td style="left: 40px; position: relative; height: 37px">
                                                            &nbsp;<asp:Button ID="btnBack" runat="server" CssClass="buttonBack" Enabled="False"
                                                                OnClick="btnBack_Click" Text="<%$ Resources:BaseInfo,Button_back %>" /><asp:Button
                                                                    ID="btnNext" runat="server" CssClass="buttonNext" Enabled="False" OnClick="btnNext_Click"
                                                                    Text="<%$ Resources:BaseInfo,Button_next %>" /></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
                <div id="PotCustomer" style="width: 100px; height: 100px">
             <table border="0" cellpadding="0" cellspacing="0" style="height: 405px; width: 553px;" width="535" id="TABLE1" >
                        <tr>
                            <td class="tdTopBackColor" style="width: 254px; height: 25px; vertical-align: middle; text-align: left;" valign="top">
                                <img alt="" class="imageLeftBack" />
                                <asp:Label ID="Label4" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,PotCustomer_labCustomerUptate %>" Width="239px"></asp:Label></td>
                            <td class="tdTopRightBackColor" colspan="2" style="width: 528px; height: 25px" valign="top">
                                <img class="imageRightBack" /></td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="2" style="width: 255px; height: 320px; text-align: left"
                                valign="top">
                                <table border="0" cellpadding="0" cellspacing="0" style="height: 310px" width="255">
                                    <tr>
                                        <td colspan="4" style="height: 1px; background-color: white">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" colspan="4" style="height: 5px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" colspan="4" style="height: 1px; text-align: left; left: 100px; vertical-align: bottom;">
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 165px">
                                                <tr>
                                                    <td style="width: 166px; height: 1px; background-color: #738495; left: 90px; position: relative; top: 3px;">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 166px; height: 1px; background-color: #ffffff; left: 90px; position: relative; top: 3px;">
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                                                        <tr>
                                        <td class="tdBackColor" colspan="4" style="height: 5px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                            <asp:Label ID="lblCustCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustCode %>"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 22px">
                                        </td>
                                        <td class="tdBackColor" style="width: 160px; height: 22px">
                                            <asp:TextBox ID="txtCustCode" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                        <td class="tdBackColor" style="width: 4px; height: 22px">
                                            </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                            <asp:Label ID="lblCustName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustName %>"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 22px">
                                        </td>
                                        <td class="tdBackColor" style="width: 160px; height: 22px">
                                            <asp:TextBox ID="txtCustName" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                        <td class="tdBackColor" style="width: 4px; height: 22px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                            <asp:Label ID="lblCustShortName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustShortName %>"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 22px">
                                        </td>
                                        <td class="tdBackColor" style="width: 160px; height: 22px">
                                            <asp:TextBox ID="txtCustShortName" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                        <td class="tdBackColor" style="width: 4px; height: 22px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                            <asp:Label ID="lblCustType" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustType %>"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 22px">
                                        </td>
                                        <td class="tdBackColor" style="width: 160px; height: 22px">
                                            <asp:DropDownList ID="cmbCustType" runat="server" Width="164px" >
                                            </asp:DropDownList></td>
                                        <td class="tdBackColor" style="width: 4px; height: 22px">
                                        </td>
                                    </tr>
                                     <tr>
                                       <td style="width:255px; height:5px;" class="tdBackColor" colspan="4"></td>
                                     </tr>
                                    <tr>
                                        <td class="tdBackColor" style="height: 1px; text-align: left; vertical-align: bottom;" colspan="4">
                                        </td>
                                    </tr>
                                                                         <tr>
                                       <td style="width:255px; height:5px; vertical-align: middle;" class="tdBackColor" colspan="4">
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 166px">
                                                <tr>
                                                    <td style="width: 166px; height: 1px; background-color: #738495; left: 90px; position: relative;">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 166px; height: 1px; background-color: #ffffff; left: 90px; position: relative;">
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                     </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                            <asp:Label ID="lblLegalRep" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblLegalRep %>"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 22px">
                                        </td>
                                        <td class="tdBackColor" style="width: 160px; height: 22px">
                                            <asp:TextBox ID="txtLegalRep" runat="server" CssClass="ipt160px" MaxLength="16"></asp:TextBox></td>
                                        <td class="tdBackColor" style="width: 4px; height: 22px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                            <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblLegalRepTitle %>"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 22px">
                                        </td>
                                        <td class="tdBackColor" style="width: 160px; height: 22px">
                                            <asp:TextBox ID="txtLegalRepTitle" runat="server" CssClass="ipt160px" MaxLength="32"></asp:TextBox></td>
                                        <td class="tdBackColor" style="width: 4px; height: 22px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                            <asp:Label ID="lblRegCap" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblRegCap %>"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 22px">
                                        </td>
                                        <td class="tdBackColor" style="width: 160px; height: 22px">
                                            <asp:TextBox ID="txtRegCap" runat="server" CssClass="ipt160px" MaxLength="19"></asp:TextBox></td>
                                        <td class="tdBackColor" style="width: 4px; height: 22px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                            <asp:Label ID="lblRegAddr" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblRegAddr %>"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 22px">
                                        </td>
                                        <td class="tdBackColor" style="width: 160px; height: 22px">
                                            <asp:TextBox ID="txtRegAddr" runat="server" CssClass="ipt160px" MaxLength="64"></asp:TextBox></td>
                                        <td class="tdBackColor" style="width: 4px; height: 22px">
                                        </td>
                                    </tr>
                                   <tr>
                                       <td style="width:255px; height:5px;" class="tdBackColor" colspan="4"></td>
                                     </tr>
                                    <tr>
                                        <td class="tdBackColor" colspan="4" style="height: 1px; text-align: center">
                                        </td>
                                    </tr>
                                     <tr>
                                       <td style="width:255px; height:5px;" class="tdBackColor" colspan="4">
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 166px; vertical-align: middle;">
                                                <tr>
                                                    <td style="width: 166px; height: 1px; background-color: #738495; left: 90px; position: relative;">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 166px; height: 1px; background-color: #ffffff; left: 90px; position: relative;">
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                     </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 85px; height: 22px">
                                            <asp:Label ID="lblRegCode" runat="server" CssClass="labelStyle" Height="18px" Text="<%$ Resources:BaseInfo,PotCustomer_lblRegCode %>"
                                                Width="88px"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 22px">
                                        </td>
                                        <td class="tdBackColor" style="width: 160px; height: 22px">
                                            <asp:TextBox ID="txtRegCode" runat="server" CssClass="ipt160px" MaxLength="32"></asp:TextBox></td>
                                        <td class="tdBackColor" style="width: 4px; height: 22px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                            <asp:Label ID="lblTaxCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblTaxCode %>"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 22px">
                                        </td>
                                        <td class="tdBackColor" style="width: 160px; height: 22px">
                                            <asp:TextBox ID="txtTaxCode" runat="server" CssClass="ipt160px" MaxLength="32"></asp:TextBox></td>
                                        <td class="tdBackColor" style="width: 4px; height: 22px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                            <asp:Label ID="lblBankName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblBankName %>"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 22px">
                                        </td>
                                        <td class="tdBackColor" style="width: 160px; height: 22px">
                                            <asp:TextBox ID="txtBankName" runat="server" CssClass="ipt160px" MaxLength="64"></asp:TextBox></td>
                                        <td class="tdBackColor" style="width: 4px; height: 22px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                            <asp:Label ID="lblBankAcct" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblBankAcct %>"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 22px">
                                        </td>
                                        <td class="tdBackColor" style="width: 160px; height: 22px">
                                            <asp:TextBox ID="txtBankAcct" runat="server" CssClass="ipt160px" MaxLength="32"></asp:TextBox></td>
                                        <td class="tdBackColor" style="width: 4px; height: 22px">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="tdBackColor" style="width: 280px; height: 320px" valign="top">
                                <table border="0" cellpadding="0" cellspacing="0" style="height: 100px" width="280">
                                    <tr>
                                        <td colspan="4" style="width: 280px; height: 1px; background-color: white">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" colspan="4" style="width: 280px; height: 10px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 85px; height: 1px">
                                        </td>
                                        <td class="tdBackColor" style="width: 5px; height: 1px">
                                        </td>
                                        <td colspan="2">
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 170px">
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
                                        <td class="tdBackColor" colspan="4" style="width: 255px; height: 2px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                            <asp:Label ID="lblOfficeAddr" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblOfficeAddr %>"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 22px">
                                        </td>
                                        <td class="tdBackColor" style="width: 160px; height: 22px">
                                            <asp:TextBox ID="txtOfficeAddr" runat="server" CssClass="ipt160px" MaxLength="64"></asp:TextBox></td>
                                        <td class="tdBackColor" style="width: 30px; height: 22px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                            <asp:Label ID="lblPostAddr" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblPostAddr %>"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 22px">
                                        </td>
                                        <td class="tdBackColor" style="width: 160px; height: 22px">
                                            <asp:TextBox ID="txtPostAddr" runat="server" CssClass="ipt160px" MaxLength="64"></asp:TextBox></td>
                                        <td class="tdBackColor" style="width: 30px; height: 22px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                            <asp:Label ID="lblPostCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblPostCode %>"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 22px">
                                        </td>
                                        <td class="tdBackColor" style="width: 160px; height: 22px">
                                            <asp:TextBox ID="txtPostCode" runat="server" CssClass="ipt160px" MaxLength="16"></asp:TextBox></td>
                                        <td class="tdBackColor" style="width: 30px; height: 22px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                            <asp:Label ID="lblWeb" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblWeb %>"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 22px">
                                        </td>
                                        <td class="tdBackColor" style="width: 160px; height: 22px">
                                            <asp:TextBox ID="txtWeb" runat="server" CssClass="ipt160px" MaxLength="128"></asp:TextBox></td>
                                        <td class="tdBackColor" style="width: 30px; height: 22px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 85px; height: 1px">
                                        </td>
                                        <td class="tdBackColor" style="width: 5px; height: 1px">
                                        </td>
                                        <td class="tdBackColor" style="width: 160px; height: 5px">
                                        </td>
                                        <td class="tdBackColor" style="width: 30px; height: 1px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 85px; height: 1px">
                                        </td>
                                        <td class="tdBackColor" style="width: 5px; height: 1px">
                                        </td>
                                        <td colspan="2">
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 170px">
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
                                        <td class="tdBackColor" colspan="4" style="width: 255px; height: 2px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                            <asp:Label ID="lblContactorName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblContactorName %>"
                                                Width="86px"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 22px">
                                        </td>
                                        <td class="tdBackColor" style="width: 160px; height: 22px">
                                            <asp:TextBox ID="txtContactorName" runat="server" CssClass="ipt160px" MaxLength="32"></asp:TextBox></td>
                                        <td class="tdBackColor" style="width: 30px; height: 22px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                            <asp:Label ID="lblTitle" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblTitle %>"
                                                Width="83px"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 22px">
                                        </td>
                                        <td class="tdBackColor" style="width: 160px; height: 22px">
                                            <asp:TextBox ID="txtTitle" runat="server" CssClass="ipt160px" MaxLength="32"></asp:TextBox></td>
                                        <td class="tdBackColor" style="width: 30px; height: 22px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                            <asp:Label ID="lblOfficeTel" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblOfficeTel %>"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 22px">
                                        </td>
                                        <td class="tdBackColor" style="width: 160px; height: 22px">
                                            <asp:TextBox ID="txtOfficeTel" runat="server" CssClass="ipt160px" MaxLength="16"></asp:TextBox></td>
                                        <td class="tdBackColor" style="width: 30px; height: 22px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                            <asp:Label ID="lblMobileTel" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblMobileTel %>"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 22px">
                                        </td>
                                        <td class="tdBackColor" style="width: 160px; height: 22px">
                                            <asp:TextBox ID="txtMobileTel" runat="server" CssClass="ipt160px" MaxLength="32"></asp:TextBox></td>
                                        <td class="tdBackColor" style="width: 30px; height: 22px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                            <asp:Label ID="lblEMail" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblEMail %>"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 22px">
                                        </td>
                                        <td class="tdBackColor" style="width: 160px; height: 22px">
                                            <asp:TextBox ID="txtEMail" runat="server" CssClass="ipt160px" MaxLength="128"></asp:TextBox></td>
                                        <td class="tdBackColor" style="width: 30px; height: 22px">
                                        </td>
                                    </tr>
                                    <tr>
                                                    <td class="tdBackColor" style="width: 85px; height: 1px">
                                                    </td>
                                                    <td class="tdBackColor" style="width: 5px; height: 1px">
                                                    </td>
                                                    <td class="tdBackColor" style="width: 160px; height: 5px">
                                                    </td>
                                                    <td class="tdBackColor" style="width: 30px; height: 1px">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdBackColor" style="width: 85px; height: 1px">
                                                    </td>
                                                    <td class="tdBackColor" style="width: 5px; height: 1px">
                                                    </td>
                                                    <td colspan="2">
                                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 170px">
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
                                                    <td class="tdBackColor" colspan="4" style="width: 255px; height: 5px">
                                                    </td>
                                                </tr>
                                                                                                <tr>
                                                    <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                                        <asp:Label ID="labAttract" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContractAuditing_labAttract %>"></asp:Label></td>
                                                    <td class="tdBackColor" style="width: 6px; height: 22px">
                                                    </td>
                                                    <td class="tdBackColor" style="width: 160px; height: 22px">
                                                        <asp:DropDownList ID="cmbCommOper" runat="server" CssClass="ipt160px">
                                                        </asp:DropDownList></td>
                                                    <td class="tdBackColor" style="width: 20px; height: 22px">
                                                    </td>
                                                </tr>
                                                                                                <tr>
                                                    <td class="tdBackColor" colspan="4" style="width: 255px; height:20px">
                                                    </td>
                                                </tr>
                                    <tr>
                                        <td colspan="4" style="text-align: left">
                                            <table border="0" cellpadding="0" cellspacing="0" style="left: 10px; width: 243px">
                                                <tr>
                                                    <td style="left: 17px; width: 160px; position: relative; height: 1px; background-color: #738495">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="left: 17px; width: 160px; position: relative; height: 1px; background-color: #ffffff">
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" style="height: 44px; text-align: right">
                                            <table>
                                                <tr>
                                                    <td style="height: 20px">
                                                        <asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel" Height="31px" OnClick="btnCancel_Click"
                                                            Text="<%$ Resources:BaseInfo,User_btnCancel %>" Width="70px" /><asp:Button ID="btnSave"
                                                                runat="server" CssClass="buttonSave" Height="31px" OnClick="btnSave_Click" Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>"
                                                                Width="78px" /></td>
                                                    <td style="width: 30px">
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                </table>
        </div>
       
            <div style="width: 142px; height: 100px" id="PotCustLicense">
                <table border="0" cellpadding="0" cellspacing="0" style="height: 405px; width: 553px;">
                    <tr>
                        <td class="tdTopBackColor" style="width: 306px; height: 25px; vertical-align: middle; text-align: left;" valign="top">
                            <img alt="" class="imageLeftBack" /><asp:Label ID="Label5" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,PotCustomer_labCustomerUptate %>" Width="259px"></asp:Label></td>
                        <td class="tdTopRightBackColor" colspan="2" style="width: 528px; height: 25px" valign="top">
                            <img class="imageRightBack" /></td>
                    </tr>
                    <tr>
                        <td class="tdBackColor" colspan="2" style="width: 280px; height: 330px; text-align: center"
                            valign="top">
                            <table border="0" cellpadding="0" cellspacing="0" style="height: 216px; width: 270px;">
                                                            <tr>
                                    <td style="width: 280px; height: 1px; background-color: white" colspan="4">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" colspan="4" style="width: 255px; height: 15px; vertical-align: middle; text-align: center;" valign="bottom">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 243px">
                                            <tr>
                                                <td style="width: 160px; height: 1px; background-color: #738495; left: 10px; position: relative; top: 10px;">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 160px; height: 1px; background-color: #ffffff; left: 10px; position: relative; top: 10px;">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" colspan="4" style="width: 255px; height: 5px; text-align: center; vertical-align: top;"
                                        valign="bottom">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" colspan="4" style="height: 5px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                        <asp:Label ID="lblCustLicenseCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustLicenseCode %>"
                                            Width="69px"></asp:Label></td>
                                    <td class="tdBackColor" style="width: 5px; height: 22px">
                                    </td>
                                    <td class="tdBackColor" style="width: 160px; height: 22px">
                                        <asp:TextBox ID="txtLicenseID" runat="server" CssClass="ipt160px" MaxLength="32"></asp:TextBox></td>
                                    <td class="tdBackColor" style="width: 5px; height: 22px">
                                        </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" colspan="4" style="width: 255px; height: 10px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                        <asp:Label ID="lblCustLicenseName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustLicenseName %>"></asp:Label></td>
                                    <td class="tdBackColor" style="width: 5px; height: 22px">
                                    </td>
                                    <td class="tdBackColor" style="width: 160px; height: 22px">
                                        <asp:TextBox ID="txtLicenseName" runat="server" CssClass="ipt160px" MaxLength="32"></asp:TextBox></td>
                                    <td class="tdBackColor" style="width: 5px; height: 22px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" colspan="4" style="width: 255px; height: 10px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                        <asp:Label ID="lblCustLicenseType" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustLicenseType %>"></asp:Label></td>
                                    <td class="tdBackColor" style="width: 5px; height: 22px">
                                    </td>
                                    <td class="tdBackColor" style="width: 160px; height: 22px">
                                        <asp:DropDownList ID="cmbLicenseType" runat="server" Width="163px" >
                                        </asp:DropDownList></td>
                                    <td class="tdBackColor" style="width: 5px; height: 22px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" colspan="4" style="width: 255px; height: 10px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                        <asp:Label ID="lblCustLicenseStartDate" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblShopStartDate %>"></asp:Label></td>
                                    <td class="tdBackColor" style="width: 5px; height: 22px">
                                    </td>
                                    <td class="tdBackColor" style="width: 160px; height: 22px">
                                        <asp:TextBox ID="txtLicenseBeginDate" runat="server" CssClass="ipt160px" onclick="calendar()"></asp:TextBox></td>
                                    <td class="tdBackColor" style="width: 5px; height: 22px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" colspan="4" style="width: 255px; height: 10px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                        <asp:Label ID="lblCustLicenseEndDate" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblShopEndDate %>"></asp:Label></td>
                                    <td class="tdBackColor" style="width: 5px; height: 22px">
                                    </td>
                                    <td class="tdBackColor" style="width: 160px; height: 22px">
                                        <asp:TextBox ID="txtLicenseEndDate" runat="server" CssClass="ipt160px" onclick="calendar()"></asp:TextBox></td>
                                    <td class="tdBackColor" style="width: 5px; height: 22px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" colspan="4" style="width: 255px; height: 22px; text-align: center">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" colspan="4" style="width: 255px; height: 22px; text-align: center">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 243px">
                                            <tr>
                                                <td style="width: 160px; height: 1px; background-color: #738495; left: 10px; position: relative;">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 160px; height: 1px; background-color: #ffffff; left: 10px; position: relative;">
                                                </td>
                                            </tr>
                                        </table>
                                      </td>
                                </tr>
                            </table>
                            <asp:Button ID="btnAdd" runat="server" CssClass="buttonSave" Height="32px" OnClick="btnAdd_Click"
                                Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" Width="70px" /><asp:Button ID="btnEdit"
                                    runat="server" CssClass="buttonEdit" Height="30px" OnClick="btnEdit_Click" Text="<%$ Resources:BaseInfo,PotCustomer_butUpdate %>"
                                    Width="70px" Enabled="False" /><asp:Button ID="btnQuit" runat="server" CssClass="buttonCancel" Height="30px"
                                        OnClick="btnQuit_Click" Text="<%$ Resources:BaseInfo,User_btnCancel %>" Width="74px" /></td>
                        <td class="tdBackColor" style="width: 255px; height: 351px" valign="top">
                            <table border="0" cellpadding="0" cellspacing="0" style="height: 311px" width="280">
                                <tr>
                                    <td style="width: 280px; height: 1px; background-color: white">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style="width: 280px; height: 22px; text-align: center">
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
                                    <td class="tdBackColor" style="width: 280px; height: 254px; text-align: center" valign="top">
                                        <asp:GridView ID="GrdCustLicense" runat="server" AutoGenerateColumns="False" BackColor="White" BorderStyle="Inset" BorderWidth="1px" CellPadding="3" Height="285px"
                                            OnRowDataBound="GrdCustLicense_RowDataBound" OnSelectedIndexChanged="GrdCustLicense_SelectedIndexChanged"
                                            PageSize="11" Width="273px">
                                            <Columns>
                                                <asp:BoundField DataField="CustLicenseID">
                                                    <ItemStyle CssClass="hidden" />
                                                    <HeaderStyle CssClass="hidden" />
                                                    <FooterStyle CssClass="hidden" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CustLicenseName" HeaderText="<%$ Resources:BaseInfo,PotCustomer_lblCustLicenseName %>">
                                                    <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CustLicenseStartDate" HeaderText="<%$ Resources:BaseInfo,PotShop_lblShopStartDate %>">
                                                    <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CustLicenseEndDate" HeaderText="<%$ Resources:BaseInfo,PotShop_lblShopEndDate %>">
                                                    <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                </asp:BoundField>
                                                <asp:CommandField HeaderText="<%$ Resources:BaseInfo,User_btnChang %>" ShowSelectButton="True">
                                                    <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                </asp:CommandField>
                                            </Columns>
                                                <FooterStyle BackColor="Red" ForeColor="#000066"/>
                                                <RowStyle ForeColor="Black" Height="10px" Font-Overline="False" Font-Size="10pt" />
                                                <SelectedRowStyle BackColor="#FFFFCD" Font-Bold="False" ForeColor="Black" />
                                                <PagerStyle BackColor="White" ForeColor="#000066"   HorizontalAlign="Left" />
                                                <HeaderStyle BackColor="#E1E0B2" Height="10px" Font-Bold="False"  />
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style="width: 280px; height: 22px; text-align: center">
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
                                    <td class="tdBackColor" style="width: 280px; height: 22px; text-align: right">
                                        <asp:Button ID="btnLBack" runat="server" CssClass="buttonBack" Enabled="False" OnClick="btnLBack_Click"
                                            Text="<%$ Resources:BaseInfo,Button_back %>" /><asp:Button ID="btnLNext" runat="server"
                                                CssClass="buttonNext" Enabled="False" OnClick="btnLNext_Click" Text="<%$ Resources:BaseInfo,Button_next %>" /><asp:Label
                                                    ID="lblLCurrent" runat="server" ForeColor="Red">1</asp:Label><asp:Label ID="lblLTotalNum" runat="server"></asp:Label>&nbsp;</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            </ContentTemplate>
        </asp:UpdatePanel>
                </td>
                <td style="width: 60px; height: 401px; text-align: center" valign="top">
                    <img height="401" src="../../images/shuxian.jpg" />
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hidInsert" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidInsert %>" />
        <asp:HiddenField ID="hidUpdate" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidUpdate %>" />
        <asp:HiddenField ID="hidAdd" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidAdd %>" />
        <asp:HiddenField ID="hidWrite" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidWrite %>" />
    </form>
</body>
</html>

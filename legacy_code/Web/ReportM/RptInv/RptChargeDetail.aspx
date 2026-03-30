<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RptChargeDetail.aspx.cs" Inherits="ReportM_RptInv_RptChargeDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<!--
/// Author: hesijian
/// Date: 2009-11-04
/// Content: Create
-->
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Rpt_ChargeDetail") %></title>
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
	        addTabTool("<%=baseInfo %>,ReportM/RptInv/RptChargeDetail.aspx");
	        loadTitle();
	    }
	    
	     function ShowTree()
        {
        	strreturnval=window.showModalDialog('../../Lease/Shop/SelectShop.aspx','window','dialogWidth=237px;dialogHeight=420px');
			window.document.all("allvalue").value = strreturnval;
			if ((window.document.all("allvalue").value != "undefined") && (window.document.all("allvalue").value != ""))
			{
			     var objImgBtn1 = document.getElementById('<%= LinkButton1.ClientID %>');
                objImgBtn1.click();
            }
			else
			{
				return;	
			}  
        }
	</script>
</head>
<body style="margin:0px"  onload ="Load();">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                                            <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="width:5px" class="tdTopRightBackColor">
                                                     <img class="imageLeftBack" />
                                                    </td>
                                                    <td class="tdTopRightBackColor" style="text-align:left;">
                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo,Rpt_ChargeDetail %>"></asp:Label>
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
                                                    <td style="width: 50px">
                                                    </td>
                                                    <td style="width: 250px">
                                                    </td>
                                                    <td style="width: 1300px">
                                                        
                                                    </td>
                                                    <td style="width: 20px">
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 50px">
                                                    </td>
                                                    <td style="width: 250px" class="lable">
                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:BaseInfo,LeaseholdContract_labContractCode %>" CssClass="labelStyle"></asp:Label></td>
                                                    <td style="width: 1300px">
                                                        <asp:TextBox ID="txtContractID" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                                    <td style="width: 20px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 50px">
                                                    </td>
                                                    <td class="lable" style="width: 250px">
                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:BaseInfo,Lease_lblShopCode %>" CssClass="labelStyle"></asp:Label></td>
                                                    <td style="width: 1300px">
                                                        <asp:TextBox ID="txtShopCode" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                                    <td style="width: 20px">
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 50px">
                                                    </td>
                                                    <td style="width: 260px" class="lable">
                                                        <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Account_lblAccountMon %>" Width="70px"></asp:Label></td>
                                                    <td style="width: 1300px">
                                                        <asp:TextBox ID="txtAccountMon" runat="server" CssClass="ipt160px" onclick="calendar()"></asp:TextBox></td>
                                                    <td style="width: 20px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" rowspan="1" style="width: 50px">
                                                    </td>
                                                    <td class="lable" rowspan="1" style="width: 250px">
                                                        <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Lease_lblConChargeType %>"></asp:Label><br />
                                                        &nbsp;<asp:CheckBox ID="All" runat="server" Text="<%$ Resources:BaseInfo,ckck_selectAll %>" Font-Size="9pt" OnCheckedChanged="All_CheckedChanged" Checked="True" AutoPostBack="true"/></td>
                                                    <td rowspan="1" style="width: 1300px;">
                                                        <asp:CheckBoxList ID="ChargeList" runat="server" DataTextField="ChargeTypeName"
                                                            DataValueField="ChargeTypeID" Font-Size="9pt" RepeatDirection="Horizontal" RepeatColumns="5" Width="480px">
                                                        </asp:CheckBoxList></td>
                                                    <td style="width: 20px">
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 50px">
                                                    </td>
                                                    <td class="lable" style="width: 250px">
                                                    </td>
                                                    <td style="width: 1300px">
                                                    </td>
                                                    <td style="width: 20px">
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 50px">
                                                    </td>
                                                    <td class="lable" style="width: 250px">
                                                        </td>
                                                    <td style="width: 1300px">
                                                        <asp:Button ID="btnOK" runat="server" CssClass="buttonQuery" Text="<%$ Resources:BaseInfo,User_lblQuery %> " OnClick="btnOK_Click"  />
                                                        <asp:Button ID="BtnCel" runat="server" CssClass="buttonCancel" Text="<%$ Resources:BaseInfo,User_btnCancel %> " OnClick="BtnCel_Click"/></td>
                                                    <td style="width: 20px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 50px">
                                                    </td>
                                                    <td class="lable" style="width: 250px">
                                                    </td>
                                                    <td style="width: 1300px">
                                                    </td>
                                                    <td style="width: 20px">
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 50px">
                                                    </td>
                                                    <td class="lable" style="width: 250px">
                                                    </td>
                                                    <td style="width: 1300px">
                                                    </td>
                                                    <td style="width: 20px">
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 50px">
                                                    </td>
                                                    <td class="lable" style="width: 250px">
                                                    </td>
                                                    <td style="width: 1300px">
                                                    </td>
                                                    <td style="width: 20px">
                                                    </td>
                                                </tr>
                                            </table>
                                            <input id="allvalue" runat="server" style="width: 25px" type="hidden" />
                                            <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click"></asp:LinkButton>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>

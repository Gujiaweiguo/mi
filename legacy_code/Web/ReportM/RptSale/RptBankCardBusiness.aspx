<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RptBankCardBusiness.aspx.cs" Inherits="ReportM_RptSale_RptBankCardBusiness" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml"  >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Menu_RptBankCrdSumRpt")%></title>
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
    <script type="text/javascript" src="../../JavaScript/Calendar.js" charset="gb2312"></script>
            <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
        <script type="text/javascript">
        function Load()
	    {
	        addTabTool("<%=baseInfo %>,ReportM/RptSale/RptBankCardBusiness.aspx");
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
<body style="margin:0px"	onload ="Load();">
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
                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo,Menu_RptBankCrdSumRpt %>"></asp:Label>
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
                                                <tr style="height: 10px">
                                                    <td style="width: 89px">
                                                    </td>
                                                    <td style="width: 218px">
                                                    </td>
                                                    <td style="width: 43px">
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 22px">
                                                        <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Lease_lblShopCode %>"></asp:Label></td>
                                                    <td style="width: 218px; height: 22px">
                                                        <asp:TextBox ID="txtShopCode" runat="server" CssClass="ipt160px" Height="18px"></asp:TextBox></td>
                                                    <td align="right" class="lable" style="width: 43px; height: 22px">
                                                        <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Height="17px" 
                                                            Text="<%$ Resources:BaseInfo,Lease_lblPayInDataSource %>" Width="73px"></asp:Label>
                                                    </td>
                                                    <td style="height: 22px">
                                                        <asp:RadioButton ID="RB1" runat="server" Checked="True" CssClass="labelStyle" 
                                                            GroupName="c" Text="<%$ Resources:BaseInfo,Rpt_rdoAll %>" />
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 22px;" class="lable">
                                                        <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdBoard_lblContractID %>"></asp:Label></td>
                                                    <td style="width: 218px; height: 22px;">
                                                        <asp:TextBox ID="txtContractCode" runat="server" CssClass="ipt160px" Height="18px"></asp:TextBox></td>
                                                    <td  align="right"  class="lable" style="width: 43px; height: 22px;">
                                                        &nbsp;</td>
                                                    <td style="height: 22px">
                                                        <asp:RadioButton ID="RB2" runat="server" CssClass="labelStyle" GroupName="c" 
                                                            Text="<%$ Resources:BaseInfo,DataSource_POS %>" />
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 17px;">
                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustCode %>" CssClass="labelStyle"></asp:Label></td>
                                                    <td style="width: 218px; height: 17px;">
                                                        <asp:TextBox ID="txtCustCode" runat="server" CssClass="ipt160px" Height="18px"></asp:TextBox></td>
                                                    <td style="width: 43px; height: 17px;">
                                                    </td>
                                                    <td style="height: 17px">
                                                        <asp:RadioButton ID="RB3" runat="server" CssClass="labelStyle" GroupName="c" 
                                                            Text="<%$ Resources:BaseInfo,DataSource_Put %>" />
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 15px;" class="lable">
                                                        <asp:Label ID="Label12" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Rpt_SDate %>"></asp:Label></td>
                                                    <td style="width: 218px; height: 15px;">
                                                        <asp:TextBox ID="txtStartBizTime" runat="server" CssClass="ipt160px" Height="18px" onclick="calendar()"></asp:TextBox></td>
                                                    <td style="width: 43px; height: 15px;" class="lable" align="right">
                                                        </td>
                                                    <td style="height: 15px">
                                                        <asp:RadioButton ID="RB4" runat="server" CssClass="labelStyle" GroupName="c" 
                                                            Text="<%$ Resources:BaseInfo,DataSource_Manual %>" />
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 7px;">
                                                        <asp:Label ID="Label7" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Rpt_EDate %>"></asp:Label></td>
                                                    <td style="width: 218px; height: 7px;">
                                                        <asp:TextBox ID="txtEndBizTime" runat="server" CssClass="ipt160px" Height="18px" onclick="calendar()"></asp:TextBox></td>
                                                    <td align="right" class="lable" style="width: 43px; height: 7px;">
                                                    </td>
                                                    <td style="height: 7px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 12px;">
                                                        <asp:Label ID="lblBizMode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_BizMode %>"></asp:Label></td>
                                                    <td style="width: 218px; height: 12px;">
                                                        <asp:DropDownList ID="cmbBizMode" runat="server" CssClass="cmb160px">
                                                        </asp:DropDownList></td>
                                                    <td align="right" class="lable" style="width: 43px; height: 12px;">
                                                    </td>
                                                    <td style="height: 12px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 10px;">
                                                        </td>
                                                    <td style="width: 218px; height: 10px;">
                                                        <asp:RadioButton ID="rdoRptPub_Sum" runat="server" GroupName="Rpt" Visible="true" Text="<%$ Resources:ReportInfo,RptPub_Sum %>" Checked="True" CssClass="labelStyle" /><asp:RadioButton ID="rdoRptPub_Particular" runat="server" GroupName="Rpt" Visible="true"  Text="<%$ Resources:ReportInfo,RptPub_Particular %>" CssClass="labelStyle"/> </td>
                                                    <td align="right" class="lable" style="width: 43px; height: 10px;">
                                                    </td>
                                                    <td style="height: 10px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 16px;" class="lable">
                                                        </td>
                                                    <td style="width: 218px; height: 16px;">
                                                        <asp:Button ID="btnOK" runat="server" CssClass="buttonQuery" Text="<%$ Resources:BaseInfo,User_lblQuery %> " OnClick="btnOK_Click"  />
                                                        <asp:Button ID="BtnCel" runat="server" CssClass="buttonCancel" Text="<%$ Resources:BaseInfo,User_btnCancel %> " OnClick="BtnCel_Click"/></td>
                                                    <td style="width: 43px; height: 16px;">
                                                    </td>
                                                    <td style="height: 16px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 10px;" class="lable">
                                                        </td>
                                                    <td style="width: 218px; height: 10px;">
                                                        </td>
                                                    <td style="width: 43px; height: 10px;">
                                                    </td>
                                                    <td style="height: 10px">
                                                        </td>
                                                </tr>
                                                                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 10px;" class="lable">
                                                        </td>
                                                    <td style="width: 218px; height: 10px;">
                                                        </td>
                                                    <td style="width: 43px; height: 10px;">
                                                    </td>
                                                    <td style="height: 10px">
                                                        </td>
                                                </tr>
                                                                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 10px;" class="lable">
                                                        </td>
                                                    <td style="width: 218px; height: 10px;">
                                                        </td>
                                                    <td style="width: 43px; height: 10px;">
                                                    </td>
                                                    <td style="height: 10px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 4px;">
                                                        </td>
                                                    <td style="width: 218px; height: 4px;">
                                                        </td>
                                                    <td style="width: 43px; height: 4px;">
                                                    </td>
                                                    <td style="height: 4px">
                                                        </td>
                                                </tr>
                                            </table>
                <input id="allvalue" runat="server" style="width: 25px" type="hidden" /><asp:LinkButton
                    ID="LinkButton1" runat="server" OnClick="LinkButton1_Click"></asp:LinkButton>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RptMoreContractQuery.aspx.cs" Inherits="ReportM_RptInv_RptMoreContractQuery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml"  >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("ReportInfo", "Rpt_MoreContractQuery")%></title>
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
	        addTabTool("<%=baseInfo %>,ReportM/RptInv/RptMoreContractQuery.aspx");
	        loadTitle();
	    }
	    function selectShopBrand()
		{
			//strreturnval=window.showModalDialog('../Lease/SelectShopBrand.aspx','window','dialogWidth=350px;dialogHeight=380px');
			strreturnval=window.showModalDialog('../../Lease/Brand/BrandSelect.aspx','window','dialogWidth=337px;dialogHeight=420px');
			window.document.all("allvalue").value = strreturnval;
			if ((window.document.all("allvalue").value != "undefined") && (window.document.all("allvalue").value != ""))
			{
			    var objImgBtn1 = document.getElementById('<%= LinkButton1.ClientID %>');
                objImgBtn1.click();
            }
			else
				return;	
		}
	</script>
	
</head>
<body style="margin:0px" onload ="Load();">
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
                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:ReportInfo,Rpt_MoreContractQuery %>"></asp:Label>
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
                                                    <td style="width: 89px; height: 10px;">
                                                    </td>
                                                    <td style="width: 218px; height: 10px;">
                                                        
                                                    </td>
                                                    <td style="width: 44px; height: 10px;">
                                                    </td>
                                                    <td style="height: 10px">
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 22px;" class="lable">
                                                        <asp:Label ID="Label7" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustName %>">
                                            </asp:Label></td>
                                                    <td style="width: 218px; height: 22px;">
                                                        <asp:TextBox ID="txtCustName" runat="server" CssClass="ipt160px" Width="180px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 44px; height: 22px;">
                                                        &nbsp;</td>
                                                    <td style="height: 22px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 17px;">
                                                        <asp:Label ID="Label8" runat="server" CssClass="labelStyle" 
                                                            Text="<%$ Resources:BaseInfo,Rpt_BrandName %>"> </asp:Label>
                                                    </td>
                                                    <td style="width: 218px; height: 17px;">
                                                        <asp:TextBox ID="txtBrand" runat="server" CssClass="ipt160px" Width="180px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 44px; height: 17px;">
                                                    </td>
                                                    <td style="height: 17px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 16px">
                                                        <asp:Label ID="Label9" runat="server" CssClass="labelStyle" 
                                                            Text="<%$ Resources:BaseInfo,RPT_lblGroupBy %>"> </asp:Label>
                                                    </td>
                                                    <td style="width: 218px; height: 16px">
                                                        <asp:RadioButton ID="rbBrand" runat="server" CssClass="labelStyle"  
                                                            Text="<%$ Resources:ReportInfo,Rpt_Brand %>" Checked="True" GroupName="g" />
                                                        &nbsp;<asp:RadioButton ID="rbCustName" runat="server" CssClass="labelStyle" 
                                                            Text="<%$ Resources:BaseInfo,PotCustomer_lblCustName %>" GroupName="g" />
                                                    </td>
                                                    <td style="width: 44px; height: 16px">
                                                    </td>
                                                    <td style="height: 16px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 15px;" class="lable">
                                                        &nbsp;</td>
                                                    <td style="width: 218px; height: 15px;">
                                                        &nbsp;</td>
                                                    <td style="width: 44px; height: 15px;" class="lable" align="right">
                                                        </td>
                                                    <td style="height: 15px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 7px;">
                                                        &nbsp;</td>
                                                    <td style="width: 218px; height: 7px;">
                                                        &nbsp;</td>
                                                    <td align="right" class="lable" style="width: 44px; height: 7px;">
                                                    </td>
                                                    <td style="height: 7px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 7px;">
                                                        &nbsp;</td>
                                                    <td style="width: 218px; height: 7px;">
                                                        <asp:Button ID="btnQuery" runat="server" CssClass="buttonQuery" 
                                                            OnClick="btnOK_Click" onmouseout="BtnUp(this.id);" 
                                                            onmouseover="BtnOver(this.id);" onmouseup="BtnUp(this.id);" 
                                                            Text="<%$ Resources:BaseInfo,User_lblQuery %> " />
                                                        <asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel" 
                                                            OnClick="btnCancel_Click" onmouseout="BtnUp(this.id);" 
                                                            onmouseover="BtnOver(this.id);" onmouseup="BtnUp(this.id);" 
                                                            Text="<%$ Resources:BaseInfo,User_btnCancel %> " />
                                                    </td>
                                                    <td align="right" class="lable" style="width: 44px; height: 7px;">
                                                        &nbsp;</td>
                                                    <td style="height: 7px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 7px;">
                                                        &nbsp;</td>
                                                    <td style="width: 218px; height: 7px;">
                                                        &nbsp;</td>
                                                    <td align="right" class="lable" style="width: 44px; height: 7px;">
                                                        &nbsp;</td>
                                                    <td style="height: 7px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 4px;">
                                                        </td>
                                                    <td style="width: 218px; height: 4px;">
                                                        &nbsp;</td>
                                                    <td style="width: 44px; height: 4px;">
                                                    </td>
                                                    <td style="height: 4px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 4px;">
                                                        &nbsp;</td>
                                                    <td style="width: 218px; height: 4px;">
                                                        &nbsp;</td>
                                                    <td style="width: 44px; height: 4px;">
                                                        &nbsp;</td>
                                                    <td style="height: 4px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; text-align: right;">
                                                        &nbsp;</td>
                                                    <td style="width: 218px">&nbsp;</td>
                                                    <td style="width: 44px">
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; text-align: right;">
                                                        &nbsp;</td>
                                                    <td style="width: 218px">
                                                        &nbsp;</td>
                                                    <td style="width: 44px">
                                                        &nbsp;</td>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; text-align: right;">
                                                        &nbsp;</td>
                                                    <td style="width: 218px">
                                                        &nbsp;</td>
                                                    <td style="width: 44px">
                                                        &nbsp;</td>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px">
                                                        <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click"></asp:LinkButton>
                                                    </td>
                                                    <td style="width: 218px">
                                                        &nbsp;</td>
                                                    <td style="width: 44px">
                                                        &nbsp;</td>
                                                    <td>
                                                    </td>
                                                </tr>
                                            </table>
                                            <input id="allvalue" runat="server" style="width: 25px" type="hidden" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>


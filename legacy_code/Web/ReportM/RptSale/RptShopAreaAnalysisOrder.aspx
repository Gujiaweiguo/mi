<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RptShopAreaAnalysisOrder.aspx.cs" Inherits="ReportM_RptSale_RptShopAreaAnalysisOrder" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml"  >
<head id="Head1" runat="server">
    <title><%=baseInfo %></title>
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        <!--
            table.mainTbl {width:572px;height:401px;}
            
            tr{height:28px;}
            td.lable{padding-right:5px;text-align:right;}
            
        .style1
        {
            width: 218px;
        }
        .style2
        {
            width: 44px;
        }
            
        -->
    </style>
    <script type="text/javascript" src="../../JavaScript/Common.js"></script>
    <script type="text/javascript" src="../../JavaScript/Calendar.js" charset="gb2312"></script>
            <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
        <script type="text/javascript">
        function Load()
	    {
	        addTabTool("<%=Fresh %>,ReportM/RptSale/RptShopAreaAnalysisOrder.aspx");
	        loadTitle();
	    }

	    function text()
	    {
	        var key=window.event.keyCode;
	        if ((key>=48&&key<=57)||(key==08)) 
            {
                window.event.returnValue =true;
            }
            else
            {
            window.event.returnValue =false;
            }
	    
	    }
	    	    function textnull()
	    {
            window.event.returnValue =false;
	    }
	</script>
	
</head>
<body style="margin:0px" onload ="Load();" onKeyDown="text()" >
    <form id="form1" runat="server"  >
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
                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo,Menu_ShopAreaAnalysisOrder %>"></asp:Label>
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
                                                    <td style="width: 44px">
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 22px;" class="lable">
                                                        <asp:Label ID="Label4" runat="server" CssClass="labelStyle" 
                                                            Text="<%$ Resources:BaseInfo,PotCustomer_BusinessItem %>">
                                            </asp:Label>
                                                    </td>
                                                    <td style="width: 218px; height: 22px;">
                                                        <asp:DropDownList ID="ddlBizproject" runat="server" AutoPostBack="true" 
                                                            OnSelectedIndexChanged="ddlBizproject_SelectedIndexChanged" Width="165px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="width: 44px; height: 22px;">
                                                        <asp:Label ID="Label13" runat="server" Text="<%$ Resources:BaseInfo,Lease_lblPayInDataSource %>" CssClass="labelStyle" Width="73px" Height="17px"></asp:Label></td>
                                                    <td style="height: 22px">
                                                        <asp:RadioButton ID="RB1" runat="server" GroupName="a"  Text="<%$ Resources:BaseInfo,Rpt_rdoAll %>" Checked="True" CssClass="labelStyle"/></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 17px;">
                                                        <asp:Label ID="Label15" runat="server" CssClass="labelStyle" 
                                                            Text="<%$ Resources:BaseInfo,RentableArea_lblFloorName %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 218px; height: 17px;">
                                                        <asp:DropDownList ID="ddlFloorName" runat="server" AutoPostBack="True" 
                                                            Width="165px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="width: 44px; height: 17px;">
                                                    </td>
                                                    <td style="height: 17px">
                                                        <asp:RadioButton ID="RB2" runat="server" GroupName="a"  Text="<%$ Resources:BaseInfo,DataSource_POS %>" CssClass="labelStyle"/></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 16px;" class="lable">
                                                        <asp:Label ID="Label2" runat="server" CssClass="labelStyle" 
                                                            Text="<%$ Resources:BaseInfo,LeaseholdContract_labShopCode %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 218px; height: 16px;">
                                                        <asp:DropDownList ID="ddlShopCode" runat="server" Width="165px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="width: 44px; height: 16px;">
                                                        </td>
                                                    <td style="height: 16px">
                                                        <asp:RadioButton ID="RB3" runat="server" GroupName="a"  Text="<%$ Resources:BaseInfo,DataSource_Put %>" CssClass="labelStyle"/></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 15px;" class="lable">
                                                        <asp:Label ID="Label16" runat="server" CssClass="labelStyle" 
                                                            Text="<%$ Resources:BaseInfo,RentableArea_lblAreaName %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 218px; height: 15px;">
                                                        <asp:DropDownList ID="ddlAreaName" runat="server" Width="165px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="width: 44px; height: 15px;" class="lable" align="right">
                                                        </td>
                                                    <td style="height: 15px">
                                                        <asp:RadioButton ID="RB4" runat="server" GroupName="a"  Text="<%$ Resources:BaseInfo,DataSource_Manual %>" CssClass="labelStyle"/></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 7px;">
                                                        <asp:Label ID="Label17" runat="server" CssClass="labelStyle" 
                                                            Text="<%$ Resources:BaseInfo,LeaseholdContract_labTradeID %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 218px; height: 7px;">
                                                        <asp:DropDownList ID="ddlTrade2Name" runat="server" Width="165px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td align="right" class="lable" style="width: 44px; height: 7px;">
                                                    </td>
                                                    <td style="height: 7px">
                                                        <asp:RadioButton ID="RadioButton8" runat="server" GroupName="b" Visible="False" /></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 12px;">
                                                        <asp:Label ID="Label12" runat="server" CssClass="labelStyle" 
                                                            Text="<%$ Resources:BaseInfo,Rpt_SDate %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 218px; height: 12px;">
                                                        <asp:TextBox ID="txtStartBizTime" runat="server" CssClass="ipt160px" 
                                                            onclick="calendar()" onkeydown="textnull()"></asp:TextBox>
                                                    </td>
                                                    <td align="right" class="lable" style="width: 44px; height: 12px;">
                                                        <asp:Label ID="Label9" runat="server" CssClass="labelStyle" Height="17px" Text="<%$ Resources:ReportInfo,Rpt_OrderingTerm %>"
                                                            Width="73px"></asp:Label></td>
                                                    <td style="height: 12px">
                                                        <asp:RadioButton ID="RB5" runat="server" GroupName="b" CssClass="labelStyle" Text="<%$ Resources:ReportInfo,Rpt_ShopCode %>"  Checked="true"/></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 10px;">
                                                        <asp:Label ID="Label7" runat="server" CssClass="labelStyle" 
                                                            Text="<%$ Resources:BaseInfo,Rpt_EDate %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 218px; height: 10px;">
                                                        <asp:TextBox ID="txtEndBizTime" runat="server" CssClass="ipt160px" 
                                                            onclick="calendar()" onkeydown="textnull()"></asp:TextBox>
                                                    </td>
                                                    <td align="right" class="lable" style="width: 44px; height: 10px;">
                                                    </td>
                                                    <td style="height: 10px">
                                                        <asp:RadioButton ID="RB6" runat="server" GroupName="b" CssClass="labelStyle" Text="<%$ Resources:ReportInfo,Rpt_SalesEfficiency %>" /></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 16px;" class="lable">
                                                        <asp:Label ID="Label18" runat="server" CssClass="labelStyle" 
                                                            Text="<%$ Resources:BaseInfo,Rpt_NumberRank %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 218px; height: 16px;">
                                                        <asp:TextBox ID="txtCount" runat="server" CssClass="ipt160px" 
                                                            onKeyDown="text()"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 44px; height: 16px;">
                                                    </td>
                                                    <td style="height: 16px">
                                                        <asp:RadioButton ID="RB7" runat="server" GroupName="b" CssClass="labelStyle" Text="<%$ Resources:ReportInfo,RptSale_Trade2Name %>" /></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; " class="lable">
                                                        <asp:Label ID="lblArea" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblRentArea %>"></asp:Label></td>
                                                    <td class="style1">
                                                        <asp:TextBox ID="txtAreaB" runat="server" CssClass="ipt160px" Width="41px" onKeyDown="text()"></asp:TextBox>
                                                        <asp:Label ID="Label14" runat="server" Text="-" Width="2px" Height="23px"></asp:Label>
                                                        <asp:TextBox ID="txtAreaE" runat="server" CssClass="ipt160px" Width="41px" onKeyDown="text()"></asp:TextBox></td>
                                                    <td class="style2">
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="RadioButton9" runat="server" GroupName="b" Visible="False" /></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 4px;">
                                                        &nbsp;</td>
                                                    <td style="width: 218px; height: 4px;">
                                                        <asp:Button ID="btnOK0" runat="server" CssClass="buttonQuery" 
                                                            OnClick="btnOK_Click" Text="<%$ Resources:BaseInfo,User_lblQuery %> " />
                                                        <asp:Button ID="BtnCel0" runat="server" CssClass="buttonCancel" 
                                                            OnClick="BtnCel_Click" Text="<%$ Resources:BaseInfo,User_btnCancel %> " />
                                                    </td>
                                                    <td style="width: 44px; height: 4px;">
                                                    </td>
                                                    <td style="height: 4px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 4px;">
                                                        </td>
                                                    <td style="width: 218px">
                                                        </td>
                                                    <td style="width: 44px">
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px">
                                                    </td>
                                                    <td style="width: 218px">
                                                        </td>
                                                    <td style="width: 44px">
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                            </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>

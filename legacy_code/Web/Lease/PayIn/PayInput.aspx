<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayInput.aspx.cs" Inherits="Lease_PayIn_PayInput" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Menu_InputReceivables")%></title>
    <link href="../../App_Themes/CSS/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        <!--
        
        table.payPunIn tr.rowHeight{ height:28px;}
        
        table.payPunIn tr.headLine{ height:1px; }
        table.payPunIn tr.bodyLine{ height:1px; }
        
        td.baseLable{ padding-right:10px;text-align:right;}
        td.baseInput{ align:left;padding-right:20px }
        -->
      </style>
      <script type="text/javascript" src="../../JavaScript/Calendar.js" language="javascript" charset="gb2312"></script>
      <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
      <script type="text/javascript">
        <!--
             //验证数字类型
        function textleave(form1)
        {   
            var key=window.event.keyCode;
            if(key==8 || key==46 || key==48 || key==49 || key==50 || key==51 || key==52 || key==53 || key==54 || key==55 || key==56 ||
               key==57 || key==190 || key == 96 || key == 97 || key == 98 || key == 99 || key == 100 || key == 101 || key == 102 ||
               key == 103 || key == 104 || key == 105 || key == 110)
            {
		        window.event.returnValue =true;
	        }else
	        {
		        window.event.returnValue =false;
	        }
	    }    
	    function ShowTree()
        {
    	    strreturnval=window.showModalDialog('../Shop/SelectShop.aspx','window','dialogWidth=237px;dialogHeight=420px');
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
	     function Load()
        {
            addTabTool("<%=baseInfo %>,Lease/PayIn/PayInput.aspx");
            loadTitle();
        }
        -->
      </script>
</head>
<body style="margin:0px" onload="Load()">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table style="width: 100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 6px; text-align:left" class="tdTopRightBackColor">
                        <img alt="" class="imageLeftBack" style=" text-align:left"  />
                        </td>
                        <td class="tdTopRightBackColor" style="text-align:left">
                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo,Lease_lblPayInPut %>" Height="12pt" Width="521px"></asp:Label>
                        </td>
                        <td class="tdTopRightBackColor">
                        <img class="imageRightBack" style="width: 7px; height: 22px" />
                        </td>
                    </tr>
                    <tr class="headLine">
                        <td style="width: 6px">
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" class="tdBackColor">
                            <table style="width:100%"  border="0" cellpadding="0" cellspacing="0" class="payPunIn">
                                <tr style="height:10px">
                                    <td style="width: 154px">
                                    </td>
                                    <td style="width: 198px">
                                    </td>
                                    <td style="width: 118px">
                                    </td>
                                    <td style="width: 178px">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr class="rowHeight">
                                    <td style="width: 154px" class="baseLable">
                                        <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseAreaType_CustCode %>"></asp:Label></td>
                                    <td style="width: 198px">
                                        <asp:TextBox ID="txtCustCode" runat="server" CssClass="ipt160px" MaxLength="16"></asp:TextBox></td>
                                    <td style="width: 118px" class="baseLable">
                                        <asp:Label ID="Label14" runat="server" CssClass="labelStyle" 
                                            Text="<%$ Resources:BaseInfo,PotShop_lblPotShopName %>"></asp:Label>
                                    </td>
                                    <td style="width: 178px">
                                        <asp:TextBox ID="txtShopCode" runat="server" CssClass="ipt160px" MaxLength="16"></asp:TextBox></td>
                                    <td>
                                        <asp:Button ID="btnQuery" runat="server" CssClass="buttonQuery" OnClick="btnQuery_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                            Text="<%$ Resources:BaseInfo,User_lblQuery %>" />&nbsp;</td>
                                </tr>
                                <tr class="rowHeight">
                                    <td colspan="5" align="center">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 95%">
                                            <tr style="height: 1px">
                                                <td style="width: 160px; height: 1px; background-color: #738495">
                                                </td>
                                            </tr>
                                            <tr style="height: 1px">
                                                <td style="width: 160px; height: 1px; background-color: #ffffff">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr class="rowHeight">
                                    <td style="width: 154px" class="baseLable">
                                        <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustName %>"></asp:Label></td>
                                    <td style="width: 198px">
                                        <asp:TextBox ID="txtCustName" runat="server" CssClass="ipt160px" MaxLength="32"></asp:TextBox></td>
                                    <td style="width: 118px" class="baseLable">
                                        <asp:Label ID="Label11" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Lease_lblPayInDataSource %>"></asp:Label></td>
                                    <td style="width: 178px">
                                        <asp:DropDownList ID="dropPayInDataSource" runat="server" Width="167px" Enabled="False">
                                        </asp:DropDownList></td>
                                    <td>
                                    </td>
                                </tr>
                                <tr class="rowHeight">
                                    <td style="width: 154px" class="baseLable">
                                        <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdBoard_lblContractID %>"></asp:Label></td>
                                    <td style="width: 198px">
                                        <asp:DropDownList ID="dropContractID" runat="server" Width="167px">
                                        </asp:DropDownList></td>
                                    <td style="width: 118px" class="baseLable">
                                        <asp:Label ID="Label12" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Lease_lblPayInAmt %>"></asp:Label></td>
                                    <td style="width: 178px">
                                        <asp:TextBox ID="txtPayInAmt" runat="server" CssClass="ipt160px" style="ime-mode:disabled;"></asp:TextBox></td>
                                    <td>
                                    </td>
                                </tr>
                                <tr class="rowHeight">
                                    <td style="width: 154px" class="baseLable">
                                        <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Lease_lblShopCode %>"></asp:Label></td>
                                    <td style="width: 198px">
                                        <asp:DropDownList ID="dropShopID" runat="server" Width="167px">
                                        </asp:DropDownList></td>
                                    <td style="width: 118px">
                                    </td>
                                    <td style="width: 178px">
                                        </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr class="rowHeight">
                                    <td style="width: 154px" class="baseLable">
                                        <asp:Label ID="Label7" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Lease_lblPayInPeriod %>"></asp:Label></td>
                                    <td style="width: 198px">
                                        <asp:TextBox ID="txtPayInPeriod" runat="server" CssClass="ipt160px" onclick="calendar()"></asp:TextBox></td>
                                    <td style="width: 118px">
                                    </td>
                                    <td style="width: 178px">
                                        <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" OnClick="btnOK_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);" Text="<%$ Resources:BaseInfo,User_btnOk %> " />
                                        <asp:Button ID="BtnCancel" runat="server" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                                CssClass="buttonCancel" OnClick="BtnCel_Click" Text="<%$ Resources:BaseInfo,User_btnCancel %> " /></td>
                                    <td>
                                    </td>
                                </tr>
                                <tr class="rowHeight">
                                    <td style="width: 154px" class="baseLable">
                                        <asp:Label ID="Label8" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labChargeStartDate %>"></asp:Label></td>
                                    <td style="width: 198px">
                                        <asp:TextBox ID="txtPayInStartDate" runat="server" CssClass="ipt160px" onclick="calendar()"></asp:TextBox></td>
                                    <td style="width: 118px">
                                    </td>
                                    <td style="width: 178px">
                                        <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click"></asp:LinkButton>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr class="rowHeight">
                                    <td style="width: 154px" class="baseLable">
                                        <asp:Label ID="Label9" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labChargeEndDate %>"></asp:Label></td>
                                    <td style="width: 198px">
                                        <asp:TextBox ID="txtPayInEndDate" runat="server" CssClass="ipt160px" onclick="calendar()"></asp:TextBox></td>
                                    <td style="width: 118px">
                                    </td>
                                    <td style="width: 178px">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr class="rowHeight">
                                    <td style="width: 154px" class="baseLable">
                                        <asp:Label ID="Label10" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Lease_lblPayInType %>"></asp:Label></td>
                                    <td style="width: 198px"><asp:DropDownList ID="dropPayInType" runat="server" Width="167px">
                                    </asp:DropDownList></td>
                                    <td style="width: 118px">
                                    </td>
                                    <td style="width: 178px">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr class="rowHeight">
                                    <td style="width: 154px">
                                    </td>
                                    <td style="width: 198px">
                                    </td>
                                    <td style="width: 118px">
                                    </td>
                                    <td style="width: 178px">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr class="rowHeight">
                                    <td style="width: 154px">
                                    </td>
                                    <td style="width: 198px">
                                    </td>
                                    <td style="width: 118px">
                                    </td>
                                    <td style="width: 178px">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                 <input id="allvalue" runat="server" style="width: 25px" type="hidden" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>

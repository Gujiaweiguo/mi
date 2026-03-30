<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AuditingGroundContract.aspx.cs" Inherits="Lease_AuditingAdAndGround_AuditingGroundContract" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<!--
/// Author:hesijian
/// Date:2009-11-19
/// Content:Created
-->
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Lease_lblBasicContractInfo")%></title>
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/longCss/webtab.css" rel="stylesheet" type="text/css"/>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
      <style type="text/css">
        <!--
        
        table.tblBase tr{ height:30px; }
        
        table.tblBase tr.headLine{ height:1px; }
        table.tblBase tr.bodyLine{ height:1px; }
        table.tblBase tr.colLine{ height:10px; }
        
        table.tblBase td.baseLable{ padding-right:10px;text-align:right;}
        table.tblBase td.baseInput{ align:left;padding-right:20px }
        -->
    </style>  
    <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
     <script type="text/javascript"  src="../../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"> </script>
	<script type="text/javascript">
	    function Load()
	    {
	    var menus=document.getElementById("HidMenu").value;
	        addTabTool(menus);
	        loadTitle();
	    }
    </script>
</head>
<body onload="Load()" style="margin:0px" >
    <form id="form1" runat="server">
     <div id="BaseBargain">
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
        <tr>
            <td style="width: 669px; text-align: left; height: 28px;" class="tdTopRightBackColor" align="left">
                <img class="imageLeftBack" src="" style="width: 7px"  />
                <asp:Label ID="Label31" runat="server" Text="<%$ Resources:BaseInfo,Lease_lblBasicContractInfo %>" Width="251px"></asp:Label></td>
            <td style="width: 562px; height: 28px;" class="tdTopRightBackColor" align="left"></td>
            <td style="width: 7px; height: 28px;" class="tdTopRightBackColor" valign="top">
                <img class="imageRightBack" src="" style="width: 7px;" align="right" /></td>
        </tr>
        <tr>
            <td class="tdBackColor" colspan="3" style="width:100%;"
                    valign="top">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                    
                    <tr class="headLine">
                        <td style="background-color:White; height:1" colspan="4"　width="710px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width:710px; height:10px;" colspan="4" class="tdBackColor">
                        </td>
                    </tr>
                    
                    <tr>
                    <!--  *********left
                    -->
                    <td width="50%" valign="top">
                    
                <table class="tblBase" width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr style="height:8px">
                        <td style="width:202px;">
                        </td>
                        <td style="width:164px;">
                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                <tr class="bodyLine">
                                    <td style="height: 1px; background-color: #738495;" >
                                    </td>
                                </tr>
                                <tr class="bodyLine">
                                    <td style="height: 1px; background-color: #FFFFFF;">
                                    </td>
                                </tr>
                            </table>
                        </td>
    </tr>
    
    <tr>
        <td class="baseLable" style="width: 202px">
            <asp:Label ID="Label1" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustCode %>"></asp:Label></td>
        <td>
            <asp:TextBox ID="txtCustCode" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" ReadOnly="True"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="baseLable" style="width: 202px">
            <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustName %>"></asp:Label></td>
        <td>
            <asp:TextBox ID="txtCustName" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" ReadOnly="True"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="baseLable" style="width: 202px">
            <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustShortName %>"></asp:Label></td>
        <td>
            <asp:TextBox ID="txtCustShortName" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" ReadOnly="True"></asp:TextBox></td>
    </tr>
      <tr style="height:20px">
    <td colspan="2"></td>
    </tr>
        <tr style="height:8px">
            <td style="width: 202px">
            </td>
            <td style="width:164px;">
                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr class="bodyLine">
                        <td style="height: 1px; background-color: #738495;">
                        </td>
                    </tr>
                    <tr class="bodyLine">
                        <td style="height: 1px; background-color: #FFFFFF;">
                        </td>
                    </tr>
                </table>
            </td>
    </tr>
                    <tr style="height: 8px">
                        <td class="baseLable" style="width: 202px">
                            <asp:Label ID="Label14" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Contract_ManageCompany %>"></asp:Label></td>
                        <td style="width: 164px">
                            <asp:DropDownList ID="ddlSubs" runat="server" BackColor="#F5F5F4" Width="165px">
                            </asp:DropDownList></td>
                    </tr>
    <tr>
        <td class="baseLable" style="width: 202px">
                <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labContractStatus %>"></asp:Label></td>
        <td style="height:18px;">
                <asp:TextBox ID="cmbContractStatus" runat="server" CssClass="ipt160px" ReadOnly="True" BackColor="#F5F5F4"></asp:TextBox></td>
    </tr>
        <tr>
            <td class="baseLable" style="width: 202px">
            <asp:Label ID="Label7" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labContractCode %>"></asp:Label></td>
            <td>
            <asp:TextBox ID="txtContractCode" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" ReadOnly="True"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="baseLable" style="height: 19px; width: 202px;">
            <asp:Label ID="Label8" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labRefID %>"></asp:Label></td>
            <td style="height: 19px">
            <asp:TextBox ID="txtRefID" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" ReadOnly="True"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="baseLable" style="height: 28px; width: 202px;">
            <asp:Label ID="Label9" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labConStartDate %>"></asp:Label></td>
            <td style="height: 28px">
            <asp:TextBox ID="txtConStartDate" onclick="calendar()" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" Enabled="false"></asp:TextBox></td>
        </tr>
    <tr>
        <td class="baseLable" style="height: 28px; width: 202px;">
                <asp:Label ID="Label10" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labConEndDate %>"></asp:Label></td>
        <td style="height: 28px">
                <asp:TextBox ID="txtConEndDate" onclick="calendar()" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" Enabled="false"></asp:TextBox></td>
    </tr>
        <tr>
            <td class="baseLable" style="width: 202px">
            <asp:Label ID="Label13" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labChargeStartDate %>"></asp:Label></td>
            <td>
            <asp:TextBox ID="txtChargeStart" onclick="calendar()" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" Enabled="false"></asp:TextBox></td>
        </tr>
                <tr>
            <td class="baseLable" style="width: 202px">
            </td>
            <td>
            </td>
        </tr>
               </table>   
               </td>  
                    <!--  *********right
                    -->
                    <td width="50%"  valign="top">
                    
                <table class="tblBase" width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr style="height:8px">
                        <td style="width:105px;">
                        </td>
                        <td class="baseInput">
                            <table width="165px" border="0" cellpadding="0" cellspacing="0">
                                <tr class="bodyLine">
                                    <td style="height: 1px; background-color: #738495;">
                                    </td>
                                </tr>
                                <tr class="bodyLine">
                                    <td style="height: 1px; background-color: #FFFFFF;" >
                                    </td>
                                </tr>
                              </table>
                       </td>
                    </tr>
    <tr>
        <td class="baseLable" style="width: 105px; height: 30px;">
                            <asp:Label ID="Label17" runat="server" Text="<%$ Resources:BaseInfo,Lease_lblNorentDays %>"  CssClass="labelStyle"></asp:Label></td>
        <td class="baseInput" style="height: 30px">
                            <asp:TextBox ID="txtNorentDays" runat="server" BackColor="#F5F5F4" CssClass="ipt160px"
                                ReadOnly="True"></asp:TextBox></td>
    </tr>
                    <tr>
                        <td class="baseLable" style="width: 105px">
            <asp:Label ID="Label32" runat="server" Text="<%$ Resources:BaseInfo,LeaseholdContract_labEConURL %>" CssClass="labelStyle"></asp:Label></td>
                        <td class="baseInput">
            <asp:TextBox ID="txtBargain" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" ReadOnly="True"></asp:TextBox></td>
                    </tr>
    <tr>
        <td class="baseLable" style="width: 105px">
            <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdContract_lblStopDate %>"
                Width="98px"></asp:Label></td>
        <td>
            <asp:TextBox ID="txtStopDate" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
    </tr>
      <tr style="height:20px">
    <td colspan="2"></td>
    </tr>
        <tr style="height:8px">
            <td style="width: 105px">
            </td>
            <td class="baseInput">
                <table width="165px" border="0" cellpadding="0" cellspacing="0">
                    <tr class="bodyLine">
                        <td style="height: 1px; background-color: #738495;">
                        </td>
                    </tr>
                    <tr class="bodyLine">
                        <td style="height: 1px; background-color: #FFFFFF;">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
                   
                    <tr>
                        <td class="baseLable" style="width: 105px">
                        <asp:Label ID="Label11" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labNotice %>"></asp:Label>
                        </td>
                        <td >
                            <asp:DropDownList ID="DDownListTerm" runat="server" Width="165px" BackColor="#F5F5F4" Enabled="False">
            </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td class="baseLable" style="width: 105px">
                        <asp:Label ID="Label52" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labPenalty %>" Width="99px"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DDownListPenalty" runat="server" Width="165px" BackColor="#F5F5F4" Enabled="False">
                </asp:DropDownList></td>
                    </tr>
                    <tr style="height:10px">
    <td colspan="2"></td>
    </tr>
        <tr style="height:5px">
            <td style="width: 105px; height: 5px;">
            </td>
            <td class="baseInput" style="height: 5px">
                <table width="165px" border="0" cellpadding="0" cellspacing="0">
                    <tr class="bodyLine">
                        <td style="height: 1px; background-color: #738495;">
                        </td>
                    </tr>
                    <tr class="bodyLine">
                        <td style="height: 1px; background-color: #FFFFFF;">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
              <tr style="height: 8px">
                        <td class="baseLable" style="width: 105px;">
                            <asp:Label ID="Label18" runat="server" Text="<%$ Resources:BaseInfo,Lease_lblCommOper %>" CssClass="labelStyle"></asp:Label>
                        </td>
                        <td class="baseInput" style="height: 8px">
                            <asp:TextBox ID="txtCommOper" runat="server" BackColor="#F5F5F4" CssClass="ipt160px"
                                ReadOnly="True"></asp:TextBox></td>
                    </tr>       
    <tr style="height:10px">
    <td colspan="2"></td>
    </tr>
        <tr style="height:5px">
            <td style="width: 105px">
            </td>
            <td class="baseInput">
                <table width="165px" border="0" cellpadding="0" cellspacing="0">
                    <tr class="bodyLine">
                        <td style="height: 1px; background-color: #738495;">
                        </td>
                    </tr>
                    <tr class="bodyLine">
                        <td style="height: 1px; background-color: #FFFFFF;">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    <tr>
        <td class="baseLable" style="width: 105px">
            <asp:Label ID="Label59" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labNote %>"></asp:Label></td>
        <td rowspan="2" valign="top">
            <asp:TextBox ID="listBoxRemark" runat="server" Height="50px" CssClass="ipt160px" ReadOnly="true"></asp:TextBox></td>
    </tr>
        <tr>
            <td style="width: 105px">
            </td>
        </tr>
                    <tr>
                        <td style="width: 105px; height: 30px;">
                        </td>
                        <td rowspan="1" valign="top" style="height: 30px">
                            </td>
                    </tr>
                </table>
                    
                    </td>
                    
                    </tr>
                    </table>           
             </td>
             </tr>
             </table>
  </div>
        <asp:HiddenField ID="HidMenu" runat="server" />
    </form>
</body>
</html>

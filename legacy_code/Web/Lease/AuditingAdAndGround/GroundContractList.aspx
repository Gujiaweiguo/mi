<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GroundContractList.aspx.cs" Inherits="Lease_AuditingAdAndGround_GroundContractList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<!--
/// Author:hesijian
/// Date:2009-11-19
/// Content:Created
-->
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Tab_lblAreaInfo")%></title>
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/longCss/webtab.css" rel="stylesheet" type="text/css"/>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        
          <!--
            talbe.baseShop tr{ height:50px;}
            table.baseShop tr.headLine{ height:1px; }
            table.baseShop tr.bodyLine{ height:1px;}
        -->
        
        
    </style>    
    <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
     <script type="text/javascript"  src="../../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript" src="../../JavaScript/Calendar.js" language="javascript" charset="gb2312"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"> </script>
	<script type="text/javascript">
	    function Load()
	    {
	        loadTitle();
	    }
	</script>
</head>
<body onload="Load()" style="margin:0px">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
    <div>
       <table class="baseShop" border="0" cellpadding="0" cellspacing="0" style="height: 1px" width="100%">
       <tr>
                <td class="tdTopBackColor" style="width: 100%; height: 25px" valign="top">
                    <img alt="" class="imageLeftBack" src="" />
                    <asp:Label ID="Label12" runat="server" Text="<%$ Resources:BaseInfo,Tab_lblAreaInfo %>" Width="344px"></asp:Label></td>
                <td class="tdTopRightBackColor" colspan="2" style="width: 100%; height: 25px; text-align:right;" valign="top">
                    <img class="imageRightBack" alt="img"  src=""/></td>
            </tr>
            <tr class="headLine"> 
                <td class="tdBackColor" style="background-color:White;" colspan="4">
                </td>
             </tr>
            </table>
            <table border="0" cellpadding="0" cellspacing="0" style="width:100%; height:357px" >
            <tr >
            <td class="tdBackColor" valign="top" style="width:30%; height: 349px;" align="right">
                <table width="100%">
                    <tr style="height:25px">
                        <td colspan="3" style="text-align: center">
                            <table style="width:182px;" border="0" cellpadding="0" cellspacing="0">
                                <tr  class="bodyLine">
                                    <td style="height: 1px; background-color: #738495;" >
                                    </td>
                                </tr>
                                <tr  class="bodyLine">
                                    <td style="height: 1px; background-color: #ffffff;" >
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="text-align: center">
                            <asp:GridView ID="gvShop" runat="server" AutoGenerateColumns="False" BackColor="White"
                                Width="248px" OnSelectedIndexChanged="gvShop_SelectedIndexChanged" BorderStyle="Inset">
                                <Columns>
                                    <asp:BoundField DataField="ConAreaID">
                                        <ItemStyle CssClass="hidden" />
                                        <HeaderStyle CssClass="hidden" />
                                        <FooterStyle CssClass="hidden" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="AreaCode" HeaderText="<%$ Resources:BaseInfo,Rpt_AreaTypeCode %>">
                                        <ItemStyle BorderColor="#E1E0B2" />
                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ConAreaName" HeaderText="<%$ Resources:BaseInfo,Rpt_AreaName %>">
                                        <ItemStyle BorderColor="#E1E0B2" />
                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                    </asp:BoundField>
                                    <asp:CommandField HeaderText="<%$ Resources:BaseInfo,PotShop_Selected %>" ShowSelectButton="True">
                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                        <ItemStyle BorderColor="#E1E0B2" />
                                    </asp:CommandField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </td>
                <td class="tdBackColor" colspan="2" style="height: 349px" valign="top" align="center">
                 <table class="tdBackColor" style="width: 238px" >
                       <tr style="height:15px">
                           <td style="width: 2239px" ></td>
                           <td valign="bottom" style="width: 329px">
                               <table style="width:136px;" border="0" cellpadding="0" cellspacing="0">
                                   <tr  class="bodyLine">
                                       <td style="height: 1px; background-color: #738495;" >
                                       </td>
                                   </tr>
                                   <tr  class="bodyLine">
                                       <td style="height: 1px; background-color: #ffffff;" >
                                       </td>
                                   </tr>
                               </table>
                           </td>
                       </tr>
                       <tr>
                           <td style="text-align: right; width: 2239px; height: 30px;" class="tdBackColor">
                               <asp:Label ID="lblAdBoardCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Rpt_AreaTypeCode %>" Width="74px"></asp:Label></td>
                           <td style="width: 329px; height: 30px;" class="tdBackColor">
                               <asp:DropDownList ID="ddlAreaCode" runat="server" AutoPostBack="True" Enabled ="false"
                                   Width="133px">
                               </asp:DropDownList></td>
                       </tr>
                       <tr><td style="TEXT-ALIGN: right; width: 2239px;" class="tdBackColor">
                           <asp:Label ID="lblAdBoardName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Rpt_AreaName %>"></asp:Label></td><td style="WIDTH: 329px; HEIGHT: 21px" class="tdBackColor"><asp:TextBox id="txtConAreaName" runat="server" CssClass="ipt160px" Width="128px" MaxLength="64" ReadOnly="True"></asp:TextBox></td></tr>
                       <tr>
                           <td style="text-align: right; width: 2239px;" class="tdBackColor">
                               <asp:Label CssClass="labelStyle" ID="Label53" runat="server" Text="<%$ Resources:BaseInfo,LeaseAreaType_AreaType %>"></asp:Label></td>
                           <td style="" class="tdBackColor">
                               <asp:DropDownList ID="cmbAreaType" runat="server" Width="133px" Enabled="False">
                               </asp:DropDownList></td>
                       </tr>
                       <tr><td style="TEXT-ALIGN: right; width: 2239px;" class="tdBackColor">
                               <asp:Label CssClass="labelStyle" ID="Label60" runat="server" Text="<%$ Resources:BaseInfo,PotShop_lblShopStartDate %>"></asp:Label></td><td style="WIDTH: 329px; HEIGHT: 21px" class="tdBackColor">
                               <asp:TextBox ID="txtStartDate"  runat="server" CssClass="ipt160px" Width="129px" ReadOnly="true"></asp:TextBox></td></tr>
                       <tr>
                           <td style="text-align: right; width: 2239px;" class="tdBackColor">
                               <asp:Label CssClass="labelStyle" ID="Label61" runat="server" Text="<%$ Resources:BaseInfo,LblDate_EndDate %>"></asp:Label></td>
                           <td class="tdBackColor">
                               <asp:TextBox ID="txtEndDate"  runat="server" CssClass="ipt160px" Width="129px" ReadOnly="true"></asp:TextBox></td>
                       </tr>
                       <tr>
                           <td style="text-align: right; width: 2239px;" class="tdBackColor">
                               <asp:Label ID="lblNote" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,User_lblNote %>"></asp:Label></td>
                           <td class="tdBackColor" rowspan="2">
                               &nbsp; &nbsp;&nbsp;
                               <asp:TextBox ID="txtNote" runat="server" CssClass="ipt150px" Height="97px" ReadOnly="true"></asp:TextBox></td>
                       </tr>
                       <tr>
                           <td style="text-align: right; width: 2239px;" class="tdBackColor">
                               </td>
                       </tr>
                                              <tr>
                           <td style="text-align: right; width: 2239px;" >
                               <asp:RadioButton ID="rdoConfirm" runat="server" Text="<%$ Resources:BaseInfo,Conshop_Confirm %>" Checked="True" GroupName="shop" Enabled="false" /></td>
                           <td  style="text-align: left">
                               <asp:RadioButton ID="rdoYes" runat="server" Text="<%$ Resources:Parameter,BizGrp_YES %>" GroupName="shop" Enabled="false" />
                               <asp:RadioButton ID="rdoNo" runat="server" Text="<%$ Resources:Parameter,BizGrp_NO %>" GroupName="shop" Enabled="false" /></td>
                       </tr>
            </table>
                </td>
            </tr>
                <tr style="height:3px">
                    <td class="tdBackColor" colspan="3" style="text-align: center" valign="top">
                        <table style="width:95%;" border="0" cellpadding="0" cellspacing="0">
                            <tr  class="bodyLine">
                                <td style="height: 1px; background-color: #738495;" >
                                </td>
                            </tr>
                            <tr  class="bodyLine">
                                <td style="height: 1px; background-color: #ffffff;" >
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr style="height:45px" valign="top">
                    <td class="tdBackColor" style="width: 30%; height: 44px;" valign="top">
                    </td>
                    <td class="tdBackColor" style="width: 30%; text-align: right; height: 44px;" valign="top">
                    </td>
                    <td class="tdBackColor" style="width: 40%; text-align: right; padding-right:20px; height: 44px;" valign="top"></td>
                </tr>
            </table>
    </div>
            <input id="allvalue" runat="server" style="width: 25px" type="hidden" />
             
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>

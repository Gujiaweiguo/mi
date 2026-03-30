<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdContractList.aspx.cs" Inherits="Lease_AuditingAdAndGround_AdContractList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Tab_lblADInfo")%></title>
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
                    <asp:Label ID="Label12" runat="server" Text="<%$ Resources:BaseInfo,Tab_lblADInfo %>" Width="344px"></asp:Label></td>
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
                    <tr>
                        <td colspan="3" style="text-align: center">
                            <asp:GridView ID="gvShop" runat="server" AutoGenerateColumns="False" BackColor="White"
                                Width="248px" OnSelectedIndexChanged="gvShop_SelectedIndexChanged" BorderStyle="Inset" OnRowDataBound="gvShop_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="ConAdBoardID">
                                        <ItemStyle CssClass="hidden" />
                                        <HeaderStyle CssClass="hidden" />
                                        <FooterStyle CssClass="hidden" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="AdBoardCode" HeaderText="<%$ Resources:BaseInfo,AdBoard_lblAdBoardCode %>">
                                        <ItemStyle BorderColor="#E1E0B2" />
                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="AdBoardName" HeaderText="<%$ Resources:BaseInfo,AdBoard_lblAdBoardName %>">
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
                <td class="tdBackColor" colspan="2" style="height: 300px; width: 513px;" valign="top" align="left">
                 <table class="tdBackColor">
                       <tr style="height:15px">
                           <td style="width: 154px" ></td>
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
                           <td style="width: 329px" valign="bottom"><table style="width:136px;" border="0" cellpadding="0" cellspacing="0">
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
                           <td style="text-align: right; height: 30px; width: 154px;" class="tdBackColor">
                               <asp:Label ID="lblAdBoardCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdBoard_lblAdBoardCode %>" Width="74px"></asp:Label></td>
                           <td style="width: 329px; height: 30px;" class="tdBackColor" align="left">
                               <asp:TextBox ID="txtAdBoardCode" runat="server" CssClass="ipt160px" Width="129px" Enabled="False"></asp:TextBox></td>
                           <td align="left" class="tdBackColor" style="width: 329px; height: 30px">
                             <asp:RadioButton ID="rbtDay" runat="server" Checked="True" GroupName="freq" Text="<%$ Resources:BaseInfo,Internet_optDaily %>" AutoPostBack="True" Enabled="False" />
                             <asp:RadioButton ID="rbtWeek" runat="server" GroupName="freq" Text="<%$ Resources:BaseInfo,Internet_optWeekly %>" AutoPostBack="True" Enabled="False" />
                             <asp:RadioButton ID="rbtMonth" runat="server" GroupName="freq" Text="<%$ Resources:BaseInfo,Internet_optMthly %>" AutoPostBack="True" Enabled="False" /></td>
                       </tr>
                       <tr><td style="TEXT-ALIGN: right; width: 154px;" class="tdBackColor">
                           <asp:Label ID="lblAdBoardName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdBoard_lblAdBoardName %>"></asp:Label></td><td style="WIDTH: 329px; HEIGHT: 21px" class="tdBackColor" align="left"><asp:TextBox id="txtConAdBoardName" runat="server" CssClass="ipt160px" Width="129px" MaxLength="64" ReadOnly="True" Enabled="False"></asp:TextBox></td>
                           <td align="left" class="tdBackColor" style="width: 329px;" rowspan="9" valign="top">
                           <table style="height:100%; width:100%;">
                           <tr>
                           <td>
                               <asp:DropDownList ID="ddlDay" runat="server" Enabled="False">
                               </asp:DropDownList>
                               <asp:Label ID="Label3" runat="server" Text="天"></asp:Label>
                               <asp:CheckBoxList ID="cblWeek" runat="server" Visible="False" Enabled="False">
                               </asp:CheckBoxList>
                               <asp:DropDownList ID="ddlMonthFrom" runat="server" Visible="False" Enabled="False">
                               </asp:DropDownList>
                               <asp:Label ID="Label2" runat="server" Text="—" Visible="False"></asp:Label>
                               <asp:DropDownList ID="ddlMonthTo" runat="server" Visible="False" Enabled="False">
                               </asp:DropDownList>
                           </td>
                           </tr>
                           </table>
                           </td>
                       </tr>
                       <tr>
                           <td style="text-align: right; width: 154px;" class="tdBackColor">
                               <asp:Label CssClass="labelStyle" ID="Label60" runat="server" Text="<%$ Resources:BaseInfo,PotShop_lblShopStartDate %>"></asp:Label></td>
                           <td style="WIDTH: 329px" class="tdBackColor" align="left">
                               <asp:TextBox ID="txtStartDate" onclick="calendar()" runat="server" CssClass="ipt160px" Width="129px" Enabled="False"></asp:TextBox></td>
                       </tr>
                       <tr><td style="TEXT-ALIGN: right; width: 154px;" class="tdBackColor">
                               <asp:Label CssClass="labelStyle" ID="Label61" runat="server" Text="<%$ Resources:BaseInfo,LblDate_EndDate %>"></asp:Label></td><td style="WIDTH: 329px; HEIGHT: 21px" class="tdBackColor" align="left">
                               <asp:TextBox ID="txtEndDate" onclick="calendar()" runat="server" CssClass="ipt160px" Width="129px" Enabled="False"></asp:TextBox></td>
                       </tr>
                       <tr>
                           <td class="tdBackColor" align="right" style="width: 154px">
                               <asp:Label ID="Label1" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Radio_lblAirtime %>"></asp:Label></td>
                           <td class="tdBackColor" align="left" style="width: 329px">
                               <asp:TextBox ID="txtTime" runat="server" CssClass="ipt160px" Width="50px" Enabled="False"></asp:TextBox></td>
                       </tr>
                     <tr>
                         <td align="right" class="tdBackColor" style="width: 154px">
                             <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblRentArea %>"></asp:Label></td>
                         <td align="left" class="tdBackColor" style="width: 329px">
                             <asp:TextBox ID="txtArea" runat="server" CssClass="ipt160px" Enabled="False" MaxLength="4"
                                 Width="50px"></asp:TextBox></td>
                     </tr>
                       <tr>
                           <td class="tdBackColor" colspan="2" rowspan="2">
                               </td>
                       </tr>
                       <tr>
                       </tr>
                                              <tr>
                           <td class="tdBackColor" colspan="2" align="center">
                               <asp:RadioButton ID="rdoConfirm" runat="server" Text="<%$ Resources:BaseInfo,Conshop_Confirm %>" Checked="True" GroupName="shop" /><asp:RadioButton ID="rdoYes" runat="server" Text="<%$ Resources:Parameter,BizGrp_YES %>" GroupName="shop" />
                               <asp:RadioButton ID="rdoNo" runat="server" Text="<%$ Resources:Parameter,BizGrp_NO %>" GroupName="shop" /></td>
                       </tr>
                     <tr>
                         <td align="center" class="tdBackColor" colspan="2" style="height: 25px">
                         </td>
                     </tr>
            </table>
                </td>
            </tr>
                <tr style="height:3px">
                    <td class="tdBackColor" valign="top" colspan="3">
                        <table style="width:95%; text-align:center;" border="0" cellpadding="0" cellspacing="0">
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
                    <td class="tdBackColor" style="text-align: center; height: 44px; width: 513px;" valign="top" colspan="2">
                        &nbsp;
                    </td>
                </tr>
            </table>
    </div>
            <input id="allvalue" runat="server" style="width: 25px" type="hidden" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>

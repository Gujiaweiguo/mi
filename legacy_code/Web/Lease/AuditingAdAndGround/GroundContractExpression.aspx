<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GroundContractExpression.aspx.cs" Inherits="Lease_AuditingAdAndGround_GroundContractExpression" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<!--
/// Author:hesijian
/// Date:2009-11-19
/// Content:Created
-->
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Lease_lbBalance")%></title>
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/longCss/webtab.css" rel="stylesheet" type="text/css"/>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
         <!--
        table.tblLeft tr{ height:15px;}
        table.tblLeft tr.headLine{ height:1px; }
        table.tblLeft tr.colLine{ height:10px; }
        table.tblLeft tr.bodyLine{ height:1px; }
        table.tblLeft tr.topLine{ height:25px; }
        table.tblLeft tr.gridView{ height:200px; }
        table.tblLeft td.leftLable{ padding-right:5px;text-align:right;}
        
        table.tblFast tr{ height:28px;}
        -->
        
        
    </style>  
     <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
     <script type="text/javascript"  src="../../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript" src="../../JavaScript/Calendar.js"  charset="gb2312" language="javascript"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"> </script>
	<script type="text/javascript">
	    function Load()
	    {
	        loadTitle();
          	document.getElementById("lblTotalNumType").style.display="none";
            document.getElementById("lblCurrentType").style.display="none";  
	    }
	    
        var prevselitem=null; 
        function selectx(row) 
        { 
            if(prevselitem!=null) 
            { 
                prevselitem.style.backgroundColor='#FFFFFF';
                 
            } 
            row.style.backgroundColor='PeachPuff'; 
            prevselitem=row;
        } 
        
          function ClickEvent(d)
        {
              window.document.all("Hidden1").value = d; 
             if ((window.document.all("Hidden1").value != "undefined") && (window.document.all("Hidden1").value != "") && (window.document.all("Hidden1").value != "&nbsp;"))
             {
				var objImgBtn1 = document.getElementById('<%= LinkButton1.ClientID %>');  
                objImgBtn1.click();
             }
			 else
				return;    
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
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
            <!--********************left**********************-->
                <td class="tdBackColor" colspan="2" style="text-align:left;width:34%;" valign="top">
        
                <table class="tblLeft" border="0" cellpadding="0" cellspacing="0" style="width: 89%">
    <tr class="topLine">
       <td class="tdTopBackColor" style="width: 293px;" colspan="3">
                    <img alt="" class="imageLeftBack" /><asp:Label ID="Label10" runat="server"
                        Text="<%$ Resources:BaseInfo,Formula_lblContent %>" Width="194px"></asp:Label></td>
       <td class="tdTopBackColor" valign="top" style="width: 115px">
                    <img  align="right" class="imageRightBack" style="width: 6px;"/>
                </td> 
    </tr>
    <tr class="headLine" >
        <td style="background-color:White;" colspan="4"></td> 
    </tr>
    <tr class="colLine">
        <td colspan="4" valign="middle" style="text-align: center">
            <table border="0" cellpadding="0" cellspacing="0" style="width: 273px">
                <tr class="bodyLine">
                    <td style="width:280px; height:1px; background-color:#738495;" class="tdBackColor"></td>
                </tr>
                <tr class="bodyLine">
                    <td style="width:280px; height:1px; background-color:#FFFFFF;" class="tdBackColor"></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="leftLable" align="left" style="padding-left:5px; height: 15px;">
            <asp:Label ID="labChargeTypeID" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ConLease_labChargeTypeID %>" Width="57px"></asp:Label></td>   
        <td align="left" style="height: 15px" colspan="3">
            <asp:DropDownList ID="cmbChargeTypeID" runat="server" CssClass="textBoxStyle" Width="200px"  AutoPostBack="True">
            </asp:DropDownList></td> 
    </tr>
    <tr>
        <td class="leftLable" style="height: 14px">
            <asp:Label ID="labFormulaType" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ConLease_labFormulaType %>" Width="60px"></asp:Label></td> 
        <td align="left" colspan="3" style="height: 14px">
            <asp:DropDownList ID="cmbFormulaType" runat="server" CssClass="textBoxStyle" Width="200px" AutoPostBack="True" >
            </asp:DropDownList></td>
    </tr>
    <tr>
        <td class="leftLable">
            <asp:Label ID="Label15" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblShopStartDate %>"></asp:Label></td>
        <td align="left" style="width: 63px">
            <asp:TextBox ID="txtBeginDate"　onclick="calendar()" runat="server" Width="70px" CssClass="ipt160px" Height="18px"></asp:TextBox></td>
        <td class="leftLable">
            <asp:Label ID="labEndDate" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LblDate_EndDate %>" Width="57px"></asp:Label></td>
        <td align="left" style="width: 115px">
            <asp:TextBox ID="txtOverDate" onclick="calendar()" runat="server" CssClass="ipt160px" Width="70px" Height="18px"></asp:TextBox></td>
    </tr>
    
    <tr>
        <td class="leftLable" style="height: 15px">
            <asp:Label ID="labBaseAmt" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ConLease_labBaseAmt %>"></asp:Label></td> 
        <td colspan="3" align="left" style="height: 15px">
            <asp:TextBox ID="txtBaseAmt" runat="server" CssClass="ipt160px" Width="70px" style="ime-mode:disabled;" Height="18px" Enabled="False">0</asp:TextBox></td> 
    </tr>
    <tr height="20">
        <td colspan="4" valign="middle" style="text-align: center">
            <table border="0" cellpadding="0" cellspacing="0" width="280">
                <tr class="bodyLine">
                    <td style="width: 280px;background-color: #738495;" class="tdBackColor">
                    </td>
                </tr>
                <tr class="bodyLine">
                    <td style="width: 280px;background-color: #FFFFFF;" class="tdBackColor">
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr class="gridView" style="padding-left:10px;">
        <td style="width:296px; text-align: center;" colspan="7" class="tdBackColor" valign="top">
            <asp:GridView ID="GVType" runat="server" AutoGenerateColumns="False" Height="150px"
                Width="286px" OnRowDataBound="GVType_RowDataBound" BackColor="White" BorderColor="#E1E0B2" BorderWidth="1px" PageSize="8">
                <Columns>
                    <asp:BoundField DataField="FormulaID" >
                        <ItemStyle CssClass="hidden" Font-Size="12px" />
                        <HeaderStyle CssClass="hidden" />
                        <FooterStyle CssClass="hidden" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ChargeTypeName" HeaderText="<%$ Resources:BaseInfo,ConLease_labChargeTypeID %>" >
                        <HeaderStyle CssClass="gridviewtitle" Font-Size="12px" BorderColor="#E1E0B2" />
                        <ItemStyle Font-Size="12px" BorderColor="#E1E0B2" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FStartDate" HeaderText="<%$ Resources:BaseInfo,PotShop_lblShopStartDate %>" DataFormatString="{0:d}" HtmlEncode="False" >
                        <HeaderStyle CssClass="gridviewtitle" Font-Size="12px" BorderColor="#E1E0B2" />
                        <ItemStyle Font-Size="12px" BorderColor="#E1E0B2" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FEndDate" HeaderText="<%$ Resources:BaseInfo,ConLease_labEndDate %>" DataFormatString="{0:d}" HtmlEncode="False" >
                        <HeaderStyle CssClass="gridviewtitle" Font-Size="12px" BorderColor="#E1E0B2" />
                        <ItemStyle Font-Size="12px" BorderColor="#E1E0B2" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FormulaTypeName" HeaderText="<%$ Resources:BaseInfo,ConLease_labFormulaType %>" >
                        <HeaderStyle CssClass="gridviewtitle" Font-Size="12px" BorderColor="#E1E0B2" />
                        <ItemStyle Font-Size="12px" BorderColor="#E1E0B2" />
                    </asp:BoundField>
                </Columns>
                <HeaderStyle BackColor="#E1E0B2" Height="10px" Font-Bold="False" Font-Size="50px"  />
            </asp:GridView><asp:Label
                                                                        ID="lblTotalNumType" runat="server" Height="1px" Width="32px"></asp:Label><asp:Label
                                                                            ID="lblCurrentType" runat="server" ForeColor="Red" Height="1px" Width="1px">1</asp:Label></td> 
    </tr>
    <tr height="20px">
        <td colspan="4" valign="middle" style="text-align: center">
            <table border="0" cellpadding="0" cellspacing="0" width="280">
                <tr class="bodyLine">
                    <td style="width: 280px;background-color: #738495;" class="tdBackColor">
                    </td>
                </tr>
                <tr class="bodyLine">
                    <td style="width: 280px;background-color: #FFFFFF;" class="tdBackColor">
                    </td>
                </tr>
            </table>
        </td> 
    </tr>
</table>
                    <input id="Hidden1" runat="server" type="hidden" style="width: 16px; height: 14px;" />
                    &nbsp;
                    <asp:TextBox ID="TextBox1" runat="server" AutoPostBack="True" Visible="False" Width="12px" Height="1px"></asp:TextBox>
                    <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click" Height="1px" Width="1px"></asp:LinkButton>
                    
                    <asp:Button ID="btnBackType" runat="server" CssClass="buttonBack" Height="31px"
                                                                OnClick="btnBackType_Click" Text="<%$ Resources:BaseInfo,Button_back %>" Width="71px" Visible="False" /><asp:Button
                                                                    ID="btnNextType" runat="server" CssClass="buttonNext" Height="30px"
                                                                    OnClick="btnNextType_Click" Text="<%$ Resources:BaseInfo,Button_next %>" Width="73px" Visible="False" /></td>
                <!--*****************middle***********************
                -->
                <td style="3%; height: 370px; padding-left:6px; width: 14px;"></td> 
                <!--************************right********************-->
                <td style="text-align: center; width:49%; height: 370px;" valign="top">
        <table border="0" cellpadding="0" cellspacing="0" class="tdBackColor" style="width: 90%">
             <tr class="topLine">
                <td class="tdTopBackColor" style="width: 373px;">
                    <img alt="" class="imageLeftBack" /><asp:Label ID="Label43" runat="server"
                        Text="<%$ Resources:BaseInfo,Formula_lblContent %>" Width="260px"></asp:Label></td>
                <td class="tdTopRightBackColor" style="text-align: right">
                    <img class="imageRightBack" />
                </td>
            </tr>
            <tr class="headLine">
                <td colspan="2" style="background-color: white;" >
                </td>
            </tr>
            <tr height="95">
                <td colspan="2" style="text-align: left; height: 95px; padding-left: 10px;" >
                                <fieldset style="border-right: #c0c0c0 1px solid; border-top: #c0c0c0 1px solid;
                                    border-left: #c0c0c0 1px solid; border-bottom: #c0c0c0 1px solid; width: 365px; padding-left:10px">
                                    <legend style="text-align: left">
                                        <asp:Label ID="labFastnessHire" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ConLease_labFastnessHire %>"></asp:Label>
                                    </legend>
                                    <table class="tblFast" border="0" cellpadding="0" cellspacing="0" width="100%" style="height: 90px">
                                        <tr>
                                            <td style="height: 28px"></td>
                                            <td style="width: 107px; height: 28px;" colspan=2>
                                                <asp:RadioButton ID="rabMonthHire" runat="server" Text="<%$ Resources:BaseInfo,ConLease_rabMonthHire %>"
                                                    TextAlign="Left" CssClass="labelStyle" GroupName="b" Width="65px" />
                                                </td>
                                            <td width="240" style="padding-left:25px; height: 28px;" colspan=3 align="left">
                                                <asp:RadioButton ID="rabDayHire" runat="server" Text="<%$ Resources:BaseInfo,ConLease_rabDayHire %>"
                                                    TextAlign="Left" CssClass="labelStyle" GroupName="b" /></td>
                                        </tr>
                                        <tr>
                                            <td style="height: 28px"></td>
                                            <td class="tdBackColor" colspan="2" style="vertical-align: top; height: 28px">
                                                <asp:Label ID="labFixedRental" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ConLease_labFixedRental %>"
                                                    Width="45px"></asp:Label>
                                                <asp:TextBox ID="txtFixedRental" runat="server" CssClass="ipt160px" Width="65px" style="ime-mode:disabled;" Height="18px">0</asp:TextBox></td>
                                            <td style="vertical-align: top; width: 65px; height: 28px;">
                                                &nbsp;</td>
                                            <td class="tdBackColor" style="width: 65px; vertical-align: top; height: 28px;" align="left">
                                                </td>
                                        </tr>
                                    </table>
                                </fieldset>
                </td>
            </tr>
            <tr>
                <td colspan="2" valign="top" align="left">
                    <table border="0" cellpadding="0" cellspacing="0" class="tdBackColor" style="width: 100%">
                        <tr>
                            <td valign="top" style="padding-left:10px; padding-right:10px; width: 364px; text-align: left;">
                                &nbsp;<table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td valign="top" style="height: 281px;" colspan="2">
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%" style=" padding-left:5px;">
                                                    <tr height="30">
                                                        <td class="tdBackColor" colspan="4" align="left" rowspan="2" valign="top">
                                                            &nbsp;&nbsp;
                               
                                                            &nbsp;&nbsp;
                                                        </td>
                                                    </tr>                                                    
                                                    <tr height="125px" style="overflow-x:hidden;overflow-y:scroll">
                                                    </tr>
                                                    <tr >
                                                        <td align="left" style=" padding-left:5px;width:10px; padding-right:3px; height: 24px;">
                                                            </td>
                                                        <td style="width: 57px; height: 24px;">
                                                            </td>
                                                        <td style="width: 45px; height: 24px;">
                                                            </td>
                                                        <td align="left" style="width: 38px; height: 24px;">
                                                            </td>
                                                    </tr>
                                                    <tr >
                                                        <td style="height: 9px;" colspan="4">
                                                            &nbsp;&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr class="colLine">
                                                        <td colspan="4" align="center">
                                                        </td>
                                                    </tr>
                                                    <tr height="36px">
                                                        <td align="center" width="50%" colspan=2>
                                                            &nbsp;</td>
                                                        <td align="center" style="width: 50%" colspan="3">
            &nbsp;</td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <!--*********************-->
                                        </tr>
                                        
                                                    <tr height="8"></tr>
                                    </table>
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>

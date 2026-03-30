<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExpressionPalaver.aspx.cs" Inherits="Lease_ExpressionPalaver" ResponseEncoding="gb2312"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "LeaseBargain_lblPostil")%></title>
    <link href="../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Tabbar/css/longCss/webtab.css" rel="stylesheet" type="text/css"/>
    <link href="../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
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
    <script language="javascript" type="text/javascript" src="../JavaScript/TabTools.js"></script>
     <script type="text/javascript"  src="../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript" src="../JavaScript/Calendar.js" language="javascript"></script>
	<script type="text/javascript" src="../JavaScript/Common.js"> </script>
	<script type="text/javascript">
	    
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

        function Load()
	    {
//	        document.getElementById("lblTotalNum").style.display="none";
//            document.getElementById("lblCurrent").style.display="none";
//	        document.getElementById("lblTotalNumT").style.display="none";
//            document.getElementById("lblCurrentT").style.display="none";
//          	document.getElementById("lblTotalNumType").style.display="none";
//            document.getElementById("lblCurrentType").style.display="none";  
	    }
	    function BtnUp( p )
        {
	        var t = String(p)
	        var l = t.substring(3,15); 
	        document.getElementById( p ).style.backgroundImage = 'url(../App_Themes/CSS/BtnImage/btn_' + l + '.gif)';
        }
        function BtnOver( p )
        {
	        var t = String(p)
	        var l = t.substring(3,15); 
	        document.getElementById( p ).style.backgroundImage = 'url(../App_Themes/CSS/BtnImage/over_' + l + '.gif)';
        }
    </script>
</head>
<body onload='Load()' style="margin:0">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
   <div id="Expression">
 <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
            <tr>
            <!--********************left**********************-->
                <td class="tdBackColor" colspan="2" style="text-align:left; width:45%; height: 335px;" valign="top">
        
                <table class="tblLeft" border="0" cellpadding="0" cellspacing="0" style="width: 100%" >
     <tr class="topLine">
       <td class="tdTopBackColor" style="width: 293px; height: 25px;" colspan="3">
                    <img alt="" class="imageLeftBack" /><asp:Label ID="Label4" runat="server"
                        Text="<%$ Resources:BaseInfo,Formula_lblContent %>" Width="189px"></asp:Label></td>
       <td class="tdTopBackColor" valign="top" style="width: 100px; height: 25px;">
                    <img  align="right" class="imageRightBack" style="width: 6px;"/>
                </td> 
    </tr>
    <tr class="headLine" >
        <td style="background-color:White;" colspan="4"></td> 
    </tr>
    <tr class="colLine">
        <td colspan="4" valign="middle" align="center">
            <table border="0" cellpadding="0" cellspacing="0" width="95%">
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
        <td class="leftLable" align="left" style="padding-left:5px">
            <asp:Label ID="labChargeTypeID" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ConLease_labChargeTypeID %>" Width="57px"></asp:Label></td>   
        <td align="left" colspan="3" style="padding-right: 5px">
            <asp:DropDownList ID="cmbChargeTypeID" runat="server" CssClass="textBoxStyle" Width="200px" BackColor="#F5F5F4" Enabled="False">
            </asp:DropDownList></td>
    </tr>
    <tr>
        <td class="leftLable">
            <asp:Label ID="labFormulaType" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ConLease_labFormulaType %>" Width="60px"></asp:Label></td> 
        <td align="left" colspan="3">
            <asp:DropDownList ID="cmbFormulaType" runat="server" CssClass="textBoxStyle" Width="200px" BackColor="#F5F5F4" Enabled="False">
            </asp:DropDownList></td>
    </tr>
    <tr style=" text-align:left;">
        <td class="leftLable" >
            <asp:Label ID="Label15" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblShopStartDate %>"></asp:Label></td>
        <td align="left" style="text-align:left;">
            <asp:TextBox ID="txtBeginDate"　onclick="calendar()" runat="server" Width="69px" BackColor="#F5F5F4" ReadOnly="True" Enabled="False" Height="18px" CssClass="ipt160px"></asp:TextBox></td>
        <td class="leftLable" style="text-align: left;" align="left">
            <asp:Label ID="labEndDate" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ConLease_labEndDate %>" Width="53px"></asp:Label></td>
        <td  style="width: 70px; text-align:left;">
            <asp:TextBox ID="txtOverDate" onclick="calendar()" runat="server" CssClass="ipt160px" Width="70px" BackColor="#F5F5F4" ReadOnly="True" Enabled="False" Height="18px"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="leftLable">
            <asp:Label ID="labBaseAmt" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ConLease_labBaseAmt %>"></asp:Label></td> 
        <td colspan="3" align="left">
            <asp:TextBox ID="txtBaseAmt" runat="server" CssClass="ipt160px" Width="69px" BackColor="#F5F5F4" ReadOnly="True" Height="18px"></asp:TextBox></td> 
    </tr>
    <tr height="20">
        <td colspan="4" valign="middle" align="center">
            <table border="0" cellpadding="0" cellspacing="0" width="95%">
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
    <tr class="gridView" style="width:100%">
        <td style="width:100%; text-align:center;" colspan="7" class="tdBackColor" valign="top">
            <asp:GridView ID="GVType" runat="server" AutoGenerateColumns="False" Height="100%"
                Width="90%" OnRowCommand="GVType_RowCommand" OnRowDataBound="GVType_RowDataBound" BackColor="White" BorderColor="#E1E0B2" BorderWidth="1px" PageSize="8" AllowPaging="True" BorderStyle="Outset" OnPageIndexChanging="GrdBrandOperateType_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="FormulaID" >
                        <ItemStyle CssClass="hidden" Font-Size="12px" />
                        <HeaderStyle CssClass="hidden" />
                        <FooterStyle CssClass="hidden" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ChargeTypeName" HeaderText="<%$ Resources:BaseInfo,ConLease_labChargeTypeID %>" >
                        <HeaderStyle CssClass="gridviewtitle" Font-Size="12px" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                        <ItemStyle Font-Size="12px" BorderColor="#E1E0B2" />
                    </asp:BoundField>
                    <asp:BoundField DataField="StartDate" HeaderText="<%$ Resources:BaseInfo,PotShop_lblShopStartDate %>" HtmlEncode="False" >
                        <HeaderStyle CssClass="gridviewtitle" Font-Size="12px" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                        <ItemStyle Font-Size="12px" BorderColor="#E1E0B2" />
                    </asp:BoundField>
                    <asp:BoundField DataField="EndDate" HeaderText="<%$ Resources:BaseInfo,ConLease_labEndDate %>" HtmlEncode="False" >
                        <HeaderStyle CssClass="gridviewtitle" Font-Size="12px" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                        <ItemStyle Font-Size="12px" BorderColor="#E1E0B2" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FormulaTypeName" HeaderText="<%$ Resources:BaseInfo,ConLease_labFormulaType %>" >
                        <HeaderStyle CssClass="gridviewtitle" Font-Size="12px" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                        <ItemStyle Font-Size="12px" BorderColor="#E1E0B2" />
                    </asp:BoundField>
                </Columns>
                <HeaderStyle BackColor="#E1E0B2" Height="10px" Font-Bold="False" Font-Size="50px"  />
                <PagerStyle BackColor="White" ForeColor="#000066"   HorizontalAlign="Right" />
                                                                <PagerTemplate>                                                       

                                                                <asp:LinkButton ID="LinkButtonPreviousPage" runat="server" CommandArgument="Prev" CommandName="Page" 
                                                                 Visible="<%# ((GridView)Container.NamingContainer).PageIndex != 0 %>">上一页</asp:LinkButton> 

                                                                <asp:LinkButton ID="LinkButtonNextPage" runat="server" CommandArgument="Next" CommandName="Page" 
                                                                 Visible="<%# ((GridView)Container.NamingContainer).PageIndex != ((GridView)Container.NamingContainer).PageCount - 1 %>">下一页</asp:LinkButton> 

                                                                <asp:textbox id="txtNewPageIndex" runat="server" width="20px" text='<%# ((GridView)Container.Parent.Parent).PageIndex + 1 %>' />/
                                                                <asp:Label ID="LabelPageCount" runat="server" 
                                                                 Text="<%# ((GridView)Container.NamingContainer).PageCount %>"></asp:Label> 
                                                                <asp:linkbutton id="btnGo" runat="server" causesvalidation="False" commandargument="-1" commandname="Page" text="GO" /> 
                                                                  </PagerTemplate> 
            </asp:GridView>
        </td> 
    </tr>
    <tr height="10px">
        <td colspan="4" valign="middle" align="center" style="height: 15px">
            <table border="0" cellpadding="0" cellspacing="0" width="95%">
                <tr class="bodyLine">
                    <td style="background-color: #738495;" class="tdBackColor">
                    </td>
                </tr>
                <tr class="bodyLine">
                    <td style="background-color: #FFFFFF;" class="tdBackColor">
                    </td>
                </tr>
            </table>
        </td> 
    </tr>
</table>
                    <input id="Hidden1" runat="server" type="hidden" style="width: 16px; height: 1px;" />
                    <input id="Hidden2" runat="server" type="hidden" style="width: 8px; height: 1px;" />&nbsp;
                    <asp:TextBox ID="TextBox1" runat="server" AutoPostBack="True" Visible="False" Width="12px" Height="1px"></asp:TextBox>
                    <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click" Height="1px" Width="1px"></asp:LinkButton></td>
                <!--*****************middle***********************
                -->
                <td style="width:10px; height: 335px; padding-left:6px"></td> 
                <!--************************right********************-->
                <td style="text-align: left; width:49%; height: 335px;" valign="top">
        <table border="0" cellpadding="0" cellspacing="0" class="tdBackColor" style="width: 94%">
            <tr class="topLine">
                <td class="tdTopBackColor" style="width: 393px;">
                    <img alt="" class="imageLeftBack" /><asp:Label ID="Label43" runat="server"
                        Text="<%$ Resources:BaseInfo,Formula_lblContent %>" Width="293px"></asp:Label></td>
                <td class="tdTopRightBackColor" style="width: 7px;; text-align: right">
                    <img class="imageRightBack" />
                </td>
            </tr>
            <tr class="headLine">
                <td colspan="2" style="background-color: white;" >
                </td>
            </tr>
            <tr height="100">
                <td colspan="2" style="text-align: center">
                                <fieldset style="border-right: #c0c0c0 1px solid; border-top: #c0c0c0 1px solid;
                                    border-left: #c0c0c0 1px solid; border-bottom: #c0c0c0 1px solid; width: 94%;">
                                    <legend style="text-align: left">
                                        <asp:Label ID="labFastnessHire" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ConLease_labFastnessHire %>"></asp:Label>
                                    </legend>
                                    <table class="tblFast" border="0" cellpadding="0" cellspacing="0" width="100%" style="height: 90px">
                                        <tr>
                                            <td width="5"></td>
                                            <td style="width: 107px;" colspan=2>
                                                <asp:RadioButton ID="rabMonthHire" runat="server" Text="<%$ Resources:BaseInfo,ConLease_rabMonthHire %>"
                                                    TextAlign="Left" CssClass="labelStyle" GroupName="b" Width="65px" Enabled="False" />
                                                </td>
                                            <td width="240" style="padding-left:25px" colspan=3 align="left">
                                                <asp:RadioButton ID="rabDayHire" runat="server" Text="<%$ Resources:BaseInfo,ConLease_rabDayHire %>"
                                                    TextAlign="Left" CssClass="labelStyle" GroupName="b" Enabled="False" /></td>
                                        </tr>
                                        <tr>
                                            <td width="10"></td>
                                            <td style="width: 60px;" align="right">
                                                <asp:Label ID="lblRentArea" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblRentArea %>"
                                                    Width="60px"></asp:Label></td>
                                            <td class="tdBackColor" style="width: 50px;">
                                                <asp:TextBox ID="txtArea" runat="server" CssClass="ipt160px" Width="50px" ReadOnly="True" BackColor="#F5F5F4" Height="18px"></asp:TextBox></td>
                                            <td style="padding-left:5px; width: 65px;">
                                                <asp:Label ID="labUnitHire" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ConLease_labUnitHire %>"
                                                    Width="60px"></asp:Label></td>
                                            <td class="tdBackColor" style="width: 50px;">
                                                <asp:TextBox ID="txtUnitHire" runat="server" CssClass="ipt160px" Width="50px" BackColor="#F5F5F4" ReadOnly="True"></asp:TextBox></td>
                                            <td style="padding-left:5px;">
                                                <asp:Label ID="labFixedRental" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ConLease_labFixedRental %>"
                                                    Width="45px"></asp:Label></td>
                                            <td class="tdBackColor" style="width: 85px;" align="left">
                                                <asp:TextBox ID="txtFixedRental" runat="server" CssClass="ipt160px" Width="65px" BackColor="#F5F5F4" ReadOnly="True"></asp:TextBox></td>
                                        </tr>
                                    </table>
                                </fieldset>
                </td>
            </tr>
            <tr>
                <td colspan="2" valign="top" style="height: 257px; text-align: left;">
                    <table border="0" cellpadding="0" cellspacing="0" class="tdBackColor" style="width: 99%">
                        <tr>
                            <td valign="top" style="padding-left:10px; padding-right:10px; width: 100%;">
                                <fieldset style="border-right: #c0c0c0 1px solid; border-top: #c0c0c0 1px solid;
                                    border-left: #c0c0c0 1px solid; border-bottom: #c0c0c0 1px solid; width: 100%;">
                                    <legend style="text-align: left">
                                    <asp:Label ID="Label33" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ConLease_labDeductAndKeep %>"></asp:Label>
                                    </legend>
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 200px;">
                                        <tr>
                                            <td  valign="top" style="height: 240px; width:50%;">
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%" style=" padding-left:5px;">
                                                    <tr height="30">
                                                        <td class="tdBackColor" colspan="4" align="left" style="height: 30px">
                                                            <asp:RadioButton ID="rabFastness" runat="server" Text="<%$ Resources:BaseInfo,ConLease_rabFastness %>"
                                                                TextAlign="Left" CssClass="labelStyle" GroupName="a" Enabled="False" />
                                                            <asp:RadioButton ID="rabMonopole" runat="server" Text="<%$ Resources:BaseInfo,ConLease_rabMonopole %>"
                                                                TextAlign="Left" CssClass="labelStyle" GroupName="a" Enabled="False" />
                                                            <asp:RadioButton ID="rabMultilevel" runat="server" Text="<%$ Resources:BaseInfo,ConLease_rabMultilevel %>"
                                                                TextAlign="Left" CssClass="labelStyle" GroupName="a" Enabled="False" /></td>
                                                    </tr>
                                                    <tr height="130" >
                                                        <td class="tdBackColor" colspan="4" style="text-align: center; height: 160px;"
                                                            valign="middle">
                                                            <asp:GridView ID="GVDeductMoney" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#E1E0B2" BorderWidth="1px" OnRowDataBound="GVDeductMoney_RowDataBound" Width="180px" AllowPaging="True" Height="100%" OnPageIndexChanging="GrdBrandOperateType_PageIndexChanging" PageSize="6">
                                                                <Columns>
                                                                    <asp:BoundField DataField="FormulaID" Visible="False" />
                                                                    <asp:BoundField DataField="SalesTo" HeaderText="<%$ Resources:BaseInfo,ConLease_labSellCount %>" >
                                                                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Pcent" HeaderText="<%$ Resources:BaseInfo,ConLease_labDistill %>" >
                                                                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                    </asp:BoundField>
                                                                </Columns>
                                                                <HeaderStyle BackColor="#E1E0B2" Height="10px" Font-Bold="False" Font-Size="50px"  />
                                                                <PagerStyle BackColor="White" ForeColor="#000066"   HorizontalAlign="Right" />
                                                                <PagerTemplate>                                                       

                                                                <asp:LinkButton ID="LinkButtonPreviousPage" runat="server" CommandArgument="Prev" CommandName="Page" 
                                                                 Visible="<%# ((GridView)Container.NamingContainer).PageIndex != 0 %>">上一页</asp:LinkButton> 

                                                                <asp:LinkButton ID="LinkButtonNextPage" runat="server" CommandArgument="Next" CommandName="Page" 
                                                                 Visible="<%# ((GridView)Container.NamingContainer).PageIndex != ((GridView)Container.NamingContainer).PageCount - 1 %>">下一页</asp:LinkButton> 

                                                                <asp:textbox id="txtNewPageIndex" runat="server" width="20px" text='<%# ((GridView)Container.Parent.Parent).PageIndex + 1 %>' />/
                                                                <asp:Label ID="LabelPageCount" runat="server" 
                                                                 Text="<%# ((GridView)Container.NamingContainer).PageCount %>"></asp:Label> 
                                                                <asp:linkbutton id="btnGo" runat="server" causesvalidation="False" commandargument="-1" commandname="Page" text="GO" /> 
                                                                  </PagerTemplate> 
                                                            </asp:GridView></td>
                                                    </tr>
                                                    <tr >
                                                        <td style="height: 9px;" colspan="4">
                                                            <input id="HiddenDeduct1" runat="server" style="width: 3px; height: 4px" type="hidden" />
                                                            <input id="HiddenDeduct2" runat="server" style="width: 7px; height: 5px" type="hidden" />
                                                            <asp:Button ID="btnDeduct" runat="server" Height="4px" Text="Button" Width="17px" Visible="False" OnClick="btnDeduct_Click" /></td>
                                                    </tr>
                                                    <tr class="colLine">
                                                        <td colspan="4" align="center" style="text-align: center">
                                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 80%">
                                                                <tr class="bodyLine">
                                                                    <td style="width: 175px; height: 1px; background-color: #738495;">
                                                                    </td>
                                                                </tr>
                                                                <tr class="bodyLine">
                                                                    <td style="width: 175px; height: 1px; background-color: #FFFFFF;">
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" colspan=3 style="height: 15px">
                                                            &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp;</td>
                                                    </tr>
                                                </table>
                                                &nbsp;
                                            </td>
                                            <!--*********************-->
                                            <td width="50%" valign="top" style="height: 240px">
                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                    <tr height="30">
                                                        <td class="tdBackColor" colspan="3">
                                                            <asp:RadioButton ID="rabFastness2" runat="server" Text="<%$ Resources:BaseInfo,ConLease_rabFastness %>"
                                                                TextAlign="Left" CssClass="labelStyle" GroupName="c" Enabled="False" />
                                                            <asp:RadioButton ID="rabLevel" runat="server" Text="<%$ Resources:BaseInfo,ConLease_rabLevel %>"
                                                                TextAlign="Left" CssClass="labelStyle" GroupName="c" Enabled="False" /></td>
                                                    </tr>
                                                    <tr height="130">
                                                        <td class="tdBackColor" colspan="3" style=" text-align: center; height: 160px;"
                                                            valign="middle">
                                                            <asp:GridView ID="GVKeepMin" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#E1E0B2" BorderWidth="1px" OnRowDataBound="GVKeepMin_RowDataBound" Width="180px" AllowPaging="True" Height="100%" OnPageIndexChanging="GrdBrandOperateType_PageIndexChanging" PageSize="6">
                                                                <Columns>
                                                                    <asp:BoundField DataField="FormulaID" Visible="False" />
                                                                    <asp:BoundField DataField="SalesTo" HeaderText="<%$ Resources:BaseInfo,ConLease_labSellCount %>" >
                                                                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="MinSum" HeaderText="<%$ Resources:BaseInfo,ConLease_LalKeepMin %>" >
                                                                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                    </asp:BoundField>
                                                                </Columns>
                                                                <HeaderStyle BackColor="#E1E0B2" Height="10px" Font-Bold="False" Font-Size="50px"  />
                                                                <PagerStyle BackColor="White" ForeColor="#000066"   HorizontalAlign="Right" />
                                                                <PagerTemplate>                                                       

                                                                <asp:LinkButton ID="LinkButtonPreviousPage" runat="server" CommandArgument="Prev" CommandName="Page" 
                                                                 Visible="<%# ((GridView)Container.NamingContainer).PageIndex != 0 %>">上一页</asp:LinkButton> 

                                                                <asp:LinkButton ID="LinkButtonNextPage" runat="server" CommandArgument="Next" CommandName="Page" 
                                                                 Visible="<%# ((GridView)Container.NamingContainer).PageIndex != ((GridView)Container.NamingContainer).PageCount - 1 %>">下一页</asp:LinkButton> 

                                                                <asp:textbox id="txtNewPageIndex" runat="server" width="20px" text='<%# ((GridView)Container.Parent.Parent).PageIndex + 1 %>' />/
                                                                <asp:Label ID="LabelPageCount" runat="server" 
                                                                 Text="<%# ((GridView)Container.NamingContainer).PageCount %>"></asp:Label> 
                                                                <asp:linkbutton id="btnGo" runat="server" causesvalidation="False" commandargument="-1" commandname="Page" text="GO" /> 
                                                                  </PagerTemplate> 
                                                            </asp:GridView></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 9px;" colspan="5" class="tdBackColor">
                                                            <input id="HiddenKeepMin1" style="width: 2px; height: 4px" type="hidden" runat="server" />
                                                            <input id="HiddenKeepMin2" style="width: 4px; height: 5px" type="hidden" runat="server" />
                                                            <asp:Button ID="btnKeepMin" runat="server" Height="4px" Text="Button" Width="19px" Visible="False" OnClick="btnKeepMin_Click" /></td>
                                                    </tr>
                                                    <tr class="colLine">
                                                        <td colspan="5" align="center">
                                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 80%">
                                                                <tr class="bodyLine">
                                                                    <td style="width: 175px; height: 1px; background-color: #738495;">
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 175px; height: 1px; background-color: #FFFFFF;">
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" align="right" >       &nbsp; &nbsp;&nbsp; &nbsp; &nbsp;&nbsp;
                                                        </td>
                                                      
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        
                                                    <tr height="8"></tr>
                                    </table>
                                </fieldset>
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

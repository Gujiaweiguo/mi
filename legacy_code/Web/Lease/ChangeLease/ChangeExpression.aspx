<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangeExpression.aspx.cs" Inherits="Lease_ChangeLease_ChangeExpression" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
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
	<script type="text/javascript" src="../../JavaScript/Calendar.js" language="javascript"  charset="gb2312"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"> </script>
	<script type="text/javascript">
	    function Load()
	    {
	        loadTitle();
	        document.getElementById("lblTotalNum").style.display="none";
            document.getElementById("lblCurrent").style.display="none";
	        document.getElementById("lblTotalNumT").style.display="none";
            document.getElementById("lblCurrentT").style.display="none";
          	document.getElementById("lblTotalNumType").style.display="none";
            document.getElementById("lblCurrentType").style.display="none";  
	    }
	    
	        function GetRental()
            {
                if((document.getElementById("txtArea").value != "undefined")&&(document.getElementById("txtArea").value != "")&&(document.getElementById("txtUnitHire").value != "undefined")&&(document.getElementById("txtUnitHire").value != ""))
                {
                    document.getElementById("txtFixedRental").value = FormatNumber(document.getElementById("txtArea").value * document.getElementById("txtUnitHire").value,2)
                }
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

        
        function ClickEventDeduct(d)
        {	
			 window.document.all("HiddenDeduct1").value = d; 
             if ((window.document.all("HiddenDeduct1").value != "undefined") && (window.document.all("HiddenDeduct1").value != "") && (window.document.all("HiddenDeduct1").value != "&nbsp;"))
             {
				var objImgBtn1 = document.getElementById('<%= lBtnP.ClientID %>');  
                objImgBtn1.click(); 
             }
			 else
				return;        
        }
        
        function ClickEventKeepMin(d)
        {
		    window.document.all("HiddenKeepMin1").value = d; 
             if ((window.document.all("HiddenKeepMin1").value != "undefined") && (window.document.all("HiddenKeepMin1").value != "") && (window.document.all("HiddenKeepMin1").value != "&nbsp;"))
             {
				var objImgBtn1 = document.getElementById('<%= lBtnM.ClientID %>');  
                objImgBtn1.click(); 
             }
			 else
				return;            
        }
        
        function FormulaValidator(sForm)
        {
             if(!isDigit(document.all.cmbChargeTypeID.value))
            {
                alert('<%= emptyStr %>');
                document.all.cmbChargeTypeID.focus();
                return false;
            }
            
            if(document.getElementById("txtOverDate").value < document.getElementById("txtBeginDate").value)
            {
                parent.document.all.txtWroMessage.value='<%= dateError %>';
                return false;
            }
        
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
        
        function BtnUp( p )
{
	var t = String(p)
	var l = t.substring(3,15); 
	document.getElementById( p ).style.backgroundImage = 'url(../../App_Themes/CSS/BtnImage/btn_' + l + '.gif)';
}
function BtnOver( p )
{
	var t = String(p)
	var l = t.substring(3,15); 
	document.getElementById( p ).style.backgroundImage = 'url(../../App_Themes/CSS/BtnImage/over_' + l + '.gif)';
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
                <td class="tdBackColor" colspan="2" style="text-align:left;width:34%; height: 370px;" valign="top">
        
                <table class="tblLeft" border="0" cellpadding="0" cellspacing="0" style="width: 100%">
    <tr class="topLine">
       <td class="tdTopBackColor" style="width: 293px;" colspan="3">
                    <img alt="" class="imageLeftBack" /><asp:Label ID="Label10" runat="server"
                        Text="<%$ Resources:BaseInfo,ChangeLease_Update %>" Width="194px"></asp:Label></td>
       <td class="tdTopBackColor" valign="top" style="width: 115px">
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
        <td class="leftLable" align="left" style="padding-left:5px; height: 15px;">
            <asp:Label ID="labChargeTypeID" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ConLease_labChargeTypeID %>" Width="57px"></asp:Label></td>   
        <td align="left" style="height: 15px" colspan="3">
            <asp:DropDownList ID="cmbChargeTypeID" runat="server" CssClass="textBoxStyle" Width="200px" OnSelectedIndexChanged="cmbChargeTypeID_SelectedIndexChanged" AutoPostBack="True">
            </asp:DropDownList></td> 
    </tr>
    <tr>
        <td class="leftLable" style="height: 14px">
            <asp:Label ID="labFormulaType" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ConLease_labFormulaType %>" Width="60px"></asp:Label></td> 
        <td align="left" colspan="3" style="height: 14px">
            <asp:DropDownList ID="cmbFormulaType" runat="server" CssClass="textBoxStyle" Width="200px" AutoPostBack="True" OnSelectedIndexChanged="cmbFormulaType_SelectedIndexChanged">
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
    <tr class="gridView">
        <td style="width:100%; text-align: center;" colspan="4" class="tdBackColor" 
            valign="top">
            <asp:GridView ID="GVType" runat="server" AutoGenerateColumns="False" Height="90%"
                Width="90%" OnRowDataBound="GVType_RowDataBound" BackColor="White" 
                BorderColor="#E1E0B2" BorderWidth="1px" PageSize="8" AllowPaging="True" 
                onpageindexchanging="GrdView_PageIndexChanging" BorderStyle="Outset">
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
            <asp:Label
                                                                        ID="lblTotalNumType" runat="server" Height="1px" Width="32px"></asp:Label><asp:Label
                                                                            ID="lblCurrentType" runat="server" ForeColor="Red" Height="1px" Width="1px">1</asp:Label></td> 
    </tr>
    <tr height="20px">
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
    <tr>
        <td style="text-align:center; height: 15px;" colspan="4">
            <asp:Button ID="BtnSave" runat="server" CssClass="buttonSave" OnClick="IBtnSave_Click"
                Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" Enabled="False" 
                onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" 
                onmouseup="BtnUp(this.id);"/>
            <asp:Button ID="BtnAdd" runat="server" CssClass="buttonAdd" 
                OnClick="IBtnAdd_Click" 
                Text="<%$ Resources:BaseInfo,PotCustomer_butAdd %>" 
                onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" 
                onmouseup="BtnUp(this.id);"/>
            <asp:Button ID="BtnBlankOut" runat="server" CssClass="buttonClear" 
                OnClick="IBtnDel_Click" Text="<%$ Resources:BaseInfo,Btn_Del %>" 
                onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" 
                onmouseup="BtnUp(this.id);"/>
            <asp:Button ID="IBtnModify" runat="server" CssClass="buttonEdit" 
                OnClick="IBtnModify_Click" Text="<%$ Resources:BaseInfo,Btn_Edit %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                Visible="False" />
        </td> 
    </tr>
</table>
                    <input id="Hidden1" runat="server" type="hidden" style="width: 16px; height: 14px;" />
                    &nbsp;
                    <asp:TextBox ID="TextBox1" runat="server" AutoPostBack="True" Visible="False" Width="12px" Height="1px"></asp:TextBox>
                    <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click" Height="1px" Width="1px"></asp:LinkButton></td>

                <td style="text-align: center; width:40%; height: 370px;" valign="top">
        <table border="0" cellpadding="0" cellspacing="0" class="tdBackColor" style="width: 90%;">
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
            <tr>
                <td colspan="2" style="text-align: center" >
                                <fieldset style="border-right: #c0c0c0 1px solid; border-top: #c0c0c0 1px solid;
                                    border-left: #c0c0c0 1px solid; border-bottom: #c0c0c0 1px solid; width: 94%; height: 110px; padding-left:5px">
                                    <legend style="text-align: left">
                                        <asp:Label ID="labFastnessHire" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ConLease_labFastnessHire %>"></asp:Label>
                                    </legend>
                                    <table class="tblFast" border="0" cellpadding="0" cellspacing="0" width="100%" style="height: 95px">
                                        <tr>
                                            <td></td>
                                            <td style="width: 107px;" colspan=2>
                                                <asp:RadioButton ID="rabMonthHire" runat="server" Text="<%$ Resources:BaseInfo,ConLease_rabMonthHire %>"
                                                    TextAlign="Left" CssClass="labelStyle" GroupName="b" Width="65px" />
                                                </td>
                                            <td width="240" style="padding-left:25px" colspan=3 align="left">
                                                <asp:RadioButton ID="rabDayHire" runat="server" Text="<%$ Resources:BaseInfo,ConLease_rabDayHire %>"
                                                    TextAlign="Left" CssClass="labelStyle" GroupName="b" /></td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td align="right">
                                                <asp:Label ID="lblRentArea" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblRentArea %>"
                                                    Width="60px"></asp:Label></td>
                                            <td class="tdBackColor">
                                                <asp:TextBox ID="txtArea" runat="server" CssClass="ipt160px" Width="50px" ReadOnly="True" BackColor="#F5F5F4" Height="18px"></asp:TextBox></td>
                                            <td align="right">
                                                <asp:Label ID="labUnitHire" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ConLease_labUnitHire %>"
                                                    Width="60px"></asp:Label></td>
                                            <td class="tdBackColor">
                                                <asp:TextBox ID="txtUnitHire" runat="server" CssClass="ipt160px" Width="50px" style="ime-mode:disabled;" Height="18px">0</asp:TextBox></td>
                                            <td>
                                                <asp:Label ID="labFixedRental" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ConLease_labFixedRental %>"
                                                    Width="45px"></asp:Label></td>
                                            <td class="tdBackColor" style="width:100px" align="left">
                                                <asp:TextBox ID="txtFixedRental" runat="server" CssClass="ipt160px" Width="65px" style="ime-mode:disabled;" Height="18px">0</asp:TextBox></td>
                                        </tr>
                                    </table>
                                </fieldset>
                </td>
            </tr>
            <tr>
                <td colspan="2" valign="top" align="center">
                    <table border="0" cellpadding="0" cellspacing="0" class="tdBackColor" style="width: 100%">
                        <tr>
                            <td valign="top" style="padding-left:10px; padding-right:10px; width: 100%; text-align: left;">
                                <fieldset style="border-right: #c0c0c0 1px solid; border-top: #c0c0c0 1px solid;
                                    border-left: #c0c0c0 1px solid; border-bottom: #c0c0c0 1px solid; width: 100%; height:270px;">
                                    <legend style="text-align: left">
                                    <asp:Label ID="Label33" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ConLease_labDeductAndKeep %>"></asp:Label>
                                    </legend>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td valign="top" style="width: 50%">
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%" style=" padding-left:5px;">
                                                    <tr height="30">
                                                        <td class="tdBackColor" colspan="4" align="left">
                                                            <asp:RadioButton ID="rabFastness" runat="server" Text="<%$ Resources:BaseInfo,ConLease_rabFastness %>"
                                                                TextAlign="Left" CssClass="labelStyle" GroupName="a" />
                                                            <asp:RadioButton ID="rabMonopole" runat="server" Text="<%$ Resources:BaseInfo,ConLease_rabMonopole %>"
                                                                TextAlign="Left" CssClass="labelStyle" GroupName="a" />
                                                            <asp:RadioButton ID="rabMultilevel" runat="server" Text="<%$ Resources:BaseInfo,ConLease_rabMultilevel %>"
                                                                TextAlign="Left" CssClass="labelStyle" GroupName="a" /></td>
                                                    </tr>                                                    
                                                    <tr height="125px" style="overflow-x:hidden;overflow-y:scroll">
                                                        <td class="tdBackColor" colspan="4" style="text-align: center; height: 125px;"
                                                            valign="top">
                                                            <asp:GridView ID="GVDeductMoney" runat="server" AutoGenerateColumns="False" 
                                                                BackColor="White" BorderColor="#E1E0B2" BorderWidth="1px" 
                                                                OnRowDataBound="GVDeductMoney_RowDataBound" Width="177px" 
                                                                OnPageIndexChanging="GrdView_PageIndexChanging" PageSize="5" 
                                                                AllowPaging="True">
                                                                <Columns>
                                                                    <asp:BoundField DataField="ConFormulaPModID">
                                                                        <ItemStyle CssClass="hidden" />
                                                                        <HeaderStyle CssClass="hidden" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="FormulaID" >
                                                                        <ItemStyle CssClass="hidden" Font-Size="12px" />
                                                                        <HeaderStyle CssClass="hidden" />
                                                                        <FooterStyle CssClass="hidden" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="SalesTo" HeaderText="<%$ Resources:BaseInfo,ConLease_labSellCount %>" >
                                                                        <HeaderStyle CssClass="gridviewtitle" BorderColor="#E1E0B2" />
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Pcent" HeaderText="<%$ Resources:BaseInfo,ConLease_labDistill %>" >
                                                                        <HeaderStyle CssClass="gridviewtitle" BorderColor="#E1E0B2" />
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
                                                            </asp:GridView>
                                                            &nbsp;
                                                            &nbsp;<asp:Label
                                                                        ID="lblTotalNum" runat="server" Height="1px" Width="32px"></asp:Label>
                                                            <asp:Label ID="lblCurrent" runat="server" ForeColor="Red" Height="1px" Width="1px">1</asp:Label></td>
                                                    </tr>
                                                    <tr >
                                                        <td align="left" style=" padding-left:5px;width:10px; padding-right:3px;">
                                                            <asp:Label ID="labDegree" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ConLease_labDegree %>"
                                                                Width="30px"></asp:Label></td>
                                                        <td style="width: 57px">
                                                            <asp:TextBox ID="txtFore" runat="server" CssClass="ipt160px" Width="53px" style="ime-mode:disabled;" Height="18px"></asp:TextBox></td>
                                                        <td style="width: 45px">
                                                            <asp:TextBox ID="txtForePer" runat="server" CssClass="ipt160px" Width="45px" style="ime-mode:disabled;" Height="18px"></asp:TextBox></td>
                                                        <td align="left" style="width: 38px">
                                                            <asp:Label ID="Label20" runat="server" CssClass="labelStyle" Text="%" Width="8px"></asp:Label></td>
                                                    </tr>
                                                    <tr >
                                                        <td style="height: 9px;" colspan="4">
                                                            <input id="HiddenDeduct1" runat="server" style="width: 3px; height: 4px" type="hidden" />&nbsp;&nbsp;
                                                            <asp:LinkButton ID="lBtnP" runat="server" Height="2px" OnClick="lBtnP_Click" Width="31px"></asp:LinkButton></td>
                                                    </tr>
                                                    <tr class="colLine">
                                                        <td colspan="4" align="center">
                                                            <table border="0" cellpadding="0" cellspacing="0" width="175">
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
                                                    <tr height="50px">
                                                        <td align="center" valign="middle" width="50%" colspan=4>
                                                        <asp:Button ID="bt1Add" runat="server" CssClass="buttonAdd" 
                                                                OnClick="BtnDeductAdd_Click" Text="<%$ Resources:BaseInfo,Dept_TitleAdd %>" 
                                                                onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" 
                                                                onmouseup="BtnUp(this.id);"/>
                                                        <asp:Button ID="bt1BlankOut" runat="server" CssClass="buttonClear" 
                                                                OnClick="BtnDeductDel_Click" Text="<%$ Resources:BaseInfo,Btn_Del %>" 
                                                                onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" 
                                                                onmouseup="BtnUp(this.id);"/>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <!--*********************-->
                                            <td valign="top" style="width: 50%">
                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 95%">
                                                    <tr height="30">
                                                        <td class="tdBackColor" colspan="3" style="height: 30px">
                                                            <asp:RadioButton ID="rabFastness2" runat="server" Text="<%$ Resources:BaseInfo,AdContract_rbtnFastness %>"
                                                                TextAlign="Left" CssClass="labelStyle" GroupName="c" />
                                                            <asp:RadioButton ID="rabLevel" runat="server" Text="<%$ Resources:BaseInfo,ConLease_rabLevel %>"
                                                                TextAlign="Left" CssClass="labelStyle" GroupName="c" /></td>
                                                    </tr>                                                    
                                                    <tr height="125px" style="overflow-x:hidden;overflow-y:scroll">
                                                        <td class="tdBackColor" colspan="3" style=" text-align: center"
                                                            valign="top">
                                                            <asp:GridView ID="GVKeepMin" runat="server" AutoGenerateColumns="False" 
                                                                BackColor="White" BorderColor="#E1E0B2" BorderWidth="1px" 
                                                                OnRowDataBound="GVKeepMin_RowDataBound" Width="173px" 
                                                                OnPageIndexChanging="GrdView_PageIndexChanging" PageSize="5" 
                                                                AllowPaging="True">
                                                                <Columns>
                                                                    <asp:BoundField DataField="ConFormulaMModID">
                                                                        <ItemStyle CssClass="hidden" />
                                                                        <HeaderStyle CssClass="hidden" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="FormulaID" >
                                                                        <ItemStyle CssClass="hidden" />
                                                                        <HeaderStyle CssClass="hidden" />
                                                                        <FooterStyle CssClass="hidden" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="SalesTo" HeaderText="<%$ Resources:BaseInfo,ConLease_labSellCount %>" >
                                                                        <HeaderStyle CssClass="gridviewtitle" BorderColor="#E1E0B2" />
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="MinSum" HeaderText="<%$ Resources:BaseInfo,ConLease_LalKeepMin %>" >
                                                                        <HeaderStyle CssClass="gridviewtitle" BorderColor="#E1E0B2" />
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
                                                            </asp:GridView>
                                                            <asp:Label
                                                                        ID="lblTotalNumT" runat="server" Height="1px" Width="32px"></asp:Label><asp:Label
                                                                            ID="lblCurrentT" runat="server" ForeColor="Red" Height="1px" Width="1px">1</asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td >
                                                            <asp:Label ID="Label21" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ConLease_labDegree %>"
                                                                Width="35px"></asp:Label></td>
                                                        <td >
                                                            <asp:TextBox ID="txtForeKeepMin" runat="server" CssClass="ipt160px" Width="67px" style="ime-mode:disabled;" Height="18px"></asp:TextBox></td>
                                                        <td style="width: 86px">
                                                            <asp:TextBox ID="txtForeKeep" runat="server" CssClass="ipt160px" Width="57px" style="ime-mode:disabled;" Height="18px"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 9px;" colspan="3" class="tdBackColor">
                                                            <input id="HiddenKeepMin1" style="width: 2px; height: 4px" type="hidden" runat="server" />&nbsp;&nbsp;
                                                            <asp:LinkButton ID="lBtnM" runat="server" Height="1px" OnClick="lBtnM_Click" Width="25px"></asp:LinkButton></td>
                                                    </tr>
                                                    <tr class="colLine">
                                                        <td colspan="3" align="center">
                                                            <table border="0" cellpadding="0" cellspacing="0" width="175">
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
                                                        <td style="height: 50px;" valign="middle" colspan=3 align="center" >       
                                                            <asp:Button ID="bt2Add" runat="server" CssClass="buttonAdd" 
                                                                OnClick="BtnKeepAdd_Click" Text="<%$ Resources:BaseInfo,Dept_TitleAdd %>" 
                                                                onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" 
                                                                onmouseup="BtnUp(this.id);"/>
                                                            <asp:Button ID="bt2BlankOut" runat="server" CssClass="buttonClear" 
                                                                OnClick="BtnKeepDel_Click" Text="<%$ Resources:BaseInfo,Btn_Del %>" 
                                                                onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" 
                                                                onmouseup="BtnUp(this.id);"/>
                                                        </td>
                                                      
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        
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
    &nbsp;&nbsp;
</body>
</html>
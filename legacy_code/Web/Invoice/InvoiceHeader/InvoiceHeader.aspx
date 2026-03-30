<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InvoiceHeader.aspx.cs" Inherits="Lease_InvoiceHeader_InvoiceHeader" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Menu_TenancyExpenseSettlement")%></title>
        <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/longCss/webtab.css" rel="stylesheet" type="text/css"/>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    
    <script type="text/javascript"  src="../../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript" src="../../JavaScript/Calendar.js" charset="gb2312"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"> </script>
	<script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
	<script type="text/javascript">
	function txtSurAmtSum()
	{
	    document.getElementById("txtSurAmt").value = document.getElementById("txtInvPayExRate").value * document.getElementById("txtInvPaidAmtSum").value;
	    document.getElementById("txtInvPaidAmt").value = document.getElementById("HidnAmt").value - document.getElementById("txtInvPaidAmtSum").value;
	}
    function Load()
    {
        addTabTool("<%=strFresh %>,Invoice/InvoiceHeader/InvoiceHeader.aspx");
        loadTitle();
    }
     //输入验证
    function InputValidator(sForm)
    {
         if(!isDigit(document.all.cmbInvPayType.value))
        {
            alert('<%= invPayType %>');
            document.all.cmbInvPayType.focus();
            return false;
        }
        
         if(isEmpty(document.all.txtInvPaidAmtSum.value))
        {
            alert('<%= invPaidAmtSum %>');
            document.all.txtInvPaidAmtSum.focus();
            return false;
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
    </script>
</head>
<body onload="Load()" style="margin-top:0; margin-left:0">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div id="showLeaseBargain">
                                <table border="0" cellpadding="0" cellspacing="0" style="height: 100%">
                                    <tr>
                                        <td align="left" class="tdTopRightBackColor" style="width: 577px; height: 22px; text-align: left">
                                            <img class="imageLeftBack" src="" style="width: 7px; height: 22px" />
                                            <asp:Label ID="Label31" runat="server" Text="<%$ Resources:BaseInfo,Invoice_InvBalan %>" Width="330px"></asp:Label></td>
                                        <td align="left" class="tdTopRightBackColor" style="width: 562px; height: 22px">
                                        </td>
                                        <td class="tdTopRightBackColor" style="width: 7px; height: 22px" valign="top">
                                            <img align="right" class="imageRightBack" src="" style="width: 7px; height: 22px" /></td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" colspan="3" style="width: 655px; height: 339px" valign="top">
                                            <table border="0" cellpadding="0" cellspacing="0" style="height: 45px;">
                                                <tr class="headLine">
                                                    <td colspan="4" style="height: 1px; background-color: white" width="100%">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdBackColor" colspan="4" style="width: 635px; height: 15px">
                                                    <table style="width: 652px">
                                                    <tr>
                                                    <td style="width: 72px; height: 17px; text-align: right">
                                                        <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustCode %>"></asp:Label></td>
                                                    <td style="width: 91px; height: 17px">
                                                        <asp:TextBox ID="txtCustCode" runat="server" CssClass="ipt160px" Width="104px" MaxLength="16"></asp:TextBox></td>
                                                    <td style="height: 17px; width: 55px;">
                                                        <asp:Label ID="Label7" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdBoard_lblContractID %>"
                                                            Width="50px"></asp:Label></td>
                                                    <td style="width: 61px; text-align: right;">
                                                        <asp:TextBox ID="txtContractCode" runat="server" CssClass="ipt160px" Width="126px" MaxLength="32"></asp:TextBox></td>
                                                        <td style="width: 55px">
                                                        <asp:Label ID="Label12" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,InvoiceHeader_lblInvID %>" Width="55px"></asp:Label></td>
                                                    <td style="height: 35px; width: 178px;">
                                                        <asp:TextBox ID="txtInvCode" runat="server" CssClass="ipt160px" Width="103px" MaxLength="8"></asp:TextBox>
                                                        </td>
                                                            <td>
                                                        <asp:Button ID="btnQuery" runat="server" CssClass="buttonQuery" OnClick="btnQuery_Click"
                                                            Text="<%$ Resources:BaseInfo,User_lblQuery %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/></td>
                                                    </tr>
                                                    </table>
                                                    
                                                        </td>
                                                </tr>
                                                <tr>
                                                <td style="height: 5px; text-align: center" colspan="2">
                                                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 99%">
                                                                        <tr class="bodyLine">
                                                                            <td style="height: 1px; background-color: #738495">
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="bodyLine">
                                                                            <td style="height: 1px; background-color: #ffffff">
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                </tr>
                                                <tr>
                                                    <!--  *********left
                    -->
                                                    <td valign="top" style="width: 21%;">
                                                        <table border="0" cellpadding="0" cellspacing="0" class="tblBase" style="width: 97%; height: 275px">
                                                            <tr style="height: 5px">
                                                                <td style="height: 35px; text-align: center; width: 105px;">
                                                                    <asp:Label ID="Label1" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdBoard_lblContractID %>"
                                                                        Width="50px"></asp:Label></td>
                                                                <td style="width: 164px; height: 5px">
                                                                    <asp:TextBox ID="txtContractID" runat="server" CssClass="Enabledipt160px" ReadOnly="True" Width="111px"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="height: 35px; text-align: center; width: 105px;">
                                                                    <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,InvoiceHeader_lblInvPayType %>"></asp:Label></td>
                                                                <td style="height: 35px; width: 164px;">
                                                                    <asp:DropDownList ID="cmbInvPayType" runat="server" Width="115px" AutoPostBack="True" OnSelectedIndexChanged="cmbInvPayType_SelectedIndexChanged">
                                                                    </asp:DropDownList></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="height: 35px; text-align: center; width: 105px;">
                                                                    <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,InvoiceHeader_lblInvPaidAmtSum %>"></asp:Label></td>
                                                                <td style="height: 35px; width: 164px;">
                                                                    <asp:TextBox ID="txtInvPaidAmtSum" runat="server" CssClass="ipt160px" MaxLength="19" Width="110px"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="height: 35px; text-align: center; width: 105px;">
                                                                    <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,InvoiceHeader_lblInvPayCurID %>"></asp:Label></td>
                                                                <td style="height: 35px; width: 164px;">
                                                                    <asp:DropDownList ID="cmbCurrencyType" runat="server" Width="115px" AutoPostBack="True" OnSelectedIndexChanged="cmbCurrencyType_SelectedIndexChanged">
                                                                </asp:DropDownList></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="height: 35px; text-align: center; width: 105px;">
                                                                    <asp:Label ID="Label8" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,InvoiceHeader_lblInvPayExRate %>"></asp:Label></td>
                                                                <td style="height: 35px; width: 164px;">
                                                                    <asp:TextBox ID="txtInvPayExRate" runat="server" CssClass="Enabledipt160px" onclick="calendarExt(GetNorentDays)" ReadOnly="True" Width="110px"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="text-align: center; width: 105px; height: 35px;">
                                                                    <asp:Label ID="Label9" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,InvoiceHeader_lblSurAmt %>"></asp:Label></td>
                                                                <td style="height: 35px; width: 164px;">
                                                                    <asp:TextBox ID="txtSurAmt" runat="server" CssClass="Enabledipt160px" Width="111px" ReadOnly="True"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="text-align: center; width: 105px; height: 35px;">
                                                                    <asp:Label ID="Label52" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,InvoiceHeader_lblBalance %>"
                                                                        Width="58px"></asp:Label></td>
                                                                <td style="height: 35px; width: 164px;">
                                                                    <asp:TextBox ID="txtInvPaidAmt" runat="server" CssClass="Enabledipt160px" ReadOnly="True" Width="111px"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="text-align: center; width: 105px; vertical-align: middle; height: 35px;">
                                                                    <asp:Label ID="Label11" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,InvoiceHeader_lblInvPayDate %>"></asp:Label></td>
                                                                <td style="width: 164px; height: 35px;">
                                                                    <asp:TextBox ID="txtInvPayDate" runat="server" CssClass="ipt160px" Width="110px" onclick="calendar()"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="text-align: center; width: 105px; height: 35px;">
                                                                    <asp:Label ID="Label59" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labNote %>" Width="54px"></asp:Label></td>
                                                                <td style="height: 35px; width: 164px;">
                                                                    <asp:TextBox ID="txtnote" runat="server" CssClass="ipt160px" MaxLength="64" Width="110px"></asp:TextBox></td>
                                                            </tr>
                                                        </table>
                                                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp;
                                                        &nbsp;<br />
                                                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; 
                                                                <asp:Button ID="btnSave" runat="server" CssClass="buttonSave"
                                                                        OnClick="btnAdd_Click" Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>&nbsp;
                                                                    <asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel" OnClick="btnCancel_Click"
                                                                        Text="<%$ Resources:BaseInfo,User_btnCancel %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                                                        <br />
                                                        <br />
                                                        <asp:HiddenField ID="HidnAmt" runat="server" />
                                                    </td>
                                                    <!--  *********right
                    -->
                                                    <td style="vertical-align: top;" width="50%">
                                                        <table border="0" cellpadding="0" cellspacing="0" class="tblBase" style="width: 99%; height: 31px" id="">
                                                            <tr style="height: 5px">
                                                                <td style="width: 28px; height: 5px;">
                                                                    <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustName %>" Width="51px"></asp:Label></td>
                                                                <td class="baseInput" style="height: 5px; width: 200px;">
                                                                    <asp:TextBox ID="txtCustName" runat="server" CssClass="Enabledipt160px" ReadOnly="True" Width="386px"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseInput" colspan="2" rowspan="3" style="vertical-align: top; text-align: center; height: 135px;">
                                                                    <asp:GridView ID="GrdVewInvoiceHeader" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                                        BorderStyle="Inset" BorderWidth="1px" Height="120px" Width="437px" PageSize="5" OnRowDataBound="GrdVewInvoiceHeader_RowDataBound" OnSelectedIndexChanged="GrdVewInvoiceHeader_SelectedIndexChanged" ShowFooter="True" AllowPaging="True" OnPageIndexChanging="GrdVewInvoiceHeader_OnPageIndexChanging">
                                                <RowStyle ForeColor="Black" Height="10px" Font-Overline="False" Font-Size="10pt" />
                                                <SelectedRowStyle BackColor="#FFFFCD" Font-Bold="False" ForeColor="Black" />
                                                <PagerStyle BackColor="White" ForeColor="#000066"   HorizontalAlign="Right" />
                                                <HeaderStyle BackColor="#E1E0B2" Height="10px" Font-Bold="False"  /><FooterStyle BackColor="Red" ForeColor="#000066"/>
                                                                        <Columns>
                                                                            <asp:BoundField DataField="InvID" HeaderText="<%$ Resources:BaseInfo,InvoiceHeader_lblInvID %>">
                                                                                <ItemStyle BorderColor="#E1E0B2" Width="50px" />
                                                                                <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                                <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                            </asp:BoundField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,InvoiceHeader_lblInvDate %>" FooterText="<%$ Resources:BaseInfo,Account_lblTotalMoney %>">
                                                                                <EditItemTemplate>
                                                                                    <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("InvDate") %>'></asp:TextBox>
                                                                                </EditItemTemplate>
                                                                                <ItemStyle BorderColor="#E1E0B2" />
                                                                                <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="Label4" runat="server" Text='<%# Bind("InvDate") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,InvoiceHeader_lblPayment %>">
                                                                                <EditItemTemplate>
                                                                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("InvActPayAmt") %>'></asp:TextBox>
                                                                                </EditItemTemplate>
                                                                                <ItemStyle BorderColor="#E1E0B2" />
                                                                                <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("InvActPayAmt") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,InvoiceHeader_InvPaidAmt %>">
                                                                                <EditItemTemplate>
                                                                                    <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("InvPaidAmt") %>'></asp:TextBox>
                                                                                </EditItemTemplate>
                                                                                <ItemStyle BorderColor="#E1E0B2" />
                                                                                <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("InvPaidAmt") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,InvoiceHeader_lblBalance %>">
                                                                                <EditItemTemplate>
                                                                                    <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("ThisPaid") %>'></asp:TextBox>
                                                                                </EditItemTemplate>
                                                                                <ItemStyle BorderColor="#E1E0B2" />
                                                                                <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="Label3" runat="server" Text='<%# Bind("ThisPaid") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                            </asp:TemplateField>
                                                                            <asp:CommandField ShowSelectButton="True" HeaderText="<%$ Resources:BaseInfo,PotShop_Selected %>">
                                                                                <ItemStyle BorderColor="#E1E0B2" Width="50px" />
                                                                                <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                                <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                            </asp:CommandField>
                                                                            <asp:BoundField DataField="InvExRate">
                                                                                <ItemStyle CssClass="hidden" />
                                                                                <HeaderStyle CssClass="hidden" />
                                                                                <FooterStyle CssClass="hidden" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="InvCurTypeID">
                                                                                <ItemStyle CssClass="hidden" />
                                                                                <HeaderStyle CssClass="hidden" />
                                                                                <FooterStyle CssClass="hidden" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="CustName">
                                                                                <ItemStyle CssClass="hidden" />
                                                                                <HeaderStyle CssClass="hidden" />
                                                                                <FooterStyle CssClass="hidden" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="ContractCode">
                                                                                <ItemStyle CssClass="hidden" />
                                                                                <HeaderStyle CssClass="hidden" />
                                                                                <FooterStyle CssClass="hidden" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="CustID">
                                                                                <ItemStyle CssClass="hidden" />
                                                                                <HeaderStyle CssClass="hidden" />
                                                                                <FooterStyle CssClass="hidden" />
                                                                            </asp:BoundField>
                                                                             <asp:BoundField DataField="ContractID">
                                                                                <ItemStyle CssClass="hidden" />
                                                                                <HeaderStyle CssClass="hidden" />
                                                                                <FooterStyle CssClass="hidden" />
                                                                            </asp:BoundField>
                                                                               
                                                                        </Columns>
                                                                        <PagerTemplate>                                                   
<asp:LinkButton ID="LinkButtonFirstPage" runat="server" CommandArgument="First" CommandName="Page" 
 Visible="<%# ((GridView)Container.NamingContainer).PageIndex != 0 %>" Font-Size="Small">首页</asp:LinkButton> 

<asp:LinkButton ID="LinkButtonPreviousPage" runat="server" CommandArgument="Prev" CommandName="Page" 
 Visible="<%# ((GridView)Container.NamingContainer).PageIndex != 0 %>" Font-Size="Small">上一页</asp:LinkButton> 

<asp:LinkButton ID="LinkButtonNextPage" runat="server" CommandArgument="Next" CommandName="Page" 
 Visible="<%# ((GridView)Container.NamingContainer).PageIndex != ((GridView)Container.NamingContainer).PageCount - 1 %>" Font-Size="Small">下一页</asp:LinkButton> 

<asp:LinkButton ID="LinkButtonLastPage" runat="server" CommandArgument="Last" CommandName="Page" 
 Visible="<%# ((GridView)Container.NamingContainer).PageIndex != ((GridView)Container.NamingContainer).PageCount - 1 %>" Font-Size="Small">尾页</asp:LinkButton> 
<asp:textbox id="txtNewPageIndex" runat="server" width="20px" text='<%# ((GridView)Container.Parent.Parent).PageIndex + 1 %>' />/
<asp:Label ID="LabelPageCount" runat="server" 
 Text="<%# ((GridView)Container.NamingContainer).PageCount %>"></asp:Label> 
<asp:linkbutton id="btnGo" runat="server" causesvalidation="False" commandargument="-1" commandname="Page" text="GO" Font-Size="Small"/> 
  </PagerTemplate>         
<PagerSettings Mode="NextPreviousFirstLast"  />

                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                            <tr>

                                                            </tr>
                                                            <tr>
                                                            <td style="width: 1px">
                                                            </td>
                                                            </tr>

                                                            <tr style="height: 8px">
                                                                <td colspan="2" style="height: 9px" align="right">
                                                                    &nbsp; &nbsp; &nbsp;&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" style="height: 5px" valign="top">
                                                                </td>
                                                            </tr>
                                                            <tr style="height: 20px">
                                                                <td colspan="2" style="height: 20px; vertical-align: top; text-align: center;"><asp:GridView ID="GrdVewInvoiceDetail" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                                        BorderStyle="Inset" BorderWidth="1px" Height="120px" Width="438px" PageSize="5" OnRowDataBound="GrdVewInvoiceDetail_RowDataBound" ShowFooter="True" AllowPaging="True" OnPageIndexChanging="GrdVewInvoiceDetail_OnPageIndexChanging">
                                                                    <FooterStyle BackColor="Red" ForeColor="#000066"/>
                                                                    <Columns>
                                                                        <asp:BoundField DataField="InvDetailID">
                                                                            <ItemStyle CssClass="hidden" />
                                                                            <HeaderStyle CssClass="hidden" />
                                                                            <FooterStyle CssClass="hidden" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="InvID" HeaderText="<%$ Resources:BaseInfo,InvoiceHeader_lblInvID %>">
                                                                            <ItemStyle BorderColor="#E1E0B2" />
                                                                            <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                            <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        </asp:BoundField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,ChargeType_lblChargeTypeName %>" FooterText="<%$ Resources:BaseInfo,Account_lblTotalMoney %>">
                                                                            <EditItemTemplate>
                                                                                <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("ChargeTypeName") %>'></asp:TextBox>
                                                                            </EditItemTemplate>
                                                                            <ItemStyle BorderColor="#E1E0B2" />
                                                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="Label4" runat="server" Text='<%# Bind("ChargeTypeName") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,InvoiceHeader_lblPayment %>">
                                                                            <EditItemTemplate>
                                                                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("InvActPayAmt") %>'></asp:TextBox>
                                                                            </EditItemTemplate>
                                                                            <ItemStyle BorderColor="#E1E0B2" />
                                                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("InvActPayAmt") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,InvoiceHeader_InvPaidAmt %>">
                                                                            <EditItemTemplate>
                                                                                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("InvPaidAmt") %>'></asp:TextBox>
                                                                            </EditItemTemplate>
                                                                            <ItemStyle BorderColor="#E1E0B2" />
                                                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("InvPaidAmt") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,InvoiceHeader_lblBalance %>">
                                                                            <EditItemTemplate>
                                                                                <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("ThisPaid") %>'></asp:TextBox>
                                                                            </EditItemTemplate>
                                                                            <ItemStyle BorderColor="#E1E0B2" />
                                                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle"/>
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("ThisPaid") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        </asp:TemplateField>
                                                                         <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,InvoiceHeader_lblShouldPayment %>">
                                                                                <ItemTemplate>
                                                                                    &nbsp;<asp:TextBox ID="txtPayment" runat="server" CssClass="ipt35px" Text='<%# DataBinder.Eval(Container, "DataItem.ThisPaid")%>' Font-Size="9pt" Width="75px" onkeydown="textleave()"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                
                                                                                <ItemStyle BorderColor="#E1E0B2" />
                                                                                <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                             <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                            </asp:TemplateField>
                                                                             <asp:BoundField DataField="ChargeTypeID">
                                                                            <ItemStyle CssClass="hidden" />
                                                                            <HeaderStyle CssClass="hidden" />
                                                                            <FooterStyle CssClass="hidden" />
                                                                        </asp:BoundField>
                                                                    </Columns>
                                                                    <RowStyle ForeColor="Black" Height="10px" Font-Overline="False" Font-Size="10pt" />
                                                                    <SelectedRowStyle BackColor="#FFFFCD" Font-Bold="False" ForeColor="Black" />
                                                                    <PagerStyle BackColor="White" ForeColor="#000066"   HorizontalAlign="Right" />
                                                                    <HeaderStyle BackColor="#E1E0B2" Height="10px" Font-Bold="False"  />
                                                                    <PagerTemplate>                                                   
<asp:LinkButton ID="LinkButtonFirstPage" runat="server" CommandArgument="First" CommandName="Page" 
 Visible="<%# ((GridView)Container.NamingContainer).PageIndex != 0 %>" Font-Size="Small">首页</asp:LinkButton> 

<asp:LinkButton ID="LinkButtonPreviousPage" runat="server" CommandArgument="Prev" CommandName="Page" 
 Visible="<%# ((GridView)Container.NamingContainer).PageIndex != 0 %>" Font-Size="Small">上一页</asp:LinkButton> 

<asp:LinkButton ID="LinkButtonNextPage" runat="server" CommandArgument="Next" CommandName="Page" 
 Visible="<%# ((GridView)Container.NamingContainer).PageIndex != ((GridView)Container.NamingContainer).PageCount - 1 %>" Font-Size="Small">下一页</asp:LinkButton> 

<asp:LinkButton ID="LinkButtonLastPage" runat="server" CommandArgument="Last" CommandName="Page" 
 Visible="<%# ((GridView)Container.NamingContainer).PageIndex != ((GridView)Container.NamingContainer).PageCount - 1 %>" Font-Size="Small">尾页</asp:LinkButton> 
<asp:textbox id="txtNewPageIndex" runat="server" width="20px" text='<%# ((GridView)Container.Parent.Parent).PageIndex + 1 %>' />/
<asp:Label ID="LabelPageCount" runat="server" 
 Text="<%# ((GridView)Container.NamingContainer).PageCount %>"></asp:Label> 
<asp:linkbutton id="btnGo" runat="server" causesvalidation="False" commandargument="-1" commandname="Page" text="GO" Font-Size="Small"/> 
  </PagerTemplate>         
<PagerSettings Mode="NextPreviousFirstLast"  />

                                                                </asp:GridView><asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                                        BorderStyle="Inset" BorderWidth="1px" Height="120px" Width="437px" PageSize="5" OnRowDataBound="GrdVewInvoiceDetail_RowDataBound">
                                                                    <FooterStyle BackColor="Red" ForeColor="#000066"/>
                                                                    <Columns>
                                                                        <asp:BoundField DataField="InvDetailID">
                                                                            <ItemStyle CssClass="hidden" />
                                                                            <HeaderStyle CssClass="hidden" />
                                                                            <FooterStyle CssClass="hidden" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="InvID" HeaderText="<%$ Resources:BaseInfo,InvoiceHeader_lblInvID %>">
                                                                            <ItemStyle BorderColor="#E1E0B2" />
                                                                            <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="ChargeTypeName" HeaderText="<%$ Resources:BaseInfo,ChargeType_lblChargeTypeName %>">
                                                                            <ItemStyle BorderColor="#E1E0B2" />
                                                                            <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="InvPayAmt" HeaderText="<%$ Resources:BaseInfo,InvoiceHeader_lblPayment %>">
                                                                            <ItemStyle BorderColor="#E1E0B2" />
                                                                            <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="InvPaidAmt" HeaderText="<%$ Resources:BaseInfo,InvoiceHeader_InvPaidAmt %>">
                                                                            <ItemStyle BorderColor="#E1E0B2" />
                                                                            <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="ThisPaid" HeaderText="<%$ Resources:BaseInfo,InvoiceHeader_lblBalance %>">
                                                                            <ItemStyle BorderColor="#E1E0B2" />
                                                                            <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        </asp:BoundField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,InvoiceHeader_lblShouldPayment %>">
                                                                            <ItemTemplate>
                                                                                &nbsp;<asp:TextBox ID="txtPayment" runat="server" CssClass="ipt35px"></asp:TextBox>
                                                                            </ItemTemplate>
                                                                            <ItemStyle BorderColor="#E1E0B2" />
                                                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <RowStyle ForeColor="Black" Height="10px" Font-Overline="False" Font-Size="10pt" />
                                                                    <SelectedRowStyle BackColor="#FFFFCD" Font-Bold="False" ForeColor="Black" />
                                                                    <PagerStyle BackColor="White" ForeColor="#000066"   HorizontalAlign="Left" />
                                                                    <HeaderStyle BackColor="#E1E0B2" Height="10px" Font-Bold="False"  />
                                                                </asp:GridView>
                                                                </td>
                                                            </tr>
                                                            <tr style="height: 5px">
                                                                <td colspan="2" style="height: 15px" align="right">
                                                                    <br />
                                                                    &nbsp; &nbsp;&nbsp; &nbsp;<br />
                                                                    &nbsp;&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
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


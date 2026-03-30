<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InvAdj.aspx.cs" Inherits="Invoice_InvAdj_InvAdj" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "InvCancel_lblInv")%></title>
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link rel="StyleSheet" href="../../App_Themes/nlstree/nlstree-basic.css" type="text/css" />
    <link rel="StyleSheet" href="../../App_Themes/nlstree/nlsctxmenu.css" type="text/css" />
    
    <script type="text/javascript" src="../../App_Themes/nlstree/nlstree.js"></script>
    <script type="text/javascript" src="../../App_Themes/nlstree/nlsctxmenu.js"></script>
	<script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
    
    <script src="../../App_Themes/DateTime/popcalendar.js" type="text/javascript"></script>
	<script type="text/javascript"  src="../../JavaScript/setday.js"></script>
	<script type="text/javascript" src="../../JavaScript/Calendar.js" language="javascript" charset="gb2312"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"></script>
	
   <script type="text/javascript" language="javascript">
    
    function Load()
    {
        var str= document.getElementById("InvCancel_lblInv").value + ",Invoice/InvAdj/InvAdj.aspx";
        addTabTool(str);
        loadTitle();
    }
    function NumberTest(tCurQty)
    {
        try
		{
            if ( !(((window.event.keyCode >= 48) && (window.event.keyCode <= 57)) 
                || (window.event.keyCode == 13) &&(window.event.keyCode == 46)
                || (window.event.keyCode == 45)))
            {
                window.event.keyCode = 0 ;
            }
        }
	    catch(e){} 
    }
    function CheckallCust(MyControl)
	{
		for (i=0;i<form1.elements.length;i++)
		{
			if (form1.elements[i].type=="checkbox")
			{
				form1.elements[i].checked=MyControl.checked;
			}
		}
	}
	function Load()
    {
        var str= document.getElementById("InvCancel_lblInv").value + ",Invoice/InvAdj/InvAdj.aspx";
        addTabTool(str);
        loadTitle();
    }
    function ReturnDefault()
    {
        window.parent.mainFrame.location.href="../../Disktop.aspx";
    }
    function ShowMessage()
    {
        var wFlwID = document.getElementById("HidenWrkID").value;
        var vID = document.getElementById("HidenVouchID").value;
    	strreturnval=window.showModalDialog('../../Lease/NodeMessage.aspx?wrkFlwID='+encodeURI(wFlwID)+'&voucherID='+encodeURI(vID),'window','dialogWidth=600px;dialogHeight=320px'); 
    }
    //计算费用金额
	function SumTotalRmb(tThisPaid,tAdjAmt,tAdjBackAmt)
	{
	    try
		{
	        var ThisPaid = document.all[tThisPaid].value;
	        if(ThisPaid==""){ThisPaid="0";}
	        var AdjAmt =  document.all[tAdjAmt].value;
	        if(AdjAmt==""){AdjAmt="0";}
	        document.all[tAdjBackAmt].value= parseFloat(ThisPaid)+parseFloat(AdjAmt);
	     }
	     catch(e){}   
	}
   </script>
   <style type="text/css">
        .Error
        {
	         COLOR: red;
        }
    </style>
</head>
<body onload='Load();' topmargin=0 leftmargin=0>
     <form id="form1" runat="server">
    <asp:ScriptManager id="ScriptManager1" runat="server"></asp:ScriptManager>
    <div align="center" style="width:100%; ">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
            <asp:HiddenField id="depttxt" runat="server"></asp:HiddenField>
            <asp:HiddenField id="deptid" runat="server" ></asp:HiddenField>
                <asp:HiddenField ID="selectdeptid" runat="server" />
        <table border="0" cellpadding="0" cellspacing="0" style="height: 450px; width:100%; vertical-align:top" >
            <tr>                
                 <td style="width:35%; vertical-align: top;">
                    <table border="0" cellpadding="0" cellspacing="0" style=" height: 450px;vertical-align: top; width:70%">
                        
                        <tr>
                          <td style="vertical-align:top; height: 22px; width: 100%; background-color: #e1e0b2;" >          
                                 <table border="0" cellpadding="0" cellspacing="0" style="height: 22px; width:100%;   ">              
                                    <tr>
                                        <td class="tdTopRightBackColor"    valign="top" style="width: 8px; height:22px;  text-align:left" >
                                            <img alt="" class="imageLeftBack" style=" text-align:left; height: 22px;"  />
                                            </td>
                                            <td class="tdTopRightBackColor" style="height: 22px; text-align: left;" colspan="2">
                                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:BaseInfo,Customer_lblCustInfo %>"></asp:Label></td>
                                      
                                        <td class="tdTopRightBackColor"   valign="top" style="width: 20px; height: 22px;">
                                            <img class="imageRightBack" style="width: 7px; height: 22px" />
                                            </td>
                                    </tr>                       
                                </table>               
                        </td>
                        </tr>
                        <tr>  
                             <td colspan="2" style="height: 300px; background-color: #e1e0b2; vertical-align:top; text-align:center;" align="right">
                                <table>   
                                    <tr style="height:30px;">
                                        <td valign="middle"> 
                                            <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustCode %>" Width="50px"></asp:Label></td>
                                        <td  valign="middle" align="center">
                                            <asp:TextBox ID="txtCustCode" runat="server" CssClass="ipt160px" Style="ime-mode: disabled"
                                                Width="80px"></asp:TextBox></td>
                                        <td valign="middle" align="right">
                                            <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,InvoiceHeader_lblInvID %>" Width="60px"></asp:Label></td>
                                            <td valign="middle">
                                                <asp:TextBox ID="txtInvCode" runat="server" CssClass="ipt160px" Style="ime-mode: disabled"
                                                    Width="80px"></asp:TextBox></td>
                                        <td valign="top">
                                        </td>
                                    </tr>
                                    <tr style="height: 30px">
                                        <td valign="middle">
                                            <asp:Label ID="Label1" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdBoard_lblContractID %>"
                                                Width="50px"></asp:Label></td>
                                        <td align="center" style="width: 80px; height: 12px" valign="middle">
                                            <asp:TextBox ID="txtContractCode" runat="server" CssClass="ipt160px" Style="ime-mode: disabled"
                                                Width="80px"></asp:TextBox></td>
                                        <td align="left"  valign="middle">
                                            <asp:Label ID="Label52" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,InvAdj_KeepAccountsMth %>"
                                                Width="50px"></asp:Label></td>
                                        <td style="width: 10%" valign="top">
                                            <asp:TextBox ID="txtInvPeriod" runat="server" CssClass="ipt160px" Style="ime-mode: disabled"
                                                Width="80px"></asp:TextBox></td>
                                        <td  valign="top">
                                            <asp:Button ID="btnQuery" runat="server" CssClass="buttonQuery"
                                                TabIndex="1" Text="<%$ Resources:BaseInfo,User_lblQuery %>" OnClick="btnQuery_Click"  /></td>
                                    </tr>
                                </table>
                                <asp:GridView ID="gvCust" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#E1E0B2" Height="246px" AllowPaging="True" OnPageIndexChanging="gvCust_PageIndexChanging" Width="95%" OnSelectedIndexChanged="gvCust_SelectedIndexChanged" PageSize="14" OnRowDataBound="gvCust_RowDataBound">
                                    <PagerSettings Mode="NextPreviousFirstLast"  />
                                    <FooterStyle BackColor="Red" ForeColor="#000066"/>
                                    <Columns>
                                        <asp:BoundField DataField="InvID">
                                            <ItemStyle CssClass="hidden" />
                                            <HeaderStyle CssClass="hidden" />
                                            <FooterStyle CssClass="hidden" />
                                        </asp:BoundField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                &nbsp;<asp:TextBox ID="txtInvID" runat="server" CssClass="ipt35px" Text='<%# DataBinder.Eval(Container, "DataItem.InvID")%>' Font-Size="9pt" Width="80px"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="hidden" />
                                            <HeaderStyle CssClass="hidden" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="InvCode" HeaderText="<%$ Resources:BaseInfo,InvoiceHeader_lblInvID %>">
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ContractCode" HeaderText="<%$ Resources:BaseInfo,AdBoard_lblContractID %>">
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="InvType" HeaderText="<%$ Resources:BaseInfo,InvAdj_KeepAccountsMth %>">
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CustShortName" HeaderText="<%$ Resources:BaseInfo,PotCustomer_lblCustName %>">
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" HorizontalAlign="Left" />
                                        </asp:BoundField>
                                         <asp:BoundField DataField="InvPayAmt" HeaderText="<%$ Resources:BaseInfo,InvoiceHeader_lblInvPayAmtSettlement %>">
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:CommandField HeaderText="<%$ Resources:BaseInfo,PotShop_Selected %>" ShowSelectButton="True">
                                            <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                            <ItemStyle BorderColor="#E1E0B2" />
                                        </asp:CommandField>
                                        <asp:BoundField DataField="InvExRate">
                                            <ItemStyle CssClass="hidden" />
                                            <HeaderStyle CssClass="hidden" />
                                            <FooterStyle CssClass="hidden" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CustName" HeaderText="<%$ Resources:BaseInfo,PotCustomer_lblCustName %>">
                                            <ItemStyle CssClass="hidden" />
                                            <HeaderStyle CssClass="hidden" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CustCode">
                                            <ItemStyle CssClass="hidden" />
                                            <HeaderStyle CssClass="hidden" />
                                            <FooterStyle CssClass="hidden" />
                                        </asp:BoundField>
                                    </Columns>
                                    <PagerTemplate>
                                        <asp:LinkButton ID="LinkButtonFirstPage" runat="server" CommandArgument="First" CommandName="Page"
                                            Font-Size="Small" Visible="<%# ((GridView)Container.NamingContainer).PageIndex != 0 %>">首页</asp:LinkButton>
                                        <asp:LinkButton ID="LinkButtonPreviousPage" runat="server" CommandArgument="Prev"
                                            CommandName="Page" Font-Size="Small" Visible="<%# ((GridView)Container.NamingContainer).PageIndex != 0 %>">上一页</asp:LinkButton>
                                        <asp:LinkButton ID="LinkButtonNextPage" runat="server" CommandArgument="Next" CommandName="Page"
                                            Font-Size="Small" Visible="<%# ((GridView)Container.NamingContainer).PageIndex != ((GridView)Container.NamingContainer).PageCount - 1 %>">下一页</asp:LinkButton>
                                        <asp:LinkButton ID="LinkButtonLastPage" runat="server" CommandArgument="Last" CommandName="Page"
                                            Font-Size="Small" Visible="<%# ((GridView)Container.NamingContainer).PageIndex != ((GridView)Container.NamingContainer).PageCount - 1 %>">尾页</asp:LinkButton>
                                        <asp:textbox id="txtNewPageIndex" runat="server" width="20px" text='<%# ((GridView)Container.Parent.Parent).PageIndex + 1 %>' />/
                                        <asp:Label ID="LabelPageCount" runat="server" Text="<%# ((GridView)Container.NamingContainer).PageCount %>"></asp:Label>
                                        <asp:linkbutton id="btnGo" runat="server" causesvalidation="False" commandargument="-1" commandname="Page" text="GO" Font-Size="Small"/>
                                    </PagerTemplate>
                                    <RowStyle ForeColor="Black" Height="20px" Font-Overline="False" Font-Size="10pt" HorizontalAlign="Left" />
                                    <SelectedRowStyle BackColor="#FFFFCD" Font-Bold="False" ForeColor="Black" />
                                    <PagerStyle BackColor="White" ForeColor="#000066"   HorizontalAlign="Right"/>
                                    <HeaderStyle BackColor="#E1E0B2" Height="10px" Font-Bold="False"  />
                                </asp:GridView>
                                 </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2" style="vertical-align: top; height: 18px; background-color: #e1e0b2;
                                text-align: center">
                            </td>
                        </tr>
                    </table>
                </td>           
                <td style="width: 1%;">
                </td>
                <td style="height:450px; width:35%; vertical-align:top;" valign="top">
                    <table border="0" cellpadding="0" cellspacing="0" style="height: 450px; width:100%; vertical-align:top; " >
                        
                        <tr style="width:100%">
                            <td colspan="1" style="height: 22px; background-color: #e1e0b2" valign="top">
                            </td>
                            <td colspan="10" style="height: 22px; background-color: #e1e0b2" valign="top">
                                 <table border="0" cellpadding="0" cellspacing="0" style="height: 22px; width:100%; ">
                                    <tr>
                                        <td class="tdTopRightBackColor"    valign="top" style="width: 8px; height:22px;  text-align:left" >
                                            <img alt="" class="imageLeftBack" style=" text-align:left; height: 22px;"  />
                                            </td>
                                            <td class="tdTopRightBackColor" style="height: 22px; text-align: left;">
                                                <asp:Label ID="Label31" runat="server" Text="<%$ Resources:BaseInfo,InvCancel_lblInv %>"></asp:Label></td>
                                      
                                        <td class="tdTopRightBackColor"   valign="top" style="width: 20px; height: 22px;">
                                            <img class="imageRightBack" style="width: 7px; height: 22px" />
                                            </td>
                                    </tr>
                                </table>          
                            </td>
                        </tr>
                        <tr style="width:100%">
                            <td align="center" colspan="1" style="vertical-align: top; background-color: #e1e0b2;text-align: center" valign="top"></td>
                            <td colspan="10" style="vertical-align: top; background-color: #e1e0b2; text-align: center" valign="top" align="center">
                                <asp:GridView ID="gvInvoice" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#E1E0B2" Height="246px" AllowPaging="True" PageSize="15" Width="95%" OnRowDataBound="gvInvoice_RowDataBound" OnPageIndexChanging="gvInvoice_PageIndexChanging"><PagerSettings Mode="NextPreviousFirstLast"  />
                                    <FooterStyle BackColor="Red" ForeColor="#000066"/>
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <INPUT id="AllSelect" onclick="CheckallCust(this);" type="checkbox" runat="server" >
                                            </HeaderTemplate>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="Checkbox" runat="server" Value='<%# DataBinder.Eval(Container, "DataItem.InvAdjDetID")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="InvAdjDetID">
                                            <ItemStyle CssClass="hidden" />
                                            <HeaderStyle CssClass="hidden" />
                                        </asp:BoundField>
                                         <asp:TemplateField>
                                            <ItemTemplate>
                                                &nbsp;<asp:TextBox ID="txtInvDetailID" runat="server" CssClass="" Text='<%# DataBinder.Eval(Container, "DataItem.InvDetailID")%>' Font-Size="9pt" Width="100px" ReadOnly="true"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="hidden" />
                                            <HeaderStyle CssClass="hidden" />
                                         <FooterStyle CssClass="hidden" />
                                        </asp:TemplateField>
                                        
                                        <asp:BoundField DataField="InvAdjID">
                                            <ItemStyle CssClass="hidden" />
                                            <HeaderStyle CssClass="hidden" />
                                        </asp:BoundField>
                                        
                                        <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,PotCustomer_lblCustName %>">
                                            <ItemTemplate>
                                                &nbsp;<asp:Label ID="txtCustShortName" runat="server" CssClass="" Text='<%# DataBinder.Eval(Container, "DataItem.CustShortName")%>' Font-Size="9pt" Width="100px" ReadOnly="true"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                         <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,ConLease_labChargeTypeID %>">
                                            <ItemTemplate>
                                                &nbsp;<asp:Label ID="txtChargeTypeName" runat="server" CssClass="" Text='<%# DataBinder.Eval(Container, "DataItem.ChargeTypeName")%>' Font-Size="9pt" Width="60px" ReadOnly="true"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                         <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,Account_lblChargeMoney %>">
                                            <ItemTemplate>
                                                &nbsp;<asp:TextBox ID="txtThisPaid" runat="server" CssClass="ipt35px" Text='<%# DataBinder.Eval(Container, "DataItem.ThisPaid")%>'  Font-Size="9pt" Width="60px" ReadOnly="true"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                         <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                        </asp:TemplateField>
                                        
                                        
                                        
                                        <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,InvoiceHeader_InvPaidAmt %>">
                                            <ItemTemplate>
                                                &nbsp;<asp:Label ID="txtInvPaidAmt" runat="server" CssClass="ipt35px" Text='<%# DataBinder.Eval(Container, "DataItem.InvPaidAmt")%>' Font-Size="9pt" Width="60px" ReadOnly="true"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                         <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,InvAdj_AdjAmt %>">
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtAdjAmt" runat="server" CssClass="ipt35px" Font-Size="9pt" Text='<%# DataBinder.Eval(Container, "DataItem.AdjAmt")%>'
                                                    Width="50px"></asp:TextBox>
                                            </ItemTemplate>
                                            <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,InvAdj_AdjBackAmt %>">
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtAdjBackAmt" runat="server" CssClass="ipt35px" Font-Size="9pt" Text='<%# DataBinder.Eval(Container, "DataItem.AdjBackAmt")%>'
                                                    Width="60px"></asp:TextBox>
                                            </ItemTemplate>
                                            <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,InvAdj_AdjReason %>">
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                &nbsp;<asp:TextBox ID="txtAdjReason" runat="server" CssClass="ipt35px" Font-Size="9pt"　Text='<%# DataBinder.Eval(Container, "DataItem.AdjReason")%>' Width="150px"></asp:TextBox>
                                            </ItemTemplate>
                                            <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                        </asp:TemplateField>
                                         <asp:TemplateField>
                                            <ItemTemplate>
                                                &nbsp;<asp:TextBox ID="txtInvAdjDetID" runat="server" CssClass="" Text='<%# DataBinder.Eval(Container, "DataItem.InvAdjDetID")%>' Font-Size="9pt" Width="100px" ReadOnly="true"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="hidden" />
                                            <HeaderStyle CssClass="hidden" />
                                         <FooterStyle CssClass="hidden" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ErrorSign">
                                            <ItemStyle CssClass="hidden" />
                                            <HeaderStyle CssClass="hidden" />
                                        </asp:BoundField>
                                    </Columns>
                                    <PagerTemplate>
                                        <asp:LinkButton ID="LinkButtonFirstPage" runat="server" CommandArgument="First" CommandName="Page"
                                            Font-Size="Small" Visible="<%# ((GridView)Container.NamingContainer).PageIndex != 0 %>">首页</asp:LinkButton>
                                        <asp:LinkButton ID="LinkButtonPreviousPage" runat="server" CommandArgument="Prev"
                                            CommandName="Page" Font-Size="Small" Visible="<%# ((GridView)Container.NamingContainer).PageIndex != 0 %>">上一页</asp:LinkButton>
                                        <asp:LinkButton ID="LinkButtonNextPage" runat="server" CommandArgument="Next" CommandName="Page"
                                            Font-Size="Small" Visible="<%# ((GridView)Container.NamingContainer).PageIndex != ((GridView)Container.NamingContainer).PageCount - 1 %>">下一页</asp:LinkButton>
                                        <asp:LinkButton ID="LinkButtonLastPage" runat="server" CommandArgument="Last" CommandName="Page"
                                            Font-Size="Small" Visible="<%# ((GridView)Container.NamingContainer).PageIndex != ((GridView)Container.NamingContainer).PageCount - 1 %>">尾页</asp:LinkButton>
                                        <asp:textbox id="txtNewPageIndex" runat="server" width="20px" text='<%# ((GridView)Container.Parent.Parent).PageIndex + 1 %>' />/
                                        <asp:Label ID="LabelPageCount" runat="server" Text="<%# ((GridView)Container.NamingContainer).PageCount %>"></asp:Label>
                                        <asp:linkbutton id="btnGo" runat="server" causesvalidation="False" commandargument="-1" commandname="Page" text="GO" Font-Size="Small"/>
                                    </PagerTemplate>
                                    <RowStyle ForeColor="Black" Height="20px" Font-Overline="False" Font-Size="10pt" HorizontalAlign="Left" />
                                    <SelectedRowStyle BackColor="#FFFFCD" Font-Bold="False" ForeColor="Black" />
                                    <PagerStyle BackColor="White" ForeColor="#000066"   HorizontalAlign="Right"/>
                                    <HeaderStyle BackColor="#E1E0B2" Height="10px" Font-Bold="False"  />
                                </asp:GridView>
                                </td>
                        </tr>
                        <tr style="width:100%">
                            <td colspan="1" style="vertical-align: middle; height: 10px; background-color: #e1e0b2;
                                text-align: right">
                            </td>
                            <td colspan="9" style="vertical-align: middle; height: 10px; background-color: #e1e0b2;
                                text-align: right; width: 100%;">
                            </td>
                            <td colspan="1" style="vertical-align: middle; height: 10px; background-color: #e1e0b2;
                                text-align: right; width: 510px;">
                            </td>
                        </tr>
                        <tr style="width:100%">
                            <td colspan="1" style="vertical-align: middle; height: 12px; background-color: #e1e0b2;
                                text-align: right">
                            </td>
                            <td colspan="10" style="vertical-align: middle; height: 12px; background-color: #e1e0b2;
                                text-align: right">
                                <asp:Button ID="btnAdd" runat="server" CssClass="buttonSave"  Text="<%$ Resources:BaseInfo,Dept_TitleAdd %>" Enabled="False" OnClick="btnAdd_Click" Visible="False" />
                                <asp:Button ID="btnPutIn" runat="server" CssClass="buttonSave" OnClick="btnSave_Click"
                                    Text="<%$ Resources:BaseInfo,Lease_NewLineBtnPutIn%>" />
                                <asp:Button ID="btnBlankOut" runat="server" CssClass="buttonClear"
                                    Text="<%$ Resources:BaseInfo,ConLease_butDel %>" OnClick="btnBlankOut_Click" Enabled="False" />
                                <asp:Button ID="btnMessage" runat="server" CssClass="buttonMessage" Enabled="False"
                                    Height="32px" Text="<%$ Resources:BaseInfo,WrkFlwEntity_btnMessage %>" Width="70px" OnClick="btnMessage_Click" />
                                &nbsp; &nbsp; &nbsp;
                                &nbsp; &nbsp;&nbsp;
                            </td>
                            <td colspan="1" style="vertical-align: middle; height: 12px; background-color: #e1e0b2;
                                text-align: right">
                            </td>
                        </tr>
                         <tr style="width:100%">
                             <td align="center" colspan="1" style="vertical-align: top; background-color: #e1e0b2">
                             </td>
                            <td style=" background-color: #e1e0b2; vertical-align: top;" align="center" colspan="8">                            &nbsp;&nbsp;&nbsp;</td>
                             <td align="center" colspan="1" style="vertical-align: top; background-color: #e1e0b2">
                             </td>
                             <td align="center" colspan="1" style="vertical-align: top; background-color: #e1e0b2; width: 510px;">
                             </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
         <asp:HiddenField ID="HidenWrkID" runat="server">
            </asp:HiddenField>
             <asp:HiddenField ID="HidenVouchID" runat="server">
            </asp:HiddenField>
            </ContentTemplate>
        </asp:UpdatePanel>
        </div>
        <asp:HiddenField ID="hidSelect" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidSelect %>" />
        <asp:HiddenField ID="hidUpdate" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidUpdate %>" />
        <asp:HiddenField ID="hidAdd" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidAdd %>" />
        <asp:HiddenField ID="hidUpdateLost" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidUpdateLost %>" />
        <asp:HiddenField ID="hidInsert" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidInsert %>" />
        <asp:HiddenField ID="hidMessage" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidMessage %>" />
        <asp:HiddenField ID="HidConfim" runat="server" Value="<%$ Resources:BaseInfo,Prompt_ConfirmOK %>" />
        <asp:HiddenField ID="InvCancel_lblInv" runat="server" Value="<%$ Resources:BaseInfo,InvCancel_lblInv %>"/>
    </form>
</body>
</html>

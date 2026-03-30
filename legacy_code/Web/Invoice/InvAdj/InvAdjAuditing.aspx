<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InvAdjAuditing.aspx.cs" Inherits="Invoice_InvAdj_InvAdjAuditing" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "InvCancel_lblInvAuditing")%></title>
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
	    function Load()
	    {
	    	//document.getElementById("lblTotalNum").style.display="none";
            //document.getElementById("lblCurrent").style.display="none";
	        var str= document.getElementById("InvCancel_lblInvAuditing").value + ",Invoice/InvAdj/InvAdjAuditing.aspx";
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
    </script>
	<style type="text/css">
        .Error
        {
	         COLOR: blue;
        }
    </style>
</head>
<body style="margin:0px" onload='Load()'>
    <form id="form1" runat="server">
    <div style="text-align:center;">
     <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
        <table border="0" cellpadding="0" cellspacing="0" style="height: 450px;  vertical-align:top; " width="100%" class="tdBackColor">
                        <tr>
                            <td colspan="11" style="height: 22px; background-color: #e1e0b2" valign="top">
                                 <table border="0" cellpadding="0" cellspacing="0" style="height: 22px; width:100%; ">
                                    <tr>
                                        <td class="tdTopRightBackColor"    valign="top" style="width: 8px; height:22px;  text-align:left" >
                                            <img alt="" class="imageLeftBack" style=" text-align:left; height: 22px;"  />
                                            </td>
                                            <td class="tdTopRightBackColor" style="height: 22px; text-align: left;">
                                        <asp:Label
                                            ID="labUserDefine" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,InvCancel_lblInvAuditing %>"></asp:Label></td>
                                      
                                        <td class="tdTopRightBackColor"   valign="top" style="width: 20px; height: 22px;">
                                            <img class="imageRightBack" style="width: 7px; height: 22px" />
                                            </td>
                                    </tr>
                                </table>          
                            </td>
                        </tr>
                        <tr>
                            <td colspan="11" style="vertical-align: top; width: 100%; background-color: #e1e0b2;
                                text-align: center" valign="top" align="center">
                                <asp:GridView ID="gvInvoice" runat="server" AutoGenerateColumns="False"
                                    BackColor="White" BorderColor="#E1E0B2" Height="246px" Width="98%" AllowPaging="True" PageSize="15" OnRowDataBound="gvInvoice_RowDataBound" OnPageIndexChanging="gvInvoice_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox id="Checkbox" runat="server" Value='<%# DataBinder.Eval(Container, "DataItem.InvAdjDetID")%>'></asp:CheckBox>
                                            </ItemTemplate>
                                         <ItemStyle BorderColor="#E1E0B2" />
                                         <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                &nbsp;<asp:TextBox ID="txtInvAdjDetID" runat="server" CssClass="ipt35px" Text='<%# DataBinder.Eval(Container, "DataItem.InvAdjDetID")%>' Font-Size="9pt" Width="60px" ReadOnly="true"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="hidden" />
                                            <HeaderStyle CssClass="hidden" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,InvoiceHeader_lblInvID %>">
                                            <ItemTemplate>
                                                &nbsp;<asp:Label ID="txtInvCode" runat="server" CssClass="" Text='<%# DataBinder.Eval(Container, "DataItem.InvCode")%>' Font-Size="9pt" Width="100px" ReadOnly="true"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                         <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,AdBoard_lblContractID %>">
                                            <ItemTemplate>
                                                &nbsp;<asp:Label ID="txtContractCode" runat="server" CssClass="" Text='<%# DataBinder.Eval(Container, "DataItem.ContractCode")%>' Font-Size="9pt" Width="70px" ReadOnly="true"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                         <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,InvAdj_KeepAccountsMth %>">
                                            <ItemTemplate>
                                                <asp:Label ID="txtInvType" runat="server" CssClass="" Text='<%# DataBinder.Eval(Container, "DataItem.InvType")%>' Font-Size="9pt" Width="60px" ReadOnly="true"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                         <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,PotCustomer_lblCustName %>">
                                            <ItemTemplate>
                                                &nbsp;<asp:Label ID="txtCustShortName" runat="server" CssClass="" Text='<%# DataBinder.Eval(Container, "DataItem.CustShortName")%>' Font-Size="9pt" Width="60px" ReadOnly="true"></asp:Label>
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
                                       
                                        <asp:BoundField DataField="InvActPayAmt" HeaderText="<%$ Resources:BaseInfo,InvoiceHeader_lblInvPayAmt %>">
                                            <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                            <ItemStyle BorderColor="#E1E0B2" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="InvPaidAmt" HeaderText="<%$ Resources:BaseInfo,InvoiceHeader_InvPaidAmt %>">
                                            <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                            <ItemStyle BorderColor="#E1E0B2" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="AdjAmt" HeaderText="<%$ Resources:BaseInfo,InvAdj_AdjAmt %>" >
                                            <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                            <ItemStyle BorderColor="#E1E0B2" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="AdjBackAmt" HeaderText="<%$ Resources:BaseInfo,InvAdj_AdjBackAmt%>" >
                                            <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                            <ItemStyle BorderColor="#E1E0B2" />
                                        </asp:BoundField>
                                        
                                        <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,InvAdj_AdjReason %>">
                                            <ItemTemplate>
                                                &nbsp;<asp:Label ID="txtAdjReason" runat="server" CssClass="" Text='<%# DataBinder.Eval(Container, "DataItem.AdjReason")%>' Font-Size="9pt" Width="130px" ReadOnly="true"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                         <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ErrorSign">
                                            <ItemStyle CssClass="hidden" />
                                            <HeaderStyle CssClass="hidden" />
                                        </asp:BoundField>
                                        <asp:TemplateField>
                                         <ItemTemplate>
                                                &nbsp;<asp:TextBox ID="txtErrorSign" runat="server" CssClass="" Text='<%# DataBinder.Eval(Container, "DataItem.ErrorSign")%>' Font-Size="9pt" Width="80px" ReadOnly="true"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="hidden" />
                                            <HeaderStyle CssClass="hidden" />
                                        </asp:TemplateField>
                                    </Columns>
                                     <FooterStyle BackColor="Red" ForeColor="#000066"/>
                                    <RowStyle ForeColor="Black" Height="20px" Font-Overline="False" Font-Size="10pt" HorizontalAlign="Left" />
                                    <SelectedRowStyle BackColor="#FFFFCD" Font-Bold="False" ForeColor="Black" />
                                    <HeaderStyle BackColor="#E1E0B2" Height="10px" Font-Bold="False"  />
                                    <PagerStyle BackColor="White" ForeColor="#000066"   HorizontalAlign="Right"/>
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
                             <td align="center" style="vertical-align: middle; width: 75px; height: 30px; background-color: #e1e0b2" valign="bottom">
                                 <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PublicMes_OverruleMessage %>"
                                     Width="60px"></asp:Label></td>
                             <td align="center"  style="vertical-align: top; width:100%; height: 30px; background-color: #e1e0b2">
                                <asp:TextBox ID="listBoxRemark" runat="server" Width="100%"></asp:TextBox></td>
                             <td style=" background-color: #e1e0b2; vertical-align: top; height: 30px; width: 0px;" align="center" colspan="10">                            </td>
                             <td align="center" colspan="1" style="vertical-align: top; background-color: #e1e0b2; height: 30px;"></td>
                        </tr>
            <tr>
                <td align="center" colspan="1" style="vertical-align: top; height: 13px; background-color: #e1e0b2"> </td>
                <td align="center" colspan="10" style="vertical-align: top; height: 13px; background-color: #e1e0b2; text-align: right;"><asp:Button ID="btnOk" runat="server" CssClass="buttonSave" OnClick="btnPass_Click"
                                    
                                    Text="<%$ Resources:BaseInfo,CustPalaver_butConsent %>" />&nbsp;&nbsp;<asp:Button ID="btnBlankOut" runat="server" CssClass="buttonClear" OnClick="btnBack_Click"
                                    
                                    Text="<%$ Resources:BaseInfo,CustPalaver_butOverrule %>" />&nbsp;<asp:Button ID="btnMessage"
                                        runat="server" CssClass="buttonMessage" Height="32px" OnClick="btnMessage_Click"
                                        Text="<%$ Resources:BaseInfo,WrkFlwEntity_btnMessage %>" Width="70px" />
                    &nbsp;
                </td>
                <td align="center" colspan="1" style="vertical-align: top; height: 13px; background-color: #e1e0b2"> </td>
            </tr>
            <tr>
                <td align="center" colspan="1" style="vertical-align: top; height: 15px; background-color: #e1e0b2">
                </td>
                <td align="center" colspan="10" style="vertical-align: top; height: 15px; background-color: #e1e0b2;
                    text-align: right">
                </td>
                <td align="center" colspan="1" style="vertical-align: top; height: 15px; background-color: #e1e0b2">
                </td>
            </tr>
                    </table>
            </ContentTemplate>
        </asp:UpdatePanel><asp:HiddenField ID="InvCancel_lblInvAuditing" runat="server" Value="<%$ Resources:BaseInfo,InvCancel_lblInvAuditing %>"/>
         <asp:HiddenField ID="HidenWrkID" runat="server">
            </asp:HiddenField>
             <asp:HiddenField ID="HidenVouchID" runat="server">
            </asp:HiddenField>
    </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BudgetDatailAuditing.aspx.cs" Inherits="Lease_Budget_BudgetDatailAuditing" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Menu_BudgetDeailAuditing")%></title>
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
    <style type="text/css">
        <!--
           
            tr{height:15px;}
            td.lable{padding-right:5px;text-align:right;}
            
        -->
    </style>
    <script type="text/javascript" src="../../JavaScript/Calendar.js" language="javascript" charset="gb2312"></script>
    <script type="text/javascript" src="../../JavaScript/Common.js" language="javascript"></script>
    <script type="text/javascript">
        <!--
            
            function InputValidator(sForm)
            {
                 if(isEmpty(document.all.listBoxRemark.value))
                {
                    alert('<%= emptyStr %>');
                    document.all.listBoxRemark.focus();
                    return false;
                }
            }
            
            function Load()
	        {
	            addTabTool("<%=baseInfo %>,Lease/Budget/BudgetDatailAuditing.aspx");
	            loadTitle();
	        }
	        function checkall(MyControl)
	        {
		        for (i=0;i<form1.elements.length;i++)
		        {
			        if (form1.elements[i].type=="checkbox")
			        {
				        form1.elements[i].checked=MyControl.checked;
			        }
		        }
	        }
	        function ReturnDefault()
            {
                window.parent.mainFrame.location.href="../../Disktop1.aspx";
            }
        -->
    </script>
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
                                            ID="labUserDefine" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,Menu_BudgetDeailAuditing %>"></asp:Label></td>
                                      
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
                                &nbsp;<asp:GridView ID="gvCharge" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                    BackColor="White" BorderColor="#E1E0B2" Height="246px" OnPageIndexChanging="gvCharge_PageIndexChanging"
                                    OnRowDataBound="gvCharge_RowDataBound" PageSize="15" Width="100%">
                                    <Columns>

                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                &nbsp;<asp:TextBox ID="txtBudgetID" runat="server" CssClass="ipt35px" Font-Size="9pt"
                                                    Text='<%# DataBinder.Eval(Container, "DataItem.BudgetID")%>' Width="80px"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="hidden" />
                                            <HeaderStyle CssClass="hidden" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                &nbsp;<asp:TextBox ID="txtUnitID" runat="server" CssClass="ipt35px" Font-Size="9pt"
                                                    Text='<%# DataBinder.Eval(Container, "DataItem.UnitID")%>' Width="80px"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="hidden" />
                                            <HeaderStyle CssClass="hidden" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,RentableArea_lblUnitCode %>">
                                            <ItemTemplate>
                                                &nbsp;<asp:TextBox ID="txtUnitCode" runat="server" CssClass="ipt35px" Font-Size="9pt"
                                                    ReadOnly="true" Text='<%# DataBinder.Eval(Container, "DataItem.UnitCode")%>'
                                                    Width="80px"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                            <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,RentableArea_lblFloorArea %>">
                                            <ItemTemplate>
                                                &nbsp;<asp:TextBox ID="txtFloorArea" runat="server" CssClass="ipt35px" Font-Size="9pt"
                                                    ReadOnly="true" Text='<%# DataBinder.Eval(Container, "DataItem.FloorArea")%>'
                                                    Width="80px"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                            <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,Budget_lblUseArea %>">
                                            <ItemTemplate>
                                                &nbsp;<asp:TextBox ID="txtUseArea" runat="server" CssClass="ipt35px" Font-Size="9pt"
                                                    ReadOnly="true" Text='<%# DataBinder.Eval(Container, "DataItem.UseArea")%>' Width="80px"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                            <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,PotShop_lblShopType %>">
                                            <ItemTemplate>
                                                &nbsp;<asp:TextBox ID="txtShopTypeName" runat="server" CssClass="ipt35px" Font-Size="9pt"
                                                    ReadOnly="true" Text='<%# DataBinder.Eval(Container, "DataItem.ShopTypeName")%>'
                                                    Width="80px"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                            <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,RentableArea_lblTradeName %>">
                                            <ItemTemplate>
                                                &nbsp;<asp:TextBox ID="txtTradeName" runat="server" CssClass="ipt35px" Font-Size="9pt"
                                                    ReadOnly="true" Text='<%# DataBinder.Eval(Container, "DataItem.TradeName")%>'
                                                    Width="80px"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                            <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,Budget_UnitTypeName %>">
                                            <ItemTemplate>
                                                &nbsp;<asp:TextBox ID="txtUnitTypeName" runat="server" CssClass="ipt35px" Font-Size="9pt"
                                                    ReadOnly="true" Text='<%# DataBinder.Eval(Container, "DataItem.UnitTypeName")%>'
                                                    Width="80px"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                            <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,Budget_RentType %>">
                                        <ItemTemplate>
                                       <asp:DropDownList ID="ddlRentType" runat="server"  Width="50px" Height="16px" Enabled="false" />
                                         <asp:HiddenField ID="hidRentType" runat="server" Value='<%# Eval("RentType") %>' />
                                       </ItemTemplate>  
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                            <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,Budget_RentAmt %>">
                                            <ItemTemplate>
                                                &nbsp;<asp:TextBox ID="txtRentAmt" runat="server" CssClass="ipt35px" Font-Size="9pt" ReadOnly="true"
                                                    Text='<%# DataBinder.Eval(Container, "DataItem.RentAmt")%>' Width="80px"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                            <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                &nbsp;<asp:TextBox ID="txtShopTypeID" runat="server" CssClass="ipt35px" Font-Size="9pt"
                                                    Text='<%# DataBinder.Eval(Container, "DataItem.ShopTypeID")%>'></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="hidden" />
                                            <HeaderStyle CssClass="hidden" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                &nbsp;<asp:TextBox ID="txtTradeID" runat="server" CssClass="ipt35px" Font-Size="9pt"
                                                    Text='<%# DataBinder.Eval(Container, "DataItem.TradeID")%>'></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="hidden" />
                                            <HeaderStyle CssClass="hidden" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                &nbsp;<asp:TextBox ID="txtUnitTypeID" runat="server" CssClass="ipt35px" Font-Size="9pt"
                                                    Text='<%# DataBinder.Eval(Container, "DataItem.UnitTypeID")%>'></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="hidden" />
                                            <HeaderStyle CssClass="hidden" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="Red" ForeColor="#000066" />
                                    <RowStyle Font-Overline="False" Font-Size="10pt" ForeColor="Black" Height="20px"
                                        HorizontalAlign="Left" />
                                    <SelectedRowStyle BackColor="#FFFFCD" Font-Bold="False" ForeColor="Black" />
                                    <HeaderStyle BackColor="#E1E0B2" Font-Bold="False" Height="10px" HorizontalAlign="Center" />
                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Right" />
                                    <PagerTemplate>
                                        <asp:LinkButton ID="LinkButtonFirstPage" runat="server" CommandArgument="First" CommandName="Page"
                                            Font-Size="Small" Visible="<%# ((GridView)Container.NamingContainer).PageIndex != 0 %>">首页</asp:LinkButton>
                                        <asp:LinkButton ID="LinkButtonPreviousPage" runat="server" CommandArgument="Prev"
                                            CommandName="Page" Font-Size="Small" Visible="<%# ((GridView)Container.NamingContainer).PageIndex != 0 %>">上一页</asp:LinkButton>
                                        <asp:LinkButton ID="LinkButtonNextPage" runat="server" CommandArgument="Next" CommandName="Page"
                                            Font-Size="Small" Visible="<%# ((GridView)Container.NamingContainer).PageIndex != ((GridView)Container.NamingContainer).PageCount - 1 %>">下一页</asp:LinkButton>
                                        <asp:LinkButton ID="LinkButtonLastPage" runat="server" CommandArgument="Last" CommandName="Page"
                                            Font-Size="Small" Visible="<%# ((GridView)Container.NamingContainer).PageIndex != ((GridView)Container.NamingContainer).PageCount - 1 %>">尾页</asp:LinkButton>
                                        <asp:TextBox ID="txtNewPageIndex" runat="server" Text='<%# ((GridView)Container.Parent.Parent).PageIndex + 1 %>'
                                            Width="20px"></asp:TextBox>/
                                        <asp:Label ID="LabelPageCount" runat="server" Text="<%# ((GridView)Container.NamingContainer).PageCount %>"></asp:Label>
                                        <asp:LinkButton ID="btnGo" runat="server" CausesValidation="False" CommandArgument="-1"
                                            CommandName="Page" Font-Size="Small" Text="GO"></asp:LinkButton>
                                    </PagerTemplate>
                                    <PagerSettings Mode="NextPreviousFirstLast" />
                                </asp:GridView>
                                </td>
                        </tr>
            <tr>
                <td align="center" colspan="1" style="vertical-align: top; height: 15px; background-color: #e1e0b2"> </td>
                <td align="center" colspan="10" style="vertical-align: top; height: 15px; background-color: #e1e0b2; text-align: right;"><asp:Button ID="btnOk" runat="server" CssClass="buttonOk" OnClick="btnPass_Click"
                                    onmouseout="BtnUp(this.id);" onmouseover="BtnOver(this.id);" onmouseup="BtnUp(this.id);"
                                    Text="<%$ Resources:BaseInfo,CustPalaver_butConsent %>" />&nbsp;&nbsp;<asp:Button ID="btnBlankOut" runat="server" CssClass="buttonClear" OnClick="btnBack_Click"
                                    onmouseout="BtnUp(this.id);" onmouseover="BtnOver(this.id);" onmouseup="BtnUp(this.id);"
                                    Text="<%$ Resources:BaseInfo,CustPalaver_butOverrule %>" />
                    &nbsp;
                </td>
                <td align="center" colspan="1" style="vertical-align: top; height: 15px; background-color: #e1e0b2"> </td>
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
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>

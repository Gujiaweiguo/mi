<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WaterElectChargePalaver.aspx.cs" Inherits="Lease_ChargeAccount_WaterElectChargePalaver" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Account_lblWaterElectChargePalaver")%></title>
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
	            addTabTool("<%=baseInfo %>,Lease/ChargeAccount/WaterElectChargePalaver.aspx");
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
                                            ID="labUserDefine" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,Account_ShopChargeInput %>"></asp:Label></td>
                                      
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
                                <asp:GridView ID="gvCharge" runat="server" AutoGenerateColumns="False"
                                    BackColor="White" BorderColor="#E1E0B2" Height="246px" Width="98%" 
                                    AllowPaging="True" PageSize="15" 
                                    OnPageIndexChanging="gvCharge_PageIndexChanging" 
                                    onrowdatabound="gvCharge_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox id="Checkbox" runat="server" Value='<%# DataBinder.Eval(Container, "DataItem.ChgDetID")%>'></asp:CheckBox>
                                            </ItemTemplate>
                                         <ItemStyle BorderColor="#E1E0B2" />
                                         <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                &nbsp;<asp:TextBox ID="txtChgDetID" runat="server" CssClass="ipt35px" Text='<%# DataBinder.Eval(Container, "DataItem.ChgDetID")%>' Font-Size="9pt" Width="80px" ReadOnly="true"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="hidden" />
                                            <HeaderStyle CssClass="hidden" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,PotShop_lblPotShopName %>">
                                            <ItemTemplate>
                                                &nbsp;<asp:Label ID="txtShopName" runat="server" CssClass="" Text='<%# DataBinder.Eval(Container, "DataItem.ShopName")%>' Font-Size="9pt" Width="130px" ReadOnly="true"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                         <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,Shop_lblHdwCode %>">
                                            <ItemTemplate>
                                                &nbsp;<asp:Label ID="txtHdwCode" runat="server" CssClass="" Text='<%# DataBinder.Eval(Container, "DataItem.HdwCode")%>' Font-Size="9pt" Width="70px" ReadOnly="true"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                         <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,Account_lblLastRead %>">
                                            <ItemTemplate>
                                                <asp:Label ID="txtLastQty" runat="server" CssClass="" Text='<%# DataBinder.Eval(Container, "DataItem.LastQty")%>' Font-Size="9pt" Width="60px" ReadOnly="true"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                         <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,Account_lblCurQty %>">
                                            <ItemTemplate>
                                                &nbsp;<asp:Label ID="txtCurQty" runat="server" CssClass="" Text='<%# DataBinder.Eval(Container, "DataItem.CurQty")%>' Font-Size="9pt" Width="60px" ReadOnly="true"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                         <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,Account_lblTimes %>">
                                            <ItemTemplate>
                                                &nbsp;<asp:Label ID="txtTimes" runat="server" CssClass="" Text='<%# DataBinder.Eval(Container, "DataItem.Times")%>' Font-Size="9pt" Width="60px" ReadOnly="true"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                         <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,Account_lblFreeQty %>">
                                            <ItemTemplate>
                                                &nbsp;<asp:Label ID="txtFreeQty" runat="server" CssClass="" Text='<%# DataBinder.Eval(Container, "DataItem.FreeQty")%>' Font-Size="9pt" Width="60px" ReadOnly="true"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                         <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,Account_lblQty %>">
                                            <ItemTemplate>
                                                &nbsp;<asp:Label ID="txtCostQty" runat="server" CssClass="" Text='<%# DataBinder.Eval(Container, "DataItem.CostQty")%>' Font-Size="9pt" Width="60px" ReadOnly="true"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                         <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,Account_lblPrice %>">
                                            <ItemTemplate>
                                                &nbsp;<asp:Label ID="txtPrice" runat="server" CssClass="" Text='<%# DataBinder.Eval(Container, "DataItem.Price")%>' Font-Size="9pt" Width="40px" ReadOnly="true"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                         <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,PotShop_lblShopStartDate %>">
                                            <ItemTemplate>
                                                &nbsp;<asp:Label ID="txtStartDate" runat="server" CssClass="" Text='<%# DataBinder.Eval(Container, "DataItem.StartDate")%>' Font-Size="9pt" Width="70px" ReadOnly="true"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                         <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,Rpt_EDate %>">
                                            <ItemTemplate>
                                                &nbsp;<asp:Label ID="txtEndDate" runat="server" CssClass="" Text='<%# DataBinder.Eval(Container, "DataItem.EndDate")%>' Font-Size="9pt" Width="70px" ReadOnly="true"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                         <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,Account_lblChargeMoney %>">
                                            <ItemTemplate>
                                                &nbsp;<asp:Label ID="txtChgAmt" runat="server" CssClass="" Text='<%# DataBinder.Eval(Container, "DataItem.ChgAmt")%>' Font-Size="9pt" Width="60px" ReadOnly="true"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                         <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                &nbsp;<asp:Label ID="txtChargeTypeName" runat="server" CssClass="" Text='<%# DataBinder.Eval(Container, "DataItem.ChargeTypeName")%>' Font-Size="9pt" Width="80px" ReadOnly="true"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle BorderColor="#E1E0B2" CssClass="hidden" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="hidden" />
                                        </asp:TemplateField>
                                         <asp:TemplateField>
                                            <ItemTemplate>
                                                &nbsp;<asp:Label ID="txtHdwID" runat="server" CssClass="" Text='<%# DataBinder.Eval(Container, "DataItem.HdwID")%>' Font-Size="9pt" Width="80px" ReadOnly="true"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="hidden" />
                                            <HeaderStyle CssClass="hidden" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                &nbsp;<asp:Label ID="txtChargeTypeID" runat="server" CssClass="" Text='<%# DataBinder.Eval(Container, "DataItem.ChargeTypeID")%>' Font-Size="9pt" Width="80px" ReadOnly="true"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="hidden" />
                                            <HeaderStyle CssClass="hidden" />
                                        </asp:TemplateField>
                                        
                                        <asp:BoundField DataField="ShopID">
                                            <ItemStyle CssClass="hidden" />
                                            <HeaderStyle CssClass="hidden" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ShopName">
                                            <ItemStyle CssClass="hidden" />
                                            <HeaderStyle CssClass="hidden" />
                                        </asp:BoundField>
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
                             <td align="center" style="vertical-align: top; width: 75px; height: 15px; background-color: #e1e0b2">
                                <asp:Label ID="Label1" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,Ad_lblCheckIdea %>" Width="70px"></asp:Label></td>
                             <td align="center"  style="vertical-align: top; width:100%; height: 15px; background-color: #e1e0b2">
                                <asp:TextBox ID="listBoxRemark" runat="server" Width="100%"></asp:TextBox></td>
                             <td style=" background-color: #e1e0b2; vertical-align: top; height: 30px; width: 0px;" align="center" colspan="10">                            </td>
                             <td align="center" colspan="1" style="vertical-align: top; background-color: #e1e0b2; height: 15px;"></td>
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

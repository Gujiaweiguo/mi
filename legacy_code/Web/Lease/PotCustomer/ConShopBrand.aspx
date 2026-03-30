<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ConShopBrand.aspx.cs" Inherits="Lease_PotCustomer_ConShopBrand" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "ShopBrand_lblConShopBrandDefine")%></title>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
	<script type="text/javascript" src="../../JavaScript/Common.js"></script>
    
    <style type="text/css">
        <!--
        
        table.tblBase tr.rowHeight{ height:28px;}
        
        table.tblBase tr.headLine{ height:1px; }
        table.tblBase tr.bodyLine{ height:1px; }
        
        td.baseLable{ padding-right:10px;text-align:right; width:136px}
        td.baseLine{ padding-right:10px;text-align:right;}
        td.baseInput{ align:left;padding-right:20px }
        -->
      </style>
      <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
       <script type="text/javascript">
      <!--	    
	     function Load()
        {
            addTabTool("<%=strFresh %>,Lease/PotCustomer/ConShopBrand.aspx");
            loadTitle();
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
        function CheckIsNull()
        {
            if(isEmpty(document.all.txtBrandName.value))  
            {
                parent.document.all.txtWroMessage.value=document.getElementById("hidMessage").value;
                document.all.txtBrandName.focus();
                return false;					
            }
        }
        function selectShopBrand() {
            //strreturnval=window.showModalDialog('../Lease/SelectShopBrand.aspx','window','dialogWidth=350px;dialogHeight=380px');
            strreturnval = window.showModalDialog('../Brand/BrandSelect.aspx', 'window', 'dialogWidth=337px;dialogHeight=420px');
            window.document.all("allvalue").value = strreturnval;
            if ((window.document.all("allvalue").value != "undefined") && (window.document.all("allvalue").value != "")) {
                var objImgBtn1 = document.getElementById('<%= LinkButton1.ClientID %>');
                objImgBtn1.click();
            }
            else
                return;
        }
	    -->  
      </script>
</head>
<body style="margin:0px" onload="Load()">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                         <table border="0" cellpadding="0" cellspacing="0" style="height: 27px; width: 100%">
                            <tr>
                                <td class="tdTopRightBackColor"    valign="top" style="width: 8px; height:27px;  text-align:left" >
                                    <img alt="" class="imageLeftBack" style=" text-align:left"  />
                                    </td>
                                <td class="tdTopRightBackColor" style="width: 778px; height: 27px; text-align:left;">
                                    <asp:Label
                                        ID="Label1" runat="server" Text='<%$ Resources:BaseInfo,ShopBrand_lblConShopBrandDefine %>' Height="12pt" Width="218px"></asp:Label></td>
                              
                                <td class="tdTopRightBackColor"   valign="top" style="width: 63px; height: 27px; text-align: right;">
                                    <img class="imageRightBack" style="width: 7px; height: 22px" />
                                    </td>
                            </tr>
                            <tr class="headLine">
                            <td colspan="3"></td>
                            </tr>
                             <tr style="height: 1px">
                                 <td colspan="3" class="tdBackColor">
                                     <table style="width: 100%" >
                                         <tr style="height:10px">
                                             <td>
                                             </td>
                                             <td>
                                             </td>
                                             <td>
                                             </td>
                                             <td>
                                             </td>
                                         </tr>
                                         <tr>
                                             <td class="baseLable">
                                                 <asp:Label ID="Label2" runat="server" Text="<%$ Resources:BaseInfo,ShopBrand_lblBrandName %>" CssClass="labelStyle" Width="92px"></asp:Label></td>
                                             <td>
                                                 <asp:TextBox ID="txtBrandName" runat="server" CssClass="ipt160px" 
                                                     MaxLength="32" ontextchanged="txtBrandName_TextChanged" 
                                                     AutoPostBack="True"></asp:TextBox></td>
                                             <td class="baseLable">
                                                 <asp:Label ID="Label3" runat="server" Text="<%$ Resources:BaseInfo,ShopBrand_lblBrandLevel %>" CssClass="labelStyle" Width="87px"></asp:Label></td>
                                             <td class="baseInput">
                                                 <asp:DropDownList ID="DownListBrandLevel" runat="server" Width="163px">
                                                 </asp:DropDownList></td>
                                         </tr>
                                         <tr>
                                             <td class="baseLable">
                                                 <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ShopBrand_lblBrandRegDesc %>"></asp:Label></td>
                                             <td>
                                                 <asp:TextBox ID="txtBrandRegDesc" runat="server" CssClass="ipt160px" MaxLength="64" ></asp:TextBox></td>
                                             <td class="baseLable">
                                                 &nbsp;<asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ShopBrand_lblBrandProduce %>"></asp:Label></td>
                                             <td class="baseInput">
                                                 <asp:TextBox ID="txtBrandProduce" runat="server" CssClass="ipt160px" MaxLength="64"></asp:TextBox></td>
                                         </tr>
                                         <tr>
                                             <td class="baseLable">
                                                 <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ShopBrand_lblBrandTargetCust %>"></asp:Label></td>
                                             <td colspan="3">
                                                 <asp:TextBox ID="txtBrandTargetCust" runat="server" CssClass="ipt160px" Width="490px" MaxLength="128"></asp:TextBox></td>
                                         </tr>
                                         <tr style="height:10px">
                                             <td class="baseLable">
                                             </td>
                                             <td colspan="3">
                                                 </td>
                                         </tr>
                                         <tr>
                                             <td colspan="4" style="text-align: center; padding-left:10px; padding-right:10px">
                                                 <table border="0" cellpadding="0" cellspacing="0" style="left: 0px; width: 100%;
                                                     position: relative; top: 0px;">
                                                     <tbody>
                                                         <tr>
                                                             <td style="width: 160px; height: 1px; background-color: #738495">
                                                             </td>
                                                         </tr>
                                                         <tr>
                                                             <td style="width: 160px; height: 1px; background-color: #ffffff">
                                                             </td>
                                                         </tr>
                                                     </tbody>
                                                 </table>
                                             </td>
                                         </tr>
                                         <tr>
                                             <td colspan="4" style="text-align: center; padding-left:10px; padding-right:10px">
                                                 <asp:GridView ID="gvShopBrand" runat="server" AutoGenerateColumns="False" BorderStyle="Inset"  CellPadding="3"
                                                     Width="100%" BackColor="White" OnSelectedIndexChanged="gvShopBrand_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="gvShopBrand_OnPageIndexChanging" PageSize="8" OnRowDataBound="gvShopBrand_RowDataBound">
                                                     <Columns>
                                                         <asp:BoundField DataField="BrandID">
                                                             <ItemStyle CssClass="hidden" />
                                                             <HeaderStyle CssClass="hidden" />
                                                             <FooterStyle CssClass="hidden" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="BrandName" HeaderText="<%$ Resources:BaseInfo,ShopBrand_lblBrandName %>" >
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="BrandLevelName" HeaderText="<%$ Resources:BaseInfo,ShopBrand_lblBrandLevel %>" >
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="BrandRegDesc" HeaderText="<%$ Resources:BaseInfo,ShopBrand_lblBrandRegDesc %>" >
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="BrandProduce" HeaderText="<%$ Resources:BaseInfo,ShopBrand_lblBrandProduce %>" >
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="BrandTargetCust" HeaderText="<%$ Resources:BaseInfo,ShopBrand_lblBrandTargetCust %>">
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                         </asp:BoundField>
                                                         <asp:CommandField ShowSelectButton="True" HeaderText="<%$ Resources:BaseInfo,PotShop_Selected %>" >
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                         </asp:CommandField>
                                                     </Columns>
                                                     <FooterStyle BackColor="Red" ForeColor="#000066"/>
                                                    <RowStyle ForeColor="Black" Height="10px" Font-Overline="False" Font-Size="10pt" />
                                                    <SelectedRowStyle BackColor="#FFFFCD" Font-Bold="False" ForeColor="Black" />
                                                    <HeaderStyle BackColor="#E1E0B2" Height="10px" Font-Bold="False"  />
                                                    <PagerStyle BackColor="White" ForeColor="#000066"   HorizontalAlign="Right" />
                                                    <PagerTemplate>                                                   
                                                    <asp:LinkButton ID="LinkButtonFirstPage" runat="server" CommandArgument="First" CommandName="Page" 
                                                     Visible="<%# ((GridView)Container.NamingContainer).PageIndex != 0 %>">首页</asp:LinkButton> 

                                                    <asp:LinkButton ID="LinkButtonPreviousPage" runat="server" CommandArgument="Prev" CommandName="Page" 
                                                     Visible="<%# ((GridView)Container.NamingContainer).PageIndex != 0 %>">上一页</asp:LinkButton> 

                                                    <asp:LinkButton ID="LinkButtonNextPage" runat="server" CommandArgument="Next" CommandName="Page" 
                                                     Visible="<%# ((GridView)Container.NamingContainer).PageIndex != ((GridView)Container.NamingContainer).PageCount - 1 %>">下一页</asp:LinkButton> 

                                                    <asp:LinkButton ID="LinkButtonLastPage" runat="server" CommandArgument="Last" CommandName="Page" 
                                                     Visible="<%# ((GridView)Container.NamingContainer).PageIndex != ((GridView)Container.NamingContainer).PageCount - 1 %>">尾页</asp:LinkButton> 
                                                    <asp:textbox id="txtNewPageIndex" runat="server" width="20px" text='<%# ((GridView)Container.Parent.Parent).PageIndex + 1 %>' />/
                                                    <asp:Label ID="LabelPageCount" runat="server" 
                                                     Text="<%# ((GridView)Container.NamingContainer).PageCount %>"></asp:Label> 
                                                    <asp:linkbutton id="btnGo" runat="server" causesvalidation="False" commandargument="-1" commandname="Page" text="GO" /> 
                                                      </PagerTemplate>         
                                                    <PagerSettings Mode="NextPreviousFirstLast"  />

                                                 </asp:GridView>
                                             </td>
                                         </tr>
                                         <tr class="rowHeight">
                                             <td colspan="4" style="text-align: center">
                                             </td>
                                         </tr>
                                         <tr>
                                             <td colspan="4" rowspan="1" style="text-align: center">
                                              <table border="0" cellpadding="0" cellspacing="0" style="left: -6px; width: 98%; top: -32px; text-align: center;">
                                                     <tbody>
                                                         <tr>
                                                             <td style="width: 160px; height: 1px; background-color: #738495">
                                                             </td>
                                                         </tr>
                                                         <tr>
                                                             <td style="width: 160px; height: 1px; background-color: #ffffff">
                                                             </td>
                                                         </tr>
                                                     </tbody>
                                                 </table>
                                             </td>
                                         </tr>
                                         <tr>
                                             <td colspan="4" style="text-align: left; vertical-align: bottom;">
                                                 <table style="width:100%; height:100%; text-align:right"><tr><td>
                                                     <input ID="Button1" onclick="selectShopBrand()" type="button" value="合并到" /></td><td>
                                                         <asp:Button ID="btnBack" runat="server" CssClass="buttonBack" Enabled="False" OnClick="btnBack_Click"
                                                     Text="<%$ Resources:BaseInfo,Button_back %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);" Visible="False"/>&nbsp;<asp:Button ID="btnNext" runat="server"
                                                         CssClass="buttonNext" Enabled="False" OnClick="btnNext_Click" Text="<%$ Resources:BaseInfo,Button_next %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);" Visible="False"/>
                                                         <asp:Button ID="btnAdd" runat="server" CssClass="buttonAdd" OnClick="btnSave_Click"
                                                     Text="<%$ Resources:BaseInfo,PotCustomer_butAdd %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>&nbsp;<asp:Button ID="btnEdit"
                                                         runat="server" CssClass="buttonEdit" OnClick="btnEdit_Click" Text="<%$ Resources:BaseInfo,User_btnChang %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>&nbsp;
                                                         <asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel" OnClick="btnCel_Click"
                                                             Text="<%$ Resources:BaseInfo,User_btnCancel %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                             </td></tr></table>
                                                             </td>
                                         </tr>
                                         <tr style="height:10px">
                                             <td colspan="4" style="text-align: center; height: 4px;">
                                             </td>
                                         </tr>
                                     </table>
                                 </td>
                             </tr>
                        
                        </table>
                        <input id="allvalue" runat="server" style="width: 25px" type="hidden" />
        <asp:HiddenField ID="hidMessage" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidMessage %>" />
        <asp:LinkButton ID="LinkButton1" runat="server" onclick="LinkButton1_Click"></asp:LinkButton>
            </ContentTemplate>
        </asp:UpdatePanel>
        
    </form>
</body>
</html>

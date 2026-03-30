<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CashInformation.aspx.cs" Inherits="Sell_CashInformation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>    
    <link href="../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/CSS/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../JavaScript/Common.js"></script>
    <link href="../App_Themes/CSS/style.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript" src="../JavaScript/TabTools.js"></script>
    <script type="text/javascript" src="../JavaScript/Calendar.js" charset="gb2312"></script>
    <script type="text/javascript">
     function Load()
    {
        addTabTool("<%=baseInfo %>,<%=url %>");
        loadTitle();
        loadTitle();
        document.getElementById("lblTotalNum").style.display="none";
        document.getElementById("lblCurrent").style.display="none";
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


          function Cal(tb) {
          //  var b = parseFloat(tb.value);
          //  if (!isNaN(b)) {
          //      var p1 = tb.parentElement || tb.parentNode;
         //       var par = p1.parentElement || p1.parentNode;
         //       var a = parseInt(par.childNodes[2].getElementsByTagName("span")[0].innerHTML);//应收列的索引
         //       var tbC = par.childNodes[4].getElementsByTagName("input")[0];//差额列的索引
         //       tbC.value = a - b;
         //   }
            document.getElementById("Button1").click();
                       
        }



    </script>
</head>
<body topmargin=0 leftmargin=0 onload="Load()">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 370px">
                                        <tr>
                                            <td class="tdTopBackColor" style="vertical-align: middle; height: 25px;
                                                text-align: left" valign="top">
                                                <img alt="" class="imageLeftBack" />
                                                <asp:Label ID="labCustomer" runat="server" Text="<%$ Resources:BaseInfo,CashInformation_Add %>" Width="156px" Height="21px"></asp:Label></td>
                                            <td class="tdTopRightBackColor" colspan="2" style=" height: 25px; text-align: right"
                                                valign="top">
                                                <img alt="" class="imageRightBack" /></td>
                                        </tr>
                                                                                            <tr>
                                                        <td colspan="8" style="height: 1px; background-color: white">
                                                        </td>
                                                    </tr>
                                        <tr>
                                            <td class="tdBackColor" colspan="3" style="width: 100%; height: 326px; text-align: center; vertical-align: top;"
                                                valign="top">
                                                
                                                <table style="width: 100%">

                                                    <tr>
                                                        <td class="tdBackColor" colspan="8" style="height: 5px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdBackColor" style="width: 293px; height: 30px; text-align: right">
                                                            &nbsp;<asp:Label ID="lblCustTypeCode" runat="server" CssClass="labelStyle" 
                                                                Width="70px" Text="<%$ Resources:BaseInfo,Lease_lblPayInDate %>"></asp:Label></td>
                                                        <td class="tdBackColor" style="width: 8px; height: 30px">
                                                        </td>
                                                        <td class="tdBackColor" style="width: 232px; height: 30px; text-align: left">
                                                            <asp:TextBox ID="txtBizDate" runat="server" CssClass="ipt160px" MaxLength="32" onclick="calendar()"></asp:TextBox></td>
                                                        <td class="tdBackColor" style="height: 30px">
                                                        </td>
                                                        <td class="tdBackColor" style="width: 111px; height: 30px; text-align: right">
                                                            <asp:Button ID="btnQuery" runat="server" CssClass="buttonQuery" OnClick="btnQuery_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);" 
                                                                Text="<%$ Resources:BaseInfo,User_lblQuery %>" />&nbsp;</td>
                                                        <td class="tdBackColor" style="width: 7px; height: 30px">
                                                        </td>
                                                        <td class="tdBackColor" style="width: 127px; height: 30px; text-align: left">
                                                            </td>
                                                        <td class="tdBackColor" style="width: 100px; height: 30px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdBackColor" style="width: 293px; height: 30px; text-align: right">
                                                            <asp:Label ID="lblBizGrpNote" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Rpt_CasherCode  %>"></asp:Label></td>
                                                        <td class="tdBackColor" style="width: 8px; height: 30px">
                                                        </td>
                                                        <td class="tdBackColor" style="width: 232px; height: 30px; text-align: left">
                                                            <asp:TextBox ID="txtUserID" runat="server" CssClass="ipt160px" MaxLength="32"></asp:TextBox></td>
                                                        <td class="tdBackColor" style="height: 30px">
                                                        </td>
                                                        <td class="tdBackColor" style="width: 111px; height: 30px; text-align: right">
                                                            <asp:Label ID="lblBizGrpStatus" runat="server" CssClass="labelStyle" 
                                                                Width="91px" Text="<%$ Resources:BaseInfo,Rpt_CasherName %>"></asp:Label></td>
                                                        <td class="tdBackColor" style="width: 7px; height: 30px">
                                                        </td>
                                                        <td class="tdBackColor" style="width: 127px; height: 30px; text-align: left">
                                                            <asp:TextBox ID="txtUserName" runat="server" CssClass="ipt160px" MaxLength="32"></asp:TextBox></td>
                                                        <td class="tdBackColor" style="width: 100px; height: 30px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdBackColor" colspan="8" style="height: 3px; text-align: center; padding-left:10px; padding-right:10px">
                                                            <table border="0" cellpadding="0" cellspacing="0" style="width:100%;
                                                                position: relative">
                                                                <tbody>
                                                                    <tr>
                                                                        <td style="width:160px; height: 1px; background-color: #738495">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width:160px; height: 1px; background-color: #ffffff">
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="8" style="height: 192px;padding-left:10px; padding-right:10px" valign="top">
                                                            &nbsp;&nbsp;
                                                                    <asp:GridView id="GrdCashInfo" runat="server" Width="80%"    ShowFooter="True" BorderWidth="1px" BorderStyle="Inset" BackColor="White" AutoGenerateColumns="False">
                                                                    <FooterStyle BackColor="#E1E0B2" ForeColor="#000066" />
                                                                    <Columns>
                                                                        <asp:BoundField DataField="mediaMNo" >
                                                                                <ItemStyle CssClass="hidden" />
                                                                                <HeaderStyle  CssClass="hidden" />
                                                                            <ControlStyle CssClass="hidden" />
                                                                            <FooterStyle CssClass="hidden" />
                                                                        </asp:BoundField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,InvoiceHeader_lblInvPayType %>">
                                                                                <ItemStyle BorderColor="#E1E0B2" />
                                                                                <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="labmediaMDesc" runat="server" Text='<%# Eval("mediaMDesc") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                <asp:Label ID="labmediaMDescSum" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblTotal %>"></asp:Label>
                                                                                </FooterTemplate>
                                                                                <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                            </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:ReportInfo,RptSale_LocalAmt %>">
                                                                                <ItemStyle BorderColor="#E1E0B2" />
                                                                                <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2"  />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="Labamount" runat="server" Text='<%# Eval("amount") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                <asp:Label ID="labamountSum" runat="server"></asp:Label>
                                                                                </FooterTemplate>
                                                                                <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                            </asp:TemplateField>
                                                                         <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,CashInformation_PayAmt %>">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtlocalAmt" runat="server" CssClass="ipt35px" Text='<%# Eval("localAmt")%>' Font-Size="9pt" Width="75px" onkeydown="textleave()" onBlur='Cal(this)'   ></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                <asp:Label ID="lablocalAmtSum" runat="server"></asp:Label>
                                                                                </FooterTemplate>
                                                                                <ItemStyle BorderColor="#E1E0B2" />
                                                                                <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2"  />
                                                                             <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                            </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,CashInformation_balance %>">
                                                                            <ItemStyle BorderColor="#E1E0B2"  />
                                                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                            <ItemTemplate>
                                                                             <asp:TextBox ID="txtbalance" runat="server" CssClass="ipt35px" ReadOnly="true" Font-Size="9pt" Width="75px" ></asp:TextBox>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:Label ID="labbalanceSum" runat="server"></asp:Label>
                                                                                </FooterTemplate>
                                                                            <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2"  />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <RowStyle ForeColor="Black" Height="20px" Font-Overline="False" Font-Size="10pt"  />
                                                                    <SelectedRowStyle BackColor="#FFFFCD" Font-Bold="False" ForeColor="Black"  />
                                                                    <PagerStyle BackColor="White" ForeColor="#000066"   HorizontalAlign="Left"  />
                                                                    <HeaderStyle BackColor="#E1E0B2" Height="10px" Font-Bold="False"   />
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
                                                            <asp:Button ID="Button1" runat="server" Height="1px" Text="Button" Width="1px" OnClick="Button1_Click" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdBackColor" colspan="3" style="height: 2px">
                                                        </td>
                                                        <td class="tdBackColor" style="height: 2px">
                                                        </td>
                                                        <td class="tdBackColor" colspan="4" style="left: 30px; vertical-align: middle; width: 270px;
                                                            height: 2px; text-align: left">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdBackColor" colspan="3" style="height: 30px">
                                                            &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                                                        </td>
                                                        <td class="tdBackColor" style="height: 30px">
                                                        </td>
                                                        <td class="tdBackColor" colspan="4" style="left: 30px; vertical-align: middle; width: 270px;
                                                            height: 30px; text-align: left">
                                                            <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" OnClick="btnAdd_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);" 
                                                                Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" />
                                                            <asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel" Text="<%$ Resources:BaseInfo,User_btnCancel %>" OnClick="btnCel_Click"  onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"  /></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
    </form>
</body>
</html>

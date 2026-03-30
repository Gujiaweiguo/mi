<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayInEdit.aspx.cs" Inherits="Lease_PayIn_PayInEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Menu_ModifyReceivables")%></title>
    <link href="../../App_Themes/CSS/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        <!--
        
        table.payPunIn tr.rowHeight{ height:28px;}
        
        table.payPunIn tr.headLine{ height:1px; }
        table.payPunIn tr.bodyLine{ height:1px; }
        
        td.baseLable{ padding-right:10px;text-align:right;}
        td.baseInput{ align:left;padding-right:20px }
        -->
      </style>
       <script type="text/javascript" src="../../JavaScript/Calendar.js" language="javascript" charset="gb2312"></script>
       <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
       <script type="text/javascript">
        <!--
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
	    
	     function Load()
        {
            addTabTool("<%=baseInfo %>,Lease/PayIn/PayInEdit.aspx");
            loadTitle();
        }
         function ShowTree()
        {
    	    strreturnval=window.showModalDialog('../Shop/SelectShop.aspx','window','dialogWidth=237px;dialogHeight=420px');
		    window.document.all("allvalue").value = strreturnval;
		    if ((window.document.all("allvalue").value != "undefined") && (window.document.all("allvalue").value != ""))
		    {
		         var objImgBtn1 = document.getElementById('<%= LinkButton1.ClientID %>');
                objImgBtn1.click();
            }
		    else
		    {
			    return;	
		    }  
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
                <table style="width: 100%" border="0" cellpadding="0" cellspacing="0" class="payPunIn">
                    <tr>
                        <td style="width: 4px; text-align:left" class="tdTopRightBackColor">
                        <img alt="" class="imageLeftBack" style=" text-align:left"  />
                        </td>
                        <td class="tdTopRightBackColor" style="text-align:left; width: 631px;">
                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo,Lease_lblPayInEdit %>" Height="12pt" Width="218px"></asp:Label>
                        </td>
                        <td class="tdTopRightBackColor" style=" text-align: right;">
                        <img class="imageRightBack" style="width: 7px; height: 22px" />
                        </td>
                    </tr>
                    <tr class="headLine">
                        <td style="width: 4px">
                        </td>
                        <td style="width: 631px">
                        </td>
                        <td style="width: 20px">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" class="tdBackColor">
                            <table style="width:100%"  border="0" cellpadding="0" cellspacing="0">
                                <tr style="height:10px">
                                    <td style="width: 91px">
                                    </td>
                                    <td style="width: 192px">
                                    </td>
                                    <td style="width: 69px">
                                    </td>
                                    <td style="width: 202px">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr class="rowHeight">
                                    <td style="width: 91px" class="baseLable">
                                        <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Lease_lblPayInCode %>"></asp:Label></td>
                                    <td style="width: 192px">
                                        <asp:TextBox ID="txtPayInCode" runat="server" CssClass="ipt160px" MaxLength="16"></asp:TextBox></td>
                                    <td style="width: 69px" class="baseLable">
                                        <asp:Label ID="Label14" runat="server" CssClass="labelStyle" 
                                            Text="<%$ Resources:BaseInfo,PotShop_lblPotShopName %>"></asp:Label>
                                    </td>
                                    <td style="width: 202px">
                                        <asp:TextBox ID="txtShopCode" runat="server" CssClass="ipt160px" MaxLength="16"></asp:TextBox></td>
                                    <td>
                                        <asp:Button ID="btnQuery" runat="server" CssClass="buttonQuery" OnClick="btnQuery_Click"
                                            Text="<%$ Resources:BaseInfo,User_lblQuery %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                                        &nbsp;
                                        <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr class="rowHeight">
                                    <td style="text-align: center;" colspan="5">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 95%">
                                            <tr style="height: 1px">
                                                <td style="width: 160px; height: 1px; background-color: #738495">
                                                </td>
                                            </tr>
                                            <tr style="height: 1px">
                                                <td style="width: 160px; height: 1px; background-color: #ffffff">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr class="rowHeight">
                                    <td style="width: 91px" class="baseLable">
                                        <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustName %>"></asp:Label></td>
                                    <td style="width: 192px">
                                        <asp:TextBox ID="txtCustName" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                    <td align="center" colspan="3" rowspan="9">
                                        <asp:GridView ID="gdvwPayIn" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#E1E0B2" Width="85%" OnSelectedIndexChanged="gdvwPayIn_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="gdvwPayIn_OnPageIndexChanging">
                                            <Columns>
                                                <asp:BoundField DataField="PayInID" HeaderText="PayInID">
                                                    <ItemStyle CssClass="hidden" />
                                                    <HeaderStyle CssClass="hidden" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PayInCode" HeaderText="<%$ Resources:BaseInfo,Lease_lblPayInCode %>">
                                                    <ItemStyle BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PaidAmt" HeaderText="<%$ Resources:BaseInfo,Lease_lblPayInAmt %>">
                                                    <ItemStyle BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PayInStatusName" HeaderText="<%$ Resources:BaseInfo,Lease_lblPayInStatus %>">
                                                    <ItemStyle BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:CommandField ShowSelectButton="True" HeaderText="<%$ Resources:BaseInfo,PotShop_Selected %>"> 
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                    <ItemStyle BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                </asp:CommandField>
                                                <asp:BoundField DataField="PayInStartDate" HeaderText="PayInStartDate">
                                                    <ItemStyle CssClass="hidden" />
                                                    <HeaderStyle CssClass="hidden" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PayInEndDate" HeaderText="PayInEndDate">
                                                    <ItemStyle CssClass="hidden" />
                                                    <HeaderStyle CssClass="hidden" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PayInDate" HeaderText="PayInDate">
                                                    <ItemStyle CssClass="hidden" />
                                                    <HeaderStyle CssClass="hidden" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PayInDataSource" HeaderText="PayInDataSource">
                                                    <ItemStyle CssClass="hidden" />
                                                    <HeaderStyle CssClass="hidden" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ShopID" HeaderText="ShopID">
                                                    <ItemStyle CssClass="hidden" />
                                                    <HeaderStyle CssClass="hidden" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PayInStatus" HeaderText="PayInStatus">
                                                    <ItemStyle CssClass="hidden" />
                                                    <HeaderStyle CssClass="hidden" />
                                                </asp:BoundField>
                                            </Columns>
                                            <PagerStyle BackColor="White" ForeColor="#000066"   HorizontalAlign="Right" />
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
                                <tr class="rowHeight">
                                    <td style="width: 91px" class="baseLable">
                                        <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdBoard_lblContractID %>"></asp:Label></td>
                                    <td style="width: 192px">
                                        <asp:TextBox ID="txtContractCode" runat="server" CssClass="ipt160px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="rowHeight">
                                    <td style="width: 91px" class="baseLable">
                                        <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Lease_lblShopCode %>"></asp:Label></td>
                                    <td style="width: 192px">
                                        <asp:TextBox ID="txtShopCodeF" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                </tr>
                                <tr class="rowHeight">
                                    <td style="width: 91px" class="baseLable">
                                        <asp:Label ID="Label7" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Lease_lblPayInCode %>"></asp:Label></td>
                                    <td style="width: 192px">
                                        <asp:TextBox ID="txtPayInCodeF" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                </tr>
                                <tr class="rowHeight">
                                    <td style="width: 91px" class="baseLable">
                                        <asp:Label ID="Label8" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labChargeStartDate %>"></asp:Label></td>
                                    <td style="width: 192px">
                                        <asp:TextBox ID="txtPayInStartDate" runat="server" CssClass="ipt160px" onclick="calendar()"></asp:TextBox></td>
                                </tr>
                                <tr class="rowHeight">
                                    <td style="width: 91px;" class="baseLable">
                                        <asp:Label ID="Label9" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labChargeEndDate %>"></asp:Label></td>
                                    <td style="width: 192px; height: 18px">
                                        <asp:TextBox ID="txtPayInEndDate" runat="server" CssClass="ipt160px" onclick="calendar()"></asp:TextBox></td>
                                </tr>
                                <tr class="rowHeight">
                                    <td style="width: 91px" class="baseLable">
                                        <asp:Label ID="Label10" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Lease_lblPayInAmt %>"></asp:Label></td>
                                    <td style="width: 192px">
                                        <asp:TextBox ID="txtPayInAmt" runat="server" CssClass="ipt160px" style="ime-mode:disabled;"></asp:TextBox></td>
                                </tr>
                                <tr class="rowHeight">
                                    <td style="width: 91px" class="baseLable">
                                        <asp:Label ID="Label11" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Lease_lblPayInDataSource %>"></asp:Label></td>
                                    <td style="width: 192px">
                                        <asp:DropDownList ID="dropPayInDataSource" runat="server" Width="167px">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr class="rowHeight">
                                    <td style="width: 91px" class="baseLable">
                                        <asp:Label ID="Label12" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Lease_lblPayInDate %>"></asp:Label></td>
                                    <td style="width: 192px">
                                        <asp:TextBox ID="txtPayInDate" runat="server" CssClass="ipt160px" onclick="calendar()"></asp:TextBox></td>
                                </tr>
                                <tr class="rowHeight">
                                    <td class="baseLable" style="width: 91px">
                                        <asp:Label ID="Label13" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Lease_lblPayInStatus %>"></asp:Label></td>
                                    <td style="width: 192px">
                                        <asp:DropDownList ID="dropPayInStatus" runat="server" Width="167px">
                                        </asp:DropDownList></td>
                                    <td align="center" colspan="3">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 92%">
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
                                <tr style="height:40px">
                                    <td align="right" colspan="2">
                                        <asp:Button ID="btnEdit" runat="server" CssClass="buttonEdit" OnClick="btnEdit_Click"
                                            Text="<%$ Resources:BaseInfo,Btn_Edit %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>&nbsp;
                                        <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" OnClick="btnSave_Click"
                                            Text="<%$ Resources:BaseInfo,User_btnOk %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>&nbsp;
                                        <asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel" OnClick="btnCel_Click"
                                            Text="<%$ Resources:BaseInfo,User_btnCancel %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                                        &nbsp; &nbsp; &nbsp;&nbsp;
                                    </td>
                                    <td style="width: 69px">
                                    </td>
                                    <td align="right" colspan="2" style="padding-right:10px">
                                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                                    </td>
                                </tr>
                                <tr style="height:10px">
                                    <td style="width: 91px; text-align: right">
                                    </td>
                                    <td style="width: 192px">
                                    </td>
                                    <td style="width: 69px">
                                    </td>
                                    <td align="right" colspan="2">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                 <input id="allvalue" runat="server" style="width: 25px" type="hidden" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>

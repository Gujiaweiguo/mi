<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BrandLevel.aspx.cs" Inherits="Lease_Brand_BrandLevel" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "BrandLevel_BrandLevelAdd")%></title>
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
	<script type="text/javascript">
	    function Load()
	    {
	        addTabTool(  "<%=baseInfo %>,Lease/Brand/BrandLevel.aspx");
	        loadTitle();

	    }
	    function textleave(form1)
        {   
            var key=window.event.keyCode;
            if(key==8 || key==46 || key==48 || key==49 || key==50 || key==51 || key==52 || key==53 || key==54 || key==55 || key==56 ||
               key==57 || key==190 || key == 96 || key == 97 || key == 98 || key == 99 || key == 100 || key == 101 || key == 102 ||
               key == 103 || key == 104 || key == 105 || key == 110 || key == 189 || key == 109)
            {
		        window.event.returnValue =true;
	        }
	        else
	        {
		        window.event.returnValue =false;
	        }
	    }
	    function CheckData()
	    {
	        if(isEmpty(document.all.txtBrandLevelCode.value))
            {
                parent.document.all.txtWroMessage.value='<%=(string)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") %>';
                document.all.txtBrandLevelCode.focus();
                return false;
            }
            if(isEmpty(document.all.txtBrandLevelName.value))
            {
                parent.document.all.txtWroMessage.value='<%=(string)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") %>';
                document.all.txtBrandLevelName.focus();
                return false;
            }
	    }
    </script>

</head>
<body onload="Load()" style="margin:0px">
     <form id="form1" runat="server">
    <asp:ScriptManager id="ScriptManager1" runat="server"></asp:ScriptManager>
    <div align="center" style="width:100%; text-align: left;">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
               
        <table border="0" cellpadding="0" cellspacing="0" style="height: 350px; width:100%; vertical-align:top" >
            <tr>                
                
                <td style="height:350px; width: 100%; vertical-align:top;" valign="top">
                    <table border="0" cellpadding="0" cellspacing="0" style="height: 273px; width:100%; vertical-align:top; " >
                        
                        <tr>
                            <td colspan="7" style="height: 56px; background-color: #e1e0b2" valign="top">
                                 <table border="0" cellpadding="0" cellspacing="0" style="height: 22px; width:100%; ">
                                    <tr>
                                        <td class="tdTopRightBackColor"    valign="top" style="width: 8px; height:22px;  text-align:left" >
                                            <img alt="" class="imageLeftBack" style=" text-align:left; height: 22px;"  />
                                            </td>
                                            <td class="tdTopRightBackColor" style="height: 22px; text-align: left;">
                                        <asp:Label
                                            ID="labBrandLevel" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,BrandLevel_BrandLevelAdd%>"></asp:Label></td>
                                      
                                        <td class="tdTopRightBackColor"   valign="top" style="width: 20px; height: 22px;">
                                            <img class="imageRightBack" style="width: 7px; height: 22px" />
                                            </td>
                                    </tr>
                                </table>          
                            </td>
                        </tr>
                          <tr>
                            <td colspan="1" style="height: 6px; background-color: #e1e0b2" valign="top">
                                 
                            </td>
                                                        <td colspan="1" style="height: 6px; background-color: #e1e0b2; text-align: center;" valign="top">
                                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 95%; position: relative;
                                                                text-align: center">
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
                                                        <td colspan="1" style="height: 6px; background-color: #e1e0b2" valign="top">
                                 
                            </td>
                            <td colspan="2" style="height: 3px; background-color: #e1e0b2; text-align: center;" valign="top">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 95%; position: relative">
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
                             <td colspan="1" style="height: 6px; background-color: #e1e0b2" valign="top">
                                 
                            </td>
                        </tr>
                        
                        <tr>
                            <td style=" width:388px;  background-color: #e1e0b2; height: 12px; text-align: right;" align="center">                            
                                <asp:Label ID="labBrandLevelCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,BrandLevel_BrandLevelCode%>" ></asp:Label>&nbsp;</td>
                            <td style="width:147px;  background-color: #e1e0b2; text-align: left; height: 12px;"> 
                                <asp:TextBox ID="txtBrandLevelCode"  runat="server"  CssClass="ipt160px" MaxLength="8" ></asp:TextBox></td>
                            <td align="center" colspan="1" rowspan="4" style="width: 99px; background-color: #e1e0b2">
                            </td>
                          <td colspan="2" rowspan="9" style="background-color: #e1e0b2; text-align: left; width: 598px;" valign="top" id="td2">
                                <table cellpadding="0" cellspacing="0" id="tablegrid1" style="height:100%;">
                                    <tr>
                                        
                                        <td style="width: 250px; height: 233px;" valign="top" align="center">
                                            <asp:GridView ID="GrdBrandLevel" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                BorderStyle="Inset" BorderWidth="1px" CellPadding="3" CssClass="gridview" 
                                               
                                                PageSize="10" Width="250px" OnSelectedIndexChanged="GrdBrandLevel_SelectedIndexChanged" Height="232px" AllowPaging="True" OnPageIndexChanging="GrdBrandLevel_OnPageIndexChanging">
                                                <Columns>
                                                    <asp:BoundField DataField="BrandLevelID">
                                                        <ItemStyle CssClass="hidden" />
                                                        <HeaderStyle CssClass="hidden" />
                                                        <FooterStyle CssClass="hidden" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="BrandLevelCode">
                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="hidden" />
                                                        <ItemStyle CssClass="hidden" BorderColor="#E1E0B2" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="BrandLevelName" HeaderText="<%$ Resources:BaseInfo,BrandLevel_BrandLevelName %>">
                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                        <ItemStyle BorderColor="#E1E0B2"  Width="70%"  HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:CommandField ShowSelectButton="True" HeaderText="<%$ Resources:BaseInfo,PotShop_Selected %>">
                                                        <HeaderStyle CssClass="gridviewtitle" BorderColor="#E1E0B2" />
                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                    </asp:CommandField>
                                                </Columns>
                                                <FooterStyle BackColor="Red" ForeColor="#000066" />
                                                <RowStyle Font-Overline="False" Font-Size="10pt" ForeColor="Black" Height="10px" />
                                                <SelectedRowStyle BackColor="#FFFFCD" Font-Bold="False" ForeColor="Black" />
                                                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Right" />
                                                <HeaderStyle BackColor="#E1E0B2" Font-Bold="False" Height="10px" />
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
                                            &nbsp;
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        
                                        <td align="center" style="vertical-align:top; height: 67px; text-align: center;">
                                            &nbsp;&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td colspan="2" rowspan="5" style="background-color: #e1e0b2; text-align: left; width: 147px;" valign="top" id="td1">
                            </td>
                            
                        </tr>
                         <tr style="width:10px;">
                            <td style=" width:388px; background-color: #e1e0b2; text-align: right;" align="center">                            
                                <asp:Label ID="labBrandLevelName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,BrandLevel_BrandLevelName%>"></asp:Label>&nbsp;</td>
                            <td style="  background-color: #e1e0b2; text-align: left;vertical-align:top; width: 147px;"> 
                                <asp:TextBox ID="txtBrandLevelName" runat="server" CssClass="ipt160px" MaxLength="8" ></asp:TextBox></td>
                        </tr>
                        <tr style="width: 10px">
                            <td align="center" style="width: 388px; height: 26px; background-color: #e1e0b2; text-align: right;">
                                <asp:Label ID="labNode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AreaSize_lblNote%>"></asp:Label>&nbsp;</td>
                            <td style="vertical-align: top; height: 26px; background-color: #e1e0b2; text-align: left; width: 147px;">
                                <asp:TextBox ID="txtNode" runat="server" CssClass="ipt160px" MaxLength="16" ></asp:TextBox></td>
                        </tr>
                         <tr style="width:10px;">
                            <td style=" background-color: #e1e0b2; height: 206px; text-align: right;" align="center" colspan="2">                            
                                <asp:Button ID="btnSave" runat="server" CssClass="buttonSave"  OnClick="btnSave_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                    Text="<%$ Resources:BaseInfo,DeptTree_labDeptAdd%>" />&nbsp;
                                <asp:Button ID="btnEdit"
                                        runat="server" CssClass="buttonEdit"  OnClick="btnEdit_Click" Text="<%$ Resources:BaseInfo,User_btnChang %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                        />&nbsp;
                                <asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                             OnClick="btnCancel_Click" Text="<%$ Resources:BaseInfo,User_btnCancel %>"
                                           />
                                <asp:Label
                                                ID="lblTotalNum" runat="server" Height="9px" Visible="False"></asp:Label>
                                            <asp:Label ID="lblCurrent" Visible=false runat="server" ForeColor="Red" Height="9px">1</asp:Label>&nbsp;
                            </td>
                        </tr>
                                                  <tr>
                            <td colspan="7" style="height: 10px; background-color: #e1e0b2; vertical-align: top; text-align: center;" valign="top"><table border="0" cellpadding="0" cellspacing="0" style="width: 96%; height: 1px; text-align: center;" >
                                <tr >
                                    <td style="width: 180px; height: 1px; background-color: #738495; position: relative; top: -15px;" align="center">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 180px; height: 1px; background-color: #ffffff; position: relative; top: -15px;" align="center">
                                    </td>
                                </tr>
                            </table>
                                 
                            </td>
                        </tr>
                        
                        
                        
                    </table>
                </td>

            </tr>
        </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
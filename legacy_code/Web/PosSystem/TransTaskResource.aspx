<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TransTaskResource.aspx.cs" Inherits="PosSystem_TransTaskResource" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Store_TaskManage")%></title>
    <link href="../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link rel="StyleSheet" href="../App_Themes/nlstree/nlstree-basic.css" type="text/css" />
    <link rel="StyleSheet" href="../App_Themes/nlstree/nlsctxmenu.css" type="text/css" />
    <script type="text/javascript" src="../App_Themes/nlstree/nlstree.js"></script>
    <script type="text/javascript" src="../App_Themes/nlstree/nlsctxmenu.js"></script>
	<script type="text/javascript" src="../JavaScript/Common.js"></script>	
	<script type="text/javascript" src="../JavaScript/TreeShow.js"></script>
	<script language="javascript" type="text/javascript" src="../JavaScript/TabTools.js"></script>
<script type="text/javascript">
function tree()
{
	    treearray();
	    loadTitle();    
}

    //text控件文本验证
    function allTextBoxValidator(sForm)
    {
        if(isEmpty(document.all.txtAreaCode.value))  
        {
            parent.document.all.txtWroMessage.value="区域编码不能为空!";
            document.all.txtAreaCode.focus();
            return false;					
        }
        
        if(isEmpty(document.all.txtAreaName.value))  
        {
            parent.document.all.txtWroMessage.value="区域名称不能为空!";
            document.all.txtAreaName.focus();
            return false;					
        }
    }
    function BtnUp( p )
    {
	    var t = String(p)
	    var l = t.substring(3,15); 
	    document.getElementById( p ).style.backgroundImage = 'url(../App_Themes/CSS/BtnImage/btn_' + l + '.gif)';
    }
    function BtnOver( p )
    {
	    var t = String(p)
	    var l = t.substring(3,15); 
	    document.getElementById( p ).style.backgroundImage = 'url(../App_Themes/CSS/BtnImage/over_' + l + '.gif)';
    }
		-->
</script>

    <style type="text/css">
        .style1
        {
            height: 275px;
            width: 258px;
        }
        .style2
        {
            height: 20px;
            width: 258px;
        }
        .style3
        {
            height: 42px;
            width: 258px;
        }
    </style>

    </head>
<body onload='tree();' topmargin=0 leftmargin=0>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:HiddenField ID="depttxt" runat="server" EnableViewState="False" />
                <asp:HiddenField ID="selectdeptid" runat="server" />
                <asp:HiddenField ID="deptid" runat="server"  />
                <table border="0" cellpadding="0" cellspacing="0" style="height: 430px; width:98%" >
                    <tr>
                    <td style="width: 30%; height: 401px; vertical-align: top;">
                    <table border="0" cellpadding="0" cellspacing="0" style="height: 255px; vertical-align:top;" width="100%">
                                <tr>
                                    <td class="tdTopBackColor" style="width: 266px; height: 27px;">
                                        <img alt="" class="imageLeftBack" /><asp:Label ID="Label2" runat="server" CssClass="lblTitle"
                                            Text="<%$ Resources:BaseInfo,Store_TaskItems %>"></asp:Label></td>
                                    <td class="tdTopRightBackColor" valign="top" style="height: 27px">
                                        &nbsp;<img class="imageRightBack" /></td>
                                </tr>
                                <tr height="1">
                                    <td colspan="2" style="height: 1px" class="tdTopBackColor">
                                    </td>
                                </tr>
                                <tr>
                                
                                    <td class="tdBackColor" colspan="2" rowspan="10" style="height: 340px; text-align: center"
                                        valign="top">
                                        
                                        <table style="height: 423px">
                                                  <tr>
                                        <td style="height: 320px; width: 258px;">
                                                        <asp:GridView ID="gvTransTask" runat="server" AutoGenerateColumns="False" 
                                                            Height="100%" Width="90%" onrowdatabound="gvTransTask_RowDataBound" 
                                                            AllowPaging="True" onpageindexchanging="gvTransTask_PageIndexChanging" 
                                                            onselectedindexchanged="gvTransTask_SelectedIndexChanged" BackColor="White" 
                                                            BorderStyle="Inset">
                                                            <Columns>
                                                            <asp:BoundField DataField="TaskID">
                                                             <ItemStyle CssClass="hidden" />
                                                             <HeaderStyle CssClass="hidden" />
                                                             <FooterStyle CssClass="hidden" />
                                                         </asp:BoundField>
                                                                <asp:BoundField DataField="TaskName" 
                                                                    HeaderText="<%$ Resources:BaseInfo,Store_TaskName %>">
                                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                    </asp:BoundField>                                                                    
                                                                <asp:CommandField HeaderText="<%$ Resources:BaseInfo,PotShop_Selected %>" 
                                                                    ShowSelectButton="True">
                                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                    </asp:CommandField>
                                                            </Columns>
                                                            <PagerStyle BackColor="White" ForeColor="#000066"   HorizontalAlign="right" />
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

                                                            <SelectedRowStyle BackColor="#FFFFCD" />

                                                        </asp:GridView>
                                        </td>
                                        </tr>
                                            <tr>
                                                <td style="height: 53px; width: 258px;" valign="top"><table border="0" cellpadding="0" cellspacing="0" style="width: 231px">
                                                    <tr>
                                                        <td style="width: 160px; height: 1px; background-color: #738495">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 160px; height: 1px; background-color: #ffffff">
                                                        </td>
                                                    </tr>
                                                </table>
                                                </td>
                                            </tr>
                                            
                                        </table>
                        </td>
                                </tr>
                            </table>
                    </td>
                    <td style="height: 401px; width: 4%;">
                        </td>
                        <td style="width: 30%; height: 401px; vertical-align: top;">
                            <table border="0" cellpadding="0" cellspacing="0" style="height: 255px; vertical-align:top;" width="100%">
                                <tr>
                                    <td class="tdTopBackColor" style="width: 266px; height: 27px;">
                                        <img alt="" class="imageLeftBack" /><asp:Label ID="labAreaVindicate" runat="server" CssClass="lblTitle"
                                            Text="<%$ Resources:BaseInfo,Store_DateSourceTree %>"></asp:Label></td>
                                    <td class="tdTopRightBackColor" valign="top" style="height: 27px">
                                        &nbsp;<img class="imageRightBack" /></td>
                                </tr>
                                <tr height="1">
                                    <td colspan="2" style="height: 1px" class="tdTopBackColor">
                                    </td>
                                </tr>
                                <tr>
                                
                                    <td class="tdBackColor" colspan="2" rowspan="10" style="height: 340px; text-align: center"
                                        valign="top">
                                        
                                        <table>
                                                  <tr>
                                        <td class="style1" style="height:355px">
                                                        <asp:Panel ID="Panel1" runat="server" BackColor="White" BorderStyle="Inset" BorderWidth="1px"
                                            Font-Size="Medium" Height="320px" HorizontalAlign="Left" ScrollBars="Auto" Width="240px">
                                            <table>
                                                <tr>
                                                    <td style="width: 203px; height: 225px" valign="top" id="treeview">
                                                        &nbsp;</td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        </td>
                                        </tr>
                                            <tr>
                                                <td valign="top" class="style2" style="height:10px"><table border="0" cellpadding="0" cellspacing="0" style="width: 231px">
                                                    <tr>
                                                        <td style="width: 160px; height: 1px; background-color: #738495">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 160px; height: 1px; background-color: #ffffff">
                                                        </td>
                                                    </tr>
                                                </table>
                                                </td>
                                            </tr>
                                            <tr>
                                            <td style=" vertical-align:top; text-align:right;" class="style3">
                                                      <asp:Button ID="btnAdd" runat="server" CssClass="buttonAdd" 
                                                          Text="<%$ Resources:BaseInfo,User_btnAdd %>" 
                                                          onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" 
                                                          onmouseup="BtnUp(this.id);" onclick="btnAdd_Click"/>
                                                      <asp:Button ID="treeClick" runat="server" CssClass="buttonHidden" Width="12px" 
                                                          onclick="treeClick_Click" /></td>
                                            </tr>
                                        </table>
                        </td>
                                </tr>
                            </table>
                        </td>
                        <td style="height: 401px; width: 4%;">
                        </td>
                        <td style="width: 32%; height: 401px; vertical-align: top;">
                            <table border="0" cellpadding="0" cellspacing="0" style="height: 255px; vertical-align:top;" width="100%">
                                <tr>
                                    <td class="tdTopBackColor" style="width: 266px; height: 27px;">
                                        <img alt="" class="imageLeftBack" /><asp:Label ID="Label1" runat="server" CssClass="lblTitle"
                                            Text="<%$ Resources:BaseInfo,Store_DateSourceOfTaskManage %>"></asp:Label></td>
                                    <td class="tdTopRightBackColor" valign="top" style="height: 27px">
                                        &nbsp;<img class="imageRightBack" /></td>
                                </tr>
                                <tr height="1">
                                    <td colspan="2" style="height: 1px" class="tdTopBackColor">
                                    </td>
                                </tr>
                                <tr>
                                
                                    <td class="tdBackColor" colspan="2" rowspan="10" style="height: 340px; text-align: center"
                                        valign="top">
                                        
                                        <table style="height: 421px">
                                                  <tr>
                                        <td style="height: 320px; width: 100%;">
                                                        <asp:GridView ID="gvTransTaskRes" runat="server" AutoGenerateColumns="False" 
                                                            Height="100%" Width="100%" 
                                                            AllowPaging="True" BackColor="White" 
                                                            BorderStyle="Inset" onpageindexchanging="gvTransTask_PageIndexChanging" 
                                                            onrowdatabound="gvTransTaskRes_RowDataBound" 
                                                            onselectedindexchanged="gvTransTaskRes_SelectedIndexChanged">
                                                            <Columns>
                                                            <asp:BoundField DataField="ResourceID">
                                                             <ItemStyle CssClass="hidden" />
                                                             <HeaderStyle CssClass="hidden" />
                                                             <FooterStyle CssClass="hidden" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="deptname" 
                                                                    HeaderText="<%$ Resources:BaseInfo,PotCustomer_BusinessItem %>">
                                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                    </asp:BoundField>     
                                                                     
                                                         <asp:BoundField DataField="resourcename" 
                                                                    HeaderText="<%$ Resources:BaseInfo,Store_ServerResource %>">
                                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                    </asp:BoundField> 
                                                                                                                 
                                                                <asp:CommandField ShowSelectButton="True" 
                                                                    HeaderText="<%$ Resources:BaseInfo,Btn_Del %>" SelectText="删除">
                                                                <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                    </asp:CommandField>
                                                            </Columns>
                                                            <PagerStyle BackColor="White" ForeColor="#000066"   HorizontalAlign="right" />
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
                                                <td class="style2" style="height:10px" valign="top"><table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                    <tr>
                                                        <td style="width: 100%; height: 1px; background-color: #738495">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100%; height: 1px; background-color: #ffffff">
                                                        </td>
                                                    </tr>
                                                </table>
                                                </td>
                                            </tr>
                                            <tr>
                                            <td style=" vertical-align:top; text-align:right;" class="style3">
                                                <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" 
                                                    Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" 
                                                    onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" 
                                                          onmouseup="BtnUp(this.id);" onclick="btnSave_Click" Enabled="False"/>
                                                &nbsp;<asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel" 
                                                    Text="<%$ Resources:BaseInfo,User_btnCancel %>" 
                                                    onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" 
                                                          onmouseup="BtnUp(this.id);" onclick="btnCancel_Click"/>
                                                &nbsp;&nbsp;
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
    </form>
</body>
</html>
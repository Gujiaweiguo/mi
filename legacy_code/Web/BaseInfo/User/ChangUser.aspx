<%@ Page Language="C#" AutoEventWireup="true"  Title="修改用户" CodeFile="ChangUser.aspx.cs" Inherits="MI_Net.ChangUser" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Menu_ModifyanExistedUser")%></title>
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link rel="StyleSheet" href="../../App_Themes/nlstree/nlstree-basic.css" type="text/css" />
    <link rel="StyleSheet" href="../../App_Themes/nlstree/nlsctxmenu.css" type="text/css" />
    <script type="text/javascript" src="../../App_Themes/nlstree/nlstree.js"></script>
    <script type="text/javascript" src="../../App_Themes/nlstree/nlsctxmenu.js"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"></script>
	<script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
<script type="text/javascript">

/*提示信息*/
function deptidnull()
{
    parent.document.all.txtWroMessage.value =document.getElementById("hidSelect").value;
    treearray();
}
function usercodebeing()
{
    parent.document.all.txtWroMessage.value =document.getElementById("hidUserCodeBeing").value;
    treearray();
}
var tabbar ;
function treearray()
{
    var t = new NlsTree('MyTree');
    var treestr =document.getElementById("depttxt").value;
    var treearr = new Array();
    var n=0;
    var id;
    var pid;
    var name;
    var num = treestr.split("^");
    for(var i=0;i<num.length-1;i++)
    {
        if(num[i]!="")
        {
       
           var treenode = num[i].split("|");
            for(var j=0;j<treenode.length;j++)
            {
                id=treenode[0];
                pid=treenode[1];
                name=treenode[2];
            }
          
            t.add(id, pid, name, "", "", true);
            
        }
    }
            t.opt.sort='no';
            t.opt.enbScroll=true;
            t.opt.height="330px";
            t.opt.width="250px";
            t.opt.trg="mainFrame";
            t.opt.oneExp=true;
            t.opt.oneClick=true;
            
            t.render("treeview");
            
            t.treeOnClick = ev_click;   
            t.collapseAll();
            if(document.form1.selectdeptid.value!="")
            {
                t.expandNode(document.form1.selectdeptid.value);
                t.selectNodeById(document.form1.selectdeptid.value);
            }
            //document.getElementById("lblTotalNum").style.display="none";
            //document.getElementById("lblCurrent").style.display="none";
}
function ev_click(e, id)
{
	
    document.form1.deptid.value=id;
    document.form1.treeClick.click(); 
} 
    function allTextBoxValidator(sForm)
    {
        if(isEmpty(document.all.txtUserCode.value))  
        {
            parent.document.all.txtWroMessage.value=document.getElementById("hidMessage").value;
            document.all.txtUserCode.focus();
            return false;					
        }
        if(isEmpty(document.all.txtUserName.value))  
        {
            parent.document.all.txtWroMessage.value=document.getElementById("hidMessage").value;
            document.all.txtUserName.focus();
            return false;					
        }
        //if(isEmpty(document.all.txtPassword.value))  
        //{
         //   parent.document.all.txtWroMessage.value=document.getElementById("hidMessage").value;
         //   document.all.txtPassword.focus();
         //   return false;					
        //}
        if(isEmpty(document.all.txtWorkNo.value))  
        {
            parent.document.all.txtWroMessage.value=document.getElementById("hidMessage").value;
            document.all.txtWorkNo.focus();
            return false;					
        }
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
     function Load()
    {
        parent.document.all.txtWroMessage.value = "";
        addTabTool("<%=baseInfo %>,BaseInfo/User/ChangUser.aspx");
        loadTitle();
    }
		-->
</script>

    
</head>
<body onload='treearray();Load();' topmargin=0 leftmargin=0>
    <form id="form1" runat="server">
    <asp:ScriptManager id="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
            <asp:HiddenField id="depttxt" runat="server"></asp:HiddenField>
            <asp:HiddenField id="deptid" runat="server" ></asp:HiddenField>
                <asp:HiddenField ID="selectdeptid" runat="server" />
        <table border="0" cellpadding="0" cellspacing="0" style="height: 420px; width:100%; vertical-align:top; text-align:center;">
            <tr>               
                  <td style="width:33%; height: 401px; vertical-align: top;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 402px;">
                        <tr>
                                          <td    style="vertical-align:top; height: 22px; width: 100%;" >
                        
                         <table border="0" cellpadding="0" cellspacing="0" style="height: 22px; width:100%;   ">
                        
                        
                            <tr>
                                <td class="tdTopRightBackColor"    valign="top" style="width: 8px; height:22px;  text-align:left" >
                                    <img alt="" class="imageLeftBack" style=" text-align:left; height: 22px;"  />
                                    </td>
                                    <td class="tdTopRightBackColor" style="height: 22px; text-align: left;">
                                <asp:Label ID="labUserTree" runat="server" CssClass="lblTitle" Text='<%$ Resources:BaseInfo,AddUser_labUserTree %>'></asp:Label></td>
                              
                                <td class="tdTopRightBackColor"   valign="top" style="width: 20px; height: 22px;">
                                    <img class="imageRightBack" style="width: 7px; height: 22px" />
                                    </td>
                            </tr>
                        </table>
                        </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="height: 360px; background-color: #e1e0b2;
                                 vertical-align:top; text-align:center">
                                <table style="text-align:center;">
                      
                                <tr>
                                <td style="height: 350px">
                                                 <asp:Panel ID="Panel1" runat="server" BorderStyle="Inset" BorderWidth="1px"
                                    Height="340px" ScrollBars="Auto" Width="260px" BackColor="White">
                                   <table>
                                   <tr>
                                    <td valign="top" id ="treeview" style="height: 330px; width: 159px;">
                                    </td>
                                    </tr>
                                    </table>
                                </asp:Panel>
                                </td>
                                </tr>
                                </table>
                                    <asp:Button ID="treeClick" runat="server" CssClass="buttonHidden" OnClick="treeClick_Click" Width="1px" /></td>
                        </tr>
                    </table>
                </td>

                <td style="width:4%; height: 401px">
                </td>
                <td style="width: 32%; height: 100%; vertical-align: top;">
                    <table border="0" cellpadding="0" cellspacing="0" style="100%; height: 402px">
                        <tr>
                        <td    style="vertical-align:top; height: 22px;" valign="top" >
                            <table border="0" cellpadding="0" cellspacing="0" style="height: 22px; width:100%;   ">
                        
                        
                            <tr>
                                <td class="tdTopRightBackColor"    valign="top" style="width: 8px; height:22px;  text-align:left" >
                                    <img alt="" class="imageLeftBack" style=" text-align:left; height: 22px;"  />
                                    </td>
                                    <td class="tdTopRightBackColor" style="height: 22px; text-align: left;">
                                        <asp:Label ID="Label1" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,AddUser_labUserInfo %>"
                                            Width="151px"></asp:Label></td>
                              
                                <td class="tdTopRightBackColor"   valign="top" style="width: 20px; height: 22px;">
                                    <img class="imageRightBack" style="width: 7px; height: 22px" />
                                    </td>
                            </tr>
                        </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="vertical-align: top; height: 370px; background-color: #e1e0b2;text-align:center; width: 280px;">
                                <table style="text-align:center;">
                                <tr>
                                <td>
                                    <asp:GridView ID="GrdUser" runat="server" AutoGenerateColumns="False" BackColor="White" BorderStyle="Inset" BorderWidth="1px" CellPadding="3" CssClass="gridview"
                                        Height="313px"  OnRowDataBound="GrdUser_RowDataBound"
                                        OnSelectedIndexChanged="GrdUser_SelectedIndexChanged" PageSize="12" Width="280px" AllowPaging="True" OnPageIndexChanging="GrdUser_PageIndexChanging">
                                        <Columns>
                                            <asp:BoundField DataField="UserID">
                                                <ItemStyle CssClass="hidden"/>
                                                <HeaderStyle CssClass="hidden" />
                                                <FooterStyle CssClass="hidden" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="UserCode" HeaderText="<%$ Resources:BaseInfo,User_lblUserLoginCode %>">
                                                <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                <ItemStyle BorderColor="#E1E0B2" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:BaseInfo,User_lblUserName %>">
                                                <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                <ItemStyle BorderColor="#E1E0B2" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="WorkNo" HeaderText="ID">
                                                <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                <ItemStyle BorderColor="#E1E0B2" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="UserStatus" HeaderText="<%$ Resources:BaseInfo,User_lblUserStatusStr %>">
                                                <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                <ItemStyle BorderColor="#E1E0B2" />
                                            </asp:BoundField>
                                            <asp:CommandField ShowSelectButton="True" HeaderText="<%$ Resources:BaseInfo,PotShop_Selected %>">
                                                <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                <ItemStyle BorderColor="#E1E0B2" />
                                            </asp:CommandField>
                                        </Columns>
                                                <FooterStyle BackColor="Red" ForeColor="#000066"/>
                                                <RowStyle ForeColor="Black" Height="10px" Font-Overline="False" Font-Size="10pt" />
                                                <SelectedRowStyle BackColor="#FFFFCD" Font-Bold="False" ForeColor="Black" />
                                                <PagerStyle BackColor="White" ForeColor="#000066"   HorizontalAlign="Left" />
                                                <HeaderStyle BackColor="#E1E0B2" Height="10px" Font-Bold="False"  />
                                                <PagerTemplate>                                                   
                                                <asp:LinkButton ID="LinkButtonFirstPage" runat="server" CommandArgument="First" CommandName="Page" 
                                                 Visible="<%# ((GridView)Container.NamingContainer).PageIndex != 0 %>" Font-Size="Smaller">首页</asp:LinkButton> 

                                                <asp:LinkButton ID="LinkButtonPreviousPage" runat="server" CommandArgument="Prev" CommandName="Page" 
                                                 Visible="<%# ((GridView)Container.NamingContainer).PageIndex != 0 %>" Font-Size="Smaller">上一页</asp:LinkButton> 

                                                <asp:LinkButton ID="LinkButtonNextPage" runat="server" CommandArgument="Next" CommandName="Page" 
                                                 Visible="<%# ((GridView)Container.NamingContainer).PageIndex != ((GridView)Container.NamingContainer).PageCount - 1 %>" Font-Size="Smaller">下一页</asp:LinkButton> 

                                                <asp:LinkButton ID="LinkButtonLastPage" runat="server" CommandArgument="Last" CommandName="Page" 
                                                 Visible="<%# ((GridView)Container.NamingContainer).PageIndex != ((GridView)Container.NamingContainer).PageCount - 1 %>" Font-Size="Smaller">尾页</asp:LinkButton> 
                                                <asp:textbox id="txtNewPageIndex" runat="server" width="20px" text='<%# ((GridView)Container.Parent.Parent).PageIndex + 1 %>' />/
                                                <asp:Label ID="LabelPageCount" runat="server" 
                                                 Text="<%# ((GridView)Container.NamingContainer).PageCount %>" Font-Size="Small"></asp:Label> 
                                                <asp:linkbutton id="btnGo" runat="server" causesvalidation="False" commandargument="-1" commandname="Page" text="GO" Font-Size="Small" /> 
                                                </PagerTemplate>         
                                                <PagerSettings Mode="NextPreviousFirstLast"  />
                                    </asp:GridView>
                                </td>
                                </tr>
                                <tr>
                                  <td class="tdBackColor"   style="width: 215px; height: 15px; text-align: center;">
                                </td>
                                </tr>
                                </table>
                                </td>
                        </tr>
                    </table>
                </td>
                <td style="width:4%; height: 401px">
                </td>
                    <td style="width: 32%; height: 401px; vertical-align: top;">
                    <table border="0" cellpadding="0" cellspacing="0" style="height: 404px; width: 100%;">
                        <tr>
                         <td style="vertical-align:top; height: 22px; width:100%;" colspan="4" >
                         <table border="0" cellpadding="0" cellspacing="0" style="height: 22px; width: 100%">
                            <tr>
                                <td class="tdTopRightBackColor"    valign="top" style="width: 8px; height:22px;  text-align:left" >
                                    <img alt="" class="imageLeftBack" style=" text-align:left; height: 22px;"/>
                                    </td>
                                    <td class="tdTopRightBackColor" style="height: 22px; text-align: left;">
                                <asp:Label ID="labUserDefine" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,AddUser_labUserDefine %>"></asp:Label></td>
                              
                                <td class="tdTopRightBackColor"   valign="top" style="width: 20px; height: 22px;">
                                    <img class="imageRightBack" style="width: 7px; height: 22px" />
                                    </td>
                            </tr>
                        </table>
                          </td>
                        </tr>
                         <tr>
                            <td style="height: 0px; background-color: #e1e0b2; text-align: left;" colspan="4">
                        </td>
                        </tr>
                        <tr>
                            <td style="width: 82px; height: 22px; background-color: #e1e0b2; text-align: left;">
                        <asp:Label ID="lblUserCode" runat="server" Text="<%$ Resources:BaseInfo,User_lblUserLoginCode %>"
                            Width="84px" CssClass="labelStyle"></asp:Label></td>
                            <td style="width: 5px; height: 22px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 114px; height: 25px; background-color: #e1e0b2;">
                        <asp:TextBox ID="txtUserCode" runat="server" Width="110px" CssClass="ipt160px" MaxLength="16"></asp:TextBox></td>
                            <td style="width: 42px; height: 22px; background-color: #e1e0b2">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 82px; height: 25px; background-color: #e1e0b2">
                            
                    <asp:Label id="lblUserName" runat="server" Text='<%$ Resources:BaseInfo,User_lblUserName %>' Width="84px" CssClass="labelStyle"></asp:Label></td>
                            <td style="width: 5px; height: 25px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 114px; height: 25px; background-color: #e1e0b2">
                        <asp:TextBox id="txtUserName" runat="server" Width="110px" CssClass="ipt160px" MaxLength="32"></asp:TextBox></td>
                            <td style="width: 42px; height: 25px; background-color: #e1e0b2">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 82px; height: 25px; background-color: #e1e0b2">
                                <asp:Label ID="Label12" runat="server" CssClass="labelStyle" 
                                    Text="<%$ Resources:BaseInfo,User_lblWorkNo %>" Width="80px"></asp:Label>
                            </td>
                            <td style="width: 5px; height: 25px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 114px; height: 25px; background-color: #e1e0b2">
                                <asp:TextBox ID="txtWorkNo" runat="server" CssClass="ipt160px" MaxLength="16" 
                                    Width="110px"></asp:TextBox>
                            </td>
                            <td style="width: 42px; height: 25px; background-color: #e1e0b2">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 82px; height: 25px; background-color: #e1e0b2">
                                <asp:Label ID="lblMobile1" runat="server" CssClass="labelStyle" 
                                    Font-Size="11pt" Text="<%$ Resources:BaseInfo,User_lblMobile1 %>" Width="84px"></asp:Label>
                            </td>
                            <td style="width: 5px; height: 25px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 114px; height: 25px; background-color: #e1e0b2">
                                <asp:TextBox ID="txtMobile1" runat="server" CssClass="ipt160px" MaxLength="16" 
                                    Width="110px"></asp:TextBox>
                            </td>
                            <td style="width: 42px; height: 25px; background-color: #e1e0b2">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 82px; height: 22px; background-color: #e1e0b2">
                                <asp:Label ID="lblOfficeTel" runat="server" CssClass="labelStyle" 
                                    Text="<%$ Resources:BaseInfo,User_lblOfficeTel %>" Width="84px"></asp:Label>
                            </td>
                            <td style="width: 5px; height: 22px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 114px; height: 25px; background-color: #e1e0b2">
                                <asp:TextBox ID="txtOfficeTel" runat="server" CssClass="ipt160px" 
                                    MaxLength="16" Width="110px"></asp:TextBox>
                            </td>
                            <td style="width: 42px; height: 22px; background-color: #e1e0b2">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 82px; height: 22px; background-color: #e1e0b2">
                                <asp:Label ID="lblEmail" runat="server" CssClass="labelStyle" 
                                    Text="<%$ Resources:BaseInfo,User_lblEMail %>" Width="84px"></asp:Label>
                            </td>
                            <td style="width: 5px; height: 22px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 114px; height: 25px; background-color: #e1e0b2">
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="ipt160px" MaxLength="128" 
                                    Width="110px"></asp:TextBox>
                            </td>
                            <td style="width: 42px; height: 22px; background-color: #e1e0b2">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 82px; height: 22px; background-color: #e1e0b2">
                                <asp:Label ID="lblUserStatus" runat="server" CssClass="labelStyle" 
                                    Text="<%$ Resources:BaseInfo,User_lblUserStatus %>" Width="84px"></asp:Label>
                            </td>
                            <td style="width: 5px; height: 22px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 114px; height: 25px; background-color: #e1e0b2">
                                <asp:DropDownList ID="cmbUserState" runat="server" CssClass="ipt160px" 
                                    Width="110px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 42px; height: 22px; background-color: #e1e0b2">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 82px; height: 22px; background-color: #e1e0b2">
                                <asp:Label ID="lblRoleName" runat="server" CssClass="labelStyle" 
                                    Text="<%$ Resources:BaseInfo,Role_lblRoleName %>" Width="84px"></asp:Label>
                            </td>
                            <td style="width: 5px; height: 22px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 114px; height: 25px; background-color: #e1e0b2">
                        
                                <asp:ListBox ID="lstRoleName" runat="server" CssClass="list" Height="74px" 
                                    SelectionMode="Multiple" Width="110px"></asp:ListBox>
                            </td>
                            <td style="width: 42px; height: 22px; background-color: #e1e0b2">
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 25px; background-color: #e1e0b2" align="center" colspan="4">
                                <table border="0" cellpadding="0" cellspacing="0" 
                                    style="width: 204px;text-align: center;">
                                    <tr>
                                        <td style="width: 240px; height: 1px; background-color: #738495">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 240px; height: 1px; background-color: #ffffff">
                                        </td>
                                    </tr>
                                </table>
                        </td>
                        </tr>
                        <tr>
                            <td style="width: 82px; height: 22px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 5px; height: 22px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 114px; background-color: #e1e0b2">
                                &nbsp;</td>
                            <td style="width: 42px; height: 22px; background-color: #e1e0b2">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="width: 280px; height: 5px; background-color: #e1e0b2; text-align: center"
                                valign="middle">
                                <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" 
                                    OnClick="btnSave_Click" onmouseout="BtnUp(this.id);" 
                                    onmouseover="BtnOver(this.id);" onmouseup="BtnUp(this.id);" 
                                    Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" />
                                &nbsp;<asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel" 
                                    OnClick="btnCancel_Click" onmouseout="BtnUp(this.id);" 
                                    onmouseover="BtnOver(this.id);" onmouseup="BtnUp(this.id);" 
                                    Text="<%$ Resources:BaseInfo,User_btnCancel %>" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="4" style="height: 40px; background-color: #e1e0b2">
                                &nbsp; &nbsp;&nbsp;&nbsp;<asp:Label ID="lblPassword" runat="server" CssClass="labelStyle" 
                                    Text="<%$ Resources:BaseInfo,User_lblPassword %>" Visible="False" Width="84px"></asp:Label>
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="ipt160px" MaxLength="16" 
                                    TextMode="Password" Visible="False" Width="110px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        &nbsp;
        <asp:HiddenField ID="hidSelect" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidSelect %>" />
        <asp:HiddenField ID="hidUpdate" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidUpdate %>" />
        <asp:HiddenField ID="hidAdd" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidAdd %>" />
        <asp:HiddenField ID="hidUpdateLost" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidUpdateLost %>" />
        <asp:HiddenField ID="hidInsert" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidInsert %>" />
        <asp:HiddenField ID="hidMessage" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidMessage %>" />
    </form>
</body>
</html>
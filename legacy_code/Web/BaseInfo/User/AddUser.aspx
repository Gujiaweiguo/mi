<%@ Page Language="C#" Title="添加用户"  AutoEventWireup="true" CodeFile="AddUser.aspx.cs" Inherits="BaseInfo_User_AddUser" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Menu_NewUser")%></title>
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
//    treearray();
}
function usercodebeing()
{
    treearray();
    parent.document.all.txtWroMessage.value =document.getElementById("hidUserCodeBeing").value;
    
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
    var imgurl;
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
                imgurl=treenode[3];
            }
            t.add(id, pid, name, "",imgurl, true);
            
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
            document.getElementById("lblTotalNum").style.display="none";
            document.getElementById("lblCurrent").style.display="none";
            
            
}
function ev_click(e, id)
{
	
    document.form1.deptid.value=id;
    document.form1.selectdeptid.value=id;
    document.form1.treeClick.click(); 
} 
//text控件文本验证
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
        
        if(isEmpty(document.all.txtPassword.value))  
        {
            parent.document.all.txtWroMessage.value=document.getElementById("hidMessage").value;
            document.all.txtPassword.focus();
            return false;					
        }
        
        if(isEmpty(document.all.txtWorkNo.value))  
        {
            parent.document.all.txtWroMessage.value=document.getElementById("hidMessage").value;
            document.all.txtWorkNo.focus();
            return false;					
        }
    }
function TABLE1_onclick() {

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
function showline()
{
    parent.document.all.txtWroMessage.value = "";
    addTabTool("<%=baseInfo %>,BaseInfo/User/AddUser.aspx");
    loadTitle();
}
		-->
    
</script>
	
    
</head>
<body onload='treearray();showline();' topmargin=0 leftmargin=0>
    <form id="form1" runat="server">
    <asp:ScriptManager id="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table border="0" cellpadding="0" cellspacing="0"  style="height: 410px; vertical-align:top; text-align:center;">
                    <tr>
                        
                        
                        <td style="vertical-align: top; height: 401px; width:32%">
                        <table border="0" cellpadding="0" cellspacing="0" style="width:100%; height: 402px;">
                            <tr>
                                <td    style="vertical-align:top; height: 22px; width: 100%;" >
                        
                         <table border="0" cellpadding="0" cellspacing="0" style="height: 22px; width: 100%;   ">
                        
                        
                            <tr>
                                <td class="tdTopRightBackColor"    valign="top" style="width: 8px; height:22px;  text-align:left" >
                                    <img alt="" class="imageLeftBack" style=" text-align:left; height: 22px;"  />
                                    </td>
                                    <td class="tdTopRightBackColor" style="height: 22px; text-align: left;">
                                <asp:Label ID="labUserTree" runat="server" CssClass="lblTitle" Text='<%$ Resources:BaseInfo,AddUser_labUserTree %>' Width="127px"></asp:Label></td>
                              
                                <td class="tdTopRightBackColor"   valign="top" style="width: 20px; height: 22px;">
                                    <img class="imageRightBack" style="width: 7px; height: 22px; text-align:right;" />
                                    </td>
                            </tr>
                        
                        </table>
                        
                        
                        </td>
                        </tr>
                                               <tr >
                            <td style="height: 1px">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="height: 370px; background-color: #e1e0b2;text-align:center"">
                                <table>
                                <tr>
                                <td style="height: 350px; vertical-align: top; width: 183px;">
                                                 <asp:Panel ID="Panel1" runat="server" BorderStyle="Inset" BorderWidth="1px"
                                    Height="340px" ScrollBars="Auto" Width="260px" BackColor="White">
                                
                                                                    <table>
                                        <tr>
                                    <td valign="top" id ="treeview" style="height: 330px; width: 157px;">
                                             
                                         
                                    </td>
                                        </tr>
                                    </table>
                                
                                
                                
                                </asp:Panel>
                                    &nbsp;
                                </td>
                                </tr>
                                </table>
               
                            </td>
                        </tr>
                    </table>
                        </td>
                        <td style="width: 4%; height: 401px">
                        </td>
                        <td style="vertical-align: top; width:32%; height: 401px">
                    <table border="0" cellpadding="0" cellspacing="0" style="height: 402px;" id="TABLE1" onclick="return TABLE1_onclick()">
                        <tr>
                         <td    style="vertical-align:top; height: 22px;" >
                        
                         <table border="0" cellpadding="0" cellspacing="0" style="height: 22px; width: 100%;   ">
                            <tr>
                                <td class="tdTopRightBackColor"    valign="top" style="width: 8px; height:22px;  text-align:left" >
                                    <img alt="" class="imageLeftBack" style=" text-align:left; height: 22px;"  />
                                </td>
                                <td class="tdTopRightBackColor" style="height: 22px; text-align: left;">
                                <asp:Label
                                    ID="Label1" runat="server" Text='<%$ Resources:BaseInfo,AddUser_labUserInfo %>' Width="151px" CssClass="lblTitle"></asp:Label></td>
                                <td class="tdTopRightBackColor"   valign="top" style="width: 20px; height: 22px;">
                                    <img class="imageRightBack" style="width: 7px; height: 22px" />
                                </td>
                            </tr>
                        </table>
                             </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="height: 1px; background-color: white">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="vertical-align: top; height: 360px; background-color: #e1e0b2;
                                text-align:center; width: 219px;">
                                <table style="text-align:center; height: 324px;">
                                <tr>
                                <td style="width: 280px; vertical-align: top; height: 220px; text-align: center;">
                                <asp:GridView ID="GrdUser" runat="server" AutoGenerateColumns="False" CssClass="gridview"
                                    Height="310px" HorizontalAlign="Left" OnRowDataBound="GrdUser_RowDataBound" BackColor="White"  CellPadding="3"   BorderStyle="Inset" BorderWidth="1px" Width="280px" PageSize="12" AllowPaging="True" OnPageIndexChanging="GrdUser_PageIndexChanging">
                                    <Columns>
                                        <asp:BoundField DataField="UserID">
                                            <ItemStyle CssClass="hidden" />
                                            <HeaderStyle CssClass="hidden" />
                                            <FooterStyle CssClass="hidden" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="UserCode" HeaderText="<%$ Resources:BaseInfo,User_lblUserLoginCode %>" HeaderStyle-HorizontalAlign="Left">
                                            <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                            <ItemStyle BorderColor="#E1E0B2" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:BaseInfo,User_lblUserName %>" HeaderStyle-HorizontalAlign="Left">
                                            <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                            <ItemStyle BorderColor="#E1E0B2" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="WorkNo" HeaderText="ID" >
                                            <HeaderStyle CssClass="hidden" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                            <ItemStyle BorderColor="#E1E0B2" CssClass="hidden" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="UserStatus" HeaderText="<%$ Resources:BaseInfo,User_lblUserStatusStr %>" HeaderStyle-HorizontalAlign="Left" >
                                            <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                            <ItemStyle BorderColor="#E1E0B2" />
                                        </asp:BoundField>

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
                                                 Text="<%# ((GridView)Container.NamingContainer).PageCount %>"></asp:Label> 
                                                <asp:linkbutton id="btnGo" runat="server" causesvalidation="False" commandargument="-1" commandname="Page" text="GO"  Font-Size="Smaller"/> 
                                                  </PagerTemplate>         
                                                <PagerSettings Mode="NextPreviousFirstLast"  />

                                </asp:GridView>
                                </td>
                                </tr>
                                <tr>
                                                            <td class="tdBackColor"   style="width: 218px; height: 16px; text-align: center;">
                                </td>
                                </tr>
                                </table>
                                </td>
                        </tr>
                    </table>
                     </td>
                    <td style="width: 3%; height: 401px">
                    </td>
                       <td style="vertical-align: top; height: 400px; width:32%">
                        <table border="0" cellpadding="0" cellspacing="0" style="height: 402px; width:100%">
                        <tr>
                         <td    style="vertical-align:middle; height: 22px; width: 100%;" colspan="4" >
                         <table border="0" cellpadding="0" cellspacing="0" style="height: 22px; width: 100%;   ">
                            <tr>
                                <td class="tdTopRightBackColor"    valign="top" style="width: 8px; height:22px;  text-align:left" >
                                    <img alt="" class="imageLeftBack" style=" text-align:left; height: 22px;"  />
                                    </td>
                                    <td class="tdTopRightBackColor" style="height: 22px; text-align: left;">
                                <asp:Label
                                    ID="labUserDefine" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,AddUser_labUserDefine %>"></asp:Label></td>
                              
                                <td class="tdTopRightBackColor"   valign="top" style="width: 20px; height: 22px;">
                                    <img class="imageRightBack" style="width: 7px; height: 22px" />
                                    </td>
                            </tr>
                        </table>
                        </td>
                        
                        </tr>
                        <tr>
                            <td style="height: 10px; background-color: #e1e0b2; text-align: left" colspan="4">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 183px; height: 23px; background-color: #e1e0b2; text-align: left">
                                <asp:Label ID="lblUserCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,User_lblUserLoginCode %>"
                                    Width="84px"></asp:Label></td>
                            <td style="width: 5px; height: 23px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 114px; height: 23px; background-color: #e1e0b2">
                                <asp:TextBox ID="txtUserCode" runat="server" CssClass="ipt160px" Width="110px" MaxLength="16"></asp:TextBox></td>
                            <td style="width: 42px; height: 23px; background-color: #e1e0b2">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 183px; height: 23px; background-color: #e1e0b2">
                                <asp:Label ID="lblUserName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,User_lblUserName %>"
                                    Width="84px"></asp:Label></td>
                            <td style="width: 5px; height: 23px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 114px; height: 23px; background-color: #e1e0b2">
                                <asp:TextBox ID="txtUserName" runat="server" CssClass="ipt160px" Width="110px" MaxLength="32"></asp:TextBox></td>
                            <td style="width: 42px; height: 23px; background-color: #e1e0b2">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 183px; height: 22px; background-color: #e1e0b2">
                                <asp:Label ID="lblPassword" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,User_lblPassword %>"
                                    Width="84px"></asp:Label></td>
                            <td style="width: 5px; height: 22px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 114px; height: 23px; background-color: #e1e0b2">
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="ipt160px" TextMode="Password"
                                    Width="110px" MaxLength="16"></asp:TextBox></td>
                            <td style="width: 42px; height: 22px; background-color: #e1e0b2">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 183px; height: 25px; background-color: #e1e0b2">
                                <asp:Label ID="Label12" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,User_lblWorkNo %>"
                                    Width="80px"></asp:Label></td>
                            <td style="width: 5px; height: 25px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 114px; height: 23px; background-color: #e1e0b2">
                                <asp:TextBox ID="txtWorkNo" runat="server" CssClass="ipt160px" Width="110px" MaxLength="16"></asp:TextBox></td>
                            <td style="width: 42px; height: 25px; background-color: #e1e0b2">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 183px; height: 22px; background-color: #e1e0b2">
                                <asp:Label ID="lblMobile1" runat="server" CssClass="labelStyle"
                                    Text="<%$ Resources:BaseInfo,User_lblMobile1 %>" Width="84px"></asp:Label></td>
                            <td style="width: 5px; height: 22px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 114px; height: 23px; background-color: #e1e0b2">
                                <asp:TextBox ID="txtMobile1" runat="server" CssClass="ipt160px" Width="110px" MaxLength="16"></asp:TextBox></td>
                            <td style="width: 42px; height: 22px; background-color: #e1e0b2">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 183px; height: 22px; background-color: #e1e0b2">
                                <asp:Label ID="lblOfficeTel" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,User_lblOfficeTel %>"
                                    Width="84px"></asp:Label></td>
                            <td style="width: 5px; height: 22px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 114px; height: 23px; background-color: #e1e0b2">
                                <asp:TextBox ID="txtOfficeTel" runat="server" CssClass="ipt160px" Width="110px" MaxLength="16"></asp:TextBox></td>
                            <td style="width: 42px; height: 22px; background-color: #e1e0b2">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 183px; height: 22px; background-color: #e1e0b2">
                                <asp:Label ID="lblEmail" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,User_lblEMail %>"
                                    Width="84px"></asp:Label></td>
                            <td style="width: 5px; height: 22px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 114px; height: 23px; background-color: #e1e0b2">
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="ipt160px" Width="110px" MaxLength="128"></asp:TextBox></td>
                            <td style="width: 42px; height: 22px; background-color: #e1e0b2">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 183px; height: 22px; background-color: #e1e0b2">
                                <asp:Label ID="lblUserStatus" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,User_lblUserStatus %>"
                                    Width="84px"></asp:Label></td>
                            <td style="width: 5px; height: 22px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 114px; height: 23px; background-color: #e1e0b2">
                                <asp:DropDownList ID="cmbUserState" runat="server" CssClass="ipt160px" Width="110px">
                                </asp:DropDownList></td>
                            <td style="width: 42px; height: 22px; background-color: #e1e0b2">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 183px; height: 25px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 5px; height: 25px; background-color: #e1e0b2">
                            </td>
                            <td rowspan="3" style="width: 114px; background-color: #e1e0b2">
                                <asp:ListBox ID="lstRoleName" runat="server" CssClass="list" Height="90px" SelectionMode="Multiple"
                                    Width="110px"></asp:ListBox></td>
                            <td style="width: 42px; height: 25px; background-color: #e1e0b2">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 183px; height: 25px; background-color: #e1e0b2">
                                <asp:Label ID="lblRoleName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Role_lblRoleName %>"
                                    Width="84px"></asp:Label></td>
                            <td style="width: 5px; height: 25px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 42px; height: 25px; background-color: #e1e0b2">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 183px; height: 22px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 5px; height: 22px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 42px; height: 22px; background-color: #e1e0b2">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="width: 280px; height: 7px; background-color: #e1e0b2; text-align: center"
                                valign="middle"><table border="0" cellpadding="0" cellspacing="0" style="width: 201px; text-align: center;">
                                    <tr>
                                        <td style="width: 240px; height: 1px; background-color: #738495;">
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
                            <td align="right" colspan="4" style="height: 30px; background-color: #e1e0b2">
                                <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" OnClick="btnSave_Click" p
                                    Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                                <asp:Button ID="treeClick" runat="server" CssClass="buttonHidden" OnClick="treeClick_Click" Width="12px" />&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                        </tr>
                    </table>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
        <asp:HiddenField ID="hidSelect" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidSelect %>"/>
                <asp:HiddenField ID="hidUpdate" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidUpdate %>" />
             <asp:HiddenField ID="hidAdd" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidAdd %>" />  
        <asp:HiddenField ID="hidInsert" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidInsert %>" />
            <asp:HiddenField id="depttxt" runat="server"></asp:HiddenField>
            <asp:HiddenField id="deptid" runat="server" ></asp:HiddenField>
            <asp:HiddenField id="selectdeptid" runat="server"></asp:HiddenField>
            <asp:HiddenField ID="hidMessage" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidMessage %>" />
        <asp:HiddenField ID="hidUserCodeBeing" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidUserCodeBeing %>" />
                                <asp:Label
                                                ID="lblCurrent" runat="server" ForeColor="Red" Height="9px" Width="0px">1</asp:Label><asp:Label
                                                    ID="lblTotalNum" runat="server" Height="9px"></asp:Label>
    </form>
</body>
</html>





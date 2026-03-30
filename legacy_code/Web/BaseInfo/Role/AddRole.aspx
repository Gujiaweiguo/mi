<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddRole.aspx.cs" Inherits="BaseInfo_Role_AddRole"  %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Menu_NewRole")%></title>
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
function rolecodebeing()
{
    parent.document.all.txtWroMessage.value =document.getElementById("hidRoleCodeBeing").value;
    //treearray();
}
var tabbar ;
function treearray()
{
     var t = new NlsTree('MyTree');
    var treestr =document.getElementById("depttxt").value;
     var nodeid="";
    var treearr = new Array();
    var n=0;
    var id;
    var pid;
    var name;
    var chkstrtus;
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
                chkstrtus=treenode[3];
            }
            if(chkstrtus==undefined)
            {
                t.add(id, pid, name, "", "", true,false);
            }
            if(chkstrtus==0)
            {
                t.add(id, pid, name, "", "", true,false);
            }
            else if(chkstrtus==1)
            {
                t.add(id, pid, name, "", "", true,true);
                nodeid+=id+ ',';
            }
        }
    }
            document.form1.deptid.value = nodeid;
            t.opt.sort='no';
            t.opt.enbScroll=true;
            t.opt.height="330px";
            t.opt.width="220px";
            t.opt.trg="mainFrame";
            t.opt.oneExp=true;
            t.opt.check = true;
            t.opt.checkIncSub = true; //default is true.
            t.opt.checkParents = true; //default is false.
            t.opt.checkOnLeaf = true; //default is false
            
            t.treeOnCheck = enableCheckbox;
            t.render("treeview");    
            t.collapseAll();
            //document.getElementById("lblTotalNum").style.display="none";
            //document.getElementById("lblCurrent").style.display="none";
            addTabTool("null");
	    loadTitle();
}

function enableCheckbox(id,v)
{
    var str;
    str=document.form1.deptid.value;
    if(str.indexOf(id+',',0)!=-1)
    {
        document.form1.deptid.value =str.replace(id+',',"");
    }
    else
    {
        str+=id+',';
        document.form1.deptid.value =str;
    }   
} 

    //text控件文本验证
    function allTextBoxValidator(sForm)
    {
        if(isEmpty(document.all.txtRoleCode.value))  
        {
            parent.document.all.txtWroMessage.value=document.getElementById("hidMessage").value;
            document.all.txtRoleCode.focus();
            return false;					
        }
        
        if(isEmpty(document.all.txtRoleName.value))  
        {
            parent.document.all.txtWroMessage.value=document.getElementById("hidMessage").value;
            document.all.txtRoleName.focus();
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
function ShowTitle()
{
    parent.document.all.txtWroMessage.value = "";
    addTabTool("<%=baseInfo %>,BaseInfo/Role/AddRole.aspx");
    loadTitle();
}
		-->
</script>

</head>
<body onload='treearray();ShowTitle();' topmargin=0 leftmargin=0>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:HiddenField id="depttxt" runat="server"></asp:HiddenField>
            <asp:HiddenField id="deptid" runat="server" ></asp:HiddenField>
                <asp:HiddenField ID="titletext" runat="server" Value="<%$ Resources:BaseInfo,Menu_NewRole %>" />
        <table border="0" cellpadding="0" cellspacing="0" style="height: 402px; width:100%">
            <tr>
                <td style="width:30%; height: 402px; vertical-align: top;">
                    <table border="0" cellpadding="0" cellspacing="0" style="height: 95%; width:100%;">
                        <tr>
                            <td style="vertical-align:top; height: 22px;" >
                            
                         <table border="0" cellpadding="0" cellspacing="0" style="height: 22px; width: 100%;   ">
                        
                        
                            <tr>
                                <td class="tdTopRightBackColor"    valign="top" style="width: 8px; height:27px;  text-align:left" >
                                    <img alt="" class="imageLeftBack" style=" text-align:left"  />
                                    </td>
                                    <td class="tdTopRightBackColor" style="height: 27px; text-align:left;">
                                <asp:Label ID="labRoleTitle" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,AddRole_labRoleTitle %>"></asp:Label></td>
                              
                                <td class="tdTopRightBackColor"   valign="top" style="width: 20px; height: 27px;">
                                    <img class="imageRightBack" style="width: 7px; height: 22px" />
                                    </td>
                            </tr>
                        
                        </table>
                                </td>
                            
                        </tr>
                        <tr height="1">
                            <td style="height: 1px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" rowspan="10" style="height: 341px; text-align: center; width: 240px;" valign="top">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 240px; height: 370px">
                                <tr>
                                <td style="height:5px">
                                </td>
                                </tr>
                                    <tr>
                                        <td colspan="4" rowspan="9" style="height: 206px; text-align: center; width:240px; " valign="top">
                                        <table style="width:240px;">
                                        <tr>
                                        <td  align="center">
                                    <asp:GridView ID="GrdRole" runat="server" AutoGenerateColumns="False" OnRowDataBound="GrdRole_RowDataBound" Width="250px" BackColor="White"  CellPadding="3"  BorderStyle="Inset" BorderWidth="1px" OnSelectedIndexChanged="GrdRole_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="GrdRole_PageIndexChanging" PageSize="13">
                                        <Columns>
                                            <asp:BoundField FooterText="RoleID" HeaderText="RoleID" DataField="RoleID">
                                                <ItemStyle CssClass="hidden" />
                                                <HeaderStyle CssClass="hidden" />
                                                <FooterStyle CssClass="hidden" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="RoleCode" HeaderText="<%$ Resources:BaseInfo,User_lblUserCode %>">
                                                <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                <ItemStyle BackColor="White" BorderColor="#E1E0B2" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="RoleName" HeaderText="<%$ Resources:BaseInfo,Role_lblRoleName %>" >
                                                <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                <ItemStyle BorderColor="#E1E0B2" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="RoleStatus" HeaderText="<%$ Resources:BaseInfo,User_lblUserStatusStr %>">
                                                <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                <ItemStyle BorderColor="#E1E0B2" />
                                            </asp:BoundField>
                                            <asp:CommandField SelectText="<%$ Resources:BaseInfo,Btn_Edit %>" ShowSelectButton="True">
                                                <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
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
                                        </table>
                                            </td>
                                  </tr>
                             
                                </table>
                                </td>
                        </tr>
                   
                    </table>
                </td>
                <td style="width: 2%; height: 402px">
                </td>
                <td colspan="3" style="height: 402px; width:60%; vertical-align:top;">
                    <table border="0" cellpadding="0" cellspacing="0" style="height: 390px; width: 100%; ">
                        <tr>
                            <td style="vertical-align:top; height: 22px;" >
                            
                         <table border="0" cellpadding="0" cellspacing="0" style="height: 22px; width: 100%;   ">
                        
                        
                            <tr>
                                <td class="tdTopRightBackColor"    valign="top" style="width: 8px; height:27px;  text-align:left" >
                                    <img alt="" class="imageLeftBack" style=" text-align:left"  />
                                    </td>
                                    <td class="tdTopRightBackColor" style="height: 27px; text-align:left;">
                                <asp:Label ID="Label4" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,AddRole_labRoleFunctionVindicate %>"></asp:Label></td>
                              
                                <td class="tdTopRightBackColor"   valign="top" style="width: 20px; height: 27px;">
                                    <img class="imageRightBack" style="width: 7px; height: 22px" />
                                    </td>
                            </tr>
                        
                        </table>
                                </td>
                        </tr>
                        <tr height="1">
                            <td colspan="4" style="height: 1px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" rowspan="10" style="height: 341px; text-align: center"
                                valign="top">
                <table style="height: 370px">
                <tr>
                <td style="width: 173px; height: 344px; vertical-align: top;">
                         <table border="0" cellpadding="0" cellspacing="0" style="width: 170px; height: 342px">
                         <tr>
                         <td style="height:10px">
                         
                         </td>
                         </tr>
                        <tr>
                        
                            <td colspan="2" style="vertical-align: top; height: 350px; background-color: #e1e0b2;
                                text-align: center">
                                <asp:Panel ID="Panel1" runat="server" BackColor="White" BorderStyle="Inset" BorderWidth="1px"
                                    Height="340px" ScrollBars="Auto" Width="230px">
                                    <table>
                                        <tr>
                                            <td id="treeview" style="width: 130px; height: 303px" valign="top">
                                            </td>
                                        </tr>
                                    </table>
                                    </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="height: 344px; width: 265px; vertical-align:top;">
                          <table border="0" cellpadding="0" cellspacing="0" style="height: 342px; width: 217px;">
                        <tr>
                            <td class="tdBackColor" colspan="4" rowspan="10" style="height: 341px; text-align: center; vertical-align:top;"
                               >
                                <span style="font-size: 13pt">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 208px">
                                    <tr>
                                        <td colspan="3" style="height:25px;">
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 201px">
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
                                        <td style="width: 61px; height: 30px; text-align: right">
                                            <asp:Label ID="lblRoleCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Role_lblRoleCode %>"
                                                Width="66px" Font-Size="11pt"></asp:Label></td>
                                        <td style="height: 30px; width: 8px;">
                                           </td>
                                        <td style="height: 30px; width: 154px;">
                                            <asp:TextBox ID="txtRoleCode" runat="server" CssClass="Enabletextstyle" ReadOnly="True" MaxLength="16"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 61px; height: 30px; text-align: right">
                                            <asp:Label ID="lblRoleName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Role_lblRoleName %>"
                                                Width="64px" Font-Size="11pt"></asp:Label></td>
                                        <td style="height: 30px; width: 8px;">
                                           </td>
                                        <td style="height: 30px; width: 154px;">
                        <asp:TextBox ID="txtRoleName" runat="server" CssClass="Enabletextstyle" ReadOnly="True" MaxLength="16"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 61px; height: 30px; text-align: right">
                                           <asp:Label ID="lblRoleStatus" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Role_lblRoleStatus %>"
                                                Width="65px" Font-Size="11pt"></asp:Label></td>
                                        <td style="height: 30px; width: 8px;">
                                            </td>
                                        <td style="height: 30px; width: 154px;">
                        <asp:DropDownList ID="cmbRoleStatus" runat="server" CssClass="textstyle" Enabled="False">
                        </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 61px; height: 30px; text-align: right">
                                            <asp:Label ID="lblLeader" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Role_lblLeader %>"
                                                Width="64px" Font-Size="11pt"></asp:Label></td>
                                        <td style="height: 30px; width: 8px;">
                                            </td>
                                        <td style="height: 30px; width: 154px;">
                        <asp:DropDownList ID="cmbLeader" runat="server" CssClass="textstyle" Enabled="False">
                        </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" rowspan="2" style="height: 15px"><table border="0" cellpadding="0" cellspacing="0" style="width: 201px">
                                            <tr>
                                                <td style="width: 160px; height: 1px; background-color: #738495; position: relative; top: 15px;">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 160px; height: 1px; background-color: #ffffff; position: relative; top: 15px;">
                                                </td>
                                            </tr>
                                        </table>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                    </tr>
                                    <tr>
                                        <td colspan="4" rowspan="2" style="height: 17px; text-align:right; position: relative; top: 15px;">
                                            <asp:Button ID="btnAdd" runat="server" CssClass="buttonAdd" OnClick="btnAdd_Click"
                                                Text="<%$ Resources:BaseInfo,DeptTree_labDeptAdd %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                                            <asp:Button ID="btnSave"
                                                        runat="server" CssClass="buttonSave"  Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" OnClick="btnSave_Click" Enabled="False" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                                                <asp:Button ID="BtnSub" runat="server" CssClass="buttonSub" OnClick="BtnSubAuth_Click"
                                    Text="<%$ Resources:BaseInfo,Role_lblSubAuth %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>&nbsp;&nbsp;&nbsp;
                                                </td>
                                    </tr>
                                    <tr>
                                    </tr>
                                </table>
                                    </span>
                                </td>
                        </tr>
                    </table>
                    <asp:Label ID="lblTotalNum" runat="server" Height="9px" Width="62px" CssClass="hidden"></asp:Label>
                                            <asp:Label ID="lblCurrent" runat="server" ForeColor="Red"
                                                        Height="9px" Width="1px" Visible="False">1</asp:Label></td>
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
        <asp:HiddenField ID="hidSelect" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidSelect %>" />
        <asp:HiddenField ID="hidUpdate" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidUpdate %>" />
        <asp:HiddenField ID="hidAdd" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidAdd %>" />
        <asp:HiddenField ID="hidUpdateLost" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidUpdateLost %>" />
        <asp:HiddenField ID="hidInsert" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidInsert %>" />
        <asp:HiddenField ID="hidMessage" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidMessage %>" />
        <asp:HiddenField ID="hidRoleCodeBeing" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidRoleCodeBeing %>" />
    </form>
</body>
</html>





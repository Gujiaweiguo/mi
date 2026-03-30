<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StoreBrand.aspx.cs" Inherits="BaseInfo_Role_AddRole"  %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Store_ItemAttentionVariety")%></title>
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link rel="StyleSheet" href="../../App_Themes/nlstree/nlstree-basic.css" type="text/css" />
    <link rel="StyleSheet" href="../../App_Themes/nlstree/nlsctxmenu.css" type="text/css" />
    <script type="text/javascript" src="../../App_Themes/nlstree/nlstree.js"></script>
    <script type="text/javascript" src="../../App_Themes/nlstree/nlsctxmenu.js"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"></script>
	<script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
<script type="text/javascript">

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
            t.opt.height="320px";
            t.opt.width="100%";
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
    addTabTool("<%=strrefresh %>,BaseInfo/Store/StoreBrand.aspx");
    loadTitle();
}
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
                <asp:HiddenField ID="titletext" runat="server" Value="<%$ Resources:BaseInfo,Role_lblFunc %>" />
        <table border="0" cellpadding="0" cellspacing="0" style="height: 430px; width:100%;">
            <tr>
                <td style="width:45%;left:60px; height: 402px; position:relative; vertical-align: top;" align="center">
                    <table border="0" cellpadding="0" cellspacing="0" style="height: 370px;  text-align:center; width:100%">
                        <tr style="text-align:center">
                            <td style="vertical-align:top; height: 22px;" >
                                     <table border="0" cellpadding="0" cellspacing="0" style="height: 22px; width: 100%;   ">
                                        <tr>
                                            <td class="tdTopRightBackColor"    valign="top" style="width: 8px; height:27px;  text-align:left" >
                                                <img alt="" class="imageLeftBack" style=" text-align:left"  />
                                                </td>
                                                <td class="tdTopRightBackColor" style="height: 27px; text-align:left;">
                                            <asp:Label ID="labRoleTitle" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,Store_BusinessItemList%>"></asp:Label></td>
                                          
                                            <td class="tdTopRightBackColor"   valign="top" style="width: 20px; height: 27px;">
                                                <img class="imageRightBack" style="width: 7px; height: 22px" />
                                                </td>
                                        </tr>
                                    
                                    </table>
                                </td>
                            
                        </tr>
                        
                        <tr style="text-align:center; width:100%">
                            <td class="tdBackColor" align="center" colspan="4" rowspan="10" style="height: 400px; text-align: center; width: 100%;" valign="top">
                                <table border="0" cellpadding="0" cellspacing="0" style=" width:100%; height: 370px; text-align:center">
                               
                                    <tr>
                                        <td colspan="4" rowspan="9" align="center" style="height: 206px; text-align: center; " valign="top">
                                 <table style="text-align:center; width:100%">
                                <tr style="width:100%">
                                <td style="width: 100%;" align ="center">
                                    <asp:GridView ID="GrdRole" runat="server" AutoGenerateColumns="False" Height="294px" OnRowDataBound="GrdRole_RowDataBound" Width="85%" BackColor="White"  CellPadding="3"  BorderStyle="Inset" BorderWidth="1px" OnSelectedIndexChanged="GrdRole_SelectedIndexChanged" PageSize="14" AllowPaging="True" OnPageIndexChanging="GrdRole_PageIndexChanging">
                                        <Columns>
                                            <asp:BoundField FooterText="DeptID" HeaderText="DeptID" DataField="DeptID">
                                                <ItemStyle CssClass="hidden" />
                                                <HeaderStyle CssClass="hidden" />
                                                <FooterStyle CssClass="hidden" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="DeptName" HeaderText="<%$ Resources:BaseInfo,Store_StoreName %>" >
                                                <HeaderStyle CssClass="gridviewtitle" HorizontalAlign="Left" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                <ItemStyle BorderColor="#E1E0B2" />
                                            </asp:BoundField>
                                            
                                            <asp:CommandField SelectText="<%$ Resources:BaseInfo,PotShop_Selected %>" HeaderText="<%$ Resources:BaseInfo,PotShop_Selected %>" ShowSelectButton="True">
                                                <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
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
                                        <tr>
                                            <td style=" height: 58px; width: 221px;">
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
                <td style="height: 402px; width:1%;">
                </td>
                <td colspan="3" style="height: 402px;  vertical-align:top; width:100%; text-align:right; right:100px;">
                    <table border="0" cellpadding="0" cellspacing="0" style="height: 350px; width: 80%; ">
                        <tr>
                            <td style="vertical-align:top; height: 22px; width:100%;" >
                            
                         <table border="0" cellpadding="0" cellspacing="0" style="height: 22px; width: 100%;   ">
                        
                        
                            <tr>
                                <td class="tdTopRightBackColor"    valign="top" style=" height:27px;  text-align:left; width: 6px;" >
                                    <img alt="" class="imageLeftBack" style=" text-align:left"  />
                                    </td>
                                    <td class="tdTopRightBackColor" style="height: 27px; text-align:left;">
                                <asp:Label ID="Label4" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,Store_AttentionVarietyList %>"></asp:Label></td>
                              
                                <td class="tdTopRightBackColor"   valign="top" style=" height: 27px;">
                                    <img class="imageRightBack" style="width: 7px; height: 22px" />
                                    </td>
                            </tr>
                        
                        </table>
                                </td>
                        </tr>
                        
                        <tr style="height: 360px; width:80%; text-align: center" >
                            <td class="tdBackColor" colspan="4" rowspan="10" style="height: 350px; width:80%; text-align: center"  valign="top">
                                <table style="height: 330px;width:100%; vertical-align:top; text-align:center;">
                                        <tr>
                                            <td style=" height: 330px; vertical-align: top; width:100%;" align="center">
                                                     <table border="0" cellpadding="0" cellspacing="0" style="height: 335px;width:100%;">
                                                         
                                                        <tr>
                                                        
                                                             <td colspan="2" style="vertical-align: top; height: 350px; background-color: #e1e0b2;
                                                                text-align: center" align="left">
                                                                <asp:Panel ID="Panel1" runat="server" BackColor="White" BorderStyle="Inset" BorderWidth="1px"
                                                                    Height="330px" ScrollBars="Auto" Width="70%">
                                                                    <table>
                                                                        <tr>
                                                                            <td id="treeview" style="width: 300px; height: 303px" valign="top">
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    </asp:Panel>
                                                            </td>
                                        </tr>
                                        <tr><td align="center" valign="top" style="height: 45px;width:100%;"><asp:Button ID="btnSave"  runat="server" CssClass="buttonSave"  Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" OnClick="btnSave_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);" Enabled="False"/>
                                            <asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel" OnClick="btnCancel_Click"
                                            Text="<%$ Resources:BaseInfo,User_btnCancel %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/> </td></tr>
                                        </table>
                                         
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
        <asp:HiddenField ID="hidSelect" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidSelect %>" />
        <asp:HiddenField ID="hidUpdate" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidUpdate %>" />
        <asp:HiddenField ID="hidAdd" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidAdd %>" />
        <asp:HiddenField ID="hidUpdateLost" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidUpdateLost %>" />
        <asp:HiddenField ID="hidInsert" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidInsert %>" />
        <asp:HiddenField ID="hidMessage" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidMessage %>" />
        <asp:HiddenField ID="hidRoleCodeBeing" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidRoleCodeBeing %>" />
        <asp:Label   ID="lblTotalNum" Visible="false" runat="server" Height="9px"></asp:Label><asp:Label ID="lblCurrent" Visible="false" runat="server" ForeColor="Red" Height="9px">1</asp:Label>
    </form>
</body>
</html>





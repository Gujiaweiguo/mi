<%@ Page Language="C#" AutoEventWireup="true"  Title="用户调动" CodeFile="UserTransfer.aspx.cs" Inherits="MI_Net.ChangUser" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "User_Transfer")%></title>
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
            t.opt.height="310px";
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
            
            //addTabTool("null");
	        //loadTitle();
	         Load();
}



function ev_click(e, id)
{
	
    document.form1.deptid.value=id;
    document.form1.treeClick.click();
     
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
    addTabTool("<%=baseInfo %>,BaseInfo/User/UserTransfer.aspx");
    loadTitle();
}
</script>

    
</head>
<body onload='treearray();' topmargin=0 leftmargin=0>
    <form id="form1" runat="server">
    <asp:ScriptManager id="ScriptManager1" runat="server"></asp:ScriptManager>
    <div id="show"   style="vertical-align:top;text-align:center;width:100%">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
            <asp:HiddenField id="depttxt" runat="server"></asp:HiddenField>
            <asp:HiddenField id="deptid" runat="server" ></asp:HiddenField>
            <asp:HiddenField ID="selectdeptid" runat="server" />
        <table border="0" cellpadding="0" cellspacing="0" style="height: 420px; width:100%; vertical-align:top">
            <tr align="center">                
                <td style="width: 40%; height: 100%; vertical-align: top;" align="center">
                    <table border="0" cellpadding="0" cellspacing="0" style="width:100%; height: 402px">
                        <tr>
                           <td    style="vertical-align:top; height: 22px;" >
                         <table border="0" cellpadding="0" cellspacing="0" style="height: 22px; width: 100%;   ">
                            <tr>
                                <td class="tdTopRightBackColor"    valign="top" style="width: 8px; height:22px;  text-align:left" >
                                    <img alt="" class="imageLeftBack" style=" text-align:left; height: 22px;"  />
                                    </td>
                                    <td class="tdTopRightBackColor" style="height: 22px; text-align:left">
                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo,User_List %>"></asp:Label></td>
                              
                                
                                <td class="tdTopRightBackColor"   valign="top" style="width: 20px; height: 22px;">
                                   <img class="imageRightBack" style="width: 7px; height: 22px" />
                                    </td>
                            </tr>
                        
                        </table>
                        
                        
                        </td>
                        </tr>
                        <tr >
                            <td  colspan="2" style="vertical-align: top; height: 370px; background-color: #e1e0b2;
                                text-align:center; ">
                                <table style="text-align:center;" width=100%>
                                <tr>
                                <td > 
                                    <asp:GridView ID="GrdUser" runat="server" AutoGenerateColumns="False" BackColor="White" BorderStyle="Inset" BorderWidth="1px" CellPadding="3" CssClass="gridview"
                                        Height="350px"  OnRowDataBound="GrdUser_RowDataBound"
                                         PageSize="13" OnSelectedIndexChanged="GrdUser_SelectedIndexChanged" Width="98%" AllowPaging="True" OnPageIndexChanging="GrdUser_PageIndexChanging" >
                                        <Columns>
                                            <asp:BoundField DataField="UserID">
                                                <ItemStyle CssClass="hidden" />
                                                <HeaderStyle CssClass="hidden" />
                                                <FooterStyle CssClass="hidden" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="UserCode" HeaderText="<%$ Resources:BaseInfo,User_lblUserCode %>">
                                                <HeaderStyle CssClass="hidden" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                <ItemStyle BorderColor="#E1E0B2" CssClass="hidden"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:BaseInfo,User_lblUserName %>">
                                                <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                <ItemStyle BorderColor="#E1E0B2" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="DeptID">
                                                <ItemStyle CssClass="hidden" />
                                                <HeaderStyle CssClass="hidden" />
                                                <FooterStyle CssClass="hidden" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="DeptName" HeaderText="<%$ Resources:BaseInfo,Dept_lblDeptName %>">
                                                <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                <ItemStyle BorderColor="#E1E0B2" />
                                            </asp:BoundField>
                          
                                            <asp:CommandField ShowSelectButton="True" HeaderText="<%$ Resources:BaseInfo,PotShop_Selected %>">
                                                 <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                 <ItemStyle BorderColor="#E1E0B2" />
                                            </asp:CommandField>
                                             <asp:BoundField DataField="pDeptName">
                                                <HeaderStyle CssClass="hidden" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                <ItemStyle CssClass="hidden" />
                                            </asp:BoundField>
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
                                                 Text="<%# ((GridView)Container.NamingContainer).PageCount %>"></asp:Label> 
                                                <asp:linkbutton id="btnGo" runat="server" causesvalidation="False" commandargument="-1" commandname="Page" text="GO" /> 
                                                  </PagerTemplate>         
                                                <PagerSettings Mode="NextPreviousFirstLast"  />
                                    </asp:GridView>
                                </td>
                                </tr>
                                <tr>
                                 <td class="tdBackColor"   style=" height: 21px; text-align: center;">
                                </td>
                                </tr>
                                </table>
                                </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 5%; height: 401px">
                </td>
                   <td style="width:33%; height: 401px; vertical-align: top;" align="center">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; ">
                        <tr>
                         <td   style="vertical-align:top; height: 22px; width: 100%;" >
                        
                         <table border="0" cellpadding="0" cellspacing="0" style="height: 22px; width:100%;   ">
                        
                        
                            <tr>
                                <td class="tdTopRightBackColor"    valign="top" style="width: 8px; height:22px;  text-align:left" >
                                    <img alt="" class="imageLeftBack" style=" text-align:left; height: 22px;"  />
                                    </td>
                                    <td class="tdTopRightBackColor" style="height: 22px; text-align: left;">
                                        <asp:Label ID="lblUser" runat="server" Text="<%$ Resources:BaseInfo,Transfer_Department %>"></asp:Label></td>
                              
                                <td class="tdTopRightBackColor"   valign="top" style="width: 20px; height: 22px;">
                                    <img class="imageRightBack" style="width: 7px; height: 22px" />
                                    </td>
                            </tr>
                        
                        </table>
                        
                        
                        </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="height: 350px; background-color: #e1e0b2;
                                 vertical-align:top;">
                                <table style="width:100%">
                      
                                <tr>
                                <td style="height: 338px; width: 100%;" valign="top">
                                                 <asp:Panel ID="Panel1" runat="server" BorderStyle="Inset" BorderWidth="1px"
                                    Height="320px" ScrollBars="Auto" Width="95%" BackColor="White">
                                
                                                                    <table>
                                        <tr>
                                    <td valign="top" id ="treeview" style="height: 300px; width: 200px;">
                                             
                                         
                                    </td>
                                        </tr>
                                    </table>
                                
                                
                                
                                </asp:Panel>
                                </td>
                                </tr>
                                </table>
                                <table style="text-align:left; height:30;width:100%; vertical-align:top;"><tr><td style="left: 15px; position: relative; height: 34px; top: -10px;">
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; &nbsp;
                                    <asp:Button ID="btnSave" runat="server" CssClass="buttonSave"   OnClick="btnSave_Click"
                                    Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);" Enabled="False"/>
                                <asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel" OnClick="btnCancel_Click"
                                    Text="<%$ Resources:BaseInfo,User_btnCancel %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>&nbsp;
                                    <asp:Button ID="treeClick" runat="server" CssClass="buttonHidden" OnClick="treeClick_Click" Width="1px" /></td></tr></table>
                            </td>
                        </tr>
                    </table>
                </td>    
            </tr>
        </table>
            </ContentTemplate>
        </asp:UpdatePanel>
                                    
        <asp:HiddenField ID="hidSelect" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidSelect %>" />
        &nbsp;
        <asp:HiddenField ID="hidUpdate" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidUpdate %>" />
        <asp:HiddenField ID="hidAdd" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidAdd %>" />
        <asp:HiddenField ID="hidUpdateLost" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidUpdateLost %>" />
        <asp:HiddenField ID="hidInsert" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidInsert %>" />
        <asp:HiddenField ID="hidMessage" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidMessage %>" />
        </div>
    </form>
</body>
</html>
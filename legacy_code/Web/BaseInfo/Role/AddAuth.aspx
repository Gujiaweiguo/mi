<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddAuth.aspx.cs" Inherits="BaseInfo_Role_AddAuth" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Munu_Auth")%></title>
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link rel="StyleSheet" href="../../App_Themes/nlstree/nlstree-basic.css" type="text/css" />
    <link rel="StyleSheet" href="../../App_Themes/nlstree/nlsctxmenu.css" type="text/css" />
    <script type="text/javascript" src="../../App_Themes/nlstree/nlstree.js"></script>
    <script type="text/javascript" src="../../App_Themes/nlstree/nlsctxmenu.js"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"></script>
	<script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
	<style type="text/css">
    #a{position:relative;}

    #b{
        position:absolute; left: 10px; top: 30px;
    }
    .store{
        background-color:#D6D6DE; text-align:left; width:150px; 
        clear: none; border-right: darkturquoise 1px solid; 
        border-top: darkturquoise 1px solid;
        border-left: darkturquoise 1px solid;
        border-bottom: darkturquoise 1px solid;
        right: 12px; top: 90px;
        position:absolute;
    }
     .building{
        background-color:#D6D6DE; text-align:left; width:150px; 
        clear: none; border-right: darkturquoise 1px solid; 
        border-top: darkturquoise 1px solid;
        border-left: darkturquoise 1px solid;
        border-bottom: darkturquoise 1px solid;
        right: 12px; top: 145px;
        position:absolute;
    }
     .floor{
        background-color:#D6D6DE; text-align:left; width:180px; 
        clear: none; border-right: darkturquoise 1px solid; 
        border-top: darkturquoise 1px solid;
        border-left: darkturquoise 1px solid;
        border-bottom: darkturquoise 1px solid;
        right: -12px; top: 192px;
        position:absolute;
    }
    </style>
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
            t.opt.height="325px";
            t.opt.width="170px";
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
        addTabTool("<%=baseInfo %>,BaseInfo/Role/AddAuth.aspx");
        loadTitle();
    }
    function SelectStore()
    {
        document.form1.btnStore.click(); 
    }

    function SelectBuilding()
    {
        document.form1.btnBuilding.click(); 
    }
    function SelectFloor()
    {
        document.form1.btnFloor.click();
    }
    
   
    function Disp( obj )
    {
	    document.getElementById( obj ).style.display = 'block';
    }
    function Hide( obj )
    {
	    document.getElementById( obj ).style.display = 'none';
    }

	-->
</script>
</head>
<body onload='treearray();showline();' topmargin="0" leftmargin="0">
    <form id="form1" runat="server">
    <asp:ScriptManager id="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
            <asp:HiddenField id="depttxt" runat="server"></asp:HiddenField>
            <asp:HiddenField id="deptid" runat="server" ></asp:HiddenField>
            <asp:HiddenField ID="selectdeptid" runat="server" />
        <table border="0" cellpadding="0" cellspacing="0" style="height: 420px; width:100%;vertical-align:top;text-align:center;">
            <tr>
                  <td style="width:33%; height: 401px; vertical-align: top;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width:100%;height: 402px;">
                        <tr>
                         <td    style="vertical-align:top; height: 22px; width: 100%;">
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
                            <td style="height: 360px; background-color: #e1e0b2;
                                 vertical-align:top;">
                                <table style="text-align:left;">
                                <tr>
                                <td style="height: 350px">
                                   <asp:Panel ID="Panel1" runat="server" BorderStyle="Inset" BorderWidth="1px"
                                    Height="335px" ScrollBars="Auto" Width="180px" BackColor="White">
                                  <table>
                                        <tr>
                                    <td valign="top" id ="treeview" style="height: 325px; width: 159px;">
                                    </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                </td>
                                </tr>
                                </table>
                            <asp:Label ID="lblTotalNum" runat="server" Height="9px"></asp:Label>
                        <asp:Label ID="lblCurrent" runat="server" ForeColor="Red" Height="9px">1</asp:Label>
                        <asp:Button ID="treeClick" runat="server" CssClass="buttonHidden" OnClick="treeClick_Click" Width="1px" /></td>
                        </tr>
                    </table>
                </td>
                <td style="width:4%; height: 401px">
                </td>
                <td style="width: 33%; height: 100%; vertical-align: top;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width:100%; height: 402px; text-align:center;">
                        <tr>
                            <td  style="vertical-align:top; height: 22px;" >
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 22px">
                                    <tr>
                                        <td class="tdTopRightBackColor" style="width: 8px; height: 22px; text-align: left"
                                            valign="top">
                                            <img alt="" class="imageLeftBack" style="height: 22px; text-align: left" />
                                        </td>
                                        <td class="tdTopRightBackColor" style="height: 22px; text-align: left">
                                            <asp:Label ID="Label1" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,AddUser_labUserInfo %>"
                                                Width="151px"></asp:Label></td>
                                        <td class="tdTopRightBackColor" style="width: 20px; height: 22px" valign="top">
                                            <img class="imageRightBack" style="width: 7px; height: 22px" />
                                        </td>
                                    </tr>
                                </table>
                        </td>
                        </tr>
                        <tr style="text-align:center;">
                            <td style="vertical-align: top; height: 376px; background-color: #e1e0b2;
                                text-align:center;">
                                <table style="text-align:center;" id="bb">
                                <tr style="text-align:center">
                                <td style="text-align: center" align="center">
                                    <asp:GridView ID="GrdUser" runat="server" AutoGenerateColumns="False" BackColor="White" BorderStyle="Inset" BorderWidth="1px" CellPadding="3" CssClass="gridview"
                                        Height="330px"  OnRowDataBound="GrdUser_RowDataBound"
                                        OnSelectedIndexChanged="GrdUser_SelectedIndexChanged" PageSize="13" Width="280px" AllowPaging="True" OnPageIndexChanging="GrdUser_PageIndexChanging">
                                        <Columns>
                                            <asp:BoundField DataField="UserID">
                                                <ItemStyle CssClass="hidden" />
                                                <HeaderStyle CssClass="hidden" />
                                                <FooterStyle CssClass="hidden" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="UserCode" HeaderText="<%$ Resources:BaseInfo,User_lblUserCode %>">
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
                                            <asp:CommandField ShowSelectButton="True" >
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
                                                 Text="<%# ((GridView)Container.NamingContainer).PageCount %>"></asp:Label> 
                                                <asp:linkbutton id="btnGo" runat="server" causesvalidation="False" commandargument="-1" commandname="Page" text="GO" /> 
                                                  </PagerTemplate>         
                                                <PagerSettings Mode="NextPreviousFirstLast"  />
                                    </asp:GridView>
                                </td>
                                </tr>
                                <tr>
                                     <td class="tdBackColor"   style="width: 100%; height: 19px; text-align: center;">
                                </td>
                                </tr>
                                </table>
                                
                                
                                </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 4%; height: 401px">
                </td>
                <td style="width: 32%; vertical-align: top;">
                    <table border="0" cellpadding="0" cellspacing="0" style="height: 406px; width: 100%; vertical-align: top;" id="aa">
                        <tr>
                             <td style="vertical-align:top; height:22px; width: 100%;" colspan="4" >
                                 <table border="0" cellpadding="0" cellspacing="0" style="height: 22px; width: 100%; ">
                                    <tr >
                                        <td class="tdTopRightBackColor"    valign="top" style="width: 8px; height:22px;  text-align:left" >
                                            <img alt="" class="imageLeftBack" style=" text-align:left; height: 22px;"  />
                                            </td>
                                            <td class="tdTopRightBackColor" style="height: 22px; text-align: left;">
                                        <asp:Label
                                            ID="labUserDefine" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,AddAuth_AuthDef %>"></asp:Label></td>
                                        <td class="tdTopRightBackColor"   valign="top" style="width: 20px; height: 22px;">
                                            <img class="imageRightBack" style="width: 7px; height: 22px" />
                                            </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                         <tr >
                            <td style="height: 30px; background-color: #e1e0b2; text-align: left;" colspan="4"><asp:RadioButton ID="rdoFloor" runat="server" Width="60px" Text="<%$ Resources:BaseInfo,AddAuth_rdoFloor %>" Checked="True" CssClass="labelStyle" GroupName="select" AutoPostBack="True" OnCheckedChanged="rdoFloor_CheckedChanged" /></td>
                        </tr>
                        <tr>
                            <td style="width: 82px; height: 25px; background-color: #e1e0b2; text-align: center;">
                                <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Store_StoreName %>"
                                    Width="50px"></asp:Label></td>
                            <td style="width: 5px; height: 25px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 114px; height: 25px; background-color: #e1e0b2;">
                                    <asp:TextBox ID="lblStoreID" onmouseover="SelectStore()" runat="server" CssClass="Enabletextstyle" ReadOnly="true"></asp:TextBox>
                                </td>
                            <td style="width: 42px; height: 25px; background-color: #e1e0b2" >
                                <table id="Store" style="background-color: #e1e0b2;" border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <table style="background-color: #e1e0b2;" border="0" cellpadding="0" cellspacing="0" onmouseout="Hide('PalStore');" onmouseover="Disp('PalStore');">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnStore" runat="server" CssClass="buttonHidden" OnClick="btnStore_Click" Width="1px" />
                                                        <asp:Panel ID="PalStore" runat="server" CssClass="store" Visible="False" 
                                                            onclick="SelectStore()" ScrollBars="Both" Height="200px" Width="200px">
                                                            <asp:CheckBoxList ID="cblist" runat="server" 
                                                                OnSelectedIndexChanged="cblist_SelectedIndexChanged" Width="250px">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 82px; height: 14px; background-color: #e1e0b2">
                                
                        </td>
                            <td style="width: 5px; height: 14px; background-color: #e1e0b2">
                            </td>
                            <td rowspan="1" style="width: 114px; background-color: #e1e0b2; height: 14px;">
                                &nbsp;</td>
                            <td style="width: 42px; background-color: #e1e0b2; height: 14px;">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 82px; height: 24px; background-color: #e1e0b2">
                                <asp:Label ID="Label8" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblBuildingName %>"
                                    Width="50px"></asp:Label></td>
                            <td style="width: 5px; height: 24px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 114px; height: 24px; background-color: #e1e0b2">
                                <asp:TextBox ID="lblBuildingID" runat="server" CssClass="Enabletextstyle" onclick="SelectBuilding()" ReadOnly="true"></asp:TextBox></td>
                            <td style="width: 42px; height: 24px; background-color: #e1e0b2">
                                <table id="Building" style="background-color: #e1e0b2;" border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <table style="background-color: #e1e0b2;" border="0" cellpadding="0" cellspacing="0" onmouseout="Hide('PalBuilding');" onmouseover="Disp('PalBuilding');">
                                                <tr>
                                                    <td>
                                                         <asp:Button ID="btnBuilding" runat="server" CssClass="buttonHidden" OnClick="btnBuilding_Click" Width="1px" />
                                                        <asp:Panel ID="PalBuilding" runat="server"  CssClass="building" Visible="False" 
                                                             onclick="SelectBuilding()" ScrollBars="Both" Height="200px" Width="200px">
                                                                <asp:CheckBoxList ID="ckbBuilding" runat="server" Width="250px" 
                                                                    OnSelectedIndexChanged="ckbBuilding_SelectedIndexChanged"></asp:CheckBoxList>&nbsp;</asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 82px; height: 15px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 5px; height: 14px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 114px; height: 14px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 42px; height: 14px; background-color: #e1e0b2">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 82px; height: 24px; background-color: #e1e0b2">
                                <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblFloorName %>"
                                    Width="50px"></asp:Label></td>
                            <td style="width: 5px; height: 24px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 114px; height: 24px; background-color: #e1e0b2">
                                <asp:TextBox ID="lblFloor" runat="server" CssClass="Enabletextstyle" onclick="SelectFloor()" ReadOnly="true"></asp:TextBox></td>
                            <td style="width: 42px; height: 24px; background-color: #e1e0b2"><asp:Button ID="btnFloor" runat="server" CssClass="buttonHidden" OnClick="btnFloor_Click" Width="1px" />
                                <table id="Floor" style="background-color: #e1e0b2;" border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <table style="background-color: #e1e0b2;" border="0" cellpadding="0" cellspacing="0" onmouseout="Hide('PalFloor');" onmouseover="Disp('PalFloor');">
                                                <tr>
                                                    <td>
                                                         <asp:Panel ID="PalFloor" runat="server" CssClass="floor" Visible="False" 
                                                             onclick="SelectFloor()" ScrollBars="Both" Height="200px" Width="200px">
                                                            <asp:CheckBoxList ID="ckbFloor" runat="server" Width="250px" 
                                                                 OnSelectedIndexChanged="ckbFloor_SelectedIndexChanged">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                
                                
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 82px; height: 24px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 5px; height: 24px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 114px; height: 14px; background-color: #e1e0b2">
                                </td>
                            <td style="width: 42px; height: 24px; background-color: #e1e0b2">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 82px; height: 24px; background-color: #e1e0b2"><asp:RadioButton ID="rdoContract" runat="server" Width="60px" Text="<%$ Resources:BaseInfo,AddAuth_Contract %>" CssClass="labelStyle" GroupName="select" /></td>
                            <td style="width: 5px; height: 24px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 114px; height: 24px; background-color: #e1e0b2">
                                </td>
                            <td style="width: 42px; height: 24px; background-color: #e1e0b2">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 82px; height: 10px; background-color: #e1e0b2">
                        </td>
                            <td style="width: 5px; height: 10px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 114px; height: 10px; background-color: #e1e0b2">
                                <asp:RadioButton ID="rdoAll" runat="server" Text="<%$ Resources:BaseInfo,AddAuth_rdoAll %>"
                                    Width="46px" Checked="True" CssClass="labelStyle" GroupName="chk" /></td>
                            <td style="width: 42px; height: 10px; background-color: #e1e0b2">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 82px; height: 12px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 5px; height: 12px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 114px; height: 12px; background-color: #e1e0b2; vertical-align: top;">
                                <asp:RadioButton ID="rdoOnlyOne" runat="server" Text="<%$ Resources:BaseInfo,AddAuth_rdoOnlyOne %>" CssClass="labelStyle" GroupName="chk" Width="58px"/></td>
                            <td style="width: 42px; height: 12px; background-color: #e1e0b2">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 82px; height: 25px; background-color: #e1e0b2">
                        </td>
                            <td style="width: 5px; height: 25px; background-color: #e1e0b2">
                            </td>
                            <td rowspan="2" style="width: 114px; background-color: #e1e0b2; vertical-align: top;">
                                <asp:RadioButton ID="rdoOnlyDept" runat="server" Width="58px" Text="<%$ Resources:BaseInfo,AddAuth_rdoOnlyDept %>" CssClass="labelStyle" GroupName="chk"/></td>
                            <td style="width: 25px; height: 25px; background-color: #e1e0b2">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 82px; height: 25px; background-color: #e1e0b2">
                        </td>
                            <td style="width: 5px; height: 25px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 42px; height: 25px; background-color: #e1e0b2">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="width: 280px; height: 15px; background-color: #e1e0b2; text-align: center"
                                valign="middle">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 204px;text-align: center;">
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
                            <td align="right" colspan="4" style="height: 34px; vertical-align: top; background-color: #e1e0b2">
                                <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" OnClick="btnSave_Click"
                                    Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);" Enabled="False"/>
                                &nbsp;<asp:Button ID="btnCancel"
                                        runat="server" CssClass="buttonCancel" OnClick="btnCancel_Click"
                                        Text="<%$ Resources:BaseInfo,User_btnCancel %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                                &nbsp;&nbsp;</td>
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
    </form>
</body>
</html>

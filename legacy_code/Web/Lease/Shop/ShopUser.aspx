<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShopUser.aspx.cs" Inherits="Lease_Shop_ShopUser" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%=title %></title>
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link rel="StyleSheet" href="../../App_Themes/nlstree/nlstree-basic.css" type="text/css" />
    <link rel="StyleSheet" href="../../App_Themes/nlstree/nlsctxmenu.css" type="text/css" />
    <script type="text/javascript" src="../../App_Themes/nlstree/nlstree.js"></script>
    <script type="text/javascript" src="../../App_Themes/nlstree/nlsctxmenu.js"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"></script>
	<script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
	<script type="text/javascript" src="../../JavaScript/Calendar.js" charset="gb2312"></script>
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
            t.opt.width="230px";
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
            
        addTabTool("<%=strFresh %>,Lease/Shop/ShopUser.aspx");
	    loadTitle();
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
    	    function text()
	    {
	        var key=window.event.keyCode;
	        if ((key>=48&&key<=57)||(key==08)) 
            {
                window.event.returnValue =true;
            }
            else
            {
            window.event.returnValue =false;
            }
	    
	    }
		-->
</script>

    
</head>
<body onload='treearray();' topmargin=0 leftmargin=0>
    <form id="form1" runat="server">
    <asp:ScriptManager id="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
            <asp:HiddenField id="depttxt" runat="server"></asp:HiddenField>
            <asp:HiddenField id="deptid" runat="server" ></asp:HiddenField>
                <asp:HiddenField ID="selectdeptid" runat="server" />
        <table border="0" cellpadding="0" cellspacing="0" style="height: 420px; width:100%; vertical-align:top">
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
                                <asp:Label ID="labUserTree" runat="server" CssClass="lblTitle" Text='<%$ Resources:BaseInfo,TpUsr_ShopList %>'></asp:Label></td>
                              
                                <td class="tdTopRightBackColor"   valign="top" style="width: 20px; height: 22px;">
                                    <img class="imageRightBack" style="width: 7px; height: 22px" />
                                    </td>
                            </tr>
                        
                        </table>
                        
                        
                        </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="height: 360px; background-color: #e1e0b2;text-align:center;
                                 vertical-align:top;">
                                <table>
                      
                                <tr style="text-align:center">
                                <td style="height: 350px">
                                                 <asp:Panel ID="Panel1" runat="server" BorderStyle="Inset" BorderWidth="1px"
                                    Height="340px" ScrollBars="Auto" Width="240px" BackColor="White" HorizontalAlign="Left">
                                
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
               
                            <asp:Label
                                                    ID="lblTotalNum" runat="server" Height="9px"></asp:Label>
                                    <asp:Label
                                                ID="lblCurrent" runat="server" ForeColor="Red" Height="9px">1</asp:Label>
                                    <asp:Button ID="treeClick" runat="server" CssClass="buttonHidden" OnClick="treeClick_Click" Width="1px" />
                                <asp:HiddenField ID="hidnPassword" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>

                <td style="width:2%; height: 401px">
                </td>
                <td style="width: 35%; height: 100%; vertical-align: top;">
                    <table border="0" cellpadding="0" cellspacing="0" style=" width:100%; height: 402px">
                        <tr>
                           <td    style="vertical-align:top; height: 22px;" >
                         <table border="0" cellpadding="0" cellspacing="0" style="height: 22px; width: 100%;   ">
                            <tr>
                                <td class="tdTopRightBackColor"    valign="top" style="width: 8px; height:22px;  text-align:left" >
                                    <img alt="" class="imageLeftBack" style=" text-align:left; height: 22px;"  />
                                    </td>
                                    <td class="tdTopRightBackColor" style="height: 22px; width: 192px; text-align:left;">
                                <asp:Label
                                    ID="labUserInfo" runat="server" Text='<%$ Resources:BaseInfo,TpUsr_ShopUserList %>' Width="158px" CssClass="lblTitle"></asp:Label></td>
                              
                                <td class="tdTopRightBackColor"   valign="top" 
                                    style="height: 22px; text-align:right;" colspan="2">
                                    <img class="imageRightBack" style=" height: 22px" />
                                    </td>
                            </tr>
                        </table>
                        </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="vertical-align: top; height: 370px; background-color: #e1e0b2;
                                text-align:center; width:100%;">
                                <table style="text-align:center; width:100%">
                                <tr style="width:100%">
                                <td align="center" style="width:100%">
                                    <asp:GridView ID="GrdUser" runat="server" AutoGenerateColumns="False" BorderColor="#E1E0B2"
                                        BackColor="White" BorderStyle="Inset" BorderWidth="1px" CellPadding="3"
                                        Height="313px"  OnRowDataBound="GrdUser_RowDataBound"
                                        OnSelectedIndexChanged="GrdUser_SelectedIndexChanged" PageSize="13" 
                                        Width="98%" AllowPaging="True" onpageindexchanging="GrdUser_PageIndexChanging" >
                                        <Columns>
                                            <asp:BoundField DataField="TPUsrId">
                                                <ItemStyle CssClass="hidden" />
                                                <HeaderStyle CssClass="hidden" />
                                                <FooterStyle CssClass="hidden" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="TPUsrId" HeaderText="<%$ Resources:BaseInfo,User_lblWorkNo %>">
                                                <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                <ItemStyle BorderColor="#E1E0B2" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="TPUsrNm" HeaderText="<%$ Resources:BaseInfo,User_lblUserName %>">
                                                <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                <ItemStyle BorderColor="#E1E0B2" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="DateStart" HeaderText="<%$ Resources:BaseInfo,TpUse_lblBeginWorkDate %>">
                                                <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                <ItemStyle BorderColor="#E1E0B2" />
                                            </asp:BoundField>
                                            <asp:CommandField ShowSelectButton="True" HeaderText="<%$ Resources:BaseInfo,PotShop_Selected %>">
                                                <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
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
                                </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor"   style="width: 100%; height: 15px; text-align: center;">
                                    <table border="0" cellpadding="0" cellspacing="0" 
                                            style="width: 95%; text-align: center;">
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
                                <td style="left: 15px; position: relative; height: 34px; top: -8px;">
                                    &nbsp;</td>
                                </tr>
                                </table>
                                
                                
                                </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 2%; height: 401px">
                </td>
                      <td style="width: 38%; height: 401px; vertical-align: top;">
                    <table border="0" cellpadding="0" cellspacing="0" style="height: 402px; width: 100%;">
                        <tr>
                                          <td    style="vertical-align:top; height: 22px;" colspan="4" >
                        
                         <table border="0" cellpadding="0" cellspacing="0" style="height: 22px; width: 100%;   ">
                        
                        
                            <tr style="width:100%">
                                <td class="tdTopRightBackColor"    valign="top" style="width: 8px; height:22px;  text-align:left" >
                                    <img alt="" class="imageLeftBack" style=" text-align:left; height: 22px;"  />
                                    </td>
                                    <td class="tdTopRightBackColor" style="height: 22px; text-align: left;" 
                                    colspan="3">
                                <asp:Label
                                    ID="labUserDefine" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,TpUsr_ShopUserAdd %>"></asp:Label></td>
                              
                                <td class="tdTopRightBackColor"   valign="top" style="width: 20px; height: 22px;">
                                    <img class="imageRightBack" style="width: 7px; height: 22px" />
                                    </td>
                            </tr>
                        
                        </table>
                        
                        
                        </td>
                        </tr>
                         <tr>
                            <td style="height: 5px; background-color: #e1e0b2; text-align: left;" colspan="4">
                        </td>
                        </tr>
                        <tr>
                            <td style="width: 82px; height: 25px; background-color: #e1e0b2; text-align: left;">
                        <asp:Label ID="Label12" runat="server" Text="<%$ Resources:BaseInfo,User_lblWorkNo_Last %>"
                            Width="80px" CssClass="labelStyle"></asp:Label></td>
                            <td style="width: 5px; height: 25px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 114px; height: 25px; background-color: #e1e0b2;">
                        <asp:TextBox ID="txtWorkNo" runat="server" Width="110px" CssClass="ipt160px" MaxLength="2" onkeydown="text()"></asp:TextBox></td>
                            <td style="width: 42px; height: 25px; background-color: #e1e0b2">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 82px; height: 25px; background-color: #e1e0b2">
                            
                    <asp:Label id="lblUserName" runat="server" Text='<%$ Resources:BaseInfo,User_lblUserName %>' Width="84px" CssClass="labelStyle"></asp:Label></td>
                            <td style="width: 5px; height: 25px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 114px; height: 25px; background-color: #e1e0b2">
                        <asp:TextBox id="txtUserName" runat="server" Width="110px" CssClass="ipt160px" MaxLength="40"></asp:TextBox></td>
                            <td style="width: 42px; height: 25px; background-color: #e1e0b2">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 82px; height: 25px; background-color: #e1e0b2">
                        <asp:Label ID="lblPassword" runat="server" Text="<%$ Resources:BaseInfo,User_lblPassword %>"
                            Width="84px" CssClass="labelStyle"></asp:Label></td>
                            <td style="width: 5px; height: 25px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 114px; height: 25px; background-color: #e1e0b2">
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="110px" CssClass="ipt160px" MaxLength="4"></asp:TextBox></td>
                            <td style="width: 42px; height: 25px; background-color: #e1e0b2">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 82px; height: 25px; background-color: #e1e0b2">
                                <asp:Label ID="Label1" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,User_lblIdentity %>"
                                    Width="84px"></asp:Label></td>
                            <td style="width: 5px; height: 25px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 114px; height: 25px; background-color: #e1e0b2">
                                <asp:TextBox ID="txtID" runat="server" CssClass="ipt160px" MaxLength="18" Width="110px"></asp:TextBox></td>
                            <td style="width: 42px; height: 25px; background-color: #e1e0b2">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 82px; height: 22px; background-color: #e1e0b2">
                        <asp:Label ID="lblMobile1" runat="server" Text="<%$ Resources:BaseInfo,User_lblMobile1 %>"
                            Width="84px" CssClass="labelStyle"></asp:Label></td>
                            <td style="width: 5px; height: 22px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 114px; height: 25px; background-color: #e1e0b2">
                        <asp:TextBox ID="txtMobile" runat="server" Width="110px" CssClass="ipt160px" MaxLength="18"></asp:TextBox></td>
                            <td style="width: 42px; height: 22px; background-color: #e1e0b2">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 82px; height: 22px; background-color: #e1e0b2">
                        </td>
                            <td style="width: 5px; height: 22px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 114px; height: 25px; background-color: #e1e0b2">
                                <asp:RadioButton ID="rdoMan" runat="server" Text="<%$ Resources:BaseInfo,TpUse_lblSexMan %>"
                                    Width="36px" Checked="True" CssClass="labelStyle" GroupName="Sex" />
                                    <asp:RadioButton ID="rdoWoman" runat="server" Text="<%$ Resources:BaseInfo,TpUse_lblSexWoman %>"
                                    Width="37px" CssClass="labelStyle" GroupName="Sex" />
                                    </td>
                            <td style="width: 42px; height: 22px; background-color: #e1e0b2">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 82px; height: 22px; background-color: #e1e0b2">
                                <asp:Label ID="lbltxtBirth" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorBirthday %>"
                                    Width="84px"></asp:Label></td>
                            <td style="width: 5px; height: 22px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 114px; height: 25px; background-color: #e1e0b2">
                                <asp:TextBox ID="txtBirth" runat="server" CssClass="ipt160px" Width="110px" onclick="calendar()"></asp:TextBox></td>
                            <td style="width: 42px; height: 22px; background-color: #e1e0b2">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 82px; height: 22px; background-color: #e1e0b2">
                                <asp:Label ID="lblBeginWorkDate" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,TpUse_lblBeginWorkDate %>"
                                    Width="84px"></asp:Label></td>
                            <td style="width: 5px; height: 22px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 114px; height: 25px; background-color: #e1e0b2">
                                <asp:TextBox ID="txtBeginWorkDate" runat="server" CssClass="ipt160px" Width="110px" onclick="calendar()"></asp:TextBox></td>
                            <td style="width: 42px; height: 22px; background-color: #e1e0b2">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 82px; height: 22px; background-color: #e1e0b2">
                            <asp:Label id="lblStation" runat="server" Text='<%$ Resources:BaseInfo,TpUse_lblStation %>' Width="84px" CssClass="labelStyle"></asp:Label></td>
                            <td style="width: 5px; height: 22px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 114px; height: 25px; background-color: #e1e0b2">
                        
                      <asp:DropDownList id="cmbStation" runat="server" CssClass="ipt160px" Width="110px">
                    </asp:DropDownList></td>
                            <td style="width: 42px; height: 22px; background-color: #e1e0b2">
                            </td>
                        </tr>
                                                <tr>
                            <td style="width: 82px; height: 22px; background-color: #e1e0b2">
                            <asp:Label id="Label2" runat="server" Text='<%$ Resources:BaseInfo,TpUse_lblGathering %>' Width="84px" CssClass="labelStyle"></asp:Label></td>
                            <td style="width: 5px; height: 22px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 114px; height: 25px; background-color: #e1e0b2">
                        
                      <asp:DropDownList id="cmbGathering" runat="server" CssClass="ipt160px" Width="110px">
                    </asp:DropDownList></td>
                            <td style="width: 42px; height: 22px; background-color: #e1e0b2">
                            </td>
                        </tr>
                                                <tr>
                            <td style="width: 82px; height: 25px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 5px; height: 25px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 114px; height: 25px; background-color: #e1e0b2">
                                <asp:CheckBox ID="chkConcerned" runat="server" Text="<%$ Resources:BaseInfo,TpUse_lblBe_Concerned_With %>" CssClass="labelStyle" /></td>
                            <td style="width: 42px; height: 25px; background-color: #e1e0b2">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 82px; height: 22px; background-color: #e1e0b2">
                                <asp:Label ID="lblStatus" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblLocationStatus %>"
                                    Width="84px"></asp:Label></td>
                            <td style="width: 5px; height: 22px; background-color: #e1e0b2">
                            </td>
                            <td style="width: 114px; height: 25px; background-color: #e1e0b2">
                        
                      <asp:DropDownList id="cmbStatus" runat="server" CssClass="ipt160px" Width="110px">
                    </asp:DropDownList></td>
                            <td style="width: 42px; height: 22px; background-color: #e1e0b2">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="width: 280px; height: 5px; background-color: #e1e0b2; text-align: center"
                                valign="middle"><table border="0" cellpadding="0" cellspacing="0" style="width: 204px;text-align: center;">
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
                            <td align="right" colspan="4" style="height: 45px; background-color: #e1e0b2">
                                <asp:Button ID="btnSave" runat="server" CssClass="buttonSave"  
                                    OnClick="btnSave_Click" onmouseover="BtnOver(this.id);" 
                                    onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                    Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" Enabled="False"  />
                                <asp:Button ID="btnCancel"
                                        runat="server" CssClass="buttonCancel"  OnClick="btnCancel_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                        Text="<%$ Resources:BaseInfo,User_btnCancel %>" />
                                &nbsp;</td>
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

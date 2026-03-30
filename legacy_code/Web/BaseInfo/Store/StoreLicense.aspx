<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StoreLicense.aspx.cs" Inherits="Store_StoreLicense" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= baseinfo%></title>
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link rel="StyleSheet" href="../../App_Themes/nlstree/nlstree-basic.css" type="text/css" />
    <link rel="StyleSheet" href="../../App_Themes/nlstree/nlsctxmenu.css" type="text/css" />
	<script type="text/javascript" src="../../JavaScript/Common.js"></script>    
    <script type="text/javascript" src="../../App_Themes/nlstree/nlstree.js"></script>
    <script type="text/javascript" src="../../App_Themes/nlstree/nlsctxmenu.js"></script>
	<script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
    
    <script src="../../App_Themes/DateTime/popcalendar.js" type="text/javascript"></script>
	<script type="text/javascript"  src="../../JavaScript/setday.js"></script>
	<script type="text/javascript" src="../../JavaScript/Calendar.js" language="javascript" charset="gb2312"></script>
	
   <script type="text/javascript" language="javascript">
    function Load()
    {
        loadTitle();
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
        t.opt.width="235px";
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
        /*document.getElementById("lblTotalNum").style.display="none";
        document.getElementById("lblCurrent").style.display="none";
        */
        loadTitle();
    }



    function ev_click(e, id)
    {
        document.form1.deptid.value=id;
        document.form1.treeClick.click();
    } 
    function CheckIsNull()
    {
         if(isEmpty(document.all.txtLicenseCode.value))  
        {
            parent.document.all.txtWroMessage.value='<%= (String)GetGlobalResourceObject("BaseInfo", "Associator_AssociatorIdentity")%>'+document.getElementById("hidMessage").value;
            document.all.txtLicenseCode.focus();
            return false;					
        }
         if(isEmpty(document.all.txtPropertyName.value))  
        {
            parent.document.all.txtWroMessage.value='<%= (String)GetGlobalResourceObject("BaseInfo", "Store_ContentName")%>'+document.getElementById("hidMessage").value;
            document.all.txtPropertyName.focus();
            return false;					
        }
        if(isInteger(document.all.txtArea.value)==false)
        {
            alert("please input number.");   
            document.all.txtArea.focus();         
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
   </script>
</head>
<body onload='treearray();' topmargin=0 leftmargin=0>
     <form id="form1" runat="server">
    <asp:ScriptManager id="ScriptManager1" runat="server"></asp:ScriptManager>
    <div align="right" style="width:100%; ">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
            <asp:HiddenField id="depttxt" runat="server"></asp:HiddenField>
            <asp:HiddenField id="deptid" runat="server" ></asp:HiddenField>
                <asp:HiddenField ID="selectdeptid" runat="server" />
            <table border="0" cellpadding="0" cellspacing="0" style="height:400px; width:100%; vertical-align:top">
            <tr>                
                 <td style="width:33%; height: 400px; vertical-align: top;">
                    <table border="0" cellpadding="0" cellspacing="0" style=" height: 400px;">
                        
                        <tr>
                          <td style="vertical-align:top; height: 22px; width: 100%;" >          
                                 <table border="0" cellpadding="0" cellspacing="0" style="height: 22px; width:100%;   ">              
                                    <tr>
                                        <td class="tdTopRightBackColor"    valign="top" style="width: 8px; height:22px;  text-align:left" >
                                            <img alt="" class="imageLeftBack" style=" text-align:left; height: 22px;"  />
                                            </td>
                                            <td class="tdTopRightBackColor" style="height: 22px; text-align: left;">
                                        <asp:Label ID="labUserTree" runat="server" CssClass="lblTitle" Text='<%$ Resources:BaseInfo,Store_BusinessItemBrowse %>'></asp:Label></td>
                                      
                                        <td class="tdTopRightBackColor"   valign="top" style="width: 20px; height: 22px;">
                                            <img class="imageRightBack" style="width: 7px; height: 22px" />
                                            </td>
                                    </tr>                       
                                </table>               
                        </td>
                        </tr>
                        <tr> 
                            <td colspan="2" style="height: 350px; background-color: #e1e0b2; vertical-align:top; text-align:center;">                            
                                <table>   
                                    <tr>
                                        <td style="width:5px;" valign="top">   </td>
                                        <td style="height: 290px" valign="top">
                                          <asp:Panel ID="Panel1" runat="server" BorderStyle="Inset" BorderWidth="1px" Height="330px" ScrollBars="Auto" Width="260px" BackColor="White">
                                              <table>
                                                    <tr>
                                                        <td valign="top" id ="treeview" style="height: 259px; width: 207px; text-align:left;">                                    
                                                        </td>
                                                    </tr>
                                                </table>                
                                            </asp:Panel>
                                        </td>
                                        <td style="width:5px;" valign="top"> </td>
                                    </tr>
                                </table>
                                 
                                <asp:Button ID="treeClick" runat="server" CssClass="buttonHidden"  Width="1px" OnClick="treeClick_Click" />
                            </td>
                        </tr>
                    </table>
                </td>           
                <td style="width: 0.5%; height: 400px">
                </td>
                <td style=" height: 400px; text-align:center;" valign="top" align="center">
                    <table border="0" cellpadding="0" cellspacing="0" style="height: 350px; width:97%;" >
                        <tr>
                            <td colspan="4" style="height: 22px; background-color: #e1e0b2" valign="top">
                                 <table border="0" cellpadding="0" cellspacing="0" style="height: 22px; width:100%; ">
                                    <tr>
                                        <td class="tdTopRightBackColor"    valign="top" style=" height:22px;  text-align:left; width: 3px;" >
                                            <img alt="" class="imageLeftBack" style=" text-align:left; height: 22px;"  />
                                            </td>
                                            <td class="tdTopRightBackColor" style="height: 22px; text-align:left;" >
                                                <asp:Label ID="labUserDefine" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,Store_BusinessItemCardtenance %>"></asp:Label></td>
                                            <td class="tdTopRightBackColor"   valign="top" style=" height: 22px;">
                                                 <img class="imageRightBack" style="width: 7px; height: 22px" />
                                            </td>
                                    </tr>
                                </table>          
                            </td>
                        </tr>
                        <tr>
                            <td colspan="1" rowspan="2" style="width: 100%; height: 374px; background-color: #e1e0b2;
                                text-align: right" valign="top">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%">
                                    <tr>
                                        <td colspan="1" rowspan="2" style="width: 50%; height: 289px; background-color: #e1e0b2;
                                            text-align: right" valign="top">
                                            <table id="" cellpadding="0" cellspacing="0" style="width: 100%; height: 98%">
                                                <tr>
                                                    <td align="right" style="vertical-align:middle; width: 30%; height: 30px; background-color: #e1e0b2">
                                                        <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Store_CardType %>"></asp:Label>&nbsp;</td>
                                                    <td style="font-size: 12pt; vertical-align: middle; width: 80px; height: 30px; background-color: #e1e0b2">
                                                        <asp:DropDownList ID="ddlLicenseTypeId" runat="server" CssClass="ipt160px" Width="125px">
                                                        </asp:DropDownList></td>
                                                    <td style="font-size: 12pt; vertical-align: middle; width: 80px; height: 30px; background-color: #e1e0b2">
                                                    </td>
                                                </tr>
                                                <tr><td height=5px></td></tr>
                                                <tr style="font-size: 12pt">
                                                    <td align="right" style="vertical-align: middle; width: 30%; height: 23px; background-color: #e1e0b2">
                                                        <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorIdentity%>"></asp:Label>&nbsp;</td>
                                                    <td style="vertical-align: top; width: 80px; height: 23px; background-color: #e1e0b2">
                                                        <asp:TextBox ID="txtLicenseCode" runat="server" CssClass="ipt160px" Width="120px" MaxLength="32"></asp:TextBox></td>
                                                    <td style="vertical-align: middle;  height: 23px; background-color: #e1e0b2;" align="left"><img id="ImgCustShortName" src="../../App_Themes/Main/Images/must.gif" style="width: 16px; height: 16px" />
                                                    </td>
                                                </tr>
                                                <tr><td height=5px></td></tr>
                                                
                                                <tr style="font-size: 12pt">
                                                    <td align="right" style="vertical-align: middle; height: 16px; background-color: #e1e0b2">
                                                        <asp:Label ID="Label7" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Store_ContentName%>"></asp:Label></td>
                                                    <td align="center" style="vertical-align: middle; width: 80px; height: 16px; background-color: #e1e0b2">
                                                        <asp:TextBox ID="txtPropertyName" runat="server" CssClass="ipt160px" Width="120px" MaxLength="16"></asp:TextBox></td>
                                                    <td align="left" style="vertical-align: middle; height: 16px; background-color: #e1e0b2"><img id="Img1" src="../../App_Themes/Main/Images/must.gif" style="width: 16px; height: 16px" />
                                                    </td>
                                                </tr>
                                                <tr><td height=5px></td></tr>
                                                
                                                <tr style="font-size: 12pt">
                                                    <td align="right" style="height: 30px; background-color: #e1e0b2">
                                                        <asp:Label ID="Label9" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Store_Owner%>"></asp:Label>&nbsp;</td>
                                                    <td style="width: 80px; height: 25px; background-color: #e1e0b2">
                                                        <asp:TextBox ID="txtPropertyOwner" runat="server" CssClass="ipt160px" Width="120px" MaxLength="16"></asp:TextBox></td>
                                                    <td style="width: 80px; height: 25px; background-color: #e1e0b2">
                                                    </td>
                                                </tr>
                                                <tr><td height=5px></td></tr>
                                                
                                                <tr style="font-size: 12pt">
                                                    <td align="right" style="height: 30px; background-color: #e1e0b2">
                                                        <asp:Label ID="Label11" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Store_CardSize%>"></asp:Label>&nbsp;</td>
                                                    <td style="width: 80px; height: 30px; background-color: #e1e0b2">
                                                        <asp:TextBox ID="txtArea" runat="server" CssClass="ipt160px" Width="120px" 
                                                            MaxLength="8"></asp:TextBox></td>
                                                    <td style="width: 80px; height: 30px; background-color: #e1e0b2">
                                                    </td>
                                                </tr>
                                                <tr><td height=5px></td></tr>
                                                
                                                <tr style="font-size: 12pt">
                                                    <td align="right" style="height: 30px; background-color: #e1e0b2">
                                                        <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Store_DesignUse%>"></asp:Label>&nbsp;</td>
                                                    <td style="width: 80px; height: 30px; background-color: #e1e0b2">
                                                        <asp:DropDownList ID="ddlUsage" runat="server" Width="125px">
                                                        </asp:DropDownList></td>
                                                    <td style="width: 80px; height: 30px; background-color: #e1e0b2">
                                                    </td>
                                                </tr>
                                                <tr><td height=5px></td></tr>
                                                
                                                <tr style="font-size: 12pt">
                                                    <td align="right" style="height: 30px; background-color: #e1e0b2">
                                                        <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Store_PaperDate%>"></asp:Label>&nbsp;</td>
                                                    <td align="right" style="width: 80px; height: 30px; background-color: #e1e0b2">
                                                        <asp:TextBox ID="txtRegDate" runat="server" CssClass="ipt160px" onclick="calendar()"
                                                            Width="120px"></asp:TextBox></td>
                                                    <td align="right" style="width: 80px; height: 30px; background-color: #e1e0b2">
                                                    </td>
                                                </tr>
                                                <tr><td height=5px></td></tr>
                                                
                                                <tr style="font-size: 12pt">
                                                    <td align="right" style="height: 22px; background-color: #e1e0b2">
                                                        <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Store_SaveFileThing%>"></asp:Label>&nbsp;</td>
                                                    <td style="width: 80px; height: 22px; background-color: #e1e0b2">
                                                        <asp:TextBox ID="txtFiles" runat="server" CssClass="ipt160px" Width="120px" 
                                                            MaxLength="16"></asp:TextBox></td>
                                                    <td style="width: 80px; height: 22px; background-color: #e1e0b2">
                                                    </td>
                                                </tr>
                                                <tr style="font-size: 12pt">
                                                    <td align="right" style="height: 25px; background-color: #e1e0b2">
                                                    </td>
                                                    <td style="width: 80px; height: 25px; background-color: #e1e0b2">
                                                    </td>
                                                    <td style="width: 80px; height: 25px; background-color: #e1e0b2">
                                                    </td>
                                                </tr>
                                                <tr style="font-size: 12pt">
                                                    <td colspan="3" style="vertical-align: middle; height: 56px; background-color: #e1e0b2;
                                                        text-align: center">
                                                        <asp:Button ID="btnEdit" runat="server" CssClass="buttonEdit" OnClick="btnEdit_Click"
                                                            onmouseout="BtnUp(this.id);" onmouseover="BtnOver(this.id);" onmouseup="BtnUp(this.id);"
                                                            Text="<%$ Resources:BaseInfo,User_btnChang %>" />
                                                        <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" OnClick="btnSave_Click"
                                                            onmouseout="BtnUp(this.id);" onmouseover="BtnOver(this.id);" onmouseup="BtnUp(this.id);"
                                                            Text="<%$ Resources:BaseInfo,PotCustomer_butSave%>" />&nbsp;
                                                        <asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel" OnClick="btnCancel_Click"
                                                            onmouseout="BtnUp(this.id);" onmouseover="BtnOver(this.id);" onmouseup="BtnUp(this.id);"
                                                            Text="<%$ Resources:BaseInfo,User_btnCancel %>" /></td>
                                                    <td colspan="1" style="vertical-align: middle; height: 56px; background-color: #e1e0b2;
                                                        text-align: center">
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td colspan="1" rowspan="2" style="font-size: 12pt; height: 289px; background-color: #e1e0b2; width:250px;
                                            text-align: right" valign="top" >
                                            <table border="0" cellpadding="0" cellspacing="0" width="230px">
                                                <tr>
                                                    
                                                                <td align="center" colspan="2"  style="height: 85px; width:100%;" valign="top"  >
                                                                    <asp:GridView ID="GrdUser" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                                        BorderStyle="Inset" BorderWidth="1px" CellPadding="3" CssClass="gridview" OnRowDataBound="GrdUser_RowDataBound"
                                                                        OnSelectedIndexChanged="GrdUser_SelectedIndexChanged" PageSize="13" width="220px" AllowPaging="True" OnPageIndexChanging="GrdUser_PageIndexChanging">
                                                                        <FooterStyle BackColor="Red" ForeColor="#000066" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="LicenseId">
                                                                                <ItemStyle CssClass="hidden" />
                                                                                <HeaderStyle CssClass="hidden" />
                                                                                <FooterStyle CssClass="hidden" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="StoreId">
                                                                                <ItemStyle CssClass="hidden" />
                                                                                <HeaderStyle CssClass="hidden" />
                                                                                <FooterStyle CssClass="hidden" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="LicenseCode" HeaderText="<%$ Resources:BaseInfo,Associator_AssociatorIdentity %>">
                                                                                <ItemStyle CssClass="hidden" />
                                                                                <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="hidden" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="PropertyName" HeaderText="<%$ Resources:BaseInfo,Store_ContentName %>">
                                                                                <ItemStyle BorderColor="#E1E0B2" />
                                                                                <HeaderStyle BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                            </asp:BoundField>
                                                                            <asp:CommandField HeaderText="<%$ Resources:BaseInfo,PotShop_Selected %>" ShowSelectButton="True">
                                                                                <HeaderStyle BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                                <ItemStyle BorderColor="#E1E0B2" />
                                                                            </asp:CommandField>
                                                                        </Columns>
                                                                        <RowStyle Font-Overline="False" Font-Size="10pt" ForeColor="Black" Height="10px" />
                                                                        <SelectedRowStyle BackColor="#FFFFCD" Font-Bold="False" ForeColor="Black" />
                                                                        <HeaderStyle BackColor="#E1E0B2" Font-Bold="False" Height="10px" />
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
                                                    <td align="center" colspan="2" style="width: 100%; height: 36px" valign="top">
                                                    </td>
                                                </tr>
                                                 
                                            </table>
                                        </td>
                                        <td id="td1" colspan="2" rowspan="2" style="font-size: 12pt; height: 289px; background-color: #e1e0b2;
                                            text-align: center" valign="top">
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
            </tr>
        </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        </div>
        <asp:HiddenField ID="hidSelect" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidSelect %>" />
        <asp:HiddenField ID="hidUpdate" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidUpdate %>" />
        <asp:HiddenField ID="hidAdd" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidAdd %>" />
        <asp:HiddenField ID="hidUpdateLost" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidUpdateLost %>" />
        <asp:HiddenField ID="hidInsert" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidInsert %>" />
        <asp:HiddenField ID="hidMessage" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidMessage %>" />
    </form>
</body>
</html>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StoreHistory.aspx.cs" Inherits="Store_StoreHistory" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= baseinfo%></title>
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
	
   <script type="text/javascript" language="javascript">
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
        t.opt.height="260px";
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
         if(isEmpty(document.all.txtHistoryDate.value))  
        {
            parent.document.all.txtWroMessage.value='<%= (String)GetGlobalResourceObject("BaseInfo", "Store_OccurDate")%>'+document.getElementById("hidMessage").value;
            document.all.txtHistoryDate.focus();
            return false;					
        }
         if(isEmpty(document.all.txtHistoryDesc.value))  
        {
            parent.document.all.txtWroMessage.value='<%= (String)GetGlobalResourceObject("BaseInfo", "Store_BackdropExplain")%>'+document.getElementById("hidMessage").value;
            document.all.txtHistoryDesc.focus();
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

function CheckLength(obj,maxLength,lines)
   {
        if(obj.value.length > maxLength)
            {
                alert('已达到到大限制文本长度。');
                obj.value=obj.value.substring(0,maxLength);
            }
        var arr = obj.value.split("\n");
        if(arr.length > lines)
        {
                var value="";
                alert('已达到最大行数限制。');
                for(loop=0;loop<lines;loop++)
                {
                    if(loop != lines -1)
                        value=value + arr[loop] + "\n";
                    else
                        value = value + arr[loop];
                }
                obj.value = value;
        }
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
        <table border="0" cellpadding="0" cellspacing="0" style="height: 400px; width:100%; vertical-align:top" >
            <tr>                
                 <td style="width:33%; vertical-align: top;">
                    <table border="0" cellpadding="0" cellspacing="0" style=" height: 400px;vertical-align: top;">
                        
                        <tr>
                          <td style="vertical-align:top; height: 22px; width: 100%;" >          
                                 <table border="0" cellpadding="0" cellspacing="0" style="height: 22px; width:100%;   ">              
                                    <tr>
                                        <td class="tdTopRightBackColor"    valign="top" style="width: 8px; height:22px;  text-align:left" >
                                            <img alt="" class="imageLeftBack" style=" text-align:left; height: 22px;"  />
                                            </td>
                                            <td class="tdTopRightBackColor" style="height: 22px; text-align: left;">
                                        <asp:Label ID="labUserTree" runat="server" CssClass="lblTitle" Text='<%$ Resources:BaseInfo,Store_BusinessItemBrowse%>'></asp:Label></td>
                                      
                                        <td class="tdTopRightBackColor"   valign="top" style="width: 20px; height: 22px;">
                                            <img class="imageRightBack" style="width: 7px; height: 22px" />
                                            </td>
                                    </tr>                       
                                </table>               
                        </td>
                        </tr>
                        <tr>  
                             <td colspan="2" style="height: 300px; background-color: #e1e0b2; vertical-align:top; text-align:center;">
                                <table>   
                                    <tr>
                                        <td style="width:5px;" valign="top">   </td>
                                        <td style="height: 290px" valign="top">
                                          <asp:Panel ID="Panel1" runat="server" BorderStyle="Inset" BorderWidth="1px" Height="280px" ScrollBars="Auto" Width="260px" BackColor="White">
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
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 200px;" >
                                    <tr >
                                        <td style="width: 180px; height: 1px; background-color: #738495; position: relative; top: 10px;" align="center">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 180px; height: 1px; background-color: #ffffff; position: relative;top: 10px;" align="center">
                                        </td>
                                    </tr>
                                </table>
                                <asp:Button ID="treeClick" runat="server" CssClass="buttonHidden"  Width="1px" OnClick="treeClick_Click" />
                            </td>
                        </tr>
                    </table>
                </td>           
                <td style="width: 0.5%;">
                </td>
                <td style="height:400px;  vertical-align:top;" valign="top">
                    <table border="0" cellpadding="0" cellspacing="0" style="height: 400px; width:98%; vertical-align:top; " >
                        
                        <tr>
                            <td colspan="5" style="height: 22px; background-color: #e1e0b2" valign="top">
                                 <table border="0" cellpadding="0" cellspacing="0" style="height: 22px; width:100%; ">
                                    <tr>
                                        <td class="tdTopRightBackColor"    valign="top" style="width: 8px; height:22px;  text-align:left" >
                                            <img alt="" class="imageLeftBack" style=" text-align:left; height: 22px;"  />
                                            </td>
                                            <td class="tdTopRightBackColor" style="height: 22px; text-align: left;">
                                        <asp:Label
                                            ID="labUserDefine" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,Store_BusinessItemBackdroptenance %>"></asp:Label></td>
                                      
                                        <td class="tdTopRightBackColor"   valign="top" style="width: 20px; height: 22px;">
                                            <img class="imageRightBack" style="width: 7px; height: 22px" />
                                            </td>
                                    </tr>
                                </table>          
                            </td>
                        </tr>
                        <tr>
                            <td style=" width:95px;  background-color: #e1e0b2; height: 12px;" align="right">                            
                                <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Store_OccurDate%>" ></asp:Label></td>
                            <td style="width:145px;  background-color: #e1e0b2; text-align: left; height: 12px;"> 
                                <asp:TextBox ID="txtHistoryDate" onclick="calendar()" runat="server"  CssClass="ipt160px" Width="120px"></asp:TextBox></td>
                            <td colspan="2" rowspan="9" style="background-color: #e1e0b2; text-align: left; width: 140px;" valign="top" id="td1">
                                <table cellpadding="0" cellspacing="0" id="tablegrid1" style="height:100%; width:130px;">
                                    <tr>
                                        
                                        <td style="width:95%; height: 288px; right:-10px;" valign="top" align="center" >
                                            <asp:GridView ID="GrdUser" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                BorderStyle="Inset" BorderWidth="1px" CellPadding="3" CssClass="gridview" 
                                               
                                                PageSize="13" Width="200px" OnSelectedIndexChanged="GrdUser_SelectedIndexChanged" Height="232px" OnRowDataBound="GrdUser_RowDataBound" AllowPaging="True" OnPageIndexChanging="GrdUser_PageIndexChanging">
                                                <Columns>
                                                    <asp:BoundField DataField="HistoryId">
                                                        <ItemStyle CssClass="hidden" />
                                                        <HeaderStyle CssClass="hidden" />
                                                        <FooterStyle CssClass="hidden" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="StoreId">
                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="hidden" />
                                                        <ItemStyle CssClass="hidden" BorderColor="#E1E0B2" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="HistoryDate" HeaderText="<%$ Resources:BaseInfo,Store_OccurDate %>">
                                                        <HeaderStyle  CssClass="hidden" />
                                                        <ItemStyle CssClass="hidden" />
                                                    </asp:BoundField>
                                                     <asp:BoundField DataField="HistoryDesc" HeaderText="<%$ Resources:BaseInfo,Store_BackdropExplain %>">
                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                    </asp:BoundField>
                                                    <asp:CommandField ShowSelectButton="True" HeaderText="<%$ Resources:BaseInfo,PotShop_Selected %>">
                                                        <HeaderStyle CssClass="gridviewtitle" BorderColor="#E1E0B2" />
                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                    </asp:CommandField>
                                                </Columns>
                                                <FooterStyle BackColor="Red" ForeColor="#000066" />
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
                                </table>
                            </td>
                            
                        </tr>
                         <tr style="width:10px;">
                            <td style=" width:95px; background-color: #e1e0b2; vertical-align:top;" align="right">                            
                                <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Store_BackdropExplain%>"></asp:Label></td>
                            <td style="  background-color: #e1e0b2; text-align: left;vertical-align:top; width: 145px;"> 
                                <asp:TextBox ID="txtHistoryDesc" runat="server" CssClass="ipt160px" 
                                    Width="120px" Height="51px" TextMode="MultiLine" MaxLength="9"></asp:TextBox></td>
                        </tr>
                         <tr style="width:10px;">
                            <td style=" background-color: #e1e0b2; height: 206px; vertical-align: top;" align="center" colspan="2">                            <asp:Button ID="btnEdit"
                                        runat="server" CssClass="buttonEdit" OnClick="btnEdit_Click" Text="<%$ Resources:BaseInfo,User_btnChang %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                                <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" OnClick="btnSave_Click"
                                    Text="<%$ Resources:BaseInfo,PotCustomer_butSave%>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>&nbsp;&nbsp;<asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel" OnClick="btnCancel_Click" Text="<%$ Resources:BaseInfo,User_btnCancel %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/></td>
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
                                            <asp:Label ID="lblCurrent" Visible=false runat="server" ForeColor="Red" Height="9px">1</asp:Label><asp:Label
                                                ID="lblTotalNum" runat="server" Height="9px"></asp:Label>
    </form>
</body>
</html>
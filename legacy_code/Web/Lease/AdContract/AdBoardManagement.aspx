<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdBoardManagement.aspx.cs" Inherits="Lease_AdContract_AdBoardManagement" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "AdBoard_lblAdBoardManagement")%></title>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link rel="StyleSheet" href="../../App_Themes/nlstree/nlstree-basic.css" type="text/css" />
    <link rel="StyleSheet" href="../../App_Themes/nlstree/nlsctxmenu.css" type="text/css" />
    <style type="text/css">
        <!--
        
        table.tblBase tr.rowHeight{ height:28px;}
        
        table.tblBase tr.headLine{ height:1px; }
        table.tblBase tr.bodyLine{ height:1px; }
        
        td.baseLable{ padding-right:10px;text-align:right; width:136px}
        td.baseInput{ align:left;padding-right:20px }
        -->
      </style>
      <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
      <script language="javascript" type="text/javascript" src="../../JavaScript/Common.js"></script>
      <script type="text/javascript" src="../../App_Themes/nlstree/nlstree.js"></script>
      <script type="text/javascript" src="../../App_Themes/nlstree/nlsctxmenu.js"></script>
      <script type="text/javascript">
      <!--	    
	     function Load()
        {
            addTabTool("<%=strFresh %>,Lease/AdContract/AdBoardManagement.aspx");
            loadTitle();
            treearray();
        }
         //输入验证
        function InputValidator(sForm)
        {
            if(isEmpty(document.all.txtAdBordCode.value))
            {
                alert('<%= enterInfo %>');
                document.all.txtAdBordCode.focus();
                return false;
            }
            if(isEmpty(document.all.txtAdBordName.value))
            {
                alert('<%= enterInfo %>');
                document.all.txtAdBordName.focus();
                return false;
            }
            if(isEmpty(document.all.txtFloorArea.value))
            {
                alert('<%= enterInfo %>');
                document.all.txtFloorArea.focus();
                return false;
            }
            if(!isDigitDot(document.all.txtFloorArea.value))
            {
                alert('请输入数字类型');
                document.all.txtFloorArea.focus();
                return false;
            }
            if(!isDigitDot(document.all.txtUserArea.value))
            {
                alert('请输入数字类型');
                document.all.txtUserArea.focus();
                return false;
            }
        }
        
        var tabbar;
        function treearray()
        {
            var t = new NlsTree('MyTree');
            var treestr = document.getElementById("depttxt").value;
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
                  
                    t.add(id, pid, name, "", imgurl, true);
                    
                }
            }
                    t.opt.sort='no';
                    t.opt.enbScroll=true;
                    t.opt.height="280px";
                    t.opt.width="190px";
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
        }

        function ev_click(e, id)
        {
        	
            document.form1.deptid.value=id;
            document.form1.selectdeptid.value=id;
            document.form1.Button1.click(); 
             
        } 
	    -->  
      </script>
</head>
<body style="margin:0px" onload="Load()">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:HiddenField ID="depttxt" runat="server" />
                <asp:HiddenField ID="selectdeptid" runat="server" />
                <asp:HiddenField ID="deptid" runat="server"/>            
                         <table border="0" cellpadding="0" cellspacing="0" style="height: 27px; width: 100%;">
                            <tr>
                                <td class="tdTopRightBackColor"    valign="top" style="height:27px;  text-align:left" >
                                    <img alt="" class="imageLeftBack" style=" text-align:left"  />
                                    </td>
                                <td class="tdTopRightBackColor" style=" height: 27px; text-align:left;">
                                    <asp:Label
                                        ID="Label1" runat="server" Text='<%$ Resources:BaseInfo,AdBoard_lblAdBoardManagement %>' Height="12pt" Width="218px"></asp:Label></td>
                              
                                <td class="tdTopRightBackColor"   valign="top" style=" height: 27px; text-align: right;">
                                    <img class="imageRightBack" style="width: 7px; height: 22px" />
                                    </td>
                            </tr>
                            <tr class="headLine">
                            <td colspan="3"></td>
                            </tr>
                             <tr style="height: 1px">
                                 <td colspan="3" class="tdBackColor">
                                     <table style="width: 100%" >
                                         <tr style="height:10px">
                                             <td>
                                             </td>
                                             <td>
                                             </td>
                                             <td>
                                             </td>
                                             <td>
                                             </td>
                                             <td style="width: 497px">
                                             </td>
                                         </tr>
                                         <tr>
                                             <td class="baseLable" rowspan="8" style="text-align:left">
                                                <asp:Panel ID="Panel1" runat="server" BackColor="White" BorderStyle="Inset" BorderWidth="1px" Height="370px" ScrollBars="Auto" Width="200px">
                                                    <table>
                                                        <tr>
                                                            <td id="treeview" style="height: 250px; width: 100px; text-align:left;" valign="top"></td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                             </td>
                                             <td class="baseLable">
                                                 <asp:Label ID="Label2" runat="server" Text="<%$ Resources:BaseInfo,AdBoard_lblAdBoardCode %>" CssClass="labelStyle"></asp:Label></td>
                                             <td>
                                                 <asp:TextBox ID="txtAdBordCode" runat="server" CssClass="ipt160px" MaxLength="32"></asp:TextBox></td>
                                             <td class="baseLable">
                                                 <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdBoard_lblAdBoardName %>" Width="80px"></asp:Label></td>
                                             <td class="baseInput" style="width: 497px">
                                                 <asp:TextBox ID="txtAdBordName" runat="server" CssClass="ipt160px" MaxLength="32"></asp:TextBox></td>
                                         </tr>
                                         <tr>
                                             <td class="baseLable">
                                                 <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdContract_lblAdType %>"></asp:Label></td>
                                             <td>
                                                 <asp:DropDownList ID="ddlAdType" runat="server" Width="163px">
                                                 </asp:DropDownList></td>
                                             <td class="baseLable">
                                                 <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdBoard_lblAdBoardStatus %>"
                                                     Width="80px"></asp:Label></td>
                                             <td class="baseInput" style="width: 497px">
                                                 <asp:DropDownList ID="ddlStatus" runat="server" Width="163px">
                                                 </asp:DropDownList></td>
                                         </tr>
                                         <tr>
                                             <td class="baseLable">
                                                 <asp:Label ID="Label7" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblFloorArea %>"></asp:Label></td>
                                             <td>
                                                 <asp:TextBox ID="txtFloorArea" runat="server" CssClass="ipt160px" MaxLength="7"
                                                     Width="158px"></asp:TextBox></td>
                                             <td class="baseLable">
                                                 <asp:Label ID="Label8" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdBoard_UserArea %>"></asp:Label></td>
                                             <td class="baseInput" style="width: 497px">
                                                 <asp:TextBox ID="txtUserArea" runat="server" CssClass="ipt160px" MaxLength="7" Width="158px"></asp:TextBox></td>
                                         </tr>
                                         <tr>
                                             <td class="baseLable">
                                                 <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ChargeType_lblNote %>"></asp:Label></td>
                                             <td>
                                                 <asp:TextBox ID="txtNote" runat="server" CssClass="ipt160px" MaxLength="32" Width="158px"></asp:TextBox></td>
                                             <td class="baseLable">
                                                 </td>
                                             <td class="baseInput" style="width: 497px">
                                                 </td>
                                         </tr>
                                         <tr>
                                             <td class="baseLable">
                                                 </td>
                                             <td class="baseLable" style="text-align: left" colspan="3">
                                                 </td>
                                         </tr>
                                         <tr style="height:10px">
                                             <td class="baseLable">
                                             </td>
                                             <td colspan="3">
                                             </td>
                                         </tr>
                                         <tr>
                                             <td colspan="4" style="text-align: center">
                                                 <table border="0" cellpadding="0" cellspacing="0" style="left: 0px; width: 100%;
                                                     position: relative; top: 0px">
                                                     <tbody>
                                                         <tr>
                                                             <td style="width: 160px; height: 1px; background-color: #738495">
                                                             </td>
                                                         </tr>
                                                         <tr>
                                                             <td style="width: 160px; height: 1px; background-color: #ffffff">
                                                             </td>
                                                         </tr>
                                                     </tbody>
                                                 </table>
                                             </td>
                                         </tr>
                                         <tr>
                                             <td colspan="4" style="text-align: center">
                                                 <asp:GridView ID="gvAdBoard" runat="server" AutoGenerateColumns="False"  BorderStyle="Inset" BorderWidth="1px" CellPadding="3"
                                                     Width="100%" BackColor="White" OnRowDataBound="gvAdBoard_RowDataBound" OnSelectedIndexChanged="gvAdBoard_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="gvAdBoard_PageIndexChanging" Font-Size="Small">
                                                     <Columns>
                                                         <asp:BoundField DataField="AdBoardID">
                                                             <ItemStyle CssClass="hidden" />
                                                             <HeaderStyle CssClass="hidden" />
                                                             <FooterStyle CssClass="hidden" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="AdBoardCode" HeaderText="<%$ Resources:BaseInfo,AdBoard_lblAdBoardCode %>" >
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="AdBoardName" HeaderText="<%$ Resources:BaseInfo,AdBoard_lblAdBoardName %>" >
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="AdBoardTypeID" HeaderText="<%$ Resources:BaseInfo,AdContract_lblAdType %>" >
                                                             <ItemStyle CssClass="hidden" />
                                                             <HeaderStyle CssClass="hidden" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="AdBoardTypeName" HeaderText="<%$ Resources:BaseInfo,AdContract_lblAdType %>" >
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="FloorArea" HeaderText="<%$ Resources:BaseInfo,RentableArea_lblFloorArea %>" >
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="UseArea" HeaderText="<%$ Resources:BaseInfo,AdBoard_UserArea %>" >
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="AdBoardStatus" HeaderText="<%$ Resources:BaseInfo,AdBoard_lblAdBoardStatus %>" >
                                                             <ItemStyle CssClass="hidden"/>
                                                             <HeaderStyle CssClass="hidden"/>
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="AdBoardStatusName" HeaderText="<%$ Resources:BaseInfo,AdBoard_lblAdBoardStatus %>" >
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="Note" HeaderText="<%$ Resources:BaseInfo,ChargeType_lblNote %>" >
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                         </asp:BoundField>
                                                         <asp:CommandField ShowSelectButton="True" HeaderText="<%$ Resources:BaseInfo,PotShop_Selected %>" >
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                         </asp:CommandField>
                                                     </Columns>
                                                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Right" />
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
                                             <td>
                                             </td>
                                             <td>
                                                 </td>
                                             <td>
                                                 &nbsp;
                                                         <asp:Button ID="Button1" runat="server" CssClass="buttonHidden" OnClick="Button1_Click" Width="1px" /></td>
                                             <td style="text-align: right" colspan="2">
                                                 <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" OnClick="btnSave_Click"
                                                     Text="<%$ Resources:BaseInfo,Dept_TitleAdd %>" />
                                                 <asp:Button ID="btnEdit"
                                                         runat="server" CssClass="buttonEdit" OnClick="btnEdit_Click" Text="<%$ Resources:BaseInfo,User_btnChang %>" />
                                                 <asp:Button ID="btnCel" runat="server" CssClass="buttonClear" OnClick="btnCel_Click"
                                                             Text="<%$ Resources:BaseInfo,User_btnCancel %>" /></td>
                                         </tr>
                                         <tr style="height:10px">
                                             <td>
                                             </td>
                                             <td>
                                             </td>
                                             <td>
                                             </td>
                                             <td colspan="2" style="text-align: right">
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


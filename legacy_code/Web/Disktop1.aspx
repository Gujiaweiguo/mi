<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Disktop1.aspx.cs" Inherits="Disktop1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
        <link href="App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript" src="JavaScript/TabTools.js"></script>
    <link href="App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/CSS/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function styletabbar()
	    {
	       addTabTool("null");
	      // window.parent.LeftTree.location.href="Default10.aspx";
	       //window.location.reload = "Default10.aspx";
	    }
    </script>
    <style type="text/css"> 
    .bottomLine 
    { 
        border-bottom: #ffffff 1px dashed; 
        border-left-style: none; 
        border-right-style: none; 
    }
    
     #GridView1
     {
        background-image:url(pic/text-bg.jpg);
     }
     #GridView2
     {
        background-image:url(pic/text-bg.jpg);
     }
     #GridView3
     {
        background-image:url(pic/text-bg.jpg);
     }
     #GridView4
     {
        background-image:url(pic/text-bg.jpg);
     }
    </style>
</head>
<body onload="styletabbar()">
    <form id="form1" runat="server">
    <div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        <table style="z-index: 100; left: 0; position: absolute; top: 0px; width:100%" cellpadding="0" cellspacing="0">
            <tr>
                <td colspan="2" style="background-image:url(pic/title-bg.jpg); height: 40px;" class="tdTopBackColor">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lblname" runat="server" ForeColor="#C28E46" Font-Bold="False" Font-Size="Medium"></asp:Label>
                    <asp:Label ID="Label2" runat="server" Text="您好，欢迎回来！    您上次的登陆时间是：" ForeColor="#C28E46" Font-Bold="False" Font-Size="Medium"></asp:Label>
                    <asp:Label ID="lblDate" runat="server" Text="Label" ForeColor="#C28E46" Font-Bold="False" Font-Size="Medium"></asp:Label>
                </td>
            </tr>
            <tr>
            <td style="width:50%; vertical-align:top">
            <table cellpadding="0" cellspacing="0" style="width:99%">
            <tr>
                <td style="background-image:url(pic/bar-bg.jpg); height: 32px;" valign="middle">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <img src="pic/point.gif"/>&nbsp;&nbsp;
                <asp:Label ID="Label1" runat="server" Text="待 处 理" Font-Bold="True" ForeColor="#A96834"></asp:Label>&nbsp;&nbsp;
                <img id="img1" src="pic/open.jpg"/>
                </td> 
            </tr>
            <tr id="tr1" style="height:170px">
                <td>
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                        BorderWidth="0px" Height="100%" Width="100%" 
                        OnRowDataBound="GridView1_RowDataBound" BorderStyle="None" CellPadding="8" 
                        GridLines="Horizontal" ShowHeader="False" AllowPaging="True" 
                        OnPageIndexChanging="GridView1_PageIndexChanging" 
                        OnSelectedIndexChanged="GridView1_SelectedIndexChanged" PageSize="5">
                        <Columns>
                            <asp:BoundField DataField="FuncUrl">
                                <ItemStyle CssClass="hidden" />
                                <HeaderStyle CssClass="hidden" />
                                <FooterStyle CssClass="hidden" />
                            </asp:BoundField>
                            <asp:BoundField DataField="WrkFlwName" HeaderText="WrkFlwName" SortExpression="WrkFlwName">
                            <ItemStyle CssClass="gridviewtitle" ForeColor="#666666"/>
                            </asp:boundField>                            
                            <asp:BoundField DataField="StartTime" HeaderText="StartTime" SortExpression="StartTime">
                            <ItemStyle CssClass="hidden" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CompletedTime" HeaderText="CompletedTime" SortExpression="CompletedTime" >
                                <ItemStyle CssClass="hidden" />
                            </asp:BoundField>
                            <asp:BoundField DataField="UserID" HeaderText="UserID" SortExpression="UserID" Visible="False" />
                            <asp:BoundField DataField="VoucherID" HeaderText="VoucherID" SortExpression="VoucherID"
                                Visible="False" />
                            <asp:BoundField DataField="VoucherHints" HeaderText="VoucherHints" SortExpression="VoucherHints" >
                                <ItemStyle CssClass="hidden" />
                            </asp:BoundField>
                            <asp:BoundField DataField="VoucherMemo" HeaderText="VoucherMemo" SortExpression="VoucherMemo"
                                Visible="False" />
                            <asp:CommandField ShowSelectButton="True" Visible="False" />
                        </Columns>
                        <HeaderStyle CssClass="hidden"/>
                        <PagerStyle BackColor="White" ForeColor="#000066"   HorizontalAlign="Right" />
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
<asp:linkbutton id="btnGo" runat="server" causesvalidation="False" commandargument="-1" commandname="Page" text="GO"  Font-Size="Small"/> 
  </PagerTemplate>         
<PagerSettings Mode="NextPreviousFirstLast"  />

                    </asp:GridView></td>
            </tr>
            <tr>
                <td style="background-image:url(pic/bar-bg.jpg); height: 32px;" valign="middle">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <img src="pic/point.gif"/>&nbsp;&nbsp;
                    <asp:Label ID="Label3" runat="server" Text="最近更新" Font-Bold="True" ForeColor="#A96834"></asp:Label>&nbsp;&nbsp;
                    <img id="img2" src="pic/open.jpg"/>
                </td>
            </tr>
            <tr id="tr2" style="height:170px">
                <td>
                    <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" BorderWidth="0px" Height="100%" Width="100%" OnRowDataBound="GridView2_RowDataBound" BorderStyle="None" CellPadding="8" GridLines="Horizontal" ShowHeader="False" AllowPaging="True" OnPageIndexChanging="GridView1_PageIndexChanging" OnSelectedIndexChanged="GridView2_SelectedIndexChanged" PageSize="5">
                        <Columns>
                            <asp:BoundField DataField="FuncUrl">
                                <ItemStyle CssClass="hidden" />
                                <HeaderStyle CssClass="hidden" />
                                <FooterStyle CssClass="hidden" />
                            </asp:BoundField>
                            <asp:BoundField DataField="WrkFlwName" HeaderText="WrkFlwName" SortExpression="WrkFlwName">
                            <ItemStyle CssClass="gridviewtitle" ForeColor="#666666"/>
                            </asp:boundField>
                            <asp:BoundField DataField="StartTime" HeaderText="StartTime" SortExpression="StartTime">
                            <ItemStyle CssClass="hidden" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CompletedTime" HeaderText="CompletedTime" SortExpression="CompletedTime" >
                                <ItemStyle CssClass="hidden" />
                            </asp:BoundField>
                            <asp:BoundField DataField="UserID" HeaderText="UserID" SortExpression="UserID" Visible="False" />
                            <asp:BoundField DataField="VoucherID" HeaderText="VoucherID" SortExpression="VoucherID"
                                Visible="False" />
                            <asp:BoundField DataField="VoucherHints" HeaderText="VoucherHints" SortExpression="VoucherHints" >
                                <ItemStyle CssClass="hidden" />
                            </asp:BoundField>
                            <asp:BoundField DataField="VoucherMemo" HeaderText="VoucherMemo" SortExpression="VoucherMemo"
                                Visible="False" />
                            <asp:CommandField ShowSelectButton="True" Visible="False"/>
                        </Columns>
                        <HeaderStyle CssClass="hidden"/>
                        <PagerStyle BackColor="White" ForeColor="#000066"   HorizontalAlign="Right" />
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
<asp:linkbutton id="btnGo" runat="server" causesvalidation="False" commandargument="-1" commandname="Page" text="GO"  Font-Size="Small"/> 
  </PagerTemplate>         
<PagerSettings Mode="NextPreviousFirstLast"  />
                    </asp:GridView></td>
            </tr>
            </table>
            </td>
            <td style="width:50%; vertical-align:top">
            <table cellpadding="0" cellspacing="0" style="width:100%">
            <tr>
                <td style="background-image:url(pic/bar-bg.jpg); height: 32px;" valign="middle">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <img src="pic/point.gif"/>&nbsp;&nbsp;
                <asp:Label ID="Label4" runat="server" Text="每日工作" Font-Bold="True" ForeColor="#A96834"></asp:Label>&nbsp;&nbsp;
                <img id="img3" src="pic/open.jpg"/>
                </td> 
            </tr>
            <tr id="tr3" style="height:170px">
                <td>
                    <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" 
                        BorderWidth="0px" Height="100%" Width="100%" BorderStyle="None" CellPadding="8" 
                        GridLines="Horizontal" ShowHeader="False" OnRowDataBound="GridView3_RowDataBound" PageSize="5" AllowPaging="True" OnPageIndexChanging="GridView1_PageIndexChanging">
                        <Columns>
                            <asp:BoundField DataField="PageUrl">
                                <ItemStyle CssClass="hidden" />
                                <HeaderStyle CssClass="hidden" />
                                <FooterStyle CssClass="hidden" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MenuName" HeaderText="WrkFlwName" SortExpression="WrkFlwName">
                            <ItemStyle CssClass="gridviewtitle" ForeColor="#666666"/>
                            </asp:boundField>                            
                            <asp:CommandField ShowSelectButton="True" Visible="False" />
                        </Columns>
                        <HeaderStyle CssClass="hidden"/>
                        <PagerStyle BackColor="White" ForeColor="#000066"   HorizontalAlign="Right" />
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
<asp:linkbutton id="btnGo" runat="server" causesvalidation="False" commandargument="-1" commandname="Page" text="GO"  Font-Size="Small"/> 
  </PagerTemplate>         
<PagerSettings Mode="NextPreviousFirstLast"  />
                    </asp:GridView></td>
            </tr>
            <tr>
                <td style="background-image:url(pic/bar-bg.jpg); height: 32px;" valign="middle">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <img src="pic/point.gif"/>&nbsp;&nbsp;
                <asp:Label ID="Label6" runat="server" Text="每月工作" Font-Bold="True" ForeColor="#A96834"></asp:Label>&nbsp;&nbsp;
                <img id="img4" src="pic/open.jpg"/>
                </td> 
            </tr>
            <tr id="tr4" style="height:170px">
                <td>
                    <asp:GridView ID="GridView4" runat="server" AutoGenerateColumns="False" 
                        BorderWidth="0px" Height="100%" Width="100%" BorderStyle="None" CellPadding="8" 
                        GridLines="Horizontal" ShowHeader="False" OnRowDataBound="GridView3_RowDataBound" PageSize="5" AllowPaging="True" OnPageIndexChanging="GridView1_PageIndexChanging">
                        <Columns>
                            <asp:BoundField DataField="PageUrl">
                                <ItemStyle CssClass="hidden" />
                                <HeaderStyle CssClass="hidden" />
                                <FooterStyle CssClass="hidden" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MenuName" HeaderText="WrkFlwName" SortExpression="WrkFlwName">
                            <ItemStyle CssClass="gridviewtitle" ForeColor="#666666"/>
                            </asp:boundField>         
                            <asp:CommandField ShowSelectButton="True" Visible="False" />
                        </Columns>
                        <HeaderStyle CssClass="hidden"/>
                        <PagerStyle BackColor="White" ForeColor="#000066"   HorizontalAlign="Right" />
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
<asp:linkbutton id="btnGo" runat="server" causesvalidation="False" commandargument="-1" commandname="Page" text="GO"  Font-Size="Small"/> 
  </PagerTemplate>         
<PagerSettings Mode="NextPreviousFirstLast"  />
                    </asp:GridView></td>
            </tr>
        </table>
            </td>
            </tr>
        </table>        
       </ContentTemplate>
     </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>

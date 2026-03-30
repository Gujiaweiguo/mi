<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BrandSelect.aspx.cs" Inherits="Lease_TradeRelation_TradeRelationSelect" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server" >
<base target="_self"/>
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "LeaseholdContract_labTradeID")%></title>
    
    <link rel="StyleSheet" href="../../App_Themes/nlstree/nlstree-basic.css" type="text/css" />
    <link rel="StyleSheet" href="../../App_Themes/nlstree/nlsctxmenu.css" type="text/css" />
    <script type="text/javascript" src="../../App_Themes/nlstree/nlstree.js"></script>
    <script type="text/javascript" src="../../App_Themes/nlstree/nlsctxmenu.js"></script>
        <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
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
                t.opt.height="300px";
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
    }

    function ev_click(e, id)
    {
//        var value=id;
//        var txtObjId = window.opener.document.getElementById("hidTradeID");
//        txtObjId.value = value;
        var tradeLevel =id;
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
                if(id == tradeLevel)
                {
                    if( pid == 9999999 )
                    {
                        return;
                    }
                }
            }
        }
        //alert(tradeLevel.length);
        if(tradeLevel.length==3)
        {
            window.returnValue=tradeLevel;
            window.close();
            return true;
        }
    } 
    //选择列表中的品牌
    function SelectBrand(id)
    {
        window.returnValue=id;
        window.close();
    }
    </script>
</head>
<body onload='treearray();' topmargin=0 leftmargin=0>
    <form id="form1" runat="server">
    <div>
        <table border="0" cellpadding="0" cellspacing="0" style="height: 32px; width: 230px;">
            <tr>
                <td class="tdTopRightBackColor" style="height: 27px; text-align: left" valign="top">
                    <img alt="" class="imageLeftBack" style="text-align: left" />
                </td>
                <td class="tdTopRightBackColor" style="height: 27px; text-align: left">
                    <asp:Label ID="Label1" runat="server" Height="12pt" Text="<%$ Resources:BaseInfo,ShopBrand_lblBrandName %>"></asp:Label></td>
                <td class="tdTopRightBackColor" style="height: 27px; text-align: right" valign="top">
                    <img class="imageRightBack" style="width: 7px; height: 22px" />
                </td>
            </tr>
            <tr class="headLine">
                <td colspan="3">
                </td>
            </tr>
            <tr style="height: 1px">
                <td class="tdBackColor" colspan="3">
                    <table style="width: 331px">
                        <tr style="height: 10px">
                            <td colspan="4" style="height: 2px">
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
                            <td align="right" colspan="4" 
                                style="text-align: center; vertical-align: top; height: 300px;">
        <table>
            <tr>
                <td style="height: 290px; vertical-align:text-top; text-align: left;">
                    <asp:Panel ID="Panel1" runat="server" BackColor="White" BorderStyle="Inset" BorderWidth="1px"
                        Height="320px" ScrollBars="Auto" Width="180px">
                        <table>
                            <tr>
                                <td id="treeview" style="height: 280px; width: 159px;" valign="top">
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
                <td style="vertical-align:text-top; text-align:left;">
                <table>
                    <tr style="vertical-align:top;">
                        <td valign="top"><asp:TextBox ID="txtSearch" runat="server" Width="120px" 
                                AutoPostBack="True" ontextchanged="txtSearch_TextChanged"></asp:TextBox>      
                                                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
                                  CellPadding="3"  AutoGenerateColumns="False" Width="130px" 
                                BackColor="White" Font-Size="Small" 
                                onpageindexchanging="GridView1_PageIndexChanging" 
                                onrowdatabound="GridView1_RowDataBound" PageSize="11">
                                <Columns>
                                     <asp:BoundField DataField="BrandID">
                                         <ItemStyle CssClass="hidden" />
                                         <HeaderStyle CssClass="hidden" />
                                         <FooterStyle CssClass="hidden" />
                                     </asp:BoundField>
                                     <asp:BoundField DataField="BrandName" >
                                         <ItemStyle BorderColor="#E1E0B2" />
                                         <HeaderStyle  Height="1px" CssClass="hidden" />
                                     </asp:BoundField>
                                 </Columns>
                                 <FooterStyle BackColor="Red" ForeColor="#000066"/>
                                                <RowStyle ForeColor="Black" Height="10px" Font-Overline="False" Font-Size="10pt" />
                                                <SelectedRowStyle BackColor="#FFFFCD" Font-Bold="False" ForeColor="Black" />
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
                    </asp:GridView>
                        </td>
                    </tr>
                </table>
                    
                </td>
            </tr>
        </table>
                                &nbsp;</td>
                        </tr>
                        </table>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="depttxt" runat="server" />
        <asp:HiddenField ID="deptid" runat="server" />
        <asp:HiddenField ID="selectdeptid" runat="server" />
    
    </div>
    </form>
</body>
</html>

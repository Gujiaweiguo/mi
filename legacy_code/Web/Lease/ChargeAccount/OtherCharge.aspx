<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OtherCharge.aspx.cs" Inherits="Lease_ChargeAccount_OtherCharge" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Account_lblOtherCharge")%></title>
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
        t.opt.height="385px";
        t.opt.width="220px";
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
        addTabTool("<%=baseinfo %>,Lease/ChargeAccount/OtherCharge.aspx");
        loadTitle();
    }
    function checkall(MyControl)
	{
		for (i=0;i<form1.elements.length;i++)
		{
			if (form1.elements[i].type=="checkbox")
			{
				form1.elements[i].checked=MyControl.checked;
			}
		}
	}
	
    function NumberTest(tCurQty)
    {
        try
		{
            if ( !(((window.event.keyCode >= 48) && (window.event.keyCode <= 57)) 
                || (window.event.keyCode == 13) &&(window.event.keyCode == 46)
                || (window.event.keyCode == 45)))
            {
                window.event.keyCode = 0 ;
            }
        }
	    catch(e){} 
    }
    function ReturnDefault()
    {
        window.parent.mainFrame.location.href="../../Disktop1.aspx";
    }
    function CheckKeyCode(obj,keyCodes)
    {
	    if(event.keyCode==110 || event.keyCode==190)
	    {
		    if(obj.value.indexOf('.')!=-1)
		    {
			    return false;
		    }
		    else
		    {
			    return true;
		    }
	    }
	    else
	    {
		    var keyCodes=new Array(8,9,13,16,17,35,36,37,38,39,40,46,48,49,50,51,52,53,54,55,56,57,96,97,98,99,100,101,102,103,104,105);
		    for(var i=0;i<keyCodes.length;i++)
		    {
			    if(event.keyCode==keyCodes[i])
			    {
				    return true;
			    }
		    }
		    return false;
	    }
    }
   </script>
</head>
<body onload='treearray();Load();' topmargin=0 leftmargin=0>
     <form id="form1" runat="server">
    <asp:ScriptManager id="ScriptManager1" runat="server"></asp:ScriptManager>
    <div align="center" style="width:100%; ">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
            <asp:HiddenField id="depttxt" runat="server"></asp:HiddenField>
            <asp:HiddenField id="deptid" runat="server" ></asp:HiddenField>
                <asp:HiddenField ID="selectdeptid" runat="server" />
        <table border="0" cellpadding="0" cellspacing="0" style="height: 450px; width:100%; vertical-align:top" >
            <tr>                
                 <td style="width:40%; vertical-align: top;">
                    <table border="0" cellpadding="0" cellspacing="0" style=" height: 450px;vertical-align: top; width:70%">
                        
                        <tr>
                          <td style="vertical-align:top; height: 22px; width: 100%; background-color: #e1e0b2;" >          
                                 <table border="0" cellpadding="0" cellspacing="0" style="height: 22px; width:100%;   ">              
                                    <tr>
                                        <td class="tdTopRightBackColor"    valign="top" style="width: 8px; height:22px;  text-align:left" >
                                            <img alt="" class="imageLeftBack" style=" text-align:left; height: 22px;"  />
                                            </td>
                                            <td class="tdTopRightBackColor" style="height: 22px; text-align: left;">
                                        <asp:Label ID="labUserTree" runat="server" CssClass="lblTitle" Text='<%$ Resources:BaseInfo,Account_lblOtherCharge%>'></asp:Label></td>
                                      
                                        <td class="tdTopRightBackColor"   valign="top" style="width: 20px; height: 22px;">
                                            <img class="imageRightBack" style="width: 7px; height: 22px" />
                                            </td>
                                    </tr>                       
                                </table>               
                        </td>
                        </tr>
                        <tr>  
                             <td colspan="2" style="height: 300px; background-color: #e1e0b2; vertical-align:top; text-align:center;" align="right">
                                <table>   
                                    <tr>
                                        <td style="width: 10%" valign="top"></td>
                                        <td colspan="2" valign="top">
                                          <asp:Panel ID="Panel1" runat="server" BorderStyle="Inset" BorderWidth="1px" Height="405px" ScrollBars="Auto" Width="230px" BackColor="White">
                                              <table>
                                                    <tr>
                                                        <td valign="top" id ="treeview" style="height: 285px; width: 160px; text-align:left;">                                    
                                                        </td>
                                                    </tr>
                                                </table>                
                                            </asp:Panel>
                                        </td>
                                        <td style="width: 10%" valign="top"></td>
                                    </tr>
                                </table>
                                 </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2" style="vertical-align: top; height: 10px; background-color: #e1e0b2;
                                text-align: center">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="vertical-align: top; height: 17px; background-color: #e1e0b2; text-align: right" align="center">
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="width: 10%"></td>
                                        <td >
                                        <asp:Button ID="treeClick" runat="server" CssClass="buttonHidden"  Width="1px" OnClick="treeClick_Click" />
                                        <asp:Button ID="btnAdd" runat="server" CssClass="buttonAdd" onmouseout="BtnUp(this.id);" onmouseover="BtnOver(this.id);" onmouseup="BtnUp(this.id);" Text="<%$ Resources:BaseInfo,Dept_TitleAdd %>" Enabled="False" OnClick="btnAdd_Click" /></td>
                                        <td style="width: 10%"> </td>
                                    </tr>
                                </table>
                                 </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="vertical-align: top; height: 13px; background-color: #e1e0b2;
                                text-align: center">
                            </td>
                        </tr>
                    </table>
                </td>           
                <td style="width: 2%;">
                </td>
                <td style="height:450px; width:55%; vertical-align:top;" valign="top">
                    <table border="0" cellpadding="0" cellspacing="0" style="height: 450px; width:100%; vertical-align:top; " >
                        
                        <tr style="width:100%">
                            <td colspan="1" style="height: 22px; background-color: #e1e0b2" valign="top">
                            </td>
                            <td colspan="10" style="height: 22px; background-color: #e1e0b2" valign="top">
                                 <table border="0" cellpadding="0" cellspacing="0" style="height: 22px; width:100%; ">
                                    <tr>
                                        <td class="tdTopRightBackColor"    valign="top" style="width: 8px; height:22px;  text-align:left" >
                                            <img alt="" class="imageLeftBack" style=" text-align:left; height: 22px;"  />
                                            </td>
                                            <td class="tdTopRightBackColor" style="height: 22px; text-align: left;">
                                        <asp:Label
                                            ID="labUserDefine" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,Account_lblOtherCharge %>"></asp:Label></td>
                                      
                                        <td class="tdTopRightBackColor"   valign="top" style="width: 20px; height: 22px;">
                                            <img class="imageRightBack" style="width: 7px; height: 22px" />
                                            </td>
                                    </tr>
                                </table>          
                            </td>
                        </tr>
                        <tr style="height:38px;width:100%;">
                            <td style="background-color: #e1e0b2; text-align: left">
                            </td>
                            <td style=" background-color: #e1e0b2; text-align: left">
                                </td>
                            <td style=" background-color: #e1e0b2; text-align: left">
                                </td>
                            <td style="background-color: #e1e0b2; text-align: right; width:120px;">
                                <asp:Label ID="Label8" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Account_lblChargeType %>"></asp:Label></td>
                            <td style="width: 105px;  background-color: #e1e0b2; text-align: left; height: 30px;">
                                <asp:DropDownList ID="ddlChargeType" runat="server" Width="115px">
                                </asp:DropDownList></td>
                            <td style=" background-color: #e1e0b2; text-align: left">
                                <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblShopStartDate %>" Width="60px"></asp:Label></td>
                            <td style="width: 105px;  background-color: #e1e0b2; text-align: left">
                                <asp:TextBox ID="txtStartDate" runat="server" CssClass="ipt160px" onclick="calendar()"
                                    Width="100px"></asp:TextBox></td>
                            <td style=" background-color: #e1e0b2; text-align:left;">
                                <asp:Label ID="Label7" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Rpt_EDate %>" Width="60px"></asp:Label></td>
                            <td style="width: 200px; background-color: #e1e0b2; text-align: left">
                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="ipt160px" onclick="calendar()"
                                    Width="100px"></asp:TextBox></td>
                            <td style="width: 190px; background-color: #e1e0b2; text-align: left" colspan="2">
                            </td>
                        </tr>
                        <tr style="width:100%">
                            <td align="center" colspan="1" style="vertical-align: top; background-color: #e1e0b2;text-align: center" valign="top">
                            </td>
                            <td colspan="10" style="vertical-align: top; width: 95%; background-color: #e1e0b2; text-align: center" valign="top" align="center">
                                <asp:GridView ID="gvCharge" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#E1E0B2" Height="246px" AllowPaging="True" PageSize="15" OnRowDataBound="gvCharge_RowDataBound" Width="95%" OnPageIndexChanging="gvCharge_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <INPUT id="AllSelect" onclick="checkall(this);" type="checkbox" ><asp:Label id="lbAllCheckBox" runat="server">全选</asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox id="Checkbox" runat="server" Value='<%# DataBinder.Eval(Container, "DataItem.OtherChargeDID")%>'></asp:CheckBox>
                                            </ItemTemplate>
                                         <ItemStyle BorderColor="#E1E0B2" />
                                         <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                &nbsp;<asp:TextBox ID="txtOtherChargeDID" runat="server" CssClass="ipt35px" Text='<%# DataBinder.Eval(Container, "DataItem.OtherChargeDID")%>' Font-Size="9pt" Width="80px"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="hidden" />
                                            <HeaderStyle CssClass="hidden" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,PotShop_lblPotShopName %>">
                                            <ItemTemplate>
                                                &nbsp;<asp:Label ID="txtShopName" runat="server" CssClass="" Text='<%# DataBinder.Eval(Container, "DataItem.ShopName")%>' Font-Size="9pt" Width="160px" ReadOnly="true"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                         <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,Account_lblChargeMoney %>">
                                            <ItemTemplate>
                                                &nbsp;<asp:TextBox ID="txtChgAmt" runat="server" CssClass="ipt35px" Text='<%# DataBinder.Eval(Container, "DataItem.ChgAmt")%>' Font-Size="9pt" Width="80px"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                         <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,PotShop_lblShopStartDate %>">
                                            <ItemTemplate>
                                                &nbsp;<asp:TextBox ID="txtStartDate" runat="server" CssClass="ipt35px" Text='<%# DataBinder.Eval(Container, "DataItem.StartDate")%>' Font-Size="9pt" Width="80px" onclick="calendar()"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                         <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,Rpt_EDate %>">
                                            <ItemTemplate>
                                                &nbsp;<asp:TextBox ID="txtEndDate" runat="server" CssClass="ipt35px" Text='<%# DataBinder.Eval(Container, "DataItem.EndDate")%>' Font-Size="9pt" Width="80px" onclick="calendar()"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                         <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:BaseInfo,LeaseholdContract_labRefID%>">
                                            <ItemTemplate>
                                                &nbsp;<asp:TextBox ID="txtRefID" runat="server" CssClass="ipt35px" Text='<%# DataBinder.Eval(Container, "DataItem.RefID")%>' Font-Size="9pt" Width="100px"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                         <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                &nbsp;<asp:TextBox ID="txtShopID" runat="server" CssClass="ipt35px" Text='<%# DataBinder.Eval(Container, "DataItem.ShopID")%>' Font-Size="9pt"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="hidden" />
                                            <HeaderStyle CssClass="hidden" />
                                        </asp:TemplateField>
                                    </Columns>
                                     <FooterStyle BackColor="Red" ForeColor="#000066"/>
                                    <RowStyle ForeColor="Black" Height="20px" Font-Overline="False" Font-Size="10pt" HorizontalAlign="Left" />
                                    <SelectedRowStyle BackColor="#FFFFCD" Font-Bold="False" ForeColor="Black" />
                                    <HeaderStyle BackColor="#E1E0B2" Height="10px" Font-Bold="False"  />
                                    <PagerStyle BackColor="White" ForeColor="#000066"   HorizontalAlign="Right"/>
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
                            <td align="center" colspan="1" style="vertical-align: top; background-color: #e1e0b2;
                                text-align: center" valign="top">
                            </td>
                        </tr>
                        <tr style="width:100%">
                            <td colspan="1" style="vertical-align: middle; height: 10px; background-color: #e1e0b2;
                                text-align: right">
                            </td>
                            <td colspan="10" style="vertical-align: middle; height: 10px; background-color: #e1e0b2;
                                text-align: right; width: 100%;">
                            </td>
                            <td colspan="1" style="vertical-align: middle; height: 10px; background-color: #e1e0b2;
                                text-align: right">
                            </td>
                        </tr>
                        <tr style="width:100%">
                            <td colspan="1" style="vertical-align: middle; height: 12px; background-color: #e1e0b2;
                                text-align: right">
                            </td>
                            <td colspan="10" style="vertical-align: middle; height: 12px; background-color: #e1e0b2;
                                text-align: right">
                                &nbsp;<asp:Button ID="btnSave" runat="server" CssClass="buttonSave"
                                    Text="<%$ Resources:BaseInfo,PotCustomer_butSave%>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);" OnClick="btnSave_Click"/>
                                <asp:Button ID="btnPutIn" runat="server" CssClass="buttonPutIn"
                                    onmouseout="BtnUp(this.id);" onmouseover="BtnOver(this.id);" onmouseup="BtnUp(this.id);"
                                    Text="<%$ Resources:BaseInfo,Lease_NewLineBtnPutIn %>" OnClick="btnPutIn_Click" />&nbsp;<asp:Button
                                        ID="btnBlankOut" runat="server" CssClass="buttonClear" OnClick="btnBlankOut_Click"
                                        onmouseout="BtnUp(this.id);" onmouseover="BtnOver(this.id);" onmouseup="BtnUp(this.id);"
                                        Text="<%$ Resources:BaseInfo,ConLease_butDel %>" />
                                &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp;
                            </td>
                            <td colspan="1" style="vertical-align: middle; height: 12px; background-color: #e1e0b2;
                                text-align: right">
                            </td>
                        </tr>
                         <tr style="width:100%">
                             <td align="center" colspan="1" style="vertical-align: top; background-color: #e1e0b2">
                             </td>
                            <td style=" background-color: #e1e0b2; vertical-align: top;" align="center" colspan="8">                            &nbsp;&nbsp;&nbsp;</td>
                             <td align="center" colspan="1" style="vertical-align: top; background-color: #e1e0b2">
                             </td>
                             <td align="center" colspan="1" style="vertical-align: top; background-color: #e1e0b2; width: 145px;">
                             </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
         <asp:HiddenField ID="HidenWrkID" runat="server">
            </asp:HiddenField>
             <asp:HiddenField ID="HidenVouchID" runat="server">
            </asp:HiddenField>
            </ContentTemplate>
        </asp:UpdatePanel>
        </div>
        <asp:HiddenField ID="hidSelect" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidSelect %>" />
        <asp:HiddenField ID="hidUpdate" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidUpdate %>" />
        <asp:HiddenField ID="hidAdd" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidAdd %>" />
        <asp:HiddenField ID="hidUpdateLost" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidUpdateLost %>" />
        <asp:HiddenField ID="hidInsert" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidInsert %>" />
        <asp:HiddenField ID="hidMessage" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidMessage %>" />
        <asp:HiddenField ID="HidConfim" runat="server" Value="<%$ Resources:BaseInfo,Prompt_ConfirmOK %>" />
    </form>
</body>
</html>

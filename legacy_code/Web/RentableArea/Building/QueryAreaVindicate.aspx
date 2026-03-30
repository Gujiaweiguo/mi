<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QueryAreaVindicate.aspx.cs" Inherits="RentableArea_Building_QueryAreaVindicate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Menu_BrowsingTenantGroups")%></title>
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link rel="StyleSheet" href="../../App_Themes/nlstree/nlstree-basic.css" type="text/css" />
    <link rel="StyleSheet" href="../../App_Themes/nlstree/nlsctxmenu.css" type="text/css" />
    <script type="text/javascript" src="../../App_Themes/nlstree/nlstree.js"></script>
    <script type="text/javascript" src="../../App_Themes/nlstree/nlsctxmenu.js"></script>
	<script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
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
            t.add(id, pid, name, "",imgurl, true);
            
        }
    }
            t.opt.sort='no';
            t.opt.enbScroll=true;
            t.opt.height="245px";
            t.opt.width="300px";
            t.opt.trg="mainFrame";
            t.opt.oneExp=true;

            t.treeOnClick = ev_click;
            t.render("treeview");      
            t.collapseAll();
            
            if(document.form1.selectdeptid.value!="")
            {
                t.expandNode(document.form1.selectdeptid.value);
                t.selectNodeById(document.form1.selectdeptid.value);
            }
             
            addTabTool("<%=strFresh %>,RentableArea/Building/QueryAreaVindicate.aspx");
	    loadTitle(); 
}



function ev_click(e, id)
{
    document.form1.deptid.value=id;
    document.form1.selectdeptid.value=id;
    document.form1.treeClick.click(); 
     
} 
		-->
</script>
</head>
<body onload='treearray();' topmargin=0 leftmargin=0>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:HiddenField ID="depttxt" runat="server" EnableViewState="False" />
                <asp:HiddenField ID="selectdeptid" runat="server" />
                <asp:HiddenField ID="deptid" runat="server"  />
                <table border="0" cellpadding="0" cellspacing="0" style="height: 430px; width:98%">
                    <tr>
                        <td style="width: 45%; height: 380px; text-align: right; vertical-align: top;">
                            <table border="0" cellpadding="0" cellspacing="0" style="height: 365px; vertical-align:top;" width="100%">
                                <tr>
                                    <td class="tdTopBackColor" colspan="2" style="width: 266px; height: 27px;">
                                        <img alt="" class="imageLeftBack" /><asp:Label ID="labAreaVindicate" runat="server" CssClass="lblTitle"
                                            Text="<%$ Resources:BaseInfo,RentableArea_lblAreaQuery %>"></asp:Label></td>
                                    <td class="tdTopRightBackColor" colspan="2" valign="top" style="height: 27px">
                                        &nbsp;<img class="imageRightBack" /></td>
                                </tr>
                                <tr height="1">
                                    <td colspan="5" style="height: 1px">
                                    </td>
                                </tr>
                                <tr>
                                
                                    <td class="tdBackColor" colspan="4" rowspan="10" style="height: 345px; width:100%; text-align: center"
                                        valign="top">
                                        
                                        <table style="height: 320px; width:100%;">
                                        <tr>
                                        <td style="height: 10px; width: 100%;">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 90%">
                                            <tr>
                                                <td style="width: 160px; height: 1px; background-color: #738495">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 160px; height: 1px; background-color: #ffffff">
                                                </td>
                                            </tr>
                                        </table>
                                        
                                        </td>
                                        </tr>
                                        
                                      <tr>
                                        <td style="height: 275px; width: 100%;">
                                         <asp:Panel ID="Panel1" runat="server" BackColor="White" BorderStyle="Inset" BorderWidth="1px"
                                            Font-Size="Medium" Height="260px" HorizontalAlign="Left" ScrollBars="Auto" Width="330px">
                                            <table>
                                                <tr>
                                                    <td style="width: 203px; height: 225px" valign="top" id="treeview">
                                                        &nbsp;</td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        </td>
                                        </tr>
                                            <tr>
                                                <td style="height: 23px; width: 100%;">
                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 90%">
                                                    <tr>
                                                        <td style="width: 160px; height: 1px; background-color: #738495">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 160px; height: 1px; background-color: #ffffff">
                                                        </td>
                                                    </tr>
                                                </table>
                                                </td>
                                            </tr>
                                        
                                        </table>
                                                    <asp:Button ID="treeClick" runat="server" CssClass="buttonHidden" OnClick="treeClick_Click"
                                                        Width="1px" Height="1px" /></td>
                                </tr>
                            </table>
                        </td>
                        <td style="height: 380px; width:4%;">
                        </td>
                        <td colspan="3" style="width:45%; height: 380px; vertical-align: top;">
                            <table border="0" cellpadding="0" cellspacing="0" style="height: 280px; vertical-align:top; text-align: right;" width="100%">
                                <tr>
                                    <td class="tdTopBackColor" colspan="2" valign="top">
                                        <img alt="" class="imageLeftBack" />
                                        <asp:Label ID="titleArea" runat="server" CssClass="lblTitle"></asp:Label><asp:Label
                                                ID="labAreaTitle" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,AreaVindicate_labAreaTitle %>"></asp:Label><a
                                                    style="font-size: 18px"></a></td>
                                    <td class="tdTopRightBackColor" colspan="2" valign="top">
                                        &nbsp;<img class="imageRightBack" /></td>
                                </tr>
                                <tr height="1">
                                    <td colspan="4" style="height:1px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" colspan="4" rowspan="10" style="height: 257px; text-align: right; width:100%;"
                                        valign="top">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; left: -40px; position: relative; text-align: right;">
                                            <tr>
                                                <td style="height: 30px; width:100%;" colspan="3">
                                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 75%; text-align: right;">
                                                        <tr>
                                                            <td style="width: 160px; height: 1px; background-color: #738495; position: relative; top: 2px;">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 160px; height: 1px; background-color: #ffffff; position: relative; top: 2px;">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    &nbsp;&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td style="width: 229px; height: 26px; text-align: right">
                                                    <asp:Label ID="labAreaCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AreaVindicate_labAreaCode %>"
                                                        Width="90px"></asp:Label></td>
                                                <td style="height: 26px">
                                                    &nbsp;</td>
                                                <td style="width: 194px; height: 28px; text-align: left">
                                                    <asp:TextBox ID="txtAreaCode" runat="server" CssClass="Enabledipt160px" MaxLength="50" Width="146px" ReadOnly="True"></asp:TextBox></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 229px; text-align: right; height: 18px;">
                                                    &nbsp;</td>
                                                <td style="height: 18px">
                                                    &nbsp;</td>
                                                <td style="width: 194px; text-align: left; height: 18px;">
                                                    </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 229px; height: 24px; text-align: right">
                                                    <asp:Label ID="labAreaName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AreaVindicate_labAreaName %>"
                                                        Width="90px"></asp:Label></td>
                                                <td style="height: 24px">
                                                    &nbsp;</td>
                                                <td style="width: 194px; height: 28px; text-align: left">
                                                    <asp:TextBox ID="txtAreaName" runat="server" CssClass="Enabledipt160px" MaxLength="50" Width="146px" ReadOnly="True"></asp:TextBox></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 229px; height: 18px; text-align: right">
                                                    </td>
                                                <td style="height: 0px">
                                                    &nbsp;</td>
                                                <td style="width: 194px; height: 18px; text-align: left">
                                                    </td>
                                            </tr>
                                            
                                            <tr>
                                                <td style="width: 229px; height: 24px; text-align: right">
                                                    <asp:Label ID="lblStatus" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblLocationStatus %>"
                                                        Width="90px"></asp:Label></td>
                                                <td style="height: 24px">
                                                    &nbsp;</td>
                                                <td style="width: 194px; height: 28px; text-align: left">
                                                    <asp:DropDownList ID="cmbStatus" runat="server" CssClass="ipt160px" Enabled="False" Width="153px">
                                                    </asp:DropDownList></td>
                                            </tr>                                            
                                            
                                                                                        <tr>
                                                <td style="width: 229px; height: 18px; text-align: right">
                                                    </td>
                                                <td style="height: 18px">
                                                    &nbsp;</td>
                                                <td style="width: 194px; height: 18px; text-align: left">
                                                    </td>
                                            </tr>
                                            
                                            
                                            <tr>
                                                <td style=" text-align: right">
                                                    <asp:Label ID="lblNode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labNote %>"
                                                        Width="79px"></asp:Label></td>
                                                <td>
                                                    &nbsp;</td>
                                                <td style=" text-align: left" rowspan="4">
                                                    <asp:TextBox ID="txtNote" runat="server" CssClass="EnabledColor" MaxLength="50" Width="144px" Height="93px" TextMode="MultiLine" ReadOnly="True"></asp:TextBox>&nbsp;</td>
                                            </tr>
                                            
                                            
                                            <tr>
                                                <td style=" text-align: right">
                                                    </td>
                                                <td>
                                                    </td>
                                            </tr>
                                            <tr>
                                                <td style=" height: 24px; text-align: right">
                                                    </td>
                                                <td style="height: 24px">
                                                    </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style=" height: 10px; text-align: right">
                                                    </td>
                                               
                                            </tr>
                                            <tr>
                                                <td colspan="3" style="height: 30px; text-align: right;width: 100%;">
                                                   <table border="0" cellpadding="0" cellspacing="0" style="width: 75%; text-align: right;" id="TABLE1"
                                                        >
                                                        <tr>
                                                            <td style="width: 160px;  height: 1px; background-color: #738495; position: relative;
                                                                top: 22px;">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 160px; height: 1px; background-color: #ffffff; position: relative;
                                                                top: 22px;">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" style="height: 20px; text-align: left">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" style="height: 26px; text-align: right">
                                                    &nbsp;</td>
                                            </tr>
                                        </table>
                                      </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="hidlblUnitit" runat="server" Value="<%$ Resources:BaseInfo,Menu_BrowsingTenantGroups %>" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QueryBuilding.aspx.cs" Inherits="RentableArea_Building_QueryBuilding" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Menu_BrowsingMallGeographicalInformation")%></title>
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
            
            t.add(id, pid, name, "", imgurl, true);
            
        }
    }
            t.opt.sort='no';
            t.opt.enbScroll=true;
            t.opt.height="270px";
            t.opt.width="235px";
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
            addTabTool("<%=strFresh %>,RentableArea/Building/QueryBuilding.aspx");
            //addTabTool(document.getElementById("hidlblUnitit").value + ",RentableArea/Building/QueryBuilding.aspx~<%=mallTitle %>,RentableArea/Building/MiInfoVindicateQuery.aspx");
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
                <asp:HiddenField id="depttxt" runat="server"></asp:HiddenField>
            <asp:HiddenField id="deptid" runat="server" ></asp:HiddenField>
                <asp:HiddenField ID="selectdeptid" runat="server" />
        <table border="0" cellpadding="0" cellspacing="0" style="height: 430px; width:100%;">
            <tr>
                <td style="width:49%; height: 402px; vertical-align: top; text-align: right;">
                    <table border="0" cellpadding="0" cellspacing="0" style="height: 330px; width:100%;">
                        <tr>
                            <td style="vertical-align:top; height: 22px;" >
                            
                         <table border="0" cellpadding="0" cellspacing="0" style="height: 22px; width:100%;   ">
                        
                        
                            <tr>
                                <td class="tdTopRightBackColor"    valign="top" style="width: 8px; height:27px;  text-align:left" >
                                    <img alt="" class="imageLeftBack" style=" text-align:left"  />
                                    </td>
                                    <td class="tdTopRightBackColor" style="height: 27px; text-align:left;">
                                <asp:Label ID="labBuildingTitle" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,RentableArea_lblBuildingtit %>"></asp:Label></td>
                              
                                <td class="tdTopRightBackColor"   valign="top" style="width: 20px; height: 27px;">
                                    <img class="imageRightBack" style="width: 7px; height: 22px" />
                                    </td>
                            </tr>
                        
                        </table>
                                </td>
                            
                        </tr>
                        <tr height="1">
                            <td style="height: 1px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" rowspan="10" style="height: 365px; text-align:left; width: 100%;"
                                valign="top">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 330px">
                                
                                
                          <tr>
                                <td style="height:8px; width:0px;">
                        
                                </td>
                                </tr>
                                
                                
                                
                                
                                
                                <tr>
                                <td style="height:5px; text-align: center; width: 100%;">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 96%">
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
                                        <td colspan="4" rowspan="9" style="height: 307px; text-align: center; width: 100%;" valign="top">
                         <table border="0" cellpadding="0" cellspacing="0" style="  width: 100%;">
                         <tr>
                         <td style="height:10px">
                         
                         </td>
                         </tr>
                        <tr>
                        
                            <td colspan="2" style="vertical-align: top; height: 300px; background-color: #e1e0b2;
                                text-align: center; top: 20px; width: 100%;">
                                <asp:Panel ID="Panel1" runat="server" BackColor="White" BorderStyle="Inset" BorderWidth="1px"
                                    Height="298px" ScrollBars="Auto" Width="250px">
                                    <table style="width: 100%; height: 263px">
                                        <tr>
                                            <td id="treeview" style="width: 174px; height: 174px; text-align:left;" valign="top">
                                            </td>
                                        </tr>
                                    </table>
                                    </asp:Panel>
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 95%">
                                        <tr>
                                            <td style="width: 160px; height: 1px; background-color: #738495; position: relative; top: 13px;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 160px; height: 1px; background-color: #ffffff; position: relative; top: 13px;">
                                            </td>
                                        </tr>
                                    </table>
                               
                               </td>
                        </tr>
                    </table>
     
                            </td>
                                  </tr>
                             
                                </table>
                                <asp:Button ID="treeClick" runat="server" CssClass="buttonHidden" 
                                    Width="24px" OnClick="treeClick_Click" /></td>
                        </tr>
                   
                    </table>
                </td>
                <td style="height: 402px; width:2%;">
                </td>
                <td colspan="3" style="height: 402px; width: 49%; vertical-align:top;">
                    <table border="0" cellpadding="0" cellspacing="0" style="height: 367px; width: 100%; ">
                        <tr>
                            <td style="vertical-align:top; height: 22px;" >
                            
                         <table border="0" cellpadding="0" cellspacing="0" style="height: 22px; width: 100%;   ">
                        
                        
                            <tr>
                                <td class="tdTopRightBackColor"    valign="top" style="width: 8px; height:27px;  text-align:left" >
                                    <img alt="" class="imageLeftBack" style=" text-align:left"  />
                                    </td>
                                    <td class="tdTopRightBackColor" style="height: 27px; text-align:left;">
                                <asp:Label ID="Label4" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,RentableArea_lblBuildingtitrig %>"></asp:Label></td>
                              
                                <td class="tdTopRightBackColor"   valign="top" style="width: 20px; height: 27px;">
                                    <img class="imageRightBack" style="width: 7px; height: 22px" />
                                    </td>
                            </tr>
                        
                        </table>
                                </td>
                        </tr>
                        <tr height="1">
                            <td colspan="4" style="height: 1px; width: 0px;">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" rowspan="10" style="height: 365px; text-align: center; width: 391px;"
                                valign="top"><table border="0" cellpadding="0" cellspacing="0" style="height: 300px; width: 290px; ">
                                    <tr>
                                        <td class="tdBackColor" colspan="4" rowspan="10" style="height: 365px; text-align: center; width: 100%;"
                                valign="top">
                <table style="height: 365px">
                <tr>
                <td style="width: 100%; height: 335px; vertical-align: top;">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 360px;">
                                    <tr>
                                        <td colspan="3" style="height:16px;width: 100%;">
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 95%">
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
                                        <td style="width: 61px; height: 30px; text-align: right">
                                            <asp:Label ID="lblBuildingCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblBuildingCode %>"
                                                Width="60px"></asp:Label></td>
                                        <td style="height: 30px; width: 13px;">
                                           </td>
                                        <td style="height: 30px; width: 154px;">
                                            <asp:TextBox ID="txtBuildingCode" runat="server" CssClass="Enabletextstyle" ReadOnly="True"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 61px; height: 30px; text-align: right">
                                            <asp:Label ID="lblBuildingName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblBuildingName %>" Width="57px"></asp:Label></td>
                                        <td style="height: 30px; width: 13px;">
                                           </td>
                                        <td style="height: 30px; width: 154px;">
                        <asp:TextBox ID="txtBuildingName" runat="server" CssClass="Enabletextstyle" ReadOnly="True"></asp:TextBox></td>
                                    </tr>
                                    
                                 <tr>
                                        <td style="width: 61px; height: 30px; text-align: right">
                                            <asp:Label ID="lblBuildingSatus" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblLocationStatus %>"
                                                Width="57px"></asp:Label></td>
                                        <td style="height: 30px; width: 13px;">
                                           </td>
                                        <td style="height: 30px; width: 154px;">
                                            <asp:DropDownList ID="cmbBuildingStatus" runat="server" CssClass="textstyle" Enabled="False">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td style="height: 9px; text-align: center" colspan="3"><table border="0" cellpadding="0" cellspacing="0" style="width: 201px">
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
                                        <td style="width: 61px; height: 30px; text-align: right">
                                            <asp:Label ID="lblFloorCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblFloorCode %>" Width="59px"></asp:Label></td>
                                        <td style="height: 30px; width: 13px;">
                                            </td>
                                        <td style="height: 30px; width: 154px;">
                                            <asp:TextBox ID="txtFloorCode" runat="server" CssClass="Enabletextstyle" ReadOnly="True"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 61px; height: 30px; text-align: right">
                                            <asp:Label ID="lblFloorName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblFloorName %>" Width="57px"></asp:Label></td>
                                        <td style="height: 30px; width: 13px;">
                                            </td>
                                        <td style="height: 30px; width: 154px;">
                                            <asp:TextBox ID="txtFloorName" runat="server" CssClass="Enabletextstyle" ReadOnly="True"></asp:TextBox></td>
                                    </tr>
                                                                        <tr>
                                        <td style="width: 61px; height: 30px; text-align: right">
                                            <asp:Label ID="lblFloorStatus" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblLocationStatus %>" Width="57px"></asp:Label></td>
                                        <td style="height: 30px; width: 13px;">
                                            </td>
                                        <td style="height: 30px; width: 154px;">
                                            <asp:DropDownList ID="cmbFloorStatus" runat="server" CssClass="textstyle" Enabled="False">
                                            </asp:DropDownList></td>
                                    </tr>
                                                                        <tr>
                                                                            <td colspan="3" style="height: 9px"><table border="0" cellpadding="0" cellspacing="0" style="width: 201px">
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
                                        <td style="width: 61px; height: 30px; text-align: right">
                                            <asp:Label ID="lblLocationCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblLocationCode %>" Width="57px"></asp:Label></td>
                                        <td style="height: 30px; width: 13px;">
                                            </td>
                                        <td style="height: 30px; width: 154px;">
                                            <asp:TextBox ID="txtLocationCode" runat="server" CssClass="Enabletextstyle" ReadOnly="True"></asp:TextBox></td>
                                    </tr>
                                                                        <tr>
                                        <td style="width: 61px; height: 30px; text-align: right">
                                            <asp:Label ID="lblLocationName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblLocationName %>"
                                                Width="59px"></asp:Label></td>
                                        <td style="height: 30px; width: 13px;">
                                            </td>
                                        <td style="height: 30px; width: 154px;">
                                            <asp:TextBox ID="txtLocationName" runat="server" CssClass="Enabletextstyle" ReadOnly="True"></asp:TextBox></td>
                                    </tr>
                                                                                                            <tr>
                                        <td style="width: 61px; height: 30px; text-align: right">
                                            <asp:Label ID="lblLocationStatus" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblLocationStatus %>"
                                                Width="59px"></asp:Label></td>
                                        <td style="height: 30px; width: 13px;">
                                            </td>
                                        <td style="height: 30px; width: 154px;">
                                            <asp:DropDownList ID="cmbLocationStatus" runat="server" CssClass="textstyle" Enabled="False">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" rowspan="2" style="height: 15px"><table border="0" cellpadding="0" cellspacing="0" style="width: 201px">
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
                                    </tr>
                                    <tr>
                                        <td colspan="3" rowspan="2" style="height: 17px; text-align:right;">
                                            </td>
                                    </tr>
                                    <tr>
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
           
          
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hidlblUnitit" runat="server" Value="<%$ Resources:BaseInfo,Menu_BrowsingMallGeographicalInformation %>" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>


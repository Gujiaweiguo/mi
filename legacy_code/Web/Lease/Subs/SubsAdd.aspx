<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SubsAdd.aspx.cs" Inherits="Lease_Subs_SubsAdd"  %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Subs_SubCompanyManage")%></title>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link rel="StyleSheet" href="../../App_Themes/nlstree/nlstree-basic.css" type="text/css" />
    <link rel="StyleSheet" href="../../App_Themes/nlstree/nlsctxmenu.css" type="text/css" />
    
    <script type="text/javascript"  src="../../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"></script>
	<script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
    <script type="text/javascript" src="../../App_Themes/nlstree/nlstree.js"></script>
    <script type="text/javascript" src="../../App_Themes/nlstree/nlsctxmenu.js"></script>
	<script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
<script type="text/javascript">
    function Load()
    {
        addTabTool("<%=strFresh %>,Lease/subs/SubsAdd.aspx");
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
            t.opt.height="260px";
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
}



function ev_click(e, id)
{
	
    document.form1.deptid.value=id;
    document.form1.Button1.click(); 
     
} 
		-->
		
</script>
	

		<script type="text/javascript">
	<!--
		
	    function styletabbar()
	    {
	       
	        tabbar=new dhtmlXTabBar("a_tabbar","top");
            tabbar.setImagePath("../../App_Themes/Tabbar/imgs/");
            
            tabbar.setSkinColors("#FCFBFC","#F4F3EE","#e1e0b2");
         
            tabbar.addTab("PotCustomer_a",document.getElementById("hidConcessionAuth").value,"80px");
            tabbar.addTab("PotCustomer_b",document.getElementById("hidContractAuth").value,"80px");
            tabbar.addTab("PotCustomer_c",document.getElementById("hidTradeAuth").value,"80px");
            tabbar.addTab("PotCustomer_d",document.getElementById("hidOther").value,"80px");
     
            tabbar.setTabActive("PotCustomer_a");
			
            tabbar.setContent("PotCustomer_a","a1");
            tabbar.setContent("PotCustomer_b","a2");
            tabbar.setContent("PotCustomer_c","a3");
            tabbar.setContent("PotCustomer_d","a4");
            
             treearray();
             
            addTabTool("null");
	        loadTitle();
	    }
	    function styletabbar_atv()
	      
	    {
	   
	        if(  typeof(a_tabbar)=="undefined")
	        {
	        }
	        else
	        { 
	          
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
	-->
        </script>
        
</head>
<body onload="treearray();Load();" topmargin=0 leftmargin=0>
    <form id="form1" runat="server">
 <asp:ScriptManager id="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel id="UpdatePanel1" runat="server"     >
        <ContentTemplate  >
       <asp:HiddenField ID="depttxt" runat="server" />
            <asp:HiddenField ID="selectdeptid" runat="server" />
                        <asp:HiddenField ID="deptid" runat="server" />
            <table border="0" cellpadding="0" cellspacing="0" style="height: 430px">
                    <td style="width: 43%; height: 401px;   vertical-align: top;"   >
                    
                        <table border="0" cellpadding="0" cellspacing="0" style="height:400px; width: 100%; vertical-align:top " id="TABLE1">
                       
                        <tr>
                        <td    style="vertical-align:top; width: 100%;" >
                        
                         <table border="0" cellpadding="0" cellspacing="0" style="height: 22px; width: 100%;   ">
                        
                        
                            <tr>
                                <td class="tdTopRightBackColor"    valign="top" style="width: 8px; height:27px;  text-align:left" >
                                    <img alt="" class="imageLeftBack" style=" text-align:left"  />
                                    </td>
                                    <td class="tdTopRightBackColor" style="width: 226px; height: 27px; text-align:left;">
                                    <asp:Label
                                        ID="Label1" runat="server" Text='<%$ Resources:BaseInfo,Subs_SubCompanyCurrentlyBrowse %>' Height="12pt" Width="218px"></asp:Label></td>
                              
                                <td class="tdTopRightBackColor"   valign="top" style="width: 20px; height: 27px;">
                                    
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
                                <td class="tdBackColor"   style="width: 100%; height: 15px; text-align: center;">
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 80%;text-align: center;">
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
                                <td class="tdBackColor" valign="top" style="text-align:center; height: 318px; width: 100%;">
                                    
                                    <asp:Panel ID="Panel1" runat="server" BackColor="White" BorderStyle="Inset" BorderWidth="1px"
                                        Font-Size="Medium" Height="270px" HorizontalAlign="Left" ScrollBars="Auto" Width="240px">
                                    <table style="width: 170px; height: 248px">
                                        <tr>
                                    <td valign="top" id ="treeview" style="height: 166px; width: 109px;">
                                             
                                         
                                    </td>
                                        </tr>
                                    </table>
                                    </asp:Panel>
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 80%" >
                                        <tr >
                                            <td style="width: 160px; height: 1px; background-color: #738495; position: relative; top: 14px;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 160px; height: 1px; background-color: #ffffff; position: relative; top: 14px;">
                                            </td>
                                        </tr>
                                    </table>
                                    <table style="vertical-align:top; position: relative;">
                                    <tr>
                                    <td style="height: 33px; width: 212px; text-align:right; position: relative; top: 30px;">
                                        &nbsp;</td>
                                    <td style="width:20px; height: 33px;">
                                    </td>
                                    </tr>
                                    </table>
                                   
                                    </td>
                            </tr>
                            
                            <tr>
                                <td class="tdBackColor"    style="  text-align:right; vertical-align: top; width: 247px; height: 40px;"
                                    valign="top" id="treetest">
                                    
                                        
                                        </td>
                                     
                                </tr>
     
                                <tr height="1">
                                <td   style="height: 3px; width: 247px;">
                                </td>
                            </tr>
     
                        </table>
                       
                    </td>
                    <td style="width: 2%; height: 401px">
                    </td>
                    <td colspan="3" style="width: 55%; height: 401px; vertical-align:top;">
                       
                           <table border="0" cellpadding="0" cellspacing="0" style=" width:100%; height: 399px;" id="aa">
                        
                                                                                                       
                               <tr>
                                <td colspan="8" valign="top" style="height: 22px; vertical-align: top;" rowspan="">
                                   <table border="0" cellpadding="0" cellspacing="0"style="height: 22px; width: 100%;">
                        <tr>
                                  <td valign="top" style="height: 27px ; text-align:left; width: 8px;" class="tdTopRightBackColor">
                                    <img alt="" class="imageLeftBack" style="height: 22px" />
                                    </TD>
                                    <td style="height: 27px;WIDTH:378px; text-align: left;" class="tdTopRightBackColor">
                                    
                                    <asp:Label ID="labDeptAdd" runat="server" CssClass="lblTitle"  ForeColor="White"></asp:Label><asp:Label
                                        ID="treetext" runat="server" CssClass="lblTitle"></asp:Label><asp:Label ID="labDeptContext" runat="server" CssClass="lblTitle" Text='<%$ Resources:BaseInfo,Subs_SubCompanyDefinition %>' ForeColor="White"></asp:Label></td>
                                   <td style="height: 27px ; text-align:RIGHT; width: 3px;" class="tdTopRightBackColor">
                                    <img class="imageRightBack" style="height: 22px" /> </td>
                                    
                        </tr>
                                    </table>
                                    
                                   </td>
                                    
                                
                            </tr>
                       <tr height="1">
                            <td style="height: 1px">
                            </td>
                        </tr>
                            
                            
                            <tr>
                                <td class="tdBackColor" colspan="8" style="width: 495px; height: 15px; text-align: center;">
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 369px">
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
                                <td  class="tdBackColor" colspan="8" style="width: 495px; height: 2px">
                                </td>
                            </tr>
                            
                            
                            
                            <tr>
                                <td class="tdBackColor" style="width: 151px; height: 22px; text-align: right;">
                                            <asp:Label ID="lblDeptCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Subs_SubCompanyCode %>"
                                                Width="60px"></asp:Label>&nbsp;</td>
                                <td class="tdBackColor" style="width: 5px; height: 22px">
                                </td>
                                <td class="tdBackColor" style="width: 200px; height: 22px">
                                            <asp:TextBox ID="txtCode" runat="server" CssClass="Enabledipt160px"
                                                Width="100px" ReadOnly="True" MaxLength="8"></asp:TextBox></td>
                                <td class="tdBackColor" style="height: 22px">
                                </td>
                                <td class="tdBackColor" style="width: 151px; height: 22px; text-align: center; vertical-align: middle;">
                                    <asp:Label ID="lblDeptLevel" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Subs_SubCompanyShortName %>" Width="60px"></asp:Label>&nbsp;</td>
                                <td class="tdBackColor" style="width: 5px; height: 22px">
                                </td>
                                <td class="tdBackColor" style="width: 203px; height: 22px; vertical-align: top;">
                                            <asp:TextBox ID="txtCompanyShortName" runat="server" CssClass="Enabledipt160px" MaxLength="18" Width="100px" ReadOnly="True"></asp:TextBox></td>
                                <td class="tdBackColor" style="width: 50px; height: 22px">
                                    </td>
                            </tr>
                            
                                                        <tr>
                                <td class="tdBackColor" style="width: 151px; height: 22px; text-align: right;">
                                    <asp:Label ID="lblDeptName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Rpt_InvComTitile %>" Width="60px"></asp:Label>&nbsp;</td>
                                <td class="tdBackColor" style="width: 5px; height: 22px">
                                </td>
                                <td class="tdBackColor" style="height: 22px" colspan="5">
                                    <asp:TextBox ID="txtCompanyName" runat="server" CssClass="ipt160px"
                                        MaxLength="32" Width="283px"></asp:TextBox></td>
                                <td class="tdBackColor" style="width: 50px; height: 22px">
                                    </td>
                            </tr>
                            
                                                        <tr>
                                <td class="tdBackColor" style="width: 151px; height: 24px; text-align: right;">
                                    <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblBankName %>"
                                        Width="60px"></asp:Label>&nbsp;</td>
                                <td class="tdBackColor" style="width: 5px; height: 24px">
                                </td>
                                <td class="tdBackColor" style="height: 24px" colspan="5">
                                    <asp:TextBox ID="txtBank" runat="server" CssClass="ipt160px" MaxLength="32" Width="283px"></asp:TextBox></td>
                                <td class="tdBackColor" style="width: 50px; height: 24px">
                                    </td>
                            </tr>
                               <tr>
                                   <td class="tdBackColor" style="width: 151px; height: 24px; text-align: right">
                                       <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblBankAcct %>"
                                           Width="60px"></asp:Label>&nbsp;</td>
                                   <td class="tdBackColor" style="width: 5px; height: 24px">
                                   </td>
                                   <td class="tdBackColor" colspan="5" style="height: 24px">
                                       <asp:TextBox ID="txtBankAccount" runat="server" CssClass="ipt160px" MaxLength="32" Width="283px"></asp:TextBox></td>
                                   <td class="tdBackColor" style="width: 50px; height: 24px">
                                   </td>
                               </tr>
                               <tr>
                                   <td class="tdBackColor" style="width: 151px; height: 24px; text-align: right">
                                    <asp:Label ID="lblDeptType" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Subs_SubCompanyFinanceType %>" Width="60px"></asp:Label>&nbsp;</td>
                                   <td class="tdBackColor" style="width: 5px; height: 24px">
                                   </td>
                                   <td class="tdBackColor" style="width: 200px; height: 24px">
                                            <asp:DropDownList ID="ddlFinType" runat="server" CssClass="ipt160px"
                                                
                                                Width="103px">
                                            </asp:DropDownList></td>
                                   <td class="tdBackColor" style="height: 24px">
                                   </td>
                                   <td class="tdBackColor" style="width: 85px; height: 14px; text-align: right">
                                            <asp:Label ID="lblStatus" runat="server" Text="<%$ Resources:BaseInfo,Dept_lblStatus %>"
                                                Width="60px" CssClass="labelStyle"></asp:Label>&nbsp;</td>
                                   <td class="tdBackColor" style="width: 5px; height: 14px">
                                   </td>
                                   <td class="tdBackColor" style="width: 203px; height: 14px">
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="ipt160px" Width="103px">
                                            </asp:DropDownList></td>
                                   <td class="tdBackColor" style="width: 50px; height: 24px">
                                   </td>
                               </tr>
                            
                            
                            <tr>
                                <td class="tdBackColor" colspan="8" style="width: 495px; height: 43px; text-align: center;">
                                    &nbsp;<table>
                                        <tr>
                                    <td style="  text-align:right; vertical-align:top; height:45px; width: 310px; margin-left: 100px;">
                                    <asp:Button ID="btnSave"
                                            runat="server" CssClass="buttonSave" OnClick="btnSave_Click" Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                                    <asp:Button ID="Button1" runat="server" CssClass="buttonHidden" OnClick="Button1_Click" Width="1px" />
                                        &nbsp;
                                        <asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel" OnClick="btnCancel_Click"
                                        Text="<%$ Resources:BaseInfo,User_btnCancel %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>&nbsp; &nbsp;&nbsp;
                                    </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdBackColor" colspan="8" style="width: 495px; height: 168px; text-align: left;" valign="top">
                                    
                                </td>
                            </tr>
                        </table>
                    </td>
            </table>
      
    <%--     <iframe width="248px" height="401px"  id="aaaaaa" frameborder="0" src="../../Default4.aspx" scrolling="No">
            
         </iframe>--%>
          
        </ContentTemplate>
    </asp:UpdatePanel>
        <asp:HiddenField id="HiddenField1" runat="server"></asp:HiddenField>
            <asp:HiddenField id="HiddenField2" runat="server" ></asp:HiddenField>
                <asp:HiddenField ID="HiddenField3" runat="server" />
        <asp:HiddenField ID="hidSelect" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidSelect %>" />
        <asp:HiddenField ID="hidUpdate" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidUpdate %>" />
        <asp:HiddenField ID="hidAdd" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidAdd %>" />
        <asp:HiddenField ID="hidUpdateLost" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidUpdateLost %>" />
        <asp:HiddenField ID="hidInsert" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidInsert %>" />
        <asp:HiddenField ID="hidMessage" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidMessage %>" />
    </form>
</body>
    
</html>
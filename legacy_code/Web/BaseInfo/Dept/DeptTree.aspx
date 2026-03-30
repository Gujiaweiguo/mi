<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DeptTree.aspx.cs" Inherits="DeptTree"  %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Menu_DefineMallDepartments")%></title>
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
            t.opt.height="280px";
            t.opt.width="280px";
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
	          if (a_tabbar.innerHTML=="")
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
                }
              }
          
        }
          
            function allShopChange(){
                document.getElementById("txtConcessionAuth").readOnly=true;                
                document.getElementById("txtConcessionAuth").value="";
                return false;
            }
           function ShopChange(){
                document.getElementById("txtConcessionAuth").readOnly=false;                
                return false;
            }
            
           function AllTreaty(){
                document.getElementById("txtContractAuth").readOnly=true;                
                document.getElementById("txtContractAuth").value="";
                return false;
            }
           function Treaty(){
                document.getElementById("txtContractAuth").readOnly=false;                
                return false;
            }
            
           function AllVocation(){
                document.getElementById("txtTradeAuth").readOnly=true;                
                document.getElementById("txtTradeAuth").value="";
                return false;
            }
           function Vocation(){
                document.getElementById("txtTradeAuth").readOnly=false;                
                return false;
            }
            
            function NoRrestrict(){
                document.getElementById("txtOtherAuth").readOnly=true;                
                document.getElementById("txtOtherAuth").value="";
                return false;
            }
           function Rrestrict(){
                document.getElementById("txtOtherAuth").readOnly=false;                
                return false;
            }
            //text控件文本验证
            function allTextBoxValidator(sForm)
            {
                if(isEmpty(document.all.txtDeptCode.value))  
                {
                    parent.document.all.txtWroMessage.value='<%=(String)GetGlobalResourceObject("BaseInfo", "Dept_lblDeptCode") %>'+document.getElementById("hidMessage").value;
                    document.all.txtDeptCode.focus();
                    return false;					
                }
                
                if(isEmpty(document.all.txtDeptName.value))  
                {
                    parent.document.all.txtWroMessage.value='<%=(String)GetGlobalResourceObject("BaseInfo", "Dept_lblDeptName") %>'+document.getElementById("hidMessage").value;
                    document.all.txtDeptName.focus();
                    return false;					
                }
                 if(isInteger(document.all.txtOrderID.value)==false)
                {
                    parent.document.all.txtWroMessage.value="please input number.";
                    //alert("please input number."); 
                    document.all.txtOrderID.value="";
                    document.all.txtOrderID.focus();         
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
            function showline()
            {
                parent.document.all.txtWroMessage.value = "";
                addTabTool("<%=baseInfo %>,BaseInfo/Dept/DeptTree.aspx");
                loadTitle();
            }
	-->
        </script>
        
</head>
<body onload='treearray();showline();' topmargin=0 leftmargin=0>
    <form id="form1" runat="server">
 <asp:ScriptManager id="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel id="UpdatePanel1" runat="server"     >
        <ContentTemplate  >
       <asp:HiddenField ID="depttxt" runat="server" />
            <asp:HiddenField ID="selectdeptid" runat="server" />
                        <asp:HiddenField ID="deptid" runat="server" OnValueChanged="deptid_ValueChanged" />
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
                                        ID="Label1" runat="server" Text='<%$ Resources:BaseInfo,Dept_Title %>' Height="12pt" Width="218px"></asp:Label></td>
                              
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
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 85%;text-align: center;">
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
                                        Font-Size="Medium" Height="300px" HorizontalAlign="Left" ScrollBars="Auto" Width="300px">
                                    <table style="width: 170px; height: 248px">
                                        <tr>
                                    <td valign="top" id ="treeview" style="height: 166px; width: 159px;">
                                             
                                         
                                    </td>
                                        </tr>
                                    </table>
                                    </asp:Panel>
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 85%;text-align: center;" >
                                        <tr >
                                            <td style="width: 160px; height: 1px; background-color: #738495; position: relative; top: 8px;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 160px; height: 1px; background-color: #ffffff; position: relative; top: 8px;">
                                            </td>
                                        </tr>
                                    </table>
                                    <table style="vertical-align:top; position: relative;">
                                    <tr>
                                    <td style="height: 33px; width: 240px; text-align:right; position: relative; top: 8px;">
                                     <asp:Button ID="btnAdd" runat="server" CssClass="buttonAdd" OnClick="btnAdd_Click"
                                        Text="<%$ Resources:BaseInfo,DeptTree_labDeptAdd %>" EnableTheming="True" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                                    </td>
                                    <td style="width:20px; height: 33px;">
                                    </td>
                                    </tr>
                                    </table>
                                   
                                    </td>
                            </tr>
                            
                            <tr>
                                <td class="tdBackColor"    style="  text-align:right; vertical-align: top; width: 247px; height: 16px;"
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
                       
                           <table border="0" cellpadding="0" cellspacing="0" style="height: 300px; width:100%;">
                        
                                                                                                       
                               <tr>
                                <td colspan="8" valign="top" style="height: 22px; vertical-align: top;" rowspan="">
                                   <table border="0" cellpadding="0" cellspacing="0"style="height: 22px; width: 100%;">
                        <tr>
                                  <td valign="top" style="height: 27px ; text-align:left; width: 8px;" class="tdTopRightBackColor">
                                    <img alt="" class="imageLeftBack" style="height: 22px" />
                                    </TD>
                                    <td style="height: 27px;WIDTH:378px; text-align: left;" class="tdTopRightBackColor">
                                    
                                    <asp:Label ID="labDeptAdd" runat="server" CssClass="lblTitle"  ForeColor="White"></asp:Label><asp:Label
                                        ID="treetext" runat="server" CssClass="lblTitle"></asp:Label><asp:Label ID="labDeptContext" runat="server" CssClass="lblTitle" Text='<%$ Resources:BaseInfo,DeptTree_labDeptContext %>' ForeColor="White"></asp:Label></td>
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
                                <td  class="tdBackColor" colspan="8" style="width: 495px; height: 4px">
                                </td>
                            </tr>
                            
                            
                            
                            <tr>
                                <td class="tdBackColor" style="width: 151px; height: 22px; text-align: right;">
                                            <asp:Label ID="lblDeptCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Dept_lblDeptCode %>"
                                                Width="60px"></asp:Label>&nbsp;</td>
                                <td class="tdBackColor" style="width: 5px; height: 22px">
                                </td>
                                <td class="tdBackColor" style="width: 200px; height: 22px">
                                            <asp:TextBox ID="txtDeptCode" runat="server" CssClass="Enabledipt160px"
                                                Width="100px" ReadOnly="True" MaxLength="12"></asp:TextBox></td>
                                <td class="tdBackColor" style="height: 22px">
                                </td>
                                <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right;">
                                    <asp:Label ID="lblDeptName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Dept_lblDeptName %>" Width="60px"></asp:Label>&nbsp;</td>
                                <td class="tdBackColor" style="width: 5px; height: 22px">
                                </td>
                                <td class="tdBackColor" style="width: 203px; height: 22px; vertical-align: top;">
                                            <asp:TextBox ID="txtDeptName" runat="server" CssClass="Enabledipt160px" MaxLength="32"
                                                ReadOnly="True" Width="99px"></asp:TextBox></td>
                                <td class="tdBackColor" style="width: 50px; height: 22px">
                                    </td>
                            </tr>
                            
                                                        <tr>
                                <td class="tdBackColor" style="width: 151px; height: 22px; text-align: right;">
                                    <asp:Label ID="lblDeptLevel" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Dept_lblDeptLevel %>" Width="60px"></asp:Label>&nbsp;</td>
                                <td class="tdBackColor" style="width: 5px; height: 22px">
                                </td>
                                <td class="tdBackColor" style="width: 200px; height: 22px">
                                            <asp:TextBox ID="txtDeptLevel" runat="server" CssClass="Enabledipt160px" MaxLength="18"
                                                ReadOnly="True" Width="100px"></asp:TextBox></td>
                                <td class="tdBackColor" style="height: 22px">
                                </td>
                                <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right;">
                                    <asp:Label ID="lblDeptType" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Dept_lblDeptType %>" Width="60px"></asp:Label>&nbsp;</td>
                                <td class="tdBackColor" style="width: 5px; height: 22px">
                                </td>
                                <td class="tdBackColor" style="width: 203px; height: 22px">
                                            <asp:DropDownList ID="ddlstDeptType" runat="server" CssClass="ipt160px"
                                                
                                                Width="103px" Enabled="False" OnSelectedIndexChanged="ddlstDeptType_SelectedIndexChanged">
                                            </asp:DropDownList></td>
                                <td class="tdBackColor" style="width: 50px; height: 22px">
                                    </td>
                            </tr>
                            
                                                        <tr>
                                <td class="tdBackColor" style="width: 151px; height: 24px; text-align: right;">
                                    <asp:Label ID="lblIndepBalance" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Dept_lblIndepBalance %>" Width="60px"></asp:Label>&nbsp;</td>
                                <td class="tdBackColor" style="width: 5px; height: 24px">
                                </td>
                                <td class="tdBackColor" style="width: 200px; height: 24px">
                                            <asp:DropDownList ID="cmbIndepBalance" runat="server" CssClass="ipt160px" Width="103px" Enabled="False" >
                                            </asp:DropDownList></td>
                                <td class="tdBackColor" style="height: 24px">
                                </td>
                                <td class="tdBackColor" style="width: 85px; height: 24px; text-align: right;">
                                            <asp:Label ID="lblStatus" runat="server" Text="<%$ Resources:BaseInfo,Dept_lblStatus %>"
                                                Width="60px" CssClass="labelStyle"></asp:Label>&nbsp;</td>
                                <td class="tdBackColor" style="width: 5px; height: 24px">
                                </td>
                                <td class="tdBackColor" style="width: 203px; height: 24px">
                                            <asp:DropDownList ID="cmbDeptStatus" runat="server" CssClass="ipt160px" Width="103px" Enabled="False">
                                            </asp:DropDownList></td>
                                <td class="tdBackColor" style="width: 50px; height: 24px">
                                    </td>
                            </tr>
                               <tr>
                                   <td class="tdBackColor" style="width: 151px; height: 24px; text-align: right">
                                       <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Dept_lblOrderID %>"
                                           Width="60px"></asp:Label>&nbsp;</td>
                                   <td class="tdBackColor" style="width: 5px; height: 24px">
                                   </td>
                                   <td class="tdBackColor" style="width: 200px; height: 24px">
                                       <asp:TextBox ID="txtOrderID" runat="server" CssClass="Enabledipt160px" MaxLength="12"
                                           ReadOnly="True" Width="100px"></asp:TextBox></td>
                                   <td class="tdBackColor" style="height: 24px">
                                   </td>
                                   <td class="tdBackColor" style="width: 85px; height: 24px; text-align: right">
                                   </td>
                                   <td class="tdBackColor" style="width: 5px; height: 24px">
                                   </td>
                                   <td class="tdBackColor" style="width: 203px; height: 24px">
                                   </td>
                                   <td class="tdBackColor" style="width: 50px; height: 24px">
                                   </td>
                               </tr>
                            
                            
                            <tr>
                                <td class="tdBackColor" colspan="8" style="width: 495px; height: 43px; text-align: center;">
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 366px">
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
                                <td class="tdBackColor" colspan="8" style="width: 495px; height: 215px; text-align: left;" valign="top">
                                    <div id="a_tabbar" style="width: 392px; height: 191px" align="center">
                                    <table>
                                    <tr>
                                    <td style="  text-align:right; vertical-align:top; height:45px; width: 310px; margin-left: 100px;">
                                    <asp:Button ID="Button1" runat="server" CssClass="buttonHidden" OnClick="Button1_Click" Width="1px" /><asp:Button ID="btnEdit" runat="server" CssClass="buttonEdit" OnClick="btnEdit_Click"
                                        Text="<%$ Resources:BaseInfo,User_btnChang %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                                        &nbsp;&nbsp;<asp:Button ID="btnSave"
                                            runat="server" CssClass="buttonSave" OnClick="btnSave_Click" Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" Enabled="False" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>&nbsp;
                                        <asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel" OnClick="btnCancel_Click"
                                        Text="<%$ Resources:BaseInfo,User_btnCancel %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>&nbsp; &nbsp;
                                    </td>
                                    </tr>
                                    </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
            </table>
      
    <%--     <iframe width="248px" height="401px"  id="aaaaaa" frameborder="0" src="../../Default4.aspx" scrolling="No">
            
         </iframe>--%>
          
        </ContentTemplate>
    </asp:UpdatePanel>
        <asp:HiddenField ID="hidSelect" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidSelect %>" />
        <asp:HiddenField ID="hidCondition" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidCondition %>" />
        <asp:HiddenField ID="hidInsert" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidInsert %>" />
        <asp:HiddenField ID="hidUpdate" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidUpdate %>" />
        <asp:HiddenField ID="hidAdd" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidAdd %>" />   
        <asp:HiddenField ID="hidMessage" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidMessage %>" />
        <asp:HiddenField ID="hidConcessionAuth" runat="server" Value="<%$ Resources:BaseInfo,Dept_lblConcessionAuth %>" />
        <asp:HiddenField ID="hidOther" runat="server" Value="<%$ Resources:BaseInfo,Dept_Other %>" />
        <asp:HiddenField ID="hidContractAuth" runat="server" Value="<%$ Resources:BaseInfo,Dept_lblContractAuth %>" />
        <asp:HiddenField ID="hidTradeAuth" runat="server" Value="<%$ Resources:BaseInfo,Dept_lblTradeAuth %>" />
        <asp:HiddenField ID="hidUserBeing" runat="server" Value="<%$ Resources:BaseInfo,Hidden_DeptCodeBeing %>" />
        <asp:HiddenField ID="hidNotSelect" runat="server" Value="You can't select this department type!" />
    </form>
</body>
    
</html>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MainTree.aspx.cs" Inherits="BaseInfo_User_MainTree" StylesheetTheme="Enhanced" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    <link runat="server" rel="stylesheet" href="~/CSS/Import.css" type="text/css" id="AdaptersInvariantImportCSS" />
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
            <script  type="text/javascript"> 
            function Onclick()
            {  
            window.document.getElementById("Button1").click();   

//                document.getElementById("Button1")click();
             }
             var imgflag1="1";
             var imgflag2="0";
             var imgflag3="0";
             var imgflag4="0";
             var imgflag5="0";
             var imgflag6="0";
             function chgimg1(){
                if(imgflag1=="0"){
                   imgflag1="1";
                   document.getElementById("imgscr1").src="../../App_Themes/CSS/Images/1_2.GIF" ;
                }else{
                
                   imgflag1="0";
                   document.getElementById("imgscr1").src="../../App_Themes/CSS/Images/1_1.GIF";
                }
             }
             
             function chgimg2(){
                if(imgflag2=="0"){
                   imgflag2="1";
                   document.getElementById("imgscr2").src="../../App_Themes/CSS/Imagess/1_2.GIF" ;
                }else{
                
                   imgflag2="0";
                   document.getElementById("imgscr2").src="../../App_Themes/CSS/Images/1_1.GIF";
                }
             }
             function chgimg3(){
                if(imgflag3=="0"){
                   imgflag3="1";
                   document.getElementById("imgscr3").src="../../App_Themes/CSS/Images/3_2.GIF" ;
                }else{
                
                   imgflag3="0";
                   document.getElementById("imgscr3").src="../../App_Themes/CSS/Images/3_1.GIF";
                }
             }
            function chgimg4(){
                if(imgflag4=="0"){
                   imgflag4="1";
                   document.getElementById("imgscr4").src="../../App_Themes/CSS/Images/4_2.GIF" ;
                }else{
                
                   imgflag4="0";
                   document.getElementById("imgscr4").src="../../App_Themes/CSS/Images/4_1.GIF";
                }
             }
             function chgimg5(){
                if(imgflag5=="0"){
                   imgflag5="1";
                   document.getElementById("imgscr5").src="../../App_Themes/CSS/Images/5_2.GIF" ;
                }else{
                
                   imgflag5="0";
                   document.getElementById("imgscr5").src="../../App_Themes/CSS/Images/5_1.GIF";
                }
             }
             function chgimg6(){
                if(imgflag6=="0"){
                   imgflag6="1";
                   document.getElementById("imgscr6").src="../../App_Themes/CSS/Images/6_2.GIF" ;
                }else{
                
                   imgflag6="0";
                   document.getElementById("imgscr6").src="../../App_Themes/CSS/Images/6_1.GIF";
                }
             }
             
            </script>
</head>
<body>
    <form id="form1" runat="server">


        <div style=" border-bottom-style:solid; border-left-style:solid; border-right-style:solid; border-left:2px; border-right:2px; border-bottom:2px; border-color:#909090; height:450px;">

        <asp:ScriptManager ID="ScriptManager1" runat="server"/>

                <table style="margin-left:8px; width:184px; height:5px;" border="0"; cellpadding="1"; cellspacing="1";>
           <tr>
                <td>
                
                </td>
  
            </tr>
        </table>
        <table style=" height:8px;margin-left:8px; width:184px;"border="0"; cellpadding="0"; cellspacing="0";>
            <tr style="width: 172px;height:8px">
                <td style="background-color: #98B1C4;">
                </td>
            </tr>
        </table>
        
                <table style="margin-left:8px; width:184px; height:1px;" border="0"; cellpadding="0"; cellspacing="0";>
           <tr>
                <td>
               <%-- <div style="margin-left:8px;width: 184px;">--%>
        <ajaxToolkit:Accordion ID="MyAccordion" runat="server" SelectedIndex="0"
            HeaderCssClass="accordionHeader" ContentCssClass="accordionContent"
            FadeTransitions="false" FramesPerSecond="40" TransitionDuration="250"
            AutoSize="None" RequireOpenedPane="false" SuppressHeaderPostbacks="true" OnDataBinding="MyAccordion_DataBinding" OnItemCommand="MyAccordion_ItemCommand">
           <Panes>
               <ajaxToolkit:AccordionPane ID="AccordionPane5" runat="server">
                <Header>
                    <img id="imgscr1" src="../../App_Themes/CSS/Images/1_2.GIF" onclick="chgimg1()" /></Header>
                <Content>
                 <asp:treeview id="TreeView1" runat="server" CssSelectorClass="PrettyTree" skinid="SampleTreeView">
                 </asp:treeview>
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane1" runat="server">
                <Header><img id="imgscr2" src="../../App_Themes/CSS/Images/2_1.GIF" onclick="chgimg2()" /></Header>
                <Content>

                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane2" runat="server">
                <Header><img id="imgscr3" src="../../App_Themes/CSS/Images/3_1.GIF" onclick="chgimg3()" /></Header>
                <Content>
                    
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane3" runat="server">
                <Header><img id="imgscr4" src="../../App_Themes/CSS/Images/4_1.GIF" onclick="chgimg4()" /></Header>
                <Content>
                    
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane4" runat="server">
                <Header><img id="imgscr5" src="../../App_Themes/CSS/Images/5_1.GIF" onclick="chgimg5()" /></Header>
                <Content>
                </Content>
            </ajaxToolkit:AccordionPane>
               <ajaxToolkit:AccordionPane ID="AccordionPane6" runat="server">
                   <Header>
                       <img id="imgscr6" onclick="chgimg6()" src="../../App_Themes/CSS/Images/6_1.GIF" /></Header>
                   <Content>
                   </Content>
               </ajaxToolkit:AccordionPane>
            </Panes>
        </ajaxToolkit:Accordion>
        <%--</div>--%>
             </td>
            </tr>
        </table>
        <table style="width:184px; height:2px;" border="0"; cellpadding="0"; cellspacing="0";>
            <tr style="width: 184px;">
                <td style="background-color: #FFFFFF; width: 194px;border:0px;" >
                </td>
             </tr>
        </table>
       <table style="width:184; height:19px;margin-left:8px;" border="0"; cellpadding="0"; cellspacing="0";>
            <tr style="width: 184px;">
                <td style="background-color: #C7D7E4; width: 179px;border:0px;" >
                
                </td>
                <td style="width: 5px; background-image: url(../../App_Themes/Main/Images/DownRight.jpg); border:0px;">
                 
                </td>
            </tr>
        </table>
       
            </div>
  
    </form>
</body>
</html>

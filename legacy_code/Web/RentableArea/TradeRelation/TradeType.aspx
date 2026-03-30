<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TradeType.aspx.cs" Inherits="RentableArea_TradeRelation_TradeType" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
     <style type="text/css">
    <!--
        table.tblRight tr{ height:40px; }
        table.tblRight tr.headLine{ height:1px; }
        table.tblRight tr.bodyLine{ height:1px; }
        table.tblRight td.baseLable{ padding-right:3px;text-align:right;}
    -->
    </style>
    <script type="text/javascript" src="../../App_Themes/nlstree/nlstree.js"></script>
    <script type="text/javascript" src="../../App_Themes/nlstree/nlsctxmenu.js"></script>
     <script type="text/javascript">
        <!--
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
                                t.opt.height="235px";
                                t.opt.width="252px";
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
<body onload='treearray();'>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
               <div>
                    <asp:HiddenField ID="depttxt" runat="server" />
                    <asp:HiddenField ID="deptid" runat="server" />
                    <asp:HiddenField ID="selectdeptid" runat="server" />
                    <table border="0" cellpadding="0" cellspacing="0" class="tableBoderStyle" style="height: 445px">
                        <tr height="5">
                            <td colspan="8">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 60px; height: 401px; text-align: center" valign="bottom">
                                <img height="401" src="../../images/shuxian.jpg" />
                            </td>
                            <td style="width: 280px; height: 401px; text-align: right">
                                <table border="0" cellpadding="0" cellspacing="0" style="vertical-align: top; height: 380px"
                                    width="280">
                                    <tr>
                                        <td class="tdTopBackColor" colspan="2" style="width: 266px; height: 27px">
                                            <img alt="" class="imageLeftBack" /><asp:Label ID="labUnitTitle" runat="server" CssClass="lblTitle"
                                                Text="<%$ Resources:BaseInfo,RentableArea_lblTradeTable %>"></asp:Label></td>
                                        <td class="tdTopRightBackColor" colspan="2" style="height: 27px" valign="top"><img class="imageRightBack" /></td>
                                    </tr>
                                    <tr height="1">
                                    <td colspan="2"></td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" colspan="4" rowspan="10" style="height: 341px; text-align: center"
                                            valign="top">
                                            <table >
                                                <tr>
                                                    <td style="height: 10px">
                                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 253px">
                                                            <tr>
                                                                <td style="width: 160px; position: relative; top: 3px; height: 1px; background-color: #738495">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 160px; position: relative; top: 3px; height: 1px; background-color: #ffffff">
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top">
                                                        <asp:Panel ID="Panel1" runat="server" BackColor="White" BorderStyle="Inset" BorderWidth="1px"
                                                            Font-Size="Medium" Height="265px" HorizontalAlign="Left" ScrollBars="Auto" Width="260px">
                                                            <table>
                                                                <tr>
                                                                    <td id="treeview" style="width: 200px; height: 200px" valign="top"></td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height:23px">
                                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 253px">
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
                                                <tr style="height:35px">
                                                    <td style="text-align: right" valign="top">
                                                        <asp:Button ID="treeClick"
                                                                runat="server" CssClass="buttonHidden" OnClick="treeClick_Click" Width="24px" />
                                                        <asp:Button ID="btnAdd" runat="server" CssClass="buttonCancel" Height="31px" OnClick="btnAdd_Click"
                                                            Text="<%$ Resources:BaseInfo,DeptTree_labDeptAdd %>" Width="70px" />
                                                        <asp:Button ID="btnEdit" runat="server" CssClass="buttonEdit" Height="30px" OnClick="btnEdit_Click"
                                                            Text="<%$ Resources:BaseInfo,User_btnChang %>" Width="70px" />
                                                        &nbsp;&nbsp;
                                                        </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 5px; height: 401px">
                            </td>
                            <td colspan="3" style="width: 280px; height: 401px">
                                <table class="tblRight" border="0" cellpadding="0" cellspacing="0" style="vertical-align: top; height: 380px"
                                    width="280">
                                    <tr style="height:27px" valign="top">
                                        <td class="tdTopBackColor" colspan="2" style="width: 266px" valign="top">
                                            <img alt="" class="imageLeftBack" /><asp:Label ID="Label1" runat="server" CssClass="lblTitle"
                                                Text="<%$ Resources:BaseInfo,RentableArea_lblTradeDefine %>"></asp:Label></td>
                                        <td class="tdTopRightBackColor" colspan="2" style="height: 27px; width: 8px;" valign="top"><img class="imageRightBack" /></td>
                                    </tr>
                                    <tr class="headLine">
                                        <td colspan="4">
                                        </td>
                                    </tr>
                                    <tr valign="top">
                                        <td class="tdBackColor" colspan="4" rowspan="10" style=" text-align: center; height: 338px;"
                                            valign="top">
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 257px;">
                                                <tr style="height:25px">
                                                    <td colspan="2">
                                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 243px">
                                                            <tr  class="bodyLine">
                                                                <td style="width: 160px;  background-color: #738495">
                                                                </td>
                                                            </tr>
                                                            <tr  class="bodyLine">
                                                                <td style="width: 160px;  background-color: #ffffff">
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 224px;text-align: right" class="baseLable">
                                                        <asp:Label ID="lblAdBoardCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblTradeCode %>"></asp:Label></td>
                                                    <td style="width: 195px;text-align: left">
                                                        <asp:TextBox ID="txtTradeCode" runat="server" CssClass="ipt150px" ReadOnly="True"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td class="baseLable" style="width: 224px;text-align: right">
                                                        <asp:Label ID="lblAdBoardName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblTradeName %>"></asp:Label></td>
                                                    
                                                    <td style="width: 195px; text-align: left">
                                                        <asp:TextBox ID="txtTradeName" runat="server" CssClass="ipt150px" ReadOnly="True"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td class="baseLable" style="width: 224px; text-align: right">
                                                        <asp:Label ID="lblSelFloorID" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblTradeLevel %>"></asp:Label></td>
                                                   
                                                    <td style="width: 195px;  text-align: left">
                                                        <asp:DropDownList ID="cmbTradeLevel" runat="server" CssClass="cmb160px" Width="154px">
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td class="baseLable" style="width: 224px;text-align: right">
                                                        <asp:Label ID="lblAdContractCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblTradeStatus %>"></asp:Label></td>
                                                    
                                                    <td style="width: 195px; text-align: left"><asp:DropDownList ID="cmbTradeTypeStatus" runat="server" CssClass="cmb160px" Width="154px">
                                                    </asp:DropDownList></td>
                                                </tr>
                                                <tr style="height:3px">
                                                    <td colspan="2" style=" text-align: center" valign="top">
                                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 243px">
                                                            <tr class="bodyLine">
                                                                <td style="width: 160px;background-color: #738495">
                                                                </td>
                                                            </tr>
                                                            <tr class="bodyLine">
                                                                <td style="width: 160px; background-color: #ffffff">
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr style="height:52px">
                                                    <td colspan="3" style="height: 30px; text-align: right">
                                                        <asp:Button ID="btnCancel" runat="server" CssClass="buttonClear" Text="<%$ Resources:BaseInfo,User_btnCancel %>" OnClick="btnCancel_Click" />
                                                        <asp:Button ID="btnSave"
                                                                runat="server" CssClass="buttonSave" Height="31px" OnClick="btnSave_Click" Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>"
                                                                Width="70px" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 60px; height: 401px; text-align: center" valign="middle">
                                <img height="401" src="../../images/shuxian.jpg" /></td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
        
    </form>
</body>
</html>

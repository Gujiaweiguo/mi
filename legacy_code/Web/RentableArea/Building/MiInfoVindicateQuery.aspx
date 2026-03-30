<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MiInfoVindicateQuery.aspx.cs" Inherits="RentableArea_Building_MiInfoVindicateQuery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Dept_MallNameQuery")%></title>
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link rel="StyleSheet" href="../../App_Themes/nlstree/nlstree-basic.css" type="text/css" />
    <link rel="StyleSheet" href="../../App_Themes/nlstree/nlsctxmenu.css" type="text/css" />
    <script type="text/javascript" src="../../App_Themes/nlstree/nlstree.js"></script>
    <script type="text/javascript" src="../../App_Themes/nlstree/nlsctxmenu.js"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"></script>
	<script type="text/javascript" src="../../JavaScript/TreeShow.js"></script>
	<script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
</head>
<body topmargin=0 leftmargin=0>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
           <table border="0" cellpadding="0" cellspacing="0" style="height: 301px; width: 100%;" width="500">
            <tr>
                <td class="tdTopBackColor" style="width: 478px; height: 25px" valign="top">
                    <img alt="" class="imageLeftBack" />
                    <asp:Label ID="labBuildingTitle" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,Dept_MallNameQuery %>"></asp:Label></td>
                <td class="tdTopRightBackColor" colspan="2" style="width: 493; height: 25px" valign="top">
                    <img class="imageRightBack" /></td>
            </tr>
            <tr>
                <td class="tdBackColor" colspan="3" style="width: 100%; height: 230px; text-align:center;"
                    valign="top">
        <table border="0" cellpadding="0" cellspacing="0" style="width:475px; height:230px;">
            <tr>
                <td style="width:475px; height:1px; background-color:White;" colspan="7">
                </td>
            </tr>
            <tr>
                <td style="width: 475px; height:5px;" class="tdBackColor" colspan="7">
                </td>
            </tr>
            <tr>
                <td style="width: 60px; height:2px;" class="tdBackColor">
                </td>
                <td style="width: 5px;height:2px;" class="tdBackColor">
                </td>
                <td style="width: 160px; height:2px;" class="tdBackColor">
                      <table border="0" cellpadding="0" cellspacing="0" style="width: 160px">
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
                <td style="width: 20px;height:2px;" class="tdBackColor">
                </td>
                <td style="width: 70px;height:2px;" class="tdBackColor">
                </td>
                <td style="width: 5px;height:2px;" class="tdBackColor">
                </td>
                <td style="width: 160px;height:2px;" class="tdBackColor">
                      <table border="0" cellpadding="0" cellspacing="0" style="width: 160px">
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
                <td style="width: 60px;height:22px; text-align: right;" class="tdBackColor">
                    <asp:Label ID="lblMallName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Dept_MallName %>" Width="75px"></asp:Label></td>
                <td style="width: 5px;height:22px;" class="tdBackColor">
                </td>
                <td style="width: 160px;height:22px;" class="tdBackColor">
                    <asp:TextBox ID="txtMallName" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                <td style="width: 20px;height:22px;" class="tdBackColor">
                </td>
                <td style="width: 70px;height:22px; text-align: right;" class="tdBackColor">
                    <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,MiInfoVindicate_labContentTel %>" Width="60px"></asp:Label></td>
                <td style="width: 5px;height:22px;" class="tdBackColor">
                </td>
                <td style="width: 160px;height:22px;" class="tdBackColor">
                    <asp:TextBox ID="txtPropertytel" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 60px;height:22px; text-align: right;" class="tdBackColor">
                    <asp:Label ID="lblOfficeAddr" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblOfficeAddr %>"></asp:Label></td>
                <td style="width: 5px;height:22px;" class="tdBackColor">
                </td>
                <td style="width: 160px;height:22px;" class="tdBackColor">
                    <asp:TextBox ID="txtOfficeAddr" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                <td style="width: 20px;height:22px;" class="tdBackColor">
                </td>
                <td style="width: 70px;height:22px; text-align: right;" class="tdBackColor">
                    <asp:Label ID="lblLegalRep" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblLegalRep %>"></asp:Label></td>
                <td style="width: 5px;height:22px;" class="tdBackColor">
                </td>
                <td style="width: 160px;height:22px;" class="tdBackColor">
                    <asp:TextBox ID="txtLegalRep" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 60px;height:22px; text-align: right;" class="tdBackColor">
                    <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblOfficeAddr2 %>"></asp:Label></td>
                <td style="width: 5px;height:22px;" class="tdBackColor">
                </td>
                <td style="width: 160px;height:22px;" class="tdBackColor">
                    <asp:TextBox ID="txtOfficeAddr2" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                <td style="width: 20px;height:22px;" class="tdBackColor">
                </td>
                <td style="width: 70px;height:22px; text-align: right;" class="tdBackColor">
                    <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblLegalRepTitle %>"></asp:Label></td>
                <td style="width: 5px;height:22px;" class="tdBackColor">
                </td>
                <td style="width: 160px;height:22px;" class="tdBackColor">
                    <asp:TextBox ID="txtLegalRepTitle" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 60px;height:22px; text-align: right;" class="tdBackColor">
                    <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblOfficeAddr3 %>"></asp:Label></td>
                <td style="width: 5px;height:22px;" class="tdBackColor">
                </td>
                <td style="width: 160px;height:22px;" class="tdBackColor">
                    <asp:TextBox ID="txtOfficeAddr3" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                <td style="width: 20px;height:22px;" class="tdBackColor">
                </td>
                <td style="width: 70px;height:22px; text-align: right;" class="tdBackColor">
                    <asp:Label ID="lblRegCap" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblRegCap %>"></asp:Label></td>
                <td style="width: 5px;height:22px;" class="tdBackColor">
                </td>
                <td style="width: 160px;height:22px;" class="tdBackColor">
                    <asp:TextBox ID="txtRegCap" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 60px;height:22px; text-align: right;" class="tdBackColor">
                    <asp:Label ID="lblPostAddr" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblPostAddr %>"></asp:Label></td>
                <td style="width: 5px;height:22px;" class="tdBackColor">
                </td>
                <td style="width: 160px;height:22px;" class="tdBackColor">
                    <asp:TextBox ID="txtPostAddr" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                <td style="width: 20px;height:22px;" class="tdBackColor">
                </td>
                <td style="width: 70px;height:22px; vertical-align: middle; text-align: center;" class="tdBackColor">
                    <asp:Label ID="lblRegAddr" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblRegAddr %>"></asp:Label></td>
                <td style="width: 5px;height:22px;" class="tdBackColor">
                </td>
                <td style="width: 160px;height:22px; vertical-align: middle; text-align: left;" class="tdBackColor">
                    <asp:TextBox ID="txtRegAddr" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 60px;height:22px;" class="tdBackColor">
                    <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblPostAddr2 %>"></asp:Label></td>
                <td style="width: 5px;height:22px;" class="tdBackColor">
                </td>
                <td style="width: 160px;height:22px;" class="tdBackColor">
                    <asp:TextBox ID="txtPostAddr2" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                <td style="width: 20px;height:22px;" class="tdBackColor">
                </td>
                <td style="width: 70px;height:22px; text-align: right;" class="tdBackColor">
                    </td>
                <td style="width: 5px;height:22px;" class="tdBackColor">
                </td>
                <td class="tdBackColor"><table border="0" cellpadding="0" cellspacing="0" style="width: 160px">
                    <tr>
                        <td style="width: 160px; height: 20px;">
                        </td>
                    </tr>
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
                <td style="width: 60px;height:22px; text-align: right;" class="tdBackColor">
                    <asp:Label ID="Label7" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblPostAddr3 %>"></asp:Label></td>
                <td style="width: 5px;height:22px;" class="tdBackColor">
                </td>
                <td style="width: 160px;height:22px;" class="tdBackColor">
                    <asp:TextBox ID="txtPostAddr3" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                <td style="width: 20px;height:22px;" class="tdBackColor">
                </td>
                <td style="width: 70px;height:22px; text-align: right;" class="tdBackColor">
                    <asp:Label ID="lblRegCode" runat="server" CssClass="labelStyle" Height="18px" Text="<%$ Resources:BaseInfo,PotCustomer_lblRegCode %>"
                        Width="69px"></asp:Label></td>
                <td style="width: 5px;height:22px;" class="tdBackColor">
                </td>
                <td style="width: 160px;height:22px;" class="tdBackColor">
                    <asp:TextBox ID="txtRegCode" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 60px;height:22px; text-align: right;" class="tdBackColor">
                    <asp:Label ID="lblPostCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblPostCode %>"></asp:Label></td>
                <td style="width: 5px;height:22px;" class="tdBackColor">
                </td>
                <td style="width: 160px;height:22px;" class="tdBackColor">
                    <asp:TextBox ID="txtPostCode" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                <td style="width: 20px;height:22px;" class="tdBackColor">
                </td>
                <td style="width: 70px;height:22px; text-align: right;" class="tdBackColor">
                    <asp:Label ID="lblTaxCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblTaxCode %>"></asp:Label></td>
                <td style="width: 5px;height:22px;" class="tdBackColor">
                </td>
                <td style="width: 160px;height:22px;" class="tdBackColor">
                    <asp:TextBox ID="txtTaxCode" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 60px;height:21px; text-align: right;" class="tdBackColor">
                    <asp:Label ID="lblOfficeTel" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblOfficeTel %>"></asp:Label></td>
                <td style="width: 5px;height:21px;" class="tdBackColor">
                </td>
                <td style="width: 160px;height:21px;" class="tdBackColor">
                    <asp:TextBox ID="txtOfficeTel" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                <td style="width: 20px;height:21px;" class="tdBackColor">
                </td>
                <td style="width: 70px;height:21px; text-align: right;" class="tdBackColor">
                    <asp:Label ID="lblBankName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblBankName %>"></asp:Label></td>
                <td style="width: 5px;height:21px;" class="tdBackColor">
                </td>
                <td style="width: 160px;height:21px;" class="tdBackColor">
                    <asp:TextBox ID="txtBankName" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
            </tr>
                        <tr>
                <td style="width: 60px;height:21px; text-align: right;" class="tdBackColor">
                    </td>
                <td style="width: 5px;height:21px;" class="tdBackColor">
                </td>
                <td style="width: 160px;height:21px;" class="tdBackColor"><table border="0" cellpadding="0" cellspacing="0" style="width: 160px">
                    <tr>
                        <td style="width: 160px; height: 20px;">
                        </td>
                    </tr>
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
                <td style="width: 20px;height:21px;" class="tdBackColor">
                </td>
                <td style="width: 70px;height:21px; text-align: right;" class="tdBackColor">
                    <asp:Label ID="lblBankAcct" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblBankAcct %>"></asp:Label></td>
                <td style="width: 5px;height:21px;" class="tdBackColor">
                </td>
                <td style="width: 160px;height:21px;" class="tdBackColor">
                    <asp:TextBox ID="txtBankAcct" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
            </tr>
                        <tr>
                <td style="width: 60px;height:21px; text-align: right;" class="tdBackColor">
                    <asp:Label ID="Label1" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,MiInfoVindicate_labAttractTel %>"></asp:Label></td>
                <td style="width: 5px;height:21px;" class="tdBackColor">
                </td>
                <td style="width: 160px;height:21px;" class="tdBackColor">
                    <asp:TextBox ID="txtTel" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                <td style="width: 20px;height:21px;" class="tdBackColor">
                </td>
                <td style="width: 70px;height:21px; text-align: right;" class="tdBackColor">
                    </td>
                <td style="width: 5px;height:21px;" class="tdBackColor">
                </td>
                <td style="width: 160px;height:21px;" class="tdBackColor">
                    </td>
            </tr>
                        <tr>
                <td style="width: 60px;height:21px; text-align: right;" class="tdBackColor">
                    </td>
                <td style="width: 5px;height:21px;" class="tdBackColor">
                </td>
                <td style="width: 160px;height:21px;" class="tdBackColor">
                    </td>
                <td style="width: 20px;height:21px;" class="tdBackColor">
                </td>
                <td style="width: 70px;height:21px; text-align: right;" class="tdBackColor">
                    </td>
                <td style="width: 5px;height:21px;" class="tdBackColor">
                </td>
                <td style="width: 160px;height:21px;" class="tdBackColor">
                    </td>
            </tr>
            <tr>
                <td style="width: 475px; height:24px;" class="tdBackColor" colspan="7">
                </td>
            </tr>
        </table>
                </td>
                
            </tr>
            <tr>
                <td style="width:100%; height:10px; text-align:center;" colspan="3" class="tdBackColor" >
                <hr style="width:480px; height:2px;" />
                </td>
            </tr>
            <tr>
                <td style="width:478px; height:36px; text-align:right;"  class="tdBackColor" valign="top">
                </td>
                <td style="width:10px; height:36px; text-align:right;"  class="tdBackColor" valign="top">

                </td>
                <td style="width:190px; height:36px; text-align:center;"  class="tdBackColor" valign="top">
                    </td>
            </tr>
        </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    
    </form>
</body>
</html>

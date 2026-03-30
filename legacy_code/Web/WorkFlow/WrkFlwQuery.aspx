<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WrkFlwQuery.aspx.cs" Inherits="WorkFlow_WrkFlwQuery" MasterPageFile="~/BaseInfo/User/MasterPage.master" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <table border="0" width="100%">
        <tr>
            <td style="height: 74px">
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td align="left" rowspan="2" style="width: 40px" valign="middle">
                            <img alt="" height="32" src="../App_Themes/CSS/Images/iconNew32x32.gif" width="32" /></td>
                        <td align="left" class="workAreaMainTitle" valign="middle">
                            <asp:Label ID="lblWorkFlow" runat="server" Text="<%$ Resources:BaseInfo,WrkFlw_lblWorkFlow %>"
                                ToolTip="为系统增加新的工作流和工作流节点"></asp:Label></td>
                    </tr>
                    <tr>
                        <td align="left" class="workAreaMainTitleMemo" valign="middle">
                        </td>
                    </tr>
                </table>
                <hr style="width: 100%; size: 1" />
            </td>
        </tr>
        <tr>
            <td style="width: 723px; height: 368px">
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td align="left" style="width: 50%; height: 409px" valign="top">
                            <fieldset style="height: 321px">
                                <legend class="workAreaFieldsetTitle">工作流</legend>
                                <table border="0" cellpadding="2" cellspacing="1" style="azimuth: center" width="100%">
                                    <tr>
                                        <td align="right" style="width: 60px; height: 25px">
                                            &nbsp;<asp:Label ID="lblBizGrpID" runat="server" ForeColor="Black" Text="<%$ Resources:BaseInfo,WrkFlw_lblBizGrpID %>"
                                                Width="58px" CssClass="label"></asp:Label></td>
                                        <td align="left" style="width: 426px; color: #000000; height: 25px">
                                            <asp:DropDownList ID="cmbBizGrpID" runat="server" BackColor="White" CssClass="cmb160px"
                                                Width="165px" Enabled="False">
                                                <asp:ListItem></asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 60px; height: 25px">
                                            &nbsp;<asp:Label ID="lblVoucherTypeID" runat="server" ForeColor="Black" Text="<%$ Resources:BaseInfo,WrkFlw_lblVoucherTypeID %>"
                                                Width="60px" CssClass="label"></asp:Label></td>
                                        <td align="left" style="width: 426px; color: #000000; height: 25px">
                                            <asp:DropDownList ID="cmbVoucherTypeID" runat="server" CssClass="cmb160px" Enabled="False">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 60px; height: 25px">
                                            &nbsp;<asp:Label ID="lblWrkFlwName" runat="server" ForeColor="Black" Text="<%$ Resources:BaseInfo,WrkFlw_lblWrkFlwName %>" CssClass="label" Width="62px"></asp:Label></td>
                                        <td align="left" style="width: 426px; color: #000000; height: 25px">
                                            <asp:TextBox ID="txtWrkFlwName" runat="server" CssClass="ipt160px"
                                                MaxLength="32" ReadOnly="True"></asp:TextBox></td>
                                    </tr>
                                    <tr style="color: #000000">
                                        <td align="right" style="width: 60px; height: 20px">
                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Label ID="lblInitVoucher" runat="server" Text="<%$ Resources:BaseInfo,WrkFlw_lblInitVoucher %>" CssClass="label"></asp:Label></td>
                                        <td align="left" style="width: 426px; height: 20px">
                                            <asp:DropDownList ID="cmbInitVoucher" runat="server" BackColor="White" CssClass="cmb160px"
                                                Width="167px" Enabled="False">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr style="color: #000000">
                                        <td align="right" style="width: 60px; height: 20px">
                                            &nbsp;&nbsp;&nbsp;
                                            <asp:Label ID="lblEfficiency" runat="server" Text="<%$ Resources:BaseInfo,WrkFlw_lblEfficiency %>" CssClass="label"></asp:Label></td>
                                        <td align="left" style="width: 426px; height: 20px">
                                            <asp:TextBox ID="txtEfficiency" runat="server" CssClass="ipt140px"
                                                MaxLength="18" ReadOnly="True"></asp:TextBox>
                                            <asp:Label ID="lblEfficiencyDay" runat="server" Text='<%$ Resources:BaseInfo,WrkFlw_lblEfficiencyDay %>' Width="1px"></asp:Label></td>
                                    </tr>
                                    <tr style="color: #000000">
                                        <td align="right" style="width: 60px; height: 20px">
                                            <asp:Label ID="lblTraceDays" runat="server" Text="<%$ Resources:BaseInfo,WrkFlw_lblTraceDays %>" CssClass="label" Width="62px"></asp:Label></td>
                                        <td align="left" style="width: 426px; height: 20px">
                                            <asp:TextBox ID="txtTraceDays" runat="server" CssClass="ipt140px" MaxLength="18" ReadOnly="True"></asp:TextBox>
                                            <asp:Label ID="lblTraceDaysDay" runat="server" Text='<%$ Resources:BaseInfo,WrkFlw_lblTraceDaysDay %>'></asp:Label></td>
                                    </tr>
                                    <tr style="color: #000000">
                                        <td align="right" style="width: 60px; height: 20px">
                                            &nbsp;<asp:Label ID="lblWrkFlwStatus" runat="server" Text="<%$ Resources:BaseInfo,WrkFlw_lblWrkFlwStatus %>" CssClass="label" Width="60px"></asp:Label></td>
                                        <td align="left" style="width: 426px; height: 20px">
                                            <asp:DropDownList ID="cmbWrkFlwStatus" runat="server" BackColor="White" CssClass="cmb160px"
                                                Width="167px" Enabled="False">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr style="color: #000000">
                                        <td align="right" style="width: 60px; height: 20px">
                                            <asp:Label ID="lblTransit" runat="server" Text="<%$ Resources:BaseInfo,WrkFlw_lblTransit %>" CssClass="label" Width="64px"></asp:Label></td>
                                        <td align="left" style="width: 426px; height: 20px">
                                            <asp:DropDownList ID="cmbTransit" runat="server" BackColor="White" CssClass="cmb160px"
                                                Width="167px" Enabled="False">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr style="color: #000000">
                                        <td align="right" style="width: 60px; height: 20px">
                                            &nbsp;<asp:Label ID="lblWrkFlwProcessClass" runat="server" Text="<%$ Resources:BaseInfo,WrkFlw_lblWrkFlwProcessClass %>" CssClass="label" Width="60px"></asp:Label></td>
                                        <td align="left" style="width: 426px; height: 20px">
                                            <asp:TextBox ID="txtWrkFlwProcessClass" runat="server" CssClass="ipt160px"
                                                MaxLength="128" ReadOnly="True"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                &nbsp;
                            </fieldset>
                        </td>
                        <td style="width: 58px; height: 409px">
                            &nbsp;</td>
                        <td align="center" style="width: 51%; height: 409px" valign="top">
                            <fieldset style="height: 320px">
                                <legend class="workAreaFieldsetTitle">工作流节点</legend>
                                <table border="0" cellpadding="2" cellspacing="1" style="azimuth: center" width="100%">
                                    <tr>
                                        <td align="right" style="width: 52px; height: 25px">
                                            &nbsp;<asp:Label ID="lblNodeFuncAuthID" runat="server" ForeColor="Black" Text="<%$ Resources:BaseInfo,WrkFlw_lblNodeFuncAuthID %>"
                                                Width="60px" CssClass="label"></asp:Label></td>
                                        <td align="left" style="width: 518px; color: #000000; height: 25px">
                                            <asp:DropDownList ID="cmbNodeFuncID" runat="server" BackColor="White" CssClass="cmb160px"
                                                Width="163px" Enabled="False">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 52px; height: 25px">
                                            &nbsp;&nbsp;
                                            <asp:Label ID="lblNodeRoleID" runat="server" ForeColor="Black" Text="<%$ Resources:BaseInfo,WrkFlw_lblNodeRoleID %>"
                                                Width="62px" CssClass="label"></asp:Label></td>
                                        <td align="left" style="width: 518px; color: #000000; height: 25px">
                                            <asp:DropDownList ID="cmbNodeRoleID" runat="server" BackColor="White" CssClass="cmb160px"
                                                Width="163px" Enabled="False">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 52px; height: 25px">
                                            &nbsp;<asp:Label ID="lblNodeNodetName" runat="server" ForeColor="Black" Text="<%$ Resources:BaseInfo,WrkFlw_lblNodeNodetName %>"
                                                Width="62px" CssClass="label"></asp:Label></td>
                                        <td align="left" style="width: 518px; color: #000000; height: 25px">
                                            <asp:TextBox ID="txtNodeNodeName" runat="server" CssClass="ipt160px"
                                                MaxLength="32" ReadOnly="True"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 52px; height: 25px">
                                            &nbsp;<asp:Label ID="lblNodeWrkStep" runat="server" ForeColor="Black" Text="<%$ Resources:BaseInfo,WrkFlw_lblNodeWrkStep %>"
                                                Width="62px" CssClass="label"></asp:Label></td>
                                        <td align="left" style="width: 518px; color: #000000; height: 25px">
                                            <asp:TextBox ID="txtNodeWrkStep" runat="server" CssClass="ipt160px"
                                                MaxLength="18" ReadOnly="True"></asp:TextBox></td>
                                    </tr>
                                    <tr style="color: #000000">
                                        <td align="right" style="width: 52px; height: 20px">
                                            <asp:Label ID="lblNodeSmtToMgr" runat="server" Text="<%$ Resources:BaseInfo,WrkFlw_lblNodeSmtToMgr %>"
                                                Width="74px" CssClass="label"></asp:Label></td>
                                        <td align="left" style="width: 518px; height: 20px">
                                            <asp:DropDownList ID="cmbNodeSmtToMgr" runat="server" BackColor="White" CssClass="cmb160px"
                                                Width="167px" Enabled="False">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr style="color: #000000">
                                        <td align="right" style="width: 52px; height: 20px">
                                            <asp:Label ID="lblNodeValidAfterConfirm" runat="server" Text="<%$ Resources:BaseInfo,WrkFlw_lblNodeValidAfterConfirm %>"
                                                Width="74px" CssClass="label"></asp:Label></td>
                                        <td align="left" style="width: 518px; height: 20px">
                                            <asp:DropDownList ID="cmbNodeValidAfterConfirm" runat="server" BackColor="White"
                                                CssClass="cmb160px" Width="165px" Enabled="False">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr style="color: #000000">
                                        <td align="right" style="width: 52px; height: 20px">
                                            &nbsp;<asp:Label ID="lblNodePrintAfterConfirm" runat="server" Text="<%$ Resources:BaseInfo,WrkFlw_lblNodePrintAfterConfirm %>"
                                                Width="70px" CssClass="label"></asp:Label></td>
                                        <td align="left" style="width: 518px; height: 20px">
                                            <asp:DropDownList ID="cmbNodePrintAfterConfirm" runat="server" BackColor="White"
                                                CssClass="cmb160px" Width="165px" Enabled="False">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr style="color: #000000">
                                        <td align="right" style="width: 52px; height: 20px">
                                            <asp:Label ID="lblNodeLongestDelay" runat="server" Text="<%$ Resources:BaseInfo,WrkFlw_lblNodeLongestDelay %>"
                                                Width="76px" CssClass="label"></asp:Label></td>
                                        <td align="left" style="width: 518px; height: 20px">
                                            <asp:TextBox ID="txtNodeLongestDelay" runat="server" CssClass="ipt140px"
                                                MaxLength="18" ReadOnly="True"></asp:TextBox>
                                            <asp:Label ID="lbllblNodeLongestDelayHour" runat="server" Text='<%$ Resources:BaseInfo,WrkFlw_lbllblNodeLongestDelayHour %>' Width="31px"></asp:Label></td>
                                    </tr>
                                    <tr style="color: #000000">
                                        <td align="right" style="width: 52px; height: 21px">
                                            &nbsp; &nbsp;<asp:Label ID="lblNodeTimeoutHandler" runat="server" Text="<%$ Resources:BaseInfo,WrkFlw_lblNodeTimeoutHandler %>"
                                                Width="58px" CssClass="label"></asp:Label></td>
                                        <td align="left" style="width: 518px; height: 21px">
                                            <asp:DropDownList ID="cmbNodeTimeoutHandler" runat="server" BackColor="White" CssClass="cmb160px"
                                                Width="167px" Enabled="False">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr style="color: #000000">
                                        <td align="right" style="width: 52px; height: 20px">
                                            &nbsp;<asp:Label ID="lblNodeProcessClass" runat="server" Text="<%$ Resources:BaseInfo,WrkFlw_lblNodeProcessClass %>" CssClass="label" Height="12px" Width="66px"></asp:Label></td>
                                        <td align="left" style="width: 426px; height: 20px">
                                            <asp:TextBox ID="txtNodeProcessClass" runat="server" CssClass="ipt160px"
                                                MaxLength="128" ReadOnly="True"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                            &nbsp;&nbsp;</td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="width: 723px; height: 501px">
                <table border="0" cellpadding="0" cellspacing="0" class="tblThinLine" style="height: 417px"
                    width="100%">
                    <tr>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="1" width="100%">
                                <tr>
                                    <td style="height: 461px">
                                        <table border="0" cellpadding="0" cellspacing="0" style="height: 312px" width="100%">
                                            <tr>
                                                <td style="width: 718px; height: 443px" valign="top">
                                                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="gridview"
                                                        Height="118px" 
                                                        RowHeaderColumn="abc" Width="680px">
                                                        <Columns>
                                                            <asp:BoundField DataField="NodeName" HeaderText="节点名称" />
                                                            <asp:BoundField DataField="WrkStep" HeaderText="节点步数" />
                                                            <asp:TemplateField HeaderText="业务角色">
                                                                <ItemTemplate>
                                                                    <%#Eval("RoleName")%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="可送上级">
                                                                <ItemTemplate>
                                                                    <%#Eval("SmtToMgrDesc")%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="数据是否可生效">
                                                                <ItemTemplate>
                                                                    <%#Eval("ValidAfterConfirmDesc")%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="单据是否可打印">
                                                                <ItemTemplate>
                                                                    <%#Eval("PrintAfterConfirmDesc")%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="超时后自动处理">
                                                                <ItemTemplate>
                                                                    <%#Eval("TimeoutHandlerDesc")%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="NodeID">
                                                                <ItemStyle CssClass="hidden" />
                                                                <HeaderStyle CssClass="hidden" />
                                                                <FooterStyle CssClass="hidden" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            <table>
                                                                <tr>
                                                                    <th scope="col">
                                                                        节点名称</th>
                                                                    <th scope="col">
                                                                        业务角色</th>
                                                                    <th scope="col">
                                                                        可送上级</th>
                                                                    <th scope="col">
                                                                        数据是否可生效</th>
                                                                    <th scope="col">
                                                                        单据是否可打印</th>
                                                                    <th scope="col">
                                                                        超时后自动处理</th>
                                                                </tr>
                                                            </table>
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                
                                                    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="返  回" CssClass="btnOn50px" />
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
</asp:Content>


<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WrkFlwTrack.aspx.cs" Inherits="WorkFlowEntity_WrkFlwTrack" MasterPageFile="~/BaseInfo/User/MasterPage.master" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <table border="0" width="100%">
        <tr>
            <td style="width: 727px; height: 23px">
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td align="left" rowspan="2" style="width: 40px" valign="middle">
                            <img alt="" height="32" src="../App_Themes/CSS/Images/iconNew32x32.gif" width="32" /></td>
                        <td align="left" class="workAreaMainTitle" style="height: 20px" valign="middle">
                            <asp:Label ID="lblWrkFlwTrack" runat="server" Text='<%$ Resources:BaseInfo,WrkFlwEntity_lblWrkFlwTrack %>'></asp:Label></td>
                    </tr>
                    <tr>
                        <td align="left" class="workAreaMainTitleMemo" style="height: 18px" valign="middle">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="width: 727px; height: 337px" valign="top">
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td align="left" style="width: 50%" valign="top">
                            <fieldset>
                                <legend class="workAreaFieldsetTitle">
                                    <asp:Label ID="lblInitiativeOverrule" runat="server" Width="96px" Text='<%$ Resources:BaseInfo,WrkFlwEntity_lblInitiativeOverrule %>'
                                       ></asp:Label></legend>
                                <table border="0" cellpadding="2" cellspacing="1" style="azimuth: center" width="100%">
                                    <tr>
                                        <td style="width: 644px; height: 108px">
                                            <div>
                                                <table style="width: 641px; height: 1px">
                                                    <tr>
                                                        <td style="width: 3px; height: 137px;" valign="top">
                                                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                                BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" CssClass="BizGrpGrid"
                                                                GridLines="Horizontal" Height="1px" 
                                                                 Width="415px">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="单据线索">
                                                                        <ItemTemplate>
                                                                            <%#Eval("VoucherHints")%>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="驳回人">
                                                                        <ItemTemplate>
                                                                            <%#Eval("UserName")%>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="驳回时间">
                                                                    <ItemTemplate>
                                                                            <%#Eval("StartTime")%>
                                                                    </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="驳回注释">
                                                                    <ItemTemplate>
                                                                        <%#Eval("VoucherMemo")%>
                                                                    </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:CommandField SelectText="查看" ShowSelectButton="True" />
                                                                </Columns>
                                                                <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                                                                <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
                                                                <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                                                                <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Right" />
                                                                <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                                                                <AlternatingRowStyle BackColor="#F7F7F7" />
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                            &nbsp;
                            
                            
                            <fieldset>
                                <legend class="workAreaFieldsetTitle">
                                    <asp:Label ID="lblOvertime" runat="server" Width="120px" Text='<%$ Resources:BaseInfo,WrkFlwEntity_lblOvertime %>'
                                       ></asp:Label></legend>
                                <table border="0" cellpadding="2" cellspacing="1" style="azimuth: center" width="100%">
                                    <tr>
                                        <td style="width: 644px; height: 108px">
                                            <div>
                                                <table style="width: 641px; height: 1px">
                                                    <tr>
                                                        <td style="width: 3px; height: 137px" valign="top">
                                                            <asp:GridView ID="GrdVewOvertime" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                                BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" CssClass="BizGrpGrid"
                                                                GridLines="Horizontal" Height="1px"  Width="415px">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="单据线索">
                                                                        <ItemTemplate>
                                                                        <%#Eval("VoucherHints") %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="滞留人">
                                                                        <ItemTemplate>
                                                                           <%#Eval("UserName") %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="提交时间">
                                                                    <ItemTemplate>
                                                                        <%#Eval("StartTime")%>
                                                                    </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="最大允许时间">
                                                                    <ItemTemplate>
                                                                    <%#Eval("LongestDelay")%>
                                                                    </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="已滞留时间">
                                                                    <ItemTemplate>
                                                                    <%#Eval("Stop")%>
                                                                    </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:CommandField SelectText="查看" ShowSelectButton="True" />
                                                                </Columns>
                                                                <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                                                                <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
                                                                <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                                                                <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Right" />
                                                                <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                                                                <AlternatingRowStyle BackColor="#F7F7F7" />
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                            
                            <fieldset>
                                <legend class="workAreaFieldsetTitle">
                                    <asp:Label ID="lblConfirm" runat="server" Width="80px" Text='<%$ Resources:BaseInfo,WrkFlwEntity_lblConfirm %>'
                                       ></asp:Label></legend>
                                <table border="0" cellpadding="2" cellspacing="1" style="azimuth: center" width="100%">
                                    <tr>
                                        <td style="width: 644px; height: 108px">
                                            <div>
                                                <table style="width: 641px; height: 1px">
                                                    <tr>
                                                        <td style="width: 3px; height: 137px" valign="top">
                                                            <asp:GridView ID="GrdVewConfirm" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                                BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" CssClass="BizGrpGrid"
                                                                GridLines="Horizontal" Height="1px"  Width="415px">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="单据线索">
                                                                        <ItemTemplate>
                                                                        <%#Eval("VoucherHints")%>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="提交时间">
                                                                    <ItemTemplate>
                                                                        <%#Eval("CompletedTime")%>
                                                                    </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="总处理时间占用">
                                                                    <ItemTemplate>
                                                                    <%#Eval("Stop")%>
                                                                    </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:CommandField SelectText="查看" ShowSelectButton="True" />
                                                                </Columns>
                                                                <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                                                                <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
                                                                <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                                                                <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Right" />
                                                                <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                                                                <AlternatingRowStyle BackColor="#F7F7F7" />
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                            
                            <fieldset>
                                <legend class="workAreaFieldsetTitle">
                                    <asp:Label ID="lblDisposaling" runat="server" Width="124px" Text='<%$ Resources:BaseInfo,WrkFlwEntity_lblDisposaling %>'
                                       ></asp:Label></legend>
                                <table border="0" cellpadding="2" cellspacing="1" style="azimuth: center" width="100%">
                                    <tr>
                                        <td style="width: 653px; height: 108px">
                                            <div>
                                                <table style="width: 641px; height: 1px">
                                                    <tr>
                                                        <td style="width: 3px; height: 137px" valign="top">
                                                            <asp:GridView ID="GrdVewDisposaling" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                                BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" CssClass="BizGrpGrid"
                                                                GridLines="Horizontal" Height="1px"  Width="415px">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="单据线索">
                                                                        <ItemTemplate>
                                                                        <%#Eval("VoucherHints")%>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="滞留人">
                                                                    <ItemTemplate>
                                                                    <%#Eval("UserName") %>
                                                                    </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="提交时间">
                                                                    <ItemTemplate>
                                                                        <%#Eval("CompletedTime")%>
                                                                    </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="已滞留时间">
                                                                    <ItemTemplate>
                                                                    <%#Eval("Stop") %>
                                                                    </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:CommandField SelectText="查看" ShowSelectButton="True" />
                                                                </Columns>
                                                                <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                                                                <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
                                                                <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                                                                <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Right" />
                                                                <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                                                                <AlternatingRowStyle BackColor="#F7F7F7" />
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>


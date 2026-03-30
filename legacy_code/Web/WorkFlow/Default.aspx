<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="WorkFlow_Default"  MasterPageFile="~/BaseInfo/User/MasterPage.master"%>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <div>
        <table style="height: 185px; width: 104%;">
            <tr>
                <td style="width: 160px; height: 87px">
                    <fieldset style="border-left-color: green; border-bottom-color: green; border-top-color: green;
                        border-right-color: green; height: 199px;">
                        <legend>工作流定义</legend>
                        <table border="0" style="background-color: #ffffff" width="100%">
                            <tr>
                                <td align="center" style="width: 1px; height: 25px">
                                </td>
                                <td align="center" style="width: 63px; height: 25px">
                                    <asp:Label ID="lblBizGrpID" runat="server" ForeColor="Black" Text="业务组" Width="72px"></asp:Label></td>
                                <td align="left" style="width: 426px; color: #000000; height: 25px">
                                    <asp:DropDownList ID="cmbBizGrpID" runat="server" Width="165px" BackColor="White" CssClass="ipt150px">
                                        <asp:ListItem></asp:ListItem>
                                    </asp:DropDownList></td>
                                <td style="width: 49px; color: #000000; height: 1px">
                                </td>
                            </tr>
                            <tr>
                                <td align="center" style="width: 1px; height: 25px">
                                </td>
                                <td align="center" style="width: 63px; height: 25px">
                                    <asp:Label ID="lblVoucherTypeID" runat="server" ForeColor="Black" Text="单据类别编码" Width="60px"></asp:Label></td>
                                <td align="left" style="width: 426px; color: #000000; height: 25px">
                                    <asp:TextBox ID="txtVoucherTypeID" runat="server" CssClass="ipt150px" Height="15px"
                                        MaxLength="18" Width="160px"></asp:TextBox>
                                    &nbsp;</td>
                                <td style="width: 49px; color: #000000; height: 1px">
                                </td>
                            </tr>
                            <tr>
                                <td align="center" style="width: 1px; height: 25px">
                                </td>
                                <td align="center" style="width: 63px; height: 25px">
                                    <asp:Label ID="lblDeptName" runat="server" ForeColor="Black" Text="工作流名称"></asp:Label></td>
                                <td align="left" style="width: 426px; color: #000000; height: 25px">
                                    <asp:TextBox ID="txtWrkFlwName" runat="server" CssClass="ipt150px" Height="15px" MaxLength="18"
                                        Width="160px"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator1"
                                            runat="server" ControlToValidate="txtWrkFlwName" ErrorMessage="*"></asp:RequiredFieldValidator></td>
                                <td style="width: 49px; color: #000000; height: 25px">
                                </td>
                            </tr>
                            <tr style="color: #000000">
                                <td align="center" style="width: 1px; height: 20px">
                                </td>
                                <td align="center" style="width: 63px; height: 20px">
                                    <asp:Label ID="Label3" runat="server" Text="首节点是否制作单据"></asp:Label></td>
                                <td align="left" style="width: 426px; height: 20px">
                                    <asp:DropDownList ID="cmbInitVoucher" runat="server" Width="167px" BackColor="White" CssClass="ipt150px">
                                    </asp:DropDownList></td>
                                <td style="width: 49px; height: 20px">
                                </td>
                            </tr>
                            <tr style="color: #000000">
                                <td align="center" style="width: 1px; height: 20px">
                                </td>
                                <td align="center" style="width: 63px; height: 20px">
                                    <asp:Label ID="lblDeptLevel" runat="server" Text="效率要求"></asp:Label></td>
                                <td align="left" style="width: 426px; height: 20px">
                                    <asp:TextBox ID="txtEfficiency" runat="server" CssClass="ipt150px" Height="15px" MaxLength="18"
                                        Width="160px"></asp:TextBox>
                                </td>
                                <td style="width: 49px; height: 20px">
                                </td>
                            </tr>
                            <tr style="color: #000000">
                                <td align="center" style="width: 1px; height: 20px">
                                </td>
                                <td align="center" style="width: 63px; height: 20px">
                                    <asp:Label ID="Label1" runat="server" Text="跟踪有效期"></asp:Label></td>
                                <td align="left" style="width: 426px; height: 20px">
                                    <asp:TextBox ID="txtTraceDays" runat="server" CssClass="ipt150px" Height="15px" MaxLength="18"
                                        Width="160px"></asp:TextBox>
                                </td>
                                <td style="width: 49px; height: 20px">
                                </td>
                            </tr>
                            <tr style="color: #000000">
                                <td align="center" style="width: 1px; height: 20px">
                                </td>
                                <td align="center" style="width: 63px; height: 20px">
                                    <asp:Label ID="lblWrkFlwStatus" runat="server" Text="工作流状态"></asp:Label></td>
                                <td align="left" style="width: 426px; height: 20px">
                                    <asp:DropDownList ID="cmbWrkFlwStatus" runat="server" Width="167px" BackColor="White" CssClass="ipt150px">
                                    </asp:DropDownList></td>
                                <td style="width: 49px; height: 20px">
                                </td>
                            </tr>
                            
                            <tr style="color: #000000">
                                <td align="center" style="width: 1px; height: 20px">
                                </td>
                                <td align="center" style="width: 63px; height: 20px">
                                    <asp:Label ID="Label2" runat="server" Text="工作流转接"></asp:Label></td>
                                <td align="left" style="width: 426px; height: 20px">
                                    <asp:DropDownList ID="cmb" runat="server" Width="167px" BackColor="White" CssClass="ipt150px">
                                    </asp:DropDownList></td>
                                <td style="width: 49px; height: 20px">
                                </td>
                            </tr>
                            
                            <tr style="color: #000000">
                                <td align="center" style="width: 1px; height: 20px">
                                </td>
                                <td align="center" style="width: 63px; height: 20px">
                                    <asp:Label ID="lblWrkFlwProcessClass" runat="server" Text="处理接口类名称"></asp:Label></td>
                                <td align="left" style="width: 426px; height: 20px">
                                    <asp:TextBox ID="txtWrkFlwProcessClass" runat="server" CssClass="ipt150px" Height="15px" MaxLength="18"
                                        Width="160px"></asp:TextBox>
                                </td>
                                <td style="width: 49px; height: 20px">
                                </td>
                            </tr>
                            
                        </table>
                    </fieldset>
                </td>
            </tr>
            <tr>
                <td style="width: 160px; height: 306px">
                    <fieldset style="border-left-color: green; border-bottom-color: green; border-top-color: green;
                        border-right-color: green">
                        <legend>工作流节点定义</legend>
                        <table border="0" style="background-color: #ffffff" width="100%">
                            <tr>
                                <td align="center" style="width: 1px; height: 25px">
                                </td>
                                <td align="center" style="width: 63px; height: 25px">
                                    <asp:Label ID="lblNodeFuncAuthID" runat="server" ForeColor="Black" Text="功能权限选择"
                                        Width="76px"></asp:Label></td>
                                <td align="left" style="width: 518px; color: #000000; height: 25px">
                                    <asp:DropDownList ID="cmbNodeFuncID" runat="server" Width="163px" BackColor="White" CssClass="ipt150px">
                                    </asp:DropDownList></td>
                                <td style="width: 1px; color: #000000; height: 1px">
                                </td>
                            </tr>
                            <tr>
                                <td align="center" style="width: 1px; height: 25px">
                                </td>
                                <td align="center" style="width: 63px; height: 25px">
                                    <asp:Label ID="lblNodeRoleID" runat="server" ForeColor="Black" Text="角色选择" Width="62px"></asp:Label></td>
                                <td align="left" style="width: 518px; color: #000000; height: 25px">
                                    <asp:DropDownList ID="cmbNodeRoleID" runat="server" Width="163px" BackColor="White" CssClass="ipt150px">
                                    </asp:DropDownList></td>
                                <td style="width: 1px; color: #000000; height: 1px">
                                </td>
                            </tr>
                            <tr>
                                <td align="center" style="width: 1px; height: 25px">
                                </td>
                                <td align="center" style="width: 63px; height: 25px">
                                    <asp:Label ID="lblNodeNodetName" runat="server" ForeColor="Black" Text="节点名称" Width="62px"></asp:Label></td>
                                <td align="left" style="width: 518px; color: #000000; height: 25px">
                                    <asp:TextBox ID="txtNodeNodeName" runat="server" CssClass="ipt150px" Height="15px"
                                        MaxLength="18" Width="160px"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator11"
                                            runat="server" ControlToValidate="txtNodeNodeName" ErrorMessage="*"></asp:RequiredFieldValidator></td>
                                <td style="width: 1px; color: #000000; height: 1px">
                                </td>
                            </tr>
                            <tr>
                                <td align="center" style="width: 1px; height: 25px">
                                </td>
                                <td align="center" style="width: 63px; height: 25px">
                                    <asp:Label ID="lblNodeWrkStep" runat="server" ForeColor="Black" Text="节点步数" Width="62px"></asp:Label></td>
                                <td align="left" style="width: 518px; color: #000000; height: 25px">
                                    <asp:TextBox ID="txtNodeWrkStep" runat="server" CssClass="ipt150px" Height="15px"
                                        MaxLength="18" Width="160px"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator2"
                                            runat="server" ControlToValidate="txtNodeWrkStep" ErrorMessage="*"></asp:RequiredFieldValidator></td>
                                <td style="width: 1px; color: #000000; height: 1px">
                                </td>
                            </tr>
                            <tr style="color: #000000">
                                <td align="center" style="width: 1px; height: 20px">
                                </td>
                                <td align="center" style="width: 63px; height: 20px">
                                    <asp:Label ID="lblNodeSmtToMgr" runat="server" Text="是否可送上级审批" Width="74px"></asp:Label></td>
                                <td align="left" style="width: 518px; height: 20px">
                                    <asp:DropDownList ID="cmbNodeSmtToMgr" runat="server" Width="167px" BackColor="White" CssClass="ipt150px">
                                    </asp:DropDownList></td>
                                <td style="width: 1px; height: 20px">
                                </td>
                            </tr>
                            <tr style="color: #000000">
                                <td align="center" style="width: 1px; height: 20px">
                                </td>
                                <td align="center" style="width: 63px; height: 20px">
                                    <asp:Label ID="lblNodeValidAfterConfirm" runat="server" Text="数据是否可生效" Width="74px"></asp:Label></td>
                                <td align="left" style="width: 518px; height: 20px">
                                    <asp:DropDownList ID="cmbNodeValidAfterConfirm" runat="server" Width="165px" BackColor="White" CssClass="ipt150px">
                                    </asp:DropDownList></td>
                                <td style="width: 1px; height: 20px">
                                </td>
                            </tr>
                            <tr style="color: #000000">
                                <td align="center" style="width: 1px; height: 20px">
                                </td>
                                <td align="center" style="width: 63px; height: 20px">
                                    <asp:Label ID="lblNodePrintAfterConfirm" runat="server" Text="单据是否可打印" Width="74px"></asp:Label></td>
                                <td align="left" style="width: 518px; height: 20px">
                                    <asp:DropDownList ID="cmbNodePrintAfterConfirm" runat="server" Width="165px" BackColor="White" CssClass="ipt150px">
                                    </asp:DropDownList></td>
                                <td style="width: 1px; height: 20px">
                                </td>
                            </tr>
                            <tr style="color: #000000">
                                <td align="center" style="width: 1px; height: 20px">
                                </td>
                                <td align="center" style="width: 63px; height: 20px">
                                    <asp:Label ID="lblNodeLongestDelay" runat="server" Text="最长滞留时间" Width="76px"></asp:Label></td>
                                <td align="left" style="width: 518px; height: 20px">
                                    <asp:TextBox ID="txtNodeLongestDelay" runat="server" CssClass="ipt150px" Height="15px"
                                        MaxLength="18" Width="160px"></asp:TextBox></td>
                                <td style="width: 1px; height: 20px">
                                </td>
                            </tr>
                            <tr style="color: #000000">
                                <td align="center" style="width: 1px; height: 21px">
                                </td>
                                <td align="center" style="width: 63px; height: 21px">
                                    <asp:Label ID="lblNodeTimeoutHandler" runat="server" Text="超时后自动处理" Width="76px"></asp:Label></td>
                                <td align="left" style="width: 518px; height: 21px">
                                    <asp:DropDownList ID="cmbNodeTimeoutHandler" runat="server" Width="167px" BackColor="White" CssClass="ipt150px">
                                    </asp:DropDownList></td>
                                <td style="width: 1px; height: 21px">
                                </td>
                            </tr>
                            
                            
                             <tr style="color: #000000">
                                <td align="center" style="width: 1px; height: 20px">
                                </td>
                                <td align="center" style="width: 63px; height: 20px">
                                    <asp:Label ID="lblNodeProcessClass" runat="server" Text="处理接口类名称"></asp:Label></td>
                                <td align="left" style="width: 426px; height: 20px">
                                    <asp:TextBox ID="txtNodeProcessClass" runat="server" CssClass="ipt150px" Height="15px" MaxLength="18"
                                        Width="160px"></asp:TextBox>
                                </td>
                                <td style="width: 49px; height: 20px">
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="保  存" CssClass="btn50px" /></td>
                <td style="width: 719px; height: 306px">
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Height="118px"
                        OnSelectedIndexChanged="GridView1_SelectedIndexChanged" Width="680px" CssClass="wrkflwgrid">
                        <Columns>
                        <asp:TemplateField HeaderText="节点名称">
                          <ItemTemplate>
                            <%#Eval("NodeName")%>
                          </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="业务角色">
                          <ItemTemplate>
                            <%#Eval("RoleID")%>
                          </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="可送上级">
                           <ItemTemplate>
                             <%#(int)Eval("SmtToMgr")==0?"否":"是"%>
                               </ItemTemplate>
                         </asp:TemplateField>
                         
                         <asp:TemplateField HeaderText="数据是否可生效">
                            <ItemTemplate>
                             <%#(int)Eval("ValidAfterConfirm") == 0 ? "不生效" : "生效"%>
                            </ItemTemplate>
                         </asp:TemplateField>                         
                                            
                         <asp:TemplateField HeaderText="单据是否可打印">
                            <ItemTemplate>
                             <%#(int)Eval("PrintAfterConfirm") == 0 ? "不可以打印" : "可以打印"%>
                            </ItemTemplate>
                         </asp:TemplateField>
                         
                         <asp:TemplateField HeaderText="超时后自动处理">
                            <ItemTemplate>
                              <%#(int)Eval("TimeoutHandler")%>
                            </ItemTemplate>
                         </asp:TemplateField>
                            <%--<asp:BoundField HeaderText="节点名称" />
                            <asp:BoundField HeaderText="业务角色" />
                            <asp:BoundField  HeaderText="可送上级" />
                            <asp:BoundField HeaderText="数据是否可生效" />
                            <asp:BoundField HeaderText="单据是否可打印" />
                            <asp:BoundField HeaderText="超时" />--%>
                        </Columns>
                    </asp:GridView>
                    &nbsp; &nbsp;&nbsp;
                </td>
            </tr>
        </table>
    </div>
</asp:Content>


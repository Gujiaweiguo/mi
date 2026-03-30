<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WrkFlwDelete.aspx.cs" Inherits="WorkFlow_WrkFlwDelete"  %>

<table border="0" width="100%">
    <tr>
        <td>
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td align="left" rowspan="2" style="width: 40px" valign="middle">
                        <img alt="" height="32" src="../App_Themes/CSS/Images/iconNew32x32.gif" width="32" /></td>
                    <td align="left" class="workAreaMainTitle" valign="middle">
                        <asp:label id="lblWorkFlow" runat="server" text="<%$ Resources:BaseInfo,WrkFlw_lblWorkFlow %>"
                            tooltip="为系统增加新的工作流和工作流节点"></asp:label>
                    </td>
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
                                    <td align="right" style="width: 63px; height: 25px">
                                        <asp:label id="lblBizGrpID" runat="server" cssclass="label" forecolor="Black" text="<%$ Resources:BaseInfo,WrkFlw_lblBizGrpID %>"
                                            width="54px"></asp:label>
                                    </td>
                                    <td align="left" style="width: 426px; color: #000000; height: 25px">
                                        <asp:dropdownlist id="cmbBizGrpID" runat="server" backcolor="White" cssclass="cmb160px"
                                            enabled="False">
                                                <asp:ListItem></asp:ListItem>
                                            </asp:dropdownlist>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="width: 63px; height: 25px">
                                        <asp:label id="lblVoucherTypeID" runat="server" cssclass="label" forecolor="Black"
                                            text="<%$ Resources:BaseInfo,WrkFlw_lblVoucherTypeID %>" width="60px"></asp:label>
                                    </td>
                                    <td align="left" style="width: 426px; color: #000000; height: 25px">
                                        <asp:dropdownlist id="cmbVoucherTypeID" runat="server" cssclass="cmb160px" enabled="False">
                                            </asp:dropdownlist>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="width: 63px; height: 25px">
                                        <asp:label id="lblWrkFlwName" runat="server" cssclass="label" forecolor="Black" text="<%$ Resources:BaseInfo,WrkFlw_lblWrkFlwName %>"
                                            width="101px"></asp:label>
                                    </td>
                                    <td align="left" style="width: 426px; color: #000000; height: 25px">
                                        <asp:textbox id="txtWrkFlwName" runat="server" cssclass="ipt160pxdisable" maxlength="32"
                                            readonly="True"></asp:textbox>
                                    </td>
                                </tr>
                                <tr style="color: #000000">
                                    <td align="right" style="width: 63px; height: 20px">
                                        <asp:label id="lblInitVoucher" runat="server" cssclass="label" text="<%$ Resources:BaseInfo,WrkFlw_lblInitVoucher %>"></asp:label>
                                    </td>
                                    <td align="left" style="width: 426px; height: 20px">
                                        <asp:dropdownlist id="cmbInitVoucher" runat="server" backcolor="White" cssclass="cmb160px"
                                            enabled="False">
                                            </asp:dropdownlist>
                                    </td>
                                </tr>
                                <tr style="color: #000000">
                                    <td align="right" style="width: 63px; height: 20px">
                                        <asp:label id="lblEfficiency" runat="server" cssclass="label" text="<%$ Resources:BaseInfo,WrkFlw_lblEfficiency %>"></asp:label>
                                    </td>
                                    <td align="left" style="width: 426px; height: 20px">
                                        <asp:textbox id="txtEfficiency" runat="server" cssclass="ipt140pxdisable" maxlength="18"
                                            readonly="True"></asp:textbox>
                                        <asp:label id="lblEfficiencyDay" runat="server" text="<%$ Resources:BaseInfo,WrkFlw_lblEfficiencyDay %>"
                                            width="1px"></asp:label>
                                    </td>
                                </tr>
                                <tr style="color: #000000">
                                    <td align="right" style="width: 63px; height: 20px">
                                        <asp:label id="lblTraceDays" runat="server" cssclass="label" text="<%$ Resources:BaseInfo,WrkFlw_lblTraceDays %>"></asp:label>
                                    </td>
                                    <td align="left" style="width: 426px; height: 20px">
                                        <asp:textbox id="txtTraceDays" runat="server" cssclass="ipt140pxdisable" maxlength="18"
                                            readonly="True"></asp:textbox>
                                        <asp:label id="lblTraceDaysDay" runat="server" text="<%$ Resources:BaseInfo,WrkFlw_lblTraceDaysDay %>"></asp:label>
                                    </td>
                                </tr>
                                <tr style="color: #000000">
                                    <td align="right" style="width: 63px; height: 20px">
                                        <asp:label id="lblWrkFlwStatus" runat="server" cssclass="label" text="<%$ Resources:BaseInfo,WrkFlw_lblWrkFlwStatus %>"></asp:label>
                                    </td>
                                    <td align="left" style="width: 426px; height: 20px">
                                        <asp:dropdownlist id="cmbWrkFlwStatus" runat="server" backcolor="White" cssclass="cmb160px"
                                            enabled="False">
                                            </asp:dropdownlist>
                                    </td>
                                </tr>
                                <tr style="color: #000000">
                                    <td align="right" style="width: 63px; height: 20px">
                                        <asp:label id="lblTransit" runat="server" cssclass="label" text="<%$ Resources:BaseInfo,WrkFlw_lblTransit %>"></asp:label>
                                    </td>
                                    <td align="left" style="width: 426px; height: 20px">
                                        <asp:dropdownlist id="cmbTransit" runat="server" backcolor="White" cssclass="cmb160px"
                                            enabled="False">
                                            </asp:dropdownlist>
                                    </td>
                                </tr>
                                <tr style="color: #000000">
                                    <td align="right" style="width: 63px; height: 20px">
                                        <asp:label id="lblWrkFlwProcessClass" runat="server" cssclass="label" text="<%$ Resources:BaseInfo,WrkFlw_lblWrkFlwProcessClass %>"></asp:label>
                                    </td>
                                    <td align="left" style="width: 426px; height: 20px">
                                        <asp:textbox id="txtWrkFlwProcessClass" runat="server" cssclass="ipt160pxdisable"
                                            maxlength="128" readonly="True"></asp:textbox>
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
                                    <td align="right" style="width: 61px; height: 25px">
                                        &nbsp;<asp:label id="lblNodeFuncAuthID" runat="server" cssclass="label" forecolor="Black"
                                            text="<%$ Resources:BaseInfo,WrkFlw_lblNodeFuncAuthID %>" width="66px"></asp:label></td>
                                    <td align="left" style="width: 518px; color: #000000; height: 25px">
                                        <asp:dropdownlist id="cmbNodeFuncID" runat="server" backcolor="White" cssclass="cmb160px"
                                            enabled="False">
                                            </asp:dropdownlist>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="width: 61px; height: 25px">
                                        &nbsp;<asp:label id="lblNodeRoleID" runat="server" cssclass="label" forecolor="Black"
                                            text="<%$ Resources:BaseInfo,WrkFlw_lblNodeRoleID %>" width="62px"></asp:label></td>
                                    <td align="left" style="width: 518px; color: #000000; height: 25px">
                                        <asp:dropdownlist id="cmbNodeRoleID" runat="server" backcolor="White" cssclass="cmb160px"
                                            enabled="False">
                                            </asp:dropdownlist>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="width: 61px; height: 25px">
                                        &nbsp;<asp:label id="lblNodeNodetName" runat="server" cssclass="label" forecolor="Black"
                                            text="<%$ Resources:BaseInfo,WrkFlw_lblNodeNodetName %>" width="62px"></asp:label></td>
                                    <td align="left" style="width: 518px; color: #000000; height: 25px">
                                        <asp:textbox id="txtNodeNodeName" runat="server" cssclass="ipt160pxdisable" maxlength="32"
                                            readonly="True"></asp:textbox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="width: 61px; height: 25px">
                                        &nbsp;<asp:label id="lblNodeWrkStep" runat="server" cssclass="label" forecolor="Black"
                                            text="<%$ Resources:BaseInfo,WrkFlw_lblNodeWrkStep %>" width="62px"></asp:label></td>
                                    <td align="left" style="width: 518px; color: #000000; height: 25px">
                                        <asp:textbox id="txtNodeWrkStep" runat="server" cssclass="ipt160pxdisable" maxlength="18"
                                            readonly="True"></asp:textbox>
                                    </td>
                                </tr>
                                <tr style="color: #000000">
                                    <td align="right" style="width: 61px; height: 20px">
                                        <asp:label id="lblNodeSmtToMgr" runat="server" cssclass="label" text="<%$ Resources:BaseInfo,WrkFlw_lblNodeSmtToMgr %>"
                                            width="74px"></asp:label>
                                    </td>
                                    <td align="left" style="width: 518px; height: 20px">
                                        <asp:dropdownlist id="cmbNodeSmtToMgr" runat="server" backcolor="White" cssclass="cmb160px"
                                            enabled="False">
                                            </asp:dropdownlist>
                                    </td>
                                </tr>
                                <tr style="color: #000000">
                                    <td align="right" style="width: 61px; height: 20px">
                                        &nbsp;<asp:label id="lblNodeValidAfterConfirm" runat="server" cssclass="label" text="<%$ Resources:BaseInfo,WrkFlw_lblNodeValidAfterConfirm %>"
                                            width="68px"></asp:label></td>
                                    <td align="left" style="width: 518px; height: 20px">
                                        <asp:dropdownlist id="cmbNodeValidAfterConfirm" runat="server" backcolor="White"
                                            cssclass="cmb160px" enabled="False">
                                            </asp:dropdownlist>
                                    </td>
                                </tr>
                                <tr style="color: #000000">
                                    <td align="right" style="width: 61px; height: 20px">
                                        <asp:label id="lblNodePrintAfterConfirm" runat="server" cssclass="label" text="<%$ Resources:BaseInfo,WrkFlw_lblNodePrintAfterConfirm %>"
                                            width="74px"></asp:label>
                                    </td>
                                    <td align="left" style="width: 518px; height: 20px">
                                        <asp:dropdownlist id="cmbNodePrintAfterConfirm" runat="server" backcolor="White"
                                            cssclass="cmb160px" enabled="False">
                                            </asp:dropdownlist>
                                    </td>
                                </tr>
                                <tr style="color: #000000">
                                    <td align="right" style="width: 61px; height: 20px">
                                        <asp:label id="lblNodeLongestDelay" runat="server" cssclass="label" text="<%$ Resources:BaseInfo,WrkFlw_lblNodeLongestDelay %>"
                                            width="76px"></asp:label>
                                    </td>
                                    <td align="left" style="width: 518px; height: 20px">
                                        <asp:textbox id="txtNodeLongestDelay" runat="server" cssclass="ipt140pxdisable" maxlength="18"
                                            readonly="True"></asp:textbox>
                                        <asp:label id="lbllblNodeLongestDelayHour" runat="server" text="<%$ Resources:BaseInfo,WrkFlw_lbllblNodeLongestDelayHour %>"
                                            width="31px"></asp:label>
                                    </td>
                                </tr>
                                <tr style="color: #000000">
                                    <td align="right" style="width: 61px; height: 21px">
                                        &nbsp;<asp:label id="lblNodeTimeoutHandler" runat="server" cssclass="label" text="<%$ Resources:BaseInfo,WrkFlw_lblNodeTimeoutHandler %>"
                                            width="50px"></asp:label></td>
                                    <td align="left" style="width: 518px; height: 21px">
                                        <asp:dropdownlist id="cmbNodeTimeoutHandler" runat="server" backcolor="White" cssclass="cmb160px"
                                            enabled="False">
                                            </asp:dropdownlist>
                                    </td>
                                </tr>
                                <tr style="color: #000000">
                                    <td align="right" style="width: 61px; height: 20px">
                                        <asp:label id="lblNodeProcessClass" runat="server" cssclass="label" text="<%$ Resources:BaseInfo,WrkFlw_lblNodeProcessClass %>"></asp:label>
                                    </td>
                                    <td align="left" style="width: 426px; height: 20px">
                                        <asp:textbox id="txtNodeProcessClass" runat="server" cssclass="ipt160pxdisable" maxlength="128"
                                            readonly="True"></asp:textbox>
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
                                                &nbsp;<asp:gridview id="GridView1" runat="server" autogeneratecolumns="False" cssclass="gridview"
                                                    height="118px" rowheadercolumn="abc" width="680px">
                                                        <Columns>
                                                            <asp:BoundField DataField="NodeName" HeaderText="节点名称" ></asp:BoundField>
                                                            <asp:BoundField DataField="WrkStep" HeaderText="节点步数" ></asp:BoundField>
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
                                                                <ItemStyle CssClass="hidden"  />
                                                                <HeaderStyle CssClass="hidden"  />
                                                                <FooterStyle CssClass="hidden"  />
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
                                                    </asp:gridview>&nbsp;<br />
                                                <asp:button id="btnDelete" runat="server" cssclass="btnOn50px" onclick="Button1_Click"
                                                    text="删   除" />
                                                <br />
                                                <br />
                                                <br />
                                                <br />
                                                <br />
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

   
   
   
    
 
  





 
 
 
 



<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GiftApplication.aspx.cs" Inherits="Associator_GiftApplication" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Associator_chkExtend")%></title>
    <link href="../App_Themes/CSS/Rool.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Tabbar/css/webtab.css" rel="stylesheet" type="text/css" />
    <script src="../App_Themes/DateTime/popcalendar.js" type="text/javascript"></script>
    <script type="text/javascript"  src="../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript"  src="../JavaScript/setday.js"></script>
	<script type="text/javascript" src="../JavaScript/Common.js"></script>
	<script language="javascript" type="text/javascript" src="../JavaScript/TabTools.js"></script>
	<script type="text/javascript">
		function Load()
	    {
	        addTabTool(document.getElementById("Associator_chkExtend").value + ",Associator/Largess.aspx");
	        loadTitle();
	    }
	</script>
</head>
<body  style="margin-top:0; margin-left:0" onload="Load();">
    <form id="form1" runat="server">
    <div>
        <table id="TABLE0" border="0" cellpadding="0" cellspacing="0" style="height: 24px;
            width: 100%; text-align: center;">
            <tr>
                <td class="tdTopBackColor" style="width: 5px">
                    <img alt="" class="imageLeftBack" />
                </td>
                <td class="tdTopBackColor">
                    <%= (String)GetGlobalResourceObject("BaseInfo", "Associator_chkExtend")%>
                </td>
            </tr>
        </table>
    <table width="100%" height="450" border="0" cellpadding="0" cellspacing="0">
  <tr>
    <td height="160" align="center" valign="top" class="tdBackColor"><table width="800" height="160" border="0" cellpadding="0" cellspacing="0">
      <tr>
        <td height="35" align="left" valign="middle"><table width="750" height="35" border="0" cellpadding="0" cellspacing="0">
          <tr>
            <td width="250" align="center" valign="middle"><table width="250" height="35" border="0" cellpadding="0" cellspacing="0">
              <tr>
                <td width="100" align="left" valign="middle">
                    <asp:Literal ID="lblApplicationNum" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblApplicationNum %>"></asp:Literal></td>
                <td width="150" align="left" valign="middle">&nbsp;</td>
              </tr>
            </table></td>
            <td width="250" align="center" valign="middle"><table width="250" height="35" border="0" cellpadding="0" cellspacing="0">
              <tr>
                <td width="100" align="left" valign="middle">
                    <asp:Literal ID="lblApplicationDate" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblApplicationDate %>"></asp:Literal></td>
                <td width="150" align="left" valign="middle">&nbsp;</td>
              </tr>
            </table></td>
            <td width="250" align="center" valign="middle"><table width="250" height="35" border="0" cellpadding="0" cellspacing="0">
              <tr>
                <td width="100" align="left" valign="middle">
                    <asp:Literal ID="lblExtendDate" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblExtendDate %>"></asp:Literal></td>
                <td width="150" align="left" valign="middle">&nbsp;</td>
              </tr>
            </table></td>
          </tr>
        </table></td>
      </tr>
      <tr>
        <td height="125" align="left" valign="top"><table width="750" height="125" border="0" cellpadding="0" cellspacing="0">
          <tr>
            <td width="535" height="125" align="left" valign="top"><table width="535" height="125" border="0" cellpadding="0" cellspacing="0">
              <tr>
                <td height="40"><table width="535" height="40" border="0" cellpadding="0" cellspacing="0">
                  <tr>
                    <td width="300" align="left" valign="middle"><table width="300" height="40" border="0" cellpadding="0" cellspacing="0">
                      <tr>
                        <td align="left" style="width: 80px">
                            <asp:Literal ID="lblAssociatorCard" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblAssociatorCard %>"></asp:Literal></td>
                        <td width="160" align="left">&nbsp;</td>
                        <td align="left">&nbsp;</td>
                      </tr>
                    </table></td>
                    <td width="235" align="left" valign="middle"><table width="235" height="40" border="0" cellpadding="0" cellspacing="0">
                      <tr>
                        <td colspan="2" align="left" valign="middle">　
                            <asp:Literal ID="lblCardNumInput" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblCardNumInput %>"></asp:Literal></td>
                      </tr>
                      <tr>
                        <td width="117" height="25" align="center" valign="middle">
                            <asp:RadioButton ID="rdoCardReader" runat="server" Text="<%$ Resources:BaseInfo,Associator_rdoCardReader %>" /></td>
                        <td width="118" height="25" align="center" valign="middle">
                            <asp:RadioButton ID="rdoKeyset" runat="server" Text="<%$ Resources:BaseInfo,Associator_rdoKeyset %>" /></td>
                      </tr>
                    </table></td>
                  </tr>
                </table></td>
              </tr>
              <tr>
                <td height="85"><table width="535" height="85" border="0" cellpadding="0" cellspacing="0">
                  <tr>
                    <td width="170" height="25" align="left" valign="bottom"><table width="170" height="25" border="0" cellpadding="0" cellspacing="0">
                      <tr>
                        <td align="left" valign="bottom" style="width: 70px">
                            <asp:Literal ID="lblAssociatorSort" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblAssociatorSort %>"></asp:Literal></td>
                        <td width="20" align="left" valign="bottom">&nbsp;</td>
                        <td width="80" align="left" valign="bottom">&nbsp;</td>
                      </tr>
                    </table></td>
                    <td width="185" align="center" valign="bottom"><table width="170" height="25" border="0" cellpadding="0" cellspacing="0">
                      <tr>
                        <td width="70" align="left" valign="bottom">会员卡状态</td>
                        <td width="20" align="left" valign="bottom">&nbsp;</td>
                        <td width="80" align="left" valign="bottom">&nbsp;</td>
                      </tr>
                    </table></td>
                    <td width="180" align="right" valign="bottom"><table width="170" height="25" border="0" cellpadding="0" cellspacing="0">
                      <tr>
                        <td width="70" align="left" valign="bottom">失效日期</td>
                        <td width="20" align="left" valign="bottom">&nbsp;</td>
                        <td width="80" align="left" valign="bottom">&nbsp;</td>
                      </tr>
                    </table></td>
                  </tr>
                  <tr>
                    <td width="170" height="28" align="left" valign="middle"><table width="170" height="25" border="0" cellpadding="0" cellspacing="0">
                      <tr>
                        <td width="70" align="left" valign="bottom">
                            <asp:Literal ID="lblAssociatorNum" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblAssociatorSort %>"></asp:Literal></td>
                        <td width="100" align="left" valign="bottom">&nbsp;</td>
                      </tr>
                    </table></td>
                    <td colspan="2" align="left" valign="middle"><table width="220" height="25" border="0" cellpadding="0" cellspacing="0">
                      <tr>
                        <td width="70" align="center" valign="bottom">身份证号</td>
                        <td width="150" align="left" valign="bottom">&nbsp;</td>
                      </tr>
                    </table></td>
                  </tr>
                  <tr>
                    <td height="32" colspan="3" align="left" valign="middle"><table width="400" height="25" border="0" cellpadding="0" cellspacing="0">
                      <tr>
                        <td width="80">名　　称</td>
                        <td width="60">&nbsp;</td>
                        <td width="260">&nbsp;</td>
                      </tr>
                    </table></td>
                  </tr>
                </table></td>
              </tr>
            </table></td>
            <td width="215" align="center" valign="top"><table width="200" height="100" border="0" cellpadding="0" cellspacing="0">
              <tr>
                <td height="20" align="left" valign="middle">积分信息</td>
              </tr>
              <tr>
                <td height="30" align="left" valign="middle"><table width="170" height="25" border="0" cellpadding="0" cellspacing="0">
                  <tr>
                    <td width="80" align="left" valign="middle">当前积分</td>
                    <td width="90" align="left" valign="middle">&nbsp;</td>
                  </tr>
                </table></td>
              </tr>
              <tr>
                <td height="25" align="left" valign="middle"><table width="170" height="25" border="0" cellpadding="0" cellspacing="0">
                  <tr>
                    <td width="80" align="left" valign="middle">申请使用积分</td>
                    <td width="90" align="left" valign="middle">&nbsp;</td>
                  </tr>
                </table></td>
              </tr>
              <tr>
                <td height="25" align="left" valign="middle"><table width="170" height="25" border="0" cellpadding="0" cellspacing="0">
                  <tr>
                    <td width="80" align="left" valign="middle"><p>可用积分</p></td>
                    <td width="90" align="left" valign="middle">&nbsp;</td>
                  </tr>
                </table></td>
              </tr>
            </table></td>
          </tr>
        </table></td>
      </tr>
    </table></td>
  </tr>
  <tr>
    <td height="240" align="center" valign="top" class="tdBackColor"><table width="800" height="210" border="0" cellpadding="0" cellspacing="0">
      <tr>
        <td height="160" align="center" valign="middle"><table width="780" height="140" border="0" cellpadding="0" cellspacing="0">
          <tr>
            <td width="130" height="20" align="center" valign="bottom">礼品编号</td>
            <td width="290" height="20" align="center" valign="bottom">礼品名称</td>
            <td width="110" height="20" align="center" valign="bottom">所需积分</td>
            <td width="60" height="20" align="center" valign="bottom">数量</td>
            <td width="190" height="20" align="center" valign="bottom">积分小计</td>
          </tr>
          <tr>
            <td height="20" align="center" valign="bottom">&nbsp;</td>
            <td height="20" align="center" valign="bottom">&nbsp;</td>
            <td height="20" align="center" valign="bottom">&nbsp;</td>
            <td height="20" align="center" valign="bottom">&nbsp;</td>
            <td height="20" align="center" valign="bottom">&nbsp;</td>
          </tr>
          <tr>
            <td height="20" align="center" valign="bottom">&nbsp;</td>
            <td height="20" align="center" valign="bottom">&nbsp;</td>
            <td height="20" align="center" valign="bottom">&nbsp;</td>
            <td height="20" align="center" valign="bottom">&nbsp;</td>
            <td height="20" align="center" valign="bottom">&nbsp;</td>
          </tr>
          <tr>
            <td height="20" align="center" valign="bottom">&nbsp;</td>
            <td height="20" align="center" valign="bottom">&nbsp;</td>
            <td height="20" align="center" valign="bottom">&nbsp;</td>
            <td height="20" align="center" valign="bottom">&nbsp;</td>
            <td height="20" align="center" valign="bottom">&nbsp;</td>
          </tr>
          <tr>
            <td height="20" align="center" valign="bottom">&nbsp;</td>
            <td height="20" align="center" valign="bottom">&nbsp;</td>
            <td height="20" align="center" valign="bottom">&nbsp;</td>
            <td height="20" align="center" valign="bottom">&nbsp;</td>
            <td height="20" align="center" valign="bottom">&nbsp;</td>
          </tr>
          <tr>
            <td height="20" align="center" valign="bottom">&nbsp;</td>
            <td height="20" align="center" valign="bottom">&nbsp;</td>
            <td height="20" align="center" valign="bottom">&nbsp;</td>
            <td height="20" align="center" valign="bottom">&nbsp;</td>
            <td height="20" align="center" valign="bottom">&nbsp;</td>
          </tr>
          <tr>
            <td height="20" align="center" valign="bottom">&nbsp;</td>
            <td height="20" align="center" valign="bottom">&nbsp;</td>
            <td height="20" align="center" valign="bottom">&nbsp;</td>
            <td height="20" align="center" valign="bottom">&nbsp;</td>
            <td height="20" align="center" valign="bottom">&nbsp;</td>
          </tr>
        </table></td>
      </tr>
      <tr>
        <td height="50" align="right" valign="middle"><table width="400" height="50" border="0" cellpadding="0" cellspacing="0">
          <tr>
            <td width="150" align="center" valign="middle">申请确认</td>
            <td width="250" align="left" valign="middle"><table width="200" height="40" border="0" cellpadding="0" cellspacing="0">
              <tr>
                <td width="60" align="right" valign="middle">总计</td>
                <td width="10" align="left" valign="middle">&nbsp;</td>
                <td width="130" align="left" valign="middle">&nbsp;</td>
              </tr>
              <tr>
                <td align="right" valign="middle">剩余积分</td>
                <td align="left" valign="middle">&nbsp;</td>
                <td align="left" valign="middle">&nbsp;</td>
              </tr>
            </table></td>
          </tr>
        </table></td>
      </tr>
    </table></td>
  </tr>
</table>
    </div>
    </form>
</body>
</html>

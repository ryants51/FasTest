<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="FasTest._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.3/jquery.min.js"></script>
    <script type="text/javascript">
        $(function () {
            var td = $('input[value="Log In"]').parent();
            $(td).attr("align", "center");

        });
    </script>
    <div class="parallax">
        <div class="row">
            <div class="col-sm-6 col-sm-offset-3 white">
                <div class="jumbotron">

                    <asp:Image ID="Logo" Width="100%" CssClass="Logo" ImageUrl="~/Images/LogoSmall.png" runat="server" />

                    <!--<img src="logo"/>-->
                    <asp:Login ID="Login1" CssClass="loginbox" LoginButtonStyle-CssClass="LoginbuttonClass" LoginButtonStyle-Width="100%" TextBoxStyle-CssClass="LogintextBoxes" runat="server" OnAuthenticate="ValidateUser" DisplayRememberMe="False" TitleText="" PasswordLabelText="<tr><td>Password:</td></tr>" UserNameLabelText="User:">

                        <LayoutTemplate>
                            <table cellpadding="1" cellspacing="0" style="border-collapse: collapse;">
                                <tr>
                                    <td>
                                        <table cellpadding="0">
                                            <tr>

                                                <td style="width: 103%; text-align: center;" class="nav-justified">
                                                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" Style="margin-right: 0px; text-align: center;" Width="163px">User:</asp:Label>
                                                    <br />
                                                    <asp:TextBox ID="UserName" runat="server" CssClass="LogintextBoxes"></asp:TextBox>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td align="center" style="width: 103%; text-align: center;" class="nav-justified">
                                                    <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" Width="161px">Password:</asp:Label>
                                                    <br />
                                                    <asp:TextBox ID="Password" runat="server" CssClass="LogintextBoxes" TextMode="Password"></asp:TextBox>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td align="center" colspan="2" style="color: Red;">
                                                    <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" colspan="2">
                                                    <asp:Button ID="LoginButton" runat="server" CommandName="Login" CssClass="LoginbuttonClass" Text="Log In" ValidationGroup="Login1" Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </LayoutTemplate>
                        <LoginButtonStyle CssClass="LoginbuttonClass" Width="100%"></LoginButtonStyle>

                        <TextBoxStyle CssClass="LogintextBoxes"></TextBoxStyle>

                    </asp:Login>
                </div>
            </div>
        </div>
    </div>


</asp:Content>

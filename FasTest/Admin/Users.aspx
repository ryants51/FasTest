<%@ Page Title="Admin Home" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="FasTest.Users" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <%--                  </div>--%>
    <script>function EnterKeyFilter()
    {  if (window.event.keyCode == 13)
    {   event.returnValue=false;
        event.cancel = true;
    }
    }</script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
                        <script type="text/javascript">
                            $(function () {
                                //Hide label after 5 secs
                                setTimeout(function () {
                                    $("[id$=errorDeleteUser]").fadeOut("slow");
                                }, 10000);
                            });
                        </script>
    <h1>Users</h1>
    <asp:Label ID="errorDeleteUser" runat="server" Text="Could not Delete user because they are assigned to a class" ForeColor="Red" Visible="false"></asp:Label>
    <br />
    <br />
    <div class="row">
        <div class="col-lg-8">
            <div class="row">
                <%--<div class="col-lg-10">--%>
                    <asp:Panel ID="panelContainer" runat="server" Height="480px" Width="100%"  ScrollBars="Auto">
                    <asp:GridView ID="GridView1" CssClass="Grid"  HeaderStyle-CssClass="fixedHeader" onRowCommand="GridView1_RowCommand" runat="server" AllowSorting="True" DataSourceID="SqlDataSource2" AutoGenerateColumns="False" DataKeyNames="IDNumber, FirstName, LastName, Type" GridLines="None">
                        <Columns>
                            <asp:BoundField HeaderText="ID Number" DataField="IDNumber" InsertVisible="False" ReadOnly="True" SortExpression="IDNumber" />
                            <asp:BoundField HeaderText="Type" DataField="Type" SortExpression="Type" />
                            <asp:BoundField HeaderText="First Name" DataField="FirstName" SortExpression="FirstName" />
                            <asp:BoundField HeaderText="Last Name" DataField="LastName" SortExpression="LastName" />
                            <asp:TemplateField HeaderText="Delete/Modify">
                                <ItemTemplate>
                                    <asp:ImageButton ID="deleteButton" CommandArgument="<%# Container.DataItemIndex %>"   runat="server" ImageUrl="~/Images/delete-photo.png" CommandName="DeleteUser" />
                                    <asp:ImageButton runat="server" CommandArgument="<%# Container.DataItemIndex %>"  CommandName="EditUser" ImageUrl="~/Images/pencil-edit-button.png" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:ButtonField HeaderText="Update Password" ButtonType="Button" CommandName="UpdatePassword" Text="Update Password" />
                        </Columns>
                        <EditRowStyle BackColor="#ebebeb" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                    </asp:GridView>
                        </asp:Panel>
<%--                  </div>--%>
                </div>
            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:CS414_FasTestConnectionString %>" SelectCommand="Get_All_Users" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
        </div>

        <div id="sections" runat="server">
            <div class="col-lg-1 csNewUser col-md-3" style="margin-left:10px">
                <div id="NewUserInput" runat="server">
                    <h3 style="text-align: center; padding: 5px"><span id="newUser" runat="server">New User</span></h3>

                    <asp:Label ID="error1" ForeColor="Red" runat="server" Text=""></asp:Label>
                    <br />
                    <asp:Label ID="FirstName" runat="server" Text="First Name"></asp:Label>
                    <br />
                    <asp:TextBox ID="tbFirstName" CssClass="textBoxes" MaxLength="30" runat="server"></asp:TextBox>
                    <br />
                    <asp:Label ID="LastName" runat="server" Text="Last Name"></asp:Label>
                    <br />
                    <asp:TextBox ID="tbLastName" CssClass="textBoxes" MaxLength="30" runat="server"></asp:TextBox>
                    <br />
                    <asp:Label ID="error2" runat="server" Text=""></asp:Label>
                    <br />
                    <asp:Label ID="Password" runat="server" Text="Password"></asp:Label>
                    <br />
                    <asp:TextBox ID="tbPassword" MaxLength="64" CssClass="textBoxes" runat="server"></asp:TextBox>
                    <br />
                    <asp:Label ID="error3" ForeColor="Red" runat="server" Text=""></asp:Label>
                    <br />
                    <asp:Label ID="CredentialLevel" runat="server" Text="Credential Level"></asp:Label>
                    <br />
                    <asp:DropDownList id="sctCredentialLevel" runat="server" name="SelectCategory" class="dropDownList">
                        <asp:ListItem value="1">Administrator</asp:ListItem>
                        <asp:ListItem value="2">Teacher</asp:ListItem>
                        <asp:ListItem value="3">Student</asp:ListItem>
                    </asp:DropDownList>
                    <br />
                    <div style="padding-top: 5px; padding-bottom: 5px">
                        <div style="text-align:center;">
                            <asp:Button ID="btnAddNewUser" CssClass="buttonClass" runat="server" Text="Create User" OnClick="btnAddNewUser_Click" />
                            <asp:Button ID="btnSaveUser" CssClass="buttonClass" runat="server" Text="Save User" OnClick="btnSaveUser_Click" Visible="false" />
                        <asp:Button ID="btnCancelEdit" CssClass="buttonClass" runat="server" Text="Cancel" OnClick="btnCancelEdit_Click" Visible="false" />
                        </div>
                        
                    </div>
                </div>

                <div id="changePasswordSection" style="width:300px" visible="false" runat="server">
                        <h3 style="text-align: center; padding: 5px">
                            <asp:Label id="changePasswordHeader" runat="server">Enter New Password for <p id="ChangePasswordName" runat="server"></p></asp:Label>
                        </h3>
                    <asp:Label ID="enterNewPassword"  runat="server" Text="Enter new password"></asp:Label>
                    <br />
                    <asp:TextBox ID="enterNewPasswordTB" MaxLength="64" TextMode="Password" CssClass="textBoxes" runat="server" ></asp:TextBox>
                    <br />
                    <asp:Label ID="errorPassU1" runat="server" ForeColor="Red" Text=""></asp:Label>    
                    <br />
                    <asp:Label ID="confirmNewPassword"  runat="server" Text="Confirm new password"></asp:Label>
                    <br />
                    <asp:TextBox ID="confirmNewPasswordTB" MaxLength="64" TextMode="Password" CssClass="textBoxes" runat="server" ></asp:TextBox>
                    <br />
                    
                    <div style="padding-top: 5px; padding-bottom: 5px">
                        <asp:Button ID="changePasswordButton" runat="server" CssClass="buttonClass" Text="Update Password" OnClick="changePasswordButton_Click" />
                        <asp:Button ID="btnCancleUpdatePassword" runat="server" CssClass="buttonClass" Text="Cancel" OnClick="btnCancleUpdatePassword_Click" />
                    </div>
                    <label id="lblpasswordError" style="color: red"></label>
                </div>
            </div>
        </div>
 </div>

    <asp:Button runat="server" ID="hiddenbutton" Style="display: none;" />
<!-- ModalPopupExtender -->
<ajaxToolKit:ModalPopupExtender ID="mp1" runat="server" PopupControlID="Panel1" TargetControlID="hiddenbutton"
    CancelControlID="btnCancel" BackgroundCssClass="modalBackground">
</ajaxToolKit:ModalPopupExtender>
<asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" align="center" style = "display:none">
    Are you sure you want to delete <span id="userName" runat="server"></span>?<br /><br />
    <asp:Button ID="btnConfirmDelteUser" runat="server" Text="Confirm" OnClick="btnConfirmDelteUser_Click" CssClass="buttonClass" />
    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonClass" />
</asp:Panel>
</asp:Content>

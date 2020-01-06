<%@ Page Title="Modify Accounts" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AccountPage.aspx.cs" Inherits="FasTest.AccountCreation" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Users</h1>
     <div class="SortMenu">
         <select id="Select1">
             <option>All Users</option>
             <option>Teachers</option>
             <option>Students</option>

         </select>
     </div> 

    <div class="row">
        <div class="column" style="background-color: #fff;">
           
            <table runat="server" style="width: 100%;">

                <tr>
                    <th>Id Number</th>
                    <th>First Name</th>
                    <th>Last Name</th>
                    <th># of Classes</th>
                </tr>
                
            </table>
        </div>
        <div class="columnRight" style="background-color: #ccc;">
            <div>
                
                    <h1>Create new user</h1>
                    <br />
                    Select an account type:
            <br />
                    <asp:DropDownList ID="DdlMonths" runat="server">
                        <asp:ListItem Enabled="true" Text="Account Type" Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Teacher" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Student" Value="3"></asp:ListItem>
                    </asp:DropDownList>

                    <br />
                    <br />
                    <asp:TextBox ID="InputFName" placeholder="First Name" runat="server"></asp:TextBox>
                    <br />
                    <br />
                    <asp:TextBox ID="InputLName" placeholder="Last Name" runat="server"></asp:TextBox>
                    <br />
                    <br />
                    <asp:TextBox ID="InputPass" placeholder="Password" runat="server"></asp:TextBox>
                    <br />
                    <br />
                    <asp:TextBox ID="InputVeriPass" placeholder="Verify Password" runat="server"></asp:TextBox>
                    <br />
                    <br />
                    <asp:Button ID="CreateUserBtn" Text="Create User" runat="server"></asp:Button>
                    <br />
                
            </div>
        </div>
    </div>

</asp:Content>


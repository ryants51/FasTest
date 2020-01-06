<%@ Page Title="Admin Home" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ClassOrganizer.aspx.cs" Inherits="FasTest.ClassOrganizer" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <h1>Classes</h1>
    <div class="row">
        <div style="text-align: left; border: 3px solid #2f96e8; background-color: #ededed" class="col-lg-3">
            <h3 style="text-align: center">Create New Class</h3>
            <br />
            1) Select or Create a Department:
		    <br />
            <div style="margin:3px;">
                 <asp:DropDownList ID="ddlGroupName" runat="server" CssClass="dropDownList" Style="width: auto;" DataSourceID="SqlDataSource1" DataValueField="GroupName" DataTextField="GroupName">
                </asp:DropDownList>
            </div>
           

            <asp:TextBox ID="inputNewGroup" CssClass="textBoxes" MaxLength="2" placeholder="Department" runat="server"></asp:TextBox>

            <br />

            <asp:Label ID="error1" ForeColor="Red" runat="server"></asp:Label>

            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:CS414_FasTestConnectionString %>" SelectCommand="SELECT DISTINCT [GroupName] FROM [Class]"></asp:SqlDataSource>
            <br />
            2) Select an instructor:
		    <br />
            <asp:DropDownList ID="ddlInstructor" runat="server" CssClass="dropDownList" DataSourceID="SqlDataSource3" DataTextField="Name" DataValueField="IDNumber">
            </asp:DropDownList>

            <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:CS414_FasTestConnectionString %>" SelectCommand="SELECT LastName + ', ' + FirstName as Name, IDNumber FROM FasTestUser where CredentialLevel=2 Order by LastName"></asp:SqlDataSource>
            <br />
            <br />
            3) Enter a Class Name:
			<br />
            <asp:TextBox ID="InputClassName" CssClass="textBoxes" TextMode="SingleLine" MaxLength="50" placeholder="Class Name" runat="server"></asp:TextBox>
            <br />


            <asp:Label ID="InvalidClassName" runat="server" ForeColor="Red" Text=""></asp:Label>
            <br />
            <div style="width: 100%; text-align: center">
                <asp:Button ID="btnCreateClass" Text="Create Class" CssClass="buttonClass" runat="server" OnClick="btnCreateClass_Click"></asp:Button>
            </div>

            <br />
        </div>

        <div style="text-align: center;" class="col-lg-9">
            <br />
            <asp:Panel ID="panelContainer" runat="server" Height="480px" Width="107%" ScrollBars="Auto">
                <asp:GridView ID="GridView2" runat="server" CssClass="Grid" HeaderStyle-CssClass="fixedHeader" AlternatingRowStyle-CssClass="alt" GridLines="None" AutoGenerateColumns="false" DataSourceID="SqlDataSource4" OnSelectedIndexChanged="GridView2_SelectedIndexChanged" AllowSorting="True">
                    <Columns>
                        <asp:CommandField ShowSelectButton="True" ControlStyle-CssClass="gridSelect" SelectText="Update Class" />
                        <asp:BoundField HeaderText="Class ID" DataField="ClassID" InsertVisible="False" ReadOnly="True" SortExpression="ClassID" />
                        <asp:BoundField HeaderText="Class Title" DataField="ClassTitle" SortExpression="ClassTitle" />
                        <asp:BoundField HeaderText="Instructor" DataField="InstructorName" SortExpression="InstructorName" />
                        <asp:BoundField HeaderText="Department" DataField="GroupName" SortExpression="GroupName" />
                        <asp:BoundField HeaderText="Start Date" DataField="StartDate" SortExpression="StartDate"/>
                        <asp:BoundField HeaderText="End Date" DataField="EndDate" SortExpression="EndDate"/>
                    </Columns>
                    <EditRowStyle BackColor="#ebebeb" />
                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                </asp:GridView>
            </asp:Panel>
            <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:CS414_FasTestConnectionString %>"
                SelectCommand="Get_Class_Information" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
        </div>
    </div>
</asp:Content>

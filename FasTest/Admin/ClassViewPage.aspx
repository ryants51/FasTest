<%@ Page Title="Admin Home" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ClassViewPage.aspx.cs" Inherits="FasTest.Admin.ClassViewPage" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <h1 style="text-align:center">Update Class Information:</h1>
    <h2 id="ClassTitle" runat="server" style="font-weight:700; text-align:center; margin-top:0px">Course: </h2>
     <h3 id="CourseInstructor" runat="server" style="font-weight:700; text-align:center;">&nbsp;</h3>
    <div class="row">
        <div class="col-lg-2" style="break-after: always; text-align: center;">         
            <div class="csNewUser" style="width: auto; margin-top:12px">
                
                <h3>Edit Class Information</h3>
               
            <asp:SqlDataSource ID="ClassInstructor" runat="server"></asp:SqlDataSource>
            <!--Edit class div-->
            <div id="EditClass">
                
                <div style="text-align:center; margin:2px; margin-bottom:5px">
                    Edit Instructor
                    <br />
                
                    <asp:DropDownList  ID="ddlInstructorName" runat="server" CssClass="dropDownList" DataSourceID="SelectInstructor" DataTextField="Name" DataValueField="IDNumber" OnSelectedIndexChanged="ddlInstructorName_SelectedIndexChanged">
                    </asp:DropDownList>
                    <div style="margin-top:3px;">
                       <asp:Button ID="btnSetTeacher" CssClass="buttonClass" runat="server" OnClick="btnSetTeacher_Click" Text="Update" />
                    </div>
                </div>
                <br />
                <div style="text-align:center; margin:2px">
                    Edit Class Name
                    <br />
                    <asp:TextBox ID="txtClassTitle" MaxLength="50" CssClass="textBoxes" style="max-width: 150px" runat="server"></asp:TextBox>
                    <div style="margin-top:3px;">
                        <asp:Button ID="btnUpdateClassTitle" CssClass="buttonClass" runat="server" OnClick="btnUpdateClassTitle_Click" Text="Update" />
                    </div>
                    

                </div>
                
                <asp:SqlDataSource ID="SelectInstructor" runat="server" ConnectionString="<%$ ConnectionStrings:CS414_FasTestConnectionString %>" SelectCommand="Get_Teachers" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                <br />
                <div style="text-align:center; margin:2px">
                    Edit Department
                    <br />
                    <asp:DropDownList ID="ddlGroupName" runat="server" CssClass="dropDownList" DataSourceID="SqlDataSource1" DataValueField="GroupName" DataTextField="GroupName">
                    </asp:DropDownList>
                    <br />
                    <div style="margin-top:3px;">
                        <asp:Button ID="btnUpdateGroup" runat="server" CssClass="buttonClass" Text="Update" OnClick="btnUpdateGroup_Click" />
                    </div>
                    
                </div>
                
                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:CS414_FasTestConnectionString %>" SelectCommand="SELECT DISTINCT [GroupName] FROM [Class]"></asp:SqlDataSource>
            </div>
        </div>
        </div>
        <div class="col-lg-4" style="text-align: center">
            <h3>Currently Enrolled Students</h3>
            <br />
            <asp:Panel ID="panel1" runat="server" Height="400px" Width="110%" ScrollBars="Auto">
            <asp:GridView ID="StudentList" runat="server" CssClass="Grid" HeaderStyle-CssClass="fixedHeader" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" AllowSorting="True" OnPageIndexChanging="GridView1_PageIndexChanging" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="StudentID" HeaderText="IDNumber" InsertVisible="False" ReadOnly="True" />
                    <asp:BoundField DataField="NAME" HeaderText="Student Name"/>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="chkBoxlbl" ToolTip="Whether or not the student has access to this class" runat="server" Text="Active Status"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkEnrolled" runat="server" Checked='<%# Eval("IsEnrolled") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            </asp:Panel>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:CS414_FasTestConnectionString %>" SelectCommand="select distinct GroupName from Class"></asp:SqlDataSource>
            <br />
            <div style:"text-align: right">
            <asp:Button ID="btnSubmit" runat="server" CssClass="buttonClass" Text="Activate/Deactivate Student" OnClick="btnSubmit_Click" />

            </div>
        </div>
        <div class="col-lg-4 col-lg-offset-1" style="text-align: center">
            <h3>All Un-Enrolled Students</h3>
            <br />
            <asp:Panel ID="panelContainer" runat="server" Height="400px" Width="110%"  ScrollBars="Auto">
            <asp:GridView ID="EnrollNewGridview" runat="server" CssClass="Grid" AlternatingRowStyle-CssClass="alt" AllowSorting="True" HeaderStyle-CssClass="fixedHeader" AutoGenerateColumns="False" DataSourceID="EnrollNewUser" DataKeyNames="IDNumber">
                <Columns>
                    <asp:BoundField DataField="IDNumber" HeaderText="IDNumber" InsertVisible="False" ReadOnly="True" SortExpression="IDNumber" />
                    <asp:BoundField DataField="NAME" HeaderText="Student Name" SortExpression="NAME" />
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="chkBoxlbl" ToolTip="Check the box to enroll a student" runat="server" Text="Enroll Student"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkNewEnrolled" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
                </asp:Panel>
            <asp:Button ID="btnEnrollNewUsers" runat="server" CssClass="buttonClass" Text="Enroll New Users" OnClick="btnEnrollNewUsers_Click" />
        </div>
        <asp:SqlDataSource ID="EnrollNewUser" runat="server" ConnectionString="<%$ ConnectionStrings:CS414_FasTestConnectionString %>" SelectCommand="Get_Unenrolled_Users" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:CookieParameter CookieName="SelectedClassID" DefaultValue="0" Name="pClassID" Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
</asp:Content>


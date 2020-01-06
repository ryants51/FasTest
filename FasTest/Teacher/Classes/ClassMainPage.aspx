<%@ Page Title="Classes" Language="C#" AutoEventWireup="true" MasterPageFile="~/Teacher/Teacher.Master" CodeBehind="ClassMainPage.aspx.cs" Inherits="FasTest.Classes.ClassMainPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script>$(document).ready(function () {
            $("#contentPanel").hide();
            $("#headerPanel").click(function () {
                $(this).next("#contentPanel").slideToggle("medium");
            });
        });</script>

    <div class="container">
        <div class="row">
            <div class="col-lg-6">
                <h1>Classes</h1>
                <br />

                <asp:Panel ID="panelContainer" maintainScrollPositionOnPostBack="true" runat="server" Height="480px" Width="74%" ScrollBars="Auto">
                <asp:GridView ID="GridView1" runat="server" CssClass="Grid" HeaderStyle-CssClass="fixedHeader" AlternatingRowStyle-CssClass="alt" GridLines="Horizontal" AutoGenerateColumns="False" DataKeyNames="ClassID" DataSourceID="SqlDataSource2" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
                    <Columns>
                        <asp:CommandField ShowSelectButton="True" ControlStyle-CssClass="gridSelect" />
                        <asp:BoundField DataField="GroupName" HeaderText="Department" SortExpression="GroupName" />
                        <asp:BoundField DataField="ClassTitle" HeaderText="Class Title" SortExpression="ClassTitle" />
                    </Columns>
                </asp:GridView>
                    </asp:Panel>
                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:CS414_FasTestConnectionString %>"
                    SelectCommand="SELECT * FROM [Class] WHERE InstructorID = "></asp:SqlDataSource>
            </div>
            <div class="col-lg-6">
                <h1 runat="server" id="ClassName"></h1>
                <asp:ListView ID="ClassInformation" runat="server">
                   <ItemTemplate>
                       <!-- 
                           StartDate, EndDate, @DaysLeft, @StudentCount, @HighGrade, @LowGrade
                           -->
                       <asp:Label ID="studentCountLabel" Text="Number of Students: " runat="server" ></asp:Label>
                       <asp:Label ID="StudentCount"  Text='<%# Eval("StudentCount") %>' runat="server" CssClass="gradesPageOverallGrade" />
                       <br />
                       <asp:Label ID="highestGradeLabel" Text="Highest Grade: " runat="server" ></asp:Label>
                       <asp:Label ID="HighGrade"  Text='<%# Eval("HighGrade") %>' runat="server" CssClass="gradesPageOverallGrade" />
                       <br />
                       <asp:Label ID="lowestGradeLabel" Text="Lowest Grade: " runat="server" ></asp:Label>
                       <asp:Label ID="LowGrade"  Text= '<%# Eval("LowGrade") %>' runat="server" CssClass="gradesPageOverallGrade" />
                   </ItemTemplate>
                </asp:ListView>
                <asp:Panel ID="panel1" runat="server" Height="480px"  ScrollBars="Auto">
                <asp:GridView ID="StudentsEnrolledgrid" HeaderStyle-CssClass="fixedHeader" CssClass="Grid" runat="server" AutoGenerateColumns="False" DataSourceID="ClassStudents">
                    <Columns>
                        <asp:BoundField DataField="Student Name" HeaderText="Student Name" ReadOnly="True" SortExpression="Student Name" />
                        <asp:BoundField DataField="LetterGrade" HeaderText="Class Average" ReadOnly="True" SortExpression="LetterGrade" />
                    </Columns>
                </asp:GridView>
                </asp:Panel>
            </div>
            <asp:SqlDataSource ID="ClassStudents" runat="server" ConnectionString="<%$ ConnectionStrings:CS414_FasTestConnectionString %>" SelectCommand="Get_Students_and_Grades" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:Parameter DefaultValue="0" Name="pClassID" Type="Int32" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
    </div>

    <asp:SqlDataSource ID="GetClassName" SelectCommand="SELECT [ClassTitle] FROM [Class] where classId = " runat="server" ConnectionString="<%$ ConnectionStrings:CS414_FasTestConnectionString %>"></asp:SqlDataSource>
</asp:Content>

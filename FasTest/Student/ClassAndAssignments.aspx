<%@ Page Title="" Language="C#" MasterPageFile="~/Student/Student.Master" AutoEventWireup="true" CodeBehind="ClassAndAssignments.aspx.cs" Inherits="FasTest.Student.ClassViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Tests</h1>
    <div class="container">
        <div class="row">
            <div class="col col-lg-4" style="text-align:center">
                <h3 style="text-align:left">Classes</h3>
                <asp:GridView ID="ClassList" runat="server" CssClass="Grid" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" OnRowCommand="ClassList_RowCommand" DataSourceID="ClassListSource" AutoPostBack="True" AutoGenerateColumns="False" AllowSorting="True" AllowPaging="true" DataKeyNames="ClassID" Width="10px" CellPadding="100">
                    <Columns>
                        <asp:CommandField ShowSelectButton="True" ControlStyle-CssClass="gridSelect" ItemStyle-CssClass="gridSelectCell" />
                        <asp:BoundField DataField="ClassTitle" HeaderText="Class Title" SortExpression="ClassTitle" />
                        <asp:BoundField DataField="Teacher" HeaderText="Teacher" ReadOnly="True" SortExpression="Teacher" />
                    </Columns>
                    <SelectedRowStyle BackColor="#EFEFEF" />
                </asp:GridView>
            </div>
            <div class="col col-lg-7" style="margin-left:10px">
                <h3>Available Tests</h3>
                <asp:GridView ID="AssignmentList" runat="server" CssClass="Grid" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" AllowPaging="True" DataKeyNames="AssignmentID" AutoGenerateColumns="false" EmptyDataText="There are no tests for this class" OnRowCommand="AssignmentList_RowCommand">
                    <Columns>
                        <asp:CommandField ShowSelectButton="True" SelectText="Begin Test" ControlStyle-CssClass="gridSelect" />
                        <asp:BoundField DataField="AssignmentName" HeaderText="Assignment" />
                        <asp:BoundField DataField="StartDate" HeaderText="Start Date" />
                        <asp:BoundField DataField="Deadline" HeaderText="Deadline" />
                        <asp:BoundField DataField="TestDuration" HeaderText="Test Duration (min)" />
                        <asp:BoundField DataField="PointsPossible" HeaderText="Points Possible" />
                    </Columns>
                </asp:GridView>
            </div>

        </div>
    </div>

    <br />

    <asp:SqlDataSource ID="ClassListSource" runat="server" ConnectionString="<%$ ConnectionStrings:CS414_FasTestConnectionString %>" SelectCommand="Get_Student_Classes" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:CookieParameter CookieName="StudentId" DefaultValue="-1" Name="pStudentID" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <p runat="server" id="debug"></p>
</asp:Content>

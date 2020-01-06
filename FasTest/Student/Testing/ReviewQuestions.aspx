<%@ Page Title="" Language="C#" MasterPageFile="~/Student/Student.Master" AutoEventWireup="true" CodeBehind="ReviewQuestions.aspx.cs" Inherits="FasTest.Student.Testing.ReviewQuestions" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Review Test</h1>
    <asp:GridView ID="gvQuestions" runat="server" CssClass="Grid" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" AllowPaging="true" AutoGenerateColumns="False" DataSourceID="TestQuestions">
        <Columns>
            <asp:BoundField DataField="Question" HeaderText="Question" SortExpression="Question" />
            <asp:BoundField DataField="Student Answer" HeaderText="Student Answer" ReadOnly="True" SortExpression="Student Answer" />
            <asp:BoundField DataField="Points" HeaderText="Points" SortExpression="Points" />
        </Columns>
    </asp:GridView>
    <div style="margin:20px">
        <h3>Sign Pledge</h3>
        <p>“I affirm that I did not receive nor will I give any unauthorized help on this exam, and that all work is my own."</p>
        <p>To sign the pledge, enter your name in the textbox below exactly as it appears here:</p>
        <p id="pledge" runat="server"></p>
        <asp:TextBox ID="tbxPledge" runat="server"></asp:TextBox>
    </div>
    <div style="margin:20px;">
            <asp:Button ID="btnEdit" runat="server" CssClass="buttonClass" Text="Return to Test" OnClick="btnEdit_Click"   />
           <asp:Button ID="btnSubmit" runat="server" CssClass="buttonClass" Text="Submit Test" OnClick="btnSubmit_Click" />
    </div>
    <asp:SqlDataSource ID="TestQuestions" runat="server" ConnectionString="<%$ ConnectionStrings:CS414_FasTestConnectionString %>" SelectCommand="Display_Answers_To_Student" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:CookieParameter CookieName="pStudentAssignment" DefaultValue="0" Name="pStudentTestID" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

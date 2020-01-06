<%@ Page Title="" Language="C#" MasterPageFile="~/Student/Student.Master" AutoEventWireup="true" CodeBehind="Grades.aspx.cs" Inherits="FasTest.Student.Grades" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Grades</h1>
    <asp:Repeater ID="rptGrades" runat="server" OnItemDataBound="rptGrades_ItemDataBound" DataSourceID="getAllGrades" >
    <ItemTemplate>
        <div style="color:#25467a; font-size:30px; padding-top:10px">
            <asp:Label ID="lblClassTitle"  Text='<%# Eval("ClassTitle") %>' runat="server" CssClass="Class"/>
        </div>
        
        <asp:Label ID="LblTeacher"  Text='<%# Eval("Teacher") %>' runat="server" />
        <asp:Label ID="lblClassID" runat="server" Text='<%# Eval("ClassID") %>' Visible="false" CssClass="Class" />
        <asp:GridView ID="grdClassGrades" DataSourceID="getClassGrades" runat="server" EmptyDataText="No Graded Tests Available" AlternatingRowStyle-BackColor="#d6d6d6">
            <EmptyDataRowStyle BorderWidth="0px" BorderColor="Blue"/>
        </asp:GridView>
        <asp:SqlDataSource ID="getClassGrades" runat="server" ConnectionString="<%$ ConnectionStrings:CS414_FasTestConnectionString %>" SelectCommand="Display_Student_Class_Grades" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:Parameter DefaultValue="0" Name="pStudentID" Type="Int32" />
            <asp:ControlParameter DefaultValue='0' Name="pClassID" ControlID="lblClassID" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
        Overall Grade:
        <asp:Label ID="Grade"  Text='<%# Eval("LetterGrade") %>' runat="server" CssClass="gradesPageOverallGrade" />
    </ItemTemplate>
</asp:Repeater>
    
    <asp:SqlDataSource ID="getAllGrades" runat="server" ConnectionString="<%$ ConnectionStrings:CS414_FasTestConnectionString %>" SelectCommand="Get_Bob_Some_Bananas" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:Parameter DefaultValue='0' Name="pStudentID" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

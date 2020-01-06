<%@ Page Title="" Language="C#" MasterPageFile="~/Student/Student.Master" AutoEventWireup="true" CodeBehind="TestSubmitted.aspx.cs" Inherits="FasTest.Student.Testing.TestSubmitted" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
        <div class=""style="width:100%;  align-self:center; margin-top:100px; margin-bottom:20px;">
            <div class="col" style="text-align:center">
                <h2>Your test has been successfully submitted!</h2>
                <asp:Button ID="btnReturnHome" runat="server" Text="Return Home" OnClick="btnReturnHome_Click" Class="buttonClass" />
            </div>
        </div>
</asp:Content>

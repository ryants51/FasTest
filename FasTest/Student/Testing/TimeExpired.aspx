<%@ Page Title="" Language="C#" MasterPageFile="~/Student/Student.Master" AutoEventWireup="true" CodeBehind="TimeExpired.aspx.cs" Inherits="FasTest.Student.Testing.TimeExpired" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
        <div class=""style="width:100%;  align-self:center; margin-top:100px; margin-bottom:20px;">
            <div class="col" style="text-align:center">
                <h2>Time has expired from this test!</h2>
                <asp:Button ID="btnReturnHome" runat="server" Text="Return Home" OnClick="btnReturnHome_Click" Class="buttonClass" />
            </div>
        </div>
</asp:Content>

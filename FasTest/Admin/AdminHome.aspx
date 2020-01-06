<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AdminHome.aspx.cs" Inherits="FasTest.Admin.AdminHome" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Home</h1>
            
              <div class="container ">

        <div class="col-lg-4" style="background-color:#fbaf19; height:40vh">
                <h1 style="text-align:center; width:100%; color:white; text-decoration:underline">Total Users</h1>
                <h1 id="totUsers" runat="server"  style="text-align:center; width:100%; color:white; font-size: 400%"></h1>
            </div>
            <div class="col-lg-4" style="background-color:#15c0c6; height:40vh">
                 <h1 style="text-align:center; width:100%; color:white; text-decoration:underline">Total Classes</h1>
                <h1 id="totClasses" runat="server" style="text-align:center; width:100%; color:white; font-size: 400%"></h1>
            </div>

    
            <div class="col-lg-4" style="background-color:#e27126; height:40vh">
                 <h1 style="text-align:center; width:100%; color:white; text-decoration:underline">Total Tests</h1>
                <h1 id="totTests" runat="server" style="text-align:center; width:100%; color:white; font-size: 400%"></h1>
            </div>
        </div>
</asp:Content>

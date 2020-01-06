<%@ Page Title="Teacher Home" Language="C#" MasterPageFile="~/Teacher/Teacher.Master" AutoEventWireup="true" CodeBehind="TeacherHome.aspx.cs" Inherits="FasTest.Teacher.TeacherHome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Home</h1>

    <div class="container ">

        <div class="col-lg-4" style="background-color:#fbaf19; height:60vh">
                <h1 style="text-align:center; width:100%; color:white; text-decoration:underline">Assigned Classes</h1>
                <br />
                <h1 id="assignedClasses" runat="server" style="text-align:center; width:100%; color:white; font-size: 400%"></h1>
            </div>
            <div class="col-lg-4" style="background-color:#15c0c6; height:60vh">
                 <h1 style="text-align:center; width:100%; color:white; text-decoration:underline">Most Recent Test Assigned</h1>
                <h1 id="recentTest" runat="server" style="text-align:center; width:100%; color:white; font-size: 225%"></h1>
            </div>

    
            <div class="col-lg-4" style="background-color:#e27126; height:60vh">
                 <h1 style="text-align:center; width:100%; color:white; text-decoration:underline">Time Since Most Recent Test Taken</h1>
                <h3 id="timeSinceTestDays" runat="server" style="text-align:center; width:100%; color:white; font-size: 300%"></h3>
            </div>
        </div>
</asp:Content>

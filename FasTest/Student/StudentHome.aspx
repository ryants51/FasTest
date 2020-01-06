<%@ Page Title="Student Home" Language="C#" MasterPageFile="~/Student/Student.Master" AutoEventWireup="true" CodeBehind="StudentHome.aspx.cs" Inherits="FasTest.StudentHome" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <h1>Home</h1>




        
            <div class="col-lg-4" style="background-color:#fbaf19; height:40vh">
                <h1 style="text-align:center; width:100%; color:white; text-decoration:underline">Highest Score</h1>
                <h1 id="highestScore" runat="server" style="text-align:center; width:100%; color:white; font-size: 400%"></h1>
                
            </div>
            <div class="col-lg-4" style="background-color:#15c0c6; height:40vh">
                 <h1 style="text-align:center; width:100%; color:white; text-decoration:underline">Overall Average</h1>
                <h1 id="overallAverage" runat="server" style="text-align:center; width:100%; color:white; font-size: 400%"></h1>
            </div>

    
            <div class="col-lg-4" style="background-color:#e27126; height:40vh">
                 <h1 style="text-align:center; width:100%; color:white; text-decoration:underline">Available Tests</h1>
                <h1 id="availableTests" runat="server" style="text-align:center; width:100%; color:white; font-size: 400%"></h1>
            </div>
  

</asp:Content>

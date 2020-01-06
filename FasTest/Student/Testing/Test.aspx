<%@ Page Title="Test" Language="C#" MasterPageFile="~/Student/Student.Master" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="FasTest.Student.Testing.Test" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1 runat="server" id="testName" align="center"></h1>
    <h2 runat="server" id="questionNumber" align="center"></h2>
    <br />
    <p runat="server" id="testDescript"></p>
    <div class="container">
        <div class="row">
            <div class="col col-lg-7" style="text-align: left; border: 3px solid #2f96e8; background-color: #ededed; margin:20px; margin-top:0px;">
                <div style="text-align:center;">
                    <asp:Label ID="lblQuestion" runat="server" Text="" CssClass="testQuestion"></asp:Label>
                </div>
                <div id="divTF" runat="server" style="margin-left:15px">
                    <asp:RadioButtonList ID="rblTF" runat="server">
                        <asp:ListItem Value="1">True</asp:ListItem>
                        <asp:ListItem Value="0">False</asp:ListItem>
                    </asp:RadioButtonList>
                </div>

                <div id="divMult" runat="server" style="margin-left:15px">
                    <asp:RadioButtonList ID="rblMult" runat="server">
                    </asp:RadioButtonList>
                </div>
    
                <div id="divShortAns" runat="server" style="margin-left:15px; text-align:center;">
                    <br />
                    Answer:
                    <asp:TextBox ID="tbShortAnswer" runat="server" CssClass="TestTextBoxes"></asp:TextBox>
                    <br />
                </div>
                <div style="text-align:center; margin:5px; margin-top:20px">
                    <asp:Button  ID="btnPrevQuestion" runat="server" CssClass="TestButtonClass" Text="Prev Question" OnClick="btnPrevQuestion_Click1" CausesValidation="false" />
                    <asp:Button ID="btnNextQuestion" runat="server" CssClass="TestButtonClass" Text="Next Question" OnClick="btnNextQuestion_Click1" CausesValidation="false" />
                    <asp:Button ID="btnReviewTest" runat="server" CssClass="TestButtonClass" Text="Review Questions" OnClick="btnReviewTest_Click" Width="195px" />
                </div>
                
            </div>

        <div class="col col-lg-3" style="text-align: left; border: 3px solid #2f96e8; background-color: #ededed; margin:20px; margin-top:0px;">
            <div id="divTime" runat="server">
                <h2 id="Time" style="width:100%; text-align:center;" >Time</h2>
                <asp:Label ID="lblStart" runat="server" Text="Start: "></asp:Label>
                <br />
                <asp:Label ID="lblDuration" runat="server" Text="Duration: "></asp:Label>
                <br />
                <asp:Label ID="lblEnd" runat="server" Text="End: "></asp:Label>
            </div>
       </div>
       </div>
    </div>
    
    
    
</asp:Content>

<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Teacher/Teacher.Master" AutoEventWireup="true" CodeBehind="TestGrading.aspx.cs" Inherits="FasTest.Teacher.Classes.TestGrading" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1 style="text-align:center;">Grade Completed Tests</h1>
    <div class="row">
           <div style="text-align: center; border: 3px solid #2f96e8; background-color: #ededed; width:75%; margin: 0 auto;">
               <table style="margin: 0 auto; border-collapse: separate; border-spacing: 10px">
                   <thead>
                       <tr>
                           <td><asp:Label ID="class" runat="server" Text="Select a Class: "></asp:Label></td>
                           <td><asp:Label ID="Test" runat="server" Text="Select a Test: "></asp:Label></td>
                       </tr>
                   </thead>
                   <tr>
                       <td>
                           <asp:DropDownList ID="ddlSelectclass" runat="server" CssClass="dropDownList" DataTextField="ClassTitle" DataValueField="ClassID" AutoPostBack="true" OnSelectedIndexChanged="ddlSelectclass_SelectedIndexChanged">
                            <Items>
                                <asp:ListItem Text=" -- Select desired class -- " Value="" />
                            </Items>
                            </asp:DropDownList>
                       </td>
                       <td>
                           <asp:DropDownList ID="ddlSelectAssignment" runat="server" CssClass="dropDownList" DataValueField="AssignmentID" DataTextField="AssignmentName" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlSelectAssignment_SelectedIndexChanged">
                                <Items>
                                    <asp:ListItem Text=" -- Select desired test -- " Value="" />
                                </Items>
                            </asp:DropDownList>
                       </td>
                   </tr>
               </table>
               
            </div>
        <br />
        <br />
        <div id="allTests" runat="server" visible="false" class="col-lg-6">
            <label id="AllVisible">"Mark All as Visible"</label>
                                <asp:CheckBox ID="chkAllVisible" runat="server" AutoPostBack="true" OnCheckedChanged="chkAllVisible_CheckedChanged" Enabled="false" />
            <br />
            <br />
                            <asp:Panel ID="panelContainer" runat="server" Height="400px" Width="100%"  ScrollBars="Auto">
                            <asp:GridView ID="AllStudentsGridview" DataKeyNames="StudentID, Grade, studentName" HeaderStyle-CssClass="fixedHeader" OnRowDataBound="AllStudentsGridview_RowDataBound" OnRowCommand="AllStudentsGridview_RowCommand" CssClass="Grid" AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="false" runat="server">
                                <Columns>
                                    <asp:TemplateField HeaderText="Visible?">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="visibleCheckbox" AutoPostBack="true" runat="server" Checked='<%# Eval("isVisible") %>' OnCheckedChanged="visibleCheckbox_CheckedChanged" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="StudentID" HeaderText="Question" Visible="false" />
                                    <asp:BoundField DataField="StudentName" HeaderText="Name" />
                                    <asp:BoundField DataField="Grade" HeaderText="Grade" />
                                    
                                    <asp:TemplateField HeaderText="Grade Test">
                                        <ItemTemplate>
                                            <asp:Button  CommandArgument="<%# Container.DataItemIndex %>" ID="gradeTest" runat="server" ControlStyle-CssClass="gridSelect" HeaderText="Select" Text="Grade Test" commandName="gradeTest" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                                </asp:Panel>
            </div>
                            <div id="StudentsTest" runat="server" class="col-lg-6" visible="false">
                                <h2 id="testInfo" runat="server"></h2>
                                <table>
                                    <tr>
                                        <td><label id="markTest">"Mark Test as Graded"</label></td>
                                        <td><asp:CheckBox ID="Gradedchk" runat="server" AutoPostBack="true" OnCheckedChanged="Gradedchk_CheckedChanged" /></td>
                                    </tr>
                                    <tr>
                                        <td><label id="pledgeSigned">"Pledge Signed?"</label></td>
                                        <td><asp:CheckBox ID="chkPledgeSigned" runat="server" AutoPostBack="true" OnCheckedChanged="chkPledgeSigned_CheckedChanged"/></td>
                                    </tr>
                                </table>
                                
                                
                                
                                
                                <asp:GridView ID="grdStudentsView" CssClass="Grid" EmptyDataText="No Test has been taken" runat="server" DataKeyNames="ChoiceID" AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:BoundField DataField="ChoiceID" ItemStyle-Width="30px" HeaderText="Question" Visible="false" />
                                        <asp:BoundField DataField="Question" HeaderText="Question" />
                                        <asp:BoundField DataField="Correct Answer" ItemStyle-Width="30px" HeaderText="Correct Answer" />
                                        <asp:BoundField DataField="Student Answer" ItemStyle-Width="30px" HeaderText="Student Answer" />
                                        <asp:BoundField DataField="Points" ItemStyle-Width="30px" HeaderText="Points" />
                                        <asp:TemplateField HeaderText="Correct?">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="Checkbox2" AutoPostBack="true" runat="server" Checked='<%# Eval("Correct") %>' OnCheckedChanged="CorrectCheckbox_CheckedChanged" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
        </div>
</asp:Content>

<%@ Page Title="Test Creation Page" Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Teacher/Teacher.Master" AutoEventWireup="true" CodeBehind="TestCreation.aspx.cs" Inherits="FasTest.Teacher.TestCreation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolKit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1 style="text-align: center;">Create & Edit Tests</h1>
    <p>&nbsp;</p>

    <!-- The first panel where you choose your class -->
    <div class="panel-group" id="accordion" style="text-align: center">

        <div class="panel panel-default">
            <div class="panel-heading">
                <h4 class="panel-title">
                    <asp:Button ID="btnChooseClass" Text="Change Class" CssClass="testCreation" OnClick="btnChooseClass_Click" runat="server" Visible="false" />
                    <asp:LinkButton data-toggle="collapse" ID="tabSelectClass" OnClick="btnChooseClass_Click" runat="server" data-parent="#accordion" href="#collapse1">Select the class</asp:LinkButton>
                </h4>
            </div>
            <div id="collapse1" class="panel-collapse collapse in" runat="server">
                <div class="panel-body">
                    <asp:Label ID="class" runat="server" Text="Select the class: "></asp:Label>
                    <asp:DropDownList ID="ddlSelectedClass" runat="server" CssClass="dropDownList" DataSourceID="ReceiveClasses" AppendDataBoundItems="True" DataValueField="ClassID" DataTextField="ClassTitle" OnSelectedIndexChanged="ddlSelectedClass_SelectedIndexChanged1" AutoPostBack="True">
                        <Items>
                            <asp:ListItem Text=" -- Select desired class -- " Value="" />
                        </Items>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
    </div>
    <br />
    <br />

    <!-- The second panel where you choose to make a new test or edit a test -->

    <div class="panel panel-default" style="text-align: center">
        <div class="panel-heading">
            <h4 class="panel-title">
                <asp:LinkButton data-toggle="collapse" ID="tabCreateEdit" runat="server" data-parent="#accordion" href="#collapse2">Create a new test or choose to modify an existing one</asp:LinkButton>
                <asp:Button ID="btnChooseTest" Text="Change Test" CssClass="testCreation" OnClick="btnChooseTest_Click" runat="server" Visible="false" />
            </h4>
        </div>
        <div id="collapse2" class="panel-collapse collapse" runat="server">
            <div class="panel-body">
                <div class="row">
                    <asp:Label ID="testName" runat="server" Text="Enter Test Name"></asp:Label>
                    <br />
                    <asp:TextBox ID="inputTestName" MaxLength="50" CssClass="textBoxes" runat="server" Style="margin-bottom: 10px" Height="30px" Width="434px"></asp:TextBox>
                    <br />
                    <asp:Button ID="btnCreateTest" runat="server" CssClass="buttonClass" Text="Create Test" OnClick="btnCreateTest_Click" />
                    <br />
                    <asp:Label ID="errorTestName" ForeColor="Red" runat="server" Text=""></asp:Label>
                    <h2>OR</h2>
                    <asp:Label ID="editTest" runat="server" Text="Select Existing Test"></asp:Label>

                    <br />

                    <asp:DropDownList ID="ddlInstructorTests" runat="server" AutoPostBack="true" CssClass="dropDownList" DataTextField="AssignmentName" OnSelectedIndexChanged="ddlInstructorTests_SelectedIndexChanged" DataValueField="AssignmentID" AppendDataBoundItems="true">
                        <asp:ListItem Value="0">-- Please select a test to edit --</asp:ListItem>
                    </asp:DropDownList>
                    <br />
                    <asp:Button ID="btnEditTest" runat="server" Text="Edit Test" CssClass="buttonClass" OnClick="btnEditTest_Click" />
                    <br />

                    <asp:Label ID="error" runat="server" Visible="false" Text="No test was selected" ForeColor="Red"></asp:Label>

                    <div id="editTestInfo" visible="false" runat="server" style="align-content: center">
                        <h2>Edit Test Details</h2>
                        <table class="Grid" style="margin: 0 auto">
                            <thead>
                                <tr>
                                    <th>
                                        <asp:Label ID="Label1" runat="server" Text="Test Name"></asp:Label></th>
                                    <th>
                                        <asp:Label ID="lblStartDate" runat="server" Text="Start Date"></asp:Label>
                                    </th>
                                    <th>
                                        <asp:Label ID="lblEndDate" runat="server" Text="End Date"></asp:Label>
                                    </th>
                                    <th>
                                        <asp:Label ID="Label2" runat="server" Text="Test Duration (Minutes)"></asp:Label></th>

                                </tr>
                            </thead>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtTestName" CssClass="textBoxes" MaxLength="50" runat="server"></asp:TextBox></td>
                                <td style="margin-right: 0 auto;">
                                    <asp:TextBox ID="testStartDate" runat="server" CssClass="textBoxes" TextMode="Date"></asp:TextBox>
                                    <asp:TextBox ID="testStartTime" MaxLength="10" runat="server" CssClass="textBoxes" TextMode="Time" />
                                    <br />
                                    <asp:CompareValidator ID="CompareValidator2" ForeColor="Red" ValidationGroup="errorControl" Operator="DataTypeCheck" Type="Date" ControlToValidate="testStartDate" runat="server" ErrorMessage="Invalid Date"></asp:CompareValidator>
                                 
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTestEndDate" runat="server" CssClass="textBoxes" TextMode="Date"></asp:TextBox>
                                    <asp:TextBox ID="txtTestEndTime" MaxLength="10" runat="server" CssClass="textBoxes" TextMode="Time"></asp:TextBox>
                                    <br />
                                    <asp:CompareValidator ID="CompareValidator1" ForeColor="Red" ValidationGroup="errorControl" Operator="DataTypeCheck" Type="Date" ControlToValidate="txtTestEndDate" runat="server" ErrorMessage="Invalid Date"></asp:CompareValidator>
                                </td>
                                <td>
                                 <asp:TextBox ID="txtTestDuration" CssClass="textBoxes" runat="server" TextMode="Number"></asp:TextBox>
                                    <br />
                                    <asp:Label ID="txtTestDurationerror" runat="server" ForeColor="Red" Text=""></asp:Label>
                                </td>
                                
                                </tr>
                        </table>
                        <br />
                        <asp:Label ID="publishingCatchAllError" runat="server" ForeColor="Red" Text=""></asp:Label>
                        <br />
                        <asp:Button ID="btnSaveTestInfo" runat="server" CssClass="buttonClass" ValidationGroup="errorControl" Text="Save" OnClick="btnSaveTestInfo_Click" />
                        <asp:Button ID="btnDeleteTest" runat="server" CssClass="buttonClass" Text="Delete Test" OnClick="btnDeleteTest_Click" />
                        <br />
                        <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
                        <script type="text/javascript">
                            $(function () {
                                //Hide label after 5 secs
                                setTimeout(function () {
                                    $("[id$=lblInfoSaved]").fadeOut("slow");
                                }, 1000);
                            });
                        </script>
                        <asp:Label ID="lblInfoSaved" ForeColor="Green" runat="server" Text="Saved!" Visible="false" />
                       <asp:Label ID="lblNoTestQuestions" ForeColor="Red" runat="server" Text="Cannot publish an empty test" Visible="false" />
                        <script type="text/javascript">
                            $(function () {
                                //Hide label after 5 secs
                                setTimeout(function () {
                                    $("[id$=lblNoTestQuestions]").fadeOut("slow");
                                }, 1000);
                            });
                        </script>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <br />
    <br />

    <!-- The third panel where you add questions to a test -->
    <div class="panel panel-default" style="text-align: center">
        <div class="panel-heading">
            <h4 class="panel-title">
                <a data-toggle="collapse" data-parent="#accordion" href="#collapse3">Enter Test Questions</a>
            </h4>
        </div>
        <div id="collapse3" class="panel-collapse collapse" runat="server">
            <div class="panel-body">
                <div class="row">
                    <div class="col-lg-4" style="text-align: left;">
                        <h3>Add New Question</h3>
                        <br />
                        <asp:Label ID="TestQuestion" runat="server" Text="Select Question Type:"></asp:Label>
                        <asp:DropDownList ID="sctQuestionChoice" runat="server" CssClass="dropDownList" OnSelectedIndexChanged="sctQuestionChoice_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Value="1" Text="Multiple Choice"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Short Answer"></asp:ListItem>
                            <asp:ListItem Value="3" Text="True/False"></asp:ListItem>
                        </asp:DropDownList>
                        <br />
                        <asp:Label ID="pntValuelbl" runat="server" Text="Select Point Value:"></asp:Label>
                        <asp:DropDownList ID="pointValueDropdwon" CssClass="dropDownList" runat="server">
                            <asp:ListItem Value="1" Text="1"></asp:ListItem>
                            <asp:ListItem Value="2" Text="2"></asp:ListItem>
                            <asp:ListItem Value="3" Text="3"></asp:ListItem>
                            <asp:ListItem Value="4" Text="4"></asp:ListItem>
                            <asp:ListItem Value="5" Text="5"></asp:ListItem>
                            <asp:ListItem Value="6" Text="6"></asp:ListItem>
                            <asp:ListItem Value="7" Text="7"></asp:ListItem>
                            <asp:ListItem Value="8" Text="8"></asp:ListItem>
                            <asp:ListItem Value="9" Text="9"></asp:ListItem>
                            <asp:ListItem Value="10" Text="10"></asp:ListItem>
                        </asp:DropDownList>

                        <br />

                        <!--Div for hiding create test Choice -->
                        <div id="divCreateChoice" runat="server">

                            <br />

                            <div id="allQuestions" style="text-align: center;" runat="server">
                                Enter Question &nbsp;
                                    <asp:TextBox ID="tbQuestion" MaxLength="100" runat="server" CssClass="textBoxes" Height="30px"></asp:TextBox>
                                <br />
                                <asp:Label ID="errorQuestion" ForeColor="red" runat="server" Text=""></asp:Label>

                                <br />
                                 <asp:Label ID="errorQuestion2" ForeColor="red" runat="server" Text=""></asp:Label>
                                <br />

                                <div id="tfQuestion" runat="server" hidden="hidden">
                                    <asp:Label ID="lblNoTFAnswer" runat="server" Visible="false" ForeColor="Red">Please Select either true or false</asp:Label>
                                    <br />
                                    Choose Answer:&nbsp;&nbsp;
                                        <div id="TrueAndFalse" runat="server">
                                            <asp:RadioButtonList ID="rbTrueAndFalse" runat="server" Width="72px">
                                                <asp:ListItem Value="1" Selected="True" Enabled="true">True</asp:ListItem>
                                                <asp:ListItem Value="2">False</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                </div>

                                <div id="mcQuestion" runat="server">
                                    <asp:Label ID="lbNoSelectionMultChoice" runat="server" ForeColor="Red"></asp:Label>
                                    <div id="Multiple Choice">
                                        <asp:Table ID="tableMultipleChoice" runat="server" Width="191px">
                                            <asp:TableHeaderRow>
                                                <asp:TableHeaderCell>Correct answer</asp:TableHeaderCell>
                                                <asp:TableHeaderCell>Possible answers</asp:TableHeaderCell>
                                            </asp:TableHeaderRow>
                                            <asp:TableRow>
                                                <asp:TableCell HorizontalAlign="Center">
                                                    <asp:CheckBox ID="CheckBox1" runat="server" Checked="true" OnCheckedChanged="CheckBox1_CheckedChanged" AutoPostBack="true" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:TextBox ID="tbMult1" MaxLength="100" CssClass="textBoxes" runat="server"></asp:TextBox>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow>
                                                <asp:TableCell HorizontalAlign="Center">
                                                    <asp:CheckBox ID="CheckBox2" runat="server" OnCheckedChanged="CheckBox2_CheckedChanged" AutoPostBack="true" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:TextBox ID="tbMult2" MaxLength="100" CssClass="textBoxes" runat="server"></asp:TextBox>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow>
                                                <asp:TableCell HorizontalAlign="Center">
                                                    <asp:CheckBox ID="CheckBox3" runat="server" OnCheckedChanged="CheckBox3_CheckedChanged" AutoPostBack="true" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:TextBox ID="tbMult3" MaxLength="100" CssClass="textBoxes" runat="server"></asp:TextBox>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow>
                                                <asp:TableCell HorizontalAlign="Center">
                                                    <asp:CheckBox ID="CheckBox4" runat="server" OnCheckedChanged="CheckBox4_CheckedChanged" AutoPostBack="true" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:TextBox ID="tbMult4" MaxLength="100" TextMode="SingleLine" CssClass="textBoxes" runat="server"></asp:TextBox>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </div>
                                </div>

                                <div id="saQuestion" runat="server" hidden="hidden">
                                    <asp:Label ID="shortAnswerLabel" runat="server" Text="Answer: "></asp:Label>
                                    <asp:TextBox ID="tbShortAnswer" runat="server" CssClass="textBoxes"></asp:TextBox>
                                    <br />
                                </div>


                                <asp:SqlDataSource ID="ReceiveClasses" runat="server" ConnectionString="<%$ ConnectionStrings:CS414_FasTestConnectionString %>" SelectCommand="SELECT [ClassID], [ClassTitle] FROM [Class] WHERE ([InstructorID] = @InstructorID)">
                                    <SelectParameters>
                                        <asp:CookieParameter CookieName="TeacherId" DefaultValue="0" Name="InstructorID" Type="Int32" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                                <br />
                                <asp:Button ID="btnQuestionCreate" runat="server" CssClass="buttonClass" Text="Create Question" OnClick="btnQuestionCreate_Click" />
                                <asp:Button ID="btnQuestionEdit" runat="server" CssClass="buttonClass" Text="Save Question" OnClick="btnQuestionEdit_Click" autopostback="true" Visible="false" />
                            </div>
                        </div>
                    </div>






                    <!-- Selecting questions from previous tests -->
                    <div class="col-lg-8" id="getOtherQuestions" style="overflow-y: scroll; height: 500px; overflow: auto;" hidden="hidden" runat="server">
                        <asp:Button ID="viewQuestionButton" CssClass="buttonClass" Text="Current Test Questions" OnClick="viewQuestionButton_Click" runat="server" />
                        <asp:Button ID="getOtherQuestionButton" CssClass="buttonClass" Text="Previous Test Questions" OnClick="getOtherQuestionButton_Click" runat="server" />

                        <h3>Questions from Previous Tests</h3>
                        <asp:DropDownList ID="ddlAllTeacherClasses" runat="server" CssClass="dropDownList" DataValueField="ClassID" DataTextField="ClassTitle" AppendDataBoundItems="true" OnSelectedIndexChanged="AllTeacherClass_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        <asp:DropDownList ID="ddlAllTeacherTests" runat="server" CssClass="dropDownList" DataValueField="AssignmentID" DataTextField="AssignmentName" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlAllTeacherTests_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Value="">-- Select a test --</asp:ListItem>
                        </asp:DropDownList>
                        <br />
                        <br />
                        <br />
                        <asp:Panel ID="panelContainer" runat="server" Height="400px" Width="100%" ScrollBars="Auto">
                            <asp:GridView ID="ExistingQuestionsGridview" HeaderStyle-CssClass="fixedHeaderTest" CssClass="specialGrid" AlternatingRowStyle-CssClass="alt" ShowHeaderWhenEmpty="true" Width="100%" EmptyDataText="There are no questions in this test" GridLines="Horizontal" DataKeyNames="QuestionID" runat="server" AutoGenerateColumns="false" OnRowCommand="ExistingQuestionsGridview_RowCommand">
                                <Columns>
                                    <asp:ButtonField runat="server" HeaderStyle-Width="" ButtonType="Image" ImageUrl="~/Images/add.png" CommandName="addQuestion" />
                                    <asp:BoundField DataField="QuestionID" HeaderText="QuestionID" Visible="false" />
                                    <asp:BoundField HeaderText="Question" ItemStyle-CssClass="questionGrid" HeaderStyle-CssClass="questionGridHeader" DataField="QuestionDescription" />
                                    <asp:BoundField DataField="QuestionType" ItemStyle-CssClass="questionTypeGrid" HeaderStyle-CssClass="questionTypeGridHeader" HeaderText="Question Type" />
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </div>

                    <!-- Div for viewing questions in the test -->
                    <div class="col-lg-8" id="viewQuestions" style="overflow-y: scroll; height: 500px; overflow: auto; border-left: thin solid #808080;" runat="server">
                        <h3>Questions on Test</h3>
                        <asp:Button ID="Button3" CssClass="buttonClass" Text="Current Test Questions" OnClick="viewQuestionButton_Click" runat="server" />
                        <asp:Button ID="Button4" CssClass="buttonClass" Text="Existing Test Questions" OnClick="getOtherQuestionButton_Click" runat="server" />
                        <div id="divEditChoice" runat="server">
                            <br />
                            <br />
                            <asp:Panel ID="panel2" runat="server" Height="400px" Width="100%" ScrollBars="Auto">

                                <asp:GridView ID="QuestionGridview" HeaderStyle-CssClass="fixedHeaderTest" GridLines="Horizontal" CssClass="specialGrid" AlternatingRowStyle-CssClass="alt" ShowHeaderWhenEmpty="true" EmptyDataText="There are no questions in this test" DataKeyNames="QuestionID" runat="server" AutoGenerateColumns="false" OnRowCommand="QuestionGridview_RowCommand">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Delete/Modify">
                                            <ItemTemplate>
                                                <asp:ImageButton CommandArgument="<%# Container.DataItemIndex %>" runat="server" ImageUrl="~/Images/delete-photo.png" CommandName="deleteQuestion" />
                                                <asp:ImageButton runat="server" CommandArgument="<%# Container.DataItemIndex %>" CommandName="editQuestion" ImageUrl="~/Images/pencil-edit-button.png" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="QuestionID" HeaderText="QuestionID" Visible="false" />
                                        <asp:BoundField DataField="QuestionDescription" ItemStyle-CssClass="questionGrid" HeaderStyle-CssClass="questionGridHeader" HeaderText="Question" />
                                        <asp:BoundField DataField="QuestionType" ItemStyle-CssClass="questionTypeGrid" HeaderStyle-CssClass="questionTypeGridHeader" HeaderText="Question Type" />
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                        </div>

                    </div>
                    <asp:Button ID="btnSaveQuestions" runat="server" CssClass="buttonClass" Text="Publish Test" OnClick="btnSaveQuestions_Click" />
                </div>
            </div>
        </div>
    </div>
    <br />
    <br />
    <asp:Button runat="server" ID="hiddenbutton" Style="display: none;" />
    <!-- ModalPopupExtender -->
    <ajaxToolkit:ModalPopupExtender ID="mp1" runat="server" PopupControlID="Panel1" TargetControlID="hiddenbutton"
        CancelControlID="btnCancel" BackgroundCssClass="modalBackground">
    </ajaxToolkit:ModalPopupExtender>
    <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" align="center" Style="display: none">
        Are you sure you want to delete <span id="testNameDelete" runat="server"></span>?<br />
        <br />
        <asp:Button ID="btnConfirmDeleteTest" runat="server" Text="Confirm" OnClick="btnConfirmDeleteTest_Click" CssClass="buttonClass" />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonClass" />
    </asp:Panel>
    <div class="col-lg-8">
    </div>
</asp:Content>

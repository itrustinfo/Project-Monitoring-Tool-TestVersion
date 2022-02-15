<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.import_data._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
     <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
  <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
  <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>

    <script type="text/javascript">
 $( function() {
    $("input[id$='dtIncomingRevDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });

      $("input[id$='dtDocumentDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
    <%--project selection dropdowns--%>
     <asp:ScriptManager ID="ScriptManager1" runat="server">
   </asp:ScriptManager>
    <div class="container-fluid">
            <div class="row">
                <div class="col-md-12 col-lg-4 form-group">Data Import</div>
                <div class="col-md-6 col-lg-4 form-group">
                    <label class="sr-only" for="DDLProject">Project</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Project</span>
                        </div>
                        <asp:DropDownList ID="DDlProject" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDlProject_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-6 col-lg-4 form-group">
                    <label class="sr-only" for="DDLWorkPackage">Work Package</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Work Package</span>
                        </div>
                        <asp:DropDownList ID="DDLWorkPackage" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
    <table style="display:none;">
                    <tr><td style="font-weight:bold;">Owner </td><td>:</td>
                       <td>
                           <%--<asp:TextBox ID="txtOwner" runat="server" Width="150px"></asp:TextBox>--%>
                               <asp:DropDownList ID="DDLUsers" runat="server" CssClass="form-control"></asp:DropDownList>
                           </td></tr>
                     <tr><td>
                       Document Flow
                       </td><td>:</td>
                       <td>
                           <asp:DropDownList ID="DDLDocumentFlow" runat="server" CssClass="form-control" Enabled="false" AutoPostBack="true" OnSelectedIndexChanged="DDLDocumentFlow_SelectedIndexChanged"></asp:DropDownList>
                       </td>
                   </tr>
          
                   <tr id="S1Display" runat="server"><td> <asp:Label ID="lblStep1Display" runat="server"></asp:Label></td><td>:</td><td>
                             <asp:DropDownList ID="ddlSubmissionUSer" runat="server" CssClass="drpdown">
                             </asp:DropDownList>
                             </td></tr>
                       <tr id="S1Date" runat="server"><td><asp:Label ID="lblStep1Date" runat="server"></asp:Label></td><td>:</td><td>
                            <asp:TextBox ID="dtSubTargetDate" runat="server" CssClass="form-control"></asp:TextBox>
                             </td></tr>
        
                   <tr id="S2Display" runat="server"><td><asp:Label ID="lblStep2Display" runat="server"></asp:Label> </td><td>:</td><td>
                             <asp:DropDownList ID="ddlQualityEngg" runat="server" CssClass="drpdown">
                             </asp:DropDownList>
                             </td></tr>
                   <tr id="S2Date" runat="server"><td><asp:Label ID="lblStep2Date" runat="server"></asp:Label><%--Target Date--%></td><td>:</td><td>
                       <asp:TextBox ID="dtQualTargetDate" runat="server" CssClass="form-control"></asp:TextBox>
                            
                             </td></tr>
                   <tr id="S3Display" runat="server"><td><asp:Label ID="lblStep3Display" runat="server"></asp:Label><%--Reviewer B--%> </td><td>:</td><td>
                             <asp:DropDownList ID="ddlReviewer_B" runat="server" CssClass="drpdown">
                             </asp:DropDownList>
                             </td></tr>
                   <tr id="S3Date" runat="server"><td><asp:Label ID="lblStep3Date" runat="server"></asp:Label><%--Target Date--%></td><td>:</td><td>
                       <asp:TextBox ID="dtRev_B_TargetDate" runat="server" CssClass="form-control"></asp:TextBox>
                            
                             </td></tr>
                </table>

    <div class="container-fluid" style="max-height:77vh; overflow-y:auto;">
            <div class="row">
               <div class="col-lg-6 col-xl-6 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                         <div class="form-group" id="Originator" runat="server">
                        <label class="lblCss" for="DDLOriginator">Import Data For</label>
                          <asp:RadioButtonList ID="RBList" runat="server" Width="100%" CssClass="lblCss" CellPadding="5" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="RBList_SelectedIndexChanged">
                                 <asp:ListItem Value="Document">&nbsp;Document</asp:ListItem>
                                <asp:ListItem Value="Activity">&nbsp;Activity</asp:ListItem>
                              <asp:ListItem Value="Activity New">&nbsp;Activity New</asp:ListItem>
                              <asp:ListItem Value="BOQ Details">&nbsp;BOQ Details</asp:ListItem>
                              <asp:ListItem Value="Construction Program">&nbsp;Construction Program</asp:ListItem>
                                 <asp:ListItem Value="Activity Puttenahalli">Activity Puttenahalli</asp:ListItem>
                             </asp:RadioButtonList>
                    </div>
                    
                   <div class="form-group" id="TaskOption" runat="server">
                       <label class="lblCss" for="FilCoverLetter">Option</label>
                       <asp:DropDownList ID="DDLOptions" runat="server" CssClass="form-control"></asp:DropDownList>
                       </div>

                    <div class="form-group">
                        <label class="lblCss" for="FilCoverLetter"><asp:Label ID="LblDocType" runat="server"></asp:Label></label>
                        <div class="custom-file">
                            <asp:FileUpload ID="FileUpload1" runat="server" CssClass="custom-file-input" />

                            <%--<asp:FileUpload ID="FilCoverLetter" CssClass="custom-file-input" runat="server" ClientIDMode="Static" /> --%>
                            <label class="custom-file-label" for="FilCoverLetter">Choose file</label>
                        </div>
                    </div>
                    <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary" Text="Submit" CausesValidation="false" OnClick="btnSubmit_Click" />

                        </div>
                    </div>
                   </div>
               
            </div> 
        </div>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-review.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_review" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
     <script type="text/javascript">
 $( function() {
    $("input[id$='dtReviewDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddIssuesModal" runat="server">
        <div class="container-fluid" style="max-height:80vh; overflow-y:auto;">
            <div class="row">
                <div class="col-sm-12">
                    <div class="form-group">
                        <label class="lblCss" for="DDlProject">Project</label>
                         <asp:DropDownList ID="DDlProject" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDlProject_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtIssue_Description">WorkPackage</label>
                         <asp:DropDownList ID="DDLWorkPackage" CssClass="form-control" runat="server">
                       </asp:DropDownList>
                    </div>

                    <div class="form-group">
                        <label class="lblCss" for="DDLUsers">User</label> &nbsp;<span style="color:red; font-size:1rem;">*</span>
                         <asp:DropDownList ID="DDLUsers" CssClass="form-control"  runat="server"></asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="rdList">Review Type</label>
                         <asp:RadioButtonList ID="rdList" runat="server" Width="100%" CssClass="lblCss" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rdList_SelectedIndexChanged">
                       <asp:ListItem Value="One Time" Selected="True">&nbsp;&nbsp;One Time</asp:ListItem>
                       <asp:ListItem Value="Periodic">&nbsp;&nbsp;Periodic</asp:ListItem>
                   </asp:RadioButtonList>
                    </div>
                     <div class="form-group" id="ReviewOneType" runat="server">
                        <label class="lblCss" for="dtReviewDate">Review Date</label> &nbsp;<span style="color:red; font-size:1rem;">*</span>
                        <asp:TextBox ID="dtReviewDate" CssClass="form-control" required runat="server" placeholder="dd/mm/yyyy" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                    </div>

                    <div class="form-group" id="freq" runat="server" visible="false">
                        <label class="lblCss" for="DDLFreq">Review Frequency</label>
                         <asp:DropDownList ID="DDLFreq" CssClass="form-control" runat="server">
                           <asp:ListItem>Monthly</asp:ListItem>
                           <asp:ListItem>Weekly</asp:ListItem>
                           <asp:ListItem>Quarterly</asp:ListItem>
                       </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="lstUsers">Review Attendies</label> &nbsp;<span style="color:red; font-size:1rem;">*</span>
                         <asp:ListBox ID="lstUsers" runat="server" CssClass="form-control" SelectionMode="Multiple">

                   </asp:ListBox>  
                        
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtdesc">Review Description</label>
                        <asp:TextBox ID="txtdesc" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
            </div> 
        </div>
        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                </div>
    </form>
</asp:Content>

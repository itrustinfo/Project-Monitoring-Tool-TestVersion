<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-review-record.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_review_record" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddReviewRecordModal" runat="server">
        <div class="container-fluid" style="max-height:83vh; overflow-y:auto; min-height:84vh;">
            <asp:HiddenField ID="Hidden1" runat="server" />
            <div class="row">
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="lblCss" for="lstUsers">Review Attendies</label>
                         <asp:ListBox ID="lstUsers" runat="server" CssClass="form-control" SelectionMode="Multiple">

                   </asp:ListBox>  
                        
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtrecorddesc">Record Description</label>
                        <asp:TextBox ID="txtrecorddesc" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label class="lblCss" for="txtxsummary">Record Summary</label>
                        <asp:TextBox ID="txtxsummary" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                    </div>
                    
                </div>
                <div class="col-sm-6">
                    <h6 class="text-muted">
                                   <asp:Label ID="ActivityHeading" CssClass="text-uppercase font-weight-bold" runat="server" Text="List of Review Records" />
                                       <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                               </h6>
                    <asp:GridView ID="GrdReviewRecords" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered" PageSize="10"  OnPageIndexChanging="GrdReviewRecords_PageIndexChanging" OnRowCommand="GrdReviewRecords_RowCommand" OnRowEditing="GrdReviewRecords_RowEditing" Width="100%">
                <Columns>
                    <asp:BoundField DataField="ReviewRecord_Desc" HeaderText="Review Record Description">
                    <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ReviewRecord_Summary" HeaderText="Review Record Summary" >
                    <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                   <%-- <asp:TemplateField>
                               <ItemTemplate>
                                   <a id="AddRecordActions" href='AddReviewRecordActions.aspx?Review_RecordUID=<%#Eval("Review_RecordUID")%>' class="AddRecordActions">Add Actions</a>
                               </ItemTemplate>
                           </asp:TemplateField>--%>
                    <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkedit" runat="server" CommandName="edit" CommandArgument='<%#Eval("Review_RecordUID")%>'><span title="Edit" class="fas fa-edit"></span></asp:LinkButton>
                                </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <strong>No Records Found ! </strong>
                </EmptyDataTemplate>
            </asp:GridView>
                    </div>
            </div> 
        </div>
        <div class="modal-footer text-left">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                </div>
    </form>
</asp:Content>

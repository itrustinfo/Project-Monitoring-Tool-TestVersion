<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="user-documenthistory.aspx.cs" Inherits="ProjectManagementTool._modal_pages.user_documenthistory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddIssuesModal" runat="server">
        <div class="container-fluid" style="max-height:80vh; overflow-y:auto;">
            <div class="row">
                <div class="col-sm-12">
                    <div class="table-responsive">
                         <asp:GridView ID="GrdUserDocumentHsitroy" EmptyDataText="No Documents Found." runat="server" Width="100%" AutoGenerateColumns="false" CellPadding="6" CellSpacing="16" 
                                            AllowPaging="true" PageSize="15" DataKeyNames="DocumentUID" CssClass="table table-bordered" OnPageIndexChanging="GrdUserDocumentHsitroy_PageIndexChanging" OnRowDataBound="GrdUserDocumentHsitroy_RowDataBound">
                                       <Columns>
                                           <asp:TemplateField HeaderText="Document">
                                            <ItemTemplate>
                                                <%#GetUserName(Eval("UserUID").ToString())%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           <asp:BoundField DataField="UserUID" HeaderText="Downloaded">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                            <asp:BoundField DataField="UserUID" HeaderText="Viewed">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                       </Columns>
                                       </asp:GridView>
                            </div>
                    </div>
                </div>
            </div>
        </form>
</asp:Content>

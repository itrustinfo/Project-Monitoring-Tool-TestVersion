<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="view-documentsentdetails.aspx.cs" Inherits="ProjectManagementTool._modal_pages.view_documentsentdetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
     <form id="frmDocuemntSentDetails" runat="server">
        <div class="container-fluid" style="max-height:80vh; overflow-y:auto;">
            <div class="row">
                <div class="col-sm-12">
                    <div class="table-responsive">
                         <asp:GridView ID="GrdDocumentSentDetails" EmptyDataText="No Data Found." runat="server" Width="100%" AutoGenerateColumns="false" CellPadding="6" CellSpacing="16" 
                                            AllowPaging="true" PageSize="10" DataKeyNames="Document_UID" CssClass="table table-bordered" OnPageIndexChanging="GrdDocumentSentDetails_PageIndexChanging">
                                       <Columns>
                                           <asp:TemplateField HeaderText="Document Name">
                                            <ItemTemplate>
                                                <%#GetDocumentName(Eval("Document_UID").ToString())%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Sent By">
                                            <ItemTemplate>
                                                <%#GetUserName(Eval("Sent_By").ToString())%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Sent To">
                                            <ItemTemplate>
                                                <%#Eval("Sent_To")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           <asp:BoundField DataField="Sent_Date" HeaderText="Sent Date"  DataFormatString="{0:dd MMM yyyy}" />
                                       </Columns>
                                       </asp:GridView>
                            </div>
                    </div>
                </div>
            </div>
        </form>
</asp:Content>

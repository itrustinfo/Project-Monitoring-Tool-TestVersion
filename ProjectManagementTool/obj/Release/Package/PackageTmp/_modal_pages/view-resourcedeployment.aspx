<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="view-resourcedeployment.aspx.cs" Inherits="ProjectManagementTool._modal_pages.view_resourcedeployment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <script type="text/javascript">
        function DeleteItem() {
            if (confirm("Are you sure you want to delete ...?")) {
                return true;
            }
            return false;
    }
    </script>
     <style type="text/css">
        .hiddencol { display: none; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmResourceDeploymentHistory" runat="server">
        <div class="container-fluid" style="overflow-y:auto; min-height:85vh;">
             <div class="row">
            <div class="col-sm-12">
                <div class="table-responsive">
                    <asp:GridView ID="GrdResourceDeployment" runat="server" DataKeyNames="UID" CssClass="table table-bordered" Width="100%" AutoGenerateColumns="false" EmptyDataText="No Data Found.." AllowPaging="false" OnRowCommand="GrdResourceDeployment_RowCommand" OnRowDeleting="GrdResourceDeployment_RowDeleting"
                        OnRowCancelingEdit="GrdResourceDeployment_RowCancelingEdit" OnRowEditing="GrdResourceDeployment_RowEditing" OnRowUpdating="GrdResourceDeployment_RowUpdating" OnRowDataBound="GrdResourceDeployment_RowDataBound">
                                             <Columns>
                                                 <asp:TemplateField HeaderText="Deployed">
                                                <ItemTemplate>
                                                    <%#Eval("Deployed")%>
                                                    </ItemTemplate>
                                                  <EditItemTemplate>
                                                      <asp:HiddenField ID="hiddeployeduid" runat="server" value='<%#Eval("UID")%>' />
                                                     <asp:TextBox ID="txtDeployed" runat="server" CssClass="form-control" Text='<%#Eval("Deployed")%>'></asp:TextBox>
                                                 </EditItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Deployed Date">
                                                     <ItemTemplate>
                                                         <%#Eval("DeployedDate","{0:dd/MM/yyyy}")%>
                                                     </ItemTemplate>
                                                 </asp:TemplateField>

                                                 <asp:TemplateField HeaderText="Remarks">
                                                <ItemTemplate>
                                                    <%#Eval("Remarks")%>
                                                    </ItemTemplate>
                                                  <EditItemTemplate>
                                                     <asp:TextBox ID="txtremarks" runat="server" CssClass="form-control" Text='<%#Eval("Remarks")%>'></asp:TextBox>
                                                 </EditItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField>
                                                      <EditItemTemplate>
                                                        <asp:LinkButton ID="lnkupdate" runat="server" CausesValidation="True" CommandName="Update" Text="Update"></asp:LinkButton>
                                                        &nbsp;<asp:LinkButton ID="lnkcancel" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                                                    </EditItemTemplate>
                                                     <ItemTemplate>
                                                        <asp:LinkButton ID="lnkedit" runat="server" CausesValidation="False" CommandName="Edit" ><span title="Edit" class="fas fa-edit"></span></asp:LinkButton>
                                                     </ItemTemplate>
                                                 </asp:TemplateField>
                                                 <asp:TemplateField>
                                                     <ItemTemplate>
                                                        <asp:LinkButton ID="lnkdelete" runat="server" OnClientClick="return DeleteItem()" CausesValidation="false" CommandArgument='<%#Eval("UID")%>' CommandName="delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
                                                     </ItemTemplate>
                                                 </asp:TemplateField>
                                                 <asp:BoundField DataField="ServerCopiedAdd" ItemStyle-CssClass="hiddencol"  HeaderStyle-CssClass="hiddencol" HeaderText="ServerCopiedAdd" >
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                         <asp:BoundField DataField="ServerCopiedUpdate" ItemStyle-CssClass="hiddencol"  HeaderStyle-CssClass="hiddencol" HeaderText="ServerCopiedUpdate" >
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

<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-jointinspection-to-rabill.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_jointinspection_to_rabill" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <script type="text/javascript">
        function DeleteItem() {
            if (confirm("Are you sure you want to delete ...?")) {
                return true;
            }
            return false;
        }

         function ShowProgressBar(status) {
               if (status == "true") {
               document.getElementById('dvProgressBar').style.visibility = 'visible';

            }
                
            
            else {
                document.getElementById('dvProgressBar').style.visibility = 'hidden';
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddJointInspectiontoRaBill" runat="server">
        <%--<asp:ScriptManager ID="script1" runat="server"></asp:ScriptManager>--%>
        <div class="container-fluid" style="max-height:75vh; overflow-y:scroll;">
            <div class="row">
                <div class="col-sm-12">
                    <div class="table-responsive">
                        <%--<asp:UpdatePanel ID="Update1" runat="server">
                            <ContentTemplate>--%>
                                <asp:GridView ID="grdinspectionReport" DataKeyNames="inspectionUid" runat="server" Width="100%" AllowPaging="True" CssClass="table table-bordered" PageSize="15"
                            
                            EmptyDataText="No Data Found" AutoGenerateColumns="False" OnPageIndexChanging="grdinspectionReport_PageIndexChanging" OnRowDataBound="grdinspectionReport_RowDataBound" OnRowCommand="grdinspectionReport_RowCommand" OnRowDeleting="grdinspectionReport_RowDeleting">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="ChkBox" runat="server" EnableViewState="true" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Dia Pipe">
                                
                                <ItemTemplate>
                                    <asp:HiddenField ID="hidUid" runat="server" value='<%# Bind("inspectionUid") %>' />
                                    <asp:Label ID="lbldesc" runat="server" Text='<%# Bind("DiaPipe") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="30%" />
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Unit">
                               
                                <ItemTemplate>
                                    <asp:Label ID="lblunit" runat="server" Text='<%# Bind("unit") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="10%" />
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Invoice Number">
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("invoice_number") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="20%" />
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Invoice/Inspection Date">
                                <ItemTemplate>
                                    <asp:Label ID="Label3" runat="server" Text='<%# Bind("invoicedate", "{0:dd MMM yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="20%" />
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Inspection Type">
                                <ItemTemplate>
                                    <%#Eval("Inspection_Type")%>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Quantity">
                                <ItemTemplate>
                                    <asp:Label ID="Label4" runat="server" Text='<%# Bind("quantity") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="20%" />
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                   <ItemTemplate>
                                          <asp:LinkButton ID="lnkdelete" runat="server" CausesValidation="False" CommandArgument='<%#Eval("inspectionUid")%>' OnClientClick="return DeleteItem()" CommandName="delete">
                                              <span title="Delete" class="fas fa-trash"></span>
                                          </asp:LinkButton>
                                  </ItemTemplate>
                              </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                           <%-- </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="grdinspectionReport" EventName="PageIndexChanging" />
                                <asp:AsyncPostBackTrigger ControlID="grdinspectionReport" EventName="RowCommand" />
                            </Triggers>
                        </asp:UpdatePanel>--%>
                        
             </div>
                    </div>
                </div>
            </div>
           <div id="dvProgressBar" style=" text-align:center; position:relative; visibility:hidden;" >
                     <img src="../_assets/images/progress.gif" width="40" alt="loading"  /> <span style="color:#006699; font-weight:bold;">Processing please wait...</span>
                     </div> 
        <div class="modal-footer">
                    <%--<button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>--%>
            <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary" Text="Submit" OnClientClick="ShowProgressBar('true')" OnClick="btnAdd_Click" />
                    <%--<button type="button" class="btn btn-primary">Save changes</button>--%>
                </div>
        </form>
</asp:Content>

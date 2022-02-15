<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="choose-activity.aspx.cs" Inherits="ProjectManagementTool._modal_pages.choose_activity" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
     <form id="frmAddSubmittalModal" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="container-fluid" style="height:250px; overflow:auto;">
            <div class="row">
                <div class="col-sm-12">
                    <asp:TreeView runat="server" CssClass="it_tree_view" ID="TreeView1" ImageSet="XPFileExplorer" NodeIndent="15" EnableTheming="True" NodeWrap="True">    
                                                <%--<HoverNodeStyle Font-Underline="True" ForeColor="#6666AA" />--%>  
                                                <NodeStyle CssClass="it_tree_view__node" NodeSpacing="2px" />
                                                <ParentNodeStyle Font-Bold="False" />
                                                <SelectedNodeStyle CssClass="it_tree_view__node__selected"  HorizontalPadding="4px" VerticalPadding="2px" />
                                            </asp:TreeView> 
                    </div>
                </div>
            </div>

                 <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                </div>
         </form>
</asp:Content>

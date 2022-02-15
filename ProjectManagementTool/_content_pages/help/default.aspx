<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.help._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
       
        
        <%--Help contents--%>
    <div class="container-fluid">
      
                    <div class="row">
                        <div class="col-lg-6 col-xl-3 form-group">
                            <div class="card h-100" style="max-height:700px; overflow-y:auto;">
                                <div class="card-body">
                                    <h6 class="card-title text-muted text-uppercase font-weight-bold">HELP</h6>
                                    <hr />
                                    <asp:TreeView runat="server" CssClass="it_tree_view" ID="trHelp" ImageSet="WindowsHelp" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged" EnableTheming="True" NodeWrap="True">                               
                                        <%--<HoverNodeStyle Font-Underline="True" ForeColor="#6666AA" />--%>
                                        <NodeStyle CssClass="it_tree_view__node" NodeSpacing="2px" Font-Size="Larger" />
                                                <ParentNodeStyle Font-Bold="False" />
                                                <SelectedNodeStyle CssClass="it_tree_view__node__selected"  HorizontalPadding="4px" VerticalPadding="2px" />
                                    </asp:TreeView>    
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6 col-xl-9 form-group">
                       
    
                                <div class="card mb-4" id="ProjectDetails" runat="server">
                                    <div class="card-body">
                                        <div class="card-title">
                                            <div class="d-flex justify-content-between">
                                                <h6 class="text-muted">
                                                    <asp:Label ID="lblHelpCategory" CssClass="text-uppercase font-weight-bold" runat="server"  />
                                                    
                                                    </h6>
                                                
                                                </div>
                                            </div>
                                   
                                        
                                        </div>
                                    </div>
                             <div class="card">
                                            <div class="card-body">
                                                <div id="dvHelp" runat="server" style="width:100%;" >
                                                  
                                                </div>
                                                </div>
                                 </div>
                             
                          </div>
                     </div>
                      
      </div>
</asp:Content>

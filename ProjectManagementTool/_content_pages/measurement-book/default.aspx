<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.measurement_book._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
     <style type="text/css">
        .hiddencol { display: none; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server">
   </asp:ScriptManager>
    <%--project selection dropdowns--%>
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12 col-lg-4 form-group">Measurement Book</div>
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
                        <asp:DropDownList ID="DDLWorkPackage" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDLWorkPackage_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>

    <div class="container-fluid">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                    <div class="row">
                        <div class="col-lg-6 col-xl-4 form-group">
                            <div class="card h-100" style="max-height:700px; overflow-y:auto;">
                                <div class="card-body">
                                    <h6 class="card-title text-muted text-uppercase font-weight-bold">WorkPackage Directory</h6>
                                    <div class="form-group">
                                        <label class="sr-only" for="TxtSearchDocuments">Search</label>
                                        <div class="input-group">
                                            <input id="TxtSearchDocuments" class="form-control" type="text" placeholder="Activity name" />
                                            <div class="input-group-append">
                                                <asp:Button ID="BtnSearchDocuments" CssClass="btn btn-primary" Text="Search" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                    <asp:TreeView runat="server" CssClass="it_tree_view" ID="TreeView1" ImageSet="XPFileExplorer" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged" NodeIndent="15" EnableTheming="True" NodeWrap="True">                               
                                        <NodeStyle CssClass="it_tree_view__node" NodeSpacing="2px" />
                                        <ParentNodeStyle Font-Bold="False" />
                                        <SelectedNodeStyle CssClass="it_tree_view__node__selected" HorizontalPadding="4px" VerticalPadding="2px" />
                                    </asp:TreeView>    
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6 col-xl-8 form-group">
                                <div class="card mb-4">
                                    <div class="card-body">
                                        <div class="card-title">
                                            <div class="d-flex justify-content-between">
                                                <h6 class="text-muted">
                                                    <asp:Label ID="ActivityHeading" CssClass="text-uppercase font-weight-bold" runat="server" />
                                                    <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                                                </h6>
                                                <div>
                                                   
                                                </div>
                                            </div>
                                        </div>
                                        <div class="table-responsive">
                                                <asp:GridView ID="grdMeasurementbook" runat="server" CssClass="table table-bordered" EmptyDataText="No Data" AutoGenerateColumns="False" Width="100%" OnRowCommand="grdMeasurementbook_RowCommand" OnRowDataBound="grdMeasurementbook_RowDataBound" >
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Task Name">
                                                            <ItemTemplate>
                                                                <%#GetTaskName(Eval("TaskUID").ToString())%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:BoundField DataField="UnitforProgress" HeaderText="Unit for Progress">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Quantity" HeaderText="Quantity" >
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Description" HeaderText="Description">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CreatedDate" HeaderText="Created Date"  DataFormatString="{0:dd/MM/yyyy}">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                                <asp:BoundField DataField="Upload_File" ItemStyle-CssClass="hiddencol"  HeaderStyle-CssClass="hiddencol" HeaderText="Upload_File" >
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Document">
                                                            <ItemTemplate>
                                                                 <asp:UpdatePanel runat="server" ID="UpdatePanelDownload" UpdateMode="Conditional" ChildrenAsTriggers="True">
                                                                        <ContentTemplate>
                                                                <asp:LinkButton ID="lnkDownload" runat="server" CommandArgument='<%#Eval("UID")%>' CommandName="download" CausesValidation="false">Download</asp:LinkButton>
                                                                            </ContentTemplate>
                                                                          <Triggers>
                                                                            <asp:PostBackTrigger ControlID="lnkDownload" />
                                                                          </Triggers>
                                                                        </asp:UpdatePanel>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                 </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                          </div>
                     </div>
                        </ContentTemplate>  
                        <Triggers>
                         <asp:AsyncPostBackTrigger ControlID="TreeView1" EventName="SelectedNodeChanged" />
                         </Triggers>
                 </asp:UpdatePanel>
      </div>
</asp:Content>

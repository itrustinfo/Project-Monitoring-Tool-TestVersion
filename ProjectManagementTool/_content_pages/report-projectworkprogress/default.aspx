<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.report_projectworkprogress._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
     <div class="container-fluid">
            <div class="row">
                <div class="col-md-12 col-lg-2 form-group">Report - Project Progress</div>
                <div class="col-md-6 col-lg-5 form-group">
                    <label class="sr-only" for="DDLProject">Project</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Project</span>
                        </div>
                        <asp:DropDownList ID="ddlProject" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlProject_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-6 col-lg-5 form-group">
                    <label class="sr-only" for="DDLWorkPackage">Work Package</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Work Package</span>
                        </div>
                        <asp:DropDownList ID="ddlworkpackage" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlworkpackage_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>

    <div class="container-fluid" id="DivData" runat="server">
        <asp:ScriptManager ID="Script1" runat="server"></asp:ScriptManager>
        <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>--%>
                    <div class="row">
                        <div class="col-lg-6 col-xl-4 form-group">
                            <div class="card h-100" style="max-height:700px; overflow-y:auto;">
                                <div class="card-body">
                                    <h6 class="card-title text-muted text-uppercase font-weight-bold">Description of works</h6>
                                    <asp:TreeView runat="server" CssClass="it_tree_view" ID="TreeView1" ImageSet="XPFileExplorer" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged" NodeIndent="15" EnableTheming="True" NodeWrap="True">                               
                                        <NodeStyle CssClass="it_tree_view__node" NodeSpacing="2px" />
                                        <ParentNodeStyle Font-Bold="False" />
                                        <SelectedNodeStyle CssClass="it_tree_view__node__selected" HorizontalPadding="4px" VerticalPadding="2px" />
                                    </asp:TreeView>    
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6 col-xl-8 form-group">
                           <%-- <asp:UpdateProgress ID="UpdateProgress2" runat="Server" AssociatedUpdatePanelID="UpdatePanel2" DisplayAfter="1"> 
                                        <ProgressTemplate > 
                                            <div id="loader"></div>
                                        </ProgressTemplate> 
                                    </asp:UpdateProgress>--%>
                                <div class="card mb-4">
                                    <div class="card-body">
                                        <div class="card-title">
                                            <div class="d-flex justify-content-between">
                                                <h6 class="text-muted">
                                                    <asp:Label ID="ActivityHeading" CssClass="text-uppercase font-weight-bold" runat="server" />
                                                </h6>
                                            </div>
                                        </div>
                                        <div class="table-responsive">
                                            <asp:Literal ID="ltScript_Progress" runat="server"></asp:Literal>
                                    <div id="chart_div" style="width:100%; height:500px;"></div>
                                            </div>
                                        <div class="table-responsive">
                                            <asp:GridView ID="GrdProjectProgress" runat="server" EmptyDataText="No Records Found." Font-Size="10pt" Width="100%" CssClass="table table-bordered" DataKeyNames="TaskUID" AutoGenerateColumns="False" OnRowDataBound="GrdProjectProgress_RowDataBound">
                                            <Columns>
                                                <%--<asp:TemplateField HeaderText="Sl. No." ItemStyle-HorizontalAlign="Left" HeaderStyle-Font-Bold="true">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                                <asp:BoundField DataField="EndDate" HeaderText="Month" DataFormatString="{0:MMM yyyy}" HeaderStyle-Font-Bold="true" />
                                                <asp:BoundField DataField="Schedule_Value" HeaderText="Plan" HeaderStyle-Font-Bold="true" />
                                                <asp:BoundField DataField="Achieved_Value" HeaderText="Actual" HeaderStyle-Font-Bold="true" />
                                                <asp:BoundField DataField="TaskUID" HeaderText="Cummulative Plan" HeaderStyle-Font-Bold="true" />
                                                <asp:BoundField DataField="TaskUID" HeaderText="Cummulative Actual" HeaderStyle-Font-Bold="true" />

                                            </Columns>
                                            </asp:GridView>
                                        </div>
                                        </div>
                                    </div>
                          </div>
                     </div>
                       <%-- </ContentTemplate>  
                        <Triggers>
                         <asp:AsyncPostBackTrigger ControlID="TreeView1" EventName="SelectedNodeChanged" />
                         </Triggers>
                 </asp:UpdatePanel>--%>
      </div>
</asp:Content>

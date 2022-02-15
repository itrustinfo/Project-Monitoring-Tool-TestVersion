<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.gantt_chart._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
    <link href="../../_assets/styles/gantt/jsgantt.css" rel="stylesheet" type="text/css"/>
    <script src="../../_assets/scripts/jsgantt.js" type="text/javascript"></script>
     <script type="text/javascript">

        $(document).ready(function () {
            //$('#loader').fadeOut();
            
        });
</script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
    <%--<div id="loader"></div>  --%>
    <div class="container-fluid">
            <div class="row">
                <div class="col-md-12 col-lg-4 form-group">Project Gantt Chart</div>
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
            <div class="row">
                        <div class="col-lg-12 col-xl-12 form-group">
                            <div class="card" style="width:1348px; overflow:scroll; height:600px;" >
                                <div class="gantt" id="GanttChartDIV"></div>
                                            <asp:Literal ID="LtGantt" runat="server"></asp:Literal>
                                    </div>
                            </div>
                </div>
        </div>
</asp:Content>

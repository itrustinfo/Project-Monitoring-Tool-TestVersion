<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-clientmaster.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_clientmaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <style type="text/css">
        .auto-style1 {
            display: block;
            font-size: 1rem;
            font-weight: 400;
            line-height: 1.5;
            color: #495057;
            background-clip: padding-box;
            border-radius: .25rem;
            transition: none;
            border: 1px solid #ced4da;
            background-color: #fff;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="AddMaster" runat="server">
        <div class="container-fluid" style="min-height:82vh; overflow-y:auto; max-height:82vh;">
        <div class="row">
            <div class="col-sm-12">
                 <div class="form-group">
                         <label class="lblCss" for="DDlProject">Project</label> 
                         <asp:DropDownList CssClass="form-control" ID="DDlProject" runat="server"></asp:DropDownList>
                   </div>

                            <div class="form-group">
                                <label class="lblCss" for="txtclientname">Client Name</label> &nbsp;<span style="color:red; font-size:1.2rem;">*</span>
                                <asp:TextBox ID="txtclientname" runat="server" CssClass="form-control" autocomplete="off" required></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label class="lblCss" for="txtcode">Client Code</label> &nbsp;<span style="color:red; font-size:1.2rem;">*</span> 
                                <asp:TextBox ID="txtcode" runat="server" CssClass="form-control" autocomplete="off" required></asp:TextBox>
                            </div>
                 <div class="form-group">
                                <label class="lblCss" for="txtcode">Client Details</label> &nbsp;<span style="color:red; font-size:1.2rem;"></span> 
                                <asp:TextBox ID="txtclientdetails" TextMode="MultiLine" runat="server" autocomplete="off" CssClass="form-control" style="height: 150px;"></asp:TextBox>
                            </div>
               </div>
            </div>
        </div>

        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                </div>
    </form>
</asp:Content>

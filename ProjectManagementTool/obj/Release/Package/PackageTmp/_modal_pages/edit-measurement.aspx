<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="edit-measurement.aspx.cs" Inherits="ProjectManagementTool._modal_pages.edit_measurement" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <script>
         $(function () {
             $("input[id$='dtMeasurementDate']").datepicker({
                 changeMonth: true,
                 changeYear: true,
                 dateFormat: 'dd/mm/yy'
             });
         });
     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmEditMeasurementModal" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="container-fluid" style="max-height:82vh; overflow-y:auto;">
            <div class="row">
                <div class="col-sm-12">
                    <div class="form-group">
                        <label class="lblCss" for="DDLWorkPackage">Unit for Progress</label>
                         <asp:Label ID="LblUnitforProgress" runat="server" CssClass="form-control"></asp:Label>
                        <asp:HiddenField ID="HiddenTaskID" runat="server" />
                    </div>
                    
                     <div class="form-group">
                        <label class="lblCss" for="txtquantity">Quantity</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                         <asp:TextBox ID="txtquantity" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                    </div>

                     <div class="form-group">
                        <label class="lblCss" for="dtMeasurementDate">Date</label>&nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                       <asp:TextBox ID="dtMeasurementDate" CssClass="form-control" runat="server" placeholder="dd/mm/yyyy" required autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label class="lblCss" for="txtremarks">Remarks</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="txtremarks" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                        
                    </div>
                </div>
            </div> 
        </div>

        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click"/>
                </div>
    </form>
</asp:Content>

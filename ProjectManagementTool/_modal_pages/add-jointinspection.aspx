<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-jointinspection.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_jointinspection" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <script type="text/javascript">
        function BindEvents() {
        $(".showBOQModal").click(function(e) {
        //document.getElementsByClassName("showTaskModal").click(function(e) {
            e.preventDefault();
            jQuery.noConflict();
        var url = $(this).attr("href");
        $("#ModBOQData iframe").attr("src", url);
        $("#ModBOQData").modal("show");
        });
        }
        $(document).ready(function () {
            BindEvents();
        });
</script>

    <script type="text/javascript">
 $( function() {
    $("input[id$='dtInvoiceDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
     <form id="frmAddJointInspectionModal" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="container-fluid" style="max-height:75vh; overflow-y:auto;">
            <div class="row">
                <div class="col-sm-12">
                    <%--<div class="form-group">
                        <label class="lblCss" for="txttaskname">Link to BOQ</label>
                        <asp:Label ID="lblActivityName" runat="server" CssClass="form-control"></asp:Label>                        
                         <a id="LinkBOQData" runat="server" href="/_modal_pages/boq-treeview.aspx" class="showBOQModal">
                             <asp:Button ID="btnchoose" runat="server" CausesValidation="false" Text="Choose BOQ Activity" CssClass="form-control btn-link" />
                         </a>
                    </div>--%>
                    
                     <div class="form-group">
                        <label class="lblCss" for="txtdiaofpipe">Dia of Pipe</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                         <asp:TextBox ID="txtdiaofpipe" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="DDLInspectionType">Inspection Type</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                         <asp:DropDownList ID="DDLInspectionType" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDLInspectionType_SelectedIndexChanged">
                             <asp:ListItem Value="Supply">Supply</asp:ListItem>
                             <asp:ListItem Value="Epoxy">Epoxy</asp:ListItem>
                             <asp:ListItem Value="Guniting">Guniting</asp:ListItem>
                             <asp:ListItem Value="Laying & Jointing">Laying & Jointing</asp:ListItem>
                             <asp:ListItem Value="Hydro Testing">Hydro Testing</asp:ListItem>
                             <asp:ListItem Value="EarthWork / Excavation">EarthWork / Excavation</asp:ListItem>
                             <asp:ListItem Value="Barricading">Barricading</asp:ListItem>
                             <asp:ListItem Value="DeWatering the Sewage">DeWatering the Sewage</asp:ListItem>
                             <asp:ListItem Value="Shoring and Strutting">Shoring and Strutting</asp:ListItem>
                         </asp:DropDownList>
                    </div>
                    <div class="form-group" id="PipeNumber" runat="server">
                        <label class="lblCss" for="txtinvoicenumber">Pipe Number</label> 
                        <asp:TextBox ID="txtpipenumber" CssClass="form-control" runat="server" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                    </div>

                    <div class="form-group" id="SupplierNumber" runat="server">
                        <label class="lblCss" for="txtinvoicenumber">Supplier Invoice Number</label> 
                        <asp:TextBox ID="txtinvoicenumber" CssClass="form-control" runat="server" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                    </div>

                    <div class="form-group" id="SupplierDate" runat="server">
                        <label class="lblCss" id="LblDate" runat="server" for="dtInvoiceDate">Supplier Invoice Date</label>&nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                       <asp:TextBox ID="dtInvoiceDate" CssClass="form-control" runat="server" placeholder="dd/mm/yyyy" required autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="txtunit">Unit</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="txtunit" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                        
                    </div>
                     <div class="form-group" id="DivQuantity" runat="server">
                        <label class="lblCss" for="txtquantity">Quantity</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="txtquantity" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                        
                    </div>

                    <div class="form-group" id="ChainageNum" runat="server" visible="false">
                        <label class="lblCss" for="txtchainagenumber">Chainage Number</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="txtchainagenumber" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                        
                    </div>
                    <div class="form-group" id="ChainageDesc" runat="server" visible="false">
                        <label class="lblCss" for="txtunit">Chainage Description</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="txtchainagedesc" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                        
                    </div>
                    <div class="form-group" id="ChainageStartingPoint" runat="server" visible="false">
                        <label class="lblCss" for="txtstartingpoint">Starting Point</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="txtstartingpoint" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                        
                    </div>
                    <div class="form-group" id="ChainageLength" runat="server" visible="false">
                        <label class="lblCss" for="txtlength">Length</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="txtlength" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                        
                    </div>

                    <div class="form-group" id="Guniting_QtyinRMT" runat="server" visible="false">
                        <label class="lblCss" for="txtQtyinRMT">Quantity in Rmt</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="txtQtyinRMT" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                        
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="FilDocument">Choose File/s</label> 
                        <div class="custom-file">
                            <asp:FileUpload ID="InspectionDocs" runat="server" AllowMultiple="true" CssClass="custom-file-input" />

                            <%--<asp:FileUpload ID="FilDocument" CssClass="custom-file-input" runat="server" ClientIDMode="Static" /> --%>
                            <label class="custom-file-label" id="SelectedFiles" for="FilDocument">Choose document</label>
                        </div>
                    </div>
                    <div class="form-group" id="DivNumber" runat="server" visible="false">
                        <label class="lblCss" for="txtNumber">Number</label> 
                        <asp:TextBox ID="txtnumber" CssClass="form-control" runat="server" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                        
                    </div>
                    <div class="form-group" id="DivLenght" runat="server" visible="false">
                        <label class="lblCss" for="txtLenght">Lenght</label> 
                        <asp:TextBox ID="txtMLenght" CssClass="form-control" runat="server" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                        
                    </div>
                    <div class="form-group" id="DivWidth" runat="server" visible="false">
                        <label class="lblCss" for="txtWidth">Width</label> 
                        <asp:TextBox ID="txtWidth" CssClass="form-control" runat="server" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                        
                    </div>
                    <div class="form-group" id="DivDepth" runat="server" visible="false">
                        <label class="lblCss" for="txtDepth">Depth</label> 
                        <asp:TextBox ID="txtDepth" CssClass="form-control" runat="server" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                        
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtRemarks">Remarks</label> 
                        <asp:TextBox ID="txtRemarks" CssClass="form-control" runat="server" TextMode="MultiLine" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                        
                    </div>
                </div>
            </div> 
        </div>

        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click"/>
                </div>

         <%--Link BOQ Data modal--%>
    <div id="ModBOQData" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Link BOQ Activity</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:340px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
    </form>
</asp:Content>

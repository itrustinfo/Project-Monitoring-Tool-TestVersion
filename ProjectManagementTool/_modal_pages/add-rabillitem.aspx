<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-rabillitem.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_rabillitem" %>
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="FrmAddRAbillItem" runat="server">
        <div class="container-fluid">
            <div class="row">
                <div class="col-sm-12">
                    <div class="form-group">
                        <label class="lblCss" for="txttaskname">Choose Item Number</label>  &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:Label ID="lblActivityName" runat="server" CssClass="form-control"></asp:Label>                        
                         <a id="LinkBOQData" runat="server" href="/_modal_pages/boq-treeview.aspx" class="showBOQModal">
                             <asp:Button ID="btnchoose" runat="server" CausesValidation="false" Text="Choose Item Number" CssClass="form-control btn-link" />
                         </a>
                    </div>
                    <%--<div class="form-group">
                        <label class="lblCss" for="txtRabillNumber">RA Bill Number</label>  &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="txtaddrabillnumber" CssClass="form-control" autocomplete="off" required  runat="server" ClientIDMode="Static"></asp:TextBox>
                        
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="txtRabillNumber">RA Bill Date</label>  &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="txtDate" CssClass="form-control" autocomplete="off" required  runat="server" ClientIDMode="Static"></asp:TextBox>
                        
                    </div>--%>
                    <div class="form-group">
                        <label class="lblCss" for="txtRabillNumber">Item Description</label>  &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="txtradescription" CssClass="form-control" autocomplete="off" required  runat="server" ClientIDMode="Static"></asp:TextBox>
                    </div>
                    
                    <%--<div class="form-group">
                        <label class="lblCss" for="txtRabillNumber">Actual Cost</label>
                        <asp:TextBox ID="txtracost" CssClass="form-control" autocomplete="off" required  runat="server" ClientIDMode="Static"></asp:TextBox>
                    </div>--%>
                </div> 
                
        </div>
            </div>
        <div class="modal-footer">
            <asp:Button ID="btnaddrabillitem" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnaddrabillitem_Click"  />
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

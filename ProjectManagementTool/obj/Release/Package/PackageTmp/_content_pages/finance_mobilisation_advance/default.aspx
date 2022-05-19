<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.finance_mobilisation_advance._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
      <style>
        .Hide
            {
                display : none;
            }
    </style>
  <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
  <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
  <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script type="text/javascript">
        function DeleteItem() {
            if (confirm("All data associated with this will be deleted. Are you sure you want to delete ...?")) {
                return true;
            }
            return false;
        }
       
    </script>
   
    <script type="text/javascript">
       // Sys.WebForms.PageRequestManager.getInstance().add_endRequest(DateText);
        function DateText() {
            $(".datepick").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy'
                });
            //$(function () {
                
            //    $("input[id$='dtActualPymentDate']").datepicker({
            //        changeMonth: true,
            //        changeYear: true,
            //        dateFormat: 'dd/mm/yy'
            //    });
            //    $(".datepick").datepicker({
            //        changeMonth: true,
            //        changeYear: true,
            //        dateFormat: 'dd/mm/yy'
            //    });
            //});
        }


          function BindEvents() {
           

              $(".showModalDocumentView").click(function (e) {
                 // alert("test");
                 jQuery.noConflict();
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModViewDocument iframe").attr("src", url);
        $("#ModViewDocument").modal("show");
            });
              
              $(".showBankGuaranteeModal").click(function (e) {
                 jQuery.noConflict();
                e.preventDefault();
                var url = $(this).attr("href");
                $("#ModAddBankGuarantee iframe").attr("src", url);
                $("#ModAddBankGuarantee").modal("show");
              });

              $(".showMobilisationAdvanceModal").click(function (e) {
                  jQuery.noConflict();
                  e.preventDefault();
                  var url = $(this).attr("href");
                  $("#ModAddMobilisationAdvance iframe").attr("src", url);
                  $("#ModAddMobilisationAdvance").modal("show");
              });        
        }

        $(document).ready(function () {
            //$('#loader').fadeOut();
             BindEvents();
            DateText();

           
        });

        function isNumberKey(txt, evt) {
      var charCode = (evt.which) ? evt.which : evt.keyCode;
      if (charCode == 46) {
        //Check if the text already contains the . character
        if (txt.value.indexOf('.') === -1) {
          return true;
        } else {
          return false;
        }
      } else {
        if (charCode > 31 &&
          (charCode < 48 || charCode > 57))
          return false;
      }
      return true;
    }
        
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
     <%--project selection dropdowns--%>

     <asp:ScriptManager ID="ScriptManager1" runat="server">
   </asp:ScriptManager>
    <%--<div id="loader"></div>--%>
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12 col-lg-4 form-group">Mobilization Advance</div>
                <div class="col-md-6 col-lg-4 form-group">
                    <label class="sr-only" for="DDLProject">Project</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Project</span>
                        </div>
                        <asp:DropDownList ID="ddlProject" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlProject_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-6 col-lg-4 form-group">
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
    <div class="container-fluid">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                    <div class="row">
                        <div class="col-lg-6 col-xl-4 form-group" style="display:none">
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
                                 </div>
                            </div>
                        </div>
                        <div class="col-lg-12 col-xl-12 form-group">
                            <asp:UpdateProgress ID="UpdateProgress2" runat="Server" AssociatedUpdatePanelID="UpdatePanel2" DisplayAfter="1"> 
                                        <ProgressTemplate > 
                                        <!-- <asp:Label ID="lblwait" runat="server" Text="Please wait while we fetch the data.." ForeColor="Red"></asp:Label>-->
                                            <div id="loader"></div>
                                        </ProgressTemplate> 
                                    </asp:UpdateProgress>
                                <div class="card mb-4">
                                    <div class="card-body">
                                        <div class="card-title">
                                            <div class="d-flex justify-content-between">
                                                <h6 class="text-muted">
                                                    <asp:Label ID="ActivityHeading" CssClass="text-uppercase font-weight-bold" runat="server" />
                                                    <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                                                </h6>
                                                
                                            </div>
                                        </div>
                                        <div class="table-responsive">
                                             <div align="left">
                                                 <a id="AddAdvance" runat="server" href="~/_modal_pages/add-MobilisationAdvance.aspx" class="showMobilisationAdvanceModal"><asp:Button ID="Button1" runat="server" Text="ADD" CssClass="btn btn-primary"></asp:Button></a>
                                             </div>
                                             <br />
                                            <div id="divStatus" runat="server" visible="true">
                                            <asp:GridView ID="GridView1" runat="server" DataKeyNames="MobilizationAdvanceUID"  CssClass="table table-bordered" OnRowDataBound="OnRowDataBound" OnRowEditing="OnRowEditing" OnRowCancelingEdit="OnRowCancelingEdit" OnRowUpdating="OnRowUpdating" OnRowDeleting="OnRowDeleting" EmptyDataText="No records has been added." AllowPaging="false" AutoGenerateColumns="false" AutoGenerateEditButton="false" AutoGenerateDeleteButton="false" width="100%" OnPageIndexChanging="GridView1_PageIndexChanging" PageSize="5" OnRowUpdated="GridView1_RowUpdated" ShowFooter="true" ShowHeader="true" >
                                                <Columns>  
                                                    <asp:BoundField DataField="MobilizationAdvanceUID" HeaderText="ID" Visible="false" />  
                                                    <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" />  
                                                    <asp:BoundField DataField="DateGiven" HeaderText="Date Given" DataFormatString="{0:dd/MM/yyyy}" />
                                                   
                                                    <asp:TemplateField HeaderText="Amount" SortExpression="AdvanceAmount" FooterStyle-HorizontalAlign ="Right" HeaderStyle-CssClass="text-right">
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label5" runat="server" 
                                                                Text='<%# Bind("AdvanceAmount") %>' ></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="NewAdvanceAmount" runat="server" ReadOnly="true"></asp:TextBox>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="TransactionType" HeaderText="Credit/Debit" /> 
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <a href="/_modal_pages/add-MobilisationAdvance.aspx?MobilizationAdvanceUID=<%#Eval("MobilizationAdvanceUID") %>" class="showMobilisationAdvanceModal"><span title="Edit" class="fas fa-edit"></span></a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField> 
                                                     <asp:CommandField ShowDeleteButton="True" ButtonType="Link" />
                                                     
                                                </Columns>  
                                            </asp:GridView>

                                        </div>
                                           
                                        </div>
                                        </div>
                                    </div>
                          </div>
                     </div>
                        </ContentTemplate>  
                        
        </asp:UpdatePanel>
    </div>
       <%--View document modal--%>
    <div id="ModViewDocument" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">View Payment Details</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:500px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
     <div id="ModAddBankGuarantee" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Month Data</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:500px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
    <div id="ModAddMobilisationAdvance" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Mobilisation Advance</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:500px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
</asp:Content>

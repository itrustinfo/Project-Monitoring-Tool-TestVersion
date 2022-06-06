<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.task_report_selection._default" %>
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
            <div class="row" style="padding:0px;margin-bottom:5px">
                <div class="col-md-2 col-lg-2 form-group">Task Selection Update</div>
                 <div class="col-md-3 col-lg-3 form-group">
                    <label class="sr-only" for="DropDownList1">TaskReport</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Task Report</span>
                        </div>
                        <asp:DropDownList ID="DropDownList1" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged ="DropDownList1_SelectedIndexChanged" >
                            <asp:ListItem>Select Item</asp:ListItem>
                            <asp:ListItem>Report-1</asp:ListItem>
                            <asp:ListItem>Report-2</asp:ListItem>
                            <asp:ListItem>Report-3</asp:ListItem>
                            <asp:ListItem>Report-4</asp:ListItem>
                            <asp:ListItem>Report-5</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="col-md-3 col-lg-3 form-group">
                    <label class="sr-only" for="DDLProject">Project</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Project</span>
                        </div>
                        <asp:DropDownList ID="ddlProject" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlProject_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-3 col-lg-3 form-group">
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
                    <div class="row" style="text-align:center">
                        <%--<div class="col-lg-6 col-xl-4 form-group" style="display:none">
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
                        </div>--%>
                       <div class="col-lg-3 col-xl-3 form-group"></div>
                        <div class="col-lg-6 col-xl-6 form-group">
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
                                                 <asp:Button ID="btnSubmit" runat="server" Text="Update Tasks" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                                             </div>
                                             <br />
                                            <div id="divStatus" runat="server" visible="true">
                                                <%--insert treeview--%>
                                                <asp:TreeView runat="server" CssClass="it_tree_view" ID="TreeView1" ImageSet="XPFileExplorer" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged" NodeIndent="15" EnableTheming="True" NodeWrap="True" Height ="500px">                               
                                                    <NodeStyle CssClass="it_tree_view__node" NodeSpacing="2px" />
                                                    <ParentNodeStyle Font-Bold="False" />
                                                    <SelectedNodeStyle CssClass="it_tree_view__node__selected" HorizontalPadding="4px" VerticalPadding="2px" />
                                                    <Nodes>
                                                        <asp:TreeNode Checked="false"></asp:TreeNode>
                                                    </Nodes>
                                                </asp:TreeView>   
                                            </div>

                                        </div>
                                        </div>
                                </div>

                          </div>
                    

                          </div>
                    <%-- </div>--%>
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

<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-submittalsforcategory.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_submittalsforcategory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
        <style type="text/css">
        .loadingdiv {
            position:absolute;
            text-align:center;
            vertical-align:middle;
            width:100%;
            z-index: 250;
        }
    </style>
    <script type="text/javascript">
        function BindEvents() {
        $(".showTaskModal").click(function(e) {
        //document.getElementsByClassName("showTaskModal").click(function(e) {
            e.preventDefault();
            jQuery.noConflict();
        var url = $(this).attr("href");
        $("#ModTask iframe").attr("src", url);
        $("#ModTask").modal("show");
        });
        }
        $(document).ready(function () {
            BindEvents();
        });
</script>
    <script type="text/javascript">
 $( function() {
    $("input[id$='dtSubTargetDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });

      $("input[id$='dtQualTargetDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
     });
     $("input[id$='dtRev_B_TargetDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
     });
     $("input[id$='dtRevTargetDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
     });
     $("input[id$='dtAppTargetDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });
    });
</script>

    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
     <form id="frmAddSubmittalModal" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="container-fluid" style="max-height:80vh; overflow-y:auto; margin-top:15px;">
            <div class="row">
                <div class="col-sm-12">
                    <div class="form-group" style="display:none;">
                        <label class="lblCss" for="DDlProject">Project</label>
                         <asp:DropDownList ID="DDlProject" CssClass="form-control" runat="server"></asp:DropDownList>
                    </div>
                    
                    <div class="form-group" style="display:none;">
                        <label class="lblCss" for="DDLWorkPackage">Work Package</label>
                         <asp:DropDownList ID="DDLWorkPackage" CssClass="form-control" runat="server"></asp:DropDownList>
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="DDlDiscipline">Document Category</label>
                        <asp:DropDownList ID="DDLDocumentCategory" runat="server" CssClass="form-control">
                   </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txttaskname">Link to Activity</label>
                        <asp:Label ID="lblActivityName" runat="server" CssClass="form-control"></asp:Label>                        
                         <a id="LinkActivity" runat="server" href="/_modal_pages/choose-activity.aspx" class="showTaskModal">
                             <asp:Button ID="btnchoose" runat="server" CausesValidation="false" Text="Choose Activity" CssClass="form-control btn-link" />
                         </a>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtdesc">Document Name</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="txtDocumentName" CssClass="form-control" autocomplete="off" runat="server" required ClientIDMode="Static"></asp:TextBox>
                        
                    </div>
                      <div class="form-group">
                        <label class="lblCss" for="ddlSubDocType">Submittal Type</label> &nbsp;<span style="color:red; font-size:1rem;"></span>
                        <asp:DropDownList ID="ddlSubDocType" runat="server" CssClass="form-control">
                            
                   </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtestdocs">Estimated No. Of Documents</label> &nbsp;<span style="color:red; font-size:1rem;"></span>
                        <asp:TextBox ID="txtestdocs" CssClass="form-control" runat="server" autocomplete="off" ClientIDMode="Static">0</asp:TextBox>
                        
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="dtStartdate">Document Flow</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:DropDownList ID="DDLDocumentFlow" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDLDocumentFlow_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                      <div class="form-group" id="synchDisplay" runat="server">
                        <label class="lblCss" for="dtStartdate">Synching</label> &nbsp;<span style="color:red; font-size:1rem;"></span>
                        <asp:CheckBox ID="chkSync" runat="server" CssClass="form-control"/>
                            
                        
                    </div>
                     <div class="form-group" id="S1Display" runat="server">
                        <asp:Label ID="lblStep1Display" CssClass="lblCss" runat="server"></asp:Label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:DropDownList ID="ddlSubmissionUSer" runat="server" CssClass="form-control" required>
                     </asp:DropDownList>
                    </div>

                    <div class="form-group" id="S1Date" runat="server">
                        <asp:Label ID="lblStep1Date" CssClass="lblCss" runat="server"></asp:Label>
                       <asp:TextBox ID="dtSubTargetDate" CssClass="form-control" runat="server" ClientIDMode="Static"></asp:TextBox>
                    </div>

                    <div class="form-group" id="S2Display" runat="server">
                        <asp:Label ID="lblStep2Display" CssClass="lblCss" runat="server"></asp:Label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:DropDownList ID="ddlQualityEngg" runat="server" CssClass="form-control" required>
                     </asp:DropDownList>
                    </div>

                    <div class="form-group" id="S2Date" runat="server">
                        <asp:Label ID="lblStep2Date" CssClass="lblCss" runat="server"></asp:Label>
                       <asp:TextBox ID="dtQualTargetDate" CssClass="form-control"  runat="server" ClientIDMode="Static"></asp:TextBox>
                    </div>

                    <div class="form-group" id="S3Display" runat="server">
                        <asp:Label ID="lblStep3Display" CssClass="lblCss" runat="server"></asp:Label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:DropDownList ID="ddlReviewer_B" runat="server" CssClass="form-control" required>
                     </asp:DropDownList>
                    </div>

                    <div class="form-group" id="S3Date" runat="server">
                        <asp:Label ID="lblStep3Date" CssClass="lblCss" runat="server"></asp:Label>
                       <asp:TextBox ID="dtRev_B_TargetDate" CssClass="form-control" runat="server" ClientIDMode="Static"></asp:TextBox>
                    </div>

                    <div class="form-group" id="S4Display" runat="server">
                        <asp:Label ID="lblStep4Display" CssClass="lblCss" runat="server"></asp:Label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:DropDownList ID="ddlReviewer" runat="server" CssClass="form-control" required>
                     </asp:DropDownList>
                    </div>

                    <div class="form-group" id="S4Date" runat="server">
                        <asp:Label ID="lblStep4Date" CssClass="lblCss" runat="server"></asp:Label>
                       <asp:TextBox ID="dtRevTargetDate" CssClass="form-control" runat="server" ClientIDMode="Static"></asp:TextBox>
                    </div>

                    <div class="form-group" id="S5Display" runat="server">
                        <asp:Label ID="lblStep5Display" CssClass="lblCss" runat="server"></asp:Label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:DropDownList ID="ddlApproval" runat="server" CssClass="form-control" required>
                     </asp:DropDownList>
                    </div>

                    <div class="form-group" id="S5Date" runat="server">
                        <asp:Label ID="lblStep5Date" CssClass="lblCss" runat="server"></asp:Label>
                       <asp:TextBox ID="dtAppTargetDate" CssClass="form-control" runat="server" ClientIDMode="Static"></asp:TextBox>
                    </div> <div class="form-group">
                        <label class="lblCss" for="txtremarks">Remarks</label> &nbsp;<span style="color:red; font-size:1rem;"></span>
                        <asp:TextBox ID="txtremarks" CssClass="form-control" runat="server" autocomplete="off" ClientIDMode="Static" TextMode="MultiLine"></asp:TextBox>
                        
                    </div>

                </div>
            </div> 

            <div id="loading" runat="server" visible="false">
                   <img src="../_assets/images/progress.gif" width="100" />
            </div>
        </div>

        <div class="modal-footer">
                    <%--<button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>--%>
                    <%--<button type="button" class="btn btn-primary">Save changes</button>--%>
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                </div>

         <%--add Submittal modal--%>
    <div id="ModTask" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Link Activity</h5>
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

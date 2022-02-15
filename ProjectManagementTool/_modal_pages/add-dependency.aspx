<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-dependency.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_dependency" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
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
    $("input[id$='dtStartDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });

      $("input[id$='dtEndDate']").datepicker({
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
        <div class="container-fluid" style="max-height:84vh; overflow:auto;">
            <asp:HiddenField ID="Hidden1" runat="server" />
            <div class="row">
                <div class="col-sm-12">
                    
                    <div class="form-group">
                        <label class="lblCss" for="txttaskname">Choose Dependent Activity</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        &nbsp;&nbsp;<a id="EditActivity" runat="server" href="/_modal_pages/choose-activity.aspx" class="showTaskModal">
                             Change Activity
                         </a>
                        <asp:Label ID="lblActivityName" runat="server" CssClass="form-control"></asp:Label>                        
                         <a id="LinkActivity" runat="server" href="/_modal_pages/choose-activity.aspx" class="showTaskModal">
                             <asp:Button ID="btnchoose" runat="server" CausesValidation="false" Text="Choose Activity" CssClass="form-control btn-link" />
                         </a>
                    </div>
                  <div class="form-group">
                        <label class="lblCss" for="txtdependency">Dependency Name</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="txtdependency" CssClass="form-control" autocomplete="off" runat="server" required ClientIDMode="Static"></asp:TextBox>
                        
                    </div>
                    <div class="form-group">
                        <asp:Label ID="lblStep1Date" CssClass="lblCss" runat="server">Start Date</asp:Label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                       <asp:TextBox ID="dtStartDate" CssClass="form-control" autocomplete="off" runat="server" required ClientIDMode="Static"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <asp:Label ID="lblStep2Date" CssClass="lblCss" runat="server">Planned EndDate</asp:Label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                       <asp:TextBox ID="dtEndDate" CssClass="form-control" autocomplete="off"  runat="server" required ClientIDMode="Static"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label1" CssClass="lblCss" runat="server">Dependency Type</asp:Label>
                       <asp:DropDownList ID="DDLType" runat="server" CssClass="form-control">
                           <asp:ListItem Value="FS">Finish to Start</asp:ListItem>
                         <asp:ListItem Value="SF">Start to Finish</asp:ListItem>
                         <asp:ListItem Value="SS">Start to Start</asp:ListItem>
                         <asp:ListItem Value="FF">Finish to Finish</asp:ListItem>
                       </asp:DropDownList>
                    </div>

                    <div class="form-group">
                        <label class="lblCss" for="txtdesc">Description</label>
                        <asp:TextBox ID="txtdesc" CssClass="form-control" autocomplete="off" TextMode="MultiLine" runat="server" ClientIDMode="Static"></asp:TextBox>
                        
                    </div>
                </div>
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
				    <h5 class="modal-title">Choose Activity</h5>
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

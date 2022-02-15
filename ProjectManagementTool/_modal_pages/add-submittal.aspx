<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-submittal.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_submittal" %>
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
        function compareDates(endate) {
           var startDate = document.getElementById("dtSubTargetDate").value;
           var EndDate = document.getElementById("dtQualTargetDate").value;
           if (startDate != "" && EndDate != "") {
               startDate = startDate.split("/");
               EndDate = EndDate.split("/");
               startDate = startDate[1] + "/" + startDate[0] + "/" + startDate[2];
               EndDate = EndDate[1] + "/" + EndDate[0] + "/" + EndDate[2];
               var stDate = new Date(startDate);
               var eDate = new Date(EndDate);
               
               if (stDate > eDate) {
                   document.getElementById("dtQualTargetDate").value = "";
                   alert("Review target date should be greater than Submission target date");
               }
           }
        }

        function compareApproveDates(endate) {
           var startDate = document.getElementById("dtQualTargetDate").value;
           var EndDate = document.getElementById("dtRev_B_TargetDate").value;
           if (startDate != "" && EndDate != "") {
               startDate = startDate.split("/");
               EndDate = EndDate.split("/");
               startDate = startDate[1] + "/" + startDate[0] + "/" + startDate[2];
               EndDate = EndDate[1] + "/" + EndDate[0] + "/" + EndDate[2];
               var stDate = new Date(startDate);
               var eDate = new Date(EndDate);
               
               if (stDate > eDate) {
                   document.getElementById("dtRev_B_TargetDate").value = "";
                   alert("Approve target date should be greater than Review target date");
               }
           }
        }
    </script>
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
        <div class="container-fluid" style="max-height:82vh; overflow-y:auto;">
            <asp:HiddenField ID="HiddenParentTask" runat="server" />
            <div class="row">
                <div class="col-sm-12">
                    <asp:HiddenField ID="ActivityUID" runat="server" />
                    <div class="form-group" style="display:none;">
                        <label class="lblCss" for="DDlProject">Project</label>
                         <asp:DropDownList ID="DDlProject" CssClass="form-control" runat="server"></asp:DropDownList>
                    </div>
                    
                    <div class="form-group" style="display:none;">
                        <label class="lblCss" for="DDLWorkPackage">Work Package</label>
                         <asp:DropDownList ID="DDLWorkPackage" CssClass="form-control" runat="server"></asp:DropDownList>
                    </div>
                    
                     <div class="form-group">
                        <label class="lblCss" for="txttaskname">Task Name</label>&nbsp;&nbsp;<a id="LinkActivity" runat="server" href="/_modal_pages/choose-activity.aspx" class="showTaskModal">
                             Change Activity
                         </a>
                         <asp:TextBox ID="txttaskname" CssClass="form-control" runat="server" Enabled="false" required ClientIDMode="Static"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label class="lblCss" for="txtdesc">Submittal Name</label> &nbsp;<span style="color:red; font-size:1rem;">*</span>
                        <asp:TextBox ID="txtDocumentName" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                        
                    </div>

                    <div class="form-group">
                        <label class="lblCss" for="DDlDiscipline">Submittal Category</label> &nbsp;<span style="color:red; font-size:1rem;">*</span>
                        <asp:DropDownList ID="DDLDocumentCategory" runat="server" CssClass="form-control" required>
                            
                   </asp:DropDownList>
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
                        <label class="lblCss" for="dtStartdate">Submittal Flow</label> &nbsp;<span style="color:red; font-size:1rem;">*</span>
                        <asp:DropDownList ID="DDLDocumentFlow" runat="server" CssClass="form-control" required AutoPostBack="true" OnSelectedIndexChanged="DDLDocumentFlow_SelectedIndexChanged">
                            
                        </asp:DropDownList>
                         <asp:Label ID="lblflowmsg" runat="server" Visible="false" Text="Please Note flow is allowed to be changed only if there are no documents uploaded.Please delete the documents if you want to change the flow " Font-Italic="True" ForeColor="Red"></asp:Label>
                    </div>
                       <div class="form-group" id="synchDisplay" runat="server">
                        <label class="lblCss" for="dtStartdate">Synching</label> &nbsp;<span style="color:red; font-size:1rem;"></span>
                        <asp:CheckBox ID="chkSync" runat="server" CssClass="form-control"/>
                            
                        
                    </div>
                    

                     <div class="form-group" id="S1Display" runat="server">
                        <asp:Label ID="lblStep1Display" CssClass="lblCss" runat="server"></asp:Label> &nbsp;<span style="color:red; font-size:1rem;">*</span>
                        <asp:DropDownList ID="ddlSubmissionUSer" runat="server" CssClass="form-control" required>
                     </asp:DropDownList>
                    </div>

                    <div class="form-group" id="ForPhotograph" runat="server">
                        <asp:Label ID="lblPhotograph" CssClass="lblCss" runat="server"></asp:Label> &nbsp;<span style="color:red; font-size:1rem;">*</span>
                        <div>
                            <div style="float:left; width:48%;">
                                <asp:DropDownList ID="DDLMonth" runat="server" CssClass="form-control">
                            <asp:ListItem Value="01">Jan</asp:ListItem>
                            <asp:ListItem Value="02">Feb</asp:ListItem>
                            <asp:ListItem Value="03">Mar</asp:ListItem>
                            <asp:ListItem Value="04">Apr</asp:ListItem>
                            <asp:ListItem Value="05">May</asp:ListItem>
                            <asp:ListItem Value="06">Jun</asp:ListItem>
                            <asp:ListItem Value="07">Jul</asp:ListItem>
                            <asp:ListItem Value="08">Aug</asp:ListItem>
                            <asp:ListItem Value="09">Sep</asp:ListItem>
                            <asp:ListItem Value="10">Oct</asp:ListItem>
                            <asp:ListItem Value="11">Nov</asp:ListItem>
                            <asp:ListItem Value="12">Dec</asp:ListItem>
                        </asp:DropDownList>
                            </div>
                            <div style="float:left; width:4%;">
                                &nbsp;
                                </div>
                            <div style="float:left; width:48%;">
                                <asp:DropDownList ID="DDLYear" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        
                        
                    </div>

                    <div class="form-group" id="S1Date" runat="server">
                        <asp:Label ID="lblStep1Date" CssClass="lblCss" runat="server"></asp:Label>
                       <asp:TextBox ID="dtSubTargetDate" CssClass="form-control" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                    </div>

                    <div class="form-group" id="S2Display" runat="server">
                        <asp:Label ID="lblStep2Display" CssClass="lblCss" runat="server"></asp:Label> &nbsp;<span style="color:red; font-size:1rem;">*</span>
                        <asp:DropDownList ID="ddlQualityEngg" runat="server" CssClass="form-control" required>
                     </asp:DropDownList>
                    </div>

                    <div class="form-group" id="S2Date" runat="server">
                        <asp:Label ID="lblStep2Date" CssClass="lblCss" runat="server"></asp:Label>
                       <asp:TextBox ID="dtQualTargetDate" CssClass="form-control" placeholder="dd/mm/yyyy" autocomplete="off"  runat="server" ClientIDMode="Static" onchange="javascript:compareDates(this);"></asp:TextBox>
                    </div>

                    <div class="form-group" id="S3Display" runat="server">
                        <asp:Label ID="lblStep3Display" CssClass="lblCss" runat="server"></asp:Label> &nbsp;<span style="color:red; font-size:1rem;">*</span>
                        <asp:DropDownList ID="ddlReviewer_B" runat="server" CssClass="form-control" required>
                     </asp:DropDownList>
                    </div>

                    <div class="form-group" id="S3Date" runat="server">
                        <asp:Label ID="lblStep3Date" CssClass="lblCss" runat="server"></asp:Label> 
                       <asp:TextBox ID="dtRev_B_TargetDate" CssClass="form-control" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" ClientIDMode="Static" onchange="javascript:compareApproveDates(this);"></asp:TextBox>
                    </div>

                    <div class="form-group" id="S4Display" runat="server">
                        <asp:Label ID="lblStep4Display" CssClass="lblCss" runat="server"></asp:Label> &nbsp;<span style="color:red; font-size:1rem;">*</span>
                        <asp:DropDownList ID="ddlReviewer" runat="server" CssClass="form-control" required>
                     </asp:DropDownList>
                    </div>

                    <div class="form-group" id="S4Date" runat="server">
                        <asp:Label ID="lblStep4Date" CssClass="lblCss" runat="server"></asp:Label>
                       <asp:TextBox ID="dtRevTargetDate" CssClass="form-control" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                    </div>

                    <div class="form-group" id="S5Display" runat="server">
                        <asp:Label ID="lblStep5Display" CssClass="lblCss" runat="server"></asp:Label> &nbsp;<span style="color:red; font-size:1rem;">*</span>
                        <asp:DropDownList ID="ddlApproval" runat="server" CssClass="form-control" required>
                     </asp:DropDownList>
                    </div>

                    <div class="form-group" id="S5Date" runat="server">
                        <asp:Label ID="lblStep5Date" CssClass="lblCss" runat="server"></asp:Label>
                       <asp:TextBox ID="dtAppTargetDate" CssClass="form-control" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                    </div>
                      <div class="form-group">
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

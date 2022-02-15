<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="copy-activitydata.aspx.cs" Inherits="ProjectManagementTool._modal_pages.copy_activitydata" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <script type="text/javascript">
        function ShowProgressBar(status) {
            if (status == 'true') {
                document.getElementById('dvProgressBar').style.visibility = 'visible';
            }
            else {
                document.getElementById('dvProgressBar').style.visibility = 'hidden';
            }
      }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmCopyActivityDataModal" runat="server">
     <div class="container-fluid" style="max-height:82vh; overflow-y:auto;">
            <div class="row">
                <div class="col-sm-12">
                  <div class="table-responsive">
                      <div class="form-group">
                        <label class="lblCss" for="DDLProject">Project</label>
                         <asp:DropDownList ID="DDLProject" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDLProject_SelectedIndexChanged" required >
                         </asp:DropDownList>
                    </div>  
                      <div class="form-group">
                        <label class="lblCss" for="DDLWorkpackage">Copy From</label>
                         <asp:DropDownList ID="DDLWorkpackage" runat="server" CssClass="form-control" required >
                         </asp:DropDownList>
                    </div>  
                             <div class="form-group">
                                <label class="lblCss" for="txtWorkpackagename">Copy To</label> 
                                <asp:TextBox ID="txtWorkpackagename" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                   </div>
                       
                      </div>                     
                </div>
            </div> 

        </div>
        <div id="dvProgressBar" style=" text-align:center; position:relative; visibility:hidden;" >
                     <img src="../_assets/images/progress.gif" width="40" alt="loading"  /> <span style="color:#006699; font-weight:bold;">Copying is in progress...</span>
                     </div> 
        <div class="modal-footer">
                    <%--<button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>--%>
            <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary" Text="Submit" OnClientClick="ShowProgressBar('true');" OnClick="btnAdd_Click"  />
                    <%--<button type="button" class="btn btn-primary">Save changes</button>--%>
                </div>
        
        </form>

</asp:Content>

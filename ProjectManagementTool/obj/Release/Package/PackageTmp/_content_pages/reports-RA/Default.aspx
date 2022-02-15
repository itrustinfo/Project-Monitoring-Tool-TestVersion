<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="Default.aspx.cs" Inherits="ProjectManager._content_pages.reports_RA.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
   <%--  <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
 <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
  <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>--%>
    <script type="text/javascript">
        function CopyMessage() {
            if (confirm("Are you sure you want to Copy ...?")) {
                return true;
            }
            return false;
        }
         function DeleteItem() {
            if (confirm("Are you sure you want to delete ...?")) {
                return true;
            }
            return false;
    }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {

            $(".showBankGuaranteeModal").click(function (e) {
              
                e.preventDefault();
                var url = $(this).attr("href");
              
                $("#ModAddBankGuarantee iframe").attr("src", url);
                $("#ModAddBankGuarantee").modal("show");
            });
            $(".showEditBankGuaranteeModal").click(function (e) {
                e.preventDefault();
                var url = $(this).attr("href");
                $("#ModEditBankGuarantee iframe").attr("src", url);
                $("#ModEditBankGuarantee").modal("show");
            });
           
        });
    </script>

   <%-- <script type="text/javascript">
        $(document).ready(function () {
            $('#loader').fadeOut();
        });
    </script>--%>
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">

        <div class="container-fluid">
            <div class="row">
               <div class="col-lg-12 col-xl-12 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="card-title">
                        <div class="d-flex justify-content-between">
                              <h6 class="text-muted">
                                   <asp:Label ID="ActivityHeading" CssClass="text-uppercase font-weight-bold" runat="server" Text="List of CAA/JICA Claims" />
                                       <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                               </h6>
                                <div>
                                
                                     <a id="AddBankGuarantee" runat="server" href="~/_modal_pages/add-caa-jica-claims.aspx" class="showBankGuaranteeModal"><asp:Button ID="btnaddCAA" runat="server" Text="+ Add  CAA/JICA Claims" CssClass="btn btn-primary"></asp:Button></a>
                                </div>
                          </div>
                            </div>
                        <div class="table-responsive">
                            
                <div class="col-sm-12">
                    
                        <label class="lblCss" for="DDLMeeting">Select Meeting</label>
                        <asp:DropDownList ID="ddlMeeting" runat="server" class="form-control" Width="40%" AutoPostBack="true" OnSelectedIndexChanged="ddlMeeting_SelectedIndexChanged"  ></asp:DropDownList>
                    <br />

                    <asp:Button ID="btncopy" runat="server" Text="Copy Data from Previous Report" OnClientClick="return CopyMessage()" CssClass="btn btn-primary" Visible="false" OnClick="btncopy_Click"/>
                    <br /><br />
                   
                    <asp:GridView ID="gdCP" runat="server" Width="100%" BackColor="White" CssClass="table table-bordered" 
                        BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" AutoGenerateColumns="False" OnRowCommand="gdCP_RowCommand" OnRowDeleting="gdCP_RowDeleting" OnRowDataBound="gdCP_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Project Name">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <%#Eval("Project Name")%>
                                    
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Amount (in MJPY)">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                  <%#Eval("Amount")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Description">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                     <%#Eval("Description")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Remarks">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                     <%#Eval("Remarks")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="Payment Date">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                          <%#Eval("PaymentDate","{0:dd/MM/yyyy}")%>
                                  
                                    </ItemTemplate>

                                </asp:TemplateField>--%>
                               <%--<asp:TemplateField HeaderText="Status">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                  <%#Eval("status")%>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField>
                                        <ItemTemplate>
                                            <a id="EditPhysicalProgress" href="../../_modal_pages/add-caa-jica-claims.aspx?Uid=<%#Eval("Uid")%>&meetingid=<%#Eval("meetingid")%>" class="showEditBankGuaranteeModal"><span title="Edit" class="fas fa-edit"></span></a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                            <asp:TemplateField>
                                  <ItemTemplate>
                                         <asp:LinkButton ID="lnkdelete" runat="server" OnClientClick="return DeleteItem()" CausesValidation="false" CommandArgument='<%#Eval("Uid")%>' CommandName="delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
                                  </ItemTemplate>
                           </asp:TemplateField>
                        </Columns>
                        
                 

                    </asp:GridView>
                    </div>
                </div>
        </div>

        
                    </div>
                   </div>
                </div>
        
            </div>
    <%--add Bank Guarantee modal--%>

    <div id="ModAddBankGuarantee" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add CAA/JICA Claims</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:500px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
    <%--add Bank Guarantee modal--%>
   
      <div id="ModEditBankGuarantee" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit CAA/JICA Claims</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:500px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
    <%--add CAA/Jica --%>
 <%--   <div id="ModAddCaajicaClaims" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Caa/jica Claims</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:500px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>--%>
</asp:Content>

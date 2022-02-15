<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ProjectManagementTool._content_pages.consolidated_activities.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
    
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
                                       <asp:Label ID="ActivityHeading" CssClass="text-uppercase font-weight-bold" runat="server" Text="List of Consolidated Activities" />
                                      
                                   </h6>
                                    <div>
                              
                                         <a id="AddBankGuarantee" runat="server" href="~/_modal_pages/add-conactivity.aspx" class="showBankGuaranteeModal">
                                             <asp:Button ID="btnaddCAA" runat="server" Text="+ Add  Consolidated Activities" CssClass="btn btn-primary"></asp:Button></a>
                                    </div>
                              </div>
                            </div>
                        <div class="table-responsive">
                <div class="col-sm-12">
                    <div style="width:100%; float:left; margin-bottom:20px;">
                        <div style="width:30%; float:left;">
                            <label class="lblCss" for="DDLContractPackage">Select Contract Package</label>
                    <asp:DropDownList ID="DDLContractPackage" runat="server" class="form-control"></asp:DropDownList>
                            </div>
                        <div style="width:5%; float:left;">&nbsp;</div>
                        <div style="width:30%; float:left;">
                            <label class="lblCss" for="DDLMeeting">Select Meeting</label>
                        <asp:DropDownList ID="ddlMeeting" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlMeeting_SelectedIndexChanged"  ></asp:DropDownList>
                            </div>
                        <div style="width:2%; float:left;">&nbsp;</div>
                        <div style="width:10%; float:left; margin-top:30px;">
                            <asp:Button ID="btnSubmit" runat="server" Text="View" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                            </div>
                        </div>
                        
                     <br />

                                           <asp:Button ID="btncopy" runat="server" Text="Copy Data from Previous Report" OnClientClick="return CopyMessage()" CssClass="btn btn-primary" Visible="false" OnClick="btncopy_Click"/>
          
                    <br /><br />
                    <asp:GridView ID="gdConsActivity" runat="server" CssClass="table table-bordered" Width="100%" BackColor="White"
                        EmptyDataText="No Data Found.." 
                        BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Horizontal" AutoGenerateColumns="False" OnRowCommand="gdConsActivity_RowCommand" OnRowDeleting="gdConsActivity_RowDeleting">
                        <Columns>
                             <asp:TemplateField HeaderText="Contract Package">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ProjectName") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("ProjectName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Activity">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Activity") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("Activity") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="status" HeaderText="Status" HtmlEncode="false" />
                            <%--<asp:TemplateField HeaderText="Status" >
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("status") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label3" runat="server" Text='<%# Bind("status")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                           <a id="EditPhysicalProgress" href="../../_modal_pages/add-conactivity.aspx?Uid=  <%#Eval("Uid")%> & meetinguid=<%#Eval("meetingid")%>" class="showEditBankGuaranteeModal"><span title="Edit" class="fas fa-edit"></span></a>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkdelete" runat="server" OnClientClick="return DeleteItem()" CausesValidation="false" CommandArgument='<%#Eval("uid")%>' CommandName="delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>   
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
				    <h5 class="modal-title">Add Consolidated Activities</h5>
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
				    <h5 class="modal-title">Edit Consolidated Activities</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:500px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
</asp:Content>

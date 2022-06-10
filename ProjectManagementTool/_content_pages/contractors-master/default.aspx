<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.contractors._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
      <script type="text/javascript">
        function DeleteItem() {
            if (confirm("Are you sure you want to delete ...?")) {
                return true;
            }
            return false;
    }
    </script>

    <script type="text/javascript">
        function BindEvents() {
       
        $(".showAddContractorModal").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddContractor iframe").attr("src", url);
        $("#ModAddContractor").modal("show");
          });

         $(".showEditContractorModal").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModEditContractor iframe").attr("src", url);
        $("#ModEditContractor").modal("show");
            });
        }
        $(document).ready(function () {
            BindEvents();
            //$('#loader').fadeOut();
        });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
    <%--<div id="loader"></div>--%>
    <div class="container-fluid">
        <div class="row">
            <div class="col-lg-12 col-xl-12 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="card-title">
                        <div class="d-flex justify-content-between">
                              <h6 class="text-muted">
                                   <asp:Label ID="Label1" CssClass="text-uppercase font-weight-bold" runat="server" Text="List of Contractors"  />
                               </h6>
                                <div>
                                   <a href="/_modal_pages/add-contractor.aspx?type=Add" class="showAddContractorModal"><asp:Button ID="btnaddcontractor" runat="server" Text="+ Add Contractor" CssClass="btn btn-primary"></asp:Button></a>
                                </div>
                          </div>
                            </div>
                        <div class="table-responsive">
                            <asp:GridView ID="GrdContractors" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" Width="100%" PageSize="10" OnPageIndexChanging="GrdContractors_PageIndexChanging" OnRowDataBound="GrdContractors_RowDataBound" OnRowCommand="GrdContractors_RowCommand" OnRowDeleting="GrdContractors_RowDeleting">
                               <Columns>             
                                    <asp:TemplateField HeaderText="Name">
                                      <ItemTemplate>
                                              <%#Eval("Contractor_Name")%>
                                      </ItemTemplate>
                                    </asp:TemplateField> 
                                   <asp:TemplateField HeaderText="Code">
                                      <ItemTemplate>
                                              <%#Eval("Contractor_Code")%>
                                      </ItemTemplate>
                                    </asp:TemplateField> 
                                   <asp:TemplateField HeaderText="Type of Contract">
                                      <ItemTemplate>
                                              <%#Eval("Type_of_Contract")%>
                                      </ItemTemplate>
                                    </asp:TemplateField>  
                                   <asp:TemplateField HeaderText="Contract Value (INR in crores)">
                                      <ItemTemplate>
                                          <span style="color:#006699;"><%#Eval("Currency")%></span>&nbsp;<%#string.Format(new System.Globalization.CultureInfo(Eval("Currency_CultureInfo").ToString()),"{0:N}", Eval("Contract_Value"))%>
                                      </ItemTemplate>
                                    </asp:TemplateField> 
                                   <asp:TemplateField HeaderText="Duration">
                                      <ItemTemplate>
                                              <%#Eval("Contract_Duration")%> Months
                                      </ItemTemplate>
                                    </asp:TemplateField>
                                   <asp:BoundField  DataField="NJSEI_Number" HeaderText="NJSEI Project Number"/>
                                   <asp:BoundField  DataField="ProjectSpecific_Number" HeaderText="Project Specific Package Number"/>
                                   <asp:BoundField  DataField="Contract_Agreement_Date" HeaderText="Agreement_Date" DataFormatString="{0:dd MMM yyyy}"/>
                                   <asp:BoundField  DataField="Contract_StartDate" HeaderText="StartDate" DataFormatString="{0:dd MMM yyyy}"/>
                                   <asp:BoundField  DataField="Contract_Completion_Date" HeaderText="Completion Date" DataFormatString="{0:dd MMM yyyy}"/>
                                   <asp:TemplateField>
                                      <ItemTemplate>
                                              <a href='/_modal_pages/add-contractor.aspx?type=edit&ContractID=<%#Eval("Contractor_UID")%>' class="showEditContractorModal"><span title="Edit" class="fas fa-edit"></span></a>
                                      </ItemTemplate>
                                    </asp:TemplateField>
                                   <asp:TemplateField>
                                      <ItemTemplate>
                                              <asp:LinkButton ID="lnkdelete" runat="server" CausesValidation="false" OnClientClick="return DeleteItem()" CommandName="delete" CommandArgument='<%#Eval("Contractor_UID")%>'><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
                                      </ItemTemplate>
                                    </asp:TemplateField>
                               </Columns>
                               <EmptyDataTemplate>
                                  <strong>No Records Found ! </strong>
                                </EmptyDataTemplate>
                            </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

     <%--add Add Contractor modal--%>
    <div id="ModAddContractor" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Contractor</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 500px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--add Edit Contractor modal--%>
    <div id="ModEditContractor" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit Contractor</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 500px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.masters._default" %>
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
         $(".showAddWorkpackageMasterModal").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddWorkpackageMaster iframe").attr("src", url);
        $("#ModAddWorkpackageMaster").modal("show");
              });

        $(".showEditWorkpackageMasterModal").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModEditWorkpackageMaster iframe").attr("src", url);
        $("#ModEditWorkpackageMaster").modal("show");
              });

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

            $(".showAddProjectModal").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddProject iframe").attr("src", url);
        $("#ModAddProject").modal("show");
          });

         $(".showEditProjectModal").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModEditProject iframe").attr("src", url);
        $("#ModEditProject").modal("show");
            });

            $(".showAddLocationModal").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddLocation iframe").attr("src", url);
        $("#ModAddLocation").modal("show");
          });

         $(".showEditLocationModal").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModEditLocation iframe").attr("src", url);
        $("#ModEditLocation").modal("show");
            });

             $(".showAddClientModal").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddClient iframe").attr("src", url);
        $("#ModAddClient").modal("show");
          });

         $(".showEditClientModal").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModEditClient iframe").attr("src", url);
        $("#ModEditClient").modal("show");
            });

            $(".showAddProjectClassModal").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddProjectClass iframe").attr("src", url);
        $("#ModAddProjectClass").modal("show");
            });

            $(".showEditProjectClassModal").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModEditProjectClass iframe").attr("src", url);
        $("#ModEditProjectClass").modal("show");
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
    <%--<div class="container-fluid">
        <div class="row">
            <div class="col-lg-12 col-xl-12 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="card-title">
                        <div class="d-flex justify-content-between">
                              <h6 class="text-muted">
                                   <asp:Label ID="Label2" CssClass="text-uppercase font-weight-bold" runat="server" Text="List of Projects"  />
                               </h6>
                                <div>
                                   <a href="/_modal_pages/add-project.aspx?type=Add" class="showAddProjectModal"><asp:Button ID="btnaddproject" runat="server" Text="+ Add Project" CssClass="btn btn-primary"></asp:Button></a>
                                </div>
                          </div>
                            </div>
                        <div class="table-responsive">
                            <asp:GridView ID="GrdProject" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" Width="100%" PageSize="10" OnPageIndexChanging="GrdProject_PageIndexChanging"  >
                               <Columns>             
                                    <asp:TemplateField HeaderText="Project Name">
                                      <ItemTemplate>
                                              <%#Eval("ProjectName")%>
                                      </ItemTemplate>
                                    </asp:TemplateField> 
                                   <asp:TemplateField HeaderText="Project Code">
                                      <ItemTemplate>
                                              <%#Eval("ProjectAbbrevation")%>
                                      </ItemTemplate>
                                    </asp:TemplateField> 
                                   <asp:TemplateField HeaderText="Funding Agency">
                                      <ItemTemplate>
                                              <%#Eval("Funding_Agency")%>
                                      </ItemTemplate>
                                    </asp:TemplateField> 
                                   <asp:TemplateField HeaderText="Budget">
                                  <ItemTemplate>
                                      <span style="color:#006699;"><%#Eval("Currency")%></span>&nbsp;<%#string.Format(new System.Globalization.CultureInfo(Eval("Currency_CultureInfo").ToString()),"{0:N}", Eval("Budget"))%>
                                  </ItemTemplate>
                              </asp:TemplateField>
                              <asp:TemplateField HeaderText="ActualExpenditure">
                                  <ItemTemplate>
                                      <span style="color:#006699;"><%#Eval("Currency").ToString()%></span>&nbsp;<%#string.Format(new System.Globalization.CultureInfo(Eval("Currency_CultureInfo").ToString()),"{0:N}", Eval("ActualExpenditure"))%>
                                  </ItemTemplate>
                              </asp:TemplateField>
                                   <asp:BoundField DataField="StartDate" HeaderText="Start Date" DataFormatString="{0:dd MMM yyyy}">
                                  <HeaderStyle HorizontalAlign="Left" />
                              </asp:BoundField>
                              <asp:BoundField DataField="PlannedEndDate" HeaderText="Planned EndDate" DataFormatString="{0:dd MMM yyyy}">
                                  <HeaderStyle HorizontalAlign="Left" />
                              </asp:BoundField>
                                   <asp:TemplateField>
                                      <ItemTemplate>
                                          <a href='/_modal_pages/add-project.aspx?type=edit&ProjectUID=<%#Eval("ProjectUID")%>' class="showEditProjectModal"><span title="Edit" class="fas fa-edit"></span></a>
                                             
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
                            <asp:GridView ID="GrdContractors" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" Width="100%" PageSize="10" OnPageIndexChanging="GrdContractors_PageIndexChanging" OnRowCommand="GrdContractors_RowCommand" OnRowDeleting="GrdContractors_RowDeleting">
                               <Columns>             
                                    <asp:TemplateField HeaderText="Name">
                                      <ItemTemplate>
                                              <%#Eval("Contractor_Name")%>
                                      </ItemTemplate>
                                    </asp:TemplateField> <asp:TemplateField HeaderText="Type of Contract">
                                      <ItemTemplate>
                                              <%#Eval("Type_of_Contract")%>
                                      </ItemTemplate>
                                    </asp:TemplateField>  
                                   <asp:TemplateField HeaderText="Value">
                                      <ItemTemplate>
                                          <span style="color:#006699;"><%#Eval("Currency")%></span>&nbsp;<%#string.Format(new System.Globalization.CultureInfo(Eval("Currency_CultureInfo").ToString()),"{0:N}", Eval("Contract_Value"))%>
                                      </ItemTemplate>
                                    </asp:TemplateField> 
                                   <asp:TemplateField HeaderText="Duration">
                                      <ItemTemplate>
                                              <%#Eval("Contract_Duration")%> Months
                                      </ItemTemplate>
                                    </asp:TemplateField>  
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
        </div>--%>


    <div class="container-fluid">
        <div class="row">
            <div class="col-md-6 col-lg-6 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="card-title">
                        <div class="d-flex justify-content-between">
                              <h6 class="text-muted">
                                   <asp:Label ID="LblPwrkPackage" CssClass="text-uppercase font-weight-bold" runat="server" Text="List of Workpackage Masters"  />
                               </h6>
                                <div>
                                   <a href="/_modal_pages/add-workpackagemaster.aspx?type=Add" class="showAddWorkpackageMasterModal"><asp:Button ID="BtnAddWorkpackagemaster" runat="server" Text="+ Add Workpackage Master" CssClass="btn btn-primary"></asp:Button></a>
                                </div>
                          </div>
                            </div>
                        <div class="table-responsive">
                            <asp:GridView ID="grdMasters" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" Width="100%" PageSize="10" OnPageIndexChanging="grdMasters_PageIndexChanging" OnRowCommand="grdMasters_RowCommand" OnRowDeleting="grdMasters_RowDeleting" >
                               <Columns>   
                                   <asp:TemplateField HeaderText="Project">
                                      <ItemTemplate>
                                              <%#GetProjectName(Eval("ProjectUID").ToString())%>
                                      </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Workpackage Name">
                                      <ItemTemplate>
                                              <%#Eval("MasterWorkPackageName")%>
                                      </ItemTemplate>
                                    </asp:TemplateField> <asp:TemplateField HeaderText="Workpackage Code">
                                      <ItemTemplate>
                                              <%#Eval("MasterWorkPackageCode")%>
                                      </ItemTemplate>
                                    </asp:TemplateField>  
                                   <asp:TemplateField>
                                      <ItemTemplate>
                                          <a href='/_modal_pages/add-workpackagemaster.aspx?type=edit&wmUID=<%#Eval("MasterWorkPackageUID")%>' class="showEditWorkpackageMasterModal"><span title="Edit" class="fas fa-edit"></span></a>
                                              <%--<asp:LinkButton ID="lnkedit" runat="server" CausesValidation="false" CommandName="edit" CommandArgument='<%#Eval("MasterWorkPackageUID")%>'><span title="Edit" class="fas fa-edit"></span></asp:LinkButton>--%>
                                      </ItemTemplate>
                                    </asp:TemplateField>
                                   <asp:TemplateField>
                                      <ItemTemplate>
                                              <asp:LinkButton ID="lnkdelete" runat="server" CausesValidation="false" OnClientClick="return DeleteItem()" CommandName="delete" CommandArgument='<%#Eval("MasterWorkPackageUID")%>'><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
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
            <div class="col-md-6 col-lg-6 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="card-title">
                        <div class="d-flex justify-content-between">
                              <h6 class="text-muted">
                                   <asp:Label ID="Label3" CssClass="text-uppercase font-weight-bold" runat="server" Text="List of Location Masters"  />
                               </h6>
                                <div>
                                   <a href="/_modal_pages/add-locationmaster.aspx?type=Add" class="showAddLocationModal"><asp:Button ID="Button1" runat="server" Text="+ Add Location Master" CssClass="btn btn-primary"></asp:Button></a>
                                </div>
                          </div>
                            </div>
                        <div class="table-responsive">
                            <asp:GridView ID="GrdLocationMaster" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" Width="100%" PageSize="10" OnPageIndexChanging="GrdLocationMaster_PageIndexChanging" OnRowCommand="GrdLocationMaster_RowCommand" OnRowDeleting="GrdLocationMaster_RowDeleting" >
                               <Columns>
                                   <asp:TemplateField HeaderText="Project">
                                      <ItemTemplate>
                                              <%#GetProjectName(Eval("ProjectUID").ToString())%>
                                      </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Location Name">
                                      <ItemTemplate>
                                              <%#Eval("LocationMaster_Name")%>
                                      </ItemTemplate>
                                    </asp:TemplateField> <asp:TemplateField HeaderText="Location Code">
                                      <ItemTemplate>
                                              <%#Eval("LocationMaster_Code")%>
                                      </ItemTemplate>
                                    </asp:TemplateField>  
                                   <asp:TemplateField>
                                      <ItemTemplate>
                                          <a href='/_modal_pages/add-locationmaster.aspx?type=edit&LocationUID=<%#Eval("LocationMasterUID")%>' class="showEditLocationModal"><span title="Edit" class="fas fa-edit"></span></a>
                                              <%--<asp:LinkButton ID="lnkedit" runat="server" CausesValidation="false" CommandName="edit" CommandArgument='<%#Eval("MasterWorkPackageUID")%>'><span title="Edit" class="fas fa-edit"></span></asp:LinkButton>--%>
                                      </ItemTemplate>
                                    </asp:TemplateField>
                                   <asp:TemplateField>
                                      <ItemTemplate>
                                              <asp:LinkButton ID="lnkdelete" runat="server" CausesValidation="false" OnClientClick="return DeleteItem()" CommandName="delete" CommandArgument='<%#Eval("LocationMasterUID")%>'><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
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
    
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-6 col-lg-6 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="card-title">
                        <div class="d-flex justify-content-between">
                              <h6 class="text-muted">
                                   <asp:Label ID="Label4" CssClass="text-uppercase font-weight-bold" runat="server" Text="List of Client Masters"  />
                               </h6>
                                <div>
                                   <a href="/_modal_pages/add-clientmaster.aspx?type=Add" class="showAddClientModal"><asp:Button ID="Button2" runat="server" Text="+ Add Client Master" CssClass="btn btn-primary"></asp:Button></a>
                                </div>
                          </div>
                            </div>
                        <div class="table-responsive">
                            <asp:GridView ID="GrdClient" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" Width="100%" PageSize="10" OnPageIndexChanging="GrdClient_PageIndexChanging" OnRowCommand="GrdClient_RowCommand" OnRowDeleting="GrdClient_RowDeleting" >
                               <Columns>
                                   <asp:TemplateField HeaderText="Project">
                                      <ItemTemplate>
                                              <%#GetProjectName(Eval("ProjectUID").ToString())%>
                                      </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Client Name">
                                      <ItemTemplate>
                                              <%#Eval("ClientMaster_Name")%>
                                      </ItemTemplate>
                                    </asp:TemplateField> <asp:TemplateField HeaderText="Client Code">
                                      <ItemTemplate>
                                              <%#Eval("ClientMaster_Code")%>
                                      </ItemTemplate>
                                    </asp:TemplateField>  
                                   <asp:TemplateField>
                                      <ItemTemplate>
                                          <a href='/_modal_pages/add-clientmaster.aspx?type=edit&ClientUID=<%#Eval("ClientMasterUID")%>' class="showEditClientModal"><span title="Edit" class="fas fa-edit"></span></a>
                                              <%--<asp:LinkButton ID="lnkedit" runat="server" CausesValidation="false" CommandName="edit" CommandArgument='<%#Eval("MasterWorkPackageUID")%>'><span title="Edit" class="fas fa-edit"></span></asp:LinkButton>--%>
                                      </ItemTemplate>
                                    </asp:TemplateField>
                                   <asp:TemplateField>
                                      <ItemTemplate>
                                              <asp:LinkButton ID="lnkdelete" runat="server" CausesValidation="false" OnClientClick="return DeleteItem()" CommandName="delete" CommandArgument='<%#Eval("ClientMasterUID")%>'><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
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
            <div class="col-md-6 col-lg-6 form-group">
                <%--<div class="card mb-4">
                    <div class="card-body">
                        <div class="card-title">
                        <div class="d-flex justify-content-between">
                              <h6 class="text-muted">
                                   <asp:Label ID="Label5" CssClass="text-uppercase font-weight-bold" runat="server" Text="List of Project Category"  />
                               </h6>
                                <div>
                                   <a href="/_modal_pages/add-projectclass.aspx?type=Add" class="showAddProjectClassModal"><asp:Button ID="btnAddClass" runat="server" Text="+ Add Project Category" CssClass="btn btn-primary"></asp:Button></a>
                                </div>
                          </div>
                            </div>
                        <div class="table-responsive">
                            <asp:GridView ID="GrdProjectClass" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" Width="100%" PageSize="10" OnPageIndexChanging="GrdProjectClass_PageIndexChanging">
                               <Columns>             
                                    <asp:TemplateField HeaderText="Project Category">
                                      <ItemTemplate>
                                              <%#Eval("ProjectClass_Name")%>
                                      </ItemTemplate>
                                    </asp:TemplateField> <asp:TemplateField HeaderText="Description">
                                      <ItemTemplate>
                                              <%#Eval("ProjectClass_Description")%>
                                      </ItemTemplate>
                                    </asp:TemplateField>  
                                   <asp:TemplateField>
                                      <ItemTemplate>
                                          <a href='/_modal_pages/add-projectclass.aspx?type=edit&pClassUID=<%#Eval("ProjectClass_UID")%>' class="showEditProjectClassModal"><span title="Edit" class="fas fa-edit"></span></a>
                                              
                                      </ItemTemplate>
                                    </asp:TemplateField>
                                 
                               </Columns>
                               <EmptyDataTemplate>
                                  <strong>No Records Found ! </strong>
                                </EmptyDataTemplate>
                            </asp:GridView>
                            </div>
                        </div>
                    </div>--%>
                </div>
            </div>
        </div>

     <%--add Add Workpackge Master modal--%>
    <div id="ModAddWorkpackageMaster" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Work Package Master</h5>
                    <button aria-label="Close" class="close" onclick="javascript:window.location.reload()" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:400px;" loading="lazy"></iframe>
			    </div>
		    </div>
	    </div>
    </div>

      <%--add Edit Workpackge Master modal--%>
    <div id="ModEditWorkpackageMaster" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit Work Package Master</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:400px;" loading="lazy"></iframe>
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
                    <iframe class="border-0 w-100" style="height: 450px;" loading="lazy"></iframe>
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
                    <iframe class="border-0 w-100" style="height: 450px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--add Add Project modal--%>
    <div id="ModAddProject" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Project</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 450px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--add Edit Prject modal--%>
    <div id="ModEditProject" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit Project</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 450px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--add Add Location modal--%>
    <div id="ModAddLocation" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Location</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 400px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--add Edit Location modal--%>
    <div id="ModEditLocation" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit Location</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 400px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--add Add Client modal--%>
    <div id="ModAddClient" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Client</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 400px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--add Edit Client modal--%>
    <div id="ModEditClient" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit Client</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 400px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--add Project Class modal--%>
    <div id="ModAddProjectClass" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Project Category</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:300px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--add Edit Project Class modal--%>
    <div id="ModEditProjectClass" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit Project Category</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:300px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
</asp:Content>

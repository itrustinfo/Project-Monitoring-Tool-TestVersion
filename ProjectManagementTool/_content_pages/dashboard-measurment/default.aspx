<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.dashboard_measurment._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
     <script type="text/javascript">
         function DeleteItem() {
            if (confirm("Are you sure you want to delete ...?")) {
                return true;
            }
            return false;
        }
    </script>
    <style type="text/css">
        .hiddencol { display: none; }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".showEditMeasurementModel").click(function (e) {
                e.preventDefault();
                jQuery.noConflict();
                var url = $(this).attr("href");
                $("#ModEditMeasurement iframe").attr("src", url);
                $("#ModEditMeasurement").modal("show");
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
                                   <asp:Label ID="LblDocumentHeading" Text="Mesaurments List for Contractor -> ONTB" CssClass="text-uppercase font-weight-bold" runat="server" />
                                       <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                               </h6>
                               <div class="col-md-6 col-lg-4 form-group text-right">
                    <asp:Button ID="btnback" runat="server" Text="Back To Dashboard" CssClass="btn btn-primary" PostBackUrl="/_content_pages/dashboard/"></asp:Button>
                                  
                </div>
                          </div>
                            </div>
                        <div class="table-responsive" id="divMain" runat="server">
                             <asp:Label runat="server" Text="" ID="lblTotalcount"></asp:Label>
                    <asp:GridView ID="grdMeasurementbook" runat="server" CssClass="table table-bordered" EmptyDataText="No Data" AutoGenerateColumns="False" Width="100%" OnRowDataBound="grdMeasurementbook_RowDataBound">
                                                    <Columns>
                                                          <asp:TemplateField HeaderText="Task Name">
                                                            <ItemTemplate>
                                                                <%#GetTaskName(Eval("ParentTaskID").ToString())%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Activity Name">
                                                            <ItemTemplate>
                                                                <%#GetTaskName(Eval("TaskUID").ToString())%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:BoundField DataField="UnitforProgress" HeaderText="Unit for Progress">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Quantity" HeaderText="Quantity" >
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Remarks" HeaderText="Remarks">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CreatedDate" HeaderText="Created Date"  DataFormatString="{0:dd/MM/yyyy}">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                           
                                                       
                                                        <asp:BoundField DataField="Achieved_Date" HeaderText="Achieved Date"  DataFormatString="{0:dd/MM/yyyy}">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                      <%--  <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <a id="EditMeasurement" href="/_modal_pages/edit-measurement.aspx?UID=<%#Eval("UID")%>" class="showEditMeasurementModel"><span title="Edit" class="fas fa-edit"></span></a> 
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                        <asp:TemplateField>   
                                                            <ItemTemplate>
                                                                   <asp:LinkButton ID="lnkdelete" runat="server" CausesValidation="false" OnClientClick="return DeleteItem()" CommandArgument='<%#Eval("UID")%>' CommandName="delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
                                                             </ItemTemplate>
                                                         </asp:TemplateField>--%>
                                                    </Columns>
                                                 </asp:GridView>
                            </div>
                    </div>
                </div>
                 </div>
            </div>
              </div>
        <div id="ModEditMeasurement" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit Measurement</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:450px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

        
</asp:Content>

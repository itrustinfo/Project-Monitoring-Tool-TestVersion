<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="view-Finpayments.aspx.cs" Inherits="ProjectManagementTool._modal_pages.view_Finpayments" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <style>
        .Hide
            {
                display : none;
            }
    </style>
      <script type="text/javascript">
        $(document).ready(function () {
            $(".showStatusModal").click(function (e) {
                e.preventDefault();
                jQuery.noConflict();
                var url = $(this).attr("href");
                $("#ModAddDocumentStatus iframe").attr("src", url);
                $("#ModAddDocumentStatus").modal("show");
            });

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddDocumentModal" runat="server">
 
    <div>
                                   <a id="AddPayments" runat="server" href="#" class="showStatusModal"><asp:Button ID="Button2" runat="server" Text="+ Add" CssClass="btn btn-primary"></asp:Button></a>
                                </div><br />

           <div class="container-fluid">
            <div class="row">
                <div class="col-lg-12 col-xl-12 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="table-responsive">
                            <h5 id="heading" runat="server">List of Bill Payments</h5>
                                <asp:GridView ID="GrdRABills" runat="server" HeaderStyle-HorizontalAlign="Center" CssClass="table table-bordered" EmptyDataText="No Data" AutoGenerateColumns="False" Width="100%" AlternatingRowStyle-BackColor="lightGray">
                                <Columns>
                                      <asp:BoundField DataField="PaymentUID" ItemStyle-HorizontalAlign="Center" HeaderText="PaymentUID" HeaderStyle-CssClass="Hide" ItemStyle-CssClass="Hide">
                            
                            </asp:BoundField>
                                     <asp:BoundField DataField="Invoice_Number" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="150px" HeaderText="Invoice" HtmlEncode="false">
                            
                            </asp:BoundField>
                                    <%-- <asp:BoundField DataField="RABillDesc" ItemStyle-HorizontalAlign="Center" HeaderText="RABillDesc" HtmlEncode="False">
                            
                            </asp:BoundField>--%>
                                     <asp:BoundField DataField="Amount" ItemStyle-HorizontalAlign="Right" HeaderText="Amount (Rs)" HtmlEncode="False" DataFormatString="{0:N2}">
                            
                            </asp:BoundField>
                                     <asp:BoundField DataField="TotalDeductions"  HeaderText="TotalDeductions (Rs)" ItemStyle-HorizontalAlign="Right" HtmlEncode="False" DataFormatString="{0:N2}">
                            
                            </asp:BoundField>
                                    <asp:BoundField DataField="NetAmount"  HeaderText="NetAmount (Rs)" ItemStyle-HorizontalAlign="Right" HtmlEncode="False" DataFormatString="{0:N2}">
                            
                            </asp:BoundField>
                                       <asp:BoundField DataField="PaymentDate"  HeaderText="PaymentDate" ItemStyle-HorizontalAlign="Right" HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy}">
                            
                            </asp:BoundField>
                                       <asp:TemplateField>
                                        <ItemTemplate>
                                            <a id="Edit" href="/_modal_pages//add-RABillPayments.aspx?PaymentUID=<%#Eval("PaymentUID")%>&monthUID=<%#Eval("FinMileStoneMonthUID")%>&WkpgUID=<%#Eval("WorkpackageUID")%>&Invoice=<%#Eval("Invoice_Number")%>&type=edit" class="showStatusModal"><span title="Edit" class="fas fa-edit"></span></a>
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
     <%--View document status modal--%>
    <div id="ModAddDocumentStatus" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Payments</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:600px;width:300px" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
        </form>
</asp:Content>

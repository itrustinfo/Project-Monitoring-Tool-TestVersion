<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.invoice._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
     <style type="text/css">
        .hiddencol { display: none; }
    </style>
    <script type="text/javascript">
        function DeleteItem() {
            if (confirm("Are you sure you want to delete ...?")) {
                return true;
            }
            return false;
        }

             function BindEvents() {
                 $(".showAddInvoiceModal").click(function(e) {
                     e.preventDefault();                 
                var url = $(this).attr("href");
                $("#ModAddInvoice iframe").attr("src", url);
                $("#ModAddInvoice").modal("show");
                 });

                 $(".showEditInvoiceModal").click(function(e) {
                     e.preventDefault();                 
                var url = $(this).attr("href");
                $("#ModEditInvoice iframe").attr("src", url);
                $("#ModEditInvoice").modal("show");
                 });

                 $(".showModalAddInvoiceDeduction").click(function(e) {
                     e.preventDefault();                 
                var url = $(this).attr("href");
                $("#ModAddInvoiceDeductions iframe").attr("src", url);
                $("#ModAddInvoiceDeductions").modal("show");
                 });

                 $(".showModalEditInvoiceDeduction").click(function(e) {
                     e.preventDefault();                 
                var url = $(this).attr("href");
                $("#ModEditInvoiceDeductions iframe").attr("src", url);
                $("#ModEditInvoiceDeductions").modal("show");
                 });

                 $(".showModalAddRABill").click(function(e) {
                     e.preventDefault();                 
                var url = $(this).attr("href");
                $("#ModAddRABill iframe").attr("src", url);
                $("#ModAddRABill").modal("show");
                 });

                 $(".showModalRABills").click(function(e) {
                     e.preventDefault();                 
                var url = $(this).attr("href");
                $("#ModShowRABill iframe").attr("src", url);
                $("#ModShowRABill").modal("show");
                });
             }
            $(document).ready(function () {
                BindEvents();
               });
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server">
   </asp:ScriptManager>
     <div class="container-fluid">
            <div class="row">
                <div class="col-md-12 col-lg-8 form-group">Invoice</div>
                <div class="col-md-6 col-lg-4 form-group">
                    <label class="sr-only" for="DDLProject">Project</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Project</span>
                        </div>
                        <asp:DropDownList ID="DDlProject" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDlProject_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
    <div class="container-fluid">
<%--        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>--%>
                    <div class="row">
                        <div class="col-lg-6 col-xl-3 form-group">
                            <div class="card h-100" style="max-height:700px; overflow-y:auto;">
                                <div class="card-body">
                                    <h6 class="card-title text-muted text-uppercase font-weight-bold">Invoice List</h6>
                                    <%--<div class="form-group">
                                        <label class="sr-only" for="TxtSearchDocuments">Search</label>
                                        <div class="input-group">
                                            <input id="TxtSearchDocuments" class="form-control" type="text" placeholder="Activity name" />
                                            <div class="input-group-append">
                                                <asp:Button ID="BtnSearchDocuments" CssClass="btn btn-primary" Text="Search" runat="server" />
                                            </div>
                                        </div>
                                    </div>--%>
                                    <asp:TreeView runat="server" CssClass="it_tree_view" ID="TreeView1" ImageSet="XPFileExplorer" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged" NodeIndent="15" EnableTheming="True" NodeWrap="True">                               
                                        <NodeStyle CssClass="it_tree_view__node" NodeSpacing="2px" />
                                        <ParentNodeStyle Font-Bold="False" />
                                        <SelectedNodeStyle CssClass="it_tree_view__node__selected" HorizontalPadding="4px" VerticalPadding="2px" />
                                    </asp:TreeView>    
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6 col-xl-9 form-group">
                            <div class="card mb-4" id="InvoiceDiv" runat="server" visible="false">
                                    <div class="card-body">
                                        <div class="card-title">
                                            <div class="d-flex justify-content-between">
                                                <h6 class="text-muted">
                                                    <asp:Label ID="ActivityHeading" CssClass="text-uppercase font-weight-bold" runat="server" />
                                                </h6>
                                                <div>
                                                    <asp:Button ID="btnback" runat="server" Text="Back To Dashboard" CssClass="btn btn-primary" PostBackUrl="/_content_pages/dashboard/" Visible="false"></asp:Button>
                                                   <a id="AddInvoice" runat="server" href="/_modal_pages/add-invoicemaster.aspx" class="showAddInvoiceModal"><asp:Button ID="Button2" runat="server" Text="+ Add Invoice" CssClass="btn btn-primary"></asp:Button></a>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="table-responsive">
                                             <table class="table table-borderless">
                                                 <tr>
                                                      <td>Total Amount</td><td>:</td><td>
                                                     <asp:Label ID="LblAllInvoiceTotal" ForeColor="#006699" runat="server"></asp:Label>
                                                     </td>
                                                     <td>Total Deduction Amount</td><td>:</td><td>
                                                     <asp:Label ID="LblAllInvoiceDeductionTotal" ForeColor="#006699" runat="server"></asp:Label>
                                                     </td>
                                                 </tr>
                                                 <tr>
                                                     <td>Net Amount</td><td>:</td><td>
                                                     <asp:Label ID="LblAllInvoiceNetTotal" ForeColor="#006699" runat="server"></asp:Label>
                                                     </td>
                                                 </tr>
                                               </table>
                                                
                                            </div>
                                        <div class="table-responsive">
                                                <asp:GridView ID="GrdInvoice" runat="server" AutoGenerateColumns="False" EmptyDataText="No Data Found." AllowPaging="true" PageSize="20" Width="100%" CssClass="table table-bordered" OnRowDataBound="GrdInvoice_RowDataBound" OnPageIndexChanging="GrdInvoice_PageIndexChanging" OnRowCommand="GrdInvoice_RowCommand" OnRowDeleting="GrdInvoice_RowDeleting">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sl.No" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <%# Container.DataItemIndex + 1 %>
                                                            </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Invoice_Number" HeaderText="Invoice Number">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Invoice_Desc" HeaderText="Invoice Description" >
                                                <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Invoice_Date" HeaderText="Invoice Date" DataFormatString="{0:dd MMM yyyy}">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <a id="EditInvoice" href="/_modal_pages/add-invoicemaster.aspx?InvoiceMaster_UID=<%#Eval("InvoiceMaster_UID")%>&PrjUID=<%#Eval("ProjectUID")%>&WorkUID=<%#Eval("WorkpackageUID")%>"  class="showEditInvoiceModal"><span title="Edit" class="fas fa-edit"></span></a>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkdelete" runat="server" OnClientClick="return DeleteItem()" CausesValidation="false" CommandArgument='<%#Eval("InvoiceMaster_UID")%>' CommandName="delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                         <asp:BoundField DataField="InvoiceMaster_UID" HeaderText="InvoiceMaster_UID" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                            <div class="card mb-4" id="InvoiceDetails" runat="server" visible="false">
                                <div class="card-body">
                                     <div class="card-title">
                                                <div class="d-flex justify-content-between">
                                                    <h6 class="text-muted">
                                                        <asp:Label Text="Invoice Details" CssClass="text-uppercase font-weight-bold" runat="server" />                                                        
                                                    </h6>
                                                    <div>
                                                        <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btn btn-primary" Width="70px" OnClick="btnPrint_Click"/>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="table-responsive">
                                             <table class="table table-borderless">
                                                 <tr><td>Invoice Number</td><td>:</td><td>
                                                   <asp:Label ID="LblInvoiceNumber" runat="server" Font-Bold="true"></asp:Label>
                                                     </td><td>Invoice Date</td><td>:</td><td colspan="2">
                                                     <asp:Label ID="LblInvoiceDate" runat="server"></asp:Label>
                                                     </td>
                                                 </tr>
                                                 <tr>
                                                      <td>Gross Amount</td><td>:</td><td>
                                                     <asp:Label ID="LblInvoiceTotalAmount" ForeColor="#006699" runat="server"></asp:Label>
                                                     </td>
                                                     <td>Deduction Amount</td><td>:</td><td>
                                                     <asp:Label ID="LblInvoiceDeductionAmount" ForeColor="#006699" runat="server"></asp:Label>
                                                     </td>
                                                     <td>Net Amount</td><td>:</td><td>
                                                     <asp:Label ID="LblNetAmount" ForeColor="#006699" runat="server"></asp:Label>
                                                     </td>
                                                 </tr>
                                                 <tr>
                                                     <td>Up to Prev. Bill Gross</td><td>:</td><td>
                                                     <asp:Label ID="LblUptoPrevTotal" ForeColor="#006699" runat="server"></asp:Label>
                                                     </td>
                                                     <td>Up to Prev. Bill Deduction</td><td>:</td><td>
                                                     <asp:Label ID="LblUptoPrevDeeductionTotal" ForeColor="#006699" runat="server"></asp:Label>
                                                     </td>
                                                     <td>Up to Prev. Bill Net</td><td>:</td><td>
                                                     <asp:Label ID="LblUptoPrevNetTotal" ForeColor="#006699" runat="server"></asp:Label>
                                                     </td>
                                                 </tr>
                                               </table>
                                                
                                            </div>
                                </div>
                            </div>
                            <div class="card mb-4" id="RABillsDiv" runat="server" visible="false">
                                        <div class="card-body">
                                           
                                            <div class="card-title">
                                                <div class="d-flex justify-content-between">
                                            <h6 class="text-muted">
                                                        <asp:Label Text="RA Bills" CssClass="text-uppercase font-weight-bold" runat="server" />                                                        
                                                    </h6> 
                                                <div>
                                                        <asp:Button ID="btnRABillPrint" runat="server" Text="Print" Visible="false" CssClass="btn btn-primary" Width="70px" OnClick="btnRABillPrint_Click"/>
                                                        <a id="AddRAbill" runat="server" href="/_modal_pages/add-rabill-rabillitem-invoice.aspx" class="showModalAddRABill"><asp:Button ID="btnAddRABill" runat="server" Text="+ Add RA Bill" CssClass="btn btn-primary"></asp:Button></a>
                                                    </div>
                                                    </div>
                                                </div>
                                            <div class="table-responsive">
                                                
                                            <asp:GridView ID="GrdRABillItems" runat="server" AutoGenerateColumns="false" PageSize="20" 
                                        AllowPaging="true" CssClass="table table-bordered" DataKeyNames="RABillUid" EmptyDataText="No Data" OnRowDataBound="GrdRABillItems_RowDataBound"
                                       Width="100%">
                                        <Columns>
                                            <asp:TemplateField HeaderText="RA Bill No.">
                                                <ItemTemplate>
                                                      <a id="RABillItems" href="/_modal_pages/show-RABills.aspx?RABillUid=<%#Eval("RABillUid")%>&WorkpackageUID=<%#Eval("WorkpackageUID")%>" class="showModalRABills">
                                                         <%#Eval("RABillNumber")%></a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                             <asp:TemplateField>
                                                  <HeaderTemplate>
                                                      Bill Value
                                                  </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%--<%#Eval("Bill_Value")%>--%>
                                                    <asp:Label ID="LblBillValue" ForeColor="#006699" runat="server"></asp:Label>
                                                    <asp:Label ID="LblEnteredRABillValue" runat="server" Text='<%#Eval("RABill_Amount")%>' CssClass="hiddencol"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="InvoiceRABill_Date" HeaderText="RA Bill Date" DataFormatString="{0:dd MMM yyyy}">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                            <asp:BoundField DataField="InvoiceRABill_UID" HeaderText="InvoiceRABill_UID" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                             <%--<asp:TemplateField>
                                                   <HeaderTemplate>
                                                     Net Amount
                                                  </HeaderTemplate>
                                                <ItemTemplate>
                                                   <%#Eval("Net_Amount")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                              <%--<asp:TemplateField >
                                                  <HeaderTemplate>
                                                    Recoveries
                                                  </HeaderTemplate>
                                                <ItemTemplate>
                                                   <%#Eval("Recoveries")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                        </Columns>
                                        <EmptyDataTemplate>
                                                    <strong>No Records Found ! </strong>
                                                </EmptyDataTemplate>
                                        </asp:GridView>
                                                </div>
                                            </div>
                                        </div>

                           <div class="card mb-4" id="InvoiceDeductions" runat="server" visible="false">
                                        <div class="card-body">

                                            <div class="card-title">
                                                <div class="d-flex justify-content-between">
                                                    <h6 class="text-muted">
                                                        <asp:Label Text="Invoice Deductions" CssClass="text-uppercase font-weight-bold" runat="server" />                                                        
                                                    </h6>
                                                    <div>
                                                        <a id="AddInvoiceDeductions" runat="server" href="/_modal_pages/add-invoicededuction.aspx" class="showModalAddInvoiceDeduction"><asp:Button ID="btnAddInvoiceDeductions" runat="server" Text="+ Add Deduction" CssClass="btn btn-primary"></asp:Button></a>
                                                    </div>
                                                </div>
                                            </div>
                                            
                                            <div class="table-responsive">
                                            <asp:GridView ID="GrdInvoiceDeductions" runat="server" AllowPaging="false" CssClass="table table-bordered" AutoGenerateColumns="False" Width="100%" OnRowDataBound="GrdInvoiceDeductions_RowDataBound" OnRowCommand="GrdInvoiceDeductions_RowCommand" OnRowDeleting="GrdInvoiceDeductions_RowDeleting">
                                                    <Columns>
                                                         <%--<asp:TemplateField HeaderText="Invoice Number">
                                                            <ItemTemplate>
                                                                 <%#GetInvoiceNumber(Eval("InvoiceMaster_UID").ToString())%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>
                                                        <asp:TemplateField HeaderText="Deduction Item">
                                                            <ItemTemplate>
                                                                 <%#GetDeductionMaster(Eval("Deduction_UID").ToString())%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Amount" ItemStyle-ForeColor="#006699">
                                                           <ItemTemplate>
                                                               <span style="color:#006699;"><%#Eval("Currency")%></span>&nbsp;<%#string.Format(new System.Globalization.CultureInfo(Eval("Currency_CultureInfo").ToString()),"{0:N}", Eval("Amount"))%>
                                                           </ItemTemplate>
                                                       </asp:TemplateField> 
                                                        <asp:TemplateField HeaderText="Percentage">
                                                            <ItemTemplate>
                                                                 <%#Eval("Percentage")%>%
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField>
                                                        <ItemTemplate>
                                                             <a id="EditInvoiceDeduction"  href="/_modal_pages/add-invoicededuction.aspx?Invoice_DeductionUID=<%#Eval("Invoice_DeductionUID")%>&InvoiceMaster_UID=<%#Eval("InvoiceMaster_UID")%>&WorkUID=<%#Eval("WorkpackageUID")%>" class="showModalEditInvoiceDeduction"><span title="Edit" class="fas fa-edit"></span></a>
                                                        </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField>
                                                            <ItemTemplate>
                                                                   <asp:LinkButton ID="lnkdelete" runat="server" CausesValidation="false" OnClientClick="return DeleteItem()" CommandArgument='<%#Eval("Invoice_DeductionUID")%>' CommandName="delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
                                                             </ItemTemplate>
                                                         </asp:TemplateField>
                                                         <asp:BoundField DataField="Invoice_DeductionUID" HeaderText="Invoice_DeductionUID" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <strong>No Records Found ! </strong>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                                </div>
                                            </div>
                                        </div>

                            <asp:HiddenField ID="Hidden1InvoiceTotal" runat="server" />
                            <asp:HiddenField ID="Hidden2UptoPrevInvoiceTotal" runat="server" />
                            <asp:HiddenField ID="HiddenCurrencySymbol" runat="server" />
                            <asp:HiddenField ID="HiddenCulturalInfo" runat="server" />
                            </div>
                     </div>
                        <%--</ContentTemplate>  
                        <Triggers>
                         <asp:AsyncPostBackTrigger ControlID="TreeView1" EventName="SelectedNodeChanged" />
                            <asp:PostBackTrigger ControlID="AddIssues" />
                         </Triggers>
                 </asp:UpdatePanel>--%>
        <%--<div id="Printdiv" runat="server">
            <div style="float:left; width:100%;">
                <div style="float:left; width:100%; text-align:center;">
                    <h3>Memo of Payment</h3>
                    </div>
                <div style="float:left; width:100%;">
                    <table class="table table-bordered">
                        <tr>
                            <td colspan="2">
                                <h4>Name & Address of the Contractor</h4>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <h5><asp:Label ID="LblContractorName" runat="server"></asp:Label></h5>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <h5><asp:Label ID="LblContractorAddress" runat="server"></asp:Label></h5>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <h5>Invoice</h5><h5><asp:Label ID="LblPrintInvoiceNumber" runat="server"></asp:Label></h5>
                            </td>
                            <td>
                                <h5>Date</h5><h5><asp:Label ID="LblPrintInvoiceDate" runat="server"></asp:Label></h5>
                            </td>
                        </tr>
                    </table>
                    <table class="table table-bordered">
                        <tr>
                            <td>
                                <h5>Name of Employer</h5>
                            </td>
                            <td>
                                <h5><asp:Label ID="LblNameoftheEmployer" runat="server"></asp:Label></h5>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <h5>Name of the Work</h5>
                            </td>
                            <td>
                                <h5><asp:Label ID="LblNameoftheWork" runat="server"></asp:Label></h5>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <h5>Contract Date</h5>
                            </td>
                            <td>
                                <h5><asp:Label ID="LblContractDate" runat="server"></asp:Label></h5>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <h5>Contract Value</h5>
                            </td>
                            <td>
                                <h5><asp:Label ID="LblContractValue" runat="server"></asp:Label></h5>
                            </td>
                        </tr>
                        </table>
                </div>
            </div>
        </div>--%>
      </div>

    <%--add invoice modal--%>
    <div id="ModAddInvoice" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Invoice</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 460px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--edit invoice modal--%>
    <div id="ModEditInvoice" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit Invoice</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 460px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--add invoice modal--%>
    <div id="ModAddInvoiceDeductions" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Invoice Deduction</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 480px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--edit invoice duction modal--%>
    <div id="ModEditInvoiceDeductions" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit Invoice Deduction</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 480px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--Add RA Bill modal--%>
    <div id="ModAddRABill" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add RA Bill</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 480px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--Show RA Bill modal--%>
    <div id="ModShowRABill" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">View RA Bill Items</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 480px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
</asp:Content>

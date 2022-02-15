<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.financial_status_update._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
      <style>
        .Hide
            {
                display : none;
            }
    </style>
  <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
  <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
  <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script type="text/javascript">
        function DeleteItem() {
            if (confirm("All data associated with this will be deleted. Are you sure you want to delete ...?")) {
                return true;
            }
            return false;
        }
       
    </script>
   
    <script type="text/javascript">
       // Sys.WebForms.PageRequestManager.getInstance().add_endRequest(DateText);
        function DateText() {
            $(".datepick").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy'
                });
            //$(function () {
                
            //    $("input[id$='dtActualPymentDate']").datepicker({
            //        changeMonth: true,
            //        changeYear: true,
            //        dateFormat: 'dd/mm/yy'
            //    });
            //    $(".datepick").datepicker({
            //        changeMonth: true,
            //        changeYear: true,
            //        dateFormat: 'dd/mm/yy'
            //    });
            //});
        }


          function BindEvents() {
           

              $(".showModalDocumentView").click(function (e) {
                 // alert("test");
                 jQuery.noConflict();
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModViewDocument iframe").attr("src", url);
        $("#ModViewDocument").modal("show");
            });

               
              $(".showBankGuaranteeModal").click(function (e) {
                 jQuery.noConflict();
                e.preventDefault();
                var url = $(this).attr("href");
                $("#ModAddBankGuarantee iframe").attr("src", url);
                $("#ModAddBankGuarantee").modal("show");
            });
         
        }

        $(document).ready(function () {
            //$('#loader').fadeOut();
             BindEvents();
            DateText();

           
        });

        function isNumberKey(txt, evt) {
      var charCode = (evt.which) ? evt.which : evt.keyCode;
      if (charCode == 46) {
        //Check if the text already contains the . character
        if (txt.value.indexOf('.') === -1) {
          return true;
        } else {
          return false;
        }
      } else {
        if (charCode > 31 &&
          (charCode < 48 || charCode > 57))
          return false;
      }
      return true;
    }
        
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
     <%--project selection dropdowns--%>

     <asp:ScriptManager ID="ScriptManager1" runat="server">
   </asp:ScriptManager>
    <%--<div id="loader"></div>--%>
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12 col-lg-4 form-group">Financial Status Update</div>
                <div class="col-md-6 col-lg-4 form-group">
                    <label class="sr-only" for="DDLProject">Project</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Project</span>
                        </div>
                        <asp:DropDownList ID="ddlProject" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlProject_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-6 col-lg-4 form-group">
                    <label class="sr-only" for="DDLWorkPackage">Work Package</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Work Package</span>
                        </div>
                        <asp:DropDownList ID="ddlworkpackage" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlworkpackage_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
    <div class="container-fluid">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                    <div class="row">
                        <div class="col-lg-6 col-xl-4 form-group" style="display:none">
                            <div class="card h-100" style="max-height:700px; overflow-y:auto;">
                                <div class="card-body">
                                    <h6 class="card-title text-muted text-uppercase font-weight-bold">WorkPackage Directory</h6>
                                    <div class="form-group">
                                        <label class="sr-only" for="TxtSearchDocuments">Search</label>
                                        <div class="input-group">
                                            <input id="TxtSearchDocuments" class="form-control" type="text" placeholder="Activity name" />
                                            <div class="input-group-append">
                                                <asp:Button ID="BtnSearchDocuments" CssClass="btn btn-primary" Text="Search" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                    <asp:TreeView runat="server" CssClass="it_tree_view" ID="TreeView1" ImageSet="XPFileExplorer" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged" NodeIndent="15" EnableTheming="True" NodeWrap="True">                               
                                        <NodeStyle CssClass="it_tree_view__node" NodeSpacing="2px" />
                                        <ParentNodeStyle Font-Bold="False" />
                                        <SelectedNodeStyle CssClass="it_tree_view__node__selected" HorizontalPadding="4px" VerticalPadding="2px" />
                                    </asp:TreeView>    
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-12 col-xl-12 form-group">
                            <asp:UpdateProgress ID="UpdateProgress2" runat="Server" AssociatedUpdatePanelID="UpdatePanel2" DisplayAfter="1"> 
                                        <ProgressTemplate > 
                                        <!-- <asp:Label ID="lblwait" runat="server" Text="Please wait while we fetch the data.." ForeColor="Red"></asp:Label>-->
                                            <div id="loader"></div>
                                        </ProgressTemplate> 
                                    </asp:UpdateProgress>
                                <div class="card mb-4">
                                    <div class="card-body">
                                        <div class="card-title">
                                            <div class="d-flex justify-content-between">
                                                <h6 class="text-muted">
                                                    <asp:Label ID="ActivityHeading" CssClass="text-uppercase font-weight-bold" runat="server" />
                                                    <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                                                </h6>
                                                
                                            </div>
                                        </div>
                                        <div class="table-responsive">
                                             <div align="left">
                                   <a id="AddData" runat="server" href="~/_modal_pages/add-Fin-Month-data.aspx" class="showBankGuaranteeModal"><asp:Button ID="Button2" runat="server" Text="+ Add" CssClass="btn btn-primary"></asp:Button></a>
                                </div><br />
                                        <div id="divStatus" runat="server" visible="false">
                                            <asp:GridView ID="grdFinanceMileStones" runat="server" DataKeyNames="Finance_MileStoneUID" AutoGenerateEditButton="false" CssClass="table table-bordered" AutoGenerateColumns="False" Width="100%" OnRowDataBound="grdFinanceMileStones_RowDataBound" OnRowCommand="grdFinanceMileStones_RowCommand" OnRowEditing="grdFinanceMileStones_RowEditing">
                                                <Columns>
                                                   <%-- <asp:BoundField DataField="Finance_MileStoneUID" HeaderText="UID" HeaderStyle-CssClass="Hide" ItemStyle-CssClass="Hide">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle CssClass="Hide"></ItemStyle>
                                                    </asp:BoundField>--%>
                                                    <asp:TemplateField HeaderText="MileStone Name">
                                                        <ItemTemplate>
                                                            <%#Eval("Finance_MileStoneName")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                   <%-- <asp:BoundField DataField="Finance_MileStoneName" HeaderText="MileStone Name" >
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>--%>
                                                    <asp:TemplateField HeaderText="Planned Date">
                                                        <ItemTemplate>
                                                            <%#Eval("Finance_PlannedDate","{0:dd/MM/yyyy}")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <%--<asp:BoundField DataField="Finance_PlannedDate" HeaderText="Planned Date"  DataFormatString="{0:dd/MM/yyyy}">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>--%>
                                                    <asp:TemplateField HeaderText="Allowed Payment">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtallowedpayment" CssClass="form-control" runat="server" Enabled="false" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Actual Payment">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtactualpayment" CssClass="form-control" runat="server" TextMode="Number" autocomplete="off" ClientIDMode="Static"></asp:TextBox><asp:Label ID="LblPaymentrequired" runat="server" ForeColor="Red"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Payment Date">
                                                        <ItemTemplate>
                                                            <div class="form-control">
                                                                <asp:TextBox ID="dtActualPymentDate" BorderStyle="None" BorderColor="White" class="datepick" runat="server" placeholder="dd/mm/yyyy" autocomplete="off"></asp:TextBox><asp:Label ID="LblPaymentDaterequired" runat="server" ForeColor="Red"></asp:Label>
                                                            </div>
                                                            
                                                        </ItemTemplate>
                                                        
                                                    </asp:TemplateField>
                                                       <asp:TemplateField>
                                                    <ItemTemplate>
                                                         <asp:Button ID="btnupdate" runat="server" Text="Update" CssClass="btn btn-primary" CommandName="edit" CommandArgument="<%#((GridViewRow) Container).RowIndex%>" CausesValidation="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    <strong>No Records Found ! </strong>
                                                </EmptyDataTemplate>
                                            </asp:GridView>

                                            <%--<table class="table table-bordered">
                                                <thead>
                                                    <tr><td>
                                                        Task
                                                    </td><td>
                                                        Allowed Payment
                                                    </td><td>
                                                        Actual Payment
                                                    </td><td>
                                                        Actual Payment Date
                                                    </td><td></td></tr>
                                                </thead>
                                                <tr>
                                                    
                                                    <td>
                                                        <asp:Label ID="LblTaskName" runat="server"></asp:Label>
                                                    </td>
                                               
                                                    
                                                    <td>
                                                        <asp:TextBox ID="txtallowedpayment" CssClass="form-control" Enabled="false" runat="server"></asp:TextBox>
                                                    </td>
                                                
                                                    
                                                    <td>
                                                        <asp:TextBox ID="txtactualpayment" CssClass="form-control" runat="server" required></asp:TextBox>
                                                    </td>
                                               
                                                    
                                                    <td>
                                                        <asp:TextBox ID="dtActualpayment" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                                                    </td>
                                                  
                                                    <td>
                                                        <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-primary" OnClick="btnUpdate_Click" />
                                                    </td>
                                                </tr>
                                            </table>--%>
                                             <%--<asp:GridView ID="grdTaskPayment" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered" Width="100%" OnRowDataBound="grdTaskPayment_RowDataBound" OnRowCommand="grdTaskPayment_RowCommand" OnRowEditing="grdTaskPayment_RowEditing">
                                            <Columns>
                                                <asp:BoundField DataField="PaymentUID" HeaderText="UID" HeaderStyle-CssClass="Hide" ItemStyle-CssClass="Hide">
                                                <HeaderStyle HorizontalAlign="Left" />

                                                <ItemStyle CssClass="Hide"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Name" HeaderText="Task" >
                                                <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                              
                                                <asp:TemplateField HeaderText="Allowed Payment">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtallowedpayment" CssClass="form-control" runat="server" Text='<%#Eval("AllowedPayment")%>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                             
                                                <asp:TemplateField HeaderText="Actual Payment">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtactualpayment" CssClass="form-control" runat="server" Text='<%#Eval("ActualPayment")%>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Actual Payment Date">
                                                    <ItemTemplate>
                                                        <script type="text/javascript">
                                                            $(document).ready(function () {
                                                                $(function () {
                                                                    $("input[id$='dtActualpayment']").datepicker({
                                                                        changeMonth: true,
                                                                        changeYear: true,
                                                                        dateFormat: 'dd/mm/yy'
                                                                    });
                                                                });
                                                            });
                                                    </script>
                                                        <asp:TextBox ID="dtActualpayment" CssClass="form-control" runat="server" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="ActualPaymentDate" HeaderText="Date" HeaderStyle-CssClass="Hide" ItemStyle-CssClass="Hide"  DataFormatString="{0:dd/MM/yyyy}">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                 <asp:BoundField DataField="MileStoneUID" HeaderText="MileStone" HeaderStyle-CssClass="Hide" ItemStyle-CssClass="Hide" >
                                                <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                    
                                                   <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:UpdatePanel ID="UP1" runat="server">
                                                        <ContentTemplate>
                                                            <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-primary" CommandName="edit" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CausesValidation="false" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="btnUpdate" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                     
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <strong>No Records Found ! </strong>
                                            </EmptyDataTemplate>
                                        </asp:GridView>--%>
                                        </div>
                                            <div id="divStatusMonth" runat="server" visible="false">
                                                <asp:GridView ID="grdMileStoneMonths" runat="server" DataKeyNames="FinMileStoneMonthUID" AutoGenerateEditButton="false" CssClass="table table-bordered" AutoGenerateColumns="False" Width="100%" OnRowDataBound="grdMileStoneMonths_RowDataBound" OnRowEditing="grdMileStoneMonths_RowEditing" OnRowDeleting="grdMileStoneMonths_RowDeleting" OnRowCancelingEdit="grdMileStoneMonths_RowCancelingEdit" OnRowUpdating="grdMileStoneMonths_RowUpdating">
                                                <Columns>
                                                    <%--<asp:BoundField DataField="Finance_MileStoneUID" HeaderText="UID" HeaderStyle-CssClass="Hide" ItemStyle-CssClass="Hide">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle CssClass="Hide"></ItemStyle>
                                                    </asp:BoundField>--%>
                                                  <%--  <asp:TemplateField HeaderText="MileStone Name">
                                                        <ItemTemplate>
                                                            <%#Eval("Finance_MileStoneName")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>
                                                  <%--  <asp:BoundField DataField="Finance_MileStoneName" HeaderText="MileStone Name" >
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>--%>
                                                    <asp:TemplateField HeaderText="Planned Month">
                                                        <ItemTemplate>
                                                           <%#Eval("MonthYear")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Projected Payment (Crores)" ItemStyle-HorizontalAlign="Right">
                                                        <ItemTemplate>
                                                             <%#Eval("AllowedPayment")%>
                                                        </ItemTemplate>
                                                           <EditItemTemplate>
                                                      <asp:HiddenField ID="hidrabilluid" runat="server" value='<%#Eval("FinMileStoneMonthUID")%>' />
                                                       <asp:HiddenField ID="hidPlannedMonth" runat="server" value='<%#Eval("MonthYear")%>' />
                                                                <asp:HiddenField ID="hidoldvalue" runat="server" value='<%#Eval("AllowedPayment")%>' />
                                                     <asp:TextBox ID="txtProjected" runat="server" Text='<%#Eval("AllowedPayment")%>' onkeypress="return isNumberKey(this, event);"></asp:TextBox>
                                                 </EditItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Actual Payment (Crores)" ItemStyle-HorizontalAlign="Right">
                                                        <ItemTemplate>
                                                           <%#Eval("PaymentMade")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                      <asp:TemplateField HeaderText="Total Deductions (Crores)" ItemStyle-HorizontalAlign="Right">
                                                        <ItemTemplate>
                                                           <%#Eval("PaymentMade")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                   <%--<asp:BoundField DataField="PaymentMade"  HeaderText="" ItemStyle-HorizontalAlign="Right">
                            
                            </asp:BoundField>--%>
                                                    <%-- <asp:BoundField DataField="PaymentMade"  HeaderText="Total Deductions (Crores)" ItemStyle-HorizontalAlign="Right">
                            
                            </asp:BoundField>--%>
                                                     <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <a class="showModalDocumentView" href="/_modal_pages/view-Finpayments.aspx?MonthID=<%#Eval("FinMileStoneMonthUID")%>&MilestoneUID=<%#Eval("Finance_MileStoneUID")%>&WkpgUID=<%#Eval("TaskUID")%>">Payment Details</a>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                 
                                                      <asp:TemplateField ShowHeader="False">
                                                <EditItemTemplate>
                                                    <asp:LinkButton ID="lnkupdate" runat="server" CausesValidation="True" CommandName="Update" Text="Update"></asp:LinkButton>
                                                    &nbsp;<asp:LinkButton ID="lnkcancel" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkedit" runat="server" CausesValidation="False" CommandName="Edit" ><span title="Edit" class="fas fa-edit"></span></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                                         <asp:TemplateField ShowHeader="False">
                                                <ItemTemplate>
                                                     <asp:HiddenField ID="hidDeleteuid" runat="server" value='<%#Eval("FinMileStoneMonthUID")%>' />
                                                    <asp:LinkButton ID="lnkdelete" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete" OnClientClick="return DeleteItem();"></asp:LinkButton>
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
                        </ContentTemplate>  
                        <Triggers>
                         <asp:AsyncPostBackTrigger ControlID="TreeView1" EventName="SelectedNodeChanged" />
                            <%--<asp:PostBackTrigger ControlID="grdFinanceMileStones" />--%>
                            <asp:AsyncPostBackTrigger ControlID="grdFinanceMileStones" EventName="RowEditing" />
                            <asp:AsyncPostBackTrigger ControlID="grdMileStoneMonths" EventName="RowEditing" />
                         </Triggers>
                 </asp:UpdatePanel>
      </div>
       <%--View document modal--%>
    <div id="ModViewDocument" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">View Payment Details</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:500px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
     <div id="ModAddBankGuarantee" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Month Data</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:500px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
</asp:Content>

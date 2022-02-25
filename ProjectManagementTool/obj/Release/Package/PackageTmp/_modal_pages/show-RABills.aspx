<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="show-RABills.aspx.cs" Inherits="ProjectManagementTool._modal_pages.show_RABills" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".showJointInspectionModel").click(function (e) {
                e.preventDefault();
                jQuery.noConflict();
                var url = $(this).attr("href");
                $("#ModJointInspection iframe").attr("src", url);
                $("#ModJointInspection").modal("show");
            });

           

            $(".showAddJointInspectionModel").click(function (e) {
                e.preventDefault();
                jQuery.noConflict();
                var url = $(this).attr("href");
                $("#ModAddJointInspection iframe").attr("src", url);
                $("#ModAddJointInspection").modal("show");
            });

            $(".ViewJointInspectionModel").click(function (e) {
                e.preventDefault();
                jQuery.noConflict();
                var url = $(this).attr("href");
                $("#ModViewJointInspection iframe").attr("src", url);
                $("#ModViewJointInspection").modal("show");
            });

            $(".AddRABillItemModel").click(function (e) {
                e.preventDefault();
                jQuery.noConflict();
                var url = $(this).attr("href");
                $("#ModAddRABillItem iframe").attr("src", url);
                $("#ModAddRABillItem").modal("show");
            });

            $(".showBOQModal").click(function(e) {
                e.preventDefault();
                jQuery.noConflict();
            var url = $(this).attr("href");
            $("#ModBOQData iframe").attr("src", url);
            $("#ModBOQData").modal("show");
            });
        });
    </script>
     <script>
         $(function () {
             $("input[id$='txtDate']").datepicker({
                 changeMonth: true,
                 changeYear: true,
                 dateFormat: 'dd/mm/yy'
             });

             $("input[id$='txtCAADate']").datepicker({
                 changeMonth: true,
                 changeYear: true,
                 dateFormat: 'dd/mm/yy'
             });

         });
    </script>
    <script type="text/javascript">
        var specialKeys = new Array();
        specialKeys.push(8); //Backspace
        function IsNumeric(e) {
            var keyCode = e.which ? e.which : e.keyCode
            var ret = ((keyCode >= 48 && keyCode <= 57) || specialKeys.indexOf(keyCode) != -1);
            document.getElementById("error").style.display = ret ? "none" : "inline";
            return ret;
        }

        //
          function ShowProgressBar(status) {
               if (status == "true") {
                    document.getElementById('dvProgressBar').style.visibility = 'visible';
            }
            else {
                document.getElementById('dvProgressBar').style.visibility = 'hidden';
            }
          }


         $(document).ready(function () {

         document.getElementById('dvProgressBar').style.visibility = 'hidden';

        });
      
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddDocumentModal" runat="server">
    <div class="container-fluid" style="overflow-y:auto; min-height:90vh; max-height:90vh;">
        <div id="dvProgressBar" style=" text-align:center; position:relative; visibility:visible;" >
                     <img src="../_assets/images/progress.gif" width="40" alt="loading"  /> <span style="color:#006699; font-weight:bold;">Processing please wait...</span>
                     </div> 
        <%--<div class="row">
           <div class="col-sm-6">
                <div class="form-group">
                       <div class="form-group" >
                        <label class="lblCss" for="lblWorkPackage">Issue</label>&nbsp;&nbsp;<b>:</b>&nbsp;&nbsp;
                         <asp:Label ID="LblIssue" class="lblCss" Font-Bold="true" runat="server"></asp:Label>
                    </div>
                 </div>
           </div>
            <div class="col-sm-6">
                 
                </div>
        </div>--%>
        <div class="row" id="additems" runat="server" visible="false">
            <div class="col-sm-12">
                <div class="table-responsive">
                    <h6 class="text-muted">
                                                        <asp:Label id="lblAddRaBillItem" CssClass="text-uppercase font-weight-bold" runat="server" Text="Add Item to RA Bill" />
                                                        <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                                                    </h6>
                   
                        <table style="width:100%;">
                            <tr>
                                <td style="width:45%">Choose Item Number &nbsp;<span style="color:red; font-size:1.1rem;">*</span>&nbsp;<asp:LinkButton ID="LnkChangeItem" runat="server" Text="Change Item" OnClick="LnkChangeItem_Click"></asp:LinkButton> <br />
                                    <asp:Label ID="lblActivityName" runat="server" CssClass="form-control"></asp:Label>                        
                                     <a id="LinkBOQData" runat="server" href="/_modal_pages/boq-treeview.aspx" class="showBOQModal">
                                         <asp:Button ID="btnchoose" runat="server" CausesValidation="false" Text="Choose Item Number" CssClass="form-control btn-link" />
                                     </a>
                                </td>
                                <td style="width:5%">&nbsp;</td>
                                <td style="width:40%">Item Description &nbsp;<span style="color:red; font-size:1.1rem;">*</span><br />
                                    <asp:TextBox ID="txtradescription" CssClass="form-control" autocomplete="off" required  runat="server" ClientIDMode="Static"></asp:TextBox>
                                </td>
                                <td style="width:5%">&nbsp;</td>
                                <td style="width:10%; text-align:right;">
                                    &nbsp;<span style="color:red; font-size:1.1rem;">&nbsp;</span><br />
                                    <%--<asp:Button ID="btnAddItem" runat="server" Text="+ Add Item" CssClass="btn btn-primary" OnClick="btnAddItem_Click"></asp:Button>--%>
                                    <a id="AddJointInspectionItem" runat="server" href="/_modal_pages/add-jointinspection-to-rabill.aspx" class="showAddJointInspectionModel">
                                    <asp:Button ID="btnAddItem" runat="server" Text="+ Add Item" CssClass="btn btn-primary"></asp:Button></a>
                                </td>
                            </tr>
                        </table>
                        </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-12">
                <div class="table-responsive">
                    <%--<br /><br />--%>
                    <asp:Button ID="btngetData" CssClass="btn btn-primary" runat="server" Text="Get RABill Items" OnClientClick="ShowProgressBar('true')" OnClick="btngetData_Click" />
                    <h6 class="text-muted">

                                                        <asp:Label id="LblRABillItems" CssClass="text-uppercase font-weight-bold" runat="server" Text="List of RA Bill Items" />
                                                        <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                                                    </h6>
                        <asp:GridView ID="grdRaBills" runat="server" Width="100%" AllowPaging="false" CssClass="table table-bordered" 
                            DataKeyNames="itemUId" OnRowDataBound="grdRaBills_RowDataBound" PageSize="20"
                            EmptyDataText="No Data Found" AutoGenerateColumns="False" OnPageIndexChanging="grdRaBills_PageIndexChanging"
                            OnRowEditing="grdRaBills_RowEditing" OnRowCancelingEdit="grdRaBills_RowCancelingEdit" 
                            OnRowUpdating="grdRaBills_RowUpdating"  
                         >
                        <Columns>
                            <asp:TemplateField HeaderText="Item Number">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hidUid" runat="server" value='<%# Bind("itemUId") %>' />
                                    
                                    <a id="AddJointInspection" href='/_modal_pages/add-jointinspection-to-rabill.aspx?itemUId=<%#Eval("itemUId")%>&WorkpackageUID=<%#Eval("WorkpackageUID")%>&RABillUid=<%#Eval("RABillUid")%>' class="showAddJointInspectionModel"><%#Eval("item_number")%></a> 
                                    <%--<asp:Label ID="Label1" runat="server" Text='<%# Bind("item_number") %>'></asp:Label>--%>
                                </ItemTemplate>
                                <ItemStyle Width="7%" />
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Item Description">
                                <ItemTemplate>
                                    <a href="#" data-toggle="tooltip" title='<%#GetBOQHierarchy_By_itemUId(Eval("itemUId").ToString())%>'><i class="fa fa-info-circle" style="color:black;" aria-hidden="true"></i></a>
                                    <%--<a href="#" data-toggle="tooltip" title='<%#GetBOQDescriptionHierarchy_By_itemUId(Eval("itemUId").ToString())%>'><i class="fa fa-info-circle" style="color:black;" aria-hidden="true"></i></a>--%>
                                    <%#Eval("item_desc")%><br />
                                    <asp:Label ID="LblTRDesc" runat="server"></asp:Label>
                                    <%--<a id="ShowJointInspection" href='/_modal_pages/show-joint-inspection.aspx?boqUid=<%#Eval("itemUId")%>&View=true' class="showJointInspectionModel"><%#Eval("item_desc")%></a> --%>
                                </ItemTemplate>
                                <ItemStyle Width="40%" />
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Approved Quantity">
                                <ItemTemplate>
                                    <asp:Label ID="LblApprovedQuantity" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Approved.Rate">
                                <ItemTemplate>
                                    <asp:Label ID="LblApprovedRate" runat="server"></asp:Label><br /><br />
                                    <asp:Label ID="LblTRRate" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Prev. Quantity">
                                <ItemTemplate>
                                    <asp:Label ID="LblPrevQuantity" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Prev.Amount">
                                <ItemTemplate>
                                    <asp:Label ID="LblPrevAmount" runat="server"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="12%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Current Quantity">
                                <ItemTemplate>
                                    <asp:Label ID="LblCurrentQuantity" runat="server"></asp:Label>
                                    <asp:Label ID="LblTRCurrentRate" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Current.Amount">
                                <ItemTemplate>
                                    <asp:Label ID="LblCurrentAmount" runat="server"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="12%" />
                            </asp:TemplateField>
                            <%--<asp:TemplateField HeaderText="Date">
                           
                                <ItemTemplate>
                                    <asp:Label ID="Label3" runat="server" Text='<%# Bind("created_Date", "{0:dd MMM yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="20%" />
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>--%>
                            <%--<asp:TemplateField HeaderText="Current Cost">
                                <EditItemTemplate>
                                    
                                    <asp:TextBox ID="txtItemCost" width="50px" runat="server"  onkeydown = "return (!(event.keyCode>=65) && event.keyCode!=32);" Text='<%# Bind("current_cost") %>'
                                       ></asp:TextBox>
                                     <asp:HiddenField ID="hidCost" runat="server"  Value='<%# Bind("current_cost") %>'/>
                                </EditItemTemplate>
                                <ItemTemplate>
                                   
                                    <asp:Label ID="Label4" runat="server" Text='<%# Bind("current_cost") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="20%" />
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>--%>
                            <%--<asp:TemplateField ShowHeader="False">
                                <EditItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update" Text="Update"></asp:LinkButton>
                                    &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit" ><span title="Edit" class="fas fa-edit"></span></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                  
                            <asp:TemplateField HeaderText="Joint Inspection">
                                <ItemTemplate>
                                    <a id="ViewJointInspection" href='/_modal_pages/add-jointinspection-to-rabill.aspx?itemUId=<%#Eval("itemUId")%>&View=true&RABillUid=<%#Eval("RABillUid")%>' class="ViewJointInspectionModel">View</a> 
                                </ItemTemplate>
                            </asp:TemplateField>     
                        </Columns>
                    </asp:GridView>
                    
                    <%--<div id="additems" runat="server" visible="false">

                   <table >
                    <tr style="width:100%">
                        <td style="width:10%">
                            Item Id<br /><asp:TextBox ID="txtItemNumber" runat="server" style="width:100%" ></asp:TextBox></td>
                        <td style="width:40%" >Description<br /><asp:TextBox ID="txtDescription" runat="server" style="width:100%" ></asp:TextBox></td>
                        <td style="width:20%" >Date<br /><asp:TextBox ID="txtDate" runat="server" style="width:100%" ></asp:TextBox></td>
                        <td style="width:20%">Actual Cost<br /><asp:TextBox ID="txtAddcost" runat="server" style="width:100%"></asp:TextBox></td>
                        <td style="width:10%"><br /> <asp:Button ID="btnAdd" runat="server" Text="Add Item" CssClass="btn btn-primary" style="width:100%" OnClick="btnAdd_Click"/></td>
                    </tr></table></div>--%>
             </div>

                
                           </div>
                
            </div>
    </div>
    <div class="modal-footer">
        
        <%--<a id="AddRABillItem" runat="server" href="/_modal_pages/add-rabillitem.aspx" class="AddRABillItemModel">
            <asp:Button ID="Btnadd" runat="server" Text="+ Add Item" CssClass="btn btn-primary"></asp:Button>
        </a> --%>
           <%-- <a id="AddStatus" runat="server" href="/_modal_pages/add-issuestatus.aspx" class="showStatusModal"><asp:Button ID="btnaddstatus" runat="server" Height="35px" Width="150px" Text="+ Add Status" CssClass="btn btn-primary"></asp:Button></a>--%>
                </div>

    <script>
$(document).ready(function(){
  $('[data-toggle="tooltip"]').tooltip();   
});
</script>

        <%--Joint Inspection modal--%>
    <div id="ModJointInspection" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Inspection Report</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:340px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

        <%--Assign Joint Inspection to RA bill modal--%>
    <div id="ModAddJointInspection" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Assign Inspection Report to RA Bill</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:340px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

        <%--View Joint Inspection assigend to RA bill modal--%>
    <div id="ModViewJointInspection" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">View Inspection Report</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:340px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

         <%--Add RA bill Item modal--%>
    <div id="ModAddRABillItem" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add RA Bill Item</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:280px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

        <%--Link BOQ Data modal--%>
    <div id="ModBOQData" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Link BOQ Activity</h5>
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

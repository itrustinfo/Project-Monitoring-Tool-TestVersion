<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="show-joint-inspection.aspx.cs" Inherits="ProjectManagementTool._modal_pages.show_joint_inspection" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
        <style type="text/css">
        .hiddencol { display: none; }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".showAddJointInspectionModel").click(function (e) {
                e.preventDefault();
                jQuery.noConflict();
                var url = $(this).attr("href");
                $("#ModAddJointInspection iframe").attr("src", url);
                $("#ModAddJointInspection").modal("show");
            });
            $(".showEditJointInspectionModel").click(function (e) {
                e.preventDefault();
                jQuery.noConflict();
                var url = $(this).attr("href");
                $("#ModEditJointInspection iframe").attr("src", url);
                $("#ModEditJointInspection").modal("show");
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddDocumentModal" runat="server">
    <div class="container-fluid" style="overflow-y:auto; max-height:84vh; min-height:82vh;">

        <div class="row">
            <div class="col-sm-12">
                <div class="table-responsive">
                        <asp:GridView ID="grdinspectionReport" runat="server" Width="100%" AllowPaging="True" CssClass="table table-bordered" DataKeyNames="inspectionUid"
                            
                            EmptyDataText="No Data Found" AutoGenerateColumns="False" OnPageIndexChanging="grdRaBills_PageIndexChanging"
                            OnRowEditing="grdRaBills_RowEditing" OnRowCancelingEdit="grdRaBills_RowCancelingEdit" OnRowDataBound="grdinspectionReport_RowDataBound"
                            OnRowUpdating="grdRaBills_RowUpdating" OnRowDeleting="grdinspectionReport_RowDeleting"   OnRowCommand="grdinspectionReport_RowCommand"                        >
                        <Columns>
                            <asp:TemplateField HeaderText="Dia Pipe">
                                
                                <ItemTemplate>
                                    <asp:HiddenField ID="hidUid" runat="server" value='<%# Bind("inspectionUid") %>' />
                                    <asp:Label ID="lbldesc" runat="server" Text='<%# Bind("DiaPipe") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="30%" />
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Unit">
                               
                                <ItemTemplate>
                                    <asp:Label ID="lblunit" runat="server" Text='<%# Bind("unit") %>'></asp:Label>
                                </ItemTemplate>
                                 <EditItemTemplate>
                                     <asp:TextBox ID="txtUnits" runat="server" Text='<%# Bind("unit") %>'></asp:TextBox>
                                 </EditItemTemplate>
                                <ItemStyle Width="10%" />
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Invoice Number">
                               
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("invoice_number") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="20%" />
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Invoice Date">
                           
                                <ItemTemplate>
                                    <asp:Label ID="Label3" runat="server" Text='<%# Bind("invoicedate", "{0:dd MMM yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="20%" />
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Quantity">
                                <EditItemTemplate>
                                    
                                    <asp:TextBox ID="txtQuantity" width="50px" runat="server"  onkeydown = "return (!(event.keyCode>=65) && event.keyCode!=32);" Text='<%# Bind("quantity") %>'
                                       ></asp:TextBox>
                                     <asp:HiddenField ID="hidCost" runat="server"  Value='<%# Bind("inspectionUid") %>'/>
                                </EditItemTemplate>
                                <ItemTemplate>
                                   
                                    <asp:Label ID="Label4" runat="server" Text='<%# Bind("quantity") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="20%" />
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <%--<EditItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update" Text="Update"></asp:LinkButton>
                                    &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                                </EditItemTemplate>--%>
                                <ItemTemplate>
                                    <a id="EditJointInspection" href="/_modal_pages/add-jointinspection.aspx?type=edit&BOQUID=<%#Eval("BOQUid")%>&inspectionUid=<%#Eval("inspectionUid")%>" class="showEditJointInspectionModel"><span title="Edit" class="fas fa-edit"></span></a> 
                                    <%--<asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit" ><span title="Edit" class="fas fa-edit"></span></asp:LinkButton>--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                              <asp:TemplateField ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkdelete" runat="server" CausesValidation="False" CommandName="Delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                            <asp:BoundField DataField="ServerCopiedAdd" ItemStyle-CssClass="hiddencol"  HeaderStyle-CssClass="hiddencol" HeaderText="ServerCopiedAdd" >
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                         <asp:BoundField DataField="ServerCopiedUpdate" ItemStyle-CssClass="hiddencol"  HeaderStyle-CssClass="hiddencol" HeaderText="ServerCopiedUpdate" >
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                            <asp:TemplateField>
                                  <ItemTemplate>
                                       <asp:LinkButton ID="LnkDownloadnew" runat="server" CausesValidation="false" CommandArgument='<%#Eval("inspectionUid")%>' CommandName="download"><span title="Edit" class="fas fa-paperclip"></span></asp:LinkButton>
                                   </ItemTemplate>
                           </asp:TemplateField>
                                      
                        </Columns>
                    </asp:GridView>
                    <%--<div id="additems" runat="server">
                   <table >
                    <tr style="width:100%">
                        <td style="width:20%">
                           Dia Of Pipe<br /><asp:TextBox ID="txtdiapipe" runat="server" style="width:100%" required></asp:TextBox></td>
                        <td style="width:10%" >Unit<br /><asp:TextBox ID="txtunit" runat="server" style="width:100%" required ></asp:TextBox></td>
                        <td style="width:10%" >Invoice <br /><asp:TextBox ID="txtinvoiceNumber" runat="server" style="width:100%" ></asp:TextBox></td>
                        <td style="width:20%" >Invoice Date<br /><asp:TextBox ID="txtDate" runat="server" style="width:100%" ></asp:TextBox></td>
                        <td style="width:20%">Quantity<br /><asp:TextBox ID="txtQuantity" runat="server" style="width:100%" required></asp:TextBox></td>
                        <td style="width:10%"><br />
                            <asp:Button ID="btnAdd" runat="server" Text="Add Inspection" CssClass="btn btn-primary" style="width:100%" OnClick="btnAdd_Click"/></td>
                    </tr></table></div>--%>
             </div>
                           </div>
                
            </div>
    </div>
    <div class="modal-footer">
        <a id="AddJointInspection" runat="server" href="/_modal_pages/add-jointinspection.aspx" class="showAddJointInspectionModel"><asp:Button ID="btnaddinspection" runat="server" Text="+ Add Inspection" CssClass="btn btn-primary"></asp:Button></a> 
           <%-- <a id="AddStatus" runat="server" href="/_modal_pages/add-issuestatus.aspx" class="showStatusModal"><asp:Button ID="btnaddstatus" runat="server" Height="35px" Width="150px" Text="+ Add Status" CssClass="btn btn-primary"></asp:Button></a>--%>
                </div>

    <div id="ModAddJointInspection" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Joint Inspection</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:280px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

        <div id="ModEditJointInspection" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit Joint Inspection</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:280px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
        </form>
</asp:Content>

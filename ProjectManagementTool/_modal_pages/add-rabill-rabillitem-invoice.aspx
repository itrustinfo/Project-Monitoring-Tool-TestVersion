<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-rabill-rabillitem-invoice.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_rabill_rabillitem_invoice" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <script type="text/javascript">
        function BindEvents() {
        $(".showBOQModal").click(function(e) {
        //document.getElementsByClassName("showTaskModal").click(function(e) {
            e.preventDefault();
            jQuery.noConflict();
        var url = $(this).attr("href");
        $("#ModBOQData iframe").attr("src", url);
        $("#ModBOQData").modal("show");
        });
        }
        $(document).ready(function () {
            BindEvents();
        });
</script>
    <%--<script type="text/javascript">
        function StoreRBBillData(sFor) {
            
            if (sFor == "Number") {
                var rabillnumber = document.getElementById('<%=txtaddrabillnumber.ClientID%>').value;
                '<%Session["RABillNumber"] = "' + rabillnumber + '"; %>';
            }
            else {
                var rabilldate = document.getElementById('<%=txtDate.ClientID%>').value;
                '<%Session["RABillDate"] = "' + rabilldate + '"; %>';
            }
        }
    </script>--%>
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

             $("input[id$='dtRABillDate']").datepicker({
                 changeMonth: true,
                 changeYear: true,
                 dateFormat: 'dd/mm/yy'
             });
         });

           function ShowProgressBar(status) {
               if (status == "true") {
                   
                if (document.getElementById("txtaddrabillnumber").value != "" && document.getElementById("txtDate").value != "") {
                    
                    document.getElementById('dvProgressBar').style.visibility = 'visible';
                    
                }
                    else {
                document.getElementById('dvProgressBar').style.visibility = 'hidden';
            }
                
            }
            else {
                document.getElementById('dvProgressBar').style.visibility = 'hidden';
            }
        }
     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="FrmAddActivityData" runat="server">
        <div id="dvrabill" runat="server" visible="false">
        <div class="container-fluid">
            <div class="row">
                <div class="col-sm-12">
                    <div class="form-group">
                        <label class="lblCss" for="txtoptionname">Invoice Number</label>
                        <asp:HiddenField ID="hidInvoiceUId" runat="server" />
                        <asp:TextBox ID="txtInvoiceNumber" CssClass="form-control"  Enabled="false" runat="server" ClientIDMode="Static"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtRabillNumber">RA Bill Number</label>
                        <asp:DropDownList ID="ddlRabillNumber" CssClass="form-control" required runat="server" ClientIDMode="Static"></asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="dtExpiry">RA Bill Date</label> 
                        <asp:TextBox ID="dtRABillDate" CssClass="form-control" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                    </div>
                </div> 
                
        </div>
            </div>
        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click"  />
                </div>
            </div>
          <%--<div id="dvRabillItem" runat="server" visible="false">
        <div class="container-fluid">
            <div class="row">
                <div class="col-sm-12">
                    <div class="form-group">
                        <label class="lblCss" for="txtoptionname">RA Bill Number</label>
                        <asp:TextBox ID="txtrabillnumber" CssClass="form-control"  Enabled="false" runat="server" ClientIDMode="Static"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtRabillNumber">Item Number</label>
                        <asp:TextBox ID="txtraitemnumber" CssClass="form-control" required  runat="server" ClientIDMode="Static"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtRabillNumber">Description</label>
                        <asp:TextBox ID="txtradescription" CssClass="form-control" required  runat="server" ClientIDMode="Static"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtRabillNumber">Actual Cost</label>
                        <asp:TextBox ID="txtracost" CssClass="form-control" required  runat="server" ClientIDMode="Static"></asp:TextBox>
                    </div>
                </div> 
                
        </div>
            </div>
        <div class="modal-footer">
            <asp:Button ID="Button1" runat="server" Text="Submit" CssClass="btn btn-primary"  />
                </div>
            </div>--%>

        <div id="invoice_RABill" runat="server" visible="false">
        <div class="container-fluid">
            <div class="row">
                <div class="col-sm-12">
                   

                    <div class="form-group">
                        <label class="lblCss" for="txtRabillNumber">RA Bill Number</label>  &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="txtaddrabillnumber" CssClass="form-control" autocomplete="off" required  runat="server" ClientIDMode="Static"></asp:TextBox>
                        
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="txtRabillNumber">RA Bill Date</label>  &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="txtDate" CssClass="form-control" autocomplete="off" required  runat="server" ClientIDMode="Static"></asp:TextBox>
                        
                    </div>
                    
                    <div class="form-group">
                        <label class="lblCss" for="UploadPhotographs">Choose File/s</label>
                        &nbsp;<span style="color: red; font-size: 1.2rem;">*</span>
                        <div class="custom-file">
                            <asp:FileUpload ID="ImageUpload" runat="server" AllowMultiple="true" CssClass="custom-file-input" />
                            <label class="custom-file-label" for="UploadPhotographs">Choose Files</label>
                        </div>
                    </div>
                    <%--<div class="form-group">
                        <label class="lblCss" for="txtRabillNumber">Item Number</label>
                        <asp:TextBox ID="txtraitemnumber" CssClass="form-control" required  runat="server" ClientIDMode="Static"></asp:TextBox>
                    </div>--%>
                     
                    
                    <%--<div class="form-group">
                        <label class="lblCss" for="txtRabillNumber">Actual Cost</label>
                        <asp:TextBox ID="txtracost" CssClass="form-control" autocomplete="off" required  runat="server" ClientIDMode="Static"></asp:TextBox>
                    </div>--%>
                </div> 
                
        </div>
            </div>
             <div id="dvProgressBar" style=" text-align:center; position:relative; visibility:hidden;" >
                     <img src="../_assets/images/progress.gif" width="40" alt="loading"  /> <span style="color:#006699; font-weight:bold;">Processing please wait...</span>
                     </div> 
        <div class="modal-footer">
            <asp:Button ID="btnaddrabill" runat="server" Text="Continue" CssClass="btn btn-primary" OnClientClick="ShowProgressBar('true')" OnClick="btnaddrabill_Click"  />
            
                </div>
            </div>

        <div id="AddRABillItem" runat="server" visible="false">
        <div class="container-fluid">
            <div class="row">
                <div class="col-sm-12">
                    <div class="form-group">
                        <label class="lblCss" for="txtRabillNumber">RA Bill Number</label>  
                        <asp:Label ID="LblRABillNumber" runat="server" CssClass="form-control"></asp:Label>
                    </div>
                    <div class="form-group">
                        <asp:HiddenField ID="HiddenRABillUID" runat="server" />
                        <label class="lblCss" for="txttaskname">Choose Item Number</label>  &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:Label ID="lblActivityName" runat="server" CssClass="form-control"></asp:Label>                        
                         <a id="LinkBOQData" runat="server" href="/_modal_pages/boq-treeview.aspx" class="showBOQModal">
                             <asp:Button ID="btnchoose" runat="server" CausesValidation="false" Text="Choose Item Number" CssClass="form-control btn-link" />
                         </a>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtRabillNumber">Item Description</label>  &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="txtradescription" CssClass="form-control" autocomplete="off" required  runat="server" ClientIDMode="Static"></asp:TextBox>
                    </div>
                </div> 
                
        </div>
            </div>
        <div class="modal-footer">
            <asp:Button ID="btnAddRaBillItem" runat="server" Text="Add Item" CssClass="btn btn-primary" OnClick="btnAddRaBillItem_Click"  />
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

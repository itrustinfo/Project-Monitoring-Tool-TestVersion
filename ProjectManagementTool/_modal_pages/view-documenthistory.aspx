<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="view-documenthistory.aspx.cs" Inherits="ProjectManagementTool._modal_pages.view_documenthistory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
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
            $(".showStatusModal").click(function (e) {
                e.preventDefault();
                jQuery.noConflict();
                var url = $(this).attr("href");
                $("#ModAddDocumentStatus iframe").attr("src", url);
                $("#ModAddDocumentStatus").modal("show");
            });

            $(".showuploaddocumentModal").click(function (e) {
                e.preventDefault();
                jQuery.noConflict();
                var url = $(this).attr("href");
                $("#ModResubmitdocument iframe").attr("src", url);
                $("#ModResubmitdocument").modal("show");
            });
        });
    </script>
    <script type="text/javascript"> 
        $(function () {
           
            $("[id*=imgProductsShow]").each(function () {
                if ($(this)[0].src.indexOf("minus") != -1) {
                    $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>");
                    $(this).next().remove();
                }
            });
        });
    </script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
<form id="frmAddDocumentModal" runat="server">
    
    <div class="container-fluid" style="overflow-y:auto; min-height:85vh;">
        <div class="row">
           <div class="col-sm-6">
                <div class="form-group">
                       <div class="form-group" >
                        <label class="lblCss" for="lblWorkPackage">Workpackage Name</label>&nbsp;&nbsp;<b>:</b>&nbsp;&nbsp;
                         <asp:Label ID="lblWorkPackage" class="lblCss" Font-Bold="true" runat="server"></asp:Label>
                    </div>
                 </div>
           </div>
            <div class="col-sm-6">
                 <div class="form-group">
                        <label class="lblCss" for="LblDocName">Document Name</label>&nbsp;&nbsp;<b>:</b>&nbs&nbsp;&nbsp;
                         <asp:Label ID="LblDocName" class="lblCss" Font-Bold="true" runat="server"></asp:Label>
                    </div>
                </div>
        </div>

        <div class="row">
            <div class="col-sm-6">
                <div class="form-group">
                       <div class="form-group" >
                         <asp:Label ID="LblTotalDays" class="lblCss" ForeColor="Green" Font-Bold="true" runat="server"></asp:Label>
                    </div>
                 </div>
           </div>
            <div class="col-sm-6">
                 <div class="form-group">
                        
                    </div>
                </div>
            </div>
        <div class="row">
            <div class="col-sm-12">
                <div class="table-responsive">
                        <asp:GridView ID="GrdDocStatus" runat="server" Width="100%" CssClass="table table-bordered" DataKeyNames="StatusUID" EmptyDataText="No Status Found" AutoGenerateColumns="false" OnRowDataBound="GrdDocStatus_RowDataBound" OnRowCommand="GrdDocStatus_RowCommand" OnRowDeleting="GrdDocStatus_RowDeleting">
                        <Columns>
                            <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgProductsShow" runat="server" OnClick="Show_Hide_ProductsGrid" ImageUrl="~/_assets/images/plus.png" 
                                                CommandArgument="Show" Height="25px" Width="25px"/>
                                            <asp:Panel ID="pnlDocuemnt" runat="server" Visible="false" Style="position: relative">
                                                <asp:GridView ID="gvDocumentVersion" runat="server" Width="100%" AutoGenerateColumns="false"  
                                                    AllowPaging="false" CssClass="table table-bordered" OnRowCommand="gvDocumentVersion_RowCommand" OnRowDataBound="gvDocumentVersion_RowDataBound">
                                                    <Columns>
                                                         <asp:BoundField  DataField="Doc_Version" HeaderText="Document Version" />
                                                        <asp:TemplateField HeaderText="Document Type">
                                                            <ItemTemplate>
                                                                <%#GetDocumentType(Eval("Doc_Type").ToString())%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:BoundField  DataField="Doc_StatusDate" HeaderText="Date" DataFormatString="{0:dd MMM yyyy}"/>
                                                          <asp:TemplateField HeaderText="Cover Letter">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkCoverLetterDownload" runat="server" CommandArgument='<%#Eval("DocVersion_UID")%>' CausesValidation="false" CommandName="coverletter">Download</asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Document File">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkDownload" runat="server" CommandArgument='<%#Eval("DocVersion_UID")%>' CausesValidation="false" CommandName="download">Download</asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Comments">
                                                            <ItemTemplate>
                                                                <%#Eval("Doc_Comments")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:BoundField  DataField="Doc_CoverLetter" ItemStyle-CssClass="hiddencol"  HeaderStyle-CssClass="hiddencol"  HeaderText="Cover Letter" />
                                                    </Columns>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                            <asp:BoundField DataField="ActivityType" HeaderText="Phase Name" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ActivityDate" HeaderText="Actual Date" DataFormatString="{0:dd MMM yyyy}">
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Current_Status" HeaderText="Status" HtmlEncode="false" ItemStyle-Width="200px" HeaderStyle-Width="200px">
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Status_Comments" HeaderText="Comments">
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="LinkToReviewFile" ItemStyle-CssClass="hiddencol" HtmlEncode="false"  HeaderStyle-CssClass="hiddencol" HeaderText="LinkToReviewFile" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Link Review File">
                                             <ItemTemplate>
                                                     <asp:LinkButton ID="lnkdown" runat="server" CommandArgument='<%#Eval("StatusUID")%>' CausesValidation="false" CommandName="download">Download</asp:LinkButton>
                                             </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CoverLetterFile" ItemStyle-CssClass="hiddencol"  HeaderStyle-CssClass="hiddencol" HeaderText="CoverLetterFile" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Cover Letter">
                                             <ItemTemplate>
                                                    <asp:LinkButton ID="lnkcoverdownload" runat="server" CommandArgument='<%#Eval("StatusUID")%>' CausesValidation="false" CommandName="Cover Download">Download</asp:LinkButton>
                                             </ItemTemplate>
                             </asp:TemplateField>
                            <asp:TemplateField> 
                                <ItemTemplate>
                                    <a id="UploadDoc" href='/_modal_pages/resubmit-documents.aspx?StatusUID=<%#Eval("StatusUID")%>&DocumentUID=<%#Eval("DocumentUID")%>&ProjectUID=<%#Eval("ProjectUID")%>' class="showuploaddocumentModal">Resubmit Document</a> 
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                  <ItemTemplate>
                                              <asp:LinkButton ID="lnkdelete" runat="server" OnClientClick="return DeleteItem()" CausesValidation="false" CommandArgument='<%#Eval("StatusUID")%>' CommandName="delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
                                   </ItemTemplate>
                             </asp:TemplateField>
                               <asp:BoundField DataField="Forwarded" HeaderText="" ItemStyle-CssClass="hiddencol"  HeaderStyle-CssClass="hiddencol">
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Forwarded" HeaderText="No. Of Days Taken">
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                         <%--   <asp:TemplateField>
                                <ItemTemplate>
                                    <a id="EditStatus" href='/_modal_pages/add-documentstatus.aspx?StatusUID=<%#Eval("StatusUID")%>&DocID=<%#Eval("DocumentUID")%>' class="EditStatus"><span title="Edit" class="fas fa-edit"></span></a>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                    
                        </Columns>
                    </asp:GridView>
                           </div>
                </div>
            </div>
    </div>
    <div class="modal-footer">
                    <%--<button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>--%>
                    <%--<button type="button" class="btn btn-primary">Save changes</button>--%>
           <a id="AddStatus" runat="server" href="/_modal_pages/add-documentstatus.aspx" class="showStatusModal"><asp:Button ID="Button2" runat="server" Height="35px" Width="150px" Text="+ Add Status" CssClass="btn btn-primary"></asp:Button></a>
        </div>

    <%--View document status modal--%>
    <div id="ModAddDocumentStatus" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Status</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:340px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--View document status modal--%>
    <div id="ModResubmitdocument" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Resubmit Document</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:350px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
 </form>
</asp:Content>

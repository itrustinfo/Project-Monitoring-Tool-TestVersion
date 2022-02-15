<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.Workpackage_option._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
    <script type="text/javascript">
        function BindEvents() {
            $(".showModalAddData").click(function(e) {
            e.preventDefault();
            var url = $(this).attr("href");
            $("#ModAddData iframe").attr("src", url);
            $("#ModAddData").modal("show");
            });
        }
        $(document).ready(function () {
            BindEvents();
            //$('#loader').fadeOut();
        });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
    <div class="container-fluid">
            <div class="row">
                <div class="col-md-12 col-lg-12 form-group">Workpackage Option Master</div>
                </div>
        </div>
    <div class="container-fluid">
        <asp:HiddenField ID="Hidden1" runat="server" />
            <div class="row">
                <div class="col-md-6 col-xl-6">
                    <div class="card">
                        <div class="card-body">
                            <div class="form-group">
                            <label class="lblCss" for="txtdesc">Workpackage Option</label>
                                <asp:TextBox ID="txtoption" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                            </div>
                            <div class="form-group">
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                            </div>
                            </div>
                        </div>
                    </div>
                <div class="col-md-6 col-xl-6">
                    <div class="card">
                        <div class="card-body">
                            <div class="table-responsive" id="GridDiv" runat="server">
                            <asp:GridView ID="GrdWorkpackageOptions" runat="server" EmptyDataText="No Options Found." Width="100%" CssClass="table table-bordered" AllowPaging="false" AutoGenerateColumns="False" OnRowCommand="GrdWorkpackageOptions_RowCommand" OnRowEditing="GrdWorkpackageOptions_RowEditing">
                    <Columns>
                        <asp:TemplateField HeaderText="Option">
                            <ItemTemplate>
                                  <%#Eval("Workpackage_OptionName")%>                   
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                               <ItemTemplate>
                                    <a class="showModalAddData" href="/_modal_pages/add-activitydatafromexcel.aspx?OptionUID=<%#Eval("Workpackage_OptionUID")%>">Add Data</a>
                               </ItemTemplate>
                          </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                  <asp:LinkButton ID="lnkedit" runat="server" CommandName="edit" CausesValidation="false" CommandArgument='<%#Eval("Workpackage_OptionUID")%>'><span title="Edit" class="fas fa-edit"></span></asp:LinkButton>
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

    <%--add data modal--%>
    <div id="ModAddData" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Workpackage Activity Data</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:275px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.rabill_summary._default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">

    <style type="text/css">
        .black_overlay {
            display: none;
            position: absolute;
            top: 0%;
            left: 0%;
            width: 100%;
            height: 100%;
            background-color: black;
            z-index: 1001;
            -moz-opacity: 0.8;
            opacity: .80;
            filter: alpha(opacity=80);
        }

        .white_content {
            display: none;
            position: absolute;
            top: auto;
            left: 25%;
            width: 35%;
            padding: 10px;
            border: 8px solid #3498db;
            background-color: white;
            z-index: 1002;
            overflow: auto;
            text-align: justify;
            line-height: 20px;
            box-shadow: 5px 10px #888888;
            font-weight: normal;
            font-size: large;
        }

        .hideItem {
            display: none;
        }
    </style>
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
                 $(".showModal").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddWorkpackages iframe").attr("src", url);
        $("#ModAddWorkpackages").modal("show");
              });

        $(".showModalTask").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddTask iframe").attr("src", url);
        $("#ModAddTask").modal("show");
              });

        $(".showModalSubTask").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddSubTask iframe").attr("src", url);
        $("#ModAddSubTask").modal("show");
          });

         $(".showModalEditTask").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModEditTask iframe").attr("src", url);
        $("#ModEditTask").modal("show");
            });

       $(".showModalMilestone").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddMilestone iframe").attr("src", url);
        $("#ModAddMilestone").modal("show");
          });

        $(".showModalEditMilestone").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("data-href");
        $("#ModEditMilestone iframe").attr("src", url);
        $("#ModEditMilestone").modal("show");
          });

        $(".showModalResource").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddResourceAllocation iframe").attr("src", url);
        $("#ModAddResourceAllocation").modal("show");
            });

            $(".showModalSortActivity").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModSortActivity iframe").attr("src", url);
        $("#ModSortActivity").modal("show");
            });

            $(".showModalCopyActivity").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModCopyActivity iframe").attr("src", url);
        $("#ModCopyActivity").modal("show");
            });

            $(".showModalCopyProjectData").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModCopyProjectData iframe").attr("src", url);
        $("#ModCopyProjectData").modal("show");
            });

            $(".showModalDependency").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddDependency iframe").attr("src", url);
        $("#ModAddDependency").modal("show");
          });

        $(".showModalEditDependency").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("data-href");
        $("#ModEditDependency iframe").attr("src", url);
        $("#ModEditDependency").modal("show");
            });

            $(".showModalTaskSchedule").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddTaskSchedule iframe").attr("src", url);
        $("#ModAddTaskSchedule").modal("show");
            });

            $(".showModalUploadPhotograph").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddUploadSiteImages iframe").attr("src", url);
        $("#ModAddUploadSiteImages").modal("show");
            });

            $(".showModalUploaRaDocument").click(function (e) {
                e.preventDefault();
                var url = $(this).attr("data-href");
                $("#ModUploadRaDocument iframe").attr("src", url);
                $("#ModUploadRaDocument").modal("show");
            });

        }
        $(document).ready(function () {
            BindEvents();
            //$('#loader').fadeOut();
        });
    </script>
    <script type="text/javascript">
        function DeleteItem() {
                                        if (confirm("All data associated with this RA Bill will be deleted. Are you sure you want to delete ...?")) {
                                            return true;
                                        }
                                        return false;
                                    }
                                    function DeleteCategoryItem() {
                                        if (confirm("All data associated with this BOQ will be deleted. Are you sure you want to delete ...?")) {
                                            return true;
                                        }
                                        return false;
                                    }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12 col-lg-4 form-group">RA Bills</div>
            <div class="col-md-6 col-lg-4 form-group">
                <label class="sr-only" for="DDLProject">Project</label>
                <div class="input-group">
                    <div class="input-group-prepend">
                        <span class="input-group-text">Project</span>
                    </div>
                    <asp:DropDownList ID="DDlProject" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDlProject_SelectedIndexChanged"></asp:DropDownList>

                </div>
            </div>
            <div class="col-md-6 col-lg-4 form-group">
                <label class="sr-only" for="DDLWorkPackage">Work Package</label>
                <div class="input-group">
                    <div class="input-group-prepend">
                        <span class="input-group-text">Work Package</span>
                    </div>
                    <asp:DropDownList ID="DDLWorkPackage" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDLWorkPackage_SelectedIndexChanged"></asp:DropDownList>


                </div>
            </div>
        </div>
    </div>

    <div class="container-fluid">
        <%-- <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                    <ContentTemplate>--%>

        <div class="row">

            <div class="col-lg-12 col-xl-12 form-group">

                <div class="card" id="boqsummary" runat="server">
                    <div class="card-body">
                        <div class="card-title">
                            <div class="d-flex justify-content-between">
                                <h6 class="text-muted">
                                    <asp:Label ID="lblBOQName" CssClass="text-uppercase font-weight-bold" runat="server" Text="RABills" />
                                    <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                                </h6>
                                <div>
                                    <asp:Button ID="btnback" runat="server" Text="Back To Dashboard" CssClass="btn btn-primary" PostBackUrl="/_content_pages/dashboard/" Visible="false"></asp:Button>
                                    <a id="AddRABill" runat="server" href="/_modal_pages/add-rabill-rabillitem-invoice.aspx?Type=RABillAdd" class="showModalDependency">
                                        <asp:Button ID="btnAddRABills" runat="server" Text="+ Add RA Bills" CssClass="btn btn-primary"></asp:Button></a>

                                </div>
                            </div>
                        </div>

                        <div class="table-responsive" style="height: 500px; overflow: auto">
                            <asp:GridView ID="GrdTreeView" runat="server" AutoGenerateColumns="False"  CssClass="table table-bordered" EmptyDataText="No Data Found." DataKeyNames="RABillUid" OnRowDataBound="GrdTreeView_RowDataBound"
                                Width="100%" OnRowCancelingEdit="GrdTreeView_RowCancelingEdit" OnRowDeleting="GrdTreeView_RowDeleting" OnRowEditing="GrdTreeView_RowEditing" OnRowUpdating="GrdTreeView_RowUpdating">
                                <Columns>
                                    <asp:TemplateField HeaderText="RA Bill No.">
                                        <ItemTemplate>
                                            <%--<asp:LinkButton ID="lnkview" runat="server" CommandName="view"
                                                        CommandArgument='<%#Eval("RABillUid")%>'><%#Eval("RABillNumber")%></asp:LinkButton>--%>
                                            <a id="EditDependency" href="#" data-href="../../_modal_pages/show-RABills.aspx?RABillUid=<%#Eval("RABillUid")%>&type=add&WorkpackageUID=<%#Eval("WorkpackageUID")%>" class="showModalEditDependency">
                                                <%#Eval("RABillNumber")%></a>
                                            <asp:HiddenField ID="hidrabillDeleteuid" runat="server" Value='<%#Eval("RABillUid")%>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:HiddenField ID="hidrabilluid" runat="server" Value='<%#Eval("RABillUid")%>' />
                                            <asp:TextBox ID="txtRabill" runat="server" Text='<%#Eval("RABillNumber")%>'></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>



                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Bill Value
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%--<%#Eval("Bill_Value")%>--%>
                                            <asp:Label ID="LblBillValue" runat="server"></asp:Label>

                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            RA Bill Date
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%#Eval("RABill_Date","{0:dd MMM yyyy}")%>
                                        </ItemTemplate>

                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Documents" HeaderStyle-Width="10%" ItemStyle-Width="10%" FooterStyle-Width="10%">
                                        <ItemTemplate>
                                            <a id="EditDependency1" href="#" data-href="../../_modal_pages/upload-rabill-document.aspx?RABillUid=<%#Eval("RABillUid")%>&type=add&WorkpackageUID=<%#Eval("WorkpackageUID")%>" class="showModalUploaRaDocument">
                                                <span title="Document" class="fas fa-file"></a>

                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField ShowHeader="False">
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="lnkupdate" runat="server" CausesValidation="True" CommandName="Update" Text="Update"></asp:LinkButton>
                                            &nbsp;<asp:LinkButton ID="lnkcancel" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton>

                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkedit" runat="server" CausesValidation="False" CommandName="Edit"><span title="Edit" class="fas fa-edit"></span></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkdelete" runat="server" CausesValidation="False" CommandName="Delete" OnClientClick="return DeleteItem()" Text="Delete"></asp:LinkButton>
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
    <%--add work package modal--%>
    <div id="ModAddDependency" class="modal it-modal fade">
        <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:Label ID="lblheader" runat="server"> <h5 class="modal-title">Add RA Bill </h5></asp:Label>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
                </div>
                <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 350px;" loading="lazy"></iframe>
                </div>

            </div>
        </div>
    </div>
    <div id="ModEditDependency" class="modal it-modal fade">
        <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">RA Bill Number</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button" onclick="javascript:closepopup();"><span aria-hidden="true">&times;</span></button>
                </div>
                <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 450px;" loading="lazy"></iframe>
                </div>

            </div>
        </div>
    </div>
    <div id="ModUploadRaDocument" class="modal it-modal fade">
        <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Upload Document RA Bill Number</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button" onclick="javascript:closepopup();"><span aria-hidden="true">&times;</span></button>
                </div>
                <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 450px;" loading="lazy"></iframe>
                </div>

            </div>
        </div>
    </div>
</asp:Content>

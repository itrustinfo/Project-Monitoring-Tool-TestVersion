<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-document-flow.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_document_flow" %>

<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <style type="text/css">
        .loadingdiv {
            position: absolute;
            text-align: center;
            vertical-align: middle;
            width: 100%;
            z-index: 250;
        }
    </style>
    <script type="text/javascript">


        function BindEvents() {
            $(".showTaskModal").click(function (e) {
                //document.getElementsByClassName("showTaskModal").click(function(e) {
                e.preventDefault();
                jQuery.noConflict();
                var url = $(this).attr("href");
                $("#ModTask iframe").attr("src", url);
                $("#ModTask").modal("show");
            });
        }
        $(document).ready(function () {
            BindEvents();
        });
    </script>

    <script type="text/javascript">
        $(function () {
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddSubmittalModal" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="container-fluid" style="max-height: 82vh; overflow-y: auto;">
            <asp:HiddenField ID="HiddenParentTask" runat="server" />
            <div class="row">
                <div class="form-group col-sm-6">
                    <label class="lblCss" for="dtStartdate">Submittal Flow</label>
                    &nbsp;<span style="color: red; font-size: 1rem;">*</span>
                    <asp:DropDownList ID="DDLDocumentFlow" runat="server" CssClass="form-control" required AutoPostBack="true" OnSelectedIndexChanged="DDLDocumentFlow_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>

            </div>
            <div class="row" runat="server" id="divStep1">
                <div class="form-group col-sm-6">
                    <label class="lblCss" for="txtdesc">Flow Step 1 Name</label>
                    &nbsp;<span style="color: red; font-size: 1rem;">*</span>
                    <asp:TextBox ID="txtFlowStep1" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>

                </div>
                <div class="form-group col-sm-6">
                    <label class="lblCss" for="txtdesc">Flow Step 1 Duration</label>
                    &nbsp;<span style="color: red; font-size: 1rem;">*</span>
                    <asp:TextBox ID="txtFlowDuration1" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>

                </div>
            </div>
            <div class="row" runat="server" id="divStep2">
                <div class="form-group col-sm-6">
                    <label class="lblCss" for="txtdesc">Flow Step 2 Name</label>
                    &nbsp;<span style="color: red; font-size: 1rem;">*</span>
                    <asp:TextBox ID="txtFlowStep2" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>

                </div>
                <div class="form-group col-sm-6">
                    <label class="lblCss" for="txtdesc">Flow Step 2 Duration</label>
                    &nbsp;<span style="color: red; font-size: 1rem;">*</span>
                    <asp:TextBox ID="txtFlowDuration2" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>

                </div>
            </div>
            <div class="row" runat="server" id="divStep3">
                <div class="form-group col-sm-6">
                    <label class="lblCss" for="txtdesc">Flow Step 3 Name</label>
                    &nbsp;<span style="color: red; font-size: 1rem;">*</span>
                    <asp:TextBox ID="txtFlowStep3" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>

                </div>
                <div class="form-group col-sm-6">
                    <label class="lblCss" for="txtdesc">Flow Step 3 Duration</label>
                    &nbsp;<span style="color: red; font-size: 1rem;">*</span>
                    <asp:TextBox ID="txtFlowDuration3" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>

                </div>
            </div>
            <div class="row" runat="server" id="divStep4">
                <div class="form-group col-sm-6">
                    <label class="lblCss" for="txtdesc">Flow Step 4 Name</label>
                    &nbsp;<span style="color: red; font-size: 1rem;">*</span>
                    <asp:TextBox ID="txtFlowStep4" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>

                </div>
                <div class="form-group col-sm-6">
                    <label class="lblCss" for="txtdesc">Flow Step 4 Duration</label>
                    &nbsp;<span style="color: red; font-size: 1rem;">*</span>
                    <asp:TextBox ID="txtFlowDuration4" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>

                </div>
            </div>
            <div class="row" runat="server" id="divStep5">
                <div class="form-group col-sm-6">
                    <label class="lblCss" for="txtdesc">Flow Step 5 Name</label>
                    &nbsp;<span style="color: red; font-size: 1rem;">*</span>
                    <asp:TextBox ID="txtFlowStep5" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>

                </div>
                <div class="form-group col-sm-6">
                    <label class="lblCss" for="txtdesc">Flow Step 5 Duration</label>
                    &nbsp;<span style="color: red; font-size: 1rem;">*</span>
                    <asp:TextBox ID="txtFlowDuration5" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>

                </div>
            </div>
            <div class="row" runat="server" id="divStep6">
                <div class="form-group col-sm-6">
                    <label class="lblCss" for="txtdesc">Flow Step 6 Name</label>
                    &nbsp;<span style="color: red; font-size: 1rem;">*</span>
                    <asp:TextBox ID="txtFlowStep6" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>

                </div>
                <div class="form-group col-sm-6">
                    <label class="lblCss" for="txtdesc">Flow Step 6 Duration</label>
                    &nbsp;<span style="color: red; font-size: 1rem;">*</span>
                    <asp:TextBox ID="txtFlowDuration6" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>

                </div>
            </div>
            <div class="row" runat="server" id="divStep7">
                <div class="form-group col-sm-6">
                    <label class="lblCss" for="txtdesc">Flow Step 7 Name</label>
                    &nbsp;<span style="color: red; font-size: 1rem;">*</span>
                    <asp:TextBox ID="txtFlowStep7" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>

                </div>
                <div class="form-group col-sm-6">
                    <label class="lblCss" for="txtdesc">Flow Step 7 Duration</label>
                    &nbsp;<span style="color: red; font-size: 1rem;">*</span>
                    <asp:TextBox ID="txtFlowDuration7" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>

                </div>
            </div>
            <div class="row" runat="server" id="divStep8">
                <div class="form-group col-sm-6">
                    <label class="lblCss" for="txtdesc">Flow Step 8 Name</label>
                    &nbsp;<span style="color: red; font-size: 1rem;">*</span>
                    <asp:TextBox ID="txtFlowStep8" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>

                </div>
                <div class="form-group col-sm-6">
                    <label class="lblCss" for="txtdesc">Flow Step 8 Duration</label>
                    &nbsp;<span style="color: red; font-size: 1rem;">*</span>
                    <asp:TextBox ID="txtFlowDuration8" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>

                </div>
            </div>
            <div class="row" runat="server" id="divStep9">
                <div class="form-group col-sm-6">
                    <label class="lblCss" for="txtdesc">Flow Step 9 Name</label>
                    &nbsp;<span style="color: red; font-size: 1rem;">*</span>
                    <asp:TextBox ID="txtFlowStep9" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>

                </div>
                <div class="form-group col-sm-6">
                    <label class="lblCss" for="txtdesc">Flow Step 9 Duration</label>
                    &nbsp;<span style="color: red; font-size: 1rem;">*</span>
                    <asp:TextBox ID="txtFlowDuration9" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>

                </div>
            </div>
            <div class="row" runat="server" id="divStep10">
                <div class="form-group col-sm-6">
                    <label class="lblCss" for="txtdesc">Flow Step 10 Name</label>
                    &nbsp;<span style="color: red; font-size: 1rem;">*</span>
                    <asp:TextBox ID="txtFlowStep10" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>

                </div>
                <div class="form-group col-sm-6">
                    <label class="lblCss" for="txtdesc">Flow Step 10  Duration</label>
                    &nbsp;<span style="color: red; font-size: 1rem;">*</span>
                    <asp:TextBox ID="txtFlowDuration10" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>

                </div>
            </div>
            <div class="row" runat="server" id="divStep11">
                <div class="form-group col-sm-6">
                    <label class="lblCss" for="txtdesc">Flow Step 11 Name</label>
                    &nbsp;<span style="color: red; font-size: 1rem;">*</span>
                    <asp:TextBox ID="txtFlowStep11" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>

                </div>
                <div class="form-group col-sm-6">
                    <label class="lblCss" for="txtdesc">Flow Step 11 Duration</label>
                    &nbsp;<span style="color: red; font-size: 1rem;">*</span>
                    <asp:TextBox ID="txtFlowDuration11" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>

                </div>
            </div>
            <div class="row" runat="server" id="divStep12">
                <div class="form-group col-sm-6">
                    <label class="lblCss" for="txtdesc">Flow Step 12 Name</label>
                    &nbsp;<span style="color: red; font-size: 1rem;">*</span>
                    <asp:TextBox ID="txtFlowStep12" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>

                </div>
                <div class="form-group col-sm-6">
                    <label class="lblCss" for="txtdesc">Flow Step 12 Duration</label>
                    &nbsp;<span style="color: red; font-size: 1rem;">*</span>
                    <asp:TextBox ID="txtFlowDuration12" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>

                </div>
            </div>
            <div class="row" runat="server" id="divStep13">
                <div class="form-group col-sm-6">
                    <label class="lblCss" for="txtdesc">Flow Step 13 Name</label>
                    &nbsp;<span style="color: red; font-size: 1rem;">*</span>
                    <asp:TextBox ID="txtFlowStep13" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>

                </div>
                <div class="form-group col-sm-6">
                    <label class="lblCss" for="txtdesc">Flow Step 13 Duration</label>
                    &nbsp;<span style="color: red; font-size: 1rem;">*</span>
                    <asp:TextBox ID="txtFlowDuration13" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>

                </div>
            </div>
            <div class="row" runat="server" id="divStep14">
                <div class="form-group col-sm-6">
                    <label class="lblCss" for="txtdesc">Flow Step 14 Name</label>
                    &nbsp;<span style="color: red; font-size: 1rem;">*</span>
                    <asp:TextBox ID="txtFlowStep14" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>

                </div>
                <div class="form-group col-sm-6">
                    <label class="lblCss" for="txtdesc">Flow Step 14 Duration</label>
                    &nbsp;<span style="color: red; font-size: 1rem;">*</span>
                    <asp:TextBox ID="txtFlowDuration14" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>

                </div>
            </div>
            <div class="row" runat="server" id="divStep15">
                <div class="form-group col-sm-6">
                    <label class="lblCss" for="txtdesc">Flow Step 15 Name</label>
                    &nbsp;<span style="color: red; font-size: 1rem;">*</span>
                    <asp:TextBox ID="txtFlowStep15" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>

                </div>
                <div class="form-group col-sm-6">
                    <label class="lblCss" for="txtdesc">Flow Step 15 Duration</label>
                    &nbsp;<span style="color: red; font-size: 1rem;">*</span>
                    <asp:TextBox ID="txtFlowDuration15" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>

                </div>
            </div>
            <div class="row" runat="server" id="divStep16">
                <div class="form-group col-sm-6">
                    <label class="lblCss" for="txtdesc">Flow Step 16 Name</label>
                    &nbsp;<span style="color: red; font-size: 1rem;">*</span>
                    <asp:TextBox ID="txtFlowStep16" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>

                </div>
                <div class="form-group col-sm-6">
                    <label class="lblCss" for="txtdesc">Flow Step 16 Duration</label>
                    &nbsp;<span style="color: red; font-size: 1rem;">*</span>
                    <asp:TextBox ID="txtFlowDuration16" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>

                </div>
            </div>
            <div class="row" runat="server" id="divStep17">
                <div class="form-group col-sm-6">
                    <label class="lblCss" for="txtdesc">Flow Step 17 Name</label>
                    &nbsp;<span style="color: red; font-size: 1rem;">*</span>
                    <asp:TextBox ID="txtFlowStep17" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>

                </div>
                <div class="form-group col-sm-6">
                    <label class="lblCss" for="txtdesc">Flow Step 17 Duration</label>
                    &nbsp;<span style="color: red; font-size: 1rem;">*</span>
                    <asp:TextBox ID="txtFlowDuration17" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>

                </div>
            </div>
            <div class="row" runat="server" id="divStep18">
                <div class="form-group col-sm-6">
                    <label class="lblCss" for="txtdesc">Flow Step 18 Name</label>
                    &nbsp;<span style="color: red; font-size: 1rem;">*</span>
                    <asp:TextBox ID="txtFlowStep18" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>

                </div>
                <div class="form-group col-sm-6">
                    <label class="lblCss" for="txtdesc">Flow Step 18 Duration</label>
                    &nbsp;<span style="color: red; font-size: 1rem;">*</span>
                    <asp:TextBox ID="txtFlowDuration18" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>

                </div>
            </div>
            <div class="row" runat="server" id="divStep19">
                <div class="form-group col-sm-6">
                    <label class="lblCss" for="txtdesc">Flow Step 19 Name</label>
                    &nbsp;<span style="color: red; font-size: 1rem;">*</span>
                    <asp:TextBox ID="txtFlowStep19" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>

                </div>
                <div class="form-group col-sm-6">
                    <label class="lblCss" for="txtdesc">Flow Step 19 Duration</label>
                    &nbsp;<span style="color: red; font-size: 1rem;">*</span>
                    <asp:TextBox ID="txtFlowDuration19" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>

                </div>
            </div>
            <div class="row" runat="server" id="divStep20">
                <div class="form-group col-sm-6">
                    <label class="lblCss" for="txtdesc">Flow Step 20 Name</label>
                    &nbsp;<span style="color: red; font-size: 1rem;">*</span>
                    <asp:TextBox ID="txtFlowStep20" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>

                </div>
                <div class="form-group col-sm-6">
                    <label class="lblCss" for="txtdesc">Flow Step 20 Duration</label>
                    &nbsp;<span style="color: red; font-size: 1rem;">*</span>
                    <asp:TextBox ID="txtFlowDuration20" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>

                </div>
            </div>


            <div id="loading" runat="server" visible="false">
                <img src="../_assets/images/progress.gif" width="100" />
            </div>
        </div>

        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
        </div>
        <%--add Submittal modal--%>
        <div id="ModTask" class="modal it-modal fade">
            <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">Link Activity</h5>
                        <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
                    </div>
                    <div class="modal-body">
                        <iframe class="border-0 w-100" style="height: 340px;" loading="lazy"></iframe>
                    </div>

                </div>
            </div>
        </div>

    </form>
</asp:Content>



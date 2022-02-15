<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" ValidateRequest="false" CodeBehind="add-conactivity.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_conactivity" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <script>
  $( function() {
    $("input[id$='dtPaymentDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });

      $("input[id$='txtCAADate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });

    });
     </script>
     <style type="text/css">
        textarea
        {
            resize: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddDocumentModal" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:HiddenField ID="Hidden1" runat="server" />
        <div class="container-fluid" style="min-height:85vh; overflow-y:auto; max-height:85vh;">
            <div class="row">
    <div class="col-sm-6" style="padding-top:20px">
                    <div class="form-group">
                        <label class="lblCss" for="DDLMeeting">Select Meeting</label>
                        <asp:DropDownList ID="ddlMeeting" runat="server" CssClass="form-control" ></asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="DDLProject">Select CP</label>
                        <asp:DropDownList ID="ddlContractPackage" runat="server" CssClass="form-control"  ></asp:DropDownList>
                    </div>
                 
                    <div class="form-group">
                        <label class="lblCss" id="LblProjectNumber" runat="server" for="dtProjectedEndDate">Activity</label>
                        <asp:TextBox ID="txtActivity" CssClass="form-control"  runat="server" autocomplete="off"
                            ClientIDMode="Static" TextMode="MultiLine"></asp:TextBox>
                    </div>
        </div>
                 <div class="col-sm-6" style="padding-top:20px">
                    <div class="form-group">
                        <label class="lblCss" for="dtProjectedEndDate">Status</label>
                        <asp:TextBox ID="txtStatus" CssClass="form-control"  runat="server" TextMode="MultiLine" Height="150px" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                    </div>
                    <%--<div class="form-group">
                        <label class="lblCss" for="txtworkpackage">Payment Date</label>
                       <asp:TextBox ID="dtPaymentDate" runat="server" CssClass="form-control" autocomplete="off" placeholder="dd/mm/yyyy" ClientIDMode="Static"></asp:TextBox>
                    </div>--%>
                   
                    <div class="form-group">    
                          <asp:HiddenField ID="hiduid" runat="server" />
                    </div>
                </div>    
                </div>
            </div>
    <div class="modal-footer">
             <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary" Text="Submit" OnClick="btnAdd_Click"  /> 
             <asp:Button ID="btnUpdate" runat="server" CssClass="btn btn-primary" Text="Update" OnClick="btnUpdate_Click" Visible="false"    /> 
                </div>
        </form>
</asp:Content>
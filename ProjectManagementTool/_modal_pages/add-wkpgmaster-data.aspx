<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-wkpgmaster-data.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_wkpgmaster_data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <script type="text/javascript">
 $( function() {
    $("input[id$='dtoldDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
    });

      $("input[id$='dtnewDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
    });

    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
     <form id="AddMaster" runat="server">
        <div id="divMain" runat="server" class="container-fluid" style="min-height:82vh; overflow-y:auto; max-height:82vh;">
        <div class="row">
            <div class="col-sm-12">
                 <div class="form-group">
                        <label class="lblCss" for="txtoldvalue">Old Value</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                         <asp:TextBox ID="txtoldvalue" runat="server" TextMode="SingleLine" CssClass="form-control" required></asp:TextBox>
                    </div>
                 <div class="form-group">
                        <label class="lblCss" for="txtnewvalue">New Value</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                         <asp:TextBox ID="txtnewvalue" runat="server" TextMode="SingleLine" CssClass="form-control" required></asp:TextBox>
                    </div>
               </div>
            </div>
        </div>
         <div id="divDate" runat="server" class="container-fluid" style="min-height:82vh; overflow-y:auto; max-height:82vh;">
        <div class="row">
            <div class="col-sm-12">
                 <div class="form-group">
                        <label class="lblCss" for="txtoldvalue">Old Value</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                         <asp:TextBox ID="dtoldDate" CssClass="form-control" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                    </div>
                 <div class="form-group">
                        <label class="lblCss" for="txtnewvalue">New Value</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                         <asp:TextBox ID="dtnewDate" CssClass="form-control" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                    </div>
               </div>
            </div>
        </div>

        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                </div>
    </form>
</asp:Content>

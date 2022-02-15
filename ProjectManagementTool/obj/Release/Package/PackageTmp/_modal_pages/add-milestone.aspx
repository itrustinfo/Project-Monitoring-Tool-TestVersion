<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-milestone.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_milestone" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
   <script type="text/javascript">
 $( function() {
    $("input[id$='dtTargetDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });

      $("input[id$='dtprojectDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddMileStoneModal" runat="server">
        
        <div class="container-fluid">
            <div class="row">
                <div class="col-sm-12">
                    
                    <div class="form-group">
                        <label class="lblCss" for="txtMileStone">MileStone</label> &nbsp;<span style="color:red; font-size:1.2rem;">*</span>
                        <asp:TextBox ID="txtMileStone" CssClass="form-control" runat="server" required ClientIDMode="Static"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="ddlStatus">Status</label>
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                            <asp:ListItem Value="Not Completed">Not Completed</asp:ListItem>
                         <asp:ListItem Value="Completed">Completed</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="dtTargetDate">Planned Date</label> &nbsp;<span style="color:red; font-size:1.2rem;">*</span>
                        <asp:TextBox ID="dtTargetDate" CssClass="form-control" runat="server" required placeholder="dd/mm/yyyy" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="dtprojectDate">Projected Date</label> &nbsp;<span style="color:red; font-size:1.2rem;">*</span>
                        <asp:TextBox ID="dtprojectDate" CssClass="form-control" runat="server" required placeholder="dd/mm/yyyy" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                    </div>
                    
                </div>
        </div>
            </div>
        <div class="container-lg">
            <div class="row">
                <div class="col-sm-12">
        <div class="modal-footer">
                    <%--<button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>--%>
                    <%--<button type="button" class="btn btn-primary">Save changes</button>--%>
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
            </div>
                </div>
                </div>
            </div>
    </form>
</asp:Content>

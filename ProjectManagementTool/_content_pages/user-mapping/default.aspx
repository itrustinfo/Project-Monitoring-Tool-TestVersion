<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.user_mapping.deafult" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
    <style type="text/css">
        .lblCss {
            font-size:1rem;font-weight:400;
            padding-left:2px;
            
            overflow-x:hidden;
        }

        .lblCss label 
{  
    padding-left: 5px; 
}
    </style>
    <script type="text/javascript">
        function Cancel() {
            if (confirm("All changes will be discarded .Are you sure you want to cancel ...?")) {
                return true;
            }
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
    <asp:ScriptManager ID="script1" runat="server"></asp:ScriptManager>
      <div class="container-fluid">
            <div class="row">
                <div class="col-md-6 col-lg-3 form-group">User Mapping Assigment</div>
                <div class="col-md-6 col-lg-6 form-group">
                    <label class="sr-only" for="ddlUserType">Select UserType</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Select UserType</span>
                        </div>
                        <asp:DropDownList ID="ddlUserType" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlUserType_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                </div>
              
               
            </div>
          
            
        </div>
      <div class="container-fluid" id="divProgresschart" runat="server" visible="true">
            <div class="row">
                 <div class="col-lg-6 col-xl-3 form-group"></div>
                <div class="col-lg-6 col-xl-6 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="table-responsive">
                             
               
                
                    <label class="lblCss">Select User Functionality</label> 
              
                        <asp:Panel runat="server" ID="panelchkbox" Height="500px" ScrollBars="Auto">
       
                <asp:CheckBoxList ID="chkUserFunctionality" CssClass="lblCss"  runat="server" AutoPostBack="true" OnSelectedIndexChanged="chkUserFunctionality_SelectedIndexChanged"></asp:CheckBoxList>
                    </asp:Panel>
                    
                   
        <br />
        <asp:Button runat="server" Text="Submit" ID="btnSubmit" OnClick="btnSubmit_Click" CssClass="btn btn-primary"></asp:Button>
        <asp:Button runat="server" Text="  Edit  " ID="btnEdit" OnClick="btnEdit_Click" CssClass="btn btn-primary"></asp:Button>&nbsp;&nbsp;&nbsp;
        <asp:Button runat="server" Text="Cancel" ID="btnCancel" OnClientClick="return Cancel()" OnClick="btnCancel_Click" CssClass="btn btn-primary"></asp:Button>
                 
                        </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
   <%--  <div>
         select UserType 
    </div>
    <br />
    <div>
       
    </div>--%>
</asp:Content>

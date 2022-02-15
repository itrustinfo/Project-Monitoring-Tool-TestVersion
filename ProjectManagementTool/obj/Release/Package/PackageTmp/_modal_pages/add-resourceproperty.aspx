<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-resourceproperty.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_resourceproperty" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
     <form id="frmAddResourceProperty" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="container-fluid" style="max-height:82vh; overflow-y:auto; margin-top:15px;">
            <div class="row">
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="lblCss" for="txtpropertyname  ">Property Name</label> &nbsp;<span style="color:red; font-size:1.2rem;">*</span>
                         <asp:TextBox ID="txtpropertyname" runat="server" CssClass="form-control" required></asp:TextBox>
                    </div>
                    
                    <div class="form-group">
                        <label class="lblCss" for="DDLPropertyTableName">Property Table Name</label>
                         <asp:DropDownList ID="DDLPropertyTableName" CssClass="form-control" runat="server">
                         <asp:ListItem Value="Software_Skills">Software Skills</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="form-group">
                        <label class="lblCss" for="DDLunit">Select User</label>
                         <asp:DropDownList ID="DDLUser" CssClass="form-control" runat="server"></asp:DropDownList>
                    </div>
                </div>
                
                <div class="col-sm-6">
                    <div class="form-group">
                        <h6 class="text-muted">
                                   <asp:Label ID="ActivityHeading" CssClass="text-uppercase font-weight-bold" runat="server" Text="List of Resource Properties" />
                                       <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                               </h6>
                    </div>
                    <asp:GridView ID="grdResourceProperty" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" Width="100%">
                <Columns>
                    <asp:BoundField DataField="Name_of_the_Property" HeaderText="Name of the Property" >
                    <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Table_Name" HeaderText="Table Name" >
                    <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                     <asp:TemplateField>
                    <ItemTemplate>
                         <a class="AddSkills" id="AddSkills" href="AddSkills.aspx?type=add&Property_UID=<%#Eval("Property_UID")%>&ResourceUID=<%#Eval("ResourceUID")%>&Table_Name=<%#Eval("Table_Name")%>">AddSkills</a>
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

        <div class="modal-footer" style="text-align:left;">
                    <%--<button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>--%>
                    <%--<button type="button" class="btn btn-primary">Save changes</button>--%>
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                </div>
    </form>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="change-activityorder.aspx.cs" Inherits="ProjectManagementTool._modal_pages.change_activityorder" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
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
  top:auto;
  left: 25%;
  width: 35%;
  padding: 10px;
  border: 8px solid #3498db;
  background-color: white;
  z-index: 1002;
  overflow: auto;
  
  text-align:justify;
  line-height:20px;
  box-shadow: 5px 10px #888888;
  font-weight:normal;
  font-size:large;
}
     .hideItem {
         display:none;
     }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddWorkpackageModal" runat="server">

     <div class="container-fluid" style="max-height:85vh; overflow-y:auto;  min-height:85vh;">
            <div class="row">
                <div class="col-sm-12">
                  <div class="table-responsive">
                                            <asp:GridView ID="GrdTreeView" runat="server" AutoGenerateColumns="false" PageSize="20" 
                                        AllowPaging="false" CssClass="table table-bordered" EmptyDataText="No Data"
                                         Width="100%" >
                                        <Columns>
                                            <asp:TemplateField HeaderText="Order">
                                            <ItemTemplate>
                                                 <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblName" Text='<%#Eval("Name")%>' runat="server" />
                                                    
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            <%--<asp:BoundField  DataField="Name" HeaderText="Name" />--%>
                                            <asp:TemplateField HeaderText="Description" ItemStyle-Width="25%">
                                               <ItemTemplate>
                                                   <asp:Label ID="LblDesc" Text='<%#Eval("Description")%>' runat="server" />
                                                   
                                               </ItemTemplate>
                                           </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Start Date">
                                               <ItemTemplate>
                                                   <asp:Label ID="LblStartDate" Text='<%#Eval("StartDate","{0:dd MMM yyyy}")%>' runat="server" />
                                                   
                                               </ItemTemplate>
                                           </asp:TemplateField>
                                            <asp:TemplateField HeaderText="End Date">
                                               <ItemTemplate>
                                                   <asp:Label ID="LblEndDate" Text='<%#Eval("PlannedEndDate","{0:dd MMM yyyy}")%>' runat="server" />
                                                   
                                               </ItemTemplate>
                                           </asp:TemplateField>
                                           <%-- <asp:BoundField  DataField="StartDate" HeaderText="Contract Start Date" DataFormatString="{0:dd MMM yyyy}"/>
                                              <asp:BoundField DataField="PlannedEndDate" HeaderText="Contract End Date" DataFormatString="{0:dd MMM yyyy}"/>--%>
                                                <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkUp" CommandName="Up" OnClick="ChangePreference"  CssClass="btn-link" runat="server" ><span title="Up" class="fas fa-chevron-circle-up"></span></asp:LinkButton>&nbsp;
                                                    <asp:LinkButton ID="lnkDown" CommandName="Down" OnClick="ChangePreference" CssClass="btn-link"  runat="server"><span title="Down" class="fas fa-chevron-circle-down"></span></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-CssClass="hideItem" ItemStyle-CssClass="hideItem">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTaskUID" runat="server" Text='<%#Eval("TaskUID")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                    No Records Found !
                                </EmptyDataTemplate>
                                        </asp:GridView>
                                            </div>
                    
                </div>
            </div> 

        </div>
        <div class="modal-footer">
                    <%--<button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>--%>
            <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary" Text="Submit" OnClick="btnAdd_Click"  />
                    <%--<button type="button" class="btn btn-primary">Save changes</button>--%>
                </div>
        
        </form>
</asp:Content>

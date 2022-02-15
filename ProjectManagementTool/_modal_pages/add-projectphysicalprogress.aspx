<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-projectphysicalprogress.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_projectphysicalprogress" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <style type="text/css">
         .hideItem {
         display:none;
         
     }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
     <form id="frmUploadSitePhotograph" runat="server">
        <div class="container-fluid" style="max-height:85vh; overflow-y:auto; min-height:82vh;">
            <div class="row">
                <div class="col-sm-12">
                    <div class="form-group">
                        <label class="lblCss" for="txtxsummary">Select Review Meeting</label>
                        <asp:DropDownList ID="DDLMeetingMaster" runat="server" CssClass="form-control" Width="40%">

                        </asp:DropDownList>
                    </div>
                    </div>
                </div>
            <div class="row">
                <div class="col-sm-12">
                    <div class="table-responsive">
                        <asp:GridView ID="GrdPhysicalProgress" runat="server" DataKeyNames="ProjectUID" CssClass="table table-bordered" AutoGenerateColumns="False" PageSize="10" EmptyDataText="No Data" Width="100%">
                                <Columns>
                                    <asp:TemplateField HeaderText="Contract Package">
                                        <ItemTemplate>
                                            <asp:Label ID="LblProjectName" runat="server" Text='<%#GetProjectName(Eval("ProjectUID").ToString())%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Name of the Package">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtnameofthepackage" runat="server" CssClass="form-control" required></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Awarded/Sanctioned excluding Provisional Sum and Physical Contingencies(Rs.Cr)">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtawarded_SanctionedValue" runat="server" CssClass="form-control" required></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status of Award">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtStatusofAward" runat="server" CssClass="form-control" required></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Expenditure til Today" FooterStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtExpenditureasonDate" runat="server" CssClass="form-control" required></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Targeted Physical Progress(%)">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtTragetedPhysicalprogress" runat="server" required CssClass="form-control"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Targeted Overall Weighted Progress(%)">
                                        <ItemTemplate>                                            
                                            <asp:TextBox ID="txtTargeted_Overall_WeightedProgress" runat="server" required CssClass="form-control"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Achieved Physical Progress(%)">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAchieved_PhysicalProgress" runat="server"  required CssClass="form-control"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Achieved Overall Weighted Progress(%)">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAchieved_Overall_WeightedProgress" runat="server" required CssClass="form-control"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ProjectUID" HeaderText="Path" ItemStyle-CssClass="hideItem" HeaderStyle-CssClass="hideItem">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                </Columns>
                             </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
         <div class="modal-footer">
                    <%--<button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>--%>
            <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary" Text="Submit" OnClick="btnAdd_Click" />
                    <%--<button type="button" class="btn btn-primary">Save changes</button>--%>
                </div>
         </form>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="edit-projectphysicalprogress.aspx.cs" Inherits="ProjectManagementTool._modal_pages.edit_projectphysicalprogress" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmEditProjectPhysicalProgress" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="container-fluid" style="max-height:82vh; overflow-y:auto;">
            <div class="row">
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="lblCss" for="txtxsummary">Select Review Meeting</label>
                        <asp:DropDownList ID="DDLMeetingMaster" runat="server" CssClass="form-control" Enabled="false">
                        </asp:DropDownList>
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="txtnameofthepackage">Name of the Package</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                         <asp:TextBox ID="txtnameofthepackage" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label class="lblCss" for="txtawarded_SanctionedValue">Awarded/Sanctioned excluding Provisional Sum and Physical Contingencies(Rs.Cr)</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                         <asp:TextBox ID="txtawarded_SanctionedValue" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label class="lblCss" for="txtStatusofAward">Status of Award</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                         <asp:TextBox ID="txtStatusofAward" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                    </div>

                   <div class="form-group">
                        <label class="lblCss" for="txtexpenditure">Expenditure til Today(Rs. Cr)</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                         <asp:TextBox ID="txtexpenditure" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                    </div>
                </div>
                <div class="col-sm-6">
                     <div class="form-group">
                        <label class="lblCss" for="txtTragetedPhysicalprogress">Targeted Physical Progress(%)</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="txtTragetedPhysicalprogress" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                        
                    </div>

                      <div class="form-group">
                        <label class="lblCss" for="txtTargeted_Overall_WeightedProgress">Targeted Overall Weighted Progress(%)</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="txtTargeted_Overall_WeightedProgress" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                        
                    </div>
                      <div class="form-group">
                        <label class="lblCss" for="txtAchieved_PhysicalProgress">Achieved Physical Progress(%)</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="txtAchieved_PhysicalProgress" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                        
                    </div>
                      <div class="form-group">
                        <label class="lblCss" for="txtAchieved_Overall_WeightedProgress">Achieved Overall Weighted Progress(%)</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="txtAchieved_Overall_WeightedProgress" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                        
                    </div>
                    </div>
            </div> 
        </div>

        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click"/>
                </div>
    </form>
</asp:Content>

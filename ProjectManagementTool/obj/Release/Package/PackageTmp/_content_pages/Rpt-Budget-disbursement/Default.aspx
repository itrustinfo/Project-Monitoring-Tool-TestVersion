<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ProjectManagementTool._content_pages.Rpt_Budget_disbursement.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
    <div class="container-fluid">
            <div class="row">
                <div class="col-md-6 col-lg-4 form-group">
                    <label class="sr-only" for="DDLProject">Select Meeting</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Select Meeting</span>
                        </div>
                        <asp:DropDownList ID="ddlmeeting" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlmeeting_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
     <div class="container-fluid">
            <div class="row">
                <div class="col-lg-12 col-xl-12 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="table-responsive">
                            <asp:HiddenField ID="ReviewMeetingDate" runat="server" />
                            <h3 id="heading" runat="server">Status of Budget Vs Disbursement as on 05.01.2021</h3>
                                <asp:GridView ID="GrdBudgetbsDisbursemnt" runat="server" HeaderStyle-HorizontalAlign="Center" CssClass="table table-bordered" EmptyDataText="No Data" AutoGenerateColumns="False" Width="100%" OnDataBound="GrdBudgetbsDisbursemnt_DataBound" OnRowDataBound="GrdBudgetbsDisbursemnt_RowDataBound" AlternatingRowStyle-BackColor="lightGray">
                                <Columns>
                                      <asp:BoundField DataField="ProjectName" ItemStyle-HorizontalAlign="Center" HeaderText="Contract Package" >
                            
                            </asp:BoundField>
                                     <asp:BoundField DataField="ContractorName" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="150px" HeaderText="Consultant/ <br/>Contractor" HtmlEncode="false">
                            
                            </asp:BoundField>
                                     <asp:BoundField DataField="AwardedCost" ItemStyle-HorizontalAlign="Right" HeaderText="Awarded Cost <br/>Excl. Provisional Sum and <br/>Contingencies Rs. Crores" HtmlEncode="False" DataFormatString="{0:C2}">
                            
                            </asp:BoundField>
                                     <asp:BoundField DataField="Disbursement_Amount" ItemStyle-HorizontalAlign="Right" HeaderText="Disbursement up to <br/> FY 2019-20 MJPY" HtmlEncode="False" DataFormatString="{0:C2}">
                            
                            </asp:BoundField>
                                    <asp:BoundField DataField="Disbursement_Amount_2021" ItemStyle-HorizontalAlign="Right" HeaderText="Disbursement up to <br/> FY 2020-21 MJPY" HtmlEncode="False" DataFormatString="{0:C2}">
                            
                            </asp:BoundField>
                                     <asp:BoundField DataField="Q1_Budget_Amount" HeaderText="Q1" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right">
                            
                            </asp:BoundField>
                                    <asp:BoundField DataField="Q2_Budget_Amount" HeaderText="Q2" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right">
                            
                            </asp:BoundField>
                                    <asp:BoundField DataField="Q3_Budget_Amount" HeaderText="Q3" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right">
                            
                            </asp:BoundField>
                                    <asp:BoundField DataField="Q4_Budget_Amount" HeaderText="Q4" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right">
                            
                            </asp:BoundField>
                            <asp:BoundField DataField="Q1_Actual_Amount" HeaderText="Q1" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right">
                            
                            </asp:BoundField>
                                      <asp:BoundField DataField="Q2_Actual_Amount" HeaderText="Q2" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right">
                            
                            </asp:BoundField>
                                      <asp:BoundField DataField="Q3_Actual_Amount" HeaderText="Q3" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right">
                            
                            </asp:BoundField>
                                      <asp:BoundField DataField="Q4_Actual_Amount" HeaderText="Q4" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right">
                            
                            </asp:BoundField>
                                     <asp:BoundField DataField="TotalDisburseAmnt" HeaderText="Total Disbursement" ItemStyle-HorizontalAlign="Right" ItemStyle-Font-Bold="true" HtmlEncode="False" DataFormatString="{0:C2}">
                            
                            </asp:BoundField>
                                </Columns>
                                </asp:GridView>
                             <div class="text-right">
                                <asp:Button ID="btnexport" runat="server" Text="Export to PDF" CssClass="btn btn-primary" OnClick="btnexport_Click" />
                            </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
</asp:Content>

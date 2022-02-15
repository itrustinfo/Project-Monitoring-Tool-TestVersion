<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ProjectManagementTool._content_pages.Rpt_Bwssb_Jica.Default" %>
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
                       <h3 id="heading" runat="server">Status of BWSSB Payment vs JICA Disbursement as on 05.01.2021</h3>
                        <div class="table-responsive">
                                <asp:HiddenField ID="ReviewMeetingDate" runat="server" />
                                <asp:GridView ID="GrdBudgetbsDisbursemnt" runat="server" HeaderStyle-HorizontalAlign="Center" CssClass="table table-bordered" EmptyDataText="No Data" AutoGenerateColumns="False" Width="100%" OnDataBound="GrdBudgetbsDisbursemnt_DataBound" AlternatingRowStyle-BackColor="lightGray">
                                <Columns>
                                      <asp:BoundField DataField="ProjectName" ItemStyle-HorizontalAlign="Center" HeaderText="Consultant/
Contractor" >
                           
                            </asp:BoundField>
                                     <asp:BoundField DataField="ContractorName" ItemStyle-HorizontalAlign="Left" HeaderText="ContractorName" >
                           
                            </asp:BoundField>
                                     <asp:BoundField DataField="AwardedCost" ItemStyle-HorizontalAlign="Right" HeaderText="Awarded Cost Excl. Provisional <br/>Sum and Contingencies Rs. Crores" HtmlEncode="False" DataFormatString="{0:C2}">
                            
                            </asp:BoundField>
                                     <asp:BoundField DataField="Q1_Payment_Amount" HeaderText="Q1" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right">
                            
                            </asp:BoundField>
                                    <asp:BoundField DataField="Q2_Payment_Amount" HeaderText="Q2" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right">
                            
                            </asp:BoundField>
                                    <asp:BoundField DataField="Q3_Payment_Amount" HeaderText="Q3" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right">
                            
                            </asp:BoundField>
                                    <asp:BoundField DataField="Q4_Payment_Amount" HeaderText="Q4" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right">
                            
                            </asp:BoundField>
                            <asp:BoundField DataField="Q1_Actual_Amount" HeaderText="Q1" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right">
                            
                            </asp:BoundField>
                                      <asp:BoundField DataField="Q2_Actual_Amount" HeaderText="Q2" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right">
                           
                            </asp:BoundField>
                                      <asp:BoundField DataField="Q3_Actual_Amount" HeaderText="Q3" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right">
                           
                            </asp:BoundField>
                                      <asp:BoundField DataField="Q4_Actual_Amount" HeaderText="Q4" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right">
                           
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

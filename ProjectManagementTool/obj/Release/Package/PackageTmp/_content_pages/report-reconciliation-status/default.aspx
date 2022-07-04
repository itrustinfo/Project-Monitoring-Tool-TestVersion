<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.report_reconciliation_status._default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
    <style type="text/css">
         .hideItem {
         display:none;
     }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-6 col-lg-4 form-group">Reconciliation Approved or Rejected Report</div>
            <div class="col-md-6 col-lg-4 form-group">
                <label class="sr-only" for="DDLProject">Project</label>
                <div class="input-group">
                    <div class="input-group-prepend">
                        <span class="input-group-text">Project</span>
                    </div>
                    <asp:DropDownList ID="DDlProject" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDlProject_SelectedIndexChanged"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-6 col-lg-4 form-group">
                <label class="sr-only" for="DDLWorkPackage">Work Package</label>
                <div class="input-group">
                    <div class="input-group-prepend">
                        <span class="input-group-text">Work Package</span>
                    </div>
                    <asp:DropDownList ID="DDLWorkPackage" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>
            </div>
        </div>
      
    </div>
      



        <div class="container-fluid" id="ReportFilter" runat="server">
            <div class="row">
                <div class="col-sm-12 mb-4" runat="server">
                     <div class="card">
                        <div class="card-body" >   
                            <div class="table-responsive">
                            <table style="width:100%;">
                                <tr>
                                    <td>
                                         <h6 class="card-title text-muted text-uppercase font-weight-bold">ONTB Reference #</h6>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <h6 class="card-title text-muted text-uppercase font-weight-bold">Originator Reference #</h6>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <h6 class="card-title text-muted text-uppercase font-weight-bold">Status</h6>
                                    </td>
                                     <td>&nbsp;</td>
                                    <td>
                                        <h6 class="card-title text-muted text-uppercase font-weight-bold">Flow</h6>
                                    </td>
                                </tr>
                             <tr>
                                 <td style="width:22%;">
                                     <asp:TextBox ID="txtOntbReference" runat="server" CssClass="form-control" ></asp:TextBox>
                                </td>
                                 <td>&nbsp;</td>
                                 <td style="width:22%;">
                                     <asp:TextBox ID="txtProjectRefernce" runat="server" CssClass="form-control" ></asp:TextBox>
                                 </td>
                                 <td>&nbsp;</td>
                                 <td style="width:22%;">
                                     <asp:DropDownList ID="DDLStatus" runat="server" CssClass="form-control" ></asp:DropDownList>
                                 </td>
                                 <td>&nbsp;</td>
                                 <td style="width:22%;">
                                     <asp:DropDownList ID="DDLFlow" runat="server" CssClass="form-control" ></asp:DropDownList>
                                 </td>
                                 <td style="width:7%;">
                                     <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary" Text="Submit" OnClick="btnSubmit_Click" />
                                 </td>
                             </tr>
                        </table>
                                </div>
                            </div>
                         </div>
                    </div>
                </div>
        </div>



        <div class="container-fluid" id="divTabular" runat="server" visible="false">
            <div class="row">
                <div class="col-lg-12 col-xl-12 form-group">
                    <div class="card mb-4">
                        <div class="card-body">
                            <div class="card-title">
                                <div class="d-flex justify-content-between">
                                    <h6 class="text-muted">
                                        <%--<asp:Label ID="ActivityHeading" CssClass="text-uppercase font-weight-bold" runat="server" Text="Contract Details" />--%>
                                    </h6>
                                    <div>
                                        <asp:Button ID="btnExcel" runat="server" Text="Export to Excel" CssClass="btn btn-primary" Visible="true" OnClick="btnExcel_Click" />&nbsp;
                                        <asp:Button ID="btnPDF" runat="server" Text="Export to PDF" CssClass="btn btn-primary" Visible="true" OnClick="btnPDF_Click" />&nbsp;
                                        <asp:Button ID="tnPrint" runat="server" Text="Print" CssClass="btn btn-primary" Visible="true" OnClick="tnPrint_Click" />
                                    </div>
                                </div>
                            </div>
                            <div class="table-responsive">
                                <div id="divsummary" runat="server" visible="true">
                                    <div style="overflow: hidden;" id="DivHeaderRowNew"></div>
                                    <div style="overflow: scroll;" onscroll="OnScrollDivNew(this)" id="DivMainContentNew">
                                        <asp:GridView ID="grdDataList" EmptyDataText="No Data Found." runat="server" Width="100%" AutoGenerateColumns="false" CellPadding="6" CellSpacing="16" HeaderStyle-BackColor="#666666" HeaderStyle-ForeColor="White" AlternatingRowStyle-BackColor="#F5F5F5"
                                            CssClass="table table-bordered" OnRowDataBound="grdDataList_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="SerialNo"  HeaderText="Serial No" >
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>

                                            <asp:BoundField DataField="ProjectName"  HeaderText="Project Name" >
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                                <asp:BoundField DataField="ActualDocumentUID"  HeaderText="UID" ItemStyle-CssClass="hideItem" HeaderStyle-CssClass="hideItem">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="DocName" HeaderText="Submittal Name">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ActualDocument_Name" HeaderText="Document Name">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ActualDocument_Type" HeaderText="Document Type">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ActualDocument_CurrentStatus" HeaderText="Current Status">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ProjectRef_Number" HeaderText="Ontb Reference #">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Ref_Number" HeaderText="Originator Reference #">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Flow_Name" HeaderText="Flow Name" >
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Document_Date" HeaderText="Contractor Uploaded Date" SortExpression="Document_Date" DataFormatString="{0:dd/MM/yyyy}">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CreatedDate" HeaderText="ONTB Accepted/Rejected Date" SortExpression="CreatedDate" DataFormatString="{0:dd/MM/yyyy}">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Remarks" HeaderText="Remarks" SortExpression="Remarks">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>

                                                
                                                

                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div id="DivFooterRowNew" style="overflow: hidden"></div>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

</asp:Content>


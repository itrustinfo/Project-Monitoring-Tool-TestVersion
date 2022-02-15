<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.status_wastewater._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
    <div class="container-fluid">
        <div class="row">
            <div class="col-lg-6 col-xl-8 form-group">Status of Waste Water Contract Packages</div>
            <div class="col-lg-6 col-xl-4 form-group">
                
            </div>
        </div>
    </div>
    <div class="container-fluid">
        <div class="row">
            <div class="col-lg-6 col-xl-12 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="card-title">
                            <div class="d-flex justify-content-between">
                                <h6 class="text-muted">
                                    <asp:Label Text="Status of Waste Water Contract Packages" CssClass="text-uppercase font-weight-bold" runat="server" />
                                    <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                                </h6>
                            </div>
                        </div>
                        <div class="table-responsive" id="tbData" runat="server">

                            <table class="table table-bordered" border="1" >
                                <thead style="font-weight:bold;">
                                    <tr>
                                        <td>Contract Packages</td>
                                        <td>Package Description</td>
                                        <td>Awarded Cost / Sanction Cost excluding Provisional Sum and Physical Contingency (Rs. Million)</td>
                                        <td>Project Components</td>
                                        <td>Present Status</td>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td colspan="4">
                                            <b>Sewerage Component</b>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            CP 25
                                        </td>
                                        <td>STP, ISPS and Sewer network in Hebbal Valley</td>
                                        <td>2370.00</td>
                                        <td> 4 no. of STPs of 17 MLD, 2 x 7 MLD and 6 MLD.2 no. of ISPSs of 3 MLD and 0.9 MLD</td>
                                        <td>Bids Received on 27.10.2020.
                                                4 Bidders. Technical Bid Evaluation is under process.<br />
                                                1. L&T<br />
                                                2. Ramky Environment<br />
                                                3. Triveni <br />
                                                4. Passavant India + GmBH JV
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            CP 26
                                        </td>
                                        <td>STP, ISPS and Sewer network in K & C Valley</td>
                                        <td>3850.00</td>
                                        <td> 3 no. of STPs of 25 MLD, 5 MLD and 4 MLD.
                                        2 no. of ISPSs of 15 MLD and 9 MLD 
                                        </td>
                                        <td>Bids Received on 28.10.2020.4 Bidders. Technical Bid Evaluation is under process.<br />
                                                1. L&T<br />
                                                2. Ramky Environment<br />
                                                3. Ashoka B + Gondowana JV <br />
                                                4. LC Indra + SN Enviro Tech JV
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            CP 27
                                        </td>
                                        <td>STP, ISPS and Sewer network in V Valley</td>
                                        <td>2850.00</td>
                                        <td>7 no. of STPs with  capacities 13, 10, 9, 8, 6, 4, 3 MLD.4 no. of ISPSs of capacities 8.1, 1.6, 1.1, 0.5 MLD</td>
                                        <td>Bids Received on 29.10.2020.3 Bidders. Technical Bid Evaluation is under process.<br />
                                                1. L&T<br />                                                
                                                2. Triveni <br />
                                                3. KBR + AIPL JV
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <b>MIS Component</b>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            CP 24
                                        </td>
                                        <td>MIS and SCADA</td>
                                        <td>140</td>
                                        <td>Centralized SCADA and MIS for water and sewerage components </td>
                                        <td>DDR and Bid Document preparation under progress.
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                           
                        </div>
   
                        
                   
           
        <div class="text-right">
                            <asp:Button ID="btnExportPDF" runat="server" Text="Export to PDF" CausesValidation="false" CssClass="btn btn-primary" OnClick="btnExportPDF_Click" />
                                    </div>
                     </div>
               </div>
                 </div>
             </div>
            </div>
                             
</asp:Content>

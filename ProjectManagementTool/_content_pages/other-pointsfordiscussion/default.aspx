<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs"
    Inherits="ProjectManagementTool._content_pages.other_pointsfordiscussion._default" EnableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
     
    <style type="text/css">
    .header-center{
        text-align:center;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
    <div class="container-fluid">
        <div class="row">
            <div class="col-lg-6 col-xl-8 form-group">Other points for Discussion</div>
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
                                    <asp:Label Text="Other points for Discussion" CssClass="text-uppercase font-weight-bold" runat="server" />
                                    <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                                </h6>
                            </div>
                        </div>
                        <div class="table-responsive" >

                          <%--  <ul style="line-height:35px; font-size:1.1rem;" border="1">
                                <li>
                                    Request from contractors for reduction of Performance Security BG’s from 10% to 3% for all contracts due to relaxation allowed by GOI
                                </li>
                                <li>
                                    Revision of intermediate milestones of the contract due to COVID 19

                                </li>
                                <li>
                                    Varthur STP (under CP26) Land Issue
                                </li>
                                <li>
                                    CP-12 and CP – 13 Contract concurrance

                                </li>
                            </ul>--%>
                            <label class="lblCss" for="txtxsummary">Select Meeting </label>
                            <asp:DropDownList ID="DDLMeetingMaster" runat="server" CssClass="form-control" Width="30%" AutoPostBack="true" OnSelectedIndexChanged="DDLMeetingMaster_SelectedIndexChanged">

                            </asp:DropDownList>
                                <br />

                                           
      
                            <br /><br />
                            
                            <asp:GridView ID="grdOtherPoins" runat="server" HeaderStyle-Font-Bold="true" CssClass="table table-bordered"  AutoGenerateColumns="False" EmptyDataText="No Data" Width="100%" OnRowDataBound="grdOtherPoins_RowDataBound">
                                <Columns>
                                  
                                    <asp:BoundField DataField="points" HeaderText="Sl. No." ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true" />
                                    <asp:BoundField DataField="points" HeaderText="Other Points for discussion" />

                                   <%-- <asp:TemplateField HeaderText="Other Points for discussion" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                       <asp:Label ID="LblRemarks" runat="server" Text=' <%#Eval("points")%>' />
                                          
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    
                                </Columns>
                             </asp:GridView>
                       
                        </div>

                         <div class="text-right">
                            <asp:Button ID="btnexport" runat="server" Text="Export to PDF" CausesValidation="false" CssClass="btn btn-primary" OnClick="btnexport_Click" />
                                    </div>
                    </div>
                </div>
            </div>
            </div>
        </div>
     
</asp:Content>

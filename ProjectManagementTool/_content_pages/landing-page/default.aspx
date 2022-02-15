<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.landing_page._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
     <div class="container-fluid">
            <div class="row">
                 <div class="col-lg-6 col-xl-3 form-group">
                     <div class="card" style="height:100%;">
                                        <div class="card-body">
                                            <h6 class="card-title text-muted text-uppercase font-weight-bold">Select Meeting</h6>
                                            <asp:DropDownList ID="ddlmeeting" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlmeeting_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                         </div>
                     </div>
                <div class="col-md-6 col-lg-9" >
                    <div class="card" style="height:100%; overflow-y:hidden;">
                        <div class="card-body">
                            <div style="width:90%; margin:auto;">
                                <div style="float:left; width:45%; margin-left:5%;">
                                <img src="../../_assets/images/Picture5.png" width="135" alt="Bangalore Water Supply & Sewerage Board" />
                            </div>
                            <div style="float:left; width:45%; text-align:right; vertical-align:middle; padding-top:20px; margin-right:5%;">
                                <img src="../../_assets/images/Picture6.png" width="135" alt="JICA" />
                            </div>
                            </div>
                            
                            <div style=" width:100%; text-align:center; margin:auto;">
                                    <img src="../../_assets/images/Picture7.png" alt="JICA"  width="90%" />
                                <br /><br />
                                <h3 style="font-family:Arial; font-weight:bold; color:#0066FF;" id="heading" runat="server">Progress Review Meeting on 16.01.2021</h3>
                                <%--<img src="../../_assets/images/Picture8.png" alt="" width="75%" />--%>
                                </div>
                            </div>
                        </div>
                   
                    </div>
                
                </div>
                </div>

         
</asp:Content>

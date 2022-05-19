<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-boqdetails.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_boqdetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="FrmAddResource" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="container-fluid" style="max-height:80vh; overflow-y:auto; margin-top:15px;">
            <div class="row">
                <div class="col-sm-6">
                    <asp:HiddenField ID="hidUid" runat="server" /> 
                    <%--<div class="form-group">
                        <label class="lblCss" for="ddlItems">Parent Item Id</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                       
                        <asp:TextBox ID="txtitem" runat="server" CssClass="form-control" autocomplete="off"  Enabled="false"></asp:TextBox>
                        
                    </div>--%>
                    <div class="form-group">
                        <label class="lblCss" for="txtitemnumber">Item_Number</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                         <asp:TextBox ID="txtitemnumber" runat="server" CssClass="form-control" autocomplete="off" required></asp:TextBox>
                    </div>
                   
                    <div class="form-group">
                        <label class="lblCss" for="DDLUnit">Unit for Measurement</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                         <input type="text" id="txtUnit" runat="server" class="form-control" autocomplete="off" required />
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="txtquantity">Quantity</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                         <input type="text" id="txtquantity" runat="server" class="form-control" autocomplete="off" required />
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtprice">Local(INR)-Rate</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <input type="text" id="txtInrRate" runat="server" class="form-control" autocomplete="off" required  onblur="toUSD(this)"/>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtprice">Duties</label> &nbsp;<span style="color:red; font-size:1.1rem;"></span>
                        <input type="text" id="txtDuties" runat="server" class="form-control" autocomplete="off" />
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtprice">Ex Works</label> &nbsp;<span style="color:red; font-size:1.1rem;"></span>
                        <input type="text" id="txtExWorks" runat="server" class="form-control" autocomplete="off" />
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtprice">Local Transport</label> &nbsp;<span style="color:red; font-size:1.1rem;"></span>
                        <input type="text" id="txtLocalTransport" runat="server" class="form-control" autocomplete="off" />
                    </div>
                    
                </div>
                <div class="col-sm-6">
                    
                    
                  
                     <div class="form-group">
                        <label class="lblCss" for="txtprice">Foreign(USD)-Rate</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <input type="text" id="txtForgignUsdRate" runat="server" class="form-control" autocomplete="off" required  onblur="toUSD(this)"/>
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="txtprice">Foreign(JPY)-Rate</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <input type="text" id="txtForgignJpyRate" runat="server" class="form-control" autocomplete="off" required  onblur="toUSD(this)"/>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtprice">Local(INR)-Amount</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <input type="text" id="txtLocalInrAmount" runat="server" class="form-control" autocomplete="off" required  onblur="toUSD(this)"/>
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="txtprice">Foreign(USD)-Amount</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <input type="text" id="txtforeignUsdAmount" runat="server" class="form-control" autocomplete="off" required  onblur="toUSD(this)"/>
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="txtprice">Foreign(JPY)-Amount</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <input type="text" id="txtforeignJpyAmount" runat="server" class="form-control" autocomplete="off" required  onblur="toUSD(this)"/>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtprice">GST</label> &nbsp;<span style="color:red; font-size:1.1rem;"></span>
                        <input type="text" id="txtGST" runat="server" class="form-control" autocomplete="off"/>
                    </div>
                   
                    </div>
                 <div class="col-sm-12">
                      <div class="form-group">
                        <label class="lblCss" for="txtdesc">Description</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                         <asp:TextBox ID="txtdesc" runat="server" CssClass="form-control" TextMode="MultiLine" autocomplete="off" required></asp:TextBox>
                    </div>
                     </div>
            </div> 
        </div>

        <div class="modal-footer">
                    <%--<button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>--%>
                    <asp:Button CssClass="btn btn-primary" id="btnUpdate" runat="server" Text="Update" Visible="false" OnClick="btnUpdate_Click"></asp:Button>
            <asp:Button ID="btnSubmit" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSubmit_Click"  />
                </div>
    </form>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="view-taskpaymenthistroy.aspx.cs" Inherits="ProjectManagementTool._modal_pages.view_taskpaymenthistroy" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <style type="text/css">
         .hideItem {
         display:none;
     }
    </style>
    <script type="text/javascript"> 
        $(function () {
           
            $("[id*=imgProductsShow]").each(function () {
                if ($(this)[0].src.indexOf("minus") != -1) {
                    $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>");
                    $(this).next().remove();
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddDocumentModal" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:HiddenField ID="Hidden1" runat="server" />
        <div class="container-fluid" style="min-height:85vh; overflow-y:auto; max-height:85vh;">
            <div class="row">
                <div class="col-sm-12">
                    <div class="table-responsive">
                    <asp:GridView ID="GrdFinanceMileStone" runat="server" AllowPaging="false" DataKeyNames="Finance_MileStoneUID" AllowCustomPaging="false" AutoGenerateColumns="False" CssClass="table table-bordered" Width="100%" OnRowDataBound="GrdFinanceMileStone_RowDataBound">
                     <Columns>
                         <asp:TemplateField>
                             <ItemTemplate>
                                 <asp:ImageButton ID="imgProductsShow" runat="server" OnClick="Show_Hide_ProductsGrid" ImageUrl="~/_assets/images/plus.png" 
                                                CommandArgument="Show" Height="25px" Width="25px"/>
                                    <asp:Panel ID="pnlPaymentHistory" runat="server" Visible="false" Style="position: relative">
                                        </asp:Panel>
                                     <asp:GridView ID="GrdPaymentUpdate" runat="server" AllowPaging="false" AutoGenerateColumns="false" Width="100%" CssClass="table table-bordered" OnRowCreated="GrdPaymentUpdate_RowCreated">
                                     <Columns>
                                         <asp:BoundField DataField="Actual_PaymentDate" HeaderText="Payment Date" DataFormatString="{0: dd MMM yyyy}" >
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                         <asp:TemplateField HeaderText="Actual Payment">
                                            <ItemTemplate>
                                                <span style="color:#006699;"><%#Eval("Currency")%></span>&nbsp;<%#string.Format(new System.Globalization.CultureInfo(Eval("Currency_CultureInfo").ToString()),"{0:N}", Eval("Actual_Payment"))%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                     </Columns>
                                 </asp:GridView>
                                        
                             </ItemTemplate>
                         </asp:TemplateField>
                         <asp:BoundField DataField="Finance_MileStoneName" HeaderText="MileStone Name" >
                          <HeaderStyle HorizontalAlign="Left" />
                          </asp:BoundField>
                         <asp:BoundField DataField="Finance_PlannedDate" HeaderText="Planned Date"  DataFormatString="{0:dd MMM yyyy}">
                            <HeaderStyle HorizontalAlign="Left" />
                          </asp:BoundField>
                           <asp:TemplateField HeaderText="Allowed Payment">
                               <ItemTemplate>
                                       <span style="color:#006699;"><%#Eval("Currency")%></span>&nbsp;<%#string.Format(new System.Globalization.CultureInfo(Eval("Currency_CultureInfo").ToString()),"{0:N}", Eval("Finance_AllowedPayment"))%> + GST(<%#Eval("Finance_GST")%> %)
                             </ItemTemplate>
                          </asp:TemplateField>
                                                <asp:BoundField DataField="Currency" HeaderText="Currency" HeaderStyle-CssClass="hideItem" ItemStyle-CssClass="hideItem">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle CssClass="hideItem"></ItemStyle>
                                                        </asp:BoundField>
                                                <asp:BoundField DataField="Currency_CultureInfo" HeaderText="Currency_CultureInfo" HeaderStyle-CssClass="hideItem" ItemStyle-CssClass="hideItem">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle CssClass="hideItem"></ItemStyle>
                                                        </asp:BoundField>
                     </Columns>
                     <EmptyDataTemplate>
                              <strong>No Records Found ! </strong>
                     </EmptyDataTemplate>
                     </asp:GridView>
                        </div>
                </div> 
        </div>
            </div>
        <div class="modal-footer">
            <%--<asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" />--%>
                </div>
    </form>
</asp:Content>

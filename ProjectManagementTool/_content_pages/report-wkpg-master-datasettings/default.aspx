<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.report_wkpg_master_datasettings._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
     <script type="text/javascript">
   
        //
        function MakeStaticHeader(gridId, height, width, headerHeight, isFooter) {
            //alert("hi234");
 var tbl = document.getElementById(gridId);
 if (tbl) {
 var DivHR = document.getElementById('DivHeaderRow');
 var DivMC = document.getElementById('DivMainContent');
     var DivFR = document.getElementById('DivFooterRow');
     width = DivMC.offsetWidth;
     //alert("Hi");
 //*** Set divheaderRow Properties ****
 DivHR.style.height = headerHeight + 'px';
 DivHR.style.width = (parseInt(width) - 16) + 'px';
 DivHR.style.position = 'relative';
 DivHR.style.top = '0px';
 DivHR.style.zIndex = '10';
 DivHR.style.verticalAlign = 'top';

 //*** Set divMainContent Properties ****
 DivMC.style.width = width + 'px';
 DivMC.style.height = height + 'px';
 DivMC.style.position = 'relative';
 DivMC.style.top = -headerHeight + 'px';
 DivMC.style.zIndex = '1';

 //*** Set divFooterRow Properties ****
 DivFR.style.width = (parseInt(width) - 16) + 'px';
 DivFR.style.position = 'relative';
 DivFR.style.top = -headerHeight + 'px';
 DivFR.style.verticalAlign = 'top';
 DivFR.style.paddingtop = '2px';

 if (isFooter) {
 var tblfr = tbl.cloneNode(true);
 tblfr.removeChild(tblfr.getElementsByTagName('tbody')[0]);
 var tblBody = document.createElement('tbody');
 tblfr.style.width = '100%';
 tblfr.cellSpacing = "0";
 tblfr.border = "0px";
  tblfr.rules = "none";
 //*****In the case of Footer Row *******
 tblBody.appendChild(tbl.rows[tbl.rows.length - 1]);
 tblfr.appendChild(tblBody);
 DivFR.appendChild(tblfr);
 }
 //****Copy Header in divHeaderRow****
 DivHR.appendChild(tbl.cloneNode(true));
 }
}


        function MakeStaticHeaderNew(gridId, height, width, headerHeight, isFooter) {
           //alert("hi2");
 var tbl = document.getElementById(gridId);
 if (tbl) {
 var DivHR = document.getElementById('DivHeaderRowNew');
 var DivMC = document.getElementById('DivMainContentNew');
     var DivFR = document.getElementById('DivFooterRowNew');
     width = DivMC.offsetWidth;
 //*** Set divheaderRow Properties ****
 DivHR.style.height = headerHeight + 'px';
 DivHR.style.width = (parseInt(width) - 16) + 'px';
 DivHR.style.position = 'relative';
 DivHR.style.top = '0px';
 DivHR.style.zIndex = '10';
 DivHR.style.verticalAlign = 'top';

 //*** Set divMainContent Properties ****
 DivMC.style.width = width + 'px';
 DivMC.style.height = height + 'px';
 DivMC.style.position = 'relative';
 DivMC.style.top = -headerHeight + 'px';
 DivMC.style.zIndex = '1';

 //*** Set divFooterRow Properties ****
 DivFR.style.width = (parseInt(width) - 16) + 'px';
 DivFR.style.position = 'relative';
 DivFR.style.top = -headerHeight + 'px';
 DivFR.style.verticalAlign = 'top';
 DivFR.style.paddingtop = '2px';

 if (isFooter) {
 var tblfr = tbl.cloneNode(true);
 tblfr.removeChild(tblfr.getElementsByTagName('tbody')[0]);
 var tblBody = document.createElement('tbody');
 tblfr.style.width = '100%';
 tblfr.cellSpacing = "0";
 tblfr.border = "0px";
  tblfr.rules = "none";
 //*****In the case of Footer Row *******
 tblBody.appendChild(tbl.rows[tbl.rows.length - 1]);
 tblfr.appendChild(tblBody);
 DivFR.appendChild(tblfr);
 }
 //****Copy Header in divHeaderRow****
 DivHR.appendChild(tbl.cloneNode(true));
 }
}

function OnScrollDiv(Scrollablediv) {
  document.getElementById('DivHeaderRow').scrollLeft = Scrollablediv.scrollLeft;
document.getElementById('DivFooterRow').scrollLeft = Scrollablediv.scrollLeft;
}

        function OnScrollDivNew(Scrollablediv) {
  document.getElementById('DivHeaderRowNew').scrollLeft = Scrollablediv.scrollLeft;
document.getElementById('DivFooterRowNew').scrollLeft = Scrollablediv.scrollLeft;
}

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
      <div class="container-fluid">
        <div class="row">
            <div class="col-lg-12 col-xl-12 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="card-title">
                        <div class="d-flex justify-content-between">
                              <h6 class="text-muted">
                                   <asp:Label ID="lblDocumentsNo" CssClass="text-uppercase font-weight-bold" runat="server" Text="WorkPackage Master Data Settings Report" Visible="true" />
                                 
                                       <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                               </h6>
                                <div>
                             <asp:Button ID="btnExcel" runat="server" Text="Export to Excel" CssClass="btn btn-primary" Visible="false" OnClick="btnExcel_Click"/>&nbsp;<asp:Button ID="btnPDF" runat="server" Text="Export to PDF" CssClass="btn btn-primary" Visible="false" OnClick="btnPDF_Click"/>&nbsp;
                                    <asp:Button ID="tnPrint" runat="server" Text="Print" CssClass="btn btn-primary" Visible="false" OnClick="tnPrint_Click"/>
                                </div>
                          </div>
                            </div>
                        <div class="table-responsive">
                            <div id="divsummary" runat="server" visible="true">
                                 <div style="overflow: hidden;" id="DivHeaderRowNew"></div>
                                   <div style="overflow:scroll;" onscroll="OnScrollDivNew(this)" id="DivMainContentNew">
                        <asp:GridView ID="grdDataList" EmptyDataText="No Data Found." runat="server" Width="100%" AutoGenerateColumns="false" CellPadding="6" CellSpacing="16" HeaderStyle-BackColor="#666666" HeaderStyle-ForeColor="White" AlternatingRowStyle-BackColor="#F5F5F5" 
                                             CssClass="table table-bordered" OnRowDataBound="grdDataList_RowDataBound">
                                       <Columns>
                                            <asp:BoundField DataField="UserName" HeaderText="User Name">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                          <asp:BoundField DataField="ProjectName" HeaderText="Project Name">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                            <asp:BoundField DataField="Name" HeaderText="WorkPackage">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                
                                                                    </asp:BoundField>
                                           <asp:BoundField DataField="WorkPackageUpdateFreq" HeaderText="Update Frequency">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                               
                                                                    </asp:BoundField>
                                            
                                       </Columns>
                                       </asp:GridView>
                                   </div>
                                 <div id="DivFooterRowNew" style="overflow:hidden"></div>
                            </div>
                             
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
</asp:Content>

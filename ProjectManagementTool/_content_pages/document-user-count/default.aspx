<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.document_user_count._default" EnableEventValidation="false"%>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
     <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
 <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
 <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
     <style type="text/css">
         .hideItem {
         display:none;
     }
  .pager span { color:green;font-weight:bold;font-size:17px;}
   

    </style>
    <script type="text/javascript">
              $( function() {
      $("input[id$='dtFromDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });

             $("input[id$='dtToDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
             });
        });


        function BindEvents() {
            $(".showModalPreviewDocument").click(function (e) {
                jQuery.noConflict();
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModDocumentPreview iframe").attr("src", url);
        $("#ModDocumentPreview").modal("show");
            });

            $(".showModalDocumentSent").click(function (e) {
                jQuery.noConflict();
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModDocumentSentDetails iframe").attr("src", url);
        $("#ModDocumentSentDetails").modal("show");
            });
        }

         $(document).ready(function () {
            BindEvents();
        });
        </script>
    <script type="text/javascript">
    function Confirm()
        {

            if (document.getElementById("dtFromDate").value == "" && document.getElementById("dtToDate").value =="" && document.getElementById("ddlProject").selectedIndex != "0" && document.getElementById("ddlusers").selectedIndex != "0" && document.getElementById("ddlOptions").selectedIndex != "0") {
                if (confirm("Date Range is not selected. Report will display Data from Beginning of the Project.") == true)
                    return true;
                else
                    return false;
            }
            else if (document.getElementById("dtFromDate").value == "" && document.getElementById("dtToDate").value =="" && document.getElementById("ddlProject").selectedIndex != "0" && document.getElementById("ddlusers").selectedIndex == "0" && document.getElementById("ddlOptions").selectedIndex != "0")
            {
                if (confirm("Date Range is not selected. Report will display Data from Beginning of the Project.") == true)
                    return true;
                else
                    return false;
            }
            else {
                
                 return true;
            }
    }

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
          <div class="col-lg-6 col-xl-12 form-group">
                        <div class="card">
                            <div class="card-body" style="padding-bottom:0; margin-bottom:0;">
                                <div class="row">
                        <div class="col-lg-4"><h6 class="card-title text-muted text-uppercase font-weight-bold">Report Type: </h6>
                            <asp:RadioButtonList ID="RDBReportView" runat="server" class="card-title text-muted text-uppercase font-weight-bold" AutoPostBack="true" Width="50%" RepeatDirection="Horizontal" OnSelectedIndexChanged="RDBReportView_SelectedIndexChanged">
                            <asp:ListItem Value="Project">&nbsp;By Project</asp:ListItem>
                            <asp:ListItem Value="User">&nbsp;By User</asp:ListItem>
                        </asp:RadioButtonList>
                        </div>
                                </div>
                            </div>
                        </div>
                       
                     </div>  
            </div>
    </div>
     <div class="container-fluid" id="DivProjectSelection" runat="server" visible="false">
            <div class="row">
                <div class="col-md-12 col-lg-4 form-group">
                      <label class="sr-only" for="ddlProject">Project</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Project</span>
                        </div>
                        <asp:DropDownList ID="ddlProject" runat="server" ClientIDMode="Static" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlProject_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-6 col-lg-4 form-group">
                    <label class="sr-only" for="ddlusers">Users</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Users</span>
                        </div>
                        <asp:DropDownList ID="ddlusers" runat="server" ClientIDMode="Static" CssClass="form-control" ></asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-6 col-lg-4 form-group">
                    <label class="sr-only" for="ddlOptions">Options</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Options</span>
                        </div>
                        <asp:DropDownList ID="ddlOptions" runat="server" CssClass="form-control" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="ddlOptions_SelectedIndexChanged">
                             <asp:ListItem Value="0">Select Type Of Report</asp:ListItem>
                            <asp:ListItem Value="1">Summary Details</asp:ListItem>
                            <asp:ListItem Value="2">All Details</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
    <div class="container-fluid" id="DivOptions" runat="server" visible="false">
            <div class="row">
                
                <div class="col-md-6 col-lg-3 form-group">
                    <label class="sr-only" for="ddlusers">Users</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">From Date</span>
                        </div>
                       <asp:TextBox ID="dtFromDate" CssClass="form-control" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-6 col-lg-3 form-group">
                    <label class="sr-only" for="ddlOptions">Options</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">To Date</span>
                        </div>
                       <asp:TextBox ID="dtToDate" CssClass="form-control" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-6 col-lg-3 form-group" id="DivStatus" runat="server" visible="false">
                    <label class="sr-only" for="ddlusers">Status</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Status</span>
                        </div>
                        <asp:DropDownList ID="DDLStatus" runat="server" CssClass="form-control">
                            <asp:ListItem Value="All">Select Status</asp:ListItem>
                            <asp:ListItem Value="Submitted">Submitted</asp:ListItem>
                            <asp:ListItem Value="Code A">Code A</asp:ListItem>
                            <asp:ListItem Value="Code B">Code B</asp:ListItem>
                            <asp:ListItem Value="Code C">Code C</asp:ListItem>
                            <asp:ListItem Value="Code D">Code D</asp:ListItem>
                            <asp:ListItem Value="Code E">Code E</asp:ListItem>
                            <asp:ListItem Value="Code F">Code F</asp:ListItem>
                            <asp:ListItem Value="Code G">Code G</asp:ListItem>
                            <asp:ListItem Value="Code H">Code H</asp:ListItem>
                            <asp:ListItem Value="Client Approved">Client Approved</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-6 col-lg-3 form-group" id="DivFilter" runat="server" visible="false">
                    <label class="sr-only" for="DDLFilter">Filter</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Filter</span>
                        </div>
                        <asp:DropDownList ID="DDLFilter" runat="server" CssClass="form-control">
                            <asp:ListItem Value="All">All</asp:ListItem>
                            <asp:ListItem Value="Downloaded">Downloaded</asp:ListItem>
                            <asp:ListItem Value="Document Link Sent">Document Link Sent</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>

    <div class="container-fluid" id="DivActionButtons" runat="server" visible="false">
            <div class="row" style="text-align:left;">
                <div class="col-md-12 col-lg-5 form-group"><div id="divDetialsPaging" runat="server" visible="false">No. of Records in Each Page <asp:TextBox ID="txtPageSize" runat="server" Width="40px">10</asp:TextBox></div></div>
                <div class="col-md-12 col-lg-2 form-group"><asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" OnClientClick="return Confirm();" Width="100px"/>&nbsp;<asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-primary" OnClick="btnClear_Click" Width="100px"/></div>
                <div class="col-md-12 col-lg-5 form-group" style="text-align:right;"><asp:Button ID="btnExcel" runat="server" Text="Export to Excel" CssClass="btn btn-primary"  OnClick="btnExcel_Click" Visible="false"/>&nbsp;<asp:Button ID="btnPDF" runat="server" Text="Export to PDF" CssClass="btn btn-primary" Visible="false" OnClick="btnPDF_Click"/>&nbsp;<asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btn btn-primary" Width="70px" Visible="false" OnClick="btnPrint_Click"/>&nbsp;<asp:DropDownList ID="ddlPages" runat="server" Height="35px" Visible="false">
                    <asp:ListItem Value="0">All Pages</asp:ListItem>
                    <asp:ListItem Value="1">Current Page</asp:ListItem>
                    </asp:DropDownList></div>
                </div>
      </div>
    <div class="container-fluid">
        <div class="row">
            <div class="col-lg-12 col-xl-12 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="card-title">
                        <div class="d-flex justify-content-between">
                              <h6 class="text-muted">
                                   <asp:Label ID="lblDocumentsNo" CssClass="text-uppercase font-weight-bold" runat="server" Text="Total No. Of Documents : " Visible="false" />
                                 
                                       <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                               </h6>
                                <div>
                                   
                                </div>
                          </div>
                            </div>
                        <div class="table-responsive">
                            <div id="divsummary" runat="server" visible="false">
                                 <div style="overflow: hidden;" id="DivHeaderRowNew"></div>
                                   <div style="overflow:scroll;" onscroll="OnScrollDivNew(this)" id="DivMainContentNew">
                        <asp:GridView ID="GrdDcumentHistory" EmptyDataText="No Projects Documents Found." runat="server" Width="100%" AutoGenerateColumns="false" CellPadding="6" CellSpacing="16" HeaderStyle-BackColor="#666666" HeaderStyle-ForeColor="White" AlternatingRowStyle-BackColor="#F5F5F5" 
                                             CssClass="table table-bordered" OnRowDataBound="GrdDcumentHistory_RowDataBound">
                                       <Columns>
                                            <asp:BoundField DataField="ProjectName" HeaderText="User Name">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                          <asp:BoundField DataField="ProjectName" HeaderText="Project Name">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                            <asp:BoundField DataField="ProjectUID" HeaderText="Documents Uploaded">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:BoundField>
                                           <asp:BoundField DataField="ProjectUID" HeaderText="Downloaded">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                               <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:BoundField>
                                            <asp:BoundField DataField="ProjectUID" HeaderText="Viewed">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:BoundField>
                                            <asp:BoundField DataField="ProjectUID" HeaderText="Download Link Sent">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:BoundField>
                                       </Columns>
                                       </asp:GridView>
                                  <asp:GridView ID="grdDocumenthistoryAll" EmptyDataText="No Projects Documents Found." runat="server" Width="100%" AutoGenerateColumns="false" CellPadding="6" CellSpacing="16" HeaderStyle-BackColor="#666666" HeaderStyle-ForeColor="White" AlternatingRowStyle-BackColor="#F5F5F5" 
                                             CssClass="table table-bordered" OnRowDataBound="grdDocumenthistoryAll_RowDataBound">
                                       <Columns>
                                            <asp:BoundField DataField="ProjectName" HeaderText="User Name">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                          <asp:BoundField DataField="ProjectName1" HeaderText="Project Name">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                            <asp:BoundField DataField="ProjectUID1" HeaderText="Documents Uploaded">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:BoundField>
                                           <asp:BoundField DataField="ProjectUID2" HeaderText="Downloaded">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                               <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:BoundField>
                                            <asp:BoundField DataField="ProjectUID3" HeaderText="Viewed">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:BoundField>
                                       <asp:BoundField DataField="ProjectUID4" HeaderText="Download Link Sent">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                           <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:BoundField>
                                       </Columns>
                                       </asp:GridView>

                                       <asp:GridView ID="GrdProjectwiseSummary" EmptyDataText="No Projects Documents Found." runat="server" Width="100%" AutoGenerateColumns="false" CellPadding="6" CellSpacing="16" HeaderStyle-BackColor="#666666" HeaderStyle-ForeColor="White" AlternatingRowStyle-BackColor="#F5F5F5" 
                                             CssClass="table table-bordered" OnRowDataBound="GrdProjectwiseSummary_RowDataBound">
                                       <Columns>
                                          <asp:BoundField DataField="ProjectName1" HeaderText="Project Name">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                            <asp:BoundField DataField="ProjectUID1" HeaderText="Documents Uploaded">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:BoundField>
                                           <asp:BoundField DataField="ProjectUID2" HeaderText="Downloaded">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                               <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:BoundField>
                                            <asp:BoundField DataField="ProjectUID3" HeaderText="Viewed">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:BoundField>
                                       <asp:BoundField DataField="ProjectUID4" HeaderText="Download Link Sent">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                           <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:BoundField>
                                       </Columns>
                                       </asp:GridView>

                                   </div>
                                 <div id="DivFooterRowNew" style="overflow:hidden"></div>
                            </div>
                             <div id="divDetails" runat="server" visible="false">
                                 <div style="overflow: hidden;" id="DivHeaderRow"></div>
                                   <div style="overflow:scroll;" onscroll="OnScrollDiv(this)" id="DivMainContent">
                                 <asp:GridView ID="GrdDocuments" EmptyDataText="No Documents Found." runat="server" Width="100%" AutoGenerateColumns="false" HeaderStyle-BackColor="#666666" HeaderStyle-ForeColor="White" AlternatingRowStyle-BackColor="#F5F5F5" 
                                             CssClass="table table-bordered" DataKeyNames="DocumentUID" OnRowDataBound="GrdDocuments_RowDataBound" AllowPaging="true" PageSize="10" OnPageIndexChanging="GrdDocuments_PageIndexChanging">
<AlternatingRowStyle BackColor="WhiteSmoke"></AlternatingRowStyle>
                                       <Columns>
                                           <asp:TemplateField HeaderText="UserName">
                                               <ItemTemplate>
                                                   <%#GetUserName(Eval("UserUID").ToString())%>
                                               </ItemTemplate>
                                           </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Project Name">
                                            <ItemTemplate>
                                                <%#Eval("ProjectName").ToString()%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                            <asp:BoundField DataField="Submittal" HeaderText="Submittal Name">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                             <asp:TemplateField  HeaderText="Document Name" ItemStyle-HorizontalAlign="Left" SortExpression="ActualDocument_Name">
                                            <ItemTemplate>
                                                <div id="divD" runat="server">
                                                       <img width="24" src='../../_assets/images/<%#GetDocumentTypeIcon(Eval("Type").ToString())%>' alt='<%#Eval("Type")%>' />  &nbsp;&nbsp;
                                                       <%--<asp:LinkButton ID="lnkviewpdf" class='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? "view" : "hideItem" %>' runat="server" CausesValidation="false" CommandArgument='<%#Eval("ActualDocument_Path")%>' CommandName="ViewDoc"><%#Eval("ActualDocument_Name")%></asp:LinkButton>--%>
                                                      
                                                       <a id="ShowFile" href='/_modal_pages/preview-document.aspx?ActualDocumentUID=<%#Eval("DocumentUID")%>&previewpath=<%#Eval("ActualDocument_Path").ToString().Replace('&','!')%>' class='<%#Convert.ToString(Eval("Type")) == ".pdf" ? "showModalPreviewDocument" : Convert.ToString(Eval("Type")) == ".xls" ? "showModalPreviewDocument" : Convert.ToString(Eval("Type")) == ".xlsx" ? "showModalPreviewDocument" : Convert.ToString(Eval("Type")) == ".docx" ? "showModalPreviewDocument" : Convert.ToString(Eval("Type")) == ".doc" ? "showModalPreviewDocument" : Convert.ToString(Eval("Type")) == ".PDF" ? "showModalPreviewDocument" : "hideItem" %>'><%#Eval("DocName")%></a>
                                                         
                                                       <asp:Label ID="lblName" runat="server" Text='<%#Eval("DocName")%>' Visible='<%#Convert.ToString(Eval("Type")) == ".pdf" ? false : Convert.ToString(Eval("Type")) == ".xls" ? false : Convert.ToString(Eval("Type")) == ".xlsx" ? false : Convert.ToString(Eval("Type")) == ".docx" ? false : Convert.ToString(Eval("Type")) == ".doc" ? false : Convert.ToString(Eval("Type")) == ".PDF" ? false : true %>'></asp:Label>
                                                     </div>
                                                <div id="divGD" runat="server" visible="false">
                                                       <img width="24" src='../../_assets/images/<%#GetDocumentTypeIcon(Eval("Type").ToString())%>' alt='<%#Eval("Type")%>' />  &nbsp;&nbsp;
                                                       <%--<asp:LinkButton ID="lnkviewpdf" class='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? "view" : "hideItem" %>' runat="server" CausesValidation="false" CommandArgument='<%#Eval("ActualDocument_Path")%>' CommandName="ViewDoc"><%#Eval("ActualDocument_Name")%></asp:LinkButton>--%>
                                                      
                                                       <a id="ShowFile1" href='/_modal_pages/preview-document.aspx?GeneralDocumentUID=<%#Eval("DocumentUID")%>&previewpath=<%#Eval("ActualDocument_Path").ToString().Replace('&','!')%>' class='<%#Convert.ToString(Eval("Type")) == ".pdf" ? "showModalPreviewDocument" : Convert.ToString(Eval("Type")) == ".xls" ? "showModalPreviewDocument" : Convert.ToString(Eval("Type")) == ".xlsx" ? "showModalPreviewDocument" : Convert.ToString(Eval("Type")) == ".docx" ? "showModalPreviewDocument" : Convert.ToString(Eval("Type")) == ".doc" ? "showModalPreviewDocument" : Convert.ToString(Eval("Type")) == ".PDF" ? "showModalPreviewDocument" : "hideItem" %>'><%#Eval("DocName")%></a>
                                                         
                                                       <asp:Label ID="Label1" runat="server" Text='<%#Eval("DocName")%>' Visible='<%#Convert.ToString(Eval("Type")) == ".pdf" ? false : Convert.ToString(Eval("Type")) == ".xls" ? false : Convert.ToString(Eval("Type")) == ".xlsx" ? false : Convert.ToString(Eval("Type")) == ".docx" ? false : Convert.ToString(Eval("Type")) == ".doc" ? false : Convert.ToString(Eval("Type")) == ".PDF" ? false : true %>'></asp:Label>
                                                     </div>
                                            </ItemTemplate>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Document Type">
                                            <ItemTemplate>
                                                <%#GetDocumentName(Eval("Type").ToString())%>
                                            </ItemTemplate>
                                               <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                           <asp:BoundField DataField="Status" HeaderText="Current Status" SortExpression="ActualDocument_CurrentStatus">
                                            <HeaderStyle HorizontalAlign="Left" />
                                               <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <%-- <asp:BoundField DataField="IncomingRcvDate" HeaderText="Incoming Recv. Date" DataFormatString="{0:dd/MM/yyyy}" >
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                               <asp:BoundField DataField="Document_Date" HeaderText="Document Date" DataFormatString="{0:dd/MM/yyyy}" >
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>--%>
                                          <%--   <asp:BoundField DataField="ActualDocument_Path" HeaderText="ActualDocument_Path" HeaderStyle-CssClass="hideItem" ItemStyle-CssClass="hideItem" >
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>--%>
                                             <asp:BoundField DataField="DocumentUID" HeaderText="Downloaded"  >
                                            <HeaderStyle HorizontalAlign="Left" />
                                                 <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Document Link Sent">
                                                <ItemTemplate>
                                                    <a class="showModalDocumentSent" href="/_modal_pages/view-documentsentdetails.aspx?DocumentUID=<%#Eval("DocumentUID")%>">
                                                        <asp:Label ID="LblCount" Font-Bold="true" runat="server"></asp:Label>
                                                    </a>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                       </Columns>

<HeaderStyle BackColor="#666666" ForeColor="White"></HeaderStyle>

                                       <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" NextPageText="Next" PreviousPageText="Previous" />
                                       <PagerStyle CssClass="pager" />
                                       </asp:GridView>
                                       <asp:GridView ID="grdDocumentsPDF" EmptyDataText="No Documents Found." runat="server" Width="100%" AutoGenerateColumns="false" HeaderStyle-BackColor="#666666" HeaderStyle-ForeColor="White" AlternatingRowStyle-BackColor="#F5F5F5" 
                                             CssClass="table table-bordered" DataKeyNames="DocumentUID" Visible="false" OnRowDataBound="grdDocumentsPDF_RowDataBound">
                                       <Columns>
                                           <asp:TemplateField HeaderText="UserName">
                                               <ItemTemplate>
                                                   <%#GetUserName(Eval("UserUID").ToString())%>
                                               </ItemTemplate>
                                           </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Project Name">
                                            <ItemTemplate>
                                                <%#Eval("ProjectName").ToString()%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                            <asp:BoundField DataField="Submittal" HeaderText="Submittal Name">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                             <asp:TemplateField  HeaderText="Document Name" ItemStyle-HorizontalAlign="Left" SortExpression="ActualDocument_Name">
                                            <ItemTemplate>
                                                <%#Eval("DocName").ToString()%>
                                            </ItemTemplate>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Document Type">
                                            <ItemTemplate>
                                                <%#GetDocumentName(Eval("Type").ToString())%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           <asp:BoundField DataField="Status" HeaderText="Current Status" SortExpression="ActualDocument_CurrentStatus">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <%-- <asp:BoundField DataField="IncomingRcvDate" HeaderText="Incoming Recv. Date" DataFormatString="{0:dd/MM/yyyy}" >
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                               <asp:BoundField DataField="Document_Date" HeaderText="Document Date" DataFormatString="{0:dd/MM/yyyy}" >
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>--%>
                                          <%--   <asp:BoundField DataField="ActualDocument_Path" HeaderText="ActualDocument_Path" HeaderStyle-CssClass="hideItem" ItemStyle-CssClass="hideItem" >
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>--%>
                                             <asp:BoundField DataField="DocumentUID" HeaderText="Downloaded"  >
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                             <asp:BoundField DataField="DocumentUID" HeaderText="Document Link Sent"  >
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                       </Columns>
                                     
                                       </asp:GridView>
                                   </div>
                                 <div id="DivFooterRow" style="overflow:hidden"></div>
                                 </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
     <%--View document histroy modal--%>
    <div id="ModDocumentPreview" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Document Preview</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:500px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--View document histroy modal--%>
    <div id="ModDocumentSentDetails" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Document Sent Details</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:500px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
</asp:Content>

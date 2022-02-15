<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.communication_tracking._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
    
<%--<style type="text/css">
    
  .black_overlay {
  display: none;
  position: absolute;
  top: 0%;
  left: 0%;
  width: 100%; 
  height: 100%;
  background-color: black;
  z-index: 1001;
  -moz-opacity: 0.8;
  opacity: .80;
  filter: alpha(opacity=80);
}
.white_content {
  display: none;
  position: absolute;
  top:auto;
  left: 25%;
  width: 35%;
  padding: 10px;
  border: 8px solid #3498db;
  background-color: white;
  z-index: 1002;
  overflow: auto;
  
  text-align:justify;
  line-height:20px;
  box-shadow: 5px 10px #888888;
  font-weight:normal;
  font-size:large;
}
     .hideItem {
         display:none;
         
     }
    </style>--%>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>  
    <script type="text/javascript">  
        function StatusExpand(input) {
            var displayIcon = "img" + input;
            if ($("#" + displayIcon).attr("src") == "/_assets/images/plus.png") {
                $("#" + displayIcon).closest("tr")  
                .after("<tr><td></td><td colspan = '100%'>" + $("#" + input)  
                .html() + "</td></tr>");  
                $("#" + displayIcon).attr("src", "/_assets/images/minus.png");  
            } else {  
                $("#" + displayIcon).closest("tr").next().remove();  
                $("#" + displayIcon).attr("src", "/_assets/images/plus.png");  
            }  
        }  
    </script>  
   <%-- <script type="text/javascript">
        function expand1() {
            $("[id*=imgProductsShow]").each(function () {
                if ($(this)[0].src.indexOf("minus") != -1) {
                    $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>");
                    $(this).next().remove();
                }
            });
    </script>--%>

    <%--<script type="text/javascript">

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

    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
      <%--project selection dropdowns--%>
    <%--<div id="loader"></div>--%>
    <asp:ScriptManager ID="script1" runat="server"></asp:ScriptManager>
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12 col-lg-4 form-group">Communication Tracking</div>
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
                        <asp:DropDownList ID="DDLWorkPackage" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDLWorkPackage_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>

    <div class="container-fluid">
            <div class="row">
                <div class="col-md-12 col-lg-12 form-group">
                    <div class="card">
                        <div class="card-body">
                            <div class="table-responsive">
                                 <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:UpdateProgress ID="UpdateProgress1" runat="Server" AssociatedUpdatePanelID="UpdatePanel2" DisplayAfter="1"> 
                                        <ProgressTemplate > 
                                       
                                            <div id="loader"></div>
                                        </ProgressTemplate> 
                                    </asp:UpdateProgress>--%>
                            <div style="overflow: hidden;" id="DivHeaderRowNew"></div>
                                   <div style="overflow:scroll; " onscroll="OnScrollDivNew(this)" id="DivMainContentNew">
                            <asp:GridView ID="GrdCommunicationDocs" runat="server" DataKeyNames="ActualDocumentUID" AllowPaging="false" AutoGenerateColumns="False" CssClass="table table-bordered" PageSize="10" EmptyDataText="No Data"
                                HeaderStyle-BackColor="#666666" HeaderStyle-ForeColor="White" AlternatingRowStyle-BackColor="#F5F5F5"  Width="100%" OnPageIndexChanging="GrdCommunicationDocs_PageIndexChanging" OnRowCommand="GrdCommunicationDocs_RowCommand" OnRowDataBound="GrdCommunicationDocs_RowDataBound">
                        <Columns>
                            <asp:TemplateField>
                               <ItemTemplate>
                                   <%--<asp:ImageButton ID="imgProductsShow" runat="server" OnClick="Show_Hide_StatusGrid" ImageUrl="~/_assets/images/plus.png"
                                                                CommandArgument="Show" Height="25px" Width="25px"/>     --%>                              
                                   <a href="javaScript:StatusExpand('div<%#Eval("ActualDocumentUID")%>');">  
                                <img alt="" id="imgdiv<%#Eval("ActualDocumentUID")%>"   src="/_assets/images/plus.png" />  
                                            </a>  
                                  <%--<asp:Panel ID="pnlStatus" runat="server" Visible="false" style="position:relative;">--%>
                                   <div id="div<%#Eval("ActualDocumentUID")%>" style="display: none;">
                                        <asp:GridView ID="GrdStatus" runat="server" Width="100%" AutoGenerateColumns="false"  class="table table-bordered"
                                                                    AllowPaging="false" DataKeyNames="DocumentUID" OnRowCommand="GrdStatus_RowCommand" >
                                          <Columns>
                                              <asp:TemplateField HeaderText="Origin">
                                                  <ItemTemplate>
                                                      <%#Eval("Origin")%>
                                                  </ItemTemplate>
                                              </asp:TemplateField>
                                              <asp:TemplateField HeaderText="Ref. Number">
                                                  <ItemTemplate>
                                                      <%#Eval("Ref_Number")%>
                                                  </ItemTemplate>
                                              </asp:TemplateField>
                                              <asp:TemplateField HeaderText="Date">
                                                  <ItemTemplate>
                                                      <%#Eval("ActivityDate","{0:dd MMM yyyy}")%>
                                                  </ItemTemplate>
                                              </asp:TemplateField>
                                              <asp:TemplateField HeaderText="Comments">
                                                  <ItemTemplate>
                                                      <%#Eval("Status_Comments")%>
                                                  </ItemTemplate>
                                              </asp:TemplateField>
                                              <asp:TemplateField HeaderText="Letter">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LnkcoverDownload" runat="server" CausesValidation="false" CommandArgument='<%#Eval("StatusUID")%>' CommandName="CoverLetterDownload">Download</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                          </Columns>
                                            <EmptyDataTemplate>
                                                No Status Found !
                                           </EmptyDataTemplate>
                                       </asp:GridView>
                                       </div>
                                  <%--</asp:Panel>--%>
                               </ItemTemplate>
                           </asp:TemplateField>
                            <%--<asp:TemplateField HeaderText="Sl.No">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>    --%>
                           <asp:TemplateField HeaderText="Subject/Description" ItemStyle-Width="25%">
                                <ItemTemplate>
                                    <%#Eval("Description")%>
                                </ItemTemplate>
                            </asp:TemplateField>    
                            <%--<asp:TemplateField HeaderText="P Ref. Number">
                                <ItemTemplate>
                                    <%#Eval("ProjectRef_Number")%>
                                </ItemTemplate>
                            </asp:TemplateField>--%>        
                            <asp:TemplateField HeaderText="Ref. Number" ItemStyle-Width="20%">
                                <ItemTemplate>
                                    <%#Eval("Ref_Number")%>
                                </ItemTemplate>
                            </asp:TemplateField>  
                            <asp:BoundField DataField="ActualDocumentUID" HeaderText="Current Status" />
                            <%--<asp:TemplateField HeaderText="Originator">
                                <ItemTemplate>
                                    <%#Eval("ActualDocument_Originator")%>
                                </ItemTemplate>
                            </asp:TemplateField> --%>
                            <%--<asp:TemplateField HeaderText="Incoming Rec.Date">
                                <ItemTemplate>
                                    <%#Eval("IncomingRec_Date","{0:dd MMM yyyy}")%>
                                </ItemTemplate>
                            </asp:TemplateField>      --%>  
                            <asp:TemplateField HeaderText="Document Date">
                                <ItemTemplate>
                                    <%#Eval("Document_Date","{0:dd MMM yyyy}")%>
                                </ItemTemplate>
                            </asp:TemplateField>        
                            <asp:TemplateField HeaderText="File Ref. Number" >
                                <ItemTemplate>
                                    <%#Eval("FileRef_Number")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%-- <asp:TemplateField HeaderText="Remarks">
                                <ItemTemplate>
                                    <%#Eval("Remarks")%>
                                </ItemTemplate>
                            </asp:TemplateField>    --%>
                            <%--<asp:TemplateField HeaderText="Cover Letter">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LnkDownload" runat="server" CausesValidation="false" CommandArgument='<%#Eval("ActualDocumentUID")%>' CommandName="download">Download</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                        </Columns>
                    </asp:GridView>
                                       </div>
                                <div id="DivFooterRowNew" style="overflow:hidden"></div>
                                   <%-- </ContentTemplate>
                                     </asp:UpdatePanel>--%>
                                </div>
                        </div>
                    </div>
                </div>

                </div>
        </div>

    
</asp:Content>

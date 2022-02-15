<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.categorywise_documents._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
    <style type="text/css">
    
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
  
    </style>
<script type="text/javascript">
    function expand2() {
        $("[id*=imgProductsShow1]").each(function () {
                if ($(this)[0].src.indexOf("minus") != -1) {
                    $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>");
                    $(this).next().remove();
                }
            });
    }
function DeleteItem() {
            if (confirm("Are you sure you want to delete ...?")) {
                return true;
            }
            return false;
    }


 </script>

    <script type="text/javascript">
        function BindEvents() {
        $(".showModal").click(function(e) {
            e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddsubmittal iframe").attr("src", url);
            $("#ModAddsubmittal").modal("show");
        });

        $(".showModalEdit").click(function(e) {
            e.preventDefault();
        var url = $(this).attr("href");
        $("#ModEditsubmittal iframe").attr("src", url);
        $("#ModEditsubmittal").modal("show");
            });

        $(".showModalDocument").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddDocument iframe").attr("src", url);
        $("#ModAddDocument").modal("show");
            });

        $(".showModalDocumentEdit").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModEditDocument iframe").attr("src", url);
        $("#ModEditDocument").modal("show");
            });

        $(".showModalDocumentHistory").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModViewDocumentHistory iframe").attr("src", url);
        $("#ModViewDocumentHistory").modal("show");
            });

            $(".showModalPreviewDocument").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModDocumentPreview iframe").attr("src", url);
        $("#ModDocumentPreview").modal("show");
            });

             $(".showModalDocumentMail").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModDocumentMail iframe").attr("src", url);
        $("#ModDocumentMail").modal("show");
             });

            $(".showModalDocumentMailGenDocs").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModDocumentMail iframe").attr("src", url);
        $("#ModDocumentMail").modal("show");
            });

            $(".showCopyModal").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModCopyDocument iframe").attr("src", url);
        $("#ModCopyDocument").modal("show");
        });

            $(".showAddGeneralDocumentStructureModal").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddGeneralDocumentStructure iframe").attr("src", url);
        $("#ModAddGeneralDocumentStructure").modal("show");
             });

            $(".showGeneralDocumentStructureEditModal").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModEditGeneralDocumentStructure iframe").attr("src", url);
        $("#ModEditGeneralDocumentStructure").modal("show");
            });

            $(".showAddGeneralDocumentModal").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddGeneralDocument iframe").attr("src", url);
        $("#ModAddGeneralDocument").modal("show");
             });

            $(".showGeneralDocumentEditModal").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModEditGeneralDocument iframe").attr("src", url);
        $("#ModEditGeneralDocument").modal("show");
            });
        }
        $(document).ready(function () {
            BindEvents();
            
            expand2();
        });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
   </asp:ScriptManager>
    <div class="container-fluid">
            <div class="row">
                <div class="col-md-12 col-lg-4 form-group">Dashboard</div>
                <div class="col-md-6 col-lg-4 form-group">
                    <label class="sr-only" for="DDLProject">Project</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Project</span>
                        </div>
                        <asp:DropDownList ID="DDlProject" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDlProject_SelectedIndexChanged"></asp:DropDownList>
                       <%-- <select class="form-control" id="DDlProject" runat="server">
                           
                        </select>--%>
                    </div>
                </div>
                <div class="col-md-6 col-lg-4 form-group">
                    <label class="sr-only" for="DDLWorkPackage">Work Package</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Work Package</span>
                        </div>
                        <asp:DropDownList ID="DDLWorkPackage" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDLWorkPackage_SelectedIndexChanged"></asp:DropDownList>

                        <%--<select class="form-control" id="DDLWorkPackage" runat="server">
                        </select>--%>
                    </div>
                </div>
            </div>
        </div>

        
    <div class="container-fluid" id="ByCategory" runat="server"
>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-lg-6 col-xl-4 form-group">
                                    <div class="card h-100">
                                        <div class="card-body">
                                            <h6 class="card-title text-muted text-uppercase font-weight-bold">Documents Directory</h6>
                                            <asp:TreeView runat="server" CssClass="it_tree_view" ID="TreeView2" ImageSet="XPFileExplorer" OnSelectedNodeChanged="TreeView2_SelectedNodeChanged" NodeIndent="15" EnableTheming="True" NodeWrap="True">    
                                                <%--<HoverNodeStyle Font-Underline="True" ForeColor="#6666AA" />--%>  
                                                <NodeStyle CssClass="it_tree_view__node" NodeSpacing="2px" />
                                                <ParentNodeStyle Font-Bold="False" />
                                                <SelectedNodeStyle CssClass="it_tree_view__node__selected"  HorizontalPadding="4px" VerticalPadding="2px" />
                                            </asp:TreeView>    
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-6 col-xl-8 form-group" > 
                                    <asp:UpdateProgress ID="UpdateProgress2" runat="Server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="1"> 
                                        <ProgressTemplate > 
                                        <!-- <asp:Label ID="lblwait" runat="server" Text="Please wait while we fetch the data.." ForeColor="Red"></asp:Label>-->
                                            <div id="loader"></div>
                                        </ProgressTemplate> 
                                    </asp:UpdateProgress>
                                        <div class="card mb-4" id="CategoryDocumentGridHeading" runat="server">
                                            <div class="card-body">
                                                <div class="card-title">
                                                    <div class="d-flex justify-content-between">
                                                        <h6 class="text-muted">
                                                            <asp:Label ID="ActivityHeadingCategory" CssClass="text-uppercase font-weight-bold" runat="server" />
                                                        </h6>
                                                        <div>
                                                            
                                                            <a id="AddSubmittalCategory" runat="server"  href="/_modal_pages/add-submittalsforcategory.aspx" class="showModal"><asp:Button ID="btnaddsubmittalforcategory" runat="server" Text="+ Add Submittal" CssClass="btn btn-primary" align="end"></asp:Button></a>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    <div class="card" id="CategoryDocumentGrid" runat="server">
                                        <div class="card-body">
                                             <h6 class="card-title text-muted text-uppercase font-weight-bold"><asp:Label ID="LblDocumentHeading" runat="server" Text="Submittals"></asp:Label></h6>
                                                <asp:GridView ID="GrdDocumentByCategory" class="table table-bordered" DataKeyNames="DocumentUID" PageSize="15" AllowSorting="true" runat="server" AllowPaging="true" AutoGenerateColumns="False" Width="100%" OnPageIndexChanging="GrdDocumentByCategory_PageIndexChanging" OnRowCommand="GrdDocumentByCategory_RowCommand" OnRowDeleting="GrdDocumentByCategory_RowDeleting">
                                        <Columns>
                                            <asp:TemplateField>
                                                  <ItemTemplate>
                                                      <asp:ImageButton ID="imgProductsShow1" runat="server" OnClick="Show_Hide_DocumentsGridCategory" ImageUrl="~/_assets/images/plus.png"
                                                                CommandArgument="Show" Height="25px" Width="25px"/>
                                                      <asp:Panel ID="pnlDocuments1" runat="server" Visible="false" style="position:relative;">
                                                          <script type="text/javascript">
                                                              $(document).ready(function () {
                                                                  $(".showModalDocumentEdit").click(function (e) {
                                                                      e.preventDefault();
                                                                      var url = $(this).attr("href");
                                                                      $("#ModEditDocument iframe").attr("src", url);
                                                                      $("#ModEditDocument").modal("show");
                                                                  });
                                                                  $(".showModalDocumentHistory").click(function (e) {
                                                                      e.preventDefault();
                                                                      var url = $(this).attr("href");
                                                                      $("#ModViewDocumentHistory iframe").attr("src", url);
                                                                      $("#ModViewDocumentHistory").modal("show");
                                                                  });
                                                                  $(".showModalPreviewDocument").click(function (e) {
                                                                      e.preventDefault();
                                                                      var url = $(this).attr("href");
                                                                      $("#ModDocumentPreview iframe").attr("src", url);
                                                                      $("#ModDocumentPreview").modal("show");
                                                                  });
                                                              });
                                                        </script>
                                                           <asp:GridView ID="GrdActualDocumentsCategory" runat="server" EmptyDataText="No Documents Found.." Width="100%" AutoGenerateColumns="false"  class="table table-bordered"
                                                                    AllowPaging="false" DataKeyNames="ActualDocumentUID" OnRowDataBound="GrdActualDocumentsCategory_RowDataBound" OnRowCommand="GrdActualDocumentsCategory_RowCommand" OnRowDeleting="GrdActualDocumentsCategory_RowDeleting">
                                                               <Columns>
                                                                   <asp:TemplateField HeaderText="Name" ItemStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                      
                                                                       <%-- <asp:UpdatePanel runat="server" ID="UpdatePanelview" UpdateMode="Conditional" ChildrenAsTriggers="True">
                                                                        <ContentTemplate>--%>
                                                                        <div id="divPDF" runat="server">
                                                                        <img width="24" src='../../_assets/images/<%#GetDocumentTypeIcon(Eval("ActualDocument_Type").ToString(),Eval("ActualDocumentUID").ToString(),"Actual Document")%>' alt='<%#Eval("ActualDocument_Type")%>' />  &nbsp;&nbsp;
                                                                           
                                                                            <a href='/_modal_pages/preview-document.aspx?ActualDocumentUID=<%#Eval("ActualDocumentUID")%>&previewpath=<%#Eval("ActualDocument_Path").ToString().Replace('&','!')%>' class='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".xls" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".xlsx" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".docx" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".doc" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".PDF" ? "showModalPreviewDocument" : "hideItem" %>'><%#Eval("ActualDocument_Name")%></a>
                                                                            <asp:Label ID="lblName" runat="server" Text='<%#Eval("ActualDocument_Name")%>' Visible='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".xls" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".xlsx" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".docx" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".doc" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".PDF" ? false : true %>'></asp:Label>
                                                                                <asp:Label ID="LblVersion" runat="server"></asp:Label></div>

                                                                            <img id="imgfolder" runat="server" src="/_assets/images/folder.jpg" height="15" width="20">
                                                                        
                                                                        <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Folder_View" CommandArgument='<%#Eval("ActualDocumentUID")%>'
                                                                         Text="Edit"></asp:LinkButton>
                                                                            <%--</ContentTemplate>
                                                                          <Triggers>
                                                                            <asp:PostBackTrigger ControlID="lnkviewpdf" />
                                                                          </Triggers>
                                                                        </asp:UpdatePanel>--%>
                                                                                <%--<a class='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? "view" : "hideItem" %>' target="_blank"  href='<%#Server.MapPath(Eval("ActualDocument_Path").ToString())%>'><%#Eval("ActualDocument_Name")%></a>--%>
                                                                                
                                                                             </ContentTemplate>
                                                                          
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                  <%-- <asp:BoundField DataField="ActualDocument_Version" HeaderText="Version">
                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                    </asp:BoundField>--%>
                                                                  <%-- <asp:BoundField DataField="Doc_Type" HeaderText="Document For">
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    </asp:BoundField>--%>
                                                                   <asp:TemplateField HeaderText="Type">
                                                                    <ItemTemplate>
                                                                        <%#GetDocumentName(Eval("ActualDocument_Type").ToString())%>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                   <asp:BoundField DataField="ActualDocumentUID" HeaderText="Status">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                   <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                         <%--<a class="view" href="ViewDocument.aspx?DocID=<%#Eval("DocumentUID")%>">View</a>--%>
                                                                        <asp:UpdatePanel runat="server" ID="UpdatePanel1Download" UpdateMode="Conditional" ChildrenAsTriggers="True">
                                                                        <ContentTemplate>
                                                                            <asp:LinkButton ID="LnkDownload1" runat="server" CausesValidation="false" CommandArgument='<%#Eval("ActualDocumentUID")%>' CommandName="download">Download</asp:LinkButton>
                                                                            </ContentTemplate>
                                                                          <Triggers>
                                                                            <asp:PostBackTrigger ControlID="LnkDownload1" />
                                                                          </Triggers>
                                                                        </asp:UpdatePanel>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                   <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                            <a id="ViewByCategory" class="<%#Convert.ToString(Eval("Doc_Type")) == "Cover Letter" ? "hideItem" : Convert.ToString(Eval("Doc_Type")) =="General Document" ? "hideItem" : "showModalDocumentHistory" %>" href="/_modal_pages/view-documenthistory.aspx?DocID=<%#Eval("ActualDocumentUID")%>&ProjectUID=<%#Eval("ProjectUID")%>">View History</a>
                                                                        </ItemTemplate>
                                                                       </asp:TemplateField>
                                                                   <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <a class="showModalDocumentEdit" href="/_modal_pages/edit-document.aspx?DocID=<%#Eval("ActualDocumentUID")%>&pUID=<%#Eval("ProjectUID")%>&wUID=<%#Eval("WorkPackageUID")%>"><span title="Edit" class="fas fa-edit"></span></a>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                   <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkactualdelete1" runat="server" OnClientClick="return DeleteItem()" CausesValidation="false" CommandArgument='<%#Eval("ActualDocumentUID")%>' CommandName="delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                    <asp:BoundField DataField="ActualDocument_RelativePath" HeaderText="Path" ItemStyle-CssClass="hideItem" HeaderStyle-CssClass="hideItem">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                               </Columns>
                                                               </asp:GridView>
                                                          </asp:Panel>
                                                  </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Submittal Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    
                                                    <%#Eval("DocName")%>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Activity Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <a href="/_content_pages/engineering-workpackage/Default.aspx?ActivityUID=<%#Eval("WorkPackageUID").ToString() +'*'+ Eval("TaskUID").ToString()%>"><%#GetWorkpackageName(Eval("WorkPackageUID").ToString() +'*'+ Eval("TaskUID").ToString())%></a>
                                                      
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                            </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Date" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <%#Eval("FlowStep1_TargetDate","{0:dd MMM yyyy}")%>
                                                </ItemTemplate>
                                             <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                            </asp:TemplateField>
                                             <asp:TemplateField>
                                                <ItemTemplate>
                                                    <%--<button class="btn btn-primary" type="button" data-toggle="modal" data-target="#ModAddWorkpackages"><i class="fa fa-plus" aria-hidden="true"></i> Add WorkPackage</button>--%>
                                                    <a class="showModalDocument" href="/_modal_pages/add-document.aspx?dUID=<%#Eval("DocumentUID")%>&pUID=<%#Eval("ProjectUID")%>&wUID=<%#Eval("WorkPackageUID")%>&tUID=<%#Eval("TaskUID")%>&sType=Category">Add Documents</a>
                                                    <%--<a data-toggle="modal" data-target="#ModAddDocuments" href="#">Add Documents</a>--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <a class="showModalEdit" href="/_modal_pages/add-submittal.aspx?type=edit&DocID=<%#Eval("DocumentUID")%>&TaskUID=<%#Eval("TaskUID")%>&PrjUID=<%#Eval("ProjectUID")%>&WrkUID=<%#Eval("WorkPackageUID")%>"><span title="Edit" class="fas fa-edit"></span></a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkdelete" runat="server" CausesValidation="false" OnClientClick="return DeleteItem()" CommandArgument='<%#Eval("DocumentUID")%>' CommandName="delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>             
                                        </Columns>
                                        <EmptyDataTemplate>
                                            No Documents Found !
                                        </EmptyDataTemplate>
                                    </asp:GridView>

                                            <asp:GridView ID="GrdActualDocuments1" runat="server" Width="100%" AutoGenerateColumns="false"  class="table table-bordered"
                                                                    AllowPaging="false" DataKeyNames="ActualDocumentUID" OnRowDataBound="GrdActualDocuments1_RowDataBound" OnRowCommand="GrdActualDocuments1_RowCommand" OnRowDeleting="GrdActualDocuments1_RowDeleting" >
                                                               <Columns>
                                                                   <asp:TemplateField HeaderText="Name" ItemStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                      
                                                                       <%-- <asp:UpdatePanel runat="server" ID="UpdatePanelview" UpdateMode="Conditional" ChildrenAsTriggers="True">
                                                                        <ContentTemplate>--%>
                                                                        <div id="divPDF" runat="server">
                                                                                <img width="24" src='../../_assets/images/<%#GetDocumentTypeIcon(Eval("ActualDocument_Type").ToString(),Eval("ActualDocumentUID").ToString(),"Actual Document")%>' alt='<%#Eval("ActualDocument_Type")%>' />  &nbsp;&nbsp;
                                                                            <%--<asp:LinkButton ID="lnkviewpdf" class='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? "view" : "hideItem" %>' runat="server" CausesValidation="false" CommandArgument='<%#Eval("ActualDocument_Path")%>' CommandName="ViewDoc"><%#Eval("ActualDocument_Name")%></asp:LinkButton>--%>
                                                                            <a href='/_modal_pages/preview-document.aspx?ActualDocumentUID=<%#Eval("ActualDocumentUID")%>&previewpath=<%#Eval("ActualDocument_Path").ToString().Replace('&','!')%>' class='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".docx" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".doc" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".xls" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".xlsx" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".PDF" ? "showModalPreviewDocument" : "hideItem" %>'><%#Eval("ActualDocument_Name")%></a>
                                                                            <asp:Label ID="lblName" runat="server" Text='<%#Eval("ActualDocument_Name")%>' Visible='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".docx" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".doc" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".xls" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".xlsx" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".PDF" ? false : true %>'></asp:Label>
                                                                                <asp:Label ID="LblVersion" runat="server"></asp:Label></div>
                                                                         <img id="imgfolder" runat="server" src="/_assets/images/folder.jpg" height="15" width="20">
                                                                        
                                                                        <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Folder_View" CommandArgument='<%# Container.DataItemIndex %>'
                                                                         Text="Edit"></asp:LinkButton>
                                                                            <%--</ContentTemplate>
                                                                          <Triggers>
                                                                            <asp:PostBackTrigger ControlID="lnkviewpdf" />
                                                                          </Triggers>
                                                                        </asp:UpdatePanel>--%>
                                                                                <%--<a class='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? "view" : "hideItem" %>' target="_blank"  href='<%#Server.MapPath(Eval("ActualDocument_Path").ToString())%>'><%#Eval("ActualDocument_Name")%></a>--%>
                                                                                
                                                                             </ContentTemplate>
                                                                          
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                  <%-- <asp:BoundField DataField="ActualDocument_Version" HeaderText="Version">
                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                    </asp:BoundField>--%>
                                                                  <%-- <asp:BoundField DataField="Doc_Type" HeaderText="Document For">
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    </asp:BoundField>--%>
                                                                   <asp:TemplateField HeaderText="Type">
                                                                    <ItemTemplate>
                                                                        <%#GetDocumentName(Eval("ActualDocument_Type").ToString())%>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                   <asp:BoundField DataField="ActualDocumentUID" HeaderText="Status">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                   <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                         <%--<a class="view" href="ViewDocument.aspx?DocID=<%#Eval("DocumentUID")%>">View</a>--%>
                                                                        <asp:UpdatePanel runat="server" ID="UpdatePanelDownload" UpdateMode="Conditional" ChildrenAsTriggers="True">
                                                                        <ContentTemplate>
                                                                            <asp:LinkButton ID="LnkDownload" runat="server" CausesValidation="false" CommandArgument='<%#Eval("ActualDocumentUID")%>' CommandName="download">Download</asp:LinkButton>
                                                                            </ContentTemplate>
                                                                          <Triggers>
                                                                            <asp:PostBackTrigger ControlID="LnkDownload" />
                                                                          </Triggers>
                                                                        </asp:UpdatePanel>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                   <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                            <a class="<%#Convert.ToString(Eval("Doc_Type")) == "Cover Letter" ? "hideItem" : Convert.ToString(Eval("Doc_Type")) =="General Document" ? "hideItem" : "showModalDocumentHistory" %>" href="/_modal_pages/view-documenthistory.aspx?DocID=<%#Eval("ActualDocumentUID")%>&ProjectUID=<%#Eval("ProjectUID")%>">View History</a>
                                                                        </ItemTemplate>
                                                                       </asp:TemplateField>
                                                                   <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <a class="showModalDocumentEdit" href="/_modal_pages/edit-document.aspx?DocID=<%#Eval("ActualDocumentUID")%>&pUID=<%#Eval("ProjectUID")%>&wUID=<%#Eval("WorkPackageUID")%>"><span title="Edit" class="fas fa-edit"></span></a>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                   <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkactualdelete" runat="server" OnClientClick="return DeleteItem()" CausesValidation="false" CommandArgument='<%#Eval("ActualDocumentUID")%>' CommandName="delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                    <asp:BoundField DataField="ActualDocument_RelativePath" HeaderText="Path" ItemStyle-CssClass="hideItem" HeaderStyle-CssClass="hideItem">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ActualDocument_DirectoryName" HeaderText="Path" ItemStyle-CssClass="hideItem" HeaderStyle-CssClass="hideItem">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ActualDocumentUID" HeaderText="UID" ItemStyle-CssClass="hideItem" HeaderStyle-CssClass="hideItem">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ActualDocument_Name" HeaderText="Name" ItemStyle-CssClass="hideItem" HeaderStyle-CssClass="hideItem">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                               </Columns>
                                                <EmptyDataTemplate>
                                            No Documents Found !
                                        </EmptyDataTemplate>
                                                               </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            </ContentTemplate>
                         <Triggers>
                         <asp:AsyncPostBackTrigger ControlID="TreeView2" EventName="SelectedNodeChanged" />                    
                         </Triggers>
            </asp:UpdatePanel>
        </div>

      <%--add Submittal modal--%>
    <div id="ModAddsubmittal" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Submittal</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 480px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--add Submittal modal--%>
    <div id="ModEditsubmittal" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit Submittal</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 480px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

       <%--add document modal--%>
    <div id="ModAddDocument" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Document</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:500px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

     <%--edit document modal--%>
    <div id="ModEditDocument" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit Document</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:500px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--View document histroy modal--%>
    <div id="ModViewDocumentHistory" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Document History</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:500px;" loading="lazy"></iframe>
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
    <%--Mail document  modal--%>
    <div id="ModDocumentMail" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Send Document Link</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:180px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--Copied document list  modal--%>
    <div id="ModCopyDocument" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Copied Document List</h5>
                    <button aria-label="Close" class="close" onclick="javascript:parent.location.href=parent.location.href" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:450px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--Add General Document Structure  modal--%>
    <div id="ModAddGeneralDocumentStructure" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Sub Folder</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:200px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
    <%--Edit General Document Structure  modal--%>
    <div id="ModEditGeneralDocumentStructure" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit Sub Folder</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:200px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

      <%--add general document modal--%>
    <div id="ModAddGeneralDocument" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add General Document</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:500px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

     <%--edit general document modal--%>
    <div id="ModEditGeneralDocument" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit General Document</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:500px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
</asp:Content>

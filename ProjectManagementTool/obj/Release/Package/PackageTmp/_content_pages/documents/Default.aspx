<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ProjectManager._content_pages.documents.Default" %>
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
    function expand1() {
            $("[id*=imgProductsShow]").each(function () {
                if ($(this)[0].src.indexOf("minus") != -1) {
                    $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>");
                    $(this).next().remove();
                }
        });

        
    }
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
            expand1();
            expand2();
            //$('#loader').fadeOut();
        });
</script>
<%--  <script src="http://code.jquery.com/jquery-1.8.2.js"></script> 

<script type="text/javascript">  
   $(window).load(function() {  
      $("#loader").fadeOut(0);  
   });
</script> --%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
    <%--project selection dropdowns--%>
     <asp:ScriptManager ID="ScriptManager1" runat="server">
   </asp:ScriptManager>
  <%--<div id="loader"></div>  --%>
    <div class="container-fluid">
        <div class="row">
            <div class="col-lg-6 col-xl-4 form-group">Documents</div>
             <div class="col-md-6 col-lg-3 form-group">
                   
                </div>
                           <div class="col-md-6 col-lg-5 form-group">
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
               
        </div>
    </div>
    <div class="container-fluid" id="DivDocumentView" runat="server" visible="false">
        <div class="row">
          <div class="col-lg-6 col-xl-12 form-group">
                        <div class="card">
                            <div class="card-body" style="padding-bottom:0; margin-bottom:0;">
                                <div class="row">
                         <div class="col-lg-2">
                            <h6 class="card-title text-muted text-uppercase font-weight-bold">Documents View</h6>

                        </div>
                        <div class="col-lg-5">
                            <asp:RadioButtonList ID="RDBDocumentView" runat="server" class="card-title text-muted text-uppercase font-weight-bold" AutoPostBack="true" Width="50%" RepeatDirection="Horizontal" OnSelectedIndexChanged="RDBDocumentView_SelectedIndexChanged">
                            <asp:ListItem Value="Activity" Selected="True">&nbsp;By Activity</asp:ListItem>
                            <asp:ListItem Value="Category">&nbsp;By Category</asp:ListItem>
                        </asp:RadioButtonList>
                        </div>
                                    <div class="col-lg-3">
                                        </div>
                            <div class="col-lg-2">
                                <a href="/_modal_pages/copied-documents.aspx" class="showCopyModal">
                                <asp:Label ID="LblcopyFileCount" runat="server" CssClass="text-uppercase font-weight-bold"></asp:Label>
                                </a>
                            </div>
                                </div>
                            </div>
                        </div>
                       
                     </div>  
            </div>
    </div>
  
    <%--documents contents--%>
    <div class="container-fluid" id="ByActivity" runat="server">
       <%-- <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel2">
            <ProgressTemplate>
                <div class="modal">
                    <div class="center">
                        <img alt="" src="../../_assets/images/pageloading.gif" />
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>--%>
         <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-lg-6 col-xl-4 form-group">
                                    <div class="card h-100" style="max-height:700px; overflow-y:auto;">
                                        <div class="card-body">
                                            <h6 class="card-title text-muted text-uppercase font-weight-bold">Documents Directory</h6>
                                            <div class="form-group">
                                                <label class="sr-only" for="TxtSearchDocuments">Search</label>
                                                <div class="input-group">
                                                    <input id="TxtSearchDocuments" runat="server" autocomplete="off" class="form-control" type="text" placeholder="Activity name" />
                                                    <div class="input-group-append">
                                                        <asp:Button ID="BtnSearchDocuments" CssClass="btn btn-primary" Text="Search" runat="server" OnClick="BtnSearchDocuments_Click" />
                                                    </div>
                                                </div>
                                            </div>
                                            <asp:TreeView runat="server" CssClass="it_tree_view" ID="TreeView1" ImageSet="XPFileExplorer" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged" NodeIndent="15" EnableTheming="True" NodeWrap="True">    
                                                <%--<HoverNodeStyle Font-Underline="True" ForeColor="#6666AA" />--%>  
                                                <NodeStyle CssClass="it_tree_view__node" NodeSpacing="2px" />
                                                <ParentNodeStyle Font-Bold="False" />
                                                <SelectedNodeStyle CssClass="it_tree_view__node__selected"  HorizontalPadding="4px" VerticalPadding="2px" />
                                            </asp:TreeView>   
                                            
                                            <asp:TreeView runat="server" CssClass="it_tree_view" ID="TreeView3" ImageSet="XPFileExplorer" NodeIndent="15" EnableTheming="True" NodeWrap="True" OnSelectedNodeChanged="TreeView3_SelectedNodeChanged">    
                                                <%--<HoverNodeStyle Font-Underline="True" ForeColor="#6666AA" />--%>  
                                                <NodeStyle CssClass="it_tree_view__node" NodeSpacing="2px" />
                                                <ParentNodeStyle Font-Bold="False" />
                                                <SelectedNodeStyle CssClass="it_tree_view__node__selected"  HorizontalPadding="4px" VerticalPadding="2px" />
                                            </asp:TreeView>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-6 col-xl-8 form-group">
                                    <asp:UpdateProgress ID="UpdateProgress1" runat="Server" AssociatedUpdatePanelID="UpdatePanel2" DisplayAfter="1"> 
                                        <ProgressTemplate > 
                                        <!-- <asp:Label ID="lblwait" runat="server" Text="Please wait while we fetch the data.." ForeColor="Red"></asp:Label>-->
                                            <div id="loader"></div>
                                        </ProgressTemplate> 
                                    </asp:UpdateProgress>
                                        <div class="card mb-4" id="TreeGrid" runat="server">
                                            <div class="card-body">
                                                <div class="card-title">
                                                    <div class="d-flex justify-content-between">
                                                        <h6 class="text-muted">
                                                            <asp:Label ID="ActivityHeading" CssClass="text-uppercase font-weight-bold" runat="server" />
                                                            <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                                                        </h6>
                                                        
                                                        
                                                        <div>
                                                            
                                                            <a id="AddDocument" runat="server"  href="/_modal_pages/add-submittal.aspx" class="showModal"><asp:Button ID="btnSubmittal" runat="server" Text="+ Add Submittal" CssClass="btn btn-primary" align="end"></asp:Button></a>
                                                        </div>
                                                        
                                                    </div>
                                                    <div id="DivDocumentsSyncedCount" runat="server" visible="false" style="text-align:left; font-size:10pt; margin-top:10px;display:none">
                                                            
                                                            <b class="card-title text-muted text-uppercase">Last Synced Date : </b>
                                                            <asp:Label ID="LblLastSyncedDate" runat="server" Font-Bold="true" ForeColor="Green" Text="2021-09-03 14:34:05.847"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                                            <b class="card-title text-muted text-uppercase"><asp:Label ID="LblSourceHeading" runat="server"></asp:Label> : </b>
                                                            <asp:Label ID="LblTotalSourceDocuments" runat="server" Font-Bold="true" ForeColor="Green" Text="500"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                                            <b class="card-title text-muted text-uppercase"><asp:Label ID="LblDestinationHeading" runat="server"></asp:Label> : </b>
                                                            <asp:Label ID="LblTotalDestinationDocuments" runat="server" Font-Bold="true" ForeColor="Green" Text="500"></asp:Label>
                                                        </div>
                                                </div>
                        
                                                <div class="table-responsive">
                                                    <asp:GridView ID="GrdTreeView" runat="server" AutoGenerateColumns="false" PageSize="20" class="table table-bordered" 
                                                AllowPaging="true" 
                                                DataKeyNames="TaskUID" Width="100%" OnPageIndexChanging="GrdTreeView_PageIndexChanging" OnRowCommand="GrdTreeView_RowCommand">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Activity Name">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkview" runat="server" CommandName="view" CommandArgument='<%#Eval("TaskUID")%>'><%#Eval("Name")%></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Description" ItemStyle-Width="25%">
                                                       <ItemTemplate>
                                                           <%#LimitCharts(Eval("Description").ToString())%> &nbsp; <a href="javascript:void(0)" onclick="document.getElementById('<%#Eval("TaskUID")%>').style.display='block';"><%#ShoworHide(Eval("Description").ToString())%></a>
                                                            <div id='<%#Eval("TaskUID")%>' class="white_content"><span><%#Eval("Description").ToString()%></span> <br /><a style="float:right;" href="javascript:void(0)" onclick="document.getElementById('<%#Eval("TaskUID")%>').style.display='none';">Close</a>
                                                             </div>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                                    <asp:BoundField  DataField="StartDate" ItemStyle-Width="13%" HeaderText="Start Date" DataFormatString="{0:dd MMM yyyy}"/>
                                                      <asp:BoundField DataField="PlannedEndDate" ItemStyle-Width="13%" HeaderText="End Date" DataFormatString="{0:dd MMM yyyy}"/>
                                                    <asp:TemplateField HeaderText="Status" ItemStyle-Width="13%">
                                                        <ItemTemplate>
                                                            <span class='<%#Convert.ToString(Eval("Status")) == "I" ? "badge badge-primary font-scale-90" : Convert.ToString(Eval("Status")) =="C" ? "badge badge-success font-scale-90" : "badge badge-secondary font-scale-90" %>''><%#GetStatus(Eval("Status").ToString())%></span>
                                    
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                    
                                                </asp:GridView>

                                                    <asp:GridView ID="GrdOptions" runat="server" AutoGenerateColumns="false"
                                                        AllowPaging="false" CssClass="table table-bordered" EmptyDataText="No Data" Width="100%" OnRowCommand="GrdOptions_RowCommand">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Option">
                                                                <ItemTemplate>
                                                                    <%--<asp:LinkButton ID="lnkview" runat="server" CommandName="view" CommandArgument='<%#Eval("WorkPackageUID")%>'><%#GetWorkpackageOptionName(Eval("Workpackage_OptionUID").ToString())%></asp:LinkButton>--%>
                                                                    <asp:LinkButton ID="lnkview" runat="server" CommandName="view" CommandArgument='<%#Eval("WorkpackageSelectedOption_UID")%>'><%#Eval("WorkpackageSelectedOption_Name")%></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                                    <strong>No Records Found ! </strong>
                                                                </EmptyDataTemplate>
                                                        </asp:GridView>
                                                    <%--<table class="table table-bordered">
                                                        <thead>
                                                            <tr>
                                                                <th>Activity Name</th>
                                                                <th>Description</th>
                                                                <th>Start Date</th>
                                                                <th>End Date</th>
                                                                <th>Status</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <tr>
                                                                <td>Foo</td>
                                                                <td>Description goes here</td>
                                                                <td>31/Jul/2017</td>
                                                                <td>--</td>
                                                                <td><span class="badge badge-primary font-scale-90">In progress</span></td>
                                                            </tr>
                                                            <tr>
                                                                <td>Bar</td>
                                                                <td>Description goes here</td>
                                                                <td>20/Jan/2016</td>
                                                                <td>04/Nov/2019</td>
                                                                <td><span class="badge badge-success font-scale-90">Complete</span></td>
                                                            </tr>
                                                            <tr>
                                                                <td>Foo</td>
                                                                <td>Description goes here</td>
                                                                <td>--</td>
                                                                <td>--</td>
                                                                <td><span class="badge badge-secondary font-scale-90">Not started</span></td>
                                                            </tr>
                                                            <tr>
                                                                <td>Foo</td>
                                                                <td>Description goes here</td>
                                                                <td>31/Jul/2017</td>
                                                                <td>--</td>
                                                                <td><span class="badge badge-primary font-scale-90">In progress</span></td>
                                                            </tr>
                                                            <tr>
                                                                <td>Bar</td>
                                                                <td>Description goes here</td>
                                                                <td>20/Jan/2016</td>
                                                                <td>04/Nov/2019</td>
                                                                <td><span class="badge badge-success font-scale-90">Complete</span></td>
                                                            </tr>
                                                            <tr>
                                                                <td>Foo</td>
                                                                <td>Description goes here</td>
                                                                <td>--</td>
                                                                <td>--</td>
                                                                <td><span class="badge badge-secondary font-scale-90">Not started</span></td>
                                                            </tr>
                                                        </tbody>
                                                    </table>--%>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="card" id="DocumentGrid" runat="server">
                                            <div class="card-body">
                                                <h6 class="card-title text-muted text-uppercase font-weight-bold">Submittals</h6>
                                                <asp:GridView ID="GrdNewDocument" class="table table-bordered" DataKeyNames="DocumentUID" PageSize="15" AllowSorting="true" runat="server" AllowPaging="true" AutoGenerateColumns="False" Width="100%" OnPageIndexChanging="GrdNewDocument_PageIndexChanging" OnRowCommand="GrdNewDocument_RowCommand" OnRowDeleting="GrdNewDocument_RowDeleting" OnRowDataBound="GrdNewDocument_RowDataBound" >
                                        <Columns>
                                            <asp:TemplateField>
                                                  <ItemTemplate>
                                                      <asp:ImageButton ID="imgProductsShow" runat="server" OnClick="Show_Hide_DocumentsGrid" ImageUrl="~/_assets/images/plus.png"
                                                                CommandArgument="Show" Height="25px" Width="25px"/>
                                                      <asp:Panel ID="pnlDocuments" runat="server" Visible="false" style="position:relative;">
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
                                                                  $(".showModalDocumentMail").click(function(e) {
                                                                    e.preventDefault();
                                                                    var url = $(this).attr("href");
                                                                    $("#ModDocumentMail iframe").attr("src", url);
                                                                    $("#ModDocumentMail").modal("show");
                                                                   });
                                                              });
                                                        </script>
                                                          <div style="overflow: auto;">
                                                          <asp:GridView ID="GrdActualDocuments" runat="server" Width="100%" AutoGenerateColumns="false"  class="table table-bordered"
                                                                    AllowPaging="false" DataKeyNames="ActualDocumentUID" OnRowDataBound="GrdActualDocuments_RowDataBound" OnRowCommand="GrdActualDocuments_RowCommand" OnRowDeleting="GrdActualDocuments_RowDeleting" >
                                                               <Columns>
                                                                   <asp:TemplateField HeaderText="Name" ItemStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                      
                                                                       <%-- <asp:UpdatePanel runat="server" ID="UpdatePanelview" UpdateMode="Conditional" ChildrenAsTriggers="True">
                                                                        <ContentTemplate>--%>
                                                                        <div id="divPDF" runat="server">
                                                                                <img width="24" src='<%#GetDocumentTypeIcon(Eval("ActualDocument_Type").ToString(),Eval("ActualDocumentUID").ToString(),"Actual Document")%>' alt='<%#Eval("ActualDocument_Type")%>' />  &nbsp;&nbsp;
                                                                            <%--<asp:LinkButton ID="lnkviewpdf" class='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? "view" : "hideItem" %>' runat="server" CausesValidation="false" CommandArgument='<%#Eval("ActualDocument_Path")%>' CommandName="ViewDoc"><%#Eval("ActualDocument_Name")%></asp:LinkButton>--%>
                                                                            <%--<a id="ShowFile" href='/_modal_pages/preview-document.aspx?ActualDocumentUID=<%#Eval("ActualDocumentUID")%>&previewpath=<%#Eval("ActualDocument_Path").ToString().Replace('&','!')%>' class='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".xls" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".xlsx" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".docx" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".doc" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".PDF" ? "showModalPreviewDocument" : "hideItem" %>'><%#Eval("ActualDocument_Name")%></a>--%>
                                                                            <a id="ShowFile" href='/_modal_pages/preview-document.aspx?ActualDocumentUID=<%#Eval("ActualDocumentUID")%>&previewpath=<%#Eval("ActualDocument_Path").ToString().Replace('&','!')%>' class='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".xls" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".xlsx" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".docx" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".doc" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".PDF" ? "showModalPreviewDocument" : "hideItem" %>'>
                                                                                <asp:Label ID="lblDocumentName" runat="server" Text='<%#Eval("ActualDocument_Name")%>'></asp:Label>
                                                                            </a>
                                                                            <asp:Label ID="lblName" runat="server" Text='<%#Eval("ActualDocument_Name")%>' Visible='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".xls" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".xlsx" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".docx" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".doc" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".PDF" ? false : true %>'></asp:Label>
                                                                                <asp:Label ID="LblVersion" runat="server"></asp:Label>
                                                                            </div>
                                                                        <%--<a href='/_modal_pages/preview-document.aspx?previewpath=<%#Eval("ActualDocument_Path").ToString().Replace('&','!')%>&ActualDocumentUID=<%#Eval("ActualDocumentUID")%>' class='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? "showModalPreviewDocument" : "hideItem" %>'><%#Eval("ActualDocument_Name")%></a>
                                                                            <asp:Label ID="lblName" runat="server" Text='<%#Eval("ActualDocument_Name")%>' Visible='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".xls" ? false : true %>'></asp:Label>
                                                                                <asp:Label ID="LblVersion" runat="server"></asp:Label>--%>
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
                                                                        <asp:UpdatePanel runat="server" ID="UpdatePanelDownload" UpdateMode="Conditional" ChildrenAsTriggers="True">
                                                                        <ContentTemplate>
                                                                            <asp:LinkButton ID="LnkDownloadnew" runat="server" CausesValidation="false" CommandArgument='<%#Eval("ActualDocumentUID")%>' CommandName="download">Download</asp:LinkButton>
                                                                            </ContentTemplate>
                                                                          <Triggers>
                                                                            <asp:PostBackTrigger ControlID="LnkDownloadnew" />
                                                                          </Triggers>
                                                                        </asp:UpdatePanel>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                   <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                            <a id="ViewByActivity" class="<%#Convert.ToString(Eval("Doc_Type")) == "Cover Letter" ? "hideItem" : Convert.ToString(Eval("Doc_Type")) =="General Document" ? "hideItem" : "showModalDocumentHistory" %>" 
                                                                                href="/_modal_pages/view-documenthistory.aspx?DocID=<%#Eval("ActualDocumentUID")%>&ProjectUID=<%#Eval("ProjectUID")%>&FlowUID=<%#Eval("FlowUID")%>">View/Update Status</a>

                                                                       <%-- <asp:HyperLink ID="anchorView" runat="server" CssClass='<%#Convert.ToString(Eval("Doc_Type")) == "Cover Letter" ? "hideItem" : Convert.ToString(Eval("Doc_Type")) =="General Document" ? "hideItem" : "showModalDocumentHistory" %>'
                                                                              NavigateUrl='/_modal_pages/view-documenthistory.aspx?DocID=<%#Eval("ActualDocumentUID")%>&ProjectUID=<%#Eval("ProjectUID")%>'
                                                                              Text="View/Update Status">
                                                                            </asp:HyperLink>--%>
                                                                        </ItemTemplate>
                                                                       </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <a class="showModalDocumentMail" href="/_modal_pages/mail-documentlink.aspx?DocID=<%#Eval("ActualDocumentUID")%>&pUID=<%#Eval("ProjectUID")%>&wUID=<%#Eval("WorkPackageUID")%>"><span title="Mail" class="fas fa-envelope"></span></a>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                   <%--<asp:TemplateField ItemStyle-CssClass="hideItem" HeaderStyle-CssClass="hideItem">--%>
                                                                   <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:UpdatePanel runat="server" ID="UpdatePanelCopy" UpdateMode="Conditional" ChildrenAsTriggers="True">
                                                                        <ContentTemplate>
                                                                        <asp:LinkButton ID="lnkcopy" runat="server" CausesValidation="false" CommandArgument='<%#Eval("ActualDocumentUID")%>' CommandName="copyfile"><span title="Copy" class="fas fa-copy"></span></asp:LinkButton>
                                                                             </ContentTemplate>
                                                                            <Triggers>
                                                                                <asp:PostBackTrigger ControlID="lnkcopy" />
                                                                            </Triggers>
                                                                        </asp:UpdatePanel>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                   <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <a class="showModalDocumentEdit" href="/_modal_pages/edit-document.aspx?DocID=<%#Eval("ActualDocumentUID")%>&pUID=<%#Eval("ProjectUID")%>&wUID=<%#Eval("WorkPackageUID")%>"><span title="Edit" class="fas fa-edit"></span></a>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                   <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                       <%-- <asp:UpdatePanel runat="server" ID="UpdatePanelDelete" UpdateMode="Conditional" ChildrenAsTriggers="True">
                                                                        <ContentTemplate>--%>
                                                                        <asp:LinkButton ID="lnkactualdelete" runat="server" OnClientClick="return DeleteItem()" CausesValidation="false" CommandArgument='<%#Eval("ActualDocumentUID")%>' CommandName="delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
                                                                            <%--</ContentTemplate>
                                                                            <Triggers>
                                                                                <asp:AsyncPostBackTrigger ControlID="lnkactualdelete" EventName="RowCommand" />
                                                                                
                                                                            </Triggers>
                                                                            </asp:UpdatePanel>--%>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                    <asp:BoundField DataField="ActualDocument_RelativePath" HeaderText="Path" ItemStyle-CssClass="hideItem" HeaderStyle-CssClass="hideItem">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                   
                                                               </Columns>
                                                               <EmptyDataTemplate>
                                                                    No Documents Found !
                                                                </EmptyDataTemplate>
                                                               </asp:GridView>
                                                              </div>
                                                       </asp:Panel>
                                                  </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Submittal Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <%#Eval("DocName")%>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <%#getCategoryName(Eval("Doc_Category").ToString())%>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                            </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Created Date" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <%#Eval("CreatedDate","{0:dd MMM yyyy}")%>
                                                </ItemTemplate>
                                             <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                            </asp:TemplateField>
                                             <asp:TemplateField>
                                                <ItemTemplate>
                                                    <%--<button class="btn btn-primary" type="button" data-toggle="modal" data-target="#ModAddWorkpackages"><i class="fa fa-plus" aria-hidden="true"></i> Add WorkPackage</button>--%>
                                                    <a href="/_modal_pages/add-document.aspx?dUID=<%#Eval("DocumentUID")%>&pUID=<%#Eval("ProjectUID")%>&wUID=<%#Eval("WorkPackageUID")%>&tUID=<%#Eval("TaskUID")%>&sType=<%#RDBDocumentView.SelectedValue%>&fID=<%#Eval("FlowUID")%>" class="showModalDocument"><asp:Label ID="lbl1" runat="server" Text="Add Documents"></asp:Label></a>
                                                    <%--<a data-toggle="modal" data-target="#ModAddDocuments" href="#">Add Documents</a>--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <a class="showModalEdit" href="/_modal_pages/add-submittal.aspx?type=edit&DocID=<%#Eval("DocumentUID")%>&TaskUID=<%#Eval("TaskUID")%>&PrjUID=<%#Eval("ProjectUID")%>&WrkUID=<%#Eval("WorkPackageUID")%>&fID=<%#Eval("FlowUID")%>"><span title="Edit" class="fas fa-edit"></span></a>
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

                                                <asp:GridView ID="GrdActualDocuments_new" runat="server" Width="100%" AutoGenerateColumns="false"  class="table table-bordered"
                                                                    AllowPaging="false" DataKeyNames="ActualDocumentUID" OnRowDataBound="GrdActualDocuments_new_RowDataBound" OnRowCommand="GrdActualDocuments_new_RowCommand" OnRowDeleting="GrdActualDocuments_new_RowDeleting" >
                                                               <Columns>
                                                                   <asp:TemplateField HeaderText="Name" ItemStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                      
                                                                       <%-- <asp:UpdatePanel runat="server" ID="UpdatePanelview" UpdateMode="Conditional" ChildrenAsTriggers="True">
                                                                        <ContentTemplate>--%>
                                                                        <div id="divPDF" runat="server">
                                                                                <img width="24" src='../../_assets/images/<%#GetDocumentTypeIcon(Eval("ActualDocument_Type").ToString(),Eval("ActualDocumentUID").ToString(),"Actual Document")%>' alt='<%#Eval("ActualDocument_Type")%>' />  &nbsp;&nbsp;
                                                                            <%--<asp:LinkButton ID="lnkviewpdf" class='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? "view" : "hideItem" %>' runat="server" CausesValidation="false" CommandArgument='<%#Eval("ActualDocument_Path")%>' CommandName="ViewDoc"><%#Eval("ActualDocument_Name")%></asp:LinkButton>--%>
                                                                            <%--<a href='/_modal_pages/preview-document.aspx?ActualDocumentUID=<%#Eval("ActualDocumentUID")%>&previewpath=<%#Eval("ActualDocument_Path").ToString().Replace('&','!')%>' class='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".xls" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".xlsx" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".docx" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".doc" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".PDF" ? "showModalPreviewDocument" : "hideItem" %>'><%#Eval("ActualDocument_Name")%></a>--%>
                                                                            <a href='/_modal_pages/preview-document.aspx?ActualDocumentUID=<%#Eval("ActualDocumentUID")%>&previewpath=<%#Eval("ActualDocument_Path").ToString().Replace('&','!')%>' class='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".xls" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".xlsx" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".docx" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".doc" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".PDF" ? "showModalPreviewDocument" : "hideItem" %>'>
                                                                                <asp:Label ID="lblDocumentName" runat="server" Text='<%#Eval("ActualDocument_Name")%>'></asp:Label>

                                                                            </a>
                                                                            <asp:Label ID="lblName" runat="server" Text='<%#Eval("ActualDocument_Name")%>' Visible='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".xls" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".xlsx" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".docx" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".doc" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".PDF" ? false : true %>'></asp:Label>
                                                                                <asp:Label ID="LblVersion" runat="server"></asp:Label>
                                                                        <%--<a href='/_modal_pages/preview-document.aspx?previewpath=<%#Eval("ActualDocument_Path").ToString().Replace('&','!')%>&ActualDocumentUID=<%#Eval("ActualDocumentUID")%>' class='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? "showModalPreviewDocument" : "hideItem" %>'><%#Eval("ActualDocument_Name")%></a>
                                                                            <asp:Label ID="lblName" runat="server" Text='<%#Eval("ActualDocument_Name")%>' Visible='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".xls" ? false : true %>'></asp:Label>
                                                                                <asp:Label ID="LblVersion" runat="server"></asp:Label>--%>
                                                                            </div>
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
                                                                        <asp:UpdatePanel runat="server" ID="UpdatePanelDownload1" UpdateMode="Conditional" ChildrenAsTriggers="True">
                                                                        <ContentTemplate>
                                                                            <asp:LinkButton ID="LnkDownload_new" runat="server" CausesValidation="false" CommandArgument='<%#Eval("ActualDocumentUID")%>' CommandName="download">Download</asp:LinkButton>
                                                                            </ContentTemplate>
                                                                          <Triggers>
                                                                            <asp:PostBackTrigger ControlID="LnkDownload_new" />
                                                                          </Triggers>
                                                                        </asp:UpdatePanel>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                   <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                            <a class="<%#Convert.ToString(Eval("Doc_Type")) == "Cover Letter" ? "hideItem" : Convert.ToString(Eval("Doc_Type")) =="General Document" ? "hideItem" : "showModalDocumentHistory" %>" href="/_modal_pages/view-documenthistory.aspx?DocID=<%#Eval("ActualDocumentUID")%>&ProjectUID=<%#Eval("ProjectUID")%>&FlowUID=<%#Eval("FlowUID")%>">View/Update Status</a>
                                                                        </ItemTemplate>
                                                                       </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <a class="showModalDocumentMail" href="/_modal_pages/mail-documentlink.aspx?DocID=<%#Eval("ActualDocumentUID")%>&pUID=<%#Eval("ProjectUID")%>&wUID=<%#Eval("WorkPackageUID")%>"><span title="Mail" class="fas fa-envelope"></span></a>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                   <%--<asp:TemplateField ItemStyle-CssClass="hideItem" HeaderStyle-CssClass="hideItem">--%>
                                                                   <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:UpdatePanel runat="server" ID="UpdatePanelCopy1" UpdateMode="Conditional" ChildrenAsTriggers="True">
                                                                        <ContentTemplate>
                                                                        <asp:LinkButton ID="lnkcopy1" runat="server" CausesValidation="false" CommandArgument='<%#Eval("ActualDocumentUID")%>' CommandName="copyfile"><span title="Copy" class="fas fa-copy"></span></asp:LinkButton>
                                                                             </ContentTemplate>
                                                                            <Triggers>
                                                                                <asp:PostBackTrigger ControlID="lnkcopy1" />
                                                                            </Triggers>
                                                                        </asp:UpdatePanel>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                   <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <a class="showModalDocumentEdit" href="/_modal_pages/edit-document.aspx?DocID=<%#Eval("ActualDocumentUID")%>&pUID=<%#Eval("ProjectUID")%>&wUID=<%#Eval("WorkPackageUID")%>"><span title="Edit" class="fas fa-edit"></span></a>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                   <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkactualdeletenew" runat="server" OnClientClick="return DeleteItem()" CausesValidation="false" CommandArgument='<%#Eval("ActualDocumentUID")%>' CommandName="delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
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
                                        <div id="GeneralDocumentsDiv" runat="server" visible="false">
                                            <div class="card mb-4">
                                            <div class="card-body">
                                                <div class="card-title">
                                                    <div class="d-flex justify-content-between">
                                                        <h6 class="text-muted">
                                                            <asp:Label ID="LblGeneralDocumentHeading" CssClass="text-uppercase font-weight-bold" runat="server" />
                                                        </h6>
                                                        <div>
                                                            <a id="AddGeneralDocumentStructure" runat="server"  href="/_modal_pages/add-generaldocumentstructure.aspx" class="showAddGeneralDocumentStructureModal"><asp:Button ID="btnAddGeneralDoc" runat="server" Text="+ Add SubFolder" CssClass="btn btn-primary" align="end"></asp:Button></a>
                                                            <a id="AddGeneralDocument" runat="server"  href="/_modal_pages/add-general-document.aspx" class="showAddGeneralDocumentModal"><asp:Button ID="btnaddDocs" runat="server" Text="+ Add Documents" CssClass="btn btn-primary" align="end"></asp:Button></a>
                                                        </div>
                                                    </div>
                                                </div>
                        
                                                <div class="table-responsive">
                                                    <asp:GridView ID="GrdGeneralDocumentStructure" runat="server" AutoGenerateColumns="false"
                                                        AllowPaging="false" CssClass="table table-bordered" EmptyDataText="No Data" Width="100%" OnRowCommand="GrdGeneralDocumentStructure_RowCommand" OnRowDeleting="GrdGeneralDocumentStructure_RowDeleting" >
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Folder">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkview" runat="server" CommandName="view" CommandArgument='<%#Eval("StructureUID")%>'><%#Eval("Structure_Name")%></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <a class="showGeneralDocumentStructureEditModal" href="/_modal_pages/add-generaldocumentstructure.aspx?StructureUID=<%#Eval("StructureUID")%>&ParentUID=<%#Eval("ParentStructureUID")%>"><span title="Edit" class="fas fa-edit"></span></a>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="Lnkgeneraldocstructuredelete" runat="server" OnClientClick="return DeleteItem()" CausesValidation="false" CommandArgument='<%#Eval("StructureUID")%>' CommandName="delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                                    <strong>No Records Found ! </strong>
                                                                </EmptyDataTemplate>
                                                        </asp:GridView>
                                                </div>

                                                
                                            </div>
                                        </div>
                                            <div class="card mb-4">
                                            <div class="card-body">
                                                <div class="card-title">
                                                    <div class="d-flex justify-content-between">
                                                        <h6 class="text-muted">
                                                            <asp:Label ID="Label1" CssClass="text-uppercase font-weight-bold" Text="Documents List" runat="server" />
                                                        </h6>
                                                        <div>
                                                           
                                                        </div>
                                                    </div>
                                                    </div>
                                                <div class="table-responsive">
                                                    <asp:GridView ID="GrdGeneralDocuments" runat="server" Width="100%" AutoGenerateColumns="false"  class="table table-bordered"
                                                                    AllowPaging="false" DataKeyNames="GeneralDocumentUID" OnRowCommand="GrdGeneralDocuments_RowCommand" OnRowDeleting="GrdGeneralDocuments_RowDeleting">
                                                               <Columns>
                                                                   <asp:TemplateField HeaderText="Name" ItemStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <div id="divPDF" runat="server">
                                                                                <img width="24" src='../../_assets/images/<%#GetDocumentTypeIcon(Eval("GeneralDocument_Type").ToString(),Eval("GeneralDocumentUID").ToString(),"General Document")%>' alt='<%#Eval("GeneralDocument_Type")%>' />  &nbsp;&nbsp;
                                                                            <a id="ShowFile" href='/_modal_pages/preview-document.aspx?GeneralDocumentUID=<%#Eval("GeneralDocumentUID")%>&previewpath=<%#Eval("GeneralDocument_Path").ToString().Replace('&','!')%>' class='<%#Convert.ToString(Eval("GeneralDocument_Type")) == ".pdf" ? "showModalPreviewDocument" : Convert.ToString(Eval("GeneralDocument_Type")) == ".xls" ? "showModalPreviewDocument" : Convert.ToString(Eval("GeneralDocument_Type")) == ".xlsx" ? "showModalPreviewDocument" : Convert.ToString(Eval("GeneralDocument_Type")) == ".docx" ? "showModalPreviewDocument" : Convert.ToString(Eval("GeneralDocument_Type")) == ".doc" ? "showModalPreviewDocument" : Convert.ToString(Eval("GeneralDocument_Type")) == ".PDF" ? "showModalPreviewDocument" : "hideItem" %>'><%#Eval("GeneralDocument_Name")%></a>
                                                                            <asp:Label ID="lblName" runat="server" Text='<%#Eval("GeneralDocument_Name")%>' Visible='<%#Convert.ToString(Eval("GeneralDocument_Type")) == ".pdf" ? false : Convert.ToString(Eval("GeneralDocument_Type")) == ".xls" ? false : Convert.ToString(Eval("GeneralDocument_Type")) == ".xlsx" ? false : Convert.ToString(Eval("GeneralDocument_Type")) == ".docx" ? false : Convert.ToString(Eval("GeneralDocument_Type")) == ".doc" ? false : Convert.ToString(Eval("GeneralDocument_Type")) == ".PDF" ? false : true %>'></asp:Label>
                                                                                <asp:Label ID="LblVersion" runat="server"></asp:Label>
                                                                            </div>
                                                                             </ContentTemplate>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                   <asp:TemplateField HeaderText="Type">
                                                                    <ItemTemplate>
                                                                        <%#GetDocumentName(Eval("GeneralDocument_Type").ToString())%>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                   <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:UpdatePanel runat="server" ID="UpdatePanelDownload1" UpdateMode="Conditional" ChildrenAsTriggers="True">
                                                                        <ContentTemplate>
                                                                            <asp:LinkButton ID="LnkDownloadnew1" runat="server" CausesValidation="false" CommandArgument='<%#Eval("GeneralDocumentUID")%>' CommandName="download">Download</asp:LinkButton>
                                                                            </ContentTemplate>
                                                                          <Triggers>
                                                                            <asp:PostBackTrigger ControlID="LnkDownloadnew1" />
                                                                          </Triggers>
                                                                        </asp:UpdatePanel>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <a  class="showModalDocumentMailGenDocs" href="/_modal_pages/mail-documentlink.aspx?GeneralDocumentUID=<%#Eval("GeneralDocumentUID")%>"><span title="Mail" class="fas fa-envelope"></span></a>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                   <%--<asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:UpdatePanel runat="server" ID="UpdatePanelCopy" UpdateMode="Conditional" ChildrenAsTriggers="True">
                                                                        <ContentTemplate>
                                                                        <asp:LinkButton ID="lnkcopy" runat="server" CausesValidation="false" CommandArgument='<%#Eval("ActualDocumentUID")%>' CommandName="copyfile"><span title="Copy" class="fas fa-copy"></span></asp:LinkButton>
                                                                             </ContentTemplate>
                                                                            <Triggers>
                                                                                <asp:PostBackTrigger ControlID="lnkcopy" />
                                                                            </Triggers>
                                                                        </asp:UpdatePanel>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>--%>
                                                                   <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <a class="showGeneralDocumentEditModal" href="/_modal_pages/add-general-document.aspx?GeneralDocumentUID=<%#Eval("GeneralDocumentUID")%>&StructureUID=<%#Eval("StructureUID")%>"><span title="Edit" class="fas fa-edit"></span></a>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                   <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkdelete_GD" runat="server" OnClientClick="return DeleteItem()" CausesValidation="false" CommandArgument='<%#Eval("GeneralDocumentUID")%>' CommandName="delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                               </Columns>
                                                               <EmptyDataTemplate>
                                                                   <strong>No Documents Found !</strong>
                                                                    
                                                                </EmptyDataTemplate>
                                                               </asp:GridView>
                                                </div>
                                                </div>
                                                </div>
                                        </div>
                                </div>
                            </div>
                            </ContentTemplate>  
                 <Triggers>
                         <asp:AsyncPostBackTrigger ControlID="TreeView1" EventName="SelectedNodeChanged" />  
                     <%--<asp:PostBackTrigger ControlID="TreeView1" />--%>
                     <asp:PostBackTrigger ControlID="BtnSearchDocuments" />
                     
                     <%--<asp:PostBackTrigger ControlID="GrdTreeView" />
                     <asp:PostBackTrigger ControlID="GrdOptions" />--%>
                     <asp:AsyncPostBackTrigger ControlID="GrdTreeView" EventName="RowCommand" />
                     <asp:AsyncPostBackTrigger ControlID="GrdOptions" EventName="RowCommand"/>
                     <asp:AsyncPostBackTrigger ControlID="GrdGeneralDocumentStructure" EventName="RowCommand"/>
                     <asp:AsyncPostBackTrigger ControlID="GrdGeneralDocuments" EventName="RowCommand"/>
                         </Triggers>
                 </asp:UpdatePanel>
    </div>

    <div class="container-fluid" id="ByCategory" runat="server">
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
                                    <div class="card mb-4" id="ProjectDetails" runat="server">
                                    <div class="card-body">
                                        <h6 class="card-title text-muted text-uppercase font-weight-bold">Project Details
                                            </h6>
                                        <div class="table-responsive">
                                             <table class="table table-borderless">
                                           <tr><td>Project Name</td><td>:</td><td colspan="6">
                                               <asp:Label ID="lblPrjName" runat="server" Font-Bold="true"></asp:Label>
                                               </td>
                                               </tr>
                                               <tr>
                                                   <td>Funding Agency</td>
                                                   <td>:</td><td colspan="6">
                                                   <asp:Label ID="LblFundingAgency" runat="server"></asp:Label></td>
                                               </tr>
                                                 <tr><td>Start Date</td><td>:</td><td>
                                                   <asp:Label ID="lblStartDate" runat="server"></asp:Label>
                                                     </td><td>Planned End Date</td><td>:</td><td>
                                                     <asp:Label ID="lblPlannedEndDate" runat="server"></asp:Label>
                                                     </td>
                                                 </tr>
                                                 <tr>
                                                      <td>Projected End Date</td><td>:</td><td>
                                                     <asp:Label ID="lblPrjctedEndDate" runat="server"></asp:Label>
                                                     </td>
                                                     <td>Status</td><td>:</td><td>
                                                     <asp:Label ID="lblPrjStatus" runat="server"></asp:Label>
                                                     </td>
                                                 </tr>
                                               </table>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="card mb-4" id="WorkPackageDetils" runat="server">
                                    <div class="card-body">
                                        <h6 class="card-title text-muted text-uppercase font-weight-bold">WorkPackage Details
                                            </h6>
                                        <div class="table-responsive">
                                            <table class="table table-borderless">
                               <tr><td>Name</td><td>:</td><td colspan="2">
                                   <asp:Label ID="LblWorkPackageName" runat="server" Font-Bold="true"></asp:Label>
                                   </td></tr>
                                 <tr>
                                     <td>Location </td><td>:</td><td>
                                   <asp:Label ID="LblWorkPackageLocation" runat="server"></asp:Label>
                                   </td>
                                      <td>End User</td><td>:</td><td>
                                   <asp:Label ID="LblWorkPackageClient" runat="server"></asp:Label>
                                     </td>
                                     </tr>
                               <tr>
                                   <td>Budget</td><td>:</td><td>
                                       <asp:Label ID="LblWorkPackageCurrency" ForeColor="#006699" runat="server"></asp:Label>
                                     <asp:Label ID="LblWorkPackageBudget" runat="server"></asp:Label>
                                     </td>
                                   <td>Status</td><td>:</td><td>
                                     <asp:Label ID="LblWorkPackageStatus" runat="server"></asp:Label>
                                     </td>
                               </tr>
             
                           </table>
                                            </div>
                                        </div>
                                    </div>
                                        <div class="card mb-4" id="CategoryDocumentGridHeading" runat="server">
                                            <div class="card-body">
                                                <div class="card-title">
                                                    <div class="d-flex justify-content-between">
                                                        <h6 class="text-muted">
                                                            <asp:Label ID="ActivityHeadingCategory" CssClass="text-uppercase font-weight-bold" runat="server" />
                                                            <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
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
                                                <asp:GridView ID="GrdDocumentByCategory" class="table table-bordered" DataKeyNames="DocumentUID" PageSize="15" AllowSorting="true" runat="server" AllowPaging="true" AutoGenerateColumns="False" Width="100%" OnPageIndexChanging="GrdDocumentByCategory_PageIndexChanging" OnRowCommand="GrdDocumentByCategory_RowCommand" OnRowDeleting="GrdDocumentByCategory_RowDeleting" OnRowDataBound="GrdDocumentByCategory_RowDataBound">
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
                                                                           
                                                                           <%-- <a href='/_modal_pages/preview-document.aspx?ActualDocumentUID=<%#Eval("ActualDocumentUID")%>&previewpath=<%#Eval("ActualDocument_Path").ToString().Replace('&','!')%>' class='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".xls" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".xlsx" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".docx" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".doc" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".PDF" ? "showModalPreviewDocument" : "hideItem" %>'><%#Eval("ActualDocument_Name")%></a>--%>
                                                                            <a href='/_modal_pages/preview-document.aspx?ActualDocumentUID=<%#Eval("ActualDocumentUID")%>&previewpath=<%#Eval("ActualDocument_Path").ToString().Replace('&','!')%>' class='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".xls" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".xlsx" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".docx" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".doc" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".PDF" ? "showModalPreviewDocument" : "hideItem" %>'>
                                                                                <asp:Label ID="lblDocumentName" runat="server" Text='<%#Eval("ActualDocument_Name")%>'></asp:Label>

                                                                            </a>
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
                                           <asp:TemplateField HeaderText="Created Date" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <%#Eval("CreatedDate","{0:dd MMM yyyy}")%>
                                                </ItemTemplate>
                                             <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                            </asp:TemplateField>
                                             <asp:TemplateField>
                                                <ItemTemplate>
                                                    <%--<button class="btn btn-primary" type="button" data-toggle="modal" data-target="#ModAddWorkpackages"><i class="fa fa-plus" aria-hidden="true"></i> Add WorkPackage</button>--%>
                                                    <a class="showModalDocument" href="/_modal_pages/add-document.aspx?dUID=<%#Eval("DocumentUID")%>&pUID=<%#Eval("ProjectUID")%>&wUID=<%#Eval("WorkPackageUID")%>&tUID=<%#Eval("TaskUID")%>&sType=<%#RDBDocumentView.SelectedValue%>">Add Documents</a>
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

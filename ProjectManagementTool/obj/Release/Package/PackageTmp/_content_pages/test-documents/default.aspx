<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.test_documents._default" %>
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
        });
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
      <%--project selection dropdowns--%>
     <asp:ScriptManager ID="ScriptManager1" runat="server">
   </asp:ScriptManager>
  <%--<div id="loader"></div>  --%>
    <div class="container-fluid">
        <div class="row">
            <div class="col-lg-6 col-xl-8 form-group">Documents</div>
            <div class="col-lg-6 col-xl-4 form-group">
               <%-- <label class="sr-only" for="DDLProject">Project</label>
                <div class="input-group">
                    <div class="input-group-prepend">
                        <span class="input-group-text">Project</span>
                    </div>
                    <asp:DropDownList CssClass="form-control" ID="DDLProject" runat="server"></asp:DropDownList>
                </div>--%>
            </div>
        </div>
    </div>
    <div class="container-fluid">
        <div class="row">
          <div class="col-lg-6 col-xl-12 form-group">
                        <div class="card">
                            <div class="card-body" style="padding-bottom:0; margin-bottom:0;">
                                <div class="row">
                         <div class="col-lg-2">
                            <h6 class="card-title text-muted text-uppercase font-weight-bold">Documents View</h6>

                        </div>
                        <div class="col-lg-5">
                            <asp:RadioButtonList ID="RDBDocumentView" runat="server" class="card-title text-muted text-uppercase font-weight-bold" AutoPostBack="true" Width="50%" RepeatDirection="Horizontal" >
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
                                                        <asp:Button ID="BtnSearchDocuments" CssClass="btn btn-primary" Text="Search" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                            <asp:TreeView runat="server" CssClass="it_tree_view" ID="TreeView1" ImageSet="XPFileExplorer" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged" NodeIndent="15" EnableTheming="True" NodeWrap="True">    
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
                                       <asp:Label ID="LblShowMessage" runat="server" Font-Bold="true" ForeColor="Green" Font-Size="XX-Large"></asp:Label>
                                </div>
                            </div>
                            </ContentTemplate>  
                 <Triggers>
                         <asp:AsyncPostBackTrigger ControlID="TreeView1" EventName="SelectedNodeChanged" />  
                     <%--<asp:PostBackTrigger ControlID="TreeView1" />--%>
                     <asp:PostBackTrigger ControlID="BtnSearchDocuments" />
                     
                   
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

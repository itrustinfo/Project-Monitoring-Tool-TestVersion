using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManager.DAL;

namespace ProjectManagementTool._modal_pages
{
    public class FolderStructure
    {
        public string StructureID;
        public string FolderName;
    }
    public partial class add_general_document : System.Web.UI.Page
    {

        DBGetData getdata = new DBGetData();
        TaskUpdate TKUpdate = new TaskUpdate();
        GeneralDocuments GD = new GeneralDocuments();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                if(Request.QueryString["StructureUID"] != null)
                {
                    lblFolder.Text = GD.GetFolderNameByUID(new Guid(Request.QueryString["StructureUID"].ToString()));
                }
                if (Request.QueryString["GeneralDocumentUID"] != null)
                {
                    BindGeneralDocuments(Request.QueryString["GeneralDocumentUID"]);
                }
                if (Request.QueryString["PrJUID"] != null)
                {
                    Session["PrjUID"] = Request.QueryString["PrJUID"].ToString();
                }
                
            }
        }
        private void BindGeneralDocuments(string GeneralDocumentUID)
        {
            DataSet ds = GD.GeneralDocuments_SelectBy_GeneralDocumentUID(new Guid(GeneralDocumentUID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DocNameDiv.Visible = true;
                UploadFile.Visible = false;
                UploadType.Visible = false;
                txtDocumentName.Text = ds.Tables[0].Rows[0]["GeneralDocument_Name"].ToString();
                txtdesc.Text = ds.Tables[0].Rows[0]["GeneralDocument_Description"].ToString();
                txtprefnumber.Text = ds.Tables[0].Rows[0]["Ref_Number"].ToString();

                if (ds.Tables[0].Rows[0]["IncomingRec_Date"].ToString() != null && ds.Tables[0].Rows[0]["IncomingRec_Date"].ToString() != "")
                {
                    dtIncomingRevDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["IncomingRec_Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                if (ds.Tables[0].Rows[0]["Document_Date"].ToString() != null && ds.Tables[0].Rows[0]["Document_Date"].ToString() != "")
                {
                    dtDocumentDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Document_Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                DDLDocumentMedia.Items[0].Selected = ds.Tables[0].Rows[0]["Media_HC"].ToString() == "true" ? true : false;
                DDLDocumentMedia.Items[1].Selected = ds.Tables[0].Rows[0]["Media_SC"].ToString() == "true" ? true : false;
                DDLDocumentMedia.Items[2].Selected = ds.Tables[0].Rows[0]["Media_SCEF"].ToString() == "true" ? true : false;
                DDLDocumentMedia.Items[3].Selected = ds.Tables[0].Rows[0]["Media_HCR"].ToString() == "true" ? true : false;
                DDLDocumentMedia.Items[4].Selected = ds.Tables[0].Rows[0]["Media_SCR"].ToString() == "true" ? true : false;
                DDLDocumentMedia.Items[5].Selected = ds.Tables[0].Rows[0]["Media_NA"].ToString() == "true" ? true : false;
                txtFileRefNumber.Text = ds.Tables[0].Rows[0]["FileRef_Number"].ToString();
                txtremarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                bool DocExists = false;
                int DocumentCount = 0;
                btnSubmit.Enabled = false;
                string sDocumentPath = string.Empty;
                string sStructureUID = Request.QueryString["StructureUID"].ToString();
                string CoverPagePath = string.Empty;
                DateTime CDate1 = DateTime.Now, CDate2 = DateTime.Now, CDate3 = DateTime.Now, CDate4 = DateTime.Now, CDate5 = DateTime.Now, DocStartDate = DateTime.Now;

                DateTime startdate;
                DateTime endate;
                double TimeDuration = 0;
                

                if (Request.QueryString["GeneralDocumentUID"] != null)
                {
                    Guid GeneralDocumentUID = new Guid(Request.QueryString["GeneralDocumentUID"]);
                    if (dtIncomingRevDate.Text == "")
                    {
                        dtIncomingRevDate.Text = dtDocumentDate.Text;
                    }
                    string IncomingRec_Date_String = getdata.ConvertDateFormat(dtIncomingRevDate.Text);
                    DateTime IncomingRec_Date = Convert.ToDateTime(IncomingRec_Date_String);

                    string DocumentDate = getdata.ConvertDateFormat(dtDocumentDate.Text);
                    DateTime Document_Date = Convert.ToDateTime(DocumentDate);
                    int cnt = GD.UpdateIntoGeneralDocuments(GeneralDocumentUID, new Guid(sStructureUID), txtprefnumber.Text, IncomingRec_Date, txtDocumentName.Text, txtdesc.Text,
                        DDLDocumentMedia.Items[0].Selected == true ? "true" : "false", DDLDocumentMedia.Items[1].Selected == true ? "true" : "false", DDLDocumentMedia.Items[2].Selected == true ? "true" : "false", DDLDocumentMedia.Items[3].Selected == true ? "true" : "false",
                        DDLDocumentMedia.Items[4].Selected == true ? "true" : "false", DDLDocumentMedia.Items[5].Selected == true ? "true" : "false", txtremarks.Text, txtFileRefNumber.Text, Document_Date);
                    if (cnt > 0)
                    {
                        Session["GeneralDocumentStructureUID"] = sStructureUID;
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                    }
                }
                else
                {
                    if (!FileUploadDoc.HasFile && RBLUploadType.SelectedValue == "File")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('Please upload a document.');</script>");
                        return;
                    }

                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "Progress Bar", "ShowProgressBar('true')", true);

                    if (RBLUploadType.SelectedValue == "File")
                    {
                        DocumentCount = FileUploadDoc.PostedFiles.Count;
                        int cnt = 0;
                        foreach (HttpPostedFile uploadedFile in FileUploadDoc.PostedFiles)
                        {
                            if (System.IO.Path.GetExtension(uploadedFile.FileName).ToLower() != ".exe" && System.IO.Path.GetExtension(uploadedFile.FileName).ToLower() != ".db" && System.IO.Path.GetExtension(uploadedFile.FileName).ToLower() != ".msi")
                            {
                                if (!checkDocumentExists(Path.GetFileNameWithoutExtension(uploadedFile.FileName), sStructureUID)) // added on 20/11/2020
                                {
                                    startdate = DateTime.Now;
                                    sDocumentPath = "~/GD/" + sStructureUID;

                                    if (!Directory.Exists(Server.MapPath(sDocumentPath)))
                                    {
                                        Directory.CreateDirectory(Server.MapPath(sDocumentPath));

                                    }

                                    string sFileName = Path.GetFileNameWithoutExtension(uploadedFile.FileName);
                                    string Extn = System.IO.Path.GetExtension(uploadedFile.FileName);

                                    FileUploadDoc.SaveAs(Server.MapPath(sDocumentPath + "/" + sFileName + "_1_copy" + Extn));
                                    //FileUploadDoc.SaveAs(Server.MapPath("~/Documents/Encrypted/" + sDocumentUID + "_" + txtDocName.Text + "_1" + "_enp" + InputFile));
                                    string savedPath = sDocumentPath + "/" + sFileName + "_1_copy" + Extn;
                                    CoverPagePath = sDocumentPath + "/" + sFileName + "_1" + Extn;


                                    EncryptFile(Server.MapPath(savedPath), Server.MapPath(CoverPagePath));
                                    //
                                    endate = DateTime.Now;
                                    Guid GeneralDocumentUID = Guid.NewGuid();

                                    TimeDuration = (endate - startdate).TotalSeconds;

                                    // getdata.InsertIntoDocumentUploadLog(GeneralDocumentUID, startdate, endate, new Guid(Session["UserUID"].ToString()), (float)TimeDuration);
                                    //.................................
                                    string UploadFilePhysicalpath = Server.MapPath(CoverPagePath);

                                    if (dtIncomingRevDate.Text == "")
                                    {
                                        dtIncomingRevDate.Text = dtDocumentDate.Text;
                                    }
                                    string DocumentDate = getdata.ConvertDateFormat(dtDocumentDate.Text);
                                    DateTime Document_Date = Convert.ToDateTime(DocumentDate);

                                    string IncomingRec_Date_String = getdata.ConvertDateFormat(dtIncomingRevDate.Text);
                                    DateTime IncomingRec_Date = Convert.ToDateTime(IncomingRec_Date_String);
                                    cnt = GD.InsertIntoGeneralDocuments(GeneralDocumentUID, new Guid(sStructureUID), txtprefnumber.Text,
                                                                   IncomingRec_Date, sFileName, txtdesc.Text, 1, Extn,
                                                                  DDLDocumentMedia.Items[0].Selected == true ? "true" : "false", DDLDocumentMedia.Items[1].Selected == true ? "true" : "false", DDLDocumentMedia.Items[2].Selected == true ? "true" : "false", DDLDocumentMedia.Items[3].Selected == true ? "true" : "false",
                                                                  DDLDocumentMedia.Items[4].Selected == true ? "true" : "false", DDLDocumentMedia.Items[5].Selected == true ? "true" : "false", CoverPagePath, txtremarks.Text, txtFileRefNumber.Text, "Submitted", Document_Date,
                                                                  "", "", UploadFilePhysicalpath, new Guid(Session["UserUID"].ToString()));
                                    if (cnt == 0)
                                    {
                                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "Error", "<script language='javascript'>alert('Error Code : AGD 01 There is a problem with this feature. Please contact system admin.');</script>");
                                    }

                                    try
                                    {
                                        if (File.Exists(Server.MapPath(savedPath)))
                                        {
                                            File.Delete(Server.MapPath(savedPath));
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //throw
                                    }
                                }
                            }
                        }
                        btnSubmit.Enabled = true;
                        Session["GeneralDocumentStructureUID"] = sStructureUID;
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                    }
                    else
                    {
                        if (Request.Files.Count > 0)
                        {
                            DocumentCount = Request.Files.Count;
                            HttpFileCollection attachments = Request.Files;
                            //string ParentUID = string.Empty;
                            string FolderName = string.Empty;
                            List<FolderStructure> finallist = new List<FolderStructure>();
                            for (int i = 0; i < attachments.Count; i++)
                            {
                                startdate = DateTime.Now;
                                HttpPostedFile attachment = attachments[i];
                                if (System.IO.Path.GetExtension(Path.GetFileName(attachment.FileName)).ToLower() != ".exe" && System.IO.Path.GetExtension(Path.GetFileName(attachment.FileName)).ToLower() != ".db" && System.IO.Path.GetExtension(Path.GetFileName(attachment.FileName)).ToLower() != ".msi")
                                {
                                    if (!checkFileExists(attachment.FileName, sStructureUID))
                                    {
                                        if (attachment.ContentLength > 0 && !String.IsNullOrEmpty(attachment.FileName))
                                        {
                                            sDocumentPath = "~/GD/" + sStructureUID;

                                            if (!Directory.Exists(Server.MapPath(sDocumentPath)))
                                            {
                                                Directory.CreateDirectory(Server.MapPath(sDocumentPath));
                                            }

                                            string RelativePath = attachment.FileName;
                                            string[] Foldername = Path.GetDirectoryName(attachment.FileName).Split('\\');
                                            string sRes = Foldername[Foldername.Length - 1];
                                            string sFileName = Path.GetFileNameWithoutExtension(Path.GetFileName(attachment.FileName));
                                            string Extn = System.IO.Path.GetExtension(Path.GetFileName(attachment.FileName));
                                            if (i == 0)
                                            {
                                                HiddenVal.Value = sStructureUID;
                                            }
                                            else
                                            {
                                                string fol_Name = Foldername[Foldername.Length - 1];
                                                string pUID = finallist.Where(x => x.FolderName == fol_Name).Select(x => x.StructureID).FirstOrDefault();
                                                if (pUID != null)
                                                {
                                                    HiddenVal.Value = pUID;
                                                }
                                                else
                                                {
                                                    if (Foldername.Length == 2)
                                                    {
                                                        pUID = finallist.Where(x => x.FolderName == Foldername[0]).Select(x => x.StructureID).FirstOrDefault();
                                                        if (pUID != null)
                                                        {
                                                            HiddenVal.Value = pUID;
                                                        }
                                                    }
                                                    else if (Foldername.Length > 2)
                                                    {
                                                        pUID = finallist.Where(x => x.FolderName == Foldername[Foldername.Length - 2]).Select(x => x.StructureID).FirstOrDefault();
                                                        if (pUID != null)
                                                        {
                                                            HiddenVal.Value = pUID;
                                                        }
                                                    }
                                                }

                                            }
                                            //if (Foldername.Length > 1)
                                            //{
                                            //    ParentUID = HiddenVal.Value;
                                            //}
                                            //}
                                            //else
                                            //{
                                            //    ParentUID = sStructureUID;
                                            //}
                                            string NewStructureUID = GD.CheckGeneralDocumentRelativePathExists(new Guid(HiddenVal.Value), RelativePath);
                                            if (NewStructureUID == null || NewStructureUID == "")
                                            {
                                                Guid StructureUID = Guid.NewGuid();
                                                int structurecount = GD.GeneralDocumentStructure_InsertorUpdate(StructureUID, Foldername[Foldername.Length - 1], new Guid(HiddenVal.Value), new Guid(Session["UserUID"].ToString()));
                                                if (structurecount > 0)
                                                {
                                                    if (FolderName != Foldername[Foldername.Length - 1])
                                                    {
                                                        HiddenVal.Value = Convert.ToString(StructureUID);
                                                        FolderName = Foldername[Foldername.Length - 1];

                                                        FolderStructure fs = new FolderStructure();
                                                        fs.StructureID = Convert.ToString(StructureUID);
                                                        fs.FolderName = Foldername[Foldername.Length - 1];
                                                        finallist.Add(fs);
                                                    }
                                                    //else
                                                    //{
                                                    //    FolderName = Foldername[Foldername.Length - 1];
                                                    //}
                                                }
                                            }
                                            else
                                            {
                                                HiddenVal.Value = NewStructureUID;
                                            }
                                            //..............................................................................................
                                            //string ssss = string.Empty;
                                            //if (Foldername.Length > 1)
                                            //{
                                            //    DataSet ds = GD.GetGeneralDocumentStructure_By_StructureUID(new Guid(HiddenVal.Value));
                                            //    if (ds.Tables[0].Rows.Count > 0)
                                            //    {
                                            //        //if (ds.Tables[0].Rows[0]["Structure_Name"].ToString() != Foldername[Foldername.Length - 1])
                                            //        if(HiddenField1.Value != Foldername.Length.ToString() && Foldername.Length < Convert.ToInt32(HiddenField1.Value))
                                            //        {
                                            //            ssss = HiddenVal.Value;
                                            //        }
                                            //        else if(HiddenField1.Value != Foldername.Length.ToString())
                                            //        {
                                            //            HiddenField1.Value = Foldername.Length.ToString();
                                            //        }
                                            //        else
                                            //        {
                                            //            ssss = ds.Tables[0].Rows[0]["ParentStructureUID"].ToString();
                                            //        }
                                            //    }
                                            //}
                                            //else
                                            //{
                                            //    ssss = sStructureUID;
                                            //}
                                            //string NewStructureUID = GD.CheckGeneralDocumentStructure_NameExist_For_StructureUID(new Guid(ssss), Foldername[Foldername.Length - 1]);
                                            //if (NewStructureUID == null)
                                            //{
                                            //    Guid StructureUID = Guid.NewGuid();
                                            //    int structurecount = GD.GeneralDocumentStructure_InsertorUpdate(StructureUID, Foldername[Foldername.Length - 1], new Guid(ssss), new Guid(Session["UserUID"].ToString()));
                                            //    if (structurecount > 0)
                                            //    {
                                            //        HiddenVal.Value = Convert.ToString(StructureUID);
                                            //        HiddenField1.Value = Foldername.Length.ToString();
                                            //    }
                                            //}
                                            //else
                                            //{
                                            //    HiddenVal.Value = NewStructureUID;
                                            //}


                                            //..............................................................................................
                                            string savedPath = string.Empty;
                                            if (checkDocumentExists(sFileName, sStructureUID))
                                            {
                                                attachment.SaveAs(Server.MapPath(sDocumentPath + "/" + sFileName + "_1_copy" + Extn));
                                                savedPath = sDocumentPath + "/" + sFileName + "_1_copy" + Extn;
                                                CoverPagePath = sDocumentPath + "/" + sFileName + "_" + DateTime.Now.Ticks + Extn;

                                            }
                                            else
                                            {
                                                attachment.SaveAs(Server.MapPath(sDocumentPath + "/" + sFileName + "_1_copy" + Extn));
                                                savedPath = sDocumentPath + "/" + sFileName + "_1_copy" + Extn;
                                                CoverPagePath = sDocumentPath + "/" + sFileName + "_1" + Extn;
                                            }
                                            EncryptFile(Server.MapPath(savedPath), Server.MapPath(CoverPagePath));
                                            endate = DateTime.Now;
                                            TimeDuration = (endate - startdate).TotalSeconds;
                                            Guid GeneralDocumentUID = Guid.NewGuid();
                                            getdata.InsertIntoDocumentUploadLog(GeneralDocumentUID, startdate, endate, new Guid(Session["UserUID"].ToString()), (float)TimeDuration);

                                            string UploadFilePhysicalpath = Server.MapPath(CoverPagePath);

                                            if (dtIncomingRevDate.Text == "")
                                            {
                                                dtIncomingRevDate.Text = dtDocumentDate.Text;
                                            }
                                            string DocumentDate = getdata.ConvertDateFormat(dtDocumentDate.Text);
                                            DateTime Document_Date = Convert.ToDateTime(DocumentDate);

                                            string IncomingRec_Date_String = getdata.ConvertDateFormat(dtIncomingRevDate.Text);
                                            DateTime IncomingRec_Date = Convert.ToDateTime(IncomingRec_Date_String);

                                            int cnt = GD.InsertIntoGeneralDocuments(GeneralDocumentUID, new Guid(HiddenVal.Value), txtprefnumber.Text,
                                                                   IncomingRec_Date, sFileName, txtdesc.Text, 1, Extn,
                                                                  DDLDocumentMedia.Items[0].Selected == true ? "true" : "false", DDLDocumentMedia.Items[1].Selected == true ? "true" : "false", DDLDocumentMedia.Items[2].Selected == true ? "true" : "false", DDLDocumentMedia.Items[3].Selected == true ? "true" : "false",
                                                                  DDLDocumentMedia.Items[4].Selected == true ? "true" : "false", DDLDocumentMedia.Items[5].Selected == true ? "true" : "false", CoverPagePath, txtremarks.Text, txtFileRefNumber.Text, "Submitted", Document_Date,
                                                                  RelativePath, Foldername[Foldername.Length - 1], UploadFilePhysicalpath, new Guid(Session["UserUID"].ToString()));
                                            if (cnt > 0)
                                            {

                                            }

                                            try
                                            {
                                                if (File.Exists(Server.MapPath(savedPath)))
                                                {
                                                    File.Delete(Server.MapPath(savedPath));
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                //throw
                                            }
                                        }
                                    }
                                }
                            }

                            btnSubmit.Enabled = true;
                            Session["GeneralDocumentStructureUID"] = sStructureUID;
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                        }
                    }
                }
                    
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Progress Bar", "ShowProgressBar('false')", true);
                btnSubmit.Enabled = true;
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Error", "<script language='javascript'>alert('" + ex.Message + "');</script>");
                //throw ex;
            }
        }


        private bool checkFileExists(string filename, string StructuerUID)
        {
            bool result = false;
            DataSet dsfiles = new DataSet();
            try
            {
                dsfiles = GD.GeneralDocuments_SelectBy_StructureUID(new Guid(StructuerUID));
                foreach (DataRow dr in dsfiles.Tables[0].Rows)
                {
                    if (dr["GeneralDocument_RelativePath"].ToString() == filename)
                    {
                        result = true;
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
        //private string CheckRelativePathExists(string RelativePath,string StructuerUID)
        private bool checkDocumentExists(string filename, string StructuerUID)
        {
            bool result = false;
            DataSet dsfiles = new DataSet();
            try
            {
                dsfiles = GD.GeneralDocuments_SelectBy_StructureUID(new Guid(StructuerUID));
                foreach (DataRow dr in dsfiles.Tables[0].Rows)
                {
                    if (dr["GeneralDocument_Name"].ToString() == filename)
                    {
                        result = true;
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        private void EncryptFile(string inputFile, string outputFile)
        {

            try
            {
                string password = @"myKey123"; // Your Key Here
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                string cryptFile = outputFile;
                FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateEncryptor(key, key),
                    CryptoStreamMode.Write);

                FileStream fsIn = new FileStream(inputFile, FileMode.Open);

                int data;
                while ((data = fsIn.ReadByte()) != -1)
                    cs.WriteByte((byte)data);


                fsIn.Close();
                cs.Close();
                fsCrypt.Close();
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                //MessageBox.Show("Encryption failed!", "Error");
            }
        }
        protected void RBLUploadType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RBLUploadType.SelectedValue == "Folder")
            {
                UploadFolder.Visible = true;
                UploadFile.Visible = false;
            }
            else if (RBLUploadType.SelectedValue == "File")
            {
                UploadFolder.Visible = false;
                UploadFile.Visible = true;
            }
        }
    }
}
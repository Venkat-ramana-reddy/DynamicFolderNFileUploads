using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using DynamicFolderNFileUploads.Models;

namespace DynamicFolderNFileUploads.Controllers
{
    public class FileUploadController : Controller
    {
        PersonFileContext db = new PersonFileContext();
        
        public ActionResult Index(int? Id)
        {
            PersonFile person = new PersonFile();

            var idFile = db.PersonFiles.ToList();
            ViewBag.Names = new SelectList(idFile, "FileID", "Name");
            
            var files = db.PersonFiles.FirstOrDefault(p => p.FileID == Id);

            return View();
        }

        public ActionResult Helper(int? Id)
        {
            var idFile = db.PersonFiles.ToList();
            ViewBag.Names = new SelectList(idFile, "FileID", "Name");
            
            var files = db.PersonFiles.FirstOrDefault(p => p.FileID == Id);

            List<SelectListItem> DropDownFiles = new List<SelectListItem>()
            {
            new SelectListItem{ Text= "Adhar",Value = files.AdharCardPDF},
            new SelectListItem{ Text= "Pan", Value = files.PanCard },
            new SelectListItem{ Text= "Voter",Value = files.VoterCard } };

            ViewBag.Files = new SelectList(DropDownFiles,"Value","Text");

            return View("Index",files);
        }

        public ActionResult EachFile(string fileName,int id)
        {

            var fi = db.PersonFiles.SingleOrDefault(p => p.FileID == id);

            if (fileName != null)
            {
                if (fileName.EndsWith(".jpg"))
                {
                    if (fi.AdharCardPDF == fileName)
                    {
                        ViewBag.Filimg = fi.AdharCardPDF;
                    }
                    else if (fi.PanCard == fileName)
                    {
                        ViewBag.Filimg = fi.PanCard;
                    }
                    else if (fi.VoterCard == fileName)
                    {
                        ViewBag.Filimg = fi.VoterCard;
                    }
                }

                else if(fileName.EndsWith(".pdf"))
                {
                    if (fi.AdharCardPDF == fileName)
                    {
                        ViewBag.Filpdf = fi.AdharCardPDF;
                    }
                    else if (fi.PanCard == fileName)
                    {
                        ViewBag.Filpdf = fi.PanCard;
                    }
                    else if (fi.VoterCard == fileName)
                    {
                        ViewBag.Filpdf = fi.VoterCard;
                    }
                }
            }
    
            List<SelectListItem> DropDownFiles = new List<SelectListItem>()
            {
            new SelectListItem{ Text= "Adhar",Value = fi.AdharCardPDF},
            new SelectListItem{ Text= "Pan", Value = fi.PanCard },
            new SelectListItem{ Text= "Voter",Value = fi.VoterCard } };

            ViewBag.Files = new SelectList(DropDownFiles, "Value", "Text");

            var idFile = db.PersonFiles.ToList();
            ViewBag.Names = new SelectList(idFile, "FileID", "Name");

            return View("Index",fi);
        }


        public ActionResult ShowData(int ?Id)
        {
            var files = db.PersonFiles.FirstOrDefault(p => p.FileID == Id);
                 
            return PartialView("_data",files);
        }

        public ActionResult ShowFile(int? Id)
        {
            var files = db.PersonFiles.FirstOrDefault(p => p.FileID == Id);

            if(files == null)
            {
                ViewBag.message = "Please select a Name from Drop Down.";
            }
            
            return View(files);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(PersonFile File)
        {
            string fileName = File.Name;

            string path = string.Format("~\\images\\{0}\\", File.Name);
            string Dpath = Server.MapPath(path);
            if(!Directory.Exists(Dpath))
            {
                Directory.CreateDirectory(Dpath);
            }
            string adharfile = Path.GetFileName(File.Adhar.FileName);
            string panfile = Path.GetFileName(File.Pan.FileName);
            string voterfile = Path.GetFileName(File.Voter.FileName);

            File.Adhar.SaveAs(Dpath+adharfile);
            File.Pan.SaveAs(Dpath+ panfile);
            File.Voter.SaveAs(Dpath+ voterfile);

            if(File !=null)
            {
                var data = File;
                data.Name = File.Name;
                data.AdharCardPDF = fileName+"/"+adharfile;
                data.PanCard = fileName + "/" + panfile;
                data.VoterCard = fileName + "/" + voterfile;
                db.PersonFiles.Add(data);
                db.SaveChanges();
            }

            ModelState.Clear();

            return View();
        }      
        
    }
}
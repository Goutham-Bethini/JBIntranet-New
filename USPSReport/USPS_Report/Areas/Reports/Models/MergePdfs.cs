using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
namespace USPS_Report.Areas.Reports.Models
{
    public class MergePdfs
    {       
        public static void MergeFiles(string destinationFile, List<string> sourceFiles, int? num) // string[] sourceFiles
        {
            
           

            if (System.IO.File.Exists(destinationFile)) 
                System.IO.File.Delete(destinationFile);

            int no = num ??3;
            string[] sSrcFile;
            sSrcFile = new string[no];

            string[] arr = new string[no];
            for (int i = 0; i <= sourceFiles.Count() - 1; i++)
            {
                if (sourceFiles[i] != null)
                {
                    if (sourceFiles[i].Trim() != "")
                        arr[i] = sourceFiles[i].ToString();
                }
            }

            if (arr != null)
            {
                sSrcFile = new string[no];

                for (int ic = 0; ic <= arr.Length - 1; ic++)
                {
                    sSrcFile[ic] = arr[ic].ToString();
                }
            }
            try
            {
                int f = 0;

                PdfReader reader = new PdfReader(sSrcFile[f]);
                int n = reader.NumberOfPages;
                 string response = "There are " + n + " pages in the original file.";
                Document document = new Document(PageSize.A4);

                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(destinationFile, FileMode.Create));

                document.Open();
                PdfContentByte cb = writer.DirectContent;
                PdfImportedPage page;

                int rotation;
                while (f < sSrcFile.Length)
                {
                    int i = 0;
                    while (i < n)
                    {
                        i++;

                        document.SetPageSize(PageSize.A4);
                        document.NewPage();
                        page = writer.GetImportedPage(reader, i);

                        rotation = reader.GetPageRotation(i);
                        if (rotation == 90 || rotation == 270)
                        {
                            cb.AddTemplate(page, 0, -1f, 1f, 0, 0, reader.GetPageSizeWithRotation(i).Height);
                        }
                        else
                        {
                            cb.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);
                        }
                        response = "Processed page " + i;
                    }

                    f++;
                    if (f < sSrcFile.Length)
                    {
                        reader = new PdfReader(sSrcFile[f]);
                        n = reader.NumberOfPages;
                        response = "There are " + n + " pages in the original file.";
                    }
                }
                response = "Success";
                document.Close();
            }
            catch (Exception e)
            {
             string   response = e.Message;
            }


        }


        public static void MergeFilesInbetween(string destinationFile, List<string> sourceFiles, int? num) // string[] sourceFiles
        {




            if (System.IO.File.Exists(destinationFile))
                System.IO.File.Delete(destinationFile);

            int no = num ?? 3;
            string[] sSrcFile;
            sSrcFile = new string[no];

            string[] arr = new string[no];

            //getting all the source file in to aray of strings
            for (int i = 0; i <= sourceFiles.Count() - 1; i++)
            {
                if (sourceFiles[i] != null)
                {
                    if (sourceFiles[i].Trim() != "")
                        arr[i] = sourceFiles[i].ToString();
                }
            }

            if (arr != null)
            {
                sSrcFile = new string[no];

                for (int ic = 0; ic <= arr.Length - 1; ic++)
                {
                    sSrcFile[ic] = arr[ic].ToString();
                }
            }
            try
            {
                int f = 0;
                int j = 1;

                PdfReader reader = new PdfReader(sSrcFile[f]);
                int n = reader.NumberOfPages;
                string response = "There are " + n + " pages in the original file.";
                Document document = new Document(PageSize.A4);

                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(destinationFile, FileMode.Create));

                document.Open();
                PdfContentByte cb = writer.DirectContent;
                PdfImportedPage page;

                int rotation;
                while (f < sSrcFile.Length)
                {
                    int i = 0;
                    
                    while (i < n)
                    {
                        i++;
 
                         

                        document.SetPageSize(PageSize.A4);
                        document.NewPage();

                        page = writer.GetImportedPage(reader, i);

                        rotation = reader.GetPageRotation(i);
                        if (rotation == 90 || rotation == 270)
                        {
                            cb.AddTemplate(page, 0, -1f, 1f, 0, 0, reader.GetPageSizeWithRotation(i).Height);
                        }
                        else
                        {
                            cb.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);
                        }
                        response = "Processed page " + i;
                            if (j == 1 && i == 2)
                        { i = n; }
                      
                    }
                    j++;
                    f++;
                    if (f < sSrcFile.Length)
                    {
                        reader = new PdfReader(sSrcFile[f]);
                        n = reader.NumberOfPages;
                        response = "There are " + n + " pages in the original file.";
                    }
                }

                if (1 == 1)
                {
                    reader = new PdfReader(sSrcFile[0]);
                   
                    int i = 2;

                        while (i < 4)
                        {
                            i++;
                         

                            document.SetPageSize(PageSize.A4);
                            document.NewPage();

                            page = writer.GetImportedPage(reader, i);

                            rotation = reader.GetPageRotation(i);
                            if (rotation == 90 || rotation == 270)
                            {
                                cb.AddTemplate(page, 0, -1f, 1f, 0, 0, reader.GetPageSizeWithRotation(i).Height);
                            }
                            else
                            {
                                cb.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);
                            }
                            response = "Processed page " + i;
                            

                        }
                       
                      
                    
                }
                response = "Success";
                document.Close();
            }
            catch (Exception e)
            {
                string response = e.Message;
            }


        }

      


        public static void DeleteFiles(string file ) 
        {

            if (System.IO.File.Exists(file))
            {
                // Use a try block to catch IOExceptions, to
                // handle the case of the file already being
                // opened by another process.
                try
                {
                    System.IO.File.Delete(file);
                }
                catch (System.IO.IOException e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }
            }
        }
    }

 
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XHX;
using System.IO;
using XHX.Common;

namespace XHX.View
{
    public partial class AllPictureShow2 : DevExpress.XtraEditors.XtraForm
    {
        localhost.Service service = new XHX.localhost.Service();

        public AllPictureShow2()
        {
            InitializeComponent();

            this.Shown += new EventHandler(AllPictureShow2_Shown);
        }

        void AllPictureShow2_Shown(object sender, EventArgs e)
        {
            this.kpImageViewer1.FitToScreen();
        }

        public AllPictureShow2(string filePath, string[] fileName, string shopName, string subjectCode,string type, string code)
            : this()
        {
            this.LookAndFeel.SetSkinStyle(CommonHandler.Skin_Name);
            List<Image> pictures = new List<Image>();

            for (int i = 0; i < fileName.Length; i++)
            {
                byte[] bytes = null;
                if (type == "SpecialCase" || type == "Notice")
                {
                    bytes = service.SearchAnswerDtl2Pic(fileName[i], shopName, subjectCode, type, code);
                }
                else
                {
                    bytes = SearchAnswerDtl2Pic(fileName[i].Replace(".jpg", ""), shopName, subjectCode, type, code);
                }
                if (bytes != null && bytes.Length != 0)
                {
                    MemoryStream ms = new MemoryStream(bytes);
                    Image image = Image.FromStream(ms);
                    pictures.Add(image);
                }
            }

            if (pictures.Count != 0)
            {
                this.kpImageViewer1.ImageList = pictures;
            }
            else
            {
                for (int i = 0; i < fileName.Length; i++)
                {
                    if (type != "SpecialCase")
                    {
                        if (File.Exists(filePath + fileName[i] + ".jpg"))
                        {
                            Image image = Image.FromFile(filePath + fileName[i] + ".jpg");
                            pictures.Add(image);
                        }
                    }
                    else
                    {
                        if (File.Exists(filePath + fileName[i]))
                        {

                            Image image = Image.FromFile(filePath + fileName[i]);
                            pictures.Add(image);
                        }
                    }
                }
                this.kpImageViewer1.ImageList = pictures;
            }
        }

        public byte[] SearchAnswerDtl2Pic(string picName, string shopName, string subjectCode, string type, string code)
        {
            string appDomainPath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = "";
            if (type == "SpecialCase")
            {
                filePath = appDomainPath + @"UploadImage\" + @"SpecialCasePictures\" + code + @"\" + picName;
            }
            else if (type == "Notice")
            {
                filePath = appDomainPath + @"UploadImage\" + @"NoticeAttachment\" + code + @"\" + picName;
            }
            else
            {
                if (File.Exists(appDomainPath + @"UploadImage\" + shopName + @"\" + subjectCode + @"\" + picName + ".jpg"))
                {
                    filePath = appDomainPath + @"UploadImage\" + shopName + @"\" + subjectCode + @"\" + picName + ".jpg";
                }
                if (File.Exists(appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".jpg"))
                {
                    filePath = appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".jpg";
                }
                if (File.Exists(appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".doc"))
                {
                    filePath = appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".doc";
                }
                if (File.Exists(appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".docx"))
                {
                    filePath = appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".docx";
                }
                if (File.Exists(appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".xls"))
                {
                    filePath = appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".xls";
                }
                if (File.Exists(appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".xlsx"))
                {
                    filePath = appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".xlsx";
                }
                if (File.Exists(appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".ppt"))
                {
                    filePath = appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".ppt";
                }
                if (File.Exists(appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".pptx"))
                {
                    filePath = appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".pptx";
                }
            }
            //if (!File.Exists(filePath))
            //{
            if (!Directory.Exists(appDomainPath + @"UploadImage\"))
            {
                Directory.CreateDirectory(appDomainPath + @"UploadImage\");
            }
            if (!Directory.Exists(appDomainPath + @"UploadImage\" + @"\" + shopName))
            {
                Directory.CreateDirectory(appDomainPath + @"UploadImage\" + @"\" + shopName);
            }
            if (!Directory.Exists(appDomainPath + @"UploadImage\" + @"\" + shopName + @"\" + subjectCode))
            {
                Directory.CreateDirectory(appDomainPath + @"UploadImage\" + @"\" + shopName + @"\" + subjectCode);
            }

            try
            {
                UploadFileToAliyun aliyun = new UploadFileToAliyun();
                aliyun.GetObject("yrtech", "GACFCA" + @"/" + shopName + @"/" + subjectCode + @"/" + picName.Replace(".jpg", "") + ".jpg",
                               appDomainPath + @"UploadImage\" + shopName + @"\" + subjectCode + @"\" + picName.Replace(".jpg", "") + ".jpg");
                filePath = appDomainPath + @"UploadImage\" + shopName + @"\" + subjectCode + @"\" + picName.Replace(".jpg", "") + ".jpg";
            }
            catch (Aliyun.OpenServices.OpenStorageService.OssException ex)
            {

            }

            //}
            if (File.Exists(filePath))
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    byte[] b = new byte[fs.Length];
                    fs.Read(b, 0, b.Length);
                    fs.Close();
                    return b;
                }
            }
            else
            {
                return null;
            }

        } 
    }
}

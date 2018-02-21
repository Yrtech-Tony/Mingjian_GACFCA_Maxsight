﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XHX.Common;
using XHX.DTO;
using Microsoft.Office.Interop.Excel;

namespace XHX.View
{
    public partial class Subjects : BaseForm
    {
        localhost.Service webService = new localhost.Service();
        XtraGridDataHandler<ProjectDto> dataHandler = null;
        XtraGridDataHandler<ChapterDto> dataHandlerChapter = null;
        XtraGridDataHandler<SubjectDto> dataHandlerSubject = null;
        XtraGridDataHandler<LinkDto> dataHandlerLink = null;
        MSExcelUtil msExcelUtil = new MSExcelUtil();
        public int MAXSEQNO = 0;
        ProjectDto project = null;
        GridCheckMarksSelection selection;
        internal GridCheckMarksSelection Selection
        {
            get
            {
                return selection;
            }
        }

        GridCheckMarksSelection selectionCharter;
        internal GridCheckMarksSelection SelectionCharter
        {
            get
            {
                return selectionCharter;
            }
        }

        GridCheckMarksSelection selectionLink;
        internal GridCheckMarksSelection SelectionLink
        {
            get
            {
                return selectionLink;
            }
        }
        public Subjects()
        {
            InitializeComponent();
            
            dataHandler = new XtraGridDataHandler<ProjectDto>(grvProject);
            dataHandlerChapter = new XtraGridDataHandler<ChapterDto>(grvCharter);
            dataHandlerSubject = new XtraGridDataHandler<SubjectDto>(grvSubject);
            dataHandlerLink = new XtraGridDataHandler<LinkDto>(grvLink);
            CommonHandler.SetRowNumberIndicator(grvProject);
            CommonHandler.SetRowNumberIndicator(grvCharter);
            CommonHandler.SetRowNumberIndicator(grvLink);
            CommonHandler.SetRowNumberIndicator(grvSubject);
            CommonHandler.SetButtonImage(btnInspectionStandard, ButtonImageType.POPUP);
            CommonHandler.SetButtonImage(btnFile, ButtonImageType.POPUP);
            CommonHandler.SetButtonImage(btnLoss, ButtonImageType.POPUP);
            CommonHandler.SetButtonImage(btnSroceSet, ButtonImageType.POPUP);
            CommonHandler.SetButtonImage(btnDetail, ButtonImageType.DETAIL);
            CommonHandler.SetButtonImage(btnLinkDetail, ButtonImageType.DETAIL);
            grcSubject.DataSource = new List<SubjectDto>();
            selection = new GridCheckMarksSelection(grvSubject);
            selection.CheckMarkColumn.VisibleIndex = 0;

            grcCharter.DataSource = new List<ChapterDto>();
            selectionCharter = new GridCheckMarksSelection(grvCharter);
            selectionCharter.CheckMarkColumn.VisibleIndex = 0;

            grcLink.DataSource = new List<LinkDto>();
            selectionLink = new GridCheckMarksSelection(grvLink);
            selectionLink.CheckMarkColumn.VisibleIndex = 0;

            btnCharterSave.Enabled = false;
            btnCharterAddRow.Enabled = false; ;
            btnAddRow_Down.Enabled = false;
            btnDelRow.Enabled = false;
            btnSave_Down.Enabled = false;

            btnLinkSave.Enabled = false;
            btnLinkAddRow.Enabled = false;
            this.textBox1.Visible = false;
            this.btnAddNewData.Visible = false;
            InitData();
        }
        private void InitData()
        {
            BindComBox.BindProject(cboProject);
            BindComBox.BindProject(cboProjectForSubject);
            initCboSubjectType();
            initCboExamType();
        }
        private void initCboSubjectType()
        {
            DataSet ds = webService.GetSubjectTypeForCbo();
            List<SubjectTypeDto> list = new List<SubjectTypeDto>();
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    SubjectTypeDto subjectType = new SubjectTypeDto();
                    subjectType.SubjectTypeCode = Convert.ToString(ds.Tables[0].Rows[i]["SubjectTypeCode"]);
                    subjectType.SubjectTypeName = Convert.ToString(ds.Tables[0].Rows[i]["SubjectTypeName"]);
                    list.Add(subjectType);
                }
            }
            CommonHandler.BindComboBoxItems<SubjectTypeDto>(cboSubjectType, list, "SubjectTypeName", "SubjectTypeCode");
        }
        private void initCboExamType()
        {
            List<ExamTypeDto> list1 = new List<ExamTypeDto>();
            ExamTypeDto examType1 = new ExamTypeDto();
            ExamTypeDto examType2 = new ExamTypeDto();
            ExamTypeDto examType3 = new ExamTypeDto();
            ExamTypeDto examType4 = new ExamTypeDto();
            ExamTypeDto examType5 = new ExamTypeDto();
            ExamTypeDto examType6 = new ExamTypeDto();
            ExamTypeDto examType7 = new ExamTypeDto();
            examType1.ExamTypeCode = "";
            examType1.ExamTypeName = "全部";
            list1.Add(examType1);
           
            examType2.ExamTypeCode = "A";
            examType2.ExamTypeName = "A卷";
            list1.Add(examType2);
           
            examType3.ExamTypeCode = "B";
            examType3.ExamTypeName = "B卷";
            list1.Add(examType3);
            
            examType4.ExamTypeCode = "E";
            examType4.ExamTypeName = "E卷";
            list1.Add(examType4);
           
            examType5.ExamTypeCode = "D";
            examType5.ExamTypeName = "D卷";
            list1.Add(examType5);

            examType6.ExamTypeCode = "F";
            examType6.ExamTypeName = "F卷";
            list1.Add(examType6);

            examType7.ExamTypeCode = "G";
            examType7.ExamTypeName = "G卷";
            list1.Add(examType7);

            CommonHandler.SetComboBoxEditItems(cboExamType, list1, "ExamTypeName", "ExamTypeCode");

            List<ExamTypeDto> list2 = new List<ExamTypeDto>();
            ExamTypeDto gridexamType1 = new ExamTypeDto();
            ExamTypeDto gridexamType2 = new ExamTypeDto();
            ExamTypeDto gridexamType3 = new ExamTypeDto();
            ExamTypeDto gridexamType4 = new ExamTypeDto();
            ExamTypeDto gridexamType5 = new ExamTypeDto();
            ExamTypeDto gridexamType6 = new ExamTypeDto();
            ExamTypeDto gridexamType7 = new ExamTypeDto();
            gridexamType1.ExamTypeCode = "C";
            gridexamType1.ExamTypeName = "普通";
            list2.Add(gridexamType1);
            gridexamType2.ExamTypeCode = "A";
            gridexamType2.ExamTypeName = "A卷";
            list2.Add(gridexamType2);
            gridexamType3.ExamTypeCode = "B";
            gridexamType3.ExamTypeName = "B卷";
            list2.Add(gridexamType3);
            gridexamType4.ExamTypeCode = "E";
            gridexamType4.ExamTypeName = "E卷";
            list2.Add(gridexamType4);
            gridexamType5.ExamTypeCode = "D";
            gridexamType5.ExamTypeName = "D卷";
            list2.Add(gridexamType5);
            gridexamType6.ExamTypeCode = "F";
            gridexamType6.ExamTypeName = "F卷";
            list2.Add(gridexamType6);
            gridexamType7.ExamTypeCode = "G";
            gridexamType7.ExamTypeName = "G卷";
            list2.Add(gridexamType7);


            CommonHandler.BindComboBoxItems<ExamTypeDto>(cboGridExamType, list2, "ExamTypeName", "ExamTypeCode");
        }
        private void SearchProject()
        {
            grcProject.DataSource = null;
            grcSubject.DataSource = null;
            List<ProjectDto> projectList = new List<ProjectDto>();
            DataSet ds = webService.SearchProject();
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ProjectDto project = new ProjectDto();
                    project.ProjectCode = Convert.ToString(ds.Tables[0].Rows[i]["ProjectCode"]);
                    project.ProjectName = Convert.ToString(ds.Tables[0].Rows[i]["ProjectName"]);
                    project.Year = Convert.ToString(ds.Tables[0].Rows[i]["Year"]);
                    project.Quarter = Convert.ToString(ds.Tables[0].Rows[i]["Quarter"]);
                    project.OrderNO = Convert.ToInt32(ds.Tables[0].Rows[i]["OrderNO"]);
                    projectList.Add(project);
                }
                grcProject.DataSource = projectList;
            }
            else
            {
                CommonHandler.ShowMessage(MessageType.Information, "没有数据");
                return;
            }
        }

        private void SearchChapter()
        {
            project = grvProject.GetRow(grvProject.FocusedRowHandle) as ProjectDto;
            List<ChapterDto> chapterList = new List<ChapterDto>();
            DataSet ds = webService.SearchChapter(project.ProjectCode, "");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ChapterDto chapter = new ChapterDto();
                    chapter.ProjectCode = Convert.ToString(ds.Tables[0].Rows[i]["ProjectCode"]);
                    chapter.CharterCode = Convert.ToString(ds.Tables[0].Rows[i]["CharterCode"]);
                    chapter.CharterName = Convert.ToString(ds.Tables[0].Rows[i]["CharterName"]);
                    chapter.CharterContent = Convert.ToString(ds.Tables[0].Rows[i]["CharterContent"]);
                    chapter.OrderNo = Convert.ToInt32(ds.Tables[0].Rows[i]["OrderNo"]);
                    chapter.InUserID = Convert.ToString(ds.Tables[0].Rows[i]["InUserID"]);
                    chapter.InDateTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["InDateTime"]);
                    chapter.Weight = Convert.ToDecimal(ds.Tables[0].Rows[i]["Weight"]);
                    chapterList.Add(chapter);
                }
                grcCharter.DataSource = chapterList;
            }
            else
            {
                CommonHandler.ShowMessage(MessageType.Information, "没有数据");
            }
        }
        private void SearchLink()
        {
            string projectCode = CommonHandler.GetComboBoxSelectedValue(cboProject).ToString();
            string chapterCode = CommonHandler.GetComboBoxSelectedValue(cboChapter).ToString();
            List<LinkDto> linkList = new List<LinkDto>();
            DataSet ds = webService.SearchLink(projectCode, chapterCode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    LinkDto link = new LinkDto();
                    link.CharterCode = Convert.ToString(ds.Tables[0].Rows[i]["CharterCode"]);
                    link.CharterName = Convert.ToString(ds.Tables[0].Rows[i]["CharterName"]);
                    link.LinkCode = Convert.ToString(ds.Tables[0].Rows[i]["LinkCode"]);
                    link.LinkName = Convert.ToString(ds.Tables[0].Rows[i]["LinkName"]);
                    link.OrderNO = Convert.ToInt32(ds.Tables[0].Rows[i]["OrderNO"]);
                    link.LinkContent = Convert.ToString(ds.Tables[0].Rows[i]["LinkContent"]);
                    link.InDateTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["InDateTime"]);
                    link.InUserID = Convert.ToString(ds.Tables[0].Rows[i]["InUserID"]);

                    linkList.Add(link);
                }
                grcLink.DataSource = linkList;
            }
            else
            {
                CommonHandler.ShowMessage(MessageType.Information, "没有数据");
                grcLink.DataSource = null;
            }
        }
        private void SearchSubject()
        {
            string projectCode = CommonHandler.GetComboBoxSelectedValue(cboProjectForSubject).ToString();
            string chapterCode = CommonHandler.GetComboBoxSelectedValue(cboChapterForSubject).ToString();
            string linkCode = CommonHandler.GetComboBoxSelectedValue(cboLink).ToString();
            string examTypeCode = CommonHandler.GetComboBoxSelectedValue(cboExamType).ToString();
            List<SubjectDto> subjectList = new List<SubjectDto>();
            DataSet ds = webService.SearchSubject(projectCode, chapterCode, linkCode, examTypeCode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    SubjectDto subject = new SubjectDto();
                    subject.SubjectCode = Convert.ToString(ds.Tables[0].Rows[i]["SubjectCode"]);
                    subject.Implementation = Convert.ToString(ds.Tables[0].Rows[i]["Implementation"]);
                    subject.InspectionDesc = Convert.ToString(ds.Tables[0].Rows[i]["InspectionDesc"]);
                    subject.Desc = Convert.ToString(ds.Tables[0].Rows[i]["Desc"]);
                    subject.InspectionNeedFile = Convert.ToString(ds.Tables[0].Rows[i]["InspectionNeedFile"]);
                    subject.AdditionalDesc = Convert.ToString(ds.Tables[0].Rows[i]["AdditionalDesc"]);
                    subject.CheckPoint = Convert.ToString(ds.Tables[0].Rows[i]["CheckPoint"]);
                    subject.DelChk = Convert.ToBoolean(ds.Tables[0].Rows[i]["DelChk"]);
                    subject.ProjectCode = Convert.ToString(ds.Tables[0].Rows[i]["ProjectCode"]);
                    subject.OrderNO = Convert.ToInt32(ds.Tables[0].Rows[i]["OrderNO"]);
                    subject.Remark = Convert.ToString(ds.Tables[0].Rows[i]["Remark"]);
                    subject.ScoreCheck = Convert.ToBoolean(ds.Tables[0].Rows[i]["ScoreCheck"]);
                    subject.LinkCode = Convert.ToString(ds.Tables[0].Rows[i]["LinkCode"]);
                    if (ds.Tables[0].Rows[i]["FullScore"] != DBNull.Value)
                    {
                        subject.FullScore = Convert.ToDecimal(ds.Tables[0].Rows[i]["FullScore"]);
                    }
                    else
                    {
                        subject.FullScore = 0;
                    }
                    subject.SubjectTypeCode = Convert.ToString(ds.Tables[0].Rows[i]["SubjectTypeCode"]);
                    subject.SubjectTypeCodeExam = Convert.ToString(ds.Tables[0].Rows[i]["SubjectTypeCodeExam"]);
                    if (ds.Tables[0].Rows[i]["SubjectDelChk"] != DBNull.Value)
                    {
                        subject.SubjectDelChk = Convert.ToBoolean(ds.Tables[0].Rows[i]["SubjectDelChk"]);
                    }
                    if (ds.Tables[0].Rows[i]["AddErrorChk"] != DBNull.Value)
                    {
                        subject.AddErrorChk = Convert.ToBoolean(ds.Tables[0].Rows[i]["AddErrorChk"]);
                    }
                    
                    subjectList.Add(subject);
                }
                grcSubject.DataSource = subjectList;
            }
            else
            {
                CommonHandler.ShowMessage(MessageType.Information, "没有数据");
                grcSubject.DataSource = null;
            }
        }
        private void grvProject_DoubleClick(object sender, EventArgs e)
        {
            grcCharter.DataSource = null;
            SearchChapter();
            if (base.UserInfoDto.RoleType != "C")
            {
                btnCharterSave.Enabled = true;
                btnCharterAddRow.Enabled = true;
                btnAddRow_Down.Enabled = true;
                btnDelRow.Enabled = true;
                btnSave_Down.Enabled = true;
            }
            else
            {
                btnCharterSave.Enabled = false;
                btnCharterAddRow.Enabled = false;
                btnAddRow_Down.Enabled = false;
                btnDelRow.Enabled = false;
                btnSave_Down.Enabled = false;
            }
            ProjectDto projectDouble = grvProject.GetFocusedRow() as ProjectDto;
            CommonHandler.SetComboBoxSelectedValue(cboProject, projectDouble.ProjectCode);
            CommonHandler.SetComboBoxSelectedValue(cboProjectForSubject, projectDouble.ProjectCode);

        }

        private void btnAddRow_Down_Click(object sender, EventArgs e)
        {
            if (CommonHandler.GetComboBoxSelectedValue(cboLink) == null || string.IsNullOrEmpty(CommonHandler.GetComboBoxSelectedValue(cboLink).ToString()))
            {
                CommonHandler.ShowMessage(MessageType.Information, "请先选择环节");
                return;
            }
            SubjectDto subject = new SubjectDto();
            subject.LinkCode = CommonHandler.GetComboBoxSelectedValue(cboLink).ToString();
            subject.ProjectCode = CommonHandler.GetComboBoxSelectedValue(cboProject).ToString();
            subject.FullScore = 2;
            subject.ScoreCheck = true;
            if (cboExamType.SelectedIndex == 0)
            {
                subject.SubjectTypeCodeExam = "C";
            }
            else
            {
                subject.SubjectTypeCodeExam = CommonHandler.GetComboBoxSelectedValue(cboExamType).ToString();
            }

            dataHandlerSubject.AddRow(subject);
        }

        private void btnSave_Down_Click(object sender, EventArgs e)
        {
            if (base.UserInfoDto.RoleType != "S")
            {
                CommonHandler.ShowMessage(MessageType.Information, "没有权限");
            }
            foreach (SubjectDto sub in grcSubject.DataSource as List<SubjectDto>)
            {
                if (string.IsNullOrEmpty(sub.SubjectCode))
                {
                    CommonHandler.ShowMessage(MessageType.Information, "体系定位号不能为空");
                    grvSubject.FocusedColumn = gcSubjectCode;
                    grvSubject.FocusedRowHandle = (grcSubject.DataSource as List<SubjectDto>).IndexOf(sub);
                    return;
                }
                foreach (SubjectDto subj in dataHandlerSubject.DataList)
                {
                    if (sub != subj)
                    {
                        if (sub.SubjectCode == subj.SubjectCode && subj.StatusType != 'D')
                        {
                            CommonHandler.ShowMessage(MessageType.Information, "体系定位号重复");
                            grvSubject.FocusedColumn = gcSubjectCode;
                            grvSubject.FocusedRowHandle = (grcSubject.DataSource as List<SubjectDto>).IndexOf(sub);
                            return;
                        }
                        if (sub.OrderNO == subj.OrderNO && subj.StatusType != 'D')
                        {
                            // CommonHandler.ShowMessage(MessageType.Information, "序号重复");
                            grvSubject.FocusedColumn = gridColumn9;
                            grvSubject.FocusedRowHandle = (grcSubject.DataSource as List<SubjectDto>).IndexOf(sub);
                            return;
                        }
                    }
                }

            }
            foreach (SubjectDto sub in dataHandlerSubject.DataList)
            {
                if (sub.OrderNO == 0)
                {
                    CommonHandler.ShowMessage(MessageType.Information, "序号不能为0");
                    grvSubject.FocusedColumn = gridColumn9;
                    grvSubject.FocusedRowHandle = (grcSubject.DataSource as List<SubjectDto>).IndexOf(sub);
                    return;
                }

            }
            if (CommonHandler.ShowMessage(MessageType.Confirm, "确定要保存吗？") == DialogResult.Yes)
            {
                List<SubjectDto> subjectList = dataHandlerSubject.DataList;
                foreach (SubjectDto subject in subjectList)
                {
                    if (subject.Weight == null)
                    {
                        subject.Weight = 0;
                    }
                    webService.SaveSubject(subject.StatusType, subject.ProjectCode, subject.SubjectCode, subject.Implementation,
                                           subject.CheckPoint, subject.Desc, subject.AdditionalDesc, subject.InspectionDesc,
                                           subject.InspectionNeedFile, subject.Remark, subject.OrderNO, subject.LinkCode, subject.FullScore,
                                           subject.ScoreCheck, subject.SubjectTypeCode, subject.SubjectTypeCodeExam, subject.SubjectDelChk, subject.AddErrorChk,subject.Weight);
                }
            }
            SearchSubject();
            CommonHandler.ShowMessage(MessageType.Information, "保存完毕");
        }

        private void grvProject_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column == gcCode)
            {
                e.Appearance.BackColor = Color.Gray;
            }
        }

        private void btnInspectionStandard_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {

            SubjectDto subject = grvSubject.GetRow(grvSubject.FocusedRowHandle) as SubjectDto;
            DataSet ds = webService.CheckSubjectExists(subject.ProjectCode, subject.SubjectCode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                InspectionStandard ins = new InspectionStandard(subject.ProjectCode, subject.SubjectCode, base.UserInfoDto.RoleType);
                ins.ShowDialog();
            }
            else
            {
                CommonHandler.ShowMessage(MessageType.Information, "请先保存");
            }
        }

        private void btnDelRow_Click(object sender, EventArgs e)
        {
            List<SubjectDto> subjectList = new List<SubjectDto>();
            for (int i = 0; i < grvSubject.RowCount; i++)
            {

                object dd = grvSubject.GetRowCellValue(i, grvSubject.Columns[10]);
                if (dd != null)
                {

                    if (grvSubject.GetRowCellValue(i, grvSubject.Columns[10]).ToString() == "True")
                    {
                        subjectList.Add(grvSubject.GetRow(i) as SubjectDto);
                    }
                }
            }
            foreach (SubjectDto subject in subjectList)
            {
                DataSet dsInstand = webService.SearchInspectionStandardByProjectCodeAndSubjectCode(subject.ProjectCode, subject.SubjectCode);
                if (dsInstand.Tables[0].Rows.Count > 0)
                {
                    CommonHandler.ShowMessage(MessageType.Information, "请先删除检查标准");
                    return;
                }
                DataSet dsFile = webService.SearchFileAndPictureByProjectCodeAndSubjectCode(subject.ProjectCode, subject.SubjectCode);
                if (dsFile.Tables[0].Rows.Count > 0)
                {
                    CommonHandler.ShowMessage(MessageType.Information, "请先删除文件名称/拍照点");
                    return;
                }
            }
            dataHandlerSubject.DelCheckedRow(grvSubject.Columns.ColumnByFieldName("CheckMarkSelection"));
            selection.ClearSelection();
        }

        private void grvSubject_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (grcSubject.DataSource == null) return;
            SubjectDto s = grvSubject.GetRow(grvSubject.FocusedRowHandle) as SubjectDto;
            if (grvSubject.FocusedColumn == gcSubjectCode && s.StatusType != 'I')
            {
                e.Cancel = true;
            }
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            SubjectDto subject = grvSubject.GetRow(grvSubject.FocusedRowHandle) as SubjectDto;
            DataSet ds = webService.CheckSubjectExists(subject.ProjectCode, subject.SubjectCode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                SubjectFile ins = new SubjectFile(subject.ProjectCode, subject.SubjectCode, base.UserInfoDto.RoleType);
                ins.ShowDialog();
            }
            else
            {
                CommonHandler.ShowMessage(MessageType.Information, "请先保存");
            }
        }

        private void btnCharterAddRow_Click(object sender, EventArgs e)
        {
            ChapterDto chapter = new ChapterDto();
            chapter.ProjectCode = project.ProjectCode;
            chapter.Weight = Convert.ToDecimal(0.00);
            dataHandlerChapter.AddRow(chapter);
        }

        private void btnChartAddRow_Click(object sender, EventArgs e)
        {
            dataHandlerSubject.DelCheckedRow(grvSubject.Columns.ColumnByFieldName("CheckMarkSelection"));
            selection.ClearSelection();
        }

        private void btnLinkSearch_Click(object sender, EventArgs e)
        {
            SearchLink();
            if (base.UserInfoDto.RoleType != "C")
            {
                btnLinkSave.Enabled = true;
                btnLinkAddRow.Enabled = true;

            }
            else
            {
                btnLinkSave.Enabled = false;
                btnLinkAddRow.Enabled = true;
            }
        }

        private void cboProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CommonHandler.GetComboBoxSelectedValue(cboProject) != null && !string.IsNullOrEmpty(CommonHandler.GetComboBoxSelectedValue(cboProject).ToString()))
            {
                BindComBox.BindChapter(CommonHandler.GetComboBoxSelectedValue(cboProject).ToString(), cboChapter);
            }
            else
            {
                BindComBox.BindChapter(CommonHandler.GetComboBoxSelectedValue(cboProject).ToString(), cboChapter);
                CommonHandler.SetComboBoxSelectedValue(cboChapter, "");
            }
        }

        private void btnLinkAddRow_Click(object sender, EventArgs e)
        {
            LinkDto link = new LinkDto();
            link.CharterCode = CommonHandler.GetComboBoxSelectedValue(cboChapter).ToString();
            link.CharterName = cboChapter.Text;
            dataHandlerLink.AddRow(link);
        }

        private void btnLinkSave_Click(object sender, EventArgs e)
        {
            if (base.UserInfoDto.RoleType != "S")
            {
                CommonHandler.ShowMessage(MessageType.Information, "没有权限");
            }
            foreach (LinkDto link in grcLink.DataSource as List<LinkDto>)
            {
                if (string.IsNullOrEmpty(link.LinkCode))
                {
                    CommonHandler.ShowMessage(MessageType.Information, "环节号码不能为空");
                    grvLink.FocusedColumn = gcLinkCode;
                    grvLink.FocusedRowHandle = (grcLink.DataSource as List<LinkDto>).IndexOf(link);
                    return;
                }
                foreach (LinkDto dlink in dataHandlerLink.DataList)
                {
                    if (link != dlink)
                    {
                        if (link.LinkCode == dlink.LinkCode && dlink.StatusType != 'D')
                        {
                            CommonHandler.ShowMessage(MessageType.Information, "环节号码重复");
                            grvLink.FocusedColumn = gcLinkCode;
                            grvLink.FocusedRowHandle = (grcLink.DataSource as List<LinkDto>).IndexOf(link);
                            return;
                        }
                    }
                }
            }
            if (CommonHandler.ShowMessage(MessageType.Confirm, "确定要保存吗？") == DialogResult.Yes)
            {
                List<LinkDto> linkList = dataHandlerLink.DataList;
                foreach (LinkDto link in linkList)
                {
                    webService.SaveLink(CommonHandler.GetComboBoxSelectedValue(cboProject).ToString(),
                    link.CharterCode, link.LinkCode, link.LinkName, link.LinkContent, link.InUserID,link.StatusType);
                }
            }
            SearchLink();
            CommonHandler.ShowMessage(MessageType.Information, "保存完毕");
            BindComBox.BindProject(cboProjectForSubject);
        }

        private void grvCharter_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            ChapterDto chapter = grvCharter.GetRow(e.RowHandle) as ChapterDto;
            if ((e.Column == gcCharterCodeChapter && chapter.StatusType != 'I') || e.Column == gcProjectCodeChapter)
            {
                e.Appearance.BackColor = Color.Gray;
            }
        }

        private void grvCharter_ShowingEditor(object sender, CancelEventArgs e)
        {
            ChapterDto chapter = grvCharter.GetFocusedRow() as ChapterDto;
            if (grvCharter.FocusedColumn == gcCharterCodeChapter && chapter.StatusType != 'I')
            {
                e.Cancel = true;
            }

        }

        private void grvLink_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            LinkDto link = grvLink.GetRow(e.RowHandle) as LinkDto;
            if (e.Column == gbChapterCode || e.Column == gbChapterName || (e.Column == gcLinkCode && link.StatusType != 'I'))
            {
                e.Appearance.BackColor = Color.Gray;
            }
        }

        private void grvLink_ShowingEditor(object sender, CancelEventArgs e)
        {
            LinkDto link = grvLink.GetFocusedRow() as LinkDto;
            if (grvLink.FocusedColumn == gbChapterCode || grvLink.FocusedColumn == gbChapterName
                || (grvLink.FocusedColumn == gcLinkCode && link.StatusType != 'I'))
            {
                e.Cancel = true;
            }
        }

        private void cboProjectForSubject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CommonHandler.GetComboBoxSelectedValue(cboProjectForSubject) != null &&
                !string.IsNullOrEmpty(CommonHandler.GetComboBoxSelectedValue(cboProjectForSubject).ToString()))
            {
                BindComBox.BindChapter(CommonHandler.GetComboBoxSelectedValue(cboProjectForSubject).ToString(), cboChapterForSubject);
                BindComBox.BindChapter(CommonHandler.GetComboBoxSelectedValue(cboProject).ToString(), cboChapter);
            }
            else
            {
                BindComBox.BindChapter(CommonHandler.GetComboBoxSelectedValue(cboProjectForSubject).ToString(), cboChapterForSubject);
                CommonHandler.SetComboBoxSelectedValue(cboChapterForSubject, "");
            }
        }

        private void cboChapterForSubject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CommonHandler.GetComboBoxSelectedValue(cboChapterForSubject) != null
                && !string.IsNullOrEmpty(CommonHandler.GetComboBoxSelectedValue(cboChapterForSubject).ToString()))
            {
                BindComBox.BindLink(CommonHandler.GetComboBoxSelectedValue(cboProjectForSubject).ToString(),
                   CommonHandler.GetComboBoxSelectedValue(cboChapterForSubject).ToString(), cboLink);
            }
            else
            {
                BindComBox.BindLink(CommonHandler.GetComboBoxSelectedValue(cboProjectForSubject).ToString(),
                      CommonHandler.GetComboBoxSelectedValue(cboChapterForSubject).ToString(), cboLink);
                CommonHandler.SetComboBoxSelectedValue(cboLink, "");
            }
        }

        private void btnSearchSubject_Click(object sender, EventArgs e)
        {
            SearchSubject();
            if (base.UserInfoDto.RoleType != "C")
            {
                btnAddRow_Down.Enabled = true;
                btnDelRow.Enabled = true;
                btnSave_Down.Enabled = true;
            }
            else
            {
                btnAddRow_Down.Enabled = false;
                btnDelRow.Enabled = false;
                btnSave_Down.Enabled = false;
            }
        }

        private void btnDetail_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;
            ChapterDto chapter = grvCharter.GetFocusedRow() as ChapterDto;

            CommonHandler.SetComboBoxSelectedValue(cboProject, chapter.ProjectCode);
            CommonHandler.SetComboBoxSelectedValue(cboChapter, chapter.CharterCode);
            SearchLink();
        }

        private void btnLinkDetail_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            tabControl1.SelectedTab = tabPage3;
            LinkDto lin = grvLink.GetFocusedRow() as LinkDto;
            CommonHandler.SetComboBoxSelectedValue(cboProjectForSubject, CommonHandler.GetComboBoxSelectedValue(cboProject).ToString());
            CommonHandler.SetComboBoxSelectedValue(cboChapterForSubject, lin.CharterCode);
            CommonHandler.SetComboBoxSelectedValue(cboLink, lin.LinkCode);
            SearchSubject();
        }

        private void btnLoss_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            SubjectDto subject = grvSubject.GetRow(grvSubject.FocusedRowHandle) as SubjectDto;
            //string sql = string.Format("EXEC up_XHX_CheckSubjectExists_R '{0}','{1}'", subject.ProjectCode, subject.SubjectCode);
            DataSet ds = webService.CheckSubjectExists(subject.ProjectCode, subject.SubjectCode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                LossResultForm ins = new LossResultForm(subject.ProjectCode, subject.SubjectCode, base.UserInfoDto.RoleType);
                ins.ShowDialog();
            }
            else
            {
                CommonHandler.ShowMessage(MessageType.Information, "请先保存");
            }
        }

        private void btnSroceSet_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            SubjectDto subject = grvSubject.GetRow(grvSubject.FocusedRowHandle) as SubjectDto;
            //string sql = string.Format("EXEC up_XHX_CheckSubjectExists_R '{0}','{1}'", subject.ProjectCode, subject.SubjectCode);
            DataSet ds = webService.CheckSubjectExists(subject.ProjectCode, subject.SubjectCode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ScoreSet ss = new ScoreSet(subject.ProjectCode, subject.SubjectCode, UserInfoDto.UserID);
                ss.ShowDialog();
            }
            else
            {
                CommonHandler.ShowMessage(MessageType.Information, "请先保存");
            }
        }
        public override void SearchButtonClick()
        {
            SearchProject();
            if (base.UserInfoDto.RoleType != "C")
            {
                this.CSParentForm.EnabelButton(ButtonType.AddRowButton, true);
                this.CSParentForm.EnabelButton(ButtonType.SaveButton, true);

            }
            else
            {
                this.CSParentForm.EnabelButton(ButtonType.AddRowButton, false);
                this.CSParentForm.EnabelButton(ButtonType.SaveButton, false);
            }
            if (base.UserInfoDto.RoleType == "S")
            {
                btnAddNewData.Visible = true;
                textBox1.Visible = true;
                textBox2.Visible = true;
                label2.Visible = true;
                label1.Visible = true;

            }
            else
            {
                btnAddNewData.Visible = false;
                textBox1.Visible = false;
                textBox2.Visible = false;
                label2.Visible = false;
                label1.Visible = false;
            }
        }
        public override void AddRowButtonClick()
        {
            ProjectDto project = new ProjectDto();
            dataHandler.AddRow(project);
        }
        public override void SaveButtonClick()
        {
            grvProject.CloseEditor();
            grvProject.UpdateCurrentRow();
            grvProject.RefreshData();
            if (base.UserInfoDto.RoleType != "S")
            {
                CommonHandler.ShowMessage(MessageType.Information, "没有权限");
            }
            if (CommonHandler.ShowMessage(MessageType.Confirm, "确定要保存吗？") == DialogResult.Yes)
            {
                List<ProjectDto> projectList = dataHandler.DataList;
                foreach (ProjectDto project in projectList)
                {
                    webService.SaveProject(project.StatusType, project.ProjectCode, project.Year, project.Quarter, project.OrderNO);
                }
            }
            SearchProject();
            CommonHandler.ShowMessage(MessageType.Information, "保存完毕");
            BindComBox.BindProject(cboProjectForSubject);
            BindComBox.BindProject(cboProject);
        }
        public override List<ButtonType> CreateButton()
        {
            List<ButtonType> list = new List<ButtonType>();
            //list.Add(ButtonType.InitButton);
            list.Add(ButtonType.SearchButton);
            list.Add(ButtonType.AddRowButton);
            list.Add(ButtonType.SaveButton);
            return list;
        }

        private void Subjects_Load(object sender, EventArgs e)
        {
            this.CSParentForm.EnabelButton(ButtonType.AddRowButton, false);
            this.CSParentForm.EnabelButton(ButtonType.SaveButton, false);
        }

        private void btnCharterSave_Click(object sender, EventArgs e)
        {
            if (base.UserInfoDto.RoleType != "S")
            {
                CommonHandler.ShowMessage(MessageType.Information, "没有权限");
                return;
            }
            foreach (ChapterDto chapter in grcCharter.DataSource as List<ChapterDto>)
            {
                if (string.IsNullOrEmpty(chapter.CharterCode))
                {
                    CommonHandler.ShowMessage(MessageType.Information, "章节代码不能为空");
                    grvCharter.FocusedColumn = gcCharterCodeChapter;
                    grvCharter.FocusedRowHandle = (grcCharter.DataSource as List<ChapterDto>).IndexOf(chapter);
                    return;
                }
                foreach (ChapterDto dchapter in dataHandlerChapter.DataList)
                {
                    if (chapter != dchapter)
                    {
                        if (chapter.CharterCode == dchapter.CharterCode && dchapter.StatusType != 'D')
                        {
                            CommonHandler.ShowMessage(MessageType.Information, "章节代码重复");
                            grvCharter.FocusedColumn = gcCharterCodeChapter;
                            grvCharter.FocusedRowHandle = (grcCharter.DataSource as List<ChapterDto>).IndexOf(chapter);
                            return;
                        }
                    }
                }
            }
            foreach (ChapterDto dchapter in dataHandlerChapter.DataList)
            {
                if (dchapter.Weight == Convert.ToDecimal(0.00))
                {
                    CommonHandler.ShowMessage(MessageType.Information, "权重不能为0");
                    grvCharter.FocusedColumn = gcWeight;
                    grvCharter.FocusedRowHandle = (grcCharter.DataSource as List<ChapterDto>).IndexOf(dchapter);
                    return;
                }
            }

            if (CommonHandler.ShowMessage(MessageType.Confirm, "确定要保存吗？") == DialogResult.Yes)
            {
                List<ChapterDto> chapterlist = dataHandlerChapter.DataList;
                foreach (ChapterDto chapter in chapterlist)
                {

                    webService.SaveChapter(chapter.ProjectCode, chapter.CharterCode, chapter.CharterName, chapter.CharterContent, chapter.OrderNo, chapter.InUserID, chapter.Weight,chapter.StatusType);
                }
            }
            SearchChapter();
            CommonHandler.ShowMessage(MessageType.Information, "保存完毕");
            BindComBox.BindProject(cboProject);
            BindComBox.BindProject(cboProjectForSubject);
        }

        private void btnAddNewData_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    CommonHandler.ShowMessage(MessageType.Information,"请输入期号");
                    return;
                }
                webService.CopyLastData(textBox2.Text.Replace(" ", "").Trim(), textBox1.Text.Replace(" ", "").Trim());
                CommonHandler.ShowMessage(MessageType.Information, "复制完成");
            }
            catch (Exception ex)
            {
                CommonHandler.ShowMessage(ex);
            }
        }

        private void btnCharterDelRow_Click(object sender, EventArgs e)
        {
            List<ChapterDto> subjectList = new List<ChapterDto>();
            for (int i = 0; i < grvCharter.RowCount; i++)
            {

                object dd = grvCharter.GetRowCellValue(i, grvCharter.Columns[6]);
                if (dd != null)
                {

                    if (grvCharter.GetRowCellValue(i, grvCharter.Columns[6]).ToString() == "True")
                    {
                        subjectList.Add(grvCharter.GetRow(i) as ChapterDto);
                    }
                }
            }
            foreach (ChapterDto subject in subjectList)
            {
                DataSet dsInstand = webService.SearchLink(subject.ProjectCode, subject.CharterCode);
                if (dsInstand.Tables[0].Rows.Count > 0)
                {
                    CommonHandler.ShowMessage(MessageType.Information, "请先删除环节");
                    return;
                }
            }
            dataHandlerChapter.DelCheckedRow(grvCharter.Columns.ColumnByFieldName("CheckMarkSelection"));
            selectionCharter.ClearSelection();
        }

        private void btnLinkDelRow_Click(object sender, EventArgs e)
        {
            List<LinkDto> subjectList = new List<LinkDto>();
            for (int i = 0; i < grvLink.RowCount; i++)
            {

                object dd = grvLink.GetRowCellValue(i, grvLink.Columns[6]);
                if (dd != null)
                {

                    if (grvLink.GetRowCellValue(i, grvLink.Columns[6]).ToString() == "True")
                    {
                        subjectList.Add(grvLink.GetRow(i) as LinkDto);
                    }
                }
            }
            foreach (LinkDto subject in subjectList)
            {
                DataSet dsInstand = webService.SearchSubject(CommonHandler.GetComboBoxSelectedValue(cboProject).ToString(), subject.CharterCode,subject.LinkCode,"");
                if (dsInstand.Tables[0].Rows.Count > 0)
                {
                    CommonHandler.ShowMessage(MessageType.Information, "请先删除执行体系");
                    return;
                }
            }
            dataHandlerLink.DelCheckedRow(grvLink.Columns.ColumnByFieldName("CheckMarkSelection"));
            selectionLink.ClearSelection();
        }

        private void btnModule_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog ofp = new OpenFileDialog();
            ofp.Filter = "Excel(*.xlsx)|";
            ofp.FilterIndex = 2;
            if (ofp.ShowDialog() == DialogResult.OK)
            {
                btnModule.Text = ofp.FileName;
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(btnModule.Text);
            Worksheet worksheet_FengMian = workbook.Worksheets["Link"] as Worksheet;
            Worksheet worksheet_Subjects = workbook.Worksheets["Subjects"] as Worksheet;

            for (int i = 2; i < 146; i++)
            {
                string subjectCode = msExcelUtil.GetCellValue(worksheet_Subjects, "A", i);
                if (!string.IsNullOrEmpty(subjectCode))
                {
                    string linkCode = msExcelUtil.GetCellValue(worksheet_Subjects, "B", i);
                    string checkItem = msExcelUtil.GetCellValue(worksheet_Subjects, "C", i);
                    string standard = msExcelUtil.GetCellValue(worksheet_Subjects, "D", i);
                    decimal weight = Convert.ToDecimal(msExcelUtil.GetCellValue(worksheet_Subjects, "E", i));
                    int orderNo = Convert.ToInt32(msExcelUtil.GetCellValue(worksheet_Subjects, "F", i));
                    webService.Area_SubjectSave("201701", "0", checkItem, standard, orderNo, weight, linkCode, "sysadmin", subjectCode);
                }
            }
            CommonHandler.ShowMessage(MessageType.Information, "上传完毕");
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (grcSubject.DataSource != null)
                //CommonHandler.ExcelExport(grvShopScore);
                CommonHandler.ExcelExportByExporter(grvSubject);
        }

        

    }
}

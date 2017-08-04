﻿using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using MangaRipper.Core.Providers;
using NLog;
using System.Collections.Generic;
using MangaRipper.Core.Models;

namespace MangaRipper.Presenters
{
    public class MainViewPresenter
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private IMainView View { get; set; }
        public MainViewPresenter(IMainView view)
        {
            View = view;
            View.FindChaptersClicked = OnFindChapters;
            View.FindTitlesClicked = OnFindTitles;
        }

        private async Task OnFindTitles(string obj)
        {
            var worker = FrameworkProvider.GetWorker();
            var titles = await worker.FindTitlesListAsync(obj);
            var Sites = new List<Site>();
            Sites.Add(new Site("MangaHere", titles));
            View.SetTitles(Sites);
        }
        private async Task OnFindChapters(string obj)
        {
            try
            {
                var worker = FrameworkProvider.GetWorker();
                var progressInt = new Progress<int>(progress => View.SetChaptersProgress(progress + @"%"));
                var chapters = await worker.FindChapterListAsync(obj, progressInt);
                View.SetChapters(chapters);
            }
            catch (OperationCanceledException ex)
            {
                View.SetStatusText(@"Download cancelled! Reason: " + ex.Message);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                View.SetStatusText(@"Download cancelled! Reason: " + ex.Message);
                View.ShowMessageBox(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
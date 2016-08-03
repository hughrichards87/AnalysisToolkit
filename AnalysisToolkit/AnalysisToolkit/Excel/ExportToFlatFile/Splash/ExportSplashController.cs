using System;
using System.Threading;


namespace AnalysisToolkit.Excel.ExportToFlatFile.Splash
{
    class ExportSplashController 
    {
        private Thread _thread;
        private Splash _userControl;
        private Microsoft.Office.Interop.Excel.Application application;
        private bool _cancelPressed = false;
        private int dispose = 15;
        private int seconds = 0;

        public ExportSplashController(Microsoft.Office.Interop.Excel.Application application)
        {
            this.application = application;
            _thread = new Thread(delegate ()
            {
                _userControl = new Splash();
                _userControl.Show();
                _userControl.OnCancel -= _userControl_OnCancel;
                _userControl.OnCancel += _userControl_OnCancel;
                System.Windows.Threading.Dispatcher.Run();
            });

            _cancelPressed = false;
            _thread.SetApartmentState(ApartmentState.STA); // needs to be STA or throws exception
            _thread.Start();
        }

        private void _userControl_OnCancel(object source, EventArgs e)
        {
            _cancelPressed = true;
            Finish();
        }

        internal void Finish()
        {

            System.Timers.Timer timer = new System.Timers.Timer(1000);
            timer.Elapsed -= Timer_Elapsed;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            if (_userControl != null)
            {
                _userControl.progressBar1.Dispatcher.Invoke(new UpdateProgressCallback(updateProgress), new object[] { 100 });
            }
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_userControl != null)
            {
                seconds += 1;
                _userControl.progressBar1.Dispatcher.Invoke(new UpdateProgressCallback(updateProgress), new object[] { 100 });
                _userControl.button.Dispatcher.Invoke(new UpdateButtonCallback(updateButton), new object[] { "Close" });
                string val = string.Format("Export " + (_cancelPressed ? "Cancelled" : "Complete") + ". This window will automatically close in {0} second{1}s", dispose - seconds, (dispose - seconds) > 1 ? "s": "");
                _userControl.label.Dispatcher.Invoke(new UpdatetextCallback(updateText), new object[] { val });

                if (seconds == dispose)
                {
                    _userControl.Dispatcher.Invoke(new CloseCallback(close));
                   
                }
            }
        }

        private delegate void CloseCallback();
        private void close()
        {
            _userControl.Close();
            _userControl = null;
        }

        private delegate void ShowCallback();
        private void show()
        {
            _userControl.Show();
        }

        private delegate void UpdateButtonCallback(string progress);
        private delegate void UpdateProgressCallback(int progress);
        private delegate void UpdatetextCallback(string progress);

        private void updateProgress(int progress)
        {
            _userControl.progressBar1.Value = progress;
        }

        private void updateButton(string contents)
        {
            _userControl.button.Content = contents;
        }

        private void updateText(string progress)
        {
            _userControl.label.Content = progress;
        }

        internal bool Update(double i, double total)
        {
            if (_userControl != null && _cancelPressed == false)
            {
                int percent = (int)((i / total) * 100);

                _userControl.progressBar1.Dispatcher.Invoke(new UpdateProgressCallback(updateProgress), new object[] { (percent) });
                _userControl.label.Dispatcher.Invoke(new UpdatetextCallback(updateText), new object[] { String.Format("Exporting {0} out of {1} lines", i, total) });
            }

            return  _cancelPressed;
        }
    }
}

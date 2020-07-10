using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SeminarCTDLGT.Windows
{
    /// <summary>
    /// Interaction logic for ViewCode.xaml
    /// </summary>
    public partial class ViewCode : Window
    {
        private readonly string _path;

        public ViewCode(string title, string path)
        {
            InitializeComponent();
            _path = path;
            TextBlockTitle.Text = title ?? "View Code";
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            LoadCode();
        }

        private void LoadCode()
        {
            if (!File.Exists(_path))
            {
                var popup = new PopupMessage("File does not exist", "Please check your path and try again!");
                popup.ShowDialog();
                return;
            }
            TextBoxCode.Text = File.ReadAllText(_path);
        }

        private void ButtonCopyCode_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(_path))
            {
                var popup = new PopupMessage("File does not exist", "Please check your path and try again!");
                popup.ShowDialog();
                return;
            }

            Clipboard.SetData(DataFormats.Text, File.ReadAllText(_path));

            SnackbarInfo.IsActive = true;
            var task = new Task(() =>
            {
                Thread.Sleep(3000);
                Dispatcher.Invoke(() =>
                {
                    SnackbarInfo.IsActive = false;
                });
            });
            task.Start();
        }

        private void ButtonMinimizeWindow_OnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Normal ? WindowState.Minimized : WindowState.Normal;
        }

        private void ButtonToggleWindowState_OnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
        }

        private void ButtonCloseWindow_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ColorZone_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}

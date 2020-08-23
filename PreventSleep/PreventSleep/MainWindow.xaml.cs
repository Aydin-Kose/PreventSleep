using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Button = System.Windows.Controls.Button;
using ContextMenu = System.Windows.Controls.ContextMenu;
using MenuItem = System.Windows.Forms.MenuItem;

namespace PreventSleep
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _isActive = false;
        private System.Windows.Forms.NotifyIcon nIcon = new System.Windows.Forms.NotifyIcon();
        private string ctx_Activate = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
            PrepareVisualize();
        }
        private void PrepareVisualize()
        {
            nIcon.DoubleClick += NIcon_DoubleClick;
            //ContextMenu ctx = new ContextMenu();
            //ctx.Items.Add(new MenuItem("Activate!"));
            //ctx.Items.Add(new Separator());
            //ctx.Items.Add(new MenuItem("Close"));
            System.Windows.Forms.ContextMenu ctx = new System.Windows.Forms.ContextMenu();
            ctx.MenuItems.Add(new MenuItem("Deactivate!", ctx_Activate_OnClick));
            ctx.MenuItems.Add("-");
            ctx.MenuItems.Add(new MenuItem("Close",ctx_Close_OnClick));
            nIcon.ContextMenu = ctx;
        }

        private void ctx_Close_OnClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ctx_Activate_OnClick(object sender, EventArgs e)
        {
            if (_isActive)
            {
                ((MenuItem) sender).Text = "Activate!";
            }
            else
            {
                ((MenuItem)sender).Text = "Deactivate!";
            }
            Btn_Activate_OnClick(this.Btn_Activate, null);
        }


        private void Btn_Activate_OnClick(object sender, RoutedEventArgs e)
        {
            if (_isActive)
            {
                DeactivatePreventSleep();
                ((Button) sender).Content = "Activate!";
            }
            else
            {
                ActivatePreventSleep();
                ((Button)sender).Content = "Deactivate!";
            }
        }

        private void ActivatePreventSleep()
        {
            nIcon.Visible = true;
            SetIcons(Properties.Resources.sun, "Prevent Sleep Active!");
            SleepController.Activate();
            _isActive = true;
            Hide();
            nIcon.ShowBalloonTip(1000, "PreventSleep", "Prevent Sleep Active!", System.Windows.Forms.ToolTipIcon.Info);
        }
        private void DeactivatePreventSleep()
        {
            SetIcons(Properties.Resources.sleep, "Prevent Sleep Deactive!");
            SleepController.Deactivate();
            _isActive = false;
        }


        private void SetIcons(System.Drawing.Icon icon, string tipText)
        {
            ImageSource imageSource =
                Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            this.Icon = imageSource;
            nIcon.Icon = icon;
            nIcon.ShowBalloonTip(1000, "PreventSleep", tipText, System.Windows.Forms.ToolTipIcon.Info);
        }
        private void NIcon_DoubleClick(object sender, EventArgs e)
        {
            nIcon.Visible = false;
            this.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            nIcon.Visible = false;
            SleepController.Deactivate();
        }
    }
}

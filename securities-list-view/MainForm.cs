using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace securities_list_view
{
    public partial class MainForm : Form
    {
        public MainForm() => InitializeComponent();
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            for (int i = 1; i <= 3000; i++)
            {
                SecuritiesList.Add(new Security { Text = $"Security {i}", IsChecked = false });
            }
            m_lsvSecurities.View = View.Details;
            m_lsvSecurities.HeaderStyle = ColumnHeaderStyle.None;
            m_lsvSecurities.Columns.Add("", 80);
            m_lsvSecurities.Columns.Add("Security", 300); // Text column with a width of 200 pixels
            m_lsvSecurities.CheckBoxes = true;
            m_lsvSecurities.VirtualMode = true;
            m_lsvSecurities.RetrieveVirtualItem += (sender, e) =>
            {
                if (e.ItemIndex >= 0 && e.ItemIndex < SecuritiesList.Count)
                {
                    var security = SecuritiesList[e.ItemIndex];
                    ListViewItem item = new ListViewItem(text: security.IsChecked ? "\u2612" : "\u2610");
                    item.SubItems.Add(security.Text);
                    e.Item = item;
                }
                else
                {
                    e.Item = new ListViewItem();
                }
            };
            m_lsvSecurities.MouseClick += (sender, e) =>
            {
                var hti = m_lsvSecurities.HitTest(e.Location);
                if (hti.Location == ListViewHitTestLocations.Label)
                {
                    var security = SecuritiesList[hti.Item.Index];
                    security.IsChecked = !security.IsChecked;
                    m_lsvSecurities.Invalidate(hti.Item.Bounds);
                }
            };
            m_lsvSecurities.VirtualListSize = SecuritiesList.Count;
            _ = benchmark();
        }

        private void setListOfItemsConditionalChecked()
        {
            for (int i = 0; i < SecuritiesList.Count; i++) 
            {
                var mod = i + 1;
                SecuritiesList[i].IsChecked = (mod % 4 == 0);
            }
        }

        /// <summary>
        ///  Show the dialog, wait 2 seconds, then 
        ///  run conditional set check for 3000 items.
        /// </summary>
        async Task benchmark()
        {
            await Task.Delay(TimeSpan.FromSeconds(2));
            Text = DateTime.Now.ToString(@"hh\:mm\:ss\.ffff tt");
            // screenshot("screenshot.1.png");
            var stopwatch = Stopwatch.StartNew();

            // Example of a function that will get list
            // of items that need to be checked.
            setListOfItemsConditionalChecked();

            m_lsvSecurities.Refresh();
            stopwatch.Stop();
            Text = DateTime.Now.ToString(@"hh\:mm\:ss\.ffff tt");
            await Task.Delay(TimeSpan.FromSeconds(0.5));
            MessageBox.Show($"{stopwatch.ElapsedMilliseconds} ms");
        }



        private async void screenshot(string fileName)
        {
            var imagePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                Assembly.GetEntryAssembly().GetName().Name,
                fileName
            );
            Directory.CreateDirectory(Path.GetDirectoryName(imagePath));
            Bitmap bmp = new Bitmap(this.Width, this.Height);
            DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
            bmp.Save(imagePath, ImageFormat.Png);
            await Task.Delay(TimeSpan.FromSeconds(5));
            Process.Start(new ProcessStartInfo { FileName = imagePath, UseShellExecute = true,});
        }

        private List<Security> SecuritiesList { get; } = new List<Security>();
    }
    class Security
    {
        public string Text { get; set; }
        public bool IsChecked { get; set; }
    }
}
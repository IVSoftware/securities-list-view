using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
                    ListViewItem item = new ListViewItem(text: security.IsChecked ? "\u2612" : "\u2610" );
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
                if(hti.Location == ListViewHitTestLocations.Label)
                {
                    var security = SecuritiesList[hti.Item.Index];
                    security.IsChecked = !security.IsChecked;
                    m_lsvSecurities.Invalidate(hti.Item.Bounds);
                }
            };
            m_lsvSecurities.VirtualListSize = SecuritiesList.Count;
            _ = benchmark();
        }

        /// <summary>
        ///  Show the dialog, then in 2 seconds set the 
        ///  checkboxes to true for 3000 items.
        /// </summary>
        async Task benchmark()
        {
            await Task.Delay(TimeSpan.FromSeconds(2));
            var stopwatch = Stopwatch.StartNew();
            foreach (var item in SecuritiesList)
            {
                item.IsChecked = true;
            }
            m_lsvSecurities.Refresh();
            stopwatch.Stop();
            MessageBox.Show($"{stopwatch.ElapsedMilliseconds} ms");
        }
        private ObservableCollection<Security> SecuritiesList { get; } = new ObservableCollection<Security>();
    }
    class Security : INotifyPropertyChanged
    {
        public string Text
        {
            get => _text;
            set
            {
                if (!Equals(_text, value))
                {
                    _text = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                if (!Equals(_isChecked, value))
                {
                    _isChecked = value;
                    OnPropertyChanged();
                }
            }
        }
        bool _isChecked = false;

        private void OnPropertyChanged([CallerMemberName] string caller = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }

        string _text = string.Empty;

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
# Securities List View

For controls like `ListView` and `DataGridView` that host a large item count, one way to improve performance is to use `VirtualMode=true` to decouple changes to the underlying list items from the UI draw.

```
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
    private List<Security> SecuritiesList { get; } = new List<Security>();
}
```

After generating a list of 3000 securities and showing the list, after 2 seconds the `benchmark()` method is called to set them all to checked.

[![screenshot][1]][1]

```

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
        await Task.Delay(TimeSpan.FromSeconds(0.5));
        MessageBox.Show($"{stopwatch.ElapsedMilliseconds} ms");
    }
```

Where:

```class Security
{
    public string Text { get; set; }
    public bool IsChecked { get; set; }
}
```


  [1]: https://i.stack.imgur.com/3nRSY.png
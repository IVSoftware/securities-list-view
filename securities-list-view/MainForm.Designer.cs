namespace securities_list_view
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            m_lsvSecurities = new ListView();
            SuspendLayout();
            // 
            // m_lsvSecurities
            // 
            m_lsvSecurities.Dock = DockStyle.Fill;
            m_lsvSecurities.Font = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Point);
            m_lsvSecurities.Location = new Point(0, 0);
            m_lsvSecurities.Name = "m_lsvSecurities";
            m_lsvSecurities.Size = new Size(478, 744);
            m_lsvSecurities.TabIndex = 0;
            m_lsvSecurities.UseCompatibleStateImageBehavior = false;
            m_lsvSecurities.View = View.List;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(478, 744);
            Controls.Add(m_lsvSecurities);
            Name = "MainForm";
            Text = "Main Form";
            ResumeLayout(false);
        }

        #endregion

        private ListView m_lsvSecurities;
    }
}
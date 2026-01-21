using System;
using System.Drawing;
using System.Windows.Forms;

namespace MatematikOyunu
{
    public class BaseGameForm : Form
    {
        Label lblTimer;
        Timer uiTimer;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // ⏱ Süre Label
            lblTimer = new Label();
            lblTimer.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblTimer.AutoSize = true;
            lblTimer.Text = "00:00";
            lblTimer.ForeColor = Color.Black;
            lblTimer.BackColor = Color.Transparent;
            lblTimer.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblTimer.Location = new Point(this.ClientSize.Width - 80, 10);

            this.Controls.Add(lblTimer);

            // 🔄 UI Timer
            uiTimer = new Timer();
            uiTimer.Interval = 500; // yarım saniye
            uiTimer.Tick += (s, ev) =>
            {
                lblTimer.Text = GameTimer.GetFormattedTime();
            };
            uiTimer.Start();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            uiTimer?.Stop();
            base.OnFormClosed(e);
        }
    }
}

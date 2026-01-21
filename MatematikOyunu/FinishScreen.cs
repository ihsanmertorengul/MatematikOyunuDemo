using System;
using System.Drawing;
using System.Windows.Forms;

namespace MatematikOyunu
{
    public partial class FinishScreen : Form
    {
        Button btnRestart;
        Button btnExit;
        Label lblTime;
        ListBox lstScores;

        public FinishScreen()
        {
            this.Text = "Oyun Bitti";
            this.Size = new Size(600, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            EkraniOlustur();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            TimeSpan sure = GameTimer.GetTime();
            lblTime.Text = $"Toplam Süre: {sure.Minutes:D2}:{sure.Seconds:D2}";

            var bestScores = ScoreManager.GetBestScores();

            lstScores.Items.Clear();

            int rank = 1;
            foreach (var s in bestScores)
            {
                lstScores.Items.Add(
                    $"{rank}. {s.Minutes:D2}:{s.Seconds:D2}"
                );
                rank++;
            }
        }

        private void EkraniOlustur()
        {
            //  Başlık
            Label lblTitle = new Label();
            lblTitle.Text = "TEBRİKLER 🎉";
            lblTitle.Font = new Font("Segoe UI", 28, FontStyle.Bold);
            lblTitle.AutoSize = false;
            lblTitle.Size = new Size(600, 80);
            lblTitle.Location = new Point(0, 120);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            this.Controls.Add(lblTitle);

            //  Açıklama
            Label lblInfo = new Label();
            lblInfo.Text = "Oyunu başarıyla tamamladınız!";
            lblInfo.Font = new Font("Segoe UI", 16, FontStyle.Regular);
            lblInfo.AutoSize = false;
            lblInfo.Size = new Size(600, 50);
            lblInfo.Location = new Point(0, 210);
            lblInfo.TextAlign = ContentAlignment.MiddleCenter;

            this.Controls.Add(lblInfo);

            //  Süre
            lblTime = new Label();
            lblTime.Text = "Toplam Süre: 00:00";
            lblTime.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTime.AutoSize = false;
            lblTime.Size = new Size(600, 40);
            lblTime.Location = new Point(0, 250);
            lblTime.TextAlign = ContentAlignment.MiddleCenter;

            this.Controls.Add(lblTime);

            // 🏆 En İyi Süreler
            Label lblBest = new Label();
            lblBest.Text = "🏆 En İyi Süreler";
            lblBest.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblBest.AutoSize = false;
            lblBest.Size = new Size(600, 30);
            lblBest.Location = new Point(0, 420);
            lblBest.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblBest);

            lstScores = new ListBox();
            lstScores.Font = new Font("Segoe UI", 12);
            lstScores.Size = new Size(250, 120);
            lstScores.Location = new Point((this.ClientSize.Width - 250) / 2, 460);
            this.Controls.Add(lstScores);

            //  Tekrar Oyna
            btnRestart = new Button();
            btnRestart.Text = "Tekrar Oyna";
            btnRestart.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            btnRestart.Size = new Size(220, 65);
            btnRestart.Location = new Point(
                (this.ClientSize.Width - btnRestart.Width) / 2,
                300
            );
            btnRestart.BackColor = Color.LightSkyBlue;
            btnRestart.FlatStyle = FlatStyle.Flat;
            btnRestart.FlatAppearance.BorderSize = 0;
            btnRestart.Cursor = Cursors.Hand;

            btnRestart.Click += BtnRestart_Click;

            this.Controls.Add(btnRestart);

            //  Çıkış
            btnExit = new Button();
            btnExit.Text = "Çıkış";
            btnExit.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            btnExit.Size = new Size(150, 55);
            btnExit.Location = new Point(
                (this.ClientSize.Width - btnExit.Width) / 2,
                380
            );
            btnExit.BackColor = Color.LightCoral;
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.FlatAppearance.BorderSize = 0;
            btnExit.Cursor = Cursors.Hand;

            btnExit.Click += BtnExit_Click;

            this.Controls.Add(btnExit);
        }

        private void BtnRestart_Click(object sender, EventArgs e)
        {
            GameTimer.Start(); // ⏱ SIFIRLA + BAŞLAT

            Form1 oyunFormu = new Form1();
            oyunFormu.Show();

            this.Close();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


    }
}

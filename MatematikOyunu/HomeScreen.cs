using System;
using System.Drawing;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;

namespace MatematikOyunu
{
    public partial class HomeScreen : Form
    {
        Button btnStart;
        Button btnHowToPlay;
        Button btnEasy, btnMedium, btnHard;
        public static string SelectedLevel = "Kolay"; 


        public HomeScreen()
        {
            this.Text = "Matematik Oyunu";
            this.Size = new Size(600, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            EkraniOlustur();
        }

        private void EkraniOlustur()
        {
            // Başlık
            Label lblTitle = new Label();
            lblTitle.Text = "MATEMATİK OYUNU";
            lblTitle.Font = new Font("Segoe UI", 26, FontStyle.Bold);
            lblTitle.AutoSize = false;
            lblTitle.Size = new Size(600, 80);
            lblTitle.Location = new Point(0, 120);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblTitle.ForeColor = Color.Black;

            this.Controls.Add(lblTitle);

            // START butonu
            btnStart = new Button();
            btnStart.Text = "START";
            btnStart.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            btnStart.Size = new Size(200, 70);
            btnStart.Location = new Point(
                (this.ClientSize.Width - btnStart.Width) / 2,
                280
            );

            btnStart.BackColor = Color.LightGreen;
            btnStart.FlatStyle = FlatStyle.Flat;
            btnStart.FlatAppearance.BorderSize = 0;
            btnStart.Cursor = Cursors.Hand;

            btnStart.Click += BtnStart_Click;

            this.Controls.Add(btnStart);

            // NASIL OYNANIR butonu
            btnHowToPlay = new Button();
            btnHowToPlay.Text = "Nasıl Oynanır?";
            btnHowToPlay.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            btnHowToPlay.Size = new Size(200, 45);
            btnHowToPlay.Location = new Point(
                (this.ClientSize.Width - btnHowToPlay.Width) / 2,
                370
            );

            btnHowToPlay.BackColor = Color.LightBlue;
            btnHowToPlay.FlatStyle = FlatStyle.Flat;
            btnHowToPlay.FlatAppearance.BorderSize = 0;
            btnHowToPlay.Cursor = Cursors.Hand;

            btnHowToPlay.Click += BtnHowToPlay_Click;

            this.Controls.Add(btnHowToPlay);

            // LEVEL başlığı
            Label lblLevel = new Label();
            lblLevel.Text = "Seviye Seç:";
            lblLevel.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblLevel.AutoSize = true;
            lblLevel.Location = new Point((this.ClientSize.Width - lblLevel.Width) / 2, 365);
            this.Controls.Add(lblLevel);

            // 3 adet level kutucuğu (button)
            int boxW = 110;
            int boxH = 55;
            int gap = 15;

            int totalW = (boxW * 3) + (gap * 2);
            int startX = (this.ClientSize.Width - totalW) / 2;
            int y = 450;

            btnEasy = CreateLevelButton("Kolay", startX, y);
            btnMedium = CreateLevelButton("Orta", startX + boxW + gap, y);
            btnHard = CreateLevelButton("Zor", startX + (boxW + gap) * 2, y);

            this.Controls.Add(btnEasy);
            this.Controls.Add(btnMedium);
            this.Controls.Add(btnHard);

            // Varsayılan seçimi görsel olarak işaretle
            SetSelectedLevel(btnEasy);


        }



        private void BtnStart_Click(object sender, EventArgs e)
        {
            GameTimer.Start();

            Form1 oyunFormu = new Form1();
            oyunFormu.Show();

            this.Hide(); // HomeScreen kapanmasın, gizlensin
        }

        private void BtnHowToPlay_Click(object sender, EventArgs e)
        {
            string howToPlayText =
                "🧮 NASIL OYNANIR?\n\n" +
                "• Ekranda bir matematik bulmacası gösterilir.\n" +
                "• Doğru cevabı en kısa süre içinde seçmelisin.\n" +
                "• Yanlış cevaplarda oyun devam eder.\n\n" +
                "🎯 Amaç: En kısa sürede oyunu bitirmek.";

            MessageBox.Show(
                howToPlayText,
                "Nasıl Oynanır?",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
        private Button CreateLevelButton(string text, int x, int y)
        {
            Button b = new Button();
            b.Text = text;
            b.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            b.Size = new Size(110, 55);
            b.Location = new Point(x, y);

            b.BackColor = Color.Gainsboro;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.Cursor = Cursors.Hand;

            b.Click += LevelButton_Click;
            return b;
        }

        private void LevelButton_Click(object sender, EventArgs e)
        {
            var clicked = sender as Button;
            SetSelectedLevel(clicked);
        }

        private void SetSelectedLevel(Button selectedButton)
        {
            // Hepsini pasif görünüm yap
            btnEasy.BackColor = Color.Gainsboro;
            btnMedium.BackColor = Color.Gainsboro;
            btnHard.BackColor = Color.Gainsboro;

            // Seçileni aktif göster
            selectedButton.BackColor = Color.LightSkyBlue;

            // Seçimi kaydet
            SelectedLevel = selectedButton.Text; // "Kolay" / "Orta" / "Zor"
        }


    }
}

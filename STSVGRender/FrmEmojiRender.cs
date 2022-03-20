using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ST.Library.Drawing;

namespace STSVGRender
{
    public partial class FrmEmojiRender : Form
    {
        public FrmEmojiRender() {
            InitializeComponent();
        }

        private EmojiRender m_redner;

        private void FrmEmojiRender_Load(object sender, EventArgs e) {
            //EmojiRender.PackageSvgFiles("./svg_files", "./svg_mix_new");
            m_redner = new EmojiRender("./svg_mix_twemoji");
            textBox1.Text = "😀";
        }

        private void button1_Click(object sender, EventArgs e) {
            string strEmoji = textBox1.Text;
            if (!m_redner.IsEmoji(strEmoji)) {
                MessageBox.Show("Can not found this emoji from package file!");
            }
            RectangleF rectF = new RectangleF(20, 50, 50, 50);
            using (Graphics g = this.CreateGraphics()) {
                g.Clear(Color.White);
                m_redner.DrawEmoji(g, strEmoji, 20, 50, 50, false);
                m_redner.DrawEmoji(g, strEmoji, 100, 50, 50, true);
                // Note: the selected mean that...like the selected text forecolor .. 
                // When emoji selected DrawEmoji will set the alpha 0.5
            }
        }
    }
}

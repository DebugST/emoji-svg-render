[![VS2010](https://img.shields.io/badge/Visual%20Studio-2010-blueviolet)](https://visualstudio.microsoft.com/zh-hans/vs/)   [![.NET40](https://img.shields.io/badge/DotNet-4.0-blue)](https://www.microsoft.com/zh-cn/download/details.aspx?id=25150)    [![license](https://img.shields.io/badge/License-MIT-green)](https://github.com/DebugST/emoji-svg-render/blob/main/LICENSE)
# emoji-svg-render
emoji-svg-render is an svg renderer for emoji only. So the code is very lightweight.

![EmojiRenderForm](https://gitee.com/DebugST/emoji-svg-render/raw/main/images/EmojiRenderForm.png)

```cs
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
```

# Implemented svg elements

`circle` `ellipse` `g` `line` `path` `polygon` `polyline` `lineargradient` `radialgradient` `stop` `rect` `defs` `use`

NOTE：`lineargradient` `radialgradient` implementation is not complete, and complex gradients cannot currently be implemented. such as transform

So this project is friendly to Emoji support without gradients. 

such as:

`[Openmoji](https://openmoji.org/library/)` `[Twemoji](https://github.com/twitter/twemoji)`

# Extend

```cs
[SvgElement("circle")]  // for automatic registration
public class SvgTestCircle : SvgElement
{
    public override string TargetName {
        get { return "circle"; }
    }

    public override bool AllowElementDraw {
        get { return true; }  // some element not need to draw. such as: <defs>
    }

    public float CX { get; set; }
    public float CY { get; set; }
    public float R { get; set; }

    protected internal override void OnInitAttribute(string strName, string strValue) {
        switch (strName) {
            case "cx":
                this.CX = SvgAttributes.GetSize(this, "cx");
                break;
            case "cy":
                this.CY = SvgAttributes.GetSize(this, "cy");
                break;
            case "r":
                this.R = SvgAttributes.GetSize(this, "r");
                break;
        }
    }

    public override GraphicsPath GetPath() {
        GraphicsPath gp = new GraphicsPath();
        RectangleF rectF = new RectangleF(
            SvgAttributes.GetSize(this.CurrentParent, "x") + this.CX - this.R,
            SvgAttributes.GetSize(this.CurrentParent, "y") + this.CY - this.R,
            this.R * 2,
            this.R * 2);
        gp.AddEllipse(rectF);
        return gp;
    }

    protected internal override void Dispose() { }
}
// [register the element] =================
SvgDocument.RegisterType("circle", typeof(SvgTestCircle));
SvgDocument.RegisterType(Application.ExecutablePath); // will check [SvgElement("TargetName")]
...
SvgDocument svg = SvgDocument.FromXml(strXml);
// When an unregistered tag name appears in strXml. . Parsing of this element is ignored.
```
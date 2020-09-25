using System.Text;
using System.IO;
using System.Windows.Forms;

class TextBoxStreamWriter : StringWriter
{
    public TextBox _output = null;

    public TextBoxStreamWriter(TextBox output)
    {
        _output = output;
    }

    public override void WriteLine(string value)
    {
        base.WriteLine(value);
        _output.AppendText($"{value}\r\n");
    }

    public override Encoding Encoding
    {
        get
        {
            return Encoding.UTF8;
        }
    }
}

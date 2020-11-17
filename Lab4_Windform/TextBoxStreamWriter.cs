using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab4_Windform
{
    public class TextBoxStreamWriter : TextWriter
    {
        private TextBox output;
        
        public TextBoxStreamWriter(TextBox output)
        {
            this.output = output;
        }

        public override void Write(char value)
        {
            base.Write(value);
            output.AppendText(value.ToString());
        }
        
        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }

    }
}

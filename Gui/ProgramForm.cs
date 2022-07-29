using Library.InterpretLib;

namespace Gui
{
    public partial class ProgramForm : Form
    {
        private Interpret Interpret { get; set; }
        public ProgramForm()
        {
            
            InitializeComponent();
        }

        private void SetDelegates() {
            Interpret.Print = Print;
        }

        private void loadSource(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Path.Combine(Directory.GetCurrentDirectory(), "SourceFiles");
            openFileDialog.Filter = "txt files (*.txt) |";
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                source.Text = File.ReadAllText(openFileDialog.FileName) ;
            }
        }

        private void saveSource(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Path.Combine(Directory.GetCurrentDirectory(), "SourceFiles");
            saveFileDialog.Filter = "txt file (*.txt) |";
            saveFileDialog.Title = "Save a source code";
            saveFileDialog.DefaultExt = "txt";
            saveFileDialog.AddExtension = true;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (saveFileDialog.FileName != "")
                {
                    File.WriteAllText(saveFileDialog.FileName, source.Text);
                }
                else {
                    console.Text = "Something wrong with save.";
                }
            }

        }

        private void runCode(object sender, EventArgs e)
        {
            console.Clear();
            if (source.Text != "")
            {
                try
                {
                    Interpret = new Interpret(source.Text);
                    SetDelegates();
                    Interpret.Run();
                }
                catch(Exception ex) {
                    Print(ex.Message);
                }
            }
            else {
                console.Text = "Empty source";
            }
        }

        private void showEBNF(object sender, EventArgs e)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "SourceFiles/grammar.txt");
            
            if(File.Exists(path))
            {
                console.Text = File.ReadAllText(path);
            }
            else
            {
                console.Text = "grammar.txt not found";
            }
        }

        private void Print(string text) {
            console.Text += text + Environment.NewLine;
            console.SelectionStart = console.Text.Length;
            console.ScrollToCaret();
        }

        private void Clear(object sender, EventArgs e)
        {
            console.Clear();
            source.Clear();
        }
    }
}
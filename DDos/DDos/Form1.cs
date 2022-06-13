using System.Diagnostics;


namespace DDosAttack

{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            String url;  
            int myInt;

            //function to check if the url exist
            static bool UrlChecker(string url)
            {
                Uri uriResult;
                bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
                return result;
            }
            
            myInt = int.TryParse(textBox1.Text, out myInt) ? myInt : 0;   //try to parse the user input number if not valid(negative or float) will set it 0
            url = textBox2.Text;

            bool goodURL = UrlChecker(url);

            if (goodURL && myInt > 0)
            {
              
                for (int i = 0; i < myInt; i++)
                {

                    ProcessStartInfo psi = new ProcessStartInfo(url) { UseShellExecute = true };
                    Process.Start(psi);

                }
            }
            else
            {
                MessageBox.Show("One or more parameter is not valid\ngive a netural number and valid URL");
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {


            Process[] AllProcesses = Process.GetProcesses();
            foreach (Process process in AllProcesses)
            {
                if (process.MainWindowTitle != "")
                {
                    string s = process.ProcessName.ToLower();
                    if (s == "msedge" || s == "chrome" || s == "firefox")
                        process.Kill();
                }
            }


        }
    }
}
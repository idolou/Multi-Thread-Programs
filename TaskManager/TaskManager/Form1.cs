using System.Diagnostics;
namespace TaskManager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ListAllProcesses();

            dataGridView1.EnableHeadersVisualStyles = false;
        }


        public void ListAllProcesses()
        {
            Process[] AllProcess = Process.GetProcesses();
            foreach (Process p in AllProcess)
            {
                try
                {

                    _ = dataGridView1.Rows.Add(Icon.ExtractAssociatedIcon(p.MainModule.FileName), p.ProcessName, p.Id, p.StartTime.ToShortTimeString());

                }
                catch (Exception ex)
                {

                }
            }
        }

        //End Process button
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Process p = Process.GetProcessById(Int32.Parse(dataGridView1.SelectedRows[0].Cells[2].Value.ToString()));
                p.Kill();
            }
            catch (Exception ex)
            {

            }
            //we want to remove the process we just killed
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            //will add all the running processes
            Process[] AllProcess = Process.GetProcesses();
            foreach (Process p in AllProcess)
            {
                try
                {
                    dataGridView1.Rows.Add(Icon.ExtractAssociatedIcon(p.MainModule.FileName), p.ProcessName, p.Id, p.StartTime.ToShortTimeString());
                }
                catch (Exception ex)
                {

                }
            }
        }

        //Refresh data table button
        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            //will add all the running processes
            Process[] AllProcess = Process.GetProcesses();
            foreach (Process p in AllProcess)
            {
                try

                {
                    dataGridView1.Rows.Add(Icon.ExtractAssociatedIcon(p.MainModule.FileName), p.ProcessName, p.Id, p.StartTime.ToShortTimeString());

                }
                catch (Exception ex)
                {

                }
            }
        }

        //Search box
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            string searchValue = textBox1.Text.ToLower();

            Process[] AllProcess = Process.GetProcesses();
            foreach (Process p in AllProcess)
            {
                try
                {
                    if (p.ProcessName.ToLower().Contains(searchValue) || p.Id.ToString().Contains(searchValue))
                    {
                        dataGridView1.Rows.Add(Icon.ExtractAssociatedIcon(p.MainModule.FileName), p.ProcessName, p.Id, p.StartTime.ToShortTimeString());
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }


    }

}
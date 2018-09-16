using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Numirologia
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

 

        private void button1_Click(object sender, EventArgs e)
        {
            Class1 Cl = new Class1(dateTimePicker1.Value);
            label11.Text = Cl.GetNumItog().ToString();
            label13.Text = Cl.CalcYearsOld(dateTimePicker1.Value).ToString();
            label15.Text = Cl.CalcMenNum().ToString();
            label17.Text = Cl.CalcWomenNum().ToString();
            label19.Text = Cl.GetCursorFate().ToString();
            label21.Text = Cl.GetCursorVolition().ToString();
            int[] _Matrix = Cl.GetNumCountArray();
            Label[] LabelMatrix = new Label[9] { label1, label4 , label7 ,
                label2 , label5 , label8 , label3 , label6 , label9 };
            for (int i = 1; i < 10; i++)
            {
                string CurVal="";
                if (_Matrix[i]!=0)
                {
                    for (int j = 0; j < _Matrix[i]; j++)
                    {
                        CurVal += i.ToString();
                    }
                    LabelMatrix[i - 1].Text = CurVal;
                }
                else
                    LabelMatrix[i - 1].Text = "-";
            }
            int[] CountYear = new int[7] { 0, 12, 24, 36, 48, 60, 72 };
            int[] FateArray = Cl.GetFateArray();
            int[] VolitionArray = Cl.GetVolitionArray();
            DateTime[] DateRootsVect = Cl.GetDateRootsVect();

            chart1

        }
    }
}

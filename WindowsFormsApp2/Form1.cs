using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp2;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.Remoting.Messaging;
using System.Collections;
using System.Globalization;

namespace WindowsFormsApp2
{


    public partial class Form1 : Form
    {
        public struct Mys
        {
            public string sub;
            public string pnt;
            public int min;
            public int max;
            public bool glass;

        };

        Mys decodesize(string in_str)
        {
            Mys rslt;
            rslt.sub = in_str.Substring(0, 30).Trim();
            rslt.pnt = in_str.Substring(30, 30).Trim();
            rslt.min = int.Parse(in_str.Substring(60, 5).Trim());
            rslt.max = int.Parse(in_str.Substring(65, 5).Trim());
            rslt.glass = bool.Parse(in_str.Substring(70, 5).Trim());
            return rslt;
        }
        //
        Mys[] sizes;
        //
        public struct Fasade
        {
            //0;12.0;GENUA;GENUA;GENUA_SHORT;ORIGINAL; ; ; ;298;398;0;0;310;410;6;6;304;404;
            public int fx0, fy0, fx1, fy1; //координаты точек полного размера фасада 0;0;310;410 // левый низ и правый вверх размеры+12(нож)
            public float tx0, ty0, tx1, ty1; //координаты точек чистого размера фасада 6;6;304;404;
            public float truesizex, truesizey; //размеры  полного и чистого размера фасада 298;398 // наши размеры
            public int fidx; //порядковый номер фасада в массиве 
            public string fbasicpnt;
            public string fsubbasicpnt;
            public string fpainting; //GENUA_SHORT
            public string fthermo;// THERMO or ORIGINAL
            public string fthermodiag; //D or " "
            public string fthermohor; //H or " "
            public string fthermovert;// V or " "
        }
        List<Fasade> Fasades = new List<Fasade>();
        //Fasades = new List<Fasade>();
        public Graphics pic;
        public Form1()
        {            
            InitializeComponent();

        }

        
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {           
            // Очистка comboBox2
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            // Чтение файла er2.txt
            string[] sr2 = File.ReadAllLines(@"er2.txt");
            //int zz = sr2.Length/2;
            for (var i=0; i< (sr2.Length)/2; i++)
            {
                if (sr2[i*2] == comboBox1.Text) 
                {
                    comboBox2.Items.Add(sr2[i * 2 + 1]);
                }
            }
            
            if (comboBox2.Items.Count > 0)
            {
                comboBox2.SelectedIndex = 0;
            }
            
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            


            // Очистка comboBox3
            comboBox3.Items.Clear();

            string[] sr3 = File.ReadAllLines(@"er3.txt");
            // Проверка каждого элемента в er2.txt
            for (int i = 0; i < (sr3.Length)/2; i++)
            {
                // Если элемент в er2.txt соответствует выбранному элементу в comboBox1, добавьте следующий элемент в comboBox2
                if (sr3[i*2] == comboBox2.Text)
                {
                    comboBox3.Items.Add(sr3[i*2+1]);
                }
            }
            comboBox3.Items.Add("");
            //Автоматическое заполнение ComboBox3
            if (comboBox3.Items.Count > 0)
            {
                comboBox3.SelectedIndex = 0;
            }
            textBox1_TextChanged(sender, e);
            textBox2_TextChanged(sender, e);
        }
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            
        }
        Graphics g;
        float knife;
        private void Form1_Shown(object sender, EventArgs e)
        {

            pic = pictureBox1.CreateGraphics();

            foreach (string s in File.ReadAllLines(@"er1.txt"))
            {
                comboBox1.Items.Add(s);
            }
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }
            
            string[] sz = File.ReadAllLines(@"er4.txt");
            int z = sz.Length;
            sizes = new Mys[z];
            for (int j = 0; j < z; j++)
            {
                sizes[j] = decodesize(sz[j]);
              
            }
            comboBox4.SelectedItem = "12";
            knife = float.Parse(comboBox4.SelectedItem.ToString());
            //Отключаем область рисования

        }
        


        int textB1, textB2;
        bool textT1, textT2,zerochek;
        private void button1_Click(object sender, EventArgs e)
        {
            if (true)
            {
                //запомнили тексбокс и значения
            string tempTextBox1 = textBox1.Text;
            

            //int tempTextB1 = textB1;
            

            //поменяли местами
            textBox1.Text = textBox2.Text;
            textBox2.Text = tempTextBox1;

            //textB1 = tempTextB2;
            //textB2 = tempTextB1;
             OtherFunction(textB1, textB2);
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string textBox1Value1 = textBox1.Text;
            int number1;
            if (int.TryParse(textBox1Value1, out number1))
            {
                if (number1 <= 2070) 
                {
                    textB1 = int.Parse(textBox1Value1);
                    textT1 = true;
                    textBox1.BackColor = Color.White;
                    
                }
                else
                {
                    textBox1.BackColor = Color.MistyRose;
                    textT1 = false;
                    comboBox3.Text = "";
                }
            }
            else
            {
                textBox1.BackColor = Color.MistyRose;
                textT1 = false;
                comboBox3.Text = "";
            }

            if (textT1 && textT2)
            {
                OtherFunction(textB1, textB2);
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            
        }
        
        Pen penBlack = new Pen(Color.Black, 2);
        // массив  PictureBox
        Fasade fsd;
        private int[,] pictureBoxArray = new int[4140, 5600];
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            
            Graphics g = pictureBox1.CreateGraphics();
            
            int width = textB1+(int)knife; // ширина прямоугольника
            int height = textB2+(int)knife; // высота прямоугольника
            bool flag = false; 
            int x = e.X*4 ; 
            int y = (pictureBox1.Height - e.Y)*4 ;
            int dx, dy;
            dx = x; dy = y;                        
                if (pictureBoxArray[dx, dy] == 0)
                {
                    do
                    {

                        if (dy != 0)
                        {
                            dy--;
                        } 
                    } while ((dy > 0) && (pictureBoxArray[dx, dy] == 0));
                    if (pictureBoxArray[dx, dy] == 1)
                    {
                        dy++;
                    }                    
                    do
                    {
                        if (dx != 0)
                        {
                            dx--;
                        }
                    } while ((dx > 0) && (pictureBoxArray[dx, y] == 0));
                    if (pictureBoxArray[dx, y] == 1)
                    {
                        dx++;
                    }

                    if (CanDrawRectangle(pictureBoxArray, dx, dy, width, height))
                    {
                        flag = true;
                    }
                }
            if (flag)
            {
                x = dx; y = dy;
                g.DrawRectangle(penBlack, x / 4, pictureBox1.Height - y / 4 - height/4, width / 4, height / 4);                
                UpdateArray(pictureBoxArray, x, y, width, height);
                //Fasade fsd;
                fsd.fidx = Fasades.Count;
                fsd.fbasicpnt = comboBox1.Text;
                fsd.fsubbasicpnt = comboBox2.Text;
                fsd.fpainting = comboBox3.Text;
                fsd.truesizex = width;
                fsd.truesizey = height;
                fsd.fx0 = x;
                fsd.fy0 = y;
                fsd.fx1 = x + width;
                fsd.fy1 = y + height;
                fsd.tx0 = x+knife/2;
                fsd.ty0 = y+knife/2;
                fsd.tx1 = fsd.fx1- +knife / 2;
                fsd.ty1 = fsd.fy1- +knife / 2;
                if (checkBox2.Checked || checkBox3.Checked || checkBox4.Checked)
                {
                    fsd.fthermo = "THERMO";
                }
                else fsd.fthermo = "ORIGINAL";
                if (checkBox2.Checked)
                {
                    fsd.fthermodiag = "D";
                }
                else fsd.fthermodiag = " ";
                if (checkBox3.Checked)
                {
                    fsd.fthermovert = "V";
                }
                else fsd.fthermovert = " ";
                if (checkBox4.Checked)
                {
                    fsd.fthermohor = "H";
                }
                else fsd.fthermohor = " ";
                Fasades.Add(fsd);
                listBox1.Items.Add($"{fsd.fpainting}:{fsd.truesizex-knife}x{fsd.truesizey-knife}");
                FasadeText1(fsd);
                
            }
            if (e.Button == MouseButtons.Right)
            {
                // Перебираем все прямоугольники в списке Fasades
                for (int i = 0; i < Fasades.Count; i++)
                {
                    Fasade fasade = Fasades[i];

                    // Проверяем, был ли клик внутри прямоугольника
                    if (e.X >= fasade.fx0 / 4 && e.X <= fasade.fx1 / 4 &&
                        pictureBox1.Height - e.Y >= fasade.fy0 / 4 && pictureBox1.Height - e.Y <= fasade.fy1 / 4)
                    {
                        // Если клик был внутри прямоугольника, выделяем соответствующий элемент в listBox1
                        listBox1.SelectedIndex = i;
                        break;
                    }
                }
            }

            g.Dispose();                       
        }
        private bool CanDrawRectangle(int[,] array, int x, int y, int width, int height)
        {
           
                for (int i = x; i <= x + width; i++)
                {
                    for (int j = y; j < y + height; j++)
                    {
                        if (array[i, j] == 1)
                        {
                            return false;
                        }
                    }
                }    
                if (((x+width)>array.GetLength(0)/2) || ((y + height) > array.GetLength(1) / 2))
                {
                   return false;
                }
            
            return true;
            
        }

        private void UpdateArray(int[,] array, int x, int y, int width, int height)
        {
            for (int i = x; i < x + width; i++)
            {
                for (int j = y; j < y + height; j++)
                {
                         array[i, j] = 1;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {            
            for (int i = Fasades[Fasades.Count-1].fx0; i < Fasades[Fasades.Count - 1].fx0 + Fasades[Fasades.Count - 1].truesizex; i++)
            {
                for (int j = Fasades[Fasades.Count - 1].fy0; j < Fasades[Fasades.Count - 1].fy0 + Fasades[Fasades.Count - 1].truesizey; j++)
                {
                    pictureBoxArray[i, j] = 0;
                }
            }
            Fasades.RemoveAt(Fasades.Count - 1);
            Graphics g = pictureBox1.CreateGraphics();
            pictureBox1.CreateGraphics().Clear(pictureBox1.BackColor);
            //g.Clear(Color.White);
            for (var i=0; Fasades.Count - 1 >= i; i++)
            {
                var fsd = Fasades[i];
                g.DrawRectangle(penBlack, fsd.fx0/4,pictureBox1.Height-fsd.fy0/4-fsd.truesizey/4,fsd.truesizex/4,fsd.truesizey/4);
                FasadeText1(fsd);
            }
            listBox1.Items.RemoveAt(listBox1.Items.Count - 1);
            //FasadeText();
            g.Dispose();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public void SaveFasadesToFile(List<Fasade> fasades)
        {           
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Текстовый файл (*.txt)|*.txt";
                saveFileDialog.Title = "Сохранить данные фасадов";
            if (checkBox5.Checked)
            {
                saveFileDialog.FileName = $"levard_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
            }
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
              // List<string>  savelines;
                    using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                    {
                        foreach (var fasade in fasades)
                        {
                        //string ln;
                        //ln = fasade.fidx.ToString();
                        string knifeStr = knife.ToString("F1", System.Globalization.CultureInfo.InvariantCulture);
                        writer.WriteLine($"{fasade.fidx};{knifeStr};{fasade.fbasicpnt};{fasade.fsubbasicpnt};{fasade.fpainting};{fasade.fthermo};{fasade.fthermodiag};{fasade.fthermovert};{fasade.fthermohor};{fasade.truesizex};{fasade.truesizey};{fasade.fx0};{fasade.fy0};{fasade.fx1};{fasade.fy1};{fasade.tx0};{fasade.ty0};{fasade.tx1};{fasade.ty1};");
                        }
                   
                }
                
            }
            
        }
        private void button3_Click(object sender, EventArgs e)
        {
            SaveFasadesToFile(Fasades);
        }
        public void LoadFasadesFromFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Текстовый файл (*.txt)|*.txt";
            openFileDialog.Title = "Загрузить данные фасадов";
            Graphics g = pictureBox1.CreateGraphics();
            //Fasade fsd = new Fasade();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                listBox1.Items.Clear();
                textBox3.Text = openFileDialog.FileName;
                
                    for (int i = 0; i < pictureBoxArray.GetLength(0); i++)
                {
                    for (int j = 0; j < pictureBoxArray.GetLength(1); j++)
                    {
                        pictureBoxArray[i, j] = 0;
                    }
                 }
                pictureBox1.CreateGraphics().Clear(pictureBox1.BackColor);
                using (StreamReader reader = new StreamReader(openFileDialog.FileName))
                {
                    Fasades.Clear(); // Очищаем список перед загрузкой новых данных

                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(';');

                        
                        fsd.fidx = int.Parse(parts[0]);
                        knife = float.Parse(parts[1], System.Globalization.CultureInfo.InvariantCulture);
                        fsd.fbasicpnt = parts[2];
                        fsd.fsubbasicpnt = parts[3];
                        fsd.fpainting = parts[4];
                        fsd.fthermo = parts[5];
                        fsd.fthermodiag = parts[6];
                        fsd.fthermohor = parts[7];
                        fsd.fthermovert = parts[8];
                        fsd.truesizex = float.Parse(parts[9]);
                        fsd.truesizey = float.Parse(parts[10]);
                        fsd.fx0 = int.Parse(parts[11]);
                        fsd.fy0 = int.Parse(parts[12]);
                        fsd.fx1 = int.Parse(parts[13]);
                        fsd.fy1 = int.Parse(parts[14]);
                        fsd.tx0 = float.Parse(parts[15]);
                        fsd.ty0 = float.Parse(parts[16]);
                        fsd.tx1 = float.Parse(parts[17]);
                        fsd.ty1 = float.Parse(parts[18]);
                        Fasades.Add(fsd);
                        listBox1.Items.Add($"{fsd.fpainting}:{fsd.truesizex-knife}x{fsd.truesizey-knife}");
                        FasadeText1(fsd);
                        for (int i = fsd.fx0; i < fsd.fx0 + fsd.truesizex; i++)
                        {
                            for (int j = fsd.fy0; j < fsd.fy0 + fsd.truesizey; j++)
                            {
                                pictureBoxArray[i, j] = 1;
                            }
                        }
                        //for (var i = 0; fsd.fidx >= i; i++)
                        //{
                            g.DrawRectangle(penBlack, fsd.fx0 / 4, pictureBox1.Height - fsd.fy0 / 4 - fsd.truesizey / 4, fsd.truesizex / 4, fsd.truesizey / 4);
                       // }
                        //Fasades.Add(fsd);
                        //FasadeText();
                        
                    }
                }
            }
            
            g.Dispose();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoadFasadesFromFile();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < pictureBoxArray.GetLength(0); i++)
            {
                for (int j = 0; j < pictureBoxArray.GetLength(1); j++)
                {
                    pictureBoxArray[i, j] = 0;
                }
            }
            pictureBox1.CreateGraphics().Clear(pictureBox1.BackColor);
            listBox1.Items.Clear();
        }
        
        private Fasade? selectedFasade = null;
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            int selectedIndex = listBox1.SelectedIndex;
            if (selectedIndex != -1)
            {
                // Если ранее был выделен прямоугольник, измените его цвет на черный
                if (selectedFasade != null)
                {
                    DrawFasadeText(selectedFasade.Value, Brushes.Black);
                }

                // Изменение цвета текста выбранного прямоугольника
                selectedFasade = Fasades[selectedIndex];
                DrawFasadeText(selectedFasade.Value, Brushes.Red);
            }

        }
        /*
        private void CheckKnife()
        {
            if (fsd.fidx.Count > 0)
            {
                comboBox4.Enabled = false; // Делаем comboBox4 неактивным
            }
            else
            {
                comboBox4.Enabled = true; // Делаем comboBox4 активным
            }
        }
        
        private void FasadeText()
        {
            Graphics g = pictureBox1.CreateGraphics();
            //string drawStringCenter = $"Раскрой {fsd.fidx + 1}";
            string drawStringBottom = $"{fsd.truesizex}х{fsd.truesizey}";
            Font drawFont = new Font("Arial", 8);
            float xCenter = fsd.fx0 / 4 + fsd.truesizex / 8;
            float yCenter = pictureBox1.Height - fsd.fy0 / 4 - fsd.truesizey / 8;
            SizeF textSizeBottom = g.MeasureString(drawStringBottom, drawFont);
            PointF drawPointBottom = new PointF(xCenter - textSizeBottom.Width / 2, yCenter - textSizeBottom.Height / 2);
            g.DrawString(drawStringBottom, drawFont, Brushes.Black, drawPointBottom);
        }
        */
        private void FasadeText1(Fasade fsd)
        {
            Graphics g = pictureBox1.CreateGraphics();
            string drawStringBottom = $"{fsd.truesizex-knife}х{fsd.truesizey-knife}";
            Font drawFont = new Font("Arial", 8);
            float xCenter = fsd.fx0 / 4 + fsd.truesizex / 8;
            float yCenter = pictureBox1.Height - fsd.fy0 / 4 - fsd.truesizey / 8;
            SizeF textSizeBottom = g.MeasureString(drawStringBottom, drawFont);
            PointF drawPointBottom = new PointF(xCenter - textSizeBottom.Width / 2, yCenter - textSizeBottom.Height / 2);
            g.DrawString(drawStringBottom, drawFont, Brushes.Black, drawPointBottom);
        }
        private void DrawFasadeText(Fasade fasade, Brush brush)
        {
            using (Graphics g = pictureBox1.CreateGraphics())
            {
                string drawStringBottom = $"{fasade.truesizex-knife}х{fasade.truesizey-knife}";
                Font drawFont = new Font("Arial", 8);
                float xCenter = fasade.fx0 / 4 + fasade.truesizex / 8;
                float yCenter = pictureBox1.Height - fasade.fy0 / 4 - fasade.truesizey / 8;
                SizeF textSizeBottom = g.MeasureString(drawStringBottom, drawFont);
                PointF drawPointBottom = new PointF(xCenter - textSizeBottom.Width / 2, yCenter - textSizeBottom.Height / 2);

                // Сначала стираем старый текст, рисуя его цветом фона
                g.DrawString(drawStringBottom, drawFont, Brushes.White, drawPointBottom);

                // Затем рисуем новый текст нужным цветом
                g.DrawString(drawStringBottom, drawFont, brush, drawPointBottom);
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
            {
                string fileName = $"levard_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                textBox4.Text = fileName; // Устанавливаем текст в TextBox
            }
            else
            {
                textBox4.Text = ""; // Очищаем TextBox
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string textBox2Value1 = textBox2.Text;
            int number1;
            if (int.TryParse(textBox2Value1, out number1))
            {
                if (number1 <= 2800) // Добавляем проверку здесь
                {
                    textB2 = int.Parse(textBox2Value1);
                    textT2 = true;                   
                    textBox2.BackColor = Color.White;
                    if (number1 > 2070)
                    {
                        button1.Enabled = false;
                    }
                    else
                    {
                        button1.Enabled = true;
                    }
                }
                else
                {
                    textBox2.BackColor = Color.MistyRose;
                    textT2 = false;                   
                    comboBox3.Text = "";
                }
            }
            else
            {
                textBox2.BackColor = Color.MistyRose;
                textT2 = false;               
                comboBox3.Text = "";
            }

            if (textT1 && textT2)
            {
                OtherFunction(textB1, textB2);
            }
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            
            CheckBox checkBox = (CheckBox)sender;
            if (checkBox.Checked == true)
            {
                zerochek = true;               
            }
            else
            {
                zerochek = false;              
            }
            if (textT1 == true && textT2 == true)
            {
                OtherFunction(textB1, textB2);
            }
        }

        private void OtherFunction(int textB1, int textB2)
        {
            int textmin = Math.Min(textB1, textB2);
            string comboBox2Value = comboBox2.SelectedItem.ToString();
            
            for (int i = 0; i < 270; i++)
            {
                
                if (sizes[i].sub == comboBox2Value)// && sizes[i].glass == checkBox1.Checked && !checkBox1.Checked)
                {
                    //MessageBox.Show(i.ToString());
                    if ((textmin <= sizes[i].max && sizes[i].min <= textmin) && !sizes[i].glass)
                    {
                        //MessageBox.Show("Минималка = " + sizes[i].min.ToString() + "Выбранное= " + textmin.ToString() + "Максималка= " + sizes[i].max.ToString() + "Шаблон= "  + sizes[i].pnt.ToString());
                        comboBox3.Text = sizes[i].pnt;
                    };
                    if ((textmin <= sizes[i].max && sizes[i].min <= textmin) && (sizes[i].glass == checkBox1.Checked))
                    {
                        comboBox3.Text = sizes[i].pnt;
                    };
                }
               // MessageBox.Show("Шаблон= " + sizes[i].glass.ToString());       
            }
        }
    }
}

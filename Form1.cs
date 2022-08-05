using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake_Game
{
    public partial class Form1 : Form
    {
        int[] snakeMap; //0 - null  | 1 - body/tail  | 2 - head  | 3 - fruit
        int[] snakeBody;
        string direction = "right";
        int snakeLength = 3;
        int w = 14;
        int h = 10;
        int old = 0;
        int wynik = 0;
        bool canMove = true;
        int speed = 9;
        Random rnd = new Random();
        bool isDirSet = false;
        int jumpU;
        int jumpD;
        public Form1()
        {
            InitializeComponent();
            flowLayoutPanel1.Controls.Clear();
        }
        void startGame()
        {
            if (w == 14) timer1.Interval = 100 + speed * 100;
            else if (w == 28) timer1.Interval = 100 + speed * 50;
            else if (w == 35) timer1.Interval = 100 + speed * 40;
            jumpU = w * (h - 1);
            jumpD = -w * (h - 1);
            flowLayoutPanel1.Controls.Clear();
            wynik = 0;
            label1.Text = "Wynik: 0";
            snakeLength = 3;
            snakeMap = new int[w * h];
            snakeMap[0] = 1;
            snakeMap[1] = 1;
            snakeMap[2] = 2;
            //snakeMap[rnd.Next(4, w * h)] = 3;
            snakeMap[w] = 3;
            snakeBody = new int[snakeLength];
            for (int i = 0; i < 3; i++) snakeBody[i] = i;
            direction = "right";
            createMap();
            paintMap();
            timer1.Start();
        }
        void createMap()
        {
            for (int i = 0; i < w * h; i++)
            {
                Button newButton = new Button();
                newButton.Name = i + "";
                newButton.Text = "";
                newButton.Size = new Size(700 / w, 500 / h);
                newButton.Margin = new Padding(0, 0, 0, 0);
                newButton.Enabled = false;
                newButton.FlatStyle = FlatStyle.Flat;
                newButton.ForeColor = Color.LightGray;
                flowLayoutPanel1.Controls.Add(newButton);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            textBox1.Enabled = false;
            comboBox1.Enabled = false;
            label2.Enabled = false;
            label3.Enabled = false;
            startGame();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            isDirSet = false;
            switch (direction) //Turn
            {
                case "right":
                    if ((snakeBody[snakeLength - 1] + 1 - w) % w == 0) snakeSnif(-w+1);
                    else snakeSnif(1);
                    jump(canMove, (snakeBody[snakeLength - 1] + 1 - w) % w == 0, -w+1);
                    if (canMove) snakeRotation(1);
                    break;
                case "left":
                    if (snakeBody[snakeLength - 1] % w == 0) snakeSnif(w - 1);
                    else snakeSnif(-1);
                    jump(canMove, snakeBody[snakeLength - 1] % w == 0, w - 1);
                    if (canMove) snakeRotation(-1);
                    break;
                case "up":
                    if (snakeBody[snakeLength - 1] - w < 0) snakeSnif(jumpU);
                    else snakeSnif(-w);
                    jump(canMove, snakeBody[snakeLength - 1] - w < 0, jumpU);
                    if (canMove) snakeRotation(-w);
                    break;
                case "down":
                    if (snakeBody[snakeLength - 1] + w > w * h - 1) snakeSnif(jumpD);
                    else snakeSnif(w);
                    jump(canMove, snakeBody[snakeLength - 1] + w > w * h - 1, jumpD);
                    if (canMove) snakeRotation(w);
                    break;
            }
            canMove = true;
            label1.Text = "Wynik: " + wynik;
            updateMap();
            paintMap();
        }
        void snakeRotation(int x)
        {
            bool New = true;
            old = 0;
            for (int i = snakeLength - 1; i >= 0; i--)
            {
                if (New)
                {
                    old = snakeBody[i];
                    snakeBody[i] += x;
                    New = false;
                }
                else
                {
                    int olderr = snakeBody[i];
                    snakeBody[i] = old;
                    old = olderr;
                }
            }
        }
        void snakeSnif(int x)
        {
            int[] oldSnake = snakeBody;
            bool oldLen = true;
            try
            {
                if (snakeMap[snakeBody[snakeLength - 1] + x] == 3) //Snake eat fruit
                {
                    snakeLength++;
                    oldLen = false;
                    wynik++;
                }
                if (snakeMap[snakeBody[snakeLength - 1] + x] == 1) endGame(); //Snake eat tail  TODO: Zjadanie ogona przez krawęć mapy, dont work well
            }
            catch { }
            if (!oldLen)
            {
                snakeBody = new int[snakeLength];
                snakeBody[0] = -1;
                for (int i = 1; i < snakeBody.Length; i++) snakeBody[i] = oldSnake[i - 1];

                int y = rnd.Next(0, w * h);
                bool start = true;
                while (start)
                {
                    if (snakeMap[y] == 0)
                    {
                        start = false;
                        snakeMap[y] = 3;
                    }
                    else y = rnd.Next(0, w * h);
                }
            }
        }
        void jump(bool x, bool y, int jumpDir)
        {
            if (x && y)
            {
                canMove = false;
                snakeRotation(jumpDir);
            }
        }
        void updateMap()
        {
            for (int i = 0; i < h * w; i++)
            {
                if (snakeMap[i] != 3) snakeMap[i] = 0;
                else snakeMap[i] = 3;
            }
            for (int j = 0; j < snakeLength; j++)
            {
                if (j < snakeLength - 1) snakeMap[snakeBody[j]] = 1;
                else snakeMap[snakeBody[j]] = 2;
            }
        }
        void paintMap()
        {
            for (int i = 0; i < w * h; i++)//Wyświetlanie
            {
                if (snakeMap[i] == 1) flowLayoutPanel1.Controls.Find(i + "", true)[0].BackColor = Color.LimeGreen;
                else if (snakeMap[i] == 2) flowLayoutPanel1.Controls.Find(i + "", true)[0].BackColor = Color.Green;
                else if (snakeMap[i] == 3) flowLayoutPanel1.Controls.Find(i + "", true)[0].BackColor = Color.OrangeRed;
                else flowLayoutPanel1.Controls.Find(i + "", true)[0].BackColor = Color.White;
            }
        }
        void endGame()
        {
            timer1.Stop();
            label1.Text = "Wynik: " + wynik;
            button1.Enabled = true;
            textBox1.Enabled = true;
            comboBox1.Enabled = true;
            label2.Enabled = true;
            label3.Enabled = true;
            MessageBox.Show("Przegrałeś");
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.End) endGame();
            else if (e.KeyCode == Keys.Right && direction != "left") directionSet(0);
            else if (e.KeyCode == Keys.Left && direction != "right") directionSet(1);
            else if (e.KeyCode == Keys.Up && direction != "down") directionSet(2);
            else if (e.KeyCode == Keys.Down && direction != "up") directionSet(3);
            else if (e.KeyCode == Keys.Enter) startGame();
        }
        void directionSet(int d)
        {
            if (!isDirSet)
            {
                switch (d)
                {
                    case 0:
                        direction = "right";
                        break;
                    case 1:
                        direction = "left";
                        break;
                    case 2:
                        direction = "up";
                        break;
                    case 3:
                        direction = "down";
                        break;
                }
                isDirSet = true;
            }
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar)) e.Handled = true;
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    h = 10;
                    w = 14;
                    break;
                case 1:
                    h = 20;
                    w = 28;
                    break;
                case 2:
                    h = 25;
                    w = 35;
                    break;
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox1.Text != " ") speed = Int32.Parse(textBox1.Text);
            else
            {
                textBox1.Text = "0";
                speed = 0;
            }
        }
    }
}
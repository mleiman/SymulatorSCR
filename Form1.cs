using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace SymulatorSCR
{
    
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            createTeams();   // Tworzy oba zespoły
            createBall();   // piłka

        }


        Zawodnik[] blue = new Zawodnik[3]; // inicjalizowanie tablicy zawodnikow druzyny niebieskiej
        Zawodnik[] red = new Zawodnik[3];   // i czerwonej
        Ball ball;
       

        public void createTeams()        
        {
            for (int i = 0; i < 3; i++)
            {
                blue[i] = new Zawodnik(true, i); // tworzy gracza niebieskiego
                red[i] = new Zawodnik(false, i); //czerwonego
            }


            for (int i = 0; i < 3; i++)  // przeslanie listy zawodnikow do kazdego z zawodnikow
            {
                blue[i].checkPlayers(blue, red);
                red[i].checkPlayers(blue, red);
            }

        }

        public void createBall()
        {
            ball = new Ball();

            for (int i = 0; i < 3; i++)
            {
                blue[i].setTarget(ball);
                red[i].setTarget(ball);
            }
        }
        

        private void pictureBox1_Paint(object sender, PaintEventArgs e)        //Rysowanie w pictureBox
        {
            /////////////////////////Rysowanie boiska, zawodnikow i pilki
            Pen linie = new Pen(Color.White, 2);

            Graphics g = e.Graphics;
            
            g.DrawLine(linie, 0, pictureBox1.Height / 2, pictureBox1.Width, pictureBox1.Height / 2); // linia polowy boiska - do zmiany obiekt Pen
            g.FillEllipse(ball.getBrush(), ball.getPositionX(), ball.getPositionY(), Ball.width, Ball.width);   //rysowanie pilki

            for (int i = 0; i < 3; i++)//Zawodnicy
            {
                g.FillEllipse(blue[i].getBrush(), blue[i].getPositionX(), blue[i].getPositionY(), Zawodnik.width, Zawodnik.width);    // Rysowanie zawodnikow obu druzyn
                g.FillEllipse(red[i].getBrush(), red[i].getPositionX(), red[i].getPositionY(), Zawodnik.width, Zawodnik.width);       //...................................
            }
        }


 Thread loop;
        private void buttonStart_Click(object sender, EventArgs e)  
        {        
            loop = new Thread(simulationLoop);
            loop.IsBackground = true;       // ustawianie na dzialanie w tle - zabicie watku wraz z zabiciem forma
            loop.Start();        
        }/// koniec button click



  delegate void delegat();          //delegata na funkcje nie zwracajace ani nie przyjmujace zmiennych
        public void simulationLoop()
        {
            delegat d = new delegat(refresh);   // ustawienie funkcji refresh "na delegate"

            startSimulation();

            while (true) ///Odswiezanie picture box
            {

                try
                {
                    Invoke(d);      // wywolanie refresh w watku tworzacym form1
                }
                catch (Exception)
                {

                }   
                Thread.Sleep(20); // spowolnienie predkosci odswiezania
               
            }
        }

        public void refresh()
        {
           pictureBox1.Refresh();
        }


        Thread[] watekBlue = new Thread[3];
        Thread[] watekRed = new Thread[3];
        Thread watekBall;
        public void startSimulation()
        {
            /////////////////////////////////////
            //Tworzenie wątków i ich uruchomienie
            for (int i = 0; i < 3; i++)
            {
                watekBlue[i] = new Thread(blue[i].simulate);
                watekBlue[i].IsBackground = true;   // ustawianie na dzialanie w tle - zabicie watku wraz z zabiciem forma
                watekBlue[i].Start();
            }
            for (int i = 0; i < 3; i++)
            {
                watekRed[i] = new Thread(red[i].simulate);
                watekRed[i].IsBackground = true;    // ustawianie na dzialanie w tle - zabicie watku wraz z zabiciem forma
                watekRed[i].Start();
            }
            watekBall = new Thread(ball.simulate);
            watekBall.IsBackground = true;
            watekBall.Start();
        }


//////////////////////////////////////////////////////////////////////////////

        private void button1_Click(object sender, EventArgs e)
        {
            reset();
        }

        public void reset()
        {
            for (int i = 0; i < 3; i++)
            {
                blue[i] = null;
                red[i] = null;
                watekRed[i].Abort();
                watekBlue[i].Abort();
            }
            ball = null;
            watekBall.Abort();
            loop.Abort();


            if (watekBall.IsAlive || watekBlue[0].IsAlive || watekRed[2].IsAlive)
            {
                Thread.Sleep(1000);
                reset();
            }
            else
            {
                createTeams();
                createBall();
            }


        }//koniec reset
    }//koniec klasy
}

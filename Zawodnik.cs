using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Threading;

namespace SymulatorSCR
{
    class Zawodnik
    {
        public static int width = 30;// srednica zawodnika;
        
        bool team;   // True - niebiescy; False - czerwoni;
        int posX = 0, posY = 0;
        int nr;
        SolidBrush playerBrush;

        Zawodnik[] otherBlues = new Zawodnik[3];  //tablice zawodnikow
        Zawodnik[] otherReds = new Zawodnik[3];
        Ball ball;
       

/// ///////////////////////
/// Tworzenie zawodnika


        public Zawodnik(bool isBlue, int numer)
        {
            team = isBlue;
            iniBrush();
            nr = numer;
            if (team)
            {
                switch (numer)
                {
                    case 0:
                        setPosition(85, 20);
                        break;

                    case 1:
                        setPosition(20, 100);
                        break;


                    case 2:
                        setPosition(150, 100);
                        break;
                    default: break;
                }
            } // koniec if
            else
            {
                switch (numer)
                {
                    case 0:
                        setPosition(85, 240);
                        break;

                    case 1:
                        setPosition(20, 170);
                        break;


                    case 2:
                        setPosition(150, 170);
                        break;
                    default: break;
                }
            }// koniec else
        } // koniec konstruktora Zawodnik

        public bool getTeam()
        {
            return team;
        }
        public void setPosition(int x, int y)
        {
            posX = x;
            posY = y;
        }
        public int getPositionX()
        {
            return posX;
        }
        public int getPositionY()
        {
            return posY;
        }
        public void checkPlayers(Zawodnik[] teamBlue, Zawodnik[] teamRed )  // Pobiera informacje o reszcie zawodnikow
        {
            for (int i = 0; i < 3; i++)
            {
                otherBlues[i] = teamBlue[i];
                otherReds[i] = teamRed[i];
            }
        }
        public void setTarget(Ball p) // Pobiera informaje o pilce
        {
            ball = p;

        }


/// //////////////////////////////
/// Tworzenie obiektu Pen

        private void iniBrush()
        {
            playerBrush = new SolidBrush(setColor());          
        }

        private Color setColor()
        {
            if (team)
            {
                return Color.Blue;
            }
            else
            {
                return Color.Red;
            }
        }
        public SolidBrush getBrush()
        {
            return playerBrush;
        }

////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Akcje symulacji
///
        public void simulate() // symulacja zawodnika
        {
            
            while (true)
            {
                move();
                detectCollisionWall();
                Thread.Sleep(20);

            }
        }
        int vx, vy,los;
        public void move()
        {
            vx = 0; 
            vy = 0;
            ///////////////////////////////////////////////////  Część odpowiadająca za kierowanie się na piłkę
            if (getPositionX() < ball.getPositionX())
            {
                vx += 1;
                kolizjaPilka(vx, vy);
            }
            else if (getPositionX() > ball.getPositionX())
            {
                vx -= 1;
            }
            if (getPositionY() < ball.getPositionY())
            {
                vy += 1; 
            }
            else if (getPositionY() > ball.getPositionY())
            {
                vy -= 1;
            }
            ////////////////////////////////////////////////////////////////////////////////////////////
            kolizjaPilka(vx, vy);     // obslugiwanie kolizji pilek - arg vx,vy - z ktorej strony osi x i y

            if (!detectCollisionPlayer())       // Jeżeli nie ma kolizji z innym graczem
            {
                setPosition(getPositionX() + vx, getPositionY() + vy);
            }
            else if(detectCollisionPlayer())                            // Jeżeli jest kolizja z innym graczem
            {
                vx = 0;
                vy = 0;
                Random rand = new Random();
                los = rand.Next(6);
                switch (los)
                {
                    case 0:
                        vx = 1;
                        break;
                    case 1:
                        vx = -1;
                        break;
                    case 2:
                        vy = 1;
                        break;
                    case 3:
                        vy = -1;
                        break;
                    default:
                        vx = 0;
                        vy = 0;
                        break;
                }

                setPosition(getPositionX() + vx, getPositionY() + vy);
            }            
        }

        private void kolizjaPilka(int argvx, int argvy)
        {
            vx = argvx;
            vy = argvy;
            if (detectCollisionBall())  //Jeżeli wykryto kolizje z pilka
                {
                    if (vx == 1)
                    {
                        ball.vx = 1;
                    }
                    else if (vx == -1)
                    {
                        ball.vx = -1;
                    }
                    if (vy == 1)
                    {
                        ball.vy = 1;
                    }
                    else if (vy == -1)
                    {
                        ball.vy = -1;
                    }

                    for (int i = 6; i > 0; i--)  // im wieksze i, tym wieksza "bezwładność" piłki - dłużej się "toczy" - musi byc odkomentowany thread.sleep
                    {
                        ball.setPosition(ball.getPositionX() + ball.vx, ball.getPositionY() + ball.vy);
                        //Thread.Sleep(20);
                    }

                    ball.vx = 0;    //zerowanie predkosci pilki
                    ball.vy = 0;

                }
        }
////////////////////////////////
        
        private bool detectCollisionPlayer()  // wykrywanie kolizji z innnymi zawodnikami
        {
            //AxMin < BxMax i AxMax > BxMin
            //AyMin < ByMax i AyMax > ByMin

            for (int i = 0; i < 3; i++)
            {

                if (nr == otherBlues[i].nr && team == otherBlues[i].team) // jezeli ten sam zawodnik to break
                    continue;           
                else
                {
                    if ((getPositionX() < otherBlues[i].getPositionX() + width) && (getPositionX() + width > otherBlues[i].getPositionX())
                        && (getPositionY() < otherBlues[i].getPositionY() + width) && (getPositionY() + width > otherBlues[i].getPositionY()))
                    {
                        return true;
                    }
                }
            }
            for (int i = 0; i < 3; i++)
            {
                if (nr == otherReds[i].nr && team == otherReds[i].team)
                    continue;
                else
                {
                    if ((getPositionX() < otherReds[i].getPositionX() + width) && (getPositionX() + width > otherReds[i].getPositionX())
                        && (getPositionY() < otherReds[i].getPositionY() + width) && (getPositionY() + width > otherReds[i].getPositionY()))
                    {
                        return true;
                    }
                }
            }
            return false;
        }// koniec wykrywania kolizji z innym zawodnikiem
       
        //////////////////////////////////////////////////////
        private void detectCollisionWall()  // wykrywanie kolizji ze ścianą
        {
            if (posX == 0)
            {
                setPosition(posX + 1, posY);
            }
            else if (posX == 170)
            {
                setPosition(posX - 1, posY);
            }
            else if (posY == 0)
            {
                setPosition(posX, posY + 1);
            }
            else if (posY == 270)
            {
                setPosition(posX, posY - 1);
            }
        }// koniec wykrywania kolizji sciany
       
         /////////////////////////////////////////////////////////////////////////
        private bool detectCollisionBall() // wykrywanie kolizji z piłką
        {


            if ((ball.getPositionX() < getPositionX() + width) && (ball.getPositionX() + Ball.width > getPositionX())
                         && (ball.getPositionY() < getPositionY() + width) && (ball.getPositionY() + Ball.width > getPositionY()))
            {
                //ball.setPosition(ball.getPositionX() , ball.getPositionY()-1);
                return true;
            }
            else 
                return false;
        }// koniec wykrywania kolizji z piłką
    }//Koniec klasy
}

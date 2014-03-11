using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;

namespace SymulatorSCR
{
    class Ball
    {
        public static int width = 10;// srednica pilki;

        int ballPosX = 0;
        int ballPosY = 0;
        public int vx = 0;
        public int vy = 0;
        SolidBrush ballBrush;

        public Ball()
        {
            setPosition(95, 145);
            iniBrush();
        }
        public void setPosition(int x, int y)
        {
            ballPosX = x;
            ballPosY = y;
        }
        public int getPositionX()
        {
            return ballPosX;
        }
        public int getPositionY()
        {
            return ballPosY;
        }

        /// //////////////////////////////
        /// Tworzenie obiektu Pen

        private void iniBrush()
        {
            ballBrush = new SolidBrush(setColor());
        }

        private Color setColor()
        {
            return Color.DarkOrange;
        }
        public SolidBrush getBrush()
        {
            return ballBrush;
        }


        public void simulate()
        {
            while (true)
            {
                //move();
                detectCollisionWall();
                Thread.Sleep(20);
            }
        }
        public void detectCollisionWall()  // wykrywanie kolizji ze ścianą
        {
            if (ballPosX < 0)
            {
                setPosition(0, ballPosY);
            }
            else if (ballPosX > 190)
            {
                setPosition(190, ballPosY);
            }
            else if (ballPosY < 0)
            {
                if (ballPosY < 0 && ballPosX > 70 && ballPosX < 130)
                {
                    Restart();
                }
                setPosition(ballPosX, 0);
            }
            else if (ballPosY > 290)
            {
                if (ballPosY > 290 && ballPosX > 70 && ballPosX < 130)
                {
                    Restart();
                }
                setPosition(ballPosX,290);
            }
        }// koniec wykrywania kolizji ze sciana


        public void Restart()
        {
            
        }
    }//koniec klasy ball
}

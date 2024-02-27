using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Breakout_Game
{
    public partial class Form1 : Form
    {
        bool goLeft;
        bool goRight;

        int score;
        int ballx;
        int bally;
        int playerSpeed;

        private SoundPlayer hitPlayer;
        private SoundPlayer clearSound;
        private SoundPlayer loseSound;

        // Random number
        Random rnd = new Random();

        public Form1()
        {
            InitializeComponent();
            setupGame();
        }

        private void setupGame()
        {
            // Initialize
            score = 0;
            ballx = 5;
            bally = 5;
            playerSpeed = 12;
            txtScore.Text = "Score: " + score;

            hitPlayer = new SoundPlayer(@"C:\Users\piro1\OneDrive - Arkansas Tech University\Visual Programming\Week 7 (Midterm)\Breakout Game\Audio\hit_se.wav");
            clearSound = new SoundPlayer(@"C:\Users\piro1\OneDrive - Arkansas Tech University\Visual Programming\Week 7 (Midterm)\Breakout Game\Audio\clear_se.wav");
            loseSound = new SoundPlayer(@"C:\Users\piro1\OneDrive - Arkansas Tech University\Visual Programming\Week 7 (Midterm)\Breakout Game\Audio\lose_se.wav");

            gameTimer.Start();

            // Change the color of blocks randomly
            foreach(Control x in this.Controls)
            {
                if(x is PictureBox && (string)x.Tag == "blocks")
                {
                    x.BackColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                }
            }
        }

        // gameover
        private void gameOver(string message)
        {
            gameTimer.Stop();

            // shoe mesage and close game
            txtScore.Text = "Score: " + score + " " + message;
        }

        private void mainGameTimerEvent(object sender, EventArgs e)
        {
            // show score
            txtScore.Text = "Score: " + score;

            // move to left until it reaches to the edge of screen
            if (goLeft == true && player.Left > 0)
                player.Left -= playerSpeed;

            // move to right until it reaches to the edge of screen
            if (goRight == true && player.Left < 700)
                player.Left += playerSpeed;

            // move the ball
            ball.Left += ballx;
            ball.Top += bally;

            // bound when it hits the edge
            if (ball.Left < 0 || ball.Left > 775)
                ballx = -ballx;

            // bound when it hits the top
            if (ball.Top < 0)
                bally = -bally;

            // bound with random speed when it hits the player
            if (ball.Bounds.IntersectsWith(player.Bounds))
            {
                bally = rnd.Next(5, 12) * -1;

                if (ballx < 0)
                    ballx = rnd.Next(5, 12) * -1;
                else
                    ballx = rnd.Next(5, 12);
            }

            // hundle when it hits blocks
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "blocks")
                {
                    // x = block
                    if (ball.Bounds.IntersectsWith(x.Bounds))
                    {
                        // play SE
                        hitPlayer.Play();

                        // add score
                        score += 1;

                        // bound
                        bally = -bally;

                        // delete the block
                        this.Controls.Remove(x);
                    }
                }
            }

            // game clear
            if(score == 15)
            {
                // show end game message
                clearSound.Play();
                gameOver("You Win!!");
            }

            // game over (out of screen)
            if(ball.Top > 455)
            {
                // show lose message
                loseSound.Play();
                gameOver("You Lose!!");
            }
        }

        // keep moving while the key is pressed
        private void keyisdown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Left)
                goLeft = true;
            if(e.KeyCode == Keys.Right)
                goRight = true;
        }

        // stop moving when the key is released
        private void keyisup(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
                goLeft = false;
            if (e.KeyCode == Keys.Right)
                goRight = false;
        }
    }
}

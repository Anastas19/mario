﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Reflection;
using Global;
using Mario.Properties;


namespace Mario
{
    public partial class Form1 : Form
    {
        

        delegate void updateStateDelegate();
        List<PictureBox> sprites = new List<PictureBox>();
        GameAPI game;
        private List<int> keys = new List<int>(new int [4]);
        int offset = 0;

        public Form1()
        {
            
            game = new Game(ref keys);
            InitializeComponent();
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                while (true)
                {
                   game.nextFrame();
                    try
                    {
                        Invoke(new updateStateDelegate(this.updateState));
                    }catch(Exception) { };
                   Thread.Sleep(1000/Settings.Default.fps);
                }
            }).Start();
            /* For disable flicking*/
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty
                | BindingFlags.Instance | BindingFlags.NonPublic,
               null, panel1, new object[] { true });
        }


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down) keys[(int) keysNames.Down] = 1;
            if (e.KeyCode == Keys.Right) keys[(int)keysNames.Right] = 1;
            if (e.KeyCode == Keys.Left) keys[(int)keysNames.Left] = 1;
            if (e.KeyCode == Keys.Space) keys[(int)keysNames.Space] = 1;
        }


        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down) keys[(int)keysNames.Down] = 0;
            if (e.KeyCode == Keys.Right) keys[(int)keysNames.Right] = 0;
            if (e.KeyCode == Keys.Left) keys[(int)keysNames.Left] = 0;
            if (e.KeyCode == Keys.Space) keys[(int)keysNames.Space] = 0;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            updateState();
        }


        private void updateState()
        {
            foreach(Control c in this.panel1.Controls)
            {
                c.Dispose();
            }
            foreach(PictureBox p in sprites)
            {
                p.Dispose();
            }
            this.panel1.Controls.Clear();
            sprites.Clear();
            List<Tuple<Coordinates,Image>> crd = game.getAllUnitsCoordinatesImages();

            foreach ( Tuple<Coordinates, Image>  c in crd)
            {
                PictureBox p = new PictureBox();
                p.Size = mapSize(c.Item1);
                p.Image = c.Item2;
                p.Location = mapPosition(c.Item1, offset);
                sprites.Add(p);
                p.SizeMode = PictureBoxSizeMode.StretchImage;
                this.panel1.Controls.Add(p);
            }

            if(game.getPlayerPosition().topRight.X > this.Width/2+offset)
            {
                offset += game.getPlayerPosition().topRight.X - this.Width / 2 -offset;
            }
            if (game.getPlayerPosition().topRight.X < this.Width / 2 + offset)
            {
                offset += game.getPlayerPosition().topRight.X - this.Width / 2 - offset;
            }

        }


        /* map position from top left to bottom left*/
        private Point mapPosition(Coordinates p, int offset = 0)
        {
            Point res = new Point();
            res.X = p.bottomLeft.X - offset;
            res.Y = this.panel1.Height - p.topRight.Y;
            return res;
        }


        private Size mapSize(Coordinates c)
        {
            Size res = new Size();
            res.Width = c.topRight.X - c.bottomLeft.X;
            res.Height= c.topRight.Y - c.bottomLeft.Y;
            return res;
        }

        
    }
}

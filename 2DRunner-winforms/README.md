# Frågesport

## Fråga till AI

I'd like to create a simple 2d runner game like super mario with scrolling background in winforms with c#, can you provide code for that?

## Svar

Of course! Here's a basic implementation of a 2D runner game like Super Mario with a scrolling background using WinForms in C#:
```
using System;
using System.Drawing;
using System.Windows.Forms;
namespace _2DRunner
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.Timer timer;
        private int playerX = 50;
        private int playerY = 350;
        private int scrollOffset = 0;
        private bool jumping = false;
        private int jumpForce = 15;
        private int gravity = 1;
        public Form1()
        {
            InitializeComponent();
            InitializeGame();
        }
        private void InitializeGame()
        {
            Width = 800;
            Height = 400;
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 20;
            timer.Tick += GameLoop;
            timer.Start();
            DoubleBuffered = true;
        }
        private void GameLoop(object sender, EventArgs e)
        {
            // Scroll the background
            scrollOffset -= 5;
            // Check if the player is jumping
            if (jumping)
            {
                playerY -= jumpForce;
                jumpForce -= gravity;
            }
            // Check if the player has landed on the ground
            if (playerY >= 350)
            {
                playerY = 350;
                jumpForce = 15;
                jumping = false;
            }
            Invalidate();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics graphics = e.Graphics;
            // Draw the background
            for (int i = 0; i < 10; i++)
            {
                graphics.DrawImage(Properties.Resources.background, i * 800 + scrollOffset, 0, 800, 400);
            }
            // Draw the player
            graphics.DrawImage(Properties.Resources.player, playerX, playerY, 50, 50);
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            // Check if the jump key is pressed
            if (e.KeyCode == Keys.Space && !jumping)
            {
                jumping = true;
            }
        }
    }
}
```

## Fråga till AI

How do I add Properties.Resources?

## Svar

To add resources to your project, follow these steps:

1. Open the Solution Explorer in Visual Studio.

2. Right-click on your project and select "Properties".

3. In the Properties window, navigate to the "Resources" tab.

4. Click on the "Add Resource" dropdown button and select the type of resource you want to add. For example, "Add Existing File" to add an image.

5. Select the file you want to add as a resource from the file dialog and click "Open".

Once you have added the resource, you can access it in your code using the `Properties.Resources` class followed by the resource name. For example, if you added an image called "background.png", you can access it like this: `Properties.Resources.background`.

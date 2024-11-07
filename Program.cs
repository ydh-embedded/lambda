using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace Wärmeleitung

//TODO - CoordinateSystem.exe <input_file> <X> <Y>
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Usage: CoordinateSystem <input_file> <X> <Y>");
                return;
            }

            string inputFile = args[0];
            if (!double.TryParse(args[1], out double X) || !double.TryParse(args[2], out double Y))
            {
                Console.WriteLine("Error: X and Y must be valid numbers.");
                return;
            }

            double xllcorner = 0;
            double yllcorner = 0;

            try
            {
                // Read the input file to find xllcorner and yllcorner
                using (StreamReader reader = new StreamReader(inputFile))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith("xllcorner"))
                        {
                            if (!double.TryParse(line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1], out xllcorner))
                            {
                                Console.WriteLine("Error: Unable to parse xllcorner.");
                                return;
                            }
                        }
                        else if (line.StartsWith("yllcorner"))
                        {
                            if (!double.TryParse(line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1], out yllcorner))
                            {
                                Console.WriteLine("Error: Unable to parse yllcorner.");
                                return;
                            }
                        }

                        // Break if both values are found
                        if (xllcorner != 0 && yllcorner != 0)
                        {
                            break;
                        }
                    }
                }

                // Calculate line and column
                int lineIndex = (int)((Y - yllcorner + 7) / 1);
                int colIndex = (int)((X - xllcorner + 1) / 1);

                // Read the specified line and extract the desired column
                using (StreamReader reader = new StreamReader(inputFile))
                {
                    string line;
                    int currentLineIndex = 0;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (currentLineIndex == lineIndex)
                        {
                            string[] columns = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            if (colIndex < columns.Length)
                            {
                                Console.WriteLine(columns[colIndex]);
                            }
                            else
                            {
                                Console.WriteLine("Column index out of range.");
                            }
                            break;
                        }
                        currentLineIndex++;
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Error: The file '{inputFile}' was not found.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }
    }

    public partial class Form : System.Windows.Forms.Form
    {
        // Variables for parameters
        private double lambda1, lambda2, lambda3, A, deltaTheta1, deltaTheta2, deltaTheta3;

        // Variables for graphics
        private Pen penBlack, penRed;
        private Font font;

        public Form()
        {
            InitializeComponent();

            // Initialize parameters
            lambda1 = 1.0;
            lambda2 = 2.0;
            lambda3 = 3.0;
            A = 1.0;
            deltaTheta1 = 10.0; // Added missing semicolon
            deltaTheta2 = 20.0;
            deltaTheta3 = 30.0;

            // Initialize graphics
            penBlack = new Pen(Color.Black, 1);
            penRed = new Pen(Color.Red, 2);
            font = new Font("Arial", 10);
        }

        // Override OnPaint method for custom drawing
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e); // Call the base class method

            // Get the graphics object from the PaintEventArgs
            Graphics g = e.Graphics;

            // Example drawing code
            // Draw a black line
            g.DrawLine(penBlack, 10, 10, 200, 10);

            // Draw a red line
            g.DrawLine(penRed, 10, 20, 200, 20);

            // Draw some text
            g.DrawString("Lambda1: " + lambda1, font, Brushes.Black, new PointF(10, 40));
        }

        // Dispose method to clean up resources
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                penBlack?.Dispose();
                penRed?.Dispose();
                font?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
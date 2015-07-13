using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TwitterAnalysis
{
    public partial class Form1 : Form
    {
        string[] lines;
        List<string> tempWords = new List<string>();
        List<int> medianWords = new List<int>();
        List<int> medianCounts = new List<int>();
        List<string> theWords = new List<string>();
        List<string> allWords = new List<string>();
        List<string> allWordsFinal = new List<string>();
        string allText;
        int iForeachLoop = 1;
        int counter = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lines = getTweets();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            theWords.Clear();
            allWords.Clear();
            tempWords.Clear();
            medianWords.Clear();

            lines = getTweets();
            textBox1.Text = makeText(lines);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            allWordsFinal.Clear();
            tempWords.Clear();
            medianWords.Clear();
            textBox2.Text = null;
            counter = 0;

            if (textBox1.Text == null || textBox1.Text == "")
                MessageBox.Show("Please get tweets first");
            else
            {
                foreach (string word in allWords)
                {
                    if (!allWordsFinal.Contains(word))
                    {
                        allWordsFinal.Add(word);
                    }
                }

                foreach (string wordF in allWordsFinal)
                {
                    counter = 0;

                    foreach (string word in allWords)
                    {
                        if (word == wordF)
                            counter++;
                    }

                    textBox2.Text += wordF + "                  " + counter + "\r\n\r\n";
                }

                WriteToFile(textBox2.Text, @"tweet_output\ft1.txt");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = null;
            textBox2.Text = null;
            textBox3.Text = null;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox3.Text = null;
            counter = 0;
            medianWords.Clear();

            if (textBox2.Text == null || textBox2.Text == "")
                MessageBox.Show("Please count words first");
            else
            {
                foreach (string line in lines)
                {
                    counter = 0;
                    tempWords.Clear();
                    theWords = new List<string>(line.Split(new char[] { ' ' }));

                    foreach (string word in theWords)
                    {
                        if (!tempWords.Contains(word))
                        {
                            tempWords.Add(word);
                            counter++;
                        }
                    }

                    medianWords.Add(counter);
                }

                for (int i = 0; i < medianWords.Count; i++ )
                {
                    medianCounts.Clear();
                    for (int j = 0; j <= i; j++)
                    {
                        medianCounts.Add(medianWords[j]);
                    }
                    medianCounts.Sort();

                    //textBox3.Text += "{";
                    //foreach (int medianc in medianCounts)
                    //    textBox3.Text += medianc + ", ";

                    //textBox3.Text = textBox3.Text.Remove(textBox3.Text.Length - 2) + "}\r\n";

                    if (medianCounts.Count == 1)
                        textBox3.Text += medianCounts[medianCounts.Count / 2] + "\r\n\r\n";
                    else if (medianCounts.Count % 2 != 0)
                        textBox3.Text += medianCounts[medianCounts.Count / 2] + "\r\n\r\n";
                    else if (medianCounts.Count % 2 == 0)
                        textBox3.Text += (((medianCounts[medianCounts.Count / 2]) + ((medianCounts[(medianCounts.Count / 2) - 1]))) / 2f) + "\r\n\r\n";
                }

                WriteToFile(textBox3.Text, @"tweet_output\ft2.txt");
            }
        }

        string makeText(string[] lines)
        {
            allText = "";
            iForeachLoop = 1;

            foreach (string line in lines)
            {
                allText += "Tweet#" + iForeachLoop + ":  " + line + "\r\n\r\n";
                theWords = new List<string>(line.Split(new char[] { ' ' }));

                foreach (string word in theWords)
                    allWords.Add(word);

                allWords.Sort();

                iForeachLoop++;
            }

            return allText;
        }

        string[] getTweets()
        {
            return File.ReadAllLines(@"tweet_input\tweets.txt");
        }

        private void WriteToFile(string text, string filePath)
        {
            string path = filePath;
            File.WriteAllText(path, text);
        }
    }
}

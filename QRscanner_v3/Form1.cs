using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using ZXing;
using System.IO;
using System.Net.Http;
using System.Net;
using LattePanda.Firmata;
using System.Threading;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.IO.Ports;
using System.Management;
using System.Diagnostics;

namespace QRscanner_v3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        UInt16 redPin = 10;
        UInt16 yelPin = 9;
        UInt16 grnPin = 11;
        static Arduino arduino;

        public delegate void AddDataDelegate(String myString);
        public AddDataDelegate myDelegate;


        private void Form1_Load(object sender, EventArgs e)
        {
            /*
            arduino = new Arduino();
            arduino.pinMode(grnPin, Arduino.OUTPUT);
            arduino.pinMode(yelPin, Arduino.OUTPUT);
            arduino.pinMode(redPin, Arduino.OUTPUT);
            */

            //turn off all lights
            UpdateLight(4);

            this.BackColor = Color.FromArgb(0xca, 0xca, 0xca);


            this.myDelegate = new AddDataDelegate(AddDataMethod);

            AssignSerial();
            serialPort1.Open();
            

        }

        public void AddDataMethod(String myString)
        {
            textBox1.Text=myString;
        }

        //color nr: 1:red, 2:yellow, 3:green
        private void UpdateLight(UInt16 color_nr)
        {
            
            switch (color_nr)
            {
                case 1:
                    /*
                    arduino.digitalWrite(grnPin, Arduino.LOW);
                    arduino.digitalWrite(yelPin, Arduino.LOW);
                    arduino.digitalWrite(redPin, Arduino.HIGH);*/
                    this.BackColor = Color.FromArgb(0xff, 0x0c, 0x0c);
                    break;

                case 2:
                    /*arduino.digitalWrite(grnPin, Arduino.LOW);
                    arduino.digitalWrite(redPin, Arduino.LOW);
                    arduino.digitalWrite(yelPin, Arduino.HIGH);*/
                    this.BackColor = Color.FromArgb(0xca, 0xca, 0xca);
                    break;

                case 3:
                    /*arduino.digitalWrite(yelPin, Arduino.LOW);
                    arduino.digitalWrite(redPin, Arduino.LOW);
                    arduino.digitalWrite(grnPin, Arduino.HIGH);*/
                    this.BackColor = Color.FromArgb(0x51,0xb5,0x49);
                    break;
                default:
                    /*arduino.digitalWrite(yelPin, Arduino.LOW);
                    arduino.digitalWrite(redPin, Arduino.LOW);
                    arduino.digitalWrite(grnPin, Arduino.LOW);*/
                    break;
            }
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public async void CallURL(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(url))
                {
                    using (HttpContent content = response.Content)
                    {
                        string mycontent = await content.ReadAsStringAsync();
                        //textBox1.Text = mycontent;
                        if (mycontent.StartsWith("{"))
                        {
                            PersonalData myCurrentData = new PersonalData();
                            myCurrentData.decodeJson(mycontent);
                            if (myCurrentData.Nume != null)
                                textBox2.Text=myCurrentData.Nume;

                            if (myCurrentData.Prenume != null)
                                textBox3.Text=myCurrentData.Prenume;
                        }
                        timer1.Start();
                        UpdateLight(3);
                        
                    }

                }
            }
        }


        private void textBox1_TextChangedComplete(object sender, EventArgs e)
        {
            outBox.Text = "Done writing";
            var QrResult = textBox1.Text;

            //send certificate code to server:
            if (QrResult.StartsWith("HC1:"))
            {

                var QrResult2 = System.Text.Encoding.UTF8.GetBytes(QrResult);
                string QrResult3 = System.Convert.ToBase64String(QrResult2);
                string final_link = "http://localhost:8000/" + QrResult3;

                //CallURL(final_link);
                callPyScript(QrResult3);

            }
            else if(QrResult.Equals("Close Application"))
            {
                Environment.Exit(0);
                this.Close();
            }
            else if(QrResult.Equals(""))
            {
                textBox2.Text = "";
                textBox3.Text = "";
                UpdateLight(2);
            }
            else
            {
                textBox2.Text = "";
                textBox3.Text = "";
                label3.Text = "INVALID";
                //turn on RED light
                UpdateLight(1);
                timer1.Start();
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            label3.Text = "";
            UpdateLight(2);

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();

            //string recText = serialPort1.ReadExisting();

            textBox1.Invoke(this.myDelegate, new Object[] { indata });
        }
        private void AssignSerial()
        {
            var instances = new ManagementClass("Win32_SerialPort").GetInstances();
            foreach (ManagementObject port in instances)
            {
                serialPort1.PortName = port["deviceid"].ToString();
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_BackColorChanged(object sender, EventArgs e)
        {
            textBox2.BackColor = this.BackColor;
            textBox3.BackColor = this.BackColor;
        }

        private void callPyScript(string parsedInput)
        {
            Process myProcess = new Process();

            //Provide the start information for the process
            myProcess.StartInfo.FileName = @"pythonw.exe";
            myProcess.StartInfo.Arguments = @"pyScripts\test_server.py";
            myProcess.StartInfo.UseShellExecute = false;
            myProcess.StartInfo.RedirectStandardInput = true;
            myProcess.StartInfo.RedirectStandardOutput = true;

            //Invoke the process from current process
            myProcess.Start();

            StreamWriter myStreamWriter = myProcess.StandardInput;
            myStreamWriter.WriteLine(parsedInput);

            StreamReader myStreamReader=myProcess.StandardOutput;
            string mycontent=myStreamReader.ReadToEnd();

            //close the process
            myProcess.WaitForExit();
            myStreamWriter.Close();
            myStreamReader.Close();
            myProcess.Close();

            //verify received string
            if (mycontent.StartsWith("{"))
            {
                Console.WriteLine(mycontent);
                PersonalData myCurrentData = new PersonalData();
                myCurrentData.decodeJson(mycontent);
                if (myCurrentData.Nume != null)
                    textBox2.Text = myCurrentData.Nume;

                if (myCurrentData.Prenume != null)
                    textBox3.Text = myCurrentData.Prenume;

                UpdateLight(3);
            }
            timer1.Start();
            

        }

    }


    public class PersonalData
    {
        public string Nume = "Default";
        public string Prenume = "Default";

        public PersonalData()
        {
        }

        public void decodeJson(string InputJson)
        {
            JToken stuff = JObject.Parse(InputJson);
            if (stuff["-260"]["1"]["nam"]["fn"] != null)
                this.Nume = stuff["-260"]["1"]["nam"]["fn"].ToString();

            if (stuff["-260"]["1"]["nam"]["fn"] != null)
                this.Prenume = stuff["-260"]["1"]["nam"]["gn"].ToString();
        }
    }


}

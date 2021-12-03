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

            groupBox1.BackColor = Color.White;

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
                    groupBox1.BackColor = Color.LightCoral;
                    break;

                case 2:
                    /*arduino.digitalWrite(grnPin, Arduino.LOW);
                    arduino.digitalWrite(redPin, Arduino.LOW);
                    arduino.digitalWrite(yelPin, Arduino.HIGH);*/
                    groupBox1.BackColor = Color.LightYellow;
                    break;

                case 3:
                    /*arduino.digitalWrite(yelPin, Arduino.LOW);
                    arduino.digitalWrite(redPin, Arduino.LOW);
                    arduino.digitalWrite(grnPin, Arduino.HIGH);*/
                    groupBox1.BackColor = Color.LightGreen;
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

                CallURL(final_link);

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
                textBox2.Text = "INVALID";
                textBox3.Text = "INVALID";
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

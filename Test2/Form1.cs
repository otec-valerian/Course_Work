using Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test2
{
    public partial class Form1 : Form
    {
        delegate void SetTextCallback(FlowLayoutPanel panel, TextBox box);
        delegate void AddTextCallback(FlowLayoutPanel panel, Button box);
        delegate void DelTextCallback(FlowLayoutPanel panel, Button box);

        internal Tax_Park Tax_park { get; set; }

        public Form1(Tax_Park tp)
        {
            Tax_park = tp;
            tp.SetEventHanlders(UserCarArrived, UserCarGone, ParkTripStarted, ParkTripEnded, OrderStarted);
            InitializeComponent();
            foreach (string t in tp)
            {
                flowLayoutPanel1.Controls.Add(CreateTaxerButton(t));
            }
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private TextBox CreateMessageTextBox()
        {
            TextBox txtbox = new TextBox();
            txtbox.BackColor = Color.LemonChiffon;
            txtbox.Font = new Font(txtbox.Font, FontStyle.Bold);
            txtbox.Enabled = false;
            txtbox.Multiline = true;
            txtbox.ReadOnly = true;
            txtbox.TextChanged += textBox_TextChanged;
            txtbox.MinimumSize = new Size(470, 40);
            return txtbox;
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            TextBox box = sender as TextBox;
            Size size = TextRenderer.MeasureText(box.Text, box.Font);
            box.Width = size.Width;
            box.Height = size.Height;
        }

        private Button CreateTaxerButton(string t)
        {
            Button but = new Button();
            but.UseVisualStyleBackColor = false;
            but.BackColor = Color.OldLace;
            but.Font = new Font(but.Font, FontStyle.Bold);
            but.Size = new Size(100, 60);
            but.Text = t;
            return but;
        }

        private Button CreateAddTaxerButton(string t)
        {
            Button but = new Button();
            but.UseVisualStyleBackColor = false;
            but.BackColor = Color.PaleGoldenrod;
            but.Font = new Font(but.Font, FontStyle.Bold);
            but.Size = new Size(100, 40);
            but.Text = t;
            return but;
        }

        private void UserCarGone(object o, UserEventArgs args)
        {

            TextBox txtbox = CreateMessageTextBox();
            txtbox.MinimumSize = new Size(259, 42);
            txtbox.MaximumSize = new Size(261, 0);
            txtbox.Size = new Size(260, 45);
            txtbox.Lines = new string[] { $"---To client {args._Client.Name} {args._Client.Id}--- : Your {args._Taxer.Car} car has just gone to you!" };
            SetText(flowLayoutPanel5, txtbox);
        }

        private void OrderStarted(object o, UserEventArgs args)
        {

            TextBox txtbox = CreateMessageTextBox();
            txtbox.MinimumSize = new Size(259, 42);
            txtbox.MaximumSize = new Size(261, 0);
            txtbox.Size = new Size(260, 45);
            txtbox.Text = $"{args._Taxer.Name} has taken the {args._Client.Id} client!";
            foreach (object t in flowLayoutPanel2.Controls)
            {
                Button tax = t as Button;
                if (tax.Text == args._Taxer.Name)
                {
                    AddText(flowLayoutPanel3, tax);

                    DelText(flowLayoutPanel2, tax);
                }

            }
            SetText(flowLayoutPanel4, txtbox);
        }

        private void ParkTripStarted(object o, UserEventArgs args)
        {
            TextBox txtbox = CreateMessageTextBox();
            txtbox.MinimumSize = new Size(259, 42);
            txtbox.MaximumSize = new Size(261, 0);
            txtbox.Size = new Size(260, 45);
            txtbox.Text = $"Notification : {args._Taxer.Name} {args._Taxer.Id} has taken an order to client {args._Client.Id}";
            foreach (object t in flowLayoutPanel1.Controls)
            {
                Button tax = t as Button;
                if (tax.Text == args._Taxer.Name)
                {
                    flowLayoutPanel1.Controls.Remove(tax);

                    flowLayoutPanel2.Controls.Add(tax);
                }

            }
            SetText(flowLayoutPanel4, txtbox);
        }

        private void UserCarArrived(object o, UserEventArgs args)
        {
            TextBox txtbox = CreateMessageTextBox();
            txtbox.MinimumSize = new Size(259, 42);
            txtbox.MaximumSize = new Size(261, 0);
            txtbox.Size = new Size(260, 45);
            txtbox.Text = $"---To client {args._Client.Name} {args._Client.Id}--- : Your car is waiting for you!";
            SetText(flowLayoutPanel5, txtbox);
        }

        private void ParkTaxerAction(object o, UserEventArgs args)
        {
            TextBox txtbox = CreateMessageTextBox();
            txtbox.MinimumSize = new Size(259, 42);
            txtbox.MaximumSize = new Size(261, 0);
            txtbox.Size = new Size(260, 45);
            txtbox.Text = args._Message;
            SetText(flowLayoutPanel4, txtbox);
        }

        private void ParkTripEnded(object o, UserEventArgs args)
        {
            TextBox txtbox = CreateMessageTextBox();
            txtbox.MinimumSize = new Size(259, 42);
            txtbox.MaximumSize = new Size(261, 0);
            txtbox.Size = new Size(260, 45);
            txtbox.Text = $"Notification : {args._Message}";
            foreach (object t in flowLayoutPanel3.Controls)
            {
                Button tax = t as Button;
                if (tax.Text == args._Taxer.Name)
                {
                    DelText(flowLayoutPanel3, tax);

                    AddText(flowLayoutPanel1, tax);
                }

            }
            SetText(flowLayoutPanel4, txtbox);
        }


        private void SetText(FlowLayoutPanel panel, TextBox box)
        {
            if (panel.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, panel, box);
            }
            else
            {
                panel.Controls.Add(box);
            }
        }

        private void AddText(FlowLayoutPanel panel, Button box)
        {
            if (panel.InvokeRequired)
            {
                AddTextCallback d = new AddTextCallback(AddText);
                this.Invoke(d, panel, box);
            }
            else
            {
                panel.Controls.Add(box);
            }
        }

        private void DelText(FlowLayoutPanel panel, Button box)
        {
            if (panel.InvokeRequired)
            {
                DelTextCallback d = new DelTextCallback(DelText);
                this.Invoke(d, panel, box);
            }
            else
            {
                panel.Controls.Remove(box);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TextBox txtbox = CreateMessageTextBox();
            txtbox.Lines = new string[] { $"---To new client--- :", $"You've just called to {Tax_park.Name}!", $"Where do you want to go ?" };
            txtbox.Height = 80;
            flowLayoutPanel6.Controls.Add(txtbox);

            foreach (string x in (from t in Tax_park.WorkTown.Locations select t.Name).ToList())
            {
                Button tempBut = CreateTaxerButton(x);
                tempBut.AutoSize = true;
                tempBut.Click += FirstNumberButton_Click;
                flowLayoutPanel6.Controls.Add(tempBut);
            }
        }

        static Place[] cur = new Place[2];
        
        private void FirstNumberButton_Click(object sender, EventArgs args)
        {
            Button b = sender as Button;
            int index = flowLayoutPanel6.Controls.GetChildIndex(b);
            cur[0] = Tax_park.WorkTown.Locations[index - 1];
            Control toRemove = flowLayoutPanel6.Controls[index];
            flowLayoutPanel6.Controls.Remove(toRemove);
            toRemove.Dispose();
            TextBox box = flowLayoutPanel6.Controls[0] as TextBox;
            box.Lines = new string[] { $"---To new client--- : Ok, now choose your current location:" };
            flowLayoutPanel6.Controls.SetChildIndex(box, 0);
            foreach (Control x in flowLayoutPanel6.Controls)
            {
                if (x.GetType().Name == "Button")
                {
                    x.Click -= FirstNumberButton_Click;
                    x.Click += SecondNumberButton_Click;
                }
            }
        }

        private void SecondNumberButton_Click(object sender, EventArgs args)
        {
            Button b = sender as Button;
            string name = b.Text;
            cur[1] = Tax_park.WorkTown.Locations.Where(t => t.Name == name).ElementAt(0);
            Tax_park.SetCurrentOrder(cur[0], cur[1]);
            List<Control> listControls = new List<Control>();
            foreach (Control control in flowLayoutPanel6.Controls)
            {
                if (control.GetType().Name == "Button")
                {
                    listControls.Add(control);
                }
            }

            foreach (Control control in listControls)
            {
                flowLayoutPanel6.Controls.Remove(control);
                control.Dispose();
            }
            TextBox box = flowLayoutPanel6.Controls[0] as TextBox;
            box.Lines = new string[] { $"--- To new client --- : Have you used our service before?" };
            flowLayoutPanel6.Controls.SetChildIndex(box, 0);
            Button YesAnswer = CreateTaxerButton("Yes");
            YesAnswer.Click += YesAnswer_Click;
            Button NoAnswer = CreateTaxerButton("No");
            NoAnswer.Click += NoAnswer_Click;
            flowLayoutPanel6.Controls.Add(YesAnswer);
            flowLayoutPanel6.Controls.Add(NoAnswer);
        }

        private void YesAnswer_Click(object o, EventArgs args)
        {
            Button a = flowLayoutPanel6.Controls[1] as Button;
            Button b = flowLayoutPanel6.Controls[2] as Button;
            flowLayoutPanel6.Controls.Remove(a);
            flowLayoutPanel6.Controls.Remove(b);
            a.Dispose();
            b.Dispose();
            TextBox box = flowLayoutPanel6.Controls[0] as TextBox;
            box.Lines = new string[] { $"--- To new client --- : Ok, say please your ID or phone number." };
            flowLayoutPanel6.Controls.SetChildIndex(box, 0);
            TextBox txt = new TextBox();
            txt.Width = 470;
            flowLayoutPanel6.Controls.Add(txt);
            txt.KeyDown += new KeyEventHandler(txt_KeyDown1);
        }

        private void NoAnswer_Click(object o, EventArgs args)
        {
            Button a = flowLayoutPanel6.Controls[1] as Button;
            Button b = flowLayoutPanel6.Controls[2] as Button;
            flowLayoutPanel6.Controls.Remove(a);
            flowLayoutPanel6.Controls.Remove(b);
            a.Dispose();
            b.Dispose();
            TextBox box = flowLayoutPanel6.Controls[0] as TextBox;
            box.Lines = new string[] { $"--- To new client --- : Ok, say please your name and phone number (separated by space)." };
            flowLayoutPanel6.Controls.SetChildIndex(box, 0);
            TextBox txt = new TextBox();
            txt.Width = 470;
            flowLayoutPanel6.Controls.Add(txt);
            txt.KeyDown += new KeyEventHandler(txt_KeyDown2);
        }

        private void txt_KeyDown1(object sender, KeyEventArgs e)
        {
            TextBox box = sender as TextBox;
            if (e.KeyCode == Keys.Enter)
            {
                if (txt_Validating1(box))
                {
                    if (Tax_park.CheckUser(Convert.ToInt32(box.Text)))
                    {
                        Client curUser = Tax_park.FindClient(Convert.ToInt32(box.Text));
                        Tax_park.SetCurrentClient(curUser);
                        TextBox boxx = flowLayoutPanel6.Controls[0] as TextBox;
                        boxx.Lines = new string[] { $"--- To {curUser.Name} {curUser.Id} client --- : Ok, we found you!",
                        $"With the discount as for our client the cost of the {Tax_park.CurrentOrder.location.Name}-{Tax_park.CurrentOrder.destination.Name} trip",
                        $"will be {Tax_park.GetPrice(Tax_park.CurrentOrder.location, Tax_park.CurrentOrder.destination, curUser)}",
                        "Do you agree?"};
                        flowLayoutPanel6.Controls.SetChildIndex(boxx, 0);
                        Control toRemove = flowLayoutPanel6.Controls[1];
                        flowLayoutPanel6.Controls.Remove(toRemove);
                        toRemove.Dispose();
                        Button but1 = CreateTaxerButton("Yes");
                        Button but2 = CreateTaxerButton("No");
                        flowLayoutPanel6.Controls.Add(but1);
                        flowLayoutPanel6.Controls.Add(but2);
                        but1.Click += YesAnswer2_Click;
                        but2.Click += NoAnswer2_Click;
                    }
                }
            }
        }

        private void txt_KeyDown2(object sender, KeyEventArgs e)
        {
            TextBox box = sender as TextBox;
            if (e.KeyCode == Keys.Enter)
            {
                if (txt_Validating2(box))
                {
                    string[] data = box.Text.Split(' ');
                    Client curUser = new Client(data[0], Convert.ToInt32(data[1]));
                    Tax_park.SetCurrentClient(curUser);
                    TextBox boxx = flowLayoutPanel6.Controls[0] as TextBox;
                    boxx.Lines = new string[] { $"--- To {curUser.Name} {curUser.Id} client --- :", $"Ok, now you're registered! Your ID is {curUser.Id}",
                    $"As a general purpose without discount the cost of", $"the {Tax_park.CurrentOrder.location.Name}-{Tax_park.CurrentOrder.destination.Name} trip",
                    $"will be {Tax_park.GetPrice(Tax_park.CurrentOrder.location, Tax_park.CurrentOrder.destination, curUser)}",
                    "Do you agree?"};
                    flowLayoutPanel6.Controls.SetChildIndex(boxx, 0);
                    Control toRemove = flowLayoutPanel6.Controls[1];
                    flowLayoutPanel6.Controls.Remove(toRemove);
                    toRemove.Dispose();
                    Button but1 = CreateTaxerButton("Yes");
                    Button but2 = CreateTaxerButton("No");
                    flowLayoutPanel6.Controls.Add(but1);
                    flowLayoutPanel6.Controls.Add(but2);
                    but1.Click += YesAnswer2_Click;
                    but2.Click += NoAnswer2_Click;

                }
            }
        }

        private bool txt_Validating1(object sender)
        {
            TextBox box = sender as TextBox;
            if (String.IsNullOrEmpty(box.Text))
            {
                TextBox boxx = flowLayoutPanel6.Controls[0] as TextBox;
                boxx.Lines = new string[] { "Empty line!" };
                flowLayoutPanel6.Controls.SetChildIndex(boxx, 0);
                return false;
            }
            else if (!box.Text.All(t => Char.IsDigit(t)))
            {
                TextBox boxx = flowLayoutPanel6.Controls[0] as TextBox;
                boxx.Lines = new string[] { "Input must contain only integers!" };
                flowLayoutPanel6.Controls.SetChildIndex(boxx, 0);
                return false;
            }
            else
            {
                return true;
            }
        }
        private bool txt_Validating2(object sender)
        {
            TextBox box = sender as TextBox;
            string[] data = box.Text.Split(' ');
            if (String.IsNullOrEmpty(box.Text))
            {
                TextBox boxx = flowLayoutPanel6.Controls[0] as TextBox;
                boxx.Lines = new string[] { "Empty line!" };
                flowLayoutPanel6.Controls.SetChildIndex(boxx, 0);
                return false;
            }
            else if (!data[0].All(t => Char.IsLetter(t)))
            {
                TextBox boxx = flowLayoutPanel6.Controls[0] as TextBox;
                boxx.Lines = new string[] { "Name must contain only letters!" };
                flowLayoutPanel6.Controls.SetChildIndex(boxx, 0);
                return false;
            }
            else if (data.Length != 2)
            {
                TextBox boxx = flowLayoutPanel6.Controls[0] as TextBox;
                boxx.Lines = new string[] { "Too many or few arguments!" };
                flowLayoutPanel6.Controls.SetChildIndex(boxx, 0);
                return false;
            }
            else if (!data[1].All(t => Char.IsDigit(t)))
            {
                TextBox boxx = flowLayoutPanel6.Controls[0] as TextBox;
                boxx.Lines = new string[] { "Phone must contain only digits!" };
                flowLayoutPanel6.Controls.SetChildIndex(boxx, 0);
                return false;
            }
            else
            {
                return true;
            }
        }

        private void YesAnswer2_Click(object o, EventArgs args)
        {
            Button a = flowLayoutPanel6.Controls[1] as Button;
            Button b = flowLayoutPanel6.Controls[2] as Button;
            TextBox c = flowLayoutPanel6.Controls[0] as TextBox;
            flowLayoutPanel6.Controls.Remove(a);
            flowLayoutPanel6.Controls.Remove(b);
            flowLayoutPanel6.Controls.Remove(c);
            a.Dispose();
            b.Dispose();
            c.Dispose();
            Tax_park.TakeOrder(Tax_park.CurrentClient, Tax_park.CurrentOrder.location, Tax_park.CurrentOrder.destination);
        }

        private void NoAnswer2_Click(object o, EventArgs args)
        {
            Button a = flowLayoutPanel6.Controls[1] as Button;
            Button b = flowLayoutPanel6.Controls[2] as Button;
            TextBox c = flowLayoutPanel6.Controls[0] as TextBox;
            flowLayoutPanel6.Controls.Remove(a);
            flowLayoutPanel6.Controls.Remove(b);
            a.Dispose();
            b.Dispose();
            c.Dispose();
            TextBox txtbox = CreateMessageTextBox();
            txtbox.Text = $"---To client {Tax_park.CurrentClient.Name} {Tax_park.CurrentClient.Id}--- : Ok, good day!";
            flowLayoutPanel5.Controls.Add(txtbox);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            TextBox txtbox = CreateMessageTextBox();
            txtbox.TextChanged -= textBox_TextChanged;
            txtbox.MinimumSize = new Size(375, 45);
            txtbox.Font = new Font(txtbox.Font.FontFamily, 14);
            txtbox.Size = new Size(0, 0);
            txtbox.Text = $"Registration the new taxer: ";
            flowLayoutPanel6.Controls.Add(txtbox);

            Button accept = new Button();
            accept.Size = new Size(40, 45);
            accept.BackColor = Color.LimeGreen;
            accept.Font = new Font(accept.Font, FontStyle.Bold);
            accept.Font = new Font(accept.Font.FontFamily, 8);
            accept.Text = "OK";
            accept.Click += accept_Click;
            flowLayoutPanel6.Controls.Add(accept);

            Button decline = new Button();
            decline.Size = new Size(40, 45);
            decline.BackColor = Color.DeepPink;
            decline.Font = new Font(decline.Font, FontStyle.Bold);
            decline.Font = new Font(decline.Font.FontFamily, 12);
            decline.Text = "X";
            decline.Click += decline_Click;
            flowLayoutPanel6.Controls.Add(decline);

            TextBox name = CreateMessageTextBox();
            name.MinimumSize = new Size(0, 40);
            name.Text = "Name";
            name.Width = 140;
            flowLayoutPanel6.Controls.Add(name);
            TextBox EnterName = new TextBox();
            EnterName.Validating += nameBox_Validating;
            EnterName.TextChanged += EnterName_TextChanged;
            EnterName.Multiline = true;
            EnterName.Size = new Size(230, 40);
            flowLayoutPanel6.Controls.Add(EnterName);

            TextBox car = CreateMessageTextBox();
            car.MinimumSize = new Size(0, 40);
            car.Text = "Car";
            car.Width = 140;
            flowLayoutPanel6.Controls.Add(car);
            TextBox EnterCar = new TextBox();
            EnterCar.Multiline = true;
            EnterCar.Size = new Size(230, 40);
            flowLayoutPanel6.Controls.Add(EnterCar);

            TextBox shift = CreateMessageTextBox();
            shift.MinimumSize = new Size(0, 40);
            shift.Text = "Shift";
            shift.Width = 140;
            flowLayoutPanel6.Controls.Add(shift);
            Button DayShift = CreateAddTaxerButton("Day");
            DayShift.Click += DayShift_Click;
            flowLayoutPanel6.Controls.Add(DayShift);

            Button NightShift = CreateAddTaxerButton("Night");
            NightShift.Click += NightShift_Click;
            flowLayoutPanel6.Controls.Add(NightShift);

            TextBox week = CreateMessageTextBox();
            week.MinimumSize = new Size(0, 40);
            week.Text = "Work days";
            week.Width = 140;
            flowLayoutPanel6.Controls.Add(week);


            foreach (var a in Enum.GetValues(typeof(WorkDays)))
            {
                Button day = CreateAddTaxerButton(a.ToString());
                day.Click += day_Click;
                flowLayoutPanel6.Controls.Add(day);
            }


        }

        private void DayShift_Click(object o, EventArgs args)
        {
            Button b = o as Button;
            if (b.BackColor == Color.PaleGoldenrod)
            {
                b.BackColor = Color.PaleGreen;
                Button b2 = flowLayoutPanel6.Controls[9] as Button;
                if (b2.BackColor == Color.PaleGreen)
                {
                    b2.BackColor = Color.PaleGoldenrod;
                }
            }
        }

        private void NightShift_Click(object o, EventArgs args)
        {
            Button b = o as Button;
            if (b.BackColor == Color.PaleGoldenrod)
            {
                b.BackColor = Color.PaleGreen;
                Button b2 = flowLayoutPanel6.Controls[8] as Button;
                if (b2.BackColor == Color.PaleGreen)
                {
                    b2.BackColor = Color.PaleGoldenrod;
                }
            }
        }

        private void day_Click(object o, EventArgs args)
        {
            Button b = o as Button;
            if (b.BackColor == Color.PaleGoldenrod) { b.BackColor = Color.LimeGreen; } else { b.BackColor = Color.PaleGoldenrod; }
        }

        private void nameBox_Validating(object sender, CancelEventArgs e)
        {
            TextBox b = sender as TextBox;
            if (String.IsNullOrEmpty(b.Text))
            {
                errorProvider1.SetError(b, "Empty line!");
                b.BackColor = Color.Crimson;
            }
            else if (!b.Text.All(t => Char.IsLetter(t)))
            {
                errorProvider1.SetError(b, "Line must contain only letters!");
                b.BackColor = Color.Crimson;
            }
            else
            {
                errorProvider1.Clear();
            }
        }

        private void accept_Click(object o, EventArgs args)
        {

            TextBox b = flowLayoutPanel6.Controls[4] as TextBox;
            if (b.BackColor == Color.Crimson)
            {
                TextBox New = flowLayoutPanel6.Controls[0] as TextBox;
                New.Text = "Invalid input in the name column!!";
                flowLayoutPanel6.Controls.SetChildIndex(New, 0);
            }
            else if (flowLayoutPanel6.Controls[8].BackColor == Color.PaleGoldenrod && flowLayoutPanel6.Controls[9].BackColor == Color.PaleGoldenrod)
            {
                TextBox New = flowLayoutPanel6.Controls[0] as TextBox;
                New.Text = "Choose the shift!";
                flowLayoutPanel6.Controls.SetChildIndex(New, 0);
            }
            else if (flowLayoutPanel6.Controls[11].BackColor == Color.PaleGoldenrod && flowLayoutPanel6.Controls[12].BackColor == Color.PaleGoldenrod &&
               flowLayoutPanel6.Controls[13].BackColor == Color.PaleGoldenrod && flowLayoutPanel6.Controls[14].BackColor == Color.PaleGoldenrod &&
                   flowLayoutPanel6.Controls[15].BackColor == Color.PaleGoldenrod && flowLayoutPanel6.Controls[16].BackColor == Color.PaleGoldenrod &&
                   flowLayoutPanel6.Controls[17].BackColor == Color.PaleGoldenrod)
            {
                TextBox New = flowLayoutPanel6.Controls[0] as TextBox;
                New.Text = "Choose at least one work day!";
                flowLayoutPanel6.Controls.SetChildIndex(New, 0);
            }
            else
            {
                TextBox name = flowLayoutPanel6.Controls[4] as TextBox;
                string taxName = name.Text;
                TextBox car = flowLayoutPanel6.Controls[6] as TextBox;
                string taxCar = car.Text;
                Button day = flowLayoutPanel6.Controls[8] as Button;
                Button night = flowLayoutPanel6.Controls[9] as Button;
                WorkChange wc;
                if (day.BackColor == Color.PaleGreen) { wc = (WorkChange)Enum.Parse(typeof(WorkChange), day.Text); }
                else
                {
                    wc = (WorkChange)Enum.Parse(typeof(WorkChange), night.Text);
                }
                List<WorkDays> wd = new List<WorkDays>();
                Button m = flowLayoutPanel6.Controls[11] as Button;
                Button t = flowLayoutPanel6.Controls[12] as Button;
                Button w = flowLayoutPanel6.Controls[13] as Button;
                Button th = flowLayoutPanel6.Controls[14] as Button;
                Button f = flowLayoutPanel6.Controls[15] as Button;
                Button sat = flowLayoutPanel6.Controls[16] as Button;
                Button sun = flowLayoutPanel6.Controls[17] as Button;
                if (m.BackColor == Color.LimeGreen) { wd.Add((WorkDays)Enum.Parse(typeof(WorkDays), m.Text)); }
                if (t.BackColor == Color.LimeGreen) { wd.Add((WorkDays)Enum.Parse(typeof(WorkDays), t.Text)); }
                if (w.BackColor == Color.LimeGreen) { wd.Add((WorkDays)Enum.Parse(typeof(WorkDays), w.Text)); }
                if (th.BackColor == Color.LimeGreen) { wd.Add((WorkDays)Enum.Parse(typeof(WorkDays), th.Text)); }
                if (f.BackColor == Color.LimeGreen) { wd.Add((WorkDays)Enum.Parse(typeof(WorkDays), f.Text)); }
                if (sat.BackColor == Color.LimeGreen) { wd.Add((WorkDays)Enum.Parse(typeof(WorkDays), sat.Text)); }
                if (sun.BackColor == Color.LimeGreen) { wd.Add((WorkDays)Enum.Parse(typeof(WorkDays), sun.Text)); }
                Tax_park.AddTaxers(new Taxer(taxName, taxCar, wc, wd));
                Button newTax = CreateTaxerButton(taxName);
                flowLayoutPanel1.Controls.Add(newTax);
                List<Control> listControls = new List<Control>();
                foreach (Control control in flowLayoutPanel6.Controls)
                {
                    listControls.Add(control);
                }

                foreach (Control control in listControls)
                {
                    flowLayoutPanel6.Controls.Remove(control);
                    control.Dispose();
                }
            }

        }

        private void decline_Click(object o, EventArgs args)
        {
            List<Control> listControls = new List<Control>();
            foreach (Control control in flowLayoutPanel6.Controls)
            {
                listControls.Add(control);
            }

            foreach (Control control in listControls)
            {
                flowLayoutPanel6.Controls.Remove(control);
                control.Dispose();
            }
        }

        private void EnterName_TextChanged(object o, EventArgs args)
        {
            TextBox b = o as TextBox;
            if (b.BackColor == Color.Crimson)
            {
                b.BackColor = Color.White;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            TextBox txtbox = CreateMessageTextBox();
            txtbox.TextChanged -= textBox_TextChanged;
            txtbox.MinimumSize = new Size(375, 45);
            txtbox.Font = new Font(txtbox.Font.FontFamily, 14);
            txtbox.Size = new Size(0, 0);
            txtbox.Text = $"Enter the taxer's name or ID: ";
            flowLayoutPanel6.Controls.Add(txtbox);

            Button decline = new Button();
            decline.Size = new Size(40, 45);
            decline.BackColor = Color.DeepPink;
            decline.Font = new Font(decline.Font, FontStyle.Bold);
            decline.Font = new Font(decline.Font.FontFamily, 12);
            decline.Text = "X";
            decline.Click += decline_Click;
            flowLayoutPanel6.Controls.Add(decline);

            TextBox EnterTax = new TextBox();
            EnterTax.TextChanged += EnterTax_TextChanged;
            EnterTax.Multiline = true;
            EnterTax.Size = new Size(230, 40);
            flowLayoutPanel6.Controls.Add(EnterTax);

            Button del = new Button();
            del.Size = new Size(130, 45);
            del.BackColor = Color.IndianRed;
            del.Font = new Font(del.Font, FontStyle.Bold);
            del.Font = new Font(del.Font.FontFamily, 15);
            del.Text = "OK";
            del.Click += del_Click;
            flowLayoutPanel6.Controls.Add(del);
        }

        private void EnterTax_TextChanged(object o, EventArgs args)
        {
            TextBox txt = o as TextBox;
            if (Tax_park.FindTaxer(txt.Text) != null)
            {
                txt.BackColor = Color.LimeGreen;
            }
            else
            {
                txt.BackColor = Color.Crimson;
            }
        }

        private void del_Click(object o, EventArgs args)
        {
            TextBox box = flowLayoutPanel6.Controls[2] as TextBox;
            if (flowLayoutPanel6.Controls[2].BackColor == Color.Crimson)
            {
                TextBox b = flowLayoutPanel6.Controls[0] as TextBox;
                b.Text = "Taxer with such ID or name have not been found!";
            }
            else if (!CheckTaxIsFree(Tax_park.FindTaxer(box.Text)))
            {
                TextBox b = flowLayoutPanel6.Controls[0] as TextBox;
                b.Text = "Taxer is in a trip now. Wait till it comes!";
            }
            else
            {
                Taxer tax = Tax_park.FindTaxer(box.Text);
                List<Control> listControls = new List<Control>();
                foreach (Control control in flowLayoutPanel1.Controls)
                {
                    listControls.Add(control);
                }

                foreach (Control control in listControls)
                {
                    Button b = control as Button;
                    if (b.Text == tax.Name)
                    {
                        flowLayoutPanel1.Controls.Remove(control);
                        control.Dispose();
                    }
                }
                Tax_park.DelTaxer(tax);
                List<Control> listControls2 = new List<Control>();
                foreach (Control control in flowLayoutPanel6.Controls)
                {
                    listControls2.Add(control);
                }

                foreach (Control control in listControls2)
                {
                    flowLayoutPanel6.Controls.Remove(control);
                    control.Dispose();
                }
            }

        }

        private bool CheckTaxIsFree(Taxer tax)
        {
            List<Control> listControls = new List<Control>();
            foreach (Control control in flowLayoutPanel1.Controls)
            {
                listControls.Add(control);
            }

            foreach (Control control in listControls)
            {
                Button b = control as Button;
                if (tax.Name == b.Text) { return true; }
            }
            return false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            TextBox txtbox = CreateMessageTextBox();
            txtbox.TextChanged -= textBox_TextChanged;
            txtbox.MinimumSize = new Size(310, 45);
            txtbox.Font = new Font(txtbox.Font.FontFamily, 14);
            txtbox.Size = new Size(0, 0);
            txtbox.Text = $"Editing the taxer's schedule: ";
            flowLayoutPanel6.Controls.Add(txtbox);

            Button accept = new Button();
            accept.Size = new Size(60, 45);
            accept.BackColor = Color.Green;
            accept.Font = new Font(accept.Font, FontStyle.Bold);
            accept.Font = new Font(accept.Font.FontFamily, 8);
            accept.Text = "Save";
            accept.Click += accept2_Click;
            flowLayoutPanel6.Controls.Add(accept);

            Button decline = new Button();
            decline.Size = new Size(60, 45);
            decline.BackColor = Color.DeepPink;
            decline.Font = new Font(decline.Font, FontStyle.Bold);
            decline.Font = new Font(decline.Font.FontFamily, 12);
            decline.Text = "X";
            decline.Click += decline_Click;
            flowLayoutPanel6.Controls.Add(decline);

            TextBox v = CreateMessageTextBox();
            v.TextChanged -= textBox_TextChanged;
            v.MinimumSize = new Size(80, 40);
            v.Font = new Font(txtbox.Font.FontFamily, 8);
            v.Size = new Size(0, 0);
            v.Text = "";
            flowLayoutPanel6.Controls.Add(v);

            foreach (var a in Enum.GetValues(typeof(WorkDays)))
            {
                TextBox b = CreateScheduleBox(a.ToString());
                flowLayoutPanel6.Controls.Add(b);
            }

            foreach (string n in Tax_park)
            {
                TextBox b = CreateScheduleBox(n);
                b.Size = new Size(80, 40);
                flowLayoutPanel6.Controls.Add(b);
                foreach (WorkDays day in Enum.GetValues(typeof(WorkDays)))
                {
                    Button b2 = CreateScheduleButton("");
                    if (Tax_park.IsTaxersDay(Tax_park.FindTaxer(n), day))
                    {
                        b2.BackColor = Color.LimeGreen;
                    }
                    b2.Click += schedule_Click;
                    flowLayoutPanel6.Controls.Add(b2);
                }
            }

        }

        private Button CreateScheduleButton(string t)
        {
            Button but = new Button();
            but.UseVisualStyleBackColor = false;
            but.BackColor = Color.OldLace;
            but.Font = new Font(but.Font, FontStyle.Bold);
            but.Size = new Size(40, 40);
            but.Text = t;
            return but;
        }

        private TextBox CreateScheduleBox(string t)
        {
            TextBox but = new TextBox();
            but.Multiline = true;
            but.Enabled = false;
            but.BackColor = Color.OldLace;
            but.Font = new Font(but.Font, FontStyle.Bold);
            but.Size = new Size(40, 40);
            but.Text = t;
            return but;
        }
        private void schedule_Click(object o, EventArgs args)
        {
            Button b = o as Button;
            if (b.BackColor == Color.LimeGreen)
            {
                b.BackColor = Color.OldLace;
            }
            else
            {
                b.BackColor = Color.LimeGreen;
            }
        }

        private void accept2_Click(object o, EventArgs args)
        {

            List<WorkDays> days = Enum.GetValues(typeof(WorkDays)).Cast<WorkDays>().ToList();
            foreach (Control c in flowLayoutPanel6.Controls)
            {
                if (c.GetType().Name == "Button" && flowLayoutPanel6.Controls.GetChildIndex(c) > 11)
                {
                    Button b = c as Button;
                    int index = flowLayoutPanel6.Controls.GetChildIndex(b);
                    TextBox txt = flowLayoutPanel6.Controls[3 + 8 * ((int)((index - 4) / 8))] as TextBox;
                    if (b.BackColor == Color.OldLace)
                    {
                        if (Tax_park.IsTaxersDay(Tax_park.FindTaxer(txt.Text), days[index - 4 - 8 * ((int)((index - 4) / 8))]))
                        {
                            Tax_park.DelTaxerDay(Tax_park.FindTaxer(txt.Text), days[index - 4 - 8 * ((int)((index - 4) / 8))]);
                        }
                    }
                    else
                    {
                        if (!Tax_park.IsTaxersDay(Tax_park.FindTaxer(txt.Text), days[index - 4 - 8 * ((int)((index - 4) / 8))]))
                        {
                            Tax_park.AddTaxerDay(Tax_park.FindTaxer(txt.Text), days[index - 4 - 8 * ((int)((index - 4) / 8))]);
                        }
                    }
                }
            }

            List<Control> listControls2 = new List<Control>();
            foreach (Control control in flowLayoutPanel6.Controls)
            {
                listControls2.Add(control);
            }

            foreach (Control control in listControls2)
            {
                flowLayoutPanel6.Controls.Remove(control);
                control.Dispose();
            }


        }

        private void button4_Click(object sender, EventArgs e)
        {
            TextBox txtbox = CreateMessageTextBox();
            txtbox.TextChanged -= textBox_TextChanged;
            txtbox.MinimumSize = new Size(310, 45);
            txtbox.Font = new Font(txtbox.Font.FontFamily, 14);
            txtbox.Size = new Size(0, 0);
            txtbox.Text = $"Editing the taxer's salary: ";
            flowLayoutPanel6.Controls.Add(txtbox);

            Button accept = new Button();
            accept.Size = new Size(60, 45);
            accept.BackColor = Color.Green;
            accept.Font = new Font(accept.Font, FontStyle.Bold);
            accept.Font = new Font(accept.Font.FontFamily, 8);
            accept.Text = "Save";
            accept.Click += accept3_Click;
            flowLayoutPanel6.Controls.Add(accept);

            Button decline = new Button();
            decline.Size = new Size(60, 45);
            decline.BackColor = Color.DeepPink;
            decline.Font = new Font(decline.Font, FontStyle.Bold);
            decline.Font = new Font(decline.Font.FontFamily, 12);
            decline.Text = "X";
            decline.Click += decline_Click;
            flowLayoutPanel6.Controls.Add(decline);

            TextBox taxer = CreateMessageTextBox();
            taxer.TextChanged -= textBox_TextChanged;
            taxer.MinimumSize = new Size(130, 45);
            taxer.Font = new Font(taxer.Font.FontFamily, 14);
            taxer.Size = new Size(0, 0);
            taxer.Text = $"Taxer";
            flowLayoutPanel6.Controls.Add(taxer);

            TextBox sal = CreateMessageTextBox();
            sal.TextChanged -= textBox_TextChanged;
            sal.MinimumSize = new Size(150, 45);
            sal.Font = new Font(sal.Font.FontFamily, 12);
            sal.Size = new Size(0, 0);
            sal.Text = $"Current salary";
            flowLayoutPanel6.Controls.Add(sal);

            TextBox salp = CreateMessageTextBox();
            salp.TextChanged -= textBox_TextChanged;
            salp.MinimumSize = new Size(80, 45);
            salp.Font = new Font(salp.Font.FontFamily, 10);
            salp.Size = new Size(0, 0);
            salp.Text = $"Salary per trip";
            flowLayoutPanel6.Controls.Add(salp);


            foreach (string n in Tax_park)
            {
                TextBox b = CreateScheduleBox(n);
                b.Size = new Size(130, 45);
                flowLayoutPanel6.Controls.Add(b);

                TextBox Sal = CreateScheduleBox(Tax_park.GetTaxerSalary(Tax_park.FindTaxer(n)).ToString());
                Sal.Size = new Size(150, 45);
                flowLayoutPanel6.Controls.Add(Sal);

                TextBox Salp = CreateScheduleBox(Tax_park.GetTaxerSalaryPerTrip(Tax_park.FindTaxer(n)).ToString());
                Salp.Enabled = true;
                Salp.TextChanged += Salp_TextChanged;
                Salp.Size = new Size(80, 45);
                flowLayoutPanel6.Controls.Add(Salp);
            }
        }

        private void Salp_TextChanged(object o, EventArgs args)
        {
            TextBox txt = o as TextBox;
            if (!txt.Text.All(t => Char.IsDigit(t)))
            {
                txt.BackColor = Color.Crimson;
            }
            else
            {
                txt.BackColor = Color.OldLace;
            }
        }

        private void accept3_Click(object o, EventArgs args)
        {

            List<Control> controls = new List<Control>();

            foreach (Control c in flowLayoutPanel6.Controls)
            {
                controls.Add(c);
            }
            if (controls.Any(x=>x.BackColor == Color.Crimson))
            {
                TextBox b2 = flowLayoutPanel6.Controls[0] as TextBox;
                b2.Text = "Salary must contain only integers!!!";
            }
            else
            {
                foreach(Control cont in controls)
                {
                    if (cont.Enabled && cont.BackColor == Color.OldLace)
                    {
                        TextBox b = cont as TextBox;
                        int index = flowLayoutPanel6.Controls.GetChildIndex(b);
                        Taxer curTax = Tax_park.FindTaxer(flowLayoutPanel6.Controls[3 + 3 * ((int)((index - 3) / 3))].Text);
                        Tax_park.SetTaxerSalaryPerTrip(curTax, Convert.ToInt32(b.Text));
                    }
                    
                }

                List<Control> listControls2 = new List<Control>();
                foreach (Control control in flowLayoutPanel6.Controls)
                {
                    listControls2.Add(control);
                }

                foreach (Control control in listControls2)
                {
                    flowLayoutPanel6.Controls.Remove(control);
                    control.Dispose();
                }

            }
        }
    }
}

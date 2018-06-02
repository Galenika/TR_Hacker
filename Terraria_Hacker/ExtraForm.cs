﻿/*
 * Created by SharpDevelop.
 * User: Qiu233
 * Date: 2016/7/28
 * Time: 9:27
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Reflection;
using System.Linq;

namespace Terraria_Hacker
{
    /// <summary>
    /// Description of ExtraForm.
    /// </summary>
    public partial class ExtraForm : Form
    {
        private Panel MainPanel;
        private Hashtable hacks;
        private int ControlID = 0;
        public SpecialForm specialForm = null;
        public static Form Window;

        public ExtraForm()
        {
            Window = this;
			BackColor = Color.LightGray;
            hacks = new Hashtable();
            InitializeComponent();
            InitControls();
        }
        unsafe private void InitControls()
        {
            Label healtip = new Label()
            {
                Text = Lang.maxLife,
                Location = new Point(0, 5),
                Size = new Size(80, 20)
            };
            this.Controls.Add(healtip);

            TextBox heal = new TextBox()
            {
                Location = new Point(80, 0),
                Size = new Size(100, 20),
                Text = HackFunctions.getMaxHealth() + ""
            };
            heal.KeyPress += delegate (object sender, KeyPressEventArgs e)
             {
                 if (!Char.IsNumber(e.KeyChar) && e.KeyChar != 8 && e.KeyChar != '-')
                 {
                     e.Handled = true;
                 }
             };
            this.Controls.Add(heal);

            Label manatip = new Label()
            {
                Text = Lang.maxMana,
                Location = new Point(0, 25),
                Size = new Size(80, 20)
            };
            this.Controls.Add(manatip);

            TextBox mana = new TextBox()
            {
                Location = new Point(80, 20),
                Size = new Size(100, 20),
                Text = HackFunctions.getMaxMana() + ""
            };
            mana.KeyPress += delegate (object sender, KeyPressEventArgs e)
             {
                 if (!Char.IsNumber(e.KeyChar) && e.KeyChar != 8 && e.KeyChar != '-')
                 {
                     e.Handled = true;
                 }
             };
            this.Controls.Add(mana);

            Button healok = new Button()
            {
                Text = Lang.confirm,
                Location = new Point(185, 0),
                Size = new Size(70, 40)
            };
            healok.Click += delegate (object sender, EventArgs e)
             {
                 if (heal.Text.Trim() != "")
                 {
                     HackFunctions.setMaxHealth(Convert.ToInt32(heal.Text));
                     HackFunctions.setMaxMana(Convert.ToInt32(mana.Text));
                 }
             };
            this.Controls.Add(healok);

            MainPanel = new Panel()
            {
                Size = new Size(260, 450),
                Location = new Point(0, 50)
            };
            this.Controls.Add(MainPanel);


            Button itemAdd = AddButton(Lang.addItem, delegate (object sender, EventArgs e)
            {
                Form f = new Form();
                TextBox ItemID = new TextBox();
                TextBox ItemCount = new TextBox();
                ComboBox prefix = new ComboBox();
                Button et = new Button();

                f.Text = Lang.addItem;
                f.StartPosition = FormStartPosition.CenterParent;
                f.FormBorderStyle = FormBorderStyle.FixedSingle;
                f.MaximizeBox = false;
                f.MinimizeBox = false;
                f.Size = new Size(250, 90);

                Label tip1 = new Label()
                {
                    Text = "ItemID",
                    Location = new Point(0, 5),
                    Size = new Size(80, 20)
                };
                f.Controls.Add(tip1);

                ItemID.Location = new Point(85, 0);
                ItemID.Size = new Size(95, 20);
                ItemID.Text = "0";
                ItemID.KeyPress += delegate (object sender1, KeyPressEventArgs e1)
                {
                    if (!Char.IsNumber(e1.KeyChar) && e1.KeyChar != 8)
                    {
                        e1.Handled = true;
                    }
                };
                f.Controls.Add(ItemID);


                Label tip2 = new Label()
                {
                    Text = "ItemStack",
                    Location = new Point(0, 25),
                    Size = new Size(80, 20)
                };
                f.Controls.Add(tip2);

                ItemCount.Location = new Point(85, 20);
                ItemCount.Size = new Size(95, 20);
                ItemCount.Text = "1";
                ItemCount.KeyPress += delegate (object sender1, KeyPressEventArgs e1)
                {
                    if (!Char.IsNumber(e1.KeyChar) && e1.KeyChar != 8)
                    {
                        e1.Handled = true;
                    }
                };
                f.Controls.Add(ItemCount);

                prefix.Location = new Point(85, 40);
                prefix.Size = new Size(95, 20);
                prefix.DropDownStyle = ComboBoxStyle.DropDownList;
                prefix.DropDownHeight = 150;
                foreach (var o in MainForm.resource.Prefix)
                {
                    string[] t = o.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                    string v = t[0];
                    prefix.Items.Add(v);
                }
                prefix.SelectedIndex = 0;

                f.Controls.Add(prefix);

                Label tip3 = new Label()
                {
                    Text = "ItemPrefix",
                    Location = new Point(0, 45),
                    Size = new Size(80, 20)
                };
                f.Controls.Add(tip3);


                et.Text = Lang.confirm;
                et.Size = new Size(65, 60);
                et.Location = new Point(180, 0);
                et.Click += delegate (object sender1, EventArgs e1)
                {
					HackFunctions.AddItem(Convert.ToInt32(ItemID.Text), Convert.ToInt32(ItemCount.Text), InvEditor.GetPrefixFromIndex(prefix.SelectedIndex));
                    f.Dispose();
                };
                f.Controls.Add(et);
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog(this);
            }
            );
            itemAdd.Size = new Size(255, 30);
            itemAdd.Location = new Point(0, 60);

            Button itemHack = AddButton(Lang.hackInv, delegate (object sender, EventArgs e)
            {
                InvEditor ie = new InvEditor();
                ie.Show();
            }
            );
            itemHack.Size = new Size(255, 30);
            itemHack.Location = new Point(0, 30);


            Button addBuff = AddButton(Lang.addBuff, delegate (object sender, EventArgs e)
                                        {
                                            Form f = new Form();
                                            TextBox BuffID = new TextBox();
                                            TextBox BuffTime = new TextBox();
                                            Button et = new Button();

                                            f.Text = Lang.addBuffWnd;
                                            f.StartPosition = FormStartPosition.CenterParent;
                                            f.FormBorderStyle = FormBorderStyle.FixedSingle;
                                            f.MaximizeBox = false;
                                            f.MinimizeBox = false;
                                            f.Size = new Size(250, 90);

                                            Label tip1 = new Label()
                                            {
                                                Text = Lang.wndBuffID,
                                                Location = new Point(0, 25),
                                                Size = new Size(80, 20)
                                            };
                                            f.Controls.Add(tip1);

                                            BuffID.Location = new Point(85, 20);
                                            BuffID.Size = new Size(95, 20);
                                            BuffID.KeyPress += delegate (object sender1, KeyPressEventArgs e1)
                                          {
                                              if (!Char.IsNumber(e1.KeyChar) && e1.KeyChar != 8 && e1.KeyChar != '-')
                                              {
                                                  e1.Handled = true;
                                              }
                                          };
                                            f.Controls.Add(BuffID);


                                            Label tip2 = new Label()
                                            {
                                                Text = Lang.wndBuffTime,
                                                Location = new Point(0, 45),
                                                Size = new Size(80, 20)
                                            };
                                            f.Controls.Add(tip2);

                                            BuffTime.Location = new Point(85, 40);
                                            BuffTime.Size = new Size(95, 20);
                                            BuffTime.KeyPress += delegate (object sender1, KeyPressEventArgs e1)
                                          {
                                              if (!Char.IsNumber(e1.KeyChar) && e1.KeyChar != 8 && e1.KeyChar != '-')
                                              {
                                                  e1.Handled = true;
                                              }
                                          };
                                            f.Controls.Add(BuffTime);

                                            et.Text = Lang.confirm;
                                            et.Size = new Size(65, 40);
                                            et.Location = new Point(180, 20);
                                            et.Click += delegate (object sender1, EventArgs e1)
                                          {
                                              HackFunctions.AddBuff(Convert.ToInt32(BuffID.Text), Convert.ToInt32(BuffTime.Text));
                                              f.Dispose();
                                          };
                                            f.Controls.Add(et);
                                            f.StartPosition = FormStartPosition.CenterParent;
                                            f.ShowDialog(this);
                                        }
                                    );
            addBuff.Location = new Point(0, 0);
            addBuff.Size = new Size(85, 30);

            Button addPet = AddButton(Lang.addPet, delegate (object sender, EventArgs e)
                                       {
                                           Form f = new Form();
                                           Button et = new Button();

                                           f.Text = Lang.addPetWnd;
                                           f.StartPosition = FormStartPosition.CenterParent;
                                           f.FormBorderStyle = FormBorderStyle.FixedSingle;
                                           f.MaximizeBox = false;
                                           f.MinimizeBox = false;
                                           f.Size = new Size(250, 70);


                                           Label tip = new Label()
                                           {
                                               Text = Lang.addPetWnd,
                                               Size = new Size(55, 20),
                                               Location = new Point(0, 25)
                                           };
                                           f.Controls.Add(tip);

                                           ComboBox pet = new ComboBox()
                                           {
                                               DropDownStyle = ComboBoxStyle.DropDownList,
                                               DropDownHeight = 150
                                           };
                                           foreach (var o in MainForm.resource.Pets)
                                           {
                                               string[] t = o.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                                               string v = t[0];
                                               pet.Items.Add(v);
                                           }
                                           pet.SelectedIndex = 0;
                                           pet.Location = new Point(60, 20);
                                           pet.Size = new Size(120, 20);
                                           f.Controls.Add(pet);

                                           et.Text = Lang.confirm;
                                           et.Size = new Size(65, 20);
                                           et.Location = new Point(180, 20);
                                           et.Click += delegate (object sender1, EventArgs e1)
                                         {
                                             HackFunctions.AddBuff(GetPetFromIndex(pet.SelectedIndex), 18000);
                                             f.Dispose();
                                         };
                                           f.Controls.Add(et);
                                           f.StartPosition = FormStartPosition.CenterParent;
                                           f.ShowDialog(this);
                                       }
                                   );
            addPet.Location = new Point(85, 0);
            addPet.Size = new Size(85, 30);

            Button setMount = AddButton(Lang.setMount, delegate (object sender, EventArgs e)
                                         {
                                             Form f = new Form();
                                             Button et = new Button();

                                             f.Text = Lang.setMountWnd;
                                             f.StartPosition = FormStartPosition.CenterParent;
                                             f.FormBorderStyle = FormBorderStyle.FixedSingle;
                                             f.MaximizeBox = false;
                                             f.MinimizeBox = false;
                                             f.Size = new Size(250, 70);



                                             Label tip = new Label()
                                             {
                                                 Text = Lang.setMountWnd,
                                                 Size = new Size(55, 20),
                                                 Location = new Point(0, 25)
                                             };
                                             f.Controls.Add(tip);

                                             ComboBox mount = new ComboBox()
                                             {
                                                 DropDownStyle = ComboBoxStyle.DropDownList,
                                                 DropDownHeight = 150
                                             };
                                             foreach (var o in MainForm.resource.Mounts)
                                             {
                                                 string[] t = o.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                                                 string v = t[0];
                                                 mount.Items.Add(v);
                                             }
                                             mount.SelectedIndex = 0;
                                             mount.Location = new Point(60, 20);
                                             mount.Size = new Size(120, 20);
                                             f.Controls.Add(mount);

                                             et.Text = Lang.confirm;
                                             et.Size = new Size(65, 20);
                                             et.Location = new Point(180, 20);
                                             et.Click += delegate (object sender1, EventArgs e1)
                                           {
                                               HackFunctions.AddBuff(GetMountFromIndex(mount.SelectedIndex), 18000);
                                               f.Dispose();
                                           };
                                             f.Controls.Add(et);
                                             f.StartPosition = FormStartPosition.CenterParent;
                                             f.ShowDialog(this);
                                         }
                                     );
            setMount.Location = new Point(170, 0);
            setMount.Size = new Size(85, 30);

            Button NewNPC = AddButton(Lang.newNpc, delegate (object sender, EventArgs e)
            {
                Form f = new Form();
                TextBox NPCType = new TextBox();
                TextBox Times = new TextBox();
                Button et = new Button();

                f.Text = Lang.newNpc;
                f.StartPosition = FormStartPosition.CenterParent;
                f.FormBorderStyle = FormBorderStyle.FixedSingle;
                f.MaximizeBox = false;
                f.MinimizeBox = false;
                f.Size = new Size(250, 70);

                Label tip1 = new Label()
                {
                    Text = "NPC ID",
                    Location = new Point(0, 5),
                    Size = new Size(80, 20)
                };
                f.Controls.Add(tip1);

                NPCType.Location = new Point(85, 0);
                NPCType.Size = new Size(95, 20);
                NPCType.KeyPress += delegate (object sender1, KeyPressEventArgs e1)
                {
                    if (!Char.IsNumber(e1.KeyChar) && e1.KeyChar != 8 && e1.KeyChar != '-')
                    {
                        e1.Handled = true;
                    }
                };
                f.Controls.Add(NPCType);


                Label tip2 = new Label()
                {
                    Text = Lang.number,
                    Location = new Point(0, 25),
                    Size = new Size(80, 20)
                };
                f.Controls.Add(tip2);

                Times.Location = new Point(85, 20);
                Times.Size = new Size(95, 20);
                Times.KeyPress += delegate (object sender1, KeyPressEventArgs e1)
                {
                    if (!Char.IsNumber(e1.KeyChar) && e1.KeyChar != 8 && e1.KeyChar != '-')
                    {
                        e1.Handled = true;
                    }
                };
                f.Controls.Add(Times);

                et.Text = Lang.confirm;
                et.Size = new Size(65, 40);
                et.Location = new Point(180, 0);
                et.Click += delegate (object sender1, EventArgs e1)
                {
                    HackFunctions.NewNPC(HackFunctions.getX(), HackFunctions.getY(), Convert.ToInt32(NPCType.Text), Convert.ToInt32(Times.Text));
                    f.Dispose();
                };
                f.Controls.Add(et);
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog(this);
            }
                                   );
            NewNPC.Location = new Point(0, 90);
            NewNPC.Size = new Size(255, 30);

            Button special = AddButton(Lang.more, delegate (object sender, EventArgs e)
                                        {
                                            if (specialForm == null)
                                            {
                                                specialForm = new SpecialForm();
                                                specialForm.Show(this);
                                                specialForm.Location = new Point(Location.X + Width, Location.Y);
                                                ((Button)sender).Font = new Font("Arial", 8, FontStyle.Bold);
                                            }
                                            else
                                            {
                                                specialForm.Dispose();
                                                specialForm = null;
                                                ((Button)sender).Font = new Font("Arial", 8);
                                            }
                                        }
                                    );
            special.Font = new Font("Arial", 8);
            special.ForeColor = Color.Red;
            special.Location = new Point(0, 120);
            special.Size = new Size(255, 30);

            //slot.Text = "1";


        }
        private int GetMountFromIndex(int id)
        {
            return Convert.ToInt32(MainForm.resource.Mounts[id].Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries)[1]);
        }
        private int GetPetFromIndex(int id)
        {
            return Convert.ToInt32(MainForm.resource.Pets[id].Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries)[1]);
        }

        private Button AddButton(string tipstr, EventHandler handler)
        {
            int a = ControlID % 2, b = (int)Math.Floor((double)ControlID / 2);
            Button button = new Button()
            {
                Text = tipstr
            };
            button.Click += handler;
            MainPanel.Controls.Add(button);
            return button;
        }

    }
}

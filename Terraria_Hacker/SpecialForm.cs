﻿/*
 * Created by SharpDevelop.
 * User: Qiu233
 * Date: 2016/7/28
 * Time: 17:26
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Threading;
using System.Runtime.InteropServices;

namespace Terraria_Hacker
{
	/// <summary>
	/// Description of SpecialForm.
	/// </summary>
	public partial class SpecialForm : Form
	{
		private AntiBlinkListView warpList, playerList;
		public Form Window;
		const int WM_NCLBUTTONDOWN = 0x00A1;
		const int HTCAPTION = 2;
		public SpecialForm()
		{
			Window = this;
			BackColor = Color.LightGray;
			HackFunctions.WarpLoad();
			InitializeComponent();
			InitControls();
		}
		public void UpdateWarpList()
		{
			warpList.Items.Clear();
			foreach (DictionaryEntry de in HackFunctions.warps)
			{
				string name = (string)de.Key;
				string dec = ((Position)de.Value).Dec;
				warpList.Items.Add(name, name, 0);
				warpList.Items[name].SubItems.Add(dec);
			}
		}
		private void InitControls()
		{
			{
				warpList = new AntiBlinkListView()
				{
					Location = new Point(0, 0),
					Size = new Size(200, 180),
					View = View.Details,
					MultiSelect = false,
					FullRowSelect = true,
			};
				warpList.BeginUpdate();
				warpList.Columns.Add(Lang.telePoint, warpList.Size.Width / 3);
				warpList.Columns.Add(Lang.descr, warpList.Size.Width / 3 * 2);
				foreach (DictionaryEntry de in HackFunctions.warps)
				{
					string name = (string)de.Key;
					string dec = ((Position)de.Value).Dec;
					warpList.Items.Add(name, name, 0);
					warpList.Items[name].SubItems.Add(dec);
				}
				warpList.EndUpdate();
				this.Controls.Add(warpList);

				Button warp = new Button();
				warp.Text = Lang.teleport;
				warp.Click += delegate (object sender, EventArgs e)
				{
					if (warpList.SelectedItems.Count == 0)
						return;
					if (MessageBox.Show(Lang.teleMessage + "\"" + warpList.SelectedItems[0].Text + "\"？", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
					{
						HackFunctions.Warp(warpList.SelectedItems[0].Text);
					}
				};
				warp.Location = new Point(205, 0);
				warp.Size = new Size(85, 30);
				this.Controls.Add(warp);

				Button warpadd = new Button();
				warpadd.Text = Lang.addText;
				warpadd.Click += delegate (object sender, EventArgs e)
				{
					Form f = new Form();
					TextBox name = new TextBox();
					TextBox dec = new TextBox();
					Button ok = new Button();
					f.Text = Lang.addText;
					f.StartPosition = FormStartPosition.CenterParent;
					f.FormBorderStyle = FormBorderStyle.FixedSingle;
					f.MaximizeBox = false;
					f.MinimizeBox = false;
					f.Size = new Size(220, 70);

					Label tip1 = new Label();
					tip1.Text = Lang.nameText;
					tip1.Location = new Point(0, 5);
					tip1.Size = new Size(30, 20);
					f.Controls.Add(tip1);

					name.Location = new Point(35, 0);
					name.Size = new Size(120, 20);
					name.KeyPress += delegate (object sender1, KeyPressEventArgs e1)
					{
						if (e1.KeyChar == ' ')
							e1.Handled = true;
					};
					f.Controls.Add(name);


					Label tip2 = new Label();
					tip2.Text = Lang.descr;
					tip2.Location = new Point(0, 25);
					tip2.Size = new Size(30, 20);
					f.Controls.Add(tip2);

					dec.Location = new Point(35, 20);
					dec.Size = new Size(120, 20);
					dec.KeyPress += delegate (object sender1, KeyPressEventArgs e1)
					{
						if (e1.KeyChar == ' ')
							e1.Handled = true;
					};
					f.Controls.Add(dec);

					ok.Text = Lang.confirm;
					ok.Size = new Size(60, 40);
					ok.Location = new Point(155, 0);
					ok.Click += delegate (object sender1, EventArgs e1)
					{
						if (!HackFunctions.warps.Contains(name.Text))
						{
							AddWarp(name.Text, dec.Text);
							HackFunctions.WarpSave();
							f.Dispose();
						}
						else
						{
							MessageBox.Show(Lang.telePoint + "\"" + name.Text + "\"" + Lang.exists);
						}
					};
					f.Controls.Add(ok);
					f.ShowDialog(this);
				};
				warpadd.Location = new Point(205, 30);
				warpadd.Size = new Size(85, 30);
				this.Controls.Add(warpadd);

				Button warpdel = new Button();
				warpdel.Text = Lang.delete;
				warpdel.Click += delegate (object sender, EventArgs e)
				{
					if (warpList.SelectedItems.Count == 0)
						return;
					if (MessageBox.Show(Lang.deleteMessage + "\"" + warpList.SelectedItems[0].Text + "\"？", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
					{
						DelWarp();
					}
				};
				warpdel.Location = new Point(205, 60);
				warpdel.Size = new Size(85, 30);
				this.Controls.Add(warpdel);

				Button warprename = new Button();
				warprename.Text = Lang.rename;
				warprename.Click += delegate (object sender, EventArgs e)
				{
					if (warpList.SelectedItems.Count == 0)
						return;
					Form f = new Form();
					TextBox name = new TextBox();
					TextBox dec = new TextBox();
					Button ok = new Button();
					f.Text = Lang.rename;
					f.StartPosition = FormStartPosition.CenterParent;
					f.FormBorderStyle = FormBorderStyle.FixedSingle;
					f.MaximizeBox = false;
					f.MinimizeBox = false;
					f.Size = new Size(220, 50);

					Label tip1 = new Label();
					tip1.Text = Lang.newName;
					tip1.Location = new Point(0, 5);
					tip1.Size = new Size(50, 20);
					f.Controls.Add(tip1);

					name.Location = new Point(55, 0);
					name.Size = new Size(100, 20);
					name.KeyPress += delegate (object sender1, KeyPressEventArgs e1)
					{
						if (e1.KeyChar == ' ')
							e1.Handled = true;

					};
					f.Controls.Add(name);


					ok.Text = Lang.confirm;
					ok.Size = new Size(60, 20);
					ok.Location = new Point(155, 0);
					ok.Click += delegate (object sender1, EventArgs e1)
					{
						if (warpList.SelectedItems.Count == 0)
							return;
						Position p = (Position)HackFunctions.warps[warpList.SelectedItems[0].Text];
						DelWarp();
						HackFunctions.warps.Add(name.Text, p);
						warpList.Items.Add(name.Text, name.Text, 0);
						warpList.Items[name.Text].SubItems.Add(p.Dec);
						HackFunctions.WarpSave();
						f.Dispose();
					};
					f.Controls.Add(ok);
					f.ShowDialog(this);
				};
				warprename.Location = new Point(205, 90);
				warprename.Size = new Size(85, 30);
				this.Controls.Add(warprename);

				Button warpFresh = new Button();
				warpFresh.Text = Lang.refresh;
				warpFresh.Click += delegate (object sender, EventArgs e)
				{
					UpdateWarpList();
				};
				warpFresh.Location = new Point(205, 120);
				warpFresh.Size = new Size(85, 30);
				this.Controls.Add(warpFresh);
			}
			{
				Label tipX = new Label();
				tipX.Text = "X:";
				tipX.Size = new Size(20, 20);
				tipX.Location = new Point(5, 200);
				this.Controls.Add(tipX);

				TextBox X = new TextBox();
				X.Size = new Size(70, 20);
				X.Location = new Point(30, 195);
				X.KeyPress += delegate (object sender, KeyPressEventArgs e)
				{
					if (!Char.IsNumber(e.KeyChar) && e.KeyChar != 8 && e.KeyChar != '-')
					{
						e.Handled = true;
					}
					if (e.KeyChar == '.')
						e.Handled = false;
				};
				this.Controls.Add(X);

				Label tipY = new Label();
				tipY.Text = "Y:";
				tipY.Size = new Size(20, 20);
				tipY.Location = new Point(110, 200);
				this.Controls.Add(tipY);

				TextBox Y = new TextBox();
				Y.Size = new Size(70, 20);
				Y.Location = new Point(135, 195);
				Y.KeyPress += delegate (object sender, KeyPressEventArgs e)
				{
					if (!Char.IsNumber(e.KeyChar) && e.KeyChar != 8 && e.KeyChar != '-')
					{
						e.Handled = true;
					}
					if (e.KeyChar == '.')
						e.Handled = false;
				};
				this.Controls.Add(Y);

				Button ok = new Button();
				ok.Text = Lang.teleport;
				ok.Size = new Size(60, 30);
				ok.Location = new Point(225, 190);
				ok.Click += delegate (object sender, EventArgs e)
				{
					HackFunctions.setX((float)Convert.ToDouble(X.Text));
					HackFunctions.setY((float)Convert.ToDouble(Y.Text));
				};
				this.Controls.Add(ok);
			}
			{
				{
					playerList = new AntiBlinkListView();
					playerList.Location = new Point(0, 260);
					playerList.Size = new Size(295, 200);
					playerList.View = View.Details;
					playerList.MultiSelect = false;
					playerList.FullRowSelect = true;
					ContextMenuStrip cms = new ContextMenuStrip();
					cms.Items.Add(Lang.teleport);
					cms.Items.Add(Lang.convertTo);
					cms.Items.Add(Lang.checkInv);
					cms.Items.Add(Lang.checkBuff);
					cms.ItemClicked += delegate (object sender, ToolStripItemClickedEventArgs e)
					{
						if (playerList.SelectedItems.Count > 0)
						{
							if (e.ClickedItem.Text == Lang.teleport)
							{
								ContextMenuStrip s = sender as ContextMenuStrip;
								ListView l = (ListView)s.SourceControl;
								TPToPlayer((float)Convert.ToDouble(l.SelectedItems[0].SubItems[2].Text), (float)Convert.ToDouble(l.SelectedItems[0].SubItems[3].Text));
							}
							else if (e.ClickedItem.Text == Lang.convertTo)
							{
								ContextMenuStrip s = sender as ContextMenuStrip;
								ListView l = (ListView)s.SourceControl;
								HackFunctions.setMyPlayer(Convert.ToInt32(l.SelectedItems[0].SubItems[0].Text));
							}
							else if (e.ClickedItem.Text == Lang.checkInv)
							{
								ContextMenuStrip s = sender as ContextMenuStrip;
								ListView l = (ListView)s.SourceControl;
								new PlayerInventory(Convert.ToInt32(playerList.SelectedItems[0].SubItems[0].Text), l.SelectedItems[0].SubItems[1].Text).Show();
							}
							else if (e.ClickedItem.Text == Lang.checkBuff)
							{
								ContextMenuStrip s = sender as ContextMenuStrip;
								ListView l = (ListView)s.SourceControl;
								new PlayerDetail_Buff(Convert.ToInt32(playerList.SelectedItems[0].SubItems[0].Text), l.SelectedItems[0].SubItems[1].Text).Show();
							}
						}
					};
					playerList.ContextMenuStrip = cms;
					playerList.BeginUpdate();
					playerList.Columns.Add("ID", playerList.Size.Width / 6);
					playerList.Columns.Add(Lang.player, playerList.Size.Width / 4);
					playerList.Columns.Add("X", playerList.Size.Width / 4);
					playerList.Columns.Add("Y", playerList.Size.Width / 4);
					playerList.Columns.Add(Lang.currentLife, playerList.Size.Width / 4);
					playerList.Columns.Add(Lang.maxLife, playerList.Size.Width / 4);
					System.Windows.Forms.Timer updateTimer = new System.Windows.Forms.Timer();
					updateTimer.Interval = 100;
					updateTimer.Tick += (sender, e) =>
					{
						UpdatePlayerList();
					};
					updateTimer.Start();
					playerList.EndUpdate();
					playerList.DoubleClick += delegate (object sender, EventArgs e)
					{
						TPToPlayer((float)Convert.ToDouble(playerList.SelectedItems[0].SubItems[2].Text), (float)Convert.ToDouble(playerList.SelectedItems[0].SubItems[3].Text));
					};
					this.Controls.Add(playerList);
				}
			}

		}
		public static void TPToPlayer(float X, float Y)
		{
			HackFunctions.setX(X);
			HackFunctions.setY(Y);
		}
		private void UpdatePlayerList()
		{
			int i = 0;
			for (; i < 50; i++)
			{
				if (HackFunctions.getPlayerActive(i))
				{
					if (playerList.Items.ContainsKey(i.ToString()))
					{
						float playerX = HackFunctions.getPlayerX(i);
						float playerY = HackFunctions.getPlayerY(i);
						int health = HackFunctions.getPlayerHealth(i);
						int maxHealth = HackFunctions.getPlayerMaxHealth(i);
						string name = GetPlayerName(i);
						int index = playerList.Items.IndexOfKey(i.ToString());
						playerList.Items[index].SubItems[1].Text = name;
						playerList.Items[index].SubItems[2].Text = Convert.ToString(playerX);
						playerList.Items[index].SubItems[3].Text = Convert.ToString(playerY);
						playerList.Items[index].SubItems[4].Text = Convert.ToString(health);
						playerList.Items[index].SubItems[5].Text = Convert.ToString(maxHealth);
					}
					else
					{
						float playerX = HackFunctions.getPlayerX(i);
						float playerY = HackFunctions.getPlayerY(i);
						int health = HackFunctions.getPlayerHealth(i);
						int maxHealth = HackFunctions.getPlayerMaxHealth(i);
						string name = GetPlayerName(i);
						playerList.Items.Add(i.ToString(), i.ToString(), 0);
						playerList.Items[i.ToString()].SubItems.Add(name);
						playerList.Items[i.ToString()].SubItems.Add(Convert.ToString(playerX));
						playerList.Items[i.ToString()].SubItems.Add(Convert.ToString(playerY));
						playerList.Items[i.ToString()].SubItems.Add(Convert.ToString(health));
						playerList.Items[i.ToString()].SubItems.Add(Convert.ToString(maxHealth));
					}
				}
				else
				{
					if(playerList.Items.ContainsKey(i.ToString()))
					{
						playerList.Items[playerList.Items.IndexOfKey(i.ToString())].Remove();
					}
				}
			}
		}
		unsafe private static string GetPlayerName(int id)
		{
			int len = 0;
			char* str = HackFunctions.getPlayerName(id, &len);
			if (len == 0)
				return "";
			string rst = "";
			int i = 0;
			while (i < len)
			{
				rst += *(str + i);
				i++;
			}
			//MainForm.FreeMemory((int)str);
			Marshal.FreeHGlobal((IntPtr)str);
			return rst;
		}
		private void DelWarp()
		{
			HackFunctions.WarpDel(warpList.SelectedItems[0].Text);
			HackFunctions.WarpSave();
			warpList.Items.Remove(warpList.SelectedItems[0]);
		}
		private void AddWarp(string name, string dec)
		{
			if (dec.Trim() == "")
			{
				dec = Lang.none;
			}
			HackFunctions.WarpAdd(name, dec);
			HackFunctions.WarpSave();
			warpList.Items.Add(name, name, 0);
			warpList.Items[name].SubItems.Add(dec);
		}
	}
}

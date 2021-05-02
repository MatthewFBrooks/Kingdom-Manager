using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Kingdom_Manager
{
    class TroopLevy : IComparable
    {
        public Panel TroopPanel;
        private int troopNum;
        public TextBox TroopCountBox;
        public int TroopNum {
            get
            {
                return troopNum;
            }
            set
            {
                if (value >= 0)
                {
                    troopNum = value;
                }
                if (TroopCountBox != null)
                {
                    TroopCountBox.Text = TroopNum.ToString();
                    Program.UpdateTotalTroopsText();
                }
            }
        }

        public string County { get; set; }
        public string UUID { get; set; }

        //For now, food is just measured in "Person Months", but if some other measure was used this would be able to be changed to calculate the right amount.
        public int FoodUsage { get { return TroopNum; } }

        public TroopLevy(string County, int TroopNum)
        {
            this.County = County;
            this.TroopNum = TroopNum;
            TroopPanel = new Panel() { Width = Program.MERC_BOX_WIDTH, Height = 55, Name = County, BorderStyle = BorderStyle.Fixed3D, BackColor = Color.LightGreen};
            TroopCountBox = new TextBox() { Location = new Point(75, 20 + Program.DEFAULT_PADDING), Text = TroopNum.ToString(), BorderStyle = BorderStyle.FixedSingle };
            TroopCountBox.Validating += new System.ComponentModel.CancelEventHandler(this.TroopNumTextValidate);
            TroopPanel.Controls.Add(new Label() { Location = new Point(Program.DEFAULT_PADDING, 0), Text = "Peasant levy from: " + County, TextAlign = System.Drawing.ContentAlignment.MiddleLeft, Width = Program.MERC_BOX_WIDTH - Program.DEFAULT_PADDING * 2 });
            TroopPanel.Controls.Add(new Label() { Location = new Point(Program.DEFAULT_PADDING, 20 + Program.DEFAULT_PADDING), Text = "Levy:", TextAlign = System.Drawing.ContentAlignment.MiddleLeft, Width = 70 });
            TroopPanel.Controls.Add(TroopCountBox);
        }

        private void TroopNumTextValidate(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (int.TryParse(TroopCountBox.Text, out _))
            {
                if (int.Parse(TroopCountBox.Text) >= 0)
                {
                    TroopNum = int.Parse(TroopCountBox.Text);
                    Program.updateTroop(UUID);
                    Program.UpdateResourceGraph(Program.FOOD_INDEX);
                } else
                {
                    MessageBox.Show("Error:\nTroop count cannot be negative.");
                    e.Cancel = true;
                }
                
            } else
            {
                MessageBox.Show("Error:\nEntered text does not appear to be numeric.");
                e.Cancel = true;
            }
        }

        public int CompareTo(object obj)
        {
            if (obj is TroopLevy)
            {
                return TroopNum.CompareTo(((TroopLevy)obj).TroopNum);
            }
            else
            {
                throw new InvalidCastException("Not a valid TroopLevy object.");
            }
        }

        public override string ToString()
        {
            return this.GetType().Name + ", County: " + this.County + ", Troops: " + this.TroopNum;
        }
    }

    class Mercenary : TroopLevy
    {
        private int goldCost;
        public TextBox TroopGoldBox;
        public int GoldCost
        {
            get
            {
                return goldCost;
            }
            set
            {
                if (value >= 0)
                {
                    goldCost = value;
                }
                if (TroopGoldBox != null)
                {
                    TroopGoldBox.Text = goldCost.ToString();
                }
            }
        }

        public Mercenary(string County, int TroopNum, int GoldCost) : base (County, TroopNum)
        {
            this.GoldCost = GoldCost;
            TroopPanel.BackColor = Color.Gold;
            TroopPanel.Controls[0].Text = "Mercenaries garrisoned in: " + County;
            TroopPanel.Height += 20 + Program.DEFAULT_PADDING;
            TroopPanel.Controls[1].Text = "Troop count:";
            TroopPanel.Controls.Add(new Label() { Location = new Point(5, 55), Text = "Gold Cost: " + this.GoldCost, TextAlign = System.Drawing.ContentAlignment.MiddleLeft, Width = 70 });
            TroopGoldBox = new TextBox() { Location = new Point(70 + Program.DEFAULT_PADDING, 50 + Program.DEFAULT_PADDING), Text = GoldCost.ToString(), BorderStyle = BorderStyle.FixedSingle };
            TroopGoldBox.Validating += new System.ComponentModel.CancelEventHandler(this.TroopGoldTextValidate);
            TroopPanel.Controls.Add(TroopGoldBox);
        }

        private void TroopGoldTextValidate(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (int.TryParse(TroopGoldBox.Text, out _))
            {
                if (int.Parse(TroopGoldBox.Text) >= 0)
                {
                    GoldCost = int.Parse(TroopGoldBox.Text);
                    Program.updateTroop(UUID);
                    Program.UpdateResourceGraph(Program.GOLD_INDEX);
                }
                else
                {
                    MessageBox.Show("Error:\nTroop cost cannot be negative.");
                    e.Cancel = true;
                }

            }
            else
            {
                MessageBox.Show("Error:\nEntered text does not appear to be numeric.");
                e.Cancel = true;
            }
        }

        public override string ToString()
        {
            return base.ToString() + ", Gold cost: " + this.GoldCost;
        }
    }
}

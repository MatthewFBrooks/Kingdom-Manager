using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms.VisualStyles;
using System.Data.SQLite;
using System.Data;
using Dapper;

namespace Kingdom_Manager
{
    static class Program
    {
        const int WIDTH = 1280;
        const int HEIGHT = 720;
        public const int FOOD_INDEX = 0;
        public const int GOLD_INDEX = 1;
        public const int MONTHS = 12;
        const int AXISMAXIMUM = 100000;
        public const int DEFAULT_PADDING = 5;
        public const int MERC_BOX_WIDTH = 210;
        const int SCROLLBAR_PADDING = 20;
        const string CONNECTION_STRING = "Data Source=.\\KingdomManager.sqlite3;Version=3;";

        //Random fake counties named with namegenerator2.com/country-name-generator.php
        public static readonly string[] COUNTIES = new string[] {
                                        "Aelhollow",
                                        "Springley",
                                        "Wellfort",
                                        "Oldbeach",
                                        "Crystalport",
                                        "Pryden",
                                        "Silvermaple",
                                        "Woodbay",
                                        "Hedgelea",
                                        "Wellsummer",
                                        "Grasslyn",
                                        "Winterbarrow"};

        static Chart[] Charts;
        public static List<Year> Years;
        public static int SelectedYear;
        static int[][] resources;
        static int[][] projectedResources;
        static string[] ResourceName;
        static MonthDisplay[] MonthBoxes;
        static int TroopFoodUsage
        {
            get
            {
                int CumulativeNum = 0;
                for (int troop = 0; troop < Troops.Count; troop++)
                {
                    CumulativeNum += Troops[troop].TroopNum;
                }
                return CumulativeNum;
            }
        }

        static Label TotalTroopsLabel;
        static int TotalTroops
        {
            get
            {
                int TroopAmount = 0;
                if (Troops.Count >= 1)
                {
                    for (int i = 0; i < Troops.Count; i++)
                    {
                        TroopAmount += Troops[i].TroopNum;
                    }
                }
                return TroopAmount;
            }
        }

        static Panel TroopListPanel;
        static Random RandomNumber = new Random();
        static List<TroopLevy> Troops;
        static Label[] TroopResourceUsageLabel;
        static ComboBox troopRemoveComboBox;
        static Label troopRemoveLabel;
        static Button troopRemoveButton;
        static Button[] PreviousYearButton;
        static Button[] NextYearButton;
        static Label[] YearLabel;
        static int TroopResourceUsage(int resource)
        {
            int troopUsage = 0;
            if (resource == FOOD_INDEX)
            {

                if (Troops.Count >= 1)
                {
                    for (int i = 0; i < Troops.Count; i++)
                    {
                        troopUsage += Troops[i].FoodUsage;
                    }
                }
                //MessageBox.Show("" + troopUsage);
                return troopUsage;
            }
            else if (resource == GOLD_INDEX)
            {
                if (Troops.Count >= 1)
                {
                    for (int i = 0; i < Troops.Count; i++)
                    {
                        //MessageBox.Show(Troops[i].GetType().Name);
                        if (Troops[i] is Mercenary)
                        {
                            troopUsage += ((Mercenary)Troops[i]).GoldCost;
                        }
                    }
                }
                //MessageBox.Show("" + troopUsage);
                return troopUsage;
            }
            else
            {
                throw new System.IndexOutOfRangeException("Tried to index resource: " + resource);
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ResourceName = new string[] { "Food", "Gold" };



            Troops = getTroops();

            Color[] Colors = new Color[2] { Color.Blue, Color.Red };

            Form KingdomWindow = new Form();

            String[] TabNames = { "Food", "Gold", "Troops", "MilitaryStrength" };

            TabControl MainTabControl = new TabControl();
            MainTabControl.Location = new Point(DEFAULT_PADDING, DEFAULT_PADDING);
            MainTabControl.Size = new Size(WIDTH - 10, HEIGHT - 10);
            MainTabControl.TabIndex = 0;
            MainTabControl.SelectedIndex = 0;
            TabPage[] Tabs = new TabPage[TabNames.Length];

            MainTabControl.SuspendLayout();

            for (int i = 0; i < TabNames.Length - 1; i++)
            {
                Tabs[i] = new TabPage() { Name = TabNames[i] + "Tab", Text = TabNames[i] };
                MainTabControl.Controls.Add(Tabs[i]);
                Tabs[i].TabIndex = 0;
            }

            //MonthNames returns an array of length 13, because reasons. We only need or want 12, ever. So manually add months so "names" has the correct size.
            string[] monthNames = new string[MONTHS];
            MonthBoxes = new MonthDisplay[12];
            for (int i = 0; i < MONTHS; i++)
            {
                MonthBoxes[i] = new MonthDisplay() { MonthNum = i };
                monthNames[i] = System.Globalization.DateTimeFormatInfo.CurrentInfo.MonthNames[i];
            }

            //Initialize both charts and their controls
            Years = getYears();
            SelectedYear = Years.Count - 1;
            for (int year = 0; year < Years.Count; year++)
            {
                List<Month> tempMonths = getMonthsForYear(Years[year].UUID);
                if (tempMonths.Count == 12)
                {
                    Years[year].Months = tempMonths;
                }
                else
                {
                    for (int month = 0; month < tempMonths.Count; month++)
                    {
                        Years[year].Months[tempMonths[month].Num] = tempMonths[month];
                    }
                }
            }

            resources = new int[2][];
            projectedResources = new int[2][];
            for (int resource = 0; resource < 2; resource++)
            {
                resources[resource] = new int[MONTHS];
                projectedResources[resource] = new int[MONTHS];

            }
            Label[,] MonthLabels = new Label[2, MONTHS];

            int X = DEFAULT_PADDING;
            int Y = DEFAULT_PADDING;
            TroopResourceUsageLabel = new Label[2];
            string[] DataType = new string[] { "Actual", "Projected" };
            Charts = new Chart[2];
            Panel[] Panels = new Panel[2];
            PreviousYearButton = new Button[2];
            NextYearButton = new Button[2];
            Button[] AddPreviousYearButton = new Button[2];
            Button[] AddNextYearButton = new Button[2];
            YearLabel = new Label[2];
            int ChartWidth = WIDTH - 195;
            for (int ChartNum = 0; ChartNum < 2; ChartNum++)
            {
                //Labels and text boxes
                Panels[ChartNum] = new Panel() { Location = new Point(WIDTH - 195, 5), Name = ResourceName[ChartNum] + "MonthsPanel", Size = new Size(165, HEIGHT - 50), BorderStyle = BorderStyle.Fixed3D };
                Tabs[ChartNum].Controls.Add(Panels[ChartNum]);

                X = DEFAULT_PADDING;
                Y = DEFAULT_PADDING;

                TroopResourceUsageLabel[ChartNum] = new Label();
                TroopResourceUsageLabel[ChartNum].Width = 155;
                Panels[ChartNum].Controls.Add(TroopResourceUsageLabel[ChartNum]);
                Y += 20 + DEFAULT_PADDING;
                //Note: Couldn't seem to reuse the month labels in both tabs, as they would only show up in the second tab they were added to. Thus, I'm making identical labels for each list of months.
                for (int month = 0; month < MONTHS; month++)
                {
                    MonthLabels[ChartNum, month] = new Label() { Name = "Food" + monthNames[month] + "Label", Text = monthNames[month], Location = new Point(X, Y), Size = new Size(60, 20) };
                    MonthLabels[ChartNum, month].TabStop = false;
                    MonthLabels[ChartNum, month].TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                    MonthBoxes[month].IntakeBoxes[ChartNum].Name = "Food" + monthNames[month] + "Box";
                    MonthBoxes[month].IntakeBoxes[ChartNum].Location = new Point(X + 60 + DEFAULT_PADDING, Y);
                    MonthBoxes[month].IntakeBoxes[ChartNum].Size = new Size(80, 20);
                    MonthBoxes[month].IntakeBoxes[ChartNum].Text = Years[SelectedYear].Months[month][ChartNum].ToString();
                    Panels[ChartNum].Controls.Add(MonthLabels[ChartNum, month]);
                    Panels[ChartNum].Controls.Add(MonthBoxes[month].IntakeBoxes[ChartNum]);
                    Y += 20 + DEFAULT_PADDING;
                }

                int defaultButtonWidth = 75;
                int halfDefaultWidth = defaultButtonWidth / 2;
                int FourEqualPoints = ChartWidth / 5;
                AddPreviousYearButton[ChartNum] = new Button() { Text = "+", Location = new Point(FourEqualPoints * 1 - halfDefaultWidth, DEFAULT_PADDING) };
                AddPreviousYearButton[ChartNum].Click += new EventHandler(AddPreviousYear);
                PreviousYearButton[ChartNum] = new Button() { Text = "<", Location = new Point(FourEqualPoints * 2 - halfDefaultWidth, DEFAULT_PADDING) };
                PreviousYearButton[ChartNum].Click += new EventHandler(SelectPreviousYear);
                YearLabel[ChartNum] = new Label() { Text = "Year: " + Years[SelectedYear].Num, Location = new Point(ChartWidth / 2 - 50, DEFAULT_PADDING), TextAlign = System.Drawing.ContentAlignment.MiddleCenter };
                NextYearButton[ChartNum] = new Button() { Text = ">", Location = new Point(FourEqualPoints * 3 - halfDefaultWidth, DEFAULT_PADDING) };
                NextYearButton[ChartNum].Click += new EventHandler(SelectNextYear);
                AddNextYearButton[ChartNum] = new Button() { Text = "+", Location = new Point(FourEqualPoints * 4 - halfDefaultWidth, DEFAULT_PADDING) };
                AddNextYearButton[ChartNum].Click += new EventHandler(AddNextYear);
                Tabs[ChartNum].Controls.Add(AddPreviousYearButton[ChartNum]);
                Tabs[ChartNum].Controls.Add(PreviousYearButton[ChartNum]);
                Tabs[ChartNum].Controls.Add(YearLabel[ChartNum]);
                Tabs[ChartNum].Controls.Add(NextYearButton[ChartNum]);
                Tabs[ChartNum].Controls.Add(AddNextYearButton[ChartNum]);

                //Charts
                Charts[ChartNum] = new Chart();

                ((System.ComponentModel.ISupportInitialize)Charts[ChartNum]).BeginInit();

                Charts[ChartNum].ChartAreas.Add(new ChartArea());
                Charts[ChartNum].ChartAreas[0].Name = ResourceName[ChartNum] + "Area";
                Charts[ChartNum].ChartAreas[0].AxisX.Minimum = 1;
                Charts[ChartNum].ChartAreas[0].AxisX.Maximum = 12;
                Charts[ChartNum].ChartAreas[0].AxisY.Minimum = 0;
                Charts[ChartNum].ChartAreas[0].AxisY.Maximum = AXISMAXIMUM;
                Charts[ChartNum].Legends.Add(new Legend());
                Charts[ChartNum].Legends[0].Name = ResourceName[ChartNum] + "Legend";
                Charts[ChartNum].Location = new System.Drawing.Point(DEFAULT_PADDING, DEFAULT_PADDING * 2 + 20);
                Charts[ChartNum].Name = ResourceName[ChartNum] + "Chart";

                for (int SeriesNum = 0; SeriesNum < 2; SeriesNum++)
                {
                    Charts[ChartNum].Series.Add(new Series());
                    Charts[ChartNum].Series[SeriesNum].ChartType = SeriesChartType.Line;
                    Charts[ChartNum].Series[SeriesNum].LegendText = DataType[SeriesNum] + " " + ResourceName[ChartNum];
                    Charts[ChartNum].Series[SeriesNum].Color = Colors[SeriesNum];
                }

                Charts[ChartNum].Size = new System.Drawing.Size(WIDTH - 195, HEIGHT - 65);
                Charts[ChartNum].TabIndex = 0;
                Charts[ChartNum].Text = "Yearly " + ResourceName[ChartNum];
                Charts[ChartNum].ChartAreas[0].AxisX.IsMarginVisible = false;
                Charts[ChartNum].ChartAreas[0].AxisY.IsMarginVisible = false;

                //Technically, we probably shouldn't select the tab with ChartNum, but the food and gold tabs are 0 and 1, so it works, and simplifies things a lot.
                Tabs[ChartNum].Controls.Add(Charts[ChartNum]);

                ((System.ComponentModel.ISupportInitialize)Charts[ChartNum]).EndInit();
            }
            YearButtonEnableDisable();
            UpdateResourceGraphs();


            //Init troops
            TroopListPanel = new Panel() { Location = new Point(DEFAULT_PADDING, 25), Name = "TroopPanel", Size = new Size(MERC_BOX_WIDTH + SCROLLBAR_PADDING + DEFAULT_PADDING * 2, HEIGHT - 65), BorderStyle = BorderStyle.Fixed3D, AutoScroll = true };
            Panel TroopManagePanel = new Panel() { Location = new Point(WIDTH - 340, DEFAULT_PADDING), Name = "TroopPanel", Size = new Size(315, HEIGHT - 50), BorderStyle = BorderStyle.Fixed3D };
            Tabs[2].Controls.Add(TroopListPanel);
            Tabs[2].Controls.Add(TroopManagePanel);

            TotalTroopsLabel = new Label() { Location = new Point(DEFAULT_PADDING, DEFAULT_PADDING), Width = 200, Text = "Total Troops: " + TotalTroops };
            Tabs[2].Controls.Add(TotalTroopsLabel);

            Tabs[2].Controls[1].Controls.Add(new Button() { Location = new Point(DEFAULT_PADDING, DEFAULT_PADDING), Text = "Add troop" });
            Tabs[2].Controls[1].Controls[0].Click += new EventHandler(ShowNewTroopWindow);
            troopRemoveLabel = new Label() { Location = new Point(DEFAULT_PADDING, 65), Width = 150, Text = "Select troop to remove :" };
            Tabs[2].Controls[1].Controls.Add(troopRemoveLabel);
            troopRemoveComboBox = new ComboBox() { Location = new Point(DEFAULT_PADDING, 90), Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };
            troopRemoveComboBox.SelectedValueChanged += new EventHandler(troopRemoveComboBox_changed);
            Tabs[2].Controls[1].Controls.Add(troopRemoveComboBox);
            troopRemoveButton = new Button() { Location = new Point(DEFAULT_PADDING, 120), Text = "Remove troop" };
            troopRemoveButton.Click += new EventHandler(removeTroopButton_click);
            Tabs[2].Controls[1].Controls.Add(troopRemoveButton);
            UpdateTroopsList();

            //Tabs[3].Controls.Add(new Label() { Location = new Point(WIDTH / 2 - 50, HEIGHT / 2 - 50), Text = "Work in progress" });

            MainTabControl.ResumeLayout(false);

            KingdomWindow.ClientSize = new Size(WIDTH, HEIGHT);
            KingdomWindow.Controls.Add(MainTabControl);
            KingdomWindow.Name = "KingdomManager";
            KingdomWindow.Text = "Kingdom Manager";
            KingdomWindow.FormBorderStyle = FormBorderStyle.FixedSingle;
            KingdomWindow.MaximizeBox = false;
            KingdomWindow.ResumeLayout(false);


            //KingdomWindow.Show();
            Application.Run(KingdomWindow);

        }

        static void UpdateTroopsList()
        {
            int Y = DEFAULT_PADDING;
            TroopListPanel.Controls.Clear();
            troopRemoveComboBox.Items.Clear();
            for (int TroopNumber = 0; TroopNumber < Troops.Count; TroopNumber++)
            {
                Troops[TroopNumber].TroopPanel.Location = new Point(DEFAULT_PADDING, Y);
                Y += Troops[TroopNumber].TroopPanel.Height + DEFAULT_PADDING;
                TroopListPanel.Controls.Add(Troops[TroopNumber].TroopPanel);
                troopRemoveComboBox.Items.Add(Troops[TroopNumber]);
            }
            UpdateTotalTroopsText();
        }

        public static void UpdateTotalTroopsText()
        {
            TotalTroopsLabel.Text = "Total Troops: " + TotalTroops;
            UpdateTroopResourceUsageLabels();
            UpdateResourceGraphs();
        }

        public static void UpdateResourceGraph(int resource)
        {
            //TODO: Make food start take into account the food left over from previous years.
            int previousYearResources = 0;
            if (SelectedYear > 0)
            {
                for (int year = 0; year < SelectedYear; year++)
                {
                    for (int month = 0; month < MONTHS; month++)
                    {
                        previousYearResources += Years[year].Months[month][resource];
                    }
                }
            }
            int cumulativeResource = previousYearResources;
            int avgResources = 0;
            int cumulativeAvgResources = 0;
            int avgCount = 0;
            for (int month = 0; month < MONTHS; month++)
            {
                MonthBoxes[month].IntakeBoxes[resource].Text = Years[SelectedYear].Months[month][resource].ToString();
                avgCount = 0;
                if (int.TryParse(MonthBoxes[month].IntakeBoxes[resource].Text, out _))
                {
                    cumulativeAvgResources = 0;
                    for (int year = 0; year < Years.Count; year++)
                    {
                        if (Years[year].Months[month][resource] != 0)
                        {
                            cumulativeAvgResources += Years[year].Months[month][resource];
                            avgCount++;
                        }
                    }
                    avgResources = cumulativeAvgResources / avgCount;
                    projectedResources[resource][month] = cumulativeResource + avgResources;

                    cumulativeResource += Years[SelectedYear].Months[month][resource];
                    resources[resource][month] = cumulativeResource;
                }
            }
            Charts[resource].Series[0].Points.DataBindY(resources[resource]);
            Charts[resource].Series[1].Points.DataBindY(projectedResources[resource]);
        }

        static void UpdateResourceGraphs()
        {
            for (int resource = 0; resource < 2; resource++)
            {
                UpdateResourceGraph(resource);
                YearLabel[resource].Text = "Year: " + Years[SelectedYear].Num;
            }
        }

        static void UpdateTroopResourceUsageLabels()
        {
            for (int resource = 0; resource < 2; resource++)
            {
                TroopResourceUsageLabel[resource].Text = "Troop " + ResourceName[resource] + " usage: " + TroopResourceUsage(resource);
            }
        }

        static List<Year> getYears()
        {
            List<Year> years;
            using (IDbConnection connection = new SQLiteConnection(CONNECTION_STRING))
            {
                years = connection.Query<Year>("select * from Years order by Num", new DynamicParameters()).ToList();
            }
            return years;
        }

        static List<Month> getMonthsForYear(string yearUUID)
        {
            List<Month> months;
            using (IDbConnection connection = new SQLiteConnection(CONNECTION_STRING))
            {
                months = connection.Query<Month>("select * from Months where YearUUID = @YearUUID order by Num", new DynamicParameters(new { YearUUID = yearUUID })).ToList();
            }
            return months;
        }

        static List<TroopLevy> getTroops()
        {
            List<TroopLevy> troops = new List<TroopLevy>();
            List<Object> result;
            using (IDbConnection connection = new SQLiteConnection(CONNECTION_STRING))
            {
                result = connection.Query("select * from Troops order by County asc, TroopNum desc", new DynamicParameters()).ToList();
            }
            for (int troop = 0; troop < result.Count; troop++)
            {
                IDictionary<string, object> values = (IDictionary<string, object>)result[troop];
                if (values["GoldCost"] is null)
                {
                    troops.Add(new TroopLevy(values["County"].ToString(), int.Parse(values["TroopNum"].ToString())) { UUID = values["UUID"].ToString() });
                }
                else
                {
                    troops.Add(new Mercenary(values["County"].ToString(), int.Parse(values["TroopNum"].ToString()), int.Parse(values["GoldCost"].ToString())) { UUID = values["UUID"].ToString() });
                }
            }
            return troops;

        }

        public static void updateTroop(string UUID)
        {
            string levyCommand =
                            "update Troops " +
                            "set County = @County, TroopNum = @TroopNum " +
                            "where UUID = @UUID";
            string mercCommand =
                            "update Troops " +
                            "set County = @County, TroopNum = @TroopNum, GoldCost = @GoldCost " +
                            "where UUID = @UUID";
            string command;
            DynamicParameters parameters;
            // INSERT INTO table_name(column1, column2, column3, ...)
            //VALUES(value1, value2, value3, ...);

            for (int i = 0; i < Troops.Count; i++)
            {
                if (Troops[i].UUID == UUID)
                {
                    parameters = new DynamicParameters();
                    parameters.Add("@UUID", Troops[i].UUID);
                    parameters.Add("@County", Troops[i].County);
                    parameters.Add("@TroopNum", Troops[i].TroopNum);
                    if (Troops[i] is Mercenary)
                    {
                        parameters.Add("@GoldCost", ((Mercenary)Troops[i]).GoldCost);
                        command = mercCommand;

                    }
                    else
                    {
                        command = levyCommand;
                    }
                    using (IDbConnection connection = new SQLiteConnection(CONNECTION_STRING))
                    {
                        int output = connection.Execute(command, parameters);
                    }
                    //MessageBox.Show(output.GetType().ToString());
                }
            }
            //troops = connection.Query<TroopLevy>("select * from Troops", new DynamicParameters()).ToList();
        }

        public static void AddNewTroop(TroopLevy troop)
        {
            Troops.Add(troop);
            //UPDATE table_name
            //SET column1 = value1, column2 = value2, ...
            //WHERE condition;

            string levyCommand =
                            "insert into Troops " +
                            "(UUID, County, TroopNum) " +
                            "values (@UUID, @County, @TroopNum)";
            string mercCommand =
                            "insert into Troops " +
                            "(UUID, County, TroopNum, GoldCost) " +
                            "values (@UUID, @County, @TroopNum, @GoldCost)";
            string command;
            DynamicParameters parameters;
            // INSERT INTO table_name(column1, column2, column3, ...)
            //VALUES(value1, value2, value3, ...);
            parameters = new DynamicParameters();
            parameters.Add("@UUID", troop.UUID);
            parameters.Add("@County", troop.County);
            parameters.Add("@TroopNum", troop.TroopNum);
            if (troop is Mercenary)
            {
                parameters.Add("@GoldCost", ((Mercenary)troop).GoldCost);
                command = mercCommand;

            }
            else
            {
                command = levyCommand;
            }
            using (IDbConnection connection = new SQLiteConnection(CONNECTION_STRING))
            {
                int output = connection.Execute(command, parameters);
            }
            UpdateTroopsList();
        }

        public static void UpdateMonth(Month month)
        {

            //UPDATE table_name
            //SET column1 = value1, column2 = value2, ...
            //WHERE condition;

            string command =
                            "update Months " +
                            "set FoodIncome = @FoodIncome, GoldIncome = @GoldIncome " +
                            "where UUID = @UUID";
            DynamicParameters parameters;
            // INSERT INTO table_name(column1, column2, column3, ...)
            //VALUES(value1, value2, value3, ...);
            //FIXME Make it able to add new months to the DB!
            parameters = new DynamicParameters();
            parameters.Add("@FoodIncome", month.FoodIncome);
            parameters.Add("@GoldIncome", month.GoldIncome);
            parameters.Add("@UUID", month.UUID);
            using (IDbConnection connection = new SQLiteConnection(CONNECTION_STRING))
            {
                int output = connection.Execute(command, parameters);
            }
        }

        static void ShowNewTroopWindow(object sender, EventArgs e)
        {
            AddTroop temp = new AddTroop();
            temp.CountyComboBox.Items.AddRange(COUNTIES);
            temp.Show();
        }

        static void removeTroopButton_click(object sender, EventArgs e)
        {
            if (!(troopRemoveComboBox.SelectedItem is null))
            {
                removeTroop((TroopLevy)troopRemoveComboBox.SelectedItem);
            }
            else
            {
                troopRemoveLabel.BackColor = Color.Red;
                MessageBox.Show("Must select a troop with the drop down box.");
            }
        }

        static void troopRemoveComboBox_changed(object sender, EventArgs e)
        {
            troopRemoveLabel.BackColor = default(Color);
        }

        static void removeTroop(TroopLevy troop)
        {
            Troops.Remove(troop);
            string command =
                            "delete from Troops " +
                            "where UUID = @UUID";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@UUID", troop.UUID);
            using (IDbConnection connection = new SQLiteConnection(CONNECTION_STRING))
            {
                int output = connection.Execute(command, parameters);
            }
            UpdateTroopsList();
        }

        static void AddPreviousYear(object sender, EventArgs e)
        {
            Year newYear = new Year() { Num = Years[0].Num - 1 };
            Years.Add(newYear);
            Years.Sort();
            WriteNewYear(newYear);
            SelectedYear = 0;
            YearButtonEnableDisable();
            UpdateResourceGraphs();
        }

        static void SelectPreviousYear(object sender, EventArgs e)
        {
            SelectedYear--;
            YearButtonEnableDisable();
            UpdateResourceGraphs();
        }

        static void SelectNextYear(object sender, EventArgs e)
        {
            SelectedYear++;
            YearButtonEnableDisable();
            UpdateResourceGraphs();
        }

        static void AddNextYear(object sender, EventArgs e)
        {
            Year newYear = new Year() { Num = Years[Years.Count - 1].Num + 1 };
            Years.Add(newYear);
            WriteNewYear(newYear);
            SelectedYear = Years.Count - 1;
            YearButtonEnableDisable();
            UpdateResourceGraphs();
        }

        static void YearButtonEnableDisable()
        {
            if (SelectedYear == 0)
            {
                for (int resource = 0; resource < resources.Length; resource++)
                {
                    PreviousYearButton[resource].Enabled = false;
                }
            }
            else
            {
                for (int resource = 0; resource < resources.Length; resource++)
                {
                    PreviousYearButton[resource].Enabled = true;
                }
            }

            if (SelectedYear == Years.Count - 1)
            {
                for (int resource = 0; resource < resources.Length; resource++)
                {
                    NextYearButton[resource].Enabled = false;
                }
            }
            else
            {
                for (int resource = 0; resource < resources.Length; resource++)
                {
                    NextYearButton[resource].Enabled = true;
                }
            }
        }

        static void WriteNewYear(Year year)
        {
            string command =
                            "insert into Years " +
                            "(UUID, Num) " +
                            "values (@UUID, @Num) ";
            DynamicParameters parameters;
            parameters = new DynamicParameters();
            parameters.Add("@UUID", year.UUID);
            parameters.Add("@Num", year.Num);
            using (IDbConnection connection = new SQLiteConnection(CONNECTION_STRING))
            {
                int output = connection.Execute(command, parameters);
            }
            for (int month = 0; month < MONTHS; month++)
            {
                WriteNewMonth(year.Months[month]);
            }
        }

        static void WriteNewMonth(Month month)
        {
            string command =
                            "insert into Months " +
                            "(UUID, YearUUID, Num, FoodIncome, GoldIncome) " +
                            "values (@UUID, @YearUUID, @Num, @FoodIncome, @GoldIncome) ";
            DynamicParameters parameters;
            parameters = new DynamicParameters();
            parameters.Add("@UUID", month.UUID);
            parameters.Add("@YearUUID", month.YearUUID);
            parameters.Add("@Num", month.Num);
            parameters.Add("@FoodIncome", month.FoodIncome);
            parameters.Add("@GoldIncome", month.GoldIncome);
            using (IDbConnection connection = new SQLiteConnection(CONNECTION_STRING))
            {
                int output = connection.Execute(command, parameters);
            }
        }
    }
}

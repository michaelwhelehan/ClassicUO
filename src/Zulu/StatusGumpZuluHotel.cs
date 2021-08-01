using ClassicUO.Configuration;
using ClassicUO.Data;
using ClassicUO.Game.Data;
using ClassicUO.Game.Managers;
using ClassicUO.Game.UI.Controls;
using ClassicUO.Input;
using ClassicUO.IO.Resources;
using ClassicUO.Resources;
using Microsoft.Xna.Framework;

namespace ClassicUO.Game.UI.Gumps
{
    internal sealed class StatusGumpZuluhotel : StatusGumpBase
    {
        private readonly GumpPicWithWidth[] _fillBars = new GumpPicWithWidth[3];

        public StatusGumpZuluhotel()
        {
            Point pos = Point.Zero;
            _labels = new Label[(int)MobileStats.Max];

            Add(new GumpPic(0, 0, 0x2A6E, 0));
            Add(new GumpPic(49, 41, 0x0805, 0)); // Health bar
            Add(new GumpPic(49, 54, 0x0805, 0)); // Mana bar
            Add(new GumpPic(49, 67, 0x0805, 0)); // Stamina bar

            if (Client.Version >= ClientVersion.CV_5020)
            {
                Add
                (
                    new Button((int)ButtonType.BuffIcon, 0x7538, 0x7539, 0x7539)
                    {
                        X = 453,
                        Y = 25,
                        ButtonAction = ButtonAction.Activate
                    }
                );
            }

            ushort gumpIdHp = 0x080A;

            if (World.Player.IsPoisoned)
            {
                gumpIdHp = 0x0808;
            }
            else if (World.Player.IsYellowHits)
            {
                gumpIdHp = 0x0809;
            }

            _fillBars[(int)FillStats.Hits] = new GumpPicWithWidth
            (
                49,
                41,
                gumpIdHp,
                0,
                0
            );

            _fillBars[(int)FillStats.Mana] = new GumpPicWithWidth
            (
                49,
                54,
                0x080B,
                0,
                0
            );

            _fillBars[(int)FillStats.Stam] = new GumpPicWithWidth
            (
                49,
                67,
                0x080F,
                0,
                0
            );

            Add(_fillBars[(int)FillStats.Hits]);
            Add(_fillBars[(int)FillStats.Mana]);
            Add(_fillBars[(int)FillStats.Stam]);

            UpdateStatusFillBar(FillStats.Hits, World.Player.Hits, World.Player.HitsMax);
            UpdateStatusFillBar(FillStats.Mana, World.Player.Mana, World.Player.ManaMax);
            UpdateStatusFillBar(FillStats.Stam, World.Player.Stamina, World.Player.StaminaMax);

            // Stat matrix
            int xCol1 = 72;
            int xCol2 = 178;
            int xCol3 = 302;
            int xCol4 = 420;
            int yRow1 = 104;
            int yRow2 = 139;
            int yRow3 = 174;
            int yRow4 = 209;
            int yRow5 = 244;

            // Name
            Label text = new Label
            (
                !string.IsNullOrEmpty(World.Player.Name) ? World.Player.Name : string.Empty,
                true,
                0x1,
                170,
                0,
                align: TEXT_ALIGN_TYPE.TS_CENTER
            )
            {
                X = 196,
                Y = 35
            };

            _labels[(int)MobileStats.Name] = text;
            Add(text);


            // Stat locks
            Add(_lockers[(int)StatType.Str] = new GumpPic(113, 104, GetStatLockGraphic(World.Player.StrLock), 0));

            Add(_lockers[(int)StatType.Dex] = new GumpPic(113, 139, GetStatLockGraphic(World.Player.DexLock), 0));

            Add(_lockers[(int)StatType.Int] = new GumpPic(113, 174, GetStatLockGraphic(World.Player.IntLock), 0));

            _lockers[(int)StatType.Str].MouseUp += (sender, e) =>
           {
               World.Player.StrLock = (Lock)(((byte)World.Player.StrLock + 1) % 3);
               GameActions.ChangeStatLock(0, World.Player.StrLock);

               _lockers[(int)StatType.Str].Graphic = GetStatLockGraphic(World.Player.StrLock);
           };

            _lockers[(int)StatType.Dex].MouseUp += (sender, e) =>
           {
               World.Player.DexLock = (Lock)(((byte)World.Player.DexLock + 1) % 3);
               GameActions.ChangeStatLock(1, World.Player.DexLock);

               _lockers[(int)StatType.Dex].Graphic = GetStatLockGraphic(World.Player.DexLock);
           };

            _lockers[(int)StatType.Int].MouseUp += (sender, e) =>
           {
               World.Player.IntLock = (Lock)(((byte)World.Player.IntLock + 1) % 3);
               GameActions.ChangeStatLock(2, World.Player.IntLock);

               _lockers[(int)StatType.Int].Graphic = GetStatLockGraphic(World.Player.IntLock);
           };

            // Str/dex/int text labels
            AddStatTextLabel(World.Player.Strength.ToString(), MobileStats.Strength, xCol1, yRow1);
            AddStatTextLabel(World.Player.Dexterity.ToString(), MobileStats.Dexterity, xCol1, yRow2);
            AddStatTextLabel(World.Player.Intelligence.ToString(), MobileStats.Intelligence, xCol1, yRow3);

            // Hits/stam/mana

            AddStatTextLabel
            (
                World.Player.Hits.ToString(),
                MobileStats.HealthCurrent,
                xCol2,
                yRow1 - 7,
                40,
                alignment: TEXT_ALIGN_TYPE.TS_CENTER
            );

            AddStatTextLabel
            (
                World.Player.HitsMax.ToString(),
                MobileStats.HealthMax,
                xCol2,
                yRow1 + 7,
                40,
                alignment: TEXT_ALIGN_TYPE.TS_CENTER
            );

            AddStatTextLabel
            (
                World.Player.Stamina.ToString(),
                MobileStats.StaminaCurrent,
                xCol2,
                yRow2 - 7,
                40,
                alignment: TEXT_ALIGN_TYPE.TS_CENTER
            );

            AddStatTextLabel
            (
                World.Player.StaminaMax.ToString(),
                MobileStats.StaminaMax,
                xCol2,
                yRow2 + 7,
                40,
                alignment: TEXT_ALIGN_TYPE.TS_CENTER
            );

            AddStatTextLabel
            (
                World.Player.Mana.ToString(),
                MobileStats.ManaCurrent,
                xCol2,
                yRow3 - 7,
                40,
                alignment: TEXT_ALIGN_TYPE.TS_CENTER
            );

            AddStatTextLabel
            (
                World.Player.ManaMax.ToString(),
                MobileStats.ManaMax,
                xCol2,
                yRow3 + 7,
                40,
                alignment: TEXT_ALIGN_TYPE.TS_CENTER
            );

            // CurrentProfile over max lines
            Add
            (
                new Line
                (
                    xCol2,
                    yRow1 + 6,
                    30,
                    1,
                    0xFFFFFFFF
                )
            );

            Add
            (
                new Line
                (
                    xCol2,
                    yRow2 + 6,
                    30,
                    1,
                    0xFFFFFFFF
                )
            );

            Add
            (
                new Line
                (
                    xCol2,
                    yRow3 + 6,
                    30,
                    1,
                    0xFFFFFFFF
                )
            );

            // Followers / max followers

            AddStatTextLabel
            (
                $"{World.Player.Followers}/{World.Player.FollowersMax}",
                MobileStats.Followers,
                xCol2 + 2,
                yRow5,
                alignment: TEXT_ALIGN_TYPE.TS_CENTER
            );


            // Armor, weight / max weight
            AddStatTextLabel
            (
                World.Player.PhysicalResistance.ToString(),
                MobileStats.AR,
                413,
                48,
                alignment: TEXT_ALIGN_TYPE.TS_CENTER
            );

            AddStatTextLabel
            (
                World.Player.Weight.ToString(),
                MobileStats.WeightCurrent,
                xCol1 - 10,
                yRow5 - 7,
                50,
                alignment: TEXT_ALIGN_TYPE.TS_CENTER
            );

            AddStatTextLabel
            (
                World.Player.WeightMax.ToString(),
                MobileStats.WeightMax,
                xCol1 - 10,
                yRow5 + 7,
                50,
                alignment: TEXT_ALIGN_TYPE.TS_CENTER
            );

            Add
            (
                new Line
                (
                    xCol1 - 10,
                    yRow5 + 6,
                    40,
                    1,
                    0xFFFFFFFF
                )
            );

            // Damage, Gold

            AddStatTextLabel($"{World.Player.DamageMin}-{World.Player.DamageMax}", MobileStats.Damage, xCol2 - 10, yRow4, 50, alignment: TEXT_ALIGN_TYPE.TS_CENTER);

            AddStatTextLabel(World.Player.Gold.ToString(), MobileStats.Gold, xCol1 - 20, yRow4, 70, alignment: TEXT_ALIGN_TYPE.TS_CENTER);

            // Hunger, Criminal/Murderer Timers

            AddStatTextLabel
            (
                World.Player.Hunger.ToString(),
                MobileStats.Hunger,
                218,
                74
            );

            AddStatTextLabel
            (
                World.Player.CriminalTimer.ToString(),
                MobileStats.CriminalTimer,
                268,
                74,
                alignment: TEXT_ALIGN_TYPE.TS_CENTER
            );

            AddStatTextLabel
            (
                World.Player.MurderTimer.ToString(),
                MobileStats.MurderTimer,
                331,
                74,
                alignment: TEXT_ALIGN_TYPE.TS_CENTER
            );

            // Heal Bonus, Magic Immunity, Magic Reflect
            AddStatTextLabel
            (
                World.Player.HealBonus.ToString(),
                MobileStats.HealBonus,
                xCol3,
                yRow3
            );

            AddStatTextLabel
            (
                World.Player.MagicImmunity.ToString(),
                MobileStats.MagicImmunity,
                xCol3,
                yRow4
            );

            AddStatTextLabel
            (
                World.Player.MagicReflect.ToString(),
                MobileStats.MagicReflect,
                xCol3,
                yRow5
            );

            // Protections

            AddStatTextLabel
            (
                World.Player.PhysicalProtection.ToString(),
                MobileStats.PhysicalProtection,
                xCol3,
                yRow1
            );

            AddStatTextLabel
            (
                World.Player.PoisonProtection.ToString(),
                MobileStats.PoisonProtection,
                xCol3,
                yRow2
            );

            AddStatTextLabel
            (
                World.Player.FireProtection.ToString(),
                MobileStats.FireProtection,
                xCol4,
                yRow1
            );

            AddStatTextLabel
            (
                World.Player.WaterProtection.ToString(),
                MobileStats.WaterProtection,
                xCol4,
                yRow2
            );

            AddStatTextLabel
            (
                World.Player.AirProtection.ToString(),
                MobileStats.AirProtection,
                xCol4,
                yRow3
            );

            AddStatTextLabel
            (
                World.Player.EarthProtection.ToString(),
                MobileStats.EarthProtection,
                xCol4,
                yRow4
            );

            AddStatTextLabel
            (
                World.Player.NecroProtection.ToString(),
                MobileStats.NecroProtection,
                xCol4,
                yRow5
            );

            // Tooltips
            int xCornerCol = 17;
            int yCornerRow = 95;
            int statBoxWidth = 110;
            int statBoxHeight = 32;
            Add
            (
                new HitBox
                (
                    xCornerCol,
                    yCornerRow,
                    statBoxWidth,
                    statBoxHeight,
                    ResGumps.Strength,
                    1
                )
                { CanMove = true }
            );
            Add
            (
                new HitBox
                (
                    xCornerCol,
                    yCornerRow + 33,
                    statBoxWidth,
                    statBoxHeight,
                    ResGumps.Dexterity,
                    1
                )
                { CanMove = true }
            );
            Add
            (
                new HitBox
                (
                    xCornerCol,
                    yCornerRow + 33 * 2,
                    statBoxWidth,
                    statBoxHeight,
                    ResGumps.Intelligence,
                    1
                )
                { CanMove = true }
            );
            Add
            (
                new HitBox
                (
                    xCornerCol,
                    yCornerRow + 33 * 3,
                    statBoxWidth,
                    statBoxHeight,
                    ResGumps.Gold,
                    1
                )
                { CanMove = true }
            );
            Add
            (
                new HitBox
                (
                    xCornerCol,
                    yCornerRow + 33 * 4,
                    statBoxWidth,
                    statBoxHeight,
                    ResGumps.Weight,
                    1
                )
                { CanMove = true }
            );
            Add
            (
                new HitBox
                (
                    xCornerCol + 113,
                    yCornerRow,
                    statBoxWidth,
                    statBoxHeight,
                    ResGumps.HitPoints,
                    1
                )
                { CanMove = true }
            );
            Add
            (
                new HitBox
                (
                    xCornerCol + 113,
                    yCornerRow + 33,
                    statBoxWidth,
                    statBoxHeight,
                    ResGumps.Stamina,
                    1
                )
                { CanMove = true }
            );
            Add
            (
                new HitBox
                (
                    xCornerCol + 113,
                    yCornerRow + 33 * 2,
                    statBoxWidth,
                    statBoxHeight,
                    ResGumps.Mana,
                    1
                )
                { CanMove = true }
            );
            Add
            (
                new HitBox
                (
                    xCornerCol + 113,
                    yCornerRow + 33 * 3,
                    statBoxWidth,
                    statBoxHeight,
                    ResGumps.Damage,
                    1
                )
                { CanMove = true }
            );
            Add
            (
                new HitBox
                (
                    xCornerCol + 113,
                    yCornerRow + 33 * 4,
                    statBoxWidth,
                    statBoxHeight,
                    ResGumps.Followers,
                    1
                )
                { CanMove = true }
            );
            Add
            (
                new HitBox
                (
                    xCornerCol + 113 * 2,
                    yCornerRow,
                    statBoxWidth,
                    statBoxHeight,
                    ResGumps.PhysicalProtection,
                    1
                )
                { CanMove = true }
            );
            Add
            (
                new HitBox
                (
                    xCornerCol + 113 * 2,
                    yCornerRow + 33,
                    statBoxWidth,
                    statBoxHeight,
                    ResGumps.PoisonProtection,
                    1
                )
                { CanMove = true }
            );
            Add
            (
                new HitBox
                (
                    xCornerCol + 113 * 2,
                    yCornerRow + 33 * 2,
                    statBoxWidth,
                    statBoxHeight,
                    ResGumps.Healbonus,
                    1
                )
                { CanMove = true }
            );
            Add
            (
                new HitBox
                (
                    xCornerCol + 113 * 2,
                    yCornerRow + 33 * 3,
                    statBoxWidth,
                    statBoxHeight,
                    ResGumps.MagicImmunity,
                    1
                )
                { CanMove = true }
            );
            Add
            (
                new HitBox
                (
                    xCornerCol + 113 * 2,
                    yCornerRow + 33 * 4,
                    statBoxWidth,
                    statBoxHeight,
                    ResGumps.MagicReflect,
                    1
                )
                { CanMove = true }
            );
            Add
            (
                new HitBox
                (
                    xCornerCol + 113 * 3,
                    yCornerRow,
                    statBoxWidth,
                    statBoxHeight,
                    ResGumps.FireProtection,
                    1
                )
                { CanMove = true }
            );
            Add
            (
                new HitBox
                (
                    xCornerCol + 113 * 3,
                    yCornerRow + 33,
                    statBoxWidth,
                    statBoxHeight,
                    ResGumps.WaterProtection,
                    1
                )
                { CanMove = true }
            );
            Add
            (
                new HitBox
                (
                    xCornerCol + 113 * 3,
                    yCornerRow + 33 * 2,
                    statBoxWidth,
                    statBoxHeight,
                    ResGumps.AirProtection,
                    1
                )
                { CanMove = true }
            );
            Add
            (
                new HitBox
                (
                    xCornerCol + 113 * 3,
                    yCornerRow + 33 * 3,
                    statBoxWidth,
                    statBoxHeight,
                    ResGumps.EarthProtection,
                    1
                )
                { CanMove = true }
            );
            Add
            (
                new HitBox
                (
                    xCornerCol + 113 * 3,
                    yCornerRow + 33 * 4,
                    statBoxWidth,
                    statBoxHeight,
                    ResGumps.NecroProtection,
                    1
                )
                { CanMove = true }
            );
            Add
            (
                new HitBox
                (
                    205,
                    66,
                    28,
                    24,
                    ResGumps.Hunger,
                    1
                )
                { CanMove = true }
            );
            Add
            (
                new HitBox
                (
                    398,
                    30,
                    34,
                    44,
                    ResGumps.ArmorRating,
                    1
                )
                { CanMove = true }
            );
            Add
            (
                new HitBox
                (
                    250,
                    66,
                    45,
                    24,
                    ResGumps.CriminalTimer,
                    1
                )
                { CanMove = true }
            );
            Add
            (
                new HitBox
                (
                    311,
                    66,
                    45,
                    24,
                    ResGumps.MurdererTimer,
                    1
                )
                { CanMove = true }
            );
            Add
            (
                new HitBox
                (
                    210,
                    294,
                    55,
                    58,
                    ResGumps.Notoriety,
                    1
                )
                { CanMove = true }
            );
        }

        public override void Update(double totalTime, double frameTime)
        {
            if (IsDisposed)
            {
                return;
            }

            if (_refreshTime < totalTime)
            {
                _refreshTime = (long)totalTime + 250;

                UpdateStatusFillBar(FillStats.Hits, World.Player.Hits, World.Player.HitsMax);
                UpdateStatusFillBar(FillStats.Mana, World.Player.Mana, World.Player.ManaMax);
                UpdateStatusFillBar(FillStats.Stam, World.Player.Stamina, World.Player.StaminaMax);

                _labels[(int)MobileStats.Name].Text = !string.IsNullOrEmpty(World.Player.Name) ? World.Player.Name : string.Empty;

                _labels[(int)MobileStats.Strength].Text = World.Player.Strength.ToString();

                _labels[(int)MobileStats.Dexterity].Text = World.Player.Dexterity.ToString();

                _labels[(int)MobileStats.Intelligence].Text = World.Player.Intelligence.ToString();

                _labels[(int)MobileStats.HealthCurrent].Text = World.Player.Hits.ToString();

                _labels[(int)MobileStats.HealthMax].Text = World.Player.HitsMax.ToString();

                _labels[(int)MobileStats.StaminaCurrent].Text = World.Player.Stamina.ToString();

                _labels[(int)MobileStats.StaminaMax].Text = World.Player.StaminaMax.ToString();

                _labels[(int)MobileStats.ManaCurrent].Text = World.Player.Mana.ToString();

                _labels[(int)MobileStats.ManaMax].Text = World.Player.ManaMax.ToString();

                _labels[(int)MobileStats.Followers].Text = $"{World.Player.Followers}/{World.Player.FollowersMax}";

                _labels[(int)MobileStats.AR].Text = World.Player.PhysicalResistance.ToString();

                _labels[(int)MobileStats.WeightCurrent].Text = World.Player.Weight.ToString();

                _labels[(int)MobileStats.WeightMax].Text = World.Player.WeightMax.ToString();

                _labels[(int)MobileStats.Damage].Text = $"{World.Player.DamageMin}-{World.Player.DamageMax}";

                _labels[(int)MobileStats.Gold].Text = World.Player.Gold.ToString();

                _labels[(int)MobileStats.Hunger].Text = World.Player.Hunger.ToString();

                _labels[(int)MobileStats.HealBonus].Text = World.Player.HealBonus.ToString();

                _labels[(int)MobileStats.MagicImmunity].Text = World.Player.MagicImmunity.ToString();

                _labels[(int)MobileStats.MagicReflect].Text = World.Player.MagicReflect.ToString();

                _labels[(int)MobileStats.PhysicalProtection].Text = World.Player.PhysicalProtection.ToString();

                _labels[(int)MobileStats.PoisonProtection].Text = World.Player.PoisonProtection.ToString();

                _labels[(int)MobileStats.FireProtection].Text = World.Player.FireProtection.ToString();

                _labels[(int)MobileStats.WaterProtection].Text = World.Player.WaterProtection.ToString();

                _labels[(int)MobileStats.AirProtection].Text = World.Player.AirProtection.ToString();

                _labels[(int)MobileStats.EarthProtection].Text = World.Player.EarthProtection.ToString();

                _labels[(int)MobileStats.NecroProtection].Text = World.Player.NecroProtection.ToString();

                _labels[(int)MobileStats.CriminalTimer].Text = World.Player.CriminalTimer.ToString();

                _labels[(int)MobileStats.MurderTimer].Text = World.Player.MurderTimer.ToString();
            }

            base.Update(totalTime, frameTime);
        }

        protected override void OnMouseUp(int x, int y, MouseButtonType button)
        {
            if (button == MouseButtonType.Left)
            {
                if (TargetManager.IsTargeting)
                {
                    TargetManager.Target(World.Player);
                    Mouse.LastLeftButtonClickTime = 0;
                }
                else
                {
                    Point p = new Point(x, y);
                    Rectangle rect = new Rectangle(Bounds.Width - 42, Bounds.Height - 25, Bounds.Width, Bounds.Height);

                    if (rect.Contains(p))
                    {
                        UIManager.GetGump<BaseHealthBarGump>(World.Player)?.Dispose();

                        //TCH whole if else
                        if (ProfileManager.CurrentProfile.CustomBarsToggled)
                        {
                            UIManager.Add(new HealthBarGumpCustom(World.Player) { X = ScreenCoordinateX, Y = ScreenCoordinateY });
                        }
                        else
                        {
                            UIManager.Add(new HealthBarGump(World.Player) { X = ScreenCoordinateX, Y = ScreenCoordinateY });
                        }

                        Dispose();
                    }
                }
            }
        }

        private static int CalculatePercents(int max, int current, int maxValue)
        {
            if (max > 0)
            {
                max = current * 100 / max;

                if (max > 100)
                {
                    max = 100;
                }

                if (max > 1)
                {
                    max = maxValue * max / 100;
                }
            }

            return max;
        }

        private void UpdateStatusFillBar(FillStats id, int current, int max)
        {
            ushort gumpId = 0x080A;
            ushort gumpMana = 0x080B;
            ushort gumpStam = 0x080F;

            if (id == FillStats.Hits)
            {
                if (World.Player.IsPoisoned)
                {
                    gumpId = 0x0808;
                }
                else if (World.Player.IsYellowHits)
                {
                    gumpId = 0x0809;
                }
            }

            if (max > 0)
            {
                if (id == FillStats.Hits)
                {
                    _fillBars[(int)id].Graphic = gumpId;
                }
                else if (id == FillStats.Mana)
                {
                    _fillBars[(int)id].Graphic = gumpMana;
                }
                else if (id == FillStats.Stam)
                {
                    _fillBars[(int)id].Graphic = gumpStam;
                }

                _fillBars[(int)id].Percent = CalculatePercents(max, current, 109);
            }
        }

        // TODO: move to base class?
        private void AddStatTextLabel(string text, MobileStats stat, int x, int y, int maxWidth = 0, ushort hue = 0x0386, TEXT_ALIGN_TYPE alignment = TEXT_ALIGN_TYPE.TS_LEFT)
        {
            Label label = new Label
            (
                text, false, hue, maxWidth,
                align: alignment, font: 1
            )
            {
                X = x - 5,
                Y = y
            };

            _labels[(int)stat] = label;
            Add(label);
        }


        private enum MobileStats
        {
            Name,
            Strength,
            Dexterity,
            Intelligence,
            HealthCurrent,
            HealthMax,
            StaminaCurrent,
            StaminaMax,
            ManaCurrent,
            ManaMax,
            WeightCurrent,
            WeightMax,
            Followers,
            FollowersMax,
            Gold,
            AR,
            Damage,
            PhysicalProtection,
            PoisonProtection,
            FireProtection,
            WaterProtection,
            AirProtection,
            EarthProtection,
            NecroProtection,
            HealBonus,
            MagicImmunity,
            MagicReflect,
            Hunger,
            CriminalTimer,
            MurderTimer,
            Max
        }

        private enum FillStats
        {
            Hits,
            Mana,
            Stam
        }
    }
} 
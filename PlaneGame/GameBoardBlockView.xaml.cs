﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GameruleHandler;
using MaterialDesignThemes.Wpf;

namespace PlaneGame
{
    /// <summary>
    /// GameBoardBlockView.xaml 的交互逻辑
    /// </summary>
    public partial class GameBoardBlockView : UserControl
    {
        public delegate void GBBlockClickEventHandler(int x, int y, object sender);
        public event GBBlockClickEventHandler Click; 
        public int XPos { get; set; }
        public int YPos { get; set; }
        GameBoardBlock _block;
        public GameBoardBlock Block
        {
            get
            {
                return _block;
            }
            set
            {
                _block = value;
                switch (value)
                {
                    case GameBoardBlock.Barrier:
                        CZBackGround.Mode = ColorZoneMode.Inverted;
                        break;
                    case GameBoardBlock.Body:
                        CZBackGround.Mode = ColorZoneMode.PrimaryMid;
                        break;
                    case GameBoardBlock.ModelBody:
                        CZBackGround.Mode = ColorZoneMode.PrimaryMid;
                        break;
                    case GameBoardBlock.ModelHead:
                        CZBackGround.Mode = ColorZoneMode.Accent;
                        break;
                    case GameBoardBlock.Head:
                        CZBackGround.Mode = ColorZoneMode.Accent;
                        break;
                    case GameBoardBlock.Killed:
                        CZBackGround.Mode = ColorZoneMode.PrimaryLight;
                        break;
                    case GameBoardBlock.Nothing:
                        CZBackGround.Mode = ColorZoneMode.Standard;
                        break;
                    case GameBoardBlock.Unknown:
                        CZBackGround.Mode = ColorZoneMode.Dark;
                        break;
                    default:
                        if (char.IsUpper((char)value))
                        {
                            CZBackGround.Mode = ColorZoneMode.Accent;
                        }
                        else
                        {
                            CZBackGround.Mode = ColorZoneMode.PrimaryMid;
                        }
                        TBText.Text = ((char)value).ToString();
                        break;
                }
            }
        }

        public GameBoardBlockView(GameBoardBlock block)
        {
            InitializeComponent();
            Block = block;
        }

        [System.Diagnostics.DebuggerStepThrough]
        private void CZBackGround_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Click(XPos, YPos, this);
        }

        [System.Diagnostics.DebuggerStepThrough]
        private void CZBackGround_TouchUp(object sender, TouchEventArgs e)
        {
            Click(XPos, YPos, this);
        }

        [System.Diagnostics.DebuggerStepThrough]
        private void CZBackGround_StylusUp(object sender, StylusEventArgs e)
        {
            Click(XPos, YPos, this);
        }
    }
}

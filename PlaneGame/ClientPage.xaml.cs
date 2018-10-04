using System;
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
using NetClient;

namespace PlaneGame
{
    /// <summary>
    /// ClientPage.xaml 的交互逻辑
    /// </summary>
    public partial class ClientPage : Page
    {
        public NetworkClient client { get; private set; } = new NetworkClient();
        public event RoutedEventHandler PageExitEvent;
        public ClientPage()
        {
            InitializeComponent();
            SnbMain.MessageQueue = new SnackbarMessageQueue(Properties.Settings.Default.SnakeBarMessageDuration);
            FrmMain.Content = new GameDefaultPage();
            client.FailureCaused += Client_FailureCaused;
            client.MessageRecieved += Client_MessageRecieved;
            client.Info += Client_Info;
            Loaded += (s, args) =>
            {
                ConnectToServerWindow window = new ConnectToServerWindow(client);
                window.Owner = Application.Current.MainWindow;
                if (!window.ShowDialog())
                {
                    PageExitEvent?.Invoke(this, null);
                }
            };
        }

        Dictionary<string, Chip> NameCards = new Dictionary<string, Chip>();
        Page _pge;
        Page NowPage
        {
            get
            {
                return _pge;
            }
            set
            {
                _pge = value;
                FrmMain.Content = value;
            }
        }

        private void Client_Info(string msg)
        {
            Dispatcher.Invoke(() =>
            {
                SnbMain.MessageQueue.Enqueue(msg);
            });
        }

        private void Client_MessageRecieved(string msg)
        {
            //throw new NotImplementedException();
            Dispatcher.Invoke(() =>
            {
#if DEBUG
                HandleChat("(Original Message)", msg);
#endif
                string[] vs = msg.Split('|');
                try
                {
                    switch (vs[0])
                    {
                        case "CHAT":
                            HandleChat(vs[1], vs[2]);
                            break;
                        case "SLOG":
                            HandleChat("", vs[1]);
                            break;
                        case "SBAR":
                            SnbMain.MessageQueue.Enqueue(vs[1]);
                            break;
                        case "ULGI":
                            if (!NameCards.Keys.Contains(vs[1]))
                            {
                                NameCards.Add(vs[1], new Chip()
                                {
                                    Content = vs[1]
                                });
                                WrPUsers.Children.Add(NameCards[vs[1]]);
                            }
                            break;
                        case "ULGO":
                            WrPUsers.Children.Remove(NameCards[vs[1]]);
                            NameCards.Remove(vs[1]);
                            break;
                        case "TMIF":
                            if (NameCards.TryGetValue(vs[1], out Chip chip))
                            {
                                if (vs.Length == 3)
                                {
                                    chip.Icon = vs[2];
                                }
                                else
                                {
                                    chip.Icon = null;
                                }
                            }
                            else
                            {
                                NameCards.Add(vs[1], new Chip()
                                {
                                    Content = vs[1],
                                    Icon = vs[2]?.Trim()
                                });
                                WrPUsers.Children.Add(NameCards[vs[1]]);
                            }
                            break;
                        case "TCLR":
                            WrPUsers.Children.Clear();
                            NameCards.Clear();
                            break;
                        case "MBOX":
                            MessageBox.Show(vs[1], "PlaneGame", MessageBoxButton.OK);
                            break;
                        case "GTEM":
                            TeamSelectPage page = new TeamSelectPage(vs[1]);
                            page.Selected += (ch) =>
                            {
                                client.SendMessage("STEM|" + ch);
                            };
                            NowPage = page;
                            break;
                        case "SGTM":
                            if(NowPage is TeamSelectPage)
                            {
                                NowPage = new GameDefaultPage();
                            }
                            break;
                        case "SCOR":
                            if (NameCards.TryGetValue(vs[1], out chip))
                            {
                                if (vs.Length == 3)
                                {
                                    chip.ToolTip = vs[2];
                                }
                                else
                                {
                                    chip.ToolTip = null;
                                }
                            }
                            else
                            {
                                NameCards.Add(vs[1], new Chip()
                                {
                                    Content = vs[1],
                                    ToolTip = vs[2]?.Trim()
                                });
                                WrPUsers.Children.Add(NameCards[vs[1]]);
                            }
                            break;
                        case "SPUS":
                            PutUnitPage p = new PutUnitPage();
                            p.BtnClear.Click += (s, args) =>
                            {
                                if (MaterialDesignMessageBox.Show("是否清空棋盘？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                                {
                                    client.SendMessage("PUCR|");
                                }
                            };
                            p.BtnOK.Click += (s, args) =>
                            {
                                if (MaterialDesignMessageBox.Show("是否确定方案？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                                {
                                    client.SendMessage("FPUS|");
                                }
                            };
                            p.Click += (x, y, name,f,r) =>
                            {
                                client.SendMessage("PUTU|" + name + '|' + x + '|' + y + '|' + f + '|' + r);
                            };
                            if (bool.Parse(vs[1]))
                            {
                                p.BtnXFlip.Visibility = p.BtnYFlip.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                p.BtnXFlip.Visibility = p.BtnYFlip.Visibility = Visibility.Collapsed;
                            }
                            if (bool.Parse(vs[2]))
                            {
                                p.BtnRotate.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                p.BtnRotate.Visibility = Visibility.Hidden;
                            }
                            NowPage = p;
                            break;
                        case "UNIT":
                            if (NowPage is PutUnitPage pg)
                            {
                                pg.UpdateUnit(vs[1], int.Parse(vs[2]), vs[3]);
                            }
                            break;
                        case "UCNT":
                            if (NowPage is PutUnitPage pag)
                            {
                                pag.UpdateUnit(vs[1], int.Parse(vs[2]));
                            }
                            break;
                        case "GBRD":
                            if (NowPage is PutUnitPage pge)
                            {
                                pge.board = new GameBoard.FullPlayerGameBoard(vs[1].Split('\n'));
                            }
                            break;
                        case "ASRT":
                            if(NowPage is PutUnitPage pe)
                            {
                                AttackPage attackPage = new AttackPage(pe.board);
                                attackPage.UnitPreview = pe.UnitPreview;
                                NowPage = attackPage;
                            }                            
                            break;
                    }
                }
                catch (IndexOutOfRangeException e)
                {
                    SnbMain.MessageQueue.Enqueue("收到无效信息，可能网络不稳定", "查看详情", () =>
                    {
                        LongTextDisplayWindow window = new LongTextDisplayWindow(msg + Environment.NewLine + e.Message);
                        window.Show();
                    }, true);
                }
            });
        }

        private void HandleChat(string UserName, string msg)
        {
            SPChat.Children.Add(new TextBlock()
            {
                Text = UserName + " " + DateTime.Now.ToString(),
                Style = Resources["MaterialDesignBody2TextBlock"] as Style,
                TextWrapping = TextWrapping.Wrap
            });
            SPChat.Children.Add(new TextBlock()
            {
                Text = msg,
                Style = Resources["MaterialDesignCaptionTextBlock"] as Style,
                TextWrapping = TextWrapping.Wrap
            });
            SVChat.ScrollToBottom();
        }

        private void Client_FailureCaused(string msg)
        {
            Dispatcher.Invoke(() =>
            {
                SnbMain.MessageQueue.Enqueue(msg);
            });
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            client.Stop();
        }

        private void BtnSendChat_Click(object sender, RoutedEventArgs e)
        {
            client.SendMessage("CHAT|" + TBSendMessage.Text.Trim());
        }

        private void BtnClearChat_Click(object sender, RoutedEventArgs e)
        {
            SPChat.Children.Clear();
        }
    }
}

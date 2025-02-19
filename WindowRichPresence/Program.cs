using DiscordRPC;
using DiscordRPC.Logging;
using System.Runtime.InteropServices;
using System.Text;

namespace WindowRichPresence
{
    internal class Program
    {
        [DllImport("user32")]
        private static extern IntPtr GetForegroundWindow();
        [DllImport("user32", EntryPoint = "SendMessageW", CharSet = CharSet.Unicode)]
        public static extern bool SendMessage(IntPtr hWnd, uint Msg, int wParam, StringBuilder lParam);
        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowTextLength(IntPtr hWnd);

        public static string GetCaptionOfActiveWindow()
        {
            var strTitle = string.Empty;
            var handle = GetForegroundWindow();
            var intLength = GetWindowTextLength(handle) + 1;
            var stringBuilder = new StringBuilder(intLength);
            if (GetWindowText(handle, stringBuilder, intLength) > 0) {
                strTitle = stringBuilder.ToString();
            }
            return strTitle;
        }

        const int WM_GETTEXT = 0x000D;
        public static DiscordRpcClient client;

        static void Main(string[] args)
        {
            Console.WriteLine("WindowPresence has loaded. Status updates will be put below this message.");
            InitializeClient();
            Console.Read();
        }

        public static string GetActiveWindow()
        {
            const int nChars = 256;
            IntPtr handle;
            StringBuilder Buff = new StringBuilder(nChars);
            handle = GetForegroundWindow();
            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return "Could not return process.";
        }

        public static void InitializeClient()
        {
            client = new DiscordRpcClient("1341561589144486069");
            client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };

            client.OnReady += (sender, e) =>
            {
                Console.WriteLine("Received connection from {0}", e.User.Username);
            }
            client.OnPresenceUpdate += (sender, e) =>
            {
                Console.WriteLine("Received Update! {0}", e.Presence);
            };

            client.Initialize();
          
            while (GetCaptionOfActiveWindow != null)
            {
                Thread.Sleep(200); // prevent api spam (?) 
                client.SetPresence(new RichPresence()
                {
                    Details = GetCaptionOfActiveWindow(),
                    State = "Capturing current window...",
                    Assets = new Assets()
                    {
                        LargeImageKey = "icon",
                        LargeImageText = "github.com/byeoon",
                    }
                });
            }
            if (GetCaptionOfActiveWindow == null)
            {
                client.SetPresence(new RichPresence()
                {
                    Details = GetActiveWindow(),
                    State = "Using fallback capture method!",
                    Assets = new Assets()
                    {
                        LargeImageKey = "icon",
                        LargeImageText = "github.com/byeoon",
                    }
                });
            }
        }
        }
    }


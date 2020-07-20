using Roccat_Talk.RyosTalkFX;
namespace RyosMKFXPanel {
    class Lightning {
        private static bool active = false;
        public static bool getStatus() {
            return active;
        }
        public static bool changeStatus() {
            active = ((active == false) ? true : false);
            return true;
        }

        public static byte red = 0;
        public static byte green = 255;
        public static byte blue = 255;
        public static float maxBright = 0.75f;
        public static bool changeLEDs = false;

        public static readonly int kbw = 23;
        public static readonly int kbh = 6;
        public static readonly int kbc = 110;

        public static int delay = 10;
        public static float speed = 1f;

        public static byte[] keysLight = new byte[kbc];
        public static byte[] keysColor = new byte[kbc*3];

        private static RyosTalkFXConnection connection = new RyosTalkFXConnection();

        public static bool connect() {
            connection.Initialize();
            return connection.EnterSdkMode();
        }
        public static bool disconnect() {
            return connection.ExitSdkMode();
        }

        public static void keysLightReset() {
            for (int i = 0; i < kbc; i++) {
                keysLight[i] = 0;
            }
        }
        public static void keysLightAllOn() {
            for (int i = 0; i < kbc; i++) {
                keysLight[i] = 1;
            }
        }
        public static void keysColorReset() {
            for (int i = 0; i < kbc * 3; i += 3) {
                keysColor[i] = 0;
                keysColor[i + 1] = 0;
                keysColor[i + 2] = 0;
            }
        }
        public static void keysColorUpdate() {
            for (int i = 0; i < kbc * 3; i+=3) {
                keysColor[i] = red;
                keysColor[i+1] = green;
                keysColor[i+2] = blue;
            }
        }

        public static void sendPacket() {
            connection.SetMkFxKeyboardState(keysLight, keysColor, 1);
        }
    }
}

/*
    xx  :  00  xx  01  02  03  04 :  05  06  07  08 :  09  10  11  12  :  13  14  15  :  xx  xx  xx  xx
    ....:.........................:.................:..................:..............:................
    16  :  17  18  19  20  21  22  23  24  25  26  27  28  29  3____0  :  31  32  33  :  34  35  36  37
    38  :  3___9 40  41  42  43  44  45  46  47  48  49  50  51 5___2  :  53  54  55  :  56  57  58  5x
    60  :  6____1 62  63  64  65  66  67  68  69  70  71  72  7_____3  :  nm  sh  sc  :  74  75  76  x9
    77  :  7_____8  80  81  82  83  84  85  86  87  88  89  9_______0  :  xx  91  xx  :  92  93  94  9y
    96  :  9___7 98  99  1______________________00  101  102 103 1_04  :  105 106 107 :  1___08 109  y5 

    000  :  001  002  003  004  005  006  007  008  009  010  011  012  013  014  015  :  016  017  018  :  019  020  021  022
    .....:..............................:...................:..........................:.................:....................
    023  :  024  025  026  027  028  029  030  031  032  033  034  035  036  037  038  :  039  040  041  :  042  043  044  045
    046  :  047  048  049  050  051  052  053  054  055  056  057  058  059  060  061  :  062  063  064  :  065  066  067  068
    069  :  070  071  072  073  074  075  076  077  078  079  080  081  082  083  084  :  085  086  087  :  089  090  091  092
    093  :  094  095  096  097  098  099  100  101  102  103  104  105  106  107  108  :  109  110  111  :  112  113  114  115
    116  :  117  118  119  120  121  122  123  124  125  126  127  128  129  130  131  :  132  133  134  :  135  136  137  138

    005  :  015  025  035  045  055  065  075  085  095  105  115  125  135  145  155  :  165  175  185  :  195  205  215  225
    .....:..............................:...................:..........................:.................:....................
    004  :  014  024  034  044  054  064  074  084  094  104  114  124  134  144  154  :  164  174  184  :  194  204  214  224
    003  :  013  023  033  043  053  063  073  083  093  103  113  123  133  143  153  :  163  173  183  :  193  203  213  223
    002  :  012  022  032  042  052  062  072  082  092  102  112  122  132  142  152  :  162  172  182  :  192  202  212  222
    001  :  011  021  031  041  051  061  071  081  091  101  111  121  131  141  151  :  161  171  181  :  191  201  211  221
    000  :  010  020  030  040  050  060  070  080  090  100  110  120  130  140  150  :  160  170  180  :  190  200  210  220
*/

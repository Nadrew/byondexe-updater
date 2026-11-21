namespace byondexe_updater.Helpers.Output {
    public static class Output {
        public const int MESSAGE_LEVEL = 0;
        public const int VERBOSE_LEVEL = 1;
        public const int ERROR_LEVEL = 2;
        public static int CURRENT_LEVEL = MESSAGE_LEVEL;

        private const int MAX_MESSAGE_LENGTH = 80;
        private static string lastMessage = string.Empty;
        public static void Write(string message,int level = MESSAGE_LEVEL) {
            bool hasNewLine = message.EndsWith('\n');
            if (level <= CURRENT_LEVEL) {
                if (message.Length > MAX_MESSAGE_LENGTH) {
                    message = message[..(MAX_MESSAGE_LENGTH - 3)] + "...";
                    if (hasNewLine) {
                        message += '\n';
                    }
                }
                Console.Write(message);
                lastMessage = message;
            }
        }
        public static void WriteLong(string message, int level = MESSAGE_LEVEL) {
            if (level <= CURRENT_LEVEL) {
                Console.WriteLine(message);
            }
        }
        public static void WriteStatus(string message, int level = MESSAGE_LEVEL,ConsoleColor color = ConsoleColor.White) {
            if (level <= CURRENT_LEVEL) {
                int targetColumn = Math.Max(lastMessage.Length + 1, Console.WindowWidth - message.Length);
                Console.SetCursorPosition(targetColumn, Console.CursorTop);
                Console.ForegroundColor = color;
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }
    }
}
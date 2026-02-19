using Telegram.Bot;

const string BOT_TOKEN = "8531825376:AAFlj2Y9MPoveIlctmTbQXldGO1YswWa-qM";
// "8202838615:AAH4_8xiVAV4-fuQeJb-w4gifupgcq5c76A";

const long CHAT_ID = 8249947199;
// 1580155953; 

Console.WriteLine("Bot başlatılıyor...");

var botClient = new TelegramBotClient(BOT_TOKEN);

// Bağlantıyı test et
var me = await botClient.GetMe();
Console.WriteLine($"Bot bağlandı: @{me.Username}");

Console.WriteLine("Konsola 'merhaba' yazın ve Enter'a basın...");

while (true)
{
    string? input = Console.ReadLine();

    if (input?.ToLower() == "merhaba")
    {
        Console.WriteLine("'merhaba' algılandı. 10 saniye bekleniyor...");

        await Task.Delay(5000); // 10 saniye bekle

        await botClient.SendMessage(
            chatId: CHAT_ID,
            text: "Merhaba! 👋"
        );

        Console.WriteLine("Telegram'a mesaj gönderildi!");
    }
    else if (input?.ToLower() == "çıkış" || input?.ToLower() == "cikis")
    {
        Console.WriteLine("Program kapatılıyor...");
        break;
    }
    else
    {
        Console.WriteLine("Komut tanınmadı. 'merhaba' yazın.");
    }
}

using Telegram.Bot;
using System.Text.Json;

// AYARLAR
var kullanicilar = new List<(string BotToken, long ChatId, string Ad)>
{
    ("8202838615:AAH4_8xiVAV4-fuQeJb-w4gifupgcq5c76A", 1580155953, "Melis"),
 // ("8531825376:AAFlj2Y9MPoveIlctmTbQXldGO1YswWa-qM", 8249947199, "Mert"),
 // ("8669147108:AAGLS6EAVxcAaxpcF-sW677UorKXEMAizrQ", 5266256920, "Mehmet")
};

// Mesaj saatleri
var mesajSaatleri = new List<TimeOnly>
{
    new TimeOnly(10, 0),  // Sabah 10:00
    new TimeOnly(16, 0)   // Öğleden sonra 16:00
};

// ====================================================

Console.WriteLine("✅ Bot başlatılıyor...");
Console.WriteLine($"👥 Mesaj alacaklar: {string.Join(", ", kullanicilar.Select(k => k.Ad))}");
Console.WriteLine("📅 Mesaj saatleri: 10:00 ve 16:00");
Console.WriteLine("⌨️  Manuel gönderim için 'gonder' yazın, çıkış için 'cikis'\n");

var zamanlayiciTask = ZamanlanmisGonderimBaslat(kullanicilar, mesajSaatleri);

while (true)
{
    string? input = Console.ReadLine();

    if (input?.ToLower() == "gonder")
    {
        Console.WriteLine("Manuel gönderim yapılıyor...");
        await HerkeseMesajGonder(kullanicilar);
    }
    else if (input?.ToLower() == "cikis")
    {
        Console.WriteLine("Program kapatılıyor...");
        break;
    }
    else
    {
        Console.WriteLine("Komut tanınmadı. 'gonder' veya 'cikis' yazın.");
    }
}

// ZAMANLAYICI
async Task ZamanlanmisGonderimBaslat(List<(string BotToken, long ChatId, string Ad)> kullaniciListesi, List<TimeOnly> saatler)
{
    while (true)
    {
        var simdi = DateTime.Now;
        var simdikiSaat = TimeOnly.FromDateTime(simdi);

        TimeOnly? sonrakiSaat = null;
        foreach (var saat in saatler.OrderBy(s => s))
        {
            if (saat > simdikiSaat)
            {
                sonrakiSaat = saat;
                break;
            }
        }

        DateTime sonrakiGonderim;
        if (sonrakiSaat.HasValue)
        {
            sonrakiGonderim = simdi.Date.Add(sonrakiSaat.Value.ToTimeSpan());
        }
        else
        {
            var ilkSaat = saatler.OrderBy(s => s).First();
            sonrakiGonderim = simdi.Date.AddDays(1).Add(ilkSaat.ToTimeSpan());
        }

        var beklemeSuresi = sonrakiGonderim - DateTime.Now;
        Console.WriteLine($"Sonraki otomatik gönderim: {sonrakiGonderim:HH:mm} ({(int)beklemeSuresi.TotalMinutes} dakika sonra)");

        await Task.Delay(beklemeSuresi);
        await HerkeseMesajGonder(kullaniciListesi);
    }
}

// HERKESE MESAJ GÖNDER
async Task HerkeseMesajGonder(List<(string BotToken, long ChatId, string Ad)> kullaniciListesi)
{
    string mesaj = await KurMesajiOlustur();

    foreach (var kullanici in kullaniciListesi)
    {
        try
        {
            var bot = new TelegramBotClient(kullanici.BotToken);
            await bot.SendMessage(
                chatId: kullanici.ChatId,
                text: mesaj,
                parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown
            );
            Console.WriteLine($"{kullanici.Ad}'e mesaj gönderildi! [{DateTime.Now:HH:mm:ss}]");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{kullanici.Ad}'e gönderilemedi: {ex.Message}");
        }
    }
}

// KUR METNİ OLUŞTUR
async Task<string> KurMesajiOlustur()
{
    try
    {
        Console.WriteLine("Kur verileri çekiliyor...");

        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");

        // Döviz: USD, EUR
        var dovizJson = await httpClient.GetStringAsync("https://api.genelpara.com/json/?list=doviz&sembol=USD,EUR");
        var dovizData = JsonDocument.Parse(dovizJson);

        // Altın: GA = Gram Altın, GAG = Gram Gümüş
        var altinJson = await httpClient.GetStringAsync("https://api.genelpara.com/json/?list=altin&sembol=GA,GAG");
        var altinData = JsonDocument.Parse(altinJson);

        var usd   = dovizData.RootElement.GetProperty("data").GetProperty("USD");
        var eur   = dovizData.RootElement.GetProperty("data").GetProperty("EUR");
        var altin = altinData.RootElement.GetProperty("data").GetProperty("GA");
        var gumus = altinData.RootElement.GetProperty("data").GetProperty("GAG");

        string UsdSatis     = usd.GetProperty("satis").GetString()    ?? "-";
        string UsdDegisim   = usd.GetProperty("degisim").GetString()  ?? "0";
        string EurSatis     = eur.GetProperty("satis").GetString()    ?? "-";
        string EurDegisim   = eur.GetProperty("degisim").GetString()  ?? "0";
        string AltinSatis   = altin.GetProperty("satis").GetString()  ?? "-";
        string AltinDegisim = altin.GetProperty("degisim").GetString()?? "0";
        string GumusSatis   = gumus.GetProperty("satis").GetString()  ?? "-";
        string GumusDegisim = gumus.GetProperty("degisim").GetString()?? "0";

        string YonEmoji(string degisim) =>
            degisim.StartsWith("-") ? "📉" : degisim.StartsWith("+") ? "📈" : "⚪";

        string saat  = DateTime.Now.ToString("HH:mm");
        string tarih = DateTime.Now.ToString("dd.MM.yyyy");

        return $"""
*Güncel Piyasa Verileri*
{tarih} - {saat}
━━━━━━━━━━━━━━━━━━━━

💵 *Dolar (USD)*
   Satış: `{UsdSatis} ₺`  {YonEmoji(UsdDegisim)} {UsdDegisim}

💶 *Euro (EUR)*
   Satış: `{EurSatis} ₺`  {YonEmoji(EurDegisim)} {EurDegisim}

🥇 *Gram Altın*
   Satış: `{AltinSatis} ₺`  {YonEmoji(AltinDegisim)} {AltinDegisim}

🥈 *Gram Gümüş*
   Satış: `{GumusSatis} ₺`  {YonEmoji(GumusDegisim)} {GumusDegisim}

━━━━━━━━━━━━━━━━━━━━
📊 _Kaynak: GenelPara API_
""";
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Kur verisi alınamadı: {ex.Message}");
        return "⚠️ Kur verileri şu an alınamıyor, lütfen daha sonra tekrar deneyin.";
    }
}
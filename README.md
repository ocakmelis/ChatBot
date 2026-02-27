# 📈 Telegram Kur Botu

Günlük döviz, altın ve gümüş fiyatlarını otomatik olarak Telegram'a gönderen bir bot.

## Özellikler

- Her gün saat **10:00** ve **16:00**'da otomatik mesaj gönderir
- **USD, EUR, Gram Altın ve Gram Gümüş** verilerini anlık olarak çeker
- Fiyat yönünü 📈 / 📉 emojileriyle gösterir
- Birden fazla kullanıcıya aynı anda mesaj atabilir
- Konsola `gonder` yazarak manuel olarak da tetiklenebilir

##  Kullanılan Teknolojiler

- **C# / .NET 9**
- **Telegram.Bot** — Telegram mesaj gönderimi
- **GenelPara API** — Gerçek zamanlık kur verileri

## API

Kur verileri [GenelPara API](https://api.genelpara.com) üzerinden çekilmektedir.

| Veri | Endpoint |
|------|----------|
| Döviz (USD, EUR) | `https://api.genelpara.com/json/?list=doviz&sembol=USD,EUR` |
| Altın & Gümüş | `https://api.genelpara.com/json/?list=altin&sembol=GA,GAG` |

## Kurulum

1. Repoyu klonlayın:
   ```bash
   git clone https://github.com/ocakmelis/ChatBot.git
   cd ChatBot
   ```

2. Telegram.Bot paketini yükleyin:
   ```bash
   dotnet add package Telegram.Bot
   ```

3. `Program.cs` dosyasında kendi bot token ve chat ID bilgilerinizi girin:
   ```csharp
   var kullanicilar = new List<(string BotToken, long ChatId, string Ad)>
   {
       ("BOT_TOKEN", CHAT_ID, "Ad")
   };
   ```

4. Programı çalıştırın:
   ```bash
   dotnet run
   ```

## Konsol Komutları

| `gonder` | Anlık olarak kur mesajı gönderir |
| `cikis` | Programı kapatır |

## Örnek Mesaj

```
Güncel Piyasa Verileri
27.02.2026 - 10:00
━━━━━━━━━━━━━━━━━━━━

💵 Dolar (USD)
   Satış: 43.96 ₺  📈 +0.18

💶 Euro (EUR)
   Satış: 51.90 ₺  📉 -0.04

🥇 Gram Altın
   Satış: 7311.49 ₺  📉 -0.32

🥈 Gram Gümüş
   Satış: 126.69 ₺  📈 +1.02

━━━━━━━━━━━━━━━━━━━━
📊 Kaynak: GenelPara API
```

## 📱 Uygulama Çıktısı

![Telegram Bot Çıktısı](<img src="https://github.com/user-attachments/assets/2e41ea6c-b226-42d0-a31d-235849cff6d6" width="400"/>)





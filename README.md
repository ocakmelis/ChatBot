# Telegram Bot - .NET 8

## Kurulum Adımları

### 1. Bot Token Alma
1. Telegram'da **@BotFather**'a gidin
2. `/newbot` yazın
3. Bot ismi girin (örnek: MerhabaBot)
4. Kullanıcı adı girin (örnek: MerhabaTestBot)
5. Verilen **token**'ı kopyalayın

### 2. Chat ID Bulma
1. Telegram'da **@userinfobot**'a gidin
2. `/start` yazın
3. Size **Id** numaranızı verecek, onu kopyalayın

### 3. Program.cs Dosyasını Düzenle
```csharp
const string BOT_TOKEN = "BURAYA_BOT_TOKEN_YAZIN";  // BotFather'dan aldığınız token
const long CHAT_ID = 123456789;  // userinfobot'tan aldığınız ID
```

### 4. Projeyi Çalıştır
```bash
dotnet restore
dotnet run
```

### 5. Kullanım
- Konsola `merhaba` yazın ve Enter'a basın
- 10 saniye sonra Telegram'ınıza "Merhaba! 👋" mesajı gelecek
- Çıkmak için `çıkış` veya `cikis` yazın

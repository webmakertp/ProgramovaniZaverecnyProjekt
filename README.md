# Game Tracker

Toto je muj zaverecny projekt do programovani. Je to jednoducha desktopova aplikace v Avalonia UI na sledovani her a hernich sessionu.

## Co to dela

- Umoznuje pridavat hry s nazvem, vyvojarem, rokem vydani a platformou
- Kazde hre muzes pridavat sessiony (kdy jsi hral a jak dlouho)
- Data se ukladaji do PostgreSQL databaze
- Aplikace pouziva MVVM, Repository pattern a Dependency Injection

## Pozadavky

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- Volny port 5432 na localhostu

## Jak to spustit

1. Ujisti se ze mas nainstalovany Docker
2. Prejdi do slozky s projektem
3. Vytvor soubor `.env` podle `.env.example` (zkopiruj ho)
4. Spust `docker compose up -d` pro nastartovani PostgreSQL
5. Spust aplikaci pres `dotnet run`

## Struktura projektu

```
GameTrackerApp/
├── Models/           # Domenove modely (Game, GameSession, Platform)
├── Repositories/     # Repository pattern (CRUD operace nad DB)
├── ViewModels/       # MVVM ViewModely
├── Views/            # Avalonia XAML view
├── Services.cs       # Dependency Injection kontejner
├── docker-compose.yaml
├── schema.sql        # CREATE TABLE skripty
├── seed.sql          # Naplneni ciselniku platform
└── .env.example      # Ukazkove nastaveni
```

## Troubleshooting

| Problem | Reseni |
|---------|--------|
| `chybi DB_CONNECTION_STRING v .env` | Vytvor soubor `.env` podle `.env.example` |
| Port 5432 je obsazeny | Zastav jiny PostgreSQL nebo zmen port v `docker-compose.yaml` |
| Databaze se nevytvorila | Smaz Docker volume `pgdata` a restartuj container |
| `dotnet run` neni k dispozici | Nainstaluj .NET 10 SDK |

## Poznamky

- Databaze se automaticky vytvori a naplni daty pri prvnim startu containeru
- `.env` soubor s heslem neni v repozitari, je v `.gitignore`
- Pri mazani hry se automaticky smazou i jeji sessiony (ON DELETE CASCADE)

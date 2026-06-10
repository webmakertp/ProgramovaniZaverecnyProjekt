# Game Tracker

Moje závěrečná práce z programování — desktopová aplikace v Avalonia UI na sledování her a herních sessionů.

## Co to dělá

- Umožňuje přidávat hry s názvem, vývojářem, rokem vydání a platformou
- Ke každé hře si můžeš zapisovat sessiony (kdy jsi hrál a jak dlouho)
- Data se ukládají do PostgreSQL databáze, takže vše přežije i restart
- Aplikace používá MVVM, Repository pattern a Dependency Injection

## Co potřebuješ

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- Volný port 5432 na localhostu

## Jak to rozjet

1. Ujisti se, že máš nainstalovaný Docker
2. Otevři terminál ve složce s projektem (tam, kde je `docker-compose.yaml`)
3. Vytvoř soubor `.env` podle `.env.example` — prostě ho zkopíruj a přejmenuj
4. Spusť databázi přes `docker compose up -d`
5. Spusť aplikaci přes `dotnet run`

Databáze se při prvním startu kontejneru sama vytvoří a naplní se základními platformami.

## Struktura projektu

```
GameTrackerApp/
├── Models/           # Modely (Game, GameSession, Platform)
├── Repositories/     # Repository pattern (CRUD nad databází)
├── ViewModels/       # MVVM ViewModely
├── Views/            # Avalonia XAML view
├── Services.cs       # Dependency Injection kontejner
├── docker-compose.yaml
├── schema.sql        # CREATE TABLE
├── seed.sql          # Naplnění číselníku platform
└── .env.example      # Ukázkové nastavení
```

## Řešení problémů

| Problém | Řešení |
|---------|--------|
| `chybí DB_CONNECTION_STRING v .env` | Vytvoř soubor `.env` podle `.env.example` |
| Port 5432 je obsazený | Zastav jiný PostgreSQL nebo změň port v `docker-compose.yaml` |
| Databáze se nevytvořila | Smaž Docker volume `pgdata` a restartuj kontejner |
| `dotnet run` není k dispozici | Nainstaluj .NET 10 SDK |

## Poznámky

- `.env` soubor s heslem není v repozitáři, je v `.gitignore`
- Při mazání hry se automaticky smažou i její sessiony (ON DELETE CASCADE)

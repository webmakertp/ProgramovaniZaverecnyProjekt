# Game Tracker

Toto je muj zaverecny projekt do programovani. Je to jednoducha desktopova aplikace v Avalonia UI na sledovani her a hernich sessionu.

## Co to dela

- Umoznuje pridavat hry s nazvem, vyvojarem, rokem vydani a platformou
- Kazde hre muzes pridavat sessiony (kdy jsi hral a jak dlouho)
- Data se ukladaji do PostgreSQL databaze
- Aplikace pouziva MVVM, Repository pattern a Dependency Injection

## Jak to spustit

1. Ujisti se ze mas nainstalovany Docker
2. Prejdi do slozky s projektem
3. Spust `docker compose up -d` pro nastartovani PostgreSQL
4. Vytvor soubor `.env` podle `.env.example` (zkopiruj ho)
5. Spust aplikaci pres `dotnet run`

## Poznamky

- Databaze se automaticky vytvori a naplni daty pri prvnim startu containeru
- .env soubor s connection stringem neni v repozitari, je v .gitignore

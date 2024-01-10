# .savegame

Keep track of your games!

## Screenshots

## Tech

SaveGame is a windows desktop application powered by:
- C#/.NET WPF
- MVVM via [CommunityToolkit.Mvvm](https://www.nuget.org/packages/CommunityToolkit.Mvvm)
- Dependency Injection via [Extensions.Hosting](https://www.nuget.org/packages/Microsoft.Extensions.Hosting)
- IGDB API via [igdb-dotnet](https://github.com/kamranayub/igdb-dotnet)
- Lottie JSON animations via [LottieSharp](https://github.com/quicoli/LottieSharp)
- Persistent (NoSQL) data via [LiteDB](https://www.litedb.org/)

> [!Note]
> Binaries are currently not offered.  
> See [build section](#building-or-running-you-own-savegame-application) on instructions to compile the project.

## Building or Running your own .savegame application

#### Pre-requisities
1. Visual Studio or other IDE
2. .NET 8.0
3. Twitch Client ID and Secrets for IGDB API authentication (Obtain from [Twitch Developer Console](https://dev.twitch.tv/console/apps))

#### Build or Run with environment variables:
```shell
export IGDB_CLIENT_ID=[your OAuth app client ID]
export IGDB_CLIENT_SECRET=[your OAuth app client secret]
```

# Soldier Total Column - Xenonauts 2 mod

A QoL mod for [Xenonauts 2](https://store.steampowered.com/app/538030/Xenonauts_2/). Adds a column to the Soldiers screen showing the sum of all stats, so you can spot the soldiers with the highest potential more easily.

## Install

1. Download the latest `soldier_total_column-*.zip` from the [Releases page](https://github.com/mnshdw/X2-SoldierTotalColumn/releases).
2. Extract into your Xenonauts 2 user mods folder:
   - **Windows:** `Documents\My Games\Xenonauts 2\Mods\`
   - **Linux (Steam Proton):** `~/.local/share/Steam/steamapps/compatdata/538030/pfx/drive_c/users/steamuser/AppData/LocalLow/Goldhawk Interactive/Xenonauts 2/`
3. Launch Xenonauts 2 -> main menu -> **Mods** -> enable **Soldier Total Column** -> restart.

## Build from source

Requires the [.NET SDK](https://dotnet.microsoft.com/download) (8.0 or later) and a Xenonauts 2 install.

```sh
cp Directory.Build.props.template Directory.Build.props
# edit the three paths in Directory.Build.props to match your machine
dotnet build -c Release
```

The build emits `bin/Release/netstandard2.1/SoldierTotalColumn.dll` and also copies it (plus the manifest) to `$(ModInstanceFolder)` so the game picks it up immediately.

## Cut a release

```sh
./release.sh
```

Produces `dist/soldier_total_column-<version>.zip` ready to attach to a GitHub Release. Version is read from `mod/manifest.json`.

## Screenshots

## License

[MIT](LICENSE).

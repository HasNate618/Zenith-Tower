# Third-party notices

This project was built in Unity 2020.3.34f1 and shipped on the Google Play
Store. Some of the content it used could not be redistributed, so it has been
excluded from this repository. This file records what is included, what is
excluded, and how to restore the exclusions.

The MIT license in `LICENSE` covers the game code authored for this project.
It does not apply to the third-party components listed below, which remain
under their own terms.

## Included

### NavMeshComponents

- Location: `Assets/Thirdparty/NavMeshComponents-master/`
- Author: Unity Technologies
- License: MIT — full text at `Assets/Thirdparty/NavMeshComponents-master/LICENSE`
- Upstream: https://github.com/Unity-Technologies/NavMeshComponents

Used for runtime NavMesh baking. Sixteen scenes and prefabs reference
`NavMeshSurface`, so this dependency is required for the levels to bake
correctly and is redistributable under MIT.

### Fira Sans

- Location: `Assets/Fira/`
- Author: The Mozilla Foundation and Telefonica S.A.
- License: SIL Open Font License 1.1 — full text at `Assets/Fira/OFL.txt`

### Music

- Location: `Assets/Audio/Music/`
- Source: Pixabay
- License: Pixabay Content License — https://pixabay.com/service/license-summary/

Six ambient and electronic tracks used as the level and menu soundtrack.

## Excluded

The following were used during development and remain on the original
developer's machine, but are **not** part of this repository. A clean clone
will not compile until they are restored.

### WarFX v1.8

- Original location: `Assets/Thirdparty/JMO Assets/`
- Author: Jean Moreno (JMO Assets)
- License: Standard Unity Asset Store EULA (free asset, "Extension Asset")
- Source: https://assetstore.unity.com/packages/vfx/particles/war-fx-5669

Particle effects used throughout the game. The Asset Store EULA permits
shipping the assets inside a built application but not redistributing the
source files, so the package directory is gitignored.

Consequence: VFX prefabs in `Assets/VFX/` and `Assets/Prefabs/` will show
missing material and particle references until the package is re-imported.

### Joystick Pack v2.1

- Original location: `Assets/Thirdparty/Joystick Pack/`
- Author: Fenerax Studios
- License: Standard Unity Asset Store EULA (free asset, "Extension Asset")
- Source: https://assetstore.unity.com/packages/tools/input-management/joystick-pack-107631

On-screen virtual joysticks for mobile input. Same redistribution restriction
as above.

Consequence: this is a **compile-time** dependency. `CharacterMovement` and
`CharacterShooting` both declare a `Joystick` field, so the project will not
compile until the package is imported. It is free and takes under a minute.

## Restoring excluded packages

1. Open the project once in Unity 2020.3.34f1 (Unity will regenerate
   `Library/`, `*.csproj`, and the solution files).
2. Import WarFX and Joystick Pack from the Asset Store links above.
3. Let Unity finish importing, then reopen the project.

Unity stores asset references as GUIDs inside each `.meta` file. The Asset
Store packages ship their own `.meta` files with fixed GUIDs, so importing the
same package versions listed here restores the original references and the
scenes reconnect without manual reassignment.

## Removed for other reasons

- `Key.keystore` — the Android signing keystore. Removed for security; not
  required to build or run the project in the editor.
- `Library/`, `obj/`, `.vs/`, `*.csproj`, `*.sln` — generated Unity and IDE
  artifacts, rebuilt automatically and never meant to be tracked.

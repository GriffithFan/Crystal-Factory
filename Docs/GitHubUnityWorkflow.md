# Flujo recomendado: Unity + VS Code Agent + GitHub

Si, es posible y recomendable trabajar asi:

- Unity Editor para escenas, prefabs, Canvas, sprites, audio, configuracion Android y pruebas Play Mode.
- VS Code para codigo, arquitectura, documentacion, balance JSON, revision y ayuda del agente.
- GitHub como punto central para subir avances, recuperar versiones y mantener contexto.

## Idea del flujo

1. Haces cambios de codigo o docs desde VS Code.
2. Guardas, revisas errores y haces commit.
3. Subes a GitHub con push.
4. Abres Unity, dejas que compile y pruebas.
5. Si modificas escenas, prefabs o assets desde Unity, guardas el proyecto.
6. Vuelves a VS Code, revisas cambios Git y haces commit/push.
7. El agente de VS Code puede seguir viendo el repo actualizado.

## Configuracion importante en Unity

En Unity, revisa estas opciones:

1. `Edit > Project Settings > Editor`
2. `Version Control > Mode`: `Visible Meta Files`
3. `Asset Serialization > Mode`: `Force Text`

Esto es clave para que Git pueda seguir escenas, prefabs y assets correctamente.

## Que archivos subir a GitHub

Subir:

- `Assets/`
- `ProjectSettings/`
- `Packages/`
- `Docs/`
- `README.md`
- `Juego.txt`
- `.gitignore`
- `.gitattributes`

No subir:

- `Library/`
- `Temp/`
- `Obj/`
- `Build/`
- `Builds/`
- `Logs/`
- `UserSettings/`
- APK/AAB finales si pesan mucho.

El `.gitignore` ya esta preparado para esto.

## Primer push a GitHub

Primero crea un repositorio vacio en GitHub, por ejemplo:

`crystal-factory-idle`

Despues, desde la terminal en esta carpeta:

```powershell
git status
git add .
git commit -m "Initial Unity idle clicker prototype"
git branch -M main
git remote add origin https://github.com/TU_USUARIO/crystal-factory-idle.git
git push -u origin main
```

Tambien puedes hacerlo desde la pestaña Source Control de VS Code si ya iniciaste sesion con GitHub.

## Trabajo diario recomendado

Antes de abrir Unity:

```powershell
git pull
```

Despues de trabajar en Unity:

```powershell
git status
git add .
git commit -m "Update Unity scene and prefabs"
git push
```

Despues de trabajar con el agente en VS Code:

```powershell
git status
git add .
git commit -m "Add gameplay systems"
git push
```

## Regla practica

- Cambios de codigo: hacerlos en VS Code.
- Cambios visuales: hacerlos en Unity.
- Cambios importantes: documentarlos en `Docs/DevelopmentLog.md`.
- Antes de pedirle algo grande al agente: hacer pull.
- Despues de que el agente termine algo util: hacer commit y push.

## Sobre imagenes generadas por IA

Puedes agregar imagenes generadas por IA dentro de:

- `Assets/Art/Backgrounds/`
- `Assets/Art/Icons/`
- `Assets/Art/Machines/`
- `Assets/Art/UI/`

Recomendacion:

- Usar PNG optimizados.
- Mantener nombres claros.
- No subir archivos enormes sin necesidad.
- Si empiezan a pesar demasiado, conviene configurar Git LFS.

## Git LFS opcional

Para proyectos con muchas imagenes, audio o modelos grandes, conviene usar Git LFS.

Instalacion:

```powershell
git lfs install
```

Luego se pueden trackear tipos grandes:

```powershell
git lfs track "*.png"
git lfs track "*.psd"
git lfs track "*.wav"
git lfs track "*.fbx"
```

No lo active automaticamente todavia para evitar depender de Git LFS antes de confirmar que lo quieres usar.

## Como usar el agente de VS Code con Unity

El agente no necesita ejecutarse dentro de Unity. Solo necesita que el proyecto Unity este en archivos normales dentro del repo.

Puede ayudarte con:

- Scripts C#.
- Arquitectura.
- Balance JSON.
- Documentacion.
- Refactors.
- Errores de compilacion si los copias o si VS Code los detecta.
- Preparar integraciones de Ads/IAP/Firebase.

Unity sigue siendo necesario para:

- Play Mode.
- Inspector.
- Escenas.
- Prefabs.
- Sprites y Canvas visual.
- Build Android.

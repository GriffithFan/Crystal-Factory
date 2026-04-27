# Registro de progreso y contexto

Fecha: 27/04/2026

## Vision del proyecto

Juego mobile Android hecho en Unity 2D, tipo idle clicker/incremental, pensado para ganancias sostenibles mediante retencion, rewarded ads, Remove Ads y compras opcionales.

Tema recomendado actual: **Fabrica de Cristales Cosmica**.

## Principios de monetizacion

- Primero retencion y satisfaccion del loop.
- Rewarded ads como aceleradores voluntarios.
- Interstitials limitados y nunca despues de cada toque.
- Remove Ads como compra clave.
- Compras para ahorrar tiempo, no para desbloquear diversion basica.

## Progreso implementado desde VS Code

### Preparacion Git/GitHub

- Repositorio Git local inicializado.
- `.gitignore` preparado para Unity.
- `.gitattributes` agregado para tratar correctamente archivos de texto/binarios.
- Guia `Docs/GitHubUnityWorkflow.md` creada.
- Guia `Docs/GitCommands.md` creada.

### Base de proyecto

- Estructura `Assets/` compatible con Unity.
- `Packages/manifest.json` agregado con TextMeshPro, UGUI y soporte VS Code/Visual Studio.
- `ProjectSettings/ProjectVersion.txt` agregado para que Unity Hub reconozca la carpeta como proyecto.
- Version del proyecto ajustada a Unity `6000.4.4f1`, que es la version instalada detectada en Unity Hub.
- Documentacion en `Docs/`.
- Catalogos JSON en `Assets/Resources/GameData/`.

### Sistemas principales

- `GameManager`: coordina los sistemas.
- `SaveManager`: guardado local JSON en PlayerPrefs.
- `GameState`: estado persistente del jugador.
- `CurrencySystem`: cristales, poder por toque, cristales por segundo.
- `UpgradeSystem`: compra y aplica mejoras.
- `OfflineProgressSystem`: recompensa al volver despues de cerrar.
- `DailyRewardSystem`: recompensa diaria con racha.
- `BoostSystem`: activa boost temporal x2 al completar un rewarded ad simulado.
- `MissionSystem`: misiones basicas conectadas al loop.
- `StageSystem`: desbloquea etapas narrativas segun cristales totales.
- `AdsManager`: rewarded ads simulados.
- `IAPManager`: compras simuladas.
- `AnalyticsManager`: eventos simulados por Debug.Log.

### UI

- `HUDController`: conecta contador, cristal, recompensa diaria y anuncio.
- `ShopController`: construye la tienda desde catalogo de upgrades.
- `UpgradeButtonView`: fila/boton de mejora.
- `MissionPanelController`: panel de misiones.
- `MissionRowView`: fila de mision.
- `StageDisplayController`: muestra etapa actual, lore y colores del fondo.
- `PopupController`: popup generico preparado.

### Feedback visual

- `CrystalPulseController`: pulso del cristal al tocar.
- `FloatingTextSpawner`: texto flotante para clicks y recompensas.
- `AmbientPulseController`: pulso suave de energia en el fondo.

### Datos configurables

- `UpgradeCatalog.json`: 10 upgrades iniciales.
- `MissionCatalog.json`: 5 misiones iniciales.
- `StageCatalog.json`: 6 etapas narrativas con paleta visual y lore.

### Automatizacion Unity Editor

- `PrototypeSceneBuilder`: crea una escena prototipo desde Unity.
- Ruta esperada del menu: `Tools > Cosmic Crystal Factory > Create Prototype Scene`.
- Genera `Assets/Scenes/Game.unity`.
- Crea GameManager con todos los componentes.
- Crea Canvas, HUD, botones, tienda y panel de misiones.
- Crea prefabs UI iniciales para upgrades y misiones.
- Crea fondo dinamico, texto de lore, pulso de cristal y texto flotante.

### Direccion visual/lore

- `Docs/ArtAndLoreDirection.md` creado.
- Carpetas `Assets/Art/` y `Assets/Audio/` preparadas.
- Prompts IA definidos para cristal, fondos e iconos.

## Estado actual

La logica base del MVP esta lista para compilar en Unity. Falta abrir el proyecto en Unity, dejar que compile, ejecutar el generador de escena y probar Play Mode.

## Proximo objetivo

Desde Unity:

1. Abrir el proyecto.
2. Esperar compilacion.
3. Ejecutar `Tools > Cosmic Crystal Factory > Create Prototype Scene`.
4. Abrir `Assets/Scenes/Game.unity` si no se abre automaticamente.
5. Presionar Play.
6. Probar click, upgrades, misiones, rewarded ad simulado y guardado.

## Riesgos tecnicos pendientes

- Confirmar que TextMeshPro este instalado/importado en el proyecto Unity.
- Confirmar que la version de Unity soporte el runtime usado.
- Ajustar visualmente el Canvas generado; es funcional, no arte final.
- Reemplazar ads/IAP simulados por SDKs reales cuando el loop ya funcione.

## Siguiente bloque de desarrollo recomendado

- Pulir balance inicial despues de probar el ritmo real.
- Agregar popup visual para offline reward y mission claimed.
- Agregar arte provisional o imagenes IA para cristal/fondo/iconos.
- Preparar integracion real de AdMob o Unity LevelPlay.

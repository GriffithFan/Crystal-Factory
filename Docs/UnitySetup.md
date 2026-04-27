# Montaje inicial en Unity

Este workspace ya contiene una base de scripts para el MVP de **Fabrica de Cristales Cosmica**, un idle clicker 2D para Android.

## 1. Crear el proyecto

1. Abre Unity Hub.
2. Usa **Add project from disk** y selecciona `c:\Users\ulise\Documents\JUEGO`.
3. Abre el proyecto con Unity `6000.4.4f1` o con la version instalada que tengas disponible.
4. Si Unity avisa que va a actualizar el proyecto a tu version instalada, acepta.

## 1.1. Crear escena automaticamente

Ya hay una herramienta de editor para montar el prototipo inicial sin armar todo a mano.

1. Abre el proyecto en Unity.
2. Espera a que Unity termine de compilar los scripts.
3. Si Unity pide importar TextMeshPro Essentials, aceptalo.
4. En el menu superior ejecuta:

`Tools > Cosmic Crystal Factory > Create Prototype Scene`

Esto crea:

- `Assets/Scenes/Game.unity`
- Un `GameManager` con todos los sistemas.
- Un Canvas basico con fondo dinamico, lore, contador, cristal, botones, tienda y misiones.
- Prefabs UI iniciales para upgrades y misiones.
- Feedback visual de click con pulso y texto flotante.

Despues presiona Play y prueba el loop principal.

Importante: el prototipo esta disenado para movil vertical. En la pestaña `Game`, cambia el aspect ratio a `9:16`, `1080x1920` o cualquier preset portrait antes de evaluar la composicion.

Si ya habias generado la escena y luego se actualizo el generador, vuelve a ejecutar `Tools > Cosmic Crystal Factory > Create Prototype Scene` para reconstruirla con el nuevo layout.

## 2. GameManager

Crea un GameObject vacio llamado `GameManager` y agrega estos componentes:

- `GameManager`
- `SaveManager`
- `CurrencySystem`
- `UpgradeCatalogLoader`
- `UpgradeSystem`
- `OfflineProgressSystem`
- `DailyRewardSystem`
- `BoostSystem`
- `StageCatalogLoader`
- `StageSystem`
- `MissionCatalogLoader`
- `MissionSystem`
- `AdsManager`
- `IAPManager`
- `AnalyticsManager`

Luego arrastra cada componente a su campo correspondiente dentro del componente `GameManager`.

## 3. UI principal

Crea un Canvas vertical para mobile con:

- Texto de cristales.
- Texto de cristales por segundo.
- Texto de poder por toque.
- Texto de estado.
- Boton grande central para el cristal.
- Boton de recompensa diaria.
- Boton de anuncio recompensado.
- Panel de tienda con un contenedor vertical.

Agrega `HUDController` al Canvas o a un GameObject llamado `HUD` y conecta:

- `coinsText`
- `coinsPerSecondText`
- `clickPowerText`
- `statusText`
- `crystalButton`
- `dailyRewardButton`
- `rewardedAdButton`

## 4. Tienda

1. Crea un prefab de boton para upgrades.
2. Agrega el script `UpgradeButtonView` al prefab.
3. Dentro del prefab conecta textos para nombre, descripcion, nivel y costo.
4. Conecta el boton de compra.
5. En la escena, crea un GameObject `Shop` con `ShopController`.
6. Conecta el contenedor vertical y el prefab de upgrade.

## 5. Datos de economia

El catalogo inicial esta en:

`Assets/Resources/GameData/UpgradeCatalog.json`

Puedes editar costos, nombres, desbloqueos y beneficios sin tocar codigo.

Tambien hay datos narrativos y de misiones en:

- `Assets/Resources/GameData/StageCatalog.json`
- `Assets/Resources/GameData/MissionCatalog.json`

## 6. Monetizacion

Por ahora `AdsManager` e `IAPManager` estan en modo simulado:

- Rewarded Ad da cristales inmediatamente.
- Packs de monedas agregan monedas sin compra real.
- Remove Ads se guarda localmente.

Cuando el MVP se sienta bien, se reemplazan esos metodos por:

- Google Mobile Ads SDK o Unity LevelPlay.
- Google Play Billing.
- Firebase Analytics / Crashlytics.

## 7. Primer objetivo jugable

El primer objetivo es que en Unity puedas:

1. Tocar el cristal.
2. Ver subir los cristales.
3. Comprar `Pulidor Manual`.
4. Desbloquear `Mesa Energetica`.
5. Cerrar y abrir el juego.
6. Recibir progreso offline.
7. Ver cambios de etapa/lore.
8. Completar y reclamar misiones.

Cuando eso funcione, recien conviene agregar arte final, anuncios reales y compras reales.

## 8. Registro de progreso

El contexto vivo del desarrollo queda en:

`Docs/DevelopmentLog.md`

Actualizalo cada vez que se agregue un sistema importante, se pruebe algo en Unity o se tome una decision de producto.

## 9. Imagenes generadas por IA

Los prompts listos para generar assets estan en:

`Docs/AIPrompts.md`

Guarda los PNG generados dentro de `Assets/Art/` y configuralos como `Sprite (2D and UI)` desde el Inspector.

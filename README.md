# Fabrica de Cristales Cosmica

Base inicial de un juego mobile idle clicker 2D para Android hecho en Unity.

Premisa actual: el jugador reactiva la **Estacion Aurora**, una fabrica orbital apagada que debe producir cristales de energia, cumplir pedidos, restaurar sectores y convertirse en una red energetica rentable.

## Objetivo

Crear un MVP monetizable de forma sostenible:

- Progreso por toque.
- Produccion automatica.
- Upgrades con economia configurable.
- Guardado local.
- Progreso offline.
- Recompensa diaria.
- Etapas narrativas con lore desbloqueable.
- Misiones de corto plazo.
- Menu principal, pantalla de historia, pantalla de carga y tienda simulada.
- Popups de recompensas, pedidos completados y sectores restaurados.
- Boost temporal por rewarded ad.
- Rewarded ads simulados.
- IAP simulados para reemplazar luego con Google Play Billing.

## Documentos importantes

- `Juego.txt`: documento de diseno completo.
- `Docs/UnitySetup.md`: pasos para montar la escena en Unity.
- `Docs/SustainableMonetization.md`: reglas de monetizacion sostenible.
- `Docs/GitHubUnityWorkflow.md`: flujo recomendado Unity + VS Code Agent + GitHub.
- `Docs/GitCommands.md`: comandos Git utiles para el proyecto.
- `Docs/DevelopmentLog.md`: registro vivo de progreso y contexto.
- `Docs/ArtAndLoreDirection.md`: guia visual, lore y prompts IA.
- `Docs/AIPrompts.md`: prompts listos para generar imagenes IA del juego.
- `Docs/AssetInventory.md`: assets generados, rutas finales y pendientes.
- `Docs/VerticalSlicePlan.md`: direccion actual de gameplay, historia y monetizacion.
- `Assets/Resources/GameData/UpgradeCatalog.json`: economia inicial editable.
- `Assets/Resources/GameData/MissionCatalog.json`: misiones iniciales editables.
- `Assets/Resources/GameData/StageCatalog.json`: etapas narrativas y paleta visual.

## Primer hito

Lograr en Unity que el jugador pueda:

1. Entrar desde un menu principal con contexto narrativo.
2. Ver pantalla de carga antes de la fabrica.
3. Tocar el cristal para despertar el nucleo.
4. Ganar cristales y comprar upgrades.
5. Cumplir pedidos de energia y reclamar recompensas.
6. Desbloquear sectores narrativos de Aurora.
7. Usar recompensas diarias y boosts opcionales.
8. Probar tienda simulada de packs y Remove Ads.

Cuando este hito funcione, el siguiente paso es crear una UI visualmente atractiva y reemplazar los stubs de ads/IAP por SDKs reales.

## Flujo recomendado

Usar GitHub como punto central del proyecto:

1. Codear sistemas y documentacion desde VS Code.
2. Probar escenas, prefabs y assets desde Unity.
3. Subir avances a GitHub con commits frecuentes.
4. Volver a VS Code para que el agente trabaje siempre sobre el repo actualizado.

## Vista recomendada en Unity

El juego esta pensado para movil vertical. En la pestaña `Game`, usa un aspect ratio vertical como `9:16`, `1080x1920` o `Portrait` para revisar la UI correctamente.

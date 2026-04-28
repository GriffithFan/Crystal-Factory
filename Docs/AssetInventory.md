# Inventario de assets IA

## Importados y ordenados

### Fondos

- `Assets/Resources/Art/Backgrounds/bg_sleeping_fragment.png`
- `Assets/Resources/Art/Backgrounds/bg_first_resonance.png`
- `Assets/Resources/Art/Backgrounds/bg_automatic_workshop.png`
- `Assets/Resources/Art/Backgrounds/bg_portal_core.png`
- `Assets/Resources/Art/Backgrounds/bg_orbital_foundry.png`
- `Assets/Resources/Art/Backgrounds/bg_galactic_engine.png`

### Cristales

- `Assets/Resources/Art/Crystals/crystal_main.png`
- `Assets/Resources/Art/Crystals/crystal_premium.png`

### Personajes

- `Assets/Resources/Art/Characters/robot_assistant.png`
- `Assets/Resources/Art/Characters/robot_assistant_expressions.png`

### Mejoras

- `Assets/Resources/Art/Upgrades/icon_manual_polisher.png`
- `Assets/Resources/Art/Upgrades/icon_resonance_hammer.png`
- `Assets/Resources/Art/Upgrades/icon_energy_table.png`
- `Assets/Resources/Art/Upgrades/icon_mini_mining_robot.png`
- `Assets/Resources/Art/Upgrades/icon_light_reactor.png`
- `Assets/Resources/Art/Upgrades/icon_mineral_portal.png`

### UI / Tienda

- `Assets/Resources/Art/UI/icon_currency_crystals.png`
- `Assets/Resources/Art/UI/icon_daily_reward.png`
- `Assets/Resources/Art/UI/icon_rewarded_boost.png`
- `Assets/Resources/Art/UI/icon_remove_ads.png`

## Pendientes recomendados

- Cofre normal: `Assets/Resources/Art/UI/chest_reward.png`
- Cofre premium: `Assets/Resources/Art/UI/chest_premium.png`
- Icono app: `Assets/Resources/Art/Store/app_icon.png`
- Feature graphic: `Assets/Resources/Art/Store/feature_graphic.png`
- Screenshot promocional: `Assets/Resources/Art/Store/promo_screenshot_background.png`
- Icono para `crystal_tunnel`: `Assets/Resources/Art/Upgrades/icon_crystal_tunnel.png`
- Icono para `cosmic_drone`: `Assets/Resources/Art/Upgrades/icon_cosmic_drone.png`
- Icono para `amplifier_prism`: `Assets/Resources/Art/Upgrades/icon_amplifier_prism.png`
- Icono para `galactic_core`: `Assets/Resources/Art/Upgrades/icon_galactic_core.png`

## Notas de integracion

- Los PNG bajo `Assets/Resources/Art/` se importan automaticamente como `Sprite (2D and UI)` mediante `ArtSpritePostprocessor`.
- Las etapas cargan `backgroundResource` desde `StageCatalog.json`.
- Las mejoras cargan `iconResource` desde `UpgradeCatalog.json`.
- El generador de escena mantiene el boton principal como cristal procedural para asegurar transparencia y clicks confiables.
- Si un asset generado trae fondo rectangular, no debe usarse como boton principal o icono transparente hasta regenerarlo con fondo transparente real. Por ahora el cristal principal jugable usa grafico procedural.

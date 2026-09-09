import re

with open('src/FocusForge.App/MainWindow.xaml', 'r', encoding='utf-8') as f:
    content = f.read()

replacements = {
    '#0D121C': '{DynamicResource WindowBackgroundBrush}',
    '#111824': '{DynamicResource SidebarBackgroundBrush}',
    '#1D2636': '{DynamicResource PanelAltBrush}',
    '#293044': '{DynamicResource PanelAltBrush}',
    '#202B3C': '{DynamicResource PanelAltBrush}',
    '#46546A': '{DynamicResource MutedBrush}',
    '#23433D': '{DynamicResource ScheduleDeepWorkBg}',
    '#423A25': '{DynamicResource ScheduleLunchBg}',
    '#29334C': '{DynamicResource ScheduleMoveBg}',
    '#2B384B': '{DynamicResource GraphGridBrush}',
    '{StaticResource InkBrush}': '{DynamicResource InkBrush}',
    '{StaticResource MutedBrush}': '{DynamicResource MutedBrush}',
    '{StaticResource PanelBrush}': '{DynamicResource PanelBrush}',
    '{StaticResource PanelAltBrush}': '{DynamicResource PanelAltBrush}',
    '{StaticResource AccentBrush}': '{DynamicResource AccentBrush}',
    '{StaticResource NavButton}': '{DynamicResource NavButton}'
}

for old, new in replacements.items():
    content = content.replace(old, new)

with open('src/FocusForge.App/MainWindow.xaml', 'w', encoding='utf-8') as f:
    f.write(content)

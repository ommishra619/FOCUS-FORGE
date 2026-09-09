import re

with open('src/FocusForge.App/MainWindow.xaml', 'r', encoding='utf-8') as f:
    content = f.read()

# Replace hardcoded colors with DynamicResources based on the original design
replacements = [
    ('#0B1120', '{DynamicResource WindowBrush}'),
    ('#1A2532', '{DynamicResource PanelBrush}'),
    ('#243044', '{DynamicResource GraphGridBrush}'),
    ('#3B82F6', '{DynamicResource AccentBrush}'),
    ('#60A5FA', '{DynamicResource PrimaryButton}'),
    ('#F43F5E', '{DynamicResource DangerBrush}'),
    ('#10B981', '{DynamicResource SuccessBrush}'),
    ('#F59E0B', '{DynamicResource WarmBrush}'),
    ('#FFFFFF', '#FFFFFF'),
    ('#94A3B8', '{DynamicResource MutedBrush}')
]

# Specifically replace the exact string the script looks for:
content = content.replace('<Border Grid.Row=\"1\" Grid.ColumnSpan=\"2\" Background=\"#1A2532\"', '<Border Grid.Row=\"1\" Grid.ColumnSpan=\"2\" Background=\"{DynamicResource PanelBrush}\"')

# Replace other colors
for hex_color, resource in replacements:
    if resource.startswith('{'):
        content = content.replace(f'Background=\"{hex_color}\"', f'Background=\"{resource}\"')
        content = content.replace(f'Foreground=\"{hex_color}\"', f'Foreground=\"{resource}\"')
        content = content.replace(f'Stroke=\"{hex_color}\"', f'Stroke=\"{resource}\"')
        content = content.replace(f'Fill=\"{hex_color}\"', f'Fill=\"{resource}\"')
        content = content.replace(f'BorderBrush=\"{hex_color}\"', f'BorderBrush=\"{resource}\"')

with open('src/FocusForge.App/MainWindow.xaml', 'w', encoding='utf-8') as f:
    f.write(content)

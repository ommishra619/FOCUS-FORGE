import re

with open('src/FocusForge.App/MainWindow.xaml', 'r', encoding='utf-8') as f:
    content = f.read()

# Add borders to the main cards to make them visible in solid color themes
content = content.replace(
    'Background="{DynamicResource PanelBrush}" CornerRadius="12"',
    'Background="{DynamicResource PanelBrush}" BorderBrush="{DynamicResource PanelAltBrush}" BorderThickness="2" CornerRadius="12"'
)

content = content.replace(
    'Background="{DynamicResource PanelBrush}" CornerRadius="10"',
    'Background="{DynamicResource PanelBrush}" BorderBrush="{DynamicResource PanelAltBrush}" BorderThickness="2" CornerRadius="10"'
)

with open('src/FocusForge.App/MainWindow.xaml', 'w', encoding='utf-8') as f:
    f.write(content)

print("Done")

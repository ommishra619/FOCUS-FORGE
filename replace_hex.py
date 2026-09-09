import re

with open('src/FocusForge.App/MainWindow.xaml', 'r', encoding='utf-8') as f:
    content = f.read()

replacements = {
    'Background="#0D121C"': 'Background="{DynamicResource WindowBackgroundBrush}"',
    'Background="#111824"': 'Background="{DynamicResource SidebarBackgroundBrush}"',
    'Background="#1D2636"': 'Background="{DynamicResource PanelAltBrush}"',
    'Background="#1A2532"': 'Background="{DynamicResource PanelBrush}"',
    'Background="#202B3C"': 'Background="{DynamicResource PanelAltBrush}"',
    'Background="#23433D"': 'Background="{DynamicResource ScheduleDeepWorkBg}"',
    'Background="#243044"': 'Background="{DynamicResource NavButtonHoverBrush}"',
    'Background="#263247"': 'Background="{DynamicResource GhostButtonBackgroundBrush}"',
    'Background="#293044"': 'Background="{DynamicResource PanelAltBrush}"',
    'Background="#29334C"': 'Background="{DynamicResource ScheduleMoveBg}"',
    'Background="#423A25"': 'Background="{DynamicResource ScheduleLunchBg}"',
    
    'Foreground="#10231F"': 'Foreground="{DynamicResource PrimaryButtonForegroundBrush}"',
    'Foreground="#5F6C80"': 'Foreground="{DynamicResource MutedBrush}"',
    'Foreground="#A6B8FF"': 'Foreground="{DynamicResource ScheduleMoveFg}"',
    
    'BorderBrush="#243044"': 'BorderBrush="{DynamicResource GraphGridBrush}"',
    'BorderBrush="#2B384B"': 'BorderBrush="{DynamicResource GraphGridBrush}"',
    'BorderBrush="#46546A"': 'BorderBrush="{DynamicResource GraphGridBrush}"',
    
    'Stroke="#F5F7FB"': 'Stroke="{DynamicResource InkBrush}"',
    'Fill="#F5F7FB"': 'Fill="{DynamicResource InkBrush}"',
}

for old, new in replacements.items():
    content = content.replace(old, new)

with open('src/FocusForge.App/MainWindow.xaml', 'w', encoding='utf-8') as f:
    f.write(content)

print("Done")

import re

with open('src/FocusForge.App/MainWindow.xaml', 'r', encoding='utf-8', errors='ignore') as f:
    content = f.read()

replacements = {
    '?   Today': '?   Today',
    '?   Tasks': '?   Tasks',
    '?   Weekly plan': '??   Weekly plan',
    '?   Focus sessions': '?   Focus sessions',
    '?   Protected apps': '??   Protected apps',
    '?   Settings': '?   Settings'
}

for old, new in replacements.items():
    content = content.replace(old, new)

# Move the AddTask Button to the bottom of the sidebar
# It is currently:
# </DockPanel>
# </Border>
# <Button Content="+" FontSize="28" FontWeight="Bold" Width="64" Height="64" Margin="0,24,0,0" Style="{DynamicResource PrimaryButton}" Click="AddTask_Click" ToolTip="Add a new task">

button_start = content.find('<Button Content="+" FontSize="28"')
if button_start != -1:
    button_end = content.find('</Button>', button_start) + len('</Button>')
    button_xml = content[button_start:button_end]
    content = content[:button_start] + content[button_end:]

    # Inject it right before the </DockPanel> of the sidebar
    dock_panel_end = content.find('</DockPanel>')
    if dock_panel_end != -1:
        # Wrap it in a StackPanel or DockPanel.Dock="Bottom" so it stays at the bottom
        # Wait, there's already a Border DockPanel.Dock="Bottom" (the streak card).
        # We can just add DockPanel.Dock="Bottom" to the Button!
        button_xml = button_xml.replace('Margin="0,24,0,0"', 'Margin="0,0,0,16" DockPanel.Dock="Bottom" HorizontalAlignment="Left"')
        content = content[:dock_panel_end] + button_xml + '\n              ' + content[dock_panel_end:]

with open('src/FocusForge.App/MainWindow.xaml', 'w', encoding='utf-8') as f:
    f.write(content)

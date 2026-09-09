with open('src/FocusForge.App/MainWindow.xaml', 'r', encoding='utf-8') as f:
    content = f.read()

import re

# We want to replace the StackPanel containing the accent color buttons
pattern = r'<StackPanel Orientation="Horizontal">.*?<Button Content="Deep Ocean Blue".*?</StackPanel>'

replacement = '''<StackPanel Orientation="Horizontal">
                                        <Button Content="Deep Purple" Tag="#3C008B" Click="ChangeAccentColor_Click" Style="{DynamicResource GhostButton}" Margin="0,0,10,0" />
                                        <Button Content="Productivity Blue" Tag="#4272F1" Click="ChangeAccentColor_Click" Style="{DynamicResource GhostButton}" Margin="0,0,10,0" />
                                        <Button Content="Focus Green" Tag="#26A557" Click="ChangeAccentColor_Click" Style="{DynamicResource GhostButton}" Margin="0,0,10,0" />
                                        <Button Content="Energy Yellow" Tag="#F3A919" Click="ChangeAccentColor_Click" Style="{DynamicResource GhostButton}" Margin="0,0,10,0" />
                                        <Button Content="Action Orange" Tag="#F7861B" Click="ChangeAccentColor_Click" Style="{DynamicResource GhostButton}" Margin="0,0,10,0" />
                                    </StackPanel>'''

content = re.sub(pattern, replacement, content, flags=re.DOTALL)

with open('src/FocusForge.App/MainWindow.xaml', 'w', encoding='utf-8') as f:
    f.write(content)

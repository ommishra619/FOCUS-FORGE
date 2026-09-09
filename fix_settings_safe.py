with open('src/FocusForge.App/MainWindow.xaml', 'r', encoding='utf-8') as f:
    content = f.read()

old_str = '''<Button Content="Deep Ocean Blue" Tag="#2A5C8D" Click="ChangeAccentColor_Click" Style="{DynamicResource GhostButton}" Margin="0,0,10,0" />
                                        <Button Content="Mint Green" Tag="#10B981" Click="ChangeAccentColor_Click" Style="{DynamicResource GhostButton}" Margin="0,0,10,0" />
                                        <Button Content="Lavender" Tag="#8B5CF6" Click="ChangeAccentColor_Click" Style="{DynamicResource GhostButton}" Margin="0,0,10,0" />
                                        <Button Content="Coral Pink" Tag="#F43F5E" Click="ChangeAccentColor_Click" Style="{DynamicResource GhostButton}" Margin="0,0,10,0" />
                                        <Button Content="Sunset Orange" Tag="#F97316" Click="ChangeAccentColor_Click" Style="{DynamicResource GhostButton}" Margin="0,0,10,0" />'''

new_str = '''<Button Content="Deep Purple" Tag="#3C008B" Click="ChangeAccentColor_Click" Style="{DynamicResource GhostButton}" Margin="0,0,10,0" />
                                        <Button Content="Productivity Blue" Tag="#4272F1" Click="ChangeAccentColor_Click" Style="{DynamicResource GhostButton}" Margin="0,0,10,0" />
                                        <Button Content="Focus Green" Tag="#26A557" Click="ChangeAccentColor_Click" Style="{DynamicResource GhostButton}" Margin="0,0,10,0" />
                                        <Button Content="Energy Yellow" Tag="#F3A919" Click="ChangeAccentColor_Click" Style="{DynamicResource GhostButton}" Margin="0,0,10,0" />
                                        <Button Content="Action Orange" Tag="#F7861B" Click="ChangeAccentColor_Click" Style="{DynamicResource GhostButton}" Margin="0,0,10,0" />'''

content = content.replace(old_str, new_str)

with open('src/FocusForge.App/MainWindow.xaml', 'w', encoding='utf-8') as f:
    f.write(content)

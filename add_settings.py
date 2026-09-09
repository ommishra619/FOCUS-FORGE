import re

with open('src/FocusForge.App/MainWindow.xaml', 'r', encoding='utf-8', errors='ignore') as f:
    content = f.read()

settings_panel = '''<StackPanel x:Name="SettingsPanel" Visibility="Collapsed">
                            <TextBlock Text="Appearance" FontSize="20" FontWeight="SemiBold" Margin="0,0,0,14" />
                            <Border Background="{DynamicResource PanelBrush}" BorderBrush="{DynamicResource GraphGridBrush}" BorderThickness="1" CornerRadius="8" Padding="24" Margin="0,0,0,20">
                                <StackPanel>
                                    <TextBlock Text="Theme Mode" Foreground="{DynamicResource MutedBrush}" FontWeight="SemiBold" Margin="0,0,0,10" />
                                    <Button Content="Toggle Light / Dark Mode" Click="ToggleTheme_Click" Style="{DynamicResource GhostButton}" HorizontalAlignment="Left" Margin="0,0,0,20" />
                                    
                                    <TextBlock Text="Accent Color" Foreground="{DynamicResource MutedBrush}" FontWeight="SemiBold" Margin="0,0,0,10" />
                                    <StackPanel Orientation="Horizontal">
                                        <Button Content="Deep Ocean Blue" Tag="#2A5C8D" Click="ChangeAccentColor_Click" Style="{DynamicResource GhostButton}" Margin="0,0,10,0" />
                                        <Button Content="Mint Green" Tag="#10B981" Click="ChangeAccentColor_Click" Style="{DynamicResource GhostButton}" Margin="0,0,10,0" />
                                        <Button Content="Lavender" Tag="#8B5CF6" Click="ChangeAccentColor_Click" Style="{DynamicResource GhostButton}" Margin="0,0,10,0" />
                                        <Button Content="Coral Pink" Tag="#F43F5E" Click="ChangeAccentColor_Click" Style="{DynamicResource GhostButton}" Margin="0,0,10,0" />
                                        <Button Content="Sunset Orange" Tag="#F97316" Click="ChangeAccentColor_Click" Style="{DynamicResource GhostButton}" Margin="0,0,10,0" />
                                    </StackPanel>
                                </StackPanel>
                            </Border>
                        </StackPanel>'''

# Find <Button x:Name="AddTaskButton" and insert the SettingsPanel before its parent StackPanel/DockPanel?
# Wait, let's insert it right before <Button x:Name="AddTaskButton"
# The AddTaskButton is inside a DockPanel in SectionView
target = '<Button x:Name="AddTaskButton"'
if target in content:
    content = content.replace(target, settings_panel + '\n                        ' + target)

with open('src/FocusForge.App/MainWindow.xaml', 'w', encoding='utf-8') as f:
    f.write(content)

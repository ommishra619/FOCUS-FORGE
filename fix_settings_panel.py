with open('src/FocusForge.App/MainWindow.xaml', 'r', encoding='utf-8') as f:
    content = f.read()

content = content.replace('Todayâ€™s', 'Today\'s')

replacement = '''<StackPanel x:Name="SettingsPanel" Visibility="Collapsed">
                            <TextBlock Text="Appearance" FontSize="20" FontWeight="SemiBold" Margin="0,0,0,14" />
                            <Border Background="{DynamicResource PanelBrush}" BorderBrush="{DynamicResource GraphGridBrush}" BorderThickness="1" CornerRadius="8" Padding="24" Margin="0,0,0,20">
                                <StackPanel>
                                    <TextBlock Text="Select Theme" Foreground="{DynamicResource MutedBrush}" FontWeight="SemiBold" Margin="0,0,0,10" />
                                    <StackPanel Orientation="Horizontal" Margin="0,0,0,20">
                                        <Button Content="Red + White" Tag="RedWhiteTheme" Click="ChangeTheme_Click" Style="{DynamicResource GhostButton}" Margin="0,0,10,0" />
                                        <Button Content="Pink + White" Tag="PinkWhiteTheme" Click="ChangeTheme_Click" Style="{DynamicResource GhostButton}" Margin="0,0,10,0" />
                                        <Button Content="Blue + White" Tag="BlueWhiteTheme" Click="ChangeTheme_Click" Style="{DynamicResource GhostButton}" Margin="0,0,10,0" />
                                        <Button Content="Black + White" Tag="BlackWhiteTheme" Click="ChangeTheme_Click" Style="{DynamicResource GhostButton}" Margin="0,0,10,0" />
                                    </StackPanel>
                                </StackPanel>
                            </Border>
                        </StackPanel>'''

import re
start_idx = content.find('<StackPanel x:Name="SettingsPanel"')
if start_idx != -1:
    end_pattern = r'</StackPanel>\s*</Border>\s*</StackPanel>'
    match = re.search(end_pattern, content[start_idx:])
    if match:
        end_idx = start_idx + match.end()
        content = content[:start_idx] + replacement + content[end_idx:]

with open('src/FocusForge.App/MainWindow.xaml', 'w', encoding='utf-8') as f:
    f.write(content)

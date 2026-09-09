import re

with open('src/FocusForge.App/MainWindow.xaml', 'r', encoding='utf-8') as f:
    content = f.read()

replacement = '''<StackPanel x:Name="SettingsPanel" Visibility="Collapsed" Margin="0,0,40,40">
                            <!-- Header -->
                            <TextBlock Text="Appearance" FontSize="24" FontWeight="Bold" Foreground="{DynamicResource InkBrush}" Margin="0,0,0,4" />
                            <TextBlock Text="Configure the app's visual theme and display preferences." FontSize="14" Foreground="{DynamicResource MutedBrush}" Margin="0,0,0,32" />

                            <!-- Display Settings Section -->
                            <TextBlock Text="Display Settings" FontSize="14" FontWeight="SemiBold" Foreground="{DynamicResource InkBrush}" Margin="0,0,0,8" />
                            <Border Background="Transparent" BorderBrush="{DynamicResource GraphGridBrush}" BorderThickness="1" CornerRadius="8" Margin="0,0,0,24">
                                <StackPanel>
                                    <DockPanel Padding="16">
                                        <TextBlock Text="Theme Mode" VerticalAlignment="Center" Foreground="{DynamicResource InkBrush}" FontSize="14" />
                                        <Border HorizontalAlignment="Right" Background="{DynamicResource PanelAltBrush}" CornerRadius="6" BorderBrush="{DynamicResource GraphGridBrush}" BorderThickness="1" Padding="2">
                                            <StackPanel Orientation="Horizontal">
                                                <Button Content="System" Width="60" Height="28" Style="{DynamicResource GhostButton}" BorderThickness="0" Foreground="{DynamicResource InkBrush}" Margin="0,0,2,0" />
                                                <Button Content="Light" Width="60" Height="28" Style="{DynamicResource GhostButton}" BorderThickness="0" Foreground="{DynamicResource InkBrush}" Margin="0,0,2,0" Click="ToggleTheme_Click" />
                                                <Button Content="Dark" Width="60" Height="28" Background="{DynamicResource PanelBrush}" CornerRadius="4" BorderThickness="0" Foreground="{DynamicResource InkBrush}" Click="ToggleTheme_Click" />
                                            </StackPanel>
                                        </Border>
                                    </DockPanel>
                                </StackPanel>
                            </Border>

                            <!-- Custom Theme Section -->
                            <TextBlock Text="Active Theme" FontSize="14" FontWeight="SemiBold" Foreground="{DynamicResource InkBrush}" Margin="0,0,0,8" />
                            <Border Background="Transparent" BorderBrush="{DynamicResource GraphGridBrush}" BorderThickness="1" CornerRadius="8" Margin="0,0,0,24">
                                <StackPanel>
                                    <!-- Preset Row -->
                                    <DockPanel Padding="16">
                                        <TextBlock Text="Preset" VerticalAlignment="Center" Foreground="{DynamicResource InkBrush}" FontSize="14" />
                                        <ComboBox x:Name="ThemePresetComboBox" HorizontalAlignment="Right" Width="200" SelectedIndex="0" Height="32" SelectionChanged="ThemePresetComboBox_SelectionChanged">
                                            <ComboBoxItem Content="Red + White" Tag="RedWhiteTheme" />
                                            <ComboBoxItem Content="Pink + White" Tag="PinkWhiteTheme" />
                                            <ComboBoxItem Content="Baby Blue + White" Tag="BlueWhiteTheme" />
                                            <ComboBoxItem Content="Black + White" Tag="BlackWhiteTheme" />
                                        </ComboBox>
                                    </DockPanel>
                                    <!-- Divider -->
                                    <Border Height="1" Background="{DynamicResource GraphGridBrush}" />
                                    <!-- Background Color Row -->
                                    <DockPanel Padding="16">
                                        <TextBlock Text="Background" VerticalAlignment="Center" Foreground="{DynamicResource InkBrush}" FontSize="14" />
                                        <Border HorizontalAlignment="Right" Background="{DynamicResource PanelAltBrush}" CornerRadius="6" BorderBrush="{DynamicResource GraphGridBrush}" BorderThickness="1" Padding="8,4">
                                            <StackPanel Orientation="Horizontal">
                                                <Border Width="14" Height="14" Background="{DynamicResource WindowBackgroundBrush}" CornerRadius="2" Margin="0,0,8,0" BorderBrush="{DynamicResource GraphGridBrush}" BorderThickness="1" />
                                                <TextBlock Text="# FFFFFF" x:Name="BgColorText" VerticalAlignment="Center" Foreground="{DynamicResource InkBrush}" FontSize="13" />
                                            </StackPanel>
                                        </Border>
                                    </DockPanel>
                                    <!-- Divider -->
                                    <Border Height="1" Background="{DynamicResource GraphGridBrush}" />
                                    <!-- Foreground Color Row -->
                                    <DockPanel Padding="16">
                                        <TextBlock Text="Foreground" VerticalAlignment="Center" Foreground="{DynamicResource InkBrush}" FontSize="14" />
                                        <Border HorizontalAlignment="Right" Background="{DynamicResource PanelAltBrush}" CornerRadius="6" BorderBrush="{DynamicResource GraphGridBrush}" BorderThickness="1" Padding="8,4">
                                            <StackPanel Orientation="Horizontal">
                                                <Border Width="14" Height="14" Background="{DynamicResource InkBrush}" CornerRadius="2" Margin="0,0,8,0" BorderBrush="{DynamicResource GraphGridBrush}" BorderThickness="1" />
                                                <TextBlock Text="# DC2626" x:Name="FgColorText" VerticalAlignment="Center" Foreground="{DynamicResource InkBrush}" FontSize="13" />
                                            </StackPanel>
                                        </Border>
                                    </DockPanel>
                                    <!-- Divider -->
                                    <Border Height="1" Background="{DynamicResource GraphGridBrush}" />
                                    <!-- Accent Color Row -->
                                    <DockPanel Padding="16">
                                        <TextBlock Text="Accent" VerticalAlignment="Center" Foreground="{DynamicResource InkBrush}" FontSize="14" />
                                        <Border HorizontalAlignment="Right" Background="{DynamicResource PanelAltBrush}" CornerRadius="6" BorderBrush="{DynamicResource GraphGridBrush}" BorderThickness="1" Padding="8,4">
                                            <StackPanel Orientation="Horizontal">
                                                <Border Width="14" Height="14" Background="{DynamicResource AccentBrush}" CornerRadius="2" Margin="0,0,8,0" BorderBrush="{DynamicResource GraphGridBrush}" BorderThickness="1" />
                                                <TextBlock Text="# DC2626" x:Name="AcColorText" VerticalAlignment="Center" Foreground="{DynamicResource InkBrush}" FontSize="13" />
                                            </StackPanel>
                                        </Border>
                                    </DockPanel>
                                </StackPanel>
                            </Border>
                        </StackPanel>'''

start_idx = content.find('<StackPanel x:Name="SettingsPanel"')
if start_idx != -1:
    end_pattern = r'</StackPanel>\s*</Border>\s*</StackPanel>'
    match = re.search(end_pattern, content[start_idx:])
    if match:
        end_idx = start_idx + match.end()
        content = content[:start_idx] + replacement + content[end_idx:]

with open('src/FocusForge.App/MainWindow.xaml', 'w', encoding='utf-8') as f:
    f.write(content)

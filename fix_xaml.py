import re

with open('src/FocusForge.App/MainWindow.xaml', 'r', encoding='utf-8') as f:
    content = f.read()

# 1. Today Replacement
today_start = content.find('<Border Grid.Row=\"1\" Grid.ColumnSpan=\"2\" Background=\"{DynamicResource PanelBrush}\"')
if today_start != -1:
    today_end = content.find('</Border>', today_start) + len('</Border>')
    today_replacement = '''<StackPanel Grid.Row=\"1\" Grid.ColumnSpan=\"2\" Margin=\"0,30,0,28\" Orientation=\"Horizontal\">
                    <Border Background=\"{DynamicResource AccentBrush}\" CornerRadius=\"24\" Padding=\"32,24\" Margin=\"0,0,20,0\" Width=\"380\">
                        <StackPanel>
                            <TextBlock Text=\"Today's Habits Progress\" Foreground=\"#FFFFFF\" FontSize=\"18\" FontWeight=\"SemiBold\" HorizontalAlignment=\"Center\" Margin=\"0,0,0,16\" />
                            <controls:CircularProgressBar Progress=\"{Binding HabitsProgress, Mode=OneWay}\" StrokeThickness=\"16\" ProgressBrush=\"{DynamicResource WarmBrush}\" TextBrush=\"#FFFFFF\" Width=\"140\" Height=\"140\" HorizontalAlignment=\"Center\" />
                            <TextBlock Text=\"Keep up the great work!\" Foreground=\"#FFFFFF\" Opacity=\"0.8\" FontSize=\"12\" HorizontalAlignment=\"Center\" Margin=\"0,16,0,0\" />
                        </StackPanel>
                    </Border>
                    <Border Background=\"{DynamicResource AccentBrush}\" CornerRadius=\"24\" Padding=\"32,24\" Width=\"380\">
                        <StackPanel>
                            <TextBlock Text=\"Today's Tasks Progress\" Foreground=\"#FFFFFF\" FontSize=\"18\" FontWeight=\"SemiBold\" HorizontalAlignment=\"Center\" Margin=\"0,0,0,16\" />
                            <controls:CircularProgressBar Progress=\"{Binding TasksProgress, Mode=OneWay}\" StrokeThickness=\"16\" ProgressBrush=\"{DynamicResource WarmBrush}\" TextBrush=\"#FFFFFF\" Width=\"140\" Height=\"140\" HorizontalAlignment=\"Center\" />
                            <TextBlock Text=\"Stay focused!\" Foreground=\"#FFFFFF\" Opacity=\"0.8\" FontSize=\"12\" HorizontalAlignment=\"Center\" Margin=\"0,16,0,0\" />
                        </StackPanel>
                    </Border>
                </StackPanel>'''
    content = content[:today_start] + today_replacement + content[today_end:]

# 2. Tasks Replacement
tasks_start = content.find('<ItemsControl x:Name=\"SectionTasks\" ItemsSource=\"{Binding Tasks}\"')
if tasks_start != -1:
    tasks_end = content.find('</ItemsControl>', tasks_start) + len('</ItemsControl>')
    tasks_replacement = '''<StackPanel x:Name=\"SectionTasks\">
                            <StackPanel Margin=\"0,0,0,20\">
                                <DockPanel Margin=\"0,0,0,8\">
                                    <TextBlock Text=\"Tasks Completion\" Foreground=\"{DynamicResource MutedBrush}\" FontWeight=\"SemiBold\" DockPanel.Dock=\"Left\" />
                                    <TextBlock Text=\"{Binding TasksProgress, StringFormat={}{0:P0}, Mode=OneWay}\" Foreground=\"{DynamicResource MutedBrush}\" HorizontalAlignment=\"Right\" />
                                </DockPanel>
                                <ProgressBar Value=\"{Binding TasksProgress, Mode=OneWay}\" Maximum=\"1\" Height=\"16\" Background=\"{DynamicResource PanelAltBrush}\" Foreground=\"{DynamicResource WarmBrush}\" BorderThickness=\"0\">
                                    <ProgressBar.Template>
                                        <ControlTemplate TargetType=\"ProgressBar\">
                                            <Grid>
                                                <Border Background=\"{TemplateBinding Background}\" CornerRadius=\"8\" />
                                                <Border Background=\"{TemplateBinding Foreground}\" CornerRadius=\"8\" HorizontalAlignment=\"Left\">
                                                    <Border.Width>
                                                        <MultiBinding Converter=\"{x:Static controls:ProgressBarWidthConverter.Instance}\">
                                                            <Binding Path=\"Value\" RelativeSource=\"{RelativeSource TemplatedParent}\" />
                                                            <Binding Path=\"ActualWidth\" RelativeSource=\"{RelativeSource AncestorType=Grid}\" />
                                                        </MultiBinding>
                                                    </Border.Width>
                                                </Border>
                                            </Grid>
                                        </ControlTemplate>
                                    </ProgressBar.Template>
                                </ProgressBar>
                            </StackPanel>
                            <ItemsControl ItemsSource=\"{Binding Tasks}\"'''
    content = content[:tasks_start] + tasks_replacement + content[tasks_start + len('<ItemsControl x:Name=\"SectionTasks\" ItemsSource=\"{Binding Tasks}\"'):tasks_end] + '''</StackPanel>''' + content[tasks_end:]

# 3. Habits Panel
weight_start = content.find('<StackPanel x:Name=\"WeightPanel\"')
if weight_start != -1:
    habits_panel = '''<StackPanel x:Name=\"HabitsPanel\" Visibility=\"Collapsed\">
                            <ItemsControl ItemsSource=\"{Binding Habits}\">
                                <ItemsControl.ItemTemplate>
                                    <DataTemplate>
                                        <Border Background=\"{DynamicResource PanelBrush}\" BorderBrush=\"{DynamicResource GraphGridBrush}\" BorderThickness=\"1\" CornerRadius=\"24\" Padding=\"20\" Margin=\"0,0,0,15\">
                                            <StackPanel>
                                                <DockPanel Margin=\"0,0,0,15\">
                                                    <TextBlock Text=\"{Binding Title}\" FontSize=\"18\" FontWeight=\"SemiBold\" DockPanel.Dock=\"Left\" />
                                                    <Button Content=\"Toggle Today\" Style=\"{DynamicResource GhostButton}\" Padding=\"8,4\" HorizontalAlignment=\"Right\" Tag=\"{Binding}\" Click=\"ToggleHabitToday_Click\" />
                                                </DockPanel>
                                                <ItemsControl ItemsSource=\"{Binding Heatmap}\">
                                                    <ItemsControl.ItemsPanel>
                                                        <ItemsPanelTemplate>
                                                            <UniformGrid Rows=\"7\" Columns=\"12\" />
                                                        </ItemsPanelTemplate>
                                                    </ItemsControl.ItemsPanel>
                                                    <ItemsControl.ItemTemplate>
                                                        <DataTemplate>
                                                            <Border Background=\"{Binding Color}\" Width=\"20\" Height=\"20\" CornerRadius=\"10\" Margin=\"4\" ToolTip=\"{Binding TooltipText}\" />
                                                        </DataTemplate>
                                                    </ItemsControl.ItemTemplate>
                                                </StackPanel>
                                            </StackPanel>
                                        </Border>
                                    </DataTemplate>
                                </ItemsControl.ItemTemplate>
                            </StackPanel>
                            <Button x:Name=\"AddHabitButton\" Content=\"+  Add habit\" Style=\"{DynamicResource PrimaryButton}\" HorizontalAlignment=\"Left\" Margin=\"0,12,0,0\" Click=\"AddHabit_Click\" />
                        </StackPanel>'''
    content = content[:weight_start] + habits_panel + '\n' + content[weight_start:]

# 4. ProtectedApps
content = content.replace('Click=\"ProtectedApps_Click\"', 'Click=\"ConfigureProtectedApps_Click\"')

# 5. FAB
sidebar_dock = content.find('</DockPanel>')
if sidebar_dock != -1:
    sidebar_end = content.find('</Border>', sidebar_dock)
    if sidebar_end != -1:
        fab = '''</Border>
                    <Button Content=\"+\" FontSize=\"28\" FontWeight=\"Bold\" Width=\"64\" Height=\"64\" Margin=\"0,24,0,0\" Style=\"{DynamicResource PrimaryButton}\" Click=\"AddTask_Click\" ToolTip=\"Add a new task\">
                        <Button.Template>
                            <ControlTemplate TargetType=\"Button\">
                                <Border Background=\"{TemplateBinding Background}\" CornerRadius=\"32\" x:Name=\"border\">
                                    <ContentPresenter HorizontalAlignment=\"Center\" VerticalAlignment=\"Center\" Margin=\"0,0,0,4\" />
                                </Border>
                            </ControlTemplate>
                        </Button.Template>
                    </Button>'''
        content = content[:sidebar_end] + fab + content[sidebar_end + len('</Border>'):]

# Add xmlns if not present
if 'xmlns:controls=' not in content:
    content = content.replace('xmlns:local=\"clr-namespace:FocusForge.App\"', 'xmlns:local=\"clr-namespace:FocusForge.App\"\n        xmlns:controls=\"clr-namespace:FocusForge.App.Controls\"')

with open('src/FocusForge.App/MainWindow.xaml', 'w', encoding='utf-8') as f:
    f.write(content)





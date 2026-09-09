using System;
using System.IO;

var content = File.ReadAllText("src/FocusForge.App/MainWindow.xaml");

var todayStart = content.IndexOf("<Border Grid.Row=\"1\" Grid.ColumnSpan=\"2\" Background=\"{DynamicResource PanelBrush}\"");
var todayEnd = content.IndexOf("</Border>", todayStart) + "</Border>".Length;
var todayReplacement = @"<StackPanel Grid.Row=\"1\" Grid.ColumnSpan=\"2\" Margin=\"0,30,0,28\" Orientation=\"Horizontal\">
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
                            <TextBlock Text=\"Keep going!\" Foreground=\"#FFFFFF\" Opacity=\"0.8\" FontSize=\"12\" HorizontalAlignment=\"Center\" Margin=\"0,16,0,0\" />
                        </StackPanel>
                    </Border>
                </StackPanel>
                <Border Visibility=\"Collapsed\">";

content = content.Substring(0, todayStart) + todayReplacement + content.Substring(todayEnd);

var tasksStart = content.IndexOf("<ItemsControl x:Name=\"SectionTasks\" ItemsSource=\"{Binding Tasks}\"");
var tasksReplacement = @"<StackPanel x:Name=\"SectionTasks\">
                            <StackPanel Margin=\"0,0,0,24\">
                                <DockPanel Margin=\"0,0,0,8\">
                                    <TextBlock Text=\"Today's Progress\" FontWeight=\"SemiBold\" DockPanel.Dock=\"Left\" />
                                    <TextBlock Text=\"{Binding TasksProgress, StringFormat={}{0:P0}}\" Foreground=\"{DynamicResource WarmBrush}\" FontWeight=\"Bold\" HorizontalAlignment=\"Right\" />
                                </DockPanel>
                                <ProgressBar Value=\"{Binding TasksProgress, Mode=OneWay}\" Maximum=\"1\" Height=\"16\" Foreground=\"{DynamicResource WarmBrush}\" Background=\"{DynamicResource ProgressBackgroundBrush}\" BorderThickness=\"0\">
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
                            <ItemsControl ItemsSource=\"{Binding Tasks}\"";
content = content.Substring(0, tasksStart) + tasksReplacement + content.Substring(tasksStart + "<ItemsControl x:Name=\"SectionTasks\" ItemsSource=\"{Binding Tasks}\"".Length);

var itemsControlEnd = content.IndexOf("</ItemsControl>", content.IndexOf("ItemsControl ItemsSource=\"{Binding Tasks}\""));
content = content.Substring(0, itemsControlEnd) + "</ItemsControl>\n                        </StackPanel>" + content.Substring(itemsControlEnd + "</ItemsControl>".Length);

var weightStart = content.IndexOf("<StackPanel x:Name=\"WeightPanel\"");
var habitsPanel = @"<StackPanel x:Name=\"HabitsPanel\" Visibility=\"Collapsed\">
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
                                                </ItemsControl>
                                            </StackPanel>
                                        </Border>
                                    </DataTemplate>
                                </ItemsControl.ItemTemplate>
                            </ItemsControl>
                            <Button x:Name=\"AddHabitButton\" Content=\"+  Add habit\" Style=\"{DynamicResource PrimaryButton}\" HorizontalAlignment=\"Left\" Margin=\"0,12,0,0\" Click=\"AddHabit_Click\" />
                        </StackPanel>";
var settingsPanel = @"<StackPanel x:Name=\"SettingsPanel\" Visibility=\"Collapsed\">
                            <TextBlock Text=\"Appearance\" FontSize=\"20\" FontWeight=\"SemiBold\" Margin=\"0,0,0,14\" />
                            <Border Background=\"{DynamicResource PanelBrush}\" BorderBrush=\"{DynamicResource GraphGridBrush}\" BorderThickness=\"1\" CornerRadius=\"24\" Padding=\"24\" Margin=\"0,0,0,20\">
                                <StackPanel>
                                    <TextBlock Text=\"Accent Color\" Foreground=\"{DynamicResource MutedBrush}\" FontWeight=\"SemiBold\" Margin=\"0,0,0,10\" />
                                    <StackPanel Orientation=\"Horizontal\">
                                        <Button Content=\"Deep Ocean Blue\" Tag=\"#2A5C8D\" Click=\"ChangeAccentColor_Click\" Style=\"{DynamicResource GhostButton}\" Margin=\"0,0,10,0\" />
                                        <Button Content=\"Mint Green\" Tag=\"#10B981\" Click=\"ChangeAccentColor_Click\" Style=\"{DynamicResource GhostButton}\" Margin=\"0,0,10,0\" />
                                        <Button Content=\"Lavender\" Tag=\"#8B5CF6\" Click=\"ChangeAccentColor_Click\" Style=\"{DynamicResource GhostButton}\" Margin=\"0,0,10,0\" />
                                        <Button Content=\"Coral Pink\" Tag=\"#F43F5E\" Click=\"ChangeAccentColor_Click\" Style=\"{DynamicResource GhostButton}\" Margin=\"0,0,10,0\" />
                                        <Button Content=\"Sunset Orange\" Tag=\"#F97316\" Click=\"ChangeAccentColor_Click\" Style=\"{DynamicResource GhostButton}\" Margin=\"0,0,10,0\" />
                                    </StackPanel>
                                </StackPanel>
                            </Border>
                        </StackPanel>";

content = content.Substring(0, weightStart) + habitsPanel + "\n" + settingsPanel + "\n" + content.Substring(weightStart);

content = content.Replace("Click=\"ProtectedApps_Click\"", "Click=\"ConfigureProtectedApps_Click\"");

var sidebarEnd = content.IndexOf("</Border>", content.IndexOf("</DockPanel>"));
var fab = @"</Border>
                    <Button Content=\"+\" FontSize=\"28\" FontWeight=\"Bold\" Width=\"64\" Height=\"64\" Margin=\"0,24,0,0\" Style=\"{DynamicResource PrimaryButton}\" Click=\"AddTask_Click\" ToolTip=\"Add a new task\">
                        <Button.Template>
                            <ControlTemplate TargetType=\"Button\">
                                <Border Background=\"{TemplateBinding Background}\" CornerRadius=\"32\" x:Name=\"border\">
                                    <ContentPresenter HorizontalAlignment=\"Center\" VerticalAlignment=\"Center\" Margin=\"0,0,0,4\" />
                                </Border>
                            </ControlTemplate>
                        </Button.Template>
                    </Button>";
content = content.Substring(0, sidebarEnd) + fab + content.Substring(sidebarEnd + "</Border>".Length);

File.WriteAllText("src/FocusForge.App/MainWindow.xaml", content);
$script | Set-Content fix_xaml.csx


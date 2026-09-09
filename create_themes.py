import os

themes = {
    'RedWhiteTheme.xaml': '''<ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation" xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    <SolidColorBrush x:Key="WindowBackgroundBrush" Color="#FFFFFF" />
    <SolidColorBrush x:Key="SidebarBackgroundBrush" Color="#F8FAFC" />
    <SolidColorBrush x:Key="InkBrush" Color="#0F172A" />
    <SolidColorBrush x:Key="MutedBrush" Color="#64748B" />
    <SolidColorBrush x:Key="PanelBrush" Color="#F1F5F9" />
    <SolidColorBrush x:Key="PanelAltBrush" Color="#E2E8F0" />
    <SolidColorBrush x:Key="AccentBrush" Color="#EF4444" />
    <SolidColorBrush x:Key="WarmBrush" Color="#EF4444" />
    <SolidColorBrush x:Key="GraphGridBrush" Color="#CBD5E1" />
    <SolidColorBrush x:Key="ProgressBackgroundBrush" Color="#E2E8F0" />
    
    <SolidColorBrush x:Key="NavButtonHoverBrush" Color="#E2E8F0" />
    <SolidColorBrush x:Key="PrimaryButtonForegroundBrush" Color="#FFFFFF" />
    <SolidColorBrush x:Key="GhostButtonBackgroundBrush" Color="#E2E8F0" />
    <SolidColorBrush x:Key="GhostButtonBorderBrush" Color="#CBD5E1" />

    <SolidColorBrush x:Key="ScheduleDeepWorkBg" Color="#FEE2E2" />
    <SolidColorBrush x:Key="ScheduleDeepWorkFg" Color="#EF4444" />
    <SolidColorBrush x:Key="ScheduleLunchBg" Color="#FEE2E2" />
    <SolidColorBrush x:Key="ScheduleLunchFg" Color="#EF4444" />
    <SolidColorBrush x:Key="ScheduleMoveBg" Color="#FEE2E2" />
    <SolidColorBrush x:Key="ScheduleMoveFg" Color="#EF4444" />
    
    <SolidColorBrush x:Key="TaskWorkAccent" Color="#EF4444" />
    <SolidColorBrush x:Key="TaskNotesAccent" Color="#F87171" />
    <SolidColorBrush x:Key="TaskMoveAccent" Color="#FCA5A5" />
    <SolidColorBrush x:Key="TaskPlanAccent" Color="#FECACA" />
</ResourceDictionary>''',

    'PinkWhiteTheme.xaml': '''<ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation" xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    <SolidColorBrush x:Key="WindowBackgroundBrush" Color="#FDF2F8" />
    <SolidColorBrush x:Key="SidebarBackgroundBrush" Color="#FCE7F3" />
    <SolidColorBrush x:Key="InkBrush" Color="#831843" />
    <SolidColorBrush x:Key="MutedBrush" Color="#9D174D" />
    <SolidColorBrush x:Key="PanelBrush" Color="#FFFFFF" />
    <SolidColorBrush x:Key="PanelAltBrush" Color="#FDF2F8" />
    <SolidColorBrush x:Key="AccentBrush" Color="#DB2777" />
    <SolidColorBrush x:Key="WarmBrush" Color="#BE185D" />
    <SolidColorBrush x:Key="GraphGridBrush" Color="#FBCFE8" />
    <SolidColorBrush x:Key="ProgressBackgroundBrush" Color="#FBCFE8" />
    
    <SolidColorBrush x:Key="NavButtonHoverBrush" Color="#FBCFE8" />
    <SolidColorBrush x:Key="PrimaryButtonForegroundBrush" Color="#FFFFFF" />
    <SolidColorBrush x:Key="GhostButtonBackgroundBrush" Color="#FBCFE8" />
    <SolidColorBrush x:Key="GhostButtonBorderBrush" Color="#F9A8D4" />

    <SolidColorBrush x:Key="ScheduleDeepWorkBg" Color="#FCE7F3" />
    <SolidColorBrush x:Key="ScheduleDeepWorkFg" Color="#DB2777" />
    <SolidColorBrush x:Key="ScheduleLunchBg" Color="#FCE7F3" />
    <SolidColorBrush x:Key="ScheduleLunchFg" Color="#DB2777" />
    <SolidColorBrush x:Key="ScheduleMoveBg" Color="#FCE7F3" />
    <SolidColorBrush x:Key="ScheduleMoveFg" Color="#DB2777" />
    
    <SolidColorBrush x:Key="TaskWorkAccent" Color="#DB2777" />
    <SolidColorBrush x:Key="TaskNotesAccent" Color="#F472B6" />
    <SolidColorBrush x:Key="TaskMoveAccent" Color="#F9A8D4" />
    <SolidColorBrush x:Key="TaskPlanAccent" Color="#FBCFE8" />
</ResourceDictionary>''',

    'BlueWhiteTheme.xaml': '''<ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation" xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    <SolidColorBrush x:Key="WindowBackgroundBrush" Color="#E0F2FE" />
    <SolidColorBrush x:Key="SidebarBackgroundBrush" Color="#BAE6FD" />
    <SolidColorBrush x:Key="InkBrush" Color="#0C4A6E" />
    <SolidColorBrush x:Key="MutedBrush" Color="#0369A1" />
    <SolidColorBrush x:Key="PanelBrush" Color="#FFFFFF" />
    <SolidColorBrush x:Key="PanelAltBrush" Color="#F0F9FF" />
    <SolidColorBrush x:Key="AccentBrush" Color="#0284C7" />
    <SolidColorBrush x:Key="WarmBrush" Color="#0369A1" />
    <SolidColorBrush x:Key="GraphGridBrush" Color="#7DD3FC" />
    <SolidColorBrush x:Key="ProgressBackgroundBrush" Color="#7DD3FC" />
    
    <SolidColorBrush x:Key="NavButtonHoverBrush" Color="#7DD3FC" />
    <SolidColorBrush x:Key="PrimaryButtonForegroundBrush" Color="#FFFFFF" />
    <SolidColorBrush x:Key="GhostButtonBackgroundBrush" Color="#7DD3FC" />
    <SolidColorBrush x:Key="GhostButtonBorderBrush" Color="#38BDF8" />

    <SolidColorBrush x:Key="ScheduleDeepWorkBg" Color="#BAE6FD" />
    <SolidColorBrush x:Key="ScheduleDeepWorkFg" Color="#0284C7" />
    <SolidColorBrush x:Key="ScheduleLunchBg" Color="#BAE6FD" />
    <SolidColorBrush x:Key="ScheduleLunchFg" Color="#0284C7" />
    <SolidColorBrush x:Key="ScheduleMoveBg" Color="#BAE6FD" />
    <SolidColorBrush x:Key="ScheduleMoveFg" Color="#0284C7" />
    
    <SolidColorBrush x:Key="TaskWorkAccent" Color="#0284C7" />
    <SolidColorBrush x:Key="TaskNotesAccent" Color="#0EA5E9" />
    <SolidColorBrush x:Key="TaskMoveAccent" Color="#38BDF8" />
    <SolidColorBrush x:Key="TaskPlanAccent" Color="#7DD3FC" />
</ResourceDictionary>''',

    'BlackWhiteTheme.xaml': '''<ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation" xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    <SolidColorBrush x:Key="WindowBackgroundBrush" Color="#000000" />
    <SolidColorBrush x:Key="SidebarBackgroundBrush" Color="#111111" />
    <SolidColorBrush x:Key="InkBrush" Color="#FFFFFF" />
    <SolidColorBrush x:Key="MutedBrush" Color="#A3A3A3" />
    <SolidColorBrush x:Key="PanelBrush" Color="#1A1A1A" />
    <SolidColorBrush x:Key="PanelAltBrush" Color="#262626" />
    <SolidColorBrush x:Key="AccentBrush" Color="#FFFFFF" />
    <SolidColorBrush x:Key="WarmBrush" Color="#D4D4D4" />
    <SolidColorBrush x:Key="GraphGridBrush" Color="#404040" />
    <SolidColorBrush x:Key="ProgressBackgroundBrush" Color="#404040" />
    
    <SolidColorBrush x:Key="NavButtonHoverBrush" Color="#262626" />
    <SolidColorBrush x:Key="PrimaryButtonForegroundBrush" Color="#000000" />
    <SolidColorBrush x:Key="GhostButtonBackgroundBrush" Color="#262626" />
    <SolidColorBrush x:Key="GhostButtonBorderBrush" Color="#404040" />

    <SolidColorBrush x:Key="ScheduleDeepWorkBg" Color="#262626" />
    <SolidColorBrush x:Key="ScheduleDeepWorkFg" Color="#FFFFFF" />
    <SolidColorBrush x:Key="ScheduleLunchBg" Color="#262626" />
    <SolidColorBrush x:Key="ScheduleLunchFg" Color="#FFFFFF" />
    <SolidColorBrush x:Key="ScheduleMoveBg" Color="#262626" />
    <SolidColorBrush x:Key="ScheduleMoveFg" Color="#FFFFFF" />
    
    <SolidColorBrush x:Key="TaskWorkAccent" Color="#FFFFFF" />
    <SolidColorBrush x:Key="TaskNotesAccent" Color="#D4D4D4" />
    <SolidColorBrush x:Key="TaskMoveAccent" Color="#A3A3A3" />
    <SolidColorBrush x:Key="TaskPlanAccent" Color="#737373" />
</ResourceDictionary>'''
}

for filename, xml in themes.items():
    with open('src/FocusForge.App/Themes/' + filename, 'w', encoding='utf-8') as f:
        f.write(xml)

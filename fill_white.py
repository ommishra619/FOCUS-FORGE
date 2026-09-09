import os

def replace_in_file(filepath, old, new):
    with open(filepath, 'r', encoding='utf-8') as f:
        content = f.read()
    content = content.replace(old, new)
    with open(filepath, 'w', encoding='utf-8') as f:
        f.write(content)

# PinkWhiteTheme
replace_in_file('src/FocusForge.App/Themes/PinkWhiteTheme.xaml', 
                '<SolidColorBrush x:Key="PanelBrush" Color="#F472B6" />',
                '<SolidColorBrush x:Key="PanelBrush" Color="#33FFFFFF" />')

# BlueWhiteTheme
replace_in_file('src/FocusForge.App/Themes/BlueWhiteTheme.xaml', 
                '<SolidColorBrush x:Key="PanelBrush" Color="#38BDF8" />',
                '<SolidColorBrush x:Key="PanelBrush" Color="#33FFFFFF" />')

# BlackWhiteTheme
replace_in_file('src/FocusForge.App/Themes/BlackWhiteTheme.xaml', 
                '<SolidColorBrush x:Key="PanelBrush" Color="#000000" />',
                '<SolidColorBrush x:Key="PanelBrush" Color="#1AFFFFFF" />')

# RedWhiteTheme
replace_in_file('src/FocusForge.App/Themes/RedWhiteTheme.xaml', 
                '<SolidColorBrush x:Key="PanelBrush" Color="#FFFFFF" />',
                '<SolidColorBrush x:Key="PanelBrush" Color="#FFF5F5" />')

print("Done")

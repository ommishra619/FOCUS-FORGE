import re

with open('src/FocusForge.App/MainWindow.xaml', 'r', encoding='utf-8', errors='ignore') as f:
    content = f.read()

content = re.sub(r'Content="[^"]*Today"', 'Content="❖   Today"', content)
content = re.sub(r'Content="[^"]*Tasks"', 'Content="✓   Tasks"', content)
content = re.sub(r'Content="[^"]*Weekly plan"', 'Content="📅   Weekly plan"', content)
content = re.sub(r'Content="[^"]*Focus sessions"', 'Content="⏱   Focus sessions"', content)
content = re.sub(r'Content="[^"]*Protected apps"', 'Content="🛡   Protected apps"', content)
content = re.sub(r'Content="[^"]*Settings"', 'Content="⚙   Settings"', content)

with open('src/FocusForge.App/MainWindow.xaml', 'w', encoding='utf-8') as f:
    f.write(content)

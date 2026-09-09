with open('src/FocusForge.App/MainWindow.xaml.cs', 'r', encoding='utf-8') as f:
    content = f.read()

content = content.replace('new ProtectedAppsDialog { Owner = this }', 'new ProtectedAppsDialog(new System.Collections.Generic.List<string>()) { Owner = this }')
content = content.replace('sender is Button btn', 'sender is System.Windows.Controls.Button btn')

with open('src/FocusForge.App/MainWindow.xaml.cs', 'w', encoding='utf-8') as f:
    f.write(content)

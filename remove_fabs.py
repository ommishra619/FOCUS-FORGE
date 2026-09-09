import re

with open('src/FocusForge.App/MainWindow.xaml', 'r', encoding='utf-8', errors='ignore') as f:
    content = f.read()

# Remove all FAB buttons
# They look like this:
# <Button Content="+" FontSize="28" ...>
#    <Button.Template>
#         ...
#    </Button.Template>
# </Button>
content = re.sub(r'<Button Content="\+" FontSize="28"[\s\S]*?</Button>', '', content)

with open('src/FocusForge.App/MainWindow.xaml', 'w', encoding='utf-8') as f:
    f.write(content)

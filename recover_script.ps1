python -c "
import json

log_file = r'C:\Users\mromm\.gemini\antigravity\brain\84b9795e-708b-45e9-af98-c7830de11515\.system_generated\logs\transcript_full.jsonl'
with open(log_file, 'r', encoding='utf-8') as f:
    for line in f:
        if 'fix_xaml.csx' in line and 'Set-Content fix_xaml.csx' in line:
            data = json.loads(line)
            for call in data.get('tool_calls', []):
                cmd = call.get('args', {}).get('CommandLine', '')
                if 'fix_xaml.csx' in cmd:
                    with open('recover_script.ps1', 'w', encoding='utf-8') as out:
                        out.write(cmd)
"
# PreToolUse hook: blocks "git commit" and "git push" run by Claude Code.
# Exit code 2 blocks the tool call and passes stderr back to Claude.

try {
    $payload = [Console]::In.ReadToEnd() | ConvertFrom-Json
    $command = [string]$payload.tool_input.command
}
catch {
    [Console]::Error.WriteLine('Blocked: the git hook could not read the tool input.')
    exit 2
}

$value = '(?:"[^"]*"|''[^'']*''|[^\s;&|"''])+'
# Global git options that may appear before the subcommand, e.g. git -C path commit
$option = "(?:(?:-c|--git-dir|--work-tree|--namespace)\s+$value|-{1,2}[\w-]+(?:=$value)?)"
$pattern = "(?<![\w-])git(?:\.exe)?[`"']?\s+(?:$option\s+)*(?:commit|push)(?![\w-])"

if ($command -match $pattern) {
    [Console]::Error.WriteLine('Blocked by project hook: Claude is not allowed to run git commit or git push. Ask the developer to do it manually.')
    exit 2
}

exit 0

#!/usr/bin/env pwsh
#Requires -Version 5.1 # Minimum, but 7+ recommended for better ANSI support and modern features

# Get staged git diff
$diffOutput = git diff --cached --no-color # Ensure no colors from git itself in the diff
$rawDiff = ($diffOutput | Out-String).TrimEnd()

$Esc = "$([char]27)" # ANSI Escape character

if ([string]::IsNullOrEmpty($rawDiff)) {
    Write-Host "${Esc}[1;33mNo staged changes found.${Esc}[0m"
    exit 0
}

# 1. Escape double quotes
$escapedDiff = $rawDiff.Replace('"', '\"')
# 2. Remove carriage returns
$escapedDiff = $escapedDiff.Replace("r", "")
# 3. Replace newlines with literal \n string
$escapedDiff = $escapedDiff.Replace("n", '\n')

$systemPrompt = @"
You are a senior software engineer and expert code reviewer with deep expertise in clean code principles, design patterns, and software architecture. Your responsibilities include:

1. SECURITY & BUGS: Identify security vulnerabilities, potential bugs, and critical issues
2. CLEAN CODE: Suggest improvements for code readability, maintainability, and simplicity
3. BEST PRACTICES: Recommend language-specific best practices and coding standards
4. DESIGN PATTERNS: Suggest appropriate design patterns and architectural improvements
5. PERFORMANCE: Identify performance bottlenecks and optimization opportunities
6. CODE QUALITY: Review naming conventions, function/class structure, and documentation

Classify your findings by severity:
- CRITICAL: Security vulnerabilities, major bugs that could cause system failure
- HIGH: Performance issues, significant design flaws, violation of core principles
- MEDIUM: Code quality issues, missing best practices, minor design improvements
- LOW: Style improvements, minor refactoring suggestions, documentation enhancements

For each finding, provide:
- Clear explanation of the issue
- Specific suggestion for improvement
- Code example when applicable
- Rationale based on clean code principles or design patterns

IMPORTANT: 
1. Start each actual issue with exactly "ISSUE:" followed by the severity level.
2. Format: ISSUE: [SEVERITY LEVEL] - [Description]
3. If you find ANY issues, DO NOT include an "APPROVED" statement.
4. ONLY respond with "APPROVED - Code follows good practices with no significant issues detected." if there are absolutely NO issues found.
5. Never mix issues with approval statements.
"@

$userContent = "Please perform a comprehensive code review of this git diff, focusing on clean code principles, best practices, and design patterns:nn$($escapedDiff)"

$payloadObject = @{
    model    = "qwen2.5-coder:7b"
    messages = @(
        @{ role = "system"; content = $systemPrompt },
        @{ role = "user"; content = $userContent }
    )
    stream   = $false
    options  = @{
        temperature = 0.1
        top_p       = 0.9
    }
}

# Convert the PowerShell object to a JSON string
# -Depth 5 is used to ensure all levels of the nested structure are serialized.
$payload = $payloadObject | ConvertTo-Json -Depth 5 -Compress

Write-Host "${Esc}[1;36m🔍 Performing comprehensive AI code review...${Esc}[0m"
Write-Host "${Esc}[1;36m📊 Analyzing: Security, Clean Code, Best Practices, Design Patterns${Esc}[0m"

$review = $null
try {
    $response = Invoke-RestMethod -Uri 'http://localhost:11434/api/chat' -Method Post -ContentType 'application/json' -Body $payload -ErrorAction Stop
    $review = $response.message.content
} catch {
    Write-Error "Error during API call: $($_.Exception.Message)"
    # Attempt to get response text if possible, even on HTTP errors
    if ($_.Exception.Response) {
        $errorResponseStream = $_.Exception.Response.GetResponseStream()
        $streamReader = New-Object System.IO.StreamReader($errorResponseStream)
        $errorResponseBody = $streamReader.ReadToEnd()
        $streamReader.Close()
        $errorResponseStream.Close()
        Write-Error "Raw error response: $errorResponseBody"
    }
    $review = "ERROR: Could not get review from API." # Ensure $review is not null for counts
}


Write-Host ""
Write-Host "${Esc}[1;37m📋 COMPREHENSIVE CODE REVIEW RESULTS${Esc}[0m"
Write-Host "${Esc}[1;37m==========================================${Esc}[0m"
Write-Host "$review"
Write-Host "${Esc}[1;37m==========================================${Esc}[0m"

# Count actual issues only
$reviewLines = @()
if (-not [string]::IsNullOrEmpty($review)) {
    $reviewLines = $review -split '\r?\n'
}

$criticalCount = ($reviewLines | Where-Object { $_ -match "^ISSUE: CRITICAL" }).Count
$highCount = ($reviewLines | Where-Object { $_ -match "^ISSUE: HIGH" }).Count
$mediumCount = ($reviewLines | Where-Object { $_ -match "^ISSUE: MEDIUM" }).Count
$lowCount = ($reviewLines | Where-Object { $_ -match "^ISSUE: LOW" }).Count

Write-Host ""
Write-Host "${Esc}[1;37m📈 REVIEW SUMMARY:${Esc}[0m"
Write-Host "${Esc}[1;31m  🔴 Critical Issues: $criticalCount${Esc}[0m"
Write-Host "${Esc}[0;33m  🟠 High Severity: $highCount${Esc}[0m" # Original was 0;33m
Write-Host "${Esc}[1;33m  🟡 Medium Severity: $mediumCount${Esc}[0m"
Write-Host "${Esc}[0;32m  🟢 Low Severity: $lowCount${Esc}[0m"
Write-Host ""

# Check if the review was approved (should only happen when no issues found)
# The prompt implies $review would *start with* "APPROVED"
$approvedDetected = $false
if (-not [string]::IsNullOrEmpty($review)) {
    $approvedDetected = $review.StartsWith("APPROVED")
}


if ($approvedDetected -and ($criticalCount -eq 0) -and ($highCount -eq 0) -and ($mediumCount -eq 0) -and ($lowCount -eq 0)) {
    Write-Host "${Esc}[0;32m✅ Excellent! Code follows best practices.${Esc}[0m"
    Write-Host ""
    Write-Host "${Esc}[0;32m🎉 Commit approved! Keep up the good coding practices!${Esc}[0m"
    Write-Host ""
    Write-Host "${Esc}[1;42;30m ✓ COMMIT WILL PROCEED ✓ ${Esc}[0m"
    exit 0
}

# Block commits based on NEW severity rules
if ($criticalCount -gt 0) {
    Write-Host "${Esc}[1;31m🚫 COMMIT BLOCKED: Critical issues found ($criticalCount). Fix them before committing.${Esc}[0m"
    Write-Host ""
    Write-Host "${Esc}[1;41;37m ✗ COMMIT REJECTED ✗ ${Esc}[0m"
    exit 1
} elseif ($highCount -gt 0) {
    Write-Host "${Esc}[1;31m🚫 COMMIT BLOCKED: High severity issues found ($highCount). Must be resolved.${Esc}[0m"
    Write-Host ""
    Write-Host "${Esc}[1;41;37m ✗ COMMIT REJECTED ✗ ${Esc}[0m"
    exit 1
} elseif ($mediumCount -ge 3) {
    Write-Host "${Esc}[0;33m⚠️  COMMIT BLOCKED: Too many medium issues ($mediumCount found). Please address some before committing.${Esc}[0m" # Original bash had a space after \33m, fixed here
    Write-Host "${Esc}[0;90m    To override, use: git commit --no-verify${Esc}[0m"
    Write-Host ""
    Write-Host "${Esc}[1;41;37m ✗ COMMIT REJECTED ✗ ${Esc}[0m"
    exit 1
} elseif ($mediumCount -gt 0) {
    Write-Host "${Esc}[1;33m⚠️  Medium severity issues detected ($mediumCount found). Consider fixing, but commit allowed.${Esc}[0m"
    Write-Host ""
    Write-Host "${Esc}[0;32m🎉 Commit approved with minor concerns!${Esc}[0m"
    Write-Host ""
    Write-Host "${Esc}[1;42;30m ✓ COMMIT WILL PROCEED ✓ ${Esc}[0m"
    exit 0
} elseif ($lowCount -gt 0) {
    Write-Host "${Esc}[0;32m✅ Minor improvements suggested ($lowCount found). Good code quality overall.${Esc}[0m"
    Write-Host ""
    Write-Host "${Esc}[0;32m🎉 Commit approved! Keep up the good coding practices!${Esc}[0m"
    Write-Host ""
    Write-Host "${Esc}[1;42;30m ✓ COMMIT WILL PROCEED ✓ ${Esc}[0m"
    exit 0
} else {
    # No issues found but also no explicit approval (or API error occurred and counts are 0)
    if ($review -notmatch "^ISSUE:" -and -not $approvedDetected) { # If review is not an error and no issues are formatted, and not approved
        Write-Host "${Esc}[0;32m✅ Code review completed. No blocking issues found, but ensure AI response format is as expected.${Esc}[0m"
    } elseif ($review -match "^ERROR:") {
         Write-Host "${Esc}[1;31m⚠️  Code review could not be fully performed due to an API error. Manual check required.${Esc}[0m"
         Write-Host "${Esc}[1;31m🚫 COMMIT BLOCKED: Review incomplete.${Esc}[0m"
         Write-Host ""
         Write-Host "${Esc}[1;41;37m ✗ COMMIT REJECTED ✗ ${Esc}[0m"
         exit 1
    } else { # Fallback for cases like empty review but no explicit approval
         Write-Host "${Esc}[0;32m✅ Code review completed. No blocking issues found.${Esc}[0m"
    }

    Write-Host ""
    Write-Host "${Esc}[0;32m🎉 Commit approved! Keep up the good coding practices!${Esc}[0m"
    Write-Host ""
    Write-Host "${Esc}[1;42;30m ✓ COMMIT WILL PROCEED ✓ ${Esc}[0m"
    exit 0
}
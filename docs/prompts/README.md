# Ralph Wiggum Prompts

This directory contains personas and prompt templates for use with Ralph Wiggum loops.

## Contents

| File | Purpose |
|------|---------|
| [PERSONAS.md](./PERSONAS.md) | 11 numbered personas following the development lifecycle |
| [templates/](./templates/) | Ready-to-use prompt templates |

---

## Recommended: Rotating Persona Pattern

The most effective approach uses **rotating personas** that cycle through different perspectives each iteration. This ensures comprehensive review from multiple angles.

### How It Works

```
Iteration % 6 determines the current persona:

Iteration 0, 6, 12, 18... → [0] IMPLEMENTER
Iteration 1, 7, 13, 19... → [1] REVIEWER
Iteration 2, 8, 14, 20... → [2] TESTER
Iteration 3, 9, 15, 21... → [3] UI_UX_DESIGNER
Iteration 4, 10, 16, 22... → [4] SECURITY_AUDITOR
Iteration 5, 11, 17, 23... → [5] PROJECT_MANAGER
```

### Standard 6-Persona Rotation

| Slot | Persona | Focus |
|:----:|---------|-------|
| [0] | #5 IMPLEMENTER | Complete tasks, write code |
| [1] | #9 REVIEWER | Review for bugs, code quality |
| [2] | #7 TESTER | Verify functionality, add tests |
| [3] | #3 UI_UX_DESIGNER | Review UI/UX, accessibility |
| [4] | #10 SECURITY_AUDITOR | Security review, input validation |
| [5] | #2 PROJECT_MANAGER | Check requirements, update tasks |

### Quick Start Command

```bash
/ralph-loop "
Feature: [feature-name]

PHASE 1 - TASKS:
- Read docs/features/[FOLDER]/tasks.md
- Complete unchecked tasks, mark done with [x]
- Run: msbuild GeoMagGUI.sln /p:Configuration=Debug /p:Platform=\"x86\"

PHASE 2 - ROTATING REVIEW (ITERATION MOD 6):

[0] #5 IMPLEMENTER: Complete next task, follow patterns
[1] #9 REVIEWER: Review code for bugs/issues, fix problems
[2] #7 TESTER: Verify functionality, check edge cases
[3] #3 UI_UX_DESIGNER: Review WinForms UI, check accessibility
[4] #10 SECURITY: Check for vulnerabilities, validate inputs
[5] #2 PROJECT_MANAGER: Verify requirements met, update tasks

EACH ITERATION:
1. Identify current persona (Iteration % 6)
2. Perform that persona's review/work
3. Make fixes/improvements
4. Commit: '[PERSONA] description'

OUTPUT <promise>FEATURE COMPLETE</promise> when all tasks done and 2 clean cycles.
" --completion-promise "FEATURE COMPLETE" --max-iterations 30
```

See [templates/ROTATING_FEATURE.md](./templates/ROTATING_FEATURE.md) for full examples.

---

## Single-Persona Patterns

For simpler tasks, use a single persona:

### Bug Fix

```bash
/ralph-loop "Using persona #8 (DEBUGGER), investigate and fix [bug description]. Add a regression test. Output <promise>BUG FIXED</promise> when the fix is complete and tests pass." --completion-promise "BUG FIXED" --max-iterations 15
```

### Code Refactoring

```bash
/ralph-loop "Using persona #6 (REFACTORER), [refactoring task]. Ensure all tests pass after each change. Output <promise>REFACTOR COMPLETE</promise> when done." --completion-promise "REFACTOR COMPLETE" --max-iterations 20
```

### Test Creation

```bash
/ralph-loop "Using persona #7 (TESTER), add unit tests for [component/class]. Output <promise>TESTS COMPLETE</promise> when tests are written and passing." --completion-promise "TESTS COMPLETE" --max-iterations 20
```

---

## Best Practices

### Completion Promises

Always include a clear completion promise in `<promise>` tags:
- `<promise>FEATURE COMPLETE</promise>` - Feature implementation
- `<promise>BUG FIXED</promise>` - Bug fixes
- `<promise>TESTS COMPLETE</promise>` - Test-related tasks
- `<promise>REFACTOR COMPLETE</promise>` - Refactoring tasks

### Max Iterations

Set appropriate limits based on task complexity:

| Task Type | Recommended Max |
|-----------|-----------------|
| Simple bug fix | 10-15 |
| Single-persona feature | 20-25 |
| Rotating persona feature | 30-36 (5-6 full cycles) |
| Large refactoring | 25-40 |

### Commit Message Format

Each commit should indicate which persona made the change:

```
[IMPLEMENTER] Add EMM model support to ModelReader
[REVIEWER] Fix null check in Calculator.SpotCalculation
[TESTER] Add unit test for decimal year conversion
[UI_UX_DESIGNER] Improve preferences dialog layout
[SECURITY] Add coordinate range validation
[PROJECT_MANAGER] Mark data model tasks complete
```

### Task Checklists

For feature work, always reference the tasks.md file:
```bash
/ralph-loop "... Read docs/features/01-feature-name/tasks.md. Mark tasks complete with [x] as you finish them ..."
```

This gives Claude a concrete checklist to track progress.

---

## Troubleshooting

### Loop Not Completing

If Ralph keeps iterating without completing:
1. Check that the completion promise text matches exactly
2. Ensure success criteria are achievable (build works, tests pass)
3. Verify no blocking issues (missing dependencies, etc.)
4. For rotating loops, ensure "2 clean cycles" is possible

### Cancel a Loop

```bash
/cancel-ralph
```

### View Loop State

The loop state is stored in `.claude/.ralph-loop.local.md`

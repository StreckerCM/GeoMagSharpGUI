# Prompt Templates

Ready-to-use prompt templates for Ralph Wiggum loops.

## Available Templates

| Template | Use For |
|----------|---------|
| [ROTATING_FEATURE.md](./ROTATING_FEATURE.md) | **Recommended** - Rotating personas for comprehensive feature implementation |
| [IMPLEMENT_FEATURE.md](./IMPLEMENT_FEATURE.md) | Single-persona feature implementation |
| [FIX_BUG.md](./FIX_BUG.md) | Investigating and fixing bugs |
| [ADD_TESTS.md](./ADD_TESTS.md) | Adding unit tests |
| [REFACTOR.md](./REFACTOR.md) | Code refactoring tasks |

## Usage

1. Copy the template content
2. Fill in the placeholders (marked with `[BRACKETS]`)
3. Use with `/ralph-loop` command

## Creating Custom Templates

Follow this structure:

```markdown
# [Task Type] Prompt Template

## Template

\`\`\`
Using the [PERSONA] persona from docs/prompts/PERSONAS.md:

## Task
[Clear description of what to do]

## Instructions
1. [Step 1]
2. [Step 2]
...

## Success Criteria
- [Criterion 1]
- [Criterion 2]

## Completion
When all success criteria are met, output:
<promise>[PROMISE TEXT]</promise>
\`\`\`

## Example

[Show a concrete example with the template filled in]
```
